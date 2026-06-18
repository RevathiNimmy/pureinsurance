<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
        'This call is required by the Windows Form Designer.
        Me.KeyPreview = True
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
    'Public WithEvents cryControl As AxCrystal.AxCrystalReport
    Public WithEvents lblProductCode As System.Windows.Forms.Label
    Public WithEvents lblSelectionDate As System.Windows.Forms.Label
    Public WithEvents lblAgentCode As System.Windows.Forms.Label
    Public WithEvents lblSortOrder As System.Windows.Forms.Label
    Public WithEvents lblBranchCode As System.Windows.Forms.Label
    Public WithEvents txtSelectionDate As System.Windows.Forms.TextBox
    Public WithEvents cboProductCode As System.Windows.Forms.ComboBox
    Public WithEvents cmdRePrint As System.Windows.Forms.Button
    Public WithEvents cboAgentCode As System.Windows.Forms.ComboBox
    Public WithEvents cboSortOrder As System.Windows.Forms.ComboBox
    Public WithEvents cboBranchCode As System.Windows.Forms.ComboBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblProductCode = New System.Windows.Forms.Label
        Me.lblSelectionDate = New System.Windows.Forms.Label
        Me.lblAgentCode = New System.Windows.Forms.Label
        Me.lblSortOrder = New System.Windows.Forms.Label
        Me.lblBranchCode = New System.Windows.Forms.Label
        Me.txtSelectionDate = New System.Windows.Forms.TextBox
        Me.cboProductCode = New System.Windows.Forms.ComboBox
        Me.cmdRePrint = New System.Windows.Forms.Button
        Me.cboAgentCode = New System.Windows.Forms.ComboBox
        Me.cboSortOrder = New System.Windows.Forms.ComboBox
        Me.cboBranchCode = New System.Windows.Forms.ComboBox
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.stbMain = New System.Windows.Forms.StatusStrip
        Me.MESSAGE = New System.Windows.Forms.ToolStripStatusLabel
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.stbMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(500, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(505, 203)
        Me.tabMainTab.TabIndex = 5
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblProductCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSelectionDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblAgentCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSortOrder)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBranchCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtSelectionDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboProductCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdRePrint)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboAgentCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboSortOrder)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboBranchCode)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(497, 177)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1-Renewal Invite Print"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'lblProductCode
        '
        Me.lblProductCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblProductCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblProductCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProductCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblProductCode.Location = New System.Drawing.Point(14, 52)
        Me.lblProductCode.Name = "lblProductCode"
        Me.lblProductCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblProductCode.Size = New System.Drawing.Size(94, 17)
        Me.lblProductCode.TabIndex = 6
        Me.lblProductCode.Text = "Product Code:"
        '
        'lblSelectionDate
        '
        Me.lblSelectionDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblSelectionDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSelectionDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectionDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSelectionDate.Location = New System.Drawing.Point(14, 19)
        Me.lblSelectionDate.Name = "lblSelectionDate"
        Me.lblSelectionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSelectionDate.Size = New System.Drawing.Size(105, 17)
        Me.lblSelectionDate.TabIndex = 7
        Me.lblSelectionDate.Text = "Selection Date:"
        '
        'lblAgentCode
        '
        Me.lblAgentCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAgentCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAgentCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAgentCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAgentCode.Location = New System.Drawing.Point(14, 84)
        Me.lblAgentCode.Name = "lblAgentCode"
        Me.lblAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAgentCode.Size = New System.Drawing.Size(94, 17)
        Me.lblAgentCode.TabIndex = 10
        Me.lblAgentCode.Text = "Agent Code:"
        '
        'lblSortOrder
        '
        Me.lblSortOrder.BackColor = System.Drawing.SystemColors.Control
        Me.lblSortOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSortOrder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSortOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSortOrder.Location = New System.Drawing.Point(14, 116)
        Me.lblSortOrder.Name = "lblSortOrder"
        Me.lblSortOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSortOrder.Size = New System.Drawing.Size(94, 17)
        Me.lblSortOrder.TabIndex = 12
        Me.lblSortOrder.Text = "Sort Order:"
        '
        'lblBranchCode
        '
        Me.lblBranchCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranchCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranchCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranchCode.Location = New System.Drawing.Point(14, 148)
        Me.lblBranchCode.Name = "lblBranchCode"
        Me.lblBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranchCode.Size = New System.Drawing.Size(94, 17)
        Me.lblBranchCode.TabIndex = 15
        Me.lblBranchCode.Text = "Branch Code:"
        '
        'txtSelectionDate
        '
        Me.txtSelectionDate.AcceptsReturn = True
        Me.txtSelectionDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtSelectionDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSelectionDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSelectionDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSelectionDate.Location = New System.Drawing.Point(122, 16)
        Me.txtSelectionDate.MaxLength = 0
        Me.txtSelectionDate.Name = "txtSelectionDate"
        Me.txtSelectionDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSelectionDate.Size = New System.Drawing.Size(149, 20)
        Me.txtSelectionDate.TabIndex = 0
        '
        'cboProductCode
        '
        Me.cboProductCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboProductCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboProductCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProductCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboProductCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboProductCode.Location = New System.Drawing.Point(122, 48)
        Me.cboProductCode.Name = "cboProductCode"
        Me.cboProductCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboProductCode.Size = New System.Drawing.Size(289, 21)
        Me.cboProductCode.TabIndex = 1
        '
        'cmdRePrint
        '
        Me.cmdRePrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRePrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRePrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRePrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRePrint.Location = New System.Drawing.Point(424, 144)
        Me.cmdRePrint.Name = "cmdRePrint"
        Me.cmdRePrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRePrint.Size = New System.Drawing.Size(65, 22)
        Me.cmdRePrint.TabIndex = 8
        Me.cmdRePrint.Text = "RePrint"
        Me.cmdRePrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdRePrint.UseVisualStyleBackColor = False
        '
        'cboAgentCode
        '
        Me.cboAgentCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboAgentCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAgentCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAgentCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAgentCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAgentCode.Location = New System.Drawing.Point(122, 80)
        Me.cboAgentCode.Name = "cboAgentCode"
        Me.cboAgentCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAgentCode.Size = New System.Drawing.Size(289, 21)
        Me.cboAgentCode.TabIndex = 9
        '
        'cboSortOrder
        '
        Me.cboSortOrder.BackColor = System.Drawing.SystemColors.Window
        Me.cboSortOrder.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSortOrder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSortOrder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSortOrder.Location = New System.Drawing.Point(122, 112)
        Me.cboSortOrder.Name = "cboSortOrder"
        Me.cboSortOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSortOrder.Size = New System.Drawing.Size(289, 21)
        Me.cboSortOrder.TabIndex = 11
        '
        'cboBranchCode
        '
        Me.cboBranchCode.BackColor = System.Drawing.SystemColors.Window
        Me.cboBranchCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBranchCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranchCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBranchCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBranchCode.Location = New System.Drawing.Point(122, 144)
        Me.cboBranchCode.Name = "cboBranchCode"
        Me.cboBranchCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBranchCode.Size = New System.Drawing.Size(289, 21)
        Me.cboBranchCode.TabIndex = 14
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(432, 216)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 4
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(352, 216)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 3
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
        Me.cmdOK.Location = New System.Drawing.Point(272, 216)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'stbMain
        '
        Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MESSAGE})
        Me.stbMain.Location = New System.Drawing.Point(0, 245)
        Me.stbMain.Name = "stbMain"
        Me.stbMain.Size = New System.Drawing.Size(516, 22)
        Me.stbMain.TabIndex = 14
        Me.stbMain.Text = "StatusStrip1"
        '
        'MESSAGE
        '
        Me.MESSAGE.AutoSize = False
        Me.MESSAGE.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.MESSAGE.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.MESSAGE.DoubleClickEnabled = True
        Me.MESSAGE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.MESSAGE.Margin = New System.Windows.Forms.Padding(0)
        Me.MESSAGE.Name = "MESSAGE"
        Me.MESSAGE.Size = New System.Drawing.Size(500, 22)
        Me.MESSAGE.Text = "Ready"
        Me.MESSAGE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(516, 267)
        Me.Controls.Add(Me.stbMain)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Renewal Invite"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.stbMain.ResumeLayout(False)
        Me.stbMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents stbMain As System.Windows.Forms.StatusStrip
    Public WithEvents MESSAGE As System.Windows.Forms.ToolStripStatusLabel
#End Region
End Class