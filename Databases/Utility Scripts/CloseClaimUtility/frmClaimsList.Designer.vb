<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
        isInitializingComponent = True
		InitializeComponent()
        isInitializingComponent = False
		lvwsearchdetails_InitializeColumnKeys()
		tabMainTabPreviousTab = tabMainTab.SelectedIndex
        Initialise()
	End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
     Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not fTerminateCalled_Form_Terminate_Renamed Then
                fTerminateCalled_Form_Terminate_Renamed = True
                'Form_Terminate_Renamed()
            End If
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Private WithEvents _lvwsearchdetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwsearchdetails As System.Windows.Forms.ListView
    Public WithEvents cmdView As System.Windows.Forms.Button
    Public WithEvents cmdNewSearch As System.Windows.Forms.Button
    Public WithEvents cmdFindNow As System.Windows.Forms.Button
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents stbstatus As System.Windows.Forms.StatusStrip
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Public WithEvents imglImages As System.Windows.Forms.ImageList
    Public WithEvents ImgImage As System.Windows.Forms.PictureBox
    'TODOLIST-Commented the listviewhelper as it was conflicting with icon display in listview
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lvwsearchdetails = New System.Windows.Forms.ListView()
        Me._lvwsearchdetails_ColumnHeader_0 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwsearchdetails_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdView = New System.Windows.Forms.Button()
        Me.cmdNewSearch = New System.Windows.Forms.Button()
        Me.cmdFindNow = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.stbstatus = New System.Windows.Forms.StatusStrip()
        Me._stbstatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.ImgImage = New System.Windows.Forms.PictureBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.stbstatus.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lvwsearchdetails
        '
        Me.lvwsearchdetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwsearchdetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwsearchdetails.CheckBoxes = True
        Me.lvwsearchdetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwsearchdetails_ColumnHeader_0, Me._lvwsearchdetails_ColumnHeader_1, Me._lvwsearchdetails_ColumnHeader_2, Me._lvwsearchdetails_ColumnHeader_9, Me._lvwsearchdetails_ColumnHeader_3, Me._lvwsearchdetails_ColumnHeader_4, Me._lvwsearchdetails_ColumnHeader_5, Me._lvwsearchdetails_ColumnHeader_6, Me._lvwsearchdetails_ColumnHeader_7, Me._lvwsearchdetails_ColumnHeader_8, Me._lvwsearchdetails_ColumnHeader_10})
        Me.lvwsearchdetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwsearchdetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwsearchdetails.LargeImageList = Me.imglImages
        Me.lvwsearchdetails.Location = New System.Drawing.Point(8, 162)
        Me.lvwsearchdetails.Name = "lvwsearchdetails"
        Me.lvwsearchdetails.Size = New System.Drawing.Size(718, 255)
        Me.lvwsearchdetails.SmallImageList = Me.imglImages
        Me.lvwsearchdetails.TabIndex = 17
        Me.lvwsearchdetails.UseCompatibleStateImageBehavior = False
        Me.lvwsearchdetails.View = System.Windows.Forms.View.Details
        '
        '_lvwsearchdetails_ColumnHeader_0
        '
        Me._lvwsearchdetails_ColumnHeader_0.Text = ""
        Me._lvwsearchdetails_ColumnHeader_0.Width = 17
        '
        '_lvwsearchdetails_ColumnHeader_1
        '
        Me._lvwsearchdetails_ColumnHeader_1.Tag = ""
        Me._lvwsearchdetails_ColumnHeader_1.Text = "Claim Number"
        Me._lvwsearchdetails_ColumnHeader_1.Width = 180
        '
        '_lvwsearchdetails_ColumnHeader_2
        '
        Me._lvwsearchdetails_ColumnHeader_2.Text = "Insured"
        Me._lvwsearchdetails_ColumnHeader_2.Width = 0
        '
        '_lvwsearchdetails_ColumnHeader_9
        '
        Me._lvwsearchdetails_ColumnHeader_9.Text = "Insurance File Cnt"
        Me._lvwsearchdetails_ColumnHeader_9.Width = 120
        '
        '_lvwsearchdetails_ColumnHeader_3
        '
        Me._lvwsearchdetails_ColumnHeader_3.Text = "Policy Number"
        Me._lvwsearchdetails_ColumnHeader_3.Width = 180
        '
        '_lvwsearchdetails_ColumnHeader_4
        '
        Me._lvwsearchdetails_ColumnHeader_4.Text = "Date of Loss"
        Me._lvwsearchdetails_ColumnHeader_4.Width = 100
        '
        '_lvwsearchdetails_ColumnHeader_5
        '
        Me._lvwsearchdetails_ColumnHeader_5.Text = "Class of Business"
        Me._lvwsearchdetails_ColumnHeader_5.Width = 0
        '
        '_lvwsearchdetails_ColumnHeader_6
        '
        Me._lvwsearchdetails_ColumnHeader_6.Text = "Claim Status"
        Me._lvwsearchdetails_ColumnHeader_6.Width = 0
        '
        '_lvwsearchdetails_ColumnHeader_7
        '
        Me._lvwsearchdetails_ColumnHeader_7.Text = "Claim Progress Status"
        Me._lvwsearchdetails_ColumnHeader_7.Width = 0
        '
        '_lvwsearchdetails_ColumnHeader_8
        '
        Me._lvwsearchdetails_ColumnHeader_8.Text = "Reserve Outstanding"
        Me._lvwsearchdetails_ColumnHeader_8.Width = 100
        '
        '_lvwsearchdetails_ColumnHeader_10
        '
        Me._lvwsearchdetails_ColumnHeader_10.Text = "Claim Id"
        Me._lvwsearchdetails_ColumnHeader_10.Width = 100
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "FindImage")
        '
        'cmdView
        '
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdView.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdView.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdView.Location = New System.Drawing.Point(16, 423)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdView.Size = New System.Drawing.Size(73, 22)
        Me.cmdView.TabIndex = 18
        Me.cmdView.TabStop = False
        Me.cmdView.Text = "Select All"
        Me.cmdView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(648, 56)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(81, 22)
        Me.cmdNewSearch.TabIndex = 16
        Me.cmdNewSearch.TabStop = False
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(648, 28)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
        Me.cmdFindNow.TabIndex = 15
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(656, 423)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 21
        Me.cmdHelp.TabStop = False
        Me.cmdHelp.Text = "P&rint"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(576, 423)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(496, 423)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.TabStop = False
        Me.cmdOK.Text = "&Proceed"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'stbstatus
        '
        Me.stbstatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbstatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbstatus_Panel1})
        Me.stbstatus.Location = New System.Drawing.Point(0, 451)
        Me.stbstatus.Name = "stbstatus"
        Me.stbstatus.ShowItemToolTips = True
        Me.stbstatus.Size = New System.Drawing.Size(734, 22)
        Me.stbstatus.TabIndex = 22
        '
        '_stbstatus_Panel1
        '
        Me._stbstatus_Panel1.AutoSize = False
        Me._stbstatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbstatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbstatus_Panel1.DoubleClickEnabled = True
        Me._stbstatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbstatus_Panel1.Name = "_stbstatus_Panel1"
        Me._stbstatus_Panel1.Size = New System.Drawing.Size(717, 22)
        Me._stbstatus_Panel1.Tag = ""
        Me._stbstatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(210, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(639, 148)
        Me.tabMainTab.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabMainTab.TabIndex = 23
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(631, 122)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'ImgImage
        '
        Me.ImgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.ImgImage.Image = CType(resources.GetObject("ImgImage.Image"), System.Drawing.Image)
        Me.ImgImage.Location = New System.Drawing.Point(669, 104)
        Me.ImgImage.Name = "ImgImage"
        Me.ImgImage.Size = New System.Drawing.Size(32, 32)
        Me.ImgImage.TabIndex = 24
        Me.ImgImage.TabStop = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button1.Location = New System.Drawing.Point(95, 424)
        Me.Button1.Name = "Button1"
        Me.Button1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Button1.Size = New System.Drawing.Size(81, 22)
        Me.Button1.TabIndex = 25
        Me.Button1.TabStop = False
        Me.Button1.Text = "Unselect All"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button1.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(734, 473)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lvwsearchdetails)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.stbstatus)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.ImgImage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(333, 130)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Find Claim"
        Me.stbstatus.ResumeLayout(False)
        Me.stbstatus.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        CType(Me.ImgImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lvwsearchdetails_InitializeColumnKeys()
        Me._lvwsearchdetails_ColumnHeader_1.Name = ""
    End Sub
    Private WithEvents _stbstatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents _lvwsearchdetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwsearchdetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwsearchdetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwsearchdetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwsearchdetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwsearchdetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwsearchdetails_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwsearchdetails_ColumnHeader_0 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwsearchdetails_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwsearchdetails_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
#End Region
End Class
