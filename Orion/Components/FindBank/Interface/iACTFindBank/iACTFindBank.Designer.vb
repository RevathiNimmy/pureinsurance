<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializepnlMain()
		lvwSearchResults_InitializeColumnKeys()
		tabMainPreviousTab = tabMain.SelectedIndex
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdNavigate As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtName As System.Windows.Forms.TextBox
	Public WithEvents txtShortCode As System.Windows.Forms.TextBox
	Public WithEvents lblShortCode As System.Windows.Forms.Label
	Public WithEvents lblName As System.Windows.Forms.Label
	Private WithEvents _pnlMain_0 As System.Windows.Forms.Panel
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
	Private WithEvents _lvwSearchResults_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwSearchResults_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwSearchResults As System.Windows.Forms.ListView
	Public WithEvents imglImages As System.Windows.Forms.ImageList
	Public WithEvents ImgImage As System.Windows.Forms.PictureBox
	Public pnlMain(0) As System.Windows.Forms.Panel
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdNavigate = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdNew = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me._pnlMain_0 = New System.Windows.Forms.Panel
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtShortCode = New System.Windows.Forms.TextBox
        Me.lblShortCode = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.lvwSearchResults = New System.Windows.Forms.ListView
        Me._lvwSearchResults_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchResults_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.ImgImage = New System.Windows.Forms.PictureBox
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        Me._pnlMain_0.SuspendLayout()
        Me.stbStatus.SuspendLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdNavigate
        '
        Me.cmdNavigate.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNavigate.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNavigate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNavigate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNavigate.Location = New System.Drawing.Point(168, 328)
        Me.cmdNavigate.Name = "cmdNavigate"
        Me.cmdNavigate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNavigate.Size = New System.Drawing.Size(73, 22)
        Me.cmdNavigate.TabIndex = 11
        Me.cmdNavigate.TabStop = False
        Me.cmdNavigate.Text = "&Navigate"
        Me.cmdNavigate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNavigate.UseVisualStyleBackColor = False
        Me.cmdNavigate.Visible = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(8, 328)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 10
        Me.cmdEdit.TabStop = False
        Me.cmdEdit.Text = "E&dit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdNew
        '
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNew.Location = New System.Drawing.Point(88, 328)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNew.Size = New System.Drawing.Size(73, 22)
        Me.cmdNew.TabIndex = 9
        Me.cmdNew.TabStop = False
        Me.cmdNew.Text = "N&ew"
        Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(472, 56)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(73, 22)
        Me.cmdNewSearch.TabIndex = 6
        Me.cmdNewSearch.TabStop = False
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Enabled = False
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(472, 28)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(73, 22)
        Me.cmdFindNow.TabIndex = 5
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(472, 328)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 14
        Me.cmdHelp.TabStop = False
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
        Me.cmdCancel.Location = New System.Drawing.Point(392, 328)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 13
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Enabled = False
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(312, 328)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 12
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(456, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(461, 141)
        Me.tabMain.TabIndex = 0
        Me.tabMain.TabStop = False
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me._pnlMain_0)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(453, 115)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "1 - Details"
        Me._tabMain_TabPage0.UseVisualStyleBackColor = True
        '
        '_pnlMain_0
        '
        Me._pnlMain_0.Controls.Add(Me.txtName)
        Me._pnlMain_0.Controls.Add(Me.txtShortCode)
        Me._pnlMain_0.Controls.Add(Me.lblShortCode)
        Me._pnlMain_0.Controls.Add(Me.lblName)
        Me._pnlMain_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._pnlMain_0.Location = New System.Drawing.Point(8, 4)
        Me._pnlMain_0.Name = "_pnlMain_0"
        Me._pnlMain_0.Size = New System.Drawing.Size(446, 99)
        Me._pnlMain_0.TabIndex = 8
        '
        'txtName
        '
        Me.txtName.AcceptsReturn = True
        Me.txtName.BackColor = System.Drawing.SystemColors.Window
        Me.txtName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtName.Location = New System.Drawing.Point(88, 40)
        Me.txtName.MaxLength = 0
        Me.txtName.Name = "txtName"
        Me.txtName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtName.Size = New System.Drawing.Size(249, 20)
        Me.txtName.TabIndex = 4
        '
        'txtShortCode
        '
        Me.txtShortCode.AcceptsReturn = True
        Me.txtShortCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtShortCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtShortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtShortCode.Location = New System.Drawing.Point(88, 8)
        Me.txtShortCode.MaxLength = 0
        Me.txtShortCode.Name = "txtShortCode"
        Me.txtShortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtShortCode.Size = New System.Drawing.Size(153, 20)
        Me.txtShortCode.TabIndex = 2
        '
        'lblShortCode
        '
        Me.lblShortCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblShortCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblShortCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblShortCode.Location = New System.Drawing.Point(0, 8)
        Me.lblShortCode.Name = "lblShortCode"
        Me.lblShortCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblShortCode.Size = New System.Drawing.Size(81, 17)
        Me.lblShortCode.TabIndex = 1
        Me.lblShortCode.Text = "Short Name:"
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.SystemColors.Control
        Me.lblName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblName.Location = New System.Drawing.Point(0, 40)
        Me.lblName.Name = "lblName"
        Me.lblName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblName.Size = New System.Drawing.Size(81, 17)
        Me.lblName.TabIndex = 3
        Me.lblName.Text = "Name:"
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 357)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(553, 22)
        Me.stbStatus.TabIndex = 15
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = False
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.DoubleClickEnabled = True
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(536, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lvwSearchResults
        '
        Me.lvwSearchResults.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchResults_ColumnHeader_1, Me._lvwSearchResults_ColumnHeader_2, Me._lvwSearchResults_ColumnHeader_3, Me._lvwSearchResults_ColumnHeader_4, Me._lvwSearchResults_ColumnHeader_5})
        Me.lvwSearchResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchResults.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchResults.FullRowSelect = True
        Me.lvwSearchResults.HideSelection = False
        Me.lvwSearchResults.LargeImageList = Me.imglImages
        Me.lvwSearchResults.Location = New System.Drawing.Point(8, 152)
        Me.lvwSearchResults.MultiSelect = False
        Me.lvwSearchResults.Name = "lvwSearchResults"
        Me.lvwSearchResults.Size = New System.Drawing.Size(537, 169)
        Me.lvwSearchResults.SmallImageList = Me.imglImages
        Me.lvwSearchResults.TabIndex = 7
        Me.lvwSearchResults.TabStop = False
        Me.lvwSearchResults.UseCompatibleStateImageBehavior = False
        Me.lvwSearchResults.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchResults_ColumnHeader_1
        '
        Me._lvwSearchResults_ColumnHeader_1.Tag = ""
        Me._lvwSearchResults_ColumnHeader_1.Text = "1"
        Me._lvwSearchResults_ColumnHeader_1.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_2
        '
        Me._lvwSearchResults_ColumnHeader_2.Tag = ""
        Me._lvwSearchResults_ColumnHeader_2.Text = "2"
        Me._lvwSearchResults_ColumnHeader_2.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_3
        '
        Me._lvwSearchResults_ColumnHeader_3.Tag = ""
        Me._lvwSearchResults_ColumnHeader_3.Text = "3"
        Me._lvwSearchResults_ColumnHeader_3.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_4
        '
        Me._lvwSearchResults_ColumnHeader_4.Tag = ""
        Me._lvwSearchResults_ColumnHeader_4.Text = "4"
        Me._lvwSearchResults_ColumnHeader_4.Width = 97
        '
        '_lvwSearchResults_ColumnHeader_5
        '
        Me._lvwSearchResults_ColumnHeader_5.Tag = ""
        Me._lvwSearchResults_ColumnHeader_5.Text = "5"
        Me._lvwSearchResults_ColumnHeader_5.Width = 97
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'ImgImage
        '
        Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
        Me.ImgImage.Location = New System.Drawing.Point(496, 96)
        Me.ImgImage.Name = "ImgImage"
        Me.ImgImage.Size = New System.Drawing.Size(32, 32)
        Me.ImgImage.TabIndex = 16
        Me.ImgImage.TabStop = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(553, 379)
        Me.Controls.Add(Me.cmdNavigate)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdNew)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.lvwSearchResults)
        Me.Controls.Add(Me.ImgImage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(159, 148)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Find: Bank"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._pnlMain_0.ResumeLayout(False)
        Me._pnlMain_0.PerformLayout()
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
	Sub InitializepnlMain()
		Me.pnlMain(0) = _pnlMain_0
	End Sub
	Sub lvwSearchResults_InitializeColumnKeys()
		Me._lvwSearchResults_ColumnHeader_1.Name = ""
		Me._lvwSearchResults_ColumnHeader_2.Name = ""
		Me._lvwSearchResults_ColumnHeader_3.Name = ""
		Me._lvwSearchResults_ColumnHeader_4.Name = ""
		Me._lvwSearchResults_ColumnHeader_5.Name = ""
	End Sub
#End Region 
End Class