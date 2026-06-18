<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAssign
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		Form_Initialize_Renamed()
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
	Public WithEvents lblStatus As System.Windows.Forms.Label
	Public WithEvents lblPayeeOrSupplier As System.Windows.Forms.Label
	Public WithEvents cboStatus As System.Windows.Forms.ComboBox
	Public WithEvents txtPayeeOrSupplier As System.Windows.Forms.TextBox
	Public WithEvents cmdSupplier As System.Windows.Forms.Button
	Private WithEvents _tabAssign_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabAssign As System.Windows.Forms.TabControl
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAssign))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabAssign = New System.Windows.Forms.TabControl
		Me._tabAssign_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblStatus = New System.Windows.Forms.Label
		Me.lblPayeeOrSupplier = New System.Windows.Forms.Label
		Me.cboStatus = New System.Windows.Forms.ComboBox
		Me.txtPayeeOrSupplier = New System.Windows.Forms.TextBox
		Me.cmdSupplier = New System.Windows.Forms.Button
		Me.Label1 = New System.Windows.Forms.Label
		Me.tabAssign.SuspendLayout()
		Me._tabAssign_TabPage0.SuspendLayout()
		Me.SuspendLayout()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(272, 144)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 8
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
		Me.cmdOK.Location = New System.Drawing.Point(0, 144)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 0
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "Ok"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabAssign
		' 
		Me.tabAssign.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabAssign.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabAssign.Controls.Add(Me._tabAssign_TabPage0)
		Me.tabAssign.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabAssign.ItemSize = New System.Drawing.Size(114, 18)
		Me.tabAssign.Location = New System.Drawing.Point(0, 0)
		Me.tabAssign.Multiline = True
		Me.tabAssign.Name = "tabAssign"
		Me.tabAssign.Size = New System.Drawing.Size(349, 141)
		Me.tabAssign.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabAssign.TabIndex = 1
		' 
		' _tabAssign_TabPage0
		' 
		Me._tabAssign_TabPage0.Controls.Add(Me.lblStatus)
		Me._tabAssign_TabPage0.Controls.Add(Me.lblPayeeOrSupplier)
		Me._tabAssign_TabPage0.Controls.Add(Me.cboStatus)
		Me._tabAssign_TabPage0.Controls.Add(Me.txtPayeeOrSupplier)
		Me._tabAssign_TabPage0.Controls.Add(Me.cmdSupplier)
		Me._tabAssign_TabPage0.Text = "&1-Assign"
		' 
		' lblStatus
		' 
		Me.lblStatus.AutoSize = False
		Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStatus.Enabled = True
		Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStatus.Location = New System.Drawing.Point(8, 52)
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStatus.Size = New System.Drawing.Size(113, 17)
		Me.lblStatus.TabIndex = 3
		Me.lblStatus.Text = "Status"
		Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStatus.UseMnemonic = True
		Me.lblStatus.Visible = True
		' 
		' lblPayeeOrSupplier
		' 
		Me.lblPayeeOrSupplier.AutoSize = False
		Me.lblPayeeOrSupplier.BackColor = System.Drawing.SystemColors.Control
		Me.lblPayeeOrSupplier.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPayeeOrSupplier.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPayeeOrSupplier.Enabled = True
		Me.lblPayeeOrSupplier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPayeeOrSupplier.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPayeeOrSupplier.Location = New System.Drawing.Point(8, 20)
		Me.lblPayeeOrSupplier.Name = "lblPayeeOrSupplier"
		Me.lblPayeeOrSupplier.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPayeeOrSupplier.Size = New System.Drawing.Size(113, 17)
		Me.lblPayeeOrSupplier.TabIndex = 5
		Me.lblPayeeOrSupplier.Text = "Supplier"
		Me.lblPayeeOrSupplier.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPayeeOrSupplier.UseMnemonic = True
		Me.lblPayeeOrSupplier.Visible = True
		' 
		' cboStatus
		' 
		Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
		Me.cboStatus.CausesValidation = True
		Me.cboStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboStatus.Enabled = True
		Me.cboStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboStatus.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboStatus.IntegralHeight = True
		Me.cboStatus.Location = New System.Drawing.Point(136, 52)
		Me.cboStatus.Name = "cboStatus"
		Me.cboStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboStatus.Size = New System.Drawing.Size(159, 21)
		Me.cboStatus.Sorted = False
		Me.cboStatus.TabIndex = 2
		Me.cboStatus.TabStop = True
		Me.cboStatus.Visible = True
		' 
		' txtPayeeOrSupplier
		' 
		Me.txtPayeeOrSupplier.AcceptsReturn = True
		Me.txtPayeeOrSupplier.AutoSize = False
		Me.txtPayeeOrSupplier.BackColor = System.Drawing.SystemColors.Window
		Me.txtPayeeOrSupplier.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPayeeOrSupplier.CausesValidation = True
		Me.txtPayeeOrSupplier.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPayeeOrSupplier.Enabled = True
		Me.txtPayeeOrSupplier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtPayeeOrSupplier.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPayeeOrSupplier.HideSelection = True
		Me.txtPayeeOrSupplier.Location = New System.Drawing.Point(136, 20)
		Me.txtPayeeOrSupplier.MaxLength = 0
		Me.txtPayeeOrSupplier.Multiline = False
		Me.txtPayeeOrSupplier.Name = "txtPayeeOrSupplier"
		Me.txtPayeeOrSupplier.ReadOnly = False
		Me.txtPayeeOrSupplier.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPayeeOrSupplier.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPayeeOrSupplier.Size = New System.Drawing.Size(127, 19)
		Me.txtPayeeOrSupplier.TabIndex = 6
		Me.txtPayeeOrSupplier.TabStop = True
		Me.txtPayeeOrSupplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPayeeOrSupplier.Visible = True
		' 
		' cmdSupplier
		' 
		Me.cmdSupplier.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSupplier.CausesValidation = True
		Me.cmdSupplier.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSupplier.Enabled = True
		Me.cmdSupplier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdSupplier.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSupplier.Location = New System.Drawing.Point(272, 20)
		Me.cmdSupplier.Name = "cmdSupplier"
		Me.cmdSupplier.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSupplier.Size = New System.Drawing.Size(25, 19)
		Me.cmdSupplier.TabIndex = 7
		Me.cmdSupplier.TabStop = True
		Me.cmdSupplier.Text = "..."
		Me.cmdSupplier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSupplier.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' Label1
		' 
		Me.Label1.AutoSize = False
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.Enabled = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Location = New System.Drawing.Point(0, 0)
		Me.Label1.Name = "Label1"
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.Size = New System.Drawing.Size(77, 17)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = "Supplier"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		' 
		' frmAssign
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(350, 174)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabAssign)
		Me.Controls.Add(Me.Label1)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmAssign"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Assign"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabAssign, 1)
		Me.tabAssign.ResumeLayout(False)
		Me._tabAssign_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class