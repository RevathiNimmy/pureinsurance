<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InstanceManager
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializeimgImages()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	 Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			UserControl_Terminate()
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Friend WithEvents picSplitter As System.Windows.Forms.PictureBox
	Friend WithEvents cmdDelete As System.Windows.Forms.Button
	Friend WithEvents cmdAddNew As System.Windows.Forms.Button
	Friend WithEvents picButtons As System.Windows.Forms.PictureBox
	Friend WithEvents lvListView As System.Windows.Forms.ListView
	Friend WithEvents pnlHeader As System.Windows.Forms.Panel
	Friend WithEvents tvTreeView As System.Windows.Forms.TreeView
	Friend WithEvents imgSplitter As System.Windows.Forms.PictureBox
	Friend WithEvents _imgImages_0 As System.Windows.Forms.ImageList
	Friend WithEvents _imgImages_1 As System.Windows.Forms.ImageList
	Friend imgImages(1) As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InstanceManager))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.picSplitter = New System.Windows.Forms.PictureBox
		Me.picButtons = New System.Windows.Forms.PictureBox
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.cmdAddNew = New System.Windows.Forms.Button
		Me.lvListView = New System.Windows.Forms.ListView
		Me.pnlHeader = New System.Windows.Forms.Panel
		Me.tvTreeView = New System.Windows.Forms.TreeView
		Me.imgSplitter = New System.Windows.Forms.PictureBox
		Me._imgImages_0 = New System.Windows.Forms.ImageList
		Me._imgImages_1 = New System.Windows.Forms.ImageList
		Me.picButtons.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' picSplitter
		' 
		Me.picSplitter.BackColor = System.Drawing.SystemColors.ControlDark
		Me.picSplitter.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.picSplitter.CausesValidation = True
		Me.picSplitter.Cursor = System.Windows.Forms.Cursors.Default
		Me.picSplitter.Dock = System.Windows.Forms.DockStyle.None
		Me.picSplitter.Enabled = True
		Me.picSplitter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.picSplitter.Location = New System.Drawing.Point(4, 132)
		Me.picSplitter.Name = "picSplitter"
		Me.picSplitter.Size = New System.Drawing.Size(281, 9)
		Me.picSplitter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.picSplitter.TabIndex = 3
		Me.picSplitter.TabStop = True
		Me.picSplitter.Visible = False
		' 
		' picButtons
		' 
		Me.picButtons.BackColor = System.Drawing.SystemColors.Control
		Me.picButtons.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.picButtons.CausesValidation = True
		Me.picButtons.Controls.Add(Me.cmdDelete)
		Me.picButtons.Controls.Add(Me.cmdAddNew)
		Me.picButtons.Cursor = System.Windows.Forms.Cursors.Default
		Me.picButtons.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.picButtons.Enabled = True
		Me.picButtons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.picButtons.Location = New System.Drawing.Point(0, 347)
		Me.picButtons.Name = "picButtons"
		Me.picButtons.Size = New System.Drawing.Size(313, 33)
		Me.picButtons.TabIndex = 0
		Me.picButtons.TabStop = True
		Me.picButtons.Visible = True
		' 
		' cmdDelete
		' 
		Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDelete.CausesValidation = True
		Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDelete.Enabled = False
		Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDelete.Location = New System.Drawing.Point(100, 4)
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.Size = New System.Drawing.Size(85, 23)
		Me.cmdDelete.TabIndex = 2
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Text = "&Delete"
		Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAddNew
		' 
		Me.cmdAddNew.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddNew.CausesValidation = True
		Me.cmdAddNew.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAddNew.Enabled = False
		Me.cmdAddNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAddNew.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAddNew.Location = New System.Drawing.Point(8, 4)
		Me.cmdAddNew.Name = "cmdAddNew"
		Me.cmdAddNew.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAddNew.Size = New System.Drawing.Size(85, 23)
		Me.cmdAddNew.TabIndex = 1
		Me.cmdAddNew.TabStop = True
		Me.cmdAddNew.Text = "A&dd ..."
		Me.cmdAddNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAddNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvListView
		' 
		Me.lvListView.BackColor = System.Drawing.SystemColors.Window
		Me.lvListView.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvListView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvListView.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvListView.HideSelection = False
		Me.lvListView.LabelEdit = False
		Me.lvListView.LabelWrap = True
		Me.lvListView.Location = New System.Drawing.Point(5, 188)
		Me.lvListView.Name = "lvListView"
		Me.lvListView.Size = New System.Drawing.Size(281, 97)
		Me.lvListView.SmallImageList = _imgImages_1
		Me.lvListView.TabIndex = 4
		Me.lvListView.View = System.Windows.Forms.View.Details
		' 
		' pnlHeader
		' 
		Me.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.pnlHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pnlHeader.Location = New System.Drawing.Point(4, 152)
		Me.pnlHeader.Name = "pnlHeader"
		Me.pnlHeader.Size = New System.Drawing.Size(281, 25)
		Me.pnlHeader.TabIndex = 5
		' 
		' tvTreeView
		' 
		Me.tvTreeView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.tvTreeView.CausesValidation = True
		Me.tvTreeView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tvTreeView.HideSelection = False
		Me.tvTreeView.ImageList = _imgImages_0
		Me.tvTreeView.Indent = 24
		Me.tvTreeView.LabelEdit = False
		Me.tvTreeView.LabelEdit = True
		Me.tvTreeView.Location = New System.Drawing.Point(4, 4)
		Me.tvTreeView.Name = "tvTreeView"
		Me.tvTreeView.Size = New System.Drawing.Size(285, 73)
		Me.tvTreeView.TabIndex = 6
		' 
		' imgSplitter
		' 
		Me.imgSplitter.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.imgSplitter.Cursor = System.Windows.Forms.Cursors.SizeNS
		Me.imgSplitter.Enabled = True
		Me.imgSplitter.Location = New System.Drawing.Point(4, 144)
		Me.imgSplitter.Name = "imgSplitter"
		Me.imgSplitter.Size = New System.Drawing.Size(277, 5)
		Me.imgSplitter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.imgSplitter.Visible = True
		' 
		' _imgImages_0
		' 
		Me._imgImages_0.ImageSize = New System.Drawing.Size(16, 16)
		Me._imgImages_0.ImageStream = CType(resources.GetObject("_imgImages_0.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me._imgImages_0.Images.SetKeyName(0, "closed")
		Me._imgImages_0.Images.SetKeyName(1, "open")
		Me._imgImages_0.Images.SetKeyName(2, "leaf")
		Me._imgImages_0.Images.SetKeyName(3, "scheme")
		' 
		' _imgImages_1
		' 
		Me._imgImages_1.ImageSize = New System.Drawing.Size(16, 16)
		Me._imgImages_1.ImageStream = CType(resources.GetObject("_imgImages_1.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me._imgImages_1.TransparentColor = System.Drawing.Color.FromArgb(192, 192, 192)
		Me._imgImages_1.Images.SetKeyName(0, "closed")
		Me._imgImages_1.Images.SetKeyName(1, "leaf")
		' 
		' InstanceManager
		' 
		Me.ClientSize = New System.Drawing.Size(313, 380)
		Me.Controls.Add(Me.picSplitter)
		Me.Controls.Add(Me.picButtons)
		Me.Controls.Add(Me.lvListView)
		Me.Controls.Add(Me.pnlHeader)
		Me.Controls.Add(Me.tvTreeView)
		Me.Controls.Add(Me.imgSplitter)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "InstanceManager"
		Me.listViewHelper1.SetItemClickMethod(Me.lvListView, "lvListView_ItemClick")
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvListView, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.picButtons.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub InitializeimgImages()
		Me.imgImages(0) = _imgImages_0
		Me.imgImages(1) = _imgImages_1
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("ShowInstanceEventArgs_NET.ShowInstanceEventArgs")> _
	Public NotInheritable Class ShowInstanceEventArgs
		Inherits System.EventArgs
		Public Level1 As Integer
		Public Level2 As Integer
		Public Level3 As Integer
		Public Level4 As Integer
		Public Level5 As Integer
		Public Level6 As Integer
		Public Sub New(ByRef Level1 As Integer, ByRef Level2 As Integer, ByRef Level3 As Integer, ByRef Level4 As Integer, ByRef Level5 As Integer, ByRef Level6 As Integer)
			MyBase.New()
			Me.Level1 = Level1
			Me.Level2 = Level2
			Me.Level3 = Level3
			Me.Level4 = Level4
			Me.Level5 = Level5
			Me.Level6 = Level6
		End Sub
	End Class
#End Region 
End Class