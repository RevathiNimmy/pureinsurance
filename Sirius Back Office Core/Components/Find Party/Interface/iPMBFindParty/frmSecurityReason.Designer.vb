<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSecurityReason
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwReason_InitializeColumnKeys()
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
	Private WithEvents _lvwReason_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReason_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwReason_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwReason As System.Windows.Forms.ListView
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSecurityReason))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.lvwReason = New System.Windows.Forms.ListView
		Me._lvwReason_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwReason_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwReason_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me.Label1 = New System.Windows.Forms.Label
		Me.lvwReason.SuspendLayout()
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
		Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(120, 190)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 1
		Me.cmdOK.TabStop = False
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwReason
		' 
		Me.lvwReason.BackColor = System.Drawing.SystemColors.Window
		Me.lvwReason.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwReason.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwReason.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwReason.HideSelection = False
		Me.lvwReason.LabelEdit = False
		Me.lvwReason.LabelWrap = True
		Me.lvwReason.Location = New System.Drawing.Point(8, 22)
		Me.lvwReason.Name = "lvwReason"
		Me.lvwReason.Size = New System.Drawing.Size(293, 161)
		Me.lvwReason.TabIndex = 0
		Me.lvwReason.View = System.Windows.Forms.View.Details
		Me.lvwReason.Columns.Add(Me._lvwReason_ColumnHeader_1)
		Me.lvwReason.Columns.Add(Me._lvwReason_ColumnHeader_2)
		Me.lvwReason.Columns.Add(Me._lvwReason_ColumnHeader_3)
		' 
		' _lvwReason_ColumnHeader_1
		' 
		Me._lvwReason_ColumnHeader_1.Tag = ""
		Me._lvwReason_ColumnHeader_1.Text = "Reason for viewing"
		Me._lvwReason_ColumnHeader_1.Width = 97
		' 
		' _lvwReason_ColumnHeader_2
		' 
		Me._lvwReason_ColumnHeader_2.Tag = ""
		Me._lvwReason_ColumnHeader_2.Text = "is_logged"
		Me._lvwReason_ColumnHeader_2.Width = 0
		' 
		' _lvwReason_ColumnHeader_3
		' 
		Me._lvwReason_ColumnHeader_3.Tag = ""
		Me._lvwReason_ColumnHeader_3.Text = "is_question"
		Me._lvwReason_ColumnHeader_3.Width = 0
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(10, 4)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(291, 19)
		Me.Label1.TabIndex = 2
		Me.Label1.Text = "Reason"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmSecurityReason
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(310, 218)
		Me.ControlBox = False
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.lvwReason)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmSecurityReason"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Reason for accessing this Client"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwReason, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwReason.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwReason_InitializeColumnKeys()
		Me._lvwReason_ColumnHeader_1.Name = ""
		Me._lvwReason_ColumnHeader_2.Name = ""
		Me._lvwReason_ColumnHeader_3.Name = ""
	End Sub
#End Region 
End Class