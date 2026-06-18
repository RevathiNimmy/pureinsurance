<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLicenceKeyGen
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
	Public WithEvents cmdWrite As System.Windows.Forms.Button
	Public WithEvents optCategory As System.Windows.Forms.RadioButton
	Public WithEvents optSystem As System.Windows.Forms.RadioButton
	Public WithEvents txtCategoryDescription As System.Windows.Forms.TextBox
	Public WithEvents cmdGenerate As System.Windows.Forms.Button
	Public WithEvents optNone As System.Windows.Forms.RadioButton
	Public WithEvents optWarn As System.Windows.Forms.RadioButton
	Public WithEvents optBlock As System.Windows.Forms.RadioButton
	Public WithEvents txtLicenceLimit As System.Windows.Forms.TextBox
	Public WithEvents txtCategory As System.Windows.Forms.TextBox
	Public WithEvents txtICCS As System.Windows.Forms.TextBox
	Public WithEvents lblCategoryDescription As System.Windows.Forms.Label
	Public WithEvents lblAction As System.Windows.Forms.Label
	Public WithEvents lblLicenceLimit As System.Windows.Forms.Label
	Public WithEvents lblCategory As System.Windows.Forms.Label
	Public WithEvents lblICCS As System.Windows.Forms.Label
	Public WithEvents panKeyFields As System.Windows.Forms.Panel
	Public WithEvents txtLicenceKey As System.Windows.Forms.TextBox
	Public dlgBrowseSave As System.Windows.Forms.SaveFileDialog
	Public WithEvents lblLicenceKey As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLicenceKeyGen))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdWrite = New System.Windows.Forms.Button()
        Me.optCategory = New System.Windows.Forms.RadioButton()
        Me.optSystem = New System.Windows.Forms.RadioButton()
        Me.panKeyFields = New System.Windows.Forms.Panel()
        Me.txtCategoryDescription = New System.Windows.Forms.TextBox()
        Me.cmdGenerate = New System.Windows.Forms.Button()
        Me.optNone = New System.Windows.Forms.RadioButton()
        Me.optWarn = New System.Windows.Forms.RadioButton()
        Me.optBlock = New System.Windows.Forms.RadioButton()
        Me.txtLicenceLimit = New System.Windows.Forms.TextBox()
        Me.txtCategory = New System.Windows.Forms.TextBox()
        Me.txtICCS = New System.Windows.Forms.TextBox()
        Me.lblCategoryDescription = New System.Windows.Forms.Label()
        Me.lblAction = New System.Windows.Forms.Label()
        Me.lblLicenceLimit = New System.Windows.Forms.Label()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.lblICCS = New System.Windows.Forms.Label()
        Me.txtLicenceKey = New System.Windows.Forms.TextBox()
        Me.dlgBrowseSave = New System.Windows.Forms.SaveFileDialog()
        Me.lblLicenceKey = New System.Windows.Forms.Label()
        Me.fbdGetFilePath = New System.Windows.Forms.FolderBrowserDialog()
        Me.panKeyFields.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdWrite
        '
        Me.cmdWrite.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWrite.Enabled = False
        Me.cmdWrite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWrite.Location = New System.Drawing.Point(368, 264)
        Me.cmdWrite.Name = "cmdWrite"
        Me.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWrite.Size = New System.Drawing.Size(65, 25)
        Me.cmdWrite.TabIndex = 11
        Me.cmdWrite.Text = "&Write"
        Me.cmdWrite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdWrite.UseVisualStyleBackColor = False
        '
        'optCategory
        '
        Me.optCategory.BackColor = System.Drawing.SystemColors.Control
        Me.optCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.optCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optCategory.Location = New System.Drawing.Point(160, 8)
        Me.optCategory.Name = "optCategory"
        Me.optCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optCategory.Size = New System.Drawing.Size(145, 17)
        Me.optCategory.TabIndex = 1
        Me.optCategory.TabStop = True
        Me.optCategory.Text = "Generate Category Key"
        Me.optCategory.UseVisualStyleBackColor = False
        '
        'optSystem
        '
        Me.optSystem.BackColor = System.Drawing.SystemColors.Control
        Me.optSystem.Checked = True
        Me.optSystem.Cursor = System.Windows.Forms.Cursors.Default
        Me.optSystem.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optSystem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSystem.Location = New System.Drawing.Point(16, 8)
        Me.optSystem.Name = "optSystem"
        Me.optSystem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optSystem.Size = New System.Drawing.Size(129, 17)
        Me.optSystem.TabIndex = 0
        Me.optSystem.TabStop = True
        Me.optSystem.Text = "Generate System Key"
        Me.optSystem.UseVisualStyleBackColor = False
        '
        'panKeyFields
        '
        Me.panKeyFields.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.panKeyFields.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.panKeyFields.Controls.Add(Me.txtCategoryDescription)
        Me.panKeyFields.Controls.Add(Me.cmdGenerate)
        Me.panKeyFields.Controls.Add(Me.optNone)
        Me.panKeyFields.Controls.Add(Me.optWarn)
        Me.panKeyFields.Controls.Add(Me.optBlock)
        Me.panKeyFields.Controls.Add(Me.txtLicenceLimit)
        Me.panKeyFields.Controls.Add(Me.txtCategory)
        Me.panKeyFields.Controls.Add(Me.txtICCS)
        Me.panKeyFields.Controls.Add(Me.lblCategoryDescription)
        Me.panKeyFields.Controls.Add(Me.lblAction)
        Me.panKeyFields.Controls.Add(Me.lblLicenceLimit)
        Me.panKeyFields.Controls.Add(Me.lblCategory)
        Me.panKeyFields.Controls.Add(Me.lblICCS)
        Me.panKeyFields.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panKeyFields.Location = New System.Drawing.Point(8, 32)
        Me.panKeyFields.Name = "panKeyFields"
        Me.panKeyFields.Size = New System.Drawing.Size(425, 217)
        Me.panKeyFields.TabIndex = 13
        '
        'txtCategoryDescription
        '
        Me.txtCategoryDescription.AcceptsReturn = True
        Me.txtCategoryDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtCategoryDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCategoryDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCategoryDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCategoryDescription.Location = New System.Drawing.Point(152, 80)
        Me.txtCategoryDescription.MaxLength = 0
        Me.txtCategoryDescription.Name = "txtCategoryDescription"
        Me.txtCategoryDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCategoryDescription.Size = New System.Drawing.Size(265, 19)
        Me.txtCategoryDescription.TabIndex = 4
        Me.txtCategoryDescription.Visible = False
        '
        'cmdGenerate
        '
        Me.cmdGenerate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGenerate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGenerate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGenerate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGenerate.Location = New System.Drawing.Point(352, 184)
        Me.cmdGenerate.Name = "cmdGenerate"
        Me.cmdGenerate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGenerate.Size = New System.Drawing.Size(65, 25)
        Me.cmdGenerate.TabIndex = 9
        Me.cmdGenerate.Text = "&Generate"
        Me.cmdGenerate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdGenerate.UseVisualStyleBackColor = False
        '
        'optNone
        '
        Me.optNone.BackColor = System.Drawing.SystemColors.Control
        Me.optNone.Cursor = System.Windows.Forms.Cursors.Default
        Me.optNone.Enabled = False
        Me.optNone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optNone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optNone.Location = New System.Drawing.Point(152, 192)
        Me.optNone.Name = "optNone"
        Me.optNone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optNone.Size = New System.Drawing.Size(49, 17)
        Me.optNone.TabIndex = 8
        Me.optNone.TabStop = True
        Me.optNone.Text = "None"
        Me.optNone.UseVisualStyleBackColor = False
        Me.optNone.Visible = False
        '
        'optWarn
        '
        Me.optWarn.BackColor = System.Drawing.SystemColors.Control
        Me.optWarn.Cursor = System.Windows.Forms.Cursors.Default
        Me.optWarn.Enabled = False
        Me.optWarn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optWarn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optWarn.Location = New System.Drawing.Point(152, 168)
        Me.optWarn.Name = "optWarn"
        Me.optWarn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optWarn.Size = New System.Drawing.Size(49, 17)
        Me.optWarn.TabIndex = 7
        Me.optWarn.TabStop = True
        Me.optWarn.Text = "Warn"
        Me.optWarn.UseVisualStyleBackColor = False
        Me.optWarn.Visible = False
        '
        'optBlock
        '
        Me.optBlock.BackColor = System.Drawing.SystemColors.Control
        Me.optBlock.Checked = True
        Me.optBlock.Cursor = System.Windows.Forms.Cursors.Default
        Me.optBlock.Enabled = False
        Me.optBlock.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optBlock.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optBlock.Location = New System.Drawing.Point(152, 144)
        Me.optBlock.Name = "optBlock"
        Me.optBlock.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optBlock.Size = New System.Drawing.Size(49, 17)
        Me.optBlock.TabIndex = 6
        Me.optBlock.TabStop = True
        Me.optBlock.Text = "Block"
        Me.optBlock.UseVisualStyleBackColor = False
        Me.optBlock.Visible = False
        '
        'txtLicenceLimit
        '
        Me.txtLicenceLimit.AcceptsReturn = True
        Me.txtLicenceLimit.BackColor = System.Drawing.SystemColors.Window
        Me.txtLicenceLimit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLicenceLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLicenceLimit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLicenceLimit.Location = New System.Drawing.Point(152, 112)
        Me.txtLicenceLimit.MaxLength = 0
        Me.txtLicenceLimit.Name = "txtLicenceLimit"
        Me.txtLicenceLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLicenceLimit.Size = New System.Drawing.Size(41, 19)
        Me.txtLicenceLimit.TabIndex = 5
        '
        'txtCategory
        '
        Me.txtCategory.AcceptsReturn = True
        Me.txtCategory.BackColor = System.Drawing.SystemColors.Window
        Me.txtCategory.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCategory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCategory.Location = New System.Drawing.Point(152, 48)
        Me.txtCategory.MaxLength = 0
        Me.txtCategory.Name = "txtCategory"
        Me.txtCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCategory.Size = New System.Drawing.Size(145, 19)
        Me.txtCategory.TabIndex = 3
        Me.txtCategory.Visible = False
        '
        'txtICCS
        '
        Me.txtICCS.AcceptsReturn = True
        Me.txtICCS.BackColor = System.Drawing.SystemColors.Window
        Me.txtICCS.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtICCS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtICCS.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtICCS.Location = New System.Drawing.Point(152, 16)
        Me.txtICCS.MaxLength = 0
        Me.txtICCS.Name = "txtICCS"
        Me.txtICCS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtICCS.Size = New System.Drawing.Size(57, 19)
        Me.txtICCS.TabIndex = 2
        '
        'lblCategoryDescription
        '
        Me.lblCategoryDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblCategoryDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCategoryDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCategoryDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCategoryDescription.Location = New System.Drawing.Point(8, 80)
        Me.lblCategoryDescription.Name = "lblCategoryDescription"
        Me.lblCategoryDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCategoryDescription.Size = New System.Drawing.Size(137, 17)
        Me.lblCategoryDescription.TabIndex = 18
        Me.lblCategoryDescription.Text = "Category Description:"
        Me.lblCategoryDescription.Visible = False
        '
        'lblAction
        '
        Me.lblAction.BackColor = System.Drawing.SystemColors.Control
        Me.lblAction.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAction.Location = New System.Drawing.Point(8, 144)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAction.Size = New System.Drawing.Size(137, 17)
        Me.lblAction.TabIndex = 17
        Me.lblAction.Text = "Action Above Licence Limit:"
        Me.lblAction.Visible = False
        '
        'lblLicenceLimit
        '
        Me.lblLicenceLimit.BackColor = System.Drawing.SystemColors.Control
        Me.lblLicenceLimit.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLicenceLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLicenceLimit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLicenceLimit.Location = New System.Drawing.Point(8, 112)
        Me.lblLicenceLimit.Name = "lblLicenceLimit"
        Me.lblLicenceLimit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLicenceLimit.Size = New System.Drawing.Size(73, 17)
        Me.lblLicenceLimit.TabIndex = 16
        Me.lblLicenceLimit.Text = "Licence Limit:"
        '
        'lblCategory
        '
        Me.lblCategory.BackColor = System.Drawing.SystemColors.Control
        Me.lblCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCategory.Location = New System.Drawing.Point(8, 48)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCategory.Size = New System.Drawing.Size(73, 17)
        Me.lblCategory.TabIndex = 15
        Me.lblCategory.Text = "Category Code:"
        Me.lblCategory.Visible = False
        '
        'lblICCS
        '
        Me.lblICCS.BackColor = System.Drawing.SystemColors.Control
        Me.lblICCS.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblICCS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblICCS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblICCS.Location = New System.Drawing.Point(8, 16)
        Me.lblICCS.Name = "lblICCS"
        Me.lblICCS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblICCS.Size = New System.Drawing.Size(49, 17)
        Me.lblICCS.TabIndex = 14
        Me.lblICCS.Text = "ICCS:"
        '
        'txtLicenceKey
        '
        Me.txtLicenceKey.AcceptsReturn = True
        Me.txtLicenceKey.BackColor = System.Drawing.SystemColors.Window
        Me.txtLicenceKey.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLicenceKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLicenceKey.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLicenceKey.Location = New System.Drawing.Point(80, 264)
        Me.txtLicenceKey.MaxLength = 0
        Me.txtLicenceKey.Name = "txtLicenceKey"
        Me.txtLicenceKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLicenceKey.Size = New System.Drawing.Size(281, 19)
        Me.txtLicenceKey.TabIndex = 10
        '
        'lblLicenceKey
        '
        Me.lblLicenceKey.BackColor = System.Drawing.SystemColors.Control
        Me.lblLicenceKey.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLicenceKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLicenceKey.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLicenceKey.Location = New System.Drawing.Point(8, 264)
        Me.lblLicenceKey.Name = "lblLicenceKey"
        Me.lblLicenceKey.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLicenceKey.Size = New System.Drawing.Size(65, 17)
        Me.lblLicenceKey.TabIndex = 12
        Me.lblLicenceKey.Text = "Licence Key:"
        '
        'frmLicenceKeyGen
        '
        Me.AcceptButton = Me.cmdGenerate
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(442, 293)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.optCategory)
        Me.Controls.Add(Me.optSystem)
        Me.Controls.Add(Me.panKeyFields)
        Me.Controls.Add(Me.txtLicenceKey)
        Me.Controls.Add(Me.lblLicenceKey)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLicenceKeyGen"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sirius Licence Key Generator"
        Me.panKeyFields.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fbdGetFilePath As System.Windows.Forms.FolderBrowserDialog
#End Region 
End Class