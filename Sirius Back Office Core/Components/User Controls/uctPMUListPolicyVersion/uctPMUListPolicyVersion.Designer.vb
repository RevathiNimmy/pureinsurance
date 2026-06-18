<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPMUListPolicy
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
	Friend WithEvents lblClient As System.Windows.Forms.Label
	Friend WithEvents imglImages As System.Windows.Forms.ImageList
	Friend WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Friend WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Friend WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Friend WithEvents _lvwSearchDetails_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Friend WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Friend WithEvents txtClientCode As System.Windows.Forms.TextBox
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPMUListPolicy))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblClient = New System.Windows.Forms.Label
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.txtClientCode = New System.Windows.Forms.TextBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(576, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(3, 3)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(581, 343)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClient)
        Me._tabMainTab_TabPage0.Controls.Add(Me.stbStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwSearchDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtClientCode)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(573, 317)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Policy                                                                           " & _
            "                                                         "
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblClient
        '
        Me.lblClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClient.Location = New System.Drawing.Point(16, 12)
        Me.lblClient.Name = "lblClient"
        Me.lblClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClient.Size = New System.Drawing.Size(81, 17)
        Me.lblClient.TabIndex = 3
        Me.lblClient.Text = "Client code:"
        '
        'stbStatus
        '
        Me.stbStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.stbStatus.Dock = System.Windows.Forms.DockStyle.None
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.GripMargin = New System.Windows.Forms.Padding(0)
        Me.stbStatus.ImageScalingSize = New System.Drawing.Size(0, 0)
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(9, 290)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(601, 22)
        Me.stbStatus.SizingGrip = False
        Me.stbStatus.TabIndex = 4
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(553, 19)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5, Me._lvwSearchDetails_ColumnHeader_6, Me._lvwSearchDetails_ColumnHeader_7, Me._lvwSearchDetails_ColumnHeader_8})
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(9, 46)
        Me.lvwSearchDetails.MultiSelect = False
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(553, 235)
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.lvwSearchDetails.TabIndex = 2
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = "Status"
        Me._lvwSearchDetails_ColumnHeader_1.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_2.Text = "Type"
        Me._lvwSearchDetails_ColumnHeader_2.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_3.Text = "Cover Start"
        Me._lvwSearchDetails_ColumnHeader_3.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_4.Text = "Cover End"
        Me._lvwSearchDetails_ColumnHeader_4.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_5.Text = "Renewal"
        Me._lvwSearchDetails_ColumnHeader_5.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_6
        '
        Me._lvwSearchDetails_ColumnHeader_6.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_6.Text = "Premium"
        Me._lvwSearchDetails_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwSearchDetails_ColumnHeader_6.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_7
        '
        Me._lvwSearchDetails_ColumnHeader_7.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_7.Text = "MTA Reason"
        Me._lvwSearchDetails_ColumnHeader_7.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_8
        '
        Me._lvwSearchDetails_ColumnHeader_8.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_8.Text = "Event Description"
        Me._lvwSearchDetails_ColumnHeader_8.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        Me.imglImages.Images.SetKeyName(1, "CurrentPolicyVersion")
        '
        'txtClientCode
        '
        Me.txtClientCode.AcceptsReturn = True
        Me.txtClientCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtClientCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientCode.Enabled = False
        Me.txtClientCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientCode.Location = New System.Drawing.Point(104, 12)
        Me.txtClientCode.MaxLength = 30
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientCode.Size = New System.Drawing.Size(217, 20)
        Me.txtClientCode.TabIndex = 1
        '
        'uctPMUListPolicy
        '
        Me.Controls.Add(Me.tabMainTab)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPMUListPolicy"
        Me.Size = New System.Drawing.Size(584, 350)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
	Sub lvwSearchDetails_InitializeColumnKeys()
		Me._lvwSearchDetails_ColumnHeader_1.Name = ""
		Me._lvwSearchDetails_ColumnHeader_2.Name = ""
		Me._lvwSearchDetails_ColumnHeader_3.Name = ""
		Me._lvwSearchDetails_ColumnHeader_4.Name = ""
		Me._lvwSearchDetails_ColumnHeader_5.Name = ""
		Me._lvwSearchDetails_ColumnHeader_6.Name = ""
		Me._lvwSearchDetails_ColumnHeader_7.Name = ""
		Me._lvwSearchDetails_ColumnHeader_8.Name = ""
    End Sub
#End Region
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("lvwSearchDetailsMouseDownEventArgs_NET.lvwSearchDetailsMouseDownEventArgs")> _
	Public NotInheritable Class lvwSearchDetailsMouseDownEventArgs
		Inherits System.EventArgs
		Public m_lSelected As Integer
		Public Sub New(ByRef m_lSelected As Integer)
			MyBase.New()
			Me.m_lSelected = m_lSelected
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("lvwSearchDetailsDblClickEventArgs_NET.lvwSearchDetailsDblClickEventArgs")> _
	Public NotInheritable Class lvwSearchDetailsDblClickEventArgs
		Inherits System.EventArgs
		Public m_lInsHolderCnt As Integer
		Public m_lInsuranceFolderCnt As Integer
		Public m_lInsFileCnt As Integer
		Public m_sShortName As String = ""
		Public m_sInsReference As String = ""
		Public m_lPolicyTypeID As Integer
		Public Sub New(ByRef m_lInsHolderCnt As Integer, ByRef m_lInsuranceFolderCnt As Integer, ByRef m_lInsFileCnt As Integer, ByRef m_sShortName As String, ByRef m_sInsReference As String, ByRef m_lPolicyTypeID As Integer)
			MyBase.New()
			Me.m_lInsHolderCnt = m_lInsHolderCnt
			Me.m_lInsuranceFolderCnt = m_lInsuranceFolderCnt
			Me.m_lInsFileCnt = m_lInsFileCnt
			Me.m_sShortName = m_sShortName
			Me.m_sInsReference = m_sInsReference
			Me.m_lPolicyTypeID = m_lPolicyTypeID
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