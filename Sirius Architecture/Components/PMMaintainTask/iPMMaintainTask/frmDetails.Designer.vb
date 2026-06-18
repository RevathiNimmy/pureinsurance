<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
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
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents chkAutoUpdateBatch As System.Windows.Forms.CheckBox
	Public WithEvents chkActionRequired As System.Windows.Forms.CheckBox
	Public WithEvents cboIncompleteTask As System.Windows.Forms.ComboBox
	Public WithEvents cboCompletionTask As System.Windows.Forms.ComboBox
	Public WithEvents lblAutoBatchUpdate As System.Windows.Forms.Label
	Public WithEvents lblIncompleteTask As System.Windows.Forms.Label
	Public WithEvents lblActionRequired As System.Windows.Forms.Label
	Public WithEvents lblCompletionTask As System.Windows.Forms.Label
	Public WithEvents fraDefaults As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblCode = New System.Windows.Forms.Label
		Me.lblDescription = New System.Windows.Forms.Label
		Me.txtCode = New System.Windows.Forms.TextBox
		Me.txtDescription = New System.Windows.Forms.TextBox
		Me.fraDefaults = New System.Windows.Forms.GroupBox
		Me.chkAutoUpdateBatch = New System.Windows.Forms.CheckBox
		Me.chkActionRequired = New System.Windows.Forms.CheckBox
		Me.cboIncompleteTask = New System.Windows.Forms.ComboBox
		Me.cboCompletionTask = New System.Windows.Forms.ComboBox
		Me.lblAutoBatchUpdate = New System.Windows.Forms.Label
		Me.lblIncompleteTask = New System.Windows.Forms.Label
		Me.lblActionRequired = New System.Windows.Forms.Label
		Me.lblCompletionTask = New System.Windows.Forms.Label
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.fraDefaults.SuspendLayout()
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
		Me.cmdCancel.Location = New System.Drawing.Point(376, 216)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 3
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMainTab
		' 
		Me.tabMainTab.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMainTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
		Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(448, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(453, 212)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 4
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
		Me._tabMainTab_TabPage0.Controls.Add(Me.fraDefaults)
		Me._tabMainTab_TabPage0.Text = "&Details"
		' 
		' lblCode
		' 
		Me.lblCode.AutoSize = True
		Me.lblCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCode.Enabled = True
		Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCode.Location = New System.Drawing.Point(97, 16)
		Me.lblCode.Name = "lblCode"
		Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCode.Size = New System.Drawing.Size(35, 13)
		Me.lblCode.TabIndex = 5
		Me.lblCode.Text = "Code:"
		Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCode.UseMnemonic = True
		Me.lblCode.Visible = True
		' 
		' lblDescription
		' 
		Me.lblDescription.AutoSize = True
		Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDescription.Enabled = True
		Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDescription.Location = New System.Drawing.Point(63, 41)
		Me.lblDescription.Name = "lblDescription"
		Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDescription.Size = New System.Drawing.Size(69, 13)
		Me.lblDescription.TabIndex = 6
		Me.lblDescription.Text = "Description:"
		Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDescription.UseMnemonic = True
		Me.lblDescription.Visible = True
		' 
		' txtCode
		' 
		Me.txtCode.AcceptsReturn = True
		Me.txtCode.AutoSize = False
		Me.txtCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCode.CausesValidation = True
		Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCode.Enabled = True
		Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCode.HideSelection = True
		Me.txtCode.Location = New System.Drawing.Point(144, 12)
		Me.txtCode.MaxLength = 10
		Me.txtCode.Multiline = False
		Me.txtCode.Name = "txtCode"
		Me.txtCode.ReadOnly = False
		Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCode.Size = New System.Drawing.Size(89, 21)
		Me.txtCode.TabIndex = 0
		Me.txtCode.TabStop = True
		Me.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCode.Visible = True
		' 
		' txtDescription
		' 
		Me.txtDescription.AcceptsReturn = True
		Me.txtDescription.AutoSize = False
		Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDescription.CausesValidation = True
		Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDescription.Enabled = True
		Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDescription.HideSelection = True
		Me.txtDescription.Location = New System.Drawing.Point(144, 37)
		Me.txtDescription.MaxLength = 255
		Me.txtDescription.Multiline = False
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.ReadOnly = False
		Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDescription.Size = New System.Drawing.Size(289, 21)
		Me.txtDescription.TabIndex = 1
		Me.txtDescription.TabStop = True
		Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDescription.Visible = True
		' 
		' fraDefaults
		' 
		Me.fraDefaults.BackColor = System.Drawing.SystemColors.Control
		Me.fraDefaults.Controls.Add(Me.chkAutoUpdateBatch)
		Me.fraDefaults.Controls.Add(Me.chkActionRequired)
		Me.fraDefaults.Controls.Add(Me.cboIncompleteTask)
		Me.fraDefaults.Controls.Add(Me.cboCompletionTask)
		Me.fraDefaults.Controls.Add(Me.lblAutoBatchUpdate)
		Me.fraDefaults.Controls.Add(Me.lblIncompleteTask)
		Me.fraDefaults.Controls.Add(Me.lblActionRequired)
		Me.fraDefaults.Controls.Add(Me.lblCompletionTask)
		Me.fraDefaults.Enabled = True
		Me.fraDefaults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraDefaults.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraDefaults.Location = New System.Drawing.Point(8, 60)
		Me.fraDefaults.Name = "fraDefaults"
		Me.fraDefaults.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraDefaults.Size = New System.Drawing.Size(433, 121)
		Me.fraDefaults.TabIndex = 7
		Me.fraDefaults.Text = "Defaults"
		Me.fraDefaults.Visible = True
		' 
		' chkAutoUpdateBatch
		' 
		Me.chkAutoUpdateBatch.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkAutoUpdateBatch.BackColor = System.Drawing.SystemColors.Control
		Me.chkAutoUpdateBatch.CausesValidation = True
		Me.chkAutoUpdateBatch.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkAutoUpdateBatch.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkAutoUpdateBatch.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkAutoUpdateBatch.Enabled = True
		Me.chkAutoUpdateBatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkAutoUpdateBatch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkAutoUpdateBatch.Location = New System.Drawing.Point(136, 96)
		Me.chkAutoUpdateBatch.Name = "chkAutoUpdateBatch"
		Me.chkAutoUpdateBatch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkAutoUpdateBatch.Size = New System.Drawing.Size(21, 21)
		Me.chkAutoUpdateBatch.TabIndex = 14
		Me.chkAutoUpdateBatch.TabStop = True
		Me.chkAutoUpdateBatch.Text = ""
		Me.chkAutoUpdateBatch.Visible = True
		' 
		' chkActionRequired
		' 
		Me.chkActionRequired.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkActionRequired.BackColor = System.Drawing.SystemColors.Control
		Me.chkActionRequired.CausesValidation = True
		Me.chkActionRequired.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkActionRequired.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkActionRequired.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkActionRequired.Enabled = True
		Me.chkActionRequired.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkActionRequired.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkActionRequired.Location = New System.Drawing.Point(136, 72)
		Me.chkActionRequired.Name = "chkActionRequired"
		Me.chkActionRequired.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkActionRequired.Size = New System.Drawing.Size(21, 21)
		Me.chkActionRequired.TabIndex = 10
		Me.chkActionRequired.TabStop = True
		Me.chkActionRequired.Text = ""
		Me.chkActionRequired.Visible = True
		' 
		' cboIncompleteTask
		' 
		Me.cboIncompleteTask.BackColor = System.Drawing.SystemColors.Window
		Me.cboIncompleteTask.CausesValidation = True
		Me.cboIncompleteTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboIncompleteTask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboIncompleteTask.Enabled = True
		Me.cboIncompleteTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboIncompleteTask.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboIncompleteTask.IntegralHeight = True
		Me.cboIncompleteTask.Location = New System.Drawing.Point(136, 44)
		Me.cboIncompleteTask.Name = "cboIncompleteTask"
		Me.cboIncompleteTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboIncompleteTask.Size = New System.Drawing.Size(265, 21)
		Me.cboIncompleteTask.Sorted = False
		Me.cboIncompleteTask.TabIndex = 9
		Me.cboIncompleteTask.TabStop = True
		Me.cboIncompleteTask.Visible = True
		' 
		' cboCompletionTask
		' 
		Me.cboCompletionTask.BackColor = System.Drawing.SystemColors.Window
		Me.cboCompletionTask.CausesValidation = True
		Me.cboCompletionTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboCompletionTask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboCompletionTask.Enabled = True
		Me.cboCompletionTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboCompletionTask.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboCompletionTask.IntegralHeight = True
		Me.cboCompletionTask.Location = New System.Drawing.Point(136, 16)
		Me.cboCompletionTask.Name = "cboCompletionTask"
		Me.cboCompletionTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboCompletionTask.Size = New System.Drawing.Size(265, 21)
		Me.cboCompletionTask.Sorted = False
		Me.cboCompletionTask.TabIndex = 8
		Me.cboCompletionTask.TabStop = True
		Me.cboCompletionTask.Visible = True
		' 
		' lblAutoBatchUpdate
		' 
		Me.lblAutoBatchUpdate.AutoSize = True
		Me.lblAutoBatchUpdate.BackColor = System.Drawing.SystemColors.Control
		Me.lblAutoBatchUpdate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAutoBatchUpdate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAutoBatchUpdate.Enabled = True
		Me.lblAutoBatchUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAutoBatchUpdate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAutoBatchUpdate.Location = New System.Drawing.Point(13, 100)
		Me.lblAutoBatchUpdate.Name = "lblAutoBatchUpdate"
		Me.lblAutoBatchUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAutoBatchUpdate.Size = New System.Drawing.Size(111, 13)
		Me.lblAutoBatchUpdate.TabIndex = 15
		Me.lblAutoBatchUpdate.Text = "Auto Batch Update:"
		Me.lblAutoBatchUpdate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAutoBatchUpdate.UseMnemonic = True
		Me.lblAutoBatchUpdate.Visible = True
		' 
		' lblIncompleteTask
		' 
		Me.lblIncompleteTask.AutoSize = True
		Me.lblIncompleteTask.BackColor = System.Drawing.SystemColors.Control
		Me.lblIncompleteTask.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblIncompleteTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblIncompleteTask.Enabled = True
		Me.lblIncompleteTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblIncompleteTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblIncompleteTask.Location = New System.Drawing.Point(24, 48)
		Me.lblIncompleteTask.Name = "lblIncompleteTask"
		Me.lblIncompleteTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblIncompleteTask.Size = New System.Drawing.Size(100, 13)
		Me.lblIncompleteTask.TabIndex = 13
		Me.lblIncompleteTask.Text = "Incomplete Task:"
		Me.lblIncompleteTask.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblIncompleteTask.UseMnemonic = True
		Me.lblIncompleteTask.Visible = True
		' 
		' lblActionRequired
		' 
		Me.lblActionRequired.AutoSize = True
		Me.lblActionRequired.BackColor = System.Drawing.SystemColors.Control
		Me.lblActionRequired.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblActionRequired.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblActionRequired.Enabled = True
		Me.lblActionRequired.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblActionRequired.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblActionRequired.Location = New System.Drawing.Point(29, 76)
		Me.lblActionRequired.Name = "lblActionRequired"
		Me.lblActionRequired.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblActionRequired.Size = New System.Drawing.Size(95, 13)
		Me.lblActionRequired.TabIndex = 12
		Me.lblActionRequired.Text = "Action Required:"
		Me.lblActionRequired.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblActionRequired.UseMnemonic = True
		Me.lblActionRequired.Visible = True
		' 
		' lblCompletionTask
		' 
		Me.lblCompletionTask.AutoSize = True
		Me.lblCompletionTask.BackColor = System.Drawing.SystemColors.Control
		Me.lblCompletionTask.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCompletionTask.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCompletionTask.Enabled = True
		Me.lblCompletionTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCompletionTask.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCompletionTask.Location = New System.Drawing.Point(23, 20)
		Me.lblCompletionTask.Name = "lblCompletionTask"
		Me.lblCompletionTask.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCompletionTask.Size = New System.Drawing.Size(101, 13)
		Me.lblCompletionTask.TabIndex = 11
		Me.lblCompletionTask.Text = "Completion Task:"
		Me.lblCompletionTask.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCompletionTask.UseMnemonic = True
		Me.lblCompletionTask.Visible = True
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(296, 216)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmDetails
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(455, 242)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.tabMainTab)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmDetails.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDetails"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Edit Action Type"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.fraDefaults.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class