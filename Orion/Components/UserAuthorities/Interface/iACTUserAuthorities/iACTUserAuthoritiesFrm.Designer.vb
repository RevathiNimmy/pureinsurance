<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwResults_InitializeColumnKeys()
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
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblUserCount As System.Windows.Forms.Label
	Private WithEvents _lvwResults_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwResults_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwResults As System.Windows.Forms.ListView
	Public WithEvents cboUser As PMUserLookupControl.cboPMUserLookup
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
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblUserCount = New System.Windows.Forms.Label
		Me.lvwResults = New System.Windows.Forms.ListView
		Me._lvwResults_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_13 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_14 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_15 = New System.Windows.Forms.ColumnHeader
		Me.cboUser = New PMUserLookupControl.cboPMUserLookup
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.lvwResults.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(8, 353)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 4
		Me.cmdNavigate.TabStop = True
		Me.cmdNavigate.Text = "&Navigate"
		Me.cmdNavigate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		Me.cmdNavigate.Visible = False
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(571, 353)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 2
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(491, 352)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
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
		Me.cmdOK.Location = New System.Drawing.Point(411, 353)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
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
		Me.tabMainTab.ItemSize = New System.Drawing.Size(126, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(640, 341)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 3
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblUserCount)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lvwResults)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cboUser)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabMainTab_TabPage0.Text = "&1 - General"
		' 
		' lblUserCount
		' 
		Me.lblUserCount.AutoSize = False
		Me.lblUserCount.BackColor = System.Drawing.SystemColors.Control
		Me.lblUserCount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblUserCount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblUserCount.Enabled = True
		Me.lblUserCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblUserCount.ForeColor = System.Drawing.SystemColors.GrayText
		Me.lblUserCount.Location = New System.Drawing.Point(8, 287)
		Me.lblUserCount.Name = "lblUserCount"
		Me.lblUserCount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblUserCount.Size = New System.Drawing.Size(105, 17)
		Me.lblUserCount.TabIndex = 7
		Me.lblUserCount.Text = "lblUserCount"
		Me.lblUserCount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblUserCount.UseMnemonic = True
		Me.lblUserCount.Visible = True
		' 
		' lvwResults
		' 
		Me.lvwResults.BackColor = System.Drawing.SystemColors.Window
		Me.lvwResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwResults.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwResults.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwResults.HideSelection = True
		Me.lvwResults.LabelEdit = False
		Me.lvwResults.LabelWrap = True
		Me.lvwResults.Location = New System.Drawing.Point(6, 10)
		Me.lvwResults.Name = "lvwResults"
		Me.lvwResults.Size = New System.Drawing.Size(618, 268)
		Me.lvwResults.TabIndex = 5
		Me.lvwResults.View = System.Windows.Forms.View.Details
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_1)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_2)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_3)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_4)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_5)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_6)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_7)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_8)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_9)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_10)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_11)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_12)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_13)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_14)
		Me.lvwResults.Columns.Add(Me._lvwResults_ColumnHeader_15)
		' 
		' _lvwResults_ColumnHeader_1
		' 
		Me._lvwResults_ColumnHeader_1.Tag = ""
		Me._lvwResults_ColumnHeader_1.Text = "User"
		Me._lvwResults_ColumnHeader_1.Width = 97
		' 
		' _lvwResults_ColumnHeader_2
		' 
		Me._lvwResults_ColumnHeader_2.Tag = ""
		Me._lvwResults_ColumnHeader_2.Text = "Write Offs"
		Me._lvwResults_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_2.Width = 97
		' 
		' _lvwResults_ColumnHeader_3
		' 
		Me._lvwResults_ColumnHeader_3.Tag = ""
		Me._lvwResults_ColumnHeader_3.Text = "Write Off Amount"
		Me._lvwResults_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwResults_ColumnHeader_3.Width = 97
		' 
		' _lvwResults_ColumnHeader_4
		' 
		Me._lvwResults_ColumnHeader_4.Tag = ""
		Me._lvwResults_ColumnHeader_4.Text = "Unrestricted Enquiry"
		Me._lvwResults_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_4.Width = 97
		' 
		' _lvwResults_ColumnHeader_5
		' 
		Me._lvwResults_ColumnHeader_5.Tag = ""
		Me._lvwResults_ColumnHeader_5.Text = "Unrestricted Update"
		Me._lvwResults_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_5.Width = 97
		' 
		' _lvwResults_ColumnHeader_6
		' 
		Me._lvwResults_ColumnHeader_6.Tag = ""
		Me._lvwResults_ColumnHeader_6.Text = "Currency Date"
		Me._lvwResults_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_6.Width = 97
		' 
		' _lvwResults_ColumnHeader_7
		' 
		Me._lvwResults_ColumnHeader_7.Tag = ""
		Me._lvwResults_ColumnHeader_7.Text = "Currency Rate"
		Me._lvwResults_ColumnHeader_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_7.Width = 97
		' 
		' _lvwResults_ColumnHeader_8
		' 
		Me._lvwResults_ColumnHeader_8.Tag = ""
		Me._lvwResults_ColumnHeader_8.Text = "Transation Write Offs"
		Me._lvwResults_ColumnHeader_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_8.Width = 97
		' 
		' _lvwResults_ColumnHeader_9
		' 
		Me._lvwResults_ColumnHeader_9.Tag = ""
		Me._lvwResults_ColumnHeader_9.Text = "Transaction Write Off Amount"
		Me._lvwResults_ColumnHeader_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwResults_ColumnHeader_9.Width = 97
		' 
		' _lvwResults_ColumnHeader_10
		' 
		Me._lvwResults_ColumnHeader_10.Tag = ""
		Me._lvwResults_ColumnHeader_10.Text = "Refund Authority"
		Me._lvwResults_ColumnHeader_10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_10.Width = 97
		' 
		' _lvwResults_ColumnHeader_11
		' 
		Me._lvwResults_ColumnHeader_11.Tag = ""
		Me._lvwResults_ColumnHeader_11.Text = "Transfer Authority"
		Me._lvwResults_ColumnHeader_11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_11.Width = 97
		' 
		' _lvwResults_ColumnHeader_12
		' 
		Me._lvwResults_ColumnHeader_12.Tag = ""
		Me._lvwResults_ColumnHeader_12.Text = "Payments Authority"
		Me._lvwResults_ColumnHeader_12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_12.Width = 97
		' 
		' _lvwResults_ColumnHeader_13
		' 
		Me._lvwResults_ColumnHeader_13.Tag = ""
		Me._lvwResults_ColumnHeader_13.Text = "Payments Amount"
		Me._lvwResults_ColumnHeader_13.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwResults_ColumnHeader_13.Width = 97
		' 
		' _lvwResults_ColumnHeader_14
		' 
		Me._lvwResults_ColumnHeader_14.Tag = ""
		Me._lvwResults_ColumnHeader_14.Text = "Claim Payments Authority"
		Me._lvwResults_ColumnHeader_14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		Me._lvwResults_ColumnHeader_14.Width = 97
		' 
		' _lvwResults_ColumnHeader_15
		' 
		Me._lvwResults_ColumnHeader_15.Tag = ""
		Me._lvwResults_ColumnHeader_15.Text = "Claim Payments Amount"
		Me._lvwResults_ColumnHeader_15.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwResults_ColumnHeader_15.Width = 97
		' 
		' cboUser
		' 
		Me.cboUser.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboUser.Location = New System.Drawing.Point(195, 284)
		Me.cboUser.Name = "cboUser"
		Me.cboUser.Size = New System.Drawing.Size(81, 21)
		Me.cboUser.Sorted = True
		Me.cboUser.TabIndex = 6
		Me.cboUser.Visible = False
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = False
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(552, 283)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 8
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "&Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmInterface
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(653, 382)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(374, 241)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "User Authorities"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwResults, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.lvwResults.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwResults_InitializeColumnKeys()
		Me._lvwResults_ColumnHeader_1.Name = ""
		Me._lvwResults_ColumnHeader_2.Name = ""
		Me._lvwResults_ColumnHeader_3.Name = ""
		Me._lvwResults_ColumnHeader_4.Name = ""
		Me._lvwResults_ColumnHeader_5.Name = ""
		Me._lvwResults_ColumnHeader_6.Name = ""
		Me._lvwResults_ColumnHeader_7.Name = ""
		Me._lvwResults_ColumnHeader_8.Name = ""
		Me._lvwResults_ColumnHeader_9.Name = ""
		Me._lvwResults_ColumnHeader_10.Name = ""
		Me._lvwResults_ColumnHeader_11.Name = ""
		Me._lvwResults_ColumnHeader_12.Name = ""
		Me._lvwResults_ColumnHeader_13.Name = ""
		Me._lvwResults_ColumnHeader_14.Name = ""
		Me._lvwResults_ColumnHeader_15.Name = ""
	End Sub
#End Region 
End Class