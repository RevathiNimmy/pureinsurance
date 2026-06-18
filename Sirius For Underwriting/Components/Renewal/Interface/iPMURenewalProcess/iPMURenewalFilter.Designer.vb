<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRenewalFilter
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializelblFilterCriteria()
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
	Public WithEvents txtAgent As System.Windows.Forms.TextBox
	Public WithEvents cmdAgent As System.Windows.Forms.Button
	Public WithEvents lblAgent As System.Windows.Forms.Label
	Public WithEvents fraAgentFilter As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents _stbRenewalFilter_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbRenewalFilter As System.Windows.Forms.StatusStrip
	Public WithEvents cboAgentCode As System.Windows.Forms.ComboBox
	Public WithEvents cboBranch As System.Windows.Forms.ComboBox
	Public WithEvents cboRenewalType As System.Windows.Forms.ComboBox
	Public WithEvents cboProductType As PMLookupControl.cboPMLookup
	Public WithEvents dtpRenewalDate As System.Windows.Forms.DateTimePicker
	Public WithEvents Label1 As System.Windows.Forms.Label
	Private WithEvents _lblFilterCriteria_3 As System.Windows.Forms.Label
	Private WithEvents _lblFilterCriteria_2 As System.Windows.Forms.Label
	Private WithEvents _lblFilterCriteria_1 As System.Windows.Forms.Label
	Private WithEvents _lblFilterCriteria_0 As System.Windows.Forms.Label
	Public WithEvents fraFilterCriteria As System.Windows.Forms.GroupBox
	Public WithEvents cmdPolicySearch As System.Windows.Forms.Button
	Public WithEvents txtPolicy As System.Windows.Forms.TextBox
	Public WithEvents lblPolicy As System.Windows.Forms.Label
	Public WithEvents fraPolicyFilter As System.Windows.Forms.GroupBox
	Public lblFilterCriteria(3) As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraAgentFilter = New System.Windows.Forms.GroupBox
        Me.txtAgent = New System.Windows.Forms.TextBox
        Me.cmdAgent = New System.Windows.Forms.Button
        Me.lblAgent = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.stbRenewalFilter = New System.Windows.Forms.StatusStrip
        Me._stbRenewalFilter_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.fraFilterCriteria = New System.Windows.Forms.GroupBox
        Me.cboAgentCode = New System.Windows.Forms.ComboBox
        Me.cboBranch = New System.Windows.Forms.ComboBox
        Me.cboRenewalType = New System.Windows.Forms.ComboBox
        Me.cboProductType = New PMLookupControl.cboPMLookup
        Me.dtpRenewalDate = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me._lblFilterCriteria_3 = New System.Windows.Forms.Label
        Me._lblFilterCriteria_2 = New System.Windows.Forms.Label
        Me._lblFilterCriteria_1 = New System.Windows.Forms.Label
        Me._lblFilterCriteria_0 = New System.Windows.Forms.Label
        Me.fraPolicyFilter = New System.Windows.Forms.GroupBox
        Me.cmdPolicySearch = New System.Windows.Forms.Button
        Me.txtPolicy = New System.Windows.Forms.TextBox
        Me.lblPolicy = New System.Windows.Forms.Label
        Me.fraAgentFilter.SuspendLayout()
        Me.stbRenewalFilter.SuspendLayout()
        Me.fraFilterCriteria.SuspendLayout()
        Me.fraPolicyFilter.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraAgentFilter
        '
        Me.fraAgentFilter.BackColor = System.Drawing.SystemColors.Control
        Me.fraAgentFilter.Controls.Add(Me.txtAgent)
        Me.fraAgentFilter.Controls.Add(Me.cmdAgent)
        Me.fraAgentFilter.Controls.Add(Me.lblAgent)
        Me.fraAgentFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAgentFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAgentFilter.Location = New System.Drawing.Point(3, 259)
        Me.fraAgentFilter.Name = "fraAgentFilter"
        Me.fraAgentFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAgentFilter.Size = New System.Drawing.Size(457, 73)
        Me.fraAgentFilter.TabIndex = 16
        Me.fraAgentFilter.TabStop = False
        Me.fraAgentFilter.Text = "Broker Transfer"
        '
        'txtAgent
        '
        Me.txtAgent.AcceptsReturn = True
        Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgent.Enabled = False
        Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgent.Location = New System.Drawing.Point(111, 27)
        Me.txtAgent.MaxLength = 0
        Me.txtAgent.Name = "txtAgent"
        Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgent.Size = New System.Drawing.Size(280, 20)
        Me.txtAgent.TabIndex = 18
        '
        'cmdAgent
        '
        Me.cmdAgent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgent.Location = New System.Drawing.Point(393, 27)
        Me.cmdAgent.Name = "cmdAgent"
        Me.cmdAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgent.Size = New System.Drawing.Size(25, 21)
        Me.cmdAgent.TabIndex = 17
        Me.cmdAgent.Text = "..."
        Me.cmdAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgent.UseVisualStyleBackColor = False
        '
        'lblAgent
        '
        Me.lblAgent.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgent.Location = New System.Drawing.Point(21, 30)
        Me.lblAgent.Name = "lblAgent"
        Me.lblAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgent.Size = New System.Drawing.Size(82, 16)
        Me.lblAgent.TabIndex = 19
        Me.lblAgent.Text = "Select Agent"
        Me.lblAgent.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(387, 336)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(309, 336)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'stbRenewalFilter
        '
        Me.stbRenewalFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbRenewalFilter.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbRenewalFilter_Panel1})
        Me.stbRenewalFilter.Location = New System.Drawing.Point(0, 366)
        Me.stbRenewalFilter.Name = "stbRenewalFilter"
        Me.stbRenewalFilter.ShowItemToolTips = True
        Me.stbRenewalFilter.Size = New System.Drawing.Size(463, 22)
        Me.stbRenewalFilter.TabIndex = 12
        '
        '_stbRenewalFilter_Panel1
        '
        Me._stbRenewalFilter_Panel1.AutoSize = False
        Me._stbRenewalFilter_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbRenewalFilter_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbRenewalFilter_Panel1.DoubleClickEnabled = True
        Me._stbRenewalFilter_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbRenewalFilter_Panel1.Name = "_stbRenewalFilter_Panel1"
        Me._stbRenewalFilter_Panel1.Size = New System.Drawing.Size(461, 22)
        Me._stbRenewalFilter_Panel1.Text = "Ready"
        Me._stbRenewalFilter_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'fraFilterCriteria
        '
        Me.fraFilterCriteria.BackColor = System.Drawing.SystemColors.Control
        Me.fraFilterCriteria.Controls.Add(Me.cboAgentCode)
        Me.fraFilterCriteria.Controls.Add(Me.cboBranch)
        Me.fraFilterCriteria.Controls.Add(Me.cboRenewalType)
        Me.fraFilterCriteria.Controls.Add(Me.cboProductType)
        Me.fraFilterCriteria.Controls.Add(Me.dtpRenewalDate)
        Me.fraFilterCriteria.Controls.Add(Me.Label1)
        Me.fraFilterCriteria.Controls.Add(Me._lblFilterCriteria_3)
        Me.fraFilterCriteria.Controls.Add(Me._lblFilterCriteria_2)
        Me.fraFilterCriteria.Controls.Add(Me._lblFilterCriteria_1)
        Me.fraFilterCriteria.Controls.Add(Me._lblFilterCriteria_0)
        Me.fraFilterCriteria.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraFilterCriteria.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraFilterCriteria.Location = New System.Drawing.Point(3, 84)
        Me.fraFilterCriteria.Name = "fraFilterCriteria"
        Me.fraFilterCriteria.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraFilterCriteria.Size = New System.Drawing.Size(457, 173)
        Me.fraFilterCriteria.TabIndex = 8
        Me.fraFilterCriteria.TabStop = False
        Me.fraFilterCriteria.Text = "Filter Criteria"
        '
        'cboAgentCode
        '
        Me.cboAgentCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboAgentCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAgentCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAgentCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAgentCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAgentCode.Location = New System.Drawing.Point(111, 136)
        Me.cboAgentCode.Name = "cboAgentCode"
        Me.cboAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAgentCode.Size = New System.Drawing.Size(279, 21)
        Me.cboAgentCode.TabIndex = 20
        '
        'cboBranch
        '
        Me.cboBranch.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranch.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranch.Location = New System.Drawing.Point(112, 77)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranch.Size = New System.Drawing.Size(280, 21)
        Me.cboBranch.TabIndex = 15
        '
        'cboRenewalType
        '
        Me.cboRenewalType.BackColor = System.Drawing.SystemColors.Window
        Me.cboRenewalType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRenewalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRenewalType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRenewalType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRenewalType.Location = New System.Drawing.Point(111, 107)
        Me.cboRenewalType.Name = "cboRenewalType"
        Me.cboRenewalType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRenewalType.Size = New System.Drawing.Size(280, 21)
        Me.cboRenewalType.TabIndex = 13
        '
        'cboProductType
        '
        Me.cboProductType.DefaultItemId = 0
        Me.cboProductType.FirstItem = ""
        Me.cboProductType.ItemId = 0
        Me.cboProductType.ListIndex = -1
        Me.cboProductType.Location = New System.Drawing.Point(111, 48)
        Me.cboProductType.Name = "cboProductType"
        Me.cboProductType.PMLookupProductFamily = 9
        Me.cboProductType.SingleItemId = 0
        Me.cboProductType.Size = New System.Drawing.Size(280, 21)
        Me.cboProductType.Sorted = True
        Me.cboProductType.TabIndex = 3
        Me.cboProductType.TableName = "Product"
        Me.cboProductType.ToolTipText = ""
        Me.cboProductType.WhereClause = " is_renewable=1"
        '
        'dtpRenewalDate
        '
        Me.dtpRenewalDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpRenewalDate.Location = New System.Drawing.Point(111, 18)
        Me.dtpRenewalDate.Name = "dtpRenewalDate"
        Me.dtpRenewalDate.Size = New System.Drawing.Size(280, 20)
        Me.dtpRenewalDate.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(32, 140)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "Agent Code"
        '
        '_lblFilterCriteria_3
        '
        Me._lblFilterCriteria_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblFilterCriteria_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFilterCriteria_3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFilterCriteria_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFilterCriteria_3.Location = New System.Drawing.Point(24, 111)
        Me._lblFilterCriteria_3.Name = "_lblFilterCriteria_3"
        Me._lblFilterCriteria_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFilterCriteria_3.Size = New System.Drawing.Size(79, 16)
        Me._lblFilterCriteria_3.TabIndex = 14
        Me._lblFilterCriteria_3.Text = "Renewal Type"
        Me._lblFilterCriteria_3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblFilterCriteria_2
        '
        Me._lblFilterCriteria_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblFilterCriteria_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFilterCriteria_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFilterCriteria_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFilterCriteria_2.Location = New System.Drawing.Point(24, 81)
        Me._lblFilterCriteria_2.Name = "_lblFilterCriteria_2"
        Me._lblFilterCriteria_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFilterCriteria_2.Size = New System.Drawing.Size(79, 16)
        Me._lblFilterCriteria_2.TabIndex = 11
        Me._lblFilterCriteria_2.Text = "Branch Code"
        Me._lblFilterCriteria_2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblFilterCriteria_1
        '
        Me._lblFilterCriteria_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblFilterCriteria_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFilterCriteria_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFilterCriteria_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFilterCriteria_1.Location = New System.Drawing.Point(24, 51)
        Me._lblFilterCriteria_1.Name = "_lblFilterCriteria_1"
        Me._lblFilterCriteria_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFilterCriteria_1.Size = New System.Drawing.Size(79, 16)
        Me._lblFilterCriteria_1.TabIndex = 10
        Me._lblFilterCriteria_1.Text = "Product Type"
        Me._lblFilterCriteria_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblFilterCriteria_0
        '
        Me._lblFilterCriteria_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblFilterCriteria_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblFilterCriteria_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblFilterCriteria_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblFilterCriteria_0.Location = New System.Drawing.Point(24, 21)
        Me._lblFilterCriteria_0.Name = "_lblFilterCriteria_0"
        Me._lblFilterCriteria_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblFilterCriteria_0.Size = New System.Drawing.Size(79, 16)
        Me._lblFilterCriteria_0.TabIndex = 9
        Me._lblFilterCriteria_0.Text = "Renewal Date"
        Me._lblFilterCriteria_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'fraPolicyFilter
        '
        Me.fraPolicyFilter.BackColor = System.Drawing.SystemColors.Control
        Me.fraPolicyFilter.Controls.Add(Me.cmdPolicySearch)
        Me.fraPolicyFilter.Controls.Add(Me.txtPolicy)
        Me.fraPolicyFilter.Controls.Add(Me.lblPolicy)
        Me.fraPolicyFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPolicyFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPolicyFilter.Location = New System.Drawing.Point(3, 9)
        Me.fraPolicyFilter.Name = "fraPolicyFilter"
        Me.fraPolicyFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPolicyFilter.Size = New System.Drawing.Size(457, 73)
        Me.fraPolicyFilter.TabIndex = 6
        Me.fraPolicyFilter.TabStop = False
        Me.fraPolicyFilter.Text = "By Policy"
        '
        'cmdPolicySearch
        '
        Me.cmdPolicySearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPolicySearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPolicySearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPolicySearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPolicySearch.Location = New System.Drawing.Point(393, 27)
        Me.cmdPolicySearch.Name = "cmdPolicySearch"
        Me.cmdPolicySearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPolicySearch.Size = New System.Drawing.Size(25, 21)
        Me.cmdPolicySearch.TabIndex = 1
        Me.cmdPolicySearch.Text = "..."
        Me.cmdPolicySearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPolicySearch.UseVisualStyleBackColor = False
        '
        'txtPolicy
        '
        Me.txtPolicy.AcceptsReturn = True
        Me.txtPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicy.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicy.Location = New System.Drawing.Point(111, 27)
        Me.txtPolicy.MaxLength = 0
        Me.txtPolicy.Name = "txtPolicy"
        Me.txtPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicy.Size = New System.Drawing.Size(280, 20)
        Me.txtPolicy.TabIndex = 0
        '
        'lblPolicy
        '
        Me.lblPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicy.Location = New System.Drawing.Point(21, 30)
        Me.lblPolicy.Name = "lblPolicy"
        Me.lblPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicy.Size = New System.Drawing.Size(82, 16)
        Me.lblPolicy.TabIndex = 7
        Me.lblPolicy.Text = "Select Policy"
        Me.lblPolicy.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmRenewalFilter
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(463, 388)
        Me.Controls.Add(Me.fraAgentFilter)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.stbRenewalFilter)
        Me.Controls.Add(Me.fraFilterCriteria)
        Me.Controls.Add(Me.fraPolicyFilter)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRenewalFilter"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Renewal Filter"
        Me.fraAgentFilter.ResumeLayout(False)
        Me.fraAgentFilter.PerformLayout()
        Me.stbRenewalFilter.ResumeLayout(False)
        Me.stbRenewalFilter.PerformLayout()
        Me.fraFilterCriteria.ResumeLayout(False)
        Me.fraFilterCriteria.PerformLayout()
        Me.fraPolicyFilter.ResumeLayout(False)
        Me.fraPolicyFilter.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializelblFilterCriteria()
		Me.lblFilterCriteria(3) = _lblFilterCriteria_3
		Me.lblFilterCriteria(2) = _lblFilterCriteria_2
		Me.lblFilterCriteria(1) = _lblFilterCriteria_1
		Me.lblFilterCriteria(0) = _lblFilterCriteria_0
	End Sub
#End Region 
End Class