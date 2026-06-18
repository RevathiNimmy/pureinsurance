<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntry
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cboPurchase As System.Windows.Forms.ComboBox
	Public WithEvents cboPostQuote As System.Windows.Forms.ComboBox
	Public WithEvents cboPreQuote As System.Windows.Forms.ComboBox
	Public WithEvents txtControlName As System.Windows.Forms.TextBox
	Public WithEvents lblPurchase As System.Windows.Forms.Label
	Public WithEvents lblPostQuote As System.Windows.Forms.Label
	Public WithEvents lblPreQuote As System.Windows.Forms.Label
	Public WithEvents lblControlName As System.Windows.Forms.Label
	Public WithEvents fraEntry As System.Windows.Forms.GroupBox
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEntry))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdHelp = New System.Windows.Forms.Button
		Me.fraEntry = New System.Windows.Forms.GroupBox
		Me.cboPurchase = New System.Windows.Forms.ComboBox
		Me.cboPostQuote = New System.Windows.Forms.ComboBox
		Me.cboPreQuote = New System.Windows.Forms.ComboBox
		Me.txtControlName = New System.Windows.Forms.TextBox
		Me.lblPurchase = New System.Windows.Forms.Label
		Me.lblPostQuote = New System.Windows.Forms.Label
		Me.lblPreQuote = New System.Windows.Forms.Label
		Me.lblControlName = New System.Windows.Forms.Label
		Me.fraEntry.SuspendLayout()
		Me.SuspendLayout()
		Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(96, 183)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 5
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(176, 184)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 4
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdHelp
		' 
		Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
		Me.cmdHelp.CausesValidation = True
		Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdHelp.Enabled = True
		Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdHelp.Location = New System.Drawing.Point(256, 184)
		Me.cmdHelp.Name = "cmdHelp"
		Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
		Me.cmdHelp.TabIndex = 3
		Me.cmdHelp.TabStop = True
		Me.cmdHelp.Text = "&Help"
		Me.cmdHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraEntry
		' 
		Me.fraEntry.BackColor = System.Drawing.SystemColors.Control
		Me.fraEntry.Controls.Add(Me.cboPurchase)
		Me.fraEntry.Controls.Add(Me.cboPostQuote)
		Me.fraEntry.Controls.Add(Me.cboPreQuote)
		Me.fraEntry.Controls.Add(Me.txtControlName)
		Me.fraEntry.Controls.Add(Me.lblPurchase)
		Me.fraEntry.Controls.Add(Me.lblPostQuote)
		Me.fraEntry.Controls.Add(Me.lblPreQuote)
		Me.fraEntry.Controls.Add(Me.lblControlName)
		Me.fraEntry.Enabled = True
		Me.fraEntry.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraEntry.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraEntry.Location = New System.Drawing.Point(8, 8)
		Me.fraEntry.Name = "fraEntry"
		Me.fraEntry.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraEntry.Size = New System.Drawing.Size(321, 169)
		Me.fraEntry.TabIndex = 0
		Me.fraEntry.Text = "Entry Requirements"
		Me.fraEntry.Visible = True
		' 
		' cboPurchase
		' 
		Me.cboPurchase.BackColor = System.Drawing.SystemColors.Window
		Me.cboPurchase.CausesValidation = True
		Me.cboPurchase.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPurchase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPurchase.Enabled = True
		Me.cboPurchase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPurchase.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPurchase.IntegralHeight = True
		Me.cboPurchase.Location = New System.Drawing.Point(120, 120)
		Me.cboPurchase.Name = "cboPurchase"
		Me.cboPurchase.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPurchase.Size = New System.Drawing.Size(105, 21)
		Me.cboPurchase.Sorted = False
		Me.cboPurchase.TabIndex = 10
		Me.cboPurchase.TabStop = True
		Me.cboPurchase.Visible = True
		Me.cboPurchase.Items.AddRange(New Object(){"Optional", "Mandatory"})
		' 
		' cboPostQuote
		' 
		Me.cboPostQuote.BackColor = System.Drawing.SystemColors.Window
		Me.cboPostQuote.CausesValidation = True
		Me.cboPostQuote.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPostQuote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPostQuote.Enabled = True
		Me.cboPostQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPostQuote.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPostQuote.IntegralHeight = True
		Me.cboPostQuote.Location = New System.Drawing.Point(120, 88)
		Me.cboPostQuote.Name = "cboPostQuote"
		Me.cboPostQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPostQuote.Size = New System.Drawing.Size(105, 21)
		Me.cboPostQuote.Sorted = False
		Me.cboPostQuote.TabIndex = 8
		Me.cboPostQuote.TabStop = True
		Me.cboPostQuote.Visible = True
		Me.cboPostQuote.Items.AddRange(New Object(){"Optional", "Mandatory"})
		' 
		' cboPreQuote
		' 
		Me.cboPreQuote.BackColor = System.Drawing.SystemColors.Window
		Me.cboPreQuote.CausesValidation = True
		Me.cboPreQuote.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboPreQuote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboPreQuote.Enabled = True
		Me.cboPreQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboPreQuote.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboPreQuote.IntegralHeight = True
		Me.cboPreQuote.Location = New System.Drawing.Point(120, 56)
		Me.cboPreQuote.Name = "cboPreQuote"
		Me.cboPreQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboPreQuote.Size = New System.Drawing.Size(105, 21)
		Me.cboPreQuote.Sorted = False
		Me.cboPreQuote.TabIndex = 6
		Me.cboPreQuote.TabStop = True
		Me.cboPreQuote.Visible = True
		Me.cboPreQuote.Items.AddRange(New Object(){"Optional", "Mandatory"})
		' 
		' txtControlName
		' 
		Me.txtControlName.AcceptsReturn = True
		Me.txtControlName.AutoSize = False
		Me.txtControlName.BackColor = System.Drawing.SystemColors.Window
		Me.txtControlName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtControlName.CausesValidation = True
		Me.txtControlName.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtControlName.Enabled = False
		Me.txtControlName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtControlName.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtControlName.HideSelection = True
		Me.txtControlName.Location = New System.Drawing.Point(120, 24)
		Me.txtControlName.MaxLength = 0
		Me.txtControlName.Multiline = False
		Me.txtControlName.Name = "txtControlName"
		Me.txtControlName.ReadOnly = False
		Me.txtControlName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtControlName.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtControlName.Size = New System.Drawing.Size(184, 19)
		Me.txtControlName.TabIndex = 2
		Me.txtControlName.TabStop = True
		Me.txtControlName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtControlName.Visible = True
		' 
		' lblPurchase
		' 
		Me.lblPurchase.AutoSize = False
		Me.lblPurchase.BackColor = System.Drawing.SystemColors.Control
		Me.lblPurchase.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPurchase.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPurchase.Enabled = True
		Me.lblPurchase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPurchase.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPurchase.Location = New System.Drawing.Point(16, 120)
		Me.lblPurchase.Name = "lblPurchase"
		Me.lblPurchase.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPurchase.Size = New System.Drawing.Size(97, 17)
		Me.lblPurchase.TabIndex = 11
		Me.lblPurchase.Text = "Purchase:"
		Me.lblPurchase.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPurchase.UseMnemonic = True
		Me.lblPurchase.Visible = True
		' 
		' lblPostQuote
		' 
		Me.lblPostQuote.AutoSize = False
		Me.lblPostQuote.BackColor = System.Drawing.SystemColors.Control
		Me.lblPostQuote.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPostQuote.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPostQuote.Enabled = True
		Me.lblPostQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPostQuote.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPostQuote.Location = New System.Drawing.Point(16, 88)
		Me.lblPostQuote.Name = "lblPostQuote"
		Me.lblPostQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPostQuote.Size = New System.Drawing.Size(97, 17)
		Me.lblPostQuote.TabIndex = 9
		Me.lblPostQuote.Text = "Post quote:"
		Me.lblPostQuote.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPostQuote.UseMnemonic = True
		Me.lblPostQuote.Visible = True
		' 
		' lblPreQuote
		' 
		Me.lblPreQuote.AutoSize = False
		Me.lblPreQuote.BackColor = System.Drawing.SystemColors.Control
		Me.lblPreQuote.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPreQuote.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPreQuote.Enabled = True
		Me.lblPreQuote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPreQuote.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPreQuote.Location = New System.Drawing.Point(16, 56)
		Me.lblPreQuote.Name = "lblPreQuote"
		Me.lblPreQuote.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPreQuote.Size = New System.Drawing.Size(97, 17)
		Me.lblPreQuote.TabIndex = 7
		Me.lblPreQuote.Text = "Pre quote:"
		Me.lblPreQuote.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPreQuote.UseMnemonic = True
		Me.lblPreQuote.Visible = True
		' 
		' lblControlName
		' 
		Me.lblControlName.AutoSize = False
		Me.lblControlName.BackColor = System.Drawing.SystemColors.Control
		Me.lblControlName.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblControlName.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblControlName.Enabled = True
		Me.lblControlName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblControlName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblControlName.Location = New System.Drawing.Point(16, 27)
		Me.lblControlName.Name = "lblControlName"
		Me.lblControlName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblControlName.Size = New System.Drawing.Size(89, 17)
		Me.lblControlName.TabIndex = 1
		Me.lblControlName.Text = "Control:"
		Me.lblControlName.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblControlName.UseMnemonic = True
		Me.lblControlName.Visible = True
		' 
		' frmEntry
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(336, 213)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdHelp)
		Me.Controls.Add(Me.fraEntry)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = True
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(3, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmEntry"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Data Entry Requirements"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboPurchase, New Integer(){0, 0})
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboPostQuote, New Integer(){0, 0})
		Me.listBoxComboBoxHelper1.SetItemData(Me.cboPreQuote, New Integer(){0, 0})
		CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraEntry.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class