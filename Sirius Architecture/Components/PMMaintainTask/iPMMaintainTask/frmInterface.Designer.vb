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
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public dlgMainOpen As System.Windows.Forms.OpenFileDialog
	Public dlgMainSave As System.Windows.Forms.SaveFileDialog
	Public dlgMainFont As System.Windows.Forms.FontDialog
	Public dlgMainColor As System.Windows.Forms.ColorDialog
	Public dlgMainPrint As System.Windows.Forms.PrintDialog
	Public WithEvents uctPMResizer As PMResizerControl.uctPMResizer
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lvwTable As System.Windows.Forms.ListView
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdApply = New System.Windows.Forms.Button
		Me.dlgMainOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgMainSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgMainFont = New System.Windows.Forms.FontDialog
		Me.dlgMainColor = New System.Windows.Forms.ColorDialog
		Me.dlgMainPrint = New System.Windows.Forms.PrintDialog
		Me.uctPMResizer = New PMResizerControl.uctPMResizer
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.imgIcon = New System.Windows.Forms.PictureBox
		Me.lvwTable = New System.Windows.Forms.ListView
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = True
		Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(616, 488)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(73, 22)
		Me.cmdApply.TabIndex = 5
		Me.cmdApply.TabStop = True
		Me.cmdApply.Text = "App&ly"
		Me.cmdApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' uctPMResizer
		' 
		Me.uctPMResizer.Location = New System.Drawing.Point(328, 480)
		Me.uctPMResizer.Name = "uctPMResizer"
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(536, 488)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(456, 488)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(136, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(1, 0)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(692, 485)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 3
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lvwTable)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMainTab_TabPage0.Text = "&1 - Tasks"
		' 
		' imgIcon
		' 
		Me.imgIcon.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgIcon.Enabled = True
		Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
		Me.imgIcon.Location = New System.Drawing.Point(632, 4)
		Me.imgIcon.Name = "imgIcon"
		Me.imgIcon.Size = New System.Drawing.Size(32, 32)
		Me.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgIcon.Visible = True
		' 
		' lvwTable
		' 
		Me.lvwTable.BackColor = System.Drawing.SystemColors.Window
		Me.lvwTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwTable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwTable.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwTable.HideSelection = False
		Me.lvwTable.LabelEdit = False
		Me.lvwTable.LabelWrap = True
		Me.lvwTable.Location = New System.Drawing.Point(8, 12)
		Me.lvwTable.Name = "lvwTable"
		Me.lvwTable.Size = New System.Drawing.Size(593, 441)
		Me.lvwTable.TabIndex = 0
		Me.lvwTable.View = System.Windows.Forms.View.Details
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(608, 44)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 4
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(694, 517)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.uctPMResizer)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.dlgMainOpen.DefaultExt = "csv"
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Manage Task Options"
		Me.dlgMainOpen.Title = "Save As..."
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.listViewHelper1.SetItemClickMethod(Me.lvwTable, "lvwTable_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwTable, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class