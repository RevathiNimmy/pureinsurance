<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPartyCCView
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
        ' Code synched in accordance with iPMBPartyPC
        'iPMBClientManager.frmMDI.Show()
        'Me.MdiParent = m_ofrmMDI
        Me.MdiParent = parentForm
        m_parentMdiForm = parentForm
        Form_Initialize_Renamed()
        'The MDI form in the VB6 project had its
        'AutoShowChildren property set to True
        'To simulate the VB6 behavior, we need to
        'automatically Show the form whenever it
        'is loaded.  If you do not want this behavior
        'then delete the following line of code

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
    Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_2 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_3 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_4 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_5 As System.Windows.Forms.ToolStripMenuItem
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
    Public WithEvents mnuGoToSharePoint As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToFinancePlan As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToStickyNotes As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToSwift As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToEvents As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToTextFiles As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGotoNotes As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGotoiMarket As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGotoBankGuarantee As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGotoCashDeposit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuProcess As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDocumentsLetterWriting As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDocumentRiskRegister As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDocumentsMarket As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDocuments As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReportsClientSummary As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReportsStatement As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReportsPolicyList As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReportsStatements As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuNewDiary As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuFind As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTasks As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip

    Public WithEvents picIndicator As System.Windows.Forms.PictureBox
	Public WithEvents cmdCustom As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents imgCustomData As System.Windows.Forms.ImageList
	Public mnuRecentFile(5) As System.Windows.Forms.ToolStripMenuItem
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPartyCCView))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuClient = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientOpen = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_0 = New System.Windows.Forms.ToolStripSeparator
        Me._mnuRecentFile_1 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_2 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_3 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_4 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_5 = New System.Windows.Forms.ToolStripMenuItem
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
        Me.mnuGoToSharePoint = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToFinancePlan = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToStickyNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToSwift = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToEvents = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToTextFiles = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoiMarket = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoBankGuarantee = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoCashDeposit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocuments = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentsLetterWriting = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentRiskRegister = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentsMarket = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsClientSummary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsStatement = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsPolicyList = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsStatements = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTasks = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewDiary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFind = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.picIndicator = New System.Windows.Forms.PictureBox
        Me.cmdCustom = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.imgCustomData = New System.Windows.Forms.ImageList(Me.components)
        Me.uctPartyCCControl1 = New PartyCCControl.uctPartyCCControl
        Me.cmdExtractClientData = New System.Windows.Forms.Button()
        Me.MainMenu1.SuspendLayout()
        CType(Me.picIndicator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClient, Me.mnuProcess, Me.mnuDocuments, Me.mnuReports, Me.mnuTasks, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(718, 24)
        Me.MainMenu1.TabIndex = 7
        Me.MainMenu1.Visible = False
        '
        'mnuClient
        '
        Me.mnuClient.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClientOpen, Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me._mnuRecentFile_2, Me._mnuRecentFile_3, Me._mnuRecentFile_4, Me._mnuRecentFile_5, Me.mnuSeperator, Me.mnuClientExit})
        Me.mnuClient.MergeAction = System.Windows.Forms.MergeAction.Replace
        Me.mnuClient.Name = "mnuClient"
        Me.mnuClient.Size = New System.Drawing.Size(46, 20)
        Me.mnuClient.Text = "&Client"
        '
        'mnuClientOpen
        '
        Me.mnuClientOpen.Name = "mnuClientOpen"
        Me.mnuClientOpen.Size = New System.Drawing.Size(152, 22)
        Me.mnuClientOpen.Text = "&Open"
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(149, 6)
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
        'mnuClientExit
        '
        Me.mnuClientExit.Name = "mnuClientExit"
        Me.mnuClientExit.Size = New System.Drawing.Size(152, 22)
        Me.mnuClientExit.Text = "E&xit"
        '
        'mnuProcess
        '
        Me.mnuProcess.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGoToAccounts, Me.mnuGotoInsuredAccounts, Me.mnuGoToPolicy, Me.mnuTransaction, Me.mnuGoToClaim, Me.mnuGoToDocumaster, Me.mnuGoToSharePoint, Me.mnuGoToFinancePlan, Me.mnuGoToStickyNotes, Me.mnuGoToSwift, Me.mnuGoToEvents, Me.mnuGoToTextFiles, Me.mnuGotoNotes, Me.mnuGotoiMarket, Me.mnuGotoBankGuarantee, Me.mnuGotoCashDeposit})
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
        Me.mnuGotoTransactionCash.Size = New System.Drawing.Size(152, 22)
        Me.mnuGotoTransactionCash.Text = "&Cash"
        '
        'mnuGotoTransactionFee
        '
        Me.mnuGotoTransactionFee.Name = "mnuGotoTransactionFee"
        Me.mnuGotoTransactionFee.Size = New System.Drawing.Size(152, 22)
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
        'mnuGoToSharePoint
        '
        Me.mnuGoToSharePoint.Name = "mnuGoToSharePoint"
        Me.mnuGoToSharePoint.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToSharePoint.Text = "&SharePoint"
        Me.mnuGoToSharePoint.Visible = False
        '
        'mnuGoToFinancePlan
        '
        Me.mnuGoToFinancePlan.Name = "mnuGoToFinancePlan"
        Me.mnuGoToFinancePlan.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToFinancePlan.Text = "F&inancePlan"
        '
        'mnuGoToStickyNotes
        '
        Me.mnuGoToStickyNotes.Name = "mnuGoToStickyNotes"
        Me.mnuGoToStickyNotes.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToStickyNotes.Text = "S&ticky Notes"
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
        'mnuGotoBankGuarantee
        '
        Me.mnuGotoBankGuarantee.Name = "mnuGotoBankGuarantee"
        Me.mnuGotoBankGuarantee.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoBankGuarantee.Text = "Bank Guarantee"
        '
        'mnuGotoCashDeposit
        '
        Me.mnuGotoCashDeposit.Name = "mnuGotoCashDeposit"
        Me.mnuGotoCashDeposit.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoCashDeposit.Text = "Cas&h Deposit"
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
        Me.mnuReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuReportsClientSummary, Me.mnuReportsStatement, Me.mnuReportsPolicyList, Me.mnuReportsStatements})
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
        'mnuReportsStatement
        '
        Me.mnuReportsStatement.Name = "mnuReportsStatement"
        Me.mnuReportsStatement.Size = New System.Drawing.Size(148, 22)
        Me.mnuReportsStatement.Text = "&Statement"
        '
        'mnuReportsPolicyList
        '
        Me.mnuReportsPolicyList.Name = "mnuReportsPolicyList"
        Me.mnuReportsPolicyList.Size = New System.Drawing.Size(148, 22)
        Me.mnuReportsPolicyList.Text = "&Policy List"
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
        'picIndicator
        '
        Me.picIndicator.BackColor = System.Drawing.SystemColors.Control
        Me.picIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.picIndicator.Enabled = False
        Me.picIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picIndicator.Location = New System.Drawing.Point(200, 480)
        Me.picIndicator.Name = "picIndicator"
        Me.picIndicator.Size = New System.Drawing.Size(25, 22)
        Me.picIndicator.TabIndex = 5
        Me.picIndicator.TabStop = False
        '
        'cmdCustom
        '
        Me.cmdCustom.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCustom.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCustom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCustom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCustom.Location = New System.Drawing.Point(88, 480)
        Me.cmdCustom.Name = "cmdCustom"
        Me.cmdCustom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCustom.Size = New System.Drawing.Size(105, 22)
        Me.cmdCustom.TabIndex = 4
        Me.cmdCustom.Text = "C&ustom Data ..."
        Me.cmdCustom.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCustom.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(482, 480)
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
        Me.cmdCancel.Location = New System.Drawing.Point(562, 480)
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
        Me.cmdHelp.Location = New System.Drawing.Point(642, 480)
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
        Me.cmdEdit.Location = New System.Drawing.Point(8, 480)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 0
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'imgCustomData
        '
        Me.imgCustomData.ImageStream = CType(resources.GetObject("imgCustomData.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgCustomData.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imgCustomData.Images.SetKeyName(0, "TICK")
        Me.imgCustomData.Images.SetKeyName(1, "CROSS")
        '
        'UctPartyCCControl1
        '
        Me.uctPartyCCControl1.AddressLine1 = ""
        Me.uctPartyCCControl1.CurrentResolvedName = ""
        Me.uctPartyCCControl1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyCCControl1.FromEvent = False
        Me.uctPartyCCControl1.IsIncludeClosedBranchChecked = False
        Me.uctPartyCCControl1.Location = New System.Drawing.Point(3, 3)
        Me.uctPartyCCControl1.LongName = ""
        Me.uctPartyCCControl1.MainPostCode = ""
        Me.uctPartyCCControl1.Name = "UctPartyCCControl1"
        Me.uctPartyCCControl1.OrgName = ""
        Me.uctPartyCCControl1.PartyCnt = 0
        Me.uctPartyCCControl1.PartySourceID = 0
        Me.uctPartyCCControl1.ShortName = ""
        Me.uctPartyCCControl1.Size = New System.Drawing.Size(712, 456)
        Me.uctPartyCCControl1.Status = 0
        Me.uctPartyCCControl1.SwiftPartyID = 0
        Me.uctPartyCCControl1.TabIndex = 8
        Me.uctPartyCCControl1.Task = 0
        '
        'cmdExtractClientData
        '
        Me.cmdExtractClientData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExtractClientData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExtractClientData.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExtractClientData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExtractClientData.Location = New System.Drawing.Point(200, 480)
        Me.cmdExtractClientData.Name = "cmdExtractClientData"
        Me.cmdExtractClientData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExtractClientData.Size = New System.Drawing.Size(124, 22)
        Me.cmdExtractClientData.TabIndex = 9
        Me.cmdExtractClientData.Text = "Extract Client Data"
        Me.cmdExtractClientData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExtractClientData.UseVisualStyleBackColor = False
        '
        'frmPartyCCView
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(718, 507)
        Me.Controls.Add(Me.cmdExtractClientData)
        Me.Controls.Add(Me.uctPartyCCControl1)
        Me.Controls.Add(Me.picIndicator)
        Me.Controls.Add(Me.cmdCustom)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 42)
        Me.MaximizeBox = False
        Me.Name = "frmPartyCCView"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Corporate Client View"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        CType(Me.picIndicator, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializemnuRecentFile()
        'Me.mnuRecentFile(0) = _mnuRecentFile_0
        Me.mnuRecentFile(1) = _mnuRecentFile_1
        Me.mnuRecentFile(2) = _mnuRecentFile_2
        Me.mnuRecentFile(3) = _mnuRecentFile_3
        Me.mnuRecentFile(4) = _mnuRecentFile_4
        Me.mnuRecentFile(5) = _mnuRecentFile_5
    End Sub
    Friend WithEvents uctPartyCCControl1 As PartyCCControl.uctPartyCCControl
    Public WithEvents cmdExtractClientData As System.Windows.Forms.Button
#End Region
End Class