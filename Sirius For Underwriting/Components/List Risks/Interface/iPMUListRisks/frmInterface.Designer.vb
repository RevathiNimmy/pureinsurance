<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
    Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
     Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not fTerminateCalled_Form_Terminate_Renamed Then
                fTerminateCalled_Form_Terminate_Renamed = True
                Form_Terminate_Renamed()
            End If
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents OptPutMTAOnNextInstalment As System.Windows.Forms.RadioButton
    Public WithEvents OptInvoice As System.Windows.Forms.RadioButton
    Public WithEvents OptInstalments As System.Windows.Forms.RadioButton
    Public WithEvents optPayNow As System.Windows.Forms.RadioButton
    Public WithEvents optBankGuarantee As System.Windows.Forms.RadioButton
    Public WithEvents optMarkForCollection As System.Windows.Forms.RadioButton
    Public WithEvents optCashDeposit As System.Windows.Forms.RadioButton
    Public WithEvents fraPaymentTerms As System.Windows.Forms.GroupBox
    Public WithEvents txtGrossRoundTotal As System.Windows.Forms.TextBox
    Public WithEvents cmdDocArchive As System.Windows.Forms.Button
    Public WithEvents cmdPrintDocument As System.Windows.Forms.Button
    Private WithEvents _StatusBar_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents StatusBar As System.Windows.Forms.StatusStrip
	Public WithEvents cmdSaveQuote As System.Windows.Forms.Button
	Public WithEvents cmdMakeLive As System.Windows.Forms.Button
	Public WithEvents cmdRequote As System.Windows.Forms.Button
	Public WithEvents cmdPrintProposal As System.Windows.Forms.Button
	Public WithEvents cmdPrintQuote As System.Windows.Forms.Button
	Public WithEvents txtRoundAmt As System.Windows.Forms.TextBox
	Public WithEvents txtNettotal As System.Windows.Forms.TextBox
	Public WithEvents txtCurrency As System.Windows.Forms.TextBox
	Public WithEvents txtTaxTotal As System.Windows.Forms.TextBox
	Public WithEvents txtFeeTotal As System.Windows.Forms.TextBox
	Public WithEvents txtGrossTotal As System.Windows.Forms.TextBox
	Public WithEvents lblGrossRoundedTotal As System.Windows.Forms.Label
	Public WithEvents lblRoundOffAmount As System.Windows.Forms.Label
	Public WithEvents lblNetTotal As System.Windows.Forms.Label
	Public WithEvents lblGrossTotal As System.Windows.Forms.Label
	Public WithEvents lblFeeTotal As System.Windows.Forms.Label
	Public WithEvents lblTaxTotal As System.Windows.Forms.Label
	Public WithEvents lblCurrency As System.Windows.Forms.Label
	Public WithEvents fraTotals As System.Windows.Forms.GroupBox
	Public WithEvents txtExpiryDate As System.Windows.Forms.TextBox
	Public WithEvents txtInceptionDate As System.Windows.Forms.TextBox
	Public WithEvents txtCoverFromDate As System.Windows.Forms.TextBox
	Public WithEvents txtPolicyRef As System.Windows.Forms.TextBox
	Public WithEvents txtAgent As System.Windows.Forms.TextBox
	Public WithEvents cmdAddRisk As System.Windows.Forms.Button
	Public WithEvents cmdDeleteRisk As System.Windows.Forms.Button
	Public WithEvents cmdCopyRisk As System.Windows.Forms.Button
	Public WithEvents cmdEditRisk As System.Windows.Forms.Button
	Public WithEvents cmdApplyDiscount As System.Windows.Forms.Button
	Public WithEvents uctPMUListRisk1 As uctPMUListRiskControl.uctPMUListRisk
	Public WithEvents frameListRisk As System.Windows.Forms.GroupBox
	Public WithEvents cmdAddTask As System.Windows.Forms.Button
	Public WithEvents cmdBackdateMTA As System.Windows.Forms.Button
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents txtPFEFF As System.Windows.Forms.TextBox
	Public WithEvents txtPFEXFF As System.Windows.Forms.TextBox
	Public WithEvents txtPFNCTT As System.Windows.Forms.TextBox
	Public WithEvents Label12 As System.Windows.Forms.Label
	Public WithEvents Label11 As System.Windows.Forms.Label
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents txtPFF As System.Windows.Forms.TextBox
	Public WithEvents txtPEXF As System.Windows.Forms.TextBox
	Public WithEvents txtPFNCT As System.Windows.Forms.TextBox
	Public WithEvents lblPFF As System.Windows.Forms.Label
	Public WithEvents lblPEXF As System.Windows.Forms.Label
	Public WithEvents lblPNCT As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents uctPMUFees1 As uctPMUFeesControl.uctPMUFees
	Private WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents txtTotalRiskTax As System.Windows.Forms.TextBox
	Public WithEvents txtTotalNonTax As System.Windows.Forms.TextBox
	Public WithEvents txtTEF As System.Windows.Forms.TextBox
	Public WithEvents txtTEFF As System.Windows.Forms.TextBox
	Public WithEvents lblRiskCTax As System.Windows.Forms.Label
	Public WithEvents lblNonCTax As System.Windows.Forms.Label
	Public WithEvents lblTEF As System.Windows.Forms.Label
	Public WithEvents lblEFF As System.Windows.Forms.Label
	Public WithEvents fraRTaxes As System.Windows.Forms.GroupBox
	Public WithEvents txtPCT As System.Windows.Forms.TextBox
	Public WithEvents txtPNCT As System.Windows.Forms.TextBox
	Public WithEvents txtPTEXF As System.Windows.Forms.TextBox
	Public WithEvents txtPTEFF As System.Windows.Forms.TextBox
	Public WithEvents lblCT As System.Windows.Forms.Label
	Public WithEvents lblNCT As System.Windows.Forms.Label
	Public WithEvents lblEXF As System.Windows.Forms.Label
	Public WithEvents lblPEFF As System.Windows.Forms.Label
	Public WithEvents fraTaxes As System.Windows.Forms.GroupBox
	Public WithEvents uctPMURITax1 As uctPMURITaxControl.uctPMURITax
	Private WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents Commission1 As uctPMUCommission.Commission
	Private WithEvents _SSTab1_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents uctInstalments1 As uctInstalmentsControl.uctInstalments
	Private WithEvents _SSTab1_TabPage4 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Public WithEvents txtPolicyHolderFull As System.Windows.Forms.TextBox
	Public WithEvents txtPolicyHolder As System.Windows.Forms.TextBox
	Public WithEvents lblExpiryDate As System.Windows.Forms.Label
	Public WithEvents lblCoverFromDate As System.Windows.Forms.Label
	Public WithEvents lblInceptionDate As System.Windows.Forms.Label
	Public WithEvents lblPolicyRef As System.Windows.Forms.Label
	Public WithEvents lblAgent As System.Windows.Forms.Label
	Public WithEvents lblPolicyHolder As System.Windows.Forms.Label
	Public WithEvents Line1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraPaymentTerms = New System.Windows.Forms.GroupBox()
        Me.lblPaymentTerms = New System.Windows.Forms.Label()
        Me.cboPaymentTerms = New System.Windows.Forms.ComboBox()
        Me.lblCollectionFrequency = New System.Windows.Forms.Label()
        Me.cboCollectionFrequency = New System.Windows.Forms.ComboBox()
        Me.OptPutMTAOnNextInstalment = New System.Windows.Forms.RadioButton()
        Me.OptInvoice = New System.Windows.Forms.RadioButton()
        Me.OptInstalments = New System.Windows.Forms.RadioButton()
        Me.optPayNow = New System.Windows.Forms.RadioButton()
        Me.optBankGuarantee = New System.Windows.Forms.RadioButton()
        Me.optMarkForCollection = New System.Windows.Forms.RadioButton()
        Me.optCashDeposit = New System.Windows.Forms.RadioButton()
        Me.txtGrossRoundTotal = New System.Windows.Forms.TextBox()
        Me.cmdDocArchive = New System.Windows.Forms.Button()
        Me.cmdPrintDocument = New System.Windows.Forms.Button()
        Me.StatusBar = New System.Windows.Forms.StatusStrip()
        Me._StatusBar_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.cmdSaveQuote = New System.Windows.Forms.Button()
        Me.cmdMakeLive = New System.Windows.Forms.Button()
        Me.cmdRequote = New System.Windows.Forms.Button()
        Me.cmdPrintProposal = New System.Windows.Forms.Button()
        Me.cmdPrintQuote = New System.Windows.Forms.Button()
        Me.fraTotals = New System.Windows.Forms.GroupBox()
        Me.txtRoundAmt = New System.Windows.Forms.TextBox()
        Me.txtNettotal = New System.Windows.Forms.TextBox()
        Me.txtCurrency = New System.Windows.Forms.TextBox()
        Me.txtTaxTotal = New System.Windows.Forms.TextBox()
        Me.txtFeeTotal = New System.Windows.Forms.TextBox()
        Me.txtGrossTotal = New System.Windows.Forms.TextBox()
        Me.lblGrossRoundedTotal = New System.Windows.Forms.Label()
        Me.lblRoundOffAmount = New System.Windows.Forms.Label()
        Me.lblNetTotal = New System.Windows.Forms.Label()
        Me.lblGrossTotal = New System.Windows.Forms.Label()
        Me.lblFeeTotal = New System.Windows.Forms.Label()
        Me.lblTaxTotal = New System.Windows.Forms.Label()
        Me.lblCurrency = New System.Windows.Forms.Label()
        Me.txtExpiryDate = New System.Windows.Forms.TextBox()
        Me.txtInceptionDate = New System.Windows.Forms.TextBox()
        Me.txtCoverFromDate = New System.Windows.Forms.TextBox()
        Me.txtPolicyRef = New System.Windows.Forms.TextBox()
        Me.txtAgent = New System.Windows.Forms.TextBox()
        Me.SSTab1 = New System.Windows.Forms.TabControl()
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage()
        Me.cmdQuoteAllRisks = New System.Windows.Forms.Button()
        Me.btnNOChange = New System.Windows.Forms.Button()
        Me.btnNoChangeAll = New System.Windows.Forms.Button()
        Me.cmdAddRisk = New System.Windows.Forms.Button()
        Me.cmdDeleteRisk = New System.Windows.Forms.Button()
        Me.cmdCopyRisk = New System.Windows.Forms.Button()
        Me.cmdEditRisk = New System.Windows.Forms.Button()
        Me.cmdApplyDiscount = New System.Windows.Forms.Button()
        Me.frameListRisk = New System.Windows.Forms.GroupBox()
        Me.uctPMUListRisk1 = New uctPMUListRiskControl.uctPMUListRisk()
        Me.cmdAddTask = New System.Windows.Forms.Button()
        Me.cmdBackdateMTA = New System.Windows.Forms.Button()
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage()
        Me.uctPMUFees1 = New uctPMUFeesControl.uctPMUFees()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.txtPFF = New System.Windows.Forms.TextBox()
        Me.txtPEXF = New System.Windows.Forms.TextBox()
        Me.txtPFNCT = New System.Windows.Forms.TextBox()
        Me.lblPFF = New System.Windows.Forms.Label()
        Me.lblPEXF = New System.Windows.Forms.Label()
        Me.lblPNCT = New System.Windows.Forms.Label()
        Me.Frame2 = New System.Windows.Forms.GroupBox()
        Me.txtPFEFF = New System.Windows.Forms.TextBox()
        Me.txtPFEXFF = New System.Windows.Forms.TextBox()
        Me.txtPFNCTT = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage()
        Me.uctPMURITax1 = New uctPMURITaxControl.uctPMURITax()
        Me.fraTaxes = New System.Windows.Forms.GroupBox()
        Me.txtPCT = New System.Windows.Forms.TextBox()
        Me.txtPNCT = New System.Windows.Forms.TextBox()
        Me.txtPTEXF = New System.Windows.Forms.TextBox()
        Me.txtPTEFF = New System.Windows.Forms.TextBox()
        Me.lblCT = New System.Windows.Forms.Label()
        Me.lblNCT = New System.Windows.Forms.Label()
        Me.lblEXF = New System.Windows.Forms.Label()
        Me.lblPEFF = New System.Windows.Forms.Label()
        Me.fraRTaxes = New System.Windows.Forms.GroupBox()
        Me.txtTotalRiskTax = New System.Windows.Forms.TextBox()
        Me.txtTotalNonTax = New System.Windows.Forms.TextBox()
        Me.txtTEF = New System.Windows.Forms.TextBox()
        Me.txtTEFF = New System.Windows.Forms.TextBox()
        Me.lblRiskCTax = New System.Windows.Forms.Label()
        Me.lblNonCTax = New System.Windows.Forms.Label()
        Me.lblTEF = New System.Windows.Forms.Label()
        Me.lblEFF = New System.Windows.Forms.Label()
        Me._SSTab1_TabPage3 = New System.Windows.Forms.TabPage()
        Me.Commission1 = New uctPMUCommission.Commission()
        Me._SSTab1_TabPage4 = New System.Windows.Forms.TabPage()
        Me.uctInstalments1 = New uctInstalmentsControl.uctInstalments()
        Me.txtPolicyHolderFull = New System.Windows.Forms.TextBox()
        Me.txtPolicyHolder = New System.Windows.Forms.TextBox()
        Me.lblExpiryDate = New System.Windows.Forms.Label()
        Me.lblCoverFromDate = New System.Windows.Forms.Label()
        Me.lblInceptionDate = New System.Windows.Forms.Label()
        Me.lblPolicyRef = New System.Windows.Forms.Label()
        Me.lblAgent = New System.Windows.Forms.Label()
        Me.lblPolicyHolder = New System.Windows.Forms.Label()
        Me.Line1 = New System.Windows.Forms.Label()
        Me.cmdWrite = New System.Windows.Forms.Button()
        Me.fraPaymentTerms.SuspendLayout()
        Me.StatusBar.SuspendLayout()
        Me.fraTotals.SuspendLayout()
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.frameListRisk.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me.fraTaxes.SuspendLayout()
        Me.fraRTaxes.SuspendLayout()
        Me._SSTab1_TabPage3.SuspendLayout()
        Me._SSTab1_TabPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraPaymentTerms
        '
        Me.fraPaymentTerms.BackColor = System.Drawing.SystemColors.Control
        Me.fraPaymentTerms.Controls.Add(Me.lblPaymentTerms)
        Me.fraPaymentTerms.Controls.Add(Me.cboPaymentTerms)
        Me.fraPaymentTerms.Controls.Add(Me.lblCollectionFrequency)
        Me.fraPaymentTerms.Controls.Add(Me.cboCollectionFrequency)
        Me.fraPaymentTerms.Controls.Add(Me.OptPutMTAOnNextInstalment)
        Me.fraPaymentTerms.Controls.Add(Me.OptInvoice)
        Me.fraPaymentTerms.Controls.Add(Me.OptInstalments)
        Me.fraPaymentTerms.Controls.Add(Me.optPayNow)
        Me.fraPaymentTerms.Controls.Add(Me.optBankGuarantee)
        Me.fraPaymentTerms.Controls.Add(Me.optMarkForCollection)
        Me.fraPaymentTerms.Controls.Add(Me.optCashDeposit)
        Me.fraPaymentTerms.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPaymentTerms.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPaymentTerms.Location = New System.Drawing.Point(338, 479)
        Me.fraPaymentTerms.Name = "fraPaymentTerms"
        Me.fraPaymentTerms.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPaymentTerms.Size = New System.Drawing.Size(457, 179)
        Me.fraPaymentTerms.TabIndex = 16
        Me.fraPaymentTerms.TabStop = False
        Me.fraPaymentTerms.Text = "Payment Terms"
        '
        'lblPaymentTerms
        '
        Me.lblPaymentTerms.AutoSize = True
        Me.lblPaymentTerms.BackColor = System.Drawing.SystemColors.Control
        Me.lblPaymentTerms.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPaymentTerms.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaymentTerms.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPaymentTerms.Location = New System.Drawing.Point(184, 43)
        Me.lblPaymentTerms.Name = "lblPaymentTerms"
        Me.lblPaymentTerms.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPaymentTerms.Size = New System.Drawing.Size(86, 13)
        Me.lblPaymentTerms.TabIndex = 87
        Me.lblPaymentTerms.Text = "Payment Terms :"
        '
        'cboPaymentTerms
        '
        Me.cboPaymentTerms.BackColor = System.Drawing.SystemColors.Window
        Me.cboPaymentTerms.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPaymentTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPaymentTerms.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPaymentTerms.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPaymentTerms.Location = New System.Drawing.Point(298, 39)
        Me.cboPaymentTerms.Name = "cboPaymentTerms"
        Me.cboPaymentTerms.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPaymentTerms.Size = New System.Drawing.Size(153, 21)
        Me.cboPaymentTerms.TabIndex = 86
        '
        'lblCollectionFrequency
        '
        Me.lblCollectionFrequency.AutoSize = True
        Me.lblCollectionFrequency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCollectionFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCollectionFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCollectionFrequency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCollectionFrequency.Location = New System.Drawing.Point(184, 19)
        Me.lblCollectionFrequency.Name = "lblCollectionFrequency"
        Me.lblCollectionFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCollectionFrequency.Size = New System.Drawing.Size(112, 13)
        Me.lblCollectionFrequency.TabIndex = 85
        Me.lblCollectionFrequency.Text = "Collection Frequency :"
        '
        'cboCollectionFrequency
        '
        Me.cboCollectionFrequency.BackColor = System.Drawing.SystemColors.Window
        Me.cboCollectionFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCollectionFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCollectionFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCollectionFrequency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCollectionFrequency.Location = New System.Drawing.Point(298, 15)
        Me.cboCollectionFrequency.Name = "cboCollectionFrequency"
        Me.cboCollectionFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCollectionFrequency.Size = New System.Drawing.Size(153, 21)
        Me.cboCollectionFrequency.TabIndex = 84
        '
        'OptPutMTAOnNextInstalment
        '
        Me.OptPutMTAOnNextInstalment.BackColor = System.Drawing.SystemColors.Control
        Me.OptPutMTAOnNextInstalment.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptPutMTAOnNextInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptPutMTAOnNextInstalment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptPutMTAOnNextInstalment.Location = New System.Drawing.Point(18, 65)
        Me.OptPutMTAOnNextInstalment.Name = "OptPutMTAOnNextInstalment"
        Me.OptPutMTAOnNextInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptPutMTAOnNextInstalment.Size = New System.Drawing.Size(241, 17)
        Me.OptPutMTAOnNextInstalment.TabIndex = 1
        Me.OptPutMTAOnNextInstalment.TabStop = True
        Me.OptPutMTAOnNextInstalment.Text = "Put MTA on next instalment renewal"
        Me.OptPutMTAOnNextInstalment.UseVisualStyleBackColor = False
        '
        'OptInvoice
        '
        Me.OptInvoice.BackColor = System.Drawing.SystemColors.Control
        Me.OptInvoice.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptInvoice.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptInvoice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptInvoice.Location = New System.Drawing.Point(18, 19)
        Me.OptInvoice.Name = "OptInvoice"
        Me.OptInvoice.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptInvoice.Size = New System.Drawing.Size(89, 21)
        Me.OptInvoice.TabIndex = 0
        Me.OptInvoice.TabStop = True
        Me.OptInvoice.Text = "Invoice"
        Me.OptInvoice.UseVisualStyleBackColor = False
        '
        'OptInstalments
        '
        Me.OptInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.OptInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.OptInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OptInstalments.Location = New System.Drawing.Point(18, 84)
        Me.OptInstalments.Name = "OptInstalments"
        Me.OptInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OptInstalments.Size = New System.Drawing.Size(113, 21)
        Me.OptInstalments.TabIndex = 83
        Me.OptInstalments.TabStop = True
        Me.OptInstalments.Text = "Instalments"
        Me.OptInstalments.UseVisualStyleBackColor = False
        '
        'optPayNow
        '
        Me.optPayNow.BackColor = System.Drawing.SystemColors.Control
        Me.optPayNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPayNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPayNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optPayNow.Location = New System.Drawing.Point(18, 127)
        Me.optPayNow.Name = "optPayNow"
        Me.optPayNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optPayNow.Size = New System.Drawing.Size(97, 21)
        Me.optPayNow.TabIndex = 2
        Me.optPayNow.TabStop = True
        Me.optPayNow.Text = "Pay Now"
        Me.optPayNow.UseVisualStyleBackColor = False
        '
        'optBankGuarantee
        '
        Me.optBankGuarantee.BackColor = System.Drawing.SystemColors.Control
        Me.optBankGuarantee.Cursor = System.Windows.Forms.Cursors.Default
        Me.optBankGuarantee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optBankGuarantee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBankGuarantee.Location = New System.Drawing.Point(18, 40)
        Me.optBankGuarantee.Name = "optBankGuarantee"
        Me.optBankGuarantee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optBankGuarantee.Size = New System.Drawing.Size(123, 21)
        Me.optBankGuarantee.TabIndex = 3
        Me.optBankGuarantee.TabStop = True
        Me.optBankGuarantee.Text = "Bank Guarantee"
        Me.optBankGuarantee.UseVisualStyleBackColor = False
        '
        'optMarkForCollection
        '
        Me.optMarkForCollection.BackColor = System.Drawing.SystemColors.Control
        Me.optMarkForCollection.Cursor = System.Windows.Forms.Cursors.Default
        Me.optMarkForCollection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optMarkForCollection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optMarkForCollection.Location = New System.Drawing.Point(560, 40)
        Me.optMarkForCollection.Name = "optMarkForCollection"
        Me.optMarkForCollection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optMarkForCollection.Size = New System.Drawing.Size(131, 21)
        Me.optMarkForCollection.TabIndex = 5
        Me.optMarkForCollection.TabStop = True
        Me.optMarkForCollection.Text = "Mark For Collection"
        Me.optMarkForCollection.UseVisualStyleBackColor = False
        '
        'optCashDeposit
        '
        Me.optCashDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.optCashDeposit.Cursor = System.Windows.Forms.Cursors.Default
        Me.optCashDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCashDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCashDeposit.Location = New System.Drawing.Point(18, 100)
        Me.optCashDeposit.Name = "optCashDeposit"
        Me.optCashDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optCashDeposit.Size = New System.Drawing.Size(113, 29)
        Me.optCashDeposit.TabIndex = 4
        Me.optCashDeposit.TabStop = True
        Me.optCashDeposit.Text = "Cash Deposit"
        Me.optCashDeposit.UseVisualStyleBackColor = False
        '
        'txtGrossRoundTotal
        '
        Me.txtGrossRoundTotal.AcceptsReturn = True
        Me.txtGrossRoundTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtGrossRoundTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGrossRoundTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrossRoundTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGrossRoundTotal.Location = New System.Drawing.Point(169, 107)
        Me.txtGrossRoundTotal.MaxLength = 0
        Me.txtGrossRoundTotal.Name = "txtGrossRoundTotal"
        Me.txtGrossRoundTotal.ReadOnly = True
        Me.txtGrossRoundTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGrossRoundTotal.Size = New System.Drawing.Size(127, 20)
        Me.txtGrossRoundTotal.TabIndex = 15
        Me.txtGrossRoundTotal.TabStop = False
        Me.txtGrossRoundTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdDocArchive
        '
        Me.cmdDocArchive.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDocArchive.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDocArchive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDocArchive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDocArchive.Location = New System.Drawing.Point(387, 664)
        Me.cmdDocArchive.Name = "cmdDocArchive"
        Me.cmdDocArchive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDocArchive.Size = New System.Drawing.Size(99, 25)
        Me.cmdDocArchive.TabIndex = 21
        Me.cmdDocArchive.Text = "&Doc Archive"
        Me.cmdDocArchive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDocArchive.UseVisualStyleBackColor = False
        '
        'cmdPrintDocument
        '
        Me.cmdPrintDocument.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrintDocument.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrintDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrintDocument.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrintDocument.Location = New System.Drawing.Point(8, 664)
        Me.cmdPrintDocument.Name = "cmdPrintDocument"
        Me.cmdPrintDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrintDocument.Size = New System.Drawing.Size(92, 25)
        Me.cmdPrintDocument.TabIndex = 17
        Me.cmdPrintDocument.Text = "Print D&ocument"
        Me.cmdPrintDocument.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrintDocument.UseVisualStyleBackColor = False
        '
        'StatusBar
        '
        Me.StatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._StatusBar_Panel1})
        Me.StatusBar.Location = New System.Drawing.Point(0, 692)
        Me.StatusBar.Name = "StatusBar"
        Me.StatusBar.ShowItemToolTips = True
        Me.StatusBar.Size = New System.Drawing.Size(804, 22)
        Me.StatusBar.TabIndex = 24
        '
        '_StatusBar_Panel1
        '
        Me._StatusBar_Panel1.AutoSize = False
        Me._StatusBar_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._StatusBar_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._StatusBar_Panel1.DoubleClickEnabled = True
        Me._StatusBar_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._StatusBar_Panel1.Name = "_StatusBar_Panel1"
        Me._StatusBar_Panel1.Size = New System.Drawing.Size(789, 22)
        Me._StatusBar_Panel1.Spring = True
        Me._StatusBar_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdSaveQuote
        '
        Me.cmdSaveQuote.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSaveQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSaveQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSaveQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSaveQuote.Location = New System.Drawing.Point(688, 664)
        Me.cmdSaveQuote.Name = "cmdSaveQuote"
        Me.cmdSaveQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSaveQuote.Size = New System.Drawing.Size(105, 25)
        Me.cmdSaveQuote.TabIndex = 23
        Me.cmdSaveQuote.Text = "&Save Quote"
        Me.cmdSaveQuote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSaveQuote.UseVisualStyleBackColor = False
        '
        'cmdMakeLive
        '
        Me.cmdMakeLive.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMakeLive.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMakeLive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMakeLive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMakeLive.Location = New System.Drawing.Point(576, 664)
        Me.cmdMakeLive.Name = "cmdMakeLive"
        Me.cmdMakeLive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMakeLive.Size = New System.Drawing.Size(106, 25)
        Me.cmdMakeLive.TabIndex = 22
        Me.cmdMakeLive.Text = "&Make Live"
        Me.cmdMakeLive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMakeLive.UseVisualStyleBackColor = False
        '
        'cmdRequote
        '
        Me.cmdRequote.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRequote.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRequote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRequote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRequote.Location = New System.Drawing.Point(293, 664)
        Me.cmdRequote.Name = "cmdRequote"
        Me.cmdRequote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRequote.Size = New System.Drawing.Size(87, 25)
        Me.cmdRequote.TabIndex = 20
        Me.cmdRequote.Text = "Req&uote"
        Me.cmdRequote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRequote.UseVisualStyleBackColor = False
        '
        'cmdPrintProposal
        '
        Me.cmdPrintProposal.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrintProposal.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrintProposal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrintProposal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrintProposal.Location = New System.Drawing.Point(185, 664)
        Me.cmdPrintProposal.Name = "cmdPrintProposal"
        Me.cmdPrintProposal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrintProposal.Size = New System.Drawing.Size(102, 25)
        Me.cmdPrintProposal.TabIndex = 19
        Me.cmdPrintProposal.Text = "Print &Proposal"
        Me.cmdPrintProposal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrintProposal.UseVisualStyleBackColor = False
        '
        'cmdPrintQuote
        '
        Me.cmdPrintQuote.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrintQuote.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrintQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrintQuote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrintQuote.Location = New System.Drawing.Point(105, 664)
        Me.cmdPrintQuote.Name = "cmdPrintQuote"
        Me.cmdPrintQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrintQuote.Size = New System.Drawing.Size(78, 25)
        Me.cmdPrintQuote.TabIndex = 18
        Me.cmdPrintQuote.Text = "Print &Quote"
        Me.cmdPrintQuote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrintQuote.UseVisualStyleBackColor = False
        '
        'fraTotals
        '
        Me.fraTotals.BackColor = System.Drawing.SystemColors.Control
        Me.fraTotals.Controls.Add(Me.txtRoundAmt)
        Me.fraTotals.Controls.Add(Me.txtNettotal)
        Me.fraTotals.Controls.Add(Me.txtGrossRoundTotal)
        Me.fraTotals.Controls.Add(Me.txtCurrency)
        Me.fraTotals.Controls.Add(Me.txtTaxTotal)
        Me.fraTotals.Controls.Add(Me.txtFeeTotal)
        Me.fraTotals.Controls.Add(Me.txtGrossTotal)
        Me.fraTotals.Controls.Add(Me.lblGrossRoundedTotal)
        Me.fraTotals.Controls.Add(Me.lblRoundOffAmount)
        Me.fraTotals.Controls.Add(Me.lblNetTotal)
        Me.fraTotals.Controls.Add(Me.lblGrossTotal)
        Me.fraTotals.Controls.Add(Me.lblFeeTotal)
        Me.fraTotals.Controls.Add(Me.lblTaxTotal)
        Me.fraTotals.Controls.Add(Me.lblCurrency)
        Me.fraTotals.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTotals.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTotals.Location = New System.Drawing.Point(8, 472)
        Me.fraTotals.Name = "fraTotals"
        Me.fraTotals.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTotals.Size = New System.Drawing.Size(326, 186)
        Me.fraTotals.TabIndex = 14
        Me.fraTotals.TabStop = False
        Me.fraTotals.Text = "Client Totals"
        '
        'txtRoundAmt
        '
        Me.txtRoundAmt.AcceptsReturn = True
        Me.txtRoundAmt.BackColor = System.Drawing.SystemColors.Control
        Me.txtRoundAmt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRoundAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRoundAmt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRoundAmt.Location = New System.Drawing.Point(169, 68)
        Me.txtRoundAmt.MaxLength = 0
        Me.txtRoundAmt.Name = "txtRoundAmt"
        Me.txtRoundAmt.ReadOnly = True
        Me.txtRoundAmt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRoundAmt.Size = New System.Drawing.Size(127, 20)
        Me.txtRoundAmt.TabIndex = 11
        Me.txtRoundAmt.TabStop = False
        Me.txtRoundAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNettotal
        '
        Me.txtNettotal.AcceptsReturn = True
        Me.txtNettotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtNettotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNettotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNettotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNettotal.Location = New System.Drawing.Point(13, 68)
        Me.txtNettotal.MaxLength = 0
        Me.txtNettotal.Name = "txtNettotal"
        Me.txtNettotal.ReadOnly = True
        Me.txtNettotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNettotal.Size = New System.Drawing.Size(127, 20)
        Me.txtNettotal.TabIndex = 3
        Me.txtNettotal.TabStop = False
        '
        'txtCurrency
        '
        Me.txtCurrency.AcceptsReturn = True
        Me.txtCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrency.Location = New System.Drawing.Point(11, 31)
        Me.txtCurrency.MaxLength = 0
        Me.txtCurrency.Name = "txtCurrency"
        Me.txtCurrency.ReadOnly = True
        Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrency.Size = New System.Drawing.Size(127, 20)
        Me.txtCurrency.TabIndex = 1
        Me.txtCurrency.TabStop = False
        '
        'txtTaxTotal
        '
        Me.txtTaxTotal.AcceptsReturn = True
        Me.txtTaxTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtTaxTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTaxTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaxTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxTotal.Location = New System.Drawing.Point(13, 107)
        Me.txtTaxTotal.MaxLength = 0
        Me.txtTaxTotal.Name = "txtTaxTotal"
        Me.txtTaxTotal.ReadOnly = True
        Me.txtTaxTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTaxTotal.Size = New System.Drawing.Size(127, 20)
        Me.txtTaxTotal.TabIndex = 5
        Me.txtTaxTotal.TabStop = False
        '
        'txtFeeTotal
        '
        Me.txtFeeTotal.AcceptsReturn = True
        Me.txtFeeTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtFeeTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFeeTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFeeTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFeeTotal.Location = New System.Drawing.Point(12, 150)
        Me.txtFeeTotal.MaxLength = 0
        Me.txtFeeTotal.Name = "txtFeeTotal"
        Me.txtFeeTotal.ReadOnly = True
        Me.txtFeeTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFeeTotal.Size = New System.Drawing.Size(127, 20)
        Me.txtFeeTotal.TabIndex = 7
        Me.txtFeeTotal.TabStop = False
        '
        'txtGrossTotal
        '
        Me.txtGrossTotal.AcceptsReturn = True
        Me.txtGrossTotal.BackColor = System.Drawing.SystemColors.Control
        Me.txtGrossTotal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtGrossTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrossTotal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtGrossTotal.Location = New System.Drawing.Point(169, 28)
        Me.txtGrossTotal.MaxLength = 0
        Me.txtGrossTotal.Name = "txtGrossTotal"
        Me.txtGrossTotal.ReadOnly = True
        Me.txtGrossTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtGrossTotal.Size = New System.Drawing.Size(127, 20)
        Me.txtGrossTotal.TabIndex = 9
        Me.txtGrossTotal.TabStop = False
        '
        'lblGrossRoundedTotal
        '
        Me.lblGrossRoundedTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblGrossRoundedTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGrossRoundedTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrossRoundedTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGrossRoundedTotal.Location = New System.Drawing.Point(167, 91)
        Me.lblGrossRoundedTotal.Name = "lblGrossRoundedTotal"
        Me.lblGrossRoundedTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGrossRoundedTotal.Size = New System.Drawing.Size(129, 17)
        Me.lblGrossRoundedTotal.TabIndex = 12
        Me.lblGrossRoundedTotal.Text = "Gross Rounded Total:"
        '
        'lblRoundOffAmount
        '
        Me.lblRoundOffAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblRoundOffAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRoundOffAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRoundOffAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRoundOffAmount.Location = New System.Drawing.Point(167, 54)
        Me.lblRoundOffAmount.Name = "lblRoundOffAmount"
        Me.lblRoundOffAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRoundOffAmount.Size = New System.Drawing.Size(113, 25)
        Me.lblRoundOffAmount.TabIndex = 10
        Me.lblRoundOffAmount.Text = "Round Off Amount:"
        '
        'lblNetTotal
        '
        Me.lblNetTotal.AutoSize = True
        Me.lblNetTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblNetTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNetTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNetTotal.Location = New System.Drawing.Point(12, 54)
        Me.lblNetTotal.Name = "lblNetTotal"
        Me.lblNetTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNetTotal.Size = New System.Drawing.Size(51, 13)
        Me.lblNetTotal.TabIndex = 2
        Me.lblNetTotal.Text = "Net Total"
        '
        'lblGrossTotal
        '
        Me.lblGrossTotal.AutoSize = True
        Me.lblGrossTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblGrossTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGrossTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrossTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGrossTotal.Location = New System.Drawing.Point(166, 16)
        Me.lblGrossTotal.Name = "lblGrossTotal"
        Me.lblGrossTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGrossTotal.Size = New System.Drawing.Size(61, 13)
        Me.lblGrossTotal.TabIndex = 8
        Me.lblGrossTotal.Text = "Gross Total"
        '
        'lblFeeTotal
        '
        Me.lblFeeTotal.AutoSize = True
        Me.lblFeeTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblFeeTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFeeTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFeeTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFeeTotal.Location = New System.Drawing.Point(12, 134)
        Me.lblFeeTotal.Name = "lblFeeTotal"
        Me.lblFeeTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFeeTotal.Size = New System.Drawing.Size(52, 13)
        Me.lblFeeTotal.TabIndex = 6
        Me.lblFeeTotal.Text = "Fee Total"
        '
        'lblTaxTotal
        '
        Me.lblTaxTotal.AutoSize = True
        Me.lblTaxTotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaxTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaxTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaxTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaxTotal.Location = New System.Drawing.Point(13, 91)
        Me.lblTaxTotal.Name = "lblTaxTotal"
        Me.lblTaxTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaxTotal.Size = New System.Drawing.Size(52, 13)
        Me.lblTaxTotal.TabIndex = 4
        Me.lblTaxTotal.Text = "Tax Total"
        '
        'lblCurrency
        '
        Me.lblCurrency.AutoSize = True
        Me.lblCurrency.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrency.Location = New System.Drawing.Point(13, 16)
        Me.lblCurrency.Name = "lblCurrency"
        Me.lblCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrency.Size = New System.Drawing.Size(49, 13)
        Me.lblCurrency.TabIndex = 0
        Me.lblCurrency.Text = "Currency"
        '
        'txtExpiryDate
        '
        Me.txtExpiryDate.AcceptsReturn = True
        Me.txtExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExpiryDate.Location = New System.Drawing.Point(657, 64)
        Me.txtExpiryDate.MaxLength = 0
        Me.txtExpiryDate.Name = "txtExpiryDate"
        Me.txtExpiryDate.ReadOnly = True
        Me.txtExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExpiryDate.Size = New System.Drawing.Size(124, 20)
        Me.txtExpiryDate.TabIndex = 12
        Me.txtExpiryDate.TabStop = False
        '
        'txtInceptionDate
        '
        Me.txtInceptionDate.AcceptsReturn = True
        Me.txtInceptionDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtInceptionDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInceptionDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInceptionDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInceptionDate.Location = New System.Drawing.Point(104, 64)
        Me.txtInceptionDate.MaxLength = 0
        Me.txtInceptionDate.Name = "txtInceptionDate"
        Me.txtInceptionDate.ReadOnly = True
        Me.txtInceptionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInceptionDate.Size = New System.Drawing.Size(172, 20)
        Me.txtInceptionDate.TabIndex = 8
        Me.txtInceptionDate.TabStop = False
        '
        'txtCoverFromDate
        '
        Me.txtCoverFromDate.AcceptsReturn = True
        Me.txtCoverFromDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtCoverFromDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCoverFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCoverFromDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCoverFromDate.Location = New System.Drawing.Point(410, 64)
        Me.txtCoverFromDate.MaxLength = 0
        Me.txtCoverFromDate.Name = "txtCoverFromDate"
        Me.txtCoverFromDate.ReadOnly = True
        Me.txtCoverFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCoverFromDate.Size = New System.Drawing.Size(124, 20)
        Me.txtCoverFromDate.TabIndex = 10
        Me.txtCoverFromDate.TabStop = False
        '
        'txtPolicyRef
        '
        Me.txtPolicyRef.AcceptsReturn = True
        Me.txtPolicyRef.BackColor = System.Drawing.SystemColors.Control
        Me.txtPolicyRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyRef.Location = New System.Drawing.Point(104, 36)
        Me.txtPolicyRef.MaxLength = 0
        Me.txtPolicyRef.Name = "txtPolicyRef"
        Me.txtPolicyRef.ReadOnly = True
        Me.txtPolicyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyRef.Size = New System.Drawing.Size(172, 20)
        Me.txtPolicyRef.TabIndex = 4
        Me.txtPolicyRef.TabStop = False
        '
        'txtAgent
        '
        Me.txtAgent.AcceptsReturn = True
        Me.txtAgent.BackColor = System.Drawing.SystemColors.Control
        Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgent.Location = New System.Drawing.Point(338, 36)
        Me.txtAgent.MaxLength = 0
        Me.txtAgent.Name = "txtAgent"
        Me.txtAgent.ReadOnly = True
        Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgent.Size = New System.Drawing.Size(456, 20)
        Me.txtAgent.TabIndex = 6
        Me.txtAgent.TabStop = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage3)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage4)
        Me.SSTab1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(156, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 96)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(792, 377)
        Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.SSTab1.TabIndex = 13
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdQuoteAllRisks)
        Me._SSTab1_TabPage0.Controls.Add(Me.btnNOChange)
        Me._SSTab1_TabPage0.Controls.Add(Me.btnNoChangeAll)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdAddRisk)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdDeleteRisk)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdCopyRisk)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdEditRisk)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdApplyDiscount)
        Me._SSTab1_TabPage0.Controls.Add(Me.frameListRisk)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdAddTask)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdBackdateMTA)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(784, 351)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 - &Risk"
        '
        'cmdQuoteAllRisks
        '
        Me.cmdQuoteAllRisks.BackColor = System.Drawing.SystemColors.Control
        Me.cmdQuoteAllRisks.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdQuoteAllRisks.Enabled = False
        Me.cmdQuoteAllRisks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdQuoteAllRisks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdQuoteAllRisks.Location = New System.Drawing.Point(427, 323)
        Me.cmdQuoteAllRisks.Name = "cmdQuoteAllRisks"
        Me.cmdQuoteAllRisks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdQuoteAllRisks.Size = New System.Drawing.Size(91, 23)
        Me.cmdQuoteAllRisks.TabIndex = 10
        Me.cmdQuoteAllRisks.Text = "&Quote All Risks"
        Me.cmdQuoteAllRisks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdQuoteAllRisks.UseVisualStyleBackColor = False
        '
        'btnNOChange
        '
        Me.btnNOChange.BackColor = System.Drawing.SystemColors.Control
        Me.btnNOChange.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNOChange.Enabled = False
        Me.btnNOChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNOChange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnNOChange.Location = New System.Drawing.Point(135, 323)
        Me.btnNOChange.Name = "btnNOChange"
        Me.btnNOChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNOChange.Size = New System.Drawing.Size(70, 23)
        Me.btnNOChange.TabIndex = 9
        Me.btnNOChange.Text = "No Change"
        Me.btnNOChange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnNOChange.UseVisualStyleBackColor = False
        '
        'btnNoChangeAll
        '
        Me.btnNoChangeAll.BackColor = System.Drawing.SystemColors.Control
        Me.btnNoChangeAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNoChangeAll.Enabled = False
        Me.btnNoChangeAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNoChangeAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnNoChangeAll.Location = New System.Drawing.Point(519, 323)
        Me.btnNoChangeAll.Name = "btnNoChangeAll"
        Me.btnNoChangeAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNoChangeAll.Size = New System.Drawing.Size(86, 23)
        Me.btnNoChangeAll.TabIndex = 8
        Me.btnNoChangeAll.Text = "No Change All"
        Me.btnNoChangeAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnNoChangeAll.UseVisualStyleBackColor = False
        '
        'cmdAddRisk
        '
        Me.cmdAddRisk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddRisk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddRisk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddRisk.Location = New System.Drawing.Point(3, 323)
        Me.cmdAddRisk.Name = "cmdAddRisk"
        Me.cmdAddRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddRisk.Size = New System.Drawing.Size(63, 23)
        Me.cmdAddRisk.TabIndex = 1
        Me.cmdAddRisk.Text = "&Add"
        Me.cmdAddRisk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddRisk.UseVisualStyleBackColor = False
        '
        'cmdDeleteRisk
        '
        Me.cmdDeleteRisk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteRisk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteRisk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteRisk.Location = New System.Drawing.Point(274, 323)
        Me.cmdDeleteRisk.Name = "cmdDeleteRisk"
        Me.cmdDeleteRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteRisk.Size = New System.Drawing.Size(63, 23)
        Me.cmdDeleteRisk.TabIndex = 4
        Me.cmdDeleteRisk.Text = "&Delete"
        Me.cmdDeleteRisk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteRisk.UseVisualStyleBackColor = False
        '
        'cmdCopyRisk
        '
        Me.cmdCopyRisk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopyRisk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopyRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopyRisk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopyRisk.Location = New System.Drawing.Point(208, 323)
        Me.cmdCopyRisk.Name = "cmdCopyRisk"
        Me.cmdCopyRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopyRisk.Size = New System.Drawing.Size(63, 23)
        Me.cmdCopyRisk.TabIndex = 3
        Me.cmdCopyRisk.Text = "&Copy"
        Me.cmdCopyRisk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCopyRisk.UseVisualStyleBackColor = False
        '
        'cmdEditRisk
        '
        Me.cmdEditRisk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditRisk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditRisk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditRisk.Location = New System.Drawing.Point(69, 323)
        Me.cmdEditRisk.Name = "cmdEditRisk"
        Me.cmdEditRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditRisk.Size = New System.Drawing.Size(63, 23)
        Me.cmdEditRisk.TabIndex = 2
        Me.cmdEditRisk.Text = "&Edit"
        Me.cmdEditRisk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditRisk.UseVisualStyleBackColor = False
        '
        'cmdApplyDiscount
        '
        Me.cmdApplyDiscount.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApplyDiscount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApplyDiscount.Enabled = False
        Me.cmdApplyDiscount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApplyDiscount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApplyDiscount.Location = New System.Drawing.Point(339, 323)
        Me.cmdApplyDiscount.Name = "cmdApplyDiscount"
        Me.cmdApplyDiscount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApplyDiscount.Size = New System.Drawing.Size(86, 23)
        Me.cmdApplyDiscount.TabIndex = 5
        Me.cmdApplyDiscount.Text = "App&ly Discount"
        Me.cmdApplyDiscount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApplyDiscount.UseVisualStyleBackColor = False
        '
        'frameListRisk
        '
        Me.frameListRisk.BackColor = System.Drawing.SystemColors.Control
        Me.frameListRisk.Controls.Add(Me.uctPMUListRisk1)
        Me.frameListRisk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frameListRisk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frameListRisk.Location = New System.Drawing.Point(6, 2)
        Me.frameListRisk.Name = "frameListRisk"
        Me.frameListRisk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frameListRisk.Size = New System.Drawing.Size(777, 315)
        Me.frameListRisk.TabIndex = 0
        Me.frameListRisk.TabStop = False
        '
        'uctPMUListRisk1
        '
        Me.uctPMUListRisk1.AllRiskStatusQuoted = False
        Me.uctPMUListRisk1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMUListRisk1.FormLoading = False
        Me.uctPMUListRisk1.InsFileCnt = 0
        Me.uctPMUListRisk1.InsReference = ""
        Me.uctPMUListRisk1.Location = New System.Drawing.Point(4, 12)
        Me.uctPMUListRisk1.Name = "uctPMUListRisk1"
        Me.uctPMUListRisk1.ProductID = 0
        Me.uctPMUListRisk1.RiskDescription = ""
        Me.uctPMUListRisk1.RiskExpiryDate = New Date(CType(0, Long))
        Me.uctPMUListRisk1.RiskID = 0
        Me.uctPMUListRisk1.RiskInceptionDate = New Date(CType(0, Long))
        Me.uctPMUListRisk1.RiskStatus = ""
        Me.uctPMUListRisk1.RiskTotalPremium = Nothing
        Me.uctPMUListRisk1.RiskTotalSI = Nothing
        Me.uctPMUListRisk1.RiskTypeDescription = ""
        Me.uctPMUListRisk1.Size = New System.Drawing.Size(769, 299)
        Me.uctPMUListRisk1.Status = 0
        Me.uctPMUListRisk1.TabIndex = 0
        Me.uctPMUListRisk1.Task = 0
        '
        'cmdAddTask
        '
        Me.cmdAddTask.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddTask.Location = New System.Drawing.Point(703, 323)
        Me.cmdAddTask.Name = "cmdAddTask"
        Me.cmdAddTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTask.Size = New System.Drawing.Size(75, 23)
        Me.cmdAddTask.TabIndex = 7
        Me.cmdAddTask.Text = "Add &Task"
        Me.cmdAddTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddTask.UseVisualStyleBackColor = False
        '
        'cmdBackdateMTA
        '
        Me.cmdBackdateMTA.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBackdateMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBackdateMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBackdateMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBackdateMTA.Location = New System.Drawing.Point(606, 323)
        Me.cmdBackdateMTA.Name = "cmdBackdateMTA"
        Me.cmdBackdateMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBackdateMTA.Size = New System.Drawing.Size(95, 23)
        Me.cmdBackdateMTA.TabIndex = 6
        Me.cmdBackdateMTA.Text = "Backdate MTA"
        Me.cmdBackdateMTA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBackdateMTA.UseVisualStyleBackColor = False
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.uctPMUFees1)
        Me._SSTab1_TabPage1.Controls.Add(Me.Frame1)
        Me._SSTab1_TabPage1.Controls.Add(Me.Frame2)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(784, 351)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "2 - Policy &Fees"
        '
        'uctPMUFees1
        '
        Me.uctPMUFees1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMUFees1.Location = New System.Drawing.Point(8, 4)
        Me.uctPMUFees1.Name = "uctPMUFees1"
        Me.uctPMUFees1.Size = New System.Drawing.Size(769, 217)
        Me.uctPMUFees1.TabIndex = 0
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtPFF)
        Me.Frame1.Controls.Add(Me.txtPEXF)
        Me.Frame1.Controls.Add(Me.txtPFNCT)
        Me.Frame1.Controls.Add(Me.lblPFF)
        Me.Frame1.Controls.Add(Me.lblPEXF)
        Me.Frame1.Controls.Add(Me.lblPNCT)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 220)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(777, 57)
        Me.Frame1.TabIndex = 1
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Risk Fees"
        '
        'txtPFF
        '
        Me.txtPFF.AcceptsReturn = True
        Me.txtPFF.BackColor = System.Drawing.SystemColors.Control
        Me.txtPFF.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPFF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPFF.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPFF.Location = New System.Drawing.Point(32, 27)
        Me.txtPFF.MaxLength = 0
        Me.txtPFF.Name = "txtPFF"
		Me.txtPFF.ReadOnly = True
        Me.txtPFF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPFF.Size = New System.Drawing.Size(169, 20)
        Me.txtPFF.TabIndex = 1
        '
        'txtPEXF
        '
        Me.txtPEXF.AcceptsReturn = True
        Me.txtPEXF.BackColor = System.Drawing.SystemColors.Control
        Me.txtPEXF.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPEXF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPEXF.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPEXF.Location = New System.Drawing.Point(312, 27)
        Me.txtPEXF.MaxLength = 0
        Me.txtPEXF.Name = "txtPEXF"
		 Me.txtPEXF.ReadOnly = True
        Me.txtPEXF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPEXF.Size = New System.Drawing.Size(177, 20)
        Me.txtPEXF.TabIndex = 3
        '
        'txtPFNCT
        '
        Me.txtPFNCT.AcceptsReturn = True
        Me.txtPFNCT.BackColor = System.Drawing.SystemColors.Control
        Me.txtPFNCT.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPFNCT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPFNCT.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPFNCT.Location = New System.Drawing.Point(584, 27)
        Me.txtPFNCT.MaxLength = 0
        Me.txtPFNCT.Name = "txtPFNCT"
		 Me.txtPFNCT.ReadOnly = True
        Me.txtPFNCT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPFNCT.Size = New System.Drawing.Size(153, 20)
        Me.txtPFNCT.TabIndex = 5
        Me.txtPFNCT.Text = " "
        '
        'lblPFF
        '
        Me.lblPFF.BackColor = System.Drawing.SystemColors.Control
        Me.lblPFF.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPFF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPFF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPFF.Location = New System.Drawing.Point(32, 12)
        Me.lblPFF.Name = "lblPFF"
        Me.lblPFF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPFF.Size = New System.Drawing.Size(185, 17)
        Me.lblPFF.TabIndex = 0
        Me.lblPFF.Text = "Total Fees eligible for financing"
        '
        'lblPEXF
        '
        Me.lblPEXF.BackColor = System.Drawing.SystemColors.Control
        Me.lblPEXF.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPEXF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPEXF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPEXF.Location = New System.Drawing.Point(312, 12)
        Me.lblPEXF.Name = "lblPEXF"
        Me.lblPEXF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPEXF.Size = New System.Drawing.Size(217, 17)
        Me.lblPEXF.TabIndex = 2
        Me.lblPEXF.Text = "Total Fees excluded from financing"
        '
        'lblPNCT
        '
        Me.lblPNCT.BackColor = System.Drawing.SystemColors.Control
        Me.lblPNCT.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPNCT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPNCT.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPNCT.Location = New System.Drawing.Point(584, 12)
        Me.lblPNCT.Name = "lblPNCT"
        Me.lblPNCT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPNCT.Size = New System.Drawing.Size(145, 17)
        Me.lblPNCT.TabIndex = 4
        Me.lblPNCT.Text = "Total Risk Fees"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.txtPFEFF)
        Me.Frame2.Controls.Add(Me.txtPFEXFF)
        Me.Frame2.Controls.Add(Me.txtPFNCTT)
        Me.Frame2.Controls.Add(Me.Label12)
        Me.Frame2.Controls.Add(Me.Label11)
        Me.Frame2.Controls.Add(Me.Label10)
        Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(8, 284)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(777, 57)
        Me.Frame2.TabIndex = 2
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Policy Fees"
        '
        'txtPFEFF
        '
        Me.txtPFEFF.AcceptsReturn = True
        Me.txtPFEFF.BackColor = System.Drawing.SystemColors.Control
        Me.txtPFEFF.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPFEFF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPFEFF.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPFEFF.Location = New System.Drawing.Point(32, 27)
        Me.txtPFEFF.MaxLength = 0
        Me.txtPFEFF.Name = "txtPFEFF"
		Me.txtPFEFF.ReadOnly = True
        Me.txtPFEFF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPFEFF.Size = New System.Drawing.Size(169, 20)
        Me.txtPFEFF.TabIndex = 1
        '
        'txtPFEXFF
        '
        Me.txtPFEXFF.AcceptsReturn = True
        Me.txtPFEXFF.BackColor = System.Drawing.SystemColors.Control
        Me.txtPFEXFF.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPFEXFF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPFEXFF.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPFEXFF.Location = New System.Drawing.Point(312, 27)
        Me.txtPFEXFF.MaxLength = 0
        Me.txtPFEXFF.Name = "txtPFEXFF"
		Me.txtPFEXFF.ReadOnly = True
        Me.txtPFEXFF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPFEXFF.Size = New System.Drawing.Size(177, 20)
        Me.txtPFEXFF.TabIndex = 3
        '
        'txtPFNCTT
        '
        Me.txtPFNCTT.AcceptsReturn = True
        Me.txtPFNCTT.BackColor = System.Drawing.SystemColors.Control
        Me.txtPFNCTT.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPFNCTT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPFNCTT.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPFNCTT.Location = New System.Drawing.Point(584, 27)
        Me.txtPFNCTT.MaxLength = 0
        Me.txtPFNCTT.Name = "txtPFNCTT"
		Me.txtPFNCTT.ReadOnly = True
        Me.txtPFNCTT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPFNCTT.Size = New System.Drawing.Size(153, 20)
        Me.txtPFNCTT.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(32, 12)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(185, 17)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Total Fees eligible for financing"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(312, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(217, 17)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Total Fees excluded from financing"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(584, 12)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(145, 17)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Total Policy Fees"
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me.uctPMURITax1)
        Me._SSTab1_TabPage2.Controls.Add(Me.fraTaxes)
        Me._SSTab1_TabPage2.Controls.Add(Me.fraRTaxes)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(784, 351)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "3 - Policy &Tax"
        '
        'uctPMURITax1
        '
        Me.uctPMURITax1.CurrencyId = 0
        Me.uctPMURITax1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMURITax1.Location = New System.Drawing.Point(8, 4)
        Me.uctPMURITax1.Name = "uctPMURITax1"
        Me.uctPMURITax1.Size = New System.Drawing.Size(771, 217)
        Me.uctPMURITax1.TabIndex = 0
        '
        'fraTaxes
        '
        Me.fraTaxes.BackColor = System.Drawing.SystemColors.Control
        Me.fraTaxes.Controls.Add(Me.txtPCT)
        Me.fraTaxes.Controls.Add(Me.txtPNCT)
        Me.fraTaxes.Controls.Add(Me.txtPTEXF)
        Me.fraTaxes.Controls.Add(Me.txtPTEFF)
        Me.fraTaxes.Controls.Add(Me.lblCT)
        Me.fraTaxes.Controls.Add(Me.lblNCT)
        Me.fraTaxes.Controls.Add(Me.lblEXF)
        Me.fraTaxes.Controls.Add(Me.lblPEFF)
        Me.fraTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTaxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTaxes.Location = New System.Drawing.Point(8, 284)
        Me.fraTaxes.Name = "fraTaxes"
        Me.fraTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTaxes.Size = New System.Drawing.Size(771, 57)
        Me.fraTaxes.TabIndex = 2
        Me.fraTaxes.TabStop = False
        Me.fraTaxes.Text = "Policy Taxes"
        '
        'txtPCT
        '
        Me.txtPCT.AcceptsReturn = True
        Me.txtPCT.BackColor = System.Drawing.SystemColors.Control
        Me.txtPCT.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPCT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPCT.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPCT.Location = New System.Drawing.Point(620, 29)
        Me.txtPCT.MaxLength = 0
        Me.txtPCT.Name = "txtPCT"
		Me.txtPCT.ReadOnly = True
        Me.txtPCT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPCT.Size = New System.Drawing.Size(137, 20)
        Me.txtPCT.TabIndex = 7
        '
        'txtPNCT
        '
        Me.txtPNCT.AcceptsReturn = True
        Me.txtPNCT.BackColor = System.Drawing.SystemColors.Control
        Me.txtPNCT.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPNCT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPNCT.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPNCT.Location = New System.Drawing.Point(444, 29)
        Me.txtPNCT.MaxLength = 0
        Me.txtPNCT.Name = "txtPNCT"
		 Me.txtPNCT.ReadOnly = True
        Me.txtPNCT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPNCT.Size = New System.Drawing.Size(153, 20)
        Me.txtPNCT.TabIndex = 5
        '
        'txtPTEXF
        '
        Me.txtPTEXF.AcceptsReturn = True
        Me.txtPTEXF.BackColor = System.Drawing.SystemColors.Control
        Me.txtPTEXF.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPTEXF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPTEXF.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPTEXF.Location = New System.Drawing.Point(220, 29)
        Me.txtPTEXF.MaxLength = 0
        Me.txtPTEXF.Name = "txtPTEXF"
		 Me.txtPTEXF.ReadOnly = True
        Me.txtPTEXF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPTEXF.Size = New System.Drawing.Size(177, 20)
        Me.txtPTEXF.TabIndex = 3
        '
        'txtPTEFF
        '
        Me.txtPTEFF.AcceptsReturn = True
        Me.txtPTEFF.BackColor = System.Drawing.SystemColors.Control
        Me.txtPTEFF.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPTEFF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPTEFF.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPTEFF.Location = New System.Drawing.Point(12, 29)
        Me.txtPTEFF.MaxLength = 0
        Me.txtPTEFF.Name = "txtPTEFF"
		Me.txtPTEFF.ReadOnly = True
        Me.txtPTEFF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPTEFF.Size = New System.Drawing.Size(169, 20)
        Me.txtPTEFF.TabIndex = 1
        '
        'lblCT
        '
        Me.lblCT.BackColor = System.Drawing.SystemColors.Control
        Me.lblCT.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCT.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCT.Location = New System.Drawing.Point(620, 14)
        Me.lblCT.Name = "lblCT"
        Me.lblCT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCT.Size = New System.Drawing.Size(145, 17)
        Me.lblCT.TabIndex = 6
        Me.lblCT.Text = "Total Policy Client Taxes"
        '
        'lblNCT
        '
        Me.lblNCT.BackColor = System.Drawing.SystemColors.Control
        Me.lblNCT.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNCT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNCT.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNCT.Location = New System.Drawing.Point(444, 14)
        Me.lblNCT.Name = "lblNCT"
        Me.lblNCT.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNCT.Size = New System.Drawing.Size(145, 17)
        Me.lblNCT.TabIndex = 4
        Me.lblNCT.Text = "Total Non Client Taxes"
        '
        'lblEXF
        '
        Me.lblEXF.BackColor = System.Drawing.SystemColors.Control
        Me.lblEXF.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEXF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEXF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEXF.Location = New System.Drawing.Point(220, 14)
        Me.lblEXF.Name = "lblEXF"
        Me.lblEXF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEXF.Size = New System.Drawing.Size(217, 17)
        Me.lblEXF.TabIndex = 2
        Me.lblEXF.Text = "Total Taxes excluded from financing"
        '
        'lblPEFF
        '
        Me.lblPEFF.BackColor = System.Drawing.SystemColors.Control
        Me.lblPEFF.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPEFF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPEFF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPEFF.Location = New System.Drawing.Point(12, 14)
        Me.lblPEFF.Name = "lblPEFF"
        Me.lblPEFF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPEFF.Size = New System.Drawing.Size(185, 17)
        Me.lblPEFF.TabIndex = 0
        Me.lblPEFF.Text = "Total Taxes eligible for financing"
        '
        'fraRTaxes
        '
        Me.fraRTaxes.BackColor = System.Drawing.SystemColors.Control
        Me.fraRTaxes.Controls.Add(Me.txtTotalRiskTax)
        Me.fraRTaxes.Controls.Add(Me.txtTotalNonTax)
        Me.fraRTaxes.Controls.Add(Me.txtTEF)
        Me.fraRTaxes.Controls.Add(Me.txtTEFF)
        Me.fraRTaxes.Controls.Add(Me.lblRiskCTax)
        Me.fraRTaxes.Controls.Add(Me.lblNonCTax)
        Me.fraRTaxes.Controls.Add(Me.lblTEF)
        Me.fraRTaxes.Controls.Add(Me.lblEFF)
        Me.fraRTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRTaxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRTaxes.Location = New System.Drawing.Point(8, 220)
        Me.fraRTaxes.Name = "fraRTaxes"
        Me.fraRTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRTaxes.Size = New System.Drawing.Size(771, 57)
        Me.fraRTaxes.TabIndex = 1
        Me.fraRTaxes.TabStop = False
        Me.fraRTaxes.Text = "Risk Taxes"
        '
        'txtTotalRiskTax
        '
        Me.txtTotalRiskTax.AcceptsReturn = True
        Me.txtTotalRiskTax.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalRiskTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalRiskTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalRiskTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalRiskTax.Location = New System.Drawing.Point(618, 29)
        Me.txtTotalRiskTax.MaxLength = 0
        Me.txtTotalRiskTax.Name = "txtTotalRiskTax"
		Me.txtTotalRiskTax.ReadOnly = True
        Me.txtTotalRiskTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalRiskTax.Size = New System.Drawing.Size(137, 20)
        Me.txtTotalRiskTax.TabIndex = 7
        '
        'txtTotalNonTax
        '
        Me.txtTotalNonTax.AcceptsReturn = True
        Me.txtTotalNonTax.BackColor = System.Drawing.SystemColors.Control
        Me.txtTotalNonTax.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalNonTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalNonTax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalNonTax.Location = New System.Drawing.Point(444, 29)
        Me.txtTotalNonTax.MaxLength = 0
        Me.txtTotalNonTax.Name = "txtTotalNonTax"
		Me.txtTotalNonTax.ReadOnly = True
        Me.txtTotalNonTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalNonTax.Size = New System.Drawing.Size(153, 20)
        Me.txtTotalNonTax.TabIndex = 5
        '
        'txtTEF
        '
        Me.txtTEF.AcceptsReturn = True
        Me.txtTEF.BackColor = System.Drawing.SystemColors.Control
        Me.txtTEF.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTEF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTEF.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTEF.Location = New System.Drawing.Point(220, 29)
        Me.txtTEF.MaxLength = 0
        Me.txtTEF.Name = "txtTEF"
		 Me.txtTEF.ReadOnly = True
        Me.txtTEF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTEF.Size = New System.Drawing.Size(177, 20)
        Me.txtTEF.TabIndex = 3
        '
        'txtTEFF
        '
        Me.txtTEFF.AcceptsReturn = True
        Me.txtTEFF.BackColor = System.Drawing.SystemColors.Control
        Me.txtTEFF.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTEFF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTEFF.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTEFF.Location = New System.Drawing.Point(12, 29)
        Me.txtTEFF.MaxLength = 0
        Me.txtTEFF.Name = "txtTEFF"
		Me.txtTEFF.ReadOnly = True
        Me.txtTEFF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTEFF.Size = New System.Drawing.Size(169, 20)
        Me.txtTEFF.TabIndex = 1
        '
        'lblRiskCTax
        '
        Me.lblRiskCTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskCTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskCTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskCTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskCTax.Location = New System.Drawing.Point(620, 14)
        Me.lblRiskCTax.Name = "lblRiskCTax"
        Me.lblRiskCTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskCTax.Size = New System.Drawing.Size(137, 17)
        Me.lblRiskCTax.TabIndex = 6
        Me.lblRiskCTax.Text = "Total Risk Client Taxes"
        '
        'lblNonCTax
        '
        Me.lblNonCTax.BackColor = System.Drawing.SystemColors.Control
        Me.lblNonCTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNonCTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNonCTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNonCTax.Location = New System.Drawing.Point(444, 14)
        Me.lblNonCTax.Name = "lblNonCTax"
        Me.lblNonCTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNonCTax.Size = New System.Drawing.Size(145, 17)
        Me.lblNonCTax.TabIndex = 4
        Me.lblNonCTax.Text = "Total Non Client Taxes"
        '
        'lblTEF
        '
        Me.lblTEF.BackColor = System.Drawing.SystemColors.Control
        Me.lblTEF.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTEF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTEF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTEF.Location = New System.Drawing.Point(220, 14)
        Me.lblTEF.Name = "lblTEF"
        Me.lblTEF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTEF.Size = New System.Drawing.Size(217, 17)
        Me.lblTEF.TabIndex = 2
        Me.lblTEF.Text = "Total Taxes excluded from financing"
        '
        'lblEFF
        '
        Me.lblEFF.BackColor = System.Drawing.SystemColors.Control
        Me.lblEFF.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEFF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEFF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEFF.Location = New System.Drawing.Point(12, 14)
        Me.lblEFF.Name = "lblEFF"
        Me.lblEFF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEFF.Size = New System.Drawing.Size(185, 17)
        Me.lblEFF.TabIndex = 0
        Me.lblEFF.Text = "Total Taxes eligible for financing"
        '
        '_SSTab1_TabPage3
        '
        Me._SSTab1_TabPage3.Controls.Add(Me.Commission1)
        Me._SSTab1_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage3.Name = "_SSTab1_TabPage3"
        Me._SSTab1_TabPage3.Size = New System.Drawing.Size(784, 351)
        Me._SSTab1_TabPage3.TabIndex = 3
        Me._SSTab1_TabPage3.Text = "4 - A&gent Commission"
        '
        'Commission1
        '
        Me.Commission1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Commission1.Location = New System.Drawing.Point(8, 4)
        Me.Commission1.Name = "Commission1"
        Me.Commission1.Size = New System.Drawing.Size(777, 345)
        Me.Commission1.TabIndex = 0
        '
        '_SSTab1_TabPage4
        '
        Me._SSTab1_TabPage4.Controls.Add(Me.uctInstalments1)
        Me._SSTab1_TabPage4.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage4.Name = "_SSTab1_TabPage4"
        Me._SSTab1_TabPage4.Size = New System.Drawing.Size(784, 351)
        Me._SSTab1_TabPage4.TabIndex = 4
        Me._SSTab1_TabPage4.Text = "5 - Instalments"
        '
        'uctInstalments1
        '
        Me.uctInstalments1.BaseCurrency = ""
        Me.uctInstalments1.BaseCurrencyID = 0
        Me.uctInstalments1.BaseISOCode = Nothing
        Me.uctInstalments1.FeeDeposit = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalments1.FeeExcluded = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalments1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctInstalments1.GrossDue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalments1.IsFinanceAmountNetPremium = False
        Me.uctInstalments1.IsPlanSelected = False
        Me.uctInstalments1.IsTrueMonthlypolicyandNextInstalmentRenewal = False
        Me.uctInstalments1.Location = New System.Drawing.Point(28, 4)
        Me.uctInstalments1.MTAType = 0
        Me.uctInstalments1.Name = "uctInstalments1"
        Me.uctInstalments1.PremiumFinanceCnt = 0
        Me.uctInstalments1.PremiumFinanceTransactions = Nothing
        Me.uctInstalments1.PremiumFinanceVersion = 0
        Me.uctInstalments1.Size = New System.Drawing.Size(731, 345)
        Me.uctInstalments1.TabIndex = 0
        Me.uctInstalments1.Task = 0
        Me.uctInstalments1.TaxDeposit = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalments1.TaxExcluded = New Decimal(New Integer() {0, 0, 0, 0})
        Me.uctInstalments1.TransactionType = ""
        Me.uctInstalments1.TransCurrencyID = 0
        Me.uctInstalments1.TransISOCode = Nothing
        '
        'txtPolicyHolderFull
        '
        Me.txtPolicyHolderFull.AcceptsReturn = True
        Me.txtPolicyHolderFull.BackColor = System.Drawing.SystemColors.Control
        Me.txtPolicyHolderFull.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyHolderFull.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyHolderFull.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyHolderFull.Location = New System.Drawing.Point(294, 8)
        Me.txtPolicyHolderFull.MaxLength = 0
        Me.txtPolicyHolderFull.Name = "txtPolicyHolderFull"
        Me.txtPolicyHolderFull.ReadOnly = True
        Me.txtPolicyHolderFull.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyHolderFull.Size = New System.Drawing.Size(500, 20)
        Me.txtPolicyHolderFull.TabIndex = 2
        Me.txtPolicyHolderFull.TabStop = False
        '
        'txtPolicyHolder
        '
        Me.txtPolicyHolder.AcceptsReturn = True
        Me.txtPolicyHolder.BackColor = System.Drawing.SystemColors.Control
        Me.txtPolicyHolder.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyHolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyHolder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyHolder.Location = New System.Drawing.Point(104, 8)
        Me.txtPolicyHolder.MaxLength = 0
        Me.txtPolicyHolder.Name = "txtPolicyHolder"
        Me.txtPolicyHolder.ReadOnly = True
        Me.txtPolicyHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyHolder.Size = New System.Drawing.Size(172, 20)
        Me.txtPolicyHolder.TabIndex = 1
        Me.txtPolicyHolder.TabStop = False
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.AutoSize = True
        Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpiryDate.Location = New System.Drawing.Point(552, 68)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpiryDate.Size = New System.Drawing.Size(64, 13)
        Me.lblExpiryDate.TabIndex = 11
        Me.lblExpiryDate.Text = "Expiry Date:"
        '
        'lblCoverFromDate
        '
        Me.lblCoverFromDate.AutoSize = True
        Me.lblCoverFromDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblCoverFromDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCoverFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCoverFromDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCoverFromDate.Location = New System.Drawing.Point(296, 68)
        Me.lblCoverFromDate.Name = "lblCoverFromDate"
        Me.lblCoverFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCoverFromDate.Size = New System.Drawing.Size(90, 13)
        Me.lblCoverFromDate.TabIndex = 9
        Me.lblCoverFromDate.Text = "Cover From Date:"
        '
        'lblInceptionDate
        '
        Me.lblInceptionDate.AutoSize = True
        Me.lblInceptionDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblInceptionDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInceptionDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInceptionDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInceptionDate.Location = New System.Drawing.Point(14, 68)
        Me.lblInceptionDate.Name = "lblInceptionDate"
        Me.lblInceptionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInceptionDate.Size = New System.Drawing.Size(80, 13)
        Me.lblInceptionDate.TabIndex = 7
        Me.lblInceptionDate.Text = "Inception Date:"
        '
        'lblPolicyRef
        '
        Me.lblPolicyRef.AutoSize = True
        Me.lblPolicyRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyRef.Location = New System.Drawing.Point(42, 40)
        Me.lblPolicyRef.Name = "lblPolicyRef"
        Me.lblPolicyRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyRef.Size = New System.Drawing.Size(58, 13)
        Me.lblPolicyRef.TabIndex = 3
        Me.lblPolicyRef.Text = "Policy Ref:"
        '
        'lblAgent
        '
        Me.lblAgent.AutoSize = True
        Me.lblAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgent.Location = New System.Drawing.Point(296, 40)
        Me.lblAgent.Name = "lblAgent"
        Me.lblAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgent.Size = New System.Drawing.Size(38, 13)
        Me.lblAgent.TabIndex = 5
        Me.lblAgent.Text = "Agent:"
        '
        'lblPolicyHolder
        '
        Me.lblPolicyHolder.AutoSize = True
        Me.lblPolicyHolder.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyHolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyHolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyHolder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyHolder.Location = New System.Drawing.Point(24, 12)
        Me.lblPolicyHolder.Name = "lblPolicyHolder"
        Me.lblPolicyHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyHolder.Size = New System.Drawing.Size(72, 13)
        Me.lblPolicyHolder.TabIndex = 0
        Me.lblPolicyHolder.Text = "Policy Holder:"
        '
        'Line1
        '
        Me.Line1.BackColor = System.Drawing.SystemColors.WindowText
        Me.Line1.Location = New System.Drawing.Point(16, 96)
        Me.Line1.Name = "Line1"
        Me.Line1.Size = New System.Drawing.Size(768, 1)
        Me.Line1.TabIndex = 14
        '
        'cmdWrite
        '
        Me.cmdWrite.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWrite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWrite.Location = New System.Drawing.Point(491, 664)
        Me.cmdWrite.Name = "cmdWrite"
        Me.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWrite.Size = New System.Drawing.Size(80, 25)
        Me.cmdWrite.TabIndex = 25
        Me.cmdWrite.Text = "Write"
        Me.cmdWrite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdWrite.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(804, 714)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.fraPaymentTerms)
        Me.Controls.Add(Me.cmdDocArchive)
        Me.Controls.Add(Me.cmdPrintDocument)
        Me.Controls.Add(Me.StatusBar)
        Me.Controls.Add(Me.cmdSaveQuote)
        Me.Controls.Add(Me.cmdMakeLive)
        Me.Controls.Add(Me.cmdRequote)
        Me.Controls.Add(Me.cmdPrintProposal)
        Me.Controls.Add(Me.cmdPrintQuote)
        Me.Controls.Add(Me.fraTotals)
        Me.Controls.Add(Me.txtExpiryDate)
        Me.Controls.Add(Me.txtInceptionDate)
        Me.Controls.Add(Me.txtCoverFromDate)
        Me.Controls.Add(Me.txtPolicyRef)
        Me.Controls.Add(Me.txtAgent)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.txtPolicyHolderFull)
        Me.Controls.Add(Me.txtPolicyHolder)
        Me.Controls.Add(Me.lblExpiryDate)
        Me.Controls.Add(Me.lblCoverFromDate)
        Me.Controls.Add(Me.lblInceptionDate)
        Me.Controls.Add(Me.lblPolicyRef)
        Me.Controls.Add(Me.lblAgent)
        Me.Controls.Add(Me.lblPolicyHolder)
        Me.Controls.Add(Me.Line1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Risks"
        Me.fraPaymentTerms.ResumeLayout(False)
        Me.fraPaymentTerms.PerformLayout()
        Me.StatusBar.ResumeLayout(False)
        Me.StatusBar.PerformLayout()
        Me.fraTotals.ResumeLayout(False)
        Me.fraTotals.PerformLayout()
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.frameListRisk.ResumeLayout(False)
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me.fraTaxes.ResumeLayout(False)
        Me.fraTaxes.PerformLayout()
        Me.fraRTaxes.ResumeLayout(False)
        Me.fraRTaxes.PerformLayout()
        Me._SSTab1_TabPage3.ResumeLayout(False)
        Me._SSTab1_TabPage4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cmdWrite As System.Windows.Forms.Button
    Public WithEvents cboCollectionFrequency As System.Windows.Forms.ComboBox
    Public WithEvents lblPaymentTerms As System.Windows.Forms.Label
    Public WithEvents cboPaymentTerms As System.Windows.Forms.ComboBox
    Public WithEvents lblCollectionFrequency As System.Windows.Forms.Label
    Public WithEvents btnNOChange As System.Windows.Forms.Button
    Public WithEvents btnNoChangeAll As System.Windows.Forms.Button
    Public WithEvents cmdQuoteAllRisks As System.Windows.Forms.Button
    Public WithEvents cmdReRateAllRisks As System.Windows.Forms.Button

#End Region
End Class