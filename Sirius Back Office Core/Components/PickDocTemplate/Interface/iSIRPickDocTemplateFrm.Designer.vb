<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPickDocumentTemplate
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
	Public WithEvents cmdFind As System.Windows.Forms.Button
	Public WithEvents cmdClear As System.Windows.Forms.Button
	Public WithEvents txtSearchCode As System.Windows.Forms.TextBox
	Public WithEvents lblCode As System.Windows.Forms.Label
	Public WithEvents fraSearch As System.Windows.Forms.Panel
	Public WithEvents PickList As uctPickList.PickList
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents CmdOk As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPickDocumentTemplate))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraSearch = New System.Windows.Forms.Panel
		Me.cmdFind = New System.Windows.Forms.Button
		Me.cmdClear = New System.Windows.Forms.Button
		Me.txtSearchCode = New System.Windows.Forms.TextBox
		Me.lblCode = New System.Windows.Forms.Label
		Me.PickList = New uctPickList.PickList
		Me.cmdApply = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.CmdOk = New System.Windows.Forms.Button
		Me.fraSearch.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' fraSearch
		' 
		Me.fraSearch.BackColor = System.Drawing.SystemColors.Control
		Me.fraSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraSearch.Controls.Add(Me.cmdFind)
		Me.fraSearch.Controls.Add(Me.cmdClear)
		Me.fraSearch.Controls.Add(Me.txtSearchCode)
		Me.fraSearch.Controls.Add(Me.lblCode)
		Me.fraSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraSearch.Enabled = True
        Me.fraSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraSearch.Location = New System.Drawing.Point(8, 318)
		Me.fraSearch.Name = "fraSearch"
		Me.fraSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraSearch.Size = New System.Drawing.Size(322, 37)
		Me.fraSearch.TabIndex = 4
		Me.fraSearch.Visible = False
		' 
		' cmdFind
		' 
		Me.cmdFind.BackColor = System.Drawing.SystemColors.Control
		Me.cmdFind.CausesValidation = True
		Me.cmdFind.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdFind.Enabled = True
		Me.cmdFind.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdFind.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdFind.Location = New System.Drawing.Point(153, 10)
		Me.cmdFind.Name = "cmdFind"
		Me.cmdFind.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdFind.Size = New System.Drawing.Size(79, 23)
		Me.cmdFind.TabIndex = 8
		Me.cmdFind.TabStop = True
		Me.cmdFind.Text = "&Find"
		Me.cmdFind.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdClear
		' 
		Me.cmdClear.BackColor = System.Drawing.SystemColors.Control
		Me.cmdClear.CausesValidation = True
		Me.cmdClear.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdClear.Enabled = True
		Me.cmdClear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdClear.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdClear.Location = New System.Drawing.Point(237, 10)
		Me.cmdClear.Name = "cmdClear"
		Me.cmdClear.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdClear.Size = New System.Drawing.Size(75, 23)
		Me.cmdClear.TabIndex = 7
		Me.cmdClear.TabStop = True
		Me.cmdClear.Text = "C&lear"
		Me.cmdClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' txtSearchCode
		' 
		Me.txtSearchCode.AcceptsReturn = True
		Me.txtSearchCode.AutoSize = False
		Me.txtSearchCode.BackColor = System.Drawing.SystemColors.Window
		Me.txtSearchCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSearchCode.CausesValidation = True
		Me.txtSearchCode.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSearchCode.Enabled = True
		Me.txtSearchCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSearchCode.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSearchCode.HideSelection = True
		Me.txtSearchCode.Location = New System.Drawing.Point(53, 9)
		Me.txtSearchCode.MaxLength = 10
		Me.txtSearchCode.Multiline = False
		Me.txtSearchCode.Name = "txtSearchCode"
		Me.txtSearchCode.ReadOnly = False
		Me.txtSearchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSearchCode.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSearchCode.Size = New System.Drawing.Size(88, 24)
		Me.txtSearchCode.TabIndex = 6
		Me.txtSearchCode.TabStop = True
		Me.txtSearchCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSearchCode.Visible = True
		' 
		' lblCode
		' 
		Me.lblCode.AutoSize = False
		Me.lblCode.BackColor = System.Drawing.SystemColors.Control
		Me.lblCode.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCode.Enabled = True
		Me.lblCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCode.Location = New System.Drawing.Point(9, 15)
		Me.lblCode.Name = "lblCode"
		Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCode.Size = New System.Drawing.Size(41, 19)
		Me.lblCode.TabIndex = 5
		Me.lblCode.Text = "Code:"
		Me.lblCode.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCode.UseMnemonic = True
		Me.lblCode.Visible = True
		' 
		' PickList
		' 
		Me.PickList.AvailableCaption = "Clauses Available"
		Me.PickList.BusinessObject = ""
		Me.PickList.Location = New System.Drawing.Point(0, 0)
		Me.PickList.Name = "PickList"
		Me.PickList.PickListType = ""
		Me.PickList.Size = New System.Drawing.Size(649, 325)
		Me.PickList.TabIndex = 3
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = True
		Me.cmdApply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(574, 326)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(69, 27)
		Me.cmdApply.TabIndex = 2
		Me.cmdApply.TabStop = True
		Me.cmdApply.Text = "&Apply"
		Me.cmdApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(494, 326)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(75, 27)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' CmdOk
		' 
		Me.CmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.CmdOk.CausesValidation = True
		Me.CmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.CmdOk.Enabled = False
		Me.CmdOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.CmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CmdOk.Location = New System.Drawing.Point(410, 326)
		Me.CmdOk.Name = "CmdOk"
		Me.CmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CmdOk.Size = New System.Drawing.Size(79, 27)
		Me.CmdOk.TabIndex = 0
		Me.CmdOk.TabStop = True
		Me.CmdOk.Text = "&Ok"
		Me.CmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmPickDocumentTemplate
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(650, 354)
		Me.ControlBox = True
		Me.Controls.Add(Me.fraSearch)
		Me.Controls.Add(Me.PickList)
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.CmdOk)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.HelpButton = True
		Me.Icon = CType(resources.GetObject("frmPickDocumentTemplate.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(10, 29)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "frmPickDocumentTemplate"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Clause Selection"
		Me.Visible = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraSearch.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class