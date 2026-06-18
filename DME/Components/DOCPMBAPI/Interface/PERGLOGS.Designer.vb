<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPurgeLogs
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmbPurgeTo As System.Windows.Forms.ComboBox
	Public WithEvents pan3PergeTo As System.Windows.Forms.Panel
	Public WithEvents cmbPurgeFrom As System.Windows.Forms.ComboBox
	Public WithEvents pan3PergeFrom As System.Windows.Forms.Panel
	Public WithEvents lstPurgeToSorted As System.Windows.Forms.ListBox
	Public WithEvents lstPurgeFromSorted As System.Windows.Forms.ListBox
	Public WithEvents lblPergeTo As System.Windows.Forms.Label
	Public WithEvents lblPergeFrom As System.Windows.Forms.Label
	Public WithEvents fra3PergeLogs As System.Windows.Forms.GroupBox
	Public WithEvents pan3PergeLogs As System.Windows.Forms.Panel
	Private WithEvents listBoxHelper1 As Artinsoft.VB6.Gui.ListBoxHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPurgeLogs))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.pan3PergeLogs = New System.Windows.Forms.Panel
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.pan3PergeTo = New System.Windows.Forms.Panel
		Me.cmbPurgeTo = New System.Windows.Forms.ComboBox
		Me.pan3PergeFrom = New System.Windows.Forms.Panel
		Me.cmbPurgeFrom = New System.Windows.Forms.ComboBox
		Me.fra3PergeLogs = New System.Windows.Forms.GroupBox
		Me.lstPurgeToSorted = New System.Windows.Forms.ListBox
		Me.lstPurgeFromSorted = New System.Windows.Forms.ListBox
		Me.lblPergeTo = New System.Windows.Forms.Label
		Me.lblPergeFrom = New System.Windows.Forms.Label
		Me.pan3PergeLogs.SuspendLayout()
		Me.pan3PergeTo.SuspendLayout()
		Me.pan3PergeFrom.SuspendLayout()
		Me.fra3PergeLogs.SuspendLayout()
		Me.SuspendLayout()
		Me.listBoxHelper1 = New Artinsoft.VB6.Gui.ListBoxHelper(Me.components)
		' 
		' pan3PergeLogs
		' 
		Me.pan3PergeLogs.BackColor = System.Drawing.SystemColors.Control
		Me.pan3PergeLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3PergeLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.pan3PergeLogs.Controls.Add(Me.cmdCancel)
		Me.pan3PergeLogs.Controls.Add(Me.cmdOK)
		Me.pan3PergeLogs.Controls.Add(Me.pan3PergeTo)
		Me.pan3PergeLogs.Controls.Add(Me.pan3PergeFrom)
		Me.pan3PergeLogs.Controls.Add(Me.fra3PergeLogs)
		Me.pan3PergeLogs.Dock = System.Windows.Forms.DockStyle.Top
		Me.pan3PergeLogs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3PergeLogs.Location = New System.Drawing.Point(0, 0)
		Me.pan3PergeLogs.Name = "pan3PergeLogs"
		Me.pan3PergeLogs.Size = New System.Drawing.Size(276, 157)
		Me.pan3PergeLogs.TabIndex = 0
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(196, 116)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(65, 25)
		Me.cmdCancel.TabIndex = 6
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
		Me.cmdOK.Location = New System.Drawing.Point(122, 116)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(65, 25)
		Me.cmdOK.TabIndex = 5
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' pan3PergeTo
		' 
		Me.pan3PergeTo.BackColor = System.Drawing.SystemColors.Control
		Me.pan3PergeTo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.pan3PergeTo.Controls.Add(Me.cmbPurgeTo)
		Me.pan3PergeTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3PergeTo.Location = New System.Drawing.Point(112, 68)
		Me.pan3PergeTo.Name = "pan3PergeTo"
		Me.pan3PergeTo.Size = New System.Drawing.Size(137, 26)
		Me.pan3PergeTo.TabIndex = 3
		' 
		' cmbPurgeTo
		' 
		Me.cmbPurgeTo.BackColor = System.Drawing.SystemColors.Window
		Me.cmbPurgeTo.CausesValidation = True
		Me.cmbPurgeTo.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbPurgeTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbPurgeTo.Enabled = True
		Me.cmbPurgeTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbPurgeTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbPurgeTo.IntegralHeight = True
		Me.cmbPurgeTo.Location = New System.Drawing.Point(3, 3)
		Me.cmbPurgeTo.Name = "cmbPurgeTo"
		Me.cmbPurgeTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbPurgeTo.Size = New System.Drawing.Size(131, 20)
		Me.cmbPurgeTo.Sorted = False
		Me.cmbPurgeTo.TabIndex = 9
		Me.cmbPurgeTo.TabStop = True
		Me.cmbPurgeTo.Visible = True
		' 
		' pan3PergeFrom
		' 
		Me.pan3PergeFrom.BackColor = System.Drawing.SystemColors.Control
		Me.pan3PergeFrom.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.pan3PergeFrom.Controls.Add(Me.cmbPurgeFrom)
		Me.pan3PergeFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.pan3PergeFrom.Location = New System.Drawing.Point(112, 36)
		Me.pan3PergeFrom.Name = "pan3PergeFrom"
		Me.pan3PergeFrom.Size = New System.Drawing.Size(137, 26)
		Me.pan3PergeFrom.TabIndex = 1
		' 
		' cmbPurgeFrom
		' 
		Me.cmbPurgeFrom.BackColor = System.Drawing.SystemColors.Window
		Me.cmbPurgeFrom.CausesValidation = True
		Me.cmbPurgeFrom.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbPurgeFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbPurgeFrom.Enabled = True
		Me.cmbPurgeFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbPurgeFrom.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbPurgeFrom.IntegralHeight = True
		Me.cmbPurgeFrom.Location = New System.Drawing.Point(3, 3)
		Me.cmbPurgeFrom.Name = "cmbPurgeFrom"
		Me.cmbPurgeFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbPurgeFrom.Size = New System.Drawing.Size(131, 20)
		Me.cmbPurgeFrom.Sorted = False
		Me.cmbPurgeFrom.TabIndex = 8
		Me.cmbPurgeFrom.TabStop = True
		Me.cmbPurgeFrom.Visible = True
		' 
		' fra3PergeLogs
		' 
		Me.fra3PergeLogs.Controls.Add(Me.lstPurgeToSorted)
		Me.fra3PergeLogs.Controls.Add(Me.lstPurgeFromSorted)
		Me.fra3PergeLogs.Controls.Add(Me.lblPergeTo)
		Me.fra3PergeLogs.Controls.Add(Me.lblPergeFrom)
		Me.fra3PergeLogs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        'Me.fra3PergeLogs.Font3D = Threed.enumFont3DConstants._InsetLight
		Me.fra3PergeLogs.Location = New System.Drawing.Point(16, 12)
		Me.fra3PergeLogs.Name = "fra3PergeLogs"
		Me.fra3PergeLogs.Size = New System.Drawing.Size(245, 93)
		Me.fra3PergeLogs.TabIndex = 7
		Me.fra3PergeLogs.Text = "Select Log Dates To Purge"
		' 
		' lstPurgeToSorted
		' 
		Me.lstPurgeToSorted.BackColor = System.Drawing.SystemColors.Window
		Me.lstPurgeToSorted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lstPurgeToSorted.CausesValidation = True
		Me.lstPurgeToSorted.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstPurgeToSorted.Enabled = True
		Me.lstPurgeToSorted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstPurgeToSorted.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstPurgeToSorted.IntegralHeight = True
		Me.lstPurgeToSorted.Location = New System.Drawing.Point(136, 84)
		Me.lstPurgeToSorted.MultiColumn = False
		Me.lstPurgeToSorted.Name = "lstPurgeToSorted"
		Me.lstPurgeToSorted.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstPurgeToSorted.Size = New System.Drawing.Size(97, 18)
		Me.lstPurgeToSorted.Sorted = True
		Me.lstPurgeToSorted.TabIndex = 11
		Me.lstPurgeToSorted.TabStop = True
		Me.lstPurgeToSorted.Visible = False
		' 
		' lstPurgeFromSorted
		' 
		Me.lstPurgeFromSorted.BackColor = System.Drawing.SystemColors.Window
		Me.lstPurgeFromSorted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.lstPurgeFromSorted.CausesValidation = True
		Me.lstPurgeFromSorted.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstPurgeFromSorted.Enabled = True
		Me.lstPurgeFromSorted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lstPurgeFromSorted.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstPurgeFromSorted.IntegralHeight = True
		Me.lstPurgeFromSorted.Location = New System.Drawing.Point(144, 8)
		Me.lstPurgeFromSorted.MultiColumn = False
		Me.lstPurgeFromSorted.Name = "lstPurgeFromSorted"
		Me.lstPurgeFromSorted.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstPurgeFromSorted.Size = New System.Drawing.Size(97, 18)
		Me.lstPurgeFromSorted.Sorted = True
		Me.lstPurgeFromSorted.TabIndex = 10
		Me.lstPurgeFromSorted.TabStop = True
		Me.lstPurgeFromSorted.Visible = False
		' 
		' lblPergeTo
		' 
		Me.lblPergeTo.AutoSize = False
		Me.lblPergeTo.BackColor = System.Drawing.Color.Transparent
		Me.lblPergeTo.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPergeTo.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPergeTo.Enabled = True
		Me.lblPergeTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPergeTo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lblPergeTo.Location = New System.Drawing.Point(16, 60)
		Me.lblPergeTo.Name = "lblPergeTo"
		Me.lblPergeTo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPergeTo.Size = New System.Drawing.Size(65, 17)
		Me.lblPergeTo.TabIndex = 2
		Me.lblPergeTo.Text = "Purge To"
		Me.lblPergeTo.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPergeTo.UseMnemonic = True
		Me.lblPergeTo.Visible = True
		' 
		' lblPergeFrom
		' 
		Me.lblPergeFrom.AutoSize = False
		Me.lblPergeFrom.BackColor = System.Drawing.Color.Transparent
		Me.lblPergeFrom.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPergeFrom.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPergeFrom.Enabled = True
		Me.lblPergeFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPergeFrom.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lblPergeFrom.Location = New System.Drawing.Point(16, 28)
		Me.lblPergeFrom.Name = "lblPergeFrom"
		Me.lblPergeFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPergeFrom.Size = New System.Drawing.Size(81, 17)
		Me.lblPergeFrom.TabIndex = 4
		Me.lblPergeFrom.Text = "Purge From"
		Me.lblPergeFrom.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPergeFrom.UseMnemonic = True
		Me.lblPergeFrom.Visible = True
		' 
		' frmPurgeLogs
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
		Me.BackColor = System.Drawing.SystemColors.Window
		Me.ClientSize = New System.Drawing.Size(276, 157)
		Me.ControlBox = False
		Me.Controls.Add(Me.pan3PergeLogs)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.ForeColor = System.Drawing.SystemColors.WindowText
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(125, 190)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmPurgeLogs"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Purge Log Files"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxHelper1.SetSelectionMode(Me.lstPurgeToSorted, System.Windows.Forms.SelectionMode.One)
		Me.listBoxHelper1.SetSelectionMode(Me.lstPurgeFromSorted, System.Windows.Forms.SelectionMode.One)
		Me.pan3PergeLogs.ResumeLayout(False)
		Me.pan3PergeTo.ResumeLayout(False)
		Me.pan3PergeFrom.ResumeLayout(False)
		Me.fra3PergeLogs.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class