<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFollowUp
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents txtReferredTo As System.Windows.Forms.TextBox
	Public WithEvents fraReferredTo As System.Windows.Forms.GroupBox
	Public WithEvents txtFollowUpNote As System.Windows.Forms.TextBox
	Public WithEvents fraFollowUp As System.Windows.Forms.GroupBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFollowUp))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.fraReferredTo = New System.Windows.Forms.GroupBox
		Me.txtReferredTo = New System.Windows.Forms.TextBox
		Me.fraFollowUp = New System.Windows.Forms.GroupBox
		Me.txtFollowUpNote = New System.Windows.Forms.TextBox
		Me.fraReferredTo.SuspendLayout()
		Me.fraFollowUp.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(264, 288)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 5
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(344, 288)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 4
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraReferredTo
		' 
		Me.fraReferredTo.BackColor = System.Drawing.SystemColors.Control
		Me.fraReferredTo.Controls.Add(Me.txtReferredTo)
		Me.fraReferredTo.Enabled = True
		Me.fraReferredTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraReferredTo.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraReferredTo.Location = New System.Drawing.Point(8, 144)
		Me.fraReferredTo.Name = "fraReferredTo"
		Me.fraReferredTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraReferredTo.Size = New System.Drawing.Size(409, 129)
		Me.fraReferredTo.TabIndex = 2
		Me.fraReferredTo.Text = "Referred To:"
		Me.fraReferredTo.Visible = True
		' 
		' txtReferredTo
		' 
		Me.txtReferredTo.AcceptsReturn = True
		Me.txtReferredTo.AutoSize = False
		Me.txtReferredTo.BackColor = System.Drawing.SystemColors.Window
		Me.txtReferredTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReferredTo.CausesValidation = True
		Me.txtReferredTo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReferredTo.Enabled = True
		Me.txtReferredTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtReferredTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReferredTo.HideSelection = True
		Me.txtReferredTo.Location = New System.Drawing.Point(16, 24)
		Me.txtReferredTo.MaxLength = 255
		Me.txtReferredTo.Multiline = True
		Me.txtReferredTo.Name = "txtReferredTo"
		Me.txtReferredTo.ReadOnly = False
		Me.txtReferredTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReferredTo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReferredTo.Size = New System.Drawing.Size(377, 89)
		Me.txtReferredTo.TabIndex = 3
		Me.txtReferredTo.TabStop = True
		Me.txtReferredTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReferredTo.Visible = True
		' 
		' fraFollowUp
		' 
		Me.fraFollowUp.BackColor = System.Drawing.SystemColors.Control
		Me.fraFollowUp.Controls.Add(Me.txtFollowUpNote)
		Me.fraFollowUp.Enabled = True
		Me.fraFollowUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraFollowUp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraFollowUp.Location = New System.Drawing.Point(8, 8)
		Me.fraFollowUp.Name = "fraFollowUp"
		Me.fraFollowUp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraFollowUp.Size = New System.Drawing.Size(409, 129)
		Me.fraFollowUp.TabIndex = 0
		Me.fraFollowUp.Text = "Follow Up Note:"
		Me.fraFollowUp.Visible = True
		' 
		' txtFollowUpNote
		' 
		Me.txtFollowUpNote.AcceptsReturn = True
		Me.txtFollowUpNote.AutoSize = False
		Me.txtFollowUpNote.BackColor = System.Drawing.SystemColors.Window
		Me.txtFollowUpNote.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFollowUpNote.CausesValidation = True
		Me.txtFollowUpNote.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFollowUpNote.Enabled = True
		Me.txtFollowUpNote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFollowUpNote.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFollowUpNote.HideSelection = True
		Me.txtFollowUpNote.Location = New System.Drawing.Point(16, 24)
		Me.txtFollowUpNote.MaxLength = 255
		Me.txtFollowUpNote.Multiline = True
		Me.txtFollowUpNote.Name = "txtFollowUpNote"
		Me.txtFollowUpNote.ReadOnly = False
		Me.txtFollowUpNote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFollowUpNote.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFollowUpNote.Size = New System.Drawing.Size(377, 89)
		Me.txtFollowUpNote.TabIndex = 1
		Me.txtFollowUpNote.TabStop = True
		Me.txtFollowUpNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFollowUpNote.Visible = True
		' 
		' frmFollowUp
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(425, 321)
		Me.ControlBox = False
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.fraReferredTo)
		Me.Controls.Add(Me.fraFollowUp)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmFollowUp"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Follow Up Description"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraReferredTo.ResumeLayout(False)
		Me.fraFollowUp.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class