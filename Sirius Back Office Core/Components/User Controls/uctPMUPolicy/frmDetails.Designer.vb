<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwClients_InitializeColumnKeys()
	End Sub
	Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
		Dispose(True)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Private WithEvents _lvwClients_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwClients_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwClients As System.Windows.Forms.ListView
	Public WithEvents lblInstruct As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.lvwClients = New System.Windows.Forms.ListView
		Me._lvwClients_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwClients_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwClients_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwClients_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwClients_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me.lblInstruct = New System.Windows.Forms.Label
		Me.lvwClients.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(416, 288)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
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
		Me.cmdCancel.Location = New System.Drawing.Point(496, 288)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwClients
		' 
		Me.lvwClients.BackColor = System.Drawing.SystemColors.Window
		Me.lvwClients.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwClients.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwClients.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwClients.HideSelection = False
		Me.lvwClients.LabelEdit = False
		Me.lvwClients.LabelWrap = True
		Me.lvwClients.Location = New System.Drawing.Point(9, 32)
		Me.lvwClients.Name = "lvwClients"
		Me.lvwClients.Size = New System.Drawing.Size(561, 253)
		Me.lvwClients.TabIndex = 2
		Me.lvwClients.View = System.Windows.Forms.View.Details
		Me.lvwClients.Columns.Add(Me._lvwClients_ColumnHeader_1)
		Me.lvwClients.Columns.Add(Me._lvwClients_ColumnHeader_2)
		Me.lvwClients.Columns.Add(Me._lvwClients_ColumnHeader_3)
		Me.lvwClients.Columns.Add(Me._lvwClients_ColumnHeader_4)
		Me.lvwClients.Columns.Add(Me._lvwClients_ColumnHeader_5)
		' 
		' _lvwClients_ColumnHeader_1
		' 
		Me._lvwClients_ColumnHeader_1.Tag = ""
		Me._lvwClients_ColumnHeader_1.Text = "Client Number"
		Me._lvwClients_ColumnHeader_1.Width = 81
		' 
		' _lvwClients_ColumnHeader_2
		' 
		Me._lvwClients_ColumnHeader_2.Tag = ""
		Me._lvwClients_ColumnHeader_2.Text = "Client Name"
		Me._lvwClients_ColumnHeader_2.Width = 81
		' 
		' _lvwClients_ColumnHeader_3
		' 
		Me._lvwClients_ColumnHeader_3.Tag = ""
		Me._lvwClients_ColumnHeader_3.Text = "Address"
		Me._lvwClients_ColumnHeader_3.Width = 121
		' 
		' _lvwClients_ColumnHeader_4
		' 
		Me._lvwClients_ColumnHeader_4.Tag = ""
		Me._lvwClients_ColumnHeader_4.Text = "Lead Client"
		Me._lvwClients_ColumnHeader_4.Width = 87
		' 
		' _lvwClients_ColumnHeader_5
		' 
		Me._lvwClients_ColumnHeader_5.Tag = ""
		Me._lvwClients_ColumnHeader_5.Text = "Correspondence"
		Me._lvwClients_ColumnHeader_5.Width = 87
		' 
		' lblInstruct
		' 
		Me.lblInstruct.AutoSize = False
		Me.lblInstruct.BackColor = System.Drawing.SystemColors.Control
		Me.lblInstruct.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInstruct.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInstruct.Enabled = True
		Me.lblInstruct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblInstruct.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInstruct.Location = New System.Drawing.Point(8, 8)
		Me.lblInstruct.Name = "lblInstruct"
		Me.lblInstruct.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInstruct.Size = New System.Drawing.Size(265, 17)
		Me.lblInstruct.TabIndex = 3
		Me.lblInstruct.Text = "Set Lead Client"
		Me.lblInstruct.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInstruct.UseMnemonic = True
		Me.lblInstruct.Visible = True
		' 
		' frmDetails
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(576, 320)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.lvwClients)
		Me.Controls.Add(Me.lblInstruct)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmDetails.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDetails"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Lead Client"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetSorted(Me.lvwClients, True)
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwClients, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwClients.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwClients_InitializeColumnKeys()
		Me._lvwClients_ColumnHeader_1.Name = ""
		Me._lvwClients_ColumnHeader_2.Name = ""
		Me._lvwClients_ColumnHeader_3.Name = ""
		Me._lvwClients_ColumnHeader_4.Name = ""
		Me._lvwClients_ColumnHeader_5.Name = ""
	End Sub
#End Region 
End Class