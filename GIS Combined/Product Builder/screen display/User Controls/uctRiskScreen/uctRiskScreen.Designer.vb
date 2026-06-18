<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RiskScreen
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializetxtText()
        InitializetxtRate()
        InitializetxtPremium()
        InitializetxtMlText()
        InitializetxtAddress6()
        InitializetxtAddress5()
        InitializetxtAddress4()
        InitializetxtAddress3()
        InitializetxtAddress2()
        InitializetxtAddress1()
        InitializepnlTotalSumInsured()
        InitializepnlPolicyPanel()
        InitializepnlPartyPanel()
        InitializelvwSumInsured()
        InitializelvwStandardWording()
        InitializelvwListView()
        InitializelblTotalSumInsured()
        InitializelblText()
        InitializelblStandardWordingMove()
        InitializelblRate()
        InitializelblPremium()
        InitializelblPMLookup()
        InitializelblMlText()
        InitializelblList()
        InitializelblGISLookup()
        InitializelblComment()
        InitializelblCombo()
        InitializelblCheckLabel()
        InitializefraFrame()
        InitializecmdSumInsuredEdit()
        InitializecmdSumInsuredDelete()
        InitializecmdSumInsuredAdd()
        InitializecmdStandardWordingUp()
        InitializecmdStandardWordingEdit()
        InitializecmdStandardWordingDown()
        InitializecmdStandardWordingDelete()
        InitializecmdStandardWordingAdd()
        InitializecmdPolicyCommand()
        InitializecmdPartyCommand()
        InitializecmdListViewSequenceUp()
        InitializecmdListViewSequenceDown()
        InitializecmdListViewEdit()
        InitializecmdListViewDelete()
        InitializecmdListViewAdd()
        InitializecmdAddress()
        InitializechkYesNo()
        InitializecboPMLookup()
        InitializecboList()
        InitializecboGISLookup()
        InitializePBFindRT1()
        UserControl_InitProperties()
        UserControl_Initialize()
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents uctCLMCaseClaim1 As uctCLMCaseClaimList.uctCLMCaseClaim
    Friend WithEvents uctCLMCaseHeader1 As uctCLMCaseHeaders.uctCLMCaseHeader

    Friend WithEvents _cmdListViewSequenceDown_0 As System.Windows.Forms.Button
    Friend WithEvents _cmdListViewSequenceUp_0 As System.Windows.Forms.Button
    Friend WithEvents _cmdStandardWordingEdit_0 As System.Windows.Forms.Button
    Friend WithEvents _lvwStandardWording_0_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwStandardWording_0_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwStandardWording_0_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwStandardWording_0 As System.Windows.Forms.ListView
    Friend WithEvents _txtAddress6_0 As System.Windows.Forms.TextBox
    Friend WithEvents _cmdListViewDelete_0 As System.Windows.Forms.Button
    Friend WithEvents _cmdListViewEdit_0 As System.Windows.Forms.Button
    Friend WithEvents _cmdListViewAdd_0 As System.Windows.Forms.Button
    Friend WithEvents _fraFrame_0 As System.Windows.Forms.GroupBox
    Friend WithEvents _cmdStandardWordingDelete_0 As System.Windows.Forms.Button
    Friend WithEvents _cmdStandardWordingAdd_0 As System.Windows.Forms.Button
    Friend WithEvents _chkYesNo_0 As System.Windows.Forms.CheckBox
    Friend WithEvents _cmdAddress_0 As System.Windows.Forms.Button
    Friend WithEvents _txtAddress4_0 As System.Windows.Forms.TextBox
    Friend WithEvents _txtAddress3_0 As System.Windows.Forms.TextBox
    Friend WithEvents _txtAddress2_0 As System.Windows.Forms.TextBox
    Friend WithEvents _txtAddress1_0 As System.Windows.Forms.TextBox
    Friend WithEvents _cmdPartyCommand_0 As System.Windows.Forms.Button
    Friend WithEvents _txtRate_0 As System.Windows.Forms.TextBox
    Friend WithEvents _cmdSumInsuredEdit_0 As System.Windows.Forms.Button
    Friend WithEvents _cmdSumInsuredAdd_0 As System.Windows.Forms.Button
    Friend WithEvents _cmdSumInsuredDelete_0 As System.Windows.Forms.Button
    Friend WithEvents _txtPremium_0 As System.Windows.Forms.TextBox
    Friend WithEvents _cmdStandardWordingUp_0 As System.Windows.Forms.Button
    Friend WithEvents _cmdStandardWordingDown_0 As System.Windows.Forms.Button
    Friend WithEvents _txtText_0 As System.Windows.Forms.TextBox
    Friend WithEvents _txtMlText_0 As System.Windows.Forms.TextBox
    Friend WithEvents txtCurrency As System.Windows.Forms.TextBox
    Friend WithEvents txtDate As System.Windows.Forms.TextBox
    Friend WithEvents _cmdPolicyCommand_0 As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Friend WithEvents _lvwListView_0 As System.Windows.Forms.ListView
    Friend WithEvents _lvwSumInsured_0_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSumInsured_0_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSumInsured_0_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSumInsured_0_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSumInsured_0_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSumInsured_0_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSumInsured_0_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwSumInsured_0 As System.Windows.Forms.ListView
    Friend WithEvents _txtAddress5_0 As System.Windows.Forms.TextBox
    Friend WithEvents _TabStrip1_Tab1 As System.Windows.Forms.TabPage
    Friend WithEvents TabStrip1_Tabs As System.Windows.Forms.TabControl.TabPageCollection
    Friend WithEvents TabStrip1 As System.Windows.Forms.TabControl
    Friend WithEvents ImageList2 As System.Windows.Forms.ImageList
    Friend WithEvents _lblStandardWordingMove_0 As System.Windows.Forms.Label
    Friend WithEvents PBForm As System.Windows.Forms.Label
    Friend WithEvents _pnlPartyPanel_0 As System.Windows.Forms.Label
    Friend WithEvents _pnlTotalSumInsured_0 As System.Windows.Forms.Label
    Friend WithEvents _pnlPolicyPanel_0 As System.Windows.Forms.Label
    Friend WithEvents _lblComment_0 As System.Windows.Forms.Label
    Friend WithEvents pnlPosition As System.Windows.Forms.Label
    Friend WithEvents _lblCheckLabel_0 As System.Windows.Forms.Label
    Friend WithEvents _lblCombo_0 As System.Windows.Forms.Label
    Friend WithEvents _lblRate_0 As System.Windows.Forms.Label
    Friend WithEvents _lblMlText_0 As System.Windows.Forms.Label
    Friend WithEvents _lblText_0 As System.Windows.Forms.Label
    Friend WithEvents _lblList_0 As System.Windows.Forms.Label
    Friend WithEvents _lblPremium_0 As System.Windows.Forms.Label
    Friend WithEvents _lblTotalSumInsured_0 As System.Windows.Forms.Label
    Friend WithEvents _lblGISLookup_0 As System.Windows.Forms.Label
    Friend WithEvents _lblPMLookup_0 As System.Windows.Forms.Label
    Friend WithEvents lblAccumulation As System.Windows.Forms.Label
    Friend PBFindRT1(0) As uctPBFindRT.PBFindRT
    Friend cboGISLookup(0) As uctGISUserDefLookupControl.cboGISLookup
    Friend cboList(0) As PMListMgrDropdown.uctDropdown
    Friend cboPMLookup(0) As PMLookupControl.cboPMLookup
    Friend chkYesNo(0) As System.Windows.Forms.CheckBox
    Friend cmdAddress(0) As System.Windows.Forms.Button
    Friend cmdListViewAdd(0) As System.Windows.Forms.Button
    Friend cmdListViewDelete(0) As System.Windows.Forms.Button
    Friend cmdListViewEdit(0) As System.Windows.Forms.Button
    Friend cmdListViewSequenceDown(0) As System.Windows.Forms.Button
    Friend cmdListViewSequenceUp(0) As System.Windows.Forms.Button
    Friend cmdPartyCommand(0) As System.Windows.Forms.Button
    Friend cmdPolicyCommand(0) As System.Windows.Forms.Button
    Friend cmdStandardWordingAdd(0) As System.Windows.Forms.Button
    Friend cmdStandardWordingDelete(0) As System.Windows.Forms.Button
    Friend cmdStandardWordingDown(0) As System.Windows.Forms.Button
    Friend cmdStandardWordingEdit(0) As System.Windows.Forms.Button
    Friend cmdStandardWordingUp(0) As System.Windows.Forms.Button
    Friend cmdSumInsuredAdd(0) As System.Windows.Forms.Button
    Friend cmdSumInsuredDelete(0) As System.Windows.Forms.Button
    Friend cmdSumInsuredEdit(0) As System.Windows.Forms.Button
    Friend fraFrame(0) As System.Windows.Forms.GroupBox
    Friend lblCheckLabel(0) As System.Windows.Forms.Label
    Friend lblCombo(0) As System.Windows.Forms.Label
    Friend lblComment(0) As System.Windows.Forms.Label
    Friend lblGISLookup(0) As System.Windows.Forms.Label
    Friend lblList(0) As System.Windows.Forms.Label
    Friend lblMlText(0) As System.Windows.Forms.Label
    Friend lblPMLookup(0) As System.Windows.Forms.Label
    Friend lblPremium(0) As System.Windows.Forms.Label
    Friend lblRate(0) As System.Windows.Forms.Label
    Friend lblStandardWordingMove(0) As System.Windows.Forms.Label
    Friend lblText(0) As System.Windows.Forms.Label
    Friend lblTotalSumInsured(0) As System.Windows.Forms.Label
    Friend lvwListView(0) As System.Windows.Forms.ListView
    Friend lvwStandardWording(0) As System.Windows.Forms.ListView
    Friend lvwSumInsured(0) As System.Windows.Forms.ListView
    Friend pnlPartyPanel(0) As System.Windows.Forms.Label
    Friend pnlPolicyPanel(0) As System.Windows.Forms.Label
    Friend pnlTotalSumInsured(0) As System.Windows.Forms.Label
    Friend txtAddress1(0) As System.Windows.Forms.TextBox
    Friend txtAddress2(0) As System.Windows.Forms.TextBox
    Friend txtAddress3(0) As System.Windows.Forms.TextBox
    Friend txtAddress4(0) As System.Windows.Forms.TextBox
    Friend txtAddress5(0) As System.Windows.Forms.TextBox
    Friend txtAddress6(0) As System.Windows.Forms.TextBox
    Friend txtMlText(0) As System.Windows.Forms.TextBox
    Friend txtPremium(0) As System.Windows.Forms.TextBox
    Friend txtRate(0) As System.Windows.Forms.TextBox
    Friend txtText(0) As System.Windows.Forms.TextBox
    Friend txtFormattedText(0) As uctSIRRTFControl.uctRichTextBox
    Friend lblFormattedText(0) As System.Windows.Forms.Label
    'Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    ' <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RiskScreen))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uctCLMCaseClaim1 = New uctCLMCaseClaimList.uctCLMCaseClaim
        Me.uctCLMCaseHeader1 = New uctCLMCaseHeaders.uctCLMCaseHeader
        Me._cmdListViewSequenceDown_0 = New System.Windows.Forms.Button
        Me._cmdListViewSequenceUp_0 = New System.Windows.Forms.Button
        Me._cmdStandardWordingEdit_0 = New System.Windows.Forms.Button
        Me._lvwStandardWording_0 = New System.Windows.Forms.ListView
        Me._lvwStandardWording_0_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwStandardWording_0_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwStandardWording_0_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._txtAddress6_0 = New System.Windows.Forms.TextBox
        Me._cmdListViewDelete_0 = New System.Windows.Forms.Button
        Me._cmdListViewEdit_0 = New System.Windows.Forms.Button
        Me._cmdListViewAdd_0 = New System.Windows.Forms.Button
        Me._fraFrame_0 = New System.Windows.Forms.GroupBox
        Me._cmdStandardWordingDelete_0 = New System.Windows.Forms.Button
        Me._cmdStandardWordingAdd_0 = New System.Windows.Forms.Button
        Me._chkYesNo_0 = New System.Windows.Forms.CheckBox
        Me._cmdAddress_0 = New System.Windows.Forms.Button
        Me._txtAddress4_0 = New System.Windows.Forms.TextBox
        Me._txtAddress3_0 = New System.Windows.Forms.TextBox
        Me._txtAddress2_0 = New System.Windows.Forms.TextBox
        Me._txtAddress1_0 = New System.Windows.Forms.TextBox
        Me._cmdPartyCommand_0 = New System.Windows.Forms.Button
        Me._txtRate_0 = New System.Windows.Forms.TextBox
        Me._cmdSumInsuredEdit_0 = New System.Windows.Forms.Button
        Me._cmdSumInsuredAdd_0 = New System.Windows.Forms.Button
        Me._cmdSumInsuredDelete_0 = New System.Windows.Forms.Button
        Me._txtPremium_0 = New System.Windows.Forms.TextBox
        Me._cmdStandardWordingUp_0 = New System.Windows.Forms.Button
        Me._cmdStandardWordingDown_0 = New System.Windows.Forms.Button
        Me._txtText_0 = New System.Windows.Forms.TextBox
        Me._txtMlText_0 = New System.Windows.Forms.TextBox
        Me.txtCurrency = New System.Windows.Forms.TextBox
        Me.txtDate = New System.Windows.Forms.TextBox
        Me._cmdPolicyCommand_0 = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me._lvwListView_0 = New System.Windows.Forms.ListView
        Me._lvwSumInsured_0 = New System.Windows.Forms.ListView
        Me._lvwSumInsured_0_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSumInsured_0_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSumInsured_0_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSumInsured_0_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSumInsured_0_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSumInsured_0_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSumInsured_0_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._txtAddress5_0 = New System.Windows.Forms.TextBox
        Me.TabStrip1 = New System.Windows.Forms.TabControl
        Me._TabStrip1_Tab1 = New System.Windows.Forms.TabPage
        Me._cboList_0 = New PMListMgrDropdown.uctDropdown
        Me._cboGISLookup_0 = New uctGISUserDefLookupControl.cboGISLookup
        Me._PBFindRT1_0 = New uctPBFindRT.PBFindRT
        Me.uctCLMPerilRT1 = New uctCLMPerilRTControl.uctCLMPerilRT
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me._lblStandardWordingMove_0 = New System.Windows.Forms.Label
        Me.PBForm = New System.Windows.Forms.Label
        Me._pnlPartyPanel_0 = New System.Windows.Forms.Label
        Me._pnlTotalSumInsured_0 = New System.Windows.Forms.Label
        Me._pnlPolicyPanel_0 = New System.Windows.Forms.Label
        Me._lblComment_0 = New System.Windows.Forms.Label
        Me.pnlPosition = New System.Windows.Forms.Label
        Me._lblCheckLabel_0 = New System.Windows.Forms.Label
        Me._lblCombo_0 = New System.Windows.Forms.Label
        Me._lblRate_0 = New System.Windows.Forms.Label
        Me._lblMlText_0 = New System.Windows.Forms.Label
        Me._lblText_0 = New System.Windows.Forms.Label
        Me._lblList_0 = New System.Windows.Forms.Label
        Me._lblPremium_0 = New System.Windows.Forms.Label
        Me._lblTotalSumInsured_0 = New System.Windows.Forms.Label
        Me._lblGISLookup_0 = New System.Windows.Forms.Label
        Me._lblPMLookup_0 = New System.Windows.Forms.Label
        Me.lblAccumulation = New System.Windows.Forms.Label
        Me._cboPMLookup_0 = New PMLookupControl.cboPMLookup
        Me.uctCLMPayment = New uctCLMPaymentControl.uctCLMPayment1
        Me.cboAccumulation = New uctAccumulationLookup.cboAccumulation
        Me.uctCLMReserve = New uctCLMReserveControl.uctCLMReserve
        Me._txtFormattedText_0 = New uctSIRRTFControl.uctRichTextBox
        Me.TabStrip1.SuspendLayout()
        Me._TabStrip1_Tab1.SuspendLayout()
        Me.SuspendLayout()
        '
        'uctCLMCaseClaim1
        '
        Me.uctCLMCaseClaim1.BaseCaseId = 0
        Me.uctCLMCaseClaim1.CaseID = 0
        Me.uctCLMCaseClaim1.ClaimId = 0
        Me.uctCLMCaseClaim1.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMCaseClaim1.Location = New System.Drawing.Point(76, 162)
        Me.uctCLMCaseClaim1.MinimumHeight = 1995
        Me.uctCLMCaseClaim1.MinimumWidth = 8250
        Me.uctCLMCaseClaim1.Name = "uctCLMCaseClaim1"
        Me.uctCLMCaseClaim1.Size = New System.Drawing.Size(550, 133)
        Me.uctCLMCaseClaim1.TabIndex = 61
        Me.uctCLMCaseClaim1.Visible = False
        '
        'uctCLMCaseHeader1
        '
        Me.uctCLMCaseHeader1.BaseCaseID = 0
        Me.uctCLMCaseHeader1.CaseAssistantID = 0
        Me.uctCLMCaseHeader1.CaseID = 0
        Me.uctCLMCaseHeader1.CaseNumber = ""
        Me.uctCLMCaseHeader1.CaseOpenedDate = New Date(CType(0, Long))
        Me.uctCLMCaseHeader1.CaseProgressStatusID = 0
        Me.uctCLMCaseHeader1.CaseVersion = 0
        Me.uctCLMCaseHeader1.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMCaseHeader1.Location = New System.Drawing.Point(528, 80)
        Me.uctCLMCaseHeader1.MinimumHeight = 1320
        Me.uctCLMCaseHeader1.MinimumWidth = 8520
        Me.uctCLMCaseHeader1.Name = "uctCLMCaseHeader1"
        Me.uctCLMCaseHeader1.Size = New System.Drawing.Size(627, 89)
        Me.uctCLMCaseHeader1.TabIndex = 60
        Me.uctCLMCaseHeader1.Task = 0
        Me.uctCLMCaseHeader1.Visible = False
        '
        '_cmdListViewSequenceDown_0
        '
        Me._cmdListViewSequenceDown_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdListViewSequenceDown_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdListViewSequenceDown_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewSequenceDown_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewSequenceDown_0.Image = CType(resources.GetObject("_cmdListViewSequenceDown_0.Image"), System.Drawing.Image)
        Me._cmdListViewSequenceDown_0.Location = New System.Drawing.Point(286, 73)
        Me._cmdListViewSequenceDown_0.Name = "_cmdListViewSequenceDown_0"
        Me._cmdListViewSequenceDown_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewSequenceDown_0.Size = New System.Drawing.Size(33, 33)
        Me._cmdListViewSequenceDown_0.TabIndex = 56
        Me._cmdListViewSequenceDown_0.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._cmdListViewSequenceDown_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewSequenceDown_0.UseVisualStyleBackColor = False
        Me._cmdListViewSequenceDown_0.Visible = False
        '
        '_cmdListViewSequenceUp_0
        '
        Me._cmdListViewSequenceUp_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdListViewSequenceUp_0.BackgroundImage = CType(resources.GetObject("_cmdListViewSequenceUp_0.BackgroundImage"), System.Drawing.Image)
        Me._cmdListViewSequenceUp_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdListViewSequenceUp_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewSequenceUp_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewSequenceUp_0.Image = CType(resources.GetObject("_cmdListViewSequenceUp_0.Image"), System.Drawing.Image)
        Me._cmdListViewSequenceUp_0.Location = New System.Drawing.Point(283, 32)
        Me._cmdListViewSequenceUp_0.Name = "_cmdListViewSequenceUp_0"
        Me._cmdListViewSequenceUp_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewSequenceUp_0.Size = New System.Drawing.Size(33, 33)
        Me._cmdListViewSequenceUp_0.TabIndex = 55
        Me._cmdListViewSequenceUp_0.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._cmdListViewSequenceUp_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewSequenceUp_0.UseVisualStyleBackColor = False
        Me._cmdListViewSequenceUp_0.Visible = False
        '
        '_cmdStandardWordingEdit_0
        '
        Me._cmdStandardWordingEdit_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdStandardWordingEdit_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdStandardWordingEdit_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdStandardWordingEdit_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdStandardWordingEdit_0.Location = New System.Drawing.Point(64, 164)
        Me._cmdStandardWordingEdit_0.Name = "_cmdStandardWordingEdit_0"
        Me._cmdStandardWordingEdit_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdStandardWordingEdit_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdStandardWordingEdit_0.TabIndex = 57
        Me._cmdStandardWordingEdit_0.Text = "Edit"
        Me._cmdStandardWordingEdit_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdStandardWordingEdit_0.UseVisualStyleBackColor = False
        Me._cmdStandardWordingEdit_0.Visible = False
        '
        '_lvwStandardWording_0
        '
        Me._lvwStandardWording_0.BackColor = System.Drawing.SystemColors.Window
        Me._lvwStandardWording_0.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwStandardWording_0_ColumnHeader_1, Me._lvwStandardWording_0_ColumnHeader_2, Me._lvwStandardWording_0_ColumnHeader_3})
        Me._lvwStandardWording_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lvwStandardWording_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lvwStandardWording_0.Location = New System.Drawing.Point(23, -11)
        Me._lvwStandardWording_0.Name = "_lvwStandardWording_0"
        Me._lvwStandardWording_0.Size = New System.Drawing.Size(255, 67)
        Me._lvwStandardWording_0.TabIndex = 41
        Me._lvwStandardWording_0.UseCompatibleStateImageBehavior = False
        Me._lvwStandardWording_0.View = System.Windows.Forms.View.Details
        Me._lvwStandardWording_0.Visible = False
        '
        '_lvwStandardWording_0_ColumnHeader_1
        '
        Me._lvwStandardWording_0_ColumnHeader_1.Text = "Code"
        Me._lvwStandardWording_0_ColumnHeader_1.Width = 67
        '
        '_lvwStandardWording_0_ColumnHeader_2
        '
        Me._lvwStandardWording_0_ColumnHeader_2.Text = "Description"
        Me._lvwStandardWording_0_ColumnHeader_2.Width = 201
        '
        '_lvwStandardWording_0_ColumnHeader_3
        '
        Me._lvwStandardWording_0_ColumnHeader_3.Text = "Edited"
        Me._lvwStandardWording_0_ColumnHeader_3.Width = 67
        '
        '_txtAddress6_0
        '
        Me._txtAddress6_0.AcceptsReturn = True
        Me._txtAddress6_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtAddress6_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtAddress6_0.Enabled = False
        Me._txtAddress6_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtAddress6_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtAddress6_0.Location = New System.Drawing.Point(336, 160)
        Me._txtAddress6_0.MaxLength = 0
        Me._txtAddress6_0.Name = "_txtAddress6_0"
        Me._txtAddress6_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtAddress6_0.Size = New System.Drawing.Size(89, 20)
        Me._txtAddress6_0.TabIndex = 28
        Me._txtAddress6_0.Visible = False
        '
        '_cmdListViewDelete_0
        '
        Me._cmdListViewDelete_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdListViewDelete_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdListViewDelete_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewDelete_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewDelete_0.Location = New System.Drawing.Point(160, 128)
        Me._cmdListViewDelete_0.Name = "_cmdListViewDelete_0"
        Me._cmdListViewDelete_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewDelete_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdListViewDelete_0.TabIndex = 40
        Me._cmdListViewDelete_0.Text = "&Delete"
        Me._cmdListViewDelete_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewDelete_0.UseVisualStyleBackColor = False
        Me._cmdListViewDelete_0.Visible = False
        '
        '_cmdListViewEdit_0
        '
        Me._cmdListViewEdit_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdListViewEdit_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdListViewEdit_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewEdit_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewEdit_0.Location = New System.Drawing.Point(197, 259)
        Me._cmdListViewEdit_0.Name = "_cmdListViewEdit_0"
        Me._cmdListViewEdit_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewEdit_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdListViewEdit_0.TabIndex = 39
        Me._cmdListViewEdit_0.Text = "&Edit"
        Me._cmdListViewEdit_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewEdit_0.UseVisualStyleBackColor = False
        Me._cmdListViewEdit_0.Visible = False
        '
        '_cmdListViewAdd_0
        '
        Me._cmdListViewAdd_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdListViewAdd_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdListViewAdd_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdListViewAdd_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdListViewAdd_0.Location = New System.Drawing.Point(0, 128)
        Me._cmdListViewAdd_0.Name = "_cmdListViewAdd_0"
        Me._cmdListViewAdd_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdListViewAdd_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdListViewAdd_0.TabIndex = 38
        Me._cmdListViewAdd_0.Text = "&Add"
        Me._cmdListViewAdd_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdListViewAdd_0.UseVisualStyleBackColor = False
        Me._cmdListViewAdd_0.Visible = False
        '
        '_fraFrame_0
        '
        Me._fraFrame_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraFrame_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraFrame_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraFrame_0.Location = New System.Drawing.Point(8, 88)
        Me._fraFrame_0.Name = "_fraFrame_0"
        Me._fraFrame_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraFrame_0.Size = New System.Drawing.Size(225, 25)
        Me._fraFrame_0.TabIndex = 34
        Me._fraFrame_0.TabStop = False
        Me._fraFrame_0.Text = "Frame"
        Me._fraFrame_0.Visible = False
        '
        '_cmdStandardWordingDelete_0
        '
        Me._cmdStandardWordingDelete_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdStandardWordingDelete_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdStandardWordingDelete_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdStandardWordingDelete_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdStandardWordingDelete_0.Location = New System.Drawing.Point(80, 120)
        Me._cmdStandardWordingDelete_0.Name = "_cmdStandardWordingDelete_0"
        Me._cmdStandardWordingDelete_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdStandardWordingDelete_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdStandardWordingDelete_0.TabIndex = 33
        Me._cmdStandardWordingDelete_0.Text = "Delete"
        Me._cmdStandardWordingDelete_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdStandardWordingDelete_0.UseVisualStyleBackColor = False
        Me._cmdStandardWordingDelete_0.Visible = False
        '
        '_cmdStandardWordingAdd_0
        '
        Me._cmdStandardWordingAdd_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdStandardWordingAdd_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdStandardWordingAdd_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdStandardWordingAdd_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdStandardWordingAdd_0.Location = New System.Drawing.Point(0, 156)
        Me._cmdStandardWordingAdd_0.Name = "_cmdStandardWordingAdd_0"
        Me._cmdStandardWordingAdd_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdStandardWordingAdd_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdStandardWordingAdd_0.TabIndex = 32
        Me._cmdStandardWordingAdd_0.Text = "Add"
        Me._cmdStandardWordingAdd_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdStandardWordingAdd_0.UseVisualStyleBackColor = False
        Me._cmdStandardWordingAdd_0.Visible = False
        '
        '_chkYesNo_0
        '
        Me._chkYesNo_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkYesNo_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkYesNo_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkYesNo_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkYesNo_0.Location = New System.Drawing.Point(16, 24)
        Me._chkYesNo_0.Name = "_chkYesNo_0"
        Me._chkYesNo_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkYesNo_0.Size = New System.Drawing.Size(105, 17)
        Me._chkYesNo_0.TabIndex = 30
        Me._chkYesNo_0.Text = "chkYesNo"
        Me._chkYesNo_0.UseVisualStyleBackColor = False
        Me._chkYesNo_0.Visible = False
        '
        '_cmdAddress_0
        '
        Me._cmdAddress_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdAddress_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdAddress_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdAddress_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdAddress_0.Location = New System.Drawing.Point(448, 136)
        Me._cmdAddress_0.Name = "_cmdAddress_0"
        Me._cmdAddress_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdAddress_0.Size = New System.Drawing.Size(73, 19)
        Me._cmdAddress_0.TabIndex = 29
        Me._cmdAddress_0.Text = "&Change"
        Me._cmdAddress_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdAddress_0.UseVisualStyleBackColor = False
        Me._cmdAddress_0.Visible = False
        '
        '_txtAddress4_0
        '
        Me._txtAddress4_0.AcceptsReturn = True
        Me._txtAddress4_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtAddress4_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtAddress4_0.Enabled = False
        Me._txtAddress4_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtAddress4_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtAddress4_0.Location = New System.Drawing.Point(336, 112)
        Me._txtAddress4_0.MaxLength = 0
        Me._txtAddress4_0.Name = "_txtAddress4_0"
        Me._txtAddress4_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtAddress4_0.Size = New System.Drawing.Size(185, 20)
        Me._txtAddress4_0.TabIndex = 26
        Me._txtAddress4_0.Visible = False
        '
        '_txtAddress3_0
        '
        Me._txtAddress3_0.AcceptsReturn = True
        Me._txtAddress3_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtAddress3_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtAddress3_0.Enabled = False
        Me._txtAddress3_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtAddress3_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtAddress3_0.Location = New System.Drawing.Point(336, 88)
        Me._txtAddress3_0.MaxLength = 0
        Me._txtAddress3_0.Name = "_txtAddress3_0"
        Me._txtAddress3_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtAddress3_0.Size = New System.Drawing.Size(185, 20)
        Me._txtAddress3_0.TabIndex = 25
        Me._txtAddress3_0.Visible = False
        '
        '_txtAddress2_0
        '
        Me._txtAddress2_0.AcceptsReturn = True
        Me._txtAddress2_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtAddress2_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtAddress2_0.Enabled = False
        Me._txtAddress2_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtAddress2_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtAddress2_0.Location = New System.Drawing.Point(336, 64)
        Me._txtAddress2_0.MaxLength = 0
        Me._txtAddress2_0.Name = "_txtAddress2_0"
        Me._txtAddress2_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtAddress2_0.Size = New System.Drawing.Size(185, 20)
        Me._txtAddress2_0.TabIndex = 24
        Me._txtAddress2_0.Visible = False
        '
        '_txtAddress1_0
        '
        Me._txtAddress1_0.AcceptsReturn = True
        Me._txtAddress1_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtAddress1_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtAddress1_0.Enabled = False
        Me._txtAddress1_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtAddress1_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtAddress1_0.Location = New System.Drawing.Point(336, 40)
        Me._txtAddress1_0.MaxLength = 0
        Me._txtAddress1_0.Name = "_txtAddress1_0"
        Me._txtAddress1_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtAddress1_0.Size = New System.Drawing.Size(185, 20)
        Me._txtAddress1_0.TabIndex = 23
        Me._txtAddress1_0.Visible = False
        '
        '_cmdPartyCommand_0
        '
        Me._cmdPartyCommand_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPartyCommand_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPartyCommand_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPartyCommand_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPartyCommand_0.Location = New System.Drawing.Point(280, 328)
        Me._cmdPartyCommand_0.Name = "_cmdPartyCommand_0"
        Me._cmdPartyCommand_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPartyCommand_0.Size = New System.Drawing.Size(97, 22)
        Me._cmdPartyCommand_0.TabIndex = 12
        Me._cmdPartyCommand_0.Text = "Command"
        Me._cmdPartyCommand_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPartyCommand_0.UseVisualStyleBackColor = False
        Me._cmdPartyCommand_0.Visible = False
        '
        '_txtRate_0
        '
        Me._txtRate_0.AcceptsReturn = True
        Me._txtRate_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtRate_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtRate_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtRate_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtRate_0.Location = New System.Drawing.Point(184, 344)
        Me._txtRate_0.MaxLength = 0
        Me._txtRate_0.Name = "_txtRate_0"
        Me._txtRate_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtRate_0.Size = New System.Drawing.Size(105, 20)
        Me._txtRate_0.TabIndex = 11
        Me._txtRate_0.Visible = False
        '
        '_cmdSumInsuredEdit_0
        '
        Me._cmdSumInsuredEdit_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdSumInsuredEdit_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdSumInsuredEdit_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdSumInsuredEdit_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdSumInsuredEdit_0.Location = New System.Drawing.Point(168, 312)
        Me._cmdSumInsuredEdit_0.Name = "_cmdSumInsuredEdit_0"
        Me._cmdSumInsuredEdit_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdSumInsuredEdit_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdSumInsuredEdit_0.TabIndex = 10
        Me._cmdSumInsuredEdit_0.Text = "&Edit"
        Me._cmdSumInsuredEdit_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdSumInsuredEdit_0.UseVisualStyleBackColor = False
        Me._cmdSumInsuredEdit_0.Visible = False
        '
        '_cmdSumInsuredAdd_0
        '
        Me._cmdSumInsuredAdd_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdSumInsuredAdd_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdSumInsuredAdd_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdSumInsuredAdd_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdSumInsuredAdd_0.Location = New System.Drawing.Point(88, 312)
        Me._cmdSumInsuredAdd_0.Name = "_cmdSumInsuredAdd_0"
        Me._cmdSumInsuredAdd_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdSumInsuredAdd_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdSumInsuredAdd_0.TabIndex = 9
        Me._cmdSumInsuredAdd_0.Text = "Add"
        Me._cmdSumInsuredAdd_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdSumInsuredAdd_0.UseVisualStyleBackColor = False
        Me._cmdSumInsuredAdd_0.Visible = False
        '
        '_cmdSumInsuredDelete_0
        '
        Me._cmdSumInsuredDelete_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdSumInsuredDelete_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdSumInsuredDelete_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdSumInsuredDelete_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdSumInsuredDelete_0.Location = New System.Drawing.Point(248, 312)
        Me._cmdSumInsuredDelete_0.Name = "_cmdSumInsuredDelete_0"
        Me._cmdSumInsuredDelete_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdSumInsuredDelete_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdSumInsuredDelete_0.TabIndex = 8
        Me._cmdSumInsuredDelete_0.Text = "Delete"
        Me._cmdSumInsuredDelete_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdSumInsuredDelete_0.UseVisualStyleBackColor = False
        Me._cmdSumInsuredDelete_0.Visible = False
        '
        '_txtPremium_0
        '
        Me._txtPremium_0.AcceptsReturn = True
        Me._txtPremium_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtPremium_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtPremium_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtPremium_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtPremium_0.Location = New System.Drawing.Point(359, 344)
        Me._txtPremium_0.MaxLength = 0
        Me._txtPremium_0.Name = "_txtPremium_0"
        Me._txtPremium_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtPremium_0.Size = New System.Drawing.Size(177, 20)
        Me._txtPremium_0.TabIndex = 7
        Me._txtPremium_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._txtPremium_0.Visible = False
        '
        '_cmdStandardWordingUp_0
        '
        Me._cmdStandardWordingUp_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdStandardWordingUp_0.BackgroundImage = CType(resources.GetObject("_cmdStandardWordingUp_0.BackgroundImage"), System.Drawing.Image)
        Me._cmdStandardWordingUp_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdStandardWordingUp_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdStandardWordingUp_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdStandardWordingUp_0.Image = CType(resources.GetObject("_cmdStandardWordingUp_0.Image"), System.Drawing.Image)
        Me._cmdStandardWordingUp_0.Location = New System.Drawing.Point(560, 40)
        Me._cmdStandardWordingUp_0.Name = "_cmdStandardWordingUp_0"
        Me._cmdStandardWordingUp_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdStandardWordingUp_0.Size = New System.Drawing.Size(33, 33)
        Me._cmdStandardWordingUp_0.TabIndex = 6
        Me._cmdStandardWordingUp_0.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._cmdStandardWordingUp_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdStandardWordingUp_0.UseVisualStyleBackColor = False
        Me._cmdStandardWordingUp_0.Visible = False
        '
        '_cmdStandardWordingDown_0
        '
        Me._cmdStandardWordingDown_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdStandardWordingDown_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdStandardWordingDown_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdStandardWordingDown_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdStandardWordingDown_0.Image = CType(resources.GetObject("_cmdStandardWordingDown_0.Image"), System.Drawing.Image)
        Me._cmdStandardWordingDown_0.Location = New System.Drawing.Point(560, 176)
        Me._cmdStandardWordingDown_0.Name = "_cmdStandardWordingDown_0"
        Me._cmdStandardWordingDown_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdStandardWordingDown_0.Size = New System.Drawing.Size(33, 33)
        Me._cmdStandardWordingDown_0.TabIndex = 5
        Me._cmdStandardWordingDown_0.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me._cmdStandardWordingDown_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdStandardWordingDown_0.UseVisualStyleBackColor = False
        Me._cmdStandardWordingDown_0.Visible = False
        '
        '_txtText_0
        '
        Me._txtText_0.AcceptsReturn = True
        Me._txtText_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtText_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtText_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtText_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtText_0.Location = New System.Drawing.Point(118, 189)
        Me._txtText_0.MaxLength = 0
        Me._txtText_0.Name = "_txtText_0"
        Me._txtText_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtText_0.Size = New System.Drawing.Size(113, 20)
        Me._txtText_0.TabIndex = 4
        Me._txtText_0.Visible = False
        '
        '_txtMlText_0
        '
        Me._txtMlText_0.AcceptsReturn = True
        Me._txtMlText_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtMlText_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtMlText_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtMlText_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtMlText_0.Location = New System.Drawing.Point(145, 142)
        Me._txtMlText_0.MaxLength = 0
        Me._txtMlText_0.Multiline = True
        Me._txtMlText_0.Name = "_txtMlText_0"
        Me._txtMlText_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtMlText_0.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me._txtMlText_0.Size = New System.Drawing.Size(113, 19)
        Me._txtMlText_0.TabIndex = 44
        Me._txtMlText_0.Visible = False
        '
        'txtCurrency
        '
        Me.txtCurrency.AcceptsReturn = True
        Me.txtCurrency.BackColor = System.Drawing.SystemColors.Window
        Me.txtCurrency.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCurrency.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurrency.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCurrency.Location = New System.Drawing.Point(544, 272)
        Me.txtCurrency.MaxLength = 0
        Me.txtCurrency.Name = "txtCurrency"
        Me.txtCurrency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCurrency.Size = New System.Drawing.Size(41, 20)
        Me.txtCurrency.TabIndex = 3
        Me.txtCurrency.Visible = False
        '
        'txtDate
        '
        Me.txtDate.AcceptsReturn = True
        Me.txtDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDate.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.Location = New System.Drawing.Point(544, 248)
        Me.txtDate.MaxLength = 0
        Me.txtDate.Name = "txtDate"
        Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDate.Size = New System.Drawing.Size(41, 20)
        Me.txtDate.TabIndex = 2
        Me.txtDate.Visible = False
        '
        '_cmdPolicyCommand_0
        '
        Me._cmdPolicyCommand_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPolicyCommand_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPolicyCommand_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPolicyCommand_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPolicyCommand_0.Location = New System.Drawing.Point(16, 205)
        Me._cmdPolicyCommand_0.Name = "_cmdPolicyCommand_0"
        Me._cmdPolicyCommand_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPolicyCommand_0.Size = New System.Drawing.Size(97, 22)
        Me._cmdPolicyCommand_0.TabIndex = 0
        Me._cmdPolicyCommand_0.Text = "Command"
        Me._cmdPolicyCommand_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPolicyCommand_0.UseVisualStyleBackColor = False
        Me._cmdPolicyCommand_0.Visible = False
        '
        '_lvwListView_0
        '
        Me._lvwListView_0.BackColor = System.Drawing.SystemColors.Window
        Me._lvwListView_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lvwListView_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lvwListView_0.Location = New System.Drawing.Point(17, -11)
        Me._lvwListView_0.Name = "_lvwListView_0"
        Me._lvwListView_0.Size = New System.Drawing.Size(249, 49)
        Me._lvwListView_0.TabIndex = 31
        Me._lvwListView_0.UseCompatibleStateImageBehavior = False
        Me._lvwListView_0.View = System.Windows.Forms.View.Details
        Me._lvwListView_0.Visible = False
        '
        '_lvwSumInsured_0
        '
        Me._lvwSumInsured_0.BackColor = System.Drawing.SystemColors.Window
        Me._lvwSumInsured_0.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSumInsured_0_ColumnHeader_1, Me._lvwSumInsured_0_ColumnHeader_2, Me._lvwSumInsured_0_ColumnHeader_3, Me._lvwSumInsured_0_ColumnHeader_4, Me._lvwSumInsured_0_ColumnHeader_5, Me._lvwSumInsured_0_ColumnHeader_6, Me._lvwSumInsured_0_ColumnHeader_7})
        Me._lvwSumInsured_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lvwSumInsured_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lvwSumInsured_0.Location = New System.Drawing.Point(280, 276)
        Me._lvwSumInsured_0.Name = "_lvwSumInsured_0"
        Me._lvwSumInsured_0.Size = New System.Drawing.Size(207, 20)
        Me._lvwSumInsured_0.TabIndex = 35
        Me._lvwSumInsured_0.UseCompatibleStateImageBehavior = False
        Me._lvwSumInsured_0.View = System.Windows.Forms.View.Details
        Me._lvwSumInsured_0.Visible = False
        '
        '_lvwSumInsured_0_ColumnHeader_1
        '
        Me._lvwSumInsured_0_ColumnHeader_1.Text = "Description"
        Me._lvwSumInsured_0_ColumnHeader_1.Width = 97
        '
        '_lvwSumInsured_0_ColumnHeader_2
        '
        Me._lvwSumInsured_0_ColumnHeader_2.Text = "Reference"
        Me._lvwSumInsured_0_ColumnHeader_2.Width = 97
        '
        '_lvwSumInsured_0_ColumnHeader_3
        '
        Me._lvwSumInsured_0_ColumnHeader_3.Text = "Sum insured"
        Me._lvwSumInsured_0_ColumnHeader_3.Width = 97
        '
        '_lvwSumInsured_0_ColumnHeader_4
        '
        Me._lvwSumInsured_0_ColumnHeader_4.Text = "Date added"
        Me._lvwSumInsured_0_ColumnHeader_4.Width = 97
        '
        '_lvwSumInsured_0_ColumnHeader_5
        '
        Me._lvwSumInsured_0_ColumnHeader_5.Text = "Date deleted"
        Me._lvwSumInsured_0_ColumnHeader_5.Width = 97
        '
        '_lvwSumInsured_0_ColumnHeader_6
        '
        Me._lvwSumInsured_0_ColumnHeader_6.Text = "Valuation Required"
        Me._lvwSumInsured_0_ColumnHeader_6.Width = 97
        '
        '_lvwSumInsured_0_ColumnHeader_7
        '
        Me._lvwSumInsured_0_ColumnHeader_7.Text = "Valuation Date"
        Me._lvwSumInsured_0_ColumnHeader_7.Width = 97
        '
        '_txtAddress5_0
        '
        Me._txtAddress5_0.AcceptsReturn = True
        Me._txtAddress5_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtAddress5_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtAddress5_0.Enabled = False
        Me._txtAddress5_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtAddress5_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtAddress5_0.Location = New System.Drawing.Point(336, 136)
        Me._txtAddress5_0.MaxLength = 0
        Me._txtAddress5_0.Name = "_txtAddress5_0"
        Me._txtAddress5_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtAddress5_0.Size = New System.Drawing.Size(89, 20)
        Me._txtAddress5_0.TabIndex = 27
        Me._txtAddress5_0.Visible = False
        '
        'TabStrip1
        Me.TabStrip1.Controls.Add(Me._TabStrip1_Tab1)
        Me.TabStrip1.Location = New System.Drawing.Point(0, 0)
        Me.TabStrip1.Name = "TabStrip1"
        Me.TabStrip1.SelectedIndex = 0
        Me.TabStrip1.Size = New System.Drawing.Size(600, 400)
        Me.TabStrip1.TabIndex = 47
        '
        '_TabStrip1_Tab1
        '
        Me._TabStrip1_Tab1.Controls.Add(Me.uctCLMCaseClaim1)
        Me._TabStrip1_Tab1.Controls.Add(Me._lvwStandardWording_0)
        Me._TabStrip1_Tab1.Controls.Add(Me._lvwListView_0)
        Me._TabStrip1_Tab1.Location = New System.Drawing.Point(4, 22)
        Me._TabStrip1_Tab1.Name = "_TabStrip1_Tab1"
        Me._TabStrip1_Tab1.Size = New System.Drawing.Size(592, 374)
        Me._TabStrip1_Tab1.TabIndex = 0
        Me._TabStrip1_Tab1.Text = "1"
        '
        '_cboList_0
        '
        Me._cboList_0.AllowAbiCodeEntry = False
        Me._cboList_0.AutoCompleteText = False
        Me._cboList_0.DataModel = "GIIM"
        Me._cboList_0.ListIndex = -1
        Me._cboList_0.ListManager = Nothing
        Me._cboList_0.Location = New System.Drawing.Point(403, 197)
        Me._cboList_0.Login = False
        Me._cboList_0.LongList = False
        Me._cboList_0.MouseIcon = CType(resources.GetObject("_cboList_0.MouseIcon"), System.Drawing.Image)
        Me._cboList_0.MousePointer = System.Windows.Forms.Cursors.Default
        Me._cboList_0.Name = "_cboList_0"
        Me._cboList_0.PropertyId = ""
        Me._cboList_0.ReadOnly_Renamed = False
        Me._cboList_0.SelLength = 0
        Me._cboList_0.SelStart = 0
        Me._cboList_0.SelText = ""
        Me._cboList_0.Size = New System.Drawing.Size(144, 21)
        Me._cboList_0.TabIndex = 64
        Me._cboList_0.ToolTipText = ""
        Me._cboList_0.VehicleListId = ""
        Me._cboList_0.VehicleMake = ""
        Me._cboList_0.Visible = False
        '
        '_cboGISLookup_0
        '
        Me._cboGISLookup_0.DefaultItemId = 0
        Me._cboGISLookup_0.FirstItem = ""
        Me._cboGISLookup_0.GISDataModelCode = "None"
        Me._cboGISLookup_0.ItemId = 0
        Me._cboGISLookup_0.ListIndex = -1
        Me._cboGISLookup_0.Location = New System.Drawing.Point(244, 199)
        Me._cboGISLookup_0.Name = "_cboGISLookup_0"
        Me._cboGISLookup_0.ParentDetailId = 0
        Me._cboGISLookup_0.ParentHeaderId = 0
        Me._cboGISLookup_0.SingleItemId = 0
        Me._cboGISLookup_0.Size = New System.Drawing.Size(153, 21)
        Me._cboGISLookup_0.TabIndex = 63
        Me._cboGISLookup_0.Table = 0
        Me._cboGISLookup_0.ToolTipText = ""
        Me._cboGISLookup_0.Visible = False
        Me._cboGISLookup_0.WhatsThisHelpID = 0
        '
        '_PBFindRT1_0
        '
        Me._PBFindRT1_0.DataArray = Nothing
        Me._PBFindRT1_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._PBFindRT1_0.Location = New System.Drawing.Point(32, 336)
        Me._PBFindRT1_0.Name = "_PBFindRT1_0"
        Me._PBFindRT1_0.Size = New System.Drawing.Size(193, 37)
        Me._PBFindRT1_0.TabIndex = 49
        Me._PBFindRT1_0.Visible = False
        '
        'uctCLMPerilRT1
        '
        Me.uctCLMPerilRT1.Claimid = 0
        Me.uctCLMPerilRT1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMPerilRT1.IsOpenClaimNoTrans = False
        Me.uctCLMPerilRT1.Location = New System.Drawing.Point(444, 299)
        Me.uctCLMPerilRT1.Name = "uctCLMPerilRT1"
        Me.uctCLMPerilRT1.Policy = 0
        Me.uctCLMPerilRT1.Risk = 0
        Me.uctCLMPerilRT1.ScreenCaption = ""
        Me.uctCLMPerilRT1.Size = New System.Drawing.Size(132, 45)
        Me.uctCLMPerilRT1.Status = 0
        Me.uctCLMPerilRT1.TabIndex = 52
        Me.uctCLMPerilRT1.ViewRiskFlag = False
        Me.uctCLMPerilRT1.Visible = False
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "")
        Me.ImageList2.Images.SetKeyName(1, "AddressImage")
        Me.ImageList2.Images.SetKeyName(2, "")
        Me.ImageList2.Images.SetKeyName(3, "")
        Me.ImageList2.Images.SetKeyName(4, "PolicyImage")
        Me.ImageList2.Images.SetKeyName(5, " ")
        Me.ImageList2.Images.SetKeyName(6, "ContactImage")
        Me.ImageList2.Images.SetKeyName(7, "")
        Me.ImageList2.Images.SetKeyName(8, "")
        Me.ImageList2.Images.SetKeyName(9, "CampaignImage")
        Me.ImageList2.Images.SetKeyName(10, "ConvictionImage")
        Me.ImageList2.Images.SetKeyName(11, "LifestyleImage")
        '
        '_lblStandardWordingMove_0
        '
        Me._lblStandardWordingMove_0.AutoSize = True
        Me._lblStandardWordingMove_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblStandardWordingMove_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblStandardWordingMove_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblStandardWordingMove_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblStandardWordingMove_0.Location = New System.Drawing.Point(549, 96)
        Me._lblStandardWordingMove_0.Name = "_lblStandardWordingMove_0"
        Me._lblStandardWordingMove_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblStandardWordingMove_0.Size = New System.Drawing.Size(34, 13)
        Me._lblStandardWordingMove_0.TabIndex = 36
        Me._lblStandardWordingMove_0.Text = "Move"
        Me._lblStandardWordingMove_0.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me._lblStandardWordingMove_0.Visible = False
        '
        'PBForm
        '
        Me.PBForm.BackColor = System.Drawing.SystemColors.Control
        Me.PBForm.Cursor = System.Windows.Forms.Cursors.Default
        Me.PBForm.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PBForm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PBForm.Location = New System.Drawing.Point(465, 218)
        Me.PBForm.Name = "PBForm"
        Me.PBForm.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PBForm.Size = New System.Drawing.Size(93, 18)
        Me.PBForm.TabIndex = 54
        Me.PBForm.Text = "Label1"
        Me.PBForm.Visible = False
        '
        '_pnlPartyPanel_0
        '
        Me._pnlPartyPanel_0.BackColor = System.Drawing.SystemColors.Control
        Me._pnlPartyPanel_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlPartyPanel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._pnlPartyPanel_0.Enabled = False
        Me._pnlPartyPanel_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlPartyPanel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._pnlPartyPanel_0.Location = New System.Drawing.Point(392, 328)
        Me._pnlPartyPanel_0.Name = "_pnlPartyPanel_0"
        Me._pnlPartyPanel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._pnlPartyPanel_0.Size = New System.Drawing.Size(153, 22)
        Me._pnlPartyPanel_0.TabIndex = 13
        Me._pnlPartyPanel_0.Text = "Name"
        Me._pnlPartyPanel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._pnlPartyPanel_0.Visible = False
        '
        '_pnlTotalSumInsured_0
        '
        Me._pnlTotalSumInsured_0.BackColor = System.Drawing.SystemColors.Control
        Me._pnlTotalSumInsured_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlTotalSumInsured_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._pnlTotalSumInsured_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlTotalSumInsured_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._pnlTotalSumInsured_0.Location = New System.Drawing.Point(360, 312)
        Me._pnlTotalSumInsured_0.Name = "_pnlTotalSumInsured_0"
        Me._pnlTotalSumInsured_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._pnlTotalSumInsured_0.Size = New System.Drawing.Size(177, 17)
        Me._pnlTotalSumInsured_0.TabIndex = 14
        Me._pnlTotalSumInsured_0.Visible = False
        '
        '_pnlPolicyPanel_0
        '
        Me._pnlPolicyPanel_0.BackColor = System.Drawing.SystemColors.Control
        Me._pnlPolicyPanel_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._pnlPolicyPanel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._pnlPolicyPanel_0.Enabled = False
        Me._pnlPolicyPanel_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlPolicyPanel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._pnlPolicyPanel_0.Location = New System.Drawing.Point(15, 159)
        Me._pnlPolicyPanel_0.Name = "_pnlPolicyPanel_0"
        Me._pnlPolicyPanel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._pnlPolicyPanel_0.Size = New System.Drawing.Size(153, 22)
        Me._pnlPolicyPanel_0.TabIndex = 15
        Me._pnlPolicyPanel_0.Text = "Name"
        Me._pnlPolicyPanel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me._pnlPolicyPanel_0.Visible = False
        '
        '_lblComment_0
        '
        Me._lblComment_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblComment_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblComment_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblComment_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblComment_0.Location = New System.Drawing.Point(248, 272)
        Me._lblComment_0.Name = "_lblComment_0"
        Me._lblComment_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblComment_0.Size = New System.Drawing.Size(97, 17)
        Me._lblComment_0.TabIndex = 50
        Me._lblComment_0.Visible = False
        '
        'pnlPosition
        '
        Me.pnlPosition.BackColor = System.Drawing.SystemColors.Control
        Me.pnlPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlPosition.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlPosition.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPosition.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlPosition.Location = New System.Drawing.Point(25, 349)
        Me.pnlPosition.Name = "pnlPosition"
        Me.pnlPosition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.pnlPosition.Size = New System.Drawing.Size(199, 18)
        Me.pnlPosition.TabIndex = 48
        Me.pnlPosition.Text = "dummy used in textLabel_MouseMove"
        Me.pnlPosition.Visible = False
        '
        '_lblCheckLabel_0
        '
        Me._lblCheckLabel_0.AutoSize = True
        Me._lblCheckLabel_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblCheckLabel_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCheckLabel_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCheckLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCheckLabel_0.Location = New System.Drawing.Point(0, 67)
        Me._lblCheckLabel_0.Name = "_lblCheckLabel_0"
        Me._lblCheckLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCheckLabel_0.Size = New System.Drawing.Size(67, 13)
        Me._lblCheckLabel_0.TabIndex = 43
        Me._lblCheckLabel_0.Text = "Check Label"
        Me._lblCheckLabel_0.Visible = False
        '
        '_lblCombo_0
        '
        Me._lblCombo_0.AutoSize = True
        Me._lblCombo_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblCombo_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCombo_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCombo_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblCombo_0.Location = New System.Drawing.Point(126, 217)
        Me._lblCombo_0.Name = "_lblCombo_0"
        Me._lblCombo_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCombo_0.Size = New System.Drawing.Size(69, 13)
        Me._lblCombo_0.TabIndex = 42
        Me._lblCombo_0.Text = "Combo Label"
        Me._lblCombo_0.Visible = False
        '
        '_lblRate_0
        '
        Me._lblRate_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblRate_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblRate_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblRate_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblRate_0.Location = New System.Drawing.Point(72, 240)
        Me._lblRate_0.Name = "_lblRate_0"
        Me._lblRate_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblRate_0.Size = New System.Drawing.Size(73, 17)
        Me._lblRate_0.TabIndex = 37
        Me._lblRate_0.Text = "Rate:"
        Me._lblRate_0.Visible = False
        '
        '_lblMlText_0
        '
        Me._lblMlText_0.AutoSize = True
        Me._lblMlText_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblMlText_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblMlText_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblMlText_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblMlText_0.Location = New System.Drawing.Point(130, 215)
        Me._lblMlText_0.Name = "_lblMlText_0"
        Me._lblMlText_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblMlText_0.Size = New System.Drawing.Size(75, 13)
        Me._lblMlText_0.TabIndex = 22
        Me._lblMlText_0.Text = "ML Text Label"
        Me._lblMlText_0.Visible = False
        '
        '_lblText_0
        '
        Me._lblText_0.AutoSize = True
        Me._lblText_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblText_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblText_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblText_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblText_0.Location = New System.Drawing.Point(138, 220)
        Me._lblText_0.Name = "_lblText_0"
        Me._lblText_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblText_0.Size = New System.Drawing.Size(57, 13)
        Me._lblText_0.TabIndex = 45
        Me._lblText_0.Text = "Text Label"
        Me._lblText_0.Visible = False
        '
        '_lblList_0
        '
        Me._lblList_0.AutoSize = True
        Me._lblList_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblList_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblList_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblList_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblList_0.Location = New System.Drawing.Point(164, 219)
        Me._lblList_0.Name = "_lblList_0"
        Me._lblList_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblList_0.Size = New System.Drawing.Size(52, 13)
        Me._lblList_0.TabIndex = 21
        Me._lblList_0.Text = "List Label"
        Me._lblList_0.Visible = False
        '
        '_lblPremium_0
        '
        Me._lblPremium_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblPremium_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPremium_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPremium_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblPremium_0.Location = New System.Drawing.Point(336, 344)
        Me._lblPremium_0.Name = "_lblPremium_0"
        Me._lblPremium_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPremium_0.Size = New System.Drawing.Size(73, 17)
        Me._lblPremium_0.TabIndex = 20
        Me._lblPremium_0.Text = "Premium:"
        Me._lblPremium_0.Visible = False
        '
        '_lblTotalSumInsured_0
        '
        Me._lblTotalSumInsured_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblTotalSumInsured_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblTotalSumInsured_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblTotalSumInsured_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblTotalSumInsured_0.Location = New System.Drawing.Point(328, 312)
        Me._lblTotalSumInsured_0.Name = "_lblTotalSumInsured_0"
        Me._lblTotalSumInsured_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblTotalSumInsured_0.Size = New System.Drawing.Size(121, 17)
        Me._lblTotalSumInsured_0.TabIndex = 19
        Me._lblTotalSumInsured_0.Text = "Total sum insured:"
        Me._lblTotalSumInsured_0.Visible = False
        '
        '_lblGISLookup_0
        '
        Me._lblGISLookup_0.AutoSize = True
        Me._lblGISLookup_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblGISLookup_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblGISLookup_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblGISLookup_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblGISLookup_0.Location = New System.Drawing.Point(24, 264)
        Me._lblGISLookup_0.Name = "_lblGISLookup_0"
        Me._lblGISLookup_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblGISLookup_0.Size = New System.Drawing.Size(64, 13)
        Me._lblGISLookup_0.TabIndex = 18
        Me._lblGISLookup_0.Text = "GIS Lookup"
        Me._lblGISLookup_0.Visible = False
        '
        '_lblPMLookup_0
        '
        Me._lblPMLookup_0.AutoSize = True
        Me._lblPMLookup_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblPMLookup_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblPMLookup_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblPMLookup_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblPMLookup_0.Location = New System.Drawing.Point(150, 221)
        Me._lblPMLookup_0.Name = "_lblPMLookup_0"
        Me._lblPMLookup_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblPMLookup_0.Size = New System.Drawing.Size(62, 13)
        Me._lblPMLookup_0.TabIndex = 17
        Me._lblPMLookup_0.Text = "PM Lookup"
        Me._lblPMLookup_0.Visible = False
        '
        'lblAccumulation
        '
        Me.lblAccumulation.AutoSize = True
        Me.lblAccumulation.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccumulation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccumulation.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccumulation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccumulation.Location = New System.Drawing.Point(8, 280)
        Me.lblAccumulation.Name = "lblAccumulation"
        Me.lblAccumulation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccumulation.Size = New System.Drawing.Size(71, 13)
        Me.lblAccumulation.TabIndex = 16
        Me.lblAccumulation.Text = "Accumulation"
        Me.lblAccumulation.Visible = False
        '
        '_cboPMLookup_0
        '
        Me._cboPMLookup_0.DefaultItemId = 0
        Me._cboPMLookup_0.FirstItem = ""
        Me._cboPMLookup_0.ItemId = 0
        Me._cboPMLookup_0.ListIndex = -1
        Me._cboPMLookup_0.Location = New System.Drawing.Point(276, 172)
        Me._cboPMLookup_0.Name = "_cboPMLookup_0"
        Me._cboPMLookup_0.PMLookupProductFamily = 1
        Me._cboPMLookup_0.SingleItemId = 0
        Me._cboPMLookup_0.Size = New System.Drawing.Size(153, 21)
        Me._cboPMLookup_0.Sorted = True
        Me._cboPMLookup_0.TabIndex = 65
        Me._cboPMLookup_0.TableName = "None"
        Me._cboPMLookup_0.ToolTipText = ""
        Me._cboPMLookup_0.Visible = False
        Me._cboPMLookup_0.WhereClause = ""
        '
        'uctCLMPayment
        '
        Me.uctCLMPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMPayment.IsOpenClaimNoTrans = False
        Me.uctCLMPayment.Location = New System.Drawing.Point(430, 216)
        Me.uctCLMPayment.Name = "uctCLMPayment"
        Me.uctCLMPayment.RI2007Enabled = False
        Me.uctCLMPayment.ShowCoInsurers = False
        Me.uctCLMPayment.Size = New System.Drawing.Size(82, 47)
        Me.uctCLMPayment.TabIndex = 59
        Me.uctCLMPayment.Visible = False
        Me.uctCLMPayment.WorkClaimID = 0
        Me.uctCLMPayment.WorkClaimPerilId = 0
        '
        'cboAccumulation
        '
        Me.cboAccumulation.AccumulationLevel = 0
        Me.cboAccumulation.DefaultItemId = 0
        Me.cboAccumulation.Enabled_Renamed = True
        Me.cboAccumulation.FirstItem = "()"
        Me.cboAccumulation.ItemId = 0
        Me.cboAccumulation.ListIndex = -1
        Me.cboAccumulation.Location = New System.Drawing.Point(64, 288)
        Me.cboAccumulation.Name = "cboAccumulation"
        Me.cboAccumulation.SingleItemId = 0
        Me.cboAccumulation.Size = New System.Drawing.Size(105, 21)
        Me.cboAccumulation.TabIndex = 1
        Me.cboAccumulation.ToolTipText = ""
        Me.cboAccumulation.Visible = False
        Me.cboAccumulation.WhatsThisHelpID = 0
        Me.cboAccumulation.WhereClause = ""
        '
        'uctCLMReserve
        '
        Me.uctCLMReserve.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCLMReserve.IsOpenClaimNoTrans = False
        Me.uctCLMReserve.Location = New System.Drawing.Point(283, 216)
        Me.uctCLMReserve.Name = "uctCLMReserve"
        Me.uctCLMReserve.ShowCoInsurers = False
        Me.uctCLMReserve.ShowEdit = True
        Me.uctCLMReserve.Size = New System.Drawing.Size(146, 44)
        Me.uctCLMReserve.TabIndex = 58
        Me.uctCLMReserve.Visible = False
        Me.uctCLMReserve.Visible_Renamed = True
        '
        '_txtFormattedText_0
        '
        Me._txtFormattedText_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._txtFormattedText_0.BulletIndent = 250
        Me._txtFormattedText_0.Location = New System.Drawing.Point(12, 98)
        Me._txtFormattedText_0.MaxLength = 2147483647
        Me._txtFormattedText_0.Name = "_txtFormattedText_0"
        Me._txtFormattedText_0.PrinterName = ""
        Me._txtFormattedText_0.ShowToolbar = False
        Me._txtFormattedText_0.Size = New System.Drawing.Size(484, 268)
        Me._txtFormattedText_0.SpellCheck = False
        Me._txtFormattedText_0.TabIndex = 62
        Me._txtFormattedText_0.TextRTF = "{\rtf1\ansi\deff0{\fonttbl{\f0\fnil\fcharset0 Verdana;}}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "\viewkind4\uc1\pard\lan" & _
            "g16393\f0\fs17 UctRichTextBox1\par" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'RiskScreen
        '
        Me.Controls.Add(Me.uctCLMReserve)
        Me.Controls.Add(Me.cboAccumulation)
        Me.Controls.Add(Me.uctCLMPayment)
        Me.Controls.Add(Me._cboPMLookup_0)
        Me.Controls.Add(Me._cboList_0)
        Me.Controls.Add(Me._cboGISLookup_0)
        Me.Controls.Add(Me._PBFindRT1_0)
        Me.Controls.Add(Me.uctCLMPerilRT1)
        Me.Controls.Add(Me._txtMlText_0)
        Me.Controls.Add(Me.uctCLMCaseHeader1)
        Me.Controls.Add(Me._cmdListViewSequenceDown_0)
        Me.Controls.Add(Me._cmdListViewSequenceUp_0)
        Me.Controls.Add(Me._cmdStandardWordingEdit_0)
        Me.Controls.Add(Me._txtAddress6_0)
        Me.Controls.Add(Me._cmdListViewDelete_0)
        Me.Controls.Add(Me._cmdListViewEdit_0)
        Me.Controls.Add(Me._cmdListViewAdd_0)
        Me.Controls.Add(Me._fraFrame_0)
        Me.Controls.Add(Me._cmdStandardWordingDelete_0)
        Me.Controls.Add(Me._cmdStandardWordingAdd_0)
        Me.Controls.Add(Me._chkYesNo_0)
        Me.Controls.Add(Me._cmdAddress_0)
        Me.Controls.Add(Me._txtAddress4_0)
        Me.Controls.Add(Me._txtAddress3_0)
        Me.Controls.Add(Me._txtAddress2_0)
        Me.Controls.Add(Me._txtAddress1_0)
        Me.Controls.Add(Me._cmdPartyCommand_0)
        Me.Controls.Add(Me._txtRate_0)
        Me.Controls.Add(Me._cmdSumInsuredEdit_0)
        Me.Controls.Add(Me._cmdSumInsuredAdd_0)
        Me.Controls.Add(Me._cmdSumInsuredDelete_0)
        Me.Controls.Add(Me._txtPremium_0)
        Me.Controls.Add(Me._cmdStandardWordingUp_0)
        Me.Controls.Add(Me._cmdStandardWordingDown_0)
        Me.Controls.Add(Me._txtText_0)
        Me.Controls.Add(Me.txtCurrency)
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me._cmdPolicyCommand_0)
        Me.Controls.Add(Me._lvwSumInsured_0)
        Me.Controls.Add(Me._txtAddress5_0)
        Me.Controls.Add(Me.TabStrip1)
        Me.Controls.Add(Me._lblStandardWordingMove_0)
        Me.Controls.Add(Me.PBForm)
        Me.Controls.Add(Me._pnlPartyPanel_0)
        Me.Controls.Add(Me._pnlTotalSumInsured_0)
        Me.Controls.Add(Me._pnlPolicyPanel_0)
        Me.Controls.Add(Me._lblComment_0)
        Me.Controls.Add(Me.pnlPosition)
        Me.Controls.Add(Me._lblCheckLabel_0)
        Me.Controls.Add(Me._lblCombo_0)
        Me.Controls.Add(Me._lblRate_0)
        Me.Controls.Add(Me._lblMlText_0)
        Me.Controls.Add(Me._lblText_0)
        Me.Controls.Add(Me._lblList_0)
        Me.Controls.Add(Me._lblPremium_0)
        Me.Controls.Add(Me._lblTotalSumInsured_0)
        Me.Controls.Add(Me._lblGISLookup_0)
        Me.Controls.Add(Me._lblPMLookup_0)
        Me.Controls.Add(Me.lblAccumulation)
        Me.Location = New System.Drawing.Point(3, 3)
        Me.Name = "RiskScreen"
        Me.Size = New System.Drawing.Size(605, 402)
        Me.TabStrip1.ResumeLayout(False)
        Me._TabStrip1_Tab1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializetxtText()
        Me.txtText(0) = _txtText_0
    End Sub
    Sub InitializetxtRate()
        Me.txtRate(0) = _txtRate_0
    End Sub
    Sub InitializetxtPremium()
        Me.txtPremium(0) = _txtPremium_0
    End Sub
    Sub InitializetxtMlText()
        Me.txtMlText(0) = _txtMlText_0
    End Sub
    Sub InitializetxtAddress6()
        Me.txtAddress6(0) = _txtAddress6_0
    End Sub
    Sub InitializetxtAddress5()
        Me.txtAddress5(0) = _txtAddress5_0
    End Sub
    Sub InitializetxtAddress4()
        Me.txtAddress4(0) = _txtAddress4_0
    End Sub
    Sub InitializetxtAddress3()
        Me.txtAddress3(0) = _txtAddress3_0
    End Sub
    Sub InitializetxtAddress2()
        Me.txtAddress2(0) = _txtAddress2_0
    End Sub
    Sub InitializetxtAddress1()
        Me.txtAddress1(0) = _txtAddress1_0
    End Sub
    Sub InitializepnlTotalSumInsured()
        Me.pnlTotalSumInsured(0) = _pnlTotalSumInsured_0
    End Sub
    Sub InitializepnlPolicyPanel()
        Me.pnlPolicyPanel(0) = _pnlPolicyPanel_0
    End Sub
    Sub InitializepnlPartyPanel()
        Me.pnlPartyPanel(0) = _pnlPartyPanel_0
    End Sub
    Sub InitializelvwSumInsured()
        Me.lvwSumInsured(0) = _lvwSumInsured_0
    End Sub
    Sub InitializelvwStandardWording()
        Me.lvwStandardWording(0) = _lvwStandardWording_0
    End Sub
    Sub InitializelvwListView()
        Me.lvwListView(0) = _lvwListView_0
    End Sub
    Sub InitializelblTotalSumInsured()
        Me.lblTotalSumInsured(0) = _lblTotalSumInsured_0
    End Sub
    Sub InitializelblText()
        Me.lblText(0) = _lblText_0
    End Sub
    Sub InitializelblStandardWordingMove()
        Me.lblStandardWordingMove(0) = _lblStandardWordingMove_0
    End Sub
    Sub InitializelblRate()
        Me.lblRate(0) = _lblRate_0
    End Sub
    Sub InitializelblPremium()
        Me.lblPremium(0) = _lblPremium_0
    End Sub
    Sub InitializelblPMLookup()
        Me.lblPMLookup(0) = _lblPMLookup_0
    End Sub
    Sub InitializelblMlText()
        Me.lblMlText(0) = _lblMlText_0
    End Sub
    Sub InitializelblList()
        Me.lblList(0) = _lblList_0
    End Sub
    Sub InitializelblGISLookup()
        Me.lblGISLookup(0) = _lblGISLookup_0
    End Sub
    Sub InitializelblComment()
        Me.lblComment(0) = _lblComment_0
    End Sub
    Sub InitializelblCombo()
        Me.lblCombo(0) = _lblCombo_0
    End Sub
    Sub InitializelblCheckLabel()
        Me.lblCheckLabel(0) = _lblCheckLabel_0
    End Sub
    Sub InitializefraFrame()
        Me.fraFrame(0) = _fraFrame_0
    End Sub
    Sub InitializecmdSumInsuredEdit()
        Me.cmdSumInsuredEdit(0) = _cmdSumInsuredEdit_0
    End Sub
    Sub InitializecmdSumInsuredDelete()
        Me.cmdSumInsuredDelete(0) = _cmdSumInsuredDelete_0
    End Sub
    Sub InitializecmdSumInsuredAdd()
        Me.cmdSumInsuredAdd(0) = _cmdSumInsuredAdd_0
    End Sub
    Sub InitializecmdStandardWordingUp()
        Me.cmdStandardWordingUp(0) = _cmdStandardWordingUp_0
    End Sub
    Sub InitializecmdStandardWordingEdit()
        Me.cmdStandardWordingEdit(0) = _cmdStandardWordingEdit_0
    End Sub
    Sub InitializecmdStandardWordingDown()
        Me.cmdStandardWordingDown(0) = _cmdStandardWordingDown_0
    End Sub
    Sub InitializecmdStandardWordingDelete()
        Me.cmdStandardWordingDelete(0) = _cmdStandardWordingDelete_0
    End Sub
    Sub InitializecmdStandardWordingAdd()
        Me.cmdStandardWordingAdd(0) = _cmdStandardWordingAdd_0
    End Sub
    Sub InitializecmdPolicyCommand()
        Me.cmdPolicyCommand(0) = _cmdPolicyCommand_0
    End Sub
    Sub InitializecmdPartyCommand()
        Me.cmdPartyCommand(0) = _cmdPartyCommand_0
    End Sub
    Sub InitializecmdListViewSequenceUp()
        Me.cmdListViewSequenceUp(0) = _cmdListViewSequenceUp_0
    End Sub
    Sub InitializecmdListViewSequenceDown()
        Me.cmdListViewSequenceDown(0) = _cmdListViewSequenceDown_0
    End Sub
    Sub InitializecmdListViewEdit()
        Me.cmdListViewEdit(0) = _cmdListViewEdit_0
    End Sub
    Sub InitializecmdListViewDelete()
        Me.cmdListViewDelete(0) = _cmdListViewDelete_0
    End Sub
    Sub InitializecmdListViewAdd()
        Me.cmdListViewAdd(0) = _cmdListViewAdd_0
    End Sub
    Sub InitializecmdAddress()
        Me.cmdAddress(0) = _cmdAddress_0
    End Sub
    Sub InitializechkYesNo()
        Me.chkYesNo(0) = _chkYesNo_0
    End Sub
    Sub InitializecboPMLookup()
        Me.cboPMLookup(0) = _cboPMLookup_0
    End Sub
    Sub InitializecboList()
        Me.cboList(0) = _cboList_0
    End Sub
    Sub InitializecboGISLookup()
        Me.cboGISLookup(0) = _cboGISLookup_0
    End Sub
    Sub InitializePBFindRT1()
        Me.PBFindRT1(0) = _PBFindRT1_0
    End Sub
    'Friend WithEvents uctCLMReserve As uctClaimReserves.uctClaimReserve
    Friend WithEvents uctCLMPerilRT1 As uctCLMPerilRTControl.uctCLMPerilRT
    Friend WithEvents _PBFindRT1_0 As uctPBFindRT.PBFindRT
    Friend WithEvents _cboGISLookup_0 As uctGISUserDefLookupControl.cboGISLookup
    Friend WithEvents _cboList_0 As PMListMgrDropdown.uctDropdown
    Friend WithEvents _cboPMLookup_0 As PMLookupControl.cboPMLookup
    Friend WithEvents uctCLMPayment As uctCLMPaymentControl.uctCLMPayment1
    Friend WithEvents cboAccumulation As uctAccumulationLookup.cboAccumulation
    Friend WithEvents uctCLMReserve As uctCLMReserveControl.uctCLMReserve
    Friend WithEvents _txtFormattedText_0 As uctSIRRTFControl.uctRichTextBox

