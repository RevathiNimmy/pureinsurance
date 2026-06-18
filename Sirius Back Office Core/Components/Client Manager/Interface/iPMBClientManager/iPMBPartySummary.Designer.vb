<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPartySummary
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

        'Developer Guide No. 
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
	Public WithEvents mnuClientOpen As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuClientCopy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuClientMove As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuClientClose As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripSeparator
	Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuClientExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuClient As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToAccounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoInsuredAccounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToPolicy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionCash As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoTransactionFee As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuTransaction As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToClaim As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToDocumaster As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToSwift As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToEvents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoToTextFiles As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoNotes As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGotoiMarket As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuProcess As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentsLetterWriting As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentRiskRegister As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentsMarket As System.Windows.Forms.ToolStripMenuItem
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
	Public WithEvents uctPartySummControl1 As PartySummControl.uctPartySummControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
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
        Me.mnuClientOpen = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientMove = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientClose = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_0 = New System.Windows.Forms.ToolStripSeparator
        Me._mnuRecentFile_1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeperator = New System.Windows.Forms.ToolStripSeparator
        Me.mnuClientExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuProcess = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToAccounts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoInsuredAccounts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToPolicy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTransaction = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionCash = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransactionFee = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToClaim = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToDocumaster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToSwift = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToEvents = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToTextFiles = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoiMarket = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocuments = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentsLetterWriting = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentRiskRegister = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentsMarket = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsClientSummary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsStatements = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTasks = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewDiary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFind = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.uctPartySummControl1 = New PartySummControl.uctPartySummControl
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClient, Me.mnuProcess, Me.mnuDocuments, Me.mnuReports, Me.mnuTasks, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(617, 24)
        Me.MainMenu1.TabIndex = 5
        Me.MainMenu1.Visible = False
        '
        'mnuClient
        '
        Me.mnuClient.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClientOpen, Me.mnuClientCopy, Me.mnuClientMove, Me.mnuClientClose, Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me.mnuSeperator, Me.mnuClientExit})
        Me.mnuClient.MergeAction = System.Windows.Forms.MergeAction.Replace
        Me.mnuClient.Name = "mnuClient"
        Me.mnuClient.Size = New System.Drawing.Size(46, 20)
        Me.mnuClient.Text = "&Client"
        '
        'mnuClientOpen
        '
        Me.mnuClientOpen.Name = "mnuClientOpen"
        Me.mnuClientOpen.Size = New System.Drawing.Size(130, 22)
        Me.mnuClientOpen.Text = "&Open"
        '
        'mnuClientCopy
        '
        Me.mnuClientCopy.Name = "mnuClientCopy"
        Me.mnuClientCopy.Size = New System.Drawing.Size(130, 22)
        Me.mnuClientCopy.Text = "&Copy"
        '
        'mnuClientMove
        '
        Me.mnuClientMove.Name = "mnuClientMove"
        Me.mnuClientMove.Size = New System.Drawing.Size(130, 22)
        Me.mnuClientMove.Text = "&Move"
        '
        'mnuClientClose
        '
        Me.mnuClientClose.Name = "mnuClientClose"
        Me.mnuClientClose.Size = New System.Drawing.Size(130, 22)
        Me.mnuClientClose.Text = "Clo&se"
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(127, 6)
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
        'mnuProcess
        '
        Me.mnuProcess.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGoToAccounts, Me.mnuGotoInsuredAccounts, Me.mnuGoToPolicy, Me.mnuTransaction, Me.mnuGoToClaim, Me.mnuGoToDocumaster, Me.mnuGoToSwift, Me.mnuGoToEvents, Me.mnuGoToTextFiles, Me.mnuGotoNotes, Me.mnuGotoiMarket})
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
        'mnuGoToPolicy
        '
        Me.mnuGoToPolicy.Name = "mnuGoToPolicy"
        Me.mnuGoToPolicy.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToPolicy.Text = "&Policy"
        '
        'mnuTransaction
        '
        Me.mnuTransaction.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoTransactionCash, Me.mnuGotoTransactionFee})
        Me.mnuTransaction.Name = "mnuTransaction"
        Me.mnuTransaction.Size = New System.Drawing.Size(158, 22)
        Me.mnuTransaction.Text = "&Transaction"
        '
        'mnuGotoTransactionCash
        '
        Me.mnuGotoTransactionCash.Name = "mnuGotoTransactionCash"
        Me.mnuGotoTransactionCash.Size = New System.Drawing.Size(98, 22)
        Me.mnuGotoTransactionCash.Text = "&Cash"
        '
        'mnuGotoTransactionFee
        '
        Me.mnuGotoTransactionFee.Name = "mnuGotoTransactionFee"
        Me.mnuGotoTransactionFee.Size = New System.Drawing.Size(98, 22)
        Me.mnuGotoTransactionFee.Text = "&Fee"
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
        'mnuGotoiMarket
        '
        Me.mnuGotoiMarket.Name = "mnuGotoiMarket"
        Me.mnuGotoiMarket.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoiMarket.Text = "iMar&ket"
        '
        'mnuDocuments
        '
        Me.mnuDocuments.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDocumentsLetterWriting, Me.mnuDocumentRiskRegister, Me.mnuDocumentsMarket})
        Me.mnuDocuments.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuDocuments.MergeIndex = 2
        Me.mnuDocuments.Name = "mnuDocuments"
        Me.mnuDocuments.Size = New System.Drawing.Size(91, 20)
        Me.mnuDocuments.Text = "&Documentation"
        '
        'mnuDocumentsLetterWriting
        '
        Me.mnuDocumentsLetterWriting.Name = "mnuDocumentsLetterWriting"
        Me.mnuDocumentsLetterWriting.Size = New System.Drawing.Size(171, 22)
        Me.mnuDocumentsLetterWriting.Text = "&Letter Writing"
        '
        'mnuDocumentRiskRegister
        '
        Me.mnuDocumentRiskRegister.Name = "mnuDocumentRiskRegister"
        Me.mnuDocumentRiskRegister.Size = New System.Drawing.Size(171, 22)
        Me.mnuDocumentRiskRegister.Text = "&Risk Register"
        '
        'mnuDocumentsMarket
        '
        Me.mnuDocumentsMarket.Name = "mnuDocumentsMarket"
        Me.mnuDocumentsMarket.Size = New System.Drawing.Size(171, 22)
        Me.mnuDocumentsMarket.Text = "&Market Presentation"
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
        'uctPartySummControl1
        '
        Me.uctPartySummControl1.BackStyle = 0
        Me.uctPartySummControl1.BorderStyle = 0
        Me.uctPartySummControl1.Enabled = False
        Me.uctPartySummControl1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartySummControl1.Location = New System.Drawing.Point(2, 6)
        Me.uctPartySummControl1.Name = "uctPartySummControl1"
        Me.uctPartySummControl1.PartyCnt = 0
        Me.uctPartySummControl1.Size = New System.Drawing.Size(607, 384)
        Me.uctPartySummControl1.Status = 0
        Me.uctPartySummControl1.TabIndex = 4
        Me.uctPartySummControl1.Task = 0
        Me.uctPartySummControl1.UnderwritingOrAgency = ""
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(376, 412)
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
        Me.cmdCancel.Location = New System.Drawing.Point(456, 412)
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
        Me.cmdHelp.Location = New System.Drawing.Point(536, 412)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 1
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(8, 412)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 0
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'frmPartySummary
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 447)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.uctPartySummControl1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 42)
        Me.MaximizeBox = False
        Me.Name = "frmPartySummary"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Client Summary"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializemnuRecentFile()
        'Me.mnuRecentFile(0) = _mnuRecentFile_0
		Me.mnuRecentFile(1) = _mnuRecentFile_1
	End Sub
#End Region 
End Class