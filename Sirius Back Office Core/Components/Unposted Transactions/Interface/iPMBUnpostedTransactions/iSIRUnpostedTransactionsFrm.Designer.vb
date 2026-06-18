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
	Public WithEvents prgProcessingTrans As System.Windows.Forms.ProgressBar
	Public WithEvents lblProcessingTrans As System.Windows.Forms.Label
	Public WithEvents fraProgress As System.Windows.Forms.Panel
	Private WithEvents _lvTrans_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvTrans_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvTrans_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvTrans_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvTrans_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvTrans_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvTrans_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvTrans_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvTrans_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvTrans_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvTrans As System.Windows.Forms.ListView
	Public WithEvents cboPMLookupTransType As PMLookupControl.cboPMLookup
	Public WithEvents cmdFind As System.Windows.Forms.Button
	Public WithEvents txtAgentCode As System.Windows.Forms.TextBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdPost As System.Windows.Forms.Button
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraProgress = New System.Windows.Forms.Panel
		Me.prgProcessingTrans = New System.Windows.Forms.ProgressBar
		Me.lblProcessingTrans = New System.Windows.Forms.Label
		Me.lvTrans = New System.Windows.Forms.ListView
		Me._lvTrans_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvTrans_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvTrans_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvTrans_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvTrans_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvTrans_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me._lvTrans_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
		Me._lvTrans_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
		Me._lvTrans_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
		Me._lvTrans_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
		Me.cboPMLookupTransType = New PMLookupControl.cboPMLookup
		Me.cmdFind = New System.Windows.Forms.Button
		Me.txtAgentCode = New System.Windows.Forms.TextBox
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdPost = New System.Windows.Forms.Button
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.fraProgress.SuspendLayout()
		Me.lvTrans.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' fraProgress
		' 
		Me.fraProgress.BackColor = System.Drawing.SystemColors.Control
		Me.fraProgress.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraProgress.Controls.Add(Me.prgProcessingTrans)
		Me.fraProgress.Controls.Add(Me.lblProcessingTrans)
		Me.fraProgress.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraProgress.Enabled = True
		Me.fraProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraProgress.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraProgress.Location = New System.Drawing.Point(2, 324)
		Me.fraProgress.Name = "fraProgress"
		Me.fraProgress.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraProgress.Size = New System.Drawing.Size(305, 41)
		Me.fraProgress.TabIndex = 8
		Me.fraProgress.Text = "Frame1"
		Me.fraProgress.Visible = False
		' 
		' prgProcessingTrans
		' 
		Me.prgProcessingTrans.Location = New System.Drawing.Point(8, 15)
		Me.prgProcessingTrans.Name = "prgProcessingTrans"
		Me.prgProcessingTrans.Size = New System.Drawing.Size(201, 20)
		Me.prgProcessingTrans.TabIndex = 9
		' 
		' lblProcessingTrans
		' 
		Me.lblProcessingTrans.AutoSize = False
		Me.lblProcessingTrans.BackColor = System.Drawing.SystemColors.Control
		Me.lblProcessingTrans.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblProcessingTrans.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProcessingTrans.Enabled = True
		Me.lblProcessingTrans.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblProcessingTrans.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblProcessingTrans.Location = New System.Drawing.Point(10, 0)
		Me.lblProcessingTrans.Name = "lblProcessingTrans"
		Me.lblProcessingTrans.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProcessingTrans.Size = New System.Drawing.Size(217, 17)
		Me.lblProcessingTrans.TabIndex = 10
		Me.lblProcessingTrans.Text = "Processing Transactions...."
		Me.lblProcessingTrans.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblProcessingTrans.UseMnemonic = True
		Me.lblProcessingTrans.Visible = True
		' 
		' lvTrans
		' 
		Me.lvTrans.BackColor = System.Drawing.SystemColors.Window
		Me.lvTrans.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvTrans.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvTrans.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvTrans.HideSelection = True
		Me.lvTrans.LabelEdit = True
		Me.lvTrans.LabelWrap = True
		Me.lvTrans.Location = New System.Drawing.Point(8, 40)
		Me.lvTrans.Name = "lvTrans"
		Me.lvTrans.Size = New System.Drawing.Size(641, 283)
		Me.lvTrans.TabIndex = 7
		Me.lvTrans.View = System.Windows.Forms.View.Details
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_1)
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_2)
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_3)
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_4)
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_5)
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_6)
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_7)
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_8)
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_9)
		Me.lvTrans.Columns.Add(Me._lvTrans_ColumnHeader_10)
		' 
		' _lvTrans_ColumnHeader_1
		' 
		Me._lvTrans_ColumnHeader_1.Text = "Client Code"
		Me._lvTrans_ColumnHeader_1.Width = 141
		' 
		' _lvTrans_ColumnHeader_2
		' 
		Me._lvTrans_ColumnHeader_2.Text = "Agent Code"
		Me._lvTrans_ColumnHeader_2.Width = 141
		' 
		' _lvTrans_ColumnHeader_3
		' 
		Me._lvTrans_ColumnHeader_3.Text = "Policy Number"
		Me._lvTrans_ColumnHeader_3.Width = 141
		' 
		' _lvTrans_ColumnHeader_4
		' 
		Me._lvTrans_ColumnHeader_4.Text = "Cover Start Date"
		Me._lvTrans_ColumnHeader_4.Width = 114
		' 
		' _lvTrans_ColumnHeader_5
		' 
		Me._lvTrans_ColumnHeader_5.Text = "Operator"
		Me._lvTrans_ColumnHeader_5.Width = 67
		' 
		' _lvTrans_ColumnHeader_6
		' 
		Me._lvTrans_ColumnHeader_6.Text = "Gross Amount"
		Me._lvTrans_ColumnHeader_6.Width = 97
		' 
		' _lvTrans_ColumnHeader_7
		' 
		Me._lvTrans_ColumnHeader_7.Text = "Commission"
		Me._lvTrans_ColumnHeader_7.Width = 67
		' 
		' _lvTrans_ColumnHeader_8
		' 
		Me._lvTrans_ColumnHeader_8.Text = "Tax"
		Me._lvTrans_ColumnHeader_8.Width = 67
		' 
		' _lvTrans_ColumnHeader_9
		' 
		Me._lvTrans_ColumnHeader_9.Text = "Currency"
		Me._lvTrans_ColumnHeader_9.Width = 67
		' 
		' _lvTrans_ColumnHeader_10
		' 
		Me._lvTrans_ColumnHeader_10.Text = "Transaction Type"
		Me._lvTrans_ColumnHeader_10.Width = 134
		' 
		' cboPMLookupTransType
		' 
		Me.cboPMLookupTransType.FirstItem = "(All Transaction Types)"
		Me.cboPMLookupTransType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPMLookupTransType.Location = New System.Drawing.Point(344, 8)
		Me.cboPMLookupTransType.Name = "cboPMLookupTransType"
		Me.cboPMLookupTransType.Size = New System.Drawing.Size(185, 21)
		Me.cboPMLookupTransType.Sorted = True
		Me.cboPMLookupTransType.TabIndex = 6
        Me.cboPMLookupTransType.TableName = "transaction_type"
		' 
		' cmdFind
		' 
		Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFind.CausesValidation = True
		Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFind.Enabled = True
		Me.cmdFind.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFind.Location = New System.Drawing.Point(568, 8)
		Me.cmdFind.Name = "cmdFind"
		Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFind.Size = New System.Drawing.Size(81, 22)
		Me.cmdFind.TabIndex = 5
		Me.cmdFind.TabStop = True
		Me.cmdFind.Tag = "CAP;200"
		Me.cmdFind.Text = "&Find Now"
		Me.cmdFind.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtAgentCode
		' 
		Me.txtAgentCode.AcceptsReturn = True
		Me.txtAgentCode.AutoSize = False
		Me.txtAgentCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtAgentCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAgentCode.CausesValidation = True
		Me.txtAgentCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAgentCode.Enabled = True
		Me.txtAgentCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAgentCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAgentCode.HideSelection = True
		Me.txtAgentCode.Location = New System.Drawing.Point(96, 8)
		Me.txtAgentCode.MaxLength = 0
		Me.txtAgentCode.Multiline = False
		Me.txtAgentCode.Name = "txtAgentCode"
		Me.txtAgentCode.ReadOnly = False
		Me.txtAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAgentCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAgentCode.Size = New System.Drawing.Size(113, 21)
		Me.txtAgentCode.TabIndex = 4
		Me.txtAgentCode.TabStop = True
		Me.txtAgentCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAgentCode.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(568, 336)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(81, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Tag = "CAP;201"
		Me.cmdCancel.Text = "&Close"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdPost
		' 
		Me.cmdPost.BackColor = System.Drawing.SystemColors.Control
		Me.cmdPost.CausesValidation = True
		Me.cmdPost.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdPost.Enabled = True
		Me.cmdPost.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdPost.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdPost.Location = New System.Drawing.Point(480, 336)
		Me.cmdPost.Name = "cmdPost"
		Me.cmdPost.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdPost.Size = New System.Drawing.Size(81, 22)
		Me.cmdPost.TabIndex = 0
		Me.cmdPost.TabStop = True
		Me.cmdPost.Tag = "CAP;200"
		Me.cmdPost.Text = "&Post"
		Me.cmdPost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdPost.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(232, 11)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(121, 33)
		Me.Label2.TabIndex = 3
		Me.Label2.Text = "Transaction Type:"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(16, 11)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(121, 25)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Agent Code:"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(660, 369)
		Me.ControlBox = True
		Me.Controls.Add(Me.fraProgress)
		Me.Controls.Add(Me.lvTrans)
		Me.Controls.Add(Me.cboPMLookupTransType)
		Me.Controls.Add(Me.cmdFind)
		Me.Controls.Add(Me.txtAgentCode)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdPost)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmInterface.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(203, 163)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Tag = "CAP;100"
		Me.Text = "Unposted Transactions"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraProgress.ResumeLayout(False)
		Me.lvTrans.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class