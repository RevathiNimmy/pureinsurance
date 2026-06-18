<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectGIIPolType
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwGIIProcessTyes_InitializeColumnKeys()
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
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Private WithEvents _lvwGIIProcessTyes_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwGIIProcessTyes As System.Windows.Forms.ListView
	Public WithEvents imgListImages As System.Windows.Forms.ImageList
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectGIIPolType))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.lvwGIIProcessTyes = New System.Windows.Forms.ListView
		Me._lvwGIIProcessTyes_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me.imgListImages = New System.Windows.Forms.ImageList
		Me.Label1 = New System.Windows.Forms.Label
		Me.lvwGIIProcessTyes.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = False
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(208, 200)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 2
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(48, 200)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 1
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
		Me.cmdCancel.Location = New System.Drawing.Point(128, 200)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 0
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwGIIProcessTyes
		' 
		Me.lvwGIIProcessTyes.BackColor = System.Drawing.SystemColors.Window
		Me.lvwGIIProcessTyes.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwGIIProcessTyes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwGIIProcessTyes.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwGIIProcessTyes.HideSelection = False
		Me.lvwGIIProcessTyes.LabelEdit = False
		Me.lvwGIIProcessTyes.LabelWrap = False
		Me.lvwGIIProcessTyes.LargeImageList = imgListImages
		Me.lvwGIIProcessTyes.Location = New System.Drawing.Point(8, 48)
		Me.lvwGIIProcessTyes.Name = "lvwGIIProcessTyes"
		Me.lvwGIIProcessTyes.Size = New System.Drawing.Size(273, 145)
		Me.lvwGIIProcessTyes.SmallImageList = imgListImages
		Me.lvwGIIProcessTyes.TabIndex = 3
		Me.lvwGIIProcessTyes.View = System.Windows.Forms.View.Details
		Me.lvwGIIProcessTyes.Columns.Add(Me._lvwGIIProcessTyes_ColumnHeader_1)
		' 
		' _lvwGIIProcessTyes_ColumnHeader_1
		' 
		Me._lvwGIIProcessTyes_ColumnHeader_1.Tag = ""
		Me._lvwGIIProcessTyes_ColumnHeader_1.Text = "Available processes"
		Me._lvwGIIProcessTyes_ColumnHeader_1.Width = 268
		' 
		' imgListImages
		' 
		Me.imgListImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imgListImages.ImageStream = CType(resources.GetObject("imgListImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgListImages.TransparentColor = System.Drawing.Color.FromArgb(255, 255, 255)
		Me.imgListImages.Images.SetKeyName(0, "FindImage")
		Me.imgListImages.Images.SetKeyName(1, "SirIcon")
		Me.imgListImages.Images.SetKeyName(2, "IIcon")
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(8, 0)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(273, 41)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = "Select the type of transaction you wish to commit with the selected policy"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmSelectGIIPolType
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(292, 226)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.lvwGIIProcessTyes)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmSelectGIIPolType.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSelectGIIPolType"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Available Gemini II Processes"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwGIIProcessTyes, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwGIIProcessTyes.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwGIIProcessTyes_InitializeColumnKeys()
		Me._lvwGIIProcessTyes_ColumnHeader_1.Name = ""
	End Sub
#End Region 
End Class