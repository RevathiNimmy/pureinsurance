<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpdateMediaTypeStatus
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
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtCommments As System.Windows.Forms.TextBox
    Public WithEvents txtUpdateDate As System.Windows.Forms.TextBox
    Public WithEvents cboMediaTypeStatus As PMLookupControl.cboPMLookup
    Public WithEvents lblAmount As System.Windows.Forms.Label
    Public WithEvents lblBankAccountNo As System.Windows.Forms.Label
    Public WithEvents lblPolicyHolder As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.fraCtlCancelPayment = New System.Windows.Forms.GroupBox
        Me.txtCommments = New System.Windows.Forms.TextBox
        Me.txtUpdateDate = New System.Windows.Forms.TextBox
        Me.cboMediaTypeStatus = New PMLookupControl.cboPMLookup
        Me.lblAmount = New System.Windows.Forms.Label
        Me.lblBankAccountNo = New System.Windows.Forms.Label
        Me.lblPolicyHolder = New System.Windows.Forms.Label
        Me.fraCtlCancelPayment.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(246, 144)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(324, 144)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'fraCtlCancelPayment
        '
        Me.fraCtlCancelPayment.BackColor = System.Drawing.SystemColors.Control
        Me.fraCtlCancelPayment.Controls.Add(Me.txtCommments)
        Me.fraCtlCancelPayment.Controls.Add(Me.txtUpdateDate)
        Me.fraCtlCancelPayment.Controls.Add(Me.cboMediaTypeStatus)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblAmount)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblBankAccountNo)
        Me.fraCtlCancelPayment.Controls.Add(Me.lblPolicyHolder)
        Me.fraCtlCancelPayment.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCtlCancelPayment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraCtlCancelPayment.Location = New System.Drawing.Point(6, 0)
        Me.fraCtlCancelPayment.Name = "fraCtlCancelPayment"
        Me.fraCtlCancelPayment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraCtlCancelPayment.Size = New System.Drawing.Size(403, 177)
        Me.fraCtlCancelPayment.TabIndex = 2
        Me.fraCtlCancelPayment.TabStop = False
        '
        'txtCommments
        '
        Me.txtCommments.AcceptsReturn = True
        Me.txtCommments.BackColor = System.Drawing.SystemColors.Window
        Me.txtCommments.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCommments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCommments.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCommments.Location = New System.Drawing.Point(142, 102)
        Me.txtCommments.MaxLength = 0
        Me.txtCommments.Name = "txtCommments"
        Me.txtCommments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCommments.Size = New System.Drawing.Size(249, 20)
        Me.txtCommments.TabIndex = 4
        '
        'txtUpdateDate
        '
        Me.txtUpdateDate.AcceptsReturn = True
        Me.txtUpdateDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtUpdateDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtUpdateDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUpdateDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUpdateDate.Location = New System.Drawing.Point(142, 58)
        Me.txtUpdateDate.MaxLength = 0
        Me.txtUpdateDate.Name = "txtUpdateDate"
        Me.txtUpdateDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUpdateDate.Size = New System.Drawing.Size(153, 20)
        Me.txtUpdateDate.TabIndex = 3
        '
        'cboMediaTypeStatus
        '
        Me.cboMediaTypeStatus.DefaultItemId = 0
        Me.cboMediaTypeStatus.FirstItem = ""
        Me.cboMediaTypeStatus.ItemId = 0
        Me.cboMediaTypeStatus.ListIndex = -1
        Me.cboMediaTypeStatus.Location = New System.Drawing.Point(142, 16)
        Me.cboMediaTypeStatus.Name = "cboMediaTypeStatus"
        Me.cboMediaTypeStatus.PMLookupProductFamily = 1
        Me.cboMediaTypeStatus.SingleItemId = 0
        Me.cboMediaTypeStatus.Size = New System.Drawing.Size(153, 21)
        Me.cboMediaTypeStatus.Sorted = True
        Me.cboMediaTypeStatus.TabIndex = 5
        Me.cboMediaTypeStatus.TableName = "MediaType_Status"
        Me.cboMediaTypeStatus.ToolTipText = ""
        Me.cboMediaTypeStatus.WhereClause = ""
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAmount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(12, 62)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAmount.Size = New System.Drawing.Size(91, 13)
        Me.lblAmount.TabIndex = 8
        Me.lblAmount.Text = "Update Date:"
        '
        'lblBankAccountNo
        '
        Me.lblBankAccountNo.AutoSize = True
        Me.lblBankAccountNo.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankAccountNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankAccountNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankAccountNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankAccountNo.Location = New System.Drawing.Point(12, 110)
        Me.lblBankAccountNo.Name = "lblBankAccountNo"
        Me.lblBankAccountNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankAccountNo.Size = New System.Drawing.Size(59, 13)
        Me.lblBankAccountNo.TabIndex = 7
        Me.lblBankAccountNo.Text = "Comments:"
        '
        'lblPolicyHolder
        '
        Me.lblPolicyHolder.AutoSize = True
        Me.lblPolicyHolder.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicyHolder.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicyHolder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicyHolder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicyHolder.Location = New System.Drawing.Point(10, 22)
        Me.lblPolicyHolder.Name = "lblPolicyHolder"
        Me.lblPolicyHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicyHolder.Size = New System.Drawing.Size(130, 13)
        Me.lblPolicyHolder.TabIndex = 6
        Me.lblPolicyHolder.Text = "Media Type Status:"
        '
        'frmUpdateMediaTypeStatus
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(414, 182)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.fraCtlCancelPayment)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUpdateMediaTypeStatus"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Update Media Type Status"
        Me.fraCtlCancelPayment.ResumeLayout(False)
        Me.fraCtlCancelPayment.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents fraCtlCancelPayment As System.Windows.Forms.GroupBox
#End Region 
End Class