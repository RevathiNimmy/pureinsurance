<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChangeStatus
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
	Public WithEvents cboRenewalStatusType As PMLookupControl.cboPMLookup
	Public WithEvents lblRenewalStatusType As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cboRenewalStatusType = New PMLookupControl.cboPMLookup
        Me.lblRenewalStatusType = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(337, 40)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
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
        Me.cmdOK.Location = New System.Drawing.Point(256, 40)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cboRenewalStatusType
        '
        Me.cboRenewalStatusType.DefaultItemId = 0
        Me.cboRenewalStatusType.FirstItem = ""
        Me.cboRenewalStatusType.ItemId = 0
        Me.cboRenewalStatusType.ListIndex = -1
        Me.cboRenewalStatusType.Location = New System.Drawing.Point(116, 8)
        Me.cboRenewalStatusType.Name = "cboRenewalStatusType"
        Me.cboRenewalStatusType.PMLookupProductFamily = 9
        Me.cboRenewalStatusType.SingleItemId = 0
        Me.cboRenewalStatusType.Size = New System.Drawing.Size(295, 21)
        Me.cboRenewalStatusType.Sorted = True
        Me.cboRenewalStatusType.TabIndex = 0
        Me.cboRenewalStatusType.TableName = "renewal_status_type"
        Me.cboRenewalStatusType.ToolTipText = ""
        Me.cboRenewalStatusType.WhereClause = "Code <> 'BROKERXFER'"
        '
        'lblRenewalStatusType
        '
        Me.lblRenewalStatusType.BackColor = System.Drawing.Color.Transparent
        Me.lblRenewalStatusType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRenewalStatusType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenewalStatusType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRenewalStatusType.Location = New System.Drawing.Point(8, 11)
        Me.lblRenewalStatusType.Name = "lblRenewalStatusType"
        Me.lblRenewalStatusType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRenewalStatusType.Size = New System.Drawing.Size(101, 17)
        Me.lblRenewalStatusType.TabIndex = 3
        Me.lblRenewalStatusType.Text = "Renewal Status :"
        '
        'frmChangeStatus
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(417, 71)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cboRenewalStatusType)
        Me.Controls.Add(Me.lblRenewalStatusType)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmChangeStatus"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Renewal Status"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class