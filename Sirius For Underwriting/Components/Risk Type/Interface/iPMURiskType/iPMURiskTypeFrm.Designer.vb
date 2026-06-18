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
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public WithEvents txtFormatDate As System.Windows.Forms.TextBox
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents lblSectionMask As System.Windows.Forms.Label
    Public WithEvents lblStampDutyRate1 As System.Windows.Forms.Label
    Public WithEvents lblStampDutyRate2 As System.Windows.Forms.Label
    Public WithEvents lblPrimarySort As System.Windows.Forms.Label
    Public WithEvents lblSecondarySort As System.Windows.Forms.Label
    Public WithEvents lblGISScreenId As System.Windows.Forms.Label
    Public WithEvents lblHeaderClause As System.Windows.Forms.Label
    Public WithEvents lblHeaderClauseLabel As System.Windows.Forms.Label
    Public WithEvents cboGISScreenID As System.Windows.Forms.ComboBox
    Public WithEvents chkSuppressTaxes As System.Windows.Forms.CheckBox
    Public WithEvents chkShareWithReInsurer As System.Windows.Forms.CheckBox
    Public WithEvents chkShareWithCoInsurer As System.Windows.Forms.CheckBox
    Public WithEvents chkSuppressPrivateText As System.Windows.Forms.CheckBox
    Public WithEvents chkSuppressPublicText As System.Windows.Forms.CheckBox
    Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
    Public WithEvents txtCode As System.Windows.Forms.TextBox
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents txtSectionMask As System.Windows.Forms.TextBox
    Public WithEvents txtStampDutyRate1 As System.Windows.Forms.TextBox
    Public WithEvents txtStampDutyRate2 As System.Windows.Forms.TextBox
    Public WithEvents txtPrimarySort As System.Windows.Forms.TextBox
    Public WithEvents txtSecondarySort As System.Windows.Forms.TextBox
    Public WithEvents cmdHeaderClause As System.Windows.Forms.Button
    Public WithEvents txtAccumulationLevel As System.Windows.Forms.TextBox
    Public WithEvents chkDisplayClaimReinsurance As System.Windows.Forms.CheckBox
    Public WithEvents chkDisplayReinsurance As System.Windows.Forms.CheckBox
    Public WithEvents chkDeferredRI As System.Windows.Forms.CheckBox
    Public WithEvents chkIsAutoReinsured As System.Windows.Forms.CheckBox
    Public WithEvents cmdDeferredRI As System.Windows.Forms.Button
    Public WithEvents cmdRILimits As System.Windows.Forms.Button
    Public WithEvents cmdRIModel As System.Windows.Forms.Button
    Public WithEvents fraReinsurance As System.Windows.Forms.GroupBox
    Public WithEvents chkClaimsIsPostTaxes As System.Windows.Forms.CheckBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents uctSIRSelectClauses As uctSCControl.uctSIRSelectClauses
    Private WithEvents _tabMainTab_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents cmdSelectRiskTypeGroup As System.Windows.Forms.Button
    Private WithEvents _lvwRiskTypeGroup_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRiskTypeGroup_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRiskTypeGroup_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwRiskTypeGroup As System.Windows.Forms.ListView
    Private WithEvents _tabMainTab_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents cmdDeleteRule As System.Windows.Forms.Button
    Public WithEvents cmdEditRule As System.Windows.Forms.Button
    Public WithEvents cmdAddRule As System.Windows.Forms.Button
    Private WithEvents _lvwRules_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRules_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRules_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRules_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwRules As System.Windows.Forms.ListView
    Public WithEvents fmeRating As System.Windows.Forms.GroupBox
    Public WithEvents cmdAddRenRule As System.Windows.Forms.Button
    Public WithEvents cmdEditRenRule As System.Windows.Forms.Button
    Public WithEvents cmdDeleteRenRule As System.Windows.Forms.Button
    Private WithEvents _lvwRenewalRule_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalRule_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalRule_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwRenewalRule_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwRenewalRule As System.Windows.Forms.ListView
	Public WithEvents fmeRenewal As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage3 As System.Windows.Forms.TabPage
	Public WithEvents chkAllowEditRatingSectionThisPremium As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowEditRatingSectionSumInsured As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowEditRatingSectionRate As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowEditRatingSectionRateType As System.Windows.Forms.CheckBox
	Public WithEvents fraEdit As System.Windows.Forms.GroupBox
	Public WithEvents chkAllowDeleteRatingSection As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowEditRatingSection As System.Windows.Forms.CheckBox
	Public WithEvents chkAllowAddRatingSection As System.Windows.Forms.CheckBox
	Public WithEvents fraAllow As System.Windows.Forms.GroupBox
    Private WithEvents _tabMainTab_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents uctPickListRatingSections As uctPickList.PickList
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public WithEvents cboClaimsCoverBasis As PMLookupControl.cboPMLookup
    Public WithEvents cboClaimsTypeBasis As PMLookupControl.cboPMLookup
    'TODOLIST-Commented the code as conflicting with icon display image
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApply = New System.Windows.Forms.Button()
        Me.txtFormatDate = New System.Windows.Forms.TextBox()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraCover = New System.Windows.Forms.GroupBox()
        Me.chkAttachClaimOutsideOfPolicyPeriod = New System.Windows.Forms.CheckBox()
        Me.lblClaimsCoverBasis = New System.Windows.Forms.Label()
        Me.lblClaimsTypeBasis = New System.Windows.Forms.Label()
        Me.cboClaimsCoverBasis = New PMLookupControl.cboPMLookup()
        Me.cboClaimsTypeBasis = New PMLookupControl.cboPMLookup()
        Me.lblAccumulationLevel = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.lblSectionMask = New System.Windows.Forms.Label()
        Me.lblStampDutyRate1 = New System.Windows.Forms.Label()
        Me.lblStampDutyRate2 = New System.Windows.Forms.Label()
        Me.lblPrimarySort = New System.Windows.Forms.Label()
        Me.lblSecondarySort = New System.Windows.Forms.Label()
        Me.lblGISScreenId = New System.Windows.Forms.Label()
        Me.lblHeaderClause = New System.Windows.Forms.Label()
        Me.lblTrailerClause = New System.Windows.Forms.Label()
        Me.lblHeaderClauseLabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboGISScreenID = New System.Windows.Forms.ComboBox()
        Me.chkSuppressTaxes = New System.Windows.Forms.CheckBox()
        Me.chkShareWithReInsurer = New System.Windows.Forms.CheckBox()
        Me.chkShareWithCoInsurer = New System.Windows.Forms.CheckBox()
        Me.chkSuppressPrivateText = New System.Windows.Forms.CheckBox()
        Me.chkSuppressPublicText = New System.Windows.Forms.CheckBox()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtSectionMask = New System.Windows.Forms.TextBox()
        Me.txtStampDutyRate1 = New System.Windows.Forms.TextBox()
        Me.txtStampDutyRate2 = New System.Windows.Forms.TextBox()
        Me.txtPrimarySort = New System.Windows.Forms.TextBox()
        Me.txtSecondarySort = New System.Windows.Forms.TextBox()
        Me.cmdHeaderClause = New System.Windows.Forms.Button()
        Me.cmdTrailerClause = New System.Windows.Forms.Button()
        Me.txtAccumulationLevel = New System.Windows.Forms.TextBox()
        Me.fraReinsurance = New System.Windows.Forms.GroupBox()
        Me.chkDisplayClaimReinsurance = New System.Windows.Forms.CheckBox()
        Me.chkDisplayReinsurance = New System.Windows.Forms.CheckBox()
        Me.chkDeferredRI = New System.Windows.Forms.CheckBox()
        Me.chkIsAutoReinsured = New System.Windows.Forms.CheckBox()
        Me.cmdDeferredRI = New System.Windows.Forms.Button()
        Me.cmdRILimits = New System.Windows.Forms.Button()
        Me.cmdRIModel = New System.Windows.Forms.Button()
        Me.chkClaimsIsPostTaxes = New System.Windows.Forms.CheckBox()
        Me._tabMainTab_TabPage1 = New System.Windows.Forms.TabPage()
        Me.uctSIRSelectClauses = New uctSCControl.uctSIRSelectClauses()
        Me._tabMainTab_TabPage2 = New System.Windows.Forms.TabPage()
        Me.cmdSelectRiskTypeGroup = New System.Windows.Forms.Button()
        Me.lvwRiskTypeGroup = New System.Windows.Forms.ListView()
        Me._lvwRiskTypeGroup_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRiskTypeGroup_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRiskTypeGroup_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me._tabMainTab_TabPage3 = New System.Windows.Forms.TabPage()
        Me.frmLapse = New System.Windows.Forms.GroupBox()
        Me.cmdAddRenLapseRule = New System.Windows.Forms.Button()
        Me.cmdEditRenLapseRule = New System.Windows.Forms.Button()
        Me.cmdDeleteRenLapseRule = New System.Windows.Forms.Button()
        Me.lvwRenewalLapseRule = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fmeRating = New System.Windows.Forms.GroupBox()
        Me.cmdDeleteRule = New System.Windows.Forms.Button()
        Me.cmdEditRule = New System.Windows.Forms.Button()
        Me.cmdAddRule = New System.Windows.Forms.Button()
        Me.lvwRules = New System.Windows.Forms.ListView()
        Me._lvwRules_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRules_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRules_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRules_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fmeRenewal = New System.Windows.Forms.GroupBox()
        Me.cmdAddRenRule = New System.Windows.Forms.Button()
        Me.cmdEditRenRule = New System.Windows.Forms.Button()
        Me.cmdDeleteRenRule = New System.Windows.Forms.Button()
        Me.lvwRenewalRule = New System.Windows.Forms.ListView()
        Me._lvwRenewalRule_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRenewalRule_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRenewalRule_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwRenewalRule_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._tabMainTab_TabPage4 = New System.Windows.Forms.TabPage()
        Me.fraAllow = New System.Windows.Forms.GroupBox()
        Me.fraEdit = New System.Windows.Forms.GroupBox()
        Me.chkAllowEditRatingSectionThisPremium = New System.Windows.Forms.CheckBox()
        Me.chkAllowEditRatingSectionSumInsured = New System.Windows.Forms.CheckBox()
        Me.chkAllowEditRatingSectionRate = New System.Windows.Forms.CheckBox()
        Me.chkAllowEditRatingSectionRateType = New System.Windows.Forms.CheckBox()
        Me.chkAllowDeleteRatingSection = New System.Windows.Forms.CheckBox()
        Me.chkAllowEditRatingSection = New System.Windows.Forms.CheckBox()
        Me.chkAllowAddRatingSection = New System.Windows.Forms.CheckBox()
        Me.uctPickListRatingSections = New uctPickList.PickList()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraCover.SuspendLayout()
        Me.fraReinsurance.SuspendLayout()
        Me._tabMainTab_TabPage1.SuspendLayout()
        Me._tabMainTab_TabPage2.SuspendLayout()
        Me._tabMainTab_TabPage3.SuspendLayout()
        Me.frmLapse.SuspendLayout()
        Me.fmeRating.SuspendLayout()
        Me.fmeRenewal.SuspendLayout()
        Me._tabMainTab_TabPage4.SuspendLayout()
        Me.fraAllow.SuspendLayout()
        Me.fraEdit.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(864, 562)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(110, 22)
        Me.cmdApply.TabIndex = 24
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = false
        '
        'txtFormatDate
        '
        Me.txtFormatDate.AcceptsReturn = true
        Me.txtFormatDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtFormatDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatDate.Location = New System.Drawing.Point(266, 594)
        Me.txtFormatDate.MaxLength = 0
        Me.txtFormatDate.Name = "txtFormatDate"
        Me.txtFormatDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatDate.Size = New System.Drawing.Size(132, 20)
        Me.txtFormatDate.TabIndex = 40
        Me.txtFormatDate.Visible = false
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage1)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage2)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage3)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage4)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(119, 19)
        Me.tabMainTab.Location = New System.Drawing.Point(12, 8)
        Me.tabMainTab.Multiline = true
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(960, 552)
        Me.tabMainTab.TabIndex = 31
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraCover)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAccumulationLevel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSectionMask)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStampDutyRate1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStampDutyRate2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPrimarySort)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSecondarySort)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblGISScreenId)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblHeaderClause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTrailerClause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblHeaderClauseLabel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboGISScreenID)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkSuppressTaxes)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkShareWithReInsurer)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkShareWithCoInsurer)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkSuppressPrivateText)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkSuppressPublicText)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtSectionMask)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtStampDutyRate1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtStampDutyRate2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPrimarySort)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtSecondarySort)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdHeaderClause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdTrailerClause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAccumulationLevel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraReinsurance)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkClaimsIsPostTaxes)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(952, 525)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1-Risk Type"
        '
        'fraCover
        '
        Me.fraCover.Controls.Add(Me.chkAttachClaimOutsideOfPolicyPeriod)
        Me.fraCover.Controls.Add(Me.lblClaimsCoverBasis)
        Me.fraCover.Controls.Add(Me.lblClaimsTypeBasis)
        Me.fraCover.Controls.Add(Me.cboClaimsCoverBasis)
        Me.fraCover.Controls.Add(Me.cboClaimsTypeBasis)
        Me.fraCover.Location = New System.Drawing.Point(4, 442)
        Me.fraCover.Name = "fraCover"
        Me.fraCover.Size = New System.Drawing.Size(962, 80)
        Me.fraCover.TabIndex = 56
        Me.fraCover.TabStop = false
        Me.fraCover.Text = "Cover"
        '
        'chkAttachClaimOutsideOfPolicyPeriod
        '
        Me.chkAttachClaimOutsideOfPolicyPeriod.BackColor = System.Drawing.SystemColors.Control
        Me.chkAttachClaimOutsideOfPolicyPeriod.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAttachClaimOutsideOfPolicyPeriod.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAttachClaimOutsideOfPolicyPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAttachClaimOutsideOfPolicyPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAttachClaimOutsideOfPolicyPeriod.Location = New System.Drawing.Point(452, 51)
        Me.chkAttachClaimOutsideOfPolicyPeriod.Name = "chkAttachClaimOutsideOfPolicyPeriod"
        Me.chkAttachClaimOutsideOfPolicyPeriod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAttachClaimOutsideOfPolicyPeriod.Size = New System.Drawing.Size(424, 21)
        Me.chkAttachClaimOutsideOfPolicyPeriod.TabIndex = 56
        Me.chkAttachClaimOutsideOfPolicyPeriod.Text = "Attach Claim Outside of Policy Period: "
        Me.chkAttachClaimOutsideOfPolicyPeriod.UseVisualStyleBackColor = false
        '
        'lblClaimsCoverBasis
        '
        Me.lblClaimsCoverBasis.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimsCoverBasis.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimsCoverBasis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblClaimsCoverBasis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimsCoverBasis.Location = New System.Drawing.Point(452, 23)
        Me.lblClaimsCoverBasis.Name = "lblClaimsCoverBasis"
        Me.lblClaimsCoverBasis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimsCoverBasis.Size = New System.Drawing.Size(187, 19)
        Me.lblClaimsCoverBasis.TabIndex = 46
        Me.lblClaimsCoverBasis.Text = "Cover Verification Basis:"
        '
        'lblClaimsTypeBasis
        '
        Me.lblClaimsTypeBasis.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaimsTypeBasis.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaimsTypeBasis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblClaimsTypeBasis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaimsTypeBasis.Location = New System.Drawing.Point(9, 23)
        Me.lblClaimsTypeBasis.Name = "lblClaimsTypeBasis"
        Me.lblClaimsTypeBasis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaimsTypeBasis.Size = New System.Drawing.Size(169, 19)
        Me.lblClaimsTypeBasis.TabIndex = 45
        Me.lblClaimsTypeBasis.Text = "Claims Type Basis:"
        '
        'cboClaimsCoverBasis
        '
        Me.cboClaimsCoverBasis.DefaultItemId = 0
        Me.cboClaimsCoverBasis.FirstItem = ""
        Me.cboClaimsCoverBasis.ItemId = 0
        Me.cboClaimsCoverBasis.ListIndex = -1
        Me.cboClaimsCoverBasis.Location = New System.Drawing.Point(648, 21)
        Me.cboClaimsCoverBasis.Name = "cboClaimsCoverBasis"
        Me.cboClaimsCoverBasis.PMLookupProductFamily = 1
        Me.cboClaimsCoverBasis.SingleItemId = 0
        Me.cboClaimsCoverBasis.Size = New System.Drawing.Size(230, 21)
        Me.cboClaimsCoverBasis.SortColumnName = ""
        Me.cboClaimsCoverBasis.Sorted = true
        Me.cboClaimsCoverBasis.TabIndex = 0
        Me.cboClaimsCoverBasis.TableName = "claims_cover_basis"
        Me.cboClaimsCoverBasis.ToolTipText = ""
        Me.cboClaimsCoverBasis.WhereClause = ""
        '
        'cboClaimsTypeBasis
        '
        Me.cboClaimsTypeBasis.DefaultItemId = 0
        Me.cboClaimsTypeBasis.FirstItem = ""
        Me.cboClaimsTypeBasis.ItemId = 0
        Me.cboClaimsTypeBasis.ListIndex = -1
        Me.cboClaimsTypeBasis.Location = New System.Drawing.Point(194, 21)
        Me.cboClaimsTypeBasis.Name = "cboClaimsTypeBasis"
        Me.cboClaimsTypeBasis.PMLookupProductFamily = 1
        Me.cboClaimsTypeBasis.SingleItemId = 0
        Me.cboClaimsTypeBasis.Size = New System.Drawing.Size(229, 21)
        Me.cboClaimsTypeBasis.SortColumnName = ""
        Me.cboClaimsTypeBasis.Sorted = true
        Me.cboClaimsTypeBasis.TabIndex = 0
        Me.cboClaimsTypeBasis.TableName = "claims_type_basis"
        Me.cboClaimsTypeBasis.ToolTipText = ""
        Me.cboClaimsTypeBasis.WhereClause = ""
        '
        'lblAccumulationLevel
        '
        Me.lblAccumulationLevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccumulationLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccumulationLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblAccumulationLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccumulationLevel.Location = New System.Drawing.Point(24, 186)
        Me.lblAccumulationLevel.Name = "lblAccumulationLevel"
        Me.lblAccumulationLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccumulationLevel.Size = New System.Drawing.Size(192, 19)
        Me.lblAccumulationLevel.TabIndex = 28
        Me.lblAccumulationLevel.Text = "Accumulation level:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(24, 52)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(192, 19)
        Me.lblEffectiveDate.TabIndex = 32
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'lblCode
        '
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(24, 19)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(192, 19)
        Me.lblCode.TabIndex = 33
        Me.lblCode.Text = "Code:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(24, 85)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(192, 19)
        Me.lblDescription.TabIndex = 34
        Me.lblDescription.Text = "Description:"
        '
        'lblSectionMask
        '
        Me.lblSectionMask.BackColor = System.Drawing.SystemColors.Control
        Me.lblSectionMask.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSectionMask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblSectionMask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSectionMask.Location = New System.Drawing.Point(596, 216)
        Me.lblSectionMask.Name = "lblSectionMask"
        Me.lblSectionMask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSectionMask.Size = New System.Drawing.Size(180, 19)
        Me.lblSectionMask.TabIndex = 35
        Me.lblSectionMask.Text = "Section Mask:"
        '
        'lblStampDutyRate1
        '
        Me.lblStampDutyRate1.BackColor = System.Drawing.SystemColors.Control
        Me.lblStampDutyRate1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStampDutyRate1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblStampDutyRate1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStampDutyRate1.Location = New System.Drawing.Point(24, 318)
        Me.lblStampDutyRate1.Name = "lblStampDutyRate1"
        Me.lblStampDutyRate1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStampDutyRate1.Size = New System.Drawing.Size(192, 19)
        Me.lblStampDutyRate1.TabIndex = 36
        Me.lblStampDutyRate1.Text = "Stamp Duty Rate 1:"
        '
        'lblStampDutyRate2
        '
        Me.lblStampDutyRate2.BackColor = System.Drawing.SystemColors.Control
        Me.lblStampDutyRate2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStampDutyRate2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblStampDutyRate2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStampDutyRate2.Location = New System.Drawing.Point(24, 351)
        Me.lblStampDutyRate2.Name = "lblStampDutyRate2"
        Me.lblStampDutyRate2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStampDutyRate2.Size = New System.Drawing.Size(192, 19)
        Me.lblStampDutyRate2.TabIndex = 37
        Me.lblStampDutyRate2.Text = "Stamp Duty Rate 2:"
        '
        'lblPrimarySort
        '
        Me.lblPrimarySort.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrimarySort.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrimarySort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblPrimarySort.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrimarySort.Location = New System.Drawing.Point(24, 252)
        Me.lblPrimarySort.Name = "lblPrimarySort"
        Me.lblPrimarySort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrimarySort.Size = New System.Drawing.Size(192, 19)
        Me.lblPrimarySort.TabIndex = 38
        Me.lblPrimarySort.Text = "Primary Sort:"
        '
        'lblSecondarySort
        '
        Me.lblSecondarySort.BackColor = System.Drawing.SystemColors.Control
        Me.lblSecondarySort.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSecondarySort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblSecondarySort.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSecondarySort.Location = New System.Drawing.Point(24, 285)
        Me.lblSecondarySort.Name = "lblSecondarySort"
        Me.lblSecondarySort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSecondarySort.Size = New System.Drawing.Size(192, 19)
        Me.lblSecondarySort.TabIndex = 39
        Me.lblSecondarySort.Text = "Secondary Sort:"
        '
        'lblGISScreenId
        '
        Me.lblGISScreenId.BackColor = System.Drawing.SystemColors.Control
        Me.lblGISScreenId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGISScreenId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblGISScreenId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGISScreenId.Location = New System.Drawing.Point(24, 219)
        Me.lblGISScreenId.Name = "lblGISScreenId"
        Me.lblGISScreenId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGISScreenId.Size = New System.Drawing.Size(192, 19)
        Me.lblGISScreenId.TabIndex = 42
        Me.lblGISScreenId.Text = "Associated Screen:"
        '
        'lblHeaderClause
        '
        Me.lblHeaderClause.BackColor = System.Drawing.SystemColors.Control
        Me.lblHeaderClause.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblHeaderClause.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHeaderClause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblHeaderClause.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHeaderClause.Location = New System.Drawing.Point(225, 381)
        Me.lblHeaderClause.Name = "lblHeaderClause"
        Me.lblHeaderClause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHeaderClause.Size = New System.Drawing.Size(203, 21)
        Me.lblHeaderClause.TabIndex = 9
        '
        'lblTrailerClause
        '
        Me.lblTrailerClause.BackColor = System.Drawing.SystemColors.Control
        Me.lblTrailerClause.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTrailerClause.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTrailerClause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblTrailerClause.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTrailerClause.Location = New System.Drawing.Point(225, 418)
        Me.lblTrailerClause.Name = "lblTrailerClause"
        Me.lblTrailerClause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTrailerClause.Size = New System.Drawing.Size(203, 21)
        Me.lblTrailerClause.TabIndex = 11
        '
        'lblHeaderClauseLabel
        '
        Me.lblHeaderClauseLabel.BackColor = System.Drawing.SystemColors.Control
        Me.lblHeaderClauseLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHeaderClauseLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblHeaderClauseLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHeaderClauseLabel.Location = New System.Drawing.Point(24, 384)
        Me.lblHeaderClauseLabel.Name = "lblHeaderClauseLabel"
        Me.lblHeaderClauseLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHeaderClauseLabel.Size = New System.Drawing.Size(123, 19)
        Me.lblHeaderClauseLabel.TabIndex = 43
        Me.lblHeaderClauseLabel.Text = "Header clause:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(24, 417)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(111, 19)
        Me.Label1.TabIndex = 44
        Me.Label1.Text = "Trailer clause:"
        '
        'cboGISScreenID
        '
        Me.cboGISScreenID.BackColor = System.Drawing.SystemColors.Window
        Me.cboGISScreenID.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboGISScreenID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGISScreenID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cboGISScreenID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboGISScreenID.Location = New System.Drawing.Point(225, 217)
        Me.cboGISScreenID.Name = "cboGISScreenID"
        Me.cboGISScreenID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboGISScreenID.Size = New System.Drawing.Size(270, 21)
        Me.cboGISScreenID.TabIndex = 4
        '
        'chkSuppressTaxes
        '
        Me.chkSuppressTaxes.BackColor = System.Drawing.SystemColors.Control
        Me.chkSuppressTaxes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSuppressTaxes.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSuppressTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkSuppressTaxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSuppressTaxes.Location = New System.Drawing.Point(593, 145)
        Me.chkSuppressTaxes.Name = "chkSuppressTaxes"
        Me.chkSuppressTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSuppressTaxes.Size = New System.Drawing.Size(320, 21)
        Me.chkSuppressTaxes.TabIndex = 17
        Me.chkSuppressTaxes.Text = "Suppress Taxes:"
        Me.chkSuppressTaxes.UseVisualStyleBackColor = false
        '
        'chkShareWithReInsurer
        '
        Me.chkShareWithReInsurer.BackColor = System.Drawing.SystemColors.Control
        Me.chkShareWithReInsurer.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkShareWithReInsurer.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShareWithReInsurer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkShareWithReInsurer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShareWithReInsurer.Location = New System.Drawing.Point(593, 49)
        Me.chkShareWithReInsurer.Name = "chkShareWithReInsurer"
        Me.chkShareWithReInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShareWithReInsurer.Size = New System.Drawing.Size(320, 21)
        Me.chkShareWithReInsurer.TabIndex = 14
        Me.chkShareWithReInsurer.Text = "Share Tax with Reinsurer:"
        Me.chkShareWithReInsurer.UseVisualStyleBackColor = false
        '
        'chkShareWithCoInsurer
        '
        Me.chkShareWithCoInsurer.BackColor = System.Drawing.SystemColors.Control
        Me.chkShareWithCoInsurer.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkShareWithCoInsurer.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShareWithCoInsurer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkShareWithCoInsurer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShareWithCoInsurer.Location = New System.Drawing.Point(595, 17)
        Me.chkShareWithCoInsurer.Name = "chkShareWithCoInsurer"
        Me.chkShareWithCoInsurer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShareWithCoInsurer.Size = New System.Drawing.Size(320, 21)
        Me.chkShareWithCoInsurer.TabIndex = 13
        Me.chkShareWithCoInsurer.Text = "Share Tax with Coinsurer:"
        Me.chkShareWithCoInsurer.UseVisualStyleBackColor = false
        '
        'chkSuppressPrivateText
        '
        Me.chkSuppressPrivateText.BackColor = System.Drawing.SystemColors.Control
        Me.chkSuppressPrivateText.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSuppressPrivateText.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSuppressPrivateText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkSuppressPrivateText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSuppressPrivateText.Location = New System.Drawing.Point(593, 113)
        Me.chkSuppressPrivateText.Name = "chkSuppressPrivateText"
        Me.chkSuppressPrivateText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSuppressPrivateText.Size = New System.Drawing.Size(320, 21)
        Me.chkSuppressPrivateText.TabIndex = 16
        Me.chkSuppressPrivateText.Text = "Suppress Private Text:"
        Me.chkSuppressPrivateText.UseVisualStyleBackColor = false
        '
        'chkSuppressPublicText
        '
        Me.chkSuppressPublicText.BackColor = System.Drawing.SystemColors.Control
        Me.chkSuppressPublicText.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSuppressPublicText.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSuppressPublicText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkSuppressPublicText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSuppressPublicText.Location = New System.Drawing.Point(593, 81)
        Me.chkSuppressPublicText.Name = "chkSuppressPublicText"
        Me.chkSuppressPublicText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSuppressPublicText.Size = New System.Drawing.Size(320, 21)
        Me.chkSuppressPublicText.TabIndex = 15
        Me.chkSuppressPublicText.Text = "Suppress Public Text:"
        Me.chkSuppressPublicText.UseVisualStyleBackColor = false
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = true
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(225, 50)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(270, 20)
        Me.txtEffectiveDate.TabIndex = 1
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = true
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(225, 17)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(153, 20)
        Me.txtCode.TabIndex = 0
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = true
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(225, 83)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Multiline = true
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(270, 85)
        Me.txtDescription.TabIndex = 2
        '
        'txtSectionMask
        '
        Me.txtSectionMask.AcceptsReturn = true
        Me.txtSectionMask.BackColor = System.Drawing.SystemColors.Window
        Me.txtSectionMask.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSectionMask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtSectionMask.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSectionMask.Location = New System.Drawing.Point(792, 213)
        Me.txtSectionMask.MaxLength = 0
        Me.txtSectionMask.Name = "txtSectionMask"
        Me.txtSectionMask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSectionMask.Size = New System.Drawing.Size(68, 20)
        Me.txtSectionMask.TabIndex = 18
        '
        'txtStampDutyRate1
        '
        Me.txtStampDutyRate1.AcceptsReturn = true
        Me.txtStampDutyRate1.BackColor = System.Drawing.SystemColors.Window
        Me.txtStampDutyRate1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStampDutyRate1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtStampDutyRate1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStampDutyRate1.Location = New System.Drawing.Point(225, 316)
        Me.txtStampDutyRate1.MaxLength = 0
        Me.txtStampDutyRate1.Name = "txtStampDutyRate1"
        Me.txtStampDutyRate1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStampDutyRate1.Size = New System.Drawing.Size(153, 20)
        Me.txtStampDutyRate1.TabIndex = 7
        '
        'txtStampDutyRate2
        '
        Me.txtStampDutyRate2.AcceptsReturn = true
        Me.txtStampDutyRate2.BackColor = System.Drawing.SystemColors.Window
        Me.txtStampDutyRate2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStampDutyRate2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtStampDutyRate2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStampDutyRate2.Location = New System.Drawing.Point(225, 349)
        Me.txtStampDutyRate2.MaxLength = 0
        Me.txtStampDutyRate2.Name = "txtStampDutyRate2"
        Me.txtStampDutyRate2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStampDutyRate2.Size = New System.Drawing.Size(153, 20)
        Me.txtStampDutyRate2.TabIndex = 8
        '
        'txtPrimarySort
        '
        Me.txtPrimarySort.AcceptsReturn = true
        Me.txtPrimarySort.BackColor = System.Drawing.SystemColors.Window
        Me.txtPrimarySort.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPrimarySort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtPrimarySort.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPrimarySort.Location = New System.Drawing.Point(225, 250)
        Me.txtPrimarySort.MaxLength = 0
        Me.txtPrimarySort.Name = "txtPrimarySort"
        Me.txtPrimarySort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPrimarySort.Size = New System.Drawing.Size(67, 20)
        Me.txtPrimarySort.TabIndex = 5
        '
        'txtSecondarySort
        '
        Me.txtSecondarySort.AcceptsReturn = true
        Me.txtSecondarySort.BackColor = System.Drawing.SystemColors.Window
        Me.txtSecondarySort.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSecondarySort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtSecondarySort.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSecondarySort.Location = New System.Drawing.Point(225, 283)
        Me.txtSecondarySort.MaxLength = 0
        Me.txtSecondarySort.Name = "txtSecondarySort"
        Me.txtSecondarySort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSecondarySort.Size = New System.Drawing.Size(67, 20)
        Me.txtSecondarySort.TabIndex = 6
        '
        'cmdHeaderClause
        '
        Me.cmdHeaderClause.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHeaderClause.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHeaderClause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdHeaderClause.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHeaderClause.Location = New System.Drawing.Point(434, 379)
        Me.cmdHeaderClause.Name = "cmdHeaderClause"
        Me.cmdHeaderClause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHeaderClause.Size = New System.Drawing.Size(34, 22)
        Me.cmdHeaderClause.TabIndex = 10
        Me.cmdHeaderClause.Text = "..."
        Me.cmdHeaderClause.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHeaderClause.UseVisualStyleBackColor = false
        '
        'cmdTrailerClause
        '
        Me.cmdTrailerClause.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTrailerClause.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTrailerClause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdTrailerClause.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTrailerClause.Location = New System.Drawing.Point(434, 418)
        Me.cmdTrailerClause.Name = "cmdTrailerClause"
        Me.cmdTrailerClause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTrailerClause.Size = New System.Drawing.Size(34, 22)
        Me.cmdTrailerClause.TabIndex = 12
        Me.cmdTrailerClause.Text = "..."
        Me.cmdTrailerClause.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdTrailerClause.UseVisualStyleBackColor = false
        '
        'txtAccumulationLevel
        '
        Me.txtAccumulationLevel.AcceptsReturn = true
        Me.txtAccumulationLevel.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccumulationLevel.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccumulationLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtAccumulationLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccumulationLevel.Location = New System.Drawing.Point(225, 184)
        Me.txtAccumulationLevel.MaxLength = 1
        Me.txtAccumulationLevel.Name = "txtAccumulationLevel"
        Me.txtAccumulationLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccumulationLevel.Size = New System.Drawing.Size(67, 20)
        Me.txtAccumulationLevel.TabIndex = 3
        '
        'fraReinsurance
        '
        Me.fraReinsurance.BackColor = System.Drawing.SystemColors.Control
        Me.fraReinsurance.Controls.Add(Me.chkDisplayClaimReinsurance)
        Me.fraReinsurance.Controls.Add(Me.chkDisplayReinsurance)
        Me.fraReinsurance.Controls.Add(Me.chkDeferredRI)
        Me.fraReinsurance.Controls.Add(Me.chkIsAutoReinsured)
        Me.fraReinsurance.Controls.Add(Me.cmdDeferredRI)
        Me.fraReinsurance.Controls.Add(Me.cmdRILimits)
        Me.fraReinsurance.Controls.Add(Me.cmdRIModel)
        Me.fraReinsurance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraReinsurance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraReinsurance.Location = New System.Drawing.Point(591, 249)
        Me.fraReinsurance.Name = "fraReinsurance"
        Me.fraReinsurance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraReinsurance.Size = New System.Drawing.Size(375, 188)
        Me.fraReinsurance.TabIndex = 29
        Me.fraReinsurance.TabStop = false
        Me.fraReinsurance.Text = "Reinsurance"
        '
        'chkDisplayClaimReinsurance
        '
        Me.chkDisplayClaimReinsurance.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisplayClaimReinsurance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDisplayClaimReinsurance.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisplayClaimReinsurance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkDisplayClaimReinsurance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisplayClaimReinsurance.Location = New System.Drawing.Point(44, 148)
        Me.chkDisplayClaimReinsurance.Name = "chkDisplayClaimReinsurance"
        Me.chkDisplayClaimReinsurance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisplayClaimReinsurance.Size = New System.Drawing.Size(289, 17)
        Me.chkDisplayClaimReinsurance.TabIndex = 67
        Me.chkDisplayClaimReinsurance.Text = "Display Claim Reinsurance :"
        Me.chkDisplayClaimReinsurance.UseVisualStyleBackColor = false
        '
        'chkDisplayReinsurance
        '
        Me.chkDisplayReinsurance.BackColor = System.Drawing.SystemColors.Control
        Me.chkDisplayReinsurance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDisplayReinsurance.Checked = true
        Me.chkDisplayReinsurance.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDisplayReinsurance.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDisplayReinsurance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkDisplayReinsurance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDisplayReinsurance.Location = New System.Drawing.Point(44, 128)
        Me.chkDisplayReinsurance.Name = "chkDisplayReinsurance"
        Me.chkDisplayReinsurance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDisplayReinsurance.Size = New System.Drawing.Size(289, 17)
        Me.chkDisplayReinsurance.TabIndex = 56
        Me.chkDisplayReinsurance.Text = "Display Reinsurance Screen :"
        Me.chkDisplayReinsurance.UseVisualStyleBackColor = false
        '
        'chkDeferredRI
        '
        Me.chkDeferredRI.BackColor = System.Drawing.SystemColors.Control
        Me.chkDeferredRI.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDeferredRI.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkDeferredRI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkDeferredRI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDeferredRI.Location = New System.Drawing.Point(18, 71)
        Me.chkDeferredRI.Name = "chkDeferredRI"
        Me.chkDeferredRI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkDeferredRI.Size = New System.Drawing.Size(315, 19)
        Me.chkDeferredRI.TabIndex = 22
        Me.chkDeferredRI.Text = "Deferred Reinsurance Permitted:"
        Me.chkDeferredRI.UseVisualStyleBackColor = false
        '
        'chkIsAutoReinsured
        '
        Me.chkIsAutoReinsured.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsAutoReinsured.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsAutoReinsured.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsAutoReinsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkIsAutoReinsured.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsAutoReinsured.Location = New System.Drawing.Point(110, 18)
        Me.chkIsAutoReinsured.Name = "chkIsAutoReinsured"
        Me.chkIsAutoReinsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsAutoReinsured.Size = New System.Drawing.Size(224, 21)
        Me.chkIsAutoReinsured.TabIndex = 19
        Me.chkIsAutoReinsured.Text = "Auto Reinsured:"
        Me.chkIsAutoReinsured.UseVisualStyleBackColor = false
        '
        'cmdDeferredRI
        '
        Me.cmdDeferredRI.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeferredRI.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeferredRI.Enabled = false
        Me.cmdDeferredRI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdDeferredRI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeferredRI.Location = New System.Drawing.Point(159, 98)
        Me.cmdDeferredRI.Name = "cmdDeferredRI"
        Me.cmdDeferredRI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeferredRI.Size = New System.Drawing.Size(174, 22)
        Me.cmdDeferredRI.TabIndex = 23
        Me.cmdDeferredRI.Text = "De&ferred Model..."
        Me.cmdDeferredRI.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeferredRI.UseVisualStyleBackColor = false
        '
        'cmdRILimits
        '
        Me.cmdRILimits.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRILimits.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRILimits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdRILimits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRILimits.Location = New System.Drawing.Point(224, 45)
        Me.cmdRILimits.Name = "cmdRILimits"
        Me.cmdRILimits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRILimits.Size = New System.Drawing.Size(109, 22)
        Me.cmdRILimits.TabIndex = 21
        Me.cmdRILimits.Text = "&Limits..."
        Me.cmdRILimits.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRILimits.UseVisualStyleBackColor = false
        '
        'cmdRIModel
        '
        Me.cmdRIModel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRIModel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRIModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdRIModel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRIModel.Location = New System.Drawing.Point(104, 45)
        Me.cmdRIModel.Name = "cmdRIModel"
        Me.cmdRIModel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRIModel.Size = New System.Drawing.Size(109, 22)
        Me.cmdRIModel.TabIndex = 20
        Me.cmdRIModel.Text = "&Model..."
        Me.cmdRIModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRIModel.UseVisualStyleBackColor = false
        '
        'chkClaimsIsPostTaxes
        '
        Me.chkClaimsIsPostTaxes.BackColor = System.Drawing.SystemColors.Control
        Me.chkClaimsIsPostTaxes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkClaimsIsPostTaxes.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkClaimsIsPostTaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkClaimsIsPostTaxes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkClaimsIsPostTaxes.Location = New System.Drawing.Point(593, 178)
        Me.chkClaimsIsPostTaxes.Name = "chkClaimsIsPostTaxes"
        Me.chkClaimsIsPostTaxes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkClaimsIsPostTaxes.Size = New System.Drawing.Size(320, 21)
        Me.chkClaimsIsPostTaxes.TabIndex = 55
        Me.chkClaimsIsPostTaxes.Text = "Post Claim Taxes Separately:"
        Me.chkClaimsIsPostTaxes.UseVisualStyleBackColor = false
        '
        '_tabMainTab_TabPage1
        '
        Me._tabMainTab_TabPage1.Controls.Add(Me.uctSIRSelectClauses)
        Me._tabMainTab_TabPage1.Location = New System.Drawing.Point(4, 23)
        Me._tabMainTab_TabPage1.Name = "_tabMainTab_TabPage1"
        Me._tabMainTab_TabPage1.Size = New System.Drawing.Size(952, 525)
        Me._tabMainTab_TabPage1.TabIndex = 1
        Me._tabMainTab_TabPage1.Text = "2-Allowed GIS Screens"
        '
        'uctSIRSelectClauses
        '
        Me.uctSIRSelectClauses.ClauseId = 0
        Me.uctSIRSelectClauses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.uctSIRSelectClauses.imglImages = Nothing
        Me.uctSIRSelectClauses.Location = New System.Drawing.Point(9, 5)
        Me.uctSIRSelectClauses.Name = "uctSIRSelectClauses"
        Me.uctSIRSelectClauses.ProductId = 0
        Me.uctSIRSelectClauses.RiskId = 0
        Me.uctSIRSelectClauses.Size = New System.Drawing.Size(1013, 491)
        Me.uctSIRSelectClauses.SystemCurrency = 0
        Me.uctSIRSelectClauses.TabIndex = 68
        Me.uctSIRSelectClauses.Task = 0
        '
        '_tabMainTab_TabPage2
        '
        Me._tabMainTab_TabPage2.Controls.Add(Me.cmdSelectRiskTypeGroup)
        Me._tabMainTab_TabPage2.Controls.Add(Me.lvwRiskTypeGroup)
        Me._tabMainTab_TabPage2.Location = New System.Drawing.Point(4, 23)
        Me._tabMainTab_TabPage2.Name = "_tabMainTab_TabPage2"
        Me._tabMainTab_TabPage2.Size = New System.Drawing.Size(952, 525)
        Me._tabMainTab_TabPage2.TabIndex = 2
        Me._tabMainTab_TabPage2.Text = "3-Risk Group"
        '
        'cmdSelectRiskTypeGroup
        '
        Me.cmdSelectRiskTypeGroup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectRiskTypeGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectRiskTypeGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdSelectRiskTypeGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectRiskTypeGroup.Location = New System.Drawing.Point(840, 474)
        Me.cmdSelectRiskTypeGroup.Name = "cmdSelectRiskTypeGroup"
        Me.cmdSelectRiskTypeGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectRiskTypeGroup.Size = New System.Drawing.Size(110, 22)
        Me.cmdSelectRiskTypeGroup.TabIndex = 30
        Me.cmdSelectRiskTypeGroup.Text = "&Select"
        Me.cmdSelectRiskTypeGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectRiskTypeGroup.UseVisualStyleBackColor = false
        '
        'lvwRiskTypeGroup
        '
        Me.lvwRiskTypeGroup.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRiskTypeGroup.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRiskTypeGroup.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRiskTypeGroup_ColumnHeader_1, Me._lvwRiskTypeGroup_ColumnHeader_2, Me._lvwRiskTypeGroup_ColumnHeader_3})
        Me.lvwRiskTypeGroup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwRiskTypeGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRiskTypeGroup.FullRowSelect = true
        Me.lvwRiskTypeGroup.HideSelection = false
        Me.lvwRiskTypeGroup.LargeImageList = Me.ImageList1
        Me.lvwRiskTypeGroup.Location = New System.Drawing.Point(18, 33)
        Me.lvwRiskTypeGroup.Name = "lvwRiskTypeGroup"
        Me.lvwRiskTypeGroup.Size = New System.Drawing.Size(932, 433)
        Me.lvwRiskTypeGroup.SmallImageList = Me.ImageList1
        Me.lvwRiskTypeGroup.TabIndex = 41
        Me.lvwRiskTypeGroup.UseCompatibleStateImageBehavior = false
        Me.lvwRiskTypeGroup.View = System.Windows.Forms.View.Details
        '
        '_lvwRiskTypeGroup_ColumnHeader_1
        '
        Me._lvwRiskTypeGroup_ColumnHeader_1.Text = "Code"
        Me._lvwRiskTypeGroup_ColumnHeader_1.Width = 97
        '
        '_lvwRiskTypeGroup_ColumnHeader_2
        '
        Me._lvwRiskTypeGroup_ColumnHeader_2.Text = "Description"
        Me._lvwRiskTypeGroup_ColumnHeader_2.Width = 301
        '
        '_lvwRiskTypeGroup_ColumnHeader_3
        '
        Me._lvwRiskTypeGroup_ColumnHeader_3.Text = "Effective Date"
        Me._lvwRiskTypeGroup_ColumnHeader_3.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"),System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.ImageList1.Images.SetKeyName(0, "RuleFile")
        Me.ImageList1.Images.SetKeyName(1, "Tick")
        Me.ImageList1.Images.SetKeyName(2, "Blank")
        '
        '_tabMainTab_TabPage3
        '
        Me._tabMainTab_TabPage3.Controls.Add(Me.frmLapse)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fmeRating)
        Me._tabMainTab_TabPage3.Controls.Add(Me.fmeRenewal)
        Me._tabMainTab_TabPage3.Location = New System.Drawing.Point(4, 23)
        Me._tabMainTab_TabPage3.Name = "_tabMainTab_TabPage3"
        Me._tabMainTab_TabPage3.Size = New System.Drawing.Size(952, 525)
        Me._tabMainTab_TabPage3.TabIndex = 3
        Me._tabMainTab_TabPage3.Text = "4-Rules"
        '
        'frmLapse
        '
        Me.frmLapse.BackColor = System.Drawing.SystemColors.Control
        Me.frmLapse.Controls.Add(Me.cmdAddRenLapseRule)
        Me.frmLapse.Controls.Add(Me.cmdEditRenLapseRule)
        Me.frmLapse.Controls.Add(Me.cmdDeleteRenLapseRule)
        Me.frmLapse.Controls.Add(Me.lvwRenewalLapseRule)
        Me.frmLapse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.frmLapse.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmLapse.Location = New System.Drawing.Point(15, 332)
        Me.frmLapse.Name = "frmLapse"
        Me.frmLapse.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmLapse.Size = New System.Drawing.Size(947, 157)
        Me.frmLapse.TabIndex = 51
        Me.frmLapse.TabStop = false
        Me.frmLapse.Text = "Lapse Scripts"
        '
        'cmdAddRenLapseRule
        '
        Me.cmdAddRenLapseRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddRenLapseRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddRenLapseRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdAddRenLapseRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddRenLapseRule.Location = New System.Drawing.Point(561, 123)
        Me.cmdAddRenLapseRule.Name = "cmdAddRenLapseRule"
        Me.cmdAddRenLapseRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddRenLapseRule.Size = New System.Drawing.Size(109, 22)
        Me.cmdAddRenLapseRule.TabIndex = 48
        Me.cmdAddRenLapseRule.Text = "A&dd..."
        Me.cmdAddRenLapseRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddRenLapseRule.UseVisualStyleBackColor = false
        '
        'cmdEditRenLapseRule
        '
        Me.cmdEditRenLapseRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditRenLapseRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditRenLapseRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdEditRenLapseRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditRenLapseRule.Location = New System.Drawing.Point(681, 123)
        Me.cmdEditRenLapseRule.Name = "cmdEditRenLapseRule"
        Me.cmdEditRenLapseRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditRenLapseRule.Size = New System.Drawing.Size(109, 22)
        Me.cmdEditRenLapseRule.TabIndex = 47
        Me.cmdEditRenLapseRule.Text = "&Edit..."
        Me.cmdEditRenLapseRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditRenLapseRule.UseVisualStyleBackColor = false
        '
        'cmdDeleteRenLapseRule
        '
        Me.cmdDeleteRenLapseRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteRenLapseRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteRenLapseRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdDeleteRenLapseRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteRenLapseRule.Location = New System.Drawing.Point(801, 123)
        Me.cmdDeleteRenLapseRule.Name = "cmdDeleteRenLapseRule"
        Me.cmdDeleteRenLapseRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteRenLapseRule.Size = New System.Drawing.Size(109, 22)
        Me.cmdDeleteRenLapseRule.TabIndex = 46
        Me.cmdDeleteRenLapseRule.Text = "Dele&te"
        Me.cmdDeleteRenLapseRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteRenLapseRule.UseVisualStyleBackColor = false
        '
        'lvwRenewalLapseRule
        '
        Me.lvwRenewalLapseRule.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRenewalLapseRule.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRenewalLapseRule.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lvwRenewalLapseRule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwRenewalLapseRule.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRenewalLapseRule.FullRowSelect = true
        Me.lvwRenewalLapseRule.HideSelection = false
        Me.lvwRenewalLapseRule.LargeImageList = Me.ImageList1
        Me.lvwRenewalLapseRule.Location = New System.Drawing.Point(18, 20)
        Me.lvwRenewalLapseRule.Name = "lvwRenewalLapseRule"
        Me.lvwRenewalLapseRule.Size = New System.Drawing.Size(892, 94)
        Me.lvwRenewalLapseRule.SmallImageList = Me.ImageList1
        Me.lvwRenewalLapseRule.TabIndex = 49
        Me.lvwRenewalLapseRule.UseCompatibleStateImageBehavior = false
        Me.lvwRenewalLapseRule.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Code"
        Me.ColumnHeader1.Width = 97
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Description"
        Me.ColumnHeader2.Width = 334
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Effective Date"
        Me.ColumnHeader3.Width = 97
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Status"
        Me.ColumnHeader4.Width = 67
        '
        'fmeRating
        '
        Me.fmeRating.BackColor = System.Drawing.SystemColors.Control
        Me.fmeRating.Controls.Add(Me.cmdDeleteRule)
        Me.fmeRating.Controls.Add(Me.cmdEditRule)
        Me.fmeRating.Controls.Add(Me.cmdAddRule)
        Me.fmeRating.Controls.Add(Me.lvwRules)
        Me.fmeRating.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fmeRating.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeRating.Location = New System.Drawing.Point(18, 5)
        Me.fmeRating.Name = "fmeRating"
        Me.fmeRating.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeRating.Size = New System.Drawing.Size(946, 157)
        Me.fmeRating.TabIndex = 50
        Me.fmeRating.TabStop = false
        Me.fmeRating.Text = "Rating Scripts"
        '
        'cmdDeleteRule
        '
        Me.cmdDeleteRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdDeleteRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteRule.Location = New System.Drawing.Point(800, 124)
        Me.cmdDeleteRule.Name = "cmdDeleteRule"
        Me.cmdDeleteRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteRule.Size = New System.Drawing.Size(109, 22)
        Me.cmdDeleteRule.TabIndex = 54
        Me.cmdDeleteRule.Text = "Dele&te"
        Me.cmdDeleteRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteRule.UseVisualStyleBackColor = false
        '
        'cmdEditRule
        '
        Me.cmdEditRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdEditRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditRule.Location = New System.Drawing.Point(680, 124)
        Me.cmdEditRule.Name = "cmdEditRule"
        Me.cmdEditRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditRule.Size = New System.Drawing.Size(109, 22)
        Me.cmdEditRule.TabIndex = 52
        Me.cmdEditRule.Text = "&Edit..."
        Me.cmdEditRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditRule.UseVisualStyleBackColor = false
        '
        'cmdAddRule
        '
        Me.cmdAddRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdAddRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddRule.Location = New System.Drawing.Point(562, 124)
        Me.cmdAddRule.Name = "cmdAddRule"
        Me.cmdAddRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddRule.Size = New System.Drawing.Size(110, 22)
        Me.cmdAddRule.TabIndex = 51
        Me.cmdAddRule.Text = "A&dd..."
        Me.cmdAddRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddRule.UseVisualStyleBackColor = false
        '
        'lvwRules
        '
        Me.lvwRules.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRules.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRules.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRules_ColumnHeader_1, Me._lvwRules_ColumnHeader_2, Me._lvwRules_ColumnHeader_3, Me._lvwRules_ColumnHeader_5})
        Me.lvwRules.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwRules.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRules.FullRowSelect = true
        Me.lvwRules.HideSelection = false
        Me.lvwRules.LargeImageList = Me.ImageList1
        Me.lvwRules.Location = New System.Drawing.Point(18, 20)
        Me.lvwRules.Name = "lvwRules"
        Me.lvwRules.Size = New System.Drawing.Size(892, 94)
        Me.lvwRules.SmallImageList = Me.ImageList1
        Me.lvwRules.TabIndex = 53
        Me.lvwRules.UseCompatibleStateImageBehavior = false
        Me.lvwRules.View = System.Windows.Forms.View.Details
        '
        '_lvwRules_ColumnHeader_1
        '
        Me._lvwRules_ColumnHeader_1.Text = "Code"
        Me._lvwRules_ColumnHeader_1.Width = 97
        '
        '_lvwRules_ColumnHeader_2
        '
        Me._lvwRules_ColumnHeader_2.Text = "Description"
        Me._lvwRules_ColumnHeader_2.Width = 334
        '
        '_lvwRules_ColumnHeader_3
        '
        Me._lvwRules_ColumnHeader_3.Text = "Effective Date"
        Me._lvwRules_ColumnHeader_3.Width = 97
        '
        '_lvwRules_ColumnHeader_5
        '
        Me._lvwRules_ColumnHeader_5.Text = "Status"
        Me._lvwRules_ColumnHeader_5.Width = 67
        '
        'fmeRenewal
        '
        Me.fmeRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.fmeRenewal.Controls.Add(Me.cmdAddRenRule)
        Me.fmeRenewal.Controls.Add(Me.cmdEditRenRule)
        Me.fmeRenewal.Controls.Add(Me.cmdDeleteRenRule)
        Me.fmeRenewal.Controls.Add(Me.lvwRenewalRule)
        Me.fmeRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fmeRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeRenewal.Location = New System.Drawing.Point(18, 167)
        Me.fmeRenewal.Name = "fmeRenewal"
        Me.fmeRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeRenewal.Size = New System.Drawing.Size(946, 157)
        Me.fmeRenewal.TabIndex = 45
        Me.fmeRenewal.TabStop = false
        Me.fmeRenewal.Text = "Renewal Scripts"
        '
        'cmdAddRenRule
        '
        Me.cmdAddRenRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddRenRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddRenRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdAddRenRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddRenRule.Location = New System.Drawing.Point(562, 123)
        Me.cmdAddRenRule.Name = "cmdAddRenRule"
        Me.cmdAddRenRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddRenRule.Size = New System.Drawing.Size(110, 22)
        Me.cmdAddRenRule.TabIndex = 48
        Me.cmdAddRenRule.Text = "A&dd..."
        Me.cmdAddRenRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddRenRule.UseVisualStyleBackColor = false
        '
        'cmdEditRenRule
        '
        Me.cmdEditRenRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditRenRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditRenRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdEditRenRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditRenRule.Location = New System.Drawing.Point(682, 123)
        Me.cmdEditRenRule.Name = "cmdEditRenRule"
        Me.cmdEditRenRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditRenRule.Size = New System.Drawing.Size(110, 22)
        Me.cmdEditRenRule.TabIndex = 47
        Me.cmdEditRenRule.Text = "&Edit..."
        Me.cmdEditRenRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEditRenRule.UseVisualStyleBackColor = false
        '
        'cmdDeleteRenRule
        '
        Me.cmdDeleteRenRule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteRenRule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteRenRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdDeleteRenRule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteRenRule.Location = New System.Drawing.Point(802, 123)
        Me.cmdDeleteRenRule.Name = "cmdDeleteRenRule"
        Me.cmdDeleteRenRule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteRenRule.Size = New System.Drawing.Size(110, 22)
        Me.cmdDeleteRenRule.TabIndex = 46
        Me.cmdDeleteRenRule.Text = "Dele&te"
        Me.cmdDeleteRenRule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDeleteRenRule.UseVisualStyleBackColor = false
        '
        'lvwRenewalRule
        '
        Me.lvwRenewalRule.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRenewalRule.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRenewalRule.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwRenewalRule_ColumnHeader_1, Me._lvwRenewalRule_ColumnHeader_2, Me._lvwRenewalRule_ColumnHeader_3, Me._lvwRenewalRule_ColumnHeader_5})
        Me.lvwRenewalRule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwRenewalRule.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRenewalRule.FullRowSelect = true
        Me.lvwRenewalRule.HideSelection = false
        Me.lvwRenewalRule.LargeImageList = Me.ImageList1
        Me.lvwRenewalRule.Location = New System.Drawing.Point(18, 20)
        Me.lvwRenewalRule.Name = "lvwRenewalRule"
        Me.lvwRenewalRule.Size = New System.Drawing.Size(892, 94)
        Me.lvwRenewalRule.SmallImageList = Me.ImageList1
        Me.lvwRenewalRule.TabIndex = 49
        Me.lvwRenewalRule.UseCompatibleStateImageBehavior = false
        Me.lvwRenewalRule.View = System.Windows.Forms.View.Details
        '
        '_lvwRenewalRule_ColumnHeader_1
        '
        Me._lvwRenewalRule_ColumnHeader_1.Text = "Code"
        Me._lvwRenewalRule_ColumnHeader_1.Width = 97
        '
        '_lvwRenewalRule_ColumnHeader_2
        '
        Me._lvwRenewalRule_ColumnHeader_2.Text = "Description"
        Me._lvwRenewalRule_ColumnHeader_2.Width = 334
        '
        '_lvwRenewalRule_ColumnHeader_3
        '
        Me._lvwRenewalRule_ColumnHeader_3.Text = "Effective Date"
        Me._lvwRenewalRule_ColumnHeader_3.Width = 97
        '
        '_lvwRenewalRule_ColumnHeader_5
        '
        Me._lvwRenewalRule_ColumnHeader_5.Text = "Status"
        Me._lvwRenewalRule_ColumnHeader_5.Width = 67
        '
        '_tabMainTab_TabPage4
        '
        Me._tabMainTab_TabPage4.Controls.Add(Me.fraAllow)
        Me._tabMainTab_TabPage4.Controls.Add(Me.uctPickListRatingSections)
        Me._tabMainTab_TabPage4.Location = New System.Drawing.Point(4, 23)
        Me._tabMainTab_TabPage4.Name = "_tabMainTab_TabPage4"
        Me._tabMainTab_TabPage4.Size = New System.Drawing.Size(952, 525)
        Me._tabMainTab_TabPage4.TabIndex = 4
        Me._tabMainTab_TabPage4.Text = "5 - Rating Sections"
        '
        'fraAllow
        '
        Me.fraAllow.BackColor = System.Drawing.SystemColors.Control
        Me.fraAllow.Controls.Add(Me.fraEdit)
        Me.fraAllow.Controls.Add(Me.chkAllowDeleteRatingSection)
        Me.fraAllow.Controls.Add(Me.chkAllowEditRatingSection)
        Me.fraAllow.Controls.Add(Me.chkAllowAddRatingSection)
        Me.fraAllow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraAllow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAllow.Location = New System.Drawing.Point(24, 380)
        Me.fraAllow.Name = "fraAllow"
        Me.fraAllow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAllow.Size = New System.Drawing.Size(885, 113)
        Me.fraAllow.TabIndex = 58
        Me.fraAllow.TabStop = false
        Me.fraAllow.Text = "Allow"
        '
        'fraEdit
        '
        Me.fraEdit.BackColor = System.Drawing.SystemColors.Control
        Me.fraEdit.Controls.Add(Me.chkAllowEditRatingSectionThisPremium)
        Me.fraEdit.Controls.Add(Me.chkAllowEditRatingSectionSumInsured)
        Me.fraEdit.Controls.Add(Me.chkAllowEditRatingSectionRate)
        Me.fraEdit.Controls.Add(Me.chkAllowEditRatingSectionRateType)
        Me.fraEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fraEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraEdit.Location = New System.Drawing.Point(24, 48)
        Me.fraEdit.Name = "fraEdit"
        Me.fraEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraEdit.Size = New System.Drawing.Size(818, 57)
        Me.fraEdit.TabIndex = 62
        Me.fraEdit.TabStop = false
        Me.fraEdit.Text = "Editing Properties Enabled"
        '
        'chkAllowEditRatingSectionThisPremium
        '
        Me.chkAllowEditRatingSectionThisPremium.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowEditRatingSectionThisPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowEditRatingSectionThisPremium.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAllowEditRatingSectionThisPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowEditRatingSectionThisPremium.Location = New System.Drawing.Point(640, 24)
        Me.chkAllowEditRatingSectionThisPremium.Name = "chkAllowEditRatingSectionThisPremium"
        Me.chkAllowEditRatingSectionThisPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowEditRatingSectionThisPremium.Size = New System.Drawing.Size(170, 17)
        Me.chkAllowEditRatingSectionThisPremium.TabIndex = 66
        Me.chkAllowEditRatingSectionThisPremium.Text = "This Premium"
        Me.chkAllowEditRatingSectionThisPremium.UseVisualStyleBackColor = false
        '
        'chkAllowEditRatingSectionSumInsured
        '
        Me.chkAllowEditRatingSectionSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowEditRatingSectionSumInsured.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowEditRatingSectionSumInsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAllowEditRatingSectionSumInsured.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowEditRatingSectionSumInsured.Location = New System.Drawing.Point(435, 24)
        Me.chkAllowEditRatingSectionSumInsured.Name = "chkAllowEditRatingSectionSumInsured"
        Me.chkAllowEditRatingSectionSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowEditRatingSectionSumInsured.Size = New System.Drawing.Size(169, 17)
        Me.chkAllowEditRatingSectionSumInsured.TabIndex = 65
        Me.chkAllowEditRatingSectionSumInsured.Text = "Sum Insured"
        Me.chkAllowEditRatingSectionSumInsured.UseVisualStyleBackColor = false
        '
        'chkAllowEditRatingSectionRate
        '
        Me.chkAllowEditRatingSectionRate.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowEditRatingSectionRate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowEditRatingSectionRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAllowEditRatingSectionRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowEditRatingSectionRate.Location = New System.Drawing.Point(230, 24)
        Me.chkAllowEditRatingSectionRate.Name = "chkAllowEditRatingSectionRate"
        Me.chkAllowEditRatingSectionRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowEditRatingSectionRate.Size = New System.Drawing.Size(169, 17)
        Me.chkAllowEditRatingSectionRate.TabIndex = 64
        Me.chkAllowEditRatingSectionRate.Text = "Rate"
        Me.chkAllowEditRatingSectionRate.UseVisualStyleBackColor = false
        '
        'chkAllowEditRatingSectionRateType
        '
        Me.chkAllowEditRatingSectionRateType.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowEditRatingSectionRateType.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowEditRatingSectionRateType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAllowEditRatingSectionRateType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowEditRatingSectionRateType.Location = New System.Drawing.Point(24, 24)
        Me.chkAllowEditRatingSectionRateType.Name = "chkAllowEditRatingSectionRateType"
        Me.chkAllowEditRatingSectionRateType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowEditRatingSectionRateType.Size = New System.Drawing.Size(170, 17)
        Me.chkAllowEditRatingSectionRateType.TabIndex = 63
        Me.chkAllowEditRatingSectionRateType.Text = "Rate Type"
        Me.chkAllowEditRatingSectionRateType.UseVisualStyleBackColor = false
        '
        'chkAllowDeleteRatingSection
        '
        Me.chkAllowDeleteRatingSection.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowDeleteRatingSection.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowDeleteRatingSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAllowDeleteRatingSection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowDeleteRatingSection.Location = New System.Drawing.Point(566, 24)
        Me.chkAllowDeleteRatingSection.Name = "chkAllowDeleteRatingSection"
        Me.chkAllowDeleteRatingSection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowDeleteRatingSection.Size = New System.Drawing.Size(253, 18)
        Me.chkAllowDeleteRatingSection.TabIndex = 61
        Me.chkAllowDeleteRatingSection.Text = "Deleting Existing Sections"
        Me.chkAllowDeleteRatingSection.UseVisualStyleBackColor = false
        '
        'chkAllowEditRatingSection
        '
        Me.chkAllowEditRatingSection.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowEditRatingSection.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowEditRatingSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAllowEditRatingSection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowEditRatingSection.Location = New System.Drawing.Point(302, 24)
        Me.chkAllowEditRatingSection.Name = "chkAllowEditRatingSection"
        Me.chkAllowEditRatingSection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowEditRatingSection.Size = New System.Drawing.Size(253, 18)
        Me.chkAllowEditRatingSection.TabIndex = 60
        Me.chkAllowEditRatingSection.Text = "Editing Existing Sections"
        Me.chkAllowEditRatingSection.UseVisualStyleBackColor = false
        '
        'chkAllowAddRatingSection
        '
        Me.chkAllowAddRatingSection.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowAddRatingSection.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowAddRatingSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.chkAllowAddRatingSection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowAddRatingSection.Location = New System.Drawing.Point(38, 20)
        Me.chkAllowAddRatingSection.Name = "chkAllowAddRatingSection"
        Me.chkAllowAddRatingSection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowAddRatingSection.Size = New System.Drawing.Size(253, 25)
        Me.chkAllowAddRatingSection.TabIndex = 59
        Me.chkAllowAddRatingSection.Text = "Adding New Sections"
        Me.chkAllowAddRatingSection.UseVisualStyleBackColor = false
        '
        'uctPickListRatingSections
        '
        Me.uctPickListRatingSections.AutoScroll = true
        Me.uctPickListRatingSections.AvailableCaption = "Rating Section Types"
        Me.uctPickListRatingSections.BusinessObject = "bSIRRiskType.Business"
        Me.uctPickListRatingSections.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.uctPickListRatingSections.ForeignKeys = CType(resources.GetObject("uctPickListRatingSections.ForeignKeys"),Microsoft.VisualBasic.Collection)
        Me.uctPickListRatingSections.IsSearchable = false
        Me.uctPickListRatingSections.Location = New System.Drawing.Point(12, 3)
        Me.uctPickListRatingSections.Name = "uctPickListRatingSections"
        Me.uctPickListRatingSections.PickListType = ""
        Me.uctPickListRatingSections.Size = New System.Drawing.Size(907, 371)
        Me.uctPickListRatingSections.TabIndex = 57
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(624, 562)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(110, 22)
        Me.cmdOK.TabIndex = 25
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = false
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(744, 562)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(110, 22)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = false
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(12, 562)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(110, 22)
        Me.cmdHelp.TabIndex = 27
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = false
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(984, 588)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.txtFormatDate)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = false
        Me.MinimizeBox = false
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Risk Type"
        Me.tabMainTab.ResumeLayout(false)
        Me._tabMainTab_TabPage0.ResumeLayout(false)
        Me._tabMainTab_TabPage0.PerformLayout
        Me.fraCover.ResumeLayout(false)
        Me.fraReinsurance.ResumeLayout(false)
        Me._tabMainTab_TabPage1.ResumeLayout(false)
        Me._tabMainTab_TabPage2.ResumeLayout(false)
        Me._tabMainTab_TabPage3.ResumeLayout(false)
        Me.frmLapse.ResumeLayout(false)
        Me.fmeRating.ResumeLayout(false)
        Me.fmeRenewal.ResumeLayout(false)
        Me._tabMainTab_TabPage4.ResumeLayout(false)
        Me.fraAllow.ResumeLayout(false)
        Me.fraEdit.ResumeLayout(false)
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Public WithEvents frmLapse As System.Windows.Forms.GroupBox
    Public WithEvents cmdAddRenLapseRule As System.Windows.Forms.Button
    Public WithEvents cmdEditRenLapseRule As System.Windows.Forms.Button
    Public WithEvents cmdDeleteRenLapseRule As System.Windows.Forms.Button
    Public WithEvents lvwRenewalLapseRule As System.Windows.Forms.ListView
    Private WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Private WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Private WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Private WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Public WithEvents lblAccumulationLevel As System.Windows.Forms.Label
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Public WithEvents lblTrailerClause As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents cmdTrailerClause As System.Windows.Forms.Button
    Friend WithEvents fraCover As System.Windows.Forms.GroupBox
    Public WithEvents lblClaimsCoverBasis As System.Windows.Forms.Label
    Public WithEvents lblClaimsTypeBasis As System.Windows.Forms.Label
    Public WithEvents chkAttachClaimOutsideOfPolicyPeriod As System.Windows.Forms.CheckBox
#End Region
End Class