<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRiskGroup
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
	Public WithEvents cboRiskGroup As System.Windows.Forms.ComboBox
	Public WithEvents Cancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRiskGroup))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cboRiskGroup = New System.Windows.Forms.ComboBox
		Me.Cancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.Label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cboRiskGroup
		' 
		Me.cboRiskGroup.BackColor = System.Drawing.SystemColors.Window
		Me.cboRiskGroup.CausesValidation = True
		Me.cboRiskGroup.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboRiskGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown
		Me.cboRiskGroup.Enabled = True
		Me.cboRiskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboRiskGroup.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboRiskGroup.IntegralHeight = True
		Me.cboRiskGroup.Location = New System.Drawing.Point(96, 24)
		Me.cboRiskGroup.Name = "cboRiskGroup"
		Me.cboRiskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboRiskGroup.Size = New System.Drawing.Size(241, 21)
		Me.cboRiskGroup.Sorted = False
		Me.cboRiskGroup.TabIndex = 2
		Me.cboRiskGroup.TabStop = True
		Me.cboRiskGroup.Text = " "
		Me.cboRiskGroup.Visible = True
		' 
		' Cancel
		' 
		Me.Cancel.BackColor = System.Drawing.SystemColors.Control
		Me.Cancel.CausesValidation = True
		Me.Cancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.Cancel.Enabled = True
		Me.Cancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Cancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Cancel.Location = New System.Drawing.Point(264, 64)
		Me.Cancel.Name = "Cancel"
		Me.Cancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Cancel.Size = New System.Drawing.Size(73, 22)
		Me.Cancel.TabIndex = 1
		Me.Cancel.TabStop = True
		Me.Cancel.Text = "&Cancel"
		Me.Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(184, 64)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Label1
		' 
		Me.Label1.AutoSize = True
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(16, 28)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(68, 13)
		Me.Label1.TabIndex = 3
		Me.Label1.Text = "Risk Group:"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmRiskGroup
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(345, 97)
		Me.ControlBox = True
		Me.Controls.Add(Me.cboRiskGroup)
		Me.Controls.Add(Me.Cancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmRiskGroup"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Risk Groups"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class