<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializemnuWindowArrange()
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
	Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuWindowArrange_0 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuWindowArrange_1 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuWindowArrange_2 As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents _mnuWindowArrange_3 As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public mnuWindowArrange(3) As System.Windows.Forms.ToolStripMenuItem
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.MainMenu1 = New System.Windows.Forms.MenuStrip
		Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem
		Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuWindowArrange_0 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuWindowArrange_1 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuWindowArrange_2 = New System.Windows.Forms.ToolStripMenuItem
		Me._mnuWindowArrange_3 = New System.Windows.Forms.ToolStripMenuItem
		Me.SuspendLayout()
		' 
		' MainMenu1
		' 
		Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFile, Me.mnuWindow})
		' 
		' mnuFile
		' 
		Me.mnuFile.Available = True
		Me.mnuFile.Checked = False
		Me.mnuFile.Enabled = True
		Me.mnuFile.MergeAction = System.Windows.Forms.MergeAction.Remove
		Me.mnuFile.Name = "mnuFile"
		Me.mnuFile.Text = "&File"
		Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me.mnuFileExit})
		' 
		' mnuFileExit
		' 
		Me.mnuFileExit.Available = True
		Me.mnuFileExit.Checked = False
		Me.mnuFileExit.Enabled = True
		Me.mnuFileExit.Name = "mnuFileExit"
		Me.mnuFileExit.Text = "E&xit"
		' 
		' mnuWindow
		' 
		Me.mnuWindow.Available = True
		Me.mnuWindow.Checked = False
		Me.mnuWindow.Enabled = True
		Me.mnuWindow.MergeAction = System.Windows.Forms.MergeAction.Remove
		Me.mnuWindow.Name = "mnuWindow"
		Me.mnuWindow.Text = "&Window"
		Me.mnuWindow.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem(){Me._mnuWindowArrange_0, Me._mnuWindowArrange_1, Me._mnuWindowArrange_2, Me._mnuWindowArrange_3})
		' 
		' _mnuWindowArrange_0
		' 
		Me._mnuWindowArrange_0.Available = True
		Me._mnuWindowArrange_0.Checked = False
		Me._mnuWindowArrange_0.Enabled = True
		Me._mnuWindowArrange_0.Name = "_mnuWindowArrange_0"
		Me._mnuWindowArrange_0.Text = "&Cascade"
		' 
		' _mnuWindowArrange_1
		' 
		Me._mnuWindowArrange_1.Available = True
		Me._mnuWindowArrange_1.Checked = False
		Me._mnuWindowArrange_1.Enabled = True
		Me._mnuWindowArrange_1.Name = "_mnuWindowArrange_1"
		Me._mnuWindowArrange_1.Text = "Tile &Horizontally"
		' 
		' _mnuWindowArrange_2
		' 
		Me._mnuWindowArrange_2.Available = True
		Me._mnuWindowArrange_2.Checked = False
		Me._mnuWindowArrange_2.Enabled = True
		Me._mnuWindowArrange_2.Name = "_mnuWindowArrange_2"
		Me._mnuWindowArrange_2.Text = "Tile &Vertically"
		' 
		' _mnuWindowArrange_3
		' 
		Me._mnuWindowArrange_3.Available = True
		Me._mnuWindowArrange_3.Checked = False
		Me._mnuWindowArrange_3.Enabled = True
		Me._mnuWindowArrange_3.Name = "_mnuWindowArrange_3"
		Me._mnuWindowArrange_3.Text = "&Arrange Icons"
		' 
		' frmMain
		' 
		Me.BackColor = System.Drawing.SystemColors.AppWorkspace
		Me.ClientSize = New System.Drawing.Size(588, 379)
		Me.Controls.Add(MainMenu1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Icon = CType(resources.GetObject("frmMain.Icon"), System.Drawing.Icon)
		Me.IsMdiContainer = True
		Me.Location = New System.Drawing.Point(4, 42)
		Me.Name = "frmMain"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Sirius Log Viewer"
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		Me.ResumeLayout(False)
	End Sub
	Sub InitializemnuWindowArrange()
		Me.mnuWindowArrange(0) = _mnuWindowArrange_0
		Me.mnuWindowArrange(1) = _mnuWindowArrange_1
		Me.mnuWindowArrange(2) = _mnuWindowArrange_2
		Me.mnuWindowArrange(3) = _mnuWindowArrange_3
	End Sub
#End Region 
End Class