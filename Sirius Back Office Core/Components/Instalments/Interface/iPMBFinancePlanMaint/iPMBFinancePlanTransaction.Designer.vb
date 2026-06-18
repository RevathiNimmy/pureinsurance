<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTransactions
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
	Private WithEvents _lvwTransactions_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwTransactions_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTransactions As System.Windows.Forms.ListView
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTransactions))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.lvwTransactions = New System.Windows.Forms.ListView
		Me._lvwTransactions_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_11 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_12 = New System.Windows.Forms.ColumnHeader
		Me._lvwTransactions_ColumnHeader_13 = New System.Windows.Forms.ColumnHeader
		Me.cmdOK = New System.Windows.Forms.Button
		Me.ImageList1 = New System.Windows.Forms.ImageList
		Me.lvwTransactions.SuspendLayout()
		Me.SuspendLayout()
        ' 
		' lvwTransactions
		' 
		Me.lvwTransactions.BackColor = System.Drawing.SystemColors.Window
		Me.lvwTransactions.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwTransactions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwTransactions.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwTransactions.HideSelection = True
		Me.lvwTransactions.LabelEdit = False
		Me.lvwTransactions.LabelWrap = True
		Me.lvwTransactions.LargeImageList = ImageList1
		Me.lvwTransactions.Location = New System.Drawing.Point(4, 4)
		Me.lvwTransactions.Name = "lvwTransactions"
		Me.lvwTransactions.Size = New System.Drawing.Size(727, 153)
		Me.lvwTransactions.SmallImageList = ImageList1
		Me.lvwTransactions.TabIndex = 0
		Me.lvwTransactions.View = System.Windows.Forms.View.Details
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_1)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_2)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_3)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_4)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_5)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_6)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_7)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_8)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_9)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_10)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_11)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_12)
		Me.lvwTransactions.Columns.Add(Me._lvwTransactions_ColumnHeader_13)
		' 
		' _lvwTransactions_ColumnHeader_1
		' 
		Me._lvwTransactions_ColumnHeader_1.Text = "Branch"
		Me._lvwTransactions_ColumnHeader_1.Width = 54
		' 
		' _lvwTransactions_ColumnHeader_2
		' 
		Me._lvwTransactions_ColumnHeader_2.Text = "Account"
		Me._lvwTransactions_ColumnHeader_2.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_3
		' 
		Me._lvwTransactions_ColumnHeader_3.Text = "Doc. Ref."
		Me._lvwTransactions_ColumnHeader_3.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_4
		' 
		Me._lvwTransactions_ColumnHeader_4.Text = "Effective Date"
		Me._lvwTransactions_ColumnHeader_4.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_5
		' 
		Me._lvwTransactions_ColumnHeader_5.Text = "Trans Date"
		Me._lvwTransactions_ColumnHeader_5.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_6
		' 
		Me._lvwTransactions_ColumnHeader_6.Text = "Period"
		Me._lvwTransactions_ColumnHeader_6.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_7
		' 
		Me._lvwTransactions_ColumnHeader_7.Text = "Amount"
		Me._lvwTransactions_ColumnHeader_7.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_8
		' 
		Me._lvwTransactions_ColumnHeader_8.Text = "Primary Settled"
		Me._lvwTransactions_ColumnHeader_8.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_9
		' 
		Me._lvwTransactions_ColumnHeader_9.Text = "O/S Amount"
		Me._lvwTransactions_ColumnHeader_9.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_10
		' 
		Me._lvwTransactions_ColumnHeader_10.Text = "Doc. Type"
		Me._lvwTransactions_ColumnHeader_10.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_11
		' 
		Me._lvwTransactions_ColumnHeader_11.Text = "Doc. Group"
		Me._lvwTransactions_ColumnHeader_11.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_12
		' 
		Me._lvwTransactions_ColumnHeader_12.Text = "Insurance Ref"
		Me._lvwTransactions_ColumnHeader_12.Width = 97
		' 
		' _lvwTransactions_ColumnHeader_13
		' 
		Me._lvwTransactions_ColumnHeader_13.Text = "Operator Name"
		Me._lvwTransactions_ColumnHeader_13.Width = 97
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(668, 162)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(65, 22)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' ImageList1
		' 
		Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.ImageList1.Images.SetKeyName(0, "FindImage")
		' 
		' frmTransactions
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(736, 190)
		Me.ControlBox = True
		Me.Controls.Add(Me.lvwTransactions)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 21)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmTransactions"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Transactions"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.lvwTransactions.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class