<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        lvwItems_InitializeColumnKeys()
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
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdRecommend As System.Windows.Forms.Button
    Public WithEvents cmdView As System.Windows.Forms.Button
    Public WithEvents cmdAuthorise As System.Windows.Forms.Button
    Public WithEvents cmdDecline As System.Windows.Forms.Button
    Private WithEvents _lvwItems_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwItems_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwItems_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwItems_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwItems_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwItems_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwItems_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwItems_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwItems_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwItems As System.Windows.Forms.ListView
    Private WithEvents _tabLossSchedule_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabLossSchedule As System.Windows.Forms.TabControl
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabLossSchedule = New System.Windows.Forms.TabControl
        Me._tabLossSchedule_TabPage0 = New System.Windows.Forms.TabPage
        Me.cmdRecommend = New System.Windows.Forms.Button
        Me.cmdView = New System.Windows.Forms.Button
        Me.cmdAuthorise = New System.Windows.Forms.Button
        Me.cmdDecline = New System.Windows.Forms.Button
        Me.lvwItems = New System.Windows.Forms.ListView
        Me._lvwItems_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me._lvwItems_ColumnHeader_10 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.tabLossSchedule.SuspendLayout()
        Me._tabLossSchedule_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(656, 416)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(73, 22)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.Text = "E&xit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(560, 416)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "O&K"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        Me.cmdOK.Visible = False
        '
        'tabLossSchedule
        '
        Me.tabLossSchedule.Controls.Add(Me._tabLossSchedule_TabPage0)
        Me.tabLossSchedule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabLossSchedule.ItemSize = New System.Drawing.Size(242, 18)
        Me.tabLossSchedule.Location = New System.Drawing.Point(0, 8)
        Me.tabLossSchedule.Multiline = True
        Me.tabLossSchedule.Name = "tabLossSchedule"
        Me.tabLossSchedule.SelectedIndex = 0
        Me.tabLossSchedule.Size = New System.Drawing.Size(733, 397)
        Me.tabLossSchedule.TabIndex = 0
        Me.tabLossSchedule.TabStop = False
        '
        '_tabLossSchedule_TabPage0
        '
        Me._tabLossSchedule_TabPage0.Controls.Add(Me.cmdRecommend)
        Me._tabLossSchedule_TabPage0.Controls.Add(Me.cmdView)
        Me._tabLossSchedule_TabPage0.Controls.Add(Me.cmdAuthorise)
        Me._tabLossSchedule_TabPage0.Controls.Add(Me.cmdDecline)
        Me._tabLossSchedule_TabPage0.Controls.Add(Me.lvwItems)
        Me._tabLossSchedule_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabLossSchedule_TabPage0.Name = "_tabLossSchedule_TabPage0"
        Me._tabLossSchedule_TabPage0.Size = New System.Drawing.Size(725, 371)
        Me._tabLossSchedule_TabPage0.TabIndex = 0
        Me._tabLossSchedule_TabPage0.Text = "1 - Authorise Payments"
        Me._tabLossSchedule_TabPage0.UseVisualStyleBackColor = True
        '
        'cmdRecommend
        '
        Me.cmdRecommend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRecommend.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRecommend.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRecommend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRecommend.Location = New System.Drawing.Point(458, 332)
        Me.cmdRecommend.Name = "cmdRecommend"
        Me.cmdRecommend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRecommend.Size = New System.Drawing.Size(85, 22)
        Me.cmdRecommend.TabIndex = 2
        Me.cmdRecommend.Text = "&Recommend"
        Me.cmdRecommend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRecommend.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(8, 332)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(73, 22)
        Me.cmdView.TabIndex = 1
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdAuthorise
        '
        Me.cmdAuthorise.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAuthorise.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAuthorise.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAuthorise.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAuthorise.Location = New System.Drawing.Point(552, 332)
        Me.cmdAuthorise.Name = "cmdAuthorise"
        Me.cmdAuthorise.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAuthorise.Size = New System.Drawing.Size(73, 22)
        Me.cmdAuthorise.TabIndex = 3
        Me.cmdAuthorise.Text = "&Authorise"
        Me.cmdAuthorise.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAuthorise.UseVisualStyleBackColor = False
        '
        'cmdDecline
        '
        Me.cmdDecline.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDecline.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDecline.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDecline.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDecline.Location = New System.Drawing.Point(640, 332)
        Me.cmdDecline.Name = "cmdDecline"
        Me.cmdDecline.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDecline.Size = New System.Drawing.Size(73, 22)
        Me.cmdDecline.TabIndex = 4
        Me.cmdDecline.Text = "&Decline"
        Me.cmdDecline.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDecline.UseVisualStyleBackColor = False
        '
        'lvwItems
        '
        Me.lvwItems.BackColor = System.Drawing.SystemColors.Window
        Me.lvwItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwItems_ColumnHeader_1, Me._lvwItems_ColumnHeader_2, Me._lvwItems_ColumnHeader_3, Me._lvwItems_ColumnHeader_4, Me._lvwItems_ColumnHeader_5, Me._lvwItems_ColumnHeader_6, Me._lvwItems_ColumnHeader_7, Me._lvwItems_ColumnHeader_8, Me._lvwItems_ColumnHeader_9, Me._lvwItems_ColumnHeader_10})
        Me.lvwItems.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwItems.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwItems.HideSelection = False
        Me.lvwItems.Location = New System.Drawing.Point(8, 20)
        Me.lvwItems.Name = "lvwItems"
        Me.lvwItems.Size = New System.Drawing.Size(705, 297)
        Me.lvwItems.TabIndex = 0
        Me.lvwItems.UseCompatibleStateImageBehavior = False
        Me.lvwItems.View = System.Windows.Forms.View.Details
        '
        '_lvwItems_ColumnHeader_1
        '
        Me._lvwItems_ColumnHeader_1.Tag = ""
        Me._lvwItems_ColumnHeader_1.Text = "Claim Number"
        Me._lvwItems_ColumnHeader_1.Width = 97
        '
        '_lvwItems_ColumnHeader_2
        '
        Me._lvwItems_ColumnHeader_2.Tag = ""
        Me._lvwItems_ColumnHeader_2.Text = "Policy Number"
        Me._lvwItems_ColumnHeader_2.Width = 97
        '
        '_lvwItems_ColumnHeader_3
        '
        Me._lvwItems_ColumnHeader_3.Tag = ""
        Me._lvwItems_ColumnHeader_3.Text = "Payee Name"
        Me._lvwItems_ColumnHeader_3.Width = 97
        '
        '_lvwItems_ColumnHeader_10
        '
        Me._lvwItems_ColumnHeader_10.Text = "Payee Name"
        Me._lvwItems_ColumnHeader_10.Width = 97
        '
        '_lvwItems_ColumnHeader_4
        '
        Me._lvwItems_ColumnHeader_4.Tag = ""
        Me._lvwItems_ColumnHeader_4.Text = "Payment Amount"
        Me._lvwItems_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwItems_ColumnHeader_4.Width = 97
        '
        '_lvwItems_ColumnHeader_5
        '
        Me._lvwItems_ColumnHeader_5.Tag = ""
        Me._lvwItems_ColumnHeader_5.Text = "Payment Date"
        Me._lvwItems_ColumnHeader_5.Width = 97
        '
        '_lvwItems_ColumnHeader_6
        '
        Me._lvwItems_ColumnHeader_6.Tag = ""
        Me._lvwItems_ColumnHeader_6.Text = "Created By"
        Me._lvwItems_ColumnHeader_6.Width = 97
        '
        '_lvwItems_ColumnHeader_7
        '
        Me._lvwItems_ColumnHeader_7.Tag = ""
        Me._lvwItems_ColumnHeader_7.Text = "Status"
        Me._lvwItems_ColumnHeader_7.Width = 97
        '
        '_lvwItems_ColumnHeader_8
        '
        Me._lvwItems_ColumnHeader_8.Tag = ""
        Me._lvwItems_ColumnHeader_8.Text = "isRecommended"
        Me._lvwItems_ColumnHeader_8.Width = 0
        '
        '_lvwItems_ColumnHeader_9
        '
        Me._lvwItems_ColumnHeader_9.Tag = ""
        Me._lvwItems_ColumnHeader_9.Text = "Recommender"
        Me._lvwItems_ColumnHeader_9.Width = 0
        '
        '_lvwItems_ColumnHeader_10
        '
        Me._lvwItems_ColumnHeader_10.Text = "Client Name"
        Me._lvwItems_ColumnHeader_10.Width = 97
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(737, 447)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabLossSchedule)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Authorise Payments"
        Me.tabLossSchedule.ResumeLayout(False)
        Me._tabLossSchedule_TabPage0.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub lvwItems_InitializeColumnKeys()
        Me._lvwItems_ColumnHeader_1.Name = ""
        Me._lvwItems_ColumnHeader_2.Name = ""
        Me._lvwItems_ColumnHeader_3.Name = ""
        Me._lvwItems_ColumnHeader_4.Name = ""
        Me._lvwItems_ColumnHeader_5.Name = ""
        Me._lvwItems_ColumnHeader_6.Name = ""
        Me._lvwItems_ColumnHeader_7.Name = ""
        Me._lvwItems_ColumnHeader_8.Name = ""
        Me._lvwItems_ColumnHeader_9.Name = ""
    End Sub
    Private WithEvents _lvwItems_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
#End Region
End Class