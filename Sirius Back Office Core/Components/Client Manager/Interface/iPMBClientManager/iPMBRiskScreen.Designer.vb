<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSBORiskScreen
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
	Public WithEvents mnuGoToSwift As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToEvents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToTextFiles As System.Windows.Forms.ToolStripMenuItem
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
	Public WithEvents RiskScreen1 As uctRiskScreenControl.RiskScreen
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
        Me.mnuGoToSwift = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToEvents = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToTextFiles = New System.Windows.Forms.ToolStripMenuItem
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
        Me.RiskScreen1 = New uctRiskScreenControl.RiskScreen
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
        Me.MainMenu1.TabIndex = 4
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
        'mnuClientExit
        '
        Me.mnuClientExit.Name = "mnuClientExit"
        Me.mnuClientExit.Size = New System.Drawing.Size(130, 22)
        Me.mnuClientExit.Text = "E&xit"
        '
        'mnuGoTo
        '
        Me.mnuGoTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoAccounts, Me.mnuGotoTransaction, Me.mnuGoToClaim, Me.mnuGoToDocumaster, Me.mnuGoToSwift, Me.mnuGoToEvents, Me.mnuGoToTextFiles})
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
        'RiskScreen1
        '
        Me.RiskScreen1.BackStyle = 0
        Me.RiskScreen1.BaseCaseID = 0
        Me.RiskScreen1.BorderStyle = 0
        Me.RiskScreen1.CaseID = 0
        Me.RiskScreen1.ChildIndex = 0
        Me.RiskScreen1.ChildObjectName = ""
        Me.RiskScreen1.ChildOIKey = ""
        Me.RiskScreen1.ClaimID = 0
        Me.RiskScreen1.ClaimInsFileCnt = 0
        Me.RiskScreen1.ClaimPerilID = 0
        Me.RiskScreen1.ClaimRiskId = 0
        Me.RiskScreen1.ClaimTransactionType = ""
        Me.RiskScreen1.ColumnName = Nothing
        Me.RiskScreen1.DeclineReasons = ""
        Me.RiskScreen1.Enabled = False
        Me.RiskScreen1.FromEvent = False
        Me.RiskScreen1.GISObjectName = ""
        Me.RiskScreen1.InsuranceFileCnt = 0
        Me.RiskScreen1.InsuranceFolderCnt = 0
        Me.RiskScreen1.IsAutoReinsured = 0
        Me.RiskScreen1.IsRiAtRiskLevel = 0
        Me.RiskScreen1.KeyArray = Nothing
        Me.RiskScreen1.Location = New System.Drawing.Point(8, 7)
        Me.RiskScreen1.LossSchedule = False
        Me.RiskScreen1.LossScheduleTypeId = 0
        Me.RiskScreen1.Messages = ""
        Me.RiskScreen1.Name = "RiskScreen1"
        Me.RiskScreen1.ObjectType = 0
        Me.RiskScreen1.ParentObjectName = ""
        Me.RiskScreen1.ParentOIKey = ""
        Me.RiskScreen1.PartyCnt = 0
        Me.RiskScreen1.PartySourceID = 0
        Me.RiskScreen1.PerilID = 0
        Me.RiskScreen1.PerilTypeId = 0
        Me.RiskScreen1.ProductId = 0
        Me.RiskScreen1.PropertyName = Nothing
        Me.RiskScreen1.QuoteType = ""
        Me.RiskScreen1.ReferReasons = ""
        Me.RiskScreen1.RiskId = 0
        Me.RiskScreen1.RiskTypeId = 0
        Me.RiskScreen1.ScreenDesc = ""
        Me.RiskScreen1.ScreenId = 0
        Me.RiskScreen1.ScreenValues = Nothing
        Me.RiskScreen1.ShortName = ""
        Me.RiskScreen1.Size = New System.Drawing.Size(601, 401)
        Me.RiskScreen1.Stage = 0
        Me.RiskScreen1.Status = 0
        Me.RiskScreen1.SubScreen = False
        Me.RiskScreen1.TabIndex = 3
        Me.RiskScreen1.Task = 0
        Me.RiskScreen1.ValueEdited = False
        Me.RiskScreen1.ValueEditedForIndex = 0
        Me.RiskScreen1.WorkClaimID = 0
        Me.RiskScreen1.WorkClaimPerilID = 0
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(376, 434)
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
        Me.cmdCancel.Location = New System.Drawing.Point(456, 434)
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
        Me.cmdHelp.Location = New System.Drawing.Point(536, 434)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 0
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'frmSBORiskScreen
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(617, 460)
        Me.Controls.Add(Me.RiskScreen1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(10, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSBORiskScreen"
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