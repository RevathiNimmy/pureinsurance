<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWriteOffReason
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
	Public WithEvents Cancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cboWriteOffReasonID As PMLookupControl.cboPMLookup
	Public WithEvents lblMessage As System.Windows.Forms.Label
	Public WithEvents lblWriteOffReasonID As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Cancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cboWriteOffReasonID = New PMLookupControl.cboPMLookup()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.lblWriteOffReasonID = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Cancel
        '
        Me.Cancel.BackColor = System.Drawing.SystemColors.Control
        Me.Cancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.Cancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Cancel.Location = New System.Drawing.Point(252, 118)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Cancel.Size = New System.Drawing.Size(77, 23)
        Me.Cancel.TabIndex = 4
        Me.Cancel.Text = "&Cancel"
        Me.Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Cancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(170, 118)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(77, 23)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cboWriteOffReasonID
        '
        Me.cboWriteOffReasonID.DefaultItemId = 0
        Me.cboWriteOffReasonID.FirstItem = ""
        Me.cboWriteOffReasonID.ItemId = 0
        Me.cboWriteOffReasonID.ListIndex = -1
        Me.cboWriteOffReasonID.Location = New System.Drawing.Point(122, 86)
        Me.cboWriteOffReasonID.Name = "cboWriteOffReasonID"
        Me.cboWriteOffReasonID.PMLookupProductFamily = 1
        Me.cboWriteOffReasonID.SingleItemId = 0
        Me.cboWriteOffReasonID.Size = New System.Drawing.Size(205, 21)
        Me.cboWriteOffReasonID.SortColumnName = ""
        Me.cboWriteOffReasonID.Sorted = True
        Me.cboWriteOffReasonID.TabIndex = 0
        Me.cboWriteOffReasonID.TableName = "Write_Off_Reason"
        Me.cboWriteOffReasonID.ToolTipText = ""
        Me.cboWriteOffReasonID.WhereClause = "Is_Only_Valid_For_Instalment=0"
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
        Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMessage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMessage.Location = New System.Drawing.Point(8, 12)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMessage.Size = New System.Drawing.Size(319, 67)
        Me.lblMessage.TabIndex = 2
        Me.lblMessage.Text = "Message"
        '
        'lblWriteOffReasonID
        '
        Me.lblWriteOffReasonID.BackColor = System.Drawing.SystemColors.Control
        Me.lblWriteOffReasonID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWriteOffReasonID.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWriteOffReasonID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWriteOffReasonID.Location = New System.Drawing.Point(10, 90)
        Me.lblWriteOffReasonID.Name = "lblWriteOffReasonID"
        Me.lblWriteOffReasonID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWriteOffReasonID.Size = New System.Drawing.Size(125, 17)
        Me.lblWriteOffReasonID.TabIndex = 1
        Me.lblWriteOffReasonID.Text = "Write-Off Reason:"
        '
        'frmWriteOffReason
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(337, 150)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cboWriteOffReasonID)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.lblWriteOffReasonID)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWriteOffReason"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Auto-Allocate"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class