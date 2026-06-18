<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface

#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    '<System.Diagnostics.DebuggerNonUserCode()> _
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
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public WithEvents cmdOk As System.Windows.Forms.Button
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents lvwEvents As System.Windows.Forms.ListView
    Public WithEvents cmdView As System.Windows.Forms.Button
    Public WithEvents chkSiriusOnly As System.Windows.Forms.CheckBox
    Public WithEvents cmdRefresh As System.Windows.Forms.Button
    Public WithEvents txtMachine As System.Windows.Forms.TextBox
    Public WithEvents cmdGo As System.Windows.Forms.Button
    Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMain As System.Windows.Forms.TabControl
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.lvwEvents = New System.Windows.Forms.ListView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdView = New System.Windows.Forms.Button()
        Me.chkSiriusOnly = New System.Windows.Forms.CheckBox()
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.txtMachine = New System.Windows.Forms.TextBox()
        Me.cmdGo = New System.Windows.Forms.Button()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(502, 305)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(69, 25)
        Me.cmdOk.TabIndex = 7
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOk, "Exit")
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'lvwEvents
        '
        Me.lvwEvents.AllowColumnReorder = True
        Me.lvwEvents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwEvents.BackColor = System.Drawing.SystemColors.Window
        Me.lvwEvents.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwEvents.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwEvents.FullRowSelect = True
        Me.lvwEvents.HideSelection = False
        Me.lvwEvents.LargeImageList = Me.ImageList1
        Me.lvwEvents.Location = New System.Drawing.Point(12, 31)
        Me.lvwEvents.MultiSelect = False
        Me.lvwEvents.Name = "lvwEvents"
        Me.lvwEvents.Size = New System.Drawing.Size(461, 219)
        Me.lvwEvents.SmallImageList = Me.ImageList1
        Me.lvwEvents.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.lvwEvents, "Event list: Dbl-click to view details")
        Me.lvwEvents.UseCompatibleStateImageBehavior = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "Info")
        Me.ImageList1.Images.SetKeyName(1, "DOWN")
        Me.ImageList1.Images.SetKeyName(2, "UP")
        Me.ImageList1.Images.SetKeyName(3, "Error")
        Me.ImageList1.Images.SetKeyName(4, "Warning")
        '
        'cmdView
        '
        Me.cmdView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(480, 28)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(69, 25)
        Me.cmdView.TabIndex = 5
        Me.cmdView.Text = "&View"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdView, "Show event log details")
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'chkSiriusOnly
        '
        Me.chkSiriusOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkSiriusOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSiriusOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSiriusOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSiriusOnly.Location = New System.Drawing.Point(12, 3)
        Me.chkSiriusOnly.Name = "chkSiriusOnly"
        Me.chkSiriusOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSiriusOnly.Size = New System.Drawing.Size(118, 29)
        Me.chkSiriusOnly.TabIndex = 1
        Me.chkSiriusOnly.Text = "Pure only"
        Me.ToolTip1.SetToolTip(Me.chkSiriusOnly, "Filter non-Pure events")
        Me.chkSiriusOnly.UseVisualStyleBackColor = False
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefresh.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefresh.Location = New System.Drawing.Point(480, 225)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefresh.Size = New System.Drawing.Size(69, 25)
        Me.cmdRefresh.TabIndex = 6
        Me.cmdRefresh.Text = "&Refresh"
        Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdRefresh, "Update the event list")
        Me.cmdRefresh.UseVisualStyleBackColor = False
        '
        'txtMachine
        '
        Me.txtMachine.AcceptsReturn = True
        Me.txtMachine.BackColor = System.Drawing.SystemColors.Window
        Me.txtMachine.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMachine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMachine.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMachine.Location = New System.Drawing.Point(185, 5)
        Me.txtMachine.MaxLength = 0
        Me.txtMachine.Name = "txtMachine"
        Me.txtMachine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMachine.Size = New System.Drawing.Size(145, 20)
        Me.txtMachine.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.txtMachine, "Target machine (blank for local machine)")
        '
        'cmdGo
        '
        Me.cmdGo.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGo.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGo.Location = New System.Drawing.Point(336, 5)
        Me.cmdGo.Name = "cmdGo"
        Me.cmdGo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGo.Size = New System.Drawing.Size(29, 21)
        Me.cmdGo.TabIndex = 3
        Me.cmdGo.Text = "&Go"
        Me.cmdGo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdGo, "Refresh after machine change")
        Me.cmdGo.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(558, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(563, 291)
        Me.tabMain.TabIndex = 0
        Me.tabMain.TabStop = False
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.Label1)
        Me._tabMain_TabPage0.Controls.Add(Me.lvwEvents)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdView)
        Me._tabMain_TabPage0.Controls.Add(Me.chkSiriusOnly)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdRefresh)
        Me._tabMain_TabPage0.Controls.Add(Me.txtMachine)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdGo)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(555, 265)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Events"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(136, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(49, 23)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Machine:"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(575, 331)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.tabMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PMEventLogViewer"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._tabMain_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region
End Class