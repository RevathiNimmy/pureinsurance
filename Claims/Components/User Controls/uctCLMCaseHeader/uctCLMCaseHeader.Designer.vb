<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctCLMCaseHeader
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		UserControl_Initialize()
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
	Friend WithEvents txtCaseNumber As System.Windows.Forms.TextBox
	Friend WithEvents txtCaseVersion As System.Windows.Forms.TextBox
	Friend WithEvents cboAssistant As PMLookupControl.cboPMLookup
	Friend WithEvents cboAnalyst As PMLookupControl.cboPMLookup
	Friend WithEvents cboCaseProgressStatus As PMLookupControl.cboPMLookup
	Friend WithEvents cboCaseOpenDate As System.Windows.Forms.DateTimePicker
	Friend WithEvents lblCaseNumber As System.Windows.Forms.Label
	Friend WithEvents lblCaseOpenDate As System.Windows.Forms.Label
	Friend WithEvents lblCaseProgressStatus As System.Windows.Forms.Label
	Friend WithEvents lblAnalyst As System.Windows.Forms.Label
	Friend WithEvents lblAssistant As System.Windows.Forms.Label
	Friend WithEvents lblCaseVersion As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctCLMCaseHeader))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.txtCaseNumber = New System.Windows.Forms.TextBox
		Me.txtCaseVersion = New System.Windows.Forms.TextBox
		Me.cboAssistant = New PMLookupControl.cboPMLookup
		Me.cboAnalyst = New PMLookupControl.cboPMLookup
		Me.cboCaseProgressStatus = New PMLookupControl.cboPMLookup
		Me.cboCaseOpenDate = New System.Windows.Forms.DateTimePicker
		Me.lblCaseNumber = New System.Windows.Forms.Label
		Me.lblCaseOpenDate = New System.Windows.Forms.Label
		Me.lblCaseProgressStatus = New System.Windows.Forms.Label
		Me.lblAnalyst = New System.Windows.Forms.Label
		Me.lblAssistant = New System.Windows.Forms.Label
		Me.lblCaseVersion = New System.Windows.Forms.Label
		Me.SuspendLayout()
		' 
		' txtCaseNumber
        'developer guide no.
        '
        Me.txtCaseNumber.AcceptsReturn = True
        Me.txtCaseNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaseNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCaseNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaseNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCaseNumber.Location = New System.Drawing.Point(144, 8)
        Me.txtCaseNumber.MaxLength = 0
        Me.txtCaseNumber.Name = "txtCaseNumber"
        Me.txtCaseNumber.ReadOnly = True
        Me.txtCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCaseNumber.Size = New System.Drawing.Size(163, 21)
        Me.txtCaseNumber.TabIndex = 5
		' 
		' txtCaseVersion
		' 
		Me.txtCaseVersion.AcceptsReturn = True
		Me.txtCaseVersion.AutoSize = False
		Me.txtCaseVersion.BackColor = System.Drawing.SystemColors.InactiveBorder
		Me.txtCaseVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCaseVersion.CausesValidation = True
		Me.txtCaseVersion.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCaseVersion.Enabled = True
		Me.txtCaseVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtCaseVersion.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCaseVersion.HideSelection = True
		Me.txtCaseVersion.Location = New System.Drawing.Point(408, 61)
		Me.txtCaseVersion.MaxLength = 0
		Me.txtCaseVersion.Multiline = False
		Me.txtCaseVersion.Name = "txtCaseVersion"
		Me.txtCaseVersion.ReadOnly = True
		Me.txtCaseVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCaseVersion.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCaseVersion.Size = New System.Drawing.Size(85, 21)
		Me.txtCaseVersion.TabIndex = 0
		Me.txtCaseVersion.TabStop = True
		Me.txtCaseVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCaseVersion.Visible = True
		' 
		' cboAssistant
		' 
		Me.cboAssistant.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboAssistant.Location = New System.Drawing.Point(408, 35)
		Me.cboAssistant.Name = "cboAssistant"
		Me.cboAssistant.Size = New System.Drawing.Size(157, 21)
        'developer guide no.
        Me.cboAssistant.TabIndex = 1
        'developer guide no.77
        Me.cboAssistant.TableName = "Handler"
		Me.cboAssistant.WhereClause = "ISNULL(is_analyst,0)=0"
		' 
		' cboAnalyst
		' 
		Me.cboAnalyst.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboAnalyst.Location = New System.Drawing.Point(408, 8)
		Me.cboAnalyst.Name = "cboAnalyst"
		Me.cboAnalyst.Size = New System.Drawing.Size(157, 21)
        'developer guide no.
		Me.cboAnalyst.TabIndex = 2
        'developer guide no.77
        Me.cboAnalyst.TableName = "Handler"
		Me.cboAnalyst.WhereClause = "is_analyst=1"
		' 
		' cboCaseProgressStatus
		' 
		Me.cboCaseProgressStatus.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboCaseProgressStatus.Location = New System.Drawing.Point(144, 61)
		Me.cboCaseProgressStatus.Name = "cboCaseProgressStatus"
		Me.cboCaseProgressStatus.Size = New System.Drawing.Size(163, 21)
        'developer guide no.
        Me.cboCaseProgressStatus.TabIndex = 3
        'developer guide no.77
        Me.cboCaseProgressStatus.TableName = "case_progress"
		' 
		' cboCaseOpenDate
		' 
		Me.cboCaseOpenDate.Location = New System.Drawing.Point(144, 35)
		Me.cboCaseOpenDate.Name = "cboCaseOpenDate"
		Me.cboCaseOpenDate.Size = New System.Drawing.Size(163, 21)
		Me.cboCaseOpenDate.TabIndex = 4
		' 
		' lblCaseNumber
		' 
		Me.lblCaseNumber.AutoSize = False
		Me.lblCaseNumber.BackColor = System.Drawing.SystemColors.Control
		Me.lblCaseNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCaseNumber.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCaseNumber.Enabled = True
		Me.lblCaseNumber.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCaseNumber.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCaseNumber.Location = New System.Drawing.Point(8, 9)
		Me.lblCaseNumber.Name = "lblCaseNumber"
		Me.lblCaseNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCaseNumber.Size = New System.Drawing.Size(97, 17)
		Me.lblCaseNumber.TabIndex = 11
		Me.lblCaseNumber.Text = "Case Number:"
		Me.lblCaseNumber.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCaseNumber.UseMnemonic = True
		Me.lblCaseNumber.Visible = True
		' 
		' lblCaseOpenDate
		' 
		Me.lblCaseOpenDate.AutoSize = True
		Me.lblCaseOpenDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblCaseOpenDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCaseOpenDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCaseOpenDate.Enabled = True
		Me.lblCaseOpenDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCaseOpenDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCaseOpenDate.Location = New System.Drawing.Point(8, 36)
		Me.lblCaseOpenDate.Name = "lblCaseOpenDate"
		Me.lblCaseOpenDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCaseOpenDate.Size = New System.Drawing.Size(99, 13)
		Me.lblCaseOpenDate.TabIndex = 10
		Me.lblCaseOpenDate.Text = "Case Open Date:"
		Me.lblCaseOpenDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCaseOpenDate.UseMnemonic = True
		Me.lblCaseOpenDate.Visible = True
		' 
		' lblCaseProgressStatus
		' 
		Me.lblCaseProgressStatus.AutoSize = True
		Me.lblCaseProgressStatus.BackColor = System.Drawing.SystemColors.Control
		Me.lblCaseProgressStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCaseProgressStatus.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCaseProgressStatus.Enabled = True
		Me.lblCaseProgressStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCaseProgressStatus.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCaseProgressStatus.Location = New System.Drawing.Point(8, 63)
		Me.lblCaseProgressStatus.Name = "lblCaseProgressStatus"
		Me.lblCaseProgressStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCaseProgressStatus.Size = New System.Drawing.Size(128, 13)
		Me.lblCaseProgressStatus.TabIndex = 9
		Me.lblCaseProgressStatus.Text = "Case Progress Status:"
		Me.lblCaseProgressStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCaseProgressStatus.UseMnemonic = True
		Me.lblCaseProgressStatus.Visible = True
		' 
		' lblAnalyst
		' 
		Me.lblAnalyst.AutoSize = True
		Me.lblAnalyst.BackColor = System.Drawing.SystemColors.Control
		Me.lblAnalyst.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAnalyst.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAnalyst.Enabled = True
		Me.lblAnalyst.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAnalyst.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAnalyst.Location = New System.Drawing.Point(320, 9)
		Me.lblAnalyst.Name = "lblAnalyst"
		Me.lblAnalyst.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAnalyst.Size = New System.Drawing.Size(47, 13)
		Me.lblAnalyst.TabIndex = 8
		Me.lblAnalyst.Text = "Analyst:"
		Me.lblAnalyst.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAnalyst.UseMnemonic = True
		Me.lblAnalyst.Visible = True
		' 
		' lblAssistant
		' 
		Me.lblAssistant.AutoSize = True
		Me.lblAssistant.BackColor = System.Drawing.SystemColors.Control
		Me.lblAssistant.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAssistant.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAssistant.Enabled = True
		Me.lblAssistant.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblAssistant.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAssistant.Location = New System.Drawing.Point(320, 36)
		Me.lblAssistant.Name = "lblAssistant"
		Me.lblAssistant.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAssistant.Size = New System.Drawing.Size(56, 13)
		Me.lblAssistant.TabIndex = 7
		Me.lblAssistant.Text = "Assistant:"
		Me.lblAssistant.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAssistant.UseMnemonic = True
		Me.lblAssistant.Visible = True
		' 
		' lblCaseVersion
		' 
		Me.lblCaseVersion.AutoSize = True
		Me.lblCaseVersion.BackColor = System.Drawing.SystemColors.Control
		Me.lblCaseVersion.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCaseVersion.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCaseVersion.Enabled = True
		Me.lblCaseVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblCaseVersion.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCaseVersion.Location = New System.Drawing.Point(320, 63)
		Me.lblCaseVersion.Name = "lblCaseVersion"
		Me.lblCaseVersion.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCaseVersion.Size = New System.Drawing.Size(81, 13)
		Me.lblCaseVersion.TabIndex = 6
		Me.lblCaseVersion.Text = "Case Version:"
		Me.lblCaseVersion.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCaseVersion.UseMnemonic = True
		Me.lblCaseVersion.Visible = True
		' 
		' uctCLMCaseHeader
		' 
		Me.ClientSize = New System.Drawing.Size(568, 88)
		Me.Controls.Add(Me.txtCaseNumber)
		Me.Controls.Add(Me.txtCaseVersion)
		Me.Controls.Add(Me.cboAssistant)
		Me.Controls.Add(Me.cboAnalyst)
		Me.Controls.Add(Me.cboCaseProgressStatus)
		Me.Controls.Add(Me.cboCaseOpenDate)
		Me.Controls.Add(Me.lblCaseNumber)
		Me.Controls.Add(Me.lblCaseOpenDate)
		Me.Controls.Add(Me.lblCaseProgressStatus)
		Me.Controls.Add(Me.lblAnalyst)
		Me.Controls.Add(Me.lblAssistant)
		Me.Controls.Add(Me.lblCaseVersion)
		MyBase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctCLMCaseHeader"
		Me.ResumeLayout(False)
	End Sub
#End Region 
End Class