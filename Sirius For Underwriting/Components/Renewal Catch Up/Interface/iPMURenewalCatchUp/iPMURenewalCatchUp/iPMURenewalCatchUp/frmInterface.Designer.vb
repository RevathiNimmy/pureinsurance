<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmInterface
    Inherits System.Windows.Forms.Form

    Public Sub New()
        MyBase.New()

        InitializeComponent()

        Form_Initialize_Renamed()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.grpRenewalCatchUp = New System.Windows.Forms.GroupBox()
        Me.lvwVersionDetails = New System.Windows.Forms.ListView()
        Me._lvwVersionDetails_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersionDetails_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersionDetails_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersionDetails_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwVersionDetails_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.prgbVersion = New System.Windows.Forms.ProgressBar()
        Me.lblVersions = New System.Windows.Forms.Label()
        Me.btnRenewNo = New System.Windows.Forms.Button()
        Me.btnRenewYes = New System.Windows.Forms.Button()
        Me.lblPendingRenewal = New System.Windows.Forms.Label()
        Me.lblSuccessfullyRenewed = New System.Windows.Forms.Label()
        Me.btnSuccessfullyRenewedOk = New System.Windows.Forms.Button()
        Me.grpRenewalCatchUp.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpRenewalCatchUp
        '
        Me.grpRenewalCatchUp.Controls.Add(Me.lvwVersionDetails)
        Me.grpRenewalCatchUp.Controls.Add(Me.prgbVersion)
        Me.grpRenewalCatchUp.Controls.Add(Me.lblVersions)
        Me.grpRenewalCatchUp.Controls.Add(Me.btnRenewNo)
        Me.grpRenewalCatchUp.Controls.Add(Me.btnRenewYes)
        Me.grpRenewalCatchUp.Controls.Add(Me.lblPendingRenewal)
        Me.grpRenewalCatchUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRenewalCatchUp.Location = New System.Drawing.Point(12, 12)
        Me.grpRenewalCatchUp.Name = "grpRenewalCatchUp"
        Me.grpRenewalCatchUp.Size = New System.Drawing.Size(671, 299)
        Me.grpRenewalCatchUp.TabIndex = 0
        Me.grpRenewalCatchUp.TabStop = False
        Me.grpRenewalCatchUp.Text = "Renewal Catch-up"
        '
        'lvwVersionDetails
        '
        Me.lvwVersionDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwVersionDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwVersionDetails_ColumnHeader_1, Me._lvwVersionDetails_ColumnHeader_2, Me._lvwVersionDetails_ColumnHeader_3, Me._lvwVersionDetails_ColumnHeader_4, Me._lvwVersionDetails_ColumnHeader_5})
        Me.lvwVersionDetails.HideSelection = False
        Me.lvwVersionDetails.Location = New System.Drawing.Point(7, 110)
        Me.lvwVersionDetails.Name = "lvwVersionDetails"
        Me.lvwVersionDetails.Size = New System.Drawing.Size(658, 183)
        Me.lvwVersionDetails.TabIndex = 5
        Me.lvwVersionDetails.UseCompatibleStateImageBehavior = False
        Me.lvwVersionDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwVersionDetails_ColumnHeader_1
        '
        Me._lvwVersionDetails_ColumnHeader_1.Text = "Cover From"
        Me._lvwVersionDetails_ColumnHeader_1.Width = 100
        '
        '_lvwVersionDetails_ColumnHeader_2
        '
        Me._lvwVersionDetails_ColumnHeader_2.Text = "Renewal Date"
        Me._lvwVersionDetails_ColumnHeader_2.Width = 100
        '
        '_lvwVersionDetails_ColumnHeader_3
        '
        Me._lvwVersionDetails_ColumnHeader_3.Text = "Billing Method"
        Me._lvwVersionDetails_ColumnHeader_3.Width = 100
        '
        '_lvwVersionDetails_ColumnHeader_4
        '
        Me._lvwVersionDetails_ColumnHeader_4.Text = "Amount"
        Me._lvwVersionDetails_ColumnHeader_4.Width = 100
        '
        '_lvwVersionDetails_ColumnHeader_5
        '
        Me._lvwVersionDetails_ColumnHeader_5.Text = "Status"
        Me._lvwVersionDetails_ColumnHeader_5.Width = 257
        '
        'prgbVersion
        '
        Me.prgbVersion.Location = New System.Drawing.Point(373, 68)
        Me.prgbVersion.Name = "prgbVersion"
        Me.prgbVersion.Size = New System.Drawing.Size(263, 23)
        Me.prgbVersion.TabIndex = 4
        '
        'lblVersions
        '
        Me.lblVersions.AutoSize = True
        Me.lblVersions.Location = New System.Drawing.Point(32, 68)
        Me.lblVersions.Name = "lblVersions"
        Me.lblVersions.Size = New System.Drawing.Size(47, 13)
        Me.lblVersions.TabIndex = 3
        Me.lblVersions.Text = "Versions"
        '
        'btnRenewNo
        '
        Me.btnRenewNo.Location = New System.Drawing.Point(514, 26)
        Me.btnRenewNo.Name = "btnRenewNo"
        Me.btnRenewNo.Size = New System.Drawing.Size(75, 23)
        Me.btnRenewNo.TabIndex = 2
        Me.btnRenewNo.Text = "No"
        Me.btnRenewNo.UseVisualStyleBackColor = True
        '
        'btnRenewYes
        '
        Me.btnRenewYes.Location = New System.Drawing.Point(415, 27)
        Me.btnRenewYes.Name = "btnRenewYes"
        Me.btnRenewYes.Size = New System.Drawing.Size(75, 23)
        Me.btnRenewYes.TabIndex = 1
        Me.btnRenewYes.Text = "Yes"
        Me.btnRenewYes.UseVisualStyleBackColor = True
        '
        'lblPendingRenewal
        '
        Me.lblPendingRenewal.AutoSize = True
        Me.lblPendingRenewal.Location = New System.Drawing.Point(29, 27)
        Me.lblPendingRenewal.Name = "lblPendingRenewal"
        Me.lblPendingRenewal.Size = New System.Drawing.Size(322, 13)
        Me.lblPendingRenewal.TabIndex = 0
        Me.lblPendingRenewal.Text = "The monthly policy is pending for renewals. Do you want to renew?"
        '
        'lblSuccessfullyRenewed
        '
        Me.lblSuccessfullyRenewed.AutoSize = True
        Me.lblSuccessfullyRenewed.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSuccessfullyRenewed.Location = New System.Drawing.Point(47, 337)
        Me.lblSuccessfullyRenewed.Name = "lblSuccessfullyRenewed"
        Me.lblSuccessfullyRenewed.Size = New System.Drawing.Size(343, 13)
        Me.lblSuccessfullyRenewed.TabIndex = 1
        Me.lblSuccessfullyRenewed.Text = "The Policy is successfully renewed upto the current period."
        '
        'btnSuccessfullyRenewedOk
        '
        Me.btnSuccessfullyRenewedOk.Location = New System.Drawing.Point(526, 337)
        Me.btnSuccessfullyRenewedOk.Name = "btnSuccessfullyRenewedOk"
        Me.btnSuccessfullyRenewedOk.Size = New System.Drawing.Size(75, 23)
        Me.btnSuccessfullyRenewedOk.TabIndex = 2
        Me.btnSuccessfullyRenewedOk.Text = "OK"
        Me.btnSuccessfullyRenewedOk.UseVisualStyleBackColor = True
        '
        'frmInterface
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(697, 402)
        Me.Controls.Add(Me.btnSuccessfullyRenewedOk)
        Me.Controls.Add(Me.lblSuccessfullyRenewed)
        Me.Controls.Add(Me.grpRenewalCatchUp)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.grpRenewalCatchUp.ResumeLayout(False)
        Me.grpRenewalCatchUp.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents grpRenewalCatchUp As Windows.Forms.GroupBox
    Friend WithEvents btnRenewNo As Windows.Forms.Button
    Friend WithEvents btnRenewYes As Windows.Forms.Button
    Friend WithEvents lblPendingRenewal As Windows.Forms.Label
    Friend WithEvents prgbVersion As Windows.Forms.ProgressBar
    Friend WithEvents lblVersions As Windows.Forms.Label
    Friend WithEvents lvwVersionDetails As Windows.Forms.ListView
    Friend WithEvents _lvwVersionDetails_ColumnHeader_1 As Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersionDetails_ColumnHeader_2 As Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersionDetails_ColumnHeader_3 As Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersionDetails_ColumnHeader_4 As Windows.Forms.ColumnHeader
    Friend WithEvents _lvwVersionDetails_ColumnHeader_5 As Windows.Forms.ColumnHeader
    Friend WithEvents lblSuccessfullyRenewed As Windows.Forms.Label
    Friend WithEvents btnSuccessfullyRenewedOk As Windows.Forms.Button
End Class
