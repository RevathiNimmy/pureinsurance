<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPolicySummaryUnderwriting
#Region "Windows Form Designer generated code "
    Public Sub New(ByVal parentForm As frmMDI)
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializemnuRecentFile()
        'This form is an MDI child.
        'This code simulates the VB6 
        ' functionality of automatically
        ' loading and showing an MDI
        ' child's parent.
        Me.MdiParent = parentForm
        'iPMBClientManager.frmMDI.Show()
        Form_Initialize_Renamed()
        'The MDI form in the VB6 project had its
        'AutoShowChildren property set to True
        'To simulate the VB6 behavior, we need to
        'automatically Show the form whenever it
        'is loaded.  If you do not want this behavior
        'then delete the following line of code

        'Me.Show()
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
	Public WithEvents mnuPolicyDelete As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuPolicyCopy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuPolicyMove As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuPolicyExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuPolicy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToAccounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoInsuredAccounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionCash As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionCredit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionDebit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoTOTRansactionAJ As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToTRansactionAJReversal As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoTOTRansaction As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToPolicy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToClaim As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToDocumaster As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToSharePoint As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToSwift As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToEvents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToRisk As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToTextFiles As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoNotes As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuProcess As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentsLetterWriting As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocuments As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuReportsClientSummary As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuReportsStatements As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuReports As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuNewDiary As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFind As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTasks As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents uctPMUPolicySummary1 As PMUPolicySummary.uctPMUPolicySummary
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public mnuRecentFile(1) As System.Windows.Forms.ToolStripMenuItem
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuClient = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPolicy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPolicyDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPolicyCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPolicyMove = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_0 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeperator = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPolicyExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProcess = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToAccounts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoInsuredAccounts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoTOTRansaction = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionCash = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionCredit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionDebit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoTOTRansactionAJ = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToTRansactionAJReversal = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToPolicy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToClaim = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToDocumaster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToSharePoint = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToSwift = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToEvents = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToRisk = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToTextFiles = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocuments = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentsLetterWriting = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsClientSummary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsStatements = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTasks = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewDiary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFind = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.uctPMUPolicySummary1 = New PMUPolicySummary.uctPMUPolicySummary
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdView = New System.Windows.Forms.Button
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClient, Me.mnuPolicy, Me.mnuProcess, Me.mnuDocuments, Me.mnuReports, Me.mnuTasks, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(607, 24)
        Me.MainMenu1.TabIndex = 5
        Me.MainMenu1.Visible = False
        '
        'mnuClient
        '
        Me.mnuClient.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuClient.Name = "mnuClient"
        Me.mnuClient.Size = New System.Drawing.Size(46, 20)
        Me.mnuClient.Text = "&Client"
        Me.mnuClient.Visible = False
        '
        'mnuPolicy
        '
        Me.mnuPolicy.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPolicyDelete, Me.mnuPolicyCopy, Me.mnuPolicyMove, Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me.mnuSeperator, Me.mnuPolicyExit})
        Me.mnuPolicy.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuPolicy.MergeIndex = 0
        Me.mnuPolicy.Name = "mnuPolicy"
        Me.mnuPolicy.Size = New System.Drawing.Size(46, 20)
        Me.mnuPolicy.Text = "&Policy"
        '
        'mnuPolicyDelete
        '
        Me.mnuPolicyDelete.Name = "mnuPolicyDelete"
        Me.mnuPolicyDelete.Size = New System.Drawing.Size(130, 22)
        Me.mnuPolicyDelete.Text = "&Delete"
        '
        'mnuPolicyCopy
        '
        Me.mnuPolicyCopy.Name = "mnuPolicyCopy"
        Me.mnuPolicyCopy.Size = New System.Drawing.Size(130, 22)
        Me.mnuPolicyCopy.Text = "&Copy..."
        '
        'mnuPolicyMove
        '
        Me.mnuPolicyMove.Name = "mnuPolicyMove"
        Me.mnuPolicyMove.Size = New System.Drawing.Size(130, 22)
        Me.mnuPolicyMove.Text = "&Move..."
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(130, 22)
        '
        '_mnuRecentFile_1
        '
        Me._mnuRecentFile_1.Name = "_mnuRecentFile_1"
        Me._mnuRecentFile_1.Size = New System.Drawing.Size(130, 22)
        Me._mnuRecentFile_1.Text = "RecentFile1"
        '
        'mnuSeperator
        '
        Me.mnuSeperator.Name = "mnuSeperator"
        Me.mnuSeperator.Size = New System.Drawing.Size(127, 6)
        '
        'mnuPolicyExit
        '
        Me.mnuPolicyExit.Name = "mnuPolicyExit"
        Me.mnuPolicyExit.Size = New System.Drawing.Size(130, 22)
        Me.mnuPolicyExit.Text = "E&xit"
        '
        'mnuProcess
        '
        Me.mnuProcess.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGoToAccounts, Me.mnuGotoInsuredAccounts, Me.mnuGoTOTRansaction, Me.mnuGoToPolicy, Me.mnuGoToClaim, Me.mnuGoToDocumaster, Me.mnuGoToSharePoint, Me.mnuGoToSwift, Me.mnuGoToEvents, Me.mnuGoToRisk, Me.mnuGoToTextFiles, Me.mnuGotoNotes})
        Me.mnuProcess.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuProcess.MergeIndex = 1
        Me.mnuProcess.Name = "mnuProcess"
        Me.mnuProcess.Size = New System.Drawing.Size(44, 20)
        Me.mnuProcess.Text = "&GoTo"
        '
        'mnuGoToAccounts
        '
        Me.mnuGoToAccounts.Name = "mnuGoToAccounts"
        Me.mnuGoToAccounts.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToAccounts.Text = "&Accounts"
        '
        'mnuGotoInsuredAccounts
        '
        Me.mnuGotoInsuredAccounts.Name = "mnuGotoInsuredAccounts"
        Me.mnuGotoInsuredAccounts.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoInsuredAccounts.Text = "Ins&ured Accounts"
        '
        'mnuGoTOTRansaction
        '
        Me.mnuGoTOTRansaction.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoTransactionCash, Me.mnuGotoTransactionCredit, Me.mnuGotoTransactionDebit, Me.mnuGoTOTRansactionAJ, Me.mnuGoToTRansactionAJReversal})
        Me.mnuGoTOTRansaction.Name = "mnuGoTOTRansaction"
        Me.mnuGoTOTRansaction.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoTOTRansaction.Text = "&Transaction"
        '
        'mnuGotoTransactionCash
        '
        Me.mnuGotoTransactionCash.Name = "mnuGotoTransactionCash"
        Me.mnuGotoTransactionCash.Size = New System.Drawing.Size(235, 22)
        Me.mnuGotoTransactionCash.Text = "&Cash"
        '
        'mnuGotoTransactionCredit
        '
        Me.mnuGotoTransactionCredit.Name = "mnuGotoTransactionCredit"
        Me.mnuGotoTransactionCredit.Size = New System.Drawing.Size(235, 22)
        Me.mnuGotoTransactionCredit.Text = "C&redit"
        '
        'mnuGotoTransactionDebit
        '
        Me.mnuGotoTransactionDebit.Name = "mnuGotoTransactionDebit"
        Me.mnuGotoTransactionDebit.Size = New System.Drawing.Size(235, 22)
        Me.mnuGotoTransactionDebit.Text = "&Debit"
        '
        'mnuGoTOTRansactionAJ
        '
        Me.mnuGoTOTRansactionAJ.Name = "mnuGoTOTRansactionAJ"
        Me.mnuGoTOTRansactionAJ.Size = New System.Drawing.Size(235, 22)
        Me.mnuGoTOTRansactionAJ.Text = "&Manual Direct to Insurer"
        '
        'mnuGoToTRansactionAJReversal
        '
        Me.mnuGoToTRansactionAJReversal.Name = "mnuGoToTRansactionAJReversal"
        Me.mnuGoToTRansactionAJReversal.Size = New System.Drawing.Size(235, 22)
        Me.mnuGoToTRansactionAJReversal.Text = "Manual Direct to Insurer &Reversal"
        '
        'mnuGoToPolicy
        '
        Me.mnuGoToPolicy.Name = "mnuGoToPolicy"
        Me.mnuGoToPolicy.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToPolicy.Text = "&Policy"
        '
        'mnuGoToClaim
        '
        Me.mnuGoToClaim.Name = "mnuGoToClaim"
        Me.mnuGoToClaim.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToClaim.Text = "&Claim"
        '
        'mnuGoToDocumaster
        '
        Me.mnuGoToDocumaster.Name = "mnuGoToDocumaster"
        Me.mnuGoToDocumaster.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToDocumaster.Text = "&Documaster"
        '
        'mnuGoToSharePoint
        '
        Me.mnuGoToSharePoint.Name = "mnuGoToSharePoint"
        Me.mnuGoToSharePoint.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToSharePoint.Text = "&SharePoint"
        Me.mnuGoToSharePoint.Visible = False
        '
        'mnuGoToSwift
        '
        Me.mnuGoToSwift.Name = "mnuGoToSwift"
        Me.mnuGoToSwift.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToSwift.Text = "&Swift"
        '
        'mnuGoToEvents
        '
        Me.mnuGoToEvents.Name = "mnuGoToEvents"
        Me.mnuGoToEvents.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToEvents.Text = "&Events"
        '
        'mnuGoToRisk
        '
        Me.mnuGoToRisk.Name = "mnuGoToRisk"
        Me.mnuGoToRisk.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToRisk.Text = "&Risk"
        '
        'mnuGoToTextFiles
        '
        Me.mnuGoToTextFiles.Name = "mnuGoToTextFiles"
        Me.mnuGoToTextFiles.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToTextFiles.Text = "Te&xt Files"
        '
        'mnuGotoNotes
        '
        Me.mnuGotoNotes.Name = "mnuGotoNotes"
        Me.mnuGotoNotes.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoNotes.Text = "&Notes"
        '
        'mnuDocuments
        '
        Me.mnuDocuments.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDocumentsLetterWriting})
        Me.mnuDocuments.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuDocuments.MergeIndex = 2
        Me.mnuDocuments.Name = "mnuDocuments"
        Me.mnuDocuments.Size = New System.Drawing.Size(91, 20)
        Me.mnuDocuments.Text = "&Documentation"
        '
        'mnuDocumentsLetterWriting
        '
        Me.mnuDocumentsLetterWriting.Name = "mnuDocumentsLetterWriting"
        Me.mnuDocumentsLetterWriting.Size = New System.Drawing.Size(140, 22)
        Me.mnuDocumentsLetterWriting.Text = "&Letter Writing"
        '
        'mnuReports
        '
        Me.mnuReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportsClientSummary, Me.mnuReportsStatements})
        Me.mnuReports.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuReports.MergeIndex = 3
        Me.mnuReports.Name = "mnuReports"
        Me.mnuReports.Size = New System.Drawing.Size(57, 20)
        Me.mnuReports.Text = "&Reports"
        '
        'mnuReportsClientSummary
        '
        Me.mnuReportsClientSummary.Name = "mnuReportsClientSummary"
        Me.mnuReportsClientSummary.Size = New System.Drawing.Size(148, 22)
        Me.mnuReportsClientSummary.Text = "&Client Summary"
        '
        'mnuReportsStatements
        '
        Me.mnuReportsStatements.Name = "mnuReportsStatements"
        Me.mnuReportsStatements.Size = New System.Drawing.Size(148, 22)
        Me.mnuReportsStatements.Text = "&Statements"
        '
        'mnuTasks
        '
        Me.mnuTasks.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNewDiary, Me.mnuFind})
        Me.mnuTasks.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuTasks.MergeIndex = 4
        Me.mnuTasks.Name = "mnuTasks"
        Me.mnuTasks.Size = New System.Drawing.Size(46, 20)
        Me.mnuTasks.Text = "&Tasks"
        '
        'mnuNewDiary
        '
        Me.mnuNewDiary.Name = "mnuNewDiary"
        Me.mnuNewDiary.Size = New System.Drawing.Size(95, 22)
        Me.mnuNewDiary.Text = "&New"
        '
        'mnuFind
        '
        Me.mnuFind.Name = "mnuFind"
        Me.mnuFind.Size = New System.Drawing.Size(95, 22)
        Me.mnuFind.Text = "&Find"
        '
        'mnuWindow
        '
        Me.mnuWindow.MergeAction = System.Windows.Forms.MergeAction.Replace
        Me.mnuWindow.Name = "mnuWindow"
        Me.mnuWindow.Size = New System.Drawing.Size(62, 20)
        Me.mnuWindow.Text = "&Windows"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpAbout})
        Me.mnuHelp.MergeAction = System.Windows.Forms.MergeAction.Replace
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(40, 20)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(103, 22)
        Me.mnuHelpAbout.Text = "&About"
        '
        'uctPMUPolicySummary1
        '
        Me.uctPMUPolicySummary1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMUPolicySummary1.GeminiPolicyStatus = ""
        Me.uctPMUPolicySummary1.InsuranceFileCnt = 0
        Me.uctPMUPolicySummary1.InsuranceFileStructureId = 0
        Me.uctPMUPolicySummary1.InsuranceFolderCnt = 0
        Me.uctPMUPolicySummary1.Location = New System.Drawing.Point(8, 9)
        Me.uctPMUPolicySummary1.Name = "uctPMUPolicySummary1"
        Me.uctPMUPolicySummary1.PartyCnt = 0
        Me.uctPMUPolicySummary1.PolicyTypeId = 0
        Me.uctPMUPolicySummary1.RiskCodeId = ""
        Me.uctPMUPolicySummary1.RiskGroupId = Nothing
        Me.uctPMUPolicySummary1.RiskScreenId = 0
        Me.uctPMUPolicySummary1.Size = New System.Drawing.Size(585, 337)
        Me.uctPMUPolicySummary1.Status = 0
        Me.uctPMUPolicySummary1.TabIndex = 4
        Me.uctPMUPolicySummary1.Task = 0
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(360, 368)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(440, 368)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(520, 368)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 1
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(8, 368)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(73, 22)
        Me.cmdView.TabIndex = 0
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'frmPolicySummaryUnderwriting
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(607, 395)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.uctPMUPolicySummary1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 42)
        Me.MaximizeBox = False
        Me.Name = "frmPolicySummaryUnderwriting"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Underwriting Policy Summary"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializemnuRecentFile()
        Me.mnuRecentFile(0) = _mnuRecentFile_0
        Me.mnuRecentFile(1) = _mnuRecentFile_1
    End Sub
    Friend WithEvents mnuClient As System.Windows.Forms.ToolStripMenuItem
#End Region 
End Class