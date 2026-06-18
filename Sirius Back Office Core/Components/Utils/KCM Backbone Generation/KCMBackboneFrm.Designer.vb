<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOk As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.dgvGISDataModels = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.chkIncludeCoreFieldsets = New System.Windows.Forms.CheckBox()
        Me.pBar = New System.Windows.Forms.ProgressBar()
        Me.lblStatusbar = New System.Windows.Forms.Label()
        Me.lblPBarPercent = New System.Windows.Forms.Label()
        CType(Me.dgvGISDataModels, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(499, 360)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(414, 360)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 22)
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Text = "&OK"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'dgvGISDataModels
        '
        Me.dgvGISDataModels.AllowUserToAddRows = False
        Me.dgvGISDataModels.AllowUserToDeleteRows = False
        Me.dgvGISDataModels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvGISDataModels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvGISDataModels.Location = New System.Drawing.Point(31, 82)
        Me.dgvGISDataModels.Name = "dgvGISDataModels"
        Me.dgvGISDataModels.RowHeadersVisible = False
        Me.dgvGISDataModels.Size = New System.Drawing.Size(541, 263)
        Me.dgvGISDataModels.TabIndex = 2
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(28, 18)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(334, 14)
        Me.lblTitle.TabIndex = 4
        Me.lblTitle.Text = "The backbone will be built for selected data models."
        '
        'chkIncludeCoreFieldsets
        '
        Me.chkIncludeCoreFieldsets.AutoSize = True
        Me.chkIncludeCoreFieldsets.Location = New System.Drawing.Point(31, 56)
        Me.chkIncludeCoreFieldsets.Name = "chkIncludeCoreFieldsets"
        Me.chkIncludeCoreFieldsets.Size = New System.Drawing.Size(160, 18)
        Me.chkIncludeCoreFieldsets.TabIndex = 5
        Me.chkIncludeCoreFieldsets.Text = "Include core fieldsets"
        Me.chkIncludeCoreFieldsets.UseVisualStyleBackColor = True
        '
        'pBar
        '
        Me.pBar.Location = New System.Drawing.Point(31, 388)
        Me.pBar.Name = "pBar"
        Me.pBar.Size = New System.Drawing.Size(541, 23)
        Me.pBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pBar.TabIndex = 6
        '
        'lblStatusbar
        '
        Me.lblStatusbar.AutoSize = True
        Me.lblStatusbar.Location = New System.Drawing.Point(28, 360)
        Me.lblStatusbar.Name = "lblStatusbar"
        Me.lblStatusbar.Size = New System.Drawing.Size(185, 14)
        Me.lblStatusbar.TabIndex = 7
        Me.lblStatusbar.Text = "Backbone generation status"
        '
        'lblPBarPercent
        '
        Me.lblPBarPercent.AutoSize = True
        Me.lblPBarPercent.Location = New System.Drawing.Point(219, 360)
        Me.lblPBarPercent.Name = "lblPBarPercent"
        Me.lblPBarPercent.Size = New System.Drawing.Size(0, 14)
        Me.lblPBarPercent.TabIndex = 8
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 15)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(614, 426)
        Me.Controls.Add(Me.lblPBarPercent)
        Me.Controls.Add(Me.lblStatusbar)
        Me.Controls.Add(Me.pBar)
        Me.Controls.Add(Me.chkIncludeCoreFieldsets)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.dgvGISDataModels)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(176, 219)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(500, 375)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "KCM Backbone Generation"
        CType(Me.dgvGISDataModels, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents ToolTip1 As ToolTip
    Friend WithEvents dgvGISDataModels As DataGridView
    Friend WithEvents lblTitle As Label
    Friend WithEvents chkIncludeCoreFieldsets As CheckBox
    Public WithEvents pBar As ProgressBar
    Friend WithEvents lblStatusbar As Label
    Friend WithEvents lblPBarPercent As Label
#End Region
End Class