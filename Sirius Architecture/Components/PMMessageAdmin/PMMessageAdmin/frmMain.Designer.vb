<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwMessages_InitializeColumnKeys()
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
	Public WithEvents lblFilter As System.Windows.Forms.Label
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Public WithEvents lblNumberOfRecords As System.Windows.Forms.Label
	Private WithEvents _lvwMessages_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwMessages_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwMessages_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwMessages_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwMessages_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwMessages_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwMessages_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwMessages_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwMessages_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwMessages_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwMessages As System.Windows.Forms.ListView
	Public WithEvents cmdRefresh As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cboFilter As System.Windows.Forms.ComboBox
	Public WithEvents cboNumberOfRecords As System.Windows.Forms.ComboBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblFilter = New System.Windows.Forms.Label
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblNumberOfRecords = New System.Windows.Forms.Label
        Me.lvwMessages = New System.Windows.Forms.ListView
        Me._lvwMessages_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwMessages_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwMessages_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwMessages_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwMessages_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwMessages_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwMessages_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwMessages_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwMessages_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwMessages_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me.cmdRefresh = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cboFilter = New System.Windows.Forms.ComboBox
        Me.cboNumberOfRecords = New System.Windows.Forms.ComboBox
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.cmdOK = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(212, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(645, 381)
        Me.tabMain.TabIndex = 5
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.lblFilter)
        Me._tabMain_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMain_TabPage0.Controls.Add(Me.lblNumberOfRecords)
        Me._tabMain_TabPage0.Controls.Add(Me.lvwMessages)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdRefresh)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMain_TabPage0.Controls.Add(Me.cboFilter)
        Me._tabMain_TabPage0.Controls.Add(Me.cboNumberOfRecords)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(637, 355)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Messages"
        '
        'lblFilter
        '
        Me.lblFilter.AutoSize = True
        Me.lblFilter.BackColor = System.Drawing.SystemColors.Control
        Me.lblFilter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFilter.Location = New System.Drawing.Point(16, 16)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFilter.Size = New System.Drawing.Size(32, 13)
        Me.lblFilter.TabIndex = 6
        Me.lblFilter.Text = "Filter:"
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(576, 44)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 7
        Me.imgIcon.TabStop = False
        '
        'lblNumberOfRecords
        '
        Me.lblNumberOfRecords.AutoSize = True
        Me.lblNumberOfRecords.BackColor = System.Drawing.SystemColors.Control
        Me.lblNumberOfRecords.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNumberOfRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumberOfRecords.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNumberOfRecords.Location = New System.Drawing.Point(328, 16)
        Me.lblNumberOfRecords.Name = "lblNumberOfRecords"
        Me.lblNumberOfRecords.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNumberOfRecords.Size = New System.Drawing.Size(102, 13)
        Me.lblNumberOfRecords.TabIndex = 7
        Me.lblNumberOfRecords.Text = "Number of Records:"
        '
        'lvwMessages
        '
        Me.lvwMessages.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwMessages, "")
        Me.lvwMessages.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwMessages_ColumnHeader_1, Me._lvwMessages_ColumnHeader_2, Me._lvwMessages_ColumnHeader_3, Me._lvwMessages_ColumnHeader_4, Me._lvwMessages_ColumnHeader_5, Me._lvwMessages_ColumnHeader_6, Me._lvwMessages_ColumnHeader_7, Me._lvwMessages_ColumnHeader_8, Me._lvwMessages_ColumnHeader_9, Me._lvwMessages_ColumnHeader_10})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwMessages, False)
        Me.lvwMessages.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwMessages.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwMessages, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwMessages, "")
        Me.lvwMessages.Location = New System.Drawing.Point(8, 44)
        Me.lvwMessages.Name = "lvwMessages"
        Me.lvwMessages.Size = New System.Drawing.Size(545, 305)
        Me.listViewHelper1.SetSmallIcons(Me.lvwMessages, "")
        Me.listViewHelper1.SetSorted(Me.lvwMessages, False)
        Me.listViewHelper1.SetSortKey(Me.lvwMessages, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwMessages, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwMessages.TabIndex = 4
        Me.lvwMessages.UseCompatibleStateImageBehavior = False
        Me.lvwMessages.View = System.Windows.Forms.View.Details
        '
        '_lvwMessages_ColumnHeader_1
        '
        Me._lvwMessages_ColumnHeader_1.Tag = ""
        Me._lvwMessages_ColumnHeader_1.Text = "Type"
        Me._lvwMessages_ColumnHeader_1.Width = 61
        '
        '_lvwMessages_ColumnHeader_2
        '
        Me._lvwMessages_ColumnHeader_2.Tag = ""
        Me._lvwMessages_ColumnHeader_2.Text = "Date"
        Me._lvwMessages_ColumnHeader_2.Width = 94
        '
        '_lvwMessages_ColumnHeader_3
        '
        Me._lvwMessages_ColumnHeader_3.Tag = ""
        Me._lvwMessages_ColumnHeader_3.Text = "User"
        Me._lvwMessages_ColumnHeader_3.Width = 21
        '
        '_lvwMessages_ColumnHeader_4
        '
        Me._lvwMessages_ColumnHeader_4.Tag = ""
        Me._lvwMessages_ColumnHeader_4.Text = "Message"
        Me._lvwMessages_ColumnHeader_4.Width = 174
        '
        '_lvwMessages_ColumnHeader_5
        '
        Me._lvwMessages_ColumnHeader_5.Tag = ""
        Me._lvwMessages_ColumnHeader_5.Text = "Err.Number"
        Me._lvwMessages_ColumnHeader_5.Width = 54
        '
        '_lvwMessages_ColumnHeader_6
        '
        Me._lvwMessages_ColumnHeader_6.Tag = ""
        Me._lvwMessages_ColumnHeader_6.Text = "Err.Description"
        Me._lvwMessages_ColumnHeader_6.Width = 67
        '
        '_lvwMessages_ColumnHeader_7
        '
        Me._lvwMessages_ColumnHeader_7.Tag = ""
        Me._lvwMessages_ColumnHeader_7.Text = "Calling App Name"
        Me._lvwMessages_ColumnHeader_7.Width = 54
        '
        '_lvwMessages_ColumnHeader_8
        '
        Me._lvwMessages_ColumnHeader_8.Tag = ""
        Me._lvwMessages_ColumnHeader_8.Text = "App Name"
        Me._lvwMessages_ColumnHeader_8.Width = 97
        '
        '_lvwMessages_ColumnHeader_9
        '
        Me._lvwMessages_ColumnHeader_9.Tag = ""
        Me._lvwMessages_ColumnHeader_9.Text = "Class Name"
        Me._lvwMessages_ColumnHeader_9.Width = 97
        '
        '_lvwMessages_ColumnHeader_10
        '
        Me._lvwMessages_ColumnHeader_10.Tag = ""
        Me._lvwMessages_ColumnHeader_10.Text = "Method Name"
        Me._lvwMessages_ColumnHeader_10.Width = 97
        '
        'cmdRefresh
        '
        Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefresh.Location = New System.Drawing.Point(560, 100)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefresh.Size = New System.Drawing.Size(73, 22)
        Me.cmdRefresh.TabIndex = 1
        Me.cmdRefresh.Text = "&Refresh"
        Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRefresh.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(560, 140)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 2
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cboFilter
        '
        Me.cboFilter.BackColor = System.Drawing.SystemColors.Window
        Me.cboFilter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFilter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboFilter.Location = New System.Drawing.Point(64, 12)
        Me.cboFilter.Name = "cboFilter"
        Me.cboFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboFilter.Size = New System.Drawing.Size(185, 21)
        Me.cboFilter.TabIndex = 3
        '
        'cboNumberOfRecords
        '
        Me.cboNumberOfRecords.BackColor = System.Drawing.SystemColors.Window
        Me.cboNumberOfRecords.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboNumberOfRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboNumberOfRecords.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboNumberOfRecords.Location = New System.Drawing.Point(456, 12)
        Me.cboNumberOfRecords.Name = "cboNumberOfRecords"
        Me.cboNumberOfRecords.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboNumberOfRecords.Size = New System.Drawing.Size(97, 21)
        Me.cboNumberOfRecords.TabIndex = 8
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(264, 392)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 6
        Me.uctPMResizer1.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(576, 392)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmMain
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(658, 421)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmMain"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Message Viewer"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._tabMain_TabPage0.PerformLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwMessages_InitializeColumnKeys()
		Me._lvwMessages_ColumnHeader_1.Name = "Type"
		Me._lvwMessages_ColumnHeader_2.Name = "Date"
		Me._lvwMessages_ColumnHeader_3.Name = "User"
		Me._lvwMessages_ColumnHeader_4.Name = "Message"
		Me._lvwMessages_ColumnHeader_5.Name = "ErrNumber"
		Me._lvwMessages_ColumnHeader_6.Name = "ErrDescription"
		Me._lvwMessages_ColumnHeader_7.Name = "CallingAppName"
		Me._lvwMessages_ColumnHeader_8.Name = "AppName"
		Me._lvwMessages_ColumnHeader_9.Name = "ClassName"
		Me._lvwMessages_ColumnHeader_10.Name = "MethodName"
	End Sub
#End Region 
End Class