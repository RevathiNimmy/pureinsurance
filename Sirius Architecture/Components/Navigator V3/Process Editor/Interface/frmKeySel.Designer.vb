<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmKeySel
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents mnuFileNew As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents chkEnforce As System.Windows.Forms.CheckBox
	Public WithEvents cmdApplyGSK As System.Windows.Forms.Button
	Public WithEvents txtGSKDescription As System.Windows.Forms.TextBox
	Public WithEvents chkIsOptional As System.Windows.Forms.CheckBox
	Public WithEvents txtInitialKeyValue As System.Windows.Forms.TextBox
	Public WithEvents lblGSKDescription As System.Windows.Forms.Label
	Public WithEvents lblInitialValue As System.Windows.Forms.Label
	Public WithEvents fraGSKDetails As System.Windows.Forms.GroupBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents cboDataType As System.Windows.Forms.ComboBox
	Public WithEvents lblName As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents fraKeyDetails As System.Windows.Forms.GroupBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdGKRemove As System.Windows.Forms.Button
	Public WithEvents cmdGKAdd As System.Windows.Forms.Button
	Public WithEvents lstGKRequiredKeys As System.Windows.Forms.ListBox
	Public WithEvents lstSKRequiredKeys As System.Windows.Forms.ListBox
	Public WithEvents cmdSKRemove As System.Windows.Forms.Button
	Public WithEvents cmdSKAdd As System.Windows.Forms.Button
	Public WithEvents lstSystemKeys As System.Windows.Forms.ListBox
	Public WithEvents lblAllKeys As System.Windows.Forms.Label
	Public WithEvents lblSetKeys As System.Windows.Forms.Label
	Public WithEvents lblGetKeys As System.Windows.Forms.Label
	Public WithEvents fraGetKeys As System.Windows.Forms.PictureBox
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmKeySel))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileNew = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
		Me.chkEnforce = New System.Windows.Forms.CheckBox
		Me.cmdApplyGSK = New System.Windows.Forms.Button
		Me.fraGSKDetails = New System.Windows.Forms.GroupBox
		Me.txtGSKDescription = New System.Windows.Forms.TextBox
		Me.chkIsOptional = New System.Windows.Forms.CheckBox
		Me.txtInitialKeyValue = New System.Windows.Forms.TextBox
		Me.lblGSKDescription = New System.Windows.Forms.Label
		Me.lblInitialValue = New System.Windows.Forms.Label
		Me.fraKeyDetails = New System.Windows.Forms.GroupBox
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdApply = New System.Windows.Forms.Button
		Me.txtName = New System.Windows.Forms.TextBox
		Me.txtDescription = New System.Windows.Forms.TextBox
		Me.txtEffectiveDate = New System.Windows.Forms.TextBox
		Me.cboDataType = New System.Windows.Forms.ComboBox
		Me.lblName = New System.Windows.Forms.Label
		Me.lblDescription = New System.Windows.Forms.Label
		Me.lblType = New System.Windows.Forms.Label
		Me.lblEffectiveDate = New System.Windows.Forms.Label
		Me.cmdExit = New System.Windows.Forms.Button
		Me.fraGetKeys = New System.Windows.Forms.PictureBox
		Me.cmdGKRemove = New System.Windows.Forms.Button
		Me.cmdGKAdd = New System.Windows.Forms.Button
		Me.lstGKRequiredKeys = New System.Windows.Forms.ListBox
		Me.lstSKRequiredKeys = New System.Windows.Forms.ListBox
		Me.cmdSKRemove = New System.Windows.Forms.Button
		Me.cmdSKAdd = New System.Windows.Forms.Button
		Me.lstSystemKeys = New System.Windows.Forms.ListBox
		Me.lblAllKeys = New System.Windows.Forms.Label
		Me.lblSetKeys = New System.Windows.Forms.Label
		Me.lblGetKeys = New System.Windows.Forms.Label
		Me.fraGSKDetails.SuspendLayout()
		Me.fraKeyDetails.SuspendLayout()
		Me.fraGetKeys.SuspendLayout()
		Me.SuspendLayout()
		Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFile})
		' 
		' mnuFile
		' 
		Me.mnuFile.Available = True
		Me.mnuFile.Checked = False
		Me.mnuFile.Enabled = True
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Text = "&File"
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFileNew, Me.mnuFileExit})
		' 
		' mnuFileNew
		' 
		Me.mnuFileNew.Available = True
		Me.mnuFileNew.Checked = False
		Me.mnuFileNew.Enabled = True
		Me.mnuFileNew.Name = "mnuFileNew"
		Me.mnuFileNew.Text = "&New"
		' 
		' mnuFileExit
		' 
		Me.mnuFileExit.Available = True
		Me.mnuFileExit.Checked = False
		Me.mnuFileExit.Enabled = True
		Me.mnuFileExit.Name = "mnuFileExit"
		Me.mnuFileExit.Text = "&Exit"
		' 
		' chkEnforce
		' 
		Me.chkEnforce.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkEnforce.BackColor = System.Drawing.SystemColors.Control
		Me.chkEnforce.CausesValidation = True
		Me.chkEnforce.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkEnforce.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkEnforce.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkEnforce.Enabled = False
		Me.chkEnforce.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkEnforce.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkEnforce.Location = New System.Drawing.Point(8, 424)
		Me.chkEnforce.Name = "chkEnforce"
		Me.chkEnforce.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkEnforce.Size = New System.Drawing.Size(121, 17)
		Me.chkEnforce.TabIndex = 30
		Me.chkEnforce.TabStop = False
		Me.chkEnforce.Text = "Enforce Key Rules"
		Me.chkEnforce.Visible = True
		' 
		' cmdApplyGSK
		' 
		Me.cmdApplyGSK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApplyGSK.CausesValidation = True
		Me.cmdApplyGSK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApplyGSK.Enabled = False
		Me.cmdApplyGSK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApplyGSK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApplyGSK.Location = New System.Drawing.Point(408, 424)
		Me.cmdApplyGSK.Name = "cmdApplyGSK"
		Me.cmdApplyGSK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApplyGSK.Size = New System.Drawing.Size(73, 22)
		Me.cmdApplyGSK.TabIndex = 27
		Me.cmdApplyGSK.TabStop = False
		Me.cmdApplyGSK.Text = "A&pply"
		Me.cmdApplyGSK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApplyGSK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraGSKDetails
		' 
		Me.fraGSKDetails.BackColor = System.Drawing.SystemColors.Control
		Me.fraGSKDetails.Controls.Add(Me.txtGSKDescription)
		Me.fraGSKDetails.Controls.Add(Me.chkIsOptional)
		Me.fraGSKDetails.Controls.Add(Me.txtInitialKeyValue)
		Me.fraGSKDetails.Controls.Add(Me.lblGSKDescription)
		Me.fraGSKDetails.Controls.Add(Me.lblInitialValue)
		Me.fraGSKDetails.Enabled = True
		Me.fraGSKDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraGSKDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraGSKDetails.Location = New System.Drawing.Point(288, 304)
		Me.fraGSKDetails.Name = "fraGSKDetails"
		Me.fraGSKDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraGSKDetails.Size = New System.Drawing.Size(193, 113)
		Me.fraGSKDetails.TabIndex = 24
		Me.fraGSKDetails.Text = "Set/Get Key Details"
		Me.fraGSKDetails.Visible = True
		' 
		' txtGSKDescription
		' 
		Me.txtGSKDescription.AcceptsReturn = True
		Me.txtGSKDescription.AutoSize = False
		Me.txtGSKDescription.BackColor = System.Drawing.SystemColors.Window
		Me.txtGSKDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtGSKDescription.CausesValidation = True
		Me.txtGSKDescription.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtGSKDescription.Enabled = True
		Me.txtGSKDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtGSKDescription.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtGSKDescription.HideSelection = True
		Me.txtGSKDescription.Location = New System.Drawing.Point(8, 32)
		Me.txtGSKDescription.MaxLength = 0
		Me.txtGSKDescription.Multiline = False
		Me.txtGSKDescription.Name = "txtGSKDescription"
		Me.txtGSKDescription.ReadOnly = False
		Me.txtGSKDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtGSKDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtGSKDescription.Size = New System.Drawing.Size(177, 19)
		Me.txtGSKDescription.TabIndex = 5
		Me.txtGSKDescription.TabStop = True
		Me.txtGSKDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtGSKDescription.Visible = True
		' 
		' chkIsOptional
		' 
		Me.chkIsOptional.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkIsOptional.BackColor = System.Drawing.SystemColors.Control
		Me.chkIsOptional.CausesValidation = True
		Me.chkIsOptional.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkIsOptional.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkIsOptional.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkIsOptional.Enabled = True
		Me.chkIsOptional.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkIsOptional.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkIsOptional.Location = New System.Drawing.Point(112, 80)
		Me.chkIsOptional.Name = "chkIsOptional"
		Me.chkIsOptional.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkIsOptional.Size = New System.Drawing.Size(73, 17)
		Me.chkIsOptional.TabIndex = 6
		Me.chkIsOptional.TabStop = True
		Me.chkIsOptional.Text = "IsOptional"
		Me.chkIsOptional.Visible = False
		' 
		' txtInitialKeyValue
		' 
		Me.txtInitialKeyValue.AcceptsReturn = True
		Me.txtInitialKeyValue.AutoSize = False
		Me.txtInitialKeyValue.BackColor = System.Drawing.SystemColors.Window
		Me.txtInitialKeyValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInitialKeyValue.CausesValidation = True
		Me.txtInitialKeyValue.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInitialKeyValue.Enabled = True
		Me.txtInitialKeyValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtInitialKeyValue.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInitialKeyValue.HideSelection = True
		Me.txtInitialKeyValue.Location = New System.Drawing.Point(8, 80)
		Me.txtInitialKeyValue.MaxLength = 0
		Me.txtInitialKeyValue.Multiline = False
		Me.txtInitialKeyValue.Name = "txtInitialKeyValue"
		Me.txtInitialKeyValue.ReadOnly = False
		Me.txtInitialKeyValue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInitialKeyValue.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInitialKeyValue.Size = New System.Drawing.Size(177, 19)
		Me.txtInitialKeyValue.TabIndex = 7
		Me.txtInitialKeyValue.TabStop = True
		Me.txtInitialKeyValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInitialKeyValue.Visible = False
		' 
		' lblGSKDescription
		' 
		Me.lblGSKDescription.AutoSize = False
		Me.lblGSKDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblGSKDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblGSKDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblGSKDescription.Enabled = True
		Me.lblGSKDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblGSKDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblGSKDescription.Location = New System.Drawing.Point(8, 16)
		Me.lblGSKDescription.Name = "lblGSKDescription"
		Me.lblGSKDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblGSKDescription.Size = New System.Drawing.Size(177, 17)
		Me.lblGSKDescription.TabIndex = 26
		Me.lblGSKDescription.Text = "Description"
		Me.lblGSKDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblGSKDescription.UseMnemonic = True
		Me.lblGSKDescription.Visible = True
		' 
		' lblInitialValue
		' 
		Me.lblInitialValue.AutoSize = False
		Me.lblInitialValue.BackColor = System.Drawing.SystemColors.Control
		Me.lblInitialValue.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInitialValue.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInitialValue.Enabled = True
		Me.lblInitialValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInitialValue.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInitialValue.Location = New System.Drawing.Point(8, 64)
		Me.lblInitialValue.Name = "lblInitialValue"
		Me.lblInitialValue.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInitialValue.Size = New System.Drawing.Size(97, 17)
		Me.lblInitialValue.TabIndex = 25
		Me.lblInitialValue.Text = "Initial Value"
		Me.lblInitialValue.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInitialValue.UseMnemonic = True
		Me.lblInitialValue.Visible = False
		' 
		' fraKeyDetails
		' 
		Me.fraKeyDetails.BackColor = System.Drawing.SystemColors.Control
		Me.fraKeyDetails.Controls.Add(Me.cmdAdd)
		Me.fraKeyDetails.Controls.Add(Me.cmdApply)
		Me.fraKeyDetails.Controls.Add(Me.txtName)
		Me.fraKeyDetails.Controls.Add(Me.txtDescription)
		Me.fraKeyDetails.Controls.Add(Me.txtEffectiveDate)
		Me.fraKeyDetails.Controls.Add(Me.cboDataType)
		Me.fraKeyDetails.Controls.Add(Me.lblName)
		Me.fraKeyDetails.Controls.Add(Me.lblDescription)
		Me.fraKeyDetails.Controls.Add(Me.lblType)
		Me.fraKeyDetails.Controls.Add(Me.lblEffectiveDate)
		Me.fraKeyDetails.Enabled = True
		Me.fraKeyDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraKeyDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraKeyDetails.Location = New System.Drawing.Point(8, 304)
		Me.fraKeyDetails.Name = "fraKeyDetails"
		Me.fraKeyDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraKeyDetails.Size = New System.Drawing.Size(273, 113)
		Me.fraKeyDetails.TabIndex = 19
		Me.fraKeyDetails.Text = "Key Details"
		Me.fraKeyDetails.Visible = True
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(208, 64)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(57, 22)
		Me.cmdAdd.TabIndex = 29
		Me.cmdAdd.TabStop = False
		Me.cmdAdd.Text = "&New"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = False
		Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(208, 88)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(57, 22)
		Me.cmdApply.TabIndex = 28
		Me.cmdApply.TabStop = False
		Me.cmdApply.Text = "&Apply"
		Me.cmdApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtName
		' 
		Me.txtName.AcceptsReturn = True
		Me.txtName.AutoSize = False
		Me.txtName.BackColor = System.Drawing.SystemColors.Window
		Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtName.CausesValidation = True
		Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtName.Enabled = True
		Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtName.HideSelection = True
		Me.txtName.Location = New System.Drawing.Point(80, 40)
		Me.txtName.MaxLength = 0
		Me.txtName.Multiline = False
		Me.txtName.Name = "txtName"
		Me.txtName.ReadOnly = False
		Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtName.Size = New System.Drawing.Size(121, 19)
		Me.txtName.TabIndex = 2
		Me.txtName.TabStop = True
		Me.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtName.Visible = True
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
		Me.txtDescription.Location = New System.Drawing.Point(80, 16)
		Me.txtDescription.MaxLength = 0
		Me.txtDescription.Multiline = False
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.ReadOnly = False
		Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDescription.Size = New System.Drawing.Size(185, 19)
		Me.txtDescription.TabIndex = 1
		Me.txtDescription.TabStop = True
		Me.txtDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDescription.Visible = True
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
		Me.txtEffectiveDate.Location = New System.Drawing.Point(80, 88)
		Me.txtEffectiveDate.MaxLength = 0
		Me.txtEffectiveDate.Multiline = False
		Me.txtEffectiveDate.Name = "txtEffectiveDate"
		Me.txtEffectiveDate.ReadOnly = False
		Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEffectiveDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEffectiveDate.Size = New System.Drawing.Size(121, 19)
		Me.txtEffectiveDate.TabIndex = 4
		Me.txtEffectiveDate.TabStop = True
		Me.txtEffectiveDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEffectiveDate.Visible = True
		' 
		' cboDataType
		' 
		Me.cboDataType.BackColor = System.Drawing.SystemColors.Window
		Me.cboDataType.CausesValidation = True
		Me.cboDataType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboDataType.Enabled = True
		Me.cboDataType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboDataType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboDataType.IntegralHeight = True
		Me.cboDataType.Location = New System.Drawing.Point(80, 64)
		Me.cboDataType.Name = "cboDataType"
		Me.cboDataType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboDataType.Size = New System.Drawing.Size(121, 21)
		Me.cboDataType.Sorted = False
		Me.cboDataType.TabIndex = 3
		Me.cboDataType.TabStop = True
		Me.cboDataType.Visible = True
		' 
		' lblName
		' 
		Me.lblName.AutoSize = False
		Me.lblName.BackColor = System.Drawing.SystemColors.Control
		Me.lblName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblName.Enabled = True
		Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblName.Location = New System.Drawing.Point(8, 40)
		Me.lblName.Name = "lblName"
		Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblName.Size = New System.Drawing.Size(33, 17)
		Me.lblName.TabIndex = 23
		Me.lblName.Text = "Name"
		Me.lblName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblName.UseMnemonic = True
		Me.lblName.Visible = True
		' 
		' lblDescription
		' 
		Me.lblDescription.AutoSize = False
		Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
		Me.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDescription.Enabled = True
		Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDescription.Location = New System.Drawing.Point(8, 16)
		Me.lblDescription.Name = "lblDescription"
		Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDescription.Size = New System.Drawing.Size(81, 17)
		Me.lblDescription.TabIndex = 22
		Me.lblDescription.Text = "Description"
		Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDescription.UseMnemonic = True
		Me.lblDescription.Visible = True
		' 
		' lblType
		' 
		Me.lblType.AutoSize = False
		Me.lblType.BackColor = System.Drawing.SystemColors.Control
		Me.lblType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblType.Enabled = True
		Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblType.Location = New System.Drawing.Point(8, 64)
		Me.lblType.Name = "lblType"
		Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblType.Size = New System.Drawing.Size(49, 17)
		Me.lblType.TabIndex = 21
		Me.lblType.Text = "Data type"
		Me.lblType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblType.UseMnemonic = True
		Me.lblType.Visible = True
		' 
		' lblEffectiveDate
		' 
		Me.lblEffectiveDate.AutoSize = False
		Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblEffectiveDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblEffectiveDate.Enabled = True
		Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblEffectiveDate.Location = New System.Drawing.Point(8, 88)
		Me.lblEffectiveDate.Name = "lblEffectiveDate"
		Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblEffectiveDate.Size = New System.Drawing.Size(81, 17)
		Me.lblEffectiveDate.TabIndex = 20
		Me.lblEffectiveDate.Text = "Effective Date"
		Me.lblEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblEffectiveDate.UseMnemonic = True
		Me.lblEffectiveDate.Visible = True
		' 
		' cmdExit
		' 
		Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdExit.CausesValidation = True
		Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdExit.Enabled = True
		Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdExit.Location = New System.Drawing.Point(328, 424)
		Me.cmdExit.Name = "cmdExit"
		Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdExit.Size = New System.Drawing.Size(73, 22)
		Me.cmdExit.TabIndex = 13
		Me.cmdExit.TabStop = False
		Me.cmdExit.Text = "&Exit"
		Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraGetKeys
		' 
		Me.fraGetKeys.BackColor = System.Drawing.SystemColors.Control
		Me.fraGetKeys.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.fraGetKeys.CausesValidation = True
		Me.fraGetKeys.Controls.Add(Me.cmdGKRemove)
		Me.fraGetKeys.Controls.Add(Me.cmdGKAdd)
		Me.fraGetKeys.Controls.Add(Me.lstGKRequiredKeys)
		Me.fraGetKeys.Controls.Add(Me.lstSKRequiredKeys)
		Me.fraGetKeys.Controls.Add(Me.cmdSKRemove)
		Me.fraGetKeys.Controls.Add(Me.cmdSKAdd)
		Me.fraGetKeys.Controls.Add(Me.lstSystemKeys)
		Me.fraGetKeys.Controls.Add(Me.lblAllKeys)
		Me.fraGetKeys.Controls.Add(Me.lblSetKeys)
		Me.fraGetKeys.Controls.Add(Me.lblGetKeys)
		Me.fraGetKeys.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraGetKeys.Dock = System.Windows.Forms.DockStyle.None
		Me.fraGetKeys.Enabled = True
		Me.fraGetKeys.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraGetKeys.Location = New System.Drawing.Point(8, 24)
		Me.fraGetKeys.Name = "fraGetKeys"
		Me.fraGetKeys.Size = New System.Drawing.Size(473, 273)
		Me.fraGetKeys.TabIndex = 0
		Me.fraGetKeys.TabStop = True
		Me.fraGetKeys.Visible = True
		' 
		' cmdGKRemove
		' 
		Me.cmdGKRemove.BackColor = System.Drawing.SystemColors.Control
		Me.cmdGKRemove.CausesValidation = True
		Me.cmdGKRemove.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdGKRemove.Enabled = True
		Me.cmdGKRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdGKRemove.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdGKRemove.Location = New System.Drawing.Point(200, 216)
		Me.cmdGKRemove.Name = "cmdGKRemove"
		Me.cmdGKRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdGKRemove.Size = New System.Drawing.Size(73, 22)
		Me.cmdGKRemove.TabIndex = 18
		Me.cmdGKRemove.TabStop = False
		Me.cmdGKRemove.Text = "<< Remove"
		Me.cmdGKRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdGKRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdGKAdd
		' 
		Me.cmdGKAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdGKAdd.CausesValidation = True
		Me.cmdGKAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdGKAdd.Enabled = True
		Me.cmdGKAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdGKAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdGKAdd.Location = New System.Drawing.Point(200, 184)
		Me.cmdGKAdd.Name = "cmdGKAdd"
		Me.cmdGKAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdGKAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdGKAdd.TabIndex = 17
		Me.cmdGKAdd.TabStop = False
		Me.cmdGKAdd.Text = "Add >>"
		Me.cmdGKAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdGKAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lstGKRequiredKeys
		' 
		Me.lstGKRequiredKeys.BackColor = System.Drawing.SystemColors.Window
		Me.lstGKRequiredKeys.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstGKRequiredKeys.CausesValidation = True
		Me.lstGKRequiredKeys.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstGKRequiredKeys.Enabled = True
		Me.lstGKRequiredKeys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstGKRequiredKeys.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstGKRequiredKeys.IntegralHeight = True
		Me.lstGKRequiredKeys.Location = New System.Drawing.Point(288, 152)
		Me.lstGKRequiredKeys.MultiColumn = False
		Me.lstGKRequiredKeys.Name = "lstGKRequiredKeys"
		Me.lstGKRequiredKeys.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstGKRequiredKeys.Size = New System.Drawing.Size(177, 111)
		Me.lstGKRequiredKeys.Sorted = False
		Me.lstGKRequiredKeys.TabIndex = 12
		Me.lstGKRequiredKeys.TabStop = False
		Me.lstGKRequiredKeys.Visible = True
		' 
		' lstSKRequiredKeys
		' 
		Me.lstSKRequiredKeys.BackColor = System.Drawing.SystemColors.Window
		Me.lstSKRequiredKeys.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstSKRequiredKeys.CausesValidation = True
		Me.lstSKRequiredKeys.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstSKRequiredKeys.Enabled = True
		Me.lstSKRequiredKeys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstSKRequiredKeys.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstSKRequiredKeys.IntegralHeight = True
		Me.lstSKRequiredKeys.Location = New System.Drawing.Point(288, 24)
		Me.lstSKRequiredKeys.MultiColumn = False
		Me.lstSKRequiredKeys.Name = "lstSKRequiredKeys"
		Me.lstSKRequiredKeys.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstSKRequiredKeys.Size = New System.Drawing.Size(177, 111)
		Me.lstSKRequiredKeys.Sorted = False
		Me.lstSKRequiredKeys.TabIndex = 9
		Me.lstSKRequiredKeys.TabStop = False
		Me.lstSKRequiredKeys.Visible = True
		' 
		' cmdSKRemove
		' 
		Me.cmdSKRemove.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSKRemove.CausesValidation = True
		Me.cmdSKRemove.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSKRemove.Enabled = True
		Me.cmdSKRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSKRemove.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSKRemove.Location = New System.Drawing.Point(200, 80)
		Me.cmdSKRemove.Name = "cmdSKRemove"
		Me.cmdSKRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSKRemove.Size = New System.Drawing.Size(73, 22)
		Me.cmdSKRemove.TabIndex = 11
		Me.cmdSKRemove.TabStop = False
		Me.cmdSKRemove.Text = "<< Remove"
		Me.cmdSKRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSKRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdSKAdd
		' 
		Me.cmdSKAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSKAdd.CausesValidation = True
		Me.cmdSKAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSKAdd.Enabled = True
		Me.cmdSKAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSKAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSKAdd.Location = New System.Drawing.Point(200, 48)
		Me.cmdSKAdd.Name = "cmdSKAdd"
		Me.cmdSKAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSKAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdSKAdd.TabIndex = 10
		Me.cmdSKAdd.TabStop = False
		Me.cmdSKAdd.Text = "Add >>"
		Me.cmdSKAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSKAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lstSystemKeys
		' 
		Me.lstSystemKeys.BackColor = System.Drawing.SystemColors.Window
		Me.lstSystemKeys.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstSystemKeys.CausesValidation = True
		Me.lstSystemKeys.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstSystemKeys.Enabled = True
		Me.lstSystemKeys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstSystemKeys.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstSystemKeys.IntegralHeight = True
		Me.lstSystemKeys.Location = New System.Drawing.Point(8, 24)
		Me.lstSystemKeys.MultiColumn = False
		Me.lstSystemKeys.Name = "lstSystemKeys"
		Me.lstSystemKeys.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstSystemKeys.Size = New System.Drawing.Size(177, 241)
		Me.lstSystemKeys.Sorted = False
		Me.lstSystemKeys.TabIndex = 8
		Me.lstSystemKeys.TabStop = False
		Me.lstSystemKeys.Visible = True
		' 
		' lblAllKeys
		' 
		Me.lblAllKeys.AutoSize = False
		Me.lblAllKeys.BackColor = System.Drawing.SystemColors.Control
		Me.lblAllKeys.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAllKeys.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAllKeys.Enabled = True
		Me.lblAllKeys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAllKeys.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAllKeys.Location = New System.Drawing.Point(8, 10)
		Me.lblAllKeys.Name = "lblAllKeys"
		Me.lblAllKeys.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAllKeys.Size = New System.Drawing.Size(177, 17)
		Me.lblAllKeys.TabIndex = 16
		Me.lblAllKeys.Text = "System Keys"
		Me.lblAllKeys.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAllKeys.UseMnemonic = True
		Me.lblAllKeys.Visible = True
		' 
		' lblSetKeys
		' 
		Me.lblSetKeys.AutoSize = False
		Me.lblSetKeys.BackColor = System.Drawing.SystemColors.Control
		Me.lblSetKeys.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSetKeys.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSetKeys.Enabled = True
		Me.lblSetKeys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSetKeys.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSetKeys.Location = New System.Drawing.Point(288, 10)
		Me.lblSetKeys.Name = "lblSetKeys"
		Me.lblSetKeys.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSetKeys.Size = New System.Drawing.Size(121, 17)
		Me.lblSetKeys.TabIndex = 15
		Me.lblSetKeys.Text = "Set Keys"
		Me.lblSetKeys.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSetKeys.UseMnemonic = True
		Me.lblSetKeys.Visible = True
		' 
		' lblGetKeys
		' 
		Me.lblGetKeys.AutoSize = False
		Me.lblGetKeys.BackColor = System.Drawing.SystemColors.Control
		Me.lblGetKeys.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblGetKeys.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblGetKeys.Enabled = True
		Me.lblGetKeys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblGetKeys.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblGetKeys.Location = New System.Drawing.Point(288, 136)
		Me.lblGetKeys.Name = "lblGetKeys"
		Me.lblGetKeys.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblGetKeys.Size = New System.Drawing.Size(121, 17)
		Me.lblGetKeys.TabIndex = 14
		Me.lblGetKeys.Text = "Get Keys"
		Me.lblGetKeys.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblGetKeys.UseMnemonic = True
		Me.lblGetKeys.Visible = True
		' 
		' frmKeySel
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.CancelButton = Me.cmdExit
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(488, 451)
		Me.ControlBox = True
		Me.Controls.Add(Me.chkEnforce)
		Me.Controls.Add(Me.cmdApplyGSK)
		Me.Controls.Add(Me.fraGSKDetails)
		Me.Controls.Add(Me.fraKeyDetails)
		Me.Controls.Add(Me.cmdExit)
		Me.Controls.Add(Me.fraGetKeys)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmKeySel.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(10, 29)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmKeySel"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "PMNavKey Editor"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxHelper1.SetSelectionMode(Me.lstGKRequiredKeys, System.Windows.Forms.SelectionMode.MultiExtended)
		Me.listBoxHelper1.SetSelectionMode(Me.lstSKRequiredKeys, System.Windows.Forms.SelectionMode.MultiExtended)
		Me.listBoxHelper1.SetSelectionMode(Me.lstSystemKeys, System.Windows.Forms.SelectionMode.MultiExtended)
		Me.fraGSKDetails.ResumeLayout(False)
		Me.fraKeyDetails.ResumeLayout(False)
		Me.fraGetKeys.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class