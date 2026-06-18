<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRates
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
	Public WithEvents cmdSave As System.Windows.Forms.Button
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents lblZ As System.Windows.Forms.Label
	Public WithEvents lblY As System.Windows.Forms.Label
	Public WithEvents lblX As System.Windows.Forms.Label
	Public WithEvents cboZ As System.Windows.Forms.ComboBox
	Public WithEvents cboY As System.Windows.Forms.ComboBox
	Public WithEvents cboX As System.Windows.Forms.ComboBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Private WithEvents TDBGrid As Artinsoft.Windows.Forms.ExtendedDataGridView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRates))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdSave = New System.Windows.Forms.Button
		Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblZ = New System.Windows.Forms.Label
		Me.lblY = New System.Windows.Forms.Label
		Me.lblX = New System.Windows.Forms.Label
		Me.cboZ = New System.Windows.Forms.ComboBox
		Me.cboY = New System.Windows.Forms.ComboBox
		Me.cboX = New System.Windows.Forms.ComboBox
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		Me.TDBGrid = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
		CType(Me.TDBGrid, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(424, 384)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 11
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdSave
		' 
		Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSave.CausesValidation = True
		Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSave.Enabled = True
		Me.cmdSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSave.Location = New System.Drawing.Point(264, 384)
		Me.cmdSave.Name = "cmdSave"
		Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSave.Size = New System.Drawing.Size(73, 22)
		Me.cmdSave.TabIndex = 6
		Me.cmdSave.TabStop = True
		Me.cmdSave.Text = "&Apply"
		Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' uctPMResizer1
		' 
		Me.uctPMResizer1.Location = New System.Drawing.Point(8, 376)
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
		Me.cmdOK.Location = New System.Drawing.Point(344, 384)
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
		Me.cmdHelp.Location = New System.Drawing.Point(504, 384)
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
		Me.tabMain.HotTrack = False
		Me.tabMain.ItemSize = New System.Drawing.Size(188, 18)
		Me.tabMain.Location = New System.Drawing.Point(8, 8)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(573, 373)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 2
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.lblZ)
		Me._tabMain_TabPage0.Controls.Add(Me.lblY)
		Me._tabMain_TabPage0.Controls.Add(Me.lblX)
		Me._tabMain_TabPage0.Controls.Add(Me.TDBGrid)
		Me._tabMain_TabPage0.Controls.Add(Me.cboZ)
		Me._tabMain_TabPage0.Controls.Add(Me.cboY)
		Me._tabMain_TabPage0.Controls.Add(Me.cboX)
		Me._tabMain_TabPage0.Text = "&1 - Rates"
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
		Me.lblZ.Location = New System.Drawing.Point(248, 60)
		Me.lblZ.Name = "lblZ"
		Me.lblZ.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblZ.Size = New System.Drawing.Size(45, 13)
		Me.lblZ.TabIndex = 5
		Me.lblZ.Text = "Z-Axis"
		Me.lblZ.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblZ.UseMnemonic = True
		Me.lblZ.Visible = True
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
		Me.lblY.Location = New System.Drawing.Point(264, 36)
		Me.lblY.Name = "lblY"
		Me.lblY.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblY.Size = New System.Drawing.Size(29, 13)
		Me.lblY.TabIndex = 9
		Me.lblY.Text = "Y-Axis"
		Me.lblY.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblY.UseMnemonic = True
		Me.lblY.Visible = True
		' 
		' lblX
		' 
		Me.lblX.AutoSize = True
		Me.lblX.BackColor = System.Drawing.SystemColors.Control
		Me.lblX.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblX.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblX.Enabled = True
		Me.lblX.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblX.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblX.Location = New System.Drawing.Point(264, 12)
		Me.lblX.Name = "lblX"
		Me.lblX.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblX.Size = New System.Drawing.Size(29, 13)
		Me.lblX.TabIndex = 10
		Me.lblX.Text = "X-Axis"
		Me.lblX.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblX.UseMnemonic = True
		Me.lblX.Visible = True
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
		Me.cboZ.Location = New System.Drawing.Point(304, 60)
		Me.cboZ.Name = "cboZ"
		Me.cboZ.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboZ.Size = New System.Drawing.Size(249, 21)
		Me.cboZ.Sorted = False
		Me.cboZ.TabIndex = 4
		Me.cboZ.TabStop = True
		Me.cboZ.Visible = True
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
		Me.cboY.Location = New System.Drawing.Point(304, 36)
		Me.cboY.Name = "cboY"
		Me.cboY.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboY.Size = New System.Drawing.Size(249, 21)
		Me.cboY.Sorted = False
		Me.cboY.TabIndex = 7
		Me.cboY.TabStop = True
		Me.cboY.Visible = True
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
		Me.cboX.Location = New System.Drawing.Point(304, 12)
		Me.cboX.Name = "cboX"
		Me.cboX.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboX.Size = New System.Drawing.Size(249, 21)
		Me.cboX.Sorted = False
		Me.cboX.TabIndex = 8
		Me.cboX.TabStop = True
		Me.cboX.Visible = True
		' 
		' frmRates
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(584, 412)
		Me.TDBGrid.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdSave)
		Me.Controls.Add(Me.uctPMResizer1)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.TDBGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmRates.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.TDBGrid.Location = New System.Drawing.Point(16, 92)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmRates"
		Me.TDBGrid.Name = "TDBGrid"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.TDBGrid.Size = New System.Drawing.Size(537, 241)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.TDBGrid.TabIndex = 3
		Me.Text = "Rates"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		CType(Me.TDBGrid, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class