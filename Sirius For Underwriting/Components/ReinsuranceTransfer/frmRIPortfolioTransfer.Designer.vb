<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRIPortfolioTransfer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cboBranch = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboProducts = New System.Windows.Forms.ComboBox()
        Me.txtTransferDate = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtClientName = New System.Windows.Forms.TextBox()
        Me.txtClientCode = New System.Windows.Forms.TextBox()
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.sbrStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.SbrStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.spbStatus = New System.Windows.Forms.ToolStripProgressBar()
        Me.bgwTransferPolicies = New System.ComponentModel.BackgroundWorker()
        Me.chkDelayPostings = New System.Windows.Forms.CheckBox()
        Me.chkRunPostings = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.sbrStatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cboBranch)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.cboProducts)
        Me.GroupBox1.Controls.Add(Me.txtTransferDate)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(439, 127)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select Policies For Batch Transfer"
        '
        'cboBranch
        '
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Location = New System.Drawing.Point(131, 19)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.Size = New System.Drawing.Size(250, 21)
        Me.cboBranch.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(30, 22)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(47, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Branch :"
        '
        'cboProducts
        '
        Me.cboProducts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProducts.Location = New System.Drawing.Point(131, 58)
        Me.cboProducts.Name = "cboProducts"
        Me.cboProducts.Size = New System.Drawing.Size(250, 21)
        Me.cboProducts.TabIndex = 5
        '
        'txtTransferDate
        '
        Me.txtTransferDate.CustomFormat = "dd/MMM/yyyy"
        Me.txtTransferDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.txtTransferDate.Location = New System.Drawing.Point(131, 98)
        Me.txtTransferDate.Name = "txtTransferDate"
        Me.txtTransferDate.Size = New System.Drawing.Size(117, 20)
        Me.txtTransferDate.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 98)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Transfer Date :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(30, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Product :"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtClientName)
        Me.GroupBox2.Controls.Add(Me.txtClientCode)
        Me.GroupBox2.Controls.Add(Me.txtPolicyNumber)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 141)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(439, 115)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Current Policy/Claim"
        '
        'txtClientName
        '
        Me.txtClientName.Location = New System.Drawing.Point(131, 81)
        Me.txtClientName.Name = "txtClientName"
        Me.txtClientName.ReadOnly = True
        Me.txtClientName.Size = New System.Drawing.Size(250, 20)
        Me.txtClientName.TabIndex = 5
        '
        'txtClientCode
        '
        Me.txtClientCode.Location = New System.Drawing.Point(131, 53)
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.ReadOnly = True
        Me.txtClientCode.Size = New System.Drawing.Size(250, 20)
        Me.txtClientCode.TabIndex = 4
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.Location = New System.Drawing.Point(131, 23)
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.ReadOnly = True
        Me.txtPolicyNumber.Size = New System.Drawing.Size(250, 20)
        Me.txtPolicyNumber.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 84)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Client Name :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 55)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Client Code :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Policy/Claim Number :"
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(286, 286)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 3
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(376, 286)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Stop"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'sbrStatusStrip
        '
        Me.sbrStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SbrStatus, Me.spbStatus})
        Me.sbrStatusStrip.Location = New System.Drawing.Point(0, 360)
        Me.sbrStatusStrip.Name = "sbrStatusStrip"
        Me.sbrStatusStrip.Size = New System.Drawing.Size(476, 22)
        Me.sbrStatusStrip.TabIndex = 2
        Me.sbrStatusStrip.Text = "StatusStrip1"
        '
        'SbrStatus
        '
        Me.SbrStatus.Name = "SbrStatus"
        Me.SbrStatus.Size = New System.Drawing.Size(107, 17)
        Me.SbrStatus.Text = "Click Start to Begin"
        '
        'spbStatus
        '
        Me.spbStatus.Name = "spbStatus"
        Me.spbStatus.Size = New System.Drawing.Size(100, 16)
        '
        'bgwTransferPolicies
        '
        Me.bgwTransferPolicies.WorkerReportsProgress = True
        Me.bgwTransferPolicies.WorkerSupportsCancellation = True
        '
        'chkDelayPostings
        '
        Me.chkDelayPostings.AutoSize = True
        Me.chkDelayPostings.Location = New System.Drawing.Point(27, 286)
        Me.chkDelayPostings.Name = "chkDelayPostings"
        Me.chkDelayPostings.Size = New System.Drawing.Size(139, 17)
        Me.chkDelayPostings.TabIndex = 5
        Me.chkDelayPostings.Text = "Delay Accounts Posting"
        Me.chkDelayPostings.UseVisualStyleBackColor = True
        '
        'chkRunPostings
        '
        Me.chkRunPostings.AutoSize = True
        Me.chkRunPostings.Location = New System.Drawing.Point(27, 320)
        Me.chkRunPostings.Name = "chkRunPostings"
        Me.chkRunPostings.Size = New System.Drawing.Size(209, 17)
        Me.chkRunPostings.TabIndex = 6
        Me.chkRunPostings.Text = "Run Account Postings Where Delayed"
        Me.chkRunPostings.UseVisualStyleBackColor = True
        '
        'frmRIPortfolioTransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(476, 382)
        Me.Controls.Add(Me.chkRunPostings)
        Me.Controls.Add(Me.chkDelayPostings)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.sbrStatusStrip)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRIPortfolioTransfer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RI Portfolio Transfer"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.sbrStatusStrip.ResumeLayout(False)
        Me.sbrStatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Public WithEvents btnStart As System.Windows.Forms.Button
    Public WithEvents btnCancel As System.Windows.Forms.Button
    Public WithEvents txtTransferDate As System.Windows.Forms.DateTimePicker
    Public WithEvents txtClientName As System.Windows.Forms.TextBox
    Public WithEvents txtClientCode As System.Windows.Forms.TextBox
    Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents sbrStatusStrip As System.Windows.Forms.StatusStrip
    Public WithEvents spbStatus As System.Windows.Forms.ToolStripProgressBar
    Public WithEvents SbrStatus As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents bgwTransferPolicies As System.ComponentModel.BackgroundWorker
    Public WithEvents chkDelayPostings As System.Windows.Forms.CheckBox
    Public WithEvents chkRunPostings As System.Windows.Forms.CheckBox
    Public WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cboProducts As System.Windows.Forms.ComboBox
    Friend WithEvents cboBranch As System.Windows.Forms.ComboBox
End Class
