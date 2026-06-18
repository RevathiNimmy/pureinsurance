<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
        InitializecmdButton()
        InitializeLabel1()
        tabMainTabPreviousTab = tabMainTab.SelectedIndex
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


    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Private WithEvents _Label1_2 As System.Windows.Forms.Label
    Private WithEvents _Label1_1 As System.Windows.Forms.Label
    Private WithEvents _Label1_0 As System.Windows.Forms.Label
    Private WithEvents _Label1_3 As System.Windows.Forms.Label
    Private WithEvents _Label1_4 As System.Windows.Forms.Label
    Private WithEvents _Label1_5 As System.Windows.Forms.Label
    Public WithEvents lvwResvDefn As System.Windows.Forms.ListView


    Private WithEvents _cmdButton_0 As System.Windows.Forms.Button
    Private WithEvents _cmdButton_1 As System.Windows.Forms.Button
    Private WithEvents _cmdButton_2 As System.Windows.Forms.Button
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents txtReserveType As System.Windows.Forms.TextBox
    Public WithEvents YesNoCheck1 As System.Windows.Forms.CheckBox
    Public WithEvents chkIsExcess As System.Windows.Forms.CheckBox
    Public WithEvents chkIsIndemnity As System.Windows.Forms.CheckBox
    Public WithEvents chkIsExpense As System.Windows.Forms.CheckBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public Label1(5) As System.Windows.Forms.Label
    Public cmdButton(2) As System.Windows.Forms.Button
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me._Label1_2 = New System.Windows.Forms.Label
        Me._Label1_1 = New System.Windows.Forms.Label
        Me._Label1_0 = New System.Windows.Forms.Label
        Me._Label1_3 = New System.Windows.Forms.Label
        Me._Label1_4 = New System.Windows.Forms.Label
        Me._Label1_5 = New System.Windows.Forms.Label
        Me.lvwResvDefn = New System.Windows.Forms.ListView
        Me._cmdButton_0 = New System.Windows.Forms.Button
        Me._cmdButton_1 = New System.Windows.Forms.Button
        Me._cmdButton_2 = New System.Windows.Forms.Button
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtReserveType = New System.Windows.Forms.TextBox
        Me.YesNoCheck1 = New System.Windows.Forms.CheckBox
        Me.chkIsExcess = New System.Windows.Forms.CheckBox
        Me.chkIsIndemnity = New System.Windows.Forms.CheckBox
        Me.chkIsExpense = New System.Windows.Forms.CheckBox
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(445, 369)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(365, 368)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(282, 367)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(55, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(511, 357)
        Me.tabMainTab.TabIndex = 0
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me._Label1_2)
        Me._tabMainTab_TabPage0.Controls.Add(Me._Label1_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._Label1_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me._Label1_3)
        Me._tabMainTab_TabPage0.Controls.Add(Me._Label1_4)
        Me._tabMainTab_TabPage0.Controls.Add(Me._Label1_5)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwResvDefn)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdButton_0)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdButton_1)
        Me._tabMainTab_TabPage0.Controls.Add(Me._cmdButton_2)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtReserveType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.YesNoCheck1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIsExcess)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIsIndemnity)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIsExpense)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(503, 331)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        '_Label1_2
        '
        Me._Label1_2.AutoSize = True
        Me._Label1_2.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_2.Location = New System.Drawing.Point(16, 84)
        Me._Label1_2.Name = "_Label1_2"
        Me._Label1_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_2.Size = New System.Drawing.Size(0, 13)
        Me._Label1_2.TabIndex = 4
        '
        '_Label1_1
        '
        Me._Label1_1.AutoSize = True
        Me._Label1_1.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_1.Location = New System.Drawing.Point(16, 51)
        Me._Label1_1.Name = "_Label1_1"
        Me._Label1_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_1.Size = New System.Drawing.Size(0, 13)
        Me._Label1_1.TabIndex = 2
        '
        '_Label1_0
        '
        Me._Label1_0.AutoSize = True
        Me._Label1_0.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_0.Location = New System.Drawing.Point(16, 19)
        Me._Label1_0.Name = "_Label1_0"
        Me._Label1_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_0.Size = New System.Drawing.Size(0, 13)
        Me._Label1_0.TabIndex = 0
        '
        '_Label1_3
        '
        Me._Label1_3.AutoSize = True
        Me._Label1_3.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_3.Location = New System.Drawing.Point(168, 84)
        Me._Label1_3.Name = "_Label1_3"
        Me._Label1_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_3.Size = New System.Drawing.Size(0, 13)
        Me._Label1_3.TabIndex = 6
        '
        '_Label1_4
        '
        Me._Label1_4.AutoSize = True
        Me._Label1_4.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_4.Location = New System.Drawing.Point(276, 84)
        Me._Label1_4.Name = "_Label1_4"
        Me._Label1_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_4.Size = New System.Drawing.Size(0, 13)
        Me._Label1_4.TabIndex = 8
        '
        '_Label1_5
        '
        Me._Label1_5.AutoSize = True
        Me._Label1_5.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._Label1_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_5.Location = New System.Drawing.Point(400, 84)
        Me._Label1_5.Name = "_Label1_5"
        Me._Label1_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_5.Size = New System.Drawing.Size(0, 13)
        Me._Label1_5.TabIndex = 10
        '
        'lvwResvDefn
        '
        Me.lvwResvDefn.BackColor = System.Drawing.SystemColors.Window
        Me.lvwResvDefn.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwResvDefn.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwResvDefn.HideSelection = False
        Me.lvwResvDefn.Location = New System.Drawing.Point(16, 117)
        Me.lvwResvDefn.Name = "lvwResvDefn"
        Me.lvwResvDefn.Size = New System.Drawing.Size(401, 201)
        Me.lvwResvDefn.TabIndex = 12
        Me.lvwResvDefn.UseCompatibleStateImageBehavior = False
        Me.lvwResvDefn.View = System.Windows.Forms.View.Details
        '
        '_cmdButton_0
        '
        Me._cmdButton_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_0.Enabled = False
        Me._cmdButton_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_0.Location = New System.Drawing.Point(424, 117)
        Me._cmdButton_0.Name = "_cmdButton_0"
        Me._cmdButton_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_0.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_0.TabIndex = 13
        Me._cmdButton_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_0.UseVisualStyleBackColor = False
        '
        '_cmdButton_1
        '
        Me._cmdButton_1.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_1.Enabled = False
        Me._cmdButton_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_1.Location = New System.Drawing.Point(424, 149)
        Me._cmdButton_1.Name = "_cmdButton_1"
        Me._cmdButton_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_1.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_1.TabIndex = 14
        Me._cmdButton_1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_1.UseVisualStyleBackColor = False
        '
        '_cmdButton_2
        '
        Me._cmdButton_2.BackColor = System.Drawing.SystemColors.Control
        Me._cmdButton_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdButton_2.Enabled = False
        Me._cmdButton_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdButton_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdButton_2.Location = New System.Drawing.Point(424, 181)
        Me._cmdButton_2.Name = "_cmdButton_2"
        Me._cmdButton_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdButton_2.Size = New System.Drawing.Size(73, 22)
        Me._cmdButton_2.TabIndex = 15
        Me._cmdButton_2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdButton_2.UseVisualStyleBackColor = False
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(144, 48)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(249, 20)
        Me.txtDescription.TabIndex = 3
        '
        'txtReserveType
        '
        Me.txtReserveType.AcceptsReturn = True
        Me.txtReserveType.BackColor = System.Drawing.SystemColors.Window
        Me.txtReserveType.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReserveType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReserveType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReserveType.Location = New System.Drawing.Point(144, 16)
        Me.txtReserveType.MaxLength = 0
        Me.txtReserveType.Name = "txtReserveType"
        Me.txtReserveType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReserveType.Size = New System.Drawing.Size(121, 20)
        Me.txtReserveType.TabIndex = 1
        '
        'YesNoCheck1
        '
        Me.YesNoCheck1.BackColor = System.Drawing.SystemColors.Control
        Me.YesNoCheck1.Cursor = System.Windows.Forms.Cursors.Default
        Me.YesNoCheck1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.YesNoCheck1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.YesNoCheck1.Location = New System.Drawing.Point(136, 84)
        Me.YesNoCheck1.Name = "YesNoCheck1"
        Me.YesNoCheck1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.YesNoCheck1.Size = New System.Drawing.Size(17, 17)
        Me.YesNoCheck1.TabIndex = 5
        Me.YesNoCheck1.UseVisualStyleBackColor = False
        '
        'chkIsExcess
        '
        Me.chkIsExcess.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsExcess.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsExcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsExcess.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsExcess.Location = New System.Drawing.Point(251, 84)
        Me.chkIsExcess.Name = "chkIsExcess"
        Me.chkIsExcess.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsExcess.Size = New System.Drawing.Size(17, 17)
        Me.chkIsExcess.TabIndex = 7
        Me.chkIsExcess.UseVisualStyleBackColor = False
        '
        'chkIsIndemnity
        '
        Me.chkIsIndemnity.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsIndemnity.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsIndemnity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsIndemnity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsIndemnity.Location = New System.Drawing.Point(371, 84)
        Me.chkIsIndemnity.Name = "chkIsIndemnity"
        Me.chkIsIndemnity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsIndemnity.Size = New System.Drawing.Size(17, 17)
        Me.chkIsIndemnity.TabIndex = 9
        Me.chkIsIndemnity.UseVisualStyleBackColor = False
        '
        'chkIsExpense
        '
        Me.chkIsExpense.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsExpense.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsExpense.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsExpense.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsExpense.Location = New System.Drawing.Point(484, 84)
        Me.chkIsExpense.Name = "chkIsExpense"
        Me.chkIsExpense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsExpense.Size = New System.Drawing.Size(17, 17)
        Me.chkIsExpense.TabIndex = 11
        Me.chkIsExpense.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(521, 394)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializecmdButton()
        Me.cmdButton(2) = _cmdButton_2
        Me.cmdButton(1) = _cmdButton_1
        Me.cmdButton(0) = _cmdButton_0
    End Sub
    Sub InitializeLabel1()
        Me.Label1(5) = _Label1_5
        Me.Label1(4) = _Label1_4
        Me.Label1(3) = _Label1_3
        Me.Label1(0) = _Label1_0
        Me.Label1(1) = _Label1_1
        Me.Label1(2) = _Label1_2
    End Sub
#End Region
End Class