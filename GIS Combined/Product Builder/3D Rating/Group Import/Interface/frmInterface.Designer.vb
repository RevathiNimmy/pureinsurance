<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public CommonDialog1Open As System.Windows.Forms.OpenFileDialog
	Public WithEvents cmdOpen As System.Windows.Forms.Button
	Public WithEvents txtFileName As System.Windows.Forms.TextBox
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Frame3 As System.Windows.Forms.GroupBox
	Public WithEvents lblProgress As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Frame2 As System.Windows.Forms.GroupBox
	Public WithEvents cmbVersion As System.Windows.Forms.ComboBox
	Public WithEvents cmbScheme As System.Windows.Forms.ComboBox
	Public WithEvents cmbListType As System.Windows.Forms.ComboBox
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.CommonDialog1Open = New System.Windows.Forms.OpenFileDialog
		Me.Frame3 = New System.Windows.Forms.GroupBox
		Me.cmdOpen = New System.Windows.Forms.Button
		Me.txtFileName = New System.Windows.Forms.TextBox
		Me.Label5 = New System.Windows.Forms.Label
		Me.Frame2 = New System.Windows.Forms.GroupBox
		Me.lblProgress = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.Frame1 = New System.Windows.Forms.GroupBox
		Me.cmbVersion = New System.Windows.Forms.ComboBox
		Me.cmbScheme = New System.Windows.Forms.ComboBox
		Me.cmbListType = New System.Windows.Forms.ComboBox
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.Frame3.SuspendLayout()
		Me.Frame2.SuspendLayout()
		Me.Frame1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' Frame3
		' 
		Me.Frame3.BackColor = System.Drawing.SystemColors.Control
		Me.Frame3.Controls.Add(Me.cmdOpen)
		Me.Frame3.Controls.Add(Me.txtFileName)
		Me.Frame3.Controls.Add(Me.Label5)
		Me.Frame3.Enabled = True
		Me.Frame3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame3.Location = New System.Drawing.Point(8, 136)
		Me.Frame3.Name = "Frame3"
		Me.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame3.Size = New System.Drawing.Size(385, 49)
		Me.Frame3.TabIndex = 11
		Me.Frame3.Visible = True
		' 
		' cmdOpen
		' 
		Me.cmdOpen.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOpen.CausesValidation = True
		Me.cmdOpen.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOpen.Enabled = True
		Me.cmdOpen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOpen.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOpen.Location = New System.Drawing.Point(336, 16)
		Me.cmdOpen.Name = "cmdOpen"
		Me.cmdOpen.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOpen.Size = New System.Drawing.Size(33, 25)
		Me.cmdOpen.TabIndex = 13
		Me.cmdOpen.TabStop = True
		Me.cmdOpen.Text = "..."
		Me.cmdOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtFileName
		' 
		Me.txtFileName.AcceptsReturn = True
		Me.txtFileName.AutoSize = False
		Me.txtFileName.BackColor = System.Drawing.SystemColors.Window
		Me.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFileName.CausesValidation = True
		Me.txtFileName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFileName.Enabled = True
		Me.txtFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtFileName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFileName.HideSelection = True
		Me.txtFileName.Location = New System.Drawing.Point(120, 16)
		Me.txtFileName.MaxLength = 0
		Me.txtFileName.Multiline = False
		Me.txtFileName.Name = "txtFileName"
		Me.txtFileName.ReadOnly = False
		Me.txtFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFileName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFileName.Size = New System.Drawing.Size(201, 25)
		Me.txtFileName.TabIndex = 12
		Me.txtFileName.TabStop = True
		Me.txtFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtFileName.Visible = True
		' 
		' Label5
		' 
		Me.Label5.AutoSize = False
		Me.Label5.BackColor = System.Drawing.SystemColors.Control
		Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label5.Enabled = True
		Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label5.Location = New System.Drawing.Point(16, 16)
		Me.Label5.Name = "Label5"
		Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label5.Size = New System.Drawing.Size(105, 25)
		Me.Label5.TabIndex = 14
		Me.Label5.Text = "Filename"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label5.UseMnemonic = True
		Me.Label5.Visible = True
		' 
		' Frame2
		' 
		Me.Frame2.BackColor = System.Drawing.SystemColors.Control
		Me.Frame2.Controls.Add(Me.lblProgress)
		Me.Frame2.Controls.Add(Me.Label4)
		Me.Frame2.Enabled = True
		Me.Frame2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame2.Location = New System.Drawing.Point(8, 192)
		Me.Frame2.Name = "Frame2"
		Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame2.Size = New System.Drawing.Size(385, 49)
		Me.Frame2.TabIndex = 9
		Me.Frame2.Visible = True
		' 
		' lblProgress
		' 
		Me.lblProgress.AutoSize = False
		Me.lblProgress.BackColor = System.Drawing.SystemColors.Control
		Me.lblProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lblProgress.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProgress.Enabled = True
		Me.lblProgress.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblProgress.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblProgress.Location = New System.Drawing.Point(80, 16)
		Me.lblProgress.Name = "lblProgress"
		Me.lblProgress.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProgress.Size = New System.Drawing.Size(297, 25)
		Me.lblProgress.TabIndex = 15
		Me.lblProgress.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblProgress.UseMnemonic = True
		Me.lblProgress.Visible = True
		' 
		' Label4
		' 
		Me.Label4.AutoSize = False
		Me.Label4.BackColor = System.Drawing.SystemColors.Control
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.Enabled = True
		Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label4.Location = New System.Drawing.Point(8, 16)
		Me.Label4.Name = "Label4"
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.Size = New System.Drawing.Size(73, 17)
		Me.Label4.TabIndex = 10
		Me.Label4.Text = "Progress:"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		' 
		' Frame1
		' 
		Me.Frame1.BackColor = System.Drawing.SystemColors.Control
		Me.Frame1.Controls.Add(Me.cmbVersion)
		Me.Frame1.Controls.Add(Me.cmbScheme)
		Me.Frame1.Controls.Add(Me.cmbListType)
		Me.Frame1.Controls.Add(Me.Label3)
		Me.Frame1.Controls.Add(Me.Label2)
		Me.Frame1.Controls.Add(Me.Label1)
		Me.Frame1.Enabled = True
		Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Frame1.Location = New System.Drawing.Point(8, 8)
		Me.Frame1.Name = "Frame1"
		Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Frame1.Size = New System.Drawing.Size(385, 121)
		Me.Frame1.TabIndex = 2
		Me.Frame1.Visible = True
		' 
		' cmbVersion
		' 
		Me.cmbVersion.BackColor = System.Drawing.SystemColors.Window
		Me.cmbVersion.CausesValidation = True
		Me.cmbVersion.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbVersion.Enabled = True
		Me.cmbVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbVersion.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbVersion.IntegralHeight = True
		Me.cmbVersion.Location = New System.Drawing.Point(120, 88)
		Me.cmbVersion.Name = "cmbVersion"
		Me.cmbVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbVersion.Size = New System.Drawing.Size(257, 21)
		Me.cmbVersion.Sorted = False
		Me.cmbVersion.TabIndex = 8
		Me.cmbVersion.TabStop = True
		Me.cmbVersion.Visible = True
		' 
		' cmbScheme
		' 
		Me.cmbScheme.BackColor = System.Drawing.SystemColors.Window
		Me.cmbScheme.CausesValidation = True
		Me.cmbScheme.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbScheme.Enabled = True
		Me.cmbScheme.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbScheme.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbScheme.IntegralHeight = True
		Me.cmbScheme.Location = New System.Drawing.Point(120, 56)
		Me.cmbScheme.Name = "cmbScheme"
		Me.cmbScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbScheme.Size = New System.Drawing.Size(257, 21)
		Me.cmbScheme.Sorted = False
		Me.cmbScheme.TabIndex = 7
		Me.cmbScheme.TabStop = True
		Me.cmbScheme.Visible = True
		' 
		' cmbListType
		' 
		Me.cmbListType.BackColor = System.Drawing.SystemColors.Window
		Me.cmbListType.CausesValidation = True
		Me.cmbListType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmbListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cmbListType.Enabled = True
		Me.cmbListType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmbListType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cmbListType.IntegralHeight = True
		Me.cmbListType.Location = New System.Drawing.Point(120, 24)
		Me.cmbListType.Name = "cmbListType"
		Me.cmbListType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmbListType.Size = New System.Drawing.Size(257, 21)
		Me.cmbListType.Sorted = False
		Me.cmbListType.TabIndex = 6
		Me.cmbListType.TabStop = True
		Me.cmbListType.Visible = True
		' 
		' Label3
		' 
		Me.Label3.AutoSize = False
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.Enabled = True
		Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Location = New System.Drawing.Point(16, 88)
		Me.Label3.Name = "Label3"
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.Size = New System.Drawing.Size(105, 17)
		Me.Label3.TabIndex = 5
		Me.Label3.Text = "Version :"
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
		Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Location = New System.Drawing.Point(16, 56)
		Me.Label2.Name = "Label2"
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.Size = New System.Drawing.Size(105, 17)
		Me.Label2.TabIndex = 4
		Me.Label2.Text = "Scheme Name :"
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
		Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(16, 24)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(73, 17)
		Me.Label1.TabIndex = 3
		Me.Label1.Text = "List Type :"
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
		Me.cmdCancel.Location = New System.Drawing.Point(304, 248)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(89, 25)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
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
		Me.cmdOK.Location = New System.Drawing.Point(216, 248)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(81, 25)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmInterface
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(400, 280)
		Me.ControlBox = True
		Me.Controls.Add(Me.Frame3)
		Me.Controls.Add(Me.Frame2)
		Me.Controls.Add(Me.Frame1)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Import List Grouping"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Frame3.ResumeLayout(False)
		Me.Frame2.ResumeLayout(False)
		Me.Frame1.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class