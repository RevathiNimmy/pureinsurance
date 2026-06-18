<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmExternalWorkFlowConfiguration
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        Form_Initialize_Renamed()
    End Sub
    Private Sub Ctx_mnuSupervisor_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_mnuSupervisor.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_mnuSupervisor.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In mnuSupervisor.DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_mnuSupervisor.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_mnuSupervisor_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_mnuSupervisor.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_mnuSupervisor.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuSupervisor.DropDownItems.Add(item)
        Next item
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
    Public cmdlgSignatureOpen As System.Windows.Forms.OpenFileDialog
    Public WithEvents cmdAddGroup As System.Windows.Forms.Button
    Public WithEvents cmdDelAllGroups As System.Windows.Forms.Button
    Public WithEvents cmdDelGroup As System.Windows.Forms.Button
    Public WithEvents cmdAddAllGroups As System.Windows.Forms.Button
    Private WithEvents _lvwSelectedGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwSelectedGroups As System.Windows.Forms.ListView
    Private WithEvents _lvwGroups_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwGroups As System.Windows.Forms.ListView
    Public WithEvents imgGroup As System.Windows.Forms.ImageList
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents Frame4 As System.Windows.Forms.GroupBox
    Public WithEvents chkbackgroundjobforfailure As System.Windows.Forms.CheckBox
    Public chkAccHandler(2) As System.Windows.Forms.CheckBox
    Public chkAgent(1) As System.Windows.Forms.CheckBox
    Public cmdAccHandler(2) As System.Windows.Forms.Button
    Public cmdAgent(1) As System.Windows.Forms.Button
    Public cmdNext(6) As System.Windows.Forms.Button
    Public cmdPrevious(6) As System.Windows.Forms.Button
    Public lblAccHandlerYN(2) As System.Windows.Forms.Label
    Public lblAgentYN(1) As System.Windows.Forms.Label
    Public pnlAccHandler(2) As System.Windows.Forms.Panel
    Public pnlAgent(1) As System.Windows.Forms.Panel
    Public WithEvents Ctx_mnuSupervisor As System.Windows.Forms.ContextMenuStrip
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmExternalWorkFlowConfiguration))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdlgSignatureOpen = New System.Windows.Forms.OpenFileDialog
        Me.imgGroup = New System.Windows.Forms.ImageList(Me.components)
        Me.Frame4 = New System.Windows.Forms.GroupBox
        Me.cmdAddGroup = New System.Windows.Forms.Button
        Me.cmdDelAllGroups = New System.Windows.Forms.Button
        Me.cmdDelGroup = New System.Windows.Forms.Button
        Me.cmdAddAllGroups = New System.Windows.Forms.Button
        Me.lvwSelectedGroups = New System.Windows.Forms.ListView
        Me._lvwSelectedGroups_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.lvwGroups = New System.Windows.Forms.ListView
        Me._lvwGroups_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.chkbackgroundjobforfailure = New System.Windows.Forms.CheckBox
        Me.Ctx_mnuSupervisor = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuSupervisor = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSuper = New System.Windows.Forms.ToolStripMenuItem
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.Frame4.SuspendLayout()
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(510, 412)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 38
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept any changes and exit")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(589, 412)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 40
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel any changes that have not been applied to the db and exit")
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdlgSignatureOpen
        '
        Me.cmdlgSignatureOpen.Filter = "Signature Files (*.bmp;*.jpg;*.gif)|*.bmp;*.jpg,*.gif"
        Me.cmdlgSignatureOpen.InitialDirectory = "app.path"
        '
        'imgGroup
        '
        Me.imgGroup.ImageStream = CType(resources.GetObject("imgGroup.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgGroup.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgGroup.Images.SetKeyName(0, "user")
        Me.imgGroup.Images.SetKeyName(1, "group")
        Me.imgGroup.Images.SetKeyName(2, "supervisor")
        '
        'Frame4
        '
        Me.Frame4.BackColor = System.Drawing.SystemColors.Control
        Me.Frame4.Controls.Add(Me.cmdAddGroup)
        Me.Frame4.Controls.Add(Me.cmdDelAllGroups)
        Me.Frame4.Controls.Add(Me.cmdDelGroup)
        Me.Frame4.Controls.Add(Me.cmdAddAllGroups)
        Me.Frame4.Controls.Add(Me.lvwSelectedGroups)
        Me.Frame4.Controls.Add(Me.lvwGroups)
        Me.Frame4.Controls.Add(Me.Label10)
        Me.Frame4.Controls.Add(Me.Label11)
        Me.Frame4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame4.Location = New System.Drawing.Point(12, 33)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame4.Size = New System.Drawing.Size(644, 343)
        Me.Frame4.TabIndex = 89
        Me.Frame4.TabStop = False
        '
        'cmdAddGroup
        '
        Me.cmdAddGroup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddGroup.Location = New System.Drawing.Point(294, 56)
        Me.cmdAddGroup.Name = "cmdAddGroup"
        Me.cmdAddGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddGroup.Size = New System.Drawing.Size(51, 25)
        Me.cmdAddGroup.TabIndex = 92
        Me.cmdAddGroup.Text = "--&>"
        Me.cmdAddGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddGroup.UseVisualStyleBackColor = False
        '
        'cmdDelAllGroups
        '
        Me.cmdDelAllGroups.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelAllGroups.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelAllGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelAllGroups.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelAllGroups.Location = New System.Drawing.Point(294, 296)
        Me.cmdDelAllGroups.Name = "cmdDelAllGroups"
        Me.cmdDelAllGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelAllGroups.Size = New System.Drawing.Size(51, 25)
        Me.cmdDelAllGroups.TabIndex = 95
        Me.cmdDelAllGroups.Text = "<<-"
        Me.cmdDelAllGroups.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelAllGroups.UseVisualStyleBackColor = False
        '
        'cmdDelGroup
        '
        Me.cmdDelGroup.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelGroup.Location = New System.Drawing.Point(294, 264)
        Me.cmdDelGroup.Name = "cmdDelGroup"
        Me.cmdDelGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelGroup.Size = New System.Drawing.Size(51, 25)
        Me.cmdDelGroup.TabIndex = 94
        Me.cmdDelGroup.Text = "&<--"
        Me.cmdDelGroup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelGroup.UseVisualStyleBackColor = False
        '
        'cmdAddAllGroups
        '
        Me.cmdAddAllGroups.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAllGroups.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAllGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAllGroups.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAllGroups.Location = New System.Drawing.Point(294, 88)
        Me.cmdAddAllGroups.Name = "cmdAddAllGroups"
        Me.cmdAddAllGroups.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAllGroups.Size = New System.Drawing.Size(51, 25)
        Me.cmdAddAllGroups.TabIndex = 93
        Me.cmdAddAllGroups.Text = "->>"
        Me.cmdAddAllGroups.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAddAllGroups.UseVisualStyleBackColor = False
        '
        'lvwSelectedGroups
        '
        Me.lvwSelectedGroups.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSelectedGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSelectedGroups_ColumnHeader_1})
        Me.lvwSelectedGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSelectedGroups.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSelectedGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwSelectedGroups.LargeImageList = Me.imgGroup
        Me.lvwSelectedGroups.Location = New System.Drawing.Point(360, 48)
        Me.lvwSelectedGroups.Name = "lvwSelectedGroups"
        Me.lvwSelectedGroups.Size = New System.Drawing.Size(257, 287)
        Me.lvwSelectedGroups.SmallImageList = Me.imgGroup
        Me.lvwSelectedGroups.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwSelectedGroups.TabIndex = 97
        Me.lvwSelectedGroups.UseCompatibleStateImageBehavior = False
        Me.lvwSelectedGroups.View = System.Windows.Forms.View.Details
        '
        '_lvwSelectedGroups_ColumnHeader_1
        '
        Me._lvwSelectedGroups_ColumnHeader_1.Width = 167
        '
        'lvwGroups
        '
        Me.lvwGroups.BackColor = System.Drawing.SystemColors.Window
        Me.lvwGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwGroups_ColumnHeader_1})
        Me.lvwGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwGroups.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwGroups.Location = New System.Drawing.Point(24, 48)
        Me.lvwGroups.Name = "lvwGroups"
        Me.lvwGroups.Size = New System.Drawing.Size(257, 287)
        Me.lvwGroups.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwGroups.TabIndex = 91
        Me.lvwGroups.UseCompatibleStateImageBehavior = False
        Me.lvwGroups.View = System.Windows.Forms.View.Details
        '
        '_lvwGroups_ColumnHeader_1
        '
        Me._lvwGroups_ColumnHeader_1.Width = 167
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(360, 30)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(116, 21)
        Me.Label10.TabIndex = 96
        Me.Label10.Text = "Selected Groups:"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(24, 30)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(116, 21)
        Me.Label11.TabIndex = 90
        Me.Label11.Text = "Available Groups:"
        '
        'chkbackgroundjobforfailure
        '
        Me.chkbackgroundjobforfailure.BackColor = System.Drawing.SystemColors.Control
        Me.chkbackgroundjobforfailure.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkbackgroundjobforfailure.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkbackgroundjobforfailure.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkbackgroundjobforfailure.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkbackgroundjobforfailure.Location = New System.Drawing.Point(12, 7)
        Me.chkbackgroundjobforfailure.Name = "chkbackgroundjobforfailure"
        Me.chkbackgroundjobforfailure.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkbackgroundjobforfailure.Size = New System.Drawing.Size(220, 29)
        Me.chkbackgroundjobforfailure.TabIndex = 3
        Me.chkbackgroundjobforfailure.Text = "Schedule in Background Job For Failure"
        Me.chkbackgroundjobforfailure.UseVisualStyleBackColor = False
        '
        'Ctx_mnuSupervisor
        '
        Me.Ctx_mnuSupervisor.Name = "Ctx_mnuSupervisor"
        Me.Ctx_mnuSupervisor.Size = New System.Drawing.Size(61, 4)
        '
        'mnuSupervisor
        '
        Me.mnuSupervisor.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSuper})
        Me.mnuSupervisor.Enabled = False
        Me.mnuSupervisor.Name = "mnuSupervisor"
        Me.mnuSupervisor.Size = New System.Drawing.Size(74, 20)
        Me.mnuSupervisor.Text = "Supervisor"
        Me.mnuSupervisor.Visible = False
        '
        'mnuSuper
        '
        Me.mnuSuper.Name = "mnuSuper"
        Me.mnuSuper.Size = New System.Drawing.Size(129, 22)
        Me.mnuSuper.Text = "Supervisor"
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSupervisor})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(1018, 24)
        Me.MainMenu1.TabIndex = 42
        Me.MainMenu1.Visible = False
        '
        'FrmExternalWorkFlowConfiguration
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(668, 438)
        Me.Controls.Add(Me.Frame4)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.chkbackgroundjobforfailure)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(10, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmExternalWorkFlowConfiguration"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "External Work Flow Configuration"
        Me.Frame4.ResumeLayout(False)
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents mnuSupervisor As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSuper As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
#End Region
End Class