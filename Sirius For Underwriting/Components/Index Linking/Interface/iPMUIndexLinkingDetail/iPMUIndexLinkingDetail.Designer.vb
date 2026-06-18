<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwIndexLinkingDetail_InitializeColumnKeys()
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
	Public WithEvents lblIndexLinking As System.Windows.Forms.Label
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _lvwIndexLinkingDetail_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwIndexLinkingDetail_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwIndexLinkingDetail As System.Windows.Forms.ListView
	Public WithEvents pnlIndexLinking As System.Windows.Forms.Panel
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblPercentage As System.Windows.Forms.Label
	Public WithEvents cmdDetailOK As System.Windows.Forms.Button
	Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
	Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
	Public WithEvents txtPercentage As System.Windows.Forms.TextBox
	Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
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
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblIndexLinking = New System.Windows.Forms.Label
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.lvwIndexLinkingDetail = New System.Windows.Forms.ListView
        Me._lvwIndexLinkingDetail_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwIndexLinkingDetail_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.pnlIndexLinking = New System.Windows.Forms.Panel
        Me.Label1IndexLinking = New System.Windows.Forms.Label
        Me.tabDetailTab = New System.Windows.Forms.TabControl
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblPercentage = New System.Windows.Forms.Label
        Me.cmdDetailOK = New System.Windows.Forms.Button
        Me.cmdDetailCancel = New System.Windows.Forms.Button
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox
        Me.txtPercentage = New System.Windows.Forms.TextBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        'Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.pnlIndexLinking.SuspendLayout()
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(536, 296)
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
        Me.cmdCancel.Location = New System.Drawing.Point(456, 296)
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
        Me.cmdOK.Location = New System.Drawing.Point(368, 296)
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
        Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 277)
        Me.tabMainTab.TabIndex = 12
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblIndexLinking)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwIndexLinkingDetail)
        Me._tabMainTab_TabPage0.Controls.Add(Me.pnlIndexLinking)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 251)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Index Linking"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblIndexLinking
        '
        Me.lblIndexLinking.BackColor = System.Drawing.SystemColors.Control
        Me.lblIndexLinking.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIndexLinking.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIndexLinking.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIndexLinking.Location = New System.Drawing.Point(8, 20)
        Me.lblIndexLinking.Name = "lblIndexLinking"
        Me.lblIndexLinking.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIndexLinking.Size = New System.Drawing.Size(89, 17)
        Me.lblIndexLinking.TabIndex = 13
        Me.lblIndexLinking.Text = "Index Linking:"
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(520, 220)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(65, 22)
        Me.cmdDelete.TabIndex = 2
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(352, 220)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 0
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(440, 220)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 1
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'lvwIndexLinkingDetail
        '
        Me.lvwIndexLinkingDetail.BackColor = System.Drawing.SystemColors.Window
        Me.lvwIndexLinkingDetail.BorderStyle = System.Windows.Forms.BorderStyle.None
        'Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwIndexLinkingDetail, "")
        Me.lvwIndexLinkingDetail.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwIndexLinkingDetail_ColumnHeader_1, Me._lvwIndexLinkingDetail_ColumnHeader_2})
        'Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwIndexLinkingDetail, False)
        Me.lvwIndexLinkingDetail.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwIndexLinkingDetail.ForeColor = System.Drawing.SystemColors.WindowText
        'Me.listViewHelper1.SetItemClickMethod(Me.lvwIndexLinkingDetail, "")
        'Me.listViewHelper1.SetLargeIcons(Me.lvwIndexLinkingDetail, "")
        Me.lvwIndexLinkingDetail.LargeImageList = Me.ImageList1
        Me.lvwIndexLinkingDetail.Location = New System.Drawing.Point(8, 44)
        Me.lvwIndexLinkingDetail.Name = "lvwIndexLinkingDetail"
        Me.lvwIndexLinkingDetail.Size = New System.Drawing.Size(577, 169)
        'Me.listViewHelper1.SetSmallIcons(Me.lvwIndexLinkingDetail, "")
        Me.lvwIndexLinkingDetail.SmallImageList = Me.ImageList1
        'Me.listViewHelper1.SetSorted(Me.lvwIndexLinkingDetail, False)
        'Me.listViewHelper1.SetSortKey(Me.lvwIndexLinkingDetail, 0)
        'Me.listViewHelper1.SetSortOrder(Me.lvwIndexLinkingDetail, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwIndexLinkingDetail.TabIndex = 7
        Me.lvwIndexLinkingDetail.UseCompatibleStateImageBehavior = False
        Me.lvwIndexLinkingDetail.View = System.Windows.Forms.View.Details
        '
        '_lvwIndexLinkingDetail_ColumnHeader_1
        '
        Me._lvwIndexLinkingDetail_ColumnHeader_1.Tag = ""
        Me._lvwIndexLinkingDetail_ColumnHeader_1.Text = ""
        Me._lvwIndexLinkingDetail_ColumnHeader_1.Width = 97
        '
        '_lvwIndexLinkingDetail_ColumnHeader_2
        '
        Me._lvwIndexLinkingDetail_ColumnHeader_2.Tag = ""
        Me._lvwIndexLinkingDetail_ColumnHeader_2.Text = ""
        Me._lvwIndexLinkingDetail_ColumnHeader_2.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "FindImage")
        '
        'pnlIndexLinking
        '
        Me.pnlIndexLinking.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlIndexLinking.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlIndexLinking.Controls.Add(Me.Label1IndexLinking)
        Me.pnlIndexLinking.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlIndexLinking.Location = New System.Drawing.Point(103, 16)
        Me.pnlIndexLinking.Name = "pnlIndexLinking"
        Me.pnlIndexLinking.Size = New System.Drawing.Size(226, 22)
        Me.pnlIndexLinking.TabIndex = 14
        '
        'Label1IndexLinking
        '
        Me.Label1IndexLinking.AutoSize = True
        Me.Label1IndexLinking.Location = New System.Drawing.Point(4, 3)
        Me.Label1IndexLinking.Name = "Label1IndexLinking"
        Me.Label1IndexLinking.Size = New System.Drawing.Size(0, 13)
        Me.Label1IndexLinking.TabIndex = 0
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(600, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(52, 310)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(605, 285)
        Me.tabDetailTab.TabIndex = 11
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblPercentage)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtPercentage)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(597, 259)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "2 - Index Linking Detail"
        Me._tabDetailTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(24, 28)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(145, 17)
        Me.lblEffectiveDate.TabIndex = 15
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'lblPercentage
        '
        Me.lblPercentage.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercentage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercentage.Location = New System.Drawing.Point(24, 60)
        Me.lblPercentage.Name = "lblPercentage"
        Me.lblPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercentage.Size = New System.Drawing.Size(145, 17)
        Me.lblPercentage.TabIndex = 16
        Me.lblPercentage.Text = "Percentage:"
        '
        'cmdDetailOK
        '
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(448, 228)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailOK.TabIndex = 5
        Me.cmdDetailOK.Text = "&OK"
        Me.cmdDetailOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailOK.UseVisualStyleBackColor = False
        '
        'cmdDetailCancel
        '
        Me.cmdDetailCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailCancel.Location = New System.Drawing.Point(528, 228)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(65, 22)
        Me.cmdDetailCancel.TabIndex = 6
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(200, 28)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(153, 20)
        Me.txtEffectiveDate.TabIndex = 3
        Me.txtEffectiveDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPercentage
        '
        Me.txtPercentage.AcceptsReturn = True
        Me.txtPercentage.BackColor = System.Drawing.SystemColors.Window
        Me.txtPercentage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPercentage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPercentage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPercentage.Location = New System.Drawing.Point(200, 60)
        Me.txtPercentage.MaxLength = 0
        Me.txtPercentage.Name = "txtPercentage"
        Me.txtPercentage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPercentage.Size = New System.Drawing.Size(153, 20)
        Me.txtPercentage.TabIndex = 4
        Me.txtPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(614, 324)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "RI Model Lines"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.pnlIndexLinking.ResumeLayout(False)
        Me.pnlIndexLinking.PerformLayout()
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwIndexLinkingDetail_InitializeColumnKeys()
		Me._lvwIndexLinkingDetail_ColumnHeader_1.Name = ""
		Me._lvwIndexLinkingDetail_ColumnHeader_2.Name = ""
    End Sub
    Friend WithEvents Label1IndexLinking As System.Windows.Forms.Label
#End Region 
End Class