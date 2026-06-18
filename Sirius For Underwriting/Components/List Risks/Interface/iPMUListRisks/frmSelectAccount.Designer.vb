<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectAccount
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializeoptCopyType()
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
    Private WithEvents _optClient As System.Windows.Forms.RadioButton
    Public WithEvents fraType As System.Windows.Forms.GroupBox
    Public optSelectAccount(1) As System.Windows.Forms.RadioButton
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.fraType = New System.Windows.Forms.GroupBox
        Me._optAgent = New System.Windows.Forms.RadioButton
        Me._optClient = New System.Windows.Forms.RadioButton
        Me.fraType.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(254, 94)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(71, 25)
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
        Me.cmdOK.Location = New System.Drawing.Point(177, 94)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(71, 25)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'fraType
        '
        Me.fraType.BackColor = System.Drawing.SystemColors.Control
        Me.fraType.Controls.Add(Me._optAgent)
        Me.fraType.Controls.Add(Me._optClient)
        Me.fraType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraType.Location = New System.Drawing.Point(6, 8)
        Me.fraType.Name = "fraType"
        Me.fraType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraType.Size = New System.Drawing.Size(319, 70)
        Me.fraType.TabIndex = 0
        Me.fraType.TabStop = False
        Me.fraType.Text = "Please Select"
        '
        '_optAgent
        '
        Me._optAgent.BackColor = System.Drawing.SystemColors.Control
        Me._optAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me._optAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optAgent.Location = New System.Drawing.Point(144, 33)
        Me._optAgent.Name = "_optAgent"
        Me._optAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optAgent.Size = New System.Drawing.Size(189, 23)
        Me._optAgent.TabIndex = 4
        Me._optAgent.TabStop = True
        Me._optAgent.Text = "Agent"
        Me._optAgent.UseVisualStyleBackColor = False
        '
        '_optClient
        '
        Me._optClient.BackColor = System.Drawing.SystemColors.Control
        Me._optClient.Cursor = System.Windows.Forms.Cursors.Default
        Me._optClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optClient.Location = New System.Drawing.Point(16, 33)
        Me._optClient.Name = "_optClient"
        Me._optClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optClient.Size = New System.Drawing.Size(241, 23)
        Me._optClient.TabIndex = 3
        Me._optClient.TabStop = True
        Me._optClient.Text = "Client"
        Me._optClient.UseVisualStyleBackColor = False
        '
        'frmSelectAccount
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(341, 123)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.fraType)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSelectAccount"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Select Client or Agent"
        Me.fraType.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub InitializeoptCopyType()
        Me.optSelectAccount(1) = _optAgent
        Me.optSelectAccount(0) = _optClient
    End Sub
    Private WithEvents _optAgent As System.Windows.Forms.RadioButton
#End Region
End Class