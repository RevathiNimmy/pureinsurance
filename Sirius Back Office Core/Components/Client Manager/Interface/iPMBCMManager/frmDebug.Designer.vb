<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDebug
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwCMs_InitializeColumnKeys()
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
	Public WithEvents tmrRefresh As System.Windows.Forms.Timer
	Private WithEvents _lvwCMs_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCMs_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwCMs_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwCMs As System.Windows.Forms.ListView
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDebug))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.tmrRefresh = New System.Windows.Forms.Timer(components)
		Me.lvwCMs = New System.Windows.Forms.ListView
		Me._lvwCMs_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwCMs_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwCMs_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me.Label1 = New System.Windows.Forms.Label
		Me.lvwCMs.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' tmrRefresh
		' 
		Me.tmrRefresh.Enabled = True
		Me.tmrRefresh.Interval = 2000
		' 
		' lvwCMs
		' 
		Me.lvwCMs.BackColor = System.Drawing.SystemColors.Window
		Me.lvwCMs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwCMs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwCMs.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwCMs.HideSelection = True
		Me.lvwCMs.LabelEdit = True
		Me.lvwCMs.LabelWrap = True
		Me.lvwCMs.Location = New System.Drawing.Point(0, 0)
		Me.lvwCMs.Name = "lvwCMs"
		Me.lvwCMs.Size = New System.Drawing.Size(369, 185)
		Me.lvwCMs.TabIndex = 0
		Me.lvwCMs.View = System.Windows.Forms.View.Details
		Me.lvwCMs.Columns.Add(Me._lvwCMs_ColumnHeader_1)
		Me.lvwCMs.Columns.Add(Me._lvwCMs_ColumnHeader_2)
		Me.lvwCMs.Columns.Add(Me._lvwCMs_ColumnHeader_3)
		' 
		' _lvwCMs_ColumnHeader_1
		' 
		Me._lvwCMs_ColumnHeader_1.Tag = ""
		Me._lvwCMs_ColumnHeader_1.Text = "Index"
		Me._lvwCMs_ColumnHeader_1.Width = 97
		' 
		' _lvwCMs_ColumnHeader_2
		' 
		Me._lvwCMs_ColumnHeader_2.Tag = ""
		Me._lvwCMs_ColumnHeader_2.Text = "Party Count"
		Me._lvwCMs_ColumnHeader_2.Width = 97
		' 
		' _lvwCMs_ColumnHeader_3
		' 
		Me._lvwCMs_ColumnHeader_3.Tag = ""
		Me._lvwCMs_ColumnHeader_3.Text = "Status"
		Me._lvwCMs_ColumnHeader_3.Width = 97
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(0, 184)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(369, 17)
		Me.Label1.TabIndex = 1
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmDebug
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(370, 202)
		Me.ControlBox = True
		Me.Controls.Add(Me.lvwCMs)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 19)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDebug"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Debug Window"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwCMs.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub lvwCMs_InitializeColumnKeys()
		Me._lvwCMs_ColumnHeader_1.Name = ""
		Me._lvwCMs_ColumnHeader_2.Name = ""
		Me._lvwCMs_ColumnHeader_3.Name = ""
	End Sub
#End Region 
End Class