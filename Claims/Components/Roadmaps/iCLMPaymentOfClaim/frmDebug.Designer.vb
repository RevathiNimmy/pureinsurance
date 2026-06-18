<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDebug
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	Private Sub Ctx_mnuEntry_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuEntry.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuEntry.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuEntry.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuEntry.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuEntry_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuEntry.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuEntry.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuEntry.DropDownItems.Add(item)
		Next item
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
	Public WithEvents mnuEntryCopy As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuEntry As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents cmdSiriusLog As System.Windows.Forms.Button
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents panStep As System.Windows.Forms.Panel
	Private WithEvents _lvwKeys_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwKeys_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwKeys As System.Windows.Forms.ListView
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblCurrentStep As System.Windows.Forms.Label
	Public WithEvents Ctx_mnuEntry As System.Windows.Forms.ContextMenuStrip
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDebug))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuEntry = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuEntryCopy = New System.Windows.Forms.ToolStripMenuItem
		Me.cmdSiriusLog = New System.Windows.Forms.Button
		Me.cmdClose = New System.Windows.Forms.Button
		Me.panStep = New System.Windows.Forms.Panel
		Me.lvwKeys = New System.Windows.Forms.ListView
		Me._lvwKeys_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwKeys_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me.Label1 = New System.Windows.Forms.Label
		Me.lblCurrentStep = New System.Windows.Forms.Label
		Me.lvwKeys.SuspendLayout()
		Me.SuspendLayout()
		'Ctx_mnuEntry
		Me.Ctx_mnuEntry = New System.Windows.Forms.ContextMenuStrip(Me.components)
		Me.Ctx_mnuEntry.Size = New System.Drawing.Size(153, 26)
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuEntry})
		' 
		' mnuEntry
		' 
		Me.mnuEntry.Available = False
		Me.mnuEntry.Checked = False
		Me.mnuEntry.Enabled = True
		Me.mnuEntry.Name = "mnuEntry"
		Me.mnuEntry.Text = "&Entry"
		Me.mnuEntry.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuEntryCopy})
		' 
		' mnuEntryCopy
		' 
		Me.mnuEntryCopy.Available = True
		Me.mnuEntryCopy.Checked = False
		Me.mnuEntryCopy.Enabled = True
		Me.mnuEntryCopy.Name = "mnuEntryCopy"
		Me.mnuEntryCopy.Text = "&Copy to Clipboard"
		' 
		' cmdSiriusLog
		' 
		Me.cmdSiriusLog.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSiriusLog.CausesValidation = True
		Me.cmdSiriusLog.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSiriusLog.Enabled = True
		Me.cmdSiriusLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSiriusLog.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSiriusLog.Location = New System.Drawing.Point(8, 504)
		Me.cmdSiriusLog.Name = "cmdSiriusLog"
		Me.cmdSiriusLog.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSiriusLog.Size = New System.Drawing.Size(73, 22)
		Me.cmdSiriusLog.TabIndex = 5
		Me.cmdSiriusLog.TabStop = True
		Me.cmdSiriusLog.Text = "Sirius &Log"
		Me.cmdSiriusLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSiriusLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdClose
		' 
		Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
		Me.cmdClose.CausesValidation = True
		Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdClose.Enabled = True
		Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdClose.Location = New System.Drawing.Point(144, 504)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdClose.Size = New System.Drawing.Size(73, 22)
		Me.cmdClose.TabIndex = 3
		Me.cmdClose.TabStop = True
		Me.cmdClose.Text = "&Close"
		Me.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' panStep
		' 
		Me.panStep.BackColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.panStep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.panStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.panStep.Location = New System.Drawing.Point(80, 64)
		Me.panStep.Name = "panStep"
		Me.panStep.Size = New System.Drawing.Size(137, 17)
		Me.panStep.TabIndex = 2
		' 
		' lvwKeys
		' 
		Me.lvwKeys.BackColor = System.Drawing.SystemColors.Window
		Me.lvwKeys.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwKeys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwKeys.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwKeys.FullRowSelect = True
		Me.lvwKeys.GridLines = True
		Me.lvwKeys.HideSelection = True
		Me.lvwKeys.LabelEdit = False
		Me.lvwKeys.LabelWrap = True
		Me.lvwKeys.Location = New System.Drawing.Point(8, 88)
		Me.lvwKeys.Name = "lvwKeys"
		Me.lvwKeys.Size = New System.Drawing.Size(209, 409)
		Me.lvwKeys.TabIndex = 1
		Me.lvwKeys.View = System.Windows.Forms.View.Details
		Me.lvwKeys.Columns.Add(Me._lvwKeys_ColumnHeader_1)
		Me.lvwKeys.Columns.Add(Me._lvwKeys_ColumnHeader_2)
		' 
		' _lvwKeys_ColumnHeader_1
		' 
		Me._lvwKeys_ColumnHeader_1.Text = "Key Name"
		Me._lvwKeys_ColumnHeader_1.Width = 97
		' 
		' _lvwKeys_ColumnHeader_2
		' 
		Me._lvwKeys_ColumnHeader_2.Text = "Key Value"
		Me._lvwKeys_ColumnHeader_2.Width = 97
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.Color.Gray
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.HighlightText
		Me.Label1.Location = New System.Drawing.Point(8, 32)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(209, 25)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = " Debug"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' lblCurrentStep
		' 
		Me.lblCurrentStep.AutoSize = True
		Me.lblCurrentStep.BackColor = System.Drawing.SystemColors.Control
		Me.lblCurrentStep.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCurrentStep.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCurrentStep.Enabled = True
		Me.lblCurrentStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCurrentStep.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCurrentStep.Location = New System.Drawing.Point(8, 64)
		Me.lblCurrentStep.Name = "lblCurrentStep"
		Me.lblCurrentStep.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCurrentStep.Size = New System.Drawing.Size(62, 13)
		Me.lblCurrentStep.TabIndex = 0
		Me.lblCurrentStep.Text = "Current Step:"
		Me.lblCurrentStep.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCurrentStep.UseMnemonic = True
		Me.lblCurrentStep.Visible = True
		' 
		' frmDebug
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(224, 533)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdSiriusLog)
		Me.Controls.Add(Me.cmdClose)
		Me.Controls.Add(Me.panStep)
		Me.Controls.Add(Me.lvwKeys)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.lblCurrentStep)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(518, 25)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDebug"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Debug"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwKeys.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class