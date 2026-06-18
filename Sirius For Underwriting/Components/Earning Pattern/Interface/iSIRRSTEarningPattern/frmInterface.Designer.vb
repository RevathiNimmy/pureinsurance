<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents txtEffectiveDate As System.Windows.Forms.DateTimePicker
	Public WithEvents cboEarningPattern As PMLookupControl.cboPMLookup
	Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
	Public WithEvents lblEarningPattern As System.Windows.Forms.Label
	Public WithEvents fraEarningPatternDetails As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.fraEarningPatternDetails = New System.Windows.Forms.GroupBox
        Me.txtEffectiveDate = New System.Windows.Forms.DateTimePicker
        Me.cboEarningPattern = New PMLookupControl.cboPMLookup
        Me.lblEffectiveDate = New System.Windows.Forms.Label
        Me.lblEarningPattern = New System.Windows.Forms.Label
        Me.fraEarningPatternDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(280, 120)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 25)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(200, 120)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 25)
        Me.cmdOk.TabIndex = 2
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'fraEarningPatternDetails
        '
        Me.fraEarningPatternDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraEarningPatternDetails.Controls.Add(Me.txtEffectiveDate)
        Me.fraEarningPatternDetails.Controls.Add(Me.cboEarningPattern)
        Me.fraEarningPatternDetails.Controls.Add(Me.lblEffectiveDate)
        Me.fraEarningPatternDetails.Controls.Add(Me.lblEarningPattern)
        Me.fraEarningPatternDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEarningPatternDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraEarningPatternDetails.Location = New System.Drawing.Point(8, 8)
        Me.fraEarningPatternDetails.Name = "fraEarningPatternDetails"
        Me.fraEarningPatternDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraEarningPatternDetails.Size = New System.Drawing.Size(345, 105)
        Me.fraEarningPatternDetails.TabIndex = 4
        Me.fraEarningPatternDetails.TabStop = False
        Me.fraEarningPatternDetails.Text = "Earning Pattern Details"
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtEffectiveDate.Location = New System.Drawing.Point(160, 64)
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.Size = New System.Drawing.Size(169, 20)
        Me.txtEffectiveDate.TabIndex = 1
        '
        'cboEarningPattern
        '
        Me.cboEarningPattern.DefaultItemId = 1
        Me.cboEarningPattern.FirstItem = ""
        Me.cboEarningPattern.ItemId = 0
        Me.cboEarningPattern.ListIndex = -1
        Me.cboEarningPattern.Location = New System.Drawing.Point(160, 32)
        Me.cboEarningPattern.Name = "cboEarningPattern"
        Me.cboEarningPattern.PMLookupProductFamily = 1
        Me.cboEarningPattern.SingleItemId = 0
        Me.cboEarningPattern.Size = New System.Drawing.Size(169, 21)
        Me.cboEarningPattern.Sorted = True
        Me.cboEarningPattern.TabIndex = 0
        Me.cboEarningPattern.TableName = "Earning_Pattern"
        Me.cboEarningPattern.ToolTipText = ""
        Me.cboEarningPattern.WhereClause = ""
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(16, 66)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(137, 17)
        Me.lblEffectiveDate.TabIndex = 6
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'lblEarningPattern
        '
        Me.lblEarningPattern.BackColor = System.Drawing.SystemColors.Control
        Me.lblEarningPattern.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEarningPattern.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEarningPattern.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEarningPattern.Location = New System.Drawing.Point(16, 34)
        Me.lblEarningPattern.Name = "lblEarningPattern"
        Me.lblEarningPattern.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEarningPattern.Size = New System.Drawing.Size(137, 17)
        Me.lblEarningPattern.TabIndex = 5
        Me.lblEarningPattern.Text = "Earning Pattern:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(364, 152)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.fraEarningPatternDetails)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Earning Pattern"
        Me.fraEarningPatternDetails.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class