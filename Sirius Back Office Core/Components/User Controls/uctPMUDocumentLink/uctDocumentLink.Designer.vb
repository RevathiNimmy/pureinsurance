<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctDocumentLink
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Friend WithEvents lblFilterByProcess As System.Windows.Forms.Label
	Friend WithEvents lblFilterByBranch As System.Windows.Forms.Label
	Friend WithEvents cboFilterByBranch As PMLookupControl.cboPMLookup
	Friend WithEvents cmdUp As System.Windows.Forms.Button
	Friend WithEvents cmdDown As System.Windows.Forms.Button
	Friend WithEvents cmdRemove As System.Windows.Forms.Button
	Friend WithEvents cmdCopy As System.Windows.Forms.Button
	Friend WithEvents cmdAdd As System.Windows.Forms.Button
	Friend WithEvents cmdEdit As System.Windows.Forms.Button
	Friend WithEvents lvwSearchDetails As System.Windows.Forms.ListView
	Friend WithEvents cboFilterByProcess As System.Windows.Forms.ComboBox
	Friend WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Friend WithEvents tabMainTab As System.Windows.Forms.TabControl
	Friend WithEvents imglImages As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctDocumentLink))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblFilterByProcess = New System.Windows.Forms.Label
        Me.lblFilterByBranch = New System.Windows.Forms.Label
        Me.cboFilterByBranch = New PMLookupControl.cboPMLookup
        Me.cmdUp = New System.Windows.Forms.Button
        Me.cmdDown = New System.Windows.Forms.Button
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.cmdCopy = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me.cboFilterByProcess = New System.Windows.Forms.ComboBox
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(752, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(1, 2)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(785, 500)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblFilterByProcess)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblFilterByBranch)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboFilterByBranch)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdUp)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDown)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRemove)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdCopy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwSearchDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboFilterByProcess)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(777, 474)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Document Links"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblFilterByProcess
        '
        Me.lblFilterByProcess.BackColor = System.Drawing.SystemColors.Control
        Me.lblFilterByProcess.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFilterByProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilterByProcess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFilterByProcess.Location = New System.Drawing.Point(16, 12)
        Me.lblFilterByProcess.Name = "lblFilterByProcess"
        Me.lblFilterByProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFilterByProcess.Size = New System.Drawing.Size(113, 17)
        Me.lblFilterByProcess.TabIndex = 2
        Me.lblFilterByProcess.Text = "Filter by Process:"
        '
        'lblFilterByBranch
        '
        Me.lblFilterByBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblFilterByBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFilterByBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilterByBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFilterByBranch.Location = New System.Drawing.Point(365, 12)
        Me.lblFilterByBranch.Name = "lblFilterByBranch"
        Me.lblFilterByBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFilterByBranch.Size = New System.Drawing.Size(109, 17)
        Me.lblFilterByBranch.TabIndex = 1
        Me.lblFilterByBranch.Text = "Filter by Branch:"
        '
        'cboFilterByBranch
        '
        'Me.cboFilterByBranch.DefaultItemId = 0
        'Me.cboFilterByBranch.FirstItem = ""
        'Me.cboFilterByBranch.ItemId = 0
        Me.cboFilterByBranch.ListIndex = 0
        Me.cboFilterByBranch.Location = New System.Drawing.Point(477, 10)
        Me.cboFilterByBranch.Name = "cboFilterByBranch"
        Me.cboFilterByBranch.PMLookupProductFamily = 1
        Me.cboFilterByBranch.SingleItemId = 0
        Me.cboFilterByBranch.Size = New System.Drawing.Size(196, 21)
        Me.cboFilterByBranch.Sorted = True
        Me.cboFilterByBranch.TabIndex = 10
        Me.cboFilterByBranch.TableName = "Source"
        Me.cboFilterByBranch.ToolTipText = ""
        Me.cboFilterByBranch.WhereClause = ""
        '
        'cmdUp
        '
        Me.cmdUp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUp.Location = New System.Drawing.Point(680, 267)
        Me.cmdUp.Name = "cmdUp"
        Me.cmdUp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUp.Size = New System.Drawing.Size(66, 22)
        Me.cmdUp.TabIndex = 3
        Me.cmdUp.Text = "&Up"
        Me.cmdUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUp.UseVisualStyleBackColor = False
        '
        'cmdDown
        '
        Me.cmdDown.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDown.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDown.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDown.Location = New System.Drawing.Point(680, 296)
        Me.cmdDown.Name = "cmdDown"
        Me.cmdDown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDown.Size = New System.Drawing.Size(66, 22)
        Me.cmdDown.TabIndex = 4
        Me.cmdDown.Text = "&Down"
        Me.cmdDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDown.UseVisualStyleBackColor = False
        '
        'cmdRemove
        '
        Me.cmdRemove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRemove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRemove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRemove.Location = New System.Drawing.Point(680, 98)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRemove.Size = New System.Drawing.Size(66, 22)
        Me.cmdRemove.TabIndex = 5
        Me.cmdRemove.Text = "&Remove"
        Me.cmdRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRemove.UseVisualStyleBackColor = False
        '
        'cmdCopy
        '
        Me.cmdCopy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCopy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCopy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCopy.Location = New System.Drawing.Point(680, 127)
        Me.cmdCopy.Name = "cmdCopy"
        Me.cmdCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCopy.Size = New System.Drawing.Size(66, 22)
        Me.cmdCopy.TabIndex = 6
        Me.cmdCopy.Text = "&Copy"
        Me.cmdCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCopy.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(680, 39)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(66, 22)
        Me.cmdAdd.TabIndex = 7
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(680, 69)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(66, 22)
        Me.cmdEdit.TabIndex = 8
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSearchDetails, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDetails, False)
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.LabelEdit = True
        Me.listViewHelper1.SetLargeIcons(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.Location = New System.Drawing.Point(6, 37)
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(670, 290)
        Me.listViewHelper1.SetSmallIcons(Me.lvwSearchDetails, "")
        Me.listViewHelper1.SetSorted(Me.lvwSearchDetails, False)
        Me.listViewHelper1.SetSortKey(Me.lvwSearchDetails, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwSearchDetails, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSearchDetails.TabIndex = 9
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        '
        'cboFilterByProcess
        '
        Me.cboFilterByProcess.BackColor = System.Drawing.SystemColors.Window
        Me.cboFilterByProcess.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboFilterByProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFilterByProcess.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboFilterByProcess.Location = New System.Drawing.Point(140, 10)
        Me.cboFilterByProcess.Name = "cboFilterByProcess"
        Me.cboFilterByProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboFilterByProcess.Size = New System.Drawing.Size(217, 21)
        Me.cboFilterByProcess.TabIndex = 11
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        Me.imglImages.Images.SetKeyName(1, "NotesImage")
        Me.imglImages.Images.SetKeyName(2, "NotesImage2")
        Me.imglImages.Images.SetKeyName(3, "NotesImage3")
        '
        'uctDocumentLink
        '
        Me.Controls.Add(Me.tabMainTab)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctDocumentLink"
        Me.Size = New System.Drawing.Size(806, 600)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
#Region "Upgrade Support"
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