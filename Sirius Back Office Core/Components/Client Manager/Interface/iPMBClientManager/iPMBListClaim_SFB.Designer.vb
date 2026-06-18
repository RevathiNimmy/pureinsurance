<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListClaim_SFB
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

        Me.Show()
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
	Public WithEvents mnuClientNew As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuClientDelete As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuClientCopy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuClientMove As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuClientClose As System.Windows.Forms.ToolStripMenuItem
    'Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_2 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_3 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_4 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuRecentFile_5 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuSeperator As System.Windows.Forms.ToolStripSeparator
	Public WithEvents mnuClientExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuClaim As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentationLetterWriting As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuDocumentation As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents uctListClaim1 As uctListClaimControl.uctListClaim
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public mnuRecentFile(5) As System.Windows.Forms.ToolStripMenuItem
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuClient = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClaim = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientNew = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientMove = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientClose = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_0 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_1 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_2 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_3 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_4 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_5 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeperator = New System.Windows.Forms.ToolStripSeparator
        Me.mnuClientExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentation = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuDocumentationLetterWriting = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.uctListClaim1 = New uctListClaimControl.uctListClaim
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClient, Me.mnuClaim, Me.mnuDocumentation, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(595, 24)
        Me.MainMenu1.TabIndex = 4
        Me.MainMenu1.Visible = False
        '
        'mnuClient
        '
        Me.mnuClient.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuClient.Name = "mnuClient"
        Me.mnuClient.Size = New System.Drawing.Size(51, 20)
        Me.mnuClient.Text = "&Client"
        Me.mnuClient.Visible = False
        '
        'mnuClaim
        '
        Me.mnuClaim.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClientNew, Me.mnuClientDelete, Me.mnuClientCopy, Me.mnuClientMove, Me.mnuClientClose, Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me._mnuRecentFile_2, Me._mnuRecentFile_3, Me._mnuRecentFile_4, Me._mnuRecentFile_5, Me.mnuSeperator, Me.mnuClientExit})
        Me.mnuClaim.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuClaim.MergeIndex = 0
        Me.mnuClaim.Name = "mnuClaim"
        Me.mnuClaim.Size = New System.Drawing.Size(44, 20)
        Me.mnuClaim.Text = "&Claim"
        '
        'mnuClientNew
        '
        Me.mnuClientNew.Name = "mnuClientNew"
        Me.mnuClientNew.Size = New System.Drawing.Size(112, 22)
        Me.mnuClientNew.Text = "&New"
        '
        'mnuClientDelete
        '
        Me.mnuClientDelete.Name = "mnuClientDelete"
        Me.mnuClientDelete.Size = New System.Drawing.Size(112, 22)
        Me.mnuClientDelete.Text = "&Delete"
        '
        'mnuClientCopy
        '
        Me.mnuClientCopy.Name = "mnuClientCopy"
        Me.mnuClientCopy.Size = New System.Drawing.Size(112, 22)
        Me.mnuClientCopy.Text = "&Copy..."
        '
        'mnuClientMove
        '
        Me.mnuClientMove.Name = "mnuClientMove"
        Me.mnuClientMove.Size = New System.Drawing.Size(112, 22)
        Me.mnuClientMove.Text = "&Move..."
        '
        'mnuClientClose
        '
        Me.mnuClientClose.Name = "mnuClientClose"
        Me.mnuClientClose.Size = New System.Drawing.Size(112, 22)
        Me.mnuClientClose.Text = "Clo&se"
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(112, 22)
        Me._mnuRecentFile_0.Visible = False
        '
        '_mnuRecentFile_1
        '
        Me._mnuRecentFile_1.Name = "_mnuRecentFile_1"
        Me._mnuRecentFile_1.Size = New System.Drawing.Size(112, 22)
        Me._mnuRecentFile_1.Visible = False
        '
        '_mnuRecentFile_2
        '
        Me._mnuRecentFile_2.Name = "_mnuRecentFile_2"
        Me._mnuRecentFile_2.Size = New System.Drawing.Size(112, 22)
        Me._mnuRecentFile_2.Visible = False
        '
        '_mnuRecentFile_3
        '
        Me._mnuRecentFile_3.Name = "_mnuRecentFile_3"
        Me._mnuRecentFile_3.Size = New System.Drawing.Size(112, 22)
        Me._mnuRecentFile_3.Visible = False
        '
        '_mnuRecentFile_4
        '
        Me._mnuRecentFile_4.Name = "_mnuRecentFile_4"
        Me._mnuRecentFile_4.Size = New System.Drawing.Size(112, 22)
        Me._mnuRecentFile_4.Visible = False
        '
        '_mnuRecentFile_5
        '
        Me._mnuRecentFile_5.Name = "_mnuRecentFile_5"
        Me._mnuRecentFile_5.Size = New System.Drawing.Size(112, 22)
        Me._mnuRecentFile_5.Visible = False
        '
        'mnuSeperator
        '
        Me.mnuSeperator.Name = "mnuSeperator"
        Me.mnuSeperator.Size = New System.Drawing.Size(109, 6)
        '
        'mnuClientExit
        '
        Me.mnuClientExit.Name = "mnuClientExit"
        Me.mnuClientExit.Size = New System.Drawing.Size(112, 22)
        Me.mnuClientExit.Text = "E&xit"
        '
        'mnuDocumentation
        '
        Me.mnuDocumentation.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDocumentationLetterWriting})
        Me.mnuDocumentation.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuDocumentation.MergeIndex = 1
        Me.mnuDocumentation.Name = "mnuDocumentation"
        Me.mnuDocumentation.Size = New System.Drawing.Size(91, 20)
        Me.mnuDocumentation.Text = "Documentation"
        '
        'mnuDocumentationLetterWriting
        '
        Me.mnuDocumentationLetterWriting.Name = "mnuDocumentationLetterWriting"
        Me.mnuDocumentationLetterWriting.Size = New System.Drawing.Size(140, 22)
        Me.mnuDocumentationLetterWriting.Text = "Letter Writing"
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
        'uctListClaim1
        '
        Me.uctListClaim1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctListClaim1.InsReference = ""
        Me.uctListClaim1.Location = New System.Drawing.Point(8, 9)
        Me.uctListClaim1.Name = "uctListClaim1"
        Me.uctListClaim1.ShortName = ""
        Me.uctListClaim1.Size = New System.Drawing.Size(577, 329)
        Me.uctListClaim1.Status = 0
        Me.uctListClaim1.TabIndex = 3
        Me.uctListClaim1.Task = 0
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(352, 360)
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
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(432, 360)
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
        Me.cmdHelp.Location = New System.Drawing.Point(512, 360)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 0
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'frmListClaim_SFB
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(595, 387)
        Me.Controls.Add(Me.uctListClaim1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 42)
        Me.Name = "frmListClaim_SFB"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Claim List"
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