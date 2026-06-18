<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTextFiles
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
        'Me.MdiParent = m_ofrmMDI
        Me.MdiParent = parentForm
        m_parentMdiForm = parentForm
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
	Public WithEvents mnuTextFileClose As System.Windows.Forms.ToolStripMenuItem
    'Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_2 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_3 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_4 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_5 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuTextFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTextFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToAccounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoInsuredAccounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionCash As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionCredit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionDebit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransaction As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToPolicy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToClaim As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToDocumaster As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToSharePoint As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToSwift As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToEvents As System.Windows.Forms.ToolStripMenuItem
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdPrint As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents uctTextFiles1 As uctTextFilesControl.uctTextFiles
	Public WithEvents ImageList2 As System.Windows.Forms.ImageList
	Public mnuRecentFile(5) As System.Windows.Forms.ToolStripMenuItem
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTextFiles))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuClient = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTextFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTextFileClose = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_0 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_1 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_2 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_3 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_4 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_5 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeperator = New System.Windows.Forms.ToolStripSeparator
        Me.mnuTextFileExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProcess = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToAccounts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoInsuredAccounts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransaction = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionCash = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionCredit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionDebit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToPolicy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToClaim = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToDocumaster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToSharePoint = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToSwift = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToEvents = New System.Windows.Forms.ToolStripMenuItem
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
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdPrint = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.uctTextFiles1 = New uctTextFilesControl.uctTextFiles
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClient, Me.mnuTextFile, Me.mnuProcess, Me.mnuDocuments, Me.mnuReports, Me.mnuTasks, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(607, 24)
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
        'mnuTextFile
        '
        Me.mnuTextFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTextFileClose, Me.ToolStripSeparator1, Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me._mnuRecentFile_2, Me._mnuRecentFile_3, Me._mnuRecentFile_4, Me._mnuRecentFile_5, Me.mnuSeperator, Me.mnuTextFileExit})
        Me.mnuTextFile.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuTextFile.MergeIndex = 0
        Me.mnuTextFile.Name = "mnuTextFile"
        Me.mnuTextFile.Size = New System.Drawing.Size(57, 20)
        Me.mnuTextFile.Text = "&TextFile"
        '
        'mnuTextFileClose
        '
        Me.mnuTextFileClose.Name = "mnuTextFileClose"
        Me.mnuTextFileClose.Size = New System.Drawing.Size(152, 22)
        Me.mnuTextFileClose.Text = "&Close"
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(152, 22)
        Me._mnuRecentFile_0.Visible = False
        '
        '_mnuRecentFile_1
        '
        Me._mnuRecentFile_1.Name = "_mnuRecentFile_1"
        Me._mnuRecentFile_1.Size = New System.Drawing.Size(152, 22)
        Me._mnuRecentFile_1.Visible = False
        '
        '_mnuRecentFile_2
        '
        Me._mnuRecentFile_2.Name = "_mnuRecentFile_2"
        Me._mnuRecentFile_2.Size = New System.Drawing.Size(152, 22)
        Me._mnuRecentFile_2.Visible = False
        '
        '_mnuRecentFile_3
        '
        Me._mnuRecentFile_3.Name = "_mnuRecentFile_3"
        Me._mnuRecentFile_3.Size = New System.Drawing.Size(152, 22)
        Me._mnuRecentFile_3.Visible = False
        '
        '_mnuRecentFile_4
        '
        Me._mnuRecentFile_4.Name = "_mnuRecentFile_4"
        Me._mnuRecentFile_4.Size = New System.Drawing.Size(152, 22)
        Me._mnuRecentFile_4.Visible = False
        '
        '_mnuRecentFile_5
        '
        Me._mnuRecentFile_5.Name = "_mnuRecentFile_5"
        Me._mnuRecentFile_5.Size = New System.Drawing.Size(152, 22)
        Me._mnuRecentFile_5.Visible = False
        '
        'mnuSeperator
        '
        Me.mnuSeperator.Name = "mnuSeperator"
        Me.mnuSeperator.Size = New System.Drawing.Size(149, 6)
        '
        'mnuTextFileExit
        '
        Me.mnuTextFileExit.Name = "mnuTextFileExit"
        Me.mnuTextFileExit.Size = New System.Drawing.Size(152, 22)
        Me.mnuTextFileExit.Text = "E&xit"
        '
        'mnuProcess
        '
        Me.mnuProcess.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGoToAccounts, Me.mnuGotoInsuredAccounts, Me.mnuGotoTransaction, Me.mnuGoToPolicy, Me.mnuGoToClaim, Me.mnuGoToDocumaster, Me.mnuGoToSharePoint, Me.mnuGoToSwift, Me.mnuGoToEvents, Me.mnuGotoNotes})
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
        'mnuGotoTransaction
        '
        Me.mnuGotoTransaction.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoTransactionCash, Me.mnuGotoTransactionCredit, Me.mnuGotoTransactionDebit})
        Me.mnuGotoTransaction.Name = "mnuGotoTransaction"
        Me.mnuGotoTransaction.Size = New System.Drawing.Size(158, 22)
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
        Me.mnuDocumentsLetterWriting.Size = New System.Drawing.Size(152, 22)
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
        Me.mnuReportsClientSummary.Size = New System.Drawing.Size(152, 22)
        Me.mnuReportsClientSummary.Text = "&Client Summary"
        '
        'mnuReportsStatements
        '
        Me.mnuReportsStatements.Name = "mnuReportsStatements"
        Me.mnuReportsStatements.Size = New System.Drawing.Size(152, 22)
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
        Me.mnuNewDiary.Size = New System.Drawing.Size(152, 22)
        Me.mnuNewDiary.Text = "&New"
        '
        'mnuFind
        '
        Me.mnuFind.Name = "mnuFind"
        Me.mnuFind.Size = New System.Drawing.Size(152, 22)
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
        Me.mnuHelpAbout.Size = New System.Drawing.Size(152, 22)
        Me.mnuHelpAbout.Text = "&About"
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(368, 356)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 6
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
        Me.cmdCancel.Location = New System.Drawing.Point(448, 356)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 5
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
        Me.cmdHelp.Location = New System.Drawing.Point(528, 356)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 4
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(8, 356)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 3
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPrint.Enabled = False
        Me.cmdPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Location = New System.Drawing.Point(88, 356)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(73, 22)
        Me.cmdPrint.TabIndex = 2
        Me.cmdPrint.Text = "&Print"
        Me.cmdPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = False
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(168, 356)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 1
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'uctTextFiles1
        '
        Me.uctTextFiles1.ClaimCnt = 0
        Me.uctTextFiles1.ClaimDesc = ""
        Me.uctTextFiles1.FileNumber = 0
        Me.uctTextFiles1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctTextFiles1.InsuranceFileCnt = 0
        Me.uctTextFiles1.InsuranceFolderCnt = 0
        Me.uctTextFiles1.Location = New System.Drawing.Point(0, 8)
        Me.uctTextFiles1.Name = "uctTextFiles1"
        Me.uctTextFiles1.PartyCnt = 0
        Me.uctTextFiles1.PolicyDesc = ""
        Me.uctTextFiles1.RiskCodeId = 0
        Me.uctTextFiles1.RiskGroupId = 0
        Me.uctTextFiles1.Size = New System.Drawing.Size(621, 386)
        Me.uctTextFiles1.SlotNumber = 0
        Me.uctTextFiles1.Status = 0
        Me.uctTextFiles1.TabIndex = 0
        Me.uctTextFiles1.Task = 0
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList2.Images.SetKeyName(0, "")
        Me.ImageList2.Images.SetKeyName(1, "")
        Me.ImageList2.Images.SetKeyName(2, "")
        Me.ImageList2.Images.SetKeyName(3, "")
        Me.ImageList2.Images.SetKeyName(4, "")
        Me.ImageList2.Images.SetKeyName(5, "")
        Me.ImageList2.Images.SetKeyName(6, "")
        Me.ImageList2.Images.SetKeyName(7, "")
        Me.ImageList2.Images.SetKeyName(8, "")
        Me.ImageList2.Images.SetKeyName(9, "")
        Me.ImageList2.Images.SetKeyName(10, "")
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(149, 6)
        '
        'frmTextFiles
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.AutoSize = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(607, 386)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.uctTextFiles1)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.Name = "frmTextFiles"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Text Files"
        Me.TopMost = True
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
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
#End Region 
End Class