<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgImage As System.Windows.Forms.PictureBox
	Public WithEvents lblShortName As System.Windows.Forms.Label
	Public WithEvents lblName As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblMapping As System.Windows.Forms.Label
	Public WithEvents txtLedgerName As System.Windows.Forms.TextBox
	Public WithEvents txtLedgerShortName As System.Windows.Forms.TextBox
	Public WithEvents cmbLedgerType As System.Windows.Forms.ComboBox
	Public WithEvents cmbMapping As System.Windows.Forms.ComboBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
		Me.dlgHelpFont = New System.Windows.Forms.FontDialog
		Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
		Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
		Me.cmdNavigate = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.imgImage = New System.Windows.Forms.PictureBox
		Me.lblShortName = New System.Windows.Forms.Label
		Me.lblName = New System.Windows.Forms.Label
		Me.lblType = New System.Windows.Forms.Label
		Me.lblMapping = New System.Windows.Forms.Label
		Me.txtLedgerName = New System.Windows.Forms.TextBox
		Me.txtLedgerShortName = New System.Windows.Forms.TextBox
		Me.cmbLedgerType = New System.Windows.Forms.ComboBox
		Me.cmbMapping = New System.Windows.Forms.ComboBox
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdNavigate
		' 
		Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNavigate.CausesValidation = True
		Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNavigate.Enabled = True
		Me.cmdNavigate.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNavigate.Location = New System.Drawing.Point(8, 214)
		Me.cmdNavigate.Name = "cmdNavigate"
		Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
		Me.cmdNavigate.TabIndex = 6
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
		Me.cmdHelp.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(280, 214)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 5
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
		Me.cmdCancel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(200, 214)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 4
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
		Me.cmdOK.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(120, 214)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 3
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
		Me.tabMainTab.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMainTab.ItemSize = New System.Drawing.Size(68, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(349, 203)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 7
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.imgImage)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblShortName)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblName)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblMapping)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtLedgerName)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtLedgerShortName)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmbLedgerType)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmbMapping)
		Me._tabMainTab_TabPage0.Text = "&1 - Details"
		' 
		' imgImage
		' 
		Me.imgImage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgImage.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgImage.Enabled = True
		Me.imgImage.Image = CType(resources.GetObject("imgImage.Image"), System.Drawing.Image)
		Me.imgImage.Location = New System.Drawing.Point(300, 8)
		Me.imgImage.Name = "imgImage"
		Me.imgImage.Size = New System.Drawing.Size(32, 32)
		Me.imgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgImage.Visible = True
		' 
		' lblShortName
		' 
		Me.lblShortName.AutoSize = False
		Me.lblShortName.BackColor = System.Drawing.SystemColors.Control
		Me.lblShortName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblShortName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblShortName.Enabled = True
		Me.lblShortName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblShortName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblShortName.Location = New System.Drawing.Point(16, 23)
		Me.lblShortName.Name = "lblShortName"
		Me.lblShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblShortName.Size = New System.Drawing.Size(89, 17)
		Me.lblShortName.TabIndex = 8
		Me.lblShortName.Text = "Short Name:"
		Me.lblShortName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblShortName.UseMnemonic = True
		Me.lblShortName.Visible = True
		' 
		' lblName
		' 
		Me.lblName.AutoSize = False
		Me.lblName.BackColor = System.Drawing.SystemColors.Control
		Me.lblName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblName.Enabled = True
		Me.lblName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblName.Location = New System.Drawing.Point(16, 63)
		Me.lblName.Name = "lblName"
		Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblName.Size = New System.Drawing.Size(89, 17)
		Me.lblName.TabIndex = 9
		Me.lblName.Text = "Name:"
		Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblName.UseMnemonic = True
		Me.lblName.Visible = True
		' 
		' lblType
		' 
		Me.lblType.AutoSize = False
		Me.lblType.BackColor = System.Drawing.SystemColors.Control
		Me.lblType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblType.Enabled = True
		Me.lblType.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblType.Location = New System.Drawing.Point(16, 103)
		Me.lblType.Name = "lblType"
		Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblType.Size = New System.Drawing.Size(89, 17)
		Me.lblType.TabIndex = 10
		Me.lblType.Text = "Type:"
		Me.lblType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblType.UseMnemonic = True
		Me.lblType.Visible = True
		' 
		' lblMapping
		' 
		Me.lblMapping.AutoSize = False
		Me.lblMapping.BackColor = System.Drawing.SystemColors.Control
		Me.lblMapping.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMapping.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMapping.Enabled = True
		Me.lblMapping.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblMapping.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMapping.Location = New System.Drawing.Point(16, 143)
		Me.lblMapping.Name = "lblMapping"
		Me.lblMapping.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMapping.Size = New System.Drawing.Size(89, 17)
		Me.lblMapping.TabIndex = 12
		Me.lblMapping.Text = "Account Mapping:"
		Me.lblMapping.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMapping.UseMnemonic = True
		Me.lblMapping.Visible = True
		' 
		' txtLedgerName
		' 
		Me.txtLedgerName.AcceptsReturn = True
		Me.txtLedgerName.AutoSize = False
		Me.txtLedgerName.BackColor = System.Drawing.SystemColors.Window
		Me.txtLedgerName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLedgerName.CausesValidation = True
		Me.txtLedgerName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLedgerName.Enabled = True
		Me.txtLedgerName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLedgerName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLedgerName.HideSelection = True
		Me.txtLedgerName.Location = New System.Drawing.Point(128, 60)
		Me.txtLedgerName.MaxLength = 0
		Me.txtLedgerName.Multiline = False
		Me.txtLedgerName.Name = "txtLedgerName"
		Me.txtLedgerName.ReadOnly = False
		Me.txtLedgerName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLedgerName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLedgerName.Size = New System.Drawing.Size(153, 19)
		Me.txtLedgerName.TabIndex = 1
		Me.txtLedgerName.TabStop = True
		Me.txtLedgerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLedgerName.Visible = True
		' 
		' txtLedgerShortName
		' 
		Me.txtLedgerShortName.AcceptsReturn = True
		Me.txtLedgerShortName.AutoSize = False
		Me.txtLedgerShortName.BackColor = System.Drawing.SystemColors.Window
		Me.txtLedgerShortName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLedgerShortName.CausesValidation = True
		Me.txtLedgerShortName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLedgerShortName.Enabled = True
		Me.txtLedgerShortName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtLedgerShortName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLedgerShortName.HideSelection = True
		Me.txtLedgerShortName.Location = New System.Drawing.Point(128, 20)
		Me.txtLedgerShortName.MaxLength = 0
		Me.txtLedgerShortName.Multiline = False
		Me.txtLedgerShortName.Name = "txtLedgerShortName"
		Me.txtLedgerShortName.ReadOnly = False
		Me.txtLedgerShortName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLedgerShortName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLedgerShortName.Size = New System.Drawing.Size(25, 19)
		Me.txtLedgerShortName.TabIndex = 0
		Me.txtLedgerShortName.TabStop = True
		Me.txtLedgerShortName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLedgerShortName.Visible = True
		' 
		' cmbLedgerType
		' 
		Me.cmbLedgerType.BackColor = System.Drawing.SystemColors.Window
		Me.cmbLedgerType.CausesValidation = True
		Me.cmbLedgerType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbLedgerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cmbLedgerType.Enabled = True
		Me.cmbLedgerType.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbLedgerType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbLedgerType.IntegralHeight = True
		Me.cmbLedgerType.Location = New System.Drawing.Point(128, 100)
		Me.cmbLedgerType.Name = "cmbLedgerType"
		Me.cmbLedgerType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbLedgerType.Size = New System.Drawing.Size(153, 21)
		Me.cmbLedgerType.Sorted = False
		Me.cmbLedgerType.TabIndex = 2
		Me.cmbLedgerType.TabStop = True
		Me.cmbLedgerType.Visible = True
		' 
		' cmbMapping
		' 
		Me.cmbMapping.BackColor = System.Drawing.SystemColors.Window
		Me.cmbMapping.CausesValidation = True
		Me.cmbMapping.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbMapping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cmbMapping.Enabled = True
		Me.cmbMapping.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbMapping.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbMapping.IntegralHeight = True
		Me.cmbMapping.Location = New System.Drawing.Point(128, 140)
		Me.cmbMapping.Name = "cmbMapping"
		Me.cmbMapping.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbMapping.Size = New System.Drawing.Size(153, 21)
		Me.cmbMapping.Sorted = False
		Me.cmbMapping.TabIndex = 11
		Me.cmbMapping.TabStop = True
		Me.cmbMapping.Visible = True
		' 
		' frmDetails
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(361, 244)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdNavigate)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMainTab)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmDetails.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDetails"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Ledger"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMainTab, 1)
		Me.tabMainTab.ResumeLayout(False)
		Me._tabMainTab_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class