<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		lvwCaseHistory_InitializeColumnKeys()
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
	Public WithEvents cmdView As System.Windows.Forms.Button
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Private WithEvents _lvwCaseHistory_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCaseHistory_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCaseHistory_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCaseHistory_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCaseHistory_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwCaseHistory As System.Windows.Forms.ListView
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdView = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.lvwCaseHistory = New System.Windows.Forms.ListView
        Me._lvwCaseHistory_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwCaseHistory_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwCaseHistory_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwCaseHistory_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwCaseHistory_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(0, 232)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(81, 25)
        Me.cmdView.TabIndex = 2
        Me.cmdView.Text = "View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(560, 232)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(81, 25)
        Me.cmdClose.TabIndex = 0
        Me.cmdClose.Text = "Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'lvwCaseHistory
        '
        Me.lvwCaseHistory.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCaseHistory.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCaseHistory, "")
        Me.lvwCaseHistory.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCaseHistory_ColumnHeader_1, Me._lvwCaseHistory_ColumnHeader_2, Me._lvwCaseHistory_ColumnHeader_3, Me._lvwCaseHistory_ColumnHeader_4, Me._lvwCaseHistory_ColumnHeader_5})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCaseHistory, True)
        Me.lvwCaseHistory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCaseHistory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCaseHistory.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCaseHistory, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCaseHistory, "")
        Me.lvwCaseHistory.Location = New System.Drawing.Point(0, 0)
        Me.lvwCaseHistory.Name = "lvwCaseHistory"
        Me.lvwCaseHistory.Size = New System.Drawing.Size(639, 230)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCaseHistory, "")
        Me.listViewHelper1.SetSorted(Me.lvwCaseHistory, False)
        Me.listViewHelper1.SetSortKey(Me.lvwCaseHistory, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCaseHistory, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCaseHistory.TabIndex = 1
        Me.lvwCaseHistory.UseCompatibleStateImageBehavior = False
        Me.lvwCaseHistory.View = System.Windows.Forms.View.Details
        '
        '_lvwCaseHistory_ColumnHeader_1
        '
        Me._lvwCaseHistory_ColumnHeader_1.Tag = ""
        Me._lvwCaseHistory_ColumnHeader_1.Text = "case_id"
        Me._lvwCaseHistory_ColumnHeader_1.Width = 97
        '
        '_lvwCaseHistory_ColumnHeader_2
        '
        Me._lvwCaseHistory_ColumnHeader_2.Tag = ""
        Me._lvwCaseHistory_ColumnHeader_2.Text = "Date Of Change"
        Me._lvwCaseHistory_ColumnHeader_2.Width = 97
        '
        '_lvwCaseHistory_ColumnHeader_3
        '
        Me._lvwCaseHistory_ColumnHeader_3.Tag = ""
        Me._lvwCaseHistory_ColumnHeader_3.Text = "Changes Description"
        Me._lvwCaseHistory_ColumnHeader_3.Width = 97
        '
        '_lvwCaseHistory_ColumnHeader_4
        '
        Me._lvwCaseHistory_ColumnHeader_4.Tag = ""
        Me._lvwCaseHistory_ColumnHeader_4.Text = "Case Progress Status"
        Me._lvwCaseHistory_ColumnHeader_4.Width = 97
        '
        '_lvwCaseHistory_ColumnHeader_5
        '
        Me._lvwCaseHistory_ColumnHeader_5.Tag = ""
        Me._lvwCaseHistory_ColumnHeader_5.Text = "User"
        Me._lvwCaseHistory_ColumnHeader_5.Width = 97
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(642, 259)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.lvwCaseHistory)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(135, 205)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Case History"
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwCaseHistory_InitializeColumnKeys()
		Me._lvwCaseHistory_ColumnHeader_1.Name = ""
		Me._lvwCaseHistory_ColumnHeader_2.Name = ""
		Me._lvwCaseHistory_ColumnHeader_3.Name = ""
		Me._lvwCaseHistory_ColumnHeader_4.Name = ""
		Me._lvwCaseHistory_ColumnHeader_5.Name = ""
	End Sub
#End Region 
End Class