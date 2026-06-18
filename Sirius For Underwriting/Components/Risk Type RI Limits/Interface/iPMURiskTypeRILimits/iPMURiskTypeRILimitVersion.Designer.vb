<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface_Renamed
    Inherits System.Windows.Forms.Form
    Public Sub New()
        MyBase.New()
        InitializeComponent()
        Form_Initialize_Renamed()
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
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.lblModelUsage = New System.Windows.Forms.Label()
        Me.lblRiskType = New System.Windows.Forms.Label()
        Me.lvwRILimitVersion = New System.Windows.Forms.ListView()
        Me.lvwRILimitVersion_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwRILimitVersion_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwRILimitVersion_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 19)
        Me.tabMainTab.Location = New System.Drawing.Point(16, 12)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(605, 274)
        Me.tabMainTab.TabIndex = 13
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.btnCopy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblModelUsage)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblRiskType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwRILimitVersion)
        Me._tabMainTab_TabPage0.Controls.Add(Me.btnDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.btnEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.btnAdd)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(597, 247)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - RI Limit Version"
        '
        'btnCopy
        '
        Me.btnCopy.BackColor = System.Drawing.SystemColors.Control
        Me.btnCopy.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCopy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCopy.Location = New System.Drawing.Point(251, 219)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCopy.Size = New System.Drawing.Size(73, 22)
        Me.btnCopy.TabIndex = 21
        Me.btnCopy.Text = "&Copy"
        Me.btnCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCopy.UseVisualStyleBackColor = False
        '
        'lblModelUsage
        '
        Me.lblModelUsage.BackColor = System.Drawing.SystemColors.Control
        Me.lblModelUsage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblModelUsage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModelUsage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblModelUsage.Location = New System.Drawing.Point(15, 16)
        Me.lblModelUsage.Name = "lblModelUsage"
        Me.lblModelUsage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblModelUsage.Size = New System.Drawing.Size(98, 19)
        Me.lblModelUsage.TabIndex = 13
        Me.lblModelUsage.Text = "Risk Type:"
        '
        'lblRiskType
        '
        Me.lblRiskType.BackColor = System.Drawing.SystemColors.Control
        Me.lblRiskType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRiskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRiskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRiskType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRiskType.Location = New System.Drawing.Point(124, 14)
        Me.lblRiskType.Name = "lblRiskType"
        Me.lblRiskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRiskType.Size = New System.Drawing.Size(463, 21)
        Me.lblRiskType.TabIndex = 18
        '
        'lvwRILimitVersion
        '
        Me.lvwRILimitVersion.BackColor = System.Drawing.SystemColors.Window
        Me.lvwRILimitVersion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwRILimitVersion.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lvwRILimitVersion_ColumnHeader_1, Me.lvwRILimitVersion_ColumnHeader_2, Me.lvwRILimitVersion_ColumnHeader_3})
        Me.lvwRILimitVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwRILimitVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwRILimitVersion.FullRowSelect = True
        Me.lvwRILimitVersion.Location = New System.Drawing.Point(18, 50)
        Me.lvwRILimitVersion.Name = "lvwRILimitVersion"
        Me.lvwRILimitVersion.Size = New System.Drawing.Size(576, 163)
        Me.lvwRILimitVersion.TabIndex = 19
        Me.lvwRILimitVersion.UseCompatibleStateImageBehavior = False
        Me.lvwRILimitVersion.View = System.Windows.Forms.View.Details
        '
        'lvwRILimitVersion_ColumnHeader_1
        '
        Me.lvwRILimitVersion_ColumnHeader_1.Text = "Description"
        Me.lvwRILimitVersion_ColumnHeader_1.Width = 224
        '
        'lvwRILimitVersion_ColumnHeader_2
        '
        Me.lvwRILimitVersion_ColumnHeader_2.Text = "Effective Date"
        Me.lvwRILimitVersion_ColumnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.lvwRILimitVersion_ColumnHeader_2.Width = 179
        '
        'lvwRILimitVersion_ColumnHeader_3
        '
        Me.lvwRILimitVersion_ColumnHeader_3.Text = "Expiry Date"
        Me.lvwRILimitVersion_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.lvwRILimitVersion_ColumnHeader_3.Width = 172
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.SystemColors.Control
        Me.btnDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDelete.Location = New System.Drawing.Point(172, 219)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDelete.Size = New System.Drawing.Size(73, 22)
        Me.btnDelete.TabIndex = 1
        Me.btnDelete.Text = "Dele&te"
        Me.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.SystemColors.Control
        Me.btnEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEdit.Location = New System.Drawing.Point(92, 219)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnEdit.Size = New System.Drawing.Size(73, 22)
        Me.btnEdit.TabIndex = 0
        Me.btnEdit.Text = "&Edit..."
        Me.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.BackColor = System.Drawing.SystemColors.Control
        Me.btnAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnAdd.Location = New System.Drawing.Point(12, 219)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(73, 22)
        Me.btnAdd.TabIndex = 20
        Me.btnAdd.Text = "A&dd..."
        Me.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.SystemColors.Control
        Me.btnCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancel.Location = New System.Drawing.Point(467, 303)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCancel.Size = New System.Drawing.Size(73, 22)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.SystemColors.Control
        Me.btnOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnOK.Location = New System.Drawing.Point(388, 302)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnOK.Size = New System.Drawing.Size(73, 22)
        Me.btnOK.TabIndex = 14
        Me.btnOK.Text = "&OK"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'btnHelp
        '
        Me.btnHelp.BackColor = System.Drawing.SystemColors.Control
        Me.btnHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnHelp.Location = New System.Drawing.Point(546, 302)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnHelp.Size = New System.Drawing.Size(73, 22)
        Me.btnHelp.TabIndex = 16
        Me.btnHelp.Text = "&Help"
        Me.btnHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.btnHelp.UseVisualStyleBackColor = False
        '
        'frmInterface_Renamed
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(633, 337)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Name = "frmInterface_Renamed"
        Me.Text = "Risk Type RI Limit Version"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lblModelUsage As System.Windows.Forms.Label
    Public WithEvents lblRiskType As System.Windows.Forms.Label
    Public WithEvents lvwRILimitVersion As System.Windows.Forms.ListView
    Public WithEvents btnDelete As System.Windows.Forms.Button
    Public WithEvents btnEdit As System.Windows.Forms.Button
    Public WithEvents btnAdd As System.Windows.Forms.Button
    Public WithEvents btnCancel As System.Windows.Forms.Button
    Public WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lvwRILimitVersion_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwRILimitVersion_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwRILimitVersion_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Public WithEvents btnCopy As System.Windows.Forms.Button
    Public WithEvents btnHelp As System.Windows.Forms.Button
End Class
