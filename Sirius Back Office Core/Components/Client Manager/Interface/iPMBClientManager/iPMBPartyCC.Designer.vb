<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPartyCC
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
    Public WithEvents mnuGotoAccounts As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGotoInsuredAccounts As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToPolicy As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGotoTransactionCash As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGotoTransactionFee As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGotoTransaction As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToClaim As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToDocumaster As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToSharePoint As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToFinancePlan As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToStickyNotes As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToSwift As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToEvents As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToTextFiles As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoToNotes As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGotoiMarket As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuGoTo As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDocumentationLetterWriting As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDocumentationRiskRegister As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDocumentationMarkPresentation As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDocumentation As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReportsClientSummary As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReportsStatement As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReportsPolicyList As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReportsStatements As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuReports As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDiaryNew As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuDiaryFind As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTasks As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents uctPartyCCControl1 As PartyCCControl.uctPartyCCControl
	Public WithEvents picIndicator As System.Windows.Forms.PictureBox
	Public WithEvents cmdCustom As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents imgCustomData As System.Windows.Forms.ImageList
	Public mnuRecentFile(5) As System.Windows.Forms.ToolStripMenuItem
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPartyCC))
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
        Me.mnuGoTo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoAccounts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoInsuredAccounts = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoToPolicy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoTransaction = New System.Windows.Forms.ToolStripMenuItem
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
        Me.mnuGoToNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGotoiMarket = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentation = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentationLetterWriting = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentationRiskRegister = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentationMarkPresentation = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsClientSummary = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsStatement = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsPolicyList = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuReportsStatements = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTasks = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDiaryNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDiaryFind = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.uctPartyCCControl1 = New PartyCCControl.uctPartyCCControl
        Me.picIndicator = New System.Windows.Forms.PictureBox
        Me.cmdCustom = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.imgCustomData = New System.Windows.Forms.ImageList(Me.components)
        Me.MainMenu1.SuspendLayout()
        CType(Me.picIndicator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClient, Me.mnuGoTo, Me.mnuDocumentation, Me.mnuReports, Me.mnuTasks, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(718, 24)
        Me.MainMenu1.TabIndex = 6
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
        Me.mnuClientOpen.Size = New System.Drawing.Size(100, 22)
        Me.mnuClientOpen.Text = "&Open"
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(97, 6)
        Me._mnuRecentFile_0.Visible = False
        '
        '_mnuRecentFile_1
        '
        Me._mnuRecentFile_1.Name = "_mnuRecentFile_1"
        Me._mnuRecentFile_1.Size = New System.Drawing.Size(100, 22)
        Me._mnuRecentFile_1.Visible = False
        '
        '_mnuRecentFile_2
        '
        Me._mnuRecentFile_2.Name = "_mnuRecentFile_2"
        Me._mnuRecentFile_2.Size = New System.Drawing.Size(100, 22)
        Me._mnuRecentFile_2.Visible = False
        '
        '_mnuRecentFile_3
        '
        Me._mnuRecentFile_3.Name = "_mnuRecentFile_3"
        Me._mnuRecentFile_3.Size = New System.Drawing.Size(100, 22)
        Me._mnuRecentFile_3.Visible = False
        '
        '_mnuRecentFile_4
        '
        Me._mnuRecentFile_4.Name = "_mnuRecentFile_4"
        Me._mnuRecentFile_4.Size = New System.Drawing.Size(100, 22)
        Me._mnuRecentFile_4.Visible = False
        '
        '_mnuRecentFile_5
        '
        Me._mnuRecentFile_5.Name = "_mnuRecentFile_5"
        Me._mnuRecentFile_5.Size = New System.Drawing.Size(100, 22)
        Me._mnuRecentFile_5.Visible = False
        '
        'mnuSeperator
        '
        Me.mnuSeperator.Name = "mnuSeperator"
        Me.mnuSeperator.Size = New System.Drawing.Size(97, 6)
        '
        'mnuClientExit
        '
        Me.mnuClientExit.Name = "mnuClientExit"
        Me.mnuClientExit.Size = New System.Drawing.Size(100, 22)
        Me.mnuClientExit.Text = "E&xit"
        '
        'mnuGoTo
        '
        Me.mnuGoTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoAccounts, Me.mnuGotoInsuredAccounts, Me.mnuGoToPolicy, Me.mnuGotoTransaction, Me.mnuGoToClaim, Me.mnuGoToDocumaster, Me.mnuGoToSharePoint, Me.mnuGoToFinancePlan, Me.mnuGoToStickyNotes, Me.mnuGoToSwift, Me.mnuGoToEvents, Me.mnuGoToTextFiles, Me.mnuGoToNotes, Me.mnuGotoiMarket})
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
        'mnuGoToPolicy
        '
        Me.mnuGoToPolicy.Name = "mnuGoToPolicy"
        Me.mnuGoToPolicy.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToPolicy.Text = "&Policy"
        '
        'mnuGotoTransaction
        '
        Me.mnuGotoTransaction.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGotoTransactionCash, Me.mnuGotoTransactionFee})
        Me.mnuGotoTransaction.Name = "mnuGotoTransaction"
        Me.mnuGotoTransaction.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoTransaction.Text = "&Transaction"
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
        Me.mnuGoToFinancePlan.Text = "F&inance Plan"
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
        'mnuGoToNotes
        '
        Me.mnuGoToNotes.Name = "mnuGoToNotes"
        Me.mnuGoToNotes.Size = New System.Drawing.Size(158, 22)
        Me.mnuGoToNotes.Text = "&Notes"
        '
        'mnuGotoiMarket
        '
        Me.mnuGotoiMarket.Name = "mnuGotoiMarket"
        Me.mnuGotoiMarket.Size = New System.Drawing.Size(158, 22)
        Me.mnuGotoiMarket.Text = "iMar&ket"
        '
        'mnuDocumentation
        '
        Me.mnuDocumentation.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDocumentationLetterWriting, Me.mnuDocumentationRiskRegister, Me.mnuDocumentationMarkPresentation})
        Me.mnuDocumentation.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuDocumentation.MergeIndex = 2
        Me.mnuDocumentation.Name = "mnuDocumentation"
        Me.mnuDocumentation.Size = New System.Drawing.Size(91, 20)
        Me.mnuDocumentation.Text = "&Documentation"
        '
        'mnuDocumentationLetterWriting
        '
        Me.mnuDocumentationLetterWriting.Name = "mnuDocumentationLetterWriting"
        Me.mnuDocumentationLetterWriting.Size = New System.Drawing.Size(171, 22)
        Me.mnuDocumentationLetterWriting.Text = "&Letter Writing"
        '
        'mnuDocumentationRiskRegister
        '
        Me.mnuDocumentationRiskRegister.Name = "mnuDocumentationRiskRegister"
        Me.mnuDocumentationRiskRegister.Size = New System.Drawing.Size(171, 22)
        Me.mnuDocumentationRiskRegister.Text = "&Risk Register"
        '
        'mnuDocumentationMarkPresentation
        '
        Me.mnuDocumentationMarkPresentation.Name = "mnuDocumentationMarkPresentation"
        Me.mnuDocumentationMarkPresentation.Size = New System.Drawing.Size(171, 22)
        Me.mnuDocumentationMarkPresentation.Text = "&Market Presentation"
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
        'uctPartyCCControl1
        '
        Me.uctPartyCCControl1.AddressLine1 = ""
        Me.uctPartyCCControl1.CurrentResolvedName = ""
        Me.uctPartyCCControl1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPartyCCControl1.FromEvent = False
        Me.uctPartyCCControl1.IsIncludeClosedBranchChecked = False
        Me.uctPartyCCControl1.Location = New System.Drawing.Point(2, 5)
        Me.uctPartyCCControl1.LongName = ""
        Me.uctPartyCCControl1.MainPostCode = ""
        Me.uctPartyCCControl1.Name = "uctPartyCCControl1"
        Me.uctPartyCCControl1.OrgName = ""
        Me.uctPartyCCControl1.PartyCnt = 0
        Me.uctPartyCCControl1.PartySourceID = 0
        Me.uctPartyCCControl1.ShortName = ""
        Me.uctPartyCCControl1.Size = New System.Drawing.Size(711, 448)
        Me.uctPartyCCControl1.Status = 0
        Me.uctPartyCCControl1.SwiftPartyID = 0
        Me.uctPartyCCControl1.TabIndex = 5
        Me.uctPartyCCControl1.Task = 0
        '
        'picIndicator
        '
        Me.picIndicator.BackColor = System.Drawing.SystemColors.Control
        Me.picIndicator.Cursor = System.Windows.Forms.Cursors.Default
        Me.picIndicator.Enabled = False
        Me.picIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picIndicator.Location = New System.Drawing.Point(120, 480)
        Me.picIndicator.Name = "picIndicator"
        Me.picIndicator.Size = New System.Drawing.Size(25, 22)
        Me.picIndicator.TabIndex = 4
        Me.picIndicator.TabStop = False
        '
        'cmdCustom
        '
        Me.cmdCustom.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCustom.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCustom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCustom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCustom.Location = New System.Drawing.Point(8, 480)
        Me.cmdCustom.Name = "cmdCustom"
        Me.cmdCustom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCustom.Size = New System.Drawing.Size(105, 22)
        Me.cmdCustom.TabIndex = 3
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
        Me.cmdOK.Location = New System.Drawing.Point(480, 480)
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
        Me.cmdCancel.Location = New System.Drawing.Point(560, 480)
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
        Me.cmdHelp.Location = New System.Drawing.Point(640, 480)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 0
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'imgCustomData
        '
        Me.imgCustomData.ImageStream = CType(resources.GetObject("imgCustomData.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgCustomData.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imgCustomData.Images.SetKeyName(0, "TICK")
        Me.imgCustomData.Images.SetKeyName(1, "CROSS")
        '
        'frmPartyCC
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(718, 510)
        Me.Controls.Add(Me.uctPartyCCControl1)
        Me.Controls.Add(Me.picIndicator)
        Me.Controls.Add(Me.cmdCustom)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(11, 30)
        Me.MaximizeBox = False
        Me.Name = "frmPartyCC"
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
#End Region 
End Class