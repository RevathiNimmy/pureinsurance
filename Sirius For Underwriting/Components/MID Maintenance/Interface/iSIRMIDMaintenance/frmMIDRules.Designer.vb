<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMIDRules
    Inherits System.Windows.Forms.Form
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        lvwMIDRules_InitializeColumnKeys()
        Form_Initialize_Renamed()
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lvwMIDRules = New System.Windows.Forms.ListView()
        Me._lvwMIDRules_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMIDRules_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMIDRules_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMIDRules_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMIDRules_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMIDRules_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.is_deleted = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblBranch = New System.Windows.Forms.Label()
        Me.cboPMLookupSource = New System.Windows.Forms.ComboBox()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.tabMainTab.Location = New System.Drawing.Point(12, 7)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(481, 325)
        Me.tabMainTab.TabIndex = 13
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwMIDRules)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBranch)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPMLookupSource)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Padding = New System.Windows.Forms.Padding(3)
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(473, 299)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lvwMIDRules
        '
        Me.lvwMIDRules.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwMIDRules_ColumnHeader_1, Me._lvwMIDRules_ColumnHeader_2, Me._lvwMIDRules_ColumnHeader_3, Me._lvwMIDRules_ColumnHeader_4, Me._lvwMIDRules_ColumnHeader_5, Me._lvwMIDRules_ColumnHeader_6, Me.is_deleted})
        Me.lvwMIDRules.FullRowSelect = True
        Me.lvwMIDRules.Location = New System.Drawing.Point(0, 54)
        Me.lvwMIDRules.MultiSelect = False
        Me.lvwMIDRules.Name = "lvwMIDRules"
        Me.lvwMIDRules.Size = New System.Drawing.Size(465, 245)
        Me.lvwMIDRules.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwMIDRules.TabIndex = 6
        Me.lvwMIDRules.Tag = "CAP;150"
        Me.lvwMIDRules.UseCompatibleStateImageBehavior = False
        Me.lvwMIDRules.View = System.Windows.Forms.View.Details
        '
        '_lvwMIDRules_ColumnHeader_1
        '
        Me._lvwMIDRules_ColumnHeader_1.Text = "Code"
        Me._lvwMIDRules_ColumnHeader_1.Width = 90
        '
        '_lvwMIDRules_ColumnHeader_2
        '
        Me._lvwMIDRules_ColumnHeader_2.Text = "Description"
        Me._lvwMIDRules_ColumnHeader_2.Width = 170
        '
        '_lvwMIDRules_ColumnHeader_3
        '
        Me._lvwMIDRules_ColumnHeader_3.Text = "Effective Date"
        Me._lvwMIDRules_ColumnHeader_3.Width = 90
        '
        '_lvwMIDRules_ColumnHeader_4
        '
        Me._lvwMIDRules_ColumnHeader_4.Text = "MID Type"
        Me._lvwMIDRules_ColumnHeader_4.Width = 70
        '
        '_lvwMIDRules_ColumnHeader_5
        '
        Me._lvwMIDRules_ColumnHeader_5.Tag = ""
        Me._lvwMIDRules_ColumnHeader_5.Text = "Supplier Type"
        Me._lvwMIDRules_ColumnHeader_5.Width = 100
        '
        '_lvwMIDRules_ColumnHeader_6
        '
        Me._lvwMIDRules_ColumnHeader_6.Width = 0
        '
        'is_deleted
        '
        Me.is_deleted.Width = 0
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
        Me.lblBranch.TabIndex = 3
        Me.lblBranch.Tag = "CAP;502"
        Me.lblBranch.Text = "Branch:"
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
        Me.cboPMLookupSource.TabIndex = 4
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(92, 338)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 15
        Me.cmdEdit.Tag = ""
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
        Me.cmdDelete.Location = New System.Drawing.Point(172, 338)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 16
        Me.cmdDelete.Tag = ""
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
        Me.cmdClose.Location = New System.Drawing.Point(420, 338)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 22)
        Me.cmdClose.TabIndex = 17
        Me.cmdClose.Tag = ""
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
        Me.cmdAdd.Location = New System.Drawing.Point(12, 338)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 14
        Me.cmdAdd.Tag = ""
        Me.cmdAdd.Text = "Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'frmMIDRules
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(505, 367)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdAdd)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMIDRules"
        Me.Text = "MID - Rules"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents lvwMIDRules As System.Windows.Forms.ListView
    Friend WithEvents _lvwMIDRules_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwMIDRules_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwMIDRules_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwMIDRules_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwMIDRules_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents cboPMLookupSource As System.Windows.Forms.ComboBox
    Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents cmdClose As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Sub lvwMIDRules_InitializeColumnKeys()
        Me._lvwMIDRules_ColumnHeader_1.Name = ""
        Me._lvwMIDRules_ColumnHeader_2.Name = ""
        Me._lvwMIDRules_ColumnHeader_3.Name = ""
        Me._lvwMIDRules_ColumnHeader_4.Name = ""
        Me._lvwMIDRules_ColumnHeader_5.Name = ""
    End Sub
    Friend WithEvents _lvwMIDRules_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents is_deleted As System.Windows.Forms.ColumnHeader

End Class
