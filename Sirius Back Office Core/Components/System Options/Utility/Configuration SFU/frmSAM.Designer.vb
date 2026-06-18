<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSAM
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
    Public WithEvents chkOption5044 As System.Windows.Forms.CheckBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkOptionExclusiveLock = New System.Windows.Forms.CheckBox()
        Me.chkOption5044 = New System.Windows.Forms.CheckBox()
        Me.lblExclusiveLocking = New System.Windows.Forms.Label()
        Me.txtExclusiveLocking = New System.Windows.Forms.TextBox()
        Me.lblExclusiveLockingMinutes = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'chkOptionExclusiveLock
        '
        Me.chkOptionExclusiveLock.AutoSize = True
        Me.chkOptionExclusiveLock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOptionExclusiveLock.Location = New System.Drawing.Point(8, 36)
        Me.chkOptionExclusiveLock.Name = "chkOptionExclusiveLock"
        Me.chkOptionExclusiveLock.Size = New System.Drawing.Size(205, 17)
        Me.chkOptionExclusiveLock.TabIndex = 1
        Me.chkOptionExclusiveLock.Tag = "5174"
        Me.chkOptionExclusiveLock.Text = "Enable Exclusive Locking:        "
        Me.ToolTip1.SetToolTip(Me.chkOptionExclusiveLock, "Claims only!")
        Me.chkOptionExclusiveLock.UseVisualStyleBackColor = True
        '
        'chkOption5044
        '
        Me.chkOption5044.BackColor = System.Drawing.SystemColors.Control
        Me.chkOption5044.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOption5044.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOption5044.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOption5044.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOption5044.Location = New System.Drawing.Point(8, 8)
        Me.chkOption5044.Name = "chkOption5044"
        Me.chkOption5044.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOption5044.Size = New System.Drawing.Size(203, 21)
        Me.chkOption5044.TabIndex = 0
        Me.chkOption5044.Tag = "5044"
        Me.chkOption5044.Text = "Enable SSP Transaction Agent:"
        Me.chkOption5044.UseVisualStyleBackColor = False
        '
        'lblExclusiveLocking
        '
        Me.lblExclusiveLocking.AutoSize = True
        Me.lblExclusiveLocking.Location = New System.Drawing.Point(217, 35)
        Me.lblExclusiveLocking.Name = "lblExclusiveLocking"
        Me.lblExclusiveLocking.Size = New System.Drawing.Size(116, 13)
        Me.lblExclusiveLocking.TabIndex = 2
        Me.lblExclusiveLocking.Tag = "5100"
        Me.lblExclusiveLocking.Text = "Remove Lock After"
        Me.lblExclusiveLocking.Visible = False
        '
        'txtExclusiveLocking
        '
        Me.txtExclusiveLocking.AccessibleDescription = "Remove Lock After Minutes"
        Me.txtExclusiveLocking.Enabled = False
        Me.txtExclusiveLocking.Location = New System.Drawing.Point(339, 35)
        Me.txtExclusiveLocking.MaxLength = 3
        Me.txtExclusiveLocking.Name = "txtExclusiveLocking"
        Me.txtExclusiveLocking.Size = New System.Drawing.Size(100, 21)
        Me.txtExclusiveLocking.TabIndex = 3
        Me.txtExclusiveLocking.Tag = "5175,ValidateNumeric,M"
        Me.txtExclusiveLocking.Visible = False
        '
        'lblExclusiveLockingMinutes
        '
        Me.lblExclusiveLockingMinutes.AutoSize = True
        Me.lblExclusiveLockingMinutes.Location = New System.Drawing.Point(445, 36)
        Me.lblExclusiveLockingMinutes.Name = "lblExclusiveLockingMinutes"
        Me.lblExclusiveLockingMinutes.Size = New System.Drawing.Size(50, 13)
        Me.lblExclusiveLockingMinutes.TabIndex = 4
        Me.lblExclusiveLockingMinutes.Tag = "5101"
        Me.lblExclusiveLockingMinutes.Text = "Minutes"
        Me.lblExclusiveLockingMinutes.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblExclusiveLockingMinutes.Visible = False
        '
        'frmSAM
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(659, 452)
        Me.Controls.Add(Me.lblExclusiveLockingMinutes)
        Me.Controls.Add(Me.txtExclusiveLocking)
        Me.Controls.Add(Me.lblExclusiveLocking)
        Me.Controls.Add(Me.chkOptionExclusiveLock)
        Me.Controls.Add(Me.chkOption5044)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmSAM"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkOptionExclusiveLock As System.Windows.Forms.CheckBox
    Friend WithEvents lblExclusiveLocking As System.Windows.Forms.Label
    Friend WithEvents txtExclusiveLocking As System.Windows.Forms.TextBox
    Friend WithEvents lblExclusiveLockingMinutes As System.Windows.Forms.Label
#End Region
End Class