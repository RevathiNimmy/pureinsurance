<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents txtFormatDate As System.Windows.Forms.TextBox
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblPrimCause As System.Windows.Forms.Label
	Public WithEvents txtPrimCause As System.Windows.Forms.TextBox
	Private WithEvents _lvwPrimCauseRiskType_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPrimCauseRiskType_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwPrimCauseRiskType_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwPrimCauseRiskType As System.Windows.Forms.ListView
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.txtFormatDate = New System.Windows.Forms.TextBox
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblPrimCause = New System.Windows.Forms.Label
        Me.txtPrimCause = New System.Windows.Forms.TextBox
        Me.lvwPrimCauseRiskType = New System.Windows.Forms.ListView
        Me._lvwPrimCauseRiskType_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwPrimCauseRiskType_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwPrimCauseRiskType_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Location = New System.Drawing.Point(520, 246)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(73, 22)
        Me.cmdSelect.TabIndex = 1
        Me.cmdSelect.Text = "&Select"
        Me.cmdSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'txtFormatDate
        '
        Me.txtFormatDate.AcceptsReturn = True
        Me.txtFormatDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatDate.Location = New System.Drawing.Point(48, 288)
        Me.txtFormatDate.MaxLength = 0
        Me.txtFormatDate.Name = "txtFormatDate"
        Me.txtFormatDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatDate.Size = New System.Drawing.Size(88, 20)
        Me.txtFormatDate.TabIndex = 8
        Me.txtFormatDate.TabStop = False
        Me.txtFormatDate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(530, 287)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 4
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
        Me.cmdCancel.Location = New System.Drawing.Point(449, 287)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 3
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
        Me.cmdOK.Location = New System.Drawing.Point(368, 287)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(594, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(599, 275)
        Me.tabMainTab.TabIndex = 6
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPrimCause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPrimCause)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwPrimCauseRiskType)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(591, 249)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Risk Type Groups"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblPrimCause
        '
        Me.lblPrimCause.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrimCause.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrimCause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrimCause.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrimCause.Location = New System.Drawing.Point(8, 13)
        Me.lblPrimCause.Name = "lblPrimCause"
        Me.lblPrimCause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrimCause.Size = New System.Drawing.Size(104, 21)
        Me.lblPrimCause.TabIndex = 7
        Me.lblPrimCause.Text = "Primary Cause:"
        '
        'txtPrimCause
        '
        Me.txtPrimCause.AcceptsReturn = True
        Me.txtPrimCause.BackColor = System.Drawing.SystemColors.Window
        Me.txtPrimCause.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPrimCause.Enabled = False
        Me.txtPrimCause.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrimCause.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.txtPrimCause.Location = New System.Drawing.Point(135, 13)
        Me.txtPrimCause.MaxLength = 0
        Me.txtPrimCause.Name = "txtPrimCause"
        Me.txtPrimCause.ReadOnly = True
        Me.txtPrimCause.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPrimCause.Size = New System.Drawing.Size(262, 20)
        Me.txtPrimCause.TabIndex = 5
        Me.txtPrimCause.TabStop = False
        '
        'lvwPrimCauseRiskType
        '
        Me.lvwPrimCauseRiskType.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPrimCauseRiskType.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPrimCauseRiskType, "")
        Me.lvwPrimCauseRiskType.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPrimCauseRiskType_ColumnHeader_1, Me._lvwPrimCauseRiskType_ColumnHeader_2, Me._lvwPrimCauseRiskType_ColumnHeader_3})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPrimCauseRiskType, True)
        Me.lvwPrimCauseRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPrimCauseRiskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPrimCauseRiskType.FullRowSelect = True
        Me.lvwPrimCauseRiskType.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPrimCauseRiskType, "lvwPrimCauseRiskType_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPrimCauseRiskType, "")
        Me.lvwPrimCauseRiskType.Location = New System.Drawing.Point(8, 43)
        Me.lvwPrimCauseRiskType.Name = "lvwPrimCauseRiskType"
        Me.lvwPrimCauseRiskType.Size = New System.Drawing.Size(578, 168)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPrimCauseRiskType, "")
        Me.listViewHelper1.SetSorted(Me.lvwPrimCauseRiskType, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPrimCauseRiskType, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPrimCauseRiskType, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPrimCauseRiskType.TabIndex = 0
        Me.lvwPrimCauseRiskType.UseCompatibleStateImageBehavior = False
        Me.lvwPrimCauseRiskType.View = System.Windows.Forms.View.Details
        '
        '_lvwPrimCauseRiskType_ColumnHeader_1
        '
        Me._lvwPrimCauseRiskType_ColumnHeader_1.Text = "Code"
        Me._lvwPrimCauseRiskType_ColumnHeader_1.Width = 97
        '
        '_lvwPrimCauseRiskType_ColumnHeader_2
        '
        Me._lvwPrimCauseRiskType_ColumnHeader_2.Text = "Description"
        Me._lvwPrimCauseRiskType_ColumnHeader_2.Width = 97
        '
        '_lvwPrimCauseRiskType_ColumnHeader_3
        '
        Me._lvwPrimCauseRiskType_ColumnHeader_3.Text = "Effective Date"
        Me._lvwPrimCauseRiskType_ColumnHeader_3.Width = 97
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(611, 315)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.txtFormatDate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Primary Cause - Risk Type Group"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class