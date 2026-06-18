<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		Form_Initialize_Renamed()
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
	Public WithEvents cmdPrintAll As System.Windows.Forms.Button
	Public WithEvents cmdPrint As System.Windows.Forms.Button
	Public WithEvents cmdNotes As System.Windows.Forms.Button
	Public WithEvents cmdNotesAdd As System.Windows.Forms.Button
	Public WithEvents cmdNotesView As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents CmdCancel As System.Windows.Forms.Button
	Public WithEvents CmdHelp As System.Windows.Forms.Button
	Public WithEvents uctListEvents1 As uctListEventsControl.uctListEvents
	Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdPrintAll = New System.Windows.Forms.Button
        Me.cmdPrint = New System.Windows.Forms.Button
        Me.cmdNotes = New System.Windows.Forms.Button
        Me.cmdNotesAdd = New System.Windows.Forms.Button
        Me.cmdNotesView = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.CmdCancel = New System.Windows.Forms.Button
        Me.CmdHelp = New System.Windows.Forms.Button
        Me.uctListEvents1 = New uctListEventsControl.uctListEvents
        Me.commandButtonHelper1 = New Artinsoft.VB6.Gui.CommandButtonHelper(Me.components)
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdPrintAll
        '
        Me.cmdPrintAll.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdPrintAll, True)
        Me.cmdPrintAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdPrintAll, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdPrintAll, Nothing)
        Me.cmdPrintAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrintAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrintAll.Location = New System.Drawing.Point(360, 520)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdPrintAll, System.Drawing.Color.Silver)
        Me.cmdPrintAll.Name = "cmdPrintAll"
        Me.cmdPrintAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrintAll.Size = New System.Drawing.Size(80, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdPrintAll, 0)
        Me.cmdPrintAll.TabIndex = 8
        Me.cmdPrintAll.Text = "Print A&ll"
        Me.cmdPrintAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrintAll.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdPrint, True)
        Me.cmdPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdPrint, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdPrint, Nothing)
        Me.cmdPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPrint.Location = New System.Drawing.Point(272, 520)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdPrint, System.Drawing.Color.Silver)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPrint.Size = New System.Drawing.Size(80, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdPrint, 0)
        Me.cmdPrint.TabIndex = 7
        Me.cmdPrint.Text = "&Print"
        Me.cmdPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPrint.UseVisualStyleBackColor = False
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
        Me.cmdNotes.Location = New System.Drawing.Point(184, 520)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdNotes, System.Drawing.Color.Silver)
        Me.cmdNotes.Name = "cmdNotes"
        Me.cmdNotes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNotes.Size = New System.Drawing.Size(80, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdNotes, 0)
        Me.cmdNotes.TabIndex = 5
        Me.cmdNotes.Text = "Event Notes"
        Me.cmdNotes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNotes.UseVisualStyleBackColor = False
        '
        'cmdNotesAdd
        '
        Me.cmdNotesAdd.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdNotesAdd, True)
        Me.cmdNotesAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdNotesAdd, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdNotesAdd, Nothing)
        Me.cmdNotesAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNotesAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNotesAdd.Location = New System.Drawing.Point(96, 520)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdNotesAdd, System.Drawing.Color.Silver)
        Me.cmdNotesAdd.Name = "cmdNotesAdd"
        Me.cmdNotesAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNotesAdd.Size = New System.Drawing.Size(80, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdNotesAdd, 1)
        Me.cmdNotesAdd.TabIndex = 4
        Me.cmdNotesAdd.Text = "&Add"
        Me.cmdNotesAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNotesAdd.UseVisualStyleBackColor = False
        '
        'cmdNotesView
        '
        Me.cmdNotesView.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdNotesView, True)
        Me.cmdNotesView.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdNotesView, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdNotesView, Nothing)
        Me.cmdNotesView.Enabled = False
        Me.cmdNotesView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNotesView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNotesView.Location = New System.Drawing.Point(8, 520)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdNotesView, System.Drawing.Color.Silver)
        Me.cmdNotesView.Name = "cmdNotesView"
        Me.cmdNotesView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNotesView.Size = New System.Drawing.Size(80, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdNotesView, 1)
        Me.cmdNotesView.TabIndex = 3
        Me.cmdNotesView.Text = "&View"
        Me.cmdNotesView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNotesView.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdOK, True)
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdOK, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdOK, Nothing)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(536, 520)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdOK, System.Drawing.Color.Silver)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdOK, 0)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'CmdCancel
        '
        Me.CmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.CmdCancel, True)
        Me.CmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.CmdCancel, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.CmdCancel, Nothing)
        Me.CmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdCancel.Location = New System.Drawing.Point(616, 520)
        Me.commandButtonHelper1.SetMaskColor(Me.CmdCancel, System.Drawing.Color.Silver)
        Me.CmdCancel.Name = "CmdCancel"
        Me.CmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.CmdCancel, 0)
        Me.CmdCancel.TabIndex = 2
        Me.CmdCancel.Text = "&Cancel"
        Me.CmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdCancel.UseVisualStyleBackColor = False
        '
        'CmdHelp
        '
        Me.CmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.CmdHelp, True)
        Me.CmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.CmdHelp, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.CmdHelp, Nothing)
        Me.CmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdHelp.Location = New System.Drawing.Point(696, 520)
        Me.commandButtonHelper1.SetMaskColor(Me.CmdHelp, System.Drawing.Color.Silver)
        Me.CmdHelp.Name = "CmdHelp"
        Me.CmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.CmdHelp, 0)
        Me.CmdHelp.TabIndex = 1
        Me.CmdHelp.Text = "&Help"
        Me.CmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdHelp.UseVisualStyleBackColor = False
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
        Me.uctListEvents1.Location = New System.Drawing.Point(0, 0)
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
        Me.uctListEvents1.Size = New System.Drawing.Size(769, 513)
        Me.uctListEvents1.Status = 0
        Me.uctListEvents1.TabIndex = 6
        Me.uctListEvents1.Task = 0
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(775, 548)
        Me.Controls.Add(Me.cmdPrintAll)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.cmdNotes)
        Me.Controls.Add(Me.cmdNotesAdd)
        Me.Controls.Add(Me.cmdNotesView)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.CmdCancel)
        Me.Controls.Add(Me.CmdHelp)
        Me.Controls.Add(Me.uctListEvents1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(11, 30)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Event Log"
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class