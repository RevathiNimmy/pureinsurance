<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmParties
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwParties_InitializeColumnKeys()
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
	Private WithEvents _lvwParties_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwParties As System.Windows.Forms.ListView
	Public WithEvents imgListImages As System.Windows.Forms.ImageList
	Public WithEvents imgImage As System.Windows.Forms.PictureBox
	Public WithEvents Label1 As System.Windows.Forms.Label
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmParties))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.lvwParties = New System.Windows.Forms.ListView
		Me._lvwParties_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me.imgListImages = New System.Windows.Forms.ImageList
		Me.imgImage = New System.Windows.Forms.PictureBox
		Me.Label1 = New System.Windows.Forms.Label
		Me.lvwParties.SuspendLayout()
		Me.SuspendLayout()
        'Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(240, 200)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 3
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
		Me.cmdOK.Location = New System.Drawing.Point(80, 200)
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
		Me.cmdCancel.Location = New System.Drawing.Point(160, 200)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 2
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwParties
		' 
		Me.lvwParties.BackColor = System.Drawing.SystemColors.Window
		Me.lvwParties.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwParties.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwParties.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwParties.HideSelection = False
		Me.lvwParties.LabelEdit = False
		Me.lvwParties.LabelWrap = False
		Me.lvwParties.LargeImageList = imgListImages
		Me.lvwParties.Location = New System.Drawing.Point(8, 48)
		Me.lvwParties.Name = "lvwParties"
		Me.lvwParties.Size = New System.Drawing.Size(305, 145)
		Me.lvwParties.SmallImageList = imgListImages
		Me.lvwParties.TabIndex = 0
		Me.lvwParties.View = System.Windows.Forms.View.Details
		Me.lvwParties.Columns.Add(Me._lvwParties_ColumnHeader_1)
		' 
		' _lvwParties_ColumnHeader_1
		' 
		Me._lvwParties_ColumnHeader_1.Tag = ""
		Me._lvwParties_ColumnHeader_1.Text = "Description"
		Me._lvwParties_ColumnHeader_1.Width = 283
		' 
		' imgListImages
		' 
		Me.imgListImages.ImageSize = New System.Drawing.Size(16, 16)
		Me.imgListImages.ImageStream = CType(resources.GetObject("imgListImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgListImages.TransparentColor = System.Drawing.Color.FromArgb(255, 255, 255)
		Me.imgListImages.Images.SetKeyName(0, "")
		' 
		' imgImage
		' 
		Me.imgImage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgImage.Cursor = System.Windows.Forms.Cursors.Default
		Me.imgImage.Enabled = True
		Me.imgImage.Image = CType(resources.GetObject("imgImage.Image"), System.Drawing.Image)
		Me.imgImage.Location = New System.Drawing.Point(280, 8)
		Me.imgImage.Name = "imgImage"
		Me.imgImage.Size = New System.Drawing.Size(32, 32)
		Me.imgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgImage.Visible = True
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
		Me.Label1.Location = New System.Drawing.Point(8, 16)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(265, 33)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = "Please select a new Party to add from the list below:-"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmParties
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(321, 228)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.lvwParties)
		Me.Controls.Add(Me.imgImage)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmParties.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(255, 201)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmParties"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Select a new Party"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
        'Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwParties, True)
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwParties.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwParties_InitializeColumnKeys()
		Me._lvwParties_ColumnHeader_1.Name = ""
	End Sub
#End Region 
End Class