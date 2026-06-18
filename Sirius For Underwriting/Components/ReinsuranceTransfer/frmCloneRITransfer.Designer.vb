<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCloneRITransfer
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
        Me.lblCaption = New System.Windows.Forms.Label()
        Me.txtClientCode = New System.Windows.Forms.TextBox()
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox()
        Me.txtClientName = New System.Windows.Forms.TextBox()
        Me.chkProcessClaims = New System.Windows.Forms.CheckBox()
        Me.lblClientName = New System.Windows.Forms.Label()
        Me.lblClientCode = New System.Windows.Forms.Label()
        Me.chkProcessPolicies = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.lblBranch = New System.Windows.Forms.Label()
        Me.grpCurrentPolicy = New System.Windows.Forms.GroupBox()
        Me.lblPolicyNumber = New System.Windows.Forms.Label()
        Me.cboBranch = New System.Windows.Forms.ComboBox()
        Me.cboProducts = New System.Windows.Forms.ComboBox()
        Me.lblProduct = New System.Windows.Forms.Label()
        Me.grpSelectPolicy = New System.Windows.Forms.GroupBox()
        Me.sbrStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.SbrStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.spbStatus = New System.Windows.Forms.ToolStripProgressBar()
        Me.bgwTransferPolicies = New System.ComponentModel.BackgroundWorker()
        Me.grpCurrentPolicy.SuspendLayout()
        Me.grpSelectPolicy.SuspendLayout()
        Me.sbrStatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblCaption
        '
        Me.lblCaption.AutoSize = True
        Me.lblCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaption.Location = New System.Drawing.Point(17, 18)
        Me.lblCaption.Name = "lblCaption"
        Me.lblCaption.Size = New System.Drawing.Size(285, 30)
        Me.lblCaption.TabIndex = 0
        Me.lblCaption.Text = "Click 'Start' to begin processing of policies with risk " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and claims that have Cl" & _
    "oned reinsurance."
        '
        'txtClientCode
        '
        Me.txtClientCode.Location = New System.Drawing.Point(131, 57)
        Me.txtClientCode.Name = "txtClientCode"
        Me.txtClientCode.ReadOnly = True
        Me.txtClientCode.Size = New System.Drawing.Size(272, 20)
        Me.txtClientCode.TabIndex = 4
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.Location = New System.Drawing.Point(131, 27)
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.ReadOnly = True
        Me.txtPolicyNumber.Size = New System.Drawing.Size(272, 20)
        Me.txtPolicyNumber.TabIndex = 3
        '
        'txtClientName
        '
        Me.txtClientName.Location = New System.Drawing.Point(131, 85)
        Me.txtClientName.Name = "txtClientName"
        Me.txtClientName.ReadOnly = True
        Me.txtClientName.Size = New System.Drawing.Size(272, 20)
        Me.txtClientName.TabIndex = 5
        '
        'chkProcessClaims
        '
        Me.chkProcessClaims.AutoSize = True
        Me.chkProcessClaims.Location = New System.Drawing.Point(25, 371)
        Me.chkProcessClaims.Name = "chkProcessClaims"
        Me.chkProcessClaims.Size = New System.Drawing.Size(97, 17)
        Me.chkProcessClaims.TabIndex = 12
        Me.chkProcessClaims.Text = "Process Claims"
        Me.chkProcessClaims.UseVisualStyleBackColor = True
        '
        'lblClientName
        '
        Me.lblClientName.AutoSize = True
        Me.lblClientName.Location = New System.Drawing.Point(29, 88)
        Me.lblClientName.Name = "lblClientName"
        Me.lblClientName.Size = New System.Drawing.Size(70, 13)
        Me.lblClientName.TabIndex = 2
        Me.lblClientName.Text = "Client Name :"
        '
        'lblClientCode
        '
        Me.lblClientCode.AutoSize = True
        Me.lblClientCode.Location = New System.Drawing.Point(29, 60)
        Me.lblClientCode.Name = "lblClientCode"
        Me.lblClientCode.Size = New System.Drawing.Size(67, 13)
        Me.lblClientCode.TabIndex = 1
        Me.lblClientCode.Text = "Client Code :"
        '
        'chkProcessPolicies
        '
        Me.chkProcessPolicies.AutoSize = True
        Me.chkProcessPolicies.Location = New System.Drawing.Point(25, 337)
        Me.chkProcessPolicies.Name = "chkProcessPolicies"
        Me.chkProcessPolicies.Size = New System.Drawing.Size(103, 17)
        Me.chkProcessPolicies.TabIndex = 11
        Me.chkProcessPolicies.Text = "Process Policies"
        Me.chkProcessPolicies.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(374, 337)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Stop"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(284, 337)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 9
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.Location = New System.Drawing.Point(30, 39)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.Size = New System.Drawing.Size(47, 13)
        Me.lblBranch.TabIndex = 4
        Me.lblBranch.Text = "Branch :"
        '
        'grpCurrentPolicy
        '
        Me.grpCurrentPolicy.Controls.Add(Me.txtClientName)
        Me.grpCurrentPolicy.Controls.Add(Me.txtClientCode)
        Me.grpCurrentPolicy.Controls.Add(Me.txtPolicyNumber)
        Me.grpCurrentPolicy.Controls.Add(Me.lblClientName)
        Me.grpCurrentPolicy.Controls.Add(Me.lblClientCode)
        Me.grpCurrentPolicy.Controls.Add(Me.lblPolicyNumber)
        Me.grpCurrentPolicy.Location = New System.Drawing.Point(10, 202)
        Me.grpCurrentPolicy.Name = "grpCurrentPolicy"
        Me.grpCurrentPolicy.Size = New System.Drawing.Size(439, 115)
        Me.grpCurrentPolicy.TabIndex = 8
        Me.grpCurrentPolicy.TabStop = False
        Me.grpCurrentPolicy.Text = "Current Policy"
        '
        'lblPolicyNumber
        '
        Me.lblPolicyNumber.AutoSize = True
        Me.lblPolicyNumber.Location = New System.Drawing.Point(29, 27)
        Me.lblPolicyNumber.Name = "lblPolicyNumber"
        Me.lblPolicyNumber.Size = New System.Drawing.Size(81, 13)
        Me.lblPolicyNumber.TabIndex = 0
        Me.lblPolicyNumber.Text = "Policy Number :"
        '
        'cboBranch
        '
        Me.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranch.Location = New System.Drawing.Point(131, 36)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.Size = New System.Drawing.Size(272, 21)
        Me.cboBranch.TabIndex = 0
        '
        'cboProducts
        '
        Me.cboProducts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProducts.Location = New System.Drawing.Point(131, 75)
        Me.cboProducts.Name = "cboProducts"
        Me.cboProducts.Size = New System.Drawing.Size(272, 21)
        Me.cboProducts.TabIndex = 5
        '
        'lblProduct
        '
        Me.lblProduct.AutoSize = True
        Me.lblProduct.Location = New System.Drawing.Point(30, 78)
        Me.lblProduct.Name = "lblProduct"
        Me.lblProduct.Size = New System.Drawing.Size(50, 13)
        Me.lblProduct.TabIndex = 0
        Me.lblProduct.Text = "Product :"
        '
        'grpSelectPolicy
        '
        Me.grpSelectPolicy.Controls.Add(Me.cboBranch)
        Me.grpSelectPolicy.Controls.Add(Me.lblBranch)
        Me.grpSelectPolicy.Controls.Add(Me.cboProducts)
        Me.grpSelectPolicy.Controls.Add(Me.lblProduct)
        Me.grpSelectPolicy.Location = New System.Drawing.Point(10, 67)
        Me.grpSelectPolicy.Name = "grpSelectPolicy"
        Me.grpSelectPolicy.Size = New System.Drawing.Size(439, 127)
        Me.grpSelectPolicy.TabIndex = 7
        Me.grpSelectPolicy.TabStop = False
        Me.grpSelectPolicy.Text = "Select Policies For Batch Transfer"
        '
        'sbrStatusStrip
        '
        Me.sbrStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SbrStatus, Me.spbStatus})
        Me.sbrStatusStrip.Location = New System.Drawing.Point(0, 400)
        Me.sbrStatusStrip.Name = "sbrStatusStrip"
        Me.sbrStatusStrip.Size = New System.Drawing.Size(484, 22)
        Me.sbrStatusStrip.TabIndex = 13
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
        'frmCloneRITransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 422)
        Me.Controls.Add(Me.sbrStatusStrip)
        Me.Controls.Add(Me.chkProcessClaims)
        Me.Controls.Add(Me.chkProcessPolicies)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.grpCurrentPolicy)
        Me.Controls.Add(Me.grpSelectPolicy)
        Me.Controls.Add(Me.lblCaption)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCloneRITransfer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Automatic Cloned Reinsurance Processing"
        Me.grpCurrentPolicy.ResumeLayout(False)
        Me.grpCurrentPolicy.PerformLayout()
        Me.grpSelectPolicy.ResumeLayout(False)
        Me.grpSelectPolicy.PerformLayout()
        Me.sbrStatusStrip.ResumeLayout(False)
        Me.sbrStatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCaption As System.Windows.Forms.Label
    Public WithEvents txtClientCode As System.Windows.Forms.TextBox
    Public WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
    Public WithEvents txtClientName As System.Windows.Forms.TextBox
    Public WithEvents chkProcessClaims As System.Windows.Forms.CheckBox
    Public WithEvents lblClientName As System.Windows.Forms.Label
    Public WithEvents lblClientCode As System.Windows.Forms.Label
    Public WithEvents chkProcessPolicies As System.Windows.Forms.CheckBox
    Public WithEvents btnCancel As System.Windows.Forms.Button
    Public WithEvents btnStart As System.Windows.Forms.Button
    Public WithEvents lblBranch As System.Windows.Forms.Label
    Public WithEvents grpCurrentPolicy As System.Windows.Forms.GroupBox
    Public WithEvents lblPolicyNumber As System.Windows.Forms.Label
    Friend WithEvents cboBranch As System.Windows.Forms.ComboBox
    Friend WithEvents cboProducts As System.Windows.Forms.ComboBox
    Public WithEvents lblProduct As System.Windows.Forms.Label
    Public WithEvents grpSelectPolicy As System.Windows.Forms.GroupBox
    Public WithEvents sbrStatusStrip As System.Windows.Forms.StatusStrip
    Public WithEvents SbrStatus As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents spbStatus As System.Windows.Forms.ToolStripProgressBar
    Public WithEvents bgwTransferPolicies As System.ComponentModel.BackgroundWorker
End Class
