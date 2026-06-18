<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMaintain
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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents lblNewValue As System.Windows.Forms.Label
	Public WithEvents lblType As System.Windows.Forms.Label
	Public WithEvents cmdUpdate As System.Windows.Forms.Button
	Public WithEvents txtNewValue As System.Windows.Forms.TextBox
	Public WithEvents cboLookupTypes As System.Windows.Forms.ComboBox
	Public WithEvents cmdRemove As System.Windows.Forms.Button
	Public WithEvents lstNewValues As System.Windows.Forms.ListBox
	Public WithEvents fraNewValues As System.Windows.Forms.GroupBox
	Public WithEvents lstLookupValues As System.Windows.Forms.ListBox
	Public WithEvents fraStoredValues As System.Windows.Forms.GroupBox
	Public WithEvents cmdRemoveStored As System.Windows.Forms.Button
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents pnlStatus As System.Windows.Forms.Panel
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cmdSort As System.Windows.Forms.Button
	Public WithEvents cmdWrite As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblNewValue = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.cmdUpdate = New System.Windows.Forms.Button
        Me.txtNewValue = New System.Windows.Forms.TextBox
        Me.cboLookupTypes = New System.Windows.Forms.ComboBox
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.fraNewValues = New System.Windows.Forms.GroupBox
        Me.lstNewValues = New System.Windows.Forms.ListBox
        Me.fraStoredValues = New System.Windows.Forms.GroupBox
        Me.lstLookupValues = New System.Windows.Forms.ListBox
        Me.cmdRemoveStored = New System.Windows.Forms.Button
        Me.pnlStatus = New System.Windows.Forms.Panel
        Me.lblStatus = New System.Windows.Forms.Label
        Me.cmdSort = New System.Windows.Forms.Button
        Me.cmdWrite = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.fraNewValues.SuspendLayout()
        Me.fraStoredValues.SuspendLayout()
        Me.pnlStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(543, 372)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(85, 22)
        Me.cmdHelp.TabIndex = 18
        Me.cmdHelp.Text = "Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(206, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 9)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(626, 362)
        Me.tabMainTab.TabIndex = 10
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblNewValue)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdUpdate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtNewValue)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboLookupTypes)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRemove)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraNewValues)
        Me._tabMainTab_TabPage0.Controls.Add(Me.fraStoredValues)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRemoveStored)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlStatus)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(618, 336)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        'lblNewValue
        '
        Me.lblNewValue.BackColor = System.Drawing.SystemColors.Control
        Me.lblNewValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNewValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNewValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNewValue.Location = New System.Drawing.Point(18, 51)
        Me.lblNewValue.Name = "lblNewValue"
        Me.lblNewValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNewValue.Size = New System.Drawing.Size(101, 19)
        Me.lblNewValue.TabIndex = 13
        Me.lblNewValue.Text = "New Value"
        '
        'lblType
        '
        Me.lblType.BackColor = System.Drawing.SystemColors.Control
        Me.lblType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblType.Location = New System.Drawing.Point(18, 23)
        Me.lblType.Name = "lblType"
        Me.lblType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblType.Size = New System.Drawing.Size(101, 19)
        Me.lblType.TabIndex = 14
        Me.lblType.Text = "Lookup Type"
        '
        'cmdUpdate
        '
        Me.cmdUpdate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUpdate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUpdate.Enabled = False
        Me.cmdUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUpdate.Location = New System.Drawing.Point(234, 307)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUpdate.Size = New System.Drawing.Size(71, 22)
        Me.cmdUpdate.TabIndex = 5
        Me.cmdUpdate.Text = "Update"
        Me.cmdUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUpdate.UseVisualStyleBackColor = False
        '
        'txtNewValue
        '
        Me.txtNewValue.AcceptsReturn = True
        Me.txtNewValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtNewValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNewValue.Enabled = False
        Me.txtNewValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNewValue.Location = New System.Drawing.Point(120, 44)
        Me.txtNewValue.MaxLength = 0
        Me.txtNewValue.Name = "txtNewValue"
        Me.txtNewValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNewValue.Size = New System.Drawing.Size(361, 20)
        Me.txtNewValue.TabIndex = 1
        '
        'cboLookupTypes
        '
        Me.cboLookupTypes.BackColor = System.Drawing.SystemColors.Window
        Me.cboLookupTypes.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboLookupTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLookupTypes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLookupTypes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboLookupTypes.Location = New System.Drawing.Point(120, 16)
        Me.cboLookupTypes.Name = "cboLookupTypes"
        Me.cboLookupTypes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboLookupTypes.Size = New System.Drawing.Size(361, 21)
        Me.cboLookupTypes.Sorted = True
        Me.cboLookupTypes.TabIndex = 0
        '
        'cmdRemove
        '
        Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemove.Enabled = False
        Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemove.Location = New System.Drawing.Point(9, 307)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemove.Size = New System.Drawing.Size(71, 22)
        Me.cmdRemove.TabIndex = 4
        Me.cmdRemove.Text = "Remove"
        Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemove.UseVisualStyleBackColor = False
        '
        'fraNewValues
        '
        Me.fraNewValues.BackColor = System.Drawing.SystemColors.Control
        Me.fraNewValues.Controls.Add(Me.lstNewValues)
        Me.fraNewValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraNewValues.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraNewValues.Location = New System.Drawing.Point(9, 76)
        Me.fraNewValues.Name = "fraNewValues"
        Me.fraNewValues.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraNewValues.Size = New System.Drawing.Size(295, 223)
        Me.fraNewValues.TabIndex = 15
        Me.fraNewValues.TabStop = False
        Me.fraNewValues.Text = "New Values to Add"
        '
        'lstNewValues
        '
        Me.lstNewValues.BackColor = System.Drawing.SystemColors.Window
        Me.lstNewValues.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstNewValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstNewValues.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstNewValues.Location = New System.Drawing.Point(9, 24)
        Me.lstNewValues.Name = "lstNewValues"
        Me.lstNewValues.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me.lstNewValues, System.Windows.Forms.SelectionMode.One)
        Me.lstNewValues.Size = New System.Drawing.Size(276, 186)
        Me.lstNewValues.Sorted = True
        Me.lstNewValues.TabIndex = 2
        '
        'fraStoredValues
        '
        Me.fraStoredValues.BackColor = System.Drawing.SystemColors.Control
        Me.fraStoredValues.Controls.Add(Me.lstLookupValues)
        Me.fraStoredValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraStoredValues.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStoredValues.Location = New System.Drawing.Point(315, 76)
        Me.fraStoredValues.Name = "fraStoredValues"
        Me.fraStoredValues.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraStoredValues.Size = New System.Drawing.Size(295, 223)
        Me.fraStoredValues.TabIndex = 16
        Me.fraStoredValues.TabStop = False
        Me.fraStoredValues.Text = "Stored Values"
        '
        'lstLookupValues
        '
        Me.lstLookupValues.BackColor = System.Drawing.SystemColors.Window
        Me.lstLookupValues.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstLookupValues.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lstLookupValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstLookupValues.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstLookupValues.Location = New System.Drawing.Point(9, 24)
        Me.lstLookupValues.Name = "lstLookupValues"
        Me.lstLookupValues.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me.lstLookupValues, System.Windows.Forms.SelectionMode.MultiExtended)
        Me.lstLookupValues.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstLookupValues.Size = New System.Drawing.Size(276, 186)
        Me.lstLookupValues.Sorted = True
        Me.lstLookupValues.TabIndex = 3
        '
        'cmdRemoveStored
        '
        Me.cmdRemoveStored.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemoveStored.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemoveStored.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemoveStored.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemoveStored.Location = New System.Drawing.Point(315, 308)
        Me.cmdRemoveStored.Name = "cmdRemoveStored"
        Me.cmdRemoveStored.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemoveStored.Size = New System.Drawing.Size(71, 22)
        Me.cmdRemoveStored.TabIndex = 17
        Me.cmdRemoveStored.Text = "Remove"
        Me.cmdRemoveStored.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemoveStored.UseVisualStyleBackColor = False
        '
        'pnlStatus
        '
        Me.pnlStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlStatus.Controls.Add(Me.lblStatus)
        Me.pnlStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlStatus.Location = New System.Drawing.Point(194, 157)
        Me.pnlStatus.Name = "pnlStatus"
        Me.pnlStatus.Size = New System.Drawing.Size(232, 64)
        Me.pnlStatus.TabIndex = 11
        Me.pnlStatus.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(14, 21)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(206, 34)
        Me.lblStatus.TabIndex = 12
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cmdSort
        '
        Me.cmdSort.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSort.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSort.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSort.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSort.Location = New System.Drawing.Point(240, 366)
        Me.cmdSort.Name = "cmdSort"
        Me.cmdSort.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSort.Size = New System.Drawing.Size(99, 29)
        Me.cmdSort.TabIndex = 9
        Me.cmdSort.Text = "Sort"
        Me.cmdSort.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSort.UseVisualStyleBackColor = False
        Me.cmdSort.Visible = False
        '
        'cmdWrite
        '
        Me.cmdWrite.BackColor = System.Drawing.SystemColors.Control
        Me.cmdWrite.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdWrite.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdWrite.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdWrite.Location = New System.Drawing.Point(120, 372)
        Me.cmdWrite.Name = "cmdWrite"
        Me.cmdWrite.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdWrite.Size = New System.Drawing.Size(106, 22)
        Me.cmdWrite.TabIndex = 7
        Me.cmdWrite.Text = "Write"
        Me.cmdWrite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdWrite.UseVisualStyleBackColor = False
        Me.cmdWrite.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(450, 372)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(85, 22)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(357, 372)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(85, 22)
        Me.cmdOK.TabIndex = 6
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmMaintain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(633, 398)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdSort)
        Me.Controls.Add(Me.cmdWrite)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMaintain"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.fraNewValues.ResumeLayout(False)
        Me.fraStoredValues.ResumeLayout(False)
        Me.pnlStatus.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class