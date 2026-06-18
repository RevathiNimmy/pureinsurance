<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmView
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
	Private WithEvents _lvwData_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwData_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwData_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwData_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwData_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwData As System.Windows.Forms.ListView
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmView))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.lvwData = New System.Windows.Forms.ListView
		Me._lvwData_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwData_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwData_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwData_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwData_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me.cmdOK = New System.Windows.Forms.Button
		Me.Frame1.SuspendLayout()
		Me.lvwData.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.lvwData)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 8)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(465, 337)
		Me.Frame1.TabIndex = 1
		Me.Frame1.Visible = True
		' 
		' lvwData
		' 
		Me.lvwData.BackColor = System.Drawing.SystemColors.Window
		Me.lvwData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwData.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwData.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwData.GridLines = True
		Me.lvwData.HideSelection = True
		Me.lvwData.LabelEdit = True
		Me.lvwData.LabelWrap = True
		Me.lvwData.Location = New System.Drawing.Point(8, 16)
		Me.lvwData.Name = "lvwData"
		Me.lvwData.Size = New System.Drawing.Size(449, 313)
		Me.lvwData.TabIndex = 2
		Me.lvwData.View = System.Windows.Forms.View.Details
		Me.lvwData.Columns.Add(Me._lvwData_ColumnHeader_1)
		Me.lvwData.Columns.Add(Me._lvwData_ColumnHeader_2)
		Me.lvwData.Columns.Add(Me._lvwData_ColumnHeader_3)
		Me.lvwData.Columns.Add(Me._lvwData_ColumnHeader_4)
		Me.lvwData.Columns.Add(Me._lvwData_ColumnHeader_5)
		' 
		' _lvwData_ColumnHeader_1
		' 
		Me._lvwData_ColumnHeader_1.Text = "Make"
		Me._lvwData_ColumnHeader_1.Width = 97
		' 
		' _lvwData_ColumnHeader_2
		' 
		Me._lvwData_ColumnHeader_2.Text = "Model"
		Me._lvwData_ColumnHeader_2.Width = 97
		' 
		' _lvwData_ColumnHeader_3
		' 
		Me._lvwData_ColumnHeader_3.Text = "Capacity"
		Me._lvwData_ColumnHeader_3.Width = 97
		' 
		' _lvwData_ColumnHeader_4
		' 
		Me._lvwData_ColumnHeader_4.Text = "ABICode"
		Me._lvwData_ColumnHeader_4.Width = 97
		' 
		' _lvwData_ColumnHeader_5
		' 
		Me._lvwData_ColumnHeader_5.Width = 97
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(408, 352)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(65, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmView
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(481, 380)
		Me.ControlBox = True
		Me.Controls.Add(Me.Frame1)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmView.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmView"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "View Data"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Frame1.ResumeLayout(False)
		Me.lvwData.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class