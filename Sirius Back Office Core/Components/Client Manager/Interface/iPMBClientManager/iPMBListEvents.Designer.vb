<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListEvents
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
    'Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_2 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_3 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_4 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_5 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuClientExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuEvents As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuNotes As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuGoto As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents cmdNotes As System.Windows.Forms.Button
	Public WithEvents cmdNotesView As System.Windows.Forms.Button
	Public WithEvents cmdNotesAdd As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents uctListEvents1 As uctListEventsControl.uctListEvents
	Public mnuRecentFile(5) As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuCLient = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEvents = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me._mnuRecentFile_0 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_1 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_2 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_3 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_4 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_5 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeperator = New System.Windows.Forms.ToolStripSeparator
        Me.mnuClientExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuGoto = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNotes = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdNotes = New System.Windows.Forms.Button
        Me.cmdNotesView = New System.Windows.Forms.Button
        Me.cmdNotesAdd = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.uctListEvents1 = New uctListEventsControl.uctListEvents
        Me.commandButtonHelper1 = New Artinsoft.VB6.Gui.CommandButtonHelper(Me.components)
        Me.MainMenu1.SuspendLayout()
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCLient, Me.mnuEvents, Me.mnuGoto, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(597, 24)
        Me.MainMenu1.TabIndex = 7
        Me.MainMenu1.Visible = False
        '
        'mnuCLient
        '
        Me.mnuCLient.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuCLient.Name = "mnuCLient"
        Me.mnuCLient.Size = New System.Drawing.Size(46, 20)
        Me.mnuCLient.Text = "&Client"
        Me.mnuCLient.Visible = False
        '
        'mnuEvents
        '
        Me.mnuEvents.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator1, Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me._mnuRecentFile_2, Me._mnuRecentFile_3, Me._mnuRecentFile_4, Me._mnuRecentFile_5, Me.mnuSeperator, Me.mnuClientExit})
        Me.mnuEvents.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuEvents.MergeIndex = 0
        Me.mnuEvents.Name = "mnuEvents"
        Me.mnuEvents.Size = New System.Drawing.Size(52, 20)
        Me.mnuEvents.Text = "&Events"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(89, 6)
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(92, 22)
        Me._mnuRecentFile_0.Visible = False
        '
        '_mnuRecentFile_1
        '
        Me._mnuRecentFile_1.Name = "_mnuRecentFile_1"
        Me._mnuRecentFile_1.Size = New System.Drawing.Size(92, 22)
        Me._mnuRecentFile_1.Visible = False
        '
        '_mnuRecentFile_2
        '
        Me._mnuRecentFile_2.Name = "_mnuRecentFile_2"
        Me._mnuRecentFile_2.Size = New System.Drawing.Size(92, 22)
        Me._mnuRecentFile_2.Visible = False
        '
        '_mnuRecentFile_3
        '
        Me._mnuRecentFile_3.Name = "_mnuRecentFile_3"
        Me._mnuRecentFile_3.Size = New System.Drawing.Size(92, 22)
        Me._mnuRecentFile_3.Visible = False
        '
        '_mnuRecentFile_4
        '
        Me._mnuRecentFile_4.Name = "_mnuRecentFile_4"
        Me._mnuRecentFile_4.Size = New System.Drawing.Size(92, 22)
        Me._mnuRecentFile_4.Visible = False
        '
        '_mnuRecentFile_5
        '
        Me._mnuRecentFile_5.Name = "_mnuRecentFile_5"
        Me._mnuRecentFile_5.Size = New System.Drawing.Size(92, 22)
        Me._mnuRecentFile_5.Visible = False
        '
        'mnuSeperator
        '
        Me.mnuSeperator.Name = "mnuSeperator"
        Me.mnuSeperator.Size = New System.Drawing.Size(89, 6)
        '
        'mnuClientExit
        '
        Me.mnuClientExit.Name = "mnuClientExit"
        Me.mnuClientExit.Size = New System.Drawing.Size(92, 22)
        Me.mnuClientExit.Text = "E&xit"
        '
        'mnuGoto
        '
        Me.mnuGoto.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNotes})
        Me.mnuGoto.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuGoto.MergeIndex = 1
        Me.mnuGoto.Name = "mnuGoto"
        Me.mnuGoto.Size = New System.Drawing.Size(44, 20)
        Me.mnuGoto.Text = "&GoTo"
        '
        'mnuNotes
        '
        Me.mnuNotes.Name = "mnuNotes"
        Me.mnuNotes.Size = New System.Drawing.Size(102, 22)
        Me.mnuNotes.Text = "Notes"
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
        'cmdNotes
        '
        Me.cmdNotes.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdNotes, True)
        Me.cmdNotes.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdNotes, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdNotes, Nothing)
        Me.cmdNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNotes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNotes.Location = New System.Drawing.Point(168, 360)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdNotes, System.Drawing.Color.Silver)
        Me.cmdNotes.Name = "cmdNotes"
        Me.cmdNotes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNotes.Size = New System.Drawing.Size(76, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdNotes, 0)
        Me.cmdNotes.TabIndex = 5
        Me.cmdNotes.Text = "Event Notes"
        Me.cmdNotes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNotes.UseVisualStyleBackColor = False
        '
        'cmdNotesView
        '
        Me.cmdNotesView.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdNotesView, True)
        Me.cmdNotesView.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdNotesView, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdNotesView, Nothing)
        Me.cmdNotesView.Enabled = False
        Me.cmdNotesView.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNotesView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNotesView.Location = New System.Drawing.Point(8, 360)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdNotesView, System.Drawing.Color.Silver)
        Me.cmdNotesView.Name = "cmdNotesView"
        Me.cmdNotesView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNotesView.Size = New System.Drawing.Size(76, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdNotesView, 1)
        Me.cmdNotesView.TabIndex = 4
        Me.cmdNotesView.Text = "&View"
        Me.cmdNotesView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNotesView.UseVisualStyleBackColor = False
        '
        'cmdNotesAdd
        '
        Me.cmdNotesAdd.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdNotesAdd, True)
        Me.cmdNotesAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdNotesAdd, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdNotesAdd, Nothing)
        Me.cmdNotesAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNotesAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNotesAdd.Location = New System.Drawing.Point(88, 360)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdNotesAdd, System.Drawing.Color.Silver)
        Me.cmdNotesAdd.Name = "cmdNotesAdd"
        Me.cmdNotesAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNotesAdd.Size = New System.Drawing.Size(76, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdNotesAdd, 1)
        Me.cmdNotesAdd.TabIndex = 3
        Me.cmdNotesAdd.Text = "&Add"
        Me.cmdNotesAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNotesAdd.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdOK, True)
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdOK, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdOK, Nothing)
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(360, 360)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdOK, System.Drawing.Color.Silver)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdOK, 0)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdCancel, True)
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdCancel, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdCancel, Nothing)
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(440, 360)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdCancel, System.Drawing.Color.Silver)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdCancel, 0)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdHelp, True)
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdHelp, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdHelp, Nothing)
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(520, 360)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdHelp, System.Drawing.Color.Silver)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdHelp, 0)
        Me.cmdHelp.TabIndex = 0
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'uctListEvents1
        '
        Me.uctListEvents1.BaseCaseID = 0
        Me.uctListEvents1.BaseClaimId = 0
        Me.uctListEvents1.CaseID = 0
        Me.uctListEvents1.CaseNumber = ""
        Me.uctListEvents1.ClaimCnt = 0
        Me.uctListEvents1.ClaimDesc = ""
        Me.uctListEvents1.DocumentCnt = 0
        Me.uctListEvents1.EnableDefaultedFields = False
        Me.uctListEvents1.EventCnt = 0
        Me.uctListEvents1.EventType = ""
        Me.uctListEvents1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctListEvents1.FSAComplaintFolderCnt = 0
        Me.uctListEvents1.InsuranceFileCnt = 0
        Me.uctListEvents1.InsuranceFileStructureId = 0
        Me.uctListEvents1.InsuranceFolderCnt = 0
        Me.uctListEvents1.Location = New System.Drawing.Point(8, 6)
        Me.uctListEvents1.Name = "uctListEvents1"
        Me.uctListEvents1.NewAddressCnt = 0
        Me.uctListEvents1.OldAddressCnt = 0
        Me.uctListEvents1.OldPartyTypeID = 0
        Me.uctListEvents1.PartyCnt = 0
        Me.uctListEvents1.PolicyDesc = ""
        Me.uctListEvents1.RTFNotes = True
        Me.uctListEvents1.ShortName = ""
        Me.uctListEvents1.ShowNonNotes = False
        Me.uctListEvents1.ShowNotes = False
        Me.uctListEvents1.Size = New System.Drawing.Size(585, 337)
        Me.uctListEvents1.Status = 0
        Me.uctListEvents1.TabIndex = 6
        Me.uctListEvents1.Task = 0
        '
        'frmListEvents
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(597, 386)
        Me.Controls.Add(Me.cmdNotes)
        Me.Controls.Add(Me.cmdNotesView)
        Me.Controls.Add(Me.cmdNotesAdd)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.uctListEvents1)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 42)
        Me.MainMenuStrip = Me.MainMenu1
        Me.Name = "frmListEvents"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Events List"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).EndInit()
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
    Public WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuCLient As System.Windows.Forms.ToolStripMenuItem
#End Region 
End Class