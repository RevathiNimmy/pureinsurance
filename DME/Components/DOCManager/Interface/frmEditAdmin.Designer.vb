<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditAdmin
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
	Public WithEvents cboFileDelete As System.Windows.Forms.ComboBox
	Public WithEvents cboFileCopy As System.Windows.Forms.ComboBox
	Public WithEvents cboFileMove As System.Windows.Forms.ComboBox
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents cboFolderDelete As System.Windows.Forms.ComboBox
	Public WithEvents cboFolderCopy As System.Windows.Forms.ComboBox
	Public WithEvents cboFolderMove As System.Windows.Forms.ComboBox
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEditAdmin))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Frame2 = New System.Windows.Forms.GroupBox
		Me.cboFileDelete = New System.Windows.Forms.ComboBox
		Me.cboFileCopy = New System.Windows.Forms.ComboBox
		Me.cboFileMove = New System.Windows.Forms.ComboBox
		Me.Label6 = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.cboFolderDelete = New System.Windows.Forms.ComboBox
		Me.cboFolderCopy = New System.Windows.Forms.ComboBox
		Me.cboFolderMove = New System.Windows.Forms.ComboBox
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.Frame2.SuspendLayout()
		Me.Frame1.SuspendLayout()
		Me.SuspendLayout()
		Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' Frame2
		' 
		Me.Frame2.BackColor = System.Drawing.SystemColors.Control
		Me.Frame2.Controls.Add(Me.cboFileDelete)
		Me.Frame2.Controls.Add(Me.cboFileCopy)
		Me.Frame2.Controls.Add(Me.cboFileMove)
		Me.Frame2.Controls.Add(Me.Label6)
		Me.Frame2.Controls.Add(Me.Label5)
		Me.Frame2.Controls.Add(Me.Label4)
		Me.Frame2.Enabled = True
		Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame2.Location = New System.Drawing.Point(160, 8)
		Me.Frame2.Name = "Frame2"
		Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame2.Size = New System.Drawing.Size(151, 105)
		Me.Frame2.TabIndex = 9
		Me.Frame2.Text = "File Access Levels"
		Me.Frame2.Visible = True
		' 
		' cboFileDelete
		' 
		Me.cboFileDelete.BackColor = System.Drawing.SystemColors.Window
		Me.cboFileDelete.CausesValidation = True
		Me.cboFileDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboFileDelete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboFileDelete.Enabled = True
		Me.cboFileDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboFileDelete.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboFileDelete.IntegralHeight = True
		Me.cboFileDelete.Location = New System.Drawing.Point(80, 72)
		Me.cboFileDelete.Name = "cboFileDelete"
		Me.cboFileDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboFileDelete.Size = New System.Drawing.Size(57, 21)
		Me.cboFileDelete.Sorted = False
		Me.cboFileDelete.TabIndex = 5
		Me.cboFileDelete.TabStop = True
		Me.cboFileDelete.Visible = True
		Me.cboFileDelete.Items.AddRange(New Object(){"1", "2", "3", "4", "5", "6", "7", "8", "9"})
		' 
		' cboFileCopy
		' 
		Me.cboFileCopy.BackColor = System.Drawing.SystemColors.Window
		Me.cboFileCopy.CausesValidation = True
		Me.cboFileCopy.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboFileCopy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboFileCopy.Enabled = True
		Me.cboFileCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboFileCopy.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboFileCopy.IntegralHeight = True
		Me.cboFileCopy.Location = New System.Drawing.Point(80, 48)
		Me.cboFileCopy.Name = "cboFileCopy"
		Me.cboFileCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboFileCopy.Size = New System.Drawing.Size(57, 21)
		Me.cboFileCopy.Sorted = False
		Me.cboFileCopy.TabIndex = 4
		Me.cboFileCopy.TabStop = True
		Me.cboFileCopy.Visible = True
		Me.cboFileCopy.Items.AddRange(New Object(){"1", "2", "3", "4", "5", "6", "7", "8", "9"})
		' 
		' cboFileMove
		' 
		Me.cboFileMove.BackColor = System.Drawing.SystemColors.Window
		Me.cboFileMove.CausesValidation = True
		Me.cboFileMove.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboFileMove.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboFileMove.Enabled = True
		Me.cboFileMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboFileMove.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboFileMove.IntegralHeight = True
		Me.cboFileMove.Location = New System.Drawing.Point(80, 24)
		Me.cboFileMove.Name = "cboFileMove"
		Me.cboFileMove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboFileMove.Size = New System.Drawing.Size(57, 21)
		Me.cboFileMove.Sorted = False
		Me.cboFileMove.TabIndex = 3
		Me.cboFileMove.TabStop = True
		Me.cboFileMove.Visible = True
		Me.cboFileMove.Items.AddRange(New Object(){"1", "2", "3", "4", "5", "6", "7", "8", "9"})
		' 
		' Label6
		' 
		Me.Label6.AutoSize = False
		Me.Label6.BackColor = System.Drawing.SystemColors.Control
		Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label6.Enabled = True
		Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label6.Location = New System.Drawing.Point(8, 80)
		Me.Label6.Name = "Label6"
		Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label6.Size = New System.Drawing.Size(68, 16)
		Me.Label6.TabIndex = 15
		Me.Label6.Text = "Delete"
		Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label6.UseMnemonic = True
		Me.Label6.Visible = True
		' 
		' Label5
		' 
		Me.Label5.AutoSize = False
		Me.Label5.BackColor = System.Drawing.SystemColors.Control
		Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label5.Enabled = True
		Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label5.Location = New System.Drawing.Point(8, 56)
		Me.Label5.Name = "Label5"
		Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label5.Size = New System.Drawing.Size(68, 16)
		Me.Label5.TabIndex = 14
		Me.Label5.Text = "Copy"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label5.UseMnemonic = True
		Me.Label5.Visible = True
		' 
		' Label4
		' 
		Me.Label4.AutoSize = False
		Me.Label4.BackColor = System.Drawing.SystemColors.Control
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.Enabled = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label4.Location = New System.Drawing.Point(8, 32)
		Me.Label4.Name = "Label4"
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.Size = New System.Drawing.Size(68, 16)
		Me.Label4.TabIndex = 13
		Me.Label4.Text = "Move"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.cboFolderDelete)
		Me.Frame1.Controls.Add(Me.cboFolderCopy)
		Me.Frame1.Controls.Add(Me.cboFolderMove)
		Me.Frame1.Controls.Add(Me.Label3)
		Me.Frame1.Controls.Add(Me.Label2)
		Me.Frame1.Controls.Add(Me.Label1)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 8)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(141, 105)
		Me.Frame1.TabIndex = 8
		Me.Frame1.Text = "Folder Access Levels"
		Me.Frame1.Visible = True
		' 
		' cboFolderDelete
		' 
		Me.cboFolderDelete.BackColor = System.Drawing.SystemColors.Window
		Me.cboFolderDelete.CausesValidation = True
		Me.cboFolderDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboFolderDelete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboFolderDelete.Enabled = True
		Me.cboFolderDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboFolderDelete.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboFolderDelete.IntegralHeight = True
		Me.cboFolderDelete.Location = New System.Drawing.Point(64, 72)
		Me.cboFolderDelete.Name = "cboFolderDelete"
		Me.cboFolderDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboFolderDelete.Size = New System.Drawing.Size(57, 21)
		Me.cboFolderDelete.Sorted = False
		Me.cboFolderDelete.TabIndex = 2
		Me.cboFolderDelete.TabStop = True
		Me.cboFolderDelete.Visible = True
		Me.cboFolderDelete.Items.AddRange(New Object(){"1", "2", "3", "4", "5", "6", "7", "8", "9"})
		' 
		' cboFolderCopy
		' 
		Me.cboFolderCopy.BackColor = System.Drawing.SystemColors.Window
		Me.cboFolderCopy.CausesValidation = True
		Me.cboFolderCopy.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboFolderCopy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboFolderCopy.Enabled = True
		Me.cboFolderCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboFolderCopy.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboFolderCopy.IntegralHeight = True
		Me.cboFolderCopy.Location = New System.Drawing.Point(64, 48)
		Me.cboFolderCopy.Name = "cboFolderCopy"
		Me.cboFolderCopy.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboFolderCopy.Size = New System.Drawing.Size(57, 21)
		Me.cboFolderCopy.Sorted = False
		Me.cboFolderCopy.TabIndex = 1
		Me.cboFolderCopy.TabStop = True
		Me.cboFolderCopy.Visible = True
		Me.cboFolderCopy.Items.AddRange(New Object(){"1", "2", "3", "4", "5", "6", "7", "8", "9"})
		' 
		' cboFolderMove
		' 
		Me.cboFolderMove.BackColor = System.Drawing.SystemColors.Window
		Me.cboFolderMove.CausesValidation = True
		Me.cboFolderMove.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboFolderMove.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboFolderMove.Enabled = True
		Me.cboFolderMove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboFolderMove.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboFolderMove.IntegralHeight = True
		Me.cboFolderMove.Location = New System.Drawing.Point(64, 24)
		Me.cboFolderMove.Name = "cboFolderMove"
		Me.cboFolderMove.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboFolderMove.Size = New System.Drawing.Size(57, 21)
		Me.cboFolderMove.Sorted = False
		Me.cboFolderMove.TabIndex = 0
		Me.cboFolderMove.TabStop = True
		Me.cboFolderMove.Visible = True
		Me.cboFolderMove.Items.AddRange(New Object(){"1", "2", "3", "4", "5", "6", "7", "8", "9"})
		' 
		' Label3
		' 
		Me.Label3.AutoSize = False
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Location = New System.Drawing.Point(8, 80)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(52, 16)
		Me.Label3.TabIndex = 12
		Me.Label3.Text = "Delete"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		' 
		' Label2
		' 
		Me.Label2.AutoSize = False
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.Enabled = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(8, 56)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(52, 16)
		Me.Label2.TabIndex = 11
		Me.Label2.Text = "Copy"
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
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
		Me.Label1.Location = New System.Drawing.Point(8, 32)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(52, 16)
		Me.Label1.TabIndex = 10
		Me.Label1.Text = "Move"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(232, 128)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(77, 23)
		Me.cmdCancel.TabIndex = 7
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
		Me.cmdOK.Location = New System.Drawing.Point(144, 128)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(77, 23)
		Me.cmdOK.TabIndex = 6
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&Ok"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmEditAdmin
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(318, 158)
		Me.ControlBox = True
		Me.Controls.Add(Me.Frame2)
		Me.Controls.Add(Me.Frame1)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmEditAdmin"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Set Edit Access Levels"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboFileDelete, New Integer(){0, 0, 0, 0, 0, 0, 0, 0, 0})
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboFileCopy, New Integer(){0, 0, 0, 0, 0, 0, 0, 0, 0})
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboFileMove, New Integer(){0, 0, 0, 0, 0, 0, 0, 0, 0})
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboFolderDelete, New Integer(){0, 0, 0, 0, 0, 0, 0, 0, 0})
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboFolderCopy, New Integer(){0, 0, 0, 0, 0, 0, 0, 0, 0})
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboFolderMove, New Integer(){0, 0, 0, 0, 0, 0, 0, 0, 0})
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Frame2.ResumeLayout(False)
		Me.Frame1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class