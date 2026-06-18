<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwAssociates_InitializeColumnKeys()
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
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Private WithEvents _lvwAssociates_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwAssociates_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAssociates_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAssociates_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwAssociates_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwAssociates As System.Windows.Forms.ListView
	Private WithEvents _tabAssociates_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabAssociates As System.Windows.Forms.TabControl
	Public WithEvents lblRelationships As System.Windows.Forms.Label
	Public WithEvents cmdClientLookup As System.Windows.Forms.Button
	Public WithEvents cmdDetailOK As System.Windows.Forms.Button
	Public WithEvents cmdDetailCancel As System.Windows.Forms.Button
	Public WithEvents pnlClientLookup As System.Windows.Forms.Panel
	Public WithEvents cboRelationships As System.Windows.Forms.ComboBox
	Public WithEvents chkCommissionTransaction As System.Windows.Forms.CheckBox
	Private WithEvents _tabDetailTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabDetailTab As System.Windows.Forms.TabControl
    'developer guide no. 26(latest)
    Private WithEvents lblClientLookup As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tabAssociates = New System.Windows.Forms.TabControl()
        Me._tabAssociates_TabPage0 = New System.Windows.Forms.TabPage()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.lvwAssociates = New System.Windows.Forms.ListView()
        Me._lvwAssociates_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAssociates_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAssociates_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAssociates_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwAssociates_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tabDetailTab = New System.Windows.Forms.TabControl()
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblRelationships = New System.Windows.Forms.Label()
        Me.cmdClientLookup = New System.Windows.Forms.Button()
        Me.cmdDetailOK = New System.Windows.Forms.Button()
        Me.cmdDetailCancel = New System.Windows.Forms.Button()
        Me.pnlClientLookup = New System.Windows.Forms.Panel()
        Me.lblClientLookup = New System.Windows.Forms.Label()
        Me.cboRelationships = New System.Windows.Forms.ComboBox()
        Me.chkCommissionTransaction = New System.Windows.Forms.CheckBox()
        Me.tabAssociates.SuspendLayout()
        Me._tabAssociates_TabPage0.SuspendLayout()
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.pnlClientLookup.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(373, 208)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 7
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
        Me.cmdCancel.Location = New System.Drawing.Point(294, 208)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 6
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
        Me.cmdOK.Location = New System.Drawing.Point(215, 208)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 5
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabAssociates
        '
        Me.tabAssociates.Controls.Add(Me._tabAssociates_TabPage0)
        Me.tabAssociates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabAssociates.ItemSize = New System.Drawing.Size(416, 18)
        Me.tabAssociates.Location = New System.Drawing.Point(12, 12)
        Me.tabAssociates.Multiline = True
        Me.tabAssociates.Name = "tabAssociates"
        Me.tabAssociates.SelectedIndex = 0
        Me.tabAssociates.Size = New System.Drawing.Size(434, 151)
        Me.tabAssociates.TabIndex = 0
        '
        '_tabAssociates_TabPage0
        '
        Me._tabAssociates_TabPage0.Controls.Add(Me.lvwAssociates)
        Me._tabAssociates_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabAssociates_TabPage0.Name = "_tabAssociates_TabPage0"
        Me._tabAssociates_TabPage0.Size = New System.Drawing.Size(426, 125)
        Me._tabAssociates_TabPage0.TabIndex = 0
        Me._tabAssociates_TabPage0.Text = "1 - Associates"
        Me._tabAssociates_TabPage0.UseVisualStyleBackColor = True
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(364, 169)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 4
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
        Me.cmdEdit.Location = New System.Drawing.Point(195, 169)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 2
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
        Me.cmdAdd.Location = New System.Drawing.Point(285, 169)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 3
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'lvwAssociates
        '
        Me.lvwAssociates.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAssociates.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwAssociates.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwAssociates_ColumnHeader_1, Me._lvwAssociates_ColumnHeader_2, Me._lvwAssociates_ColumnHeader_3, Me._lvwAssociates_ColumnHeader_4, Me._lvwAssociates_ColumnHeader_5})
        Me.lvwAssociates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAssociates.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAssociates.HideSelection = False
        Me.lvwAssociates.Location = New System.Drawing.Point(4, 13)
        Me.lvwAssociates.Name = "lvwAssociates"
        Me.lvwAssociates.Size = New System.Drawing.Size(417, 96)
        Me.lvwAssociates.TabIndex = 1
        Me.lvwAssociates.UseCompatibleStateImageBehavior = False
        Me.lvwAssociates.View = System.Windows.Forms.View.Details
        '
        '_lvwAssociates_ColumnHeader_1
        '
        Me._lvwAssociates_ColumnHeader_1.Tag = ""
        Me._lvwAssociates_ColumnHeader_1.Text = "Associate"
        Me._lvwAssociates_ColumnHeader_1.Width = 112
        '
        '_lvwAssociates_ColumnHeader_2
        '
        Me._lvwAssociates_ColumnHeader_2.Tag = ""
        Me._lvwAssociates_ColumnHeader_2.Text = "Name"
        Me._lvwAssociates_ColumnHeader_2.Width = 150
        '
        '_lvwAssociates_ColumnHeader_3
        '
        Me._lvwAssociates_ColumnHeader_3.Tag = ""
        Me._lvwAssociates_ColumnHeader_3.Text = "Relationship"
        Me._lvwAssociates_ColumnHeader_3.Width = 145
        '
        '_lvwAssociates_ColumnHeader_4
        '
        Me._lvwAssociates_ColumnHeader_4.Tag = ""
        Me._lvwAssociates_ColumnHeader_4.Text = "Account Balance"
        Me._lvwAssociates_ColumnHeader_4.Width = 145
        '
        '_lvwAssociates_ColumnHeader_5
        '
        Me._lvwAssociates_ColumnHeader_5.Tag = ""
        Me._lvwAssociates_ColumnHeader_5.Text = "Claims Incurred Value"
        Me._lvwAssociates_ColumnHeader_5.Width = 145
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(416, 18)
        Me.tabDetailTab.Location = New System.Drawing.Point(16, 688)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(421, 189)
        Me.tabDetailTab.TabIndex = 8
        Me.tabDetailTab.Visible = False
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblRelationships)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdClientLookup)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.pnlClientLookup)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboRelationships)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.chkCommissionTransaction)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(413, 163)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "2 - Associate"
        '
        'lblRelationships
        '
        Me.lblRelationships.BackColor = System.Drawing.SystemColors.Control
        Me.lblRelationships.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRelationships.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRelationships.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRelationships.Location = New System.Drawing.Point(48, 71)
        Me.lblRelationships.Name = "lblRelationships"
        Me.lblRelationships.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRelationships.Size = New System.Drawing.Size(81, 17)
        Me.lblRelationships.TabIndex = 14
        Me.lblRelationships.Text = "Relationship :"
        '
        'cmdClientLookup
        '
        Me.cmdClientLookup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClientLookup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClientLookup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClientLookup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClientLookup.Location = New System.Drawing.Point(48, 36)
        Me.cmdClientLookup.Name = "cmdClientLookup"
        Me.cmdClientLookup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClientLookup.Size = New System.Drawing.Size(84, 20)
        Me.cmdClientLookup.TabIndex = 9
        Me.cmdClientLookup.Text = "Client..."
        Me.cmdClientLookup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClientLookup.UseVisualStyleBackColor = False
        '
        'cmdDetailOK
        '
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(232, 116)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailOK.TabIndex = 11
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
        Me.cmdDetailCancel.Location = New System.Drawing.Point(312, 116)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(65, 22)
        Me.cmdDetailCancel.TabIndex = 12
        Me.cmdDetailCancel.Text = "&Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'pnlClientLookup
        '
        Me.pnlClientLookup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlClientLookup.Controls.Add(Me.lblClientLookup)
        Me.pnlClientLookup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlClientLookup.Location = New System.Drawing.Point(152, 36)
        Me.pnlClientLookup.Name = "pnlClientLookup"
        Me.pnlClientLookup.Size = New System.Drawing.Size(225, 21)
        Me.pnlClientLookup.TabIndex = 13
        '
        'lblClientLookup
        '
        Me.lblClientLookup.AutoSize = True
        Me.lblClientLookup.Location = New System.Drawing.Point(1, 2)
        Me.lblClientLookup.Name = "lblClientLookup"
        Me.lblClientLookup.Size = New System.Drawing.Size(0, 13)
        Me.lblClientLookup.TabIndex = 144
        '
        'cboRelationships
        '
        Me.cboRelationships.BackColor = System.Drawing.SystemColors.Window
        Me.cboRelationships.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRelationships.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRelationships.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRelationships.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRelationships.Location = New System.Drawing.Point(152, 68)
        Me.cboRelationships.Name = "cboRelationships"
        Me.cboRelationships.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRelationships.Size = New System.Drawing.Size(225, 21)
        Me.cboRelationships.TabIndex = 10
        '
        'chkCommissionTransaction
        '
        Me.chkCommissionTransaction.BackColor = System.Drawing.SystemColors.Control
        Me.chkCommissionTransaction.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCommissionTransaction.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCommissionTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCommissionTransaction.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCommissionTransaction.Location = New System.Drawing.Point(8, 111)
        Me.chkCommissionTransaction.Name = "chkCommissionTransaction"
        Me.chkCommissionTransaction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCommissionTransaction.Size = New System.Drawing.Size(209, 33)
        Me.chkCommissionTransaction.TabIndex = 15
        Me.chkCommissionTransaction.Text = "Raise commission transactions against this Associated Agent"
        Me.chkCommissionTransaction.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(458, 249)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabAssociates)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Associated Clients"
        Me.tabAssociates.ResumeLayout(False)
        Me._tabAssociates_TabPage0.ResumeLayout(False)
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me.pnlClientLookup.ResumeLayout(False)
        Me.pnlClientLookup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub lvwAssociates_InitializeColumnKeys()
		Me._lvwAssociates_ColumnHeader_1.Name = ""
		Me._lvwAssociates_ColumnHeader_2.Name = ""
        Me._lvwAssociates_ColumnHeader_3.Name = ""
        Me._lvwAssociates_ColumnHeader_4.Name = ""
        Me._lvwAssociates_ColumnHeader_5.Name = ""
    End Sub
#End Region 
End Class