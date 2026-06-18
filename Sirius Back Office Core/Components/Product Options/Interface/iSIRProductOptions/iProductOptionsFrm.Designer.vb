<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwSettings_InitializeColumnKeys()
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
	Public WithEvents cmdExport As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdImport As System.Windows.Forms.Button
	Public WithEvents cmdDisable As System.Windows.Forms.Button
	Public WithEvents lblHousekeeping As System.Windows.Forms.Label
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Private WithEvents _lvwSettings_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSettings_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSettings_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSettings As System.Windows.Forms.ListView
	Public WithEvents txtValue As System.Windows.Forms.TextBox
	Public WithEvents cboBranch As System.Windows.Forms.ComboBox
	Public WithEvents cmdSet As System.Windows.Forms.Button
	Public cdgFileOpen As System.Windows.Forms.OpenFileDialog
	Public cdgFileSave As System.Windows.Forms.SaveFileDialog
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExport = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdImport = New System.Windows.Forms.Button
        Me.cmdDisable = New System.Windows.Forms.Button
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblHousekeeping = New System.Windows.Forms.Label
        Me.lblBranch = New System.Windows.Forms.Label
        Me.lvwSettings = New System.Windows.Forms.ListView
        Me._lvwSettings_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSettings_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSettings_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me.txtValue = New System.Windows.Forms.TextBox
        Me.cboBranch = New System.Windows.Forms.ComboBox
        Me.cmdSet = New System.Windows.Forms.Button
        Me.cdgFileOpen = New System.Windows.Forms.OpenFileDialog
        Me.cdgFileSave = New System.Windows.Forms.SaveFileDialog
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        'Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdExport
        '
        Me.cmdExport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExport.Location = New System.Drawing.Point(88, 344)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExport.Size = New System.Drawing.Size(73, 22)
        Me.cmdExport.TabIndex = 12
        Me.cmdExport.Text = "E&xport"
        Me.cmdExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExport.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(432, 344)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 11
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdImport
        '
        Me.cmdImport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdImport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdImport.Location = New System.Drawing.Point(8, 344)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdImport.Size = New System.Drawing.Size(73, 22)
        Me.cmdImport.TabIndex = 10
        Me.cmdImport.Text = "&Import"
        Me.cmdImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdImport.UseVisualStyleBackColor = False
        '
        'cmdDisable
        '
        Me.cmdDisable.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDisable.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDisable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDisable.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDisable.Location = New System.Drawing.Point(223, 276)
        Me.cmdDisable.Name = "cmdDisable"
        Me.cmdDisable.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDisable.Size = New System.Drawing.Size(73, 22)
        Me.cmdDisable.TabIndex = 9
        Me.cmdDisable.Text = "&Disable"
        Me.cmdDisable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDisable.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(164, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(501, 333)
        Me.tabMain.TabIndex = 4
        Me.tabMain.TabStop = False
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.lblHousekeeping)
        Me._tabMain_TabPage0.Controls.Add(Me.lblBranch)
        Me._tabMain_TabPage0.Controls.Add(Me.lvwSettings)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdDisable)
        Me._tabMain_TabPage0.Controls.Add(Me.txtValue)
        Me._tabMain_TabPage0.Controls.Add(Me.cboBranch)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdSet)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(493, 307)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Product Options"
        '
        'lblHousekeeping
        '
        Me.lblHousekeeping.AutoSize = True
        Me.lblHousekeeping.BackColor = System.Drawing.SystemColors.Control
        Me.lblHousekeeping.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHousekeeping.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHousekeeping.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHousekeeping.Location = New System.Drawing.Point(16, 276)
        Me.lblHousekeeping.Name = "lblHousekeeping"
        Me.lblHousekeeping.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHousekeeping.Size = New System.Drawing.Size(64, 13)
        Me.lblHousekeeping.TabIndex = 5
        Me.lblHousekeeping.Text = "Value Input:"
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(16, 12)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(44, 13)
        Me.lblBranch.TabIndex = 7
        Me.lblBranch.Text = "Branch:"
        '
        'lvwSettings
        '
        Me.lvwSettings.BackColor = System.Drawing.SystemColors.Window
        'Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSettings, "")
        Me.lvwSettings.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSettings_ColumnHeader_1, Me._lvwSettings_ColumnHeader_2, Me._lvwSettings_ColumnHeader_3})
        'Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSettings, False)
        Me.lvwSettings.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSettings.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSettings.FullRowSelect = True
        Me.lvwSettings.GridLines = True
        Me.lvwSettings.HideSelection = False
        'Me.listViewHelper1.SetItemClickMethod(Me.lvwSettings, "")
        'Me.listViewHelper1.SetLargeIcons(Me.lvwSettings, "")
        Me.lvwSettings.Location = New System.Drawing.Point(8, 36)
        Me.lvwSettings.Name = "lvwSettings"
        Me.lvwSettings.Size = New System.Drawing.Size(481, 233)
        'Me.listViewHelper1.SetSmallIcons(Me.lvwSettings, "")
        'Me.listViewHelper1.SetSorted(Me.lvwSettings, False)
        'Me.listViewHelper1.SetSortKey(Me.lvwSettings, 0)
        'Me.listViewHelper1.SetSortOrder(Me.lvwSettings, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSettings.TabIndex = 0
        Me.lvwSettings.UseCompatibleStateImageBehavior = False
        Me.lvwSettings.View = System.Windows.Forms.View.Details
        '
        '_lvwSettings_ColumnHeader_1
        '
        Me._lvwSettings_ColumnHeader_1.Tag = ""
        Me._lvwSettings_ColumnHeader_1.Text = "Option Name"
        Me._lvwSettings_ColumnHeader_1.Width = 97
        '
        '_lvwSettings_ColumnHeader_2
        '
        Me._lvwSettings_ColumnHeader_2.Tag = ""
        Me._lvwSettings_ColumnHeader_2.Text = "Value"
        Me._lvwSettings_ColumnHeader_2.Width = 97
        '
        '_lvwSettings_ColumnHeader_3
        '
        Me._lvwSettings_ColumnHeader_3.Tag = ""
        Me._lvwSettings_ColumnHeader_3.Text = "Settings"
        Me._lvwSettings_ColumnHeader_3.Width = 97
        '
        'txtValue
        '
        Me.txtValue.AcceptsReturn = True
        Me.txtValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtValue.Location = New System.Drawing.Point(96, 276)
        Me.txtValue.MaxLength = 0
        Me.txtValue.Name = "txtValue"
        Me.txtValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtValue.Size = New System.Drawing.Size(33, 20)
        Me.txtValue.TabIndex = 1
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(96, 12)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(129, 21)
        Me.cboBranch.TabIndex = 6
        '
        'cmdSet
        '
        Me.cmdSet.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSet.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSet.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSet.Location = New System.Drawing.Point(144, 276)
        Me.cmdSet.Name = "cmdSet"
        Me.cmdSet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSet.Size = New System.Drawing.Size(73, 22)
        Me.cmdSet.TabIndex = 8
        Me.cmdSet.Text = "&Set Value"
        Me.cmdSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSet.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(352, 344)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(73, 22)
        Me.cmdExit.TabIndex = 3
        Me.cmdExit.Text = "&Exit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(272, 344)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(513, 372)
        Me.Controls.Add(Me.cmdExport)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdImport)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(11, 27)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Product Option Settings"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._tabMain_TabPage0.PerformLayout()
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwSettings_InitializeColumnKeys()
		Me._lvwSettings_ColumnHeader_1.Name = ""
		Me._lvwSettings_ColumnHeader_2.Name = "C5"
		Me._lvwSettings_ColumnHeader_3.Name = ""
	End Sub
#End Region 
End Class