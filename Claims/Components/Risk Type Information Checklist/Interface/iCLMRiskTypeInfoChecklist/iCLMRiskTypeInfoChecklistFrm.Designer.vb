<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializelstInfoChkLst()
		InitializecmdMove()
		InitializecmdButton()
		InitializeLabel1()
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
	Private WithEvents _cmdButton_2 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_1 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_3 As System.Windows.Forms.Button
	Private WithEvents _cmdButton_0 As System.Windows.Forms.Button
	Private WithEvents _Label1_1 As System.Windows.Forms.Label
	Private WithEvents _Label1_0 As System.Windows.Forms.Label
	Private WithEvents _cmdMove_0 As System.Windows.Forms.Button
	Private WithEvents _lstInfoChkLst_0 As System.Windows.Forms.ListBox
	Private WithEvents _lstInfoChkLst_1 As System.Windows.Forms.ListBox
	Private WithEvents _cmdMove_1 As System.Windows.Forms.Button
	Private WithEvents _cmdMove_2 As System.Windows.Forms.Button
	Private WithEvents _cmdMove_3 As System.Windows.Forms.Button
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cboRskType As System.Windows.Forms.ComboBox
	Public WithEvents chkInfoCheckList As System.Windows.Forms.CheckBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Public Label1(1) As System.Windows.Forms.Label
	Public cmdButton(3) As System.Windows.Forms.Button
	Public cmdMove(3) As System.Windows.Forms.Button
	Public lstInfoChkLst(1) As System.Windows.Forms.ListBox
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._cmdButton_2 = New System.Windows.Forms.Button
        Me._cmdButton_1 = New System.Windows.Forms.Button
        Me._cmdButton_3 = New System.Windows.Forms.Button
        Me._cmdButton_0 = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me._Label1_1 = New System.Windows.Forms.Label
        Me._Label1_0 = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me._cmdMove_0 = New System.Windows.Forms.Button
        Me._lstInfoChkLst_0 = New System.Windows.Forms.ListBox
        Me._lstInfoChkLst_1 = New System.Windows.Forms.ListBox
        Me._cmdMove_1 = New System.Windows.Forms.Button
        Me._cmdMove_2 = New System.Windows.Forms.Button
        Me._cmdMove_3 = New System.Windows.Forms.Button
        Me.cboRskType = New System.Windows.Forms.ComboBox
        Me.chkInfoCheckList = New System.Windows.Forms.CheckBox
        Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        '_cmdButton_2
        '
        Me._cmdButton_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_2.Location = New System.Drawing.Point(273, 329)
        Me._cmdButton_2.Name = "_cmdButton_2"
        Me._cmdButton_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_2.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_2.TabIndex = 2
        Me._cmdButton_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_2.UseVisualStyleBackColor = False
        '
        '_cmdButton_1
        '
        Me._cmdButton_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_1.Location = New System.Drawing.Point(193, 329)
        Me._cmdButton_1.Name = "_cmdButton_1"
        Me._cmdButton_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_1.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_1.TabIndex = 1
        Me._cmdButton_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_1.UseVisualStyleBackColor = False
        '
        '_cmdButton_3
        '
        Me._cmdButton_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_3.Location = New System.Drawing.Point(433, 329)
        Me._cmdButton_3.Name = "_cmdButton_3"
        Me._cmdButton_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_3.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_3.TabIndex = 4
        Me._cmdButton_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_3.UseVisualStyleBackColor = False
        '
        '_cmdButton_0
        '
        Me._cmdButton_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_0.Location = New System.Drawing.Point(353, 329)
        Me._cmdButton_0.Name = "_cmdButton_0"
        Me._cmdButton_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_0.TabIndex = 3
        Me._cmdButton_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_0.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(61, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(503, 320)
        Me.SSTab1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me._Label1_1)
        Me._SSTab1_TabPage0.Controls.Add(Me._Label1_0)
        Me._SSTab1_TabPage0.Controls.Add(Me.Frame1)
        Me._SSTab1_TabPage0.Controls.Add(Me.cboRskType)
        Me._SSTab1_TabPage0.Controls.Add(Me.chkInfoCheckList)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(495, 294)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Tab 0"
        '
        '_Label1_1
        '
        Me._Label1_1.AutoSize = True
        Me._Label1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_1.Location = New System.Drawing.Point(24, 51)
        Me._Label1_1.Name = "_Label1_1"
        Me._Label1_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_1.Size = New System.Drawing.Size(0, 13)
        Me._Label1_1.TabIndex = 12
        '
        '_Label1_0
        '
        Me._Label1_0.AutoSize = True
        Me._Label1_0.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_0.Location = New System.Drawing.Point(24, 19)
        Me._Label1_0.Name = "_Label1_0"
        Me._Label1_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_0.Size = New System.Drawing.Size(0, 13)
        Me._Label1_0.TabIndex = 13
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me._cmdMove_0)
        Me.Frame1.Controls.Add(Me._lstInfoChkLst_0)
        Me.Frame1.Controls.Add(Me._lstInfoChkLst_1)
        Me.Frame1.Controls.Add(Me._cmdMove_1)
        Me.Frame1.Controls.Add(Me._cmdMove_2)
        Me.Frame1.Controls.Add(Me._cmdMove_3)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 84)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(481, 202)
        Me.Frame1.TabIndex = 2
        Me.Frame1.TabStop = False
        '
        '_cmdMove_0
        '
        Me._cmdMove_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMove_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdMove_0.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdMove_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdMove_0.Location = New System.Drawing.Point(224, 41)
        Me._cmdMove_0.Name = "_cmdMove_0"
        Me._cmdMove_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdMove_0.Size = New System.Drawing.Size(50, 25)
        Me._cmdMove_0.TabIndex = 1
        Me._cmdMove_0.Text = ">"
        Me._cmdMove_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdMove_0.UseVisualStyleBackColor = False
        '
        '_lstInfoChkLst_0
        '
        Me._lstInfoChkLst_0.BackColor = System.Drawing.SystemColors.Window
        Me._lstInfoChkLst_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lstInfoChkLst_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lstInfoChkLst_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lstInfoChkLst_0.Location = New System.Drawing.Point(12, 19)
        Me._lstInfoChkLst_0.Name = "_lstInfoChkLst_0"
        Me._lstInfoChkLst_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me._lstInfoChkLst_0, System.Windows.Forms.SelectionMode.One)
        Me._lstInfoChkLst_0.Size = New System.Drawing.Size(193, 173)
        Me._lstInfoChkLst_0.TabIndex = 0
        '
        '_lstInfoChkLst_1
        '
        Me._lstInfoChkLst_1.BackColor = System.Drawing.SystemColors.Window
        Me._lstInfoChkLst_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lstInfoChkLst_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lstInfoChkLst_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._lstInfoChkLst_1.Location = New System.Drawing.Point(280, 19)
        Me._lstInfoChkLst_1.Name = "_lstInfoChkLst_1"
        Me._lstInfoChkLst_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.listBoxHelper1.SetSelectionMode(Me._lstInfoChkLst_1, System.Windows.Forms.SelectionMode.One)
        Me._lstInfoChkLst_1.Size = New System.Drawing.Size(193, 173)
        Me._lstInfoChkLst_1.TabIndex = 5
        '
        '_cmdMove_1
        '
        Me._cmdMove_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMove_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdMove_1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdMove_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdMove_1.Location = New System.Drawing.Point(224, 72)
        Me._cmdMove_1.Name = "_cmdMove_1"
        Me._cmdMove_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdMove_1.Size = New System.Drawing.Size(50, 25)
        Me._cmdMove_1.TabIndex = 2
        Me._cmdMove_1.Text = "<"
        Me._cmdMove_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdMove_1.UseVisualStyleBackColor = False
        '
        '_cmdMove_2
        '
        Me._cmdMove_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMove_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdMove_2.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdMove_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdMove_2.Location = New System.Drawing.Point(224, 105)
        Me._cmdMove_2.Name = "_cmdMove_2"
        Me._cmdMove_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdMove_2.Size = New System.Drawing.Size(50, 25)
        Me._cmdMove_2.TabIndex = 3
        Me._cmdMove_2.Text = ">>"
        Me._cmdMove_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdMove_2.UseVisualStyleBackColor = False
        '
        '_cmdMove_3
        '
        Me._cmdMove_3.BackColor = System.Drawing.SystemColors.Control
        Me._cmdMove_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdMove_3.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdMove_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdMove_3.Location = New System.Drawing.Point(224, 136)
        Me._cmdMove_3.Name = "_cmdMove_3"
        Me._cmdMove_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdMove_3.Size = New System.Drawing.Size(50, 25)
        Me._cmdMove_3.TabIndex = 4
        Me._cmdMove_3.Text = "<<"
        Me._cmdMove_3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdMove_3.UseVisualStyleBackColor = False
        '
        'cboRskType
        '
        Me.cboRskType.BackColor = System.Drawing.SystemColors.Window
        Me.cboRskType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRskType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRskType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRskType.Location = New System.Drawing.Point(128, 16)
        Me.cboRskType.Name = "cboRskType"
        Me.cboRskType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRskType.Size = New System.Drawing.Size(217, 21)
        Me.cboRskType.TabIndex = 0
        '
        'chkInfoCheckList
        '
        Me.chkInfoCheckList.BackColor = System.Drawing.SystemColors.Control
        Me.chkInfoCheckList.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkInfoCheckList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInfoCheckList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkInfoCheckList.Location = New System.Drawing.Point(32, 52)
        Me.chkInfoCheckList.Name = "chkInfoCheckList"
        Me.chkInfoCheckList.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkInfoCheckList.Size = New System.Drawing.Size(305, 17)
        Me.chkInfoCheckList.TabIndex = 1
        Me.chkInfoCheckList.Text = "Show Information Checklist"
        Me.chkInfoCheckList.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(511, 355)
        Me.Controls.Add(Me._cmdButton_2)
        Me.Controls.Add(Me._cmdButton_1)
        Me.Controls.Add(Me._cmdButton_3)
        Me.Controls.Add(Me._cmdButton_0)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Risk Type - Information Checklist"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._SSTab1_TabPage0.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub InitializelstInfoChkLst()
		Me.lstInfoChkLst(0) = _lstInfoChkLst_0
		Me.lstInfoChkLst(1) = _lstInfoChkLst_1
	End Sub
	Sub InitializecmdMove()
		Me.cmdMove(0) = _cmdMove_0
		Me.cmdMove(1) = _cmdMove_1
		Me.cmdMove(2) = _cmdMove_2
		Me.cmdMove(3) = _cmdMove_3
	End Sub
	Sub InitializecmdButton()
		Me.cmdButton(2) = _cmdButton_2
		Me.cmdButton(1) = _cmdButton_1
		Me.cmdButton(3) = _cmdButton_3
		Me.cmdButton(0) = _cmdButton_0
	End Sub
	Sub InitializeLabel1()
		Me.Label1(0) = _Label1_0
		Me.Label1(1) = _Label1_1
	End Sub
#End Region 
End Class