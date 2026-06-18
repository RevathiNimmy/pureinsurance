<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwClientInstalls_InitializeColumnKeys()
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
	Public CommonDialog1Open As System.Windows.Forms.OpenFileDialog
	Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox
	Private WithEvents _lvwClientInstalls_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClientInstalls_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClientInstalls_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClientInstalls_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClientInstalls_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClientInstalls_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwClientInstalls As System.Windows.Forms.ListView
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents cboPMLookup1 As PMLookupControl.cboPMLookup
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CommonDialog1Open = New System.Windows.Forms.OpenFileDialog
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lvwClientInstalls = New System.Windows.Forms.ListView
        Me._lvwClientInstalls_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwClientInstalls_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwClientInstalls_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwClientInstalls_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwClientInstalls_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwClientInstalls_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdView = New System.Windows.Forms.Button
        Me.cboPMLookup1 = New PMLookupControl.cboPMLookup
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(88, 352)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 0
        Me.uctPMResizer1.Visible = False
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 352)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 2
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
        Me.cmdOK.Location = New System.Drawing.Point(544, 352)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&Close"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(120, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(613, 341)
        Me.tabMainTab.TabIndex = 1
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwClientInstalls)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdView)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPMLookup1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(605, 315)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Installs"
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(568, 4)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'lvwClientInstalls
        '
        Me.lvwClientInstalls.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClientInstalls, "")
        Me.lvwClientInstalls.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClientInstalls_ColumnHeader_1, Me._lvwClientInstalls_ColumnHeader_2, Me._lvwClientInstalls_ColumnHeader_3, Me._lvwClientInstalls_ColumnHeader_4, Me._lvwClientInstalls_ColumnHeader_5, Me._lvwClientInstalls_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClientInstalls, False)
        Me.lvwClientInstalls.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClientInstalls.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClientInstalls, "")
        Me.lvwClientInstalls.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwClientInstalls, "")
        Me.lvwClientInstalls.Location = New System.Drawing.Point(8, 52)
        Me.lvwClientInstalls.Name = "lvwClientInstalls"
        Me.lvwClientInstalls.Size = New System.Drawing.Size(513, 257)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClientInstalls, "")
        Me.listViewHelper1.SetSorted(Me.lvwClientInstalls, False)
        Me.listViewHelper1.SetSortKey(Me.lvwClientInstalls, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClientInstalls, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClientInstalls.TabIndex = 3
        Me.lvwClientInstalls.UseCompatibleStateImageBehavior = False
        Me.lvwClientInstalls.View = System.Windows.Forms.View.Details
        '
        '_lvwClientInstalls_ColumnHeader_1
        '
        Me._lvwClientInstalls_ColumnHeader_1.Tag = ""
        Me._lvwClientInstalls_ColumnHeader_1.Text = "Product"
        Me._lvwClientInstalls_ColumnHeader_1.Width = 97
        '
        '_lvwClientInstalls_ColumnHeader_2
        '
        Me._lvwClientInstalls_ColumnHeader_2.Tag = ""
        Me._lvwClientInstalls_ColumnHeader_2.Text = "Client Version"
        Me._lvwClientInstalls_ColumnHeader_2.Width = 97
        '
        '_lvwClientInstalls_ColumnHeader_3
        '
        Me._lvwClientInstalls_ColumnHeader_3.Tag = ""
        Me._lvwClientInstalls_ColumnHeader_3.Text = "Description"
        Me._lvwClientInstalls_ColumnHeader_3.Width = 97
        '
        '_lvwClientInstalls_ColumnHeader_4
        '
        Me._lvwClientInstalls_ColumnHeader_4.Tag = ""
        Me._lvwClientInstalls_ColumnHeader_4.Text = "Mandatory"
        Me._lvwClientInstalls_ColumnHeader_4.Width = 97
        '
        '_lvwClientInstalls_ColumnHeader_5
        '
        Me._lvwClientInstalls_ColumnHeader_5.Tag = ""
        Me._lvwClientInstalls_ColumnHeader_5.Text = "Auto Installable"
        Me._lvwClientInstalls_ColumnHeader_5.Width = 97
        '
        '_lvwClientInstalls_ColumnHeader_6
        '
        Me._lvwClientInstalls_ColumnHeader_6.Tag = ""
        Me._lvwClientInstalls_ColumnHeader_6.Text = "Server Version"
        Me._lvwClientInstalls_ColumnHeader_6.Width = 97
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(528, 52)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 4
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(528, 84)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(73, 22)
        Me.cmdView.TabIndex = 5
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cboPMLookup1
        '
        Me.cboPMLookup1.DefaultItemId = 0
        Me.cboPMLookup1.FirstItem = ""
        Me.cboPMLookup1.ItemId = 0
        Me.cboPMLookup1.ListIndex = -1
        Me.cboPMLookup1.Location = New System.Drawing.Point(8, 20)
        Me.cboPMLookup1.Name = "cboPMLookup1"
        Me.cboPMLookup1.PMLookupProductFamily = 1
        Me.cboPMLookup1.SingleItemId = 0
        Me.cboPMLookup1.Size = New System.Drawing.Size(84, 21)
        Me.cboPMLookup1.Sorted = True
        Me.cboPMLookup1.TabIndex = 6
        Me.cboPMLookup1.TableName = "PMProduct"
        Me.cboPMLookup1.ToolTipText = ""
        Me.cboPMLookup1.Visible = False
        Me.cboPMLookup1.WhereClause = ""
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(528, 116)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 7
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(626, 381)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(204, 164)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Client Install"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwClientInstalls_InitializeColumnKeys()
		Me._lvwClientInstalls_ColumnHeader_1.Name = ""
		Me._lvwClientInstalls_ColumnHeader_2.Name = ""
		Me._lvwClientInstalls_ColumnHeader_3.Name = ""
		Me._lvwClientInstalls_ColumnHeader_4.Name = ""
		Me._lvwClientInstalls_ColumnHeader_5.Name = ""
		Me._lvwClientInstalls_ColumnHeader_6.Name = ""
	End Sub
#End Region 
End Class