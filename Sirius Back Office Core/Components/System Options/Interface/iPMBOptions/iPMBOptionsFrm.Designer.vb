<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializetxtOption()
		InitializeoptOption()
		InitializelblOption()
		InitializecmdOption()
		InitializechkOption()
        InitializecboOption()
        InitializeUctOption()
        InitializeGrpBoxOption()
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
    Public WithEvents tabOptions_Tabs As System.Windows.Forms.TabControl.TabPageCollection
    Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    Public dlgQASOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public cdgGenericOpen As System.Windows.Forms.OpenFileDialog
    Public WithEvents imgSplitter1 As System.Windows.Forms.PictureBox
    Public cboOption(0) As System.Windows.Forms.ComboBox
    Public chkOption(0) As System.Windows.Forms.CheckBox
    Public cmdOption(0) As System.Windows.Forms.Button
    Public lblOption(0) As System.Windows.Forms.Label
    Public optOption(0) As System.Windows.Forms.RadioButton
    Public txtOption(0) As System.Windows.Forms.TextBox
    Public UctCompiledRuleOption(0) As uctCompiledRule.uctCompiledRule
    Public grpBoxOption(0) As System.Windows.Forms.GroupBox
    Public dlgQAS As System.Windows.Forms.OpenFileDialog
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.dlgQASOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.cdgGenericOpen = New System.Windows.Forms.OpenFileDialog()
        Me.imgSplitter1 = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.tabOptions = New System.Windows.Forms.TabControl()
        Me._tabOptions_Tab1 = New System.Windows.Forms.TabPage()
        Me.picContainer = New System.Windows.Forms.PictureBox()
        Me._cboOption_0 = New System.Windows.Forms.ComboBox()
        Me._chkOption_0 = New System.Windows.Forms.CheckBox()
        Me._txtOption_0 = New System.Windows.Forms.TextBox()
        Me._optOption_0 = New System.Windows.Forms.RadioButton()
        Me._cmdOption_0 = New System.Windows.Forms.Button()
        Me._lblOption_0 = New System.Windows.Forms.Label()
        Me._UctCompiledRuleOption_0 = New uctCompiledRule.uctCompiledRule()
        Me._grpBoxOption_0 = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.tvwTabs = New System.Windows.Forms.TreeView()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.sbrMain = New System.Windows.Forms.StatusStrip()
        Me.imgSplitter = New System.Windows.Forms.Splitter()
        CType(Me.imgSplitter1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.tabOptions.SuspendLayout()
        Me._tabOptions_Tab1.SuspendLayout()
        CType(Me.picContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.picContainer.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        '
        'dlgQASOpen
        '
        Me.dlgQASOpen.FileName = "qaddress.ini"
        Me.dlgQASOpen.Filter = "Ini files (*.ini)|*.ini"
        Me.dlgQASOpen.Title = "Locate qaddress.ini"
        '
        'imgSplitter1
        '
        Me.imgSplitter1.Cursor = System.Windows.Forms.Cursors.SizeWE
        Me.imgSplitter1.Location = New System.Drawing.Point(42, 547)
        Me.imgSplitter1.Name = "imgSplitter1"
        Me.imgSplitter1.Size = New System.Drawing.Size(49, 17)
        Me.imgSplitter1.TabIndex = 13
        Me.imgSplitter1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.tabOptions)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(191, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(655, 664)
        Me.Panel1.TabIndex = 14
        '
        'tabOptions
        '
        Me.tabOptions.Controls.Add(Me._tabOptions_Tab1)
        Me.tabOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabOptions.Location = New System.Drawing.Point(0, 0)
        Me.tabOptions.Name = "tabOptions"
        Me.tabOptions.SelectedIndex = 0
        Me.tabOptions.Size = New System.Drawing.Size(655, 664)
        Me.tabOptions.TabIndex = 16
        '
        '_tabOptions_Tab1
        '
        Me._tabOptions_Tab1.Controls.Add(Me.picContainer)
        Me._tabOptions_Tab1.Location = New System.Drawing.Point(4, 22)
        Me._tabOptions_Tab1.Name = "_tabOptions_Tab1"
        Me._tabOptions_Tab1.Size = New System.Drawing.Size(647, 638)
        Me._tabOptions_Tab1.TabIndex = 0
        Me._tabOptions_Tab1.Text = "Tab 0"
        '
        'picContainer
        '
        Me.picContainer.BackColor = System.Drawing.SystemColors.Control
        Me.picContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picContainer.Controls.Add(Me._cboOption_0)
        Me.picContainer.Controls.Add(Me._chkOption_0)
        Me.picContainer.Controls.Add(Me._txtOption_0)
        Me.picContainer.Controls.Add(Me._optOption_0)
        Me.picContainer.Controls.Add(Me._cmdOption_0)
        Me.picContainer.Controls.Add(Me._lblOption_0)
        Me.picContainer.Controls.Add(Me._UctCompiledRuleOption_0)
        Me.picContainer.Controls.Add(Me._grpBoxOption_0)
        Me.picContainer.Cursor = System.Windows.Forms.Cursors.Default
        Me.picContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picContainer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.picContainer.Location = New System.Drawing.Point(0, 0)
        Me.picContainer.Name = "picContainer"
        Me.picContainer.Size = New System.Drawing.Size(647, 638)
        Me.picContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picContainer.TabIndex = 2
        Me.picContainer.TabStop = False
        '
        '_cboOption_0
        '
        Me._cboOption_0.BackColor = System.Drawing.SystemColors.Window
        Me._cboOption_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboOption_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboOption_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboOption_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboOption_0.Location = New System.Drawing.Point(18, 72)
        Me._cboOption_0.Name = "_cboOption_0"
        Me._cboOption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboOption_0.Size = New System.Drawing.Size(129, 21)
        Me._cboOption_0.TabIndex = 10
        Me._cboOption_0.Visible = False
        '
        '_chkOption_0
        '
        Me._chkOption_0.BackColor = System.Drawing.SystemColors.Control
        Me._chkOption_0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._chkOption_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._chkOption_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._chkOption_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._chkOption_0.Location = New System.Drawing.Point(32, 43)
        Me._chkOption_0.Name = "_chkOption_0"
        Me._chkOption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._chkOption_0.Size = New System.Drawing.Size(17, 17)
        Me._chkOption_0.TabIndex = 9
        Me._chkOption_0.UseVisualStyleBackColor = False
        Me._chkOption_0.Visible = False
        '
        '_txtOption_0
        '
        Me._txtOption_0.AcceptsReturn = True
        Me._txtOption_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtOption_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtOption_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtOption_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtOption_0.Location = New System.Drawing.Point(15, 104)
        Me._txtOption_0.MaxLength = 0
        Me._txtOption_0.Name = "_txtOption_0"
        Me._txtOption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtOption_0.Size = New System.Drawing.Size(129, 20)
        Me._txtOption_0.TabIndex = 8
        Me._txtOption_0.Visible = False
        '
        '_optOption_0
        '
        Me._optOption_0.BackColor = System.Drawing.SystemColors.Control
        Me._optOption_0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._optOption_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optOption_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optOption_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optOption_0.Location = New System.Drawing.Point(93, 45)
        Me._optOption_0.Name = "_optOption_0"
        Me._optOption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optOption_0.Size = New System.Drawing.Size(17, 17)
        Me._optOption_0.TabIndex = 7
        Me._optOption_0.TabStop = True
        Me._optOption_0.Text = "Option1"
        Me._optOption_0.UseVisualStyleBackColor = False
        Me._optOption_0.Visible = False
        '
        '_cmdOption_0
        '
        Me._cmdOption_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdOption_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdOption_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdOption_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdOption_0.Location = New System.Drawing.Point(15, 138)
        Me._cmdOption_0.Name = "_cmdOption_0"
        Me._cmdOption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdOption_0.Size = New System.Drawing.Size(73, 23)
        Me._cmdOption_0.TabIndex = 6
        Me._cmdOption_0.Text = "Command"
        Me._cmdOption_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdOption_0.UseVisualStyleBackColor = False
        Me._cmdOption_0.Visible = False
        '
        '_lblOption_0
        '
        Me._lblOption_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblOption_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblOption_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblOption_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblOption_0.Location = New System.Drawing.Point(21, 12)
        Me._lblOption_0.Name = "_lblOption_0"
        Me._lblOption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblOption_0.Size = New System.Drawing.Size(131, 17)
        Me._lblOption_0.TabIndex = 11
        Me._lblOption_0.Text = "lblOption"
        Me._lblOption_0.Visible = False
        '
        '_UctCompiledRuleOption_0
        '
        Me._UctCompiledRuleOption_0.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me._UctCompiledRuleOption_0.bEnterOnlyAssemblyName = False
        Me._UctCompiledRuleOption_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._UctCompiledRuleOption_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._UctCompiledRuleOption_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._UctCompiledRuleOption_0.Location = New System.Drawing.Point(17, 184)
        Me._UctCompiledRuleOption_0.Name = "_UctCompiledRuleOption_0"
        Me._UctCompiledRuleOption_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._UctCompiledRuleOption_0.Size = New System.Drawing.Size(226, 26)
        Me._UctCompiledRuleOption_0.TabIndex = 12
        Me._UctCompiledRuleOption_0.Visible = False
        '
        '_grpBoxOption_0
        '
        Me._grpBoxOption_0.Location = New System.Drawing.Point(19, 235)
        Me._grpBoxOption_0.Name = "_grpBoxOption_0"
        Me._grpBoxOption_0.Size = New System.Drawing.Size(200, 100)
        Me._grpBoxOption_0.TabIndex = 3
        Me._grpBoxOption_0.TabStop = False
        Me._grpBoxOption_0.Text = "GroupBox1"
        Me._grpBoxOption_0.Visible = False
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.Controls.Add(Me.tvwTabs)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(186, 664)
        Me.Panel2.TabIndex = 3
        '
        'tvwTabs
        '
        Me.tvwTabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwTabs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwTabs.HideSelection = False
        Me.tvwTabs.Indent = 7
        Me.tvwTabs.LabelEdit = True
        Me.tvwTabs.Location = New System.Drawing.Point(0, 0)
        Me.tvwTabs.Name = "tvwTabs"
        Me.tvwTabs.Size = New System.Drawing.Size(186, 664)
        Me.tvwTabs.TabIndex = 3
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 664)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(846, 27)
        Me.Panel3.TabIndex = 16
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.cmdHelp)
        Me.Panel4.Controls.Add(Me.cmdOK)
        Me.Panel4.Controls.Add(Me.cmdCancel)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel4.Location = New System.Drawing.Point(607, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(239, 27)
        Me.Panel4.TabIndex = 19
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(161, 3)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 23)
        Me.cmdHelp.TabIndex = 21
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(2, 3)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 23)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(82, 3)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 23)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'sbrMain
        '
        Me.sbrMain.Location = New System.Drawing.Point(0, 691)
        Me.sbrMain.Name = "sbrMain"
        Me.sbrMain.Size = New System.Drawing.Size(846, 22)
        Me.sbrMain.TabIndex = 21
        Me.sbrMain.Text = "StatusStrip1"
        '
        'imgSplitter
        '
        Me.imgSplitter.Cursor = System.Windows.Forms.Cursors.SizeWE
        Me.imgSplitter.Location = New System.Drawing.Point(186, 0)
        Me.imgSplitter.MinSize = 20
        Me.imgSplitter.Name = "imgSplitter"
        Me.imgSplitter.Size = New System.Drawing.Size(5, 664)
        Me.imgSplitter.TabIndex = 20
        Me.imgSplitter.TabStop = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(846, 713)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.imgSplitter)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.imgSplitter1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.sbrMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "System Options"
        CType(Me.imgSplitter1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.tabOptions.ResumeLayout(False)
        Me._tabOptions_Tab1.ResumeLayout(False)
        CType(Me.picContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.picContainer.ResumeLayout(False)
        Me.picContainer.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub InitializetxtOption()
        Me.txtOption(0) = _txtOption_0
    End Sub
    Sub InitializeoptOption()
        Me.optOption(0) = _optOption_0
    End Sub
    Sub InitializelblOption()
        Me.lblOption(0) = _lblOption_0
    End Sub
    Sub InitializecmdOption()
        Me.cmdOption(0) = _cmdOption_0
    End Sub
    Sub InitializechkOption()
        Me.chkOption(0) = _chkOption_0
    End Sub
    Sub InitializecboOption()
        Me.cboOption(0) = _cboOption_0
    End Sub
    Sub InitializeUctOption()
        Me.UctCompiledRuleOption(0) = _UctCompiledRuleOption_0
    End Sub
    Sub InitializeGrpBoxOption()
        Me.grpBoxOption(0) = _grpBoxOption_0
    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents sbrMain As System.Windows.Forms.StatusStrip
    Public WithEvents tabOptions As System.Windows.Forms.TabControl
    Private WithEvents _tabOptions_Tab1 As System.Windows.Forms.TabPage
    Public WithEvents picContainer As System.Windows.Forms.PictureBox
    Private WithEvents _cboOption_0 As System.Windows.Forms.ComboBox
    Private WithEvents _chkOption_0 As System.Windows.Forms.CheckBox
    Private WithEvents _txtOption_0 As System.Windows.Forms.TextBox
    Private WithEvents _optOption_0 As System.Windows.Forms.RadioButton
    Private WithEvents _cmdOption_0 As System.Windows.Forms.Button
    Private WithEvents _lblOption_0 As System.Windows.Forms.Label
    Public WithEvents tvwTabs As System.Windows.Forms.TreeView
    Friend WithEvents imgSplitter As System.Windows.Forms.Splitter
    Private WithEvents _UctCompiledRuleOption_0 As uctCompiledRule.uctCompiledRule
    Friend WithEvents _grpBoxOption_0 As System.Windows.Forms.GroupBox
#End Region
End Class