<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectChildScreen
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
	Public WithEvents cmdListCancel As System.Windows.Forms.Button
	Public WithEvents cmdListViewAdd As System.Windows.Forms.Button
	Public WithEvents cmdListViewEdit As System.Windows.Forms.Button
	Public WithEvents ListViewSelectScreen As System.Windows.Forms.ListView
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectChildScreen))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdListCancel = New System.Windows.Forms.Button
		Me.cmdListViewAdd = New System.Windows.Forms.Button
		Me.cmdListViewEdit = New System.Windows.Forms.Button
		Me.ListViewSelectScreen = New System.Windows.Forms.ListView
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdListCancel
		' 
		Me.cmdListCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdListCancel.CausesValidation = True
		Me.cmdListCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdListCancel.Enabled = True
		Me.cmdListCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdListCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdListCancel.Location = New System.Drawing.Point(177, 182)
		Me.cmdListCancel.Name = "cmdListCancel"
		Me.cmdListCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdListCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdListCancel.TabIndex = 3
		Me.cmdListCancel.TabStop = True
		Me.cmdListCancel.Text = "Cancel"
		Me.cmdListCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdListCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdListViewAdd
		' 
		Me.cmdListViewAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdListViewAdd.CausesValidation = True
		Me.cmdListViewAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdListViewAdd.Enabled = True
		Me.cmdListViewAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdListViewAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdListViewAdd.Location = New System.Drawing.Point(22, 182)
		Me.cmdListViewAdd.Name = "cmdListViewAdd"
		Me.cmdListViewAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdListViewAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdListViewAdd.TabIndex = 2
		Me.cmdListViewAdd.TabStop = True
		Me.cmdListViewAdd.Text = "Add"
		Me.cmdListViewAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdListViewAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdListViewEdit
		' 
		Me.cmdListViewEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdListViewEdit.CausesValidation = True
		Me.cmdListViewEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdListViewEdit.Enabled = True
		Me.cmdListViewEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdListViewEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdListViewEdit.Location = New System.Drawing.Point(100, 182)
		Me.cmdListViewEdit.Name = "cmdListViewEdit"
		Me.cmdListViewEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdListViewEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdListViewEdit.TabIndex = 1
		Me.cmdListViewEdit.TabStop = True
		Me.cmdListViewEdit.Text = "Edit"
		Me.cmdListViewEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdListViewEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' ListViewSelectScreen
		' 
		Me.ListViewSelectScreen.BackColor = System.Drawing.SystemColors.Window
		Me.ListViewSelectScreen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.ListViewSelectScreen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ListViewSelectScreen.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListViewSelectScreen.HideSelection = True
		Me.ListViewSelectScreen.LabelEdit = False
		Me.ListViewSelectScreen.LabelWrap = True
		Me.ListViewSelectScreen.Location = New System.Drawing.Point(19, 11)
		Me.ListViewSelectScreen.Name = "ListViewSelectScreen"
		Me.ListViewSelectScreen.Size = New System.Drawing.Size(407, 161)
		Me.ListViewSelectScreen.TabIndex = 0
		Me.ListViewSelectScreen.View = System.Windows.Forms.View.Details
		' 
		' frmSelectChildScreen
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(450, 213)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdListCancel)
		Me.Controls.Add(Me.cmdListViewAdd)
		Me.Controls.Add(Me.cmdListViewEdit)
		Me.Controls.Add(Me.ListViewSelectScreen)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmSelectChildScreen"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Form1"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listViewHelper1.SetSorted(Me.ListViewSelectScreen, True)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class