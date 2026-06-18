<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializeoptTaxChargedTo()
        InitializeoptProtectionType()
        InitializeoptProtectionChargedTo()
        InitializeoptIncluded()
        InitializeoptFeeType()
        InitializeoptFeeChargedTo()
        InitializeoptDepositType()
        InitializeoptBackDated()
        InitializeoptAlign()
        InitializecmdPrevious()
        InitializecmdNext()
        tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Private WithEvents _optIncluded_0 As System.Windows.Forms.RadioButton
    Private WithEvents _optIncluded_1 As System.Windows.Forms.RadioButton
    Public WithEvents fraDepositChargedTo As System.Windows.Forms.GroupBox
    Private WithEvents _optProtectionChargedTo_1 As System.Windows.Forms.RadioButton
    Private WithEvents _optProtectionChargedTo_0 As System.Windows.Forms.RadioButton
    Public WithEvents Frame12 As System.Windows.Forms.Panel
    Public WithEvents fraFeesChargedTo As System.Windows.Forms.GroupBox
    Private WithEvents _optDepositType_0 As System.Windows.Forms.RadioButton
    Private WithEvents _optDepositType_1 As System.Windows.Forms.RadioButton
    Public WithEvents fraDepositType As System.Windows.Forms.GroupBox
    Public WithEvents txtDepositPC As System.Windows.Forms.TextBox
    Public WithEvents lblDepositPC As System.Windows.Forms.Label
    Public WithEvents fraDeposit As System.Windows.Forms.GroupBox
    Private WithEvents _optFeeType_1 As System.Windows.Forms.RadioButton
    Private WithEvents _optFeeType_0 As System.Windows.Forms.RadioButton
    Public WithEvents Frame14 As System.Windows.Forms.Panel
    Private WithEvents _optProtectionType_1 As System.Windows.Forms.RadioButton
    Private WithEvents _optProtectionType_0 As System.Windows.Forms.RadioButton
    Public WithEvents Frame15 As System.Windows.Forms.Panel
    Public WithEvents fraFeesType As System.Windows.Forms.GroupBox
    Public WithEvents txtArrangementFee As System.Windows.Forms.TextBox
    Public WithEvents txtProtectRate As System.Windows.Forms.TextBox
    Public WithEvents lblArrangementFee As System.Windows.Forms.Label
    Public WithEvents lblProtectRate As System.Windows.Forms.Label
    Public WithEvents fraFees As System.Windows.Forms.GroupBox
    Public WithEvents txtStartDate As System.Windows.Forms.TextBox
    Public WithEvents txtEndDate As System.Windows.Forms.TextBox
    Public WithEvents txtMnemonic As System.Windows.Forms.TextBox
    Public WithEvents cboProductFamily As System.Windows.Forms.ComboBox
    Public WithEvents lblStartDate As System.Windows.Forms.Label
    Public WithEvents lblEndDate As System.Windows.Forms.Label
    Public WithEvents lblMnemonic As System.Windows.Forms.Label
    Public WithEvents lblProductFamily As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Private WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Public WithEvents ChkFinanceNetCommission As System.Windows.Forms.CheckBox
    Public WithEvents lblFinanceNetCommission As System.Windows.Forms.Label
    Public WithEvents fraFinanceNetCommission As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_1 As System.Windows.Forms.Button
    Public WithEvents txtMaxInstalments As System.Windows.Forms.TextBox
    Public WithEvents txtR5Com As System.Windows.Forms.TextBox
    Public WithEvents txtRate5 As System.Windows.Forms.TextBox
    Public WithEvents txtMax5 As System.Windows.Forms.TextBox
    Public WithEvents txtMin5 As System.Windows.Forms.TextBox
    Public WithEvents txtR4Com As System.Windows.Forms.TextBox
    Public WithEvents txtRate4 As System.Windows.Forms.TextBox
    Public WithEvents txtMax4 As System.Windows.Forms.TextBox
    Public WithEvents txtMin4 As System.Windows.Forms.TextBox
    Public WithEvents txtR3Com As System.Windows.Forms.TextBox
    Public WithEvents txtRate3 As System.Windows.Forms.TextBox
    Public WithEvents txtMax3 As System.Windows.Forms.TextBox
    Public WithEvents txtMin3 As System.Windows.Forms.TextBox
    Public WithEvents txtR2Com As System.Windows.Forms.TextBox
    Public WithEvents txtRate2 As System.Windows.Forms.TextBox
    Public WithEvents txtMax2 As System.Windows.Forms.TextBox
    Public WithEvents txtMin2 As System.Windows.Forms.TextBox
    Public WithEvents txtR1Com As System.Windows.Forms.TextBox
    Public WithEvents txtRate1 As System.Windows.Forms.TextBox
    Public WithEvents txtMax1 As System.Windows.Forms.TextBox
    Public WithEvents txtMin1 As System.Windows.Forms.TextBox
    Public WithEvents txtMinInterest As System.Windows.Forms.TextBox
    Public WithEvents lblMinInterest As System.Windows.Forms.Label
    Public WithEvents lbl5 As System.Windows.Forms.Label
    Public WithEvents lbl4 As System.Windows.Forms.Label
    Public WithEvents lbl3 As System.Windows.Forms.Label
    Public WithEvents lbl2 As System.Windows.Forms.Label
    Public WithEvents lbl1 As System.Windows.Forms.Label
    Public WithEvents lblCommPC As System.Windows.Forms.Label
    Public WithEvents lblRate As System.Windows.Forms.Label
    Public WithEvents lblMin As System.Windows.Forms.Label
    Public WithEvents lblMax As System.Windows.Forms.Label
    Public WithEvents fraCharges As System.Windows.Forms.GroupBox
    Public WithEvents cboFrequency As System.Windows.Forms.ComboBox
    Public WithEvents lblMaxInstalments As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Private WithEvents _cmdPrevious_1 As System.Windows.Forms.Button
    Private WithEvents _cmdNext_2 As System.Windows.Forms.Button
    Public WithEvents txtMinMTAInstalments As System.Windows.Forms.TextBox
    Public WithEvents txtMinMTA As System.Windows.Forms.TextBox
    Public WithEvents chkAllowMTA As System.Windows.Forms.CheckBox
    Public WithEvents lblMinMTAInstalments As System.Windows.Forms.Label
    Public WithEvents lblMinMTA As System.Windows.Forms.Label
    Public WithEvents fraMTA As System.Windows.Forms.GroupBox
    Public WithEvents chkOnNextInstalmentDate As System.Windows.Forms.CheckBox
    Public WithEvents txtDaysLater As System.Windows.Forms.TextBox
    Public WithEvents txtRetryLimit As System.Windows.Forms.TextBox
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents lblRetryLimit As System.Windows.Forms.Label
    Public WithEvents framRecollection As System.Windows.Forms.GroupBox
    Public WithEvents txtExistingDaysDelay As System.Windows.Forms.TextBox
    Public WithEvents txtFirstInstalmentTo As System.Windows.Forms.TextBox
    Public WithEvents txtFirstInstalmentFrom As System.Windows.Forms.TextBox
    Public WithEvents lblExistingDaysDelay As System.Windows.Forms.Label
    Public WithEvents lblDaysAfterInception As System.Windows.Forms.Label
    Public WithEvents lblFirstInstalmentTo As System.Windows.Forms.Label
    Public WithEvents lblFirstInstalmentFrom As System.Windows.Forms.Label
    Public WithEvents Frame3 As System.Windows.Forms.GroupBox
    Private WithEvents _optAlign_1 As System.Windows.Forms.RadioButton
    Private WithEvents _optAlign_0 As System.Windows.Forms.RadioButton
    Public WithEvents fraAlign As System.Windows.Forms.GroupBox
    Private WithEvents _optBackDated_1 As System.Windows.Forms.RadioButton
    Private WithEvents _optBackDated_0 As System.Windows.Forms.RadioButton
    Public WithEvents fraBackDated As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents lblStatementFrequ As System.Windows.Forms.Label
    Public WithEvents lblStatementDoc As System.Windows.Forms.Label
    Public WithEvents lblAdvanceInst As System.Windows.Forms.Label
    Public WithEvents lblReviewUserGrp As System.Windows.Forms.Label
    Public WithEvents lblRemainderThreshhold As System.Windows.Forms.Label
    Public WithEvents cboStatementFrequ As System.Windows.Forms.ComboBox
    Public WithEvents cboStatementDoc As System.Windows.Forms.ComboBox
    Public WithEvents cboReviewUserGrp As System.Windows.Forms.ComboBox
    Public WithEvents txtAdvanceInst As System.Windows.Forms.TextBox
    Public WithEvents txtRemainderThreshhold As System.Windows.Forms.TextBox
    Public WithEvents chkRemainderAtEnd As System.Windows.Forms.CheckBox
    Private WithEvents _cmdPrevious_2 As System.Windows.Forms.Button
    Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public cmdNext(2) As System.Windows.Forms.Button
    Public cmdPrevious(2) As System.Windows.Forms.Button
    Public optAlign(1) As System.Windows.Forms.RadioButton
    Public optBackDated(1) As System.Windows.Forms.RadioButton
    Public optDepositType(1) As System.Windows.Forms.RadioButton
    Public optFeeChargedTo(1) As System.Windows.Forms.RadioButton
    Public optFeeType(1) As System.Windows.Forms.RadioButton
    Public optIncluded(1) As System.Windows.Forms.RadioButton
    Public optProtectionChargedTo(1) As System.Windows.Forms.RadioButton
    Public optProtectionType(1) As System.Windows.Forms.RadioButton
    Public optTaxChargedTo(1) As System.Windows.Forms.RadioButton
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNavigate = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkApplyFeePercentagesToTaxes = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkApplyFeePercentagesToFees = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.fraDepositChargedTo = New System.Windows.Forms.GroupBox()
        Me._optIncluded_0 = New System.Windows.Forms.RadioButton()
        Me._optIncluded_1 = New System.Windows.Forms.RadioButton()
        Me.fraFeesChargedTo = New System.Windows.Forms.GroupBox()
        Me.Frame11 = New System.Windows.Forms.Panel()
        Me._optFeeChargedTo_1 = New System.Windows.Forms.RadioButton()
        Me._optFeeChargedTo_0 = New System.Windows.Forms.RadioButton()
        Me.Frame12 = New System.Windows.Forms.Panel()
        Me._optProtectionChargedTo_1 = New System.Windows.Forms.RadioButton()
        Me._optProtectionChargedTo_0 = New System.Windows.Forms.RadioButton()
        Me.fraDepositType = New System.Windows.Forms.GroupBox()
        Me._optDepositType_0 = New System.Windows.Forms.RadioButton()
        Me._optDepositType_1 = New System.Windows.Forms.RadioButton()
        Me.fraDeposit = New System.Windows.Forms.GroupBox()
        Me.chkDepositOverrideAllowed = New System.Windows.Forms.CheckBox()
        Me.lblOverrideAllowed = New System.Windows.Forms.Label()
        Me.txtDepositPC = New System.Windows.Forms.TextBox()
        Me.lblDepositPC = New System.Windows.Forms.Label()
        Me.fraFeesType = New System.Windows.Forms.GroupBox()
        Me.Frame14 = New System.Windows.Forms.Panel()
        Me._optFeeType_1 = New System.Windows.Forms.RadioButton()
        Me._optFeeType_0 = New System.Windows.Forms.RadioButton()
        Me.Frame15 = New System.Windows.Forms.Panel()
        Me._optProtectionType_1 = New System.Windows.Forms.RadioButton()
        Me._optProtectionType_0 = New System.Windows.Forms.RadioButton()
        Me.fraFees = New System.Windows.Forms.GroupBox()
        Me.txtArrangementFee = New System.Windows.Forms.TextBox()
        Me.txtProtectRate = New System.Windows.Forms.TextBox()
        Me.lblArrangementFee = New System.Windows.Forms.Label()
        Me.lblProtectRate = New System.Windows.Forms.Label()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.txtStartDate = New System.Windows.Forms.TextBox()
        Me.txtEndDate = New System.Windows.Forms.TextBox()
        Me.txtMnemonic = New System.Windows.Forms.TextBox()
        Me.cboProductFamily = New System.Windows.Forms.ComboBox()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.lblMnemonic = New System.Windows.Forms.Label()
        Me.lblProductFamily = New System.Windows.Forms.Label()
        Me._cmdNext_0 = New System.Windows.Forms.Button()
        Me.fraFinanceNetCommission = New System.Windows.Forms.GroupBox()
        Me.ChkFinanceNetCommission = New System.Windows.Forms.CheckBox()
        Me.lblFinanceNetCommission = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_0 = New System.Windows.Forms.Button()
        Me._cmdNext_1 = New System.Windows.Forms.Button()
        Me.txtMaxInstalments = New System.Windows.Forms.TextBox()
        Me.fraCharges = New System.Windows.Forms.GroupBox()
        Me.txtR5Com = New System.Windows.Forms.TextBox()
        Me.txtRate5 = New System.Windows.Forms.TextBox()
        Me.txtMax5 = New System.Windows.Forms.TextBox()
        Me.txtMin5 = New System.Windows.Forms.TextBox()
        Me.txtR4Com = New System.Windows.Forms.TextBox()
        Me.txtRate4 = New System.Windows.Forms.TextBox()
        Me.txtMax4 = New System.Windows.Forms.TextBox()
        Me.txtMin4 = New System.Windows.Forms.TextBox()
        Me.txtR3Com = New System.Windows.Forms.TextBox()
        Me.txtRate3 = New System.Windows.Forms.TextBox()
        Me.txtMax3 = New System.Windows.Forms.TextBox()
        Me.txtMin3 = New System.Windows.Forms.TextBox()
        Me.txtR2Com = New System.Windows.Forms.TextBox()
        Me.txtRate2 = New System.Windows.Forms.TextBox()
        Me.txtMax2 = New System.Windows.Forms.TextBox()
        Me.txtMin2 = New System.Windows.Forms.TextBox()
        Me.txtR1Com = New System.Windows.Forms.TextBox()
        Me.txtRate1 = New System.Windows.Forms.TextBox()
        Me.txtMax1 = New System.Windows.Forms.TextBox()
        Me.txtMin1 = New System.Windows.Forms.TextBox()
        Me.txtMinInterest = New System.Windows.Forms.TextBox()
        Me.lblMinInterest = New System.Windows.Forms.Label()
        Me.lbl5 = New System.Windows.Forms.Label()
        Me.lbl4 = New System.Windows.Forms.Label()
        Me.lbl3 = New System.Windows.Forms.Label()
        Me.lbl2 = New System.Windows.Forms.Label()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.lblCommPC = New System.Windows.Forms.Label()
        Me.lblRate = New System.Windows.Forms.Label()
        Me.lblMin = New System.Windows.Forms.Label()
        Me.lblMax = New System.Windows.Forms.Label()
        Me.cboFrequency = New System.Windows.Forms.ComboBox()
        Me.lblMaxInstalments = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me._cmdPrevious_1 = New System.Windows.Forms.Button()
        Me._cmdNext_2 = New System.Windows.Forms.Button()
        Me.fraMTA = New System.Windows.Forms.GroupBox()
        Me.txtMinMTAInstalments = New System.Windows.Forms.TextBox()
        Me.txtMinMTA = New System.Windows.Forms.TextBox()
        Me.chkAllowMTA = New System.Windows.Forms.CheckBox()
        Me.lblMinMTAInstalments = New System.Windows.Forms.Label()
        Me.lblMinMTA = New System.Windows.Forms.Label()
        Me.framRecollection = New System.Windows.Forms.GroupBox()
        Me.chkOnNextInstalmentDate = New System.Windows.Forms.CheckBox()
        Me.txtDaysLater = New System.Windows.Forms.TextBox()
        Me.txtRetryLimit = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblRetryLimit = New System.Windows.Forms.Label()
        Me.Frame3 = New System.Windows.Forms.GroupBox()
        Me.chkFirstInstalmentAlignWithDayInMonth = New System.Windows.Forms.CheckBox()
        Me.txtExistingDaysDelay = New System.Windows.Forms.TextBox()
        Me.txtFirstInstalmentTo = New System.Windows.Forms.TextBox()
        Me.txtFirstInstalmentFrom = New System.Windows.Forms.TextBox()
        Me.lblExistingDaysDelay = New System.Windows.Forms.Label()
        Me.lblDaysAfterInception = New System.Windows.Forms.Label()
        Me.lblFirstInstalmentTo = New System.Windows.Forms.Label()
        Me.lblFirstInstalmentFrom = New System.Windows.Forms.Label()
        Me.fraAlign = New System.Windows.Forms.GroupBox()
        Me.chkSingleInstalmentPerMonth = New System.Windows.Forms.CheckBox()
        Me._optAlign_1 = New System.Windows.Forms.RadioButton()
        Me._optAlign_0 = New System.Windows.Forms.RadioButton()
        Me.fraBackDated = New System.Windows.Forms.GroupBox()
        Me._optBackDated_1 = New System.Windows.Forms.RadioButton()
        Me._optBackDated_0 = New System.Windows.Forms.RadioButton()
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me.lblStatementFrequ = New System.Windows.Forms.Label()
        Me.lblStatementDoc = New System.Windows.Forms.Label()
        Me.lblAdvanceInst = New System.Windows.Forms.Label()
        Me.lblReviewUserGrp = New System.Windows.Forms.Label()
        Me.lblRemainderThreshhold = New System.Windows.Forms.Label()
        Me.cboStatementFrequ = New System.Windows.Forms.ComboBox()
        Me.cboStatementDoc = New System.Windows.Forms.ComboBox()
        Me.cboReviewUserGrp = New System.Windows.Forms.ComboBox()
        Me.txtAdvanceInst = New System.Windows.Forms.TextBox()
        Me.txtRemainderThreshhold = New System.Windows.Forms.TextBox()
        Me.chkRemainderAtEnd = New System.Windows.Forms.CheckBox()
        Me._cmdPrevious_2 = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.fraDepositChargedTo.SuspendLayout()
        Me.fraFeesChargedTo.SuspendLayout()
        Me.Frame11.SuspendLayout()
        Me.Frame12.SuspendLayout()
        Me.fraDepositType.SuspendLayout()
        Me.fraDeposit.SuspendLayout()
        Me.fraFeesType.SuspendLayout()
        Me.Frame14.SuspendLayout()
        Me.Frame15.SuspendLayout()
        Me.fraFees.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.fraFinanceNetCommission.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me.fraCharges.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me.fraMTA.SuspendLayout()
        Me.framRecollection.SuspendLayout()
        Me.Frame3.SuspendLayout()
        Me.fraAlign.SuspendLayout()
        Me.fraBackDated.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 456)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 53
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(480, 456)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 79
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
        Me.cmdCancel.Location = New System.Drawing.Point(400, 456)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 77
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
        Me.cmdOK.Location = New System.Drawing.Point(320, 456)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 75
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage3)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(135, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(549, 437)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.GroupBox1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraDepositChargedTo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraFeesChargedTo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraDepositType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraDeposit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraFeesType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraFees)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraFinanceNetCommission)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(541, 411)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Rates"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.chkApplyFeePercentagesToTaxes)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.chkApplyFeePercentagesToFees)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox1.Location = New System.Drawing.Point(20, 188)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox1.Size = New System.Drawing.Size(255, 57)
        Me.GroupBox1.TabIndex = 127
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Tag = "HIDESG;"
        Me.GroupBox1.Text = "Fee Percentages Applied To"
        '
        'chkApplyFeePercentagesToTaxes
        '
        Me.chkApplyFeePercentagesToTaxes.BackColor = System.Drawing.SystemColors.Control
        Me.chkApplyFeePercentagesToTaxes.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkApplyFeePercentagesToTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkApplyFeePercentagesToTaxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkApplyFeePercentagesToTaxes.Location = New System.Drawing.Point(133, 19)
        Me.chkApplyFeePercentagesToTaxes.Name = "chkApplyFeePercentagesToTaxes"
        Me.chkApplyFeePercentagesToTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkApplyFeePercentagesToTaxes.Size = New System.Drawing.Size(20, 25)
        Me.chkApplyFeePercentagesToTaxes.TabIndex = 129
        Me.chkApplyFeePercentagesToTaxes.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(150, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(100, 17)
        Me.Label3.TabIndex = 128
        Me.Label3.Text = "Policy / Risk Taxes"
        '
        'chkApplyFeePercentagesToFees
        '
        Me.chkApplyFeePercentagesToFees.BackColor = System.Drawing.SystemColors.Control
        Me.chkApplyFeePercentagesToFees.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkApplyFeePercentagesToFees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkApplyFeePercentagesToFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkApplyFeePercentagesToFees.Location = New System.Drawing.Point(10, 18)
        Me.chkApplyFeePercentagesToFees.Name = "chkApplyFeePercentagesToFees"
        Me.chkApplyFeePercentagesToFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkApplyFeePercentagesToFees.Size = New System.Drawing.Size(19, 25)
        Me.chkApplyFeePercentagesToFees.TabIndex = 127
        Me.chkApplyFeePercentagesToFees.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(31, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(93, 17)
        Me.Label2.TabIndex = 126
        Me.Label2.Text = "Policy / Risk Fees"
        '
        'fraDepositChargedTo
        '
        Me.fraDepositChargedTo.BackColor = System.Drawing.SystemColors.Control
        Me.fraDepositChargedTo.Controls.Add(Me._optIncluded_0)
        Me.fraDepositChargedTo.Controls.Add(Me._optIncluded_1)
        Me.fraDepositChargedTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDepositChargedTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDepositChargedTo.Location = New System.Drawing.Point(330, 252)
        Me.fraDepositChargedTo.Name = "fraDepositChargedTo"
        Me.fraDepositChargedTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDepositChargedTo.Size = New System.Drawing.Size(193, 74)
        Me.fraDepositChargedTo.TabIndex = 105
        Me.fraDepositChargedTo.TabStop = False
        Me.fraDepositChargedTo.Tag = "HIDESG;"
        Me.fraDepositChargedTo.Text = "Included in Interest Charge"
        '
        '_optIncluded_0
        '
        Me._optIncluded_0.BackColor = System.Drawing.SystemColors.Control
        Me._optIncluded_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optIncluded_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optIncluded_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optIncluded_0.Location = New System.Drawing.Point(16, 24)
        Me._optIncluded_0.Name = "_optIncluded_0"
        Me._optIncluded_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optIncluded_0.Size = New System.Drawing.Size(57, 17)
        Me._optIncluded_0.TabIndex = 20
        Me._optIncluded_0.TabStop = True
        Me._optIncluded_0.Text = "Yes"
        Me._optIncluded_0.UseVisualStyleBackColor = False
        '
        '_optIncluded_1
        '
        Me._optIncluded_1.BackColor = System.Drawing.SystemColors.Control
        Me._optIncluded_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optIncluded_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optIncluded_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optIncluded_1.Location = New System.Drawing.Point(128, 24)
        Me._optIncluded_1.Name = "_optIncluded_1"
        Me._optIncluded_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optIncluded_1.Size = New System.Drawing.Size(57, 17)
        Me._optIncluded_1.TabIndex = 21
        Me._optIncluded_1.TabStop = True
        Me._optIncluded_1.Text = "No"
        Me._optIncluded_1.UseVisualStyleBackColor = False
        '
        'fraFeesChargedTo
        '
        Me.fraFeesChargedTo.BackColor = System.Drawing.SystemColors.Control
        Me.fraFeesChargedTo.Controls.Add(Me.Frame11)
        Me.fraFeesChargedTo.Controls.Add(Me.Frame12)
        Me.fraFeesChargedTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFeesChargedTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFeesChargedTo.Location = New System.Drawing.Point(330, 98)
        Me.fraFeesChargedTo.Name = "fraFeesChargedTo"
        Me.fraFeesChargedTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFeesChargedTo.Size = New System.Drawing.Size(193, 85)
        Me.fraFeesChargedTo.TabIndex = 106
        Me.fraFeesChargedTo.TabStop = False
        Me.fraFeesChargedTo.Tag = "HIDESG;"
        Me.fraFeesChargedTo.Text = "Amount Charged To"
        '
        'Frame11
        '
        Me.Frame11.BackColor = System.Drawing.SystemColors.Control
        Me.Frame11.Controls.Add(Me._optFeeChargedTo_1)
        Me.Frame11.Controls.Add(Me._optFeeChargedTo_0)
        Me.Frame11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Frame11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame11.Location = New System.Drawing.Point(8, 23)
        Me.Frame11.Name = "Frame11"
        Me.Frame11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame11.Size = New System.Drawing.Size(177, 25)
        Me.Frame11.TabIndex = 109
        Me.Frame11.Text = "Frame11"
        '
        '_optFeeChargedTo_1
        '
        Me._optFeeChargedTo_1.BackColor = System.Drawing.SystemColors.Control
        Me._optFeeChargedTo_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFeeChargedTo_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFeeChargedTo_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFeeChargedTo_1.Location = New System.Drawing.Point(128, 0)
        Me._optFeeChargedTo_1.Name = "_optFeeChargedTo_1"
        Me._optFeeChargedTo_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFeeChargedTo_1.Size = New System.Drawing.Size(49, 17)
        Me._optFeeChargedTo_1.TabIndex = 11
        Me._optFeeChargedTo_1.TabStop = True
        Me._optFeeChargedTo_1.Text = "Plan"
        Me._optFeeChargedTo_1.UseVisualStyleBackColor = False
        '
        '_optFeeChargedTo_0
        '
        Me._optFeeChargedTo_0.BackColor = System.Drawing.SystemColors.Control
        Me._optFeeChargedTo_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFeeChargedTo_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFeeChargedTo_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFeeChargedTo_0.Location = New System.Drawing.Point(8, 0)
        Me._optFeeChargedTo_0.Name = "_optFeeChargedTo_0"
        Me._optFeeChargedTo_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFeeChargedTo_0.Size = New System.Drawing.Size(121, 17)
        Me._optFeeChargedTo_0.TabIndex = 10
        Me._optFeeChargedTo_0.TabStop = True
        Me._optFeeChargedTo_0.Text = "First Instalment"
        Me._optFeeChargedTo_0.UseVisualStyleBackColor = False
        '
        'Frame12
        '
        Me.Frame12.BackColor = System.Drawing.SystemColors.Control
        Me.Frame12.Controls.Add(Me._optProtectionChargedTo_1)
        Me.Frame12.Controls.Add(Me._optProtectionChargedTo_0)
        Me.Frame12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Frame12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame12.Location = New System.Drawing.Point(8, 55)
        Me.Frame12.Name = "Frame12"
        Me.Frame12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame12.Size = New System.Drawing.Size(177, 25)
        Me.Frame12.TabIndex = 108
        Me.Frame12.Text = "Frame12"
        '
        '_optProtectionChargedTo_1
        '
        Me._optProtectionChargedTo_1.BackColor = System.Drawing.SystemColors.Control
        Me._optProtectionChargedTo_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optProtectionChargedTo_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optProtectionChargedTo_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optProtectionChargedTo_1.Location = New System.Drawing.Point(128, 0)
        Me._optProtectionChargedTo_1.Name = "_optProtectionChargedTo_1"
        Me._optProtectionChargedTo_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optProtectionChargedTo_1.Size = New System.Drawing.Size(49, 17)
        Me._optProtectionChargedTo_1.TabIndex = 16
        Me._optProtectionChargedTo_1.TabStop = True
        Me._optProtectionChargedTo_1.Text = "Plan"
        Me._optProtectionChargedTo_1.UseVisualStyleBackColor = False
        '
        '_optProtectionChargedTo_0
        '
        Me._optProtectionChargedTo_0.BackColor = System.Drawing.SystemColors.Control
        Me._optProtectionChargedTo_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optProtectionChargedTo_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optProtectionChargedTo_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optProtectionChargedTo_0.Location = New System.Drawing.Point(8, 0)
        Me._optProtectionChargedTo_0.Name = "_optProtectionChargedTo_0"
        Me._optProtectionChargedTo_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optProtectionChargedTo_0.Size = New System.Drawing.Size(121, 17)
        Me._optProtectionChargedTo_0.TabIndex = 15
        Me._optProtectionChargedTo_0.TabStop = True
        Me._optProtectionChargedTo_0.Text = "First Instalment"
        Me._optProtectionChargedTo_0.UseVisualStyleBackColor = False
        '
        'fraDepositType
        '
        Me.fraDepositType.BackColor = System.Drawing.SystemColors.Control
        Me.fraDepositType.Controls.Add(Me._optDepositType_0)
        Me.fraDepositType.Controls.Add(Me._optDepositType_1)
        Me.fraDepositType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDepositType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDepositType.Location = New System.Drawing.Point(210, 252)
        Me.fraDepositType.Name = "fraDepositType"
        Me.fraDepositType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDepositType.Size = New System.Drawing.Size(123, 74)
        Me.fraDepositType.TabIndex = 110
        Me.fraDepositType.TabStop = False
        Me.fraDepositType.Tag = "HIDESG;"
        Me.fraDepositType.Text = "Type"
        '
        '_optDepositType_0
        '
        Me._optDepositType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optDepositType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optDepositType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optDepositType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optDepositType_0.Location = New System.Drawing.Point(8, 24)
        Me._optDepositType_0.Name = "_optDepositType_0"
        Me._optDepositType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optDepositType_0.Size = New System.Drawing.Size(41, 17)
        Me._optDepositType_0.TabIndex = 18
        Me._optDepositType_0.TabStop = True
        Me._optDepositType_0.Text = "%"
        Me._optDepositType_0.UseVisualStyleBackColor = False
        '
        '_optDepositType_1
        '
        Me._optDepositType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optDepositType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optDepositType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optDepositType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optDepositType_1.Location = New System.Drawing.Point(48, 24)
        Me._optDepositType_1.Name = "_optDepositType_1"
        Me._optDepositType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optDepositType_1.Size = New System.Drawing.Size(65, 17)
        Me._optDepositType_1.TabIndex = 19
        Me._optDepositType_1.TabStop = True
        Me._optDepositType_1.Text = "Amount"
        Me._optDepositType_1.UseVisualStyleBackColor = False
        '
        'fraDeposit
        '
        Me.fraDeposit.BackColor = System.Drawing.SystemColors.Control
        Me.fraDeposit.Controls.Add(Me.chkDepositOverrideAllowed)
        Me.fraDeposit.Controls.Add(Me.lblOverrideAllowed)
        Me.fraDeposit.Controls.Add(Me.txtDepositPC)
        Me.fraDeposit.Controls.Add(Me.lblDepositPC)
        Me.fraDeposit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDeposit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDeposit.Location = New System.Drawing.Point(20, 252)
        Me.fraDeposit.Name = "fraDeposit"
        Me.fraDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDeposit.Size = New System.Drawing.Size(193, 74)
        Me.fraDeposit.TabIndex = 111
        Me.fraDeposit.TabStop = False
        Me.fraDeposit.Tag = "HIDESG;"
        Me.fraDeposit.Text = "Deposit"
        '
        'chkDepositOverrideAllowed
        '
        Me.chkDepositOverrideAllowed.AutoSize = True
        Me.chkDepositOverrideAllowed.Checked = True
        Me.chkDepositOverrideAllowed.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDepositOverrideAllowed.Location = New System.Drawing.Point(120, 50)
        Me.chkDepositOverrideAllowed.Name = "chkDepositOverrideAllowed"
        Me.chkDepositOverrideAllowed.Size = New System.Drawing.Size(15, 14)
        Me.chkDepositOverrideAllowed.TabIndex = 114
        Me.chkDepositOverrideAllowed.UseVisualStyleBackColor = True
        '
        'lblOverrideAllowed
        '
        Me.lblOverrideAllowed.AutoSize = True
        Me.lblOverrideAllowed.Location = New System.Drawing.Point(9, 50)
        Me.lblOverrideAllowed.Name = "lblOverrideAllowed"
        Me.lblOverrideAllowed.Size = New System.Drawing.Size(90, 13)
        Me.lblOverrideAllowed.TabIndex = 113
        Me.lblOverrideAllowed.Text = "&Override Allowed:"
        '
        'txtDepositPC
        '
        Me.txtDepositPC.AcceptsReturn = True
        Me.txtDepositPC.BackColor = System.Drawing.SystemColors.Window
        Me.txtDepositPC.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDepositPC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDepositPC.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDepositPC.Location = New System.Drawing.Point(120, 24)
        Me.txtDepositPC.MaxLength = 0
        Me.txtDepositPC.Name = "txtDepositPC"
        Me.txtDepositPC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDepositPC.Size = New System.Drawing.Size(65, 20)
        Me.txtDepositPC.TabIndex = 17
        Me.txtDepositPC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblDepositPC
        '
        Me.lblDepositPC.BackColor = System.Drawing.SystemColors.Control
        Me.lblDepositPC.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDepositPC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDepositPC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDepositPC.Location = New System.Drawing.Point(8, 24)
        Me.lblDepositPC.Name = "lblDepositPC"
        Me.lblDepositPC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDepositPC.Size = New System.Drawing.Size(57, 17)
        Me.lblDepositPC.TabIndex = 112
        Me.lblDepositPC.Text = "&Deposit:"
        '
        'fraFeesType
        '
        Me.fraFeesType.BackColor = System.Drawing.SystemColors.Control
        Me.fraFeesType.Controls.Add(Me.Frame14)
        Me.fraFeesType.Controls.Add(Me.Frame15)
        Me.fraFeesType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFeesType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFeesType.Location = New System.Drawing.Point(210, 98)
        Me.fraFeesType.Name = "fraFeesType"
        Me.fraFeesType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFeesType.Size = New System.Drawing.Size(123, 85)
        Me.fraFeesType.TabIndex = 113
        Me.fraFeesType.TabStop = False
        Me.fraFeesType.Tag = "HIDESG;"
        Me.fraFeesType.Text = "Type"
        '
        'Frame14
        '
        Me.Frame14.BackColor = System.Drawing.SystemColors.Control
        Me.Frame14.Controls.Add(Me._optFeeType_1)
        Me.Frame14.Controls.Add(Me._optFeeType_0)
        Me.Frame14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Frame14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame14.Location = New System.Drawing.Point(8, 17)
        Me.Frame14.Name = "Frame14"
        Me.Frame14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame14.Size = New System.Drawing.Size(105, 33)
        Me.Frame14.TabIndex = 115
        Me.Frame14.Text = "Frame14"
        '
        '_optFeeType_1
        '
        Me._optFeeType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optFeeType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFeeType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFeeType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFeeType_1.Location = New System.Drawing.Point(40, 8)
        Me._optFeeType_1.Name = "_optFeeType_1"
        Me._optFeeType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFeeType_1.Size = New System.Drawing.Size(65, 17)
        Me._optFeeType_1.TabIndex = 9
        Me._optFeeType_1.TabStop = True
        Me._optFeeType_1.Text = "Amount"
        Me._optFeeType_1.UseVisualStyleBackColor = False
        '
        '_optFeeType_0
        '
        Me._optFeeType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optFeeType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optFeeType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optFeeType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optFeeType_0.Location = New System.Drawing.Point(0, 8)
        Me._optFeeType_0.Name = "_optFeeType_0"
        Me._optFeeType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optFeeType_0.Size = New System.Drawing.Size(41, 17)
        Me._optFeeType_0.TabIndex = 8
        Me._optFeeType_0.TabStop = True
        Me._optFeeType_0.Text = "%"
        Me._optFeeType_0.UseVisualStyleBackColor = False
        '
        'Frame15
        '
        Me.Frame15.BackColor = System.Drawing.SystemColors.Control
        Me.Frame15.Controls.Add(Me._optProtectionType_1)
        Me.Frame15.Controls.Add(Me._optProtectionType_0)
        Me.Frame15.Cursor = System.Windows.Forms.Cursors.Default
        Me.Frame15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame15.Location = New System.Drawing.Point(8, 55)
        Me.Frame15.Name = "Frame15"
        Me.Frame15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame15.Size = New System.Drawing.Size(105, 17)
        Me.Frame15.TabIndex = 114
        Me.Frame15.Text = "Frame15"
        '
        '_optProtectionType_1
        '
        Me._optProtectionType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optProtectionType_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optProtectionType_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optProtectionType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optProtectionType_1.Location = New System.Drawing.Point(40, 0)
        Me._optProtectionType_1.Name = "_optProtectionType_1"
        Me._optProtectionType_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optProtectionType_1.Size = New System.Drawing.Size(65, 17)
        Me._optProtectionType_1.TabIndex = 14
        Me._optProtectionType_1.TabStop = True
        Me._optProtectionType_1.Text = "Amount"
        Me._optProtectionType_1.UseVisualStyleBackColor = False
        '
        '_optProtectionType_0
        '
        Me._optProtectionType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optProtectionType_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optProtectionType_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optProtectionType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optProtectionType_0.Location = New System.Drawing.Point(0, 0)
        Me._optProtectionType_0.Name = "_optProtectionType_0"
        Me._optProtectionType_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optProtectionType_0.Size = New System.Drawing.Size(33, 17)
        Me._optProtectionType_0.TabIndex = 13
        Me._optProtectionType_0.TabStop = True
        Me._optProtectionType_0.Text = "%"
        Me._optProtectionType_0.UseVisualStyleBackColor = False
        '
        'fraFees
        '
        Me.fraFees.BackColor = System.Drawing.SystemColors.Control
        Me.fraFees.Controls.Add(Me.txtArrangementFee)
        Me.fraFees.Controls.Add(Me.txtProtectRate)
        Me.fraFees.Controls.Add(Me.lblArrangementFee)
        Me.fraFees.Controls.Add(Me.lblProtectRate)
        Me.fraFees.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFees.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFees.Location = New System.Drawing.Point(20, 98)
        Me.fraFees.Name = "fraFees"
        Me.fraFees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFees.Size = New System.Drawing.Size(193, 85)
        Me.fraFees.TabIndex = 116
        Me.fraFees.TabStop = False
        Me.fraFees.Tag = "HIDESG;"
        Me.fraFees.Text = "Fees"
        '
        'txtArrangementFee
        '
        Me.txtArrangementFee.AcceptsReturn = True
        Me.txtArrangementFee.BackColor = System.Drawing.SystemColors.Window
        Me.txtArrangementFee.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtArrangementFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArrangementFee.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtArrangementFee.Location = New System.Drawing.Point(120, 23)
        Me.txtArrangementFee.MaxLength = 0
        Me.txtArrangementFee.Name = "txtArrangementFee"
        Me.txtArrangementFee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtArrangementFee.Size = New System.Drawing.Size(65, 20)
        Me.txtArrangementFee.TabIndex = 7
        Me.txtArrangementFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtProtectRate
        '
        Me.txtProtectRate.AcceptsReturn = True
        Me.txtProtectRate.BackColor = System.Drawing.SystemColors.Window
        Me.txtProtectRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProtectRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProtectRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProtectRate.Location = New System.Drawing.Point(120, 55)
        Me.txtProtectRate.MaxLength = 0
        Me.txtProtectRate.Name = "txtProtectRate"
        Me.txtProtectRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtProtectRate.Size = New System.Drawing.Size(65, 20)
        Me.txtProtectRate.TabIndex = 12
        Me.txtProtectRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblArrangementFee
        '
        Me.lblArrangementFee.BackColor = System.Drawing.SystemColors.Control
        Me.lblArrangementFee.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblArrangementFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArrangementFee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblArrangementFee.Location = New System.Drawing.Point(8, 23)
        Me.lblArrangementFee.Name = "lblArrangementFee"
        Me.lblArrangementFee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblArrangementFee.Size = New System.Drawing.Size(121, 17)
        Me.lblArrangementFee.TabIndex = 119
        Me.lblArrangementFee.Text = "&Arrangement Fee:"
        '
        'lblProtectRate
        '
        Me.lblProtectRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblProtectRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProtectRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProtectRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProtectRate.Location = New System.Drawing.Point(8, 55)
        Me.lblProtectRate.Name = "lblProtectRate"
        Me.lblProtectRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProtectRate.Size = New System.Drawing.Size(89, 17)
        Me.lblProtectRate.TabIndex = 118
        Me.lblProtectRate.Text = "&Protection:"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtStartDate)
        Me.Frame1.Controls.Add(Me.txtEndDate)
        Me.Frame1.Controls.Add(Me.txtMnemonic)
        Me.Frame1.Controls.Add(Me.cboProductFamily)
        Me.Frame1.Controls.Add(Me.lblStartDate)
        Me.Frame1.Controls.Add(Me.lblEndDate)
        Me.Frame1.Controls.Add(Me.lblMnemonic)
        Me.Frame1.Controls.Add(Me.lblProductFamily)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(20, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(503, 87)
        Me.Frame1.TabIndex = 120
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Rate Information"
        '
        'txtStartDate
        '
        Me.txtStartDate.AcceptsReturn = True
        Me.txtStartDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartDate.Location = New System.Drawing.Point(134, 24)
        Me.txtStartDate.MaxLength = 0
        Me.txtStartDate.Name = "txtStartDate"
        Me.txtStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartDate.Size = New System.Drawing.Size(121, 20)
        Me.txtStartDate.TabIndex = 1
        '
        'txtEndDate
        '
        Me.txtEndDate.AcceptsReturn = True
        Me.txtEndDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEndDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEndDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEndDate.Location = New System.Drawing.Point(360, 24)
        Me.txtEndDate.MaxLength = 0
        Me.txtEndDate.Name = "txtEndDate"
        Me.txtEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEndDate.Size = New System.Drawing.Size(129, 20)
        Me.txtEndDate.TabIndex = 2
        '
        'txtMnemonic
        '
        Me.txtMnemonic.AcceptsReturn = True
        Me.txtMnemonic.BackColor = System.Drawing.SystemColors.Window
        Me.txtMnemonic.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMnemonic.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMnemonic.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMnemonic.Location = New System.Drawing.Point(360, 48)
        Me.txtMnemonic.MaxLength = 0
        Me.txtMnemonic.Name = "txtMnemonic"
        Me.txtMnemonic.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMnemonic.Size = New System.Drawing.Size(49, 20)
        Me.txtMnemonic.TabIndex = 4
        '
        'cboProductFamily
        '
        Me.cboProductFamily.BackColor = System.Drawing.SystemColors.Window
        Me.cboProductFamily.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProductFamily.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProductFamily.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProductFamily.Location = New System.Drawing.Point(134, 48)
        Me.cboProductFamily.Name = "cboProductFamily"
        Me.cboProductFamily.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProductFamily.Size = New System.Drawing.Size(121, 21)
        Me.cboProductFamily.TabIndex = 3
        Me.cboProductFamily.Tag = "E;"
        '
        'lblStartDate
        '
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(12, 24)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(113, 17)
        Me.lblStartDate.TabIndex = 124
        Me.lblStartDate.Text = "&Start Date:"
        '
        'lblEndDate
        '
        Me.lblEndDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEndDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEndDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEndDate.Location = New System.Drawing.Point(272, 24)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEndDate.Size = New System.Drawing.Size(81, 17)
        Me.lblEndDate.TabIndex = 123
        Me.lblEndDate.Text = "&End Date:"
        '
        'lblMnemonic
        '
        Me.lblMnemonic.BackColor = System.Drawing.SystemColors.Control
        Me.lblMnemonic.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMnemonic.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMnemonic.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMnemonic.Location = New System.Drawing.Point(272, 48)
        Me.lblMnemonic.Name = "lblMnemonic"
        Me.lblMnemonic.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMnemonic.Size = New System.Drawing.Size(105, 17)
        Me.lblMnemonic.TabIndex = 122
        Me.lblMnemonic.Text = "&Code:"
        '
        'lblProductFamily
        '
        Me.lblProductFamily.BackColor = System.Drawing.SystemColors.Control
        Me.lblProductFamily.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProductFamily.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProductFamily.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProductFamily.Location = New System.Drawing.Point(12, 50)
        Me.lblProductFamily.Name = "lblProductFamily"
        Me.lblProductFamily.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProductFamily.Size = New System.Drawing.Size(127, 17)
        Me.lblProductFamily.TabIndex = 121
        Me.lblProductFamily.Text = "&Transaction Type:"
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(496, 388)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_0.TabIndex = 22
        Me._cmdNext_0.Text = "&>>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        '
        'fraFinanceNetCommission
        '
        Me.fraFinanceNetCommission.BackColor = System.Drawing.SystemColors.Control
        Me.fraFinanceNetCommission.Controls.Add(Me.ChkFinanceNetCommission)
        Me.fraFinanceNetCommission.Controls.Add(Me.lblFinanceNetCommission)
        Me.fraFinanceNetCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFinanceNetCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFinanceNetCommission.Location = New System.Drawing.Point(18, 334)
        Me.fraFinanceNetCommission.Name = "fraFinanceNetCommission"
        Me.fraFinanceNetCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFinanceNetCommission.Size = New System.Drawing.Size(193, 57)
        Me.fraFinanceNetCommission.TabIndex = 125
        Me.fraFinanceNetCommission.TabStop = False
        Me.fraFinanceNetCommission.Tag = "HIDESG;"
        Me.fraFinanceNetCommission.Text = "Finance Net Commission"
        '
        'ChkFinanceNetCommission
        '
        Me.ChkFinanceNetCommission.BackColor = System.Drawing.SystemColors.Control
        Me.ChkFinanceNetCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.ChkFinanceNetCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkFinanceNetCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ChkFinanceNetCommission.Location = New System.Drawing.Point(10, 18)
        Me.ChkFinanceNetCommission.Name = "ChkFinanceNetCommission"
        Me.ChkFinanceNetCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ChkFinanceNetCommission.Size = New System.Drawing.Size(23, 25)
        Me.ChkFinanceNetCommission.TabIndex = 127
        Me.ChkFinanceNetCommission.UseVisualStyleBackColor = False
        '
        'lblFinanceNetCommission
        '
        Me.lblFinanceNetCommission.BackColor = System.Drawing.SystemColors.Control
        Me.lblFinanceNetCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFinanceNetCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinanceNetCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinanceNetCommission.Location = New System.Drawing.Point(37, 24)
        Me.lblFinanceNetCommission.Name = "lblFinanceNetCommission"
        Me.lblFinanceNetCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFinanceNetCommission.Size = New System.Drawing.Size(142, 17)
        Me.lblFinanceNetCommission.TabIndex = 126
        Me.lblFinanceNetCommission.Text = "Finance Net Commission"
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabMainTab_TabPage1.Controls.Add(Me._cmdNext_1)
        Me._tabMainTab_TabPage1.Controls.Add(Me.txtMaxInstalments)
        Me._tabMainTab_TabPage1.Controls.Add(Me.fraCharges)
        Me._tabMainTab_TabPage1.Controls.Add(Me.cboFrequency)
        Me._tabMainTab_TabPage1.Controls.Add(Me.lblMaxInstalments)
        Me._tabMainTab_TabPage1.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(541, 411)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2 - Frequency/Charges"
        Me._tabMainTab_TabPage1.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(8, 388)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_0.TabIndex = 46
        Me._cmdPrevious_0.Text = "&<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        '
        '_cmdNext_1
        '
        Me._cmdNext_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_1.Location = New System.Drawing.Point(496, 388)
        Me._cmdNext_1.Name = "_cmdNext_1"
        Me._cmdNext_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_1.TabIndex = 47
        Me._cmdNext_1.Text = "&>>"
        Me._cmdNext_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_1.UseVisualStyleBackColor = False
        '
        'txtMaxInstalments
        '
        Me.txtMaxInstalments.AcceptsReturn = True
        Me.txtMaxInstalments.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaxInstalments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxInstalments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxInstalments.Location = New System.Drawing.Point(200, 52)
        Me.txtMaxInstalments.MaxLength = 15
        Me.txtMaxInstalments.Name = "txtMaxInstalments"
        Me.txtMaxInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxInstalments.Size = New System.Drawing.Size(89, 20)
        Me.txtMaxInstalments.TabIndex = 24
        '
        'fraCharges
        '
        Me.fraCharges.BackColor = System.Drawing.SystemColors.Control
        Me.fraCharges.Controls.Add(Me.txtR5Com)
        Me.fraCharges.Controls.Add(Me.txtRate5)
        Me.fraCharges.Controls.Add(Me.txtMax5)
        Me.fraCharges.Controls.Add(Me.txtMin5)
        Me.fraCharges.Controls.Add(Me.txtR4Com)
        Me.fraCharges.Controls.Add(Me.txtRate4)
        Me.fraCharges.Controls.Add(Me.txtMax4)
        Me.fraCharges.Controls.Add(Me.txtMin4)
        Me.fraCharges.Controls.Add(Me.txtR3Com)
        Me.fraCharges.Controls.Add(Me.txtRate3)
        Me.fraCharges.Controls.Add(Me.txtMax3)
        Me.fraCharges.Controls.Add(Me.txtMin3)
        Me.fraCharges.Controls.Add(Me.txtR2Com)
        Me.fraCharges.Controls.Add(Me.txtRate2)
        Me.fraCharges.Controls.Add(Me.txtMax2)
        Me.fraCharges.Controls.Add(Me.txtMin2)
        Me.fraCharges.Controls.Add(Me.txtR1Com)
        Me.fraCharges.Controls.Add(Me.txtRate1)
        Me.fraCharges.Controls.Add(Me.txtMax1)
        Me.fraCharges.Controls.Add(Me.txtMin1)
        Me.fraCharges.Controls.Add(Me.txtMinInterest)
        Me.fraCharges.Controls.Add(Me.lblMinInterest)
        Me.fraCharges.Controls.Add(Me.lbl5)
        Me.fraCharges.Controls.Add(Me.lbl4)
        Me.fraCharges.Controls.Add(Me.lbl3)
        Me.fraCharges.Controls.Add(Me.lbl2)
        Me.fraCharges.Controls.Add(Me.lbl1)
        Me.fraCharges.Controls.Add(Me.lblCommPC)
        Me.fraCharges.Controls.Add(Me.lblRate)
        Me.fraCharges.Controls.Add(Me.lblMin)
        Me.fraCharges.Controls.Add(Me.lblMax)
        Me.fraCharges.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCharges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCharges.Location = New System.Drawing.Point(16, 92)
        Me.fraCharges.Name = "fraCharges"
        Me.fraCharges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCharges.Size = New System.Drawing.Size(497, 215)
        Me.fraCharges.TabIndex = 90
        Me.fraCharges.TabStop = False
        Me.fraCharges.Text = "Charges"
        '
        'txtR5Com
        '
        Me.txtR5Com.AcceptsReturn = True
        Me.txtR5Com.BackColor = System.Drawing.SystemColors.Window
        Me.txtR5Com.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtR5Com.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtR5Com.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtR5Com.Location = New System.Drawing.Point(384, 136)
        Me.txtR5Com.MaxLength = 0
        Me.txtR5Com.Name = "txtR5Com"
        Me.txtR5Com.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtR5Com.Size = New System.Drawing.Size(97, 20)
        Me.txtR5Com.TabIndex = 44
        Me.txtR5Com.Text = " "
        Me.txtR5Com.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRate5
        '
        Me.txtRate5.AcceptsReturn = True
        Me.txtRate5.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate5.Location = New System.Drawing.Point(272, 136)
        Me.txtRate5.MaxLength = 0
        Me.txtRate5.Name = "txtRate5"
        Me.txtRate5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate5.Size = New System.Drawing.Size(97, 20)
        Me.txtRate5.TabIndex = 43
        Me.txtRate5.Text = " "
        Me.txtRate5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMax5
        '
        Me.txtMax5.AcceptsReturn = True
        Me.txtMax5.BackColor = System.Drawing.SystemColors.Window
        Me.txtMax5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMax5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMax5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMax5.Location = New System.Drawing.Point(152, 136)
        Me.txtMax5.MaxLength = 0
        Me.txtMax5.Name = "txtMax5"
        Me.txtMax5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMax5.Size = New System.Drawing.Size(105, 20)
        Me.txtMax5.TabIndex = 42
        Me.txtMax5.Text = " "
        Me.txtMax5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMin5
        '
        Me.txtMin5.AcceptsReturn = True
        Me.txtMin5.BackColor = System.Drawing.SystemColors.Window
        Me.txtMin5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMin5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMin5.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMin5.Location = New System.Drawing.Point(32, 136)
        Me.txtMin5.MaxLength = 0
        Me.txtMin5.Name = "txtMin5"
        Me.txtMin5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMin5.Size = New System.Drawing.Size(105, 20)
        Me.txtMin5.TabIndex = 41
        Me.txtMin5.Text = " "
        Me.txtMin5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtR4Com
        '
        Me.txtR4Com.AcceptsReturn = True
        Me.txtR4Com.BackColor = System.Drawing.SystemColors.Window
        Me.txtR4Com.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtR4Com.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtR4Com.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtR4Com.Location = New System.Drawing.Point(384, 112)
        Me.txtR4Com.MaxLength = 0
        Me.txtR4Com.Name = "txtR4Com"
        Me.txtR4Com.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtR4Com.Size = New System.Drawing.Size(97, 20)
        Me.txtR4Com.TabIndex = 40
        Me.txtR4Com.Text = " "
        Me.txtR4Com.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRate4
        '
        Me.txtRate4.AcceptsReturn = True
        Me.txtRate4.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate4.Location = New System.Drawing.Point(272, 112)
        Me.txtRate4.MaxLength = 0
        Me.txtRate4.Name = "txtRate4"
        Me.txtRate4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate4.Size = New System.Drawing.Size(97, 20)
        Me.txtRate4.TabIndex = 39
        Me.txtRate4.Text = " "
        Me.txtRate4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMax4
        '
        Me.txtMax4.AcceptsReturn = True
        Me.txtMax4.BackColor = System.Drawing.SystemColors.Window
        Me.txtMax4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMax4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMax4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMax4.Location = New System.Drawing.Point(152, 112)
        Me.txtMax4.MaxLength = 0
        Me.txtMax4.Name = "txtMax4"
        Me.txtMax4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMax4.Size = New System.Drawing.Size(105, 20)
        Me.txtMax4.TabIndex = 38
        Me.txtMax4.Text = " "
        Me.txtMax4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMin4
        '
        Me.txtMin4.AcceptsReturn = True
        Me.txtMin4.BackColor = System.Drawing.SystemColors.Window
        Me.txtMin4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMin4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMin4.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMin4.Location = New System.Drawing.Point(32, 112)
        Me.txtMin4.MaxLength = 0
        Me.txtMin4.Name = "txtMin4"
        Me.txtMin4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMin4.Size = New System.Drawing.Size(105, 20)
        Me.txtMin4.TabIndex = 37
        Me.txtMin4.Text = " "
        Me.txtMin4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtR3Com
        '
        Me.txtR3Com.AcceptsReturn = True
        Me.txtR3Com.BackColor = System.Drawing.SystemColors.Window
        Me.txtR3Com.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtR3Com.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtR3Com.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtR3Com.Location = New System.Drawing.Point(384, 88)
        Me.txtR3Com.MaxLength = 0
        Me.txtR3Com.Name = "txtR3Com"
        Me.txtR3Com.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtR3Com.Size = New System.Drawing.Size(97, 20)
        Me.txtR3Com.TabIndex = 36
        Me.txtR3Com.Text = " "
        Me.txtR3Com.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRate3
        '
        Me.txtRate3.AcceptsReturn = True
        Me.txtRate3.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate3.Location = New System.Drawing.Point(272, 88)
        Me.txtRate3.MaxLength = 0
        Me.txtRate3.Name = "txtRate3"
        Me.txtRate3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate3.Size = New System.Drawing.Size(97, 20)
        Me.txtRate3.TabIndex = 35
        Me.txtRate3.Text = " "
        Me.txtRate3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMax3
        '
        Me.txtMax3.AcceptsReturn = True
        Me.txtMax3.BackColor = System.Drawing.SystemColors.Window
        Me.txtMax3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMax3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMax3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMax3.Location = New System.Drawing.Point(152, 88)
        Me.txtMax3.MaxLength = 0
        Me.txtMax3.Name = "txtMax3"
        Me.txtMax3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMax3.Size = New System.Drawing.Size(105, 20)
        Me.txtMax3.TabIndex = 34
        Me.txtMax3.Text = " "
        Me.txtMax3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMin3
        '
        Me.txtMin3.AcceptsReturn = True
        Me.txtMin3.BackColor = System.Drawing.SystemColors.Window
        Me.txtMin3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMin3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMin3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMin3.Location = New System.Drawing.Point(32, 88)
        Me.txtMin3.MaxLength = 0
        Me.txtMin3.Name = "txtMin3"
        Me.txtMin3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMin3.Size = New System.Drawing.Size(105, 20)
        Me.txtMin3.TabIndex = 33
        Me.txtMin3.Text = " "
        Me.txtMin3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtR2Com
        '
        Me.txtR2Com.AcceptsReturn = True
        Me.txtR2Com.BackColor = System.Drawing.SystemColors.Window
        Me.txtR2Com.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtR2Com.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtR2Com.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtR2Com.Location = New System.Drawing.Point(384, 64)
        Me.txtR2Com.MaxLength = 0
        Me.txtR2Com.Name = "txtR2Com"
        Me.txtR2Com.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtR2Com.Size = New System.Drawing.Size(97, 20)
        Me.txtR2Com.TabIndex = 32
        Me.txtR2Com.Text = " "
        Me.txtR2Com.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRate2
        '
        Me.txtRate2.AcceptsReturn = True
        Me.txtRate2.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate2.Location = New System.Drawing.Point(272, 64)
        Me.txtRate2.MaxLength = 0
        Me.txtRate2.Name = "txtRate2"
        Me.txtRate2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate2.Size = New System.Drawing.Size(97, 20)
        Me.txtRate2.TabIndex = 31
        Me.txtRate2.Text = " "
        Me.txtRate2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMax2
        '
        Me.txtMax2.AcceptsReturn = True
        Me.txtMax2.BackColor = System.Drawing.SystemColors.Window
        Me.txtMax2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMax2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMax2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMax2.Location = New System.Drawing.Point(152, 64)
        Me.txtMax2.MaxLength = 0
        Me.txtMax2.Name = "txtMax2"
        Me.txtMax2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMax2.Size = New System.Drawing.Size(105, 20)
        Me.txtMax2.TabIndex = 30
        Me.txtMax2.Text = " "
        Me.txtMax2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMin2
        '
        Me.txtMin2.AcceptsReturn = True
        Me.txtMin2.BackColor = System.Drawing.SystemColors.Window
        Me.txtMin2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMin2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMin2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMin2.Location = New System.Drawing.Point(32, 64)
        Me.txtMin2.MaxLength = 0
        Me.txtMin2.Name = "txtMin2"
        Me.txtMin2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMin2.Size = New System.Drawing.Size(105, 20)
        Me.txtMin2.TabIndex = 29
        Me.txtMin2.Text = " "
        Me.txtMin2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtR1Com
        '
        Me.txtR1Com.AcceptsReturn = True
        Me.txtR1Com.BackColor = System.Drawing.SystemColors.Window
        Me.txtR1Com.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtR1Com.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtR1Com.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtR1Com.Location = New System.Drawing.Point(384, 40)
        Me.txtR1Com.MaxLength = 0
        Me.txtR1Com.Name = "txtR1Com"
        Me.txtR1Com.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtR1Com.Size = New System.Drawing.Size(97, 20)
        Me.txtR1Com.TabIndex = 28
        Me.txtR1Com.Text = " "
        Me.txtR1Com.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRate1
        '
        Me.txtRate1.AcceptsReturn = True
        Me.txtRate1.BackColor = System.Drawing.SystemColors.Window
        Me.txtRate1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRate1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRate1.Location = New System.Drawing.Point(272, 40)
        Me.txtRate1.MaxLength = 0
        Me.txtRate1.Name = "txtRate1"
        Me.txtRate1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRate1.Size = New System.Drawing.Size(97, 20)
        Me.txtRate1.TabIndex = 27
        Me.txtRate1.Text = " "
        Me.txtRate1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMax1
        '
        Me.txtMax1.AcceptsReturn = True
        Me.txtMax1.BackColor = System.Drawing.SystemColors.Window
        Me.txtMax1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMax1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMax1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMax1.Location = New System.Drawing.Point(152, 40)
        Me.txtMax1.MaxLength = 0
        Me.txtMax1.Name = "txtMax1"
        Me.txtMax1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMax1.Size = New System.Drawing.Size(105, 20)
        Me.txtMax1.TabIndex = 26
        Me.txtMax1.Text = " "
        Me.txtMax1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMin1
        '
        Me.txtMin1.AcceptsReturn = True
        Me.txtMin1.BackColor = System.Drawing.SystemColors.Window
        Me.txtMin1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMin1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMin1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMin1.Location = New System.Drawing.Point(32, 40)
        Me.txtMin1.MaxLength = 0
        Me.txtMin1.Name = "txtMin1"
        Me.txtMin1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMin1.Size = New System.Drawing.Size(105, 20)
        Me.txtMin1.TabIndex = 25
        Me.txtMin1.Text = " "
        Me.txtMin1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMinInterest
        '
        Me.txtMinInterest.AcceptsReturn = True
        Me.txtMinInterest.BackColor = System.Drawing.SystemColors.Window
        Me.txtMinInterest.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinInterest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinInterest.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMinInterest.Location = New System.Drawing.Point(152, 176)
        Me.txtMinInterest.MaxLength = 0
        Me.txtMinInterest.Name = "txtMinInterest"
        Me.txtMinInterest.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinInterest.Size = New System.Drawing.Size(105, 20)
        Me.txtMinInterest.TabIndex = 45
        Me.txtMinInterest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMinInterest
        '
        Me.lblMinInterest.BackColor = System.Drawing.SystemColors.Control
        Me.lblMinInterest.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinInterest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinInterest.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMinInterest.Location = New System.Drawing.Point(32, 176)
        Me.lblMinInterest.Name = "lblMinInterest"
        Me.lblMinInterest.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMinInterest.Size = New System.Drawing.Size(89, 17)
        Me.lblMinInterest.TabIndex = 100
        Me.lblMinInterest.Text = "&Min. Interest:"
        '
        'lbl5
        '
        Me.lbl5.BackColor = System.Drawing.SystemColors.Control
        Me.lbl5.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl5.Location = New System.Drawing.Point(16, 136)
        Me.lbl5.Name = "lbl5"
        Me.lbl5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl5.Size = New System.Drawing.Size(17, 17)
        Me.lbl5.TabIndex = 99
        Me.lbl5.Text = "5."
        '
        'lbl4
        '
        Me.lbl4.BackColor = System.Drawing.SystemColors.Control
        Me.lbl4.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl4.Location = New System.Drawing.Point(16, 112)
        Me.lbl4.Name = "lbl4"
        Me.lbl4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl4.Size = New System.Drawing.Size(17, 17)
        Me.lbl4.TabIndex = 98
        Me.lbl4.Text = "4."
        '
        'lbl3
        '
        Me.lbl3.BackColor = System.Drawing.SystemColors.Control
        Me.lbl3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl3.Location = New System.Drawing.Point(16, 88)
        Me.lbl3.Name = "lbl3"
        Me.lbl3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl3.Size = New System.Drawing.Size(17, 17)
        Me.lbl3.TabIndex = 97
        Me.lbl3.Text = "3."
        '
        'lbl2
        '
        Me.lbl2.BackColor = System.Drawing.SystemColors.Control
        Me.lbl2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl2.Location = New System.Drawing.Point(16, 64)
        Me.lbl2.Name = "lbl2"
        Me.lbl2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl2.Size = New System.Drawing.Size(17, 17)
        Me.lbl2.TabIndex = 96
        Me.lbl2.Text = "2."
        '
        'lbl1
        '
        Me.lbl1.BackColor = System.Drawing.SystemColors.Control
        Me.lbl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl1.Location = New System.Drawing.Point(16, 40)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbl1.Size = New System.Drawing.Size(17, 17)
        Me.lbl1.TabIndex = 95
        Me.lbl1.Text = "1."
        '
        'lblCommPC
        '
        Me.lblCommPC.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommPC.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommPC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommPC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommPC.Location = New System.Drawing.Point(384, 24)
        Me.lblCommPC.Name = "lblCommPC"
        Me.lblCommPC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommPC.Size = New System.Drawing.Size(65, 17)
        Me.lblCommPC.TabIndex = 94
        Me.lblCommPC.Text = "Comm  %"
        '
        'lblRate
        '
        Me.lblRate.BackColor = System.Drawing.SystemColors.Control
        Me.lblRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRate.Location = New System.Drawing.Point(272, 24)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRate.Size = New System.Drawing.Size(41, 17)
        Me.lblRate.TabIndex = 93
        Me.lblRate.Text = "Rate"
        '
        'lblMin
        '
        Me.lblMin.BackColor = System.Drawing.SystemColors.Control
        Me.lblMin.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMin.Location = New System.Drawing.Point(32, 24)
        Me.lblMin.Name = "lblMin"
        Me.lblMin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMin.Size = New System.Drawing.Size(57, 17)
        Me.lblMin.TabIndex = 92
        Me.lblMin.Text = "Min  Amt"
        '
        'lblMax
        '
        Me.lblMax.BackColor = System.Drawing.SystemColors.Control
        Me.lblMax.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMax.Location = New System.Drawing.Point(152, 24)
        Me.lblMax.Name = "lblMax"
        Me.lblMax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMax.Size = New System.Drawing.Size(57, 17)
        Me.lblMax.TabIndex = 91
        Me.lblMax.Text = "Max Amt"
        '
        'cboFrequency
        '
        Me.cboFrequency.BackColor = System.Drawing.SystemColors.Window
        Me.cboFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFrequency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboFrequency.Location = New System.Drawing.Point(200, 20)
        Me.cboFrequency.Name = "cboFrequency"
        Me.cboFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboFrequency.Size = New System.Drawing.Size(161, 21)
        Me.cboFrequency.TabIndex = 23
        Me.cboFrequency.Tag = "IH;"
        '
        'lblMaxInstalments
        '
        Me.lblMaxInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaxInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaxInstalments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaxInstalments.Location = New System.Drawing.Point(16, 52)
        Me.lblMaxInstalments.Name = "lblMaxInstalments"
        Me.lblMaxInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaxInstalments.Size = New System.Drawing.Size(169, 17)
        Me.lblMaxInstalments.TabIndex = 101
        Me.lblMaxInstalments.Text = "Maximum Instalments:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(80, 13)
        Me.Label1.TabIndex = 55
        Me.Label1.Text = "Frequency:"
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdPrevious_1)
        Me._tabMainTab_TabPage2.Controls.Add(Me._cmdNext_2)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraMTA)
        Me._tabMainTab_TabPage2.Controls.Add(Me.framRecollection)
        Me._tabMainTab_TabPage2.Controls.Add(Me.Frame3)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraAlign)
        Me._tabMainTab_TabPage2.Controls.Add(Me.fraBackDated)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(541, 411)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3 - Rules"
        Me._tabMainTab_TabPage2.UseVisualStyleBackColor = True
        '
        '_cmdPrevious_1
        '
        Me._cmdPrevious_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_1.Location = New System.Drawing.Point(8, 388)
        Me._cmdPrevious_1.Name = "_cmdPrevious_1"
        Me._cmdPrevious_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_1.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_1.TabIndex = 66
        Me._cmdPrevious_1.Text = "&<<"
        Me._cmdPrevious_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_1.UseVisualStyleBackColor = False
        '
        '_cmdNext_2
        '
        Me._cmdNext_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_2.Location = New System.Drawing.Point(496, 388)
        Me._cmdNext_2.Name = "_cmdNext_2"
        Me._cmdNext_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdNext_2.TabIndex = 74
        Me._cmdNext_2.Text = "&>>"
        Me._cmdNext_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_2.UseVisualStyleBackColor = False
        '
        'fraMTA
        '
        Me.fraMTA.BackColor = System.Drawing.SystemColors.Control
        Me.fraMTA.Controls.Add(Me.txtMinMTAInstalments)
        Me.fraMTA.Controls.Add(Me.txtMinMTA)
        Me.fraMTA.Controls.Add(Me.chkAllowMTA)
        Me.fraMTA.Controls.Add(Me.lblMinMTAInstalments)
        Me.fraMTA.Controls.Add(Me.lblMinMTA)
        Me.fraMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraMTA.Location = New System.Drawing.Point(8, 306)
        Me.fraMTA.Name = "fraMTA"
        Me.fraMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraMTA.Size = New System.Drawing.Size(521, 79)
        Me.fraMTA.TabIndex = 102
        Me.fraMTA.TabStop = False
        Me.fraMTA.Text = "MTA"
        '
        'txtMinMTAInstalments
        '
        Me.txtMinMTAInstalments.AcceptsReturn = True
        Me.txtMinMTAInstalments.BackColor = System.Drawing.SystemColors.Window
        Me.txtMinMTAInstalments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinMTAInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinMTAInstalments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMinMTAInstalments.Location = New System.Drawing.Point(436, 46)
        Me.txtMinMTAInstalments.MaxLength = 0
        Me.txtMinMTAInstalments.Name = "txtMinMTAInstalments"
        Me.txtMinMTAInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinMTAInstalments.Size = New System.Drawing.Size(65, 20)
        Me.txtMinMTAInstalments.TabIndex = 65
        Me.txtMinMTAInstalments.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMinMTA
        '
        Me.txtMinMTA.AcceptsReturn = True
        Me.txtMinMTA.BackColor = System.Drawing.SystemColors.Window
        Me.txtMinMTA.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinMTA.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMinMTA.Location = New System.Drawing.Point(80, 48)
        Me.txtMinMTA.MaxLength = 0
        Me.txtMinMTA.Name = "txtMinMTA"
        Me.txtMinMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinMTA.Size = New System.Drawing.Size(73, 20)
        Me.txtMinMTA.TabIndex = 64
        Me.txtMinMTA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkAllowMTA
        '
        Me.chkAllowMTA.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowMTA.Location = New System.Drawing.Point(16, 24)
        Me.chkAllowMTA.Name = "chkAllowMTA"
        Me.chkAllowMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowMTA.Size = New System.Drawing.Size(217, 17)
        Me.chkAllowMTA.TabIndex = 63
        Me.chkAllowMTA.Text = "Allow MTA On Next Instalment"
        Me.chkAllowMTA.UseVisualStyleBackColor = False
        '
        'lblMinMTAInstalments
        '
        Me.lblMinMTAInstalments.BackColor = System.Drawing.SystemColors.Control
        Me.lblMinMTAInstalments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinMTAInstalments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinMTAInstalments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMinMTAInstalments.Location = New System.Drawing.Point(250, 48)
        Me.lblMinMTAInstalments.Name = "lblMinMTAInstalments"
        Me.lblMinMTAInstalments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMinMTAInstalments.Size = New System.Drawing.Size(177, 17)
        Me.lblMinMTAInstalments.TabIndex = 104
        Me.lblMinMTAInstalments.Text = "&Min MTA Instalments:"
        Me.lblMinMTAInstalments.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblMinMTA
        '
        Me.lblMinMTA.BackColor = System.Drawing.SystemColors.Control
        Me.lblMinMTA.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinMTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinMTA.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMinMTA.Location = New System.Drawing.Point(16, 48)
        Me.lblMinMTA.Name = "lblMinMTA"
        Me.lblMinMTA.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMinMTA.Size = New System.Drawing.Size(81, 17)
        Me.lblMinMTA.TabIndex = 103
        Me.lblMinMTA.Text = "&Min MTA:"
        '
        'framRecollection
        '
        Me.framRecollection.BackColor = System.Drawing.SystemColors.Control
        Me.framRecollection.Controls.Add(Me.chkOnNextInstalmentDate)
        Me.framRecollection.Controls.Add(Me.txtDaysLater)
        Me.framRecollection.Controls.Add(Me.txtRetryLimit)
        Me.framRecollection.Controls.Add(Me.Label6)
        Me.framRecollection.Controls.Add(Me.lblRetryLimit)
        Me.framRecollection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.framRecollection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.framRecollection.Location = New System.Drawing.Point(8, 244)
        Me.framRecollection.Name = "framRecollection"
        Me.framRecollection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.framRecollection.Size = New System.Drawing.Size(521, 57)
        Me.framRecollection.TabIndex = 81
        Me.framRecollection.TabStop = False
        Me.framRecollection.Text = "Recollection of Failed Instalment"
        '
        'chkOnNextInstalmentDate
        '
        Me.chkOnNextInstalmentDate.BackColor = System.Drawing.SystemColors.Control
        Me.chkOnNextInstalmentDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOnNextInstalmentDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOnNextInstalmentDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOnNextInstalmentDate.Location = New System.Drawing.Point(16, 24)
        Me.chkOnNextInstalmentDate.Name = "chkOnNextInstalmentDate"
        Me.chkOnNextInstalmentDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOnNextInstalmentDate.Size = New System.Drawing.Size(177, 17)
        Me.chkOnNextInstalmentDate.TabIndex = 58
        Me.chkOnNextInstalmentDate.Text = "On Next Instalment Date"
        Me.chkOnNextInstalmentDate.UseVisualStyleBackColor = False
        '
        'txtDaysLater
        '
        Me.txtDaysLater.AcceptsReturn = True
        Me.txtDaysLater.BackColor = System.Drawing.SystemColors.Window
        Me.txtDaysLater.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDaysLater.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDaysLater.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDaysLater.Location = New System.Drawing.Point(208, 24)
        Me.txtDaysLater.MaxLength = 0
        Me.txtDaysLater.Name = "txtDaysLater"
        Me.txtDaysLater.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDaysLater.Size = New System.Drawing.Size(57, 20)
        Me.txtDaysLater.TabIndex = 60
        '
        'txtRetryLimit
        '
        Me.txtRetryLimit.AcceptsReturn = True
        Me.txtRetryLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtRetryLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRetryLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRetryLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRetryLimit.Location = New System.Drawing.Point(448, 24)
        Me.txtRetryLimit.MaxLength = 0
        Me.txtRetryLimit.Name = "txtRetryLimit"
        Me.txtRetryLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRetryLimit.Size = New System.Drawing.Size(65, 20)
        Me.txtRetryLimit.TabIndex = 62
        Me.txtRetryLimit.Text = "0"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(280, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(58, 13)
        Me.Label6.TabIndex = 83
        Me.Label6.Text = "Days Later"
        '
        'lblRetryLimit
        '
        Me.lblRetryLimit.AutoSize = True
        Me.lblRetryLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblRetryLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRetryLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRetryLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRetryLimit.Location = New System.Drawing.Point(368, 24)
        Me.lblRetryLimit.Name = "lblRetryLimit"
        Me.lblRetryLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRetryLimit.Size = New System.Drawing.Size(59, 13)
        Me.lblRetryLimit.TabIndex = 82
        Me.lblRetryLimit.Text = "Retry Limit:"
        '
        'Frame3
        '
        Me.Frame3.BackColor = System.Drawing.SystemColors.Control
        Me.Frame3.Controls.Add(Me.chkFirstInstalmentAlignWithDayInMonth)
        Me.Frame3.Controls.Add(Me.txtExistingDaysDelay)
        Me.Frame3.Controls.Add(Me.txtFirstInstalmentTo)
        Me.Frame3.Controls.Add(Me.txtFirstInstalmentFrom)
        Me.Frame3.Controls.Add(Me.lblExistingDaysDelay)
        Me.Frame3.Controls.Add(Me.lblDaysAfterInception)
        Me.Frame3.Controls.Add(Me.lblFirstInstalmentTo)
        Me.Frame3.Controls.Add(Me.lblFirstInstalmentFrom)
        Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame3.Location = New System.Drawing.Point(8, 148)
        Me.Frame3.Name = "Frame3"
        Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame3.Size = New System.Drawing.Size(521, 89)
        Me.Frame3.TabIndex = 61
        Me.Frame3.TabStop = False
        Me.Frame3.Text = "First Instalment Adjustment"
        '
        'chkFirstInstalmentAlignWithDayInMonth
        '
        Me.chkFirstInstalmentAlignWithDayInMonth.BackColor = System.Drawing.SystemColors.Control
        Me.chkFirstInstalmentAlignWithDayInMonth.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkFirstInstalmentAlignWithDayInMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFirstInstalmentAlignWithDayInMonth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkFirstInstalmentAlignWithDayInMonth.Location = New System.Drawing.Point(283, 58)
        Me.chkFirstInstalmentAlignWithDayInMonth.Name = "chkFirstInstalmentAlignWithDayInMonth"
        Me.chkFirstInstalmentAlignWithDayInMonth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkFirstInstalmentAlignWithDayInMonth.Size = New System.Drawing.Size(230, 18)
        Me.chkFirstInstalmentAlignWithDayInMonth.TabIndex = 85
        Me.chkFirstInstalmentAlignWithDayInMonth.Text = "First Instalment Align with Day in Month"
        Me.chkFirstInstalmentAlignWithDayInMonth.UseVisualStyleBackColor = False
        '
        'txtExistingDaysDelay
        '
        Me.txtExistingDaysDelay.AcceptsReturn = True
        Me.txtExistingDaysDelay.BackColor = System.Drawing.SystemColors.Window
        Me.txtExistingDaysDelay.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExistingDaysDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExistingDaysDelay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExistingDaysDelay.Location = New System.Drawing.Point(208, 56)
        Me.txtExistingDaysDelay.MaxLength = 0
        Me.txtExistingDaysDelay.Name = "txtExistingDaysDelay"
        Me.txtExistingDaysDelay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExistingDaysDelay.Size = New System.Drawing.Size(57, 20)
        Me.txtExistingDaysDelay.TabIndex = 56
        '
        'txtFirstInstalmentTo
        '
        Me.txtFirstInstalmentTo.AcceptsReturn = True
        Me.txtFirstInstalmentTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtFirstInstalmentTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFirstInstalmentTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFirstInstalmentTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFirstInstalmentTo.Location = New System.Drawing.Point(208, 24)
        Me.txtFirstInstalmentTo.MaxLength = 0
        Me.txtFirstInstalmentTo.Name = "txtFirstInstalmentTo"
        Me.txtFirstInstalmentTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFirstInstalmentTo.Size = New System.Drawing.Size(57, 20)
        Me.txtFirstInstalmentTo.TabIndex = 54
        '
        'txtFirstInstalmentFrom
        '
        Me.txtFirstInstalmentFrom.AcceptsReturn = True
        Me.txtFirstInstalmentFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtFirstInstalmentFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFirstInstalmentFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFirstInstalmentFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFirstInstalmentFrom.Location = New System.Drawing.Point(80, 24)
        Me.txtFirstInstalmentFrom.MaxLength = 0
        Me.txtFirstInstalmentFrom.Name = "txtFirstInstalmentFrom"
        Me.txtFirstInstalmentFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFirstInstalmentFrom.Size = New System.Drawing.Size(57, 20)
        Me.txtFirstInstalmentFrom.TabIndex = 52
        '
        'lblExistingDaysDelay
        '
        Me.lblExistingDaysDelay.AutoSize = True
        Me.lblExistingDaysDelay.BackColor = System.Drawing.SystemColors.Control
        Me.lblExistingDaysDelay.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExistingDaysDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExistingDaysDelay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExistingDaysDelay.Location = New System.Drawing.Point(16, 56)
        Me.lblExistingDaysDelay.Name = "lblExistingDaysDelay"
        Me.lblExistingDaysDelay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExistingDaysDelay.Size = New System.Drawing.Size(157, 13)
        Me.lblExistingDaysDelay.TabIndex = 84
        Me.lblExistingDaysDelay.Text = "Existing Agreement Days Delay:"
        '
        'lblDaysAfterInception
        '
        Me.lblDaysAfterInception.AutoSize = True
        Me.lblDaysAfterInception.BackColor = System.Drawing.SystemColors.Control
        Me.lblDaysAfterInception.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDaysAfterInception.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDaysAfterInception.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDaysAfterInception.Location = New System.Drawing.Point(280, 24)
        Me.lblDaysAfterInception.Name = "lblDaysAfterInception"
        Me.lblDaysAfterInception.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDaysAfterInception.Size = New System.Drawing.Size(103, 13)
        Me.lblDaysAfterInception.TabIndex = 80
        Me.lblDaysAfterInception.Text = "Days After Inception"
        '
        'lblFirstInstalmentTo
        '
        Me.lblFirstInstalmentTo.AutoSize = True
        Me.lblFirstInstalmentTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblFirstInstalmentTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFirstInstalmentTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirstInstalmentTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFirstInstalmentTo.Location = New System.Drawing.Point(160, 24)
        Me.lblFirstInstalmentTo.Name = "lblFirstInstalmentTo"
        Me.lblFirstInstalmentTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFirstInstalmentTo.Size = New System.Drawing.Size(25, 13)
        Me.lblFirstInstalmentTo.TabIndex = 78
        Me.lblFirstInstalmentTo.Text = "and"
        '
        'lblFirstInstalmentFrom
        '
        Me.lblFirstInstalmentFrom.AutoSize = True
        Me.lblFirstInstalmentFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblFirstInstalmentFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFirstInstalmentFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirstInstalmentFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFirstInstalmentFrom.Location = New System.Drawing.Point(16, 24)
        Me.lblFirstInstalmentFrom.Name = "lblFirstInstalmentFrom"
        Me.lblFirstInstalmentFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFirstInstalmentFrom.Size = New System.Drawing.Size(49, 13)
        Me.lblFirstInstalmentFrom.TabIndex = 76
        Me.lblFirstInstalmentFrom.Text = "Between"
        '
        'fraAlign
        '
        Me.fraAlign.BackColor = System.Drawing.SystemColors.Control
        Me.fraAlign.Controls.Add(Me.chkSingleInstalmentPerMonth)
        Me.fraAlign.Controls.Add(Me._optAlign_1)
        Me.fraAlign.Controls.Add(Me._optAlign_0)
        Me.fraAlign.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAlign.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAlign.Location = New System.Drawing.Point(8, 74)
        Me.fraAlign.Name = "fraAlign"
        Me.fraAlign.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAlign.Size = New System.Drawing.Size(521, 68)
        Me.fraAlign.TabIndex = 59
        Me.fraAlign.TabStop = False
        Me.fraAlign.Text = "Align Instalments"
        '
        'chkSingleInstalmentPerMonth
        '
        Me.chkSingleInstalmentPerMonth.BackColor = System.Drawing.SystemColors.Control
        Me.chkSingleInstalmentPerMonth.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSingleInstalmentPerMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSingleInstalmentPerMonth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSingleInstalmentPerMonth.Location = New System.Drawing.Point(16, 45)
        Me.chkSingleInstalmentPerMonth.Name = "chkSingleInstalmentPerMonth"
        Me.chkSingleInstalmentPerMonth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSingleInstalmentPerMonth.Size = New System.Drawing.Size(177, 17)
        Me.chkSingleInstalmentPerMonth.TabIndex = 59
        Me.chkSingleInstalmentPerMonth.Text = "Single Instalment Per Month"
        Me.chkSingleInstalmentPerMonth.UseVisualStyleBackColor = False
        '
        '_optAlign_1
        '
        Me._optAlign_1.BackColor = System.Drawing.SystemColors.Control
        Me._optAlign_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAlign_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAlign_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAlign_1.Location = New System.Drawing.Point(256, 16)
        Me._optAlign_1.Name = "_optAlign_1"
        Me._optAlign_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAlign_1.Size = New System.Drawing.Size(233, 27)
        Me._optAlign_1.TabIndex = 51
        Me._optAlign_1.TabStop = True
        Me._optAlign_1.Text = "With the Policy Renewal Date"
        Me._optAlign_1.UseVisualStyleBackColor = False
        '
        '_optAlign_0
        '
        Me._optAlign_0.BackColor = System.Drawing.SystemColors.Control
        Me._optAlign_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAlign_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAlign_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAlign_0.Location = New System.Drawing.Point(16, 18)
        Me._optAlign_0.Name = "_optAlign_0"
        Me._optAlign_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAlign_0.Size = New System.Drawing.Size(233, 23)
        Me._optAlign_0.TabIndex = 50
        Me._optAlign_0.TabStop = True
        Me._optAlign_0.Text = "With the Customers Preference"
        Me._optAlign_0.UseVisualStyleBackColor = False
        '
        'fraBackDated
        '
        Me.fraBackDated.BackColor = System.Drawing.SystemColors.Control
        Me.fraBackDated.Controls.Add(Me._optBackDated_1)
        Me.fraBackDated.Controls.Add(Me._optBackDated_0)
        Me.fraBackDated.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBackDated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBackDated.Location = New System.Drawing.Point(8, 12)
        Me.fraBackDated.Name = "fraBackDated"
        Me.fraBackDated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBackDated.Size = New System.Drawing.Size(521, 57)
        Me.fraBackDated.TabIndex = 57
        Me.fraBackDated.TabStop = False
        Me.fraBackDated.Text = "BackDated Instalments"
        '
        '_optBackDated_1
        '
        Me._optBackDated_1.BackColor = System.Drawing.SystemColors.Control
        Me._optBackDated_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBackDated_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBackDated_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBackDated_1.Location = New System.Drawing.Point(256, 24)
        Me._optBackDated_1.Name = "_optBackDated_1"
        Me._optBackDated_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBackDated_1.Size = New System.Drawing.Size(177, 17)
        Me._optBackDated_1.TabIndex = 49
        Me._optBackDated_1.TabStop = True
        Me._optBackDated_1.Text = "Spread Over Plan"
        Me._optBackDated_1.UseVisualStyleBackColor = False
        '
        '_optBackDated_0
        '
        Me._optBackDated_0.BackColor = System.Drawing.SystemColors.Control
        Me._optBackDated_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optBackDated_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optBackDated_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optBackDated_0.Location = New System.Drawing.Point(16, 24)
        Me._optBackDated_0.Name = "_optBackDated_0"
        Me._optBackDated_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optBackDated_0.Size = New System.Drawing.Size(225, 17)
        Me._optBackDated_0.TabIndex = 48
        Me._optBackDated_0.TabStop = True
        Me._optBackDated_0.Text = "Roll-up into First Instalment"
        Me._optBackDated_0.UseVisualStyleBackColor = False
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblStatementFrequ)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblStatementDoc)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblAdvanceInst)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblReviewUserGrp)
        Me._tabMainTab_TabPage3.Controls.Add(Me.lblRemainderThreshhold)
        Me._tabMainTab_TabPage3.Controls.Add(Me.cboStatementFrequ)
        Me._tabMainTab_TabPage3.Controls.Add(Me.cboStatementDoc)
        Me._tabMainTab_TabPage3.Controls.Add(Me.cboReviewUserGrp)
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtAdvanceInst)
        Me._tabMainTab_TabPage3.Controls.Add(Me.txtRemainderThreshhold)
        Me._tabMainTab_TabPage3.Controls.Add(Me.chkRemainderAtEnd)
        Me._tabMainTab_TabPage3.Controls.Add(Me._cmdPrevious_2)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(541, 411)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4 - Third Party Recovery"
        Me._tabMainTab_TabPage3.UseVisualStyleBackColor = True
        '
        'lblStatementFrequ
        '
        Me.lblStatementFrequ.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatementFrequ.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatementFrequ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatementFrequ.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatementFrequ.Location = New System.Drawing.Point(96, 92)
        Me.lblStatementFrequ.Name = "lblStatementFrequ"
        Me.lblStatementFrequ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatementFrequ.Size = New System.Drawing.Size(137, 17)
        Me.lblStatementFrequ.TabIndex = 85
        Me.lblStatementFrequ.Text = "Statement Frequency :"
        Me.lblStatementFrequ.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblStatementDoc
        '
        Me.lblStatementDoc.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatementDoc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatementDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatementDoc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatementDoc.Location = New System.Drawing.Point(96, 124)
        Me.lblStatementDoc.Name = "lblStatementDoc"
        Me.lblStatementDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatementDoc.Size = New System.Drawing.Size(137, 17)
        Me.lblStatementDoc.TabIndex = 86
        Me.lblStatementDoc.Text = "Statement Document :"
        Me.lblStatementDoc.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblAdvanceInst
        '
        Me.lblAdvanceInst.BackColor = System.Drawing.SystemColors.Control
        Me.lblAdvanceInst.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAdvanceInst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdvanceInst.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAdvanceInst.Location = New System.Drawing.Point(96, 156)
        Me.lblAdvanceInst.Name = "lblAdvanceInst"
        Me.lblAdvanceInst.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAdvanceInst.Size = New System.Drawing.Size(137, 17)
        Me.lblAdvanceInst.TabIndex = 87
        Me.lblAdvanceInst.Text = "Advance Instalments :"
        Me.lblAdvanceInst.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblReviewUserGrp
        '
        Me.lblReviewUserGrp.BackColor = System.Drawing.SystemColors.Control
        Me.lblReviewUserGrp.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReviewUserGrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReviewUserGrp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReviewUserGrp.Location = New System.Drawing.Point(104, 188)
        Me.lblReviewUserGrp.Name = "lblReviewUserGrp"
        Me.lblReviewUserGrp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReviewUserGrp.Size = New System.Drawing.Size(129, 17)
        Me.lblReviewUserGrp.TabIndex = 88
        Me.lblReviewUserGrp.Text = "Review User Group :"
        Me.lblReviewUserGrp.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblRemainderThreshhold
        '
        Me.lblRemainderThreshhold.BackColor = System.Drawing.SystemColors.Control
        Me.lblRemainderThreshhold.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRemainderThreshhold.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemainderThreshhold.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRemainderThreshhold.Location = New System.Drawing.Point(40, 220)
        Me.lblRemainderThreshhold.Name = "lblRemainderThreshhold"
        Me.lblRemainderThreshhold.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRemainderThreshhold.Size = New System.Drawing.Size(193, 17)
        Me.lblRemainderThreshhold.TabIndex = 89
        Me.lblRemainderThreshhold.Text = "Remainder Amount Threshhold :"
        Me.lblRemainderThreshhold.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cboStatementFrequ
        '
        Me.cboStatementFrequ.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatementFrequ.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatementFrequ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatementFrequ.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStatementFrequ.Location = New System.Drawing.Point(240, 92)
        Me.cboStatementFrequ.Name = "cboStatementFrequ"
        Me.cboStatementFrequ.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStatementFrequ.Size = New System.Drawing.Size(185, 21)
        Me.cboStatementFrequ.TabIndex = 67
        '
        'cboStatementDoc
        '
        Me.cboStatementDoc.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatementDoc.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatementDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatementDoc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboStatementDoc.Location = New System.Drawing.Point(240, 124)
        Me.cboStatementDoc.Name = "cboStatementDoc"
        Me.cboStatementDoc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStatementDoc.Size = New System.Drawing.Size(265, 21)
        Me.cboStatementDoc.TabIndex = 68
        '
        'cboReviewUserGrp
        '
        Me.cboReviewUserGrp.BackColor = System.Drawing.SystemColors.Window
        Me.cboReviewUserGrp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboReviewUserGrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReviewUserGrp.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboReviewUserGrp.Location = New System.Drawing.Point(240, 188)
        Me.cboReviewUserGrp.Name = "cboReviewUserGrp"
        Me.cboReviewUserGrp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboReviewUserGrp.Size = New System.Drawing.Size(185, 21)
        Me.cboReviewUserGrp.TabIndex = 70
        '
        'txtAdvanceInst
        '
        Me.txtAdvanceInst.AcceptsReturn = True
        Me.txtAdvanceInst.BackColor = System.Drawing.SystemColors.Window
        Me.txtAdvanceInst.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAdvanceInst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvanceInst.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAdvanceInst.Location = New System.Drawing.Point(240, 156)
        Me.txtAdvanceInst.MaxLength = 0
        Me.txtAdvanceInst.Name = "txtAdvanceInst"
        Me.txtAdvanceInst.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAdvanceInst.Size = New System.Drawing.Size(89, 20)
        Me.txtAdvanceInst.TabIndex = 69
        '
        'txtRemainderThreshhold
        '
        Me.txtRemainderThreshhold.AcceptsReturn = True
        Me.txtRemainderThreshhold.BackColor = System.Drawing.SystemColors.Window
        Me.txtRemainderThreshhold.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRemainderThreshhold.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemainderThreshhold.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRemainderThreshhold.Location = New System.Drawing.Point(240, 220)
        Me.txtRemainderThreshhold.MaxLength = 0
        Me.txtRemainderThreshhold.Name = "txtRemainderThreshhold"
        Me.txtRemainderThreshhold.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRemainderThreshhold.Size = New System.Drawing.Size(89, 20)
        Me.txtRemainderThreshhold.TabIndex = 71
        '
        'chkRemainderAtEnd
        '
        Me.chkRemainderAtEnd.BackColor = System.Drawing.SystemColors.Control
        Me.chkRemainderAtEnd.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRemainderAtEnd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRemainderAtEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRemainderAtEnd.Location = New System.Drawing.Point(80, 252)
        Me.chkRemainderAtEnd.Name = "chkRemainderAtEnd"
        Me.chkRemainderAtEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRemainderAtEnd.Size = New System.Drawing.Size(217, 17)
        Me.chkRemainderAtEnd.TabIndex = 72
        Me.chkRemainderAtEnd.Text = "Remainder Amount added to end"
        Me.chkRemainderAtEnd.UseVisualStyleBackColor = False
        '
        '_cmdPrevious_2
        '
        Me._cmdPrevious_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_2.Location = New System.Drawing.Point(8, 388)
        Me._cmdPrevious_2.Name = "_cmdPrevious_2"
        Me._cmdPrevious_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_2.Size = New System.Drawing.Size(38, 19)
        Me._cmdPrevious_2.TabIndex = 73
        Me._cmdPrevious_2.Text = "&<<"
        Me._cmdPrevious_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_2.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(561, 485)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.fraDepositChargedTo.ResumeLayout(False)
        Me.fraFeesChargedTo.ResumeLayout(False)
        Me.Frame11.ResumeLayout(False)
        Me.Frame12.ResumeLayout(False)
        Me.fraDepositType.ResumeLayout(False)
        Me.fraDeposit.ResumeLayout(False)
        Me.fraDeposit.PerformLayout()
        Me.fraFeesType.ResumeLayout(False)
        Me.Frame14.ResumeLayout(False)
        Me.Frame15.ResumeLayout(False)
        Me.fraFees.ResumeLayout(False)
        Me.fraFees.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.fraFinanceNetCommission.ResumeLayout(False)
        Me._tabMainTab_TabPage1.ResumeLayout(False)
        Me._tabMainTab_TabPage1.PerformLayout()
        Me.fraCharges.ResumeLayout(False)
        Me.fraCharges.PerformLayout()
        Me._tabMainTab_TabPage2.ResumeLayout(False)
        Me.fraMTA.ResumeLayout(False)
        Me.fraMTA.PerformLayout()
        Me.framRecollection.ResumeLayout(False)
        Me.framRecollection.PerformLayout()
        Me.Frame3.ResumeLayout(False)
        Me.Frame3.PerformLayout()
        Me.fraAlign.ResumeLayout(False)
        Me.fraBackDated.ResumeLayout(False)
        Me._tabMainTab_TabPage3.ResumeLayout(False)
        Me._tabMainTab_TabPage3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializeoptTaxChargedTo()
        'Me.optTaxChargedTo(1) = _optTaxChargedTo_1
        'Me.optTaxChargedTo(0) = _optTaxChargedTo_0
    End Sub
    Sub InitializeoptProtectionType()
        Me.optProtectionType(1) = _optProtectionType_1
        Me.optProtectionType(0) = _optProtectionType_0
    End Sub
    Sub InitializeoptProtectionChargedTo()
        Me.optProtectionChargedTo(1) = _optProtectionChargedTo_1
        Me.optProtectionChargedTo(0) = _optProtectionChargedTo_0
    End Sub
    Sub InitializeoptIncluded()
        Me.optIncluded(0) = _optIncluded_0
        Me.optIncluded(1) = _optIncluded_1
    End Sub
    Sub InitializeoptFeeType()
        Me.optFeeType(1) = _optFeeType_1
        Me.optFeeType(0) = _optFeeType_0
    End Sub
    Sub InitializeoptFeeChargedTo()
        Me.optFeeChargedTo(1) = _optFeeChargedTo_1
        Me.optFeeChargedTo(0) = _optFeeChargedTo_0
    End Sub
    Sub InitializeoptDepositType()
        Me.optDepositType(0) = _optDepositType_0
        Me.optDepositType(1) = _optDepositType_1
    End Sub
    Sub InitializeoptBackDated()
        Me.optBackDated(1) = _optBackDated_1
        Me.optBackDated(0) = _optBackDated_0
    End Sub
    Sub InitializeoptAlign()
        Me.optAlign(1) = _optAlign_1
        Me.optAlign(0) = _optAlign_0
    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(2) = _cmdPrevious_2
        Me.cmdPrevious(1) = _cmdPrevious_1
        Me.cmdPrevious(0) = _cmdPrevious_0
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(2) = _cmdNext_2
        Me.cmdNext(1) = _cmdNext_1
        Me.cmdNext(0) = _cmdNext_0
    End Sub
    Public WithEvents chkFirstInstalmentAlignWithDayInMonth As System.Windows.Forms.CheckBox
    Public WithEvents chkSingleInstalmentPerMonth As System.Windows.Forms.CheckBox
    Public WithEvents lblOverrideAllowed As System.Windows.Forms.Label
    Public WithEvents chkDepositOverrideAllowed As System.Windows.Forms.CheckBox
    Public WithEvents Frame11 As System.Windows.Forms.Panel
    Private WithEvents _optFeeChargedTo_1 As System.Windows.Forms.RadioButton
    Private WithEvents _optFeeChargedTo_0 As System.Windows.Forms.RadioButton
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents chkApplyFeePercentagesToTaxes As System.Windows.Forms.CheckBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents chkApplyFeePercentagesToFees As System.Windows.Forms.CheckBox
    Public WithEvents Label2 As System.Windows.Forms.Label
#End Region
End Class