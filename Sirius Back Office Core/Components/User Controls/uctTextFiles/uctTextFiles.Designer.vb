<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctTextFiles
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwSearchDetails_InitializeColumnKeys()
	End Sub
    'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Friend WithEvents lblClaim As System.Windows.Forms.Label
	Friend WithEvents lblPolicy As System.Windows.Forms.Label
	Friend WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Friend WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Friend WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Friend WithEvents txtClaim As System.Windows.Forms.TextBox
	Friend WithEvents txtPolicy As System.Windows.Forms.TextBox
	Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
	Friend WithEvents imglImages As System.Windows.Forms.ImageList
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctTextFiles))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblClaim = New System.Windows.Forms.Label
        Me.lblPolicy = New System.Windows.Forms.Label
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.txtClaim = New System.Windows.Forms.TextBox
        Me.txtPolicy = New System.Windows.Forms.TextBox
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(605, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(597, 312)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClaim)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwSearchDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClaim)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPolicy)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(589, 286)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Text Files"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblClaim
        '
        Me.lblClaim.BackColor = System.Drawing.SystemColors.Control
        Me.lblClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClaim.Location = New System.Drawing.Point(288, 15)
        Me.lblClaim.Name = "lblClaim"
        Me.lblClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClaim.Size = New System.Drawing.Size(81, 17)
        Me.lblClaim.TabIndex = 5
        Me.lblClaim.Text = "Claim code:"
        '
        'lblPolicy
        '
        Me.lblPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicy.Location = New System.Drawing.Point(8, 15)
        Me.lblPolicy.Name = "lblPolicy"
        Me.lblPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicy.Size = New System.Drawing.Size(81, 17)
        Me.lblPolicy.TabIndex = 6
        Me.lblPolicy.Text = "Policy code:"
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4})
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(8, 44)
        Me.lvwSearchDetails.MultiSelect = False
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(578, 239)
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.lvwSearchDetails.TabIndex = 3
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = "Slot"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 49
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_2.Text = "Description"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 289
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_3.Text = "File"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 134
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_4.Text = "Branch"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 34
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'txtClaim
        '
        Me.txtClaim.AcceptsReturn = True
        Me.txtClaim.BackColor = System.Drawing.SystemColors.Window
        Me.txtClaim.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClaim.Enabled = False
        Me.txtClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClaim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClaim.Location = New System.Drawing.Point(376, 12)
        Me.txtClaim.MaxLength = 30
        Me.txtClaim.Name = "txtClaim"
        Me.txtClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClaim.Size = New System.Drawing.Size(145, 20)
        Me.txtClaim.TabIndex = 1
        '
        'txtPolicy
        '
        Me.txtPolicy.AcceptsReturn = True
        Me.txtPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicy.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicy.Enabled = False
        Me.txtPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicy.Location = New System.Drawing.Point(96, 12)
        Me.txtPolicy.MaxLength = 30
        Me.txtPolicy.Name = "txtPolicy"
        Me.txtPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicy.Size = New System.Drawing.Size(145, 20)
        Me.txtPolicy.TabIndex = 2
        '
        'stbStatus
        '
        Me.stbStatus.Dock = System.Windows.Forms.DockStyle.None
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(12, 315)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(602, 22)
        Me.stbStatus.SizingGrip = False
        Me.stbStatus.TabIndex = 4
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
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(585, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'uctTextFiles
        '
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.stbStatus)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctTextFiles"
        Me.Size = New System.Drawing.Size(600, 342)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub lvwSearchDetails_InitializeColumnKeys()
		Me._lvwSearchDetails_ColumnHeader_1.Name = ""
		Me._lvwSearchDetails_ColumnHeader_2.Name = ""
		Me._lvwSearchDetails_ColumnHeader_3.Name = ""
		Me._lvwSearchDetails_ColumnHeader_4.Name = ""
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("lvwSearchDetailsClickEventArgs_NET.lvwSearchDetailsClickEventArgs")> _
	Public NotInheritable Class lvwSearchDetailsClickEventArgs
		Inherits System.EventArgs
		Public lFileNumber As Integer
		Public Sub New(ByRef lFileNumber As Integer)
			MyBase.New()
			Me.lFileNumber = lFileNumber
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseUpEventArgs_NET.MouseUpEventArgs")> _
	Public NotInheritable Class MouseUpEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public x As Single
		Public y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.x = x
			Me.y = y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseMoveEventArgs_NET.MouseMoveEventArgs")> _
	Public NotInheritable Class MouseMoveEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public x As Single
		Public y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.x = x
			Me.y = y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("MouseDownEventArgs_NET.MouseDownEventArgs")> _
	Public NotInheritable Class MouseDownEventArgs
		Inherits System.EventArgs
		Public Button As Integer
		Public Shift As Integer
		Public x As Single
		Public y As Single
		Public Sub New(ByRef Button As Integer, ByRef Shift As Integer, ByRef x As Single, ByRef y As Single)
			MyBase.New()
			Me.Button = Button
			Me.Shift = Shift
			Me.x = x
			Me.y = y
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyUpEventArgs_NET.KeyUpEventArgs")> _
	Public NotInheritable Class KeyUpEventArgs
		Inherits System.EventArgs
		Public KeyCode As Integer
		Public Shift As Integer
		Public Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
			MyBase.New()
			Me.KeyCode = KeyCode
			Me.Shift = Shift
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyPressEventArgs_NET.KeyPressEventArgs")> _
	Public NotInheritable Class KeyPressEventArgs
		Inherits System.EventArgs
		Public KeyAscii As Integer
		Public Sub New(ByRef KeyAscii As Integer)
			MyBase.New()
			Me.KeyAscii = KeyAscii
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("KeyDownEventArgs_NET.KeyDownEventArgs")> _
	Public NotInheritable Class KeyDownEventArgs
		Inherits System.EventArgs
		Public KeyCode As Integer
		Public Shift As Integer
		Public Sub New(ByRef KeyCode As Integer, ByRef Shift As Integer)
			MyBase.New()
			Me.KeyCode = KeyCode
			Me.Shift = Shift
		End Sub
	End Class
#End Region 
End Class