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
	Public WithEvents cmdOutcomes As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents lblOutcome As System.Windows.Forms.Label
	Public WithEvents lblDocumentTemplateCode As System.Windows.Forms.Label
	Public WithEvents lblDueDays As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents txtCode As System.Windows.Forms.TextBox
	Public WithEvents txtDueDays As System.Windows.Forms.TextBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents chkOutcomeEditable As System.Windows.Forms.CheckBox
	Public WithEvents txtDocTemplate As System.Windows.Forms.TextBox
	Public WithEvents cmdFindDocTemplate As System.Windows.Forms.Button
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
		Me.cmdOutcomes = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.tabMainTab = New System.Windows.Forms.TabControl
		Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblCode = New System.Windows.Forms.Label
		Me.lblOutcome = New System.Windows.Forms.Label
		Me.lblDocumentTemplateCode = New System.Windows.Forms.Label
		Me.lblDueDays = New System.Windows.Forms.Label
		Me.lblEffectiveDate = New System.Windows.Forms.Label
		Me.lblDescription = New System.Windows.Forms.Label
		Me.txtCode = New System.Windows.Forms.TextBox
		Me.txtDueDays = New System.Windows.Forms.TextBox
		Me.txtEffectiveDate = New System.Windows.Forms.TextBox
		Me.txtDescription = New System.Windows.Forms.TextBox
		Me.chkOutcomeEditable = New System.Windows.Forms.CheckBox
		Me.txtDocTemplate = New System.Windows.Forms.TextBox
		Me.cmdFindDocTemplate = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMainTab.SuspendLayout()
		Me._tabMainTab_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdOutcomes
		' 
		Me.cmdOutcomes.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOutcomes.CausesValidation = True
		Me.cmdOutcomes.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOutcomes.Enabled = True
		Me.cmdOutcomes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOutcomes.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOutcomes.Location = New System.Drawing.Point(8, 200)
		Me.cmdOutcomes.Name = "cmdOutcomes"
		Me.cmdOutcomes.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOutcomes.Size = New System.Drawing.Size(73, 22)
		Me.cmdOutcomes.TabIndex = 7
		Me.cmdOutcomes.TabStop = True
		Me.cmdOutcomes.Text = "Ou&tcomes"
		Me.cmdOutcomes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOutcomes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(424, 200)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 9
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
		Me.tabMainTab.ItemSize = New System.Drawing.Size(496, 18)
		Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
		Me.tabMainTab.Multiline = True
		Me.tabMainTab.Name = "tabMainTab"
		Me.tabMainTab.Size = New System.Drawing.Size(501, 196)
		Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMainTab.TabIndex = 10
		Me.tabMainTab.TabStop = False
		' 
		' _tabMainTab_TabPage0
		' 
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblOutcome)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDocumentTemplateCode)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDueDays)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDueDays)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
		Me._tabMainTab_TabPage0.Controls.Add(Me.chkOutcomeEditable)
		Me._tabMainTab_TabPage0.Controls.Add(Me.txtDocTemplate)
		Me._tabMainTab_TabPage0.Controls.Add(Me.cmdFindDocTemplate)
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
		Me.lblCode.Location = New System.Drawing.Point(90, 16)
		Me.lblCode.Name = "lblCode"
		Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCode.Size = New System.Drawing.Size(35, 13)
		Me.lblCode.TabIndex = 11
		Me.lblCode.Text = "Code:"
		Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCode.UseMnemonic = True
		Me.lblCode.Visible = True
		' 
		' lblOutcome
		' 
		Me.lblOutcome.AutoSize = True
		Me.lblOutcome.BackColor = System.Drawing.SystemColors.Control
		Me.lblOutcome.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOutcome.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOutcome.Enabled = True
		Me.lblOutcome.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblOutcome.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOutcome.Location = New System.Drawing.Point(20, 115)
		Me.lblOutcome.Name = "lblOutcome"
		Me.lblOutcome.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOutcome.Size = New System.Drawing.Size(105, 13)
		Me.lblOutcome.TabIndex = 12
		Me.lblOutcome.Text = "Outcome Editable:"
		Me.lblOutcome.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOutcome.UseMnemonic = True
		Me.lblOutcome.Visible = True
		' 
		' lblDocumentTemplateCode
		' 
		Me.lblDocumentTemplateCode.AutoSize = True
		Me.lblDocumentTemplateCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocumentTemplateCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocumentTemplateCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocumentTemplateCode.Enabled = True
		Me.lblDocumentTemplateCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocumentTemplateCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocumentTemplateCode.Location = New System.Drawing.Point(67, 88)
		Me.lblDocumentTemplateCode.Name = "lblDocumentTemplateCode"
		Me.lblDocumentTemplateCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocumentTemplateCode.Size = New System.Drawing.Size(58, 13)
		Me.lblDocumentTemplateCode.TabIndex = 13
		Me.lblDocumentTemplateCode.Text = "Template:"
		Me.lblDocumentTemplateCode.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblDocumentTemplateCode.UseMnemonic = True
		Me.lblDocumentTemplateCode.Visible = True
		' 
		' lblDueDays
		' 
		Me.lblDueDays.AutoSize = True
		Me.lblDueDays.BackColor = System.Drawing.SystemColors.Control
		Me.lblDueDays.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDueDays.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDueDays.Enabled = True
		Me.lblDueDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDueDays.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDueDays.Location = New System.Drawing.Point(64, 64)
		Me.lblDueDays.Name = "lblDueDays"
		Me.lblDueDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDueDays.Size = New System.Drawing.Size(61, 13)
		Me.lblDueDays.TabIndex = 14
		Me.lblDueDays.Text = "Due Days:"
		Me.lblDueDays.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDueDays.UseMnemonic = True
		Me.lblDueDays.Visible = True
		' 
		' lblEffectiveDate
		' 
		Me.lblEffectiveDate.AutoSize = True
		Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEffectiveDate.Enabled = True
		Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEffectiveDate.Location = New System.Drawing.Point(40, 139)
		Me.lblEffectiveDate.Name = "lblEffectiveDate"
		Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEffectiveDate.Size = New System.Drawing.Size(85, 13)
		Me.lblEffectiveDate.TabIndex = 15
		Me.lblEffectiveDate.Text = "Effective Date:"
		Me.lblEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblEffectiveDate.UseMnemonic = True
		Me.lblEffectiveDate.Visible = True
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
		Me.lblDescription.Location = New System.Drawing.Point(56, 41)
		Me.lblDescription.Name = "lblDescription"
		Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDescription.Size = New System.Drawing.Size(69, 13)
		Me.lblDescription.TabIndex = 16
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
		' txtDueDays
		' 
		Me.txtDueDays.AcceptsReturn = True
		Me.txtDueDays.AutoSize = False
		Me.txtDueDays.BackColor = System.Drawing.SystemColors.Window
		Me.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDueDays.CausesValidation = True
		Me.txtDueDays.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDueDays.Enabled = True
		Me.txtDueDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDueDays.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDueDays.HideSelection = True
		Me.txtDueDays.Location = New System.Drawing.Point(144, 60)
		Me.txtDueDays.MaxLength = 9
		Me.txtDueDays.Multiline = False
		Me.txtDueDays.Name = "txtDueDays"
		Me.txtDueDays.ReadOnly = False
		Me.txtDueDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDueDays.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDueDays.Size = New System.Drawing.Size(89, 21)
		Me.txtDueDays.TabIndex = 2
		Me.txtDueDays.TabStop = True
		Me.txtDueDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDueDays.Visible = True
		' 
		' txtEffectiveDate
		' 
		Me.txtEffectiveDate.AcceptsReturn = True
		Me.txtEffectiveDate.AutoSize = False
		Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEffectiveDate.CausesValidation = True
		Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEffectiveDate.Enabled = True
		Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEffectiveDate.HideSelection = True
		Me.txtEffectiveDate.Location = New System.Drawing.Point(144, 135)
		Me.txtEffectiveDate.MaxLength = 0
		Me.txtEffectiveDate.Multiline = False
		Me.txtEffectiveDate.Name = "txtEffectiveDate"
		Me.txtEffectiveDate.ReadOnly = False
		Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEffectiveDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEffectiveDate.Size = New System.Drawing.Size(89, 21)
		Me.txtEffectiveDate.TabIndex = 6
		Me.txtEffectiveDate.TabStop = True
		Me.txtEffectiveDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEffectiveDate.Visible = True
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
		' chkOutcomeEditable
		' 
		Me.chkOutcomeEditable.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkOutcomeEditable.BackColor = System.Drawing.SystemColors.Control
		Me.chkOutcomeEditable.CausesValidation = True
		Me.chkOutcomeEditable.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkOutcomeEditable.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkOutcomeEditable.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkOutcomeEditable.Enabled = True
		Me.chkOutcomeEditable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkOutcomeEditable.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkOutcomeEditable.Location = New System.Drawing.Point(144, 111)
		Me.chkOutcomeEditable.Name = "chkOutcomeEditable"
		Me.chkOutcomeEditable.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkOutcomeEditable.Size = New System.Drawing.Size(17, 21)
		Me.chkOutcomeEditable.TabIndex = 5
		Me.chkOutcomeEditable.TabStop = True
		Me.chkOutcomeEditable.Text = "Check1"
		Me.chkOutcomeEditable.Visible = True
		' 
		' txtDocTemplate
		' 
		Me.txtDocTemplate.AcceptsReturn = True
		Me.txtDocTemplate.AutoSize = False
		Me.txtDocTemplate.BackColor = System.Drawing.SystemColors.Window
		Me.txtDocTemplate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDocTemplate.CausesValidation = True
		Me.txtDocTemplate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDocTemplate.Enabled = False
		Me.txtDocTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDocTemplate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDocTemplate.HideSelection = True
		Me.txtDocTemplate.Location = New System.Drawing.Point(144, 84)
		Me.txtDocTemplate.MaxLength = 10
		Me.txtDocTemplate.Multiline = False
		Me.txtDocTemplate.Name = "txtDocTemplate"
		Me.txtDocTemplate.ReadOnly = False
		Me.txtDocTemplate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDocTemplate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDocTemplate.Size = New System.Drawing.Size(190, 21)
		Me.txtDocTemplate.TabIndex = 3
		Me.txtDocTemplate.TabStop = True
		Me.txtDocTemplate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDocTemplate.Visible = True
		' 
		' cmdFindDocTemplate
		' 
		Me.cmdFindDocTemplate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFindDocTemplate.CausesValidation = True
		Me.cmdFindDocTemplate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFindDocTemplate.Enabled = True
		Me.cmdFindDocTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFindDocTemplate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFindDocTemplate.Location = New System.Drawing.Point(336, 84)
		Me.cmdFindDocTemplate.Name = "cmdFindDocTemplate"
		Me.cmdFindDocTemplate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFindDocTemplate.Size = New System.Drawing.Size(21, 21)
		Me.cmdFindDocTemplate.TabIndex = 4
		Me.cmdFindDocTemplate.TabStop = True
		Me.cmdFindDocTemplate.Text = "..."
		Me.cmdFindDocTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFindDocTemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(344, 200)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 8
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmDetails
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(502, 228)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOutcomes)
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
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class