#End Region
#Region "Upgrade Support"
    <System.Runtime.InteropServices.ProgId("DebugModeChangedEventArgs_NET.DebugModeChangedEventArgs")> _
    Public NotInheritable Class DebugModeChangedEventArgs
        Inherits System.EventArgs
        Public bDebugEnabled As Boolean
        Friend Sub New(ByVal bDebugEnabled As Boolean)
            MyBase.New()
            Me.bDebugEnabled = bDebugEnabled
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("MouseUpEventArgs_NET.MouseUpEventArgs")> _
    Public NotInheritable Class MouseUpEventArgs
        Inherits System.EventArgs
        Public Button As Integer
        Public Shift As Integer
        Public x As Single
        Public y As Single
        Friend Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
            MyBase.New()
            Me.Button = Button
            Me.Shift = Shift
            Me.x = x
            Me.y = y
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("MouseMoveEventArgs_NET.MouseMoveEventArgs")> _
    Public NotInheritable Class MouseMoveEventArgs
        Inherits System.EventArgs
        Public Button As Integer
        Public Shift As Integer
        Public x As Single
        Public y As Single
        Friend Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
            MyBase.New()
            Me.Button = Button
            Me.Shift = Shift
            Me.x = x
            Me.y = y
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("MouseDownEventArgs_NET.MouseDownEventArgs")> _
    Public NotInheritable Class MouseDownEventArgs
        Inherits System.EventArgs
        Public Button As Integer
        Public Shift As Integer
        Public x As Single
        Public y As Single
        Friend Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
            MyBase.New()
            Me.Button = Button
            Me.Shift = Shift
            Me.x = x
            Me.y = y
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("KeyUpEventArgs_NET.KeyUpEventArgs")> _
    Public NotInheritable Class KeyUpEventArgs
        Inherits System.EventArgs
        Public KeyCode As Integer
        Public Shift As Integer
        Friend Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
            MyBase.New()
            Me.KeyCode = KeyCode
            Me.Shift = Shift
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("KeyPressEventArgs_NET.KeyPressEventArgs")> _
    Public NotInheritable Class KeyPressEventArgs
        Inherits System.EventArgs
        Public KeyAscii As Integer
        Friend Sub New(ByRef KeyAscii As Integer)
            MyBase.New()
            Me.KeyAscii = KeyAscii
        End Sub
    End Class
    <System.Runtime.InteropServices.ProgId("KeyDownEventArgs_NET.KeyDownEventArgs")> _
    Public NotInheritable Class KeyDownEventArgs
        Inherits System.EventArgs
        Public KeyCode As Integer
        Public Shift As Integer
        Friend Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
            MyBase.New()
            Me.KeyCode = KeyCode
            Me.Shift = Shift
        End Sub
    End Class
#End Region
End Class
