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
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents uctPMResizer1 As PMResizerControl.uctPMResizer
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents lblContext As System.Windows.Forms.Label
    Public WithEvents lblSubject As System.Windows.Forms.Label
    Public WithEvents lblFreeText As System.Windows.Forms.Label
    Public WithEvents lblDate As System.Windows.Forms.Label
    Public WithEvents lblUser As System.Windows.Forms.Label
    Public WithEvents lblPriority As System.Windows.Forms.Label
    Public WithEvents lblStatus As System.Windows.Forms.Label
    Public WithEvents cboSubject As System.Windows.Forms.ComboBox
	Public WithEvents cboContext As System.Windows.Forms.ComboBox
	Public WithEvents txtDate As System.Windows.Forms.TextBox
	Public WithEvents txtUser As System.Windows.Forms.TextBox
	Public WithEvents cboPriority As System.Windows.Forms.ComboBox
	Public WithEvents cboStatus As System.Windows.Forms.ComboBox
	Public WithEvents uctRichTextBox1 As uctSIRRTFControl.uctRichTextBox
	Public WithEvents txtclaim As System.Windows.Forms.TextBox
	Public WithEvents cmdOpenClaim As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Private WithEvents listBoxComboBoxHelper1 As Artinsoft.VB6.Gui.ListControlHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uctPMResizer1 = New PMResizerControl.uctPMResizer
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblContext = New System.Windows.Forms.Label
        Me.lblSubject = New System.Windows.Forms.Label
        Me.lblFreeText = New System.Windows.Forms.Label
        Me.lblDate = New System.Windows.Forms.Label
        Me.lblUser = New System.Windows.Forms.Label
        Me.lblPriority = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.txtFreeText = New System.Windows.Forms.TextBox
        Me.cboSubject = New System.Windows.Forms.ComboBox
        Me.cboContext = New System.Windows.Forms.ComboBox
        Me.txtDate = New System.Windows.Forms.TextBox
        Me.txtUser = New System.Windows.Forms.TextBox
        Me.cboPriority = New System.Windows.Forms.ComboBox
        Me.cboStatus = New System.Windows.Forms.ComboBox
        Me.uctRichTextBox1 = New uctSIRRTFControl.uctRichTextBox
        Me.txtclaim = New System.Windows.Forms.TextBox
        Me.cmdOpenClaim = New System.Windows.Forms.Button
        Me.listBoxComboBoxHelper1 = New Artinsoft.VB6.Gui.ListControlHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uctPMResizer1
        '
        Me.uctPMResizer1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer1.Location = New System.Drawing.Point(56, 440)
        Me.uctPMResizer1.Name = "uctPMResizer1"
        Me.uctPMResizer1.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer1.TabIndex = 0
        Me.uctPMResizer1.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(198, 432)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 10
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(358, 432)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 12
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(278, 432)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 11
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(84, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(429, 421)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabMainTab.TabIndex = 13
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblContext)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSubject)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblFreeText)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblUser)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblPriority)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtFreeText)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboSubject)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboContext)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtUser)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboPriority)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctRichTextBox1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtclaim)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdOpenClaim)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(421, 395)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Note Entry"
        '
        'lblContext
        '
        Me.lblContext.AutoSize = True
        Me.lblContext.BackColor = System.Drawing.SystemColors.Control
        Me.lblContext.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblContext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContext.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContext.Location = New System.Drawing.Point(16, 16)
        Me.lblContext.Name = "lblContext"
        Me.lblContext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblContext.Size = New System.Drawing.Size(46, 13)
        Me.lblContext.TabIndex = 0
        Me.lblContext.Text = "Context:"
        '
        'lblSubject
        '
        Me.lblSubject.AutoSize = True
        Me.lblSubject.BackColor = System.Drawing.SystemColors.Control
        Me.lblSubject.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubject.Location = New System.Drawing.Point(16, 40)
        Me.lblSubject.Name = "lblSubject"
        Me.lblSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubject.Size = New System.Drawing.Size(46, 13)
        Me.lblSubject.TabIndex = 2
        Me.lblSubject.Text = "Subject:"
        '
        'lblFreeText
        '
        Me.lblFreeText.BackColor = System.Drawing.SystemColors.Control
        Me.lblFreeText.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFreeText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFreeText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFreeText.Location = New System.Drawing.Point(16, 132)
        Me.lblFreeText.Name = "lblFreeText"
        Me.lblFreeText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFreeText.Size = New System.Drawing.Size(81, 17)
        Me.lblFreeText.TabIndex = 8
        Me.lblFreeText.Text = "Text:"
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDate.Location = New System.Drawing.Point(16, 64)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDate.Size = New System.Drawing.Size(33, 13)
        Me.lblDate.TabIndex = 4
        Me.lblDate.Text = "Date:"
        '
        'lblUser
        '
        Me.lblUser.AutoSize = True
        Me.lblUser.BackColor = System.Drawing.SystemColors.Control
        Me.lblUser.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUser.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUser.Location = New System.Drawing.Point(16, 88)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUser.Size = New System.Drawing.Size(32, 13)
        Me.lblUser.TabIndex = 6
        Me.lblUser.Text = "User:"
        '
        'lblPriority
        '
        Me.lblPriority.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPriority.AutoSize = True
        Me.lblPriority.BackColor = System.Drawing.SystemColors.Control
        Me.lblPriority.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPriority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPriority.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPriority.Location = New System.Drawing.Point(264, 64)
        Me.lblPriority.Name = "lblPriority"
        Me.lblPriority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPriority.Size = New System.Drawing.Size(41, 13)
        Me.lblPriority.TabIndex = 14
        Me.lblPriority.Text = "Priority:"
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStatus.Location = New System.Drawing.Point(264, 88)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStatus.Size = New System.Drawing.Size(40, 13)
        Me.lblStatus.TabIndex = 15
        Me.lblStatus.Text = "Status:"
        '
        'txtFreeText
        '
        Me.txtFreeText.AcceptsReturn = True
        Me.txtFreeText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFreeText.BackColor = System.Drawing.SystemColors.Window
        Me.txtFreeText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFreeText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFreeText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFreeText.Location = New System.Drawing.Point(8, 158)
        Me.txtFreeText.MaxLength = 0
        Me.txtFreeText.Multiline = True
        Me.txtFreeText.Name = "txtFreeText"
        Me.txtFreeText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFreeText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtFreeText.Size = New System.Drawing.Size(409, 233)
        Me.txtFreeText.TabIndex = 9
        Me.txtFreeText.MaxLength = 1000

        '
        'cboSubject
        '
        Me.cboSubject.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubject.BackColor = System.Drawing.SystemColors.Window
        Me.cboSubject.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubject.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboSubject, New Integer(-1) {})
        Me.cboSubject.Location = New System.Drawing.Point(80, 36)
        Me.cboSubject.Name = "cboSubject"
        Me.cboSubject.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSubject.Size = New System.Drawing.Size(337, 21)
        Me.cboSubject.TabIndex = 3
        '
        'cboContext
        '
        Me.cboContext.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboContext.BackColor = System.Drawing.SystemColors.Window
        Me.cboContext.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboContext.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboContext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboContext.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboContext, New Integer(-1) {})
        Me.cboContext.Location = New System.Drawing.Point(80, 12)
        Me.cboContext.Name = "cboContext"
        Me.cboContext.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboContext.Size = New System.Drawing.Size(177, 21)
        Me.cboContext.TabIndex = 1
        '
        'txtDate
        '
        Me.txtDate.AcceptsReturn = True
        Me.txtDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDate.Enabled = False
        Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDate.Location = New System.Drawing.Point(80, 61)
        Me.txtDate.MaxLength = 0
        Me.txtDate.Name = "txtDate"
        Me.txtDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDate.Size = New System.Drawing.Size(177, 20)
        Me.txtDate.TabIndex = 5
        '
        'txtUser
        '
        Me.txtUser.AcceptsReturn = True
        Me.txtUser.BackColor = System.Drawing.SystemColors.Window
        Me.txtUser.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtUser.Enabled = False
        Me.txtUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUser.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUser.Location = New System.Drawing.Point(80, 85)
        Me.txtUser.MaxLength = 0
        Me.txtUser.Name = "txtUser"
        Me.txtUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUser.Size = New System.Drawing.Size(177, 20)
        Me.txtUser.TabIndex = 7
        '
        'cboPriority
        '
        Me.cboPriority.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPriority.BackColor = System.Drawing.SystemColors.Window
        Me.cboPriority.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPriority.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPriority.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboPriority, New Integer() {0, 0, 0})
        Me.cboPriority.Items.AddRange(New Object() {"Red", "Amber", "Green"})
        Me.cboPriority.Location = New System.Drawing.Point(314, 60)
        Me.cboPriority.Name = "cboPriority"
        Me.cboPriority.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboPriority.Size = New System.Drawing.Size(105, 21)
        Me.cboPriority.TabIndex = 16
        '
        'cboStatus
        '
        Me.cboStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listBoxComboBoxHelper1.SetItemData(Me.cboStatus, New Integer() {0, 0})
        Me.cboStatus.Items.AddRange(New Object() {"Outstanding", "Completed"})
        Me.cboStatus.Location = New System.Drawing.Point(312, 84)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboStatus.Size = New System.Drawing.Size(105, 21)
        Me.cboStatus.TabIndex = 17
        '
        'uctRichTextBox1
        '
        Me.uctRichTextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uctRichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.uctRichTextBox1.BulletIndent = 25
        Me.uctRichTextBox1.Location = New System.Drawing.Point(8, 156)
        Me.uctRichTextBox1.MaxLength = 1000
        Me.uctRichTextBox1.Name = "uctRichTextBox1"
        Me.uctRichTextBox1.PrinterName = ""
        Me.uctRichTextBox1.ShowToolbar = False
        Me.uctRichTextBox1.Size = New System.Drawing.Size(409, 233)
        Me.uctRichTextBox1.SpellCheck = False
        Me.uctRichTextBox1.TabIndex = 18
        'Me.uctRichTextBox1.TextRTF = resources.GetString("uctRichTextBox1.TextRTF")
        Me.uctRichTextBox1.Visible = False

        '
        'txtclaim
        '
        Me.txtclaim.AcceptsReturn = True
        Me.txtclaim.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtclaim.BackColor = System.Drawing.SystemColors.Window
        Me.txtclaim.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtclaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtclaim.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtclaim.Location = New System.Drawing.Point(80, 108)
        Me.txtclaim.MaxLength = 0
        Me.txtclaim.Name = "txtclaim"
        Me.txtclaim.ReadOnly = True
        Me.txtclaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtclaim.Size = New System.Drawing.Size(175, 20)
        Me.txtclaim.TabIndex = 19
        Me.txtclaim.Visible = False
        '
        'cmdOpenClaim
        '
        Me.cmdOpenClaim.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOpenClaim.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOpenClaim.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOpenClaim.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOpenClaim.Location = New System.Drawing.Point(16, 108)
        Me.cmdOpenClaim.Name = "cmdOpenClaim"
        Me.cmdOpenClaim.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOpenClaim.Size = New System.Drawing.Size(57, 21)
        Me.cmdOpenClaim.TabIndex = 20
        Me.cmdOpenClaim.Text = "Claim:"
        Me.cmdOpenClaim.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOpenClaim.UseVisualStyleBackColor = False
        Me.cmdOpenClaim.Visible = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(437, 464)
        Me.Controls.Add(Me.uctPMResizer1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(204, 164)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Note Entry"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.listBoxComboBoxHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents txtFreeText As System.Windows.Forms.TextBox
#End Region 
End Class
