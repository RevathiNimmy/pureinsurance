<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTasks
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
        InitializeComponent()
        lvwAll_InitializeColumnKeys()
        lvwContents_InitializeColumnKeys()
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
    Public WithEvents imgGroup As System.Windows.Forms.ImageList
    Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTasks))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNewTask = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdDeleteTasks = New System.Windows.Forms.Button
        Me.cmdAddTasks = New System.Windows.Forms.Button
        Me.cmdAddAllTasks = New System.Windows.Forms.Button
        Me.cmdDeleteAllTasks = New System.Windows.Forms.Button
        Me.imgGroup = New System.Windows.Forms.ImageList(Me.components)
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.tabTask = New System.Windows.Forms.TabControl
        Me._tabTask_TabPage0 = New System.Windows.Forms.TabPage
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.lvwContents = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.lvwAll = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.lblAll = New System.Windows.Forms.Label
        Me.lblContents = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabTask.SuspendLayout()
        Me._tabTask_TabPage0.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNewTask
        '
        Me.cmdNewTask.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewTask.Location = New System.Drawing.Point(9, 7)
        Me.cmdNewTask.Name = "cmdNewTask"
        Me.cmdNewTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewTask.Size = New System.Drawing.Size(73, 22)
        Me.cmdNewTask.TabIndex = 16
        Me.cmdNewTask.Text = "&New Task"
        Me.cmdNewTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdNewTask, "Enter New Task")
        Me.cmdNewTask.UseVisualStyleBackColor = False
        Me.cmdNewTask.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(537, 7)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 15
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdHelp, "Help")
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(457, 7)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 14
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdCancel, "Cancel changes and return to previous screen")
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(377, 7)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 13
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOK, "Accept Changes and return to previous screen")
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdDeleteTasks
        '
        Me.cmdDeleteTasks.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cmdDeleteTasks.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteTasks.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteTasks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteTasks.Location = New System.Drawing.Point(11, 186)
        Me.cmdDeleteTasks.Name = "cmdDeleteTasks"
        Me.cmdDeleteTasks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteTasks.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteTasks.TabIndex = 9
        Me.cmdDeleteTasks.Text = "&<- Tasks"
        Me.cmdDeleteTasks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteTasks, "Unselect chosen task groups")
        Me.cmdDeleteTasks.UseVisualStyleBackColor = False
        '
        'cmdAddTasks
        '
        Me.cmdAddTasks.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cmdAddTasks.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddTasks.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddTasks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddTasks.Location = New System.Drawing.Point(11, 74)
        Me.cmdAddTasks.Name = "cmdAddTasks"
        Me.cmdAddTasks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddTasks.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddTasks.TabIndex = 6
        Me.cmdAddTasks.Text = "Tasks -&>"
        Me.cmdAddTasks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAddTasks, "Select all available task groups")
        Me.cmdAddTasks.UseVisualStyleBackColor = False
        '
        'cmdAddAllTasks
        '
        Me.cmdAddAllTasks.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cmdAddAllTasks.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddAllTasks.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddAllTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddAllTasks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddAllTasks.Location = New System.Drawing.Point(11, 114)
        Me.cmdAddAllTasks.Name = "cmdAddAllTasks"
        Me.cmdAddAllTasks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddAllTasks.Size = New System.Drawing.Size(73, 22)
        Me.cmdAddAllTasks.TabIndex = 7
        Me.cmdAddAllTasks.Text = "Tasks ->>"
        Me.cmdAddAllTasks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdAddAllTasks, "Select all available task groups")
        Me.cmdAddAllTasks.UseVisualStyleBackColor = False
        '
        'cmdDeleteAllTasks
        '
        Me.cmdDeleteAllTasks.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cmdDeleteAllTasks.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAllTasks.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAllTasks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAllTasks.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAllTasks.Location = New System.Drawing.Point(11, 226)
        Me.cmdDeleteAllTasks.Name = "cmdDeleteAllTasks"
        Me.cmdDeleteAllTasks.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAllTasks.Size = New System.Drawing.Size(73, 22)
        Me.cmdDeleteAllTasks.TabIndex = 8
        Me.cmdDeleteAllTasks.Text = "<<- Tasks"
        Me.cmdDeleteAllTasks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDeleteAllTasks, "Unselect all task groups")
        Me.cmdDeleteAllTasks.UseVisualStyleBackColor = False
        '
        'imgGroup
        '
        Me.imgGroup.ImageStream = CType(resources.GetObject("imgGroup.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgGroup.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgGroup.Images.SetKeyName(0, "task")
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(248, 384)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 13
        Me.uctPMResizer1.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmdNewTask)
        Me.Panel1.Controls.Add(Me.cmdHelp)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 384)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(619, 32)
        Me.Panel1.TabIndex = 14
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.tabTask)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(619, 384)
        Me.Panel2.TabIndex = 15
        '
        'tabTask
        '
        Me.tabTask.Controls.Add(Me._tabTask_TabPage0)
        Me.tabTask.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabTask.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabTask.ItemSize = New System.Drawing.Size(199, 18)
        Me.tabTask.Location = New System.Drawing.Point(0, 0)
        Me.tabTask.Multiline = True
        Me.tabTask.Name = "tabTask"
        Me.tabTask.SelectedIndex = 0
        Me.tabTask.Size = New System.Drawing.Size(619, 384)
        Me.tabTask.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabTask.TabIndex = 1
        Me.tabTask.TabStop = False
        '
        '_tabTask_TabPage0
        '
        Me._tabTask_TabPage0.Controls.Add(Me.Panel6)
        Me._tabTask_TabPage0.Controls.Add(Me.Panel5)
        Me._tabTask_TabPage0.Controls.Add(Me.Panel4)
        Me._tabTask_TabPage0.Controls.Add(Me.Panel3)
        Me._tabTask_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabTask_TabPage0.Name = "_tabTask_TabPage0"
        Me._tabTask_TabPage0.Size = New System.Drawing.Size(611, 358)
        Me._tabTask_TabPage0.TabIndex = 0
        Me._tabTask_TabPage0.Text = "1 - Task Details"
        '
        'Panel6
        '
        Me.Panel6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Panel6.Controls.Add(Me.cmdDeleteTasks)
        Me.Panel6.Controls.Add(Me.cmdAddTasks)
        Me.Panel6.Controls.Add(Me.cmdAddAllTasks)
        Me.Panel6.Controls.Add(Me.cmdDeleteAllTasks)
        Me.Panel6.Location = New System.Drawing.Point(258, 36)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(94, 322)
        Me.Panel6.TabIndex = 10
        '
        'Panel5
        '
        Me.Panel5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Panel5.Controls.Add(Me.lvwContents)
        Me.Panel5.Location = New System.Drawing.Point(352, 36)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(259, 322)
        Me.Panel5.TabIndex = 9
        '
        'lvwContents
        '
        Me.lvwContents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwContents.BackColor = System.Drawing.SystemColors.Window
        Me.lvwContents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lvwContents.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwContents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwContents.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwContents.HideSelection = False
        Me.lvwContents.LargeImageList = Me.imgGroup
        Me.lvwContents.Location = New System.Drawing.Point(0, 0)
        Me.lvwContents.Name = "lvwContents"
        Me.lvwContents.Size = New System.Drawing.Size(259, 322)
        Me.lvwContents.SmallImageList = Me.imgGroup
        Me.lvwContents.TabIndex = 7
        Me.lvwContents.UseCompatibleStateImageBehavior = False
        Me.lvwContents.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = ""
        Me.ColumnHeader3.Width = 110
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = ""
        Me.ColumnHeader4.Width = 140
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel4.Controls.Add(Me.lvwAll)
        Me.Panel4.Location = New System.Drawing.Point(0, 36)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(239, 322)
        Me.Panel4.TabIndex = 8
        '
        'lvwAll
        '
        Me.lvwAll.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwAll.BackColor = System.Drawing.SystemColors.Window
        Me.lvwAll.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvwAll.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwAll.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwAll.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwAll.HideSelection = False
        Me.lvwAll.LargeImageList = Me.imgGroup
        Me.lvwAll.Location = New System.Drawing.Point(0, 0)
        Me.lvwAll.Name = "lvwAll"
        Me.lvwAll.Size = New System.Drawing.Size(239, 322)
        Me.lvwAll.SmallImageList = Me.imgGroup
        Me.lvwAll.TabIndex = 2
        Me.lvwAll.UseCompatibleStateImageBehavior = False
        Me.lvwAll.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = ""
        Me.ColumnHeader1.Width = 105
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = ""
        Me.ColumnHeader2.Width = 130
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lblAll)
        Me.Panel3.Controls.Add(Me.lblContents)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(611, 36)
        Me.Panel3.TabIndex = 7
        '
        'lblAll
        '
        Me.lblAll.BackColor = System.Drawing.SystemColors.Control
        Me.lblAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAll.Location = New System.Drawing.Point(2, 10)
        Me.lblAll.Name = "lblAll"
        Me.lblAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAll.Size = New System.Drawing.Size(145, 17)
        Me.lblAll.TabIndex = 12
        Me.lblAll.Text = "Available Tasks"
        '
        'lblContents
        '
        Me.lblContents.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblContents.BackColor = System.Drawing.SystemColors.Control
        Me.lblContents.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContents.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContents.Location = New System.Drawing.Point(358, 10)
        Me.lblContents.Name = "lblContents"
        Me.lblContents.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContents.Size = New System.Drawing.Size(145, 17)
        Me.lblContents.TabIndex = 13
        Me.lblContents.Text = "Selected Tasks"
        '
        'frmTasks
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(619, 416)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(627, 443)
        Me.Name = "frmTasks"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmTasks"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.tabTask.ResumeLayout(False)
        Me._tabTask_TabPage0.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Sub lvwAll_InitializeColumnKeys()
        'Me._lvwAll_ColumnHeader_1.Name = ""
        'Me._lvwAll_ColumnHeader_2.Name = ""
    End Sub
    Sub lvwContents_InitializeColumnKeys()
        'Me._lvwContents_ColumnHeader_1.Name = ""
        'Me._lvwContents_ColumnHeader_2.Name = ""
    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents cmdNewTask As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents tabTask As System.Windows.Forms.TabControl
    Private WithEvents _tabTask_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Public WithEvents cmdDeleteTasks As System.Windows.Forms.Button
    Public WithEvents cmdAddTasks As System.Windows.Forms.Button
    Public WithEvents cmdAddAllTasks As System.Windows.Forms.Button
    Public WithEvents cmdDeleteAllTasks As System.Windows.Forms.Button
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Public WithEvents lvwContents As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Public WithEvents lvwAll As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Public WithEvents lblAll As System.Windows.Forms.Label
    Public WithEvents lblContents As System.Windows.Forms.Label
#End Region
End Class