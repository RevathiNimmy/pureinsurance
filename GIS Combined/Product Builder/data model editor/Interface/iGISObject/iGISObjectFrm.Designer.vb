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
	Public WithEvents cboObjectType As System.Windows.Forms.ComboBox
	Public WithEvents txtPolarisObjectId As System.Windows.Forms.TextBox
	Public WithEvents chkIsSelectableForScreen As System.Windows.Forms.CheckBox
	Public WithEvents chkIsQuoteObject As System.Windows.Forms.CheckBox
	Public WithEvents txtMaxInstances As System.Windows.Forms.TextBox
	Public WithEvents cboParent As System.Windows.Forms.ComboBox
	Public WithEvents txtTableName As System.Windows.Forms.TextBox
	Public WithEvents txtObjectName As System.Windows.Forms.TextBox
	Public WithEvents lblObjectType As System.Windows.Forms.Label
	Public WithEvents lblPolarisObjectId As System.Windows.Forms.Label
	Public WithEvents lblMaxInstances As System.Windows.Forms.Label
	Public WithEvents lblParent As System.Windows.Forms.Label
	Public WithEvents lblTableName As System.Windows.Forms.Label
	Public WithEvents lblObjectName As System.Windows.Forms.Label
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
        Me.cboObjectType = New System.Windows.Forms.ComboBox
        Me.txtPolarisObjectId = New System.Windows.Forms.TextBox
        Me.chkIsSelectableForScreen = New System.Windows.Forms.CheckBox
        Me.chkIsQuoteObject = New System.Windows.Forms.CheckBox
        Me.txtMaxInstances = New System.Windows.Forms.TextBox
        Me.cboParent = New System.Windows.Forms.ComboBox
        Me.txtTableName = New System.Windows.Forms.TextBox
        Me.txtObjectName = New System.Windows.Forms.TextBox
        Me.lblObjectType = New System.Windows.Forms.Label
        Me.lblPolarisObjectId = New System.Windows.Forms.Label
        Me.lblMaxInstances = New System.Windows.Forms.Label
        Me.lblParent = New System.Windows.Forms.Label
        Me.lblTableName = New System.Windows.Forms.Label
        Me.lblObjectName = New System.Windows.Forms.Label
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
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
        Me.HelpProvider1.SetShowHelp(Me.cmdOK, True)
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
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
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
        Me.fraGeneral.Controls.Add(Me.cboObjectType)
        Me.fraGeneral.Controls.Add(Me.txtPolarisObjectId)
        Me.fraGeneral.Controls.Add(Me.chkIsSelectableForScreen)
        Me.fraGeneral.Controls.Add(Me.chkIsQuoteObject)
        Me.fraGeneral.Controls.Add(Me.txtMaxInstances)
        Me.fraGeneral.Controls.Add(Me.cboParent)
        Me.fraGeneral.Controls.Add(Me.txtTableName)
        Me.fraGeneral.Controls.Add(Me.txtObjectName)
        Me.fraGeneral.Controls.Add(Me.lblObjectType)
        Me.fraGeneral.Controls.Add(Me.lblPolarisObjectId)
        Me.fraGeneral.Controls.Add(Me.lblMaxInstances)
        Me.fraGeneral.Controls.Add(Me.lblParent)
        Me.fraGeneral.Controls.Add(Me.lblTableName)
        Me.fraGeneral.Controls.Add(Me.lblObjectName)
        Me.fraGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraGeneral.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraGeneral.Location = New System.Drawing.Point(8, 4)
        Me.fraGeneral.Name = "fraGeneral"
        Me.fraGeneral.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraGeneral.Size = New System.Drawing.Size(521, 265)
        Me.fraGeneral.TabIndex = 12
        Me.fraGeneral.TabStop = False
        '
        'cboObjectType
        '
        Me.cboObjectType.BackColor = System.Drawing.SystemColors.Window
        Me.cboObjectType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboObjectType.Enabled = False
        Me.cboObjectType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboObjectType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboObjectType.Location = New System.Drawing.Point(176, 235)
        Me.cboObjectType.Name = "cboObjectType"
        Me.cboObjectType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboObjectType.Size = New System.Drawing.Size(228, 21)
        Me.cboObjectType.TabIndex = 18
        '
        'txtPolarisObjectId
        '
        Me.txtPolarisObjectId.AcceptsReturn = True
        Me.txtPolarisObjectId.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolarisObjectId.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolarisObjectId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolarisObjectId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolarisObjectId.Location = New System.Drawing.Point(176, 176)
        Me.txtPolarisObjectId.MaxLength = 0
        Me.txtPolarisObjectId.Name = "txtPolarisObjectId"
        Me.txtPolarisObjectId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolarisObjectId.Size = New System.Drawing.Size(105, 20)
        Me.txtPolarisObjectId.TabIndex = 5
        '
        'chkIsSelectableForScreen
        '
        Me.chkIsSelectableForScreen.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsSelectableForScreen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsSelectableForScreen.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsSelectableForScreen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsSelectableForScreen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsSelectableForScreen.Location = New System.Drawing.Point(16, 211)
        Me.chkIsSelectableForScreen.Name = "chkIsSelectableForScreen"
        Me.chkIsSelectableForScreen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsSelectableForScreen.Size = New System.Drawing.Size(174, 17)
        Me.chkIsSelectableForScreen.TabIndex = 6
        Me.chkIsSelectableForScreen.Text = "Is Selectable For Screen?"
        Me.chkIsSelectableForScreen.UseVisualStyleBackColor = False
        '
        'chkIsQuoteObject
        '
        Me.chkIsQuoteObject.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsQuoteObject.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsQuoteObject.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsQuoteObject.Enabled = False
        Me.chkIsQuoteObject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsQuoteObject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsQuoteObject.Location = New System.Drawing.Point(16, 115)
        Me.chkIsQuoteObject.Name = "chkIsQuoteObject"
        Me.chkIsQuoteObject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsQuoteObject.Size = New System.Drawing.Size(173, 17)
        Me.chkIsQuoteObject.TabIndex = 3
        Me.chkIsQuoteObject.Text = "Is Quote Object?"
        Me.chkIsQuoteObject.UseVisualStyleBackColor = False
        '
        'txtMaxInstances
        '
        Me.txtMaxInstances.AcceptsReturn = True
        Me.txtMaxInstances.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaxInstances.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxInstances.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxInstances.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxInstances.Location = New System.Drawing.Point(176, 80)
        Me.txtMaxInstances.MaxLength = 0
        Me.txtMaxInstances.Name = "txtMaxInstances"
        Me.txtMaxInstances.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxInstances.Size = New System.Drawing.Size(41, 20)
        Me.txtMaxInstances.TabIndex = 2
        '
        'cboParent
        '
        Me.cboParent.BackColor = System.Drawing.SystemColors.Window
        Me.cboParent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboParent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboParent.Enabled = False
        Me.cboParent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboParent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboParent.Location = New System.Drawing.Point(176, 144)
        Me.cboParent.Name = "cboParent"
        Me.cboParent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboParent.Size = New System.Drawing.Size(209, 21)
        Me.cboParent.TabIndex = 4
        '
        'txtTableName
        '
        Me.txtTableName.AcceptsReturn = True
        Me.txtTableName.BackColor = System.Drawing.SystemColors.Window
        Me.txtTableName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTableName.Enabled = False
        Me.txtTableName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTableName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTableName.Location = New System.Drawing.Point(176, 48)
        Me.txtTableName.MaxLength = 70
        Me.txtTableName.Name = "txtTableName"
        Me.txtTableName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTableName.Size = New System.Drawing.Size(209, 20)
        Me.txtTableName.TabIndex = 1
        '
        'txtObjectName
        '
        Me.txtObjectName.AcceptsReturn = True
        Me.txtObjectName.BackColor = System.Drawing.SystemColors.Window
        Me.txtObjectName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtObjectName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtObjectName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtObjectName.Location = New System.Drawing.Point(176, 16)
        Me.txtObjectName.MaxLength = 70
        Me.txtObjectName.Name = "txtObjectName"
        Me.txtObjectName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtObjectName.Size = New System.Drawing.Size(209, 20)
        Me.txtObjectName.TabIndex = 0
        '
        'lblObjectType
        '
        Me.lblObjectType.BackColor = System.Drawing.SystemColors.Control
        Me.lblObjectType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblObjectType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblObjectType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblObjectType.Location = New System.Drawing.Point(16, 239)
        Me.lblObjectType.Name = "lblObjectType"
        Me.lblObjectType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblObjectType.Size = New System.Drawing.Size(89, 17)
        Me.lblObjectType.TabIndex = 19
        Me.lblObjectType.Text = "Object Type:"
        '
        'lblPolarisObjectId
        '
        Me.lblPolarisObjectId.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolarisObjectId.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolarisObjectId.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolarisObjectId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolarisObjectId.Location = New System.Drawing.Point(16, 178)
        Me.lblPolarisObjectId.Name = "lblPolarisObjectId"
        Me.lblPolarisObjectId.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolarisObjectId.Size = New System.Drawing.Size(89, 17)
        Me.lblPolarisObjectId.TabIndex = 17
        Me.lblPolarisObjectId.Text = "Polaris Object:"
        '
        'lblMaxInstances
        '
        Me.lblMaxInstances.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaxInstances.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaxInstances.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxInstances.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaxInstances.Location = New System.Drawing.Point(16, 83)
        Me.lblMaxInstances.Name = "lblMaxInstances"
        Me.lblMaxInstances.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaxInstances.Size = New System.Drawing.Size(89, 17)
        Me.lblMaxInstances.TabIndex = 16
        Me.lblMaxInstances.Text = "Max Instances:"
        '
        'lblParent
        '
        Me.lblParent.BackColor = System.Drawing.SystemColors.Control
        Me.lblParent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblParent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParent.Location = New System.Drawing.Point(16, 147)
        Me.lblParent.Name = "lblParent"
        Me.lblParent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblParent.Size = New System.Drawing.Size(89, 17)
        Me.lblParent.TabIndex = 15
        Me.lblParent.Text = "Parent:"
        '
        'lblTableName
        '
        Me.lblTableName.BackColor = System.Drawing.SystemColors.Control
        Me.lblTableName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTableName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTableName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTableName.Location = New System.Drawing.Point(16, 51)
        Me.lblTableName.Name = "lblTableName"
        Me.lblTableName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTableName.Size = New System.Drawing.Size(89, 17)
        Me.lblTableName.TabIndex = 14
        Me.lblTableName.Text = "Table Name:"
        '
        'lblObjectName
        '
        Me.lblObjectName.BackColor = System.Drawing.SystemColors.Control
        Me.lblObjectName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblObjectName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblObjectName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblObjectName.Location = New System.Drawing.Point(16, 19)
        Me.lblObjectName.Name = "lblObjectName"
        Me.lblObjectName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblObjectName.Size = New System.Drawing.Size(121, 17)
        Me.lblObjectName.TabIndex = 13
        Me.lblObjectName.Text = "Object Name:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
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
        Me.HelpProvider1.SetShowHelp(Me, False)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "GIS Object"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.fraGeneral.ResumeLayout(False)
        Me.fraGeneral.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
#End Region 
End Class