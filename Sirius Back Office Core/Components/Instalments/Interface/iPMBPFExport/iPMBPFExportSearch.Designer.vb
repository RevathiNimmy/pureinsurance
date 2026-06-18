<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSearch
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
    Public WithEvents chkPerfectMatch As System.Windows.Forms.CheckBox
    Public WithEvents Message As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents Found As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents stbMain As System.Windows.Forms.StatusStrip
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cboSearchPosition As System.Windows.Forms.ComboBox
    Public WithEvents txtSearchValue As System.Windows.Forms.TextBox
    Public WithEvents cboSearchColumn As System.Windows.Forms.ComboBox
    Public WithEvents lblSearchPosition As System.Windows.Forms.Label
    Public WithEvents lblSearchValue As System.Windows.Forms.Label
    Public WithEvents lblSearchColumn As System.Windows.Forms.Label
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkPerfectMatch = New System.Windows.Forms.CheckBox
        Me.stbMain = New System.Windows.Forms.StatusStrip
        Me.Message = New System.Windows.Forms.ToolStripStatusLabel
        Me.Found = New System.Windows.Forms.ToolStripStatusLabel
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cboSearchPosition = New System.Windows.Forms.ComboBox
        Me.txtSearchValue = New System.Windows.Forms.TextBox
        Me.cboSearchColumn = New System.Windows.Forms.ComboBox
        Me.lblSearchPosition = New System.Windows.Forms.Label
        Me.lblSearchValue = New System.Windows.Forms.Label
        Me.lblSearchColumn = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.stbMain.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkPerfectMatch
        '
        Me.chkPerfectMatch.BackColor = System.Drawing.SystemColors.Control
        Me.chkPerfectMatch.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPerfectMatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPerfectMatch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPerfectMatch.Location = New System.Drawing.Point(214, 104)
        Me.chkPerfectMatch.Name = "chkPerfectMatch"
        Me.chkPerfectMatch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPerfectMatch.Size = New System.Drawing.Size(91, 16)
        Me.chkPerfectMatch.TabIndex = 9
        Me.chkPerfectMatch.Text = "Match Exactly"
        Me.chkPerfectMatch.UseVisualStyleBackColor = False
        '
        'stbMain
        '
        Me.stbMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.stbMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Message, Me.Found})
        Me.stbMain.Location = New System.Drawing.Point(0, 163)
        Me.stbMain.Name = "stbMain"
        Me.stbMain.ShowItemToolTips = True
        Me.stbMain.Size = New System.Drawing.Size(320, 22)
        Me.stbMain.TabIndex = 8
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
        Me.Message.Size = New System.Drawing.Size(210, 22)
        Me.Message.Tag = ""
        Me.Message.Text = "Ready"
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
        Me.Found.Size = New System.Drawing.Size(210, 19)
        Me.Found.Tag = ""
        Me.Found.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(153, 136)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 3
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
        Me.cmdCancel.Location = New System.Drawing.Point(232, 136)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cboSearchPosition
        '
        Me.cboSearchPosition.BackColor = System.Drawing.SystemColors.Window
        Me.cboSearchPosition.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSearchPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSearchPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSearchPosition.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSearchPosition.Location = New System.Drawing.Point(96, 72)
        Me.cboSearchPosition.Name = "cboSearchPosition"
        Me.cboSearchPosition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSearchPosition.Size = New System.Drawing.Size(209, 21)
        Me.cboSearchPosition.TabIndex = 2
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
        Me.txtSearchValue.Size = New System.Drawing.Size(209, 20)
        Me.txtSearchValue.TabIndex = 0
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
        'lblSearchPosition
        '
        Me.lblSearchPosition.BackColor = System.Drawing.Color.Transparent
        Me.lblSearchPosition.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSearchPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchPosition.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSearchPosition.Location = New System.Drawing.Point(12, 75)
        Me.lblSearchPosition.Name = "lblSearchPosition"
        Me.lblSearchPosition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSearchPosition.Size = New System.Drawing.Size(77, 17)
        Me.lblSearchPosition.TabIndex = 7
        Me.lblSearchPosition.Text = "Search From :"
        '
        'lblSearchValue
        '
        Me.lblSearchValue.BackColor = System.Drawing.Color.Transparent
        Me.lblSearchValue.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSearchValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSearchValue.Location = New System.Drawing.Point(12, 11)
        Me.lblSearchValue.Name = "lblSearchValue"
        Me.lblSearchValue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSearchValue.Size = New System.Drawing.Size(87, 17)
        Me.lblSearchValue.TabIndex = 6
        Me.lblSearchValue.Text = "Search Value :"
        '
        'lblSearchColumn
        '
        Me.lblSearchColumn.BackColor = System.Drawing.Color.Transparent
        Me.lblSearchColumn.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSearchColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchColumn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSearchColumn.Location = New System.Drawing.Point(12, 43)
        Me.lblSearchColumn.Name = "lblSearchColumn"
        Me.lblSearchColumn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSearchColumn.Size = New System.Drawing.Size(77, 17)
        Me.lblSearchColumn.TabIndex = 5
        Me.lblSearchColumn.Text = "Column"
        '
        'frmSearch
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(320, 185)
        Me.Controls.Add(Me.chkPerfectMatch)
        Me.Controls.Add(Me.stbMain)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cboSearchPosition)
        Me.Controls.Add(Me.txtSearchValue)
        Me.Controls.Add(Me.cboSearchColumn)
        Me.Controls.Add(Me.lblSearchPosition)
        Me.Controls.Add(Me.lblSearchValue)
        Me.Controls.Add(Me.lblSearchColumn)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
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
#End Region
End Class