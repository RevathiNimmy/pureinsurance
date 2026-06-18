<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Private WithEvents _lvwRateTypes_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRateTypes_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRateTypes_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRateTypes_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwRateTypes_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwRateTypes As System.Windows.Forms.ListView
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdDetails As System.Windows.Forms.Button
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.lvwRateTypes = New System.Windows.Forms.ListView
		Me._lvwRateTypes_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwRateTypes_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwRateTypes_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwRateTypes_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwRateTypes_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdDetails = New System.Windows.Forms.Button
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.lvwRateTypes.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' uctPMResizer1
		' 
		Me.uctPMResizer1.Location = New System.Drawing.Point(8, 384)
		Me.uctPMResizer1.Name = "uctPMResizer1"
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(288, 392)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(376, 392)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 0
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMain
		' 
		Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
		Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.ItemSize = New System.Drawing.Size(146, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 8)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(445, 381)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 2
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.lvwRateTypes)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdDetails)
		Me._tabMain_TabPage0.Text = "&1 - General"
		' 
		' lvwRateTypes
		' 
		Me.lvwRateTypes.BackColor = System.Drawing.SystemColors.Window
		Me.lvwRateTypes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwRateTypes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwRateTypes.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwRateTypes.FullRowSelect = True
		Me.lvwRateTypes.GridLines = True
		Me.lvwRateTypes.HideSelection = True
		Me.lvwRateTypes.LabelEdit = False
		Me.lvwRateTypes.LabelWrap = True
		Me.lvwRateTypes.Location = New System.Drawing.Point(8, 12)
		Me.lvwRateTypes.Name = "lvwRateTypes"
		Me.lvwRateTypes.Size = New System.Drawing.Size(425, 305)
		Me.lvwRateTypes.TabIndex = 3
		Me.lvwRateTypes.View = System.Windows.Forms.View.Details
		Me.lvwRateTypes.Columns.Add(Me._lvwRateTypes_ColumnHeader_1)
		Me.lvwRateTypes.Columns.Add(Me._lvwRateTypes_ColumnHeader_2)
		Me.lvwRateTypes.Columns.Add(Me._lvwRateTypes_ColumnHeader_3)
		Me.lvwRateTypes.Columns.Add(Me._lvwRateTypes_ColumnHeader_4)
		Me.lvwRateTypes.Columns.Add(Me._lvwRateTypes_ColumnHeader_5)
		' 
		' _lvwRateTypes_ColumnHeader_1
		' 
		Me._lvwRateTypes_ColumnHeader_1.Text = "Description"
		Me._lvwRateTypes_ColumnHeader_1.Width = 97
		' 
		' _lvwRateTypes_ColumnHeader_2
		' 
		Me._lvwRateTypes_ColumnHeader_2.Text = "Lookup 1"
		Me._lvwRateTypes_ColumnHeader_2.Width = 97
		' 
		' _lvwRateTypes_ColumnHeader_3
		' 
		Me._lvwRateTypes_ColumnHeader_3.Text = "Lookup 2"
		Me._lvwRateTypes_ColumnHeader_3.Width = 97
		' 
		' _lvwRateTypes_ColumnHeader_4
		' 
		Me._lvwRateTypes_ColumnHeader_4.Text = "Lookup 3"
		Me._lvwRateTypes_ColumnHeader_4.Width = 97
		' 
		' _lvwRateTypes_ColumnHeader_5
		' 
		Me._lvwRateTypes_ColumnHeader_5.Text = "ID"
		Me._lvwRateTypes_ColumnHeader_5.Width = 0
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(280, 324)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 4
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(360, 324)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 5
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "&Add"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdDetails
		' 
		Me.cmdDetails.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDetails.CausesValidation = True
		Me.cmdDetails.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDetails.Enabled = True
		Me.cmdDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDetails.Location = New System.Drawing.Point(200, 324)
		Me.cmdDetails.Name = "cmdDetails"
		Me.cmdDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDetails.Size = New System.Drawing.Size(73, 22)
		Me.cmdDetails.TabIndex = 6
		Me.cmdDetails.TabStop = True
		Me.cmdDetails.Text = "&Details"
		Me.cmdDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDetails.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmMain
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(455, 419)
		Me.ControlBox = True
		Me.Controls.Add(Me.uctPMResizer1)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmMain.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmMain"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Rating Maintenance"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.lvwRateTypes.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class