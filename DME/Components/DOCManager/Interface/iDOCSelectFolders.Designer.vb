<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectFolders
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwFolders_InitializeColumnKeys()
	End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdNewSearch As System.Windows.Forms.Button
    Public WithEvents cmdFind As System.Windows.Forms.Button
    Public WithEvents cmdClose As System.Windows.Forms.Button
    Public WithEvents cmdApply As System.Windows.Forms.Button
    Public WithEvents cmdSelectAll As System.Windows.Forms.Button
    Private WithEvents _ssbFolders_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents ssbFolders As System.Windows.Forms.StatusStrip
    Private WithEvents _lvwFolders_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwFolders_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwFolders As System.Windows.Forms.ListView
    Public WithEvents tabMain As System.Windows.Forms.TabControl
	Public WithEvents Label5 As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtCaption = New System.Windows.Forms.TextBox
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.cboCompany = New System.Windows.Forms.ComboBox
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFind = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdSelectAll = New System.Windows.Forms.Button
        Me.ssbFolders = New System.Windows.Forms.StatusStrip
        Me._ssbFolders_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwFolders = New System.Windows.Forms.ListView
        Me._lvwFolders_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwFolders_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame1 = New System.Windows.Forms.Panel
        Me.txtTotalChild = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.ssbFolders.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtCaption
        '
        Me.txtCaption.AcceptsReturn = True
        Me.txtCaption.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaption.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaption.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCaption.Location = New System.Drawing.Point(96, 32)
        Me.txtCaption.MaxLength = 0
        Me.txtCaption.Name = "txtCaption"
        Me.txtCaption.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaption.Size = New System.Drawing.Size(185, 20)
        Me.txtCaption.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.txtCaption, "Enter Caption to Search for, Use Wildcard *")
        '
        'txtNumber
        '
        Me.txtNumber.AcceptsReturn = True
        Me.txtNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNumber.Location = New System.Drawing.Point(96, 56)
        Me.txtNumber.MaxLength = 0
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNumber.Size = New System.Drawing.Size(40, 20)
        Me.txtNumber.TabIndex = 13
        Me.txtNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.txtNumber, "Enter Maximum Folders to Return or Leave Blank for Maximum")
        '
        'cboCompany
        '
        Me.cboCompany.BackColor = System.Drawing.SystemColors.Window
        Me.cboCompany.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCompany.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCompany.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCompany.Location = New System.Drawing.Point(96, 8)
        Me.cboCompany.Name = "cboCompany"
        Me.cboCompany.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCompany.Size = New System.Drawing.Size(185, 21)
        Me.cboCompany.TabIndex = 17
        Me.ToolTip1.SetToolTip(Me.cboCompany, "Select a company to search for folders")
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(336, 104)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 0
        Me.uctPMResizer1.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(160, 344)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 23)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "Ok"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(336, 62)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(74, 23)
        Me.cmdNewSearch.TabIndex = 1
        Me.cmdNewSearch.Text = "New Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFind
        '
        Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFind.Enabled = False
        Me.cmdFind.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFind.Location = New System.Drawing.Point(336, 30)
        Me.cmdFind.Name = "cmdFind"
        Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFind.Size = New System.Drawing.Size(74, 23)
        Me.cmdFind.TabIndex = 0
        Me.cmdFind.Text = "Find Now"
        Me.cmdFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFind.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(320, 344)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 23)
        Me.cmdClose.TabIndex = 6
        Me.cmdClose.Text = "Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdApply
        '
        Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdApply.Enabled = False
        Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApply.Location = New System.Drawing.Point(240, 344)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdApply.Size = New System.Drawing.Size(73, 23)
        Me.cmdApply.TabIndex = 5
        Me.cmdApply.Text = "Apply"
        Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdApply.UseVisualStyleBackColor = False
        '
        'cmdSelectAll
        '
        Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAll.Location = New System.Drawing.Point(8, 344)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAll.Size = New System.Drawing.Size(73, 23)
        Me.cmdSelectAll.TabIndex = 3
        Me.cmdSelectAll.Text = "Select All"
        Me.cmdSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelectAll.UseVisualStyleBackColor = False
        '
        'ssbFolders
        '
        Me.ssbFolders.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ssbFolders.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._ssbFolders_Panel1})
        Me.ssbFolders.Location = New System.Drawing.Point(0, 371)
        Me.ssbFolders.Name = "ssbFolders"
        Me.ssbFolders.ShowItemToolTips = True
        Me.ssbFolders.Size = New System.Drawing.Size(419, 22)
        Me.ssbFolders.TabIndex = 7
        '
        '_ssbFolders_Panel1
        '
        Me._ssbFolders_Panel1.AutoSize = False
        Me._ssbFolders_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._ssbFolders_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._ssbFolders_Panel1.DoubleClickEnabled = True
        Me._ssbFolders_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._ssbFolders_Panel1.Name = "_ssbFolders_Panel1"
        Me._ssbFolders_Panel1.Size = New System.Drawing.Size(96, 22)
        Me._ssbFolders_Panel1.Tag = ""
        Me._ssbFolders_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwFolders
        '
        Me.lvwFolders.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwFolders, "")
        Me.lvwFolders.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwFolders_ColumnHeader_1, Me._lvwFolders_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwFolders, True)
        Me.lvwFolders.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwFolders.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwFolders, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwFolders, "")
        Me.lvwFolders.Location = New System.Drawing.Point(8, 152)
        Me.lvwFolders.Name = "lvwFolders"
        Me.lvwFolders.Size = New System.Drawing.Size(385, 186)
        Me.listViewHelper1.SetSmallIcons(Me.lvwFolders, "")
        Me.listViewHelper1.SetSorted(Me.lvwFolders, False)
        Me.listViewHelper1.SetSortKey(Me.lvwFolders, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwFolders, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwFolders.TabIndex = 2
        Me.lvwFolders.UseCompatibleStateImageBehavior = False
        Me.lvwFolders.View = System.Windows.Forms.View.Details
        '
        '_lvwFolders_ColumnHeader_1
        '
        Me._lvwFolders_ColumnHeader_1.Tag = ""
        Me._lvwFolders_ColumnHeader_1.Text = "Folder Name"
        Me._lvwFolders_ColumnHeader_1.Width = 230
        '
        '_lvwFolders_ColumnHeader_2
        '
        Me._lvwFolders_ColumnHeader_2.Tag = ""
        Me._lvwFolders_ColumnHeader_2.Text = "Create Date"
        Me._lvwFolders_ColumnHeader_2.Width = 97
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(151, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(322, 120)
        Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMain.TabIndex = 9
        Me.tabMain.TabStop = False
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(314, 94)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Main"
        Me._tabMain_TabPage0.UseVisualStyleBackColor = True
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cboCompany)
        Me.Frame1.Controls.Add(Me.txtNumber)
        Me.Frame1.Controls.Add(Me.txtCaption)
        Me.Frame1.Controls.Add(Me.txtTotalChild)
        Me.Frame1.Controls.Add(Me.Label3)
        Me.Frame1.Controls.Add(Me.Label2)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Controls.Add(Me.Label6)
        Me.Frame1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(-4, 4)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(322, 81)
        Me.Frame1.TabIndex = 10
        Me.Frame1.Text = "Frame1"
        '
        'txtTotalChild
        '
        Me.txtTotalChild.AcceptsReturn = True
        Me.txtTotalChild.BackColor = System.Drawing.SystemColors.Menu
        Me.txtTotalChild.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTotalChild.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalChild.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTotalChild.Location = New System.Drawing.Point(240, 56)
        Me.txtTotalChild.MaxLength = 0
        Me.txtTotalChild.Name = "txtTotalChild"
        Me.txtTotalChild.ReadOnly = True
        Me.txtTotalChild.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotalChild.Size = New System.Drawing.Size(40, 20)
        Me.txtTotalChild.TabIndex = 11
        Me.txtTotalChild.TabStop = False
        Me.txtTotalChild.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(4, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(81, 17)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Company:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(1, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(97, 17)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Maximum Folders:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(4, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(77, 17)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Search For:"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(142, 64)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(92, 17)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Total Folders:"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(16, 128)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(257, 25)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Select folders to add to main view"
        '
        'frmSelectFolders
        '
        Me.AcceptButton = Me.cmdFind
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(419, 393)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFind)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.cmdSelectAll)
        Me.Controls.Add(Me.ssbFolders)
        Me.Controls.Add(Me.lvwFolders)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.Label5)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmSelectFolders"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Select Folders to Display for "
        Me.ssbFolders.ResumeLayout(False)
        Me.ssbFolders.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwFolders_InitializeColumnKeys()
		Me._lvwFolders_ColumnHeader_1.Name = ""
		Me._lvwFolders_ColumnHeader_2.Name = ""
    End Sub
    Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents Frame1 As System.Windows.Forms.Panel
    Public WithEvents cboCompany As System.Windows.Forms.ComboBox
    Public WithEvents txtNumber As System.Windows.Forms.TextBox
    Public WithEvents txtCaption As System.Windows.Forms.TextBox
    Public WithEvents txtTotalChild As System.Windows.Forms.TextBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
#End Region 
End Class