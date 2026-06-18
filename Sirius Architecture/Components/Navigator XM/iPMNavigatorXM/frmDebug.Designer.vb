<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDebug
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	Private Sub Ctx_mnuFile_Opening(ByVal sender As object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuFile.Opening
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		Ctx_mnuFile.Items.Clear()
		'We are moving the submenus from original menu to the context menu before displaying it
		For	Each item As System.Windows.Forms.ToolStripItem In mnuFile.DropDownItems
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			Ctx_mnuFile.Items.Add(item)
		Next item
		e.Cancel = False
	End Sub
	Private Sub Ctx_mnuFile_Closing(ByVal sender As object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuFile.Closing
		Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
		'We are moving the submenus the context menu back to the original menu after displaying
		For	Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuFile.Items
			list.Add(item)
		Next item
		For	Each item As System.Windows.Forms.ToolStripItem In list
			mnuFile.DropDownItems.Add(item)
		Next item
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
	Public WithEvents mnuFileSiriusLog As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileRootXML As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileExplore As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents cmdShow As System.Windows.Forms.Button
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Private WithEvents _lvwKeys_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwKeys_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwKeys As System.Windows.Forms.ListView
	Public WithEvents lblStep As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblCurrentStep As System.Windows.Forms.Label
	Public WithEvents Ctx_mnuFile As System.Windows.Forms.ContextMenuStrip
	Public WithEvents Ctx_mnuEntry As System.Windows.Forms.ContextMenuStrip
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.mnuEntry = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuEntryCopy = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSiriusLog = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileRootXML = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileExplore = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdShow = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.lvwKeys = New System.Windows.Forms.ListView
        Me._lvwKeys_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwKeys_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.lblStep = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblCurrentStep = New System.Windows.Forms.Label
        Me.Ctx_mnuFile = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Ctx_mnuEntry = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.MainMenu1.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEntry, Me.mnuFile})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(264, 24)
        Me.MainMenu1.TabIndex = 6
        '
        'mnuEntry
        '
        Me.mnuEntry.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEntryCopy})
        Me.mnuEntry.Name = "mnuEntry"
        Me.mnuEntry.Size = New System.Drawing.Size(32, 19)
        Me.mnuEntry.Text = "&Entry"
        '
        'mnuEntryCopy
        '
        Me.mnuEntryCopy.Name = "mnuEntryCopy"
        Me.mnuEntryCopy.Size = New System.Drawing.Size(160, 22)
        Me.mnuEntryCopy.Text = "&Copy to Clipboard"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileSiriusLog, Me.mnuFileRootXML, Me.mnuFileExplore})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(32, 19)
        Me.mnuFile.Text = "&File"
        '
        'mnuFileSiriusLog
        '
        Me.mnuFileSiriusLog.Name = "mnuFileSiriusLog"
        Me.mnuFileSiriusLog.Size = New System.Drawing.Size(138, 22)
        Me.mnuFileSiriusLog.Text = "&Sirius Log"
        '
        'mnuFileRootXML
        '
        Me.mnuFileRootXML.Name = "mnuFileRootXML"
        Me.mnuFileRootXML.Size = New System.Drawing.Size(138, 22)
        Me.mnuFileRootXML.Text = "&Root XML File"
        '
        'mnuFileExplore
        '
        Me.mnuFileExplore.Name = "mnuFileExplore"
        Me.mnuFileExplore.Size = New System.Drawing.Size(138, 22)
        Me.mnuFileExplore.Text = "&XML Folder"
        '
        'cmdShow
        '
        Me.cmdShow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdShow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdShow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdShow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdShow.Location = New System.Drawing.Point(8, 504)
        Me.cmdShow.Name = "cmdShow"
        Me.cmdShow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdShow.Size = New System.Drawing.Size(73, 22)
        Me.cmdShow.TabIndex = 4
        Me.cmdShow.Text = "&View..."
        Me.cmdShow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdShow.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(184, 504)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 22)
        Me.cmdClose.TabIndex = 2
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'lvwKeys
        '
        Me.lvwKeys.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwKeys, "")
        Me.lvwKeys.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwKeys_ColumnHeader_1, Me._lvwKeys_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwKeys, False)
        Me.lvwKeys.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwKeys.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwKeys.FullRowSelect = True
        Me.lvwKeys.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwKeys, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwKeys, "")
        Me.lvwKeys.Location = New System.Drawing.Point(8, 88)
        Me.lvwKeys.MultiSelect = False
        Me.lvwKeys.Name = "lvwKeys"
        Me.lvwKeys.Size = New System.Drawing.Size(249, 409)
        Me.listViewHelper1.SetSmallIcons(Me.lvwKeys, "")
        Me.listViewHelper1.SetSorted(Me.lvwKeys, False)
        Me.listViewHelper1.SetSortKey(Me.lvwKeys, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwKeys, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwKeys.TabIndex = 1
        Me.lvwKeys.UseCompatibleStateImageBehavior = False
        Me.lvwKeys.View = System.Windows.Forms.View.Details
        '
        '_lvwKeys_ColumnHeader_1
        '
        Me._lvwKeys_ColumnHeader_1.Text = "Key Name"
        Me._lvwKeys_ColumnHeader_1.Width = 97
        '
        '_lvwKeys_ColumnHeader_2
        '
        Me._lvwKeys_ColumnHeader_2.Text = "Key Value"
        Me._lvwKeys_ColumnHeader_2.Width = 97
        '
        'lblStep
        '
        Me.lblStep.AutoSize = True
        Me.lblStep.BackColor = System.Drawing.SystemColors.Control
        Me.lblStep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStep.Location = New System.Drawing.Point(95, 64)
        Me.lblStep.Name = "lblStep"
        Me.lblStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStep.Size = New System.Drawing.Size(41, 15)
        Me.lblStep.TabIndex = 5
        Me.lblStep.Text = "Label2"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Gray
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Label1.Location = New System.Drawing.Point(8, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(249, 25)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = " Debug"
        '
        'lblCurrentStep
        '
        Me.lblCurrentStep.AutoSize = True
        Me.lblCurrentStep.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrentStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentStep.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrentStep.Location = New System.Drawing.Point(8, 65)
        Me.lblCurrentStep.Name = "lblCurrentStep"
        Me.lblCurrentStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentStep.Size = New System.Drawing.Size(86, 13)
        Me.lblCurrentStep.TabIndex = 0
        Me.lblCurrentStep.Text = "Current Step:"
        '
        'Ctx_mnuFile
        '
        Me.Ctx_mnuFile.Name = "Ctx_mnuFile"
        Me.Ctx_mnuFile.Size = New System.Drawing.Size(61, 4)
        '
        'Ctx_mnuEntry
        '
        Me.Ctx_mnuEntry.Name = "Ctx_mnuEntry"
        Me.Ctx_mnuEntry.Size = New System.Drawing.Size(61, 4)
        '
        'frmDebug
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(264, 528)
        Me.Controls.Add(Me.cmdShow)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.lvwKeys)
        Me.Controls.Add(Me.lblStep)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblCurrentStep)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Location = New System.Drawing.Point(752, 19)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDebug"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Debug"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class