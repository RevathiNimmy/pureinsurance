<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUsers
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwUsers_InitializeColumnKeys()
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _lvwUsers_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwUsers As System.Windows.Forms.ListView
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUsers))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.lvwUsers = New System.Windows.Forms.ListView
		Me._lvwUsers_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me.lvwUsers.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(152, 192)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(81, 22)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwUsers
		' 
		Me.lvwUsers.BackColor = System.Drawing.SystemColors.Window
		Me.lvwUsers.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lvwUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwUsers.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwUsers.HideSelection = False
		Me.lvwUsers.LabelEdit = False
		Me.lvwUsers.LabelWrap = True
		Me.lvwUsers.Location = New System.Drawing.Point(8, 8)
		Me.lvwUsers.MultiSelect = True
		Me.lvwUsers.Name = "lvwUsers"
		Me.lvwUsers.Size = New System.Drawing.Size(222, 177)
		Me.lvwUsers.TabIndex = 0
		Me.lvwUsers.View = System.Windows.Forms.View.Details
		Me.lvwUsers.Columns.Add(Me._lvwUsers_ColumnHeader_1)
		' 
		' _lvwUsers_ColumnHeader_1
		' 
		Me._lvwUsers_ColumnHeader_1.Tag = ""
		Me._lvwUsers_ColumnHeader_1.Text = " Users"
		Me._lvwUsers_ColumnHeader_1.Width = 223
		' 
		' frmUsers
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(239, 221)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.lvwUsers)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmUsers"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Select Users"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwUsers.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwUsers_InitializeColumnKeys()
		Me._lvwUsers_ColumnHeader_1.Name = ""
	End Sub
#End Region 
End Class