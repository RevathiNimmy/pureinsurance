<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUWGeneral
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializelblOption()
        InitializelblOption15()
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Public WithEvents Check3 As System.Windows.Forms.CheckBox
    Public WithEvents chkApplyBackDatedRiskEditingRestrictions As System.Windows.Forms.CheckBox
    Public WithEvents chkOption5076 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption5075 As System.Windows.Forms.CheckBox
    Public WithEvents chkRecalProrataRIMTA As System.Windows.Forms.CheckBox
    Public WithEvents chkActRIBroker As System.Windows.Forms.CheckBox
    Public WithEvents ChkCCTurnover As System.Windows.Forms.CheckBox
    Public WithEvents chkCoInsuranceLinkToAgent As System.Windows.Forms.CheckBox
    Public WithEvents ChkCCEmployees As System.Windows.Forms.CheckBox
    Public WithEvents cboOption5019 As System.Windows.Forms.ComboBox
    Public WithEvents chkOption5000 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption5005 As System.Windows.Forms.CheckBox
    Public WithEvents chkUWYearMandatory As System.Windows.Forms.CheckBox
    Public WithEvents chkOption5004 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption200 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption1026 As System.Windows.Forms.CheckBox
    Public WithEvents Check2 As System.Windows.Forms.CheckBox
    Public WithEvents Check1 As System.Windows.Forms.CheckBox
    Public WithEvents cboOption1003 As System.Windows.Forms.ComboBox
    Public WithEvents chkOption1007 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption1001 As System.Windows.Forms.CheckBox
    Public WithEvents chkOption1002 As System.Windows.Forms.CheckBox
    Public WithEvents cboOption1004 As System.Windows.Forms.ComboBox
    Public WithEvents cboOption1005 As System.Windows.Forms.ComboBox
    Public WithEvents cboOption1006 As System.Windows.Forms.ComboBox
    Public WithEvents txtOption1008 As System.Windows.Forms.TextBox
    Public WithEvents chkOption1023 As System.Windows.Forms.CheckBox
    Public WithEvents txtOption1009 As System.Windows.Forms.TextBox
    Public WithEvents chkOption1035 As System.Windows.Forms.CheckBox
    Public WithEvents cboOption1040 As System.Windows.Forms.ComboBox
    Private WithEvents _lblOption_5019 As System.Windows.Forms.Label
    Private WithEvents _lblOption_1003 As System.Windows.Forms.Label
    Private WithEvents _lblOption_1004 As System.Windows.Forms.Label
    Private WithEvents _lblOption_1005 As System.Windows.Forms.Label
    Private WithEvents _lblOption_1006 As System.Windows.Forms.Label
    Private WithEvents _lblOption_1008 As System.Windows.Forms.Label
    Private WithEvents _lblOption_1009 As System.Windows.Forms.Label
    Private WithEvents _lblOption_0 As System.Windows.Forms.Label
    Public lblOption15(4) As System.Windows.Forms.Label
    Public lblOption(5019) As System.Windows.Forms.Label
    Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkOption5098 = New System.Windows.Forms.CheckBox()
        Me.Check3 = New System.Windows.Forms.CheckBox()
        Me.chkApplyBackDatedRiskEditingRestrictions = New System.Windows.Forms.CheckBox()
        Me.chkOption5076 = New System.Windows.Forms.CheckBox()
        Me.chkOption5075 = New System.Windows.Forms.CheckBox()
        Me.chkRecalProrataRIMTA = New System.Windows.Forms.CheckBox()
        Me.chkActRIBroker = New System.Windows.Forms.CheckBox()
        Me.ChkCCTurnover = New System.Windows.Forms.CheckBox()
        Me.chkCoInsuranceLinkToAgent = New System.Windows.Forms.CheckBox()
        Me.ChkCCEmployees = New System.Windows.Forms.CheckBox()
        Me.cboOption5019 = New System.Windows.Forms.ComboBox()
        Me.chkOption5000 = New System.Windows.Forms.CheckBox()
        Me.chkOption5005 = New System.Windows.Forms.CheckBox()
        Me.chkUWYearMandatory = New System.Windows.Forms.CheckBox()
        Me.chkOption5004 = New System.Windows.Forms.CheckBox()
        Me.chkOption200 = New System.Windows.Forms.CheckBox()
        Me.chkOption1026 = New System.Windows.Forms.CheckBox()
        Me.Check2 = New System.Windows.Forms.CheckBox()
        Me.Check1 = New System.Windows.Forms.CheckBox()
        Me.cboOption1003 = New System.Windows.Forms.ComboBox()
        Me.chkOption1007 = New System.Windows.Forms.CheckBox()
        Me.chkOption1001 = New System.Windows.Forms.CheckBox()
        Me.chkOption1002 = New System.Windows.Forms.CheckBox()
        Me.cboOption1004 = New System.Windows.Forms.ComboBox()
        Me.cboOption1005 = New System.Windows.Forms.ComboBox()
        Me.cboOption1006 = New System.Windows.Forms.ComboBox()
        Me.txtOption1008 = New System.Windows.Forms.TextBox()
        Me.chkOption1023 = New System.Windows.Forms.CheckBox()
        Me.txtOption1009 = New System.Windows.Forms.TextBox()
        Me.chkOption1035 = New System.Windows.Forms.CheckBox()
        Me.cboOption1040 = New System.Windows.Forms.ComboBox()
        Me._lblOption_5019 = New System.Windows.Forms.Label()
        Me._lblOption_1003 = New System.Windows.Forms.Label()
        Me._lblOption_1004 = New System.Windows.Forms.Label()
        Me._lblOption_1005 = New System.Windows.Forms.Label()
        Me._lblOption_1006 = New System.Windows.Forms.Label()
        Me._lblOption_1008 = New System.Windows.Forms.Label()
        Me._lblOption_1009 = New System.Windows.Forms.Label()
        Me._lblOption_0 = New System.Windows.Forms.Label()
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.cboOption13 = New System.Windows.Forms.ComboBox()
        Me.chkQuoteVersioning5089 = New System.Windows.Forms.CheckBox()
        Me.txtOption5090 = New System.Windows.Forms.TextBox()
        Me._lblOption_5090 = New System.Windows.Forms.Label()
        Me._lblOption_5092 = New System.Windows.Forms.Label()
        Me.chkOption5096 = New System.Windows.Forms.CheckBox()
        Me.chkOption5153 = New System.Windows.Forms.CheckBox()
        Me.chkExtendedRILimits = New System.Windows.Forms.CheckBox()
        Me.chkDisableTempMTA = New System.Windows.Forms.CheckBox()
        Me.chkPostFeeTaxesSeparately = New System.Windows.Forms.CheckBox()
        Me._lblOption15_4 = New System.Windows.Forms.Label()
        Me.chkEnableDoNotMergeClause = New System.Windows.Forms.CheckBox()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkOption5098
        '
        Me.chkOption5098.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5098.Location = New System.Drawing.Point(304, 562)
        Me.chkOption5098.Name = "chkOption5098"
        Me.chkOption5098.Size = New System.Drawing.Size(265, 29)
        Me.chkOption5098.TabIndex = 43
        Me.chkOption5098.Tag = "5098"
        Me.chkOption5098.Text = "Restrict Client Portfolio Transfer on Active Instalment Plans"
        Me.ToolTip1.SetToolTip(Me.chkOption5098, "Option No 5098")
        Me.chkOption5098.UseVisualStyleBackColor = True
        '
        'Check3
        '
        Me.Check3.BackColor = System.Drawing.SystemColors.Control
        Me.Check3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Check3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Check3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Check3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Check3.Location = New System.Drawing.Point(304, 499)
        Me.Check3.Name = "Check3"
        Me.Check3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Check3.Size = New System.Drawing.Size(265, 23)
        Me.Check3.TabIndex = 37
        Me.Check3.Tag = "5081"
        Me.Check3.Text = "Override Agent Tax Group Allowed"
        Me.Check3.UseVisualStyleBackColor = False
        '
        'chkApplyBackDatedRiskEditingRestrictions
        '
        Me.chkApplyBackDatedRiskEditingRestrictions.BackColor = System.Drawing.SystemColors.Control
        Me.chkApplyBackDatedRiskEditingRestrictions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkApplyBackDatedRiskEditingRestrictions.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkApplyBackDatedRiskEditingRestrictions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkApplyBackDatedRiskEditingRestrictions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkApplyBackDatedRiskEditingRestrictions.Location = New System.Drawing.Point(10, 500)
        Me.chkApplyBackDatedRiskEditingRestrictions.Name = "chkApplyBackDatedRiskEditingRestrictions"
        Me.chkApplyBackDatedRiskEditingRestrictions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkApplyBackDatedRiskEditingRestrictions.Size = New System.Drawing.Size(263, 21)
        Me.chkApplyBackDatedRiskEditingRestrictions.TabIndex = 36
        Me.chkApplyBackDatedRiskEditingRestrictions.Tag = "5079,,M"
        Me.chkApplyBackDatedRiskEditingRestrictions.Text = "Apply Back-Dated Risk Editing Restrictions"
        Me.chkApplyBackDatedRiskEditingRestrictions.UseVisualStyleBackColor = False
        '
        'chkOption5076
        '
        Me.chkOption5076.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5076.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5076.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5076.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5076.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5076.Location = New System.Drawing.Point(304, 417)
        Me.chkOption5076.Name = "chkOption5076"
        Me.chkOption5076.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5076.Size = New System.Drawing.Size(265, 23)
        Me.chkOption5076.TabIndex = 35
        Me.chkOption5076.Tag = "5076"
        Me.chkOption5076.Text = "Cancel Instalment Plan on Policy Cancellation:"
        Me.chkOption5076.UseVisualStyleBackColor = False
        '
        'chkOption5075
        '
        Me.chkOption5075.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5075.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5075.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5075.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5075.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5075.Location = New System.Drawing.Point(304, 206)
        Me.chkOption5075.Name = "chkOption5075"
        Me.chkOption5075.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5075.Size = New System.Drawing.Size(265, 18)
        Me.chkOption5075.TabIndex = 34
        Me.chkOption5075.Tag = "5075"
        Me.chkOption5075.Text = "Put on next instalment renewal"
        Me.chkOption5075.UseVisualStyleBackColor = False
        '
        'chkRecalProrataRIMTA
        '
        Me.chkRecalProrataRIMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkRecalProrataRIMTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkRecalProrataRIMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRecalProrataRIMTA.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRecalProrataRIMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRecalProrataRIMTA.Location = New System.Drawing.Point(304, 388)
        Me.chkRecalProrataRIMTA.Name = "chkRecalProrataRIMTA"
        Me.chkRecalProrataRIMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRecalProrataRIMTA.Size = New System.Drawing.Size(265, 23)
        Me.chkRecalProrataRIMTA.TabIndex = 23
        Me.chkRecalProrataRIMTA.Tag = "5070"
        Me.chkRecalProrataRIMTA.Text = "Recalculate pro-rata reinsurance rates during MTA:"
        Me.chkRecalProrataRIMTA.UseVisualStyleBackColor = False
        '
        'chkActRIBroker
        '
        Me.chkActRIBroker.BackColor = System.Drawing.SystemColors.Control
        Me.chkActRIBroker.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkActRIBroker.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkActRIBroker.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkActRIBroker.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkActRIBroker.Location = New System.Drawing.Point(304, 365)
        Me.chkActRIBroker.Name = "chkActRIBroker"
        Me.chkActRIBroker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkActRIBroker.Size = New System.Drawing.Size(265, 17)
        Me.chkActRIBroker.TabIndex = 20
        Me.chkActRIBroker.Tag = "5029"
        Me.chkActRIBroker.Text = "Activate RI Broker Participants:"
        Me.chkActRIBroker.UseVisualStyleBackColor = False
        '
        'ChkCCTurnover
        '
        Me.ChkCCTurnover.BackColor = System.Drawing.SystemColors.Control
        Me.ChkCCTurnover.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ChkCCTurnover.Cursor = System.Windows.Forms.Cursors.Default
        Me.ChkCCTurnover.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkCCTurnover.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ChkCCTurnover.Location = New System.Drawing.Point(304, 316)
        Me.ChkCCTurnover.Name = "ChkCCTurnover"
        Me.ChkCCTurnover.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ChkCCTurnover.Size = New System.Drawing.Size(265, 23)
        Me.ChkCCTurnover.TabIndex = 15
        Me.ChkCCTurnover.Tag = "5025"
        Me.ChkCCTurnover.Text = "Corporate Client Turnover Band Mandatory:"
        Me.ChkCCTurnover.UseVisualStyleBackColor = False
        '
        'chkCoInsuranceLinkToAgent
        '
        Me.chkCoInsuranceLinkToAgent.BackColor = System.Drawing.SystemColors.Control
        Me.chkCoInsuranceLinkToAgent.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCoInsuranceLinkToAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCoInsuranceLinkToAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCoInsuranceLinkToAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCoInsuranceLinkToAgent.Location = New System.Drawing.Point(304, 340)
        Me.chkCoInsuranceLinkToAgent.Name = "chkCoInsuranceLinkToAgent"
        Me.chkCoInsuranceLinkToAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCoInsuranceLinkToAgent.Size = New System.Drawing.Size(265, 23)
        Me.chkCoInsuranceLinkToAgent.TabIndex = 18
        Me.chkCoInsuranceLinkToAgent.Tag = "5026"
        Me.chkCoInsuranceLinkToAgent.Text = "Co Insurance Link to Agent on Per Policy Basis:"
        Me.chkCoInsuranceLinkToAgent.UseVisualStyleBackColor = False
        '
        'ChkCCEmployees
        '
        Me.ChkCCEmployees.BackColor = System.Drawing.SystemColors.Control
        Me.ChkCCEmployees.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ChkCCEmployees.Cursor = System.Windows.Forms.Cursors.Default
        Me.ChkCCEmployees.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkCCEmployees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ChkCCEmployees.Location = New System.Drawing.Point(304, 295)
        Me.ChkCCEmployees.Name = "ChkCCEmployees"
        Me.ChkCCEmployees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ChkCCEmployees.Size = New System.Drawing.Size(265, 23)
        Me.ChkCCEmployees.TabIndex = 13
        Me.ChkCCEmployees.Tag = "5024"
        Me.ChkCCEmployees.Text = "Corporate Client No. of Employees Mandatory:"
        Me.ChkCCEmployees.UseVisualStyleBackColor = False
        '
        'cboOption5019
        '
        Me.cboOption5019.AccessibleDescription = "Tax Effective Date:"
        Me.cboOption5019.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption5019.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption5019.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption5019.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboOption5019, New Integer(-1) {})
        Me.cboOption5019.Location = New System.Drawing.Point(260, 473)
        Me.cboOption5019.Name = "cboOption5019"
        Me.cboOption5019.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption5019.Size = New System.Drawing.Size(241, 21)
        Me.cboOption5019.TabIndex = 25
        Me.cboOption5019.Tag = "5019,,M"
        Me.cboOption5019.Text = "cboOption5019"
        '
        'chkOption5000
        '
        Me.chkOption5000.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5000.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5000.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5000.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5000.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5000.Location = New System.Drawing.Point(8, 275)
        Me.chkOption5000.Name = "chkOption5000"
        Me.chkOption5000.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5000.Size = New System.Drawing.Size(265, 21)
        Me.chkOption5000.TabIndex = 10
        Me.chkOption5000.Tag = "5000"
        Me.chkOption5000.Text = "Enable editing in Client Manager:"
        Me.chkOption5000.UseVisualStyleBackColor = False
        '
        'chkOption5005
        '
        Me.chkOption5005.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5005.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5005.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5005.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5005.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5005.Location = New System.Drawing.Point(304, 275)
        Me.chkOption5005.Name = "chkOption5005"
        Me.chkOption5005.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5005.Size = New System.Drawing.Size(265, 21)
        Me.chkOption5005.TabIndex = 11
        Me.chkOption5005.Tag = "5005"
        Me.chkOption5005.Text = "Underwriting Agency:"
        Me.chkOption5005.UseVisualStyleBackColor = False
        '
        'chkUWYearMandatory
        '
        Me.chkUWYearMandatory.BackColor = System.Drawing.SystemColors.Control
        Me.chkUWYearMandatory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkUWYearMandatory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkUWYearMandatory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUWYearMandatory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUWYearMandatory.Location = New System.Drawing.Point(304, 440)
        Me.chkUWYearMandatory.Name = "chkUWYearMandatory"
        Me.chkUWYearMandatory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkUWYearMandatory.Size = New System.Drawing.Size(265, 23)
        Me.chkUWYearMandatory.TabIndex = 24
        Me.chkUWYearMandatory.Tag = "5012, M"
        Me.chkUWYearMandatory.Text = "UW Year mandatory on cash and journals"
        Me.chkUWYearMandatory.UseVisualStyleBackColor = False
        '
        'chkOption5004
        '
        Me.chkOption5004.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5004.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5004.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5004.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5004.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5004.Location = New System.Drawing.Point(8, 440)
        Me.chkOption5004.Name = "chkOption5004"
        Me.chkOption5004.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5004.Size = New System.Drawing.Size(265, 23)
        Me.chkOption5004.TabIndex = 22
        Me.chkOption5004.Tag = "5004"
        Me.chkOption5004.Text = "Policy Discount:"
        Me.chkOption5004.UseVisualStyleBackColor = False
        '
        'chkOption200
        '
        Me.chkOption200.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption200.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption200.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption200.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption200.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption200.Location = New System.Drawing.Point(8, 312)
        Me.chkOption200.Name = "chkOption200"
        Me.chkOption200.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption200.Size = New System.Drawing.Size(265, 30)
        Me.chkOption200.TabIndex = 14
        Me.chkOption200.Tag = "200"
        Me.chkOption200.Text = "System currency is display only:"
        Me.chkOption200.UseVisualStyleBackColor = False
        '
        'chkOption1026
        '
        Me.chkOption1026.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1026.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1026.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1026.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1026.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1026.Location = New System.Drawing.Point(8, 363)
        Me.chkOption1026.Name = "chkOption1026"
        Me.chkOption1026.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1026.Size = New System.Drawing.Size(265, 21)
        Me.chkOption1026.TabIndex = 17
        Me.chkOption1026.Tag = "1026"
        Me.chkOption1026.Text = "Do not default MTA date:"
        Me.chkOption1026.UseVisualStyleBackColor = False
        '
        'Check2
        '
        Me.Check2.BackColor = System.Drawing.SystemColors.Control
        Me.Check2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Check2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Check2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Check2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Check2.Location = New System.Drawing.Point(8, 339)
        Me.Check2.Name = "Check2"
        Me.Check2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Check2.Size = New System.Drawing.Size(265, 24)
        Me.Check2.TabIndex = 16
        Me.Check2.Tag = "157"
        Me.Check2.Text = "Display multi-currency transaction screen:"
        Me.Check2.UseVisualStyleBackColor = False
        '
        'Check1
        '
        Me.Check1.BackColor = System.Drawing.SystemColors.Control
        Me.Check1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Check1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Check1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Check1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Check1.Location = New System.Drawing.Point(8, 295)
        Me.Check1.Name = "Check1"
        Me.Check1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Check1.Size = New System.Drawing.Size(265, 23)
        Me.Check1.TabIndex = 12
        Me.Check1.Tag = "156"
        Me.Check1.Text = "Display transaction exchange rate screen:"
        Me.Check1.UseVisualStyleBackColor = False
        '
        'cboOption1003
        '
        Me.cboOption1003.AccessibleDescription = "Country:"
        Me.cboOption1003.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption1003.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption1003.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption1003.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption1003.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboOption1003, New Integer() {0})
        Me.cboOption1003.Items.AddRange(New Object() {""})
        Me.cboOption1003.Location = New System.Drawing.Point(260, 52)
        Me.cboOption1003.Name = "cboOption1003"
        Me.cboOption1003.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption1003.Size = New System.Drawing.Size(241, 21)
        Me.cboOption1003.TabIndex = 2
        Me.cboOption1003.Tag = "1003,,M"
        '
        'chkOption1007
        '
        Me.chkOption1007.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1007.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1007.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1007.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1007.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1007.Location = New System.Drawing.Point(8, 205)
        Me.chkOption1007.Name = "chkOption1007"
        Me.chkOption1007.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1007.Size = New System.Drawing.Size(265, 17)
        Me.chkOption1007.TabIndex = 7
        Me.chkOption1007.Tag = "1007"
        Me.chkOption1007.Text = "When Taxes Required:"
        Me.chkOption1007.UseVisualStyleBackColor = False
        '
        'chkOption1001
        '
        Me.chkOption1001.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1001.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1001.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1001.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1001.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1001.Location = New System.Drawing.Point(8, 8)
        Me.chkOption1001.Name = "chkOption1001"
        Me.chkOption1001.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1001.Size = New System.Drawing.Size(265, 15)
        Me.chkOption1001.TabIndex = 0
        Me.chkOption1001.Tag = "1001"
        Me.chkOption1001.Text = "When Public Text Required:"
        Me.chkOption1001.UseVisualStyleBackColor = False
        '
        'chkOption1002
        '
        Me.chkOption1002.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1002.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1002.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1002.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1002.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1002.Location = New System.Drawing.Point(8, 30)
        Me.chkOption1002.Name = "chkOption1002"
        Me.chkOption1002.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1002.Size = New System.Drawing.Size(265, 15)
        Me.chkOption1002.TabIndex = 1
        Me.chkOption1002.Tag = "1002"
        Me.chkOption1002.Text = "When Private Text Required:"
        Me.chkOption1002.UseVisualStyleBackColor = False
        '
        'cboOption1004
        '
        Me.cboOption1004.AccessibleDescription = "Agent Statistics Basis:"
        Me.cboOption1004.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption1004.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption1004.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption1004.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption1004.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboOption1004, New Integer() {0, 0, 0, 0, 0, 0, 0})
        Me.cboOption1004.Items.AddRange(New Object() {"", "", "", "", "", "", ""})
        Me.cboOption1004.Location = New System.Drawing.Point(260, 77)
        Me.cboOption1004.Name = "cboOption1004"
        Me.cboOption1004.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption1004.Size = New System.Drawing.Size(241, 21)
        Me.cboOption1004.TabIndex = 3
        Me.cboOption1004.Tag = "1004,,M"
        '
        'cboOption1005
        '
        Me.cboOption1005.AccessibleDescription = "Revenue Statistics Basis:"
        Me.cboOption1005.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption1005.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption1005.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption1005.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption1005.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboOption1005, New Integer(-1) {})
        Me.cboOption1005.Location = New System.Drawing.Point(260, 102)
        Me.cboOption1005.Name = "cboOption1005"
        Me.cboOption1005.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption1005.Size = New System.Drawing.Size(241, 21)
        Me.cboOption1005.TabIndex = 4
        Me.cboOption1005.Tag = "1005,,M"
        '
        'cboOption1006
        '
        Me.cboOption1006.AccessibleDescription = "Branch Statistics Basis:"
        Me.cboOption1006.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption1006.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption1006.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption1006.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption1006.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboOption1006, New Integer(-1) {})
        Me.cboOption1006.Location = New System.Drawing.Point(260, 127)
        Me.cboOption1006.Name = "cboOption1006"
        Me.cboOption1006.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption1006.Size = New System.Drawing.Size(241, 21)
        Me.cboOption1006.TabIndex = 5
        Me.cboOption1006.Tag = "1006,,M"
        '
        'txtOption1008
        '
        Me.txtOption1008.AcceptsReturn = True
        Me.txtOption1008.AccessibleDescription = "Months in future allowed for Cover From Date:"
        Me.txtOption1008.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption1008.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption1008.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption1008.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption1008.Location = New System.Drawing.Point(290, 227)
        Me.txtOption1008.MaxLength = 2
        Me.txtOption1008.Name = "txtOption1008"
        Me.txtOption1008.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption1008.Size = New System.Drawing.Size(33, 21)
        Me.txtOption1008.TabIndex = 8
        Me.txtOption1008.Tag = "1008,ValidateNumeric"
        '
        'chkOption1023
        '
        Me.chkOption1023.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1023.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1023.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1023.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1023.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1023.Location = New System.Drawing.Point(8, 390)
        Me.chkOption1023.Name = "chkOption1023"
        Me.chkOption1023.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1023.Size = New System.Drawing.Size(265, 23)
        Me.chkOption1023.TabIndex = 19
        Me.chkOption1023.Tag = "1023"
        Me.chkOption1023.Text = "Use MTA date for Proportional Treaty calculation:"
        Me.chkOption1023.UseVisualStyleBackColor = False
        '
        'txtOption1009
        '
        Me.txtOption1009.AcceptsReturn = True
        Me.txtOption1009.AccessibleDescription = "Months in future allowed for Cover To Date:"
        Me.txtOption1009.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption1009.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption1009.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption1009.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption1009.Location = New System.Drawing.Point(290, 250)
        Me.txtOption1009.MaxLength = 2
        Me.txtOption1009.Name = "txtOption1009"
        Me.txtOption1009.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption1009.Size = New System.Drawing.Size(33, 21)
        Me.txtOption1009.TabIndex = 9
        Me.txtOption1009.Tag = "1009,ValidateNumeric"
        '
        'chkOption1035
        '
        Me.chkOption1035.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption1035.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption1035.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption1035.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption1035.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption1035.Location = New System.Drawing.Point(8, 419)
        Me.chkOption1035.Name = "chkOption1035"
        Me.chkOption1035.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption1035.Size = New System.Drawing.Size(265, 15)
        Me.chkOption1035.TabIndex = 21
        Me.chkOption1035.Tag = "1035"
        Me.chkOption1035.Text = "Associated client check at New Business:"
        Me.chkOption1035.UseVisualStyleBackColor = False
        '
        'cboOption1040
        '
        Me.cboOption1040.AccessibleDescription = "Validate cancelled agent/broker:"
        Me.cboOption1040.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption1040.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption1040.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption1040.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption1040.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboOption1040, New Integer(-1) {})
        Me.cboOption1040.Location = New System.Drawing.Point(260, 152)
        Me.cboOption1040.Name = "cboOption1040"
        Me.cboOption1040.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption1040.Size = New System.Drawing.Size(241, 21)
        Me.cboOption1040.TabIndex = 6
        Me.cboOption1040.Tag = "1040,,M"
        '
        '_lblOption_5019
        '
        Me._lblOption_5019.AutoSize = True
        Me._lblOption_5019.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_5019.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_5019.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_5019.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_5019.Location = New System.Drawing.Point(10, 476)
        Me._lblOption_5019.Name = "_lblOption_5019"
        Me._lblOption_5019.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_5019.Size = New System.Drawing.Size(116, 13)
        Me._lblOption_5019.TabIndex = 33
        Me._lblOption_5019.Tag = "1006,,M"
        Me._lblOption_5019.Text = "Tax Effective Date:"
        '
        '_lblOption_1003
        '
        Me._lblOption_1003.AutoSize = True
        Me._lblOption_1003.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_1003.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_1003.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_1003.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_1003.Location = New System.Drawing.Point(10, 56)
        Me._lblOption_1003.Name = "_lblOption_1003"
        Me._lblOption_1003.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_1003.Size = New System.Drawing.Size(58, 13)
        Me._lblOption_1003.TabIndex = 26
        Me._lblOption_1003.Tag = "1003,,M"
        Me._lblOption_1003.Text = "Country:"
        '
        '_lblOption_1004
        '
        Me._lblOption_1004.AutoSize = True
        Me._lblOption_1004.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_1004.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_1004.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_1004.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_1004.Location = New System.Drawing.Point(10, 80)
        Me._lblOption_1004.Name = "_lblOption_1004"
        Me._lblOption_1004.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_1004.Size = New System.Drawing.Size(134, 13)
        Me._lblOption_1004.TabIndex = 27
        Me._lblOption_1004.Tag = "1004,,M"
        Me._lblOption_1004.Text = "Agent Statistics Basis:"
        '
        '_lblOption_1005
        '
        Me._lblOption_1005.AutoSize = True
        Me._lblOption_1005.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_1005.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_1005.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_1005.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_1005.Location = New System.Drawing.Point(10, 106)
        Me._lblOption_1005.Name = "_lblOption_1005"
        Me._lblOption_1005.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_1005.Size = New System.Drawing.Size(151, 13)
        Me._lblOption_1005.TabIndex = 28
        Me._lblOption_1005.Tag = "1005,,M"
        Me._lblOption_1005.Text = "Revenue Statistics Basis:"
        '
        '_lblOption_1006
        '
        Me._lblOption_1006.AutoSize = True
        Me._lblOption_1006.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_1006.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_1006.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_1006.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_1006.Location = New System.Drawing.Point(10, 130)
        Me._lblOption_1006.Name = "_lblOption_1006"
        Me._lblOption_1006.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_1006.Size = New System.Drawing.Size(141, 13)
        Me._lblOption_1006.TabIndex = 29
        Me._lblOption_1006.Tag = "1006,,M"
        Me._lblOption_1006.Text = "Branch Statistics Basis:"
        '
        '_lblOption_1008
        '
        Me._lblOption_1008.AutoSize = True
        Me._lblOption_1008.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_1008.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_1008.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_1008.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_1008.Location = New System.Drawing.Point(10, 227)
        Me._lblOption_1008.Name = "_lblOption_1008"
        Me._lblOption_1008.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_1008.Size = New System.Drawing.Size(274, 13)
        Me._lblOption_1008.TabIndex = 31
        Me._lblOption_1008.Text = "Months in future allowed for Cover From Date:"
        '
        '_lblOption_1009
        '
        Me._lblOption_1009.AutoSize = True
        Me._lblOption_1009.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_1009.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_1009.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_1009.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_1009.Location = New System.Drawing.Point(10, 253)
        Me._lblOption_1009.Name = "_lblOption_1009"
        Me._lblOption_1009.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_1009.Size = New System.Drawing.Size(258, 13)
        Me._lblOption_1009.TabIndex = 32
        Me._lblOption_1009.Text = "Months in future allowed for Cover To Date:"
        '
        '_lblOption_0
        '
        Me._lblOption_0.AutoSize = True
        Me._lblOption_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_0.Location = New System.Drawing.Point(10, 156)
        Me._lblOption_0.Name = "_lblOption_0"
        Me._lblOption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_0.Size = New System.Drawing.Size(193, 13)
        Me._lblOption_0.TabIndex = 30
        Me._lblOption_0.Tag = "1006,,M"
        Me._lblOption_0.Text = "Validate cancelled agent/broker:"
        '
        'cboOption13
        '
        Me.cboOption13.AccessibleDescription = "Address Lookup Installation:"
        Me.cboOption13.BackColor = System.Drawing.SystemColors.Window
        Me.cboOption13.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOption13.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOption13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOption13.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboOption13, New Integer(-1) {})
        Me.cboOption13.Location = New System.Drawing.Point(260, 178)
        Me.cboOption13.Name = "cboOption13"
        Me.cboOption13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOption13.Size = New System.Drawing.Size(241, 21)
        Me.cboOption13.TabIndex = 47
        Me.cboOption13.Tag = "13,Locate QAS,M"
        '
        'chkQuoteVersioning5089
        '
        Me.chkQuoteVersioning5089.BackColor = System.Drawing.SystemColors.Control
        Me.chkQuoteVersioning5089.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkQuoteVersioning5089.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkQuoteVersioning5089.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkQuoteVersioning5089.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkQuoteVersioning5089.Location = New System.Drawing.Point(329, 230)
        Me.chkQuoteVersioning5089.Name = "chkQuoteVersioning5089"
        Me.chkQuoteVersioning5089.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkQuoteVersioning5089.Size = New System.Drawing.Size(240, 21)
        Me.chkQuoteVersioning5089.TabIndex = 38
        Me.chkQuoteVersioning5089.Tag = "5089"
        Me.chkQuoteVersioning5089.Text = "Quote Versioning:"
        Me.chkQuoteVersioning5089.UseVisualStyleBackColor = False
        '
        'txtOption5090
        '
        Me.txtOption5090.AcceptsReturn = True
        Me.txtOption5090.AccessibleDescription = "Delete Quote Versions After Days"
        Me.txtOption5090.BackColor = System.Drawing.SystemColors.Window
        Me.txtOption5090.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOption5090.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOption5090.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOption5090.Location = New System.Drawing.Point(502, 250)
        Me.txtOption5090.MaxLength = 2
        Me.txtOption5090.Name = "txtOption5090"
        Me.txtOption5090.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOption5090.Size = New System.Drawing.Size(33, 21)
        Me.txtOption5090.TabIndex = 40
        Me.txtOption5090.Tag = "5090,ValidateNumeric"
        '
        '_lblOption_5090
        '
        Me._lblOption_5090.AutoSize = True
        Me._lblOption_5090.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_5090.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_5090.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_5090.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_5090.Location = New System.Drawing.Point(329, 254)
        Me._lblOption_5090.Name = "_lblOption_5090"
        Me._lblOption_5090.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_5090.Size = New System.Drawing.Size(166, 13)
        Me._lblOption_5090.TabIndex = 41
        Me._lblOption_5090.Tag = "5090"
        Me._lblOption_5090.Text = "Delete Quote Versions After"
        '
        '_lblOption_5092
        '
        Me._lblOption_5092.AutoSize = True
        Me._lblOption_5092.Location = New System.Drawing.Point(542, 252)
        Me._lblOption_5092.Name = "_lblOption_5092"
        Me._lblOption_5092.Size = New System.Drawing.Size(31, 13)
        Me._lblOption_5092.TabIndex = 42
        Me._lblOption_5092.Tag = "5092"
        Me._lblOption_5092.Text = "Days"
        '
        'chkOption5096
        '
        Me.chkOption5096.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5096.Location = New System.Drawing.Point(304, 522)
        Me.chkOption5096.Name = "chkOption5096"
        Me.chkOption5096.Size = New System.Drawing.Size(263, 23)
        Me.chkOption5096.TabIndex = 43
        Me.chkOption5096.Tag = "5096"
        Me.chkOption5096.Text = "Chase Cycle Enabled"
        Me.chkOption5096.UseVisualStyleBackColor = True
        '
        'chkOption5153
        '
        Me.chkOption5153.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5153.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5153.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5153.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5153.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5153.Location = New System.Drawing.Point(10, 500)
        Me.chkOption5153.Name = "chkOption5153"
        Me.chkOption5153.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5153.Size = New System.Drawing.Size(263, 21)
        Me.chkOption5153.TabIndex = 45
        Me.chkOption5153.Tag = "5153"
        Me.chkOption5153.Text = "Copy Policy To Quote"
        Me.chkOption5153.UseVisualStyleBackColor = False
        '
        'chkExtendedRILimits
        '
        Me.chkExtendedRILimits.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkExtendedRILimits.Location = New System.Drawing.Point(308, 8)
        Me.chkExtendedRILimits.Name = "chkExtendedRILimits"
        Me.chkExtendedRILimits.Size = New System.Drawing.Size(265, 23)
        Me.chkExtendedRILimits.TabIndex = 44
        Me.chkExtendedRILimits.Tag = "5260,M"
        Me.chkExtendedRILimits.Text = "Extended RI Limits"
        Me.chkExtendedRILimits.UseVisualStyleBackColor = True
        '
        'chkDisableTempMTA
        '
        Me.chkDisableTempMTA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDisableTempMTA.Location = New System.Drawing.Point(304, 541)
        Me.chkDisableTempMTA.Name = "chkDisableTempMTA"
        Me.chkDisableTempMTA.Size = New System.Drawing.Size(265, 24)
        Me.chkDisableTempMTA.TabIndex = 45
        Me.chkDisableTempMTA.Tag = "5116"
        Me.chkDisableTempMTA.Text = "Disable Temporary MTA"
        Me.chkDisableTempMTA.UseVisualStyleBackColor = True
        '
        'chkPostFeeTaxesSeparately
        '
        Me.chkPostFeeTaxesSeparately.AccessibleDescription = "Post Fee/Taxes Separately"
        Me.chkPostFeeTaxesSeparately.BackColor = System.Drawing.SystemColors.Control
        Me.chkPostFeeTaxesSeparately.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPostFeeTaxesSeparately.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPostFeeTaxesSeparately.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPostFeeTaxesSeparately.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPostFeeTaxesSeparately.Location = New System.Drawing.Point(10, 527)
        Me.chkPostFeeTaxesSeparately.Name = "chkPostFeeTaxesSeparately"
        Me.chkPostFeeTaxesSeparately.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPostFeeTaxesSeparately.Size = New System.Drawing.Size(263, 21)
        Me.chkPostFeeTaxesSeparately.TabIndex = 46
        Me.chkPostFeeTaxesSeparately.Tag = "5118,,M"
        Me.chkPostFeeTaxesSeparately.Text = "Post Fee/Taxes Separately"
        Me.chkPostFeeTaxesSeparately.UseVisualStyleBackColor = False
        '
        '_lblOption15_4
        '
        Me._lblOption15_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption15_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption15_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption15_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption15_4.Location = New System.Drawing.Point(10, 183)
        Me._lblOption15_4.Name = "_lblOption15_4"
        Me._lblOption15_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption15_4.Size = New System.Drawing.Size(211, 18)
        Me._lblOption15_4.TabIndex = 48
        Me._lblOption15_4.Tag = "13,,M"
        Me._lblOption15_4.Text = "Address Lookup Installation:"
        '
        'chkEnableDoNotMergeClause
        '
        Me.chkEnableDoNotMergeClause.BackColor = System.Drawing.SystemColors.Control
        Me.chkEnableDoNotMergeClause.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkEnableDoNotMergeClause.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkEnableDoNotMergeClause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEnableDoNotMergeClause.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEnableDoNotMergeClause.Location = New System.Drawing.Point(10, 545)
        Me.chkEnableDoNotMergeClause.Name = "chkEnableDoNotMergeClause"
        Me.chkEnableDoNotMergeClause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkEnableDoNotMergeClause.Size = New System.Drawing.Size(263, 23)
        Me.chkEnableDoNotMergeClause.TabIndex = 43
        Me.chkEnableDoNotMergeClause.Tag = "5206"
        Me.chkEnableDoNotMergeClause.Text = "Enable Do Not Merge Clause"
        Me.chkEnableDoNotMergeClause.UseVisualStyleBackColor = False
        '
        'frmUWGeneral
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(596, 577)
        Me.Controls.Add(Me.chkOption5153)
        Me.Controls.Add(Me.chkEnableDoNotMergeClause)
        Me.Controls.Add(Me.chkOption5098)
        Me.Controls.Add(Me.cboOption13)
        Me.Controls.Add(Me._lblOption15_4)
        Me.Controls.Add(Me.chkPostFeeTaxesSeparately)
        Me.Controls.Add(Me.chkDisableTempMTA)
        Me.Controls.Add(Me.chkExtendedRILimits)
        Me.Controls.Add(Me.chkOption5096)
        Me.Controls.Add(Me._lblOption_5092)
        Me.Controls.Add(Me._lblOption_5090)
        Me.Controls.Add(Me.txtOption5090)
        Me.Controls.Add(Me.chkQuoteVersioning5089)
        Me.Controls.Add(Me.Check3)
        Me.Controls.Add(Me.chkApplyBackDatedRiskEditingRestrictions)
        Me.Controls.Add(Me.chkOption5076)
        Me.Controls.Add(Me.chkOption5075)
        Me.Controls.Add(Me.chkRecalProrataRIMTA)
        Me.Controls.Add(Me.chkActRIBroker)
        Me.Controls.Add(Me.ChkCCTurnover)
        Me.Controls.Add(Me.chkCoInsuranceLinkToAgent)
        Me.Controls.Add(Me.ChkCCEmployees)
        Me.Controls.Add(Me.cboOption5019)
        Me.Controls.Add(Me.chkOption5000)
        Me.Controls.Add(Me.chkOption5005)
        Me.Controls.Add(Me.chkUWYearMandatory)
        Me.Controls.Add(Me.chkOption5004)
        Me.Controls.Add(Me.chkOption200)
        Me.Controls.Add(Me.chkOption1026)
        Me.Controls.Add(Me.Check2)
        Me.Controls.Add(Me.Check1)
        Me.Controls.Add(Me.cboOption1003)
        Me.Controls.Add(Me.chkOption1007)
        Me.Controls.Add(Me.chkOption1001)
        Me.Controls.Add(Me.chkOption1002)
        Me.Controls.Add(Me.cboOption1004)
        Me.Controls.Add(Me.cboOption1005)
        Me.Controls.Add(Me.cboOption1006)
        Me.Controls.Add(Me.txtOption1008)
        Me.Controls.Add(Me.chkOption1023)
        Me.Controls.Add(Me.txtOption1009)
        Me.Controls.Add(Me.chkOption1035)
        Me.Controls.Add(Me.cboOption1040)
        Me.Controls.Add(Me._lblOption_5019)
        Me.Controls.Add(Me._lblOption_1003)
        Me.Controls.Add(Me._lblOption_1004)
        Me.Controls.Add(Me._lblOption_1005)
        Me.Controls.Add(Me._lblOption_1006)
        Me.Controls.Add(Me._lblOption_1008)
        Me.Controls.Add(Me._lblOption_1009)
        Me.Controls.Add(Me._lblOption_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmUWGeneral"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Tag = "5012, M"
        Me.Text = "Form1"
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializelblOption()
        Me.lblOption(5019) = _lblOption_5019
        Me.lblOption(1003) = _lblOption_1003
        Me.lblOption(1004) = _lblOption_1004
        Me.lblOption(1005) = _lblOption_1005
        Me.lblOption(1006) = _lblOption_1006
        Me.lblOption(1008) = _lblOption_1008
        Me.lblOption(1009) = _lblOption_1009
        Me.lblOption(0) = _lblOption_0
    End Sub
    Sub InitializelblOption15()
        Me.lblOption15(4) = _lblOption15_4
    End Sub
    Public WithEvents chkQuoteVersioning5089 As System.Windows.Forms.CheckBox
    Public WithEvents txtOption5090 As System.Windows.Forms.TextBox
    Private WithEvents _lblOption_5090 As System.Windows.Forms.Label
    Friend WithEvents _lblOption_5092 As System.Windows.Forms.Label
    Friend WithEvents chkOption5096 As System.Windows.Forms.CheckBox
    Friend WithEvents chkExtendedRILimits As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisableTempMTA As System.Windows.Forms.CheckBox
    Public WithEvents chkPostFeeTaxesSeparately As System.Windows.Forms.CheckBox
    Public WithEvents cboOption13 As System.Windows.Forms.ComboBox
    Private WithEvents _lblOption15_4 As System.Windows.Forms.Label
    Friend WithEvents chkOption5098 As System.Windows.Forms.CheckBox
    Public WithEvents chkEnableDoNotMergeClause As System.Windows.Forms.CheckBox
    Public WithEvents chkOption5153 As System.Windows.Forms.CheckBox
#End Region
End Class