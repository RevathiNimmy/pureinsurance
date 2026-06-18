<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHistory
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Private WithEvents _lvwHistory_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwHistory_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwHistory_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwHistory_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwHistory_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwHistory_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwHistory As System.Windows.Forms.ListView
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHistory))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.lvwHistory = New System.Windows.Forms.ListView
		Me._lvwHistory_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwHistory_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwHistory_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwHistory_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwHistory_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwHistory_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.ImageList1 = New System.Windows.Forms.ImageList
		Me.lvwHistory.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' lvwHistory
		' 
		Me.lvwHistory.BackColor = System.Drawing.SystemColors.Window
		Me.lvwHistory.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwHistory.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwHistory.HideSelection = True
		Me.lvwHistory.LabelEdit = False
		Me.lvwHistory.LabelWrap = True
		Me.lvwHistory.LargeImageList = ImageList1
		Me.lvwHistory.Location = New System.Drawing.Point(4, 4)
		Me.lvwHistory.Name = "lvwHistory"
		Me.lvwHistory.Size = New System.Drawing.Size(593, 153)
		Me.lvwHistory.SmallImageList = ImageList1
		Me.lvwHistory.TabIndex = 0
		Me.lvwHistory.View = System.Windows.Forms.View.Details
		Me.lvwHistory.Columns.Add(Me._lvwHistory_ColumnHeader_1)
		Me.lvwHistory.Columns.Add(Me._lvwHistory_ColumnHeader_2)
		Me.lvwHistory.Columns.Add(Me._lvwHistory_ColumnHeader_3)
		Me.lvwHistory.Columns.Add(Me._lvwHistory_ColumnHeader_4)
		Me.lvwHistory.Columns.Add(Me._lvwHistory_ColumnHeader_5)
		Me.lvwHistory.Columns.Add(Me._lvwHistory_ColumnHeader_6)
		' 
		' _lvwHistory_ColumnHeader_1
		' 
		Me._lvwHistory_ColumnHeader_1.Text = "Version"
		Me._lvwHistory_ColumnHeader_1.Width = 54
		' 
		' _lvwHistory_ColumnHeader_2
		' 
		Me._lvwHistory_ColumnHeader_2.Text = "Plan Reference"
		Me._lvwHistory_ColumnHeader_2.Width = 141
		' 
		' _lvwHistory_ColumnHeader_3
		' 
		Me._lvwHistory_ColumnHeader_3.Text = "Date"
		Me._lvwHistory_ColumnHeader_3.Width = 81
		' 
		' _lvwHistory_ColumnHeader_4
		' 
		Me._lvwHistory_ColumnHeader_4.Text = "Status"
		Me._lvwHistory_ColumnHeader_4.Width = 81
		' 
		' _lvwHistory_ColumnHeader_5
		' 
		Me._lvwHistory_ColumnHeader_5.Text = "Financed Amount"
		Me._lvwHistory_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwHistory_ColumnHeader_5.Width = 97
		' 
		' _lvwHistory_ColumnHeader_6
		' 
		Me._lvwHistory_ColumnHeader_6.Text = "Total Amount On Plan"
		Me._lvwHistory_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwHistory_ColumnHeader_6.Width = 121
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(456, 164)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(65, 22)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(528, 164)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(65, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' ImageList1
		' 
		Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.ImageList1.Images.SetKeyName(0, "FindImage")
		' 
		' frmHistory
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(607, 189)
		Me.ControlBox = True
		Me.Controls.Add(Me.lvwHistory)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 21)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmHistory"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "History"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwHistory, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwHistory.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class