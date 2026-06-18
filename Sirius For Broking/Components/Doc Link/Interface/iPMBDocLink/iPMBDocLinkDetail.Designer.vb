<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDetails
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
	Public WithEvents lblProcess As System.Windows.Forms.Label
	Public WithEvents lblAgent As System.Windows.Forms.Label
	Public WithEvents lblGISSCheme As System.Windows.Forms.Label
	Public WithEvents lblDocumentTemplate As System.Windows.Forms.Label
	Public WithEvents lblDocType As System.Windows.Forms.Label
	Public WithEvents txtDocumentTemplate As System.Windows.Forms.TextBox
	Public WithEvents txtAgent As System.Windows.Forms.TextBox
	Public WithEvents cboProcess As System.Windows.Forms.ComboBox
	Public WithEvents cmdFindAgent As System.Windows.Forms.Button
	Public WithEvents cmdFindDocumentTemplate As System.Windows.Forms.Button
	Public WithEvents txtDocType As System.Windows.Forms.TextBox
	Public WithEvents txtGISScheme As System.Windows.Forms.TextBox
	Public WithEvents chkSpoolDocument As System.Windows.Forms.CheckBox
	Public WithEvents chkAutoArchiveDocument As System.Windows.Forms.CheckBox
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetails))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.tabMain = New System.Windows.Forms.TabControl
		Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblProcess = New System.Windows.Forms.Label
		Me.lblAgent = New System.Windows.Forms.Label
		Me.lblGISSCheme = New System.Windows.Forms.Label
		Me.lblDocumentTemplate = New System.Windows.Forms.Label
		Me.lblDocType = New System.Windows.Forms.Label
		Me.txtDocumentTemplate = New System.Windows.Forms.TextBox
		Me.txtAgent = New System.Windows.Forms.TextBox
		Me.cboProcess = New System.Windows.Forms.ComboBox
		Me.cmdFindAgent = New System.Windows.Forms.Button
		Me.cmdFindDocumentTemplate = New System.Windows.Forms.Button
		Me.txtDocType = New System.Windows.Forms.TextBox
		Me.txtGISScheme = New System.Windows.Forms.TextBox
		Me.chkSpoolDocument = New System.Windows.Forms.CheckBox
		Me.chkAutoArchiveDocument = New System.Windows.Forms.CheckBox
		Me.tabMain.SuspendLayout()
		Me._tabMain_TabPage0.SuspendLayout()
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
		Me.cmdCancel.Location = New System.Drawing.Point(432, 224)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 8
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Ca&ncel"
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
		Me.cmdOK.Location = New System.Drawing.Point(352, 224)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 7
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "&OK"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabMain
		' 
		Me.tabMain.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
		Me.tabMain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabMain.HotTrack = False
		Me.tabMain.ItemSize = New System.Drawing.Size(167, 18)
		Me.tabMain.Location = New System.Drawing.Point(3, 3)
		Me.tabMain.Multiline = True
		Me.tabMain.Name = "tabMain"
		Me.tabMain.Size = New System.Drawing.Size(509, 218)
		Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabMain.TabIndex = 9
		Me.tabMain.TabStop = False
		' 
		' _tabMain_TabPage0
		' 
		Me._tabMain_TabPage0.Controls.Add(Me.lblProcess)
		Me._tabMain_TabPage0.Controls.Add(Me.lblAgent)
		Me._tabMain_TabPage0.Controls.Add(Me.lblGISSCheme)
		Me._tabMain_TabPage0.Controls.Add(Me.lblDocumentTemplate)
		Me._tabMain_TabPage0.Controls.Add(Me.lblDocType)
		Me._tabMain_TabPage0.Controls.Add(Me.txtDocumentTemplate)
		Me._tabMain_TabPage0.Controls.Add(Me.txtAgent)
		Me._tabMain_TabPage0.Controls.Add(Me.cboProcess)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdFindAgent)
		Me._tabMain_TabPage0.Controls.Add(Me.cmdFindDocumentTemplate)
		Me._tabMain_TabPage0.Controls.Add(Me.txtDocType)
		Me._tabMain_TabPage0.Controls.Add(Me.txtGISScheme)
		Me._tabMain_TabPage0.Controls.Add(Me.chkSpoolDocument)
		Me._tabMain_TabPage0.Controls.Add(Me.chkAutoArchiveDocument)
		Me._tabMain_TabPage0.Text = "&1 - General"
		' 
		' lblProcess
		' 
		Me.lblProcess.AutoSize = True
		Me.lblProcess.BackColor = System.Drawing.SystemColors.Control
		Me.lblProcess.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblProcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProcess.Enabled = True
		Me.lblProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblProcess.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblProcess.Location = New System.Drawing.Point(16, 84)
		Me.lblProcess.Name = "lblProcess"
		Me.lblProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProcess.Size = New System.Drawing.Size(44, 13)
		Me.lblProcess.TabIndex = 14
		Me.lblProcess.Text = "Process"
		Me.lblProcess.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblProcess.UseMnemonic = True
		Me.lblProcess.Visible = True
		' 
		' lblAgent
		' 
		Me.lblAgent.AutoSize = True
		Me.lblAgent.BackColor = System.Drawing.SystemColors.Control
		Me.lblAgent.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAgent.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAgent.Enabled = True
		Me.lblAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAgent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAgent.Location = New System.Drawing.Point(16, 108)
		Me.lblAgent.Name = "lblAgent"
		Me.lblAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAgent.Size = New System.Drawing.Size(33, 13)
		Me.lblAgent.TabIndex = 15
		Me.lblAgent.Text = "Agent"
		Me.lblAgent.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAgent.UseMnemonic = True
		Me.lblAgent.Visible = True
		' 
		' lblGISSCheme
		' 
		Me.lblGISSCheme.AutoSize = True
		Me.lblGISSCheme.BackColor = System.Drawing.SystemColors.Control
		Me.lblGISSCheme.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblGISSCheme.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblGISSCheme.Enabled = True
		Me.lblGISSCheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblGISSCheme.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblGISSCheme.Location = New System.Drawing.Point(16, 12)
		Me.lblGISSCheme.Name = "lblGISSCheme"
		Me.lblGISSCheme.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblGISSCheme.Size = New System.Drawing.Size(46, 13)
		Me.lblGISSCheme.TabIndex = 10
		Me.lblGISSCheme.Text = "Scheme"
		Me.lblGISSCheme.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblGISSCheme.UseMnemonic = True
		Me.lblGISSCheme.Visible = True
		' 
		' lblDocumentTemplate
		' 
		Me.lblDocumentTemplate.AutoSize = True
		Me.lblDocumentTemplate.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocumentTemplate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocumentTemplate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocumentTemplate.Enabled = True
		Me.lblDocumentTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocumentTemplate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocumentTemplate.Location = New System.Drawing.Point(16, 60)
		Me.lblDocumentTemplate.Name = "lblDocumentTemplate"
		Me.lblDocumentTemplate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocumentTemplate.Size = New System.Drawing.Size(115, 13)
		Me.lblDocumentTemplate.TabIndex = 12
		Me.lblDocumentTemplate.Text = "Document Template"
		Me.lblDocumentTemplate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocumentTemplate.UseMnemonic = True
		Me.lblDocumentTemplate.Visible = True
		' 
		' lblDocType
		' 
		Me.lblDocType.AutoSize = True
		Me.lblDocType.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocType.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocType.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocType.Enabled = True
		Me.lblDocType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocType.Location = New System.Drawing.Point(16, 36)
		Me.lblDocType.Name = "lblDocType"
		Me.lblDocType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocType.Size = New System.Drawing.Size(90, 13)
		Me.lblDocType.TabIndex = 11
		Me.lblDocType.Text = "Document Type"
		Me.lblDocType.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocType.UseMnemonic = True
		Me.lblDocType.Visible = True
		' 
		' txtDocumentTemplate
		' 
		Me.txtDocumentTemplate.AcceptsReturn = True
		Me.txtDocumentTemplate.AutoSize = False
		Me.txtDocumentTemplate.BackColor = System.Drawing.SystemColors.Window
		Me.txtDocumentTemplate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDocumentTemplate.CausesValidation = True
		Me.txtDocumentTemplate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDocumentTemplate.Enabled = False
		Me.txtDocumentTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDocumentTemplate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDocumentTemplate.HideSelection = True
		Me.txtDocumentTemplate.Location = New System.Drawing.Point(217, 60)
		Me.txtDocumentTemplate.MaxLength = 0
		Me.txtDocumentTemplate.Multiline = False
		Me.txtDocumentTemplate.Name = "txtDocumentTemplate"
		Me.txtDocumentTemplate.ReadOnly = False
		Me.txtDocumentTemplate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDocumentTemplate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDocumentTemplate.Size = New System.Drawing.Size(276, 22)
		Me.txtDocumentTemplate.TabIndex = 13
		Me.txtDocumentTemplate.TabStop = True
		Me.txtDocumentTemplate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDocumentTemplate.Visible = True
		' 
		' txtAgent
		' 
		Me.txtAgent.AcceptsReturn = True
		Me.txtAgent.AutoSize = False
		Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
		Me.txtAgent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAgent.CausesValidation = True
		Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAgent.Enabled = False
		Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAgent.HideSelection = True
		Me.txtAgent.Location = New System.Drawing.Point(217, 108)
		Me.txtAgent.MaxLength = 0
		Me.txtAgent.Multiline = False
		Me.txtAgent.Name = "txtAgent"
		Me.txtAgent.ReadOnly = False
		Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAgent.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAgent.Size = New System.Drawing.Size(276, 22)
		Me.txtAgent.TabIndex = 16
		Me.txtAgent.TabStop = True
		Me.txtAgent.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAgent.Visible = True
		' 
		' cboProcess
		' 
		Me.cboProcess.BackColor = System.Drawing.SystemColors.Window
		Me.cboProcess.CausesValidation = True
		Me.cboProcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboProcess.Enabled = True
		Me.cboProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboProcess.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboProcess.IntegralHeight = True
		Me.cboProcess.Location = New System.Drawing.Point(217, 84)
		Me.cboProcess.Name = "cboProcess"
		Me.cboProcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboProcess.Size = New System.Drawing.Size(276, 21)
		Me.cboProcess.Sorted = False
		Me.cboProcess.TabIndex = 3
		Me.cboProcess.TabStop = True
		Me.cboProcess.Visible = True
		' 
		' cmdFindAgent
		' 
		Me.cmdFindAgent.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFindAgent.CausesValidation = True
		Me.cmdFindAgent.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFindAgent.Enabled = True
		Me.cmdFindAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFindAgent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFindAgent.Location = New System.Drawing.Point(474, 110)
		Me.cmdFindAgent.Name = "cmdFindAgent"
		Me.cmdFindAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFindAgent.Size = New System.Drawing.Size(18, 19)
		Me.cmdFindAgent.TabIndex = 4
		Me.cmdFindAgent.TabStop = True
		Me.cmdFindAgent.Text = "..."
		Me.cmdFindAgent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFindAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdFindDocumentTemplate
		' 
		Me.cmdFindDocumentTemplate.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFindDocumentTemplate.CausesValidation = True
		Me.cmdFindDocumentTemplate.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFindDocumentTemplate.Enabled = True
		Me.cmdFindDocumentTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFindDocumentTemplate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFindDocumentTemplate.Location = New System.Drawing.Point(474, 62)
		Me.cmdFindDocumentTemplate.Name = "cmdFindDocumentTemplate"
		Me.cmdFindDocumentTemplate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFindDocumentTemplate.Size = New System.Drawing.Size(18, 19)
		Me.cmdFindDocumentTemplate.TabIndex = 2
		Me.cmdFindDocumentTemplate.TabStop = True
		Me.cmdFindDocumentTemplate.Text = "..."
		Me.cmdFindDocumentTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFindDocumentTemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtDocType
		' 
		Me.txtDocType.AcceptsReturn = True
		Me.txtDocType.AutoSize = False
		Me.txtDocType.BackColor = System.Drawing.SystemColors.Window
		Me.txtDocType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDocType.CausesValidation = True
		Me.txtDocType.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDocType.Enabled = False
		Me.txtDocType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDocType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDocType.HideSelection = True
		Me.txtDocType.Location = New System.Drawing.Point(217, 36)
		Me.txtDocType.MaxLength = 0
		Me.txtDocType.Multiline = False
		Me.txtDocType.Name = "txtDocType"
		Me.txtDocType.ReadOnly = False
		Me.txtDocType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDocType.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDocType.Size = New System.Drawing.Size(276, 19)
		Me.txtDocType.TabIndex = 1
		Me.txtDocType.TabStop = True
		Me.txtDocType.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDocType.Visible = True
		' 
		' txtGISScheme
		' 
		Me.txtGISScheme.AcceptsReturn = True
		Me.txtGISScheme.AutoSize = False
		Me.txtGISScheme.BackColor = System.Drawing.SystemColors.Window
		Me.txtGISScheme.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtGISScheme.CausesValidation = True
		Me.txtGISScheme.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtGISScheme.Enabled = False
		Me.txtGISScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtGISScheme.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtGISScheme.HideSelection = True
		Me.txtGISScheme.Location = New System.Drawing.Point(217, 12)
		Me.txtGISScheme.MaxLength = 0
		Me.txtGISScheme.Multiline = False
		Me.txtGISScheme.Name = "txtGISScheme"
		Me.txtGISScheme.ReadOnly = False
		Me.txtGISScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtGISScheme.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtGISScheme.Size = New System.Drawing.Size(276, 19)
		Me.txtGISScheme.TabIndex = 0
		Me.txtGISScheme.TabStop = True
		Me.txtGISScheme.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtGISScheme.Visible = True
		' 
		' chkSpoolDocument
		' 
		Me.chkSpoolDocument.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkSpoolDocument.BackColor = System.Drawing.SystemColors.Control
		Me.chkSpoolDocument.CausesValidation = True
		Me.chkSpoolDocument.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.chkSpoolDocument.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkSpoolDocument.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkSpoolDocument.Enabled = True
		Me.chkSpoolDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkSpoolDocument.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkSpoolDocument.Location = New System.Drawing.Point(12, 139)
		Me.chkSpoolDocument.Name = "chkSpoolDocument"
		Me.chkSpoolDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkSpoolDocument.Size = New System.Drawing.Size(217, 13)
		Me.chkSpoolDocument.TabIndex = 5
		Me.chkSpoolDocument.TabStop = True
		Me.chkSpoolDocument.Text = "Automatically Spool Documents"
		Me.chkSpoolDocument.Visible = True
		' 
		' chkAutoArchiveDocument
		' 
		Me.chkAutoArchiveDocument.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkAutoArchiveDocument.BackColor = System.Drawing.SystemColors.Control
		Me.chkAutoArchiveDocument.CausesValidation = True
		Me.chkAutoArchiveDocument.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
		Me.chkAutoArchiveDocument.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkAutoArchiveDocument.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkAutoArchiveDocument.Enabled = True
		Me.chkAutoArchiveDocument.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkAutoArchiveDocument.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkAutoArchiveDocument.Location = New System.Drawing.Point(12, 164)
		Me.chkAutoArchiveDocument.Name = "chkAutoArchiveDocument"
		Me.chkAutoArchiveDocument.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkAutoArchiveDocument.Size = New System.Drawing.Size(217, 13)
		Me.chkAutoArchiveDocument.TabIndex = 6
		Me.chkAutoArchiveDocument.TabStop = True
		Me.chkAutoArchiveDocument.Text = "Automatically Archive Documents"
		Me.chkAutoArchiveDocument.Visible = True
		' 
		' frmDetails
		' 
		Me.AcceptButton = Me.cmdOK
		Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.CancelButton = Me.cmdCancel
		Me.ClientSize = New System.Drawing.Size(512, 251)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.tabMain)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(125, 99)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmDetails"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Settings"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Artinsoft.VB6.Gui.SSTabHelper.SetTabs(Me.tabMain, 1)
		Me.tabMain.ResumeLayout(False)
		Me._tabMain_TabPage0.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class