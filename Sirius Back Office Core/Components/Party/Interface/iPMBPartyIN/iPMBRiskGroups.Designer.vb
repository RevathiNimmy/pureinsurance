<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRiskGroups
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
	Public WithEvents cmdAddAll As System.Windows.Forms.Button
	Public WithEvents cmdRemove As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdRemoveAll As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _lvwSelected_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSelected As System.Windows.Forms.ListView
	Private WithEvents _lvwAvailable_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwAvailable As System.Windows.Forms.ListView
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Public WithEvents lblAvailable As System.Windows.Forms.Label
	Public WithEvents lblSelected As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRiskGroups))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdAddAll = New System.Windows.Forms.Button
		Me.cmdRemove = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdRemoveAll = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.lvwSelected = New System.Windows.Forms.ListView
		Me._lvwSelected_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me.lvwAvailable = New System.Windows.Forms.ListView
		Me._lvwAvailable_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me.ImageList1 = New System.Windows.Forms.ImageList
		Me.lblAvailable = New System.Windows.Forms.Label
		Me.lblSelected = New System.Windows.Forms.Label
		Me.lvwSelected.SuspendLayout()
		Me.lvwAvailable.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdAddAll
		' 
		Me.cmdAddAll.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddAll.CausesValidation = True
		Me.cmdAddAll.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAddAll.Enabled = True
		Me.cmdAddAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAddAll.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAddAll.Location = New System.Drawing.Point(216, 88)
		Me.cmdAddAll.Name = "cmdAddAll"
		Me.cmdAddAll.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAddAll.Size = New System.Drawing.Size(73, 22)
		Me.cmdAddAll.TabIndex = 7
		Me.cmdAddAll.TabStop = True
		Me.cmdAddAll.Text = "Group >>"
		Me.cmdAddAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAddAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdRemove
		' 
		Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRemove.CausesValidation = True
		Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRemove.Enabled = True
		Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRemove.Location = New System.Drawing.Point(216, 160)
		Me.cmdRemove.Name = "cmdRemove"
		Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRemove.Size = New System.Drawing.Size(73, 22)
		Me.cmdRemove.TabIndex = 6
		Me.cmdRemove.TabStop = True
		Me.cmdRemove.Text = "< Group"
		Me.cmdRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(216, 56)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 5
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "Group >"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdRemoveAll
		' 
		Me.cmdRemoveAll.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRemoveAll.CausesValidation = True
		Me.cmdRemoveAll.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRemoveAll.Enabled = True
		Me.cmdRemoveAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRemoveAll.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRemoveAll.Location = New System.Drawing.Point(216, 192)
		Me.cmdRemoveAll.Name = "cmdRemoveAll"
		Me.cmdRemoveAll.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRemoveAll.Size = New System.Drawing.Size(73, 22)
		Me.cmdRemoveAll.TabIndex = 4
		Me.cmdRemoveAll.TabStop = True
		Me.cmdRemoveAll.Text = "<< Group"
		Me.cmdRemoveAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRemoveAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(424, 368)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(344, 368)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwSelected
		' 
		Me.lvwSelected.BackColor = System.Drawing.SystemColors.Window
		Me.lvwSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwSelected.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwSelected.HideSelection = True
		Me.lvwSelected.LabelEdit = False
		Me.lvwSelected.LabelWrap = True
		Me.lvwSelected.LargeImageList = ImageList1
		Me.lvwSelected.Location = New System.Drawing.Point(296, 24)
		Me.lvwSelected.Name = "lvwSelected"
		Me.lvwSelected.Size = New System.Drawing.Size(201, 337)
		Me.lvwSelected.SmallImageList = ImageList1
		Me.lvwSelected.TabIndex = 2
		Me.lvwSelected.View = System.Windows.Forms.View.Details
		Me.lvwSelected.Columns.Add(Me._lvwSelected_ColumnHeader_1)
		' 
		' _lvwSelected_ColumnHeader_1
		' 
		Me._lvwSelected_ColumnHeader_1.Width = 201
		' 
		' lvwAvailable
		' 
		Me.lvwAvailable.BackColor = System.Drawing.SystemColors.Window
		Me.lvwAvailable.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwAvailable.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwAvailable.HideSelection = True
		Me.lvwAvailable.LabelEdit = False
		Me.lvwAvailable.LabelWrap = True
		Me.lvwAvailable.LargeImageList = ImageList1
		Me.lvwAvailable.Location = New System.Drawing.Point(8, 24)
		Me.lvwAvailable.Name = "lvwAvailable"
		Me.lvwAvailable.Size = New System.Drawing.Size(201, 337)
		Me.lvwAvailable.SmallImageList = ImageList1
		Me.lvwAvailable.TabIndex = 3
		Me.lvwAvailable.View = System.Windows.Forms.View.Details
		Me.lvwAvailable.Columns.Add(Me._lvwAvailable_ColumnHeader_1)
		' 
		' _lvwAvailable_ColumnHeader_1
		' 
		Me._lvwAvailable_ColumnHeader_1.Width = 201
		' 
		' ImageList1
		' 
		Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
		Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.ImageList1.Images.SetKeyName(0, "RiskImage")
		' 
		' lblAvailable
		' 
		Me.lblAvailable.AutoSize = True
		Me.lblAvailable.BackColor = System.Drawing.SystemColors.Control
		Me.lblAvailable.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAvailable.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAvailable.Enabled = True
		Me.lblAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAvailable.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAvailable.Location = New System.Drawing.Point(8, 8)
		Me.lblAvailable.Name = "lblAvailable"
		Me.lblAvailable.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAvailable.Size = New System.Drawing.Size(125, 13)
		Me.lblAvailable.TabIndex = 9
		Me.lblAvailable.Text = "Risk Groups Available"
		Me.lblAvailable.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAvailable.UseMnemonic = True
		Me.lblAvailable.Visible = True
		' 
		' lblSelected
		' 
		Me.lblSelected.AutoSize = True
		Me.lblSelected.BackColor = System.Drawing.SystemColors.Control
		Me.lblSelected.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSelected.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSelected.Enabled = True
		Me.lblSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSelected.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSelected.Location = New System.Drawing.Point(296, 8)
		Me.lblSelected.Name = "lblSelected"
		Me.lblSelected.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSelected.Size = New System.Drawing.Size(122, 13)
		Me.lblSelected.TabIndex = 8
		Me.lblSelected.Text = "Risk Groups Selected"
		Me.lblSelected.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblSelected.UseMnemonic = True
		Me.lblSelected.Visible = True
		' 
		' frmRiskGroups
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(505, 397)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdAddAll)
		Me.Controls.Add(Me.cmdRemove)
		Me.Controls.Add(Me.cmdAdd)
		Me.Controls.Add(Me.cmdRemoveAll)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.lvwSelected)
		Me.Controls.Add(Me.lvwAvailable)
		Me.Controls.Add(Me.lblAvailable)
		Me.Controls.Add(Me.lblSelected)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmRiskGroups"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "RiskGroups"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.lvwSelected.ResumeLayout(False)
		Me.lvwAvailable.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class