<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPickList
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents trvPickList As System.Windows.Forms.TreeView
	Public WithEvents SSFrame1 As System.Windows.Forms.GroupBox
	Public WithEvents imgList As System.Windows.Forms.ImageList
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPickList))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.SSFrame1 = New System.Windows.Forms.GroupBox
		Me.trvPickList = New System.Windows.Forms.TreeView
		Me.imgList = New System.Windows.Forms.ImageList
		Me.SSFrame1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(392, 196)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(81, 25)
		Me.cmdCancel.TabIndex = 2
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
		Me.cmdOK.Location = New System.Drawing.Point(300, 196)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(81, 25)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' SSFrame1
		' 
		Me.SSFrame1.BackColor = System.Drawing.SystemColors.Control
		Me.SSFrame1.Controls.Add(Me.trvPickList)
		Me.SSFrame1.Enabled = True
		Me.SSFrame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.SSFrame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.SSFrame1.Location = New System.Drawing.Point(5, 5)
		Me.SSFrame1.Name = "SSFrame1"
		Me.SSFrame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.SSFrame1.Size = New System.Drawing.Size(469, 181)
		Me.SSFrame1.TabIndex = 0
		Me.SSFrame1.Visible = True
		' 
		' trvPickList
		' 
		Me.trvPickList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.trvPickList.CausesValidation = True
		Me.trvPickList.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.trvPickList.ImageList = imgList
		Me.trvPickList.LabelEdit = False
		Me.trvPickList.LabelEdit = True
		Me.trvPickList.Location = New System.Drawing.Point(8, 16)
		Me.trvPickList.Name = "trvPickList"
		Me.trvPickList.Size = New System.Drawing.Size(449, 157)
		Me.trvPickList.TabIndex = 3
		' 
		' imgList
		' 
		Me.imgList.ImageSize = New System.Drawing.Size(16, 16)
		Me.imgList.ImageStream = CType(resources.GetObject("imgList.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgList.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me.imgList.Images.SetKeyName(0, "HOUSE1")
		Me.imgList.Images.SetKeyName(1, "HOUSE2")
		Me.imgList.Images.SetKeyName(2, "PERSON1")
		' 
		' frmPickList
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(482, 230)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.SSFrame1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmPickList.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmPickList"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Select Address"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.SSFrame1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class