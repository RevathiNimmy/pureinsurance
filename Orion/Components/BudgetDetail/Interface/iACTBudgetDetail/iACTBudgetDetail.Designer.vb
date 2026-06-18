<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
    Public WithEvents cmdNavigate As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents imgIcon As System.Windows.Forms.PictureBox
    Public WithEvents lblBudgetRef As System.Windows.Forms.Label
    Public WithEvents lblYear As System.Windows.Forms.Label
    'Public WithEvents uctPMGrid As PMGridControl.uctPMGridControl
    Public WithEvents panBudgetRef As System.Windows.Forms.Panel
    Public WithEvents panYear As System.Windows.Forms.Panel
    Public WithEvents chkApportion As System.Windows.Forms.CheckBox
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
    Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
    Public dlgHelpFont As System.Windows.Forms.FontDialog
    Public dlgHelpColor As System.Windows.Forms.ColorDialog
    Public dlgHelpPrint As System.Windows.Forms.PrintDialog
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.grdBudgetDetails = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.imgIcon = New System.Windows.Forms.PictureBox
        Me.lblBudgetRef = New System.Windows.Forms.Label
        Me.lblYear = New System.Windows.Forms.Label
        Me.panBudgetRef = New System.Windows.Forms.Panel
        Me.lblpanBudgetRef = New System.Windows.Forms.Label
        Me.panYear = New System.Windows.Forms.Panel
        Me.lblPanYear = New System.Windows.Forms.Label
        Me.chkApportion = New System.Windows.Forms.CheckBox
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.grdBudgetDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panBudgetRef.SuspendLayout()
        Me.panYear.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(8, 432)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 4
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(576, 432)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 2
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
        Me.cmdCancel.Location = New System.Drawing.Point(496, 432)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
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
        Me.cmdOK.Location = New System.Drawing.Point(416, 432)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(127, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(645, 421)
        Me.tabMainTab.TabIndex = 3
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.grdBudgetDetails)
        Me._tabMainTab_TabPage0.Controls.Add(Me.imgIcon)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBudgetRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblYear)
        Me._tabMainTab_TabPage0.Controls.Add(Me.panBudgetRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.panYear)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkApportion)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(637, 395)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'grdBudgetDetails
        '
        Me.grdBudgetDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdBudgetDetails.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.grdBudgetDetails.Location = New System.Drawing.Point(19, 52)
        Me.grdBudgetDetails.MultiSelect = False
        Me.grdBudgetDetails.Name = "grdBudgetDetails"
        Me.grdBudgetDetails.RowHeadersWidth = 20
        Me.grdBudgetDetails.RowTemplate.Height = 18
        Me.grdBudgetDetails.Size = New System.Drawing.Size(613, 327)
        Me.grdBudgetDetails.TabIndex = 11
        '
        'Column1
        '
        Me.Column1.HeaderText = "Account"
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 70
        '
        'Column2
        '
        Me.Column2.HeaderText = "Annual"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 70
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(600, 4)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 0
        Me.imgIcon.TabStop = False
        '
        'lblBudgetRef
        '
        Me.lblBudgetRef.AutoSize = True
        Me.lblBudgetRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblBudgetRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBudgetRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBudgetRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBudgetRef.Location = New System.Drawing.Point(16, 20)
        Me.lblBudgetRef.Name = "lblBudgetRef"
        Me.lblBudgetRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBudgetRef.Size = New System.Drawing.Size(67, 13)
        Me.lblBudgetRef.TabIndex = 6
        Me.lblBudgetRef.Text = "Budget Ref.:"
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.BackColor = System.Drawing.SystemColors.Control
        Me.lblYear.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYear.Location = New System.Drawing.Point(232, 20)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblYear.Size = New System.Drawing.Size(32, 13)
        Me.lblYear.TabIndex = 8
        Me.lblYear.Text = "Year:"
        '
        'panBudgetRef
        '
        Me.panBudgetRef.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panBudgetRef.Controls.Add(Me.lblpanBudgetRef)
        Me.panBudgetRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panBudgetRef.Location = New System.Drawing.Point(96, 20)
        Me.panBudgetRef.Name = "panBudgetRef"
        Me.panBudgetRef.Size = New System.Drawing.Size(113, 17)
        Me.panBudgetRef.TabIndex = 7
        '
        'lblpanBudgetRef
        '
        Me.lblpanBudgetRef.AutoSize = True
        Me.lblpanBudgetRef.Location = New System.Drawing.Point(3, 0)
        Me.lblpanBudgetRef.Name = "lblpanBudgetRef"
        Me.lblpanBudgetRef.Size = New System.Drawing.Size(0, 13)
        Me.lblpanBudgetRef.TabIndex = 0
        '
        'panYear
        '
        Me.panYear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panYear.Controls.Add(Me.lblPanYear)
        Me.panYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.panYear.Location = New System.Drawing.Point(288, 20)
        Me.panYear.Name = "panYear"
        Me.panYear.Size = New System.Drawing.Size(49, 17)
        Me.panYear.TabIndex = 9
        '
        'lblPanYear
        '
        Me.lblPanYear.AutoSize = True
        Me.lblPanYear.Location = New System.Drawing.Point(-2, -2)
        Me.lblPanYear.Name = "lblPanYear"
        Me.lblPanYear.Size = New System.Drawing.Size(0, 13)
        Me.lblPanYear.TabIndex = 0
        '
        'chkApportion
        '
        Me.chkApportion.BackColor = System.Drawing.SystemColors.Control
        Me.chkApportion.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkApportion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkApportion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkApportion.Location = New System.Drawing.Point(360, 20)
        Me.chkApportion.Name = "chkApportion"
        Me.chkApportion.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkApportion.Size = New System.Drawing.Size(217, 17)
        Me.chkApportion.TabIndex = 10
        Me.chkApportion.Text = "Automatically apportion Annual Budget"
        Me.chkApportion.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(657, 460)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Budget Detail"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.grdBudgetDetails, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panBudgetRef.ResumeLayout(False)
        Me.panBudgetRef.PerformLayout()
        Me.panYear.ResumeLayout(False)
        Me.panYear.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblpanBudgetRef As System.Windows.Forms.Label
    Friend WithEvents lblPanYear As System.Windows.Forms.Label
    Friend WithEvents grdBudgetDetails As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
#End Region
End Class