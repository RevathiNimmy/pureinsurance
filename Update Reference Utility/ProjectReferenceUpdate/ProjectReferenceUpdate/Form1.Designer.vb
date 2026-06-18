<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.txtFolderPath = New System.Windows.Forms.TextBox()
        Me.btnUpdateReference = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtFolderPath
        '
        Me.txtFolderPath.Location = New System.Drawing.Point(89, 22)
        Me.txtFolderPath.Multiline = True
        Me.txtFolderPath.Name = "txtFolderPath"
        Me.txtFolderPath.Size = New System.Drawing.Size(533, 29)
        Me.txtFolderPath.TabIndex = 0
        '
        'btnUpdateReference
        '
        Me.btnUpdateReference.Location = New System.Drawing.Point(243, 66)
        Me.btnUpdateReference.Name = "btnUpdateReference"
        Me.btnUpdateReference.Size = New System.Drawing.Size(126, 23)
        Me.btnUpdateReference.TabIndex = 1
        Me.btnUpdateReference.Text = "UpdateReferences"
        Me.btnUpdateReference.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "File Path"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(635, 99)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnUpdateReference)
        Me.Controls.Add(Me.txtFolderPath)
        Me.Name = "Form1"
        Me.Text = "Update References"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtFolderPath As TextBox
    Friend WithEvents btnUpdateReference As Button
    Friend WithEvents Label1 As Label
End Class
