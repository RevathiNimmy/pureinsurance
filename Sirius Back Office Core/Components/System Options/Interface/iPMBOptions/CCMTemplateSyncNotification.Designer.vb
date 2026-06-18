<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CCMTemplateSyncNotification
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
        Me.lblNotificationCCM = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblNotificationCCM
        '
        Me.lblNotificationCCM.AutoSize = True
        Me.lblNotificationCCM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNotificationCCM.Location = New System.Drawing.Point(4, 31)
        Me.lblNotificationCCM.Name = "lblNotificationCCM"
        Me.lblNotificationCCM.Size = New System.Drawing.Size(276, 13)
        Me.lblNotificationCCM.TabIndex = 0
        Me.lblNotificationCCM.Text = "Please wait, Pure is currently syncing with KCM"
        '
        'CCMTemplateSyncNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(286, 85)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblNotificationCCM)
        Me.Name = "CCMTemplateSyncNotification"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "KCM Template Sync"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblNotificationCCM As System.Windows.Forms.Label
End Class
