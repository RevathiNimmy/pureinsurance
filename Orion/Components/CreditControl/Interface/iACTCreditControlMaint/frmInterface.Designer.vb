<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwCreditControlRules_InitializeColumnKeys()
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
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdClose As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents txtHelper As System.Windows.Forms.TextBox
    Public WithEvents cboPMLookupSource As System.Windows.Forms.ComboBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lvwCreditControlRules = New System.Windows.Forms.ListView
        Me._lvwCreditControlRules_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCreditControlRules_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCreditControlRules_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCreditControlRules_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwCreditControlRules_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwCreditControlRules_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.lblBranch = New System.Windows.Forms.Label
        Me.txtHelper = New System.Windows.Forms.TextBox
        Me.cboPMLookupSource = New System.Windows.Forms.ComboBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(92, 340)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 6
        Me.cmdEdit.Tag = "CAP;205"
        Me.cmdEdit.Text = "Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(172, 340)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 7
        Me.cmdDelete.Tag = "CAP;204"
        Me.cmdDelete.Text = "Delete"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(420, 340)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 22)
        Me.cmdClose.TabIndex = 8
        Me.cmdClose.Tag = "CAP;207"
        Me.cmdClose.Text = "Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(12, 340)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 5
        Me.cmdAdd.Tag = "CAP;203"
        Me.cmdAdd.Text = "Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(95, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(12, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(485, 329)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.Tag = "CAP;101"
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwCreditControlRules)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBranch)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtHelper)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPMLookupSource)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(477, 303)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        '
        'lvwCreditControlRules
        '
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCreditControlRules, "")
        Me.lvwCreditControlRules.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCreditControlRules_ColumnHeader_1, Me._lvwCreditControlRules_ColumnHeader_2, Me._lvwCreditControlRules_ColumnHeader_3, Me._lvwCreditControlRules_ColumnHeader_4, Me._lvwCreditControlRules_ColumnHeader_5, Me._lvwCreditControlRules_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCreditControlRules, False)
        Me.lvwCreditControlRules.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCreditControlRules, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCreditControlRules, "")
        Me.lvwCreditControlRules.Location = New System.Drawing.Point(8, 52)
        Me.lvwCreditControlRules.Name = "lvwCreditControlRules"
        Me.lvwCreditControlRules.Size = New System.Drawing.Size(465, 245)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCreditControlRules, "")
        Me.listViewHelper1.SetSorted(Me.lvwCreditControlRules, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCreditControlRules, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCreditControlRules, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCreditControlRules.TabIndex = 5
        Me.lvwCreditControlRules.Tag = "CAP;150"
        Me.lvwCreditControlRules.UseCompatibleStateImageBehavior = False
        Me.lvwCreditControlRules.View = System.Windows.Forms.View.Details
        '
        '_lvwCreditControlRules_ColumnHeader_1
        '
        Me._lvwCreditControlRules_ColumnHeader_1.Text = "Description"
        Me._lvwCreditControlRules_ColumnHeader_1.Width = 141
        '
        '_lvwCreditControlRules_ColumnHeader_2
        '
        Me._lvwCreditControlRules_ColumnHeader_2.Text = "Business Type"
        Me._lvwCreditControlRules_ColumnHeader_2.Width = 94
        '
        '_lvwCreditControlRules_ColumnHeader_3
        '
        Me._lvwCreditControlRules_ColumnHeader_3.Text = "Frequency"
        Me._lvwCreditControlRules_ColumnHeader_3.Width = 94
        '
        '_lvwCreditControlRules_ColumnHeader_4
        '
        Me._lvwCreditControlRules_ColumnHeader_4.Text = "Active"
        Me._lvwCreditControlRules_ColumnHeader_4.Width = 54
        '
        '_lvwCreditControlRules_ColumnHeader_5
        '
        Me._lvwCreditControlRules_ColumnHeader_5.Tag = "HIDDEN"
        Me._lvwCreditControlRules_ColumnHeader_5.Text = "ID"
        Me._lvwCreditControlRules_ColumnHeader_5.Width = 0
        '
        '_lvwCreditControlRules_ColumnHeader_6
        '
        Me._lvwCreditControlRules_ColumnHeader_6.Text = "Use Effective Date"
        Me._lvwCreditControlRules_ColumnHeader_6.Width = 114
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(10, 15)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(73, 17)
        Me.lblBranch.TabIndex = 1
        Me.lblBranch.Tag = "CAP;502"
        Me.lblBranch.Text = "Branch"
        '
        'txtHelper
        '
        Me.txtHelper.AcceptsReturn = True
        Me.txtHelper.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtHelper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtHelper.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHelper.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHelper.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHelper.Location = New System.Drawing.Point(180, 96)
        Me.txtHelper.MaxLength = 0
        Me.txtHelper.Multiline = True
        Me.txtHelper.Name = "txtHelper"
        Me.txtHelper.ReadOnly = True
        Me.txtHelper.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHelper.Size = New System.Drawing.Size(125, 173)
        Me.txtHelper.TabIndex = 4
        Me.txtHelper.Visible = False
        '
        'cboPMLookupSource
        '
        Me.cboPMLookupSource.BackColor = System.Drawing.SystemColors.Window
        Me.cboPMLookupSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPMLookupSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPMLookupSource.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPMLookupSource.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboPMLookupSource.Location = New System.Drawing.Point(90, 12)
        Me.cboPMLookupSource.Name = "cboPMLookupSource"
        Me.cboPMLookupSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPMLookupSource.Size = New System.Drawing.Size(193, 21)
        Me.cboPMLookupSource.TabIndex = 2
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdClose
        Me.ClientSize = New System.Drawing.Size(505, 367)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Tag = "CAP;100"
        Me.Text = "Credit Control - Rules"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Sub lvwCreditControlRules_InitializeColumnKeys()
        Me._lvwCreditControlRules_ColumnHeader_1.Name = ""
        Me._lvwCreditControlRules_ColumnHeader_2.Name = ""
        Me._lvwCreditControlRules_ColumnHeader_3.Name = ""
        Me._lvwCreditControlRules_ColumnHeader_4.Name = ""
        Me._lvwCreditControlRules_ColumnHeader_5.Name = ""
        Me._lvwCreditControlRules_ColumnHeader_6.Name = ""
    End Sub
    Friend WithEvents lvwCreditControlRules As System.Windows.Forms.ListView
    Friend WithEvents _lvwCreditControlRules_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwCreditControlRules_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwCreditControlRules_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwCreditControlRules_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwCreditControlRules_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwCreditControlRules_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
#End Region
End Class