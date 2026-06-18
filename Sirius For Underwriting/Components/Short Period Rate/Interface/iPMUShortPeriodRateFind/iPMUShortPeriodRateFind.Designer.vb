<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwShortPeriodRates_InitializeColumnKeys()
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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Private WithEvents _lvwShortPeriodRates_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwShortPeriodRates_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwShortPeriodRates_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwShortPeriodRates_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwShortPeriodRates_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwShortPeriodRates As System.Windows.Forms.ListView
	Public WithEvents txtFormatDate As System.Windows.Forms.TextBox
	Public WithEvents txtFormatPercent As System.Windows.Forms.TextBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents pnlProduct As System.Windows.Forms.Panel
    Public WithEvents lblProduct As System.Windows.Forms.Label
	Private WithEvents _tabMaintab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMaintab As System.Windows.Forms.TabControl
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMaintab = New System.Windows.Forms.TabControl
        Me._tabMaintab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lvwShortPeriodRates = New System.Windows.Forms.ListView
        Me._lvwShortPeriodRates_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwShortPeriodRates_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwShortPeriodRates_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwShortPeriodRates_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwShortPeriodRates_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.txtFormatDate = New System.Windows.Forms.TextBox
        Me.txtFormatPercent = New System.Windows.Forms.TextBox
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.pnlProduct = New System.Windows.Forms.Panel
        Me.lblProduct = New System.Windows.Forms.Label
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMaintab.SuspendLayout()
        Me._tabMaintab_TabPage0.SuspendLayout()
        Me.pnlProduct.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(433, 288)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 2
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
        Me.cmdCancel.Location = New System.Drawing.Point(345, 288)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
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
        Me.cmdOK.Location = New System.Drawing.Point(257, 288)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMaintab
        '
        Me.tabMaintab.Controls.Add(Me._tabMaintab_TabPage0)
        Me.tabMaintab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMaintab.ItemSize = New System.Drawing.Size(492, 18)
        Me.tabMaintab.Location = New System.Drawing.Point(13, 8)
        Me.tabMaintab.Multiline = True
        Me.tabMaintab.Name = "tabMaintab"
        Me.tabMaintab.SelectedIndex = 0
        Me.tabMaintab.Size = New System.Drawing.Size(497, 277)
        Me.tabMaintab.TabIndex = 3
        '
        '_tabMaintab_TabPage0
        '
        Me._tabMaintab_TabPage0.Controls.Add(Me.lvwShortPeriodRates)
        Me._tabMaintab_TabPage0.Controls.Add(Me.txtFormatDate)
        Me._tabMaintab_TabPage0.Controls.Add(Me.txtFormatPercent)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMaintab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMaintab_TabPage0.Controls.Add(Me.pnlProduct)
        Me._tabMaintab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMaintab_TabPage0.Name = "_tabMaintab_TabPage0"
        Me._tabMaintab_TabPage0.Size = New System.Drawing.Size(489, 251)
        Me._tabMaintab_TabPage0.TabIndex = 0
        Me._tabMaintab_TabPage0.Text = "1 - Short Period Rates"
        Me._tabMaintab_TabPage0.UseVisualStyleBackColor = True
        '
        'lvwShortPeriodRates
        '
        Me.lvwShortPeriodRates.BackColor = System.Drawing.SystemColors.Window
        Me.lvwShortPeriodRates.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwShortPeriodRates, "")
        Me.lvwShortPeriodRates.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwShortPeriodRates_ColumnHeader_1, Me._lvwShortPeriodRates_ColumnHeader_2, Me._lvwShortPeriodRates_ColumnHeader_3, Me._lvwShortPeriodRates_ColumnHeader_4, Me._lvwShortPeriodRates_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwShortPeriodRates, True)
        Me.lvwShortPeriodRates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwShortPeriodRates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwShortPeriodRates, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwShortPeriodRates, "")
        Me.lvwShortPeriodRates.LargeImageList = Me.ImageList1
        Me.lvwShortPeriodRates.Location = New System.Drawing.Point(8, 68)
        Me.lvwShortPeriodRates.Name = "lvwShortPeriodRates"
        Me.lvwShortPeriodRates.Size = New System.Drawing.Size(476, 145)
        Me.listViewHelper1.SetSmallIcons(Me.lvwShortPeriodRates, "")
        Me.lvwShortPeriodRates.SmallImageList = Me.ImageList1
        Me.listViewHelper1.SetSorted(Me.lvwShortPeriodRates, False)
        Me.listViewHelper1.SetSortKey(Me.lvwShortPeriodRates, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwShortPeriodRates, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwShortPeriodRates.TabIndex = 9
        Me.lvwShortPeriodRates.UseCompatibleStateImageBehavior = False
        Me.lvwShortPeriodRates.View = System.Windows.Forms.View.Details
        '
        '_lvwShortPeriodRates_ColumnHeader_1
        '
        Me._lvwShortPeriodRates_ColumnHeader_1.Tag = ""
        Me._lvwShortPeriodRates_ColumnHeader_1.Text = "Type"
        Me._lvwShortPeriodRates_ColumnHeader_1.Width = 139
        '
        '_lvwShortPeriodRates_ColumnHeader_2
        '
        Me._lvwShortPeriodRates_ColumnHeader_2.Tag = ""
        Me._lvwShortPeriodRates_ColumnHeader_2.Text = "Period"
        Me._lvwShortPeriodRates_ColumnHeader_2.Width = 101
        '
        '_lvwShortPeriodRates_ColumnHeader_3
        '
        Me._lvwShortPeriodRates_ColumnHeader_3.Tag = ""
        Me._lvwShortPeriodRates_ColumnHeader_3.Text = "Value"
        Me._lvwShortPeriodRates_ColumnHeader_3.Width = 97
        '
        '_lvwShortPeriodRates_ColumnHeader_4
        '
        Me._lvwShortPeriodRates_ColumnHeader_4.Tag = ""
        Me._lvwShortPeriodRates_ColumnHeader_4.Text = "Effective Date"
        Me._lvwShortPeriodRates_ColumnHeader_4.Width = 134
        '
        '_lvwShortPeriodRates_ColumnHeader_5
        '
        Me._lvwShortPeriodRates_ColumnHeader_5.Tag = ""
        Me._lvwShortPeriodRates_ColumnHeader_5.Text = "% Percentage"
        Me._lvwShortPeriodRates_ColumnHeader_5.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "FindImage")
        '
        'txtFormatDate
        '
        Me.txtFormatDate.AcceptsReturn = True
        Me.txtFormatDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatDate.Location = New System.Drawing.Point(8, 220)
        Me.txtFormatDate.MaxLength = 0
        Me.txtFormatDate.Name = "txtFormatDate"
        Me.txtFormatDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatDate.Size = New System.Drawing.Size(81, 20)
        Me.txtFormatDate.TabIndex = 4
        Me.txtFormatDate.Visible = False
        '
        'txtFormatPercent
        '
        Me.txtFormatPercent.AcceptsReturn = True
        Me.txtFormatPercent.BackColor = System.Drawing.SystemColors.Window
        Me.txtFormatPercent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFormatPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormatPercent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFormatPercent.Location = New System.Drawing.Point(112, 220)
        Me.txtFormatPercent.MaxLength = 0
        Me.txtFormatPercent.Name = "txtFormatPercent"
        Me.txtFormatPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFormatPercent.Size = New System.Drawing.Size(73, 20)
        Me.txtFormatPercent.TabIndex = 5
        Me.txtFormatPercent.Visible = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(339, 217)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 6
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(259, 217)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 7
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(419, 217)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(65, 22)
        Me.cmdDelete.TabIndex = 8
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'pnlProduct
        '
        Me.pnlProduct.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlProduct.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlProduct.Controls.Add(Me.lblProduct)
        Me.pnlProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlProduct.Location = New System.Drawing.Point(8, 28)
        Me.pnlProduct.Name = "pnlProduct"
        Me.pnlProduct.Size = New System.Drawing.Size(209, 22)
        Me.pnlProduct.TabIndex = 10
        '
        'lblProduct
        '
        Me.lblProduct.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblProduct.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProduct.Location = New System.Drawing.Point(-2, 0)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.Size = New System.Drawing.Size(209, 22)
        Me.lblProduct.TabIndex = 10
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(520, 318)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMaintab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Insurer"
        Me.tabMaintab.ResumeLayout(False)
        Me._tabMaintab_TabPage0.ResumeLayout(False)
        Me._tabMaintab_TabPage0.PerformLayout()
        Me.pnlProduct.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwShortPeriodRates_InitializeColumnKeys()
		Me._lvwShortPeriodRates_ColumnHeader_1.Name = ""
		Me._lvwShortPeriodRates_ColumnHeader_2.Name = ""
		Me._lvwShortPeriodRates_ColumnHeader_3.Name = ""
		Me._lvwShortPeriodRates_ColumnHeader_4.Name = ""
		Me._lvwShortPeriodRates_ColumnHeader_5.Name = ""
	End Sub
#End Region 
End Class