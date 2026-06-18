<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdjustment
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializelblAdjustment()
		InitializefraAdjustment()
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
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtCashDrawer As System.Windows.Forms.TextBox
	Private WithEvents _lblAdjustment_0 As System.Windows.Forms.Label
	Private WithEvents _fraAdjustment_0 As System.Windows.Forms.GroupBox
	Public WithEvents cboPMUserLookup1 As PMUserLookupControl.cboPMUserLookup
	Public WithEvents txtComments As System.Windows.Forms.TextBox
	Public WithEvents cboMethod As System.Windows.Forms.ComboBox
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents txtAdjDate As System.Windows.Forms.TextBox
	Public WithEvents lblComments As System.Windows.Forms.Label
	Public WithEvents lblMethod As System.Windows.Forms.Label
	Public WithEvents lblAmount As System.Windows.Forms.Label
	Public WithEvents lblAdjDate As System.Windows.Forms.Label
	Private WithEvents _lblAdjustment_1 As System.Windows.Forms.Label
	Private WithEvents _fraAdjustment_1 As System.Windows.Forms.GroupBox
	Private WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public fraAdjustment(1) As System.Windows.Forms.GroupBox
	Public lblAdjustment(1) As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdjustment))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me._fraAdjustment_0 = New System.Windows.Forms.GroupBox
        Me.txtCashDrawer = New System.Windows.Forms.TextBox
        Me._lblAdjustment_0 = New System.Windows.Forms.Label
        Me._fraAdjustment_1 = New System.Windows.Forms.GroupBox
        Me.cboPMUserLookup1 = New PMUserLookupControl.cboPMUserLookup
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.cboMethod = New System.Windows.Forms.ComboBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.txtAdjDate = New System.Windows.Forms.TextBox
        Me.lblComments = New System.Windows.Forms.Label
        Me.lblMethod = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.lblAdjDate = New System.Windows.Forms.Label
        Me._lblAdjustment_1 = New System.Windows.Forms.Label
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me._fraAdjustment_0.SuspendLayout()
        Me._fraAdjustment_1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(272, 344)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.Tag = "CAP;550"
        Me.cmdOK.Text = "*{OK}"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(123, 18)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Multiline = True
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(501, 333)
        Me.SSTab1.TabIndex = 12
        Me.SSTab1.Tag = "CAP;501"
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me._fraAdjustment_0)
        Me._SSTab1_TabPage0.Controls.Add(Me._fraAdjustment_1)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(493, 307)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "*{1 - Summary}"
        '
        '_fraAdjustment_0
        '
        Me._fraAdjustment_0.BackColor = System.Drawing.SystemColors.Control
        Me._fraAdjustment_0.Controls.Add(Me.txtCashDrawer)
        Me._fraAdjustment_0.Controls.Add(Me._lblAdjustment_0)
        Me._fraAdjustment_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraAdjustment_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraAdjustment_0.Location = New System.Drawing.Point(16, 12)
        Me._fraAdjustment_0.Name = "_fraAdjustment_0"
        Me._fraAdjustment_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraAdjustment_0.Size = New System.Drawing.Size(465, 65)
        Me._fraAdjustment_0.TabIndex = 6
        Me._fraAdjustment_0.TabStop = False
        Me._fraAdjustment_0.Tag = "CAP;510"
        Me._fraAdjustment_0.Text = "*{Drawer Information}"
        '
        'txtCashDrawer
        '
        Me.txtCashDrawer.AcceptsReturn = True
        Me.txtCashDrawer.BackColor = System.Drawing.SystemColors.Window
        Me.txtCashDrawer.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCashDrawer.Enabled = False
        Me.txtCashDrawer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCashDrawer.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCashDrawer.Location = New System.Drawing.Point(128, 24)
        Me.txtCashDrawer.MaxLength = 0
        Me.txtCashDrawer.Name = "txtCashDrawer"
        Me.txtCashDrawer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCashDrawer.Size = New System.Drawing.Size(321, 19)
        Me.txtCashDrawer.TabIndex = 0
        '
        '_lblAdjustment_0
        '
        Me._lblAdjustment_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblAdjustment_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAdjustment_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAdjustment_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblAdjustment_0.Location = New System.Drawing.Point(16, 28)
        Me._lblAdjustment_0.Name = "_lblAdjustment_0"
        Me._lblAdjustment_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAdjustment_0.Size = New System.Drawing.Size(109, 17)
        Me._lblAdjustment_0.TabIndex = 13
        Me._lblAdjustment_0.Tag = "CAP;512"
        Me._lblAdjustment_0.Text = "*{Cash Drawer:}"
        '
        '_fraAdjustment_1
        '
        Me._fraAdjustment_1.BackColor = System.Drawing.SystemColors.Control
        Me._fraAdjustment_1.Controls.Add(Me.cboPMUserLookup1)
        Me._fraAdjustment_1.Controls.Add(Me.txtComments)
        Me._fraAdjustment_1.Controls.Add(Me.cboMethod)
        Me._fraAdjustment_1.Controls.Add(Me.txtAmount)
        Me._fraAdjustment_1.Controls.Add(Me.txtAdjDate)
        Me._fraAdjustment_1.Controls.Add(Me.lblComments)
        Me._fraAdjustment_1.Controls.Add(Me.lblMethod)
        Me._fraAdjustment_1.Controls.Add(Me.lblAmount)
        Me._fraAdjustment_1.Controls.Add(Me.lblAdjDate)
        Me._fraAdjustment_1.Controls.Add(Me._lblAdjustment_1)
        Me._fraAdjustment_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraAdjustment_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._fraAdjustment_1.Location = New System.Drawing.Point(16, 84)
        Me._fraAdjustment_1.Name = "_fraAdjustment_1"
        Me._fraAdjustment_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraAdjustment_1.Size = New System.Drawing.Size(465, 209)
        Me._fraAdjustment_1.TabIndex = 7
        Me._fraAdjustment_1.TabStop = False
        Me._fraAdjustment_1.Tag = "CAP;511"
        Me._fraAdjustment_1.Text = "*{Adjustment}"
        '
        'cboPMUserLookup1
        '
        Me.cboPMUserLookup1.DefaultUserID = 0
        Me.cboPMUserLookup1.FirstItem = ""
        Me.cboPMUserLookup1.ListIndex = -1
        Me.cboPMUserLookup1.Location = New System.Drawing.Point(128, 24)
        Me.cboPMUserLookup1.Name = "cboPMUserLookup1"
        Me.cboPMUserLookup1.PMUserGroupID = 0
        Me.cboPMUserLookup1.SingleUserID = 0
        Me.cboPMUserLookup1.Size = New System.Drawing.Size(153, 21)
        Me.cboPMUserLookup1.Sorted = True
        Me.cboPMUserLookup1.TabIndex = 1
        Me.cboPMUserLookup1.ToolTipText = ""
        Me.cboPMUserLookup1.UserID = 0
        '
        'txtComments
        '
        Me.txtComments.AcceptsReturn = True
        Me.txtComments.BackColor = System.Drawing.SystemColors.Window
        Me.txtComments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtComments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtComments.Location = New System.Drawing.Point(128, 144)
        Me.txtComments.MaxLength = 1000
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtComments.Size = New System.Drawing.Size(321, 51)
        Me.txtComments.TabIndex = 5
        Me.txtComments.Tag = "F;M;"
        '
        'cboMethod
        '
        Me.cboMethod.BackColor = System.Drawing.SystemColors.Window
        Me.cboMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMethod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMethod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMethod.Location = New System.Drawing.Point(128, 114)
        Me.cboMethod.Name = "cboMethod"
        Me.cboMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMethod.Size = New System.Drawing.Size(153, 21)
        Me.cboMethod.TabIndex = 4
        Me.cboMethod.Tag = "F;M;"
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(128, 84)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(153, 21)
        Me.txtAmount.TabIndex = 3
        Me.txtAmount.Tag = "F;M;$;"
        Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjDate
        '
        Me.txtAdjDate.AcceptsReturn = True
        Me.txtAdjDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtAdjDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAdjDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAdjDate.Location = New System.Drawing.Point(128, 54)
        Me.txtAdjDate.MaxLength = 0
        Me.txtAdjDate.Name = "txtAdjDate"
        Me.txtAdjDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAdjDate.Size = New System.Drawing.Size(153, 21)
        Me.txtAdjDate.TabIndex = 2
        Me.txtAdjDate.Tag = "F;M;DT;"
        '
        'lblComments
        '
        Me.lblComments.BackColor = System.Drawing.SystemColors.Control
        Me.lblComments.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComments.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComments.Location = New System.Drawing.Point(16, 148)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComments.Size = New System.Drawing.Size(117, 17)
        Me.lblComments.TabIndex = 18
        Me.lblComments.Tag = "CAP;517"
        Me.lblComments.Text = "*{Comments:}"
        '
        'lblMethod
        '
        Me.lblMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMethod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMethod.Location = New System.Drawing.Point(16, 118)
        Me.lblMethod.Name = "lblMethod"
        Me.lblMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMethod.Size = New System.Drawing.Size(117, 17)
        Me.lblMethod.TabIndex = 17
        Me.lblMethod.Tag = "CAP;516"
        Me.lblMethod.Text = "*{Method:}"
        '
        'lblAmount
        '
        Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(16, 88)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(117, 17)
        Me.lblAmount.TabIndex = 16
        Me.lblAmount.Tag = "CAP;515"
        Me.lblAmount.Text = "*{Amount:}"
        '
        'lblAdjDate
        '
        Me.lblAdjDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblAdjDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAdjDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdjDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAdjDate.Location = New System.Drawing.Point(16, 58)
        Me.lblAdjDate.Name = "lblAdjDate"
        Me.lblAdjDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAdjDate.Size = New System.Drawing.Size(125, 17)
        Me.lblAdjDate.TabIndex = 15
        Me.lblAdjDate.Tag = "CAP;514"
        Me.lblAdjDate.Text = "*{Adjustment Date:}"
        '
        '_lblAdjustment_1
        '
        Me._lblAdjustment_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblAdjustment_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblAdjustment_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblAdjustment_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblAdjustment_1.Location = New System.Drawing.Point(16, 28)
        Me._lblAdjustment_1.Name = "_lblAdjustment_1"
        Me._lblAdjustment_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblAdjustment_1.Size = New System.Drawing.Size(117, 17)
        Me._lblAdjustment_1.TabIndex = 14
        Me._lblAdjustment_1.Tag = "CAP;513"
        Me._lblAdjustment_1.Text = "*{User:}"
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 344)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(81, 22)
        Me.cmdNavigate.TabIndex = 8
        Me.cmdNavigate.Tag = "CAP;203"
        Me.cmdNavigate.Text = "*{Navigate}"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(432, 344)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 11
        Me.cmdHelp.Tag = "CAP;552"
        Me.cmdHelp.Text = "*{Help}"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(352, 344)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 10
        Me.cmdCancel.Tag = "CAP;551"
        Me.cmdCancel.Text = "*{Cancel}"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'frmAdjustment
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(512, 373)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAdjustment"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Tag = "CAP;553"
        Me.Text = "*{Title}"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._fraAdjustment_0.ResumeLayout(False)
        Me._fraAdjustment_1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	Sub InitializelblAdjustment()
		Me.lblAdjustment(1) = _lblAdjustment_1
		Me.lblAdjustment(0) = _lblAdjustment_0
	End Sub
	Sub InitializefraAdjustment()
		Me.fraAdjustment(1) = _fraAdjustment_1
		Me.fraAdjustment(0) = _fraAdjustment_0
	End Sub
#End Region 
End Class