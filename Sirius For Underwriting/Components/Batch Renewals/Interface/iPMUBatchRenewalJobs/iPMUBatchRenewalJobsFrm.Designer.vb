<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBankDetailsHistory_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwBankDetailsHistory As System.Windows.Forms.ListView
	Private WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Public WithEvents cmdSuspend As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cmdSchedule As System.Windows.Forms.Button
    Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdClose = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.cmdSuspend = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdSchedule = New System.Windows.Forms.Button
        Me.lvwBankDetailsHistory = New System.Windows.Forms.ListView
        Me._lvwBankDetailsHistory_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwBankDetailsHistory_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwBankDetailsHistory_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwBankDetailsHistory_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwBankDetailsHistory_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwBankDetailsHistory_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwBankDetailsHistory_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwBankDetailsHistory_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwBankDetailsHistory_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwBankDetailsHistory_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(620, 404)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(73, 22)
        Me.cmdClose.TabIndex = 4
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(680, 18)
        Me.SSTab1.Location = New System.Drawing.Point(10, 10)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(685, 391)
        Me.SSTab1.TabIndex = 1
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.lvwSearchDetails)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdSuspend)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdDelete)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdEdit)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdAdd)
        Me._SSTab1_TabPage0.Controls.Add(Me.cmdSchedule)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(677, 365)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "1 - Job Summary"
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5, Me._lvwSearchDetails_ColumnHeader_6})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDetails, True)
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.Location = New System.Drawing.Point(10, 14)
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(658, 315)
        Me.listViewHelper1.SetSmallIcons(Me.lvwSearchDetails, "")
        Me.listViewHelper1.SetSorted(Me.lvwSearchDetails, False)
        Me.listViewHelper1.SetSortKey(Me.lvwSearchDetails, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwSearchDetails, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSearchDetails.TabIndex = 5
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Text = "Created"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Text = "Job Code"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Text = "Description"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Text = "Status"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Text = "Job Type"
        Me._lvwSearchDetails_ColumnHeader_5.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_6
        '
        Me._lvwSearchDetails_ColumnHeader_6.Text = "User"
        Me._lvwSearchDetails_ColumnHeader_6.Width = 97
        '
        'cmdSuspend
        '
        Me.cmdSuspend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSuspend.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSuspend.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSuspend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSuspend.Location = New System.Drawing.Point(596, 338)
        Me.cmdSuspend.Name = "cmdSuspend"
        Me.cmdSuspend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSuspend.Size = New System.Drawing.Size(73, 22)
        Me.cmdSuspend.TabIndex = 3
        Me.cmdSuspend.Text = "&Suspend"
        Me.cmdSuspend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSuspend.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(518, 338)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
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
        Me.cmdEdit.Location = New System.Drawing.Point(440, 338)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 1
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
        Me.cmdAdd.Location = New System.Drawing.Point(362, 338)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 0
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False

        '
        'cmdSchedule
        '
        Me.cmdSchedule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSchedule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSchedule.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSchedule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSchedule.Location = New System.Drawing.Point(16, 338)
        Me.cmdSchedule.Name = "cmdSchedule"
        Me.cmdSchedule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSchedule.Size = New System.Drawing.Size(73, 22)
        Me.cmdSchedule.TabIndex = 0
        Me.cmdSchedule.Text = "&Schedule"
        Me.cmdSchedule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSchedule.UseVisualStyleBackColor = False

        '
        'lvwBankDetailsHistory
        '
        Me.lvwBankDetailsHistory.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwBankDetailsHistory, "")
        Me.lvwBankDetailsHistory.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwBankDetailsHistory_ColumnHeader_1, Me._lvwBankDetailsHistory_ColumnHeader_2, Me._lvwBankDetailsHistory_ColumnHeader_3, Me._lvwBankDetailsHistory_ColumnHeader_4, Me._lvwBankDetailsHistory_ColumnHeader_5, Me._lvwBankDetailsHistory_ColumnHeader_6, Me._lvwBankDetailsHistory_ColumnHeader_7, Me._lvwBankDetailsHistory_ColumnHeader_8, Me._lvwBankDetailsHistory_ColumnHeader_9, Me._lvwBankDetailsHistory_ColumnHeader_10})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwBankDetailsHistory, False)
        Me.lvwBankDetailsHistory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBankDetailsHistory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBankDetailsHistory.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwBankDetailsHistory, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwBankDetailsHistory, "")
        Me.lvwBankDetailsHistory.Location = New System.Drawing.Point(-4994, 28)
        Me.lvwBankDetailsHistory.Name = "lvwBankDetailsHistory"
        Me.lvwBankDetailsHistory.Size = New System.Drawing.Size(621, 303)
        Me.listViewHelper1.SetSmallIcons(Me.lvwBankDetailsHistory, "")
        Me.listViewHelper1.SetSorted(Me.lvwBankDetailsHistory, False)
        Me.listViewHelper1.SetSortKey(Me.lvwBankDetailsHistory, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwBankDetailsHistory, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwBankDetailsHistory.TabIndex = 7
        Me.lvwBankDetailsHistory.UseCompatibleStateImageBehavior = False
        Me.lvwBankDetailsHistory.View = System.Windows.Forms.View.Details
        '
        '_lvwBankDetailsHistory_ColumnHeader_1
        '
        Me._lvwBankDetailsHistory_ColumnHeader_1.Text = "Action Code"
        Me._lvwBankDetailsHistory_ColumnHeader_1.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_2
        '
        Me._lvwBankDetailsHistory_ColumnHeader_2.Text = "Date"
        Me._lvwBankDetailsHistory_ColumnHeader_2.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_3
        '
        Me._lvwBankDetailsHistory_ColumnHeader_3.Text = "Bank Name"
        Me._lvwBankDetailsHistory_ColumnHeader_3.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_4
        '
        Me._lvwBankDetailsHistory_ColumnHeader_4.Text = "Branch"
        Me._lvwBankDetailsHistory_ColumnHeader_4.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_5
        '
        Me._lvwBankDetailsHistory_ColumnHeader_5.Text = "Account Name"
        Me._lvwBankDetailsHistory_ColumnHeader_5.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_6
        '
        Me._lvwBankDetailsHistory_ColumnHeader_6.Text = "Sort Code"
        Me._lvwBankDetailsHistory_ColumnHeader_6.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_7
        '
        Me._lvwBankDetailsHistory_ColumnHeader_7.Text = "Account Number"
        Me._lvwBankDetailsHistory_ColumnHeader_7.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_8
        '
        Me._lvwBankDetailsHistory_ColumnHeader_8.Text = "User"
        Me._lvwBankDetailsHistory_ColumnHeader_8.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_9
        '
        Me._lvwBankDetailsHistory_ColumnHeader_9.Text = "No & Street Name"
        Me._lvwBankDetailsHistory_ColumnHeader_9.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_10
        '
        Me._lvwBankDetailsHistory_ColumnHeader_10.Text = "Postcode"
        Me._lvwBankDetailsHistory_ColumnHeader_10.Width = 97
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(701, 434)
        Me.Controls.Add(Me.lvwBankDetailsHistory)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(190, 279)
        Me.MaximizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Batch Renewal Job Configuration"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class