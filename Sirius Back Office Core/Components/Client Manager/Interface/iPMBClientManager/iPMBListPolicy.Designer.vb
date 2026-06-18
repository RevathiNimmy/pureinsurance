<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListPolicy
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
    Public WithEvents mnuPolicyAdd As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuClientDelete As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPolicyCopy As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuClientMove As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuClientClose As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_0 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_1 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_2 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_3 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_4 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents _mnuRecentFile_5 As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSeperator As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuClientExit As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPolicy As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
    Public WithEvents uctPMUPolicyExplorer1 As uctPMUPolicyExpCtl.uctPMUPolicyExplorer
    Public WithEvents cmdCopy As System.Windows.Forms.Button
    Public WithEvents cmdGII As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
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
        Me.mnuPolicy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPolicyAdd = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientDelete = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPolicyCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientMove = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuClientClose = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me._mnuRecentFile_0 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_1 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_2 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_3 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_4 = New System.Windows.Forms.ToolStripMenuItem
        Me._mnuRecentFile_5 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSeperator = New System.Windows.Forms.ToolStripSeparator
        Me.mnuClientExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.uctPMUPolicyExplorer1 = New uctPMUPolicyExpCtl.uctPMUPolicyExplorer
        Me.cmdCopy = New System.Windows.Forms.Button
        Me.cmdGII = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.uctListPolicy1 = New uctListPolicyControl.uctListPolicy
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuClient, Me.mnuPolicy, Me.mnuWindow, Me.mnuHelp})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindow
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(1157, 24)
        Me.MainMenu1.TabIndex = 8
        Me.MainMenu1.Visible = False
        '
        'mnuClient
        '
        Me.mnuClient.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuClient.Name = "mnuClient"
        Me.mnuClient.Size = New System.Drawing.Size(50, 20)
        Me.mnuClient.Text = "&Client"
        Me.mnuClient.Visible = False
        '
        'mnuPolicy
        '
        Me.mnuPolicy.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPolicyAdd, Me.mnuClientDelete, Me.mnuPolicyCopy, Me.mnuClientMove, Me.mnuClientClose, Me.ToolStripSeparator1, Me._mnuRecentFile_0, Me._mnuRecentFile_1, Me._mnuRecentFile_2, Me._mnuRecentFile_3, Me._mnuRecentFile_4, Me._mnuRecentFile_5, Me.mnuSeperator, Me.mnuClientExit})
        Me.mnuPolicy.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.mnuPolicy.MergeIndex = 0
        Me.mnuPolicy.Name = "mnuPolicy"
        Me.mnuPolicy.Size = New System.Drawing.Size(51, 20)
        Me.mnuPolicy.Text = "&Policy"
        '
        'mnuPolicyAdd
        '
        Me.mnuPolicyAdd.Name = "mnuPolicyAdd"
        Me.mnuPolicyAdd.Size = New System.Drawing.Size(113, 22)
        Me.mnuPolicyAdd.Text = "&Add"
        '
        'mnuClientDelete
        '
        Me.mnuClientDelete.Enabled = False
        Me.mnuClientDelete.Name = "mnuClientDelete"
        Me.mnuClientDelete.Size = New System.Drawing.Size(113, 22)
        Me.mnuClientDelete.Text = "&Delete"
        '
        'mnuPolicyCopy
        '
        Me.mnuPolicyCopy.Name = "mnuPolicyCopy"
        Me.mnuPolicyCopy.Size = New System.Drawing.Size(113, 22)
        Me.mnuPolicyCopy.Text = "&Copy..."
        '
        'mnuClientMove
        '
        Me.mnuClientMove.Enabled = False
        Me.mnuClientMove.Name = "mnuClientMove"
        Me.mnuClientMove.Size = New System.Drawing.Size(113, 22)
        Me.mnuClientMove.Text = "&Move..."
        '
        'mnuClientClose
        '
        Me.mnuClientClose.Name = "mnuClientClose"
        Me.mnuClientClose.Size = New System.Drawing.Size(113, 22)
        Me.mnuClientClose.Text = "Clo&se"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(110, 6)
        '
        '_mnuRecentFile_0
        '
        Me._mnuRecentFile_0.Name = "_mnuRecentFile_0"
        Me._mnuRecentFile_0.Size = New System.Drawing.Size(113, 22)
        Me._mnuRecentFile_0.Visible = False
        '
        '_mnuRecentFile_1
        '
        Me._mnuRecentFile_1.Name = "_mnuRecentFile_1"
        Me._mnuRecentFile_1.Size = New System.Drawing.Size(113, 22)
        Me._mnuRecentFile_1.Visible = False
        '
        '_mnuRecentFile_2
        '
        Me._mnuRecentFile_2.Name = "_mnuRecentFile_2"
        Me._mnuRecentFile_2.Size = New System.Drawing.Size(113, 22)
        '
        '_mnuRecentFile_3
        '
        Me._mnuRecentFile_3.Name = "_mnuRecentFile_3"
        Me._mnuRecentFile_3.Size = New System.Drawing.Size(113, 22)
        Me._mnuRecentFile_3.Visible = False
        '
        '_mnuRecentFile_4
        '
        Me._mnuRecentFile_4.Name = "_mnuRecentFile_4"
        Me._mnuRecentFile_4.Size = New System.Drawing.Size(113, 22)
        Me._mnuRecentFile_4.Visible = False
        '
        '_mnuRecentFile_5
        '
        Me._mnuRecentFile_5.Name = "_mnuRecentFile_5"
        Me._mnuRecentFile_5.Size = New System.Drawing.Size(113, 22)
        Me._mnuRecentFile_5.Visible = False
        '
        'mnuSeperator
        '
        Me.mnuSeperator.Name = "mnuSeperator"
        Me.mnuSeperator.Size = New System.Drawing.Size(110, 6)
        '
        'mnuClientExit
        '
        Me.mnuClientExit.Name = "mnuClientExit"
        Me.mnuClientExit.Size = New System.Drawing.Size(113, 22)
        Me.mnuClientExit.Text = "E&xit"
        '
        'mnuWindow
        '
        Me.mnuWindow.MergeAction = System.Windows.Forms.MergeAction.Replace
        Me.mnuWindow.Name = "mnuWindow"
        Me.mnuWindow.Size = New System.Drawing.Size(68, 20)
        Me.mnuWindow.Text = "&Windows"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpAbout})
        Me.mnuHelp.MergeAction = System.Windows.Forms.MergeAction.Replace
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(107, 22)
        Me.mnuHelpAbout.Text = "&About"
        '
        'uctPMUPolicyExplorer1
        '
        Me.uctPMUPolicyExplorer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uctPMUPolicyExplorer1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMUPolicyExplorer1.InsHolderCnt = 0
        Me.uctPMUPolicyExplorer1.Location = New System.Drawing.Point(8, 4)
        Me.uctPMUPolicyExplorer1.Name = "uctPMUPolicyExplorer1"
        Me.uctPMUPolicyExplorer1.ShortName = ""
        Me.uctPMUPolicyExplorer1.Size = New System.Drawing.Size(1145, 480)
        Me.uctPMUPolicyExplorer1.Status = 0
        Me.uctPMUPolicyExplorer1.TabIndex = 6
        Me.uctPMUPolicyExplorer1.Task = 0
        Me.uctPMUPolicyExplorer1.Visible = False
        '
        'cmdCopy
        '
        Me.cmdCopy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopy.Location = New System.Drawing.Point(168, 490)
        Me.cmdCopy.Name = "cmdCopy"
        Me.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopy.Size = New System.Drawing.Size(73, 22)
        Me.cmdCopy.TabIndex = 5
        Me.cmdCopy.Text = "Copy"
        Me.cmdCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCopy.UseVisualStyleBackColor = False
        '
        'cmdGII
        '
        Me.cmdGII.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGII.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGII.Enabled = False
        Me.cmdGII.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGII.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGII.Location = New System.Drawing.Point(89, 490)
        Me.cmdGII.Name = "cmdGII"
        Me.cmdGII.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGII.Size = New System.Drawing.Size(73, 22)
        Me.cmdGII.TabIndex = 4
        Me.cmdGII.Text = "Quotes"
        Me.cmdGII.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGII.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(10, 490)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 3
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(990, 490)
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
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(1080, 490)
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
        Me.cmdHelp.Location = New System.Drawing.Point(895, 490)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 0
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        Me.cmdHelp.Visible = False
        '
        'uctListPolicy1
        '
        Me.uctListPolicy1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctListPolicy1.InsHolderCnt = 0
        Me.uctListPolicy1.InsReference = ""
        Me.uctListPolicy1.Location = New System.Drawing.Point(8, 7)
        Me.uctListPolicy1.Name = "uctListPolicy1"
        Me.uctListPolicy1.NewInsuranceFileCnt = 0
        Me.uctListPolicy1.ShortName = ""
        Me.uctListPolicy1.Size = New System.Drawing.Size(585, 337)
        Me.uctListPolicy1.Status = 0
        Me.uctListPolicy1.TabIndex = 7
        Me.uctListPolicy1.TargetLongName = ""
        Me.uctListPolicy1.TargetPartyType = ""
        Me.uctListPolicy1.TargetResolvedName = ""
        Me.uctListPolicy1.TargetShortName = ""
        Me.uctListPolicy1.Task = 0
        '
        'frmListPolicy
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1157, 520)
        Me.Controls.Add(Me.uctListPolicy1)
        Me.Controls.Add(Me.uctPMUPolicyExplorer1)
        Me.Controls.Add(Me.cmdCopy)
        Me.Controls.Add(Me.cmdGII)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmListPolicy"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Policy List"
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
    Public WithEvents uctListPolicy1 As uctListPolicyControl.uctListPolicy
    Public WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuClient As System.Windows.Forms.ToolStripMenuItem
#End Region
End Class