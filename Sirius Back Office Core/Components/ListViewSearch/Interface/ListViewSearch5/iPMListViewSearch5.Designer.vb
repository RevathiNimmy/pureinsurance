<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSearch
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		InitializeoptSearchDirection()
		InitializelblSearch()
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
	Public WithEvents Message As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents Found As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbMain As System.Windows.Forms.StatusStrip
	Private WithEvents _optSearchDirection_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optSearchDirection_0 As System.Windows.Forms.RadioButton
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents chkPerfectMatch As System.Windows.Forms.CheckBox
	Public WithEvents cboSearchColumn As System.Windows.Forms.ComboBox
	Public WithEvents txtSearchValue As System.Windows.Forms.TextBox
	Private WithEvents _lblSearch_1 As System.Windows.Forms.Label
	Private WithEvents _lblSearch_0 As System.Windows.Forms.Label
	Public lblSearch(1) As System.Windows.Forms.Label
	Public optSearchDirection(1) As System.Windows.Forms.RadioButton
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSearch))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.stbMain = New System.Windows.Forms.StatusStrip
        Me.Message = New System.Windows.Forms.ToolStripStatusLabel
        Me.Found = New System.Windows.Forms.ToolStripStatusLabel
        Me._optSearchDirection_1 = New System.Windows.Forms.RadioButton
        Me._optSearchDirection_0 = New System.Windows.Forms.RadioButton
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.chkPerfectMatch = New System.Windows.Forms.CheckBox
        Me.cboSearchColumn = New System.Windows.Forms.ComboBox
        Me.txtSearchValue = New System.Windows.Forms.TextBox
        Me._lblSearch_1 = New System.Windows.Forms.Label
        Me._lblSearch_0 = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.stbMain.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'stbMain
        '
        Me.stbMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Message, Me.Found})
        Me.stbMain.Location = New System.Drawing.Point(0, 162)
        Me.stbMain.Name = "stbMain"
        Me.stbMain.ShowItemToolTips = True
        Me.stbMain.Size = New System.Drawing.Size(316, 22)
        Me.stbMain.TabIndex = 9
        '
        'Message
        '
        Me.Message.AutoSize = False
        Me.Message.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Message.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.Message.DoubleClickEnabled = True
        Me.Message.Margin = New System.Windows.Forms.Padding(0)
        Me.Message.Name = "Message"
        Me.Message.Size = New System.Drawing.Size(90, 22)
        Me.Message.Tag = ""
        Me.Message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Found
        '
        Me.Found.AutoSize = False
        Me.Found.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.Found.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me.Found.DoubleClickEnabled = True
        Me.Found.Margin = New System.Windows.Forms.Padding(0)
        Me.Found.Name = "Found"
        Me.Found.Size = New System.Drawing.Size(167, 22)
        Me.Found.Tag = ""
        Me.Found.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        '_optSearchDirection_1
        '
        Me._optSearchDirection_1.BackColor = System.Drawing.SystemColors.Control
        Me._optSearchDirection_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSearchDirection_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSearchDirection_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSearchDirection_1.Location = New System.Drawing.Point(96, 64)
        Me._optSearchDirection_1.Name = "_optSearchDirection_1"
        Me._optSearchDirection_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSearchDirection_1.Size = New System.Drawing.Size(89, 25)
        Me._optSearchDirection_1.TabIndex = 8
        Me._optSearchDirection_1.TabStop = True
        Me._optSearchDirection_1.Text = "Up"
        Me._optSearchDirection_1.UseVisualStyleBackColor = False
        '
        '_optSearchDirection_0
        '
        Me._optSearchDirection_0.BackColor = System.Drawing.SystemColors.Control
        Me._optSearchDirection_0.Checked = True
        Me._optSearchDirection_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optSearchDirection_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optSearchDirection_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._optSearchDirection_0.Location = New System.Drawing.Point(96, 88)
        Me._optSearchDirection_0.Name = "_optSearchDirection_0"
        Me._optSearchDirection_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optSearchDirection_0.Size = New System.Drawing.Size(89, 25)
        Me._optSearchDirection_0.TabIndex = 7
        Me._optSearchDirection_0.TabStop = True
        Me._optSearchDirection_0.Text = "Down"
        Me._optSearchDirection_0.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(234, 133)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(73, 22)
        Me.cmdExit.TabIndex = 4
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Location = New System.Drawing.Point(162, 133)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(73, 22)
        Me.cmdSearch.TabIndex = 3
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'chkPerfectMatch
        '
        Me.chkPerfectMatch.BackColor = System.Drawing.SystemColors.Control
        Me.chkPerfectMatch.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPerfectMatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPerfectMatch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPerfectMatch.Location = New System.Drawing.Point(216, 64)
        Me.chkPerfectMatch.Name = "chkPerfectMatch"
        Me.chkPerfectMatch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPerfectMatch.Size = New System.Drawing.Size(91, 25)
        Me.chkPerfectMatch.TabIndex = 2
        Me.chkPerfectMatch.Text = "Match Exactly"
        Me.chkPerfectMatch.UseVisualStyleBackColor = False
        '
        'cboSearchColumn
        '
        Me.cboSearchColumn.BackColor = System.Drawing.SystemColors.Window
        Me.cboSearchColumn.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSearchColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSearchColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSearchColumn.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSearchColumn.Location = New System.Drawing.Point(96, 40)
        Me.cboSearchColumn.Name = "cboSearchColumn"
        Me.cboSearchColumn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSearchColumn.Size = New System.Drawing.Size(209, 21)
        Me.cboSearchColumn.TabIndex = 1
        '
        'txtSearchValue
        '
        Me.txtSearchValue.AcceptsReturn = True
        Me.txtSearchValue.BackColor = System.Drawing.SystemColors.Window
        Me.txtSearchValue.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSearchValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearchValue.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSearchValue.Location = New System.Drawing.Point(96, 8)
        Me.txtSearchValue.MaxLength = 0
        Me.txtSearchValue.Name = "txtSearchValue"
        Me.txtSearchValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSearchValue.Size = New System.Drawing.Size(209, 21)
        Me.txtSearchValue.TabIndex = 0
        '
        '_lblSearch_1
        '
        Me._lblSearch_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblSearch_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblSearch_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblSearch_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblSearch_1.Location = New System.Drawing.Point(12, 43)
        Me._lblSearch_1.Name = "_lblSearch_1"
        Me._lblSearch_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblSearch_1.Size = New System.Drawing.Size(77, 17)
        Me._lblSearch_1.TabIndex = 6
        Me._lblSearch_1.Text = "Column :"
        Me._lblSearch_1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        '_lblSearch_0
        '
        Me._lblSearch_0.BackColor = System.Drawing.SystemColors.Control
        Me._lblSearch_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblSearch_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblSearch_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblSearch_0.Location = New System.Drawing.Point(12, 11)
        Me._lblSearch_0.Name = "_lblSearch_0"
        Me._lblSearch_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblSearch_0.Size = New System.Drawing.Size(77, 17)
        Me._lblSearch_0.TabIndex = 5
        Me._lblSearch_0.Text = "Search Value :"
        Me._lblSearch_0.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmSearch
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(316, 184)
        Me.Controls.Add(Me.stbMain)
        Me.Controls.Add(Me._optSearchDirection_1)
        Me.Controls.Add(Me._optSearchDirection_0)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.chkPerfectMatch)
        Me.Controls.Add(Me.cboSearchColumn)
        Me.Controls.Add(Me.txtSearchValue)
        Me.Controls.Add(Me._lblSearch_1)
        Me.Controls.Add(Me._lblSearch_0)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSearch"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Search"
        Me.stbMain.ResumeLayout(False)
        Me.stbMain.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializeoptSearchDirection()
		Me.optSearchDirection(1) = _optSearchDirection_1
		Me.optSearchDirection(0) = _optSearchDirection_0
	End Sub
	Sub InitializelblSearch()
		Me.lblSearch(1) = _lblSearch_1
		Me.lblSearch(0) = _lblSearch_0
	End Sub
#End Region 
End Class