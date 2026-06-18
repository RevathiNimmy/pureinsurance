<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPolicyUnderwriting
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
        'Me.MdiParent = iPMBClientManager.frmMDI
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
    'Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_2 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_3 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_4 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_5 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuClientExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuPolicy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoAccounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoInsuredAccounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionCash As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionCredit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionDebit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionAJ As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionAJReversal As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransaction As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToClaim As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToDocumaster As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToSharePoint As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToSwift As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToEvents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToRisk As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToTextFiles As System.Windows.Forms.ToolStripMenuItem
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
	Public WithEvents cmdInstalment As System.Windows.Forms.Button
	Public WithEvents cmdFee As System.Windows.Forms.Button
	Public WithEvents cmdCommission As System.Windows.Forms.Button
	Public WithEvents cmdPolicyTax As System.Windows.Forms.Button
	Public WithEvents uctPMUPolicyControl1 As PMUPolicyControl.uctPMUPolicyControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public mnuRecentFile(5) As System.Windows.Forms.ToolStripMenuItem
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuClient = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPolicy = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPolicyDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPolicyCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPolicyMove = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_0 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_1 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_2 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_3 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_4 = New System.Windows.Forms.ToolStripMenuItem()
        Me._mnuRecentFile_5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSeperator = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuClientExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGoTo = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGotoAccounts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGotoInsuredAccounts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGotoTransaction = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGotoTransactionCash = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGotoTransactionCredit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGotoTransactionDebit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGotoTransactionAJ = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGotoTransactionAJReversal = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGoToClaim = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGoToDocumaster = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGoToSharePoint = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGoToSwift = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGoToEvents = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGoToRisk = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGoToTextFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGoToNotes = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDocumentation = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDocumentationLetterWriting = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReportsClientSummary = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReportsStatements = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTasks = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDiaryNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDiaryFind = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmdInstalment = New System.Windows.Forms.Button()
        Me.cmdFee = New System.Windows.Forms.Button()
        Me.cmdCommission = New System.Windows.Forms.Button()
        Me.cmdPolicyTax = New System.Windows.Forms.Button()
        Me.uctPMUPolicyControl1 = New PMUPolicyControl.uctPMUPolicyControl()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClient, Me.mnuPolicy, Me.mnuGoTo, Me.mnuDocumentation, Me.mnuReports, Me.mnuTasks, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(625, 24)
        Me.MainMenu1.TabIndex = 8
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
        Me.mnuPolicy.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPolicyDelete, Me.mnuPolicyCopy, Me.mnuPolicyMove, Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me._mnuRecentFile_2, Me._mnuRecentFile_3, Me._mnuRecentFile_4, Me._mnuRecentFile_5, Me.mnuSeperator, Me.mnuClientExit})
        Me.mnuPolicy.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuPolicy.MergeIndex = 0
        Me.mnuPolicy.Name = "mnuPolicy"
        Me.mnuPolicy.Size = New System.Drawing.Size(46, 20)
        Me.mnuPolicy.Text = "&Policy"
        '
        'mnuPolicyDelete
        '
        Me.mnuPolicyDelete.Name = "mnuPolicyDelete"
        Me.mnuPolicyDelete.Size = New System.Drawing.Size(105, 22)
        Me.mnuPolicyDelete.Text = "&Delete"
        '
        'mnuPolicyCopy
        '
        Me.mnuPolicyCopy.Name = "mnuPolicyCopy"
        Me.mnuPolicyCopy.Size = New System.Drawing.Size(105, 22)
        Me.mnuPolicyCopy.Text = "&Copy"
        '
        'mnuPolicyMove
        '
        Me.mnuPolicyMove.Name = "mnuPolicyMove"
        Me.mnuPolicyMove.Size = New System.Drawing.Size(105, 22)
        Me.mnuPolicyMove.Text = "&Move"
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(105, 22)
        '
        '_mnuRecentFile_1
        '
        Me._mnuRecentFile_1.Name = "_mnuRecentFile_1"
        Me._mnuRecentFile_1.Size = New System.Drawing.Size(105, 22)
        '
        '_mnuRecentFile_2
        '
        Me._mnuRecentFile_2.Name = "_mnuRecentFile_2"
        Me._mnuRecentFile_2.Size = New System.Drawing.Size(105, 22)
        '
        '_mnuRecentFile_3
        '
        Me._mnuRecentFile_3.Name = "_mnuRecentFile_3"
        Me._mnuRecentFile_3.Size = New System.Drawing.Size(105, 22)
        '
        '_mnuRecentFile_4
        '
        Me._mnuRecentFile_4.Name = "_mnuRecentFile_4"
        Me._mnuRecentFile_4.Size = New System.Drawing.Size(105, 22)
        '
        '_mnuRecentFile_5
        '
        Me._mnuRecentFile_5.Name = "_mnuRecentFile_5"
        Me._mnuRecentFile_5.Size = New System.Drawing.Size(105, 22)
        '
        'mnuSeperator
        '
        Me.mnuSeperator.Name = "mnuSeperator"
        Me.mnuSeperator.Size = New System.Drawing.Size(102, 6)
        '
        'mnuClientExit
        '
        Me.mnuClientExit.Name = "mnuClientExit"
        Me.mnuClientExit.Size = New System.Drawing.Size(105, 22)
        Me.mnuClientExit.Text = "E&xit"
        '
        'mnuGoTo
        '
        Me.mnuGoTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoAccounts, Me.mnuGotoInsuredAccounts, Me.mnuGotoTransaction, Me.mnuGoToClaim, Me.mnuGoToDocumaster, Me.mnuGoToSharePoint, Me.mnuGoToSwift, Me.mnuGoToEvents, Me.mnuGoToRisk, Me.mnuGoToTextFiles, Me.mnuGoToNotes})
        Me.mnuGoTo.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuGoTo.MergeIndex = 1
        Me.mnuGoTo.Name = "mnuGoTo"
        Me.mnuGoTo.Size = New System.Drawing.Size(44, 20)
        Me.mnuGoTo.Text = "&GoTo"
        '
        'mnuGotoAccounts
        '
        Me.mnuGotoAccounts.Name = "mnuGotoAccounts"
        Me.mnuGotoAccounts.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoAccounts.Text = "&Accounts"
        '
        'mnuGotoInsuredAccounts
        '
        Me.mnuGotoInsuredAccounts.Name = "mnuGotoInsuredAccounts"
        Me.mnuGotoInsuredAccounts.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoInsuredAccounts.Text = "Ins&ured Accounts"
        '
        'mnuGotoTransaction
        '
        Me.mnuGotoTransaction.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoTransactionCash, Me.mnuGotoTransactionCredit, Me.mnuGotoTransactionDebit, Me.mnuGotoTransactionAJ, Me.mnuGotoTransactionAJReversal})
        Me.mnuGotoTransaction.Name = "mnuGotoTransaction"
        Me.mnuGotoTransaction.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoTransaction.Text = "&Transaction"
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
        'mnuGotoTransactionAJ
        '
        Me.mnuGotoTransactionAJ.Name = "mnuGotoTransactionAJ"
        Me.mnuGotoTransactionAJ.Size = New System.Drawing.Size(235, 22)
        Me.mnuGotoTransactionAJ.Text = "&Manual Direct to Insurer"
        '
        'mnuGotoTransactionAJReversal
        '
        Me.mnuGotoTransactionAJReversal.Name = "mnuGotoTransactionAJReversal"
        Me.mnuGotoTransactionAJReversal.Size = New System.Drawing.Size(235, 22)
        Me.mnuGotoTransactionAJReversal.Text = "Manual Direct to Insurer &Reversal"
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
        'mnuGoToNotes
        '
        Me.mnuGoToNotes.Name = "mnuGoToNotes"
        Me.mnuGoToNotes.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToNotes.Text = "&Notes"
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
        'cmdInstalment
        '
        Me.cmdInstalment.BackColor = System.Drawing.SystemColors.Control
        Me.cmdInstalment.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdInstalment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdInstalment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdInstalment.Location = New System.Drawing.Point(304, 542)
        Me.cmdInstalment.Name = "cmdInstalment"
        Me.cmdInstalment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdInstalment.Size = New System.Drawing.Size(92, 22)
        Me.cmdInstalment.TabIndex = 7
        Me.cmdInstalment.Text = "&Instalment"
        Me.cmdInstalment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdInstalment.UseVisualStyleBackColor = False
        '
        'cmdFee
        '
        Me.cmdFee.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFee.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFee.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFee.Location = New System.Drawing.Point(107, 542)
        Me.cmdFee.Name = "cmdFee"
        Me.cmdFee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFee.Size = New System.Drawing.Size(92, 22)
        Me.cmdFee.TabIndex = 6
        Me.cmdFee.Text = "Policy &Fee"
        Me.cmdFee.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFee.UseVisualStyleBackColor = False
        '
        'cmdCommission
        '
        Me.cmdCommission.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCommission.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCommission.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCommission.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCommission.Location = New System.Drawing.Point(205, 542)
        Me.cmdCommission.Name = "cmdCommission"
        Me.cmdCommission.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCommission.Size = New System.Drawing.Size(92, 22)
        Me.cmdCommission.TabIndex = 5
        Me.cmdCommission.Text = "Co&mmission"
        Me.cmdCommission.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCommission.UseVisualStyleBackColor = False
        '
        'cmdPolicyTax
        '
        Me.cmdPolicyTax.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPolicyTax.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPolicyTax.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPolicyTax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPolicyTax.Location = New System.Drawing.Point(8, 542)
        Me.cmdPolicyTax.Name = "cmdPolicyTax"
        Me.cmdPolicyTax.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPolicyTax.Size = New System.Drawing.Size(92, 22)
        Me.cmdPolicyTax.TabIndex = 4
        Me.cmdPolicyTax.Text = "Policy &Tax"
        Me.cmdPolicyTax.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPolicyTax.UseVisualStyleBackColor = False
        '
        'uctPMUPolicyControl1
        '
        Me.uctPMUPolicyControl1.BackDatedMTAsAllowed = False
        Me.uctPMUPolicyControl1.BackStyle = 0
        Me.uctPMUPolicyControl1.BorderStyle = 0
        Me.uctPMUPolicyControl1.BusinessTypeId = Nothing
        Me.uctPMUPolicyControl1.Enabled = False
        Me.uctPMUPolicyControl1.EventRaised = False
        Me.uctPMUPolicyControl1.FromEvent = False
        Me.uctPMUPolicyControl1.InsuranceFileCnt = 0
        Me.uctPMUPolicyControl1.InsuranceFolderCnt = 0
        Me.uctPMUPolicyControl1.IsExit = False
        Me.uctPMUPolicyControl1.IsMTATemp = "False"
        Me.uctPMUPolicyControl1.IsPriorDate = False
        Me.uctPMUPolicyControl1.IsRenewal = False
        Me.uctPMUPolicyControl1.IsRenewed = False
        Me.uctPMUPolicyControl1.LapseAQuote = False
        Me.uctPMUPolicyControl1.LapsedDate = New Date(CType(0, Long))
        Me.uctPMUPolicyControl1.Location = New System.Drawing.Point(0, 6)
        Me.uctPMUPolicyControl1.Name = "uctPMUPolicyControl1"
        Me.uctPMUPolicyControl1.PartyCnt = 0
        Me.uctPMUPolicyControl1.PMRaiseEvent = False
        Me.uctPMUPolicyControl1.PolicyTypeId = Nothing
        Me.uctPMUPolicyControl1.ProductId = 0
        Me.uctPMUPolicyControl1.Renewaldate = New Date(CType(0, Long))
        Me.uctPMUPolicyControl1.RiskCodeId = Nothing
        Me.uctPMUPolicyControl1.RiskGroupId = Nothing
        Me.uctPMUPolicyControl1.SelectedPolicyStatus = ""
        Me.uctPMUPolicyControl1.SetQuoteToLapsed = False
        Me.uctPMUPolicyControl1.Size = New System.Drawing.Size(625, 530)
        Me.uctPMUPolicyControl1.SourceId = 0
        Me.uctPMUPolicyControl1.Status = 0
        Me.uctPMUPolicyControl1.TabIndex = 3
        Me.uctPMUPolicyControl1.Task = 0
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(376, 566)
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
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(456, 566)
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
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 566)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 0
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'frmPolicyUnderwriting
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(628, 593)
        Me.Controls.Add(Me.cmdInstalment)
        Me.Controls.Add(Me.cmdFee)
        Me.Controls.Add(Me.cmdCommission)
        Me.Controls.Add(Me.cmdPolicyTax)
        Me.Controls.Add(Me.uctPMUPolicyControl1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(11, 30)
        Me.MaximizeBox = False
        Me.Name = "frmPolicyUnderwriting"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "UnderwritingPolicy"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializemnuRecentFile()
		Me.mnuRecentFile(0) = _mnuRecentFile_0
		Me.mnuRecentFile(1) = _mnuRecentFile_1
		Me.mnuRecentFile(2) = _mnuRecentFile_2
		Me.mnuRecentFile(3) = _mnuRecentFile_3
		Me.mnuRecentFile(4) = _mnuRecentFile_4
		Me.mnuRecentFile(5) = _mnuRecentFile_5
    End Sub
    Friend WithEvents mnuClient As System.Windows.Forms.ToolStripMenuItem
#End Region 
End Class