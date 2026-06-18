<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIconLookup
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents lvwIcons As System.Windows.Forms.ListView
	Public WithEvents imgTask As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmIconLookup))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdSelect = New System.Windows.Forms.Button
		Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
		Me.lvwIcons = New System.Windows.Forms.ListView
		Me.imgTask = New System.Windows.Forms.ImageList
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(544, 384)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdSelect
		' 
		Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSelect.CausesValidation = True
		Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSelect.Enabled = True
		Me.cmdSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSelect.Location = New System.Drawing.Point(456, 384)
		Me.cmdSelect.Name = "cmdSelect"
		Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSelect.Size = New System.Drawing.Size(73, 22)
		Me.cmdSelect.TabIndex = 1
		Me.cmdSelect.TabStop = True
		Me.cmdSelect.Text = "&Select"
		Me.cmdSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' uctPMResizer1
		' 
		Me.uctPMResizer1.Location = New System.Drawing.Point(8, 376)
		Me.uctPMResizer1.Name = "uctPMResizer1"
		' 
		' lvwIcons
		' 
		Me.lvwIcons.BackColor = System.Drawing.SystemColors.Window
		Me.lvwIcons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwIcons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwIcons.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwIcons.HideSelection = True
		Me.lvwIcons.LabelEdit = True
		Me.lvwIcons.LabelWrap = True
		Me.lvwIcons.LargeImageList = imgTask
		Me.lvwIcons.Location = New System.Drawing.Point(0, 0)
		Me.lvwIcons.Name = "lvwIcons"
		Me.lvwIcons.Size = New System.Drawing.Size(617, 377)
		Me.lvwIcons.TabIndex = 0
		' 
		' imgTask
		' 
		Me.imgTask.ImageSize = New System.Drawing.Size(32, 32)
		Me.imgTask.ImageStream = CType(resources.GetObject("imgTask.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgTask.TransparentColor = System.Drawing.SystemColors.Window
		Me.imgTask.Images.SetKeyName(0, "task")
		' 
		' frmIconLookup
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(625, 418)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdSelect)
		Me.Controls.Add(Me.uctPMResizer1)
		Me.Controls.Add(Me.lvwIcons)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmIconLookup"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Select Icon"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwIcons, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class