<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLapseRenewal
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwLapseReasons_InitializeColumnKeys()
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Private WithEvents _lvwLapseReasons_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwLapseReasons_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwLapseReasons As System.Windows.Forms.ListView
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Public WithEvents lblLapseReason As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.lvwLapseReasons = New System.Windows.Forms.ListView
        Me._lvwLapseReasons_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwLapseReasons_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lblLapseReason = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lblStatus = New System.Windows.Forms.Label
        Me.stbStatus.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(329, 206)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Location = New System.Drawing.Point(254, 206)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(73, 22)
        Me.cmdSelect.TabIndex = 2
        Me.cmdSelect.Text = "&OK"
        Me.cmdSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'lvwLapseReasons
        '
        Me.lvwLapseReasons.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwLapseReasons, "")
        Me.lvwLapseReasons.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwLapseReasons_ColumnHeader_1, Me._lvwLapseReasons_ColumnHeader_2})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwLapseReasons, True)
        Me.lvwLapseReasons.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwLapseReasons.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwLapseReasons.FullRowSelect = True
        Me.lvwLapseReasons.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwLapseReasons, "")
        Me.lvwLapseReasons.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwLapseReasons, "")
        Me.lvwLapseReasons.Location = New System.Drawing.Point(8, 30)
        Me.lvwLapseReasons.MultiSelect = False
        Me.lvwLapseReasons.Name = "lvwLapseReasons"
        Me.lvwLapseReasons.Size = New System.Drawing.Size(393, 169)
        Me.listViewHelper1.SetSmallIcons(Me.lvwLapseReasons, "")
        Me.listViewHelper1.SetSorted(Me.lvwLapseReasons, True)
        Me.lvwLapseReasons.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.listViewHelper1.SetSortKey(Me.lvwLapseReasons, 1)
        Me.listViewHelper1.SetSortOrder(Me.lvwLapseReasons, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwLapseReasons.TabIndex = 1
        Me.lvwLapseReasons.UseCompatibleStateImageBehavior = False
        Me.lvwLapseReasons.View = System.Windows.Forms.View.Details
        '
        '_lvwLapseReasons_ColumnHeader_1
        '
        Me._lvwLapseReasons_ColumnHeader_1.Tag = ""
        Me._lvwLapseReasons_ColumnHeader_1.Text = "Lapse ReasonID"
        Me._lvwLapseReasons_ColumnHeader_1.Width = 0
        '
        '_lvwLapseReasons_ColumnHeader_2
        '
        Me._lvwLapseReasons_ColumnHeader_2.Tag = ""
        Me._lvwLapseReasons_ColumnHeader_2.Text = "Lapse Reason"
        Me._lvwLapseReasons_ColumnHeader_2.Width = 367
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 231)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(409, 22)
        Me.stbStatus.TabIndex = 3
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(391, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLapseReason
        '
        Me.lblLapseReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblLapseReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLapseReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLapseReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLapseReason.Location = New System.Drawing.Point(8, 8)
        Me.lblLapseReason.Name = "lblLapseReason"
        Me.lblLapseReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLapseReason.Size = New System.Drawing.Size(353, 19)
        Me.lblLapseReason.TabIndex = 0
        Me.lblLapseReason.Text = "Please select the reason why this renewal is to be lapsed"
        '
        'lblStatus
        '
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(0, 231)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(391, 22)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.Text = ""
        '
        'frmLapseRenewal
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(409, 253)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.lvwLapseReasons)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.lblLapseReason)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmLapseRenewal"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Lapse Reason"
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwLapseReasons_InitializeColumnKeys()
		Me._lvwLapseReasons_ColumnHeader_1.Name = ""
		Me._lvwLapseReasons_ColumnHeader_2.Name = ""
    End Sub
    Public WithEvents lblStatus As System.Windows.Forms.Label
#End Region 
End Class