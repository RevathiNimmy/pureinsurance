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
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents chkArchive As System.Windows.Forms.CheckBox
	Public WithEvents chkSpool As System.Windows.Forms.CheckBox
	Public WithEvents dtpASofDate As System.Windows.Forms.DateTimePicker
	Public WithEvents cboSource As PMLookupControl.cboPMLookup
	Public WithEvents lblAsOFDate As System.Windows.Forms.Label
	Public WithEvents lblBranch As System.Windows.Forms.Label
	Public WithEvents frmMainFrame As System.Windows.Forms.GroupBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents imgICON As System.Windows.Forms.PictureBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.stbStatus = New System.Windows.Forms.StatusStrip()
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.frmMainFrame = New System.Windows.Forms.GroupBox()
        Me.cmdSchedule = New System.Windows.Forms.Button()
        Me.chkArchive = New System.Windows.Forms.CheckBox()
        Me.chkSpool = New System.Windows.Forms.CheckBox()
        Me.dtpASofDate = New System.Windows.Forms.DateTimePicker()
        Me.cboSource = New PMLookupControl.cboPMLookup()
        Me.lblAsOFDate = New System.Windows.Forms.Label()
        Me.lblBranch = New System.Windows.Forms.Label()
        Me.imgICON = New System.Windows.Forms.PictureBox()
        Me.stbStatus.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.frmMainFrame.SuspendLayout()
        CType(Me.imgICON, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 222)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(453, 22)
        Me.stbStatus.TabIndex = 7
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Enabled = False
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(96, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(211, 198)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 21)
        Me.cmdOK.TabIndex = 5
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(290, 198)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 21)
        Me.cmdCancel.TabIndex = 6
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(370, 198)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 21)
        Me.cmdHelp.TabIndex = 7
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(143, 20)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 6)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(438, 189)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.frmMainFrame)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 24)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(430, 161)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Credit Control Processing"
        '
        'frmMainFrame
        '
        Me.frmMainFrame.BackColor = System.Drawing.SystemColors.Control
        Me.frmMainFrame.Controls.Add(Me.cmdSchedule)
        Me.frmMainFrame.Controls.Add(Me.chkArchive)
        Me.frmMainFrame.Controls.Add(Me.chkSpool)
        Me.frmMainFrame.Controls.Add(Me.dtpASofDate)
        Me.frmMainFrame.Controls.Add(Me.cboSource)
        Me.frmMainFrame.Controls.Add(Me.lblAsOFDate)
        Me.frmMainFrame.Controls.Add(Me.lblBranch)
        Me.frmMainFrame.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmMainFrame.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmMainFrame.Location = New System.Drawing.Point(8, 10)
        Me.frmMainFrame.Name = "frmMainFrame"
        Me.frmMainFrame.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmMainFrame.Size = New System.Drawing.Size(415, 141)
        Me.frmMainFrame.TabIndex = 18
        Me.frmMainFrame.TabStop = False
        Me.frmMainFrame.Text = "Credit Control Criteria"
        '
        'cmdSchedule
        '
        Me.cmdSchedule.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSchedule.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSchedule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSchedule.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSchedule.Location = New System.Drawing.Point(270, 27)
        Me.cmdSchedule.Name = "cmdSchedule"
        Me.cmdSchedule.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSchedule.Size = New System.Drawing.Size(125, 21)
        Me.cmdSchedule.TabIndex = 9
        Me.cmdSchedule.Text = "&Schedule"
        Me.cmdSchedule.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSchedule.UseVisualStyleBackColor = False
        '
        'chkArchive
        '
        Me.chkArchive.BackColor = System.Drawing.SystemColors.Control
        Me.chkArchive.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkArchive.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkArchive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkArchive.Location = New System.Drawing.Point(222, 108)
        Me.chkArchive.Name = "chkArchive"
        Me.chkArchive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkArchive.Size = New System.Drawing.Size(145, 19)
        Me.chkArchive.TabIndex = 4
        Me.chkArchive.Text = "Archive Documents"
        Me.chkArchive.UseVisualStyleBackColor = False
        '
        'chkSpool
        '
        Me.chkSpool.BackColor = System.Drawing.SystemColors.Control
        Me.chkSpool.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSpool.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSpool.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSpool.Location = New System.Drawing.Point(44, 110)
        Me.chkSpool.Name = "chkSpool"
        Me.chkSpool.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSpool.Size = New System.Drawing.Size(143, 13)
        Me.chkSpool.TabIndex = 3
        Me.chkSpool.Text = "Spool Documents"
        Me.chkSpool.UseVisualStyleBackColor = False
        '
        'dtpASofDate
        '
        Me.dtpASofDate.Location = New System.Drawing.Point(98, 26)
        Me.dtpASofDate.Name = "dtpASofDate"
        Me.dtpASofDate.Size = New System.Drawing.Size(199, 21)
        Me.dtpASofDate.TabIndex = 1
        '
        'cboSource
        '
        Me.cboSource.DefaultItemId = 0
        Me.cboSource.FirstItem = ""
        Me.cboSource.ItemId = 0
        Me.cboSource.ListIndex = -1
        Me.cboSource.Location = New System.Drawing.Point(98, 66)
        Me.cboSource.Name = "cboSource"
        Me.cboSource.PMLookupProductFamily = 1
        Me.cboSource.SingleItemId = 0
        Me.cboSource.Size = New System.Drawing.Size(297, 21)
        Me.cboSource.SortColumnName = ""
        Me.cboSource.Sorted = True
        Me.cboSource.TabIndex = 2
        Me.cboSource.TableName = "Source"
        Me.cboSource.ToolTipText = ""
        Me.cboSource.WhereClause = ""
        '
        'lblAsOFDate
        '
        Me.lblAsOFDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblAsOFDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAsOFDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAsOFDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAsOFDate.Location = New System.Drawing.Point(12, 31)
        Me.lblAsOFDate.Name = "lblAsOFDate"
        Me.lblAsOFDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAsOFDate.Size = New System.Drawing.Size(75, 17)
        Me.lblAsOFDate.TabIndex = 9
        Me.lblAsOFDate.Text = "As Of Date:"
        Me.lblAsOFDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblBranch
        '
        Me.lblBranch.BackColor = System.Drawing.SystemColors.Control
        Me.lblBranch.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBranch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBranch.Location = New System.Drawing.Point(12, 70)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBranch.Size = New System.Drawing.Size(75, 17)
        Me.lblBranch.TabIndex = 5
        Me.lblBranch.Text = "Branch:"
        Me.lblBranch.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'imgICON
        '
        Me.imgICON.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgICON.Image = CType(resources.GetObject("imgICON.Image"), System.Drawing.Image)
        Me.imgICON.Location = New System.Drawing.Point(0, 0)
        Me.imgICON.Name = "imgICON"
        Me.imgICON.Size = New System.Drawing.Size(38, 32)
        Me.imgICON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgICON.TabIndex = 8
        Me.imgICON.TabStop = False
        Me.imgICON.Visible = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(453, 244)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.imgICON)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(99, 210)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "100"
        Me.Text = "Credit Control Processing"
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.frmMainFrame.ResumeLayout(False)
        CType(Me.imgICON, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents cmdSchedule As Button
#End Region
End Class