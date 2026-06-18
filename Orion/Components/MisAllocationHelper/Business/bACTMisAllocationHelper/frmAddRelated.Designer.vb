<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddRelated
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
	Public WithEvents chkOnlyShowOS As System.Windows.Forms.CheckBox
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents txtDocumentRef As System.Windows.Forms.TextBox
	Public WithEvents lblAmount As System.Windows.Forms.Label
	Public WithEvents lblDocumentRef As System.Windows.Forms.Label
	Public WithEvents fraFilter As System.Windows.Forms.GroupBox
	Public WithEvents cmdRefresh As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Private WithEvents grdTransactions As Artinsoft.Windows.Forms.ExtendedDataGridView
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddRelated))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.fraFilter = New System.Windows.Forms.GroupBox
		Me.chkOnlyShowOS = New System.Windows.Forms.CheckBox
		Me.txtAmount = New System.Windows.Forms.TextBox
		Me.txtDocumentRef = New System.Windows.Forms.TextBox
		Me.lblAmount = New System.Windows.Forms.Label
		Me.lblDocumentRef = New System.Windows.Forms.Label
		Me.cmdRefresh = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.fraFilter.SuspendLayout()
		Me.SuspendLayout()
		Me.grdTransactions = New Artinsoft.Windows.Forms.ExtendedDataGridView(Me.components)
		CType(Me.grdTransactions, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' fraFilter
		' 
		Me.fraFilter.BackColor = System.Drawing.SystemColors.Control
		Me.fraFilter.Controls.Add(Me.chkOnlyShowOS)
		Me.fraFilter.Controls.Add(Me.txtAmount)
		Me.fraFilter.Controls.Add(Me.txtDocumentRef)
		Me.fraFilter.Controls.Add(Me.lblAmount)
		Me.fraFilter.Controls.Add(Me.lblDocumentRef)
		Me.fraFilter.Enabled = True
		Me.fraFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraFilter.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraFilter.Location = New System.Drawing.Point(520, 8)
		Me.fraFilter.Name = "fraFilter"
		Me.fraFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraFilter.Size = New System.Drawing.Size(153, 137)
		Me.fraFilter.TabIndex = 4
		Me.fraFilter.Text = "Filter"
		Me.fraFilter.Visible = True
		' 
		' chkOnlyShowOS
		' 
		Me.chkOnlyShowOS.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkOnlyShowOS.BackColor = System.Drawing.SystemColors.Control
		Me.chkOnlyShowOS.CausesValidation = True
		Me.chkOnlyShowOS.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkOnlyShowOS.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkOnlyShowOS.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkOnlyShowOS.Enabled = True
		Me.chkOnlyShowOS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.chkOnlyShowOS.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkOnlyShowOS.Location = New System.Drawing.Point(8, 16)
		Me.chkOnlyShowOS.Name = "chkOnlyShowOS"
		Me.chkOnlyShowOS.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkOnlyShowOS.Size = New System.Drawing.Size(137, 25)
		Me.chkOnlyShowOS.TabIndex = 9
		Me.chkOnlyShowOS.TabStop = True
		Me.chkOnlyShowOS.Text = "Only show outstanding transactions"
		Me.chkOnlyShowOS.Visible = True
		' 
		' txtAmount
		' 
		Me.txtAmount.AcceptsReturn = True
		Me.txtAmount.AutoSize = False
		Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAmount.CausesValidation = True
		Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAmount.Enabled = True
		Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAmount.HideSelection = True
		Me.txtAmount.Location = New System.Drawing.Point(8, 104)
		Me.txtAmount.MaxLength = 0
		Me.txtAmount.Multiline = False
		Me.txtAmount.Name = "txtAmount"
		Me.txtAmount.ReadOnly = False
		Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAmount.Size = New System.Drawing.Size(137, 19)
		Me.txtAmount.TabIndex = 7
		Me.txtAmount.TabStop = True
		Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtAmount.Visible = True
		' 
		' txtDocumentRef
		' 
		Me.txtDocumentRef.AcceptsReturn = True
		Me.txtDocumentRef.AutoSize = False
		Me.txtDocumentRef.BackColor = System.Drawing.SystemColors.Window
		Me.txtDocumentRef.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDocumentRef.CausesValidation = True
		Me.txtDocumentRef.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDocumentRef.Enabled = True
		Me.txtDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtDocumentRef.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDocumentRef.HideSelection = True
		Me.txtDocumentRef.Location = New System.Drawing.Point(8, 64)
		Me.txtDocumentRef.MaxLength = 0
		Me.txtDocumentRef.Multiline = False
		Me.txtDocumentRef.Name = "txtDocumentRef"
		Me.txtDocumentRef.ReadOnly = False
		Me.txtDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDocumentRef.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDocumentRef.Size = New System.Drawing.Size(137, 19)
		Me.txtDocumentRef.TabIndex = 5
		Me.txtDocumentRef.TabStop = True
		Me.txtDocumentRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDocumentRef.Visible = True
		' 
		' lblAmount
		' 
		Me.lblAmount.AutoSize = False
		Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAmount.Enabled = True
		Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAmount.Location = New System.Drawing.Point(8, 88)
		Me.lblAmount.Name = "lblAmount"
		Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAmount.Size = New System.Drawing.Size(137, 17)
		Me.lblAmount.TabIndex = 8
		Me.lblAmount.Text = "Amount"
		Me.lblAmount.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAmount.UseMnemonic = True
		Me.lblAmount.Visible = True
		' 
		' lblDocumentRef
		' 
		Me.lblDocumentRef.AutoSize = False
		Me.lblDocumentRef.BackColor = System.Drawing.SystemColors.Control
		Me.lblDocumentRef.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDocumentRef.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDocumentRef.Enabled = True
		Me.lblDocumentRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblDocumentRef.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDocumentRef.Location = New System.Drawing.Point(8, 48)
		Me.lblDocumentRef.Name = "lblDocumentRef"
		Me.lblDocumentRef.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDocumentRef.Size = New System.Drawing.Size(137, 17)
		Me.lblDocumentRef.TabIndex = 6
		Me.lblDocumentRef.Text = "Document Ref"
		Me.lblDocumentRef.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDocumentRef.UseMnemonic = True
		Me.lblDocumentRef.Visible = True
		' 
		' cmdRefresh
		' 
		Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRefresh.CausesValidation = True
		Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRefresh.Enabled = True
		Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRefresh.Location = New System.Drawing.Point(600, 152)
		Me.cmdRefresh.Name = "cmdRefresh"
		Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRefresh.Size = New System.Drawing.Size(73, 25)
		Me.cmdRefresh.TabIndex = 3
		Me.cmdRefresh.TabStop = True
		Me.cmdRefresh.Text = "&Refresh"
		Me.cmdRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(600, 184)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 25)
		Me.cmdAdd.TabIndex = 2
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "&Add"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(600, 216)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 25)
		Me.cmdCancel.TabIndex = 1
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' frmAddRelated
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(679, 246)
		Me.grdTransactions.Compatibility = Artinsoft.Windows.Forms.GridCompatibility.TrueDBGrid
		Me.ControlBox = True
		Me.Controls.Add(Me.fraFilter)
		Me.Controls.Add(Me.cmdRefresh)
		Me.Controls.Add(Me.cmdAdd)
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.grdTransactions)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.grdTransactions.Location = New System.Drawing.Point(8, 8)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmAddRelated"
		Me.grdTransactions.Name = "grdTransactions"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.grdTransactions.Size = New System.Drawing.Size(505, 233)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.grdTransactions.TabIndex = 0
		Me.Text = "Add Transaction Lines"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.ToolTip1.SetToolTip(Me.cmdAdd, "Adds the selected transaction lines into the allocation at their current o/s amount.")
		CType(Me.grdTransactions, System.ComponentModel.ISupportInitialize).EndInit()
		Me.fraFilter.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class