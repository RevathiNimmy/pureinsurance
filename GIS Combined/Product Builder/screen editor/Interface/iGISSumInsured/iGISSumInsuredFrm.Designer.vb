<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
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
	Public WithEvents txtDateDeleted As System.Windows.Forms.TextBox
	Public WithEvents txtSumInsured As System.Windows.Forms.TextBox
	Public WithEvents txtValuationDate As System.Windows.Forms.TextBox
	Public WithEvents txtDateAdded As System.Windows.Forms.TextBox
	Public WithEvents chkIsValuationRequired As System.Windows.Forms.CheckBox
	Public WithEvents txtReference As System.Windows.Forms.TextBox
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents lblDateDeleted As System.Windows.Forms.Label
	Public WithEvents lblValuationDate As System.Windows.Forms.Label
	Public WithEvents lblDateAdded As System.Windows.Forms.Label
	Public WithEvents lblSumInsured As System.Windows.Forms.Label
	Public WithEvents lblReference As System.Windows.Forms.Label
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents fraGeneral As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.fraGeneral = New System.Windows.Forms.GroupBox
        Me.txtDateDeleted = New System.Windows.Forms.TextBox
        Me.txtSumInsured = New System.Windows.Forms.TextBox
        Me.txtValuationDate = New System.Windows.Forms.TextBox
        Me.txtDateAdded = New System.Windows.Forms.TextBox
        Me.chkIsValuationRequired = New System.Windows.Forms.CheckBox
        Me.txtReference = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.lblDateDeleted = New System.Windows.Forms.Label
        Me.lblValuationDate = New System.Windows.Forms.Label
        Me.lblDateAdded = New System.Windows.Forms.Label
        Me.lblSumInsured = New System.Windows.Forms.Label
        Me.lblReference = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraGeneral.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 312)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 7
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(472, 312)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 10
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(392, 312)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 9
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(312, 312)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 8
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(106, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(541, 301)
        Me.tabMainTab.TabIndex = 11
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraGeneral)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(533, 275)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - General"
        '
        'fraGeneral
        '
        Me.fraGeneral.BackColor = System.Drawing.SystemColors.Control
        Me.fraGeneral.Controls.Add(Me.txtDateDeleted)
        Me.fraGeneral.Controls.Add(Me.txtSumInsured)
        Me.fraGeneral.Controls.Add(Me.txtValuationDate)
        Me.fraGeneral.Controls.Add(Me.txtDateAdded)
        Me.fraGeneral.Controls.Add(Me.chkIsValuationRequired)
        Me.fraGeneral.Controls.Add(Me.txtReference)
        Me.fraGeneral.Controls.Add(Me.txtDescription)
        Me.fraGeneral.Controls.Add(Me.lblDateDeleted)
        Me.fraGeneral.Controls.Add(Me.lblValuationDate)
        Me.fraGeneral.Controls.Add(Me.lblDateAdded)
        Me.fraGeneral.Controls.Add(Me.lblSumInsured)
        Me.fraGeneral.Controls.Add(Me.lblReference)
        Me.fraGeneral.Controls.Add(Me.lblDescription)
        Me.fraGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraGeneral.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraGeneral.Location = New System.Drawing.Point(8, 4)
        Me.fraGeneral.Name = "fraGeneral"
        Me.fraGeneral.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraGeneral.Size = New System.Drawing.Size(521, 265)
        Me.fraGeneral.TabIndex = 12
        Me.fraGeneral.TabStop = False
        '
        'txtDateDeleted
        '
        Me.txtDateDeleted.AcceptsReturn = True
        Me.txtDateDeleted.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateDeleted.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateDeleted.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateDeleted.Location = New System.Drawing.Point(160, 144)
        Me.txtDateDeleted.MaxLength = 70
        Me.txtDateDeleted.Name = "txtDateDeleted"
        Me.txtDateDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateDeleted.Size = New System.Drawing.Size(153, 19)
        Me.txtDateDeleted.TabIndex = 4
        '
        'txtSumInsured
        '
        Me.txtSumInsured.AcceptsReturn = True
        Me.txtSumInsured.BackColor = System.Drawing.SystemColors.Window
        Me.txtSumInsured.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSumInsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSumInsured.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSumInsured.Location = New System.Drawing.Point(160, 80)
        Me.txtSumInsured.MaxLength = 70
        Me.txtSumInsured.Name = "txtSumInsured"
        Me.txtSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSumInsured.Size = New System.Drawing.Size(153, 19)
        Me.txtSumInsured.TabIndex = 2
        '
        'txtValuationDate
        '
        Me.txtValuationDate.AcceptsReturn = True
        Me.txtValuationDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtValuationDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValuationDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValuationDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValuationDate.Location = New System.Drawing.Point(160, 208)
        Me.txtValuationDate.MaxLength = 70
        Me.txtValuationDate.Name = "txtValuationDate"
        Me.txtValuationDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValuationDate.Size = New System.Drawing.Size(153, 19)
        Me.txtValuationDate.TabIndex = 6
        '
        'txtDateAdded
        '
        Me.txtDateAdded.AcceptsReturn = True
        Me.txtDateAdded.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateAdded.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateAdded.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateAdded.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateAdded.Location = New System.Drawing.Point(160, 112)
        Me.txtDateAdded.MaxLength = 70
        Me.txtDateAdded.Name = "txtDateAdded"
        Me.txtDateAdded.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateAdded.Size = New System.Drawing.Size(153, 19)
        Me.txtDateAdded.TabIndex = 3
        '
        'chkIsValuationRequired
        '
        Me.chkIsValuationRequired.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsValuationRequired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsValuationRequired.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsValuationRequired.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsValuationRequired.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsValuationRequired.Location = New System.Drawing.Point(16, 179)
        Me.chkIsValuationRequired.Name = "chkIsValuationRequired"
        Me.chkIsValuationRequired.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsValuationRequired.Size = New System.Drawing.Size(161, 17)
        Me.chkIsValuationRequired.TabIndex = 5
        Me.chkIsValuationRequired.Text = "Is valuation required?"
        Me.chkIsValuationRequired.UseVisualStyleBackColor = False
        '
        'txtReference
        '
        Me.txtReference.AcceptsReturn = True
        Me.txtReference.BackColor = System.Drawing.SystemColors.Window
        Me.txtReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReference.Location = New System.Drawing.Point(160, 48)
        Me.txtReference.MaxLength = 20
        Me.txtReference.Name = "txtReference"
        Me.txtReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReference.Size = New System.Drawing.Size(225, 19)
        Me.txtReference.TabIndex = 1
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(160, 16)
        Me.txtDescription.MaxLength = 255
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(281, 19)
        Me.txtDescription.TabIndex = 0
        '
        'lblDateDeleted
        '
        Me.lblDateDeleted.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateDeleted.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateDeleted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateDeleted.Location = New System.Drawing.Point(16, 147)
        Me.lblDateDeleted.Name = "lblDateDeleted"
        Me.lblDateDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateDeleted.Size = New System.Drawing.Size(145, 17)
        Me.lblDateDeleted.TabIndex = 18
        Me.lblDateDeleted.Text = "Date deleted:"
        '
        'lblValuationDate
        '
        Me.lblValuationDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblValuationDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblValuationDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValuationDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblValuationDate.Location = New System.Drawing.Point(16, 211)
        Me.lblValuationDate.Name = "lblValuationDate"
        Me.lblValuationDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblValuationDate.Size = New System.Drawing.Size(137, 17)
        Me.lblValuationDate.TabIndex = 17
        Me.lblValuationDate.Text = "Valuation date:"
        '
        'lblDateAdded
        '
        Me.lblDateAdded.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateAdded.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateAdded.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateAdded.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateAdded.Location = New System.Drawing.Point(16, 115)
        Me.lblDateAdded.Name = "lblDateAdded"
        Me.lblDateAdded.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateAdded.Size = New System.Drawing.Size(145, 17)
        Me.lblDateAdded.TabIndex = 16
        Me.lblDateAdded.Text = "Date added:"
        '
        'lblSumInsured
        '
        Me.lblSumInsured.BackColor = System.Drawing.SystemColors.Control
        Me.lblSumInsured.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSumInsured.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSumInsured.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSumInsured.Location = New System.Drawing.Point(16, 83)
        Me.lblSumInsured.Name = "lblSumInsured"
        Me.lblSumInsured.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSumInsured.Size = New System.Drawing.Size(145, 17)
        Me.lblSumInsured.TabIndex = 15
        Me.lblSumInsured.Text = "Sum insured:"
        '
        'lblReference
        '
        Me.lblReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReference.Location = New System.Drawing.Point(16, 51)
        Me.lblReference.Name = "lblReference"
        Me.lblReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReference.Size = New System.Drawing.Size(145, 17)
        Me.lblReference.TabIndex = 14
        Me.lblReference.Text = "Reference:"
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(16, 19)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(145, 17)
        Me.lblDescription.TabIndex = 13
        Me.lblDescription.Text = "Description:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(553, 341)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Sum Insured"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraGeneral.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class