<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmList
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
	Public WithEvents btnOK As System.Windows.Forms.Button
	Public WithEvents btnCancel As System.Windows.Forms.Button
	Public WithEvents lvwList As System.Windows.Forms.ListView
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmList))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.btnOK = New System.Windows.Forms.Button
		Me.btnCancel = New System.Windows.Forms.Button
		Me.lvwList = New System.Windows.Forms.ListView
		Me.SuspendLayout()
        'Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' btnOK
		' 
		Me.btnOK.BackColor = System.Drawing.SystemColors.Control
		Me.btnOK.CausesValidation = True
		Me.btnOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.btnOK.Enabled = True
		Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.btnOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.btnOK.Location = New System.Drawing.Point(400, 224)
		Me.btnOK.Name = "btnOK"
		Me.btnOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.btnOK.Size = New System.Drawing.Size(73, 22)
		Me.btnOK.TabIndex = 2
		Me.btnOK.TabStop = True
		Me.btnOK.Text = "&OK"
		Me.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' btnCancel
		' 
		Me.btnCancel.BackColor = System.Drawing.SystemColors.Control
		Me.btnCancel.CausesValidation = True
		Me.btnCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.btnCancel.Enabled = True
		Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.btnCancel.Location = New System.Drawing.Point(488, 224)
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.btnCancel.Size = New System.Drawing.Size(73, 22)
		Me.btnCancel.TabIndex = 1
		Me.btnCancel.TabStop = True
		Me.btnCancel.Text = "&Cancel"
		Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' lvwList
		' 
		Me.lvwList.BackColor = System.Drawing.SystemColors.Window
		Me.lvwList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwList.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwList.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwList.FullRowSelect = True
		Me.lvwList.HideSelection = True
		Me.lvwList.LabelEdit = False
		Me.lvwList.LabelWrap = True
		Me.lvwList.Location = New System.Drawing.Point(8, 8)
		Me.lvwList.Name = "lvwList"
		Me.lvwList.Size = New System.Drawing.Size(553, 209)
		Me.lvwList.TabIndex = 0
		' 
		' frmList
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(570, 251)
		Me.ControlBox = True
		Me.Controls.Add(Me.btnOK)
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.lvwList)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = False
		Me.Icon = CType(resources.GetObject("frmList.Icon"), System.Drawing.Icon)
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmList"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Found Items"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
        'Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwList, True)
        'CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class