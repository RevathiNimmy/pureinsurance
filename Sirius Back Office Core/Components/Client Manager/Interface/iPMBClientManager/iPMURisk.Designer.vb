<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRiskUnderwriting
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
    'As used below
    'Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuClientExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuPolicy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoAccounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionCash As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionCredit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionDebit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransaction As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToClaim As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToDocumaster As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToSharePoint As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToSwift As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToEvents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToTextFiles As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoPrivateNotes As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoPublicNotes As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToNotes As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoTo As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentationLetterWriting As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentation As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuReportsClientSummary As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuReportsStatements As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuReports As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDiaryNew As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDiaryFind As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTasks As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents uctRiskScreenControl As uctRiskScreenControl.RiskScreen
	Public WithEvents cmdReInsurance As System.Windows.Forms.Button
	Public WithEvents cmdPremium As System.Windows.Forms.Button
	Public WithEvents cmdRiskTax As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
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
        Me.mnuClientExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoTo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoAccounts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransaction = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionCash = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionCredit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionDebit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToClaim = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToDocumaster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToSharePoint = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToSwift = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToEvents = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToTextFiles = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoPrivateNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoPublicNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentation = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentationLetterWriting = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsClientSummary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsStatements = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTasks = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDiaryNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDiaryFind = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.uctRiskScreenControl = New uctRiskScreenControl.RiskScreen
        Me.cmdReInsurance = New System.Windows.Forms.Button
        Me.cmdPremium = New System.Windows.Forms.Button
        Me.cmdRiskTax = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClient, Me.mnuPolicy, Me.mnuGoTo, Me.mnuDocumentation, Me.mnuReports, Me.mnuTasks, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(617, 24)
        Me.MainMenu1.TabIndex = 7
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
        Me.mnuPolicy.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPolicyDelete, Me.mnuPolicyCopy, Me.mnuPolicyMove, Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me.mnuSeperator, Me.mnuClientExit})
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
        Me.mnuPolicyCopy.Text = "&Copy"
        '
        'mnuPolicyMove
        '
        Me.mnuPolicyMove.Name = "mnuPolicyMove"
        Me.mnuPolicyMove.Size = New System.Drawing.Size(130, 22)
        Me.mnuPolicyMove.Text = "&Move"
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(130, 22)
        Me._mnuRecentFile_0.Visible = False
        '
        '_mnuRecentFile_1
        '
        Me._mnuRecentFile_1.Name = "_mnuRecentFile_1"
        Me._mnuRecentFile_1.Size = New System.Drawing.Size(130, 22)
        Me._mnuRecentFile_1.Text = "RecentFile1"
        Me._mnuRecentFile_1.Visible = False
        '
        'mnuSeperator
        '
        Me.mnuSeperator.Name = "mnuSeperator"
        Me.mnuSeperator.Size = New System.Drawing.Size(127, 6)
        '
        'mnuClientExit
        '
        Me.mnuClientExit.Name = "mnuClientExit"
        Me.mnuClientExit.Size = New System.Drawing.Size(130, 22)
        Me.mnuClientExit.Text = "E&xit"
        '
        'mnuGoTo
        '
        Me.mnuGoTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoAccounts, Me.mnuGotoTransaction, Me.mnuGoToClaim, Me.mnuGoToDocumaster, Me.mnuGoToSharePoint, Me.mnuGoToSwift, Me.mnuGoToEvents, Me.mnuGoToTextFiles, Me.mnuGoToNotes})
        Me.mnuGoTo.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuGoTo.MergeIndex = 1
        Me.mnuGoTo.Name = "mnuGoTo"
        Me.mnuGoTo.Size = New System.Drawing.Size(44, 20)
        Me.mnuGoTo.Text = "&GoTo"
        '
        'mnuGotoAccounts
        '
        Me.mnuGotoAccounts.Name = "mnuGotoAccounts"
        Me.mnuGotoAccounts.Size = New System.Drawing.Size(131, 22)
        Me.mnuGotoAccounts.Text = "&Accounts"
        '
        'mnuGotoTransaction
        '
        Me.mnuGotoTransaction.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoTransactionCash, Me.mnuGotoTransactionCredit, Me.mnuGotoTransactionDebit})
        Me.mnuGotoTransaction.Name = "mnuGotoTransaction"
        Me.mnuGotoTransaction.Size = New System.Drawing.Size(131, 22)
        Me.mnuGotoTransaction.Text = "&Transaction"
        '
        'mnuGotoTransactionCash
        '
        Me.mnuGotoTransactionCash.Name = "mnuGotoTransactionCash"
        Me.mnuGotoTransactionCash.Size = New System.Drawing.Size(103, 22)
        Me.mnuGotoTransactionCash.Text = "&Cash"
        '
        'mnuGotoTransactionCredit
        '
        Me.mnuGotoTransactionCredit.Name = "mnuGotoTransactionCredit"
        Me.mnuGotoTransactionCredit.Size = New System.Drawing.Size(103, 22)
        Me.mnuGotoTransactionCredit.Text = "C&redit"
        '
        'mnuGotoTransactionDebit
        '
        Me.mnuGotoTransactionDebit.Name = "mnuGotoTransactionDebit"
        Me.mnuGotoTransactionDebit.Size = New System.Drawing.Size(103, 22)
        Me.mnuGotoTransactionDebit.Text = "&Debit"
        '
        'mnuGoToClaim
        '
        Me.mnuGoToClaim.Name = "mnuGoToClaim"
        Me.mnuGoToClaim.Size = New System.Drawing.Size(131, 22)
        Me.mnuGoToClaim.Text = "&Claim"
        '
        'mnuGoToDocumaster
        '
        Me.mnuGoToDocumaster.Name = "mnuGoToDocumaster"
        Me.mnuGoToDocumaster.Size = New System.Drawing.Size(131, 22)
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
        Me.mnuGoToSwift.Size = New System.Drawing.Size(131, 22)
        Me.mnuGoToSwift.Text = "&Swift"
        '
        'mnuGoToEvents
        '
        Me.mnuGoToEvents.Name = "mnuGoToEvents"
        Me.mnuGoToEvents.Size = New System.Drawing.Size(131, 22)
        Me.mnuGoToEvents.Text = "&Events"
        '
        'mnuGoToTextFiles
        '
        Me.mnuGoToTextFiles.Name = "mnuGoToTextFiles"
        Me.mnuGoToTextFiles.Size = New System.Drawing.Size(131, 22)
        Me.mnuGoToTextFiles.Text = "Te&xt Files"
        '
        'mnuGoToNotes
        '
        Me.mnuGoToNotes.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoPrivateNotes, Me.mnuGotoPublicNotes})
        Me.mnuGoToNotes.Name = "mnuGoToNotes"
        Me.mnuGoToNotes.Size = New System.Drawing.Size(131, 22)
        Me.mnuGoToNotes.Text = "&Notes"
        '
        'mnuGotoPrivateNotes
        '
        Me.mnuGotoPrivateNotes.Name = "mnuGotoPrivateNotes"
        Me.mnuGotoPrivateNotes.Size = New System.Drawing.Size(108, 22)
        Me.mnuGotoPrivateNotes.Text = "P&rivate"
        '
        'mnuGotoPublicNotes
        '
        Me.mnuGotoPublicNotes.Name = "mnuGotoPublicNotes"
        Me.mnuGotoPublicNotes.Size = New System.Drawing.Size(108, 22)
        Me.mnuGotoPublicNotes.Text = "P&ublic"
        '
        'mnuDocumentation
        '
        Me.mnuDocumentation.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDocumentationLetterWriting})
        Me.mnuDocumentation.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuDocumentation.MergeIndex = 2
        Me.mnuDocumentation.Name = "mnuDocumentation"
        Me.mnuDocumentation.Size = New System.Drawing.Size(91, 20)
        Me.mnuDocumentation.Text = "&Documentation"
        '
        'mnuDocumentationLetterWriting
        '
        Me.mnuDocumentationLetterWriting.Name = "mnuDocumentationLetterWriting"
        Me.mnuDocumentationLetterWriting.Size = New System.Drawing.Size(140, 22)
        Me.mnuDocumentationLetterWriting.Text = "&Letter Writing"
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
        Me.mnuTasks.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDiaryNew, Me.mnuDiaryFind})
        Me.mnuTasks.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuTasks.MergeIndex = 4
        Me.mnuTasks.Name = "mnuTasks"
        Me.mnuTasks.Size = New System.Drawing.Size(46, 20)
        Me.mnuTasks.Text = "&Tasks"
        '
        'mnuDiaryNew
        '
        Me.mnuDiaryNew.Name = "mnuDiaryNew"
        Me.mnuDiaryNew.Size = New System.Drawing.Size(95, 22)
        Me.mnuDiaryNew.Text = "&New"
        '
        'mnuDiaryFind
        '
        Me.mnuDiaryFind.Name = "mnuDiaryFind"
        Me.mnuDiaryFind.Size = New System.Drawing.Size(95, 22)
        Me.mnuDiaryFind.Text = "&Find"
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
        'uctRiskScreenControl
        '
        Me.uctRiskScreenControl.BackStyle = 0
        Me.uctRiskScreenControl.BaseCaseID = 0
        Me.uctRiskScreenControl.BorderStyle = 0
        Me.uctRiskScreenControl.CaseID = 0
        Me.uctRiskScreenControl.ChildIndex = 0
        Me.uctRiskScreenControl.ChildObjectName = ""
        Me.uctRiskScreenControl.ChildOIKey = ""
        Me.uctRiskScreenControl.ClaimID = 0
        Me.uctRiskScreenControl.ClaimInsFileCnt = 0
        Me.uctRiskScreenControl.ClaimPerilID = 0
        Me.uctRiskScreenControl.ClaimRiskId = 0
        Me.uctRiskScreenControl.ClaimTransactionType = ""
        Me.uctRiskScreenControl.ColumnName = Nothing
        Me.uctRiskScreenControl.DeclineReasons = ""
        Me.uctRiskScreenControl.Enabled = False
        Me.uctRiskScreenControl.FromEvent = False
        Me.uctRiskScreenControl.GISObjectName = ""
        Me.uctRiskScreenControl.InsuranceFileCnt = 0
        Me.uctRiskScreenControl.InsuranceFolderCnt = 0
        Me.uctRiskScreenControl.IsAutoReinsured = 0
        Me.uctRiskScreenControl.IsRiAtRiskLevel = 0
        Me.uctRiskScreenControl.KeyArray = Nothing
        Me.uctRiskScreenControl.Location = New System.Drawing.Point(8, 8)
        Me.uctRiskScreenControl.LossSchedule = False
        Me.uctRiskScreenControl.LossScheduleTypeId = 0
        Me.uctRiskScreenControl.Messages = ""
        Me.uctRiskScreenControl.Name = "uctRiskScreenControl"
        Me.uctRiskScreenControl.ObjectType = 0
        Me.uctRiskScreenControl.ParentObjectName = ""
        Me.uctRiskScreenControl.ParentOIKey = ""
        Me.uctRiskScreenControl.PartyCnt = 0
        Me.uctRiskScreenControl.PartySourceID = 0
        Me.uctRiskScreenControl.PerilID = 0
        Me.uctRiskScreenControl.PerilTypeId = 0
        Me.uctRiskScreenControl.ProductId = 0
        Me.uctRiskScreenControl.PropertyName = Nothing
        Me.uctRiskScreenControl.QuoteType = ""
        Me.uctRiskScreenControl.ReferReasons = ""
        Me.uctRiskScreenControl.RiskId = 0
        Me.uctRiskScreenControl.RiskTypeId = 0
        Me.uctRiskScreenControl.ScreenDesc = ""
        Me.uctRiskScreenControl.ScreenId = 0
        Me.uctRiskScreenControl.ScreenValues = Nothing
        Me.uctRiskScreenControl.ShortName = ""
        Me.uctRiskScreenControl.Size = New System.Drawing.Size(601, 401)
        Me.uctRiskScreenControl.Stage = 0
        Me.uctRiskScreenControl.Status = 0
        Me.uctRiskScreenControl.SubScreen = False
        Me.uctRiskScreenControl.TabIndex = 6
        Me.uctRiskScreenControl.Task = 0
        Me.uctRiskScreenControl.ValueEdited = False
        Me.uctRiskScreenControl.ValueEditedForIndex = 0
        Me.uctRiskScreenControl.WorkClaimID = 0
        Me.uctRiskScreenControl.WorkClaimPerilID = 0
        '
        'cmdReInsurance
        '
        Me.cmdReInsurance.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReInsurance.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReInsurance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReInsurance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReInsurance.Location = New System.Drawing.Point(189, 440)
        Me.cmdReInsurance.Name = "cmdReInsurance"
        Me.cmdReInsurance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReInsurance.Size = New System.Drawing.Size(87, 22)
        Me.cmdReInsurance.TabIndex = 5
        Me.cmdReInsurance.Text = "&ReInsurance"
        Me.cmdReInsurance.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReInsurance.UseVisualStyleBackColor = False
        '
        'cmdPremium
        '
        Me.cmdPremium.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPremium.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPremium.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPremium.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPremium.Location = New System.Drawing.Point(8, 440)
        Me.cmdPremium.Name = "cmdPremium"
        Me.cmdPremium.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPremium.Size = New System.Drawing.Size(82, 22)
        Me.cmdPremium.TabIndex = 4
        Me.cmdPremium.Text = "&Premium"
        Me.cmdPremium.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPremium.UseVisualStyleBackColor = False
        '
        'cmdRiskTax
        '
        Me.cmdRiskTax.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRiskTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRiskTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRiskTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRiskTax.Location = New System.Drawing.Point(98, 440)
        Me.cmdRiskTax.Name = "cmdRiskTax"
        Me.cmdRiskTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRiskTax.Size = New System.Drawing.Size(82, 22)
        Me.cmdRiskTax.TabIndex = 3
        Me.cmdRiskTax.Text = "Risk &Tax"
        Me.cmdRiskTax.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRiskTax.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(376, 440)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(456, 440)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 440)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 0
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'frmRiskUnderwriting
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(617, 470)
        Me.Controls.Add(Me.uctRiskScreenControl)
        Me.Controls.Add(Me.cmdReInsurance)
        Me.Controls.Add(Me.cmdPremium)
        Me.Controls.Add(Me.cmdRiskTax)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(10, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRiskUnderwriting"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Risk"
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