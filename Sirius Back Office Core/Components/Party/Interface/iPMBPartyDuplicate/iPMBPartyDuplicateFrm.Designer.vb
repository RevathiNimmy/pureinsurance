<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptAction()
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
	Private WithEvents _optAction_1 As System.Windows.Forms.RadioButton
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Private WithEvents _lvwClients_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwClients As System.Windows.Forms.ListView
	Private WithEvents _optAction_0 As System.Windows.Forms.RadioButton
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents lblPotentialDuplicate As System.Windows.Forms.Label
	Public optAction(1) As System.Windows.Forms.RadioButton
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._optAction_1 = New System.Windows.Forms.RadioButton
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.lvwClients = New System.Windows.Forms.ListView
        Me._lvwClients_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwClients_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwClients_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwClients_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwClients_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwClients_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwClients_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwClients_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwClients_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._optAction_0 = New System.Windows.Forms.RadioButton
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdView = New System.Windows.Forms.Button
        Me.lblPotentialDuplicate = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_optAction_1
        '
        Me._optAction_1.BackColor = System.Drawing.SystemColors.Control
        Me._optAction_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAction_1.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAction_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAction_1.Location = New System.Drawing.Point(12, 240)
        Me._optAction_1.Name = "_optAction_1"
        Me._optAction_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAction_1.Size = New System.Drawing.Size(613, 22)
        Me._optAction_1.TabIndex = 2
        Me._optAction_1.TabStop = True
        Me._optAction_1.Text = "Create unique code. e.g. SMITHJ1"
        Me._optAction_1.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(496, 268)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(77, 23)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'lvwClients
        '
        Me.lvwClients.BackColor = System.Drawing.SystemColors.Window
        Me.lvwClients.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwClients, "")
        Me.lvwClients.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwClients_ColumnHeader_1, Me._lvwClients_ColumnHeader_2, Me._lvwClients_ColumnHeader_3, Me._lvwClients_ColumnHeader_4, Me._lvwClients_ColumnHeader_5, Me._lvwClients_ColumnHeader_6, Me._lvwClients_ColumnHeader_7, Me._lvwClients_ColumnHeader_8, Me._lvwClients_ColumnHeader_9})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClients, True)
        Me.lvwClients.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwClients.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwClients.FullRowSelect = True
        Me.lvwClients.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwClients, "lvwClients_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwClients, "")
        Me.lvwClients.Location = New System.Drawing.Point(10, 28)
        Me.lvwClients.Name = "lvwClients"
        Me.lvwClients.Size = New System.Drawing.Size(615, 181)
        Me.listViewHelper1.SetSmallIcons(Me.lvwClients, "")
        Me.listViewHelper1.SetSorted(Me.lvwClients, False)
        Me.listViewHelper1.SetSortKey(Me.lvwClients, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwClients, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwClients.TabIndex = 0
        Me.lvwClients.UseCompatibleStateImageBehavior = False
        Me.lvwClients.View = System.Windows.Forms.View.Details
        '
        '_lvwClients_ColumnHeader_1
        '
        Me._lvwClients_ColumnHeader_1.Text = "Client Code"
        Me._lvwClients_ColumnHeader_1.Width = 81
        '
        '_lvwClients_ColumnHeader_2
        '
        Me._lvwClients_ColumnHeader_2.Text = "Name"
        Me._lvwClients_ColumnHeader_2.Width = 134
        '
        '_lvwClients_ColumnHeader_3
        '
        Me._lvwClients_ColumnHeader_3.Text = "Address Line 1"
        Me._lvwClients_ColumnHeader_3.Width = 97
        '
        '_lvwClients_ColumnHeader_4
        '
        Me._lvwClients_ColumnHeader_4.Text = "Address Line 2"
        Me._lvwClients_ColumnHeader_4.Width = 97
        '
        '_lvwClients_ColumnHeader_5
        '
        Me._lvwClients_ColumnHeader_5.Text = "Postcode"
        Me._lvwClients_ColumnHeader_5.Width = 67
        '
        '_lvwClients_ColumnHeader_6
        '
        Me._lvwClients_ColumnHeader_6.Text = "Type"
        Me._lvwClients_ColumnHeader_6.Width = 67
        '
        '_lvwClients_ColumnHeader_7
        '
        Me._lvwClients_ColumnHeader_7.Text = "Branch"
        Me._lvwClients_ColumnHeader_7.Width = 67
        '
        '_lvwClients_ColumnHeader_8
        '
        Me._lvwClients_ColumnHeader_8.Text = "PartyCnt"
        Me._lvwClients_ColumnHeader_8.Width = 0
        '
        '_lvwClients_ColumnHeader_9
        '
        Me._lvwClients_ColumnHeader_9.Text = "PartyTypeID"
        Me._lvwClients_ColumnHeader_9.Width = 0
        '
        '_optAction_0
        '
        Me._optAction_0.BackColor = System.Drawing.SystemColors.Control
        Me._optAction_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAction_0.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAction_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAction_0.Location = New System.Drawing.Point(12, 218)
        Me._optAction_0.Name = "_optAction_0"
        Me._optAction_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAction_0.Size = New System.Drawing.Size(423, 16)
        Me._optAction_0.TabIndex = 1
        Me._optAction_0.TabStop = True
        Me._optAction_0.Text = "Abandon new record and use selected client"
        Me._optAction_0.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(414, 268)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(77, 23)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(10, 268)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(77, 23)
        Me.cmdView.TabIndex = 3
        Me.cmdView.Text = "View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'lblPotentialDuplicate
        '
        Me.lblPotentialDuplicate.AutoSize = True
        Me.lblPotentialDuplicate.BackColor = System.Drawing.SystemColors.Control
        Me.lblPotentialDuplicate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPotentialDuplicate.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPotentialDuplicate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPotentialDuplicate.Location = New System.Drawing.Point(10, 10)
        Me.lblPotentialDuplicate.Name = "lblPotentialDuplicate"
        Me.lblPotentialDuplicate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPotentialDuplicate.Size = New System.Drawing.Size(262, 13)
        Me.lblPotentialDuplicate.TabIndex = 6
        Me.lblPotentialDuplicate.Text = "Potential duplicate client found for ""SMITHJ"":"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(644, 345)
        Me.Controls.Add(Me._optAction_1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.lvwClients)
        Me.Controls.Add(Me._optAction_0)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.lblPotentialDuplicate)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Duplicate Client"
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializeoptAction()
		Me.optAction(1) = _optAction_1
		Me.optAction(0) = _optAction_0
	End Sub
#End Region 
End Class