<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAxis
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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents cboX As System.Windows.Forms.ComboBox
	Public WithEvents cboY As System.Windows.Forms.ComboBox
	Public WithEvents cboZ As System.Windows.Forms.ComboBox
	Public WithEvents lblX As System.Windows.Forms.Label
	Public WithEvents lblY As System.Windows.Forms.Label
	Public WithEvents lblZ As System.Windows.Forms.Label
	Public WithEvents fraDimensions As System.Windows.Forms.GroupBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAxis))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblDescription = New System.Windows.Forms.Label
		Me.fraDimensions = New System.Windows.Forms.GroupBox
		Me.cboX = New System.Windows.Forms.ComboBox
		Me.cboY = New System.Windows.Forms.ComboBox
		Me.cboZ = New System.Windows.Forms.ComboBox
		Me.lblX = New System.Windows.Forms.Label
		Me.lblY = New System.Windows.Forms.Label
		Me.lblZ = New System.Windows.Forms.Label
		Me.txtDescription = New System.Windows.Forms.TextBox
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.fraDimensions.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(136, 296)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 2
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
		Me.cmdHelp.Location = New System.Drawing.Point(296, 296)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 1
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
		Me.cmdCancel.Location = New System.Drawing.Point(216, 296)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 0
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMain
		' 
		Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
		Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.ItemSize = New System.Drawing.Size(119, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 8)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(365, 285)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 3
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.lblDescription)
		Me._tabMain_TabPage0.Controls.Add(Me.fraDimensions)
		Me._tabMain_TabPage0.Controls.Add(Me.txtDescription)
		Me._tabMain_TabPage0.Text = "&1 - General"
		' 
		' lblDescription
		' 
		Me.lblDescription.AutoSize = True
		Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDescription.Enabled = True
		Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDescription.Location = New System.Drawing.Point(8, 23)
		Me.lblDescription.Name = "lblDescription"
		Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDescription.Size = New System.Drawing.Size(69, 13)
		Me.lblDescription.TabIndex = 12
		Me.lblDescription.Text = "Description:"
		Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDescription.UseMnemonic = True
		Me.lblDescription.Visible = True
		' 
		' fraDimensions
		' 
		Me.fraDimensions.BackColor = System.Drawing.SystemColors.Control
		Me.fraDimensions.Controls.Add(Me.cboX)
		Me.fraDimensions.Controls.Add(Me.cboY)
		Me.fraDimensions.Controls.Add(Me.cboZ)
		Me.fraDimensions.Controls.Add(Me.lblX)
		Me.fraDimensions.Controls.Add(Me.lblY)
		Me.fraDimensions.Controls.Add(Me.lblZ)
		Me.fraDimensions.Enabled = True
		Me.fraDimensions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraDimensions.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraDimensions.Location = New System.Drawing.Point(16, 60)
		Me.fraDimensions.Name = "fraDimensions"
		Me.fraDimensions.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraDimensions.Size = New System.Drawing.Size(329, 145)
		Me.fraDimensions.TabIndex = 4
		Me.fraDimensions.Text = "Dimensions"
		Me.fraDimensions.Visible = True
		' 
		' cboX
		' 
		Me.cboX.BackColor = System.Drawing.SystemColors.Window
		Me.cboX.CausesValidation = True
		Me.cboX.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboX.Enabled = True
		Me.cboX.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboX.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboX.IntegralHeight = True
		Me.cboX.Location = New System.Drawing.Point(64, 24)
		Me.cboX.Name = "cboX"
		Me.cboX.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboX.Size = New System.Drawing.Size(249, 21)
		Me.cboX.Sorted = False
		Me.cboX.TabIndex = 7
		Me.cboX.TabStop = True
		Me.cboX.Visible = True
		' 
		' cboY
		' 
		Me.cboY.BackColor = System.Drawing.SystemColors.Window
		Me.cboY.CausesValidation = True
		Me.cboY.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboY.Enabled = True
		Me.cboY.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboY.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboY.IntegralHeight = True
		Me.cboY.Location = New System.Drawing.Point(64, 56)
		Me.cboY.Name = "cboY"
		Me.cboY.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboY.Size = New System.Drawing.Size(249, 21)
		Me.cboY.Sorted = False
		Me.cboY.TabIndex = 6
		Me.cboY.TabStop = True
		Me.cboY.Visible = True
		' 
		' cboZ
		' 
		Me.cboZ.BackColor = System.Drawing.SystemColors.Window
		Me.cboZ.CausesValidation = True
		Me.cboZ.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboZ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboZ.Enabled = True
		Me.cboZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboZ.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboZ.IntegralHeight = True
		Me.cboZ.Location = New System.Drawing.Point(64, 88)
		Me.cboZ.Name = "cboZ"
		Me.cboZ.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboZ.Size = New System.Drawing.Size(249, 21)
		Me.cboZ.Sorted = False
		Me.cboZ.TabIndex = 5
		Me.cboZ.TabStop = True
		Me.cboZ.Visible = True
		' 
		' lblX
		' 
		Me.lblX.AutoSize = True
		Me.lblX.BackColor = System.Drawing.SystemColors.Control
		Me.lblX.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblX.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblX.Enabled = True
		Me.lblX.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblX.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblX.Location = New System.Drawing.Point(16, 28)
		Me.lblX.Name = "lblX"
		Me.lblX.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblX.Size = New System.Drawing.Size(36, 13)
		Me.lblX.TabIndex = 10
		Me.lblX.Text = "X-Axis"
		Me.lblX.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblX.UseMnemonic = True
		Me.lblX.Visible = True
		' 
		' lblY
		' 
		Me.lblY.AutoSize = True
		Me.lblY.BackColor = System.Drawing.SystemColors.Control
		Me.lblY.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblY.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblY.Enabled = True
		Me.lblY.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblY.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblY.Location = New System.Drawing.Point(16, 60)
		Me.lblY.Name = "lblY"
		Me.lblY.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblY.Size = New System.Drawing.Size(29, 13)
		Me.lblY.TabIndex = 9
		Me.lblY.Text = "Y-Axis"
		Me.lblY.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblY.UseMnemonic = True
		Me.lblY.Visible = True
		' 
		' lblZ
		' 
		Me.lblZ.AutoSize = True
		Me.lblZ.BackColor = System.Drawing.SystemColors.Control
		Me.lblZ.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblZ.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblZ.Enabled = True
		Me.lblZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblZ.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblZ.Location = New System.Drawing.Point(16, 92)
		Me.lblZ.Name = "lblZ"
		Me.lblZ.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblZ.Size = New System.Drawing.Size(29, 13)
		Me.lblZ.TabIndex = 8
		Me.lblZ.Text = "Z-Axis"
		Me.lblZ.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblZ.UseMnemonic = True
		Me.lblZ.Visible = True
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
		Me.txtDescription.Location = New System.Drawing.Point(80, 20)
		Me.txtDescription.MaxLength = 0
		Me.txtDescription.Multiline = False
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.ReadOnly = False
		Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDescription.Size = New System.Drawing.Size(249, 19)
		Me.txtDescription.TabIndex = 11
		Me.txtDescription.TabStop = True
		Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDescription.Visible = True
		' 
		' frmAxis
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(375, 324)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmAxis.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmAxis"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Define Axes"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.fraDimensions.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class