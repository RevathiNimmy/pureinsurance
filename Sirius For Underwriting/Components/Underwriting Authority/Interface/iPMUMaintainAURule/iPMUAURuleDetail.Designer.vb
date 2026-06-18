<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetail
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		Form_Initialize_Renamed()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub
	Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not fTerminateCalled_Form_Terminate_Renamed Then
				fTerminateCalled_Form_Terminate_Renamed = True
				Form_Terminate_Renamed()
			End If
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents lblAuthLevel As System.Windows.Forms.Label
	Public WithEvents lblTransType As System.Windows.Forms.Label
	Public WithEvents lblProduct As System.Windows.Forms.Label
	Public WithEvents cboAuthLevel As System.Windows.Forms.ComboBox
	Public WithEvents cboTransType As System.Windows.Forms.ComboBox
	Public WithEvents cboProduct As System.Windows.Forms.ComboBox
	Public WithEvents chkIsUnderwriter As System.Windows.Forms.CheckBox
	Public WithEvents pnlRuleSet As System.Windows.Forms.Panel
	Public WithEvents cmdRuleSet As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetail))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblAuthLevel = New System.Windows.Forms.Label
        Me.lblTransType = New System.Windows.Forms.Label
        Me.lblProduct = New System.Windows.Forms.Label
        Me.cboAuthLevel = New System.Windows.Forms.ComboBox
        Me.cboTransType = New System.Windows.Forms.ComboBox
        Me.cboProduct = New System.Windows.Forms.ComboBox
        Me.chkIsUnderwriter = New System.Windows.Forms.CheckBox
        Me.pnlRuleSet = New System.Windows.Forms.Panel
        Me.lblRuleSet = New System.Windows.Forms.Label
        Me.cmdRuleSet = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pnlRuleSet.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(480, 225)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 4
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(9, 225)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 3
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(330, 225)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(405, 225)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(181, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 6)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(551, 218)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAuthLevel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTransType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProduct)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboAuthLevel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboTransType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboProduct)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIsUnderwriter)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlRuleSet)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRuleSet)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(543, 192)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        'lblAuthLevel
        '
        Me.lblAuthLevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblAuthLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAuthLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAuthLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAuthLevel.Location = New System.Drawing.Point(33, 34)
        Me.lblAuthLevel.Name = "lblAuthLevel"
        Me.lblAuthLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAuthLevel.Size = New System.Drawing.Size(160, 22)
        Me.lblAuthLevel.TabIndex = 11
        Me.lblAuthLevel.Text = "Auth Level"
        '
        'lblTransType
        '
        Me.lblTransType.BackColor = System.Drawing.SystemColors.Control
        Me.lblTransType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTransType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTransType.Location = New System.Drawing.Point(33, 124)
        Me.lblTransType.Name = "lblTransType"
        Me.lblTransType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTransType.Size = New System.Drawing.Size(160, 22)
        Me.lblTransType.TabIndex = 12
        Me.lblTransType.Text = "Trans Type"
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.SystemColors.Control
        Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProduct.Location = New System.Drawing.Point(33, 67)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProduct.Size = New System.Drawing.Size(160, 22)
        Me.lblProduct.TabIndex = 13
        Me.lblProduct.Text = "Prod"
        '
        'cboAuthLevel
        '
        Me.cboAuthLevel.BackColor = System.Drawing.SystemColors.Window
        Me.cboAuthLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAuthLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAuthLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAuthLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAuthLevel.Location = New System.Drawing.Point(237, 31)
        Me.cboAuthLevel.Name = "cboAuthLevel"
        Me.cboAuthLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAuthLevel.Size = New System.Drawing.Size(295, 21)
        Me.cboAuthLevel.TabIndex = 5
        '
        'cboTransType
        '
        Me.cboTransType.BackColor = System.Drawing.SystemColors.Window
        Me.cboTransType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTransType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTransType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTransType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTransType.Location = New System.Drawing.Point(237, 121)
        Me.cboTransType.Name = "cboTransType"
        Me.cboTransType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTransType.Size = New System.Drawing.Size(295, 21)
        Me.cboTransType.TabIndex = 6
        '
        'cboProduct
        '
        Me.cboProduct.BackColor = System.Drawing.SystemColors.Window
        Me.cboProduct.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProduct.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProduct.Location = New System.Drawing.Point(237, 64)
        Me.cboProduct.Name = "cboProduct"
        Me.cboProduct.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProduct.Size = New System.Drawing.Size(295, 21)
        Me.cboProduct.Sorted = True
        Me.cboProduct.TabIndex = 7
        '
        'chkIsUnderwriter
        '
        Me.chkIsUnderwriter.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsUnderwriter.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsUnderwriter.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsUnderwriter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsUnderwriter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsUnderwriter.Location = New System.Drawing.Point(33, 88)
        Me.chkIsUnderwriter.Name = "chkIsUnderwriter"
        Me.chkIsUnderwriter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsUnderwriter.Size = New System.Drawing.Size(217, 28)
        Me.chkIsUnderwriter.TabIndex = 8
        Me.chkIsUnderwriter.Text = "Is UW ?"
        Me.chkIsUnderwriter.UseVisualStyleBackColor = False
        '
        'pnlRuleSet
        '
        Me.pnlRuleSet.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlRuleSet.Controls.Add(Me.lblRuleSet)
        Me.pnlRuleSet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlRuleSet.Location = New System.Drawing.Point(237, 154)
        Me.pnlRuleSet.Name = "pnlRuleSet"
        Me.pnlRuleSet.Size = New System.Drawing.Size(295, 22)
        Me.pnlRuleSet.TabIndex = 9
        '
        'lblRuleSet
        '
        Me.lblRuleSet.AutoEllipsis = True
        Me.lblRuleSet.AutoSize = True
        Me.lblRuleSet.Location = New System.Drawing.Point(4, 3)
        Me.lblRuleSet.Name = "lblRuleSet"
        Me.lblRuleSet.Size = New System.Drawing.Size(0, 13)
        Me.lblRuleSet.TabIndex = 0
        '
        'cmdRuleSet
        '
        Me.cmdRuleSet.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRuleSet.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRuleSet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRuleSet.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRuleSet.Location = New System.Drawing.Point(33, 154)
        Me.cmdRuleSet.Name = "cmdRuleSet"
        Me.cmdRuleSet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRuleSet.Size = New System.Drawing.Size(82, 22)
        Me.cmdRuleSet.TabIndex = 10
        Me.cmdRuleSet.Text = "Rule Set"
        Me.cmdRuleSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRuleSet.UseVisualStyleBackColor = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "Closed")
        Me.ImageList1.Images.SetKeyName(1, "Open")
        '
        'frmDetail
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(556, 250)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDetail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.pnlRuleSet.ResumeLayout(False)
        Me.pnlRuleSet.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblRuleSet As System.Windows.Forms.Label
#End Region 
End Class