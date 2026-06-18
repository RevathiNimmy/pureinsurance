<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		lvwSearchDetails_InitializeColumnKeys()
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
    Public WithEvents uctPMResizer As PMResizerControl.uctPMResizer
    Public WithEvents cmdHelp As System.Windows.Forms.Button
    Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
    Public WithEvents imglImages As System.Windows.Forms.ImageList
    Public WithEvents imgImage As System.Windows.Forms.PictureBox
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    Private tabMainTabPreviousTab As Integer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uctPMResizer = New PMResizerControl.uctPMResizer
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.stbStatus = New System.Windows.Forms.StatusStrip
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.imglImages = New System.Windows.Forms.ImageList(Me.components)
        Me.imgImage = New System.Windows.Forms.PictureBox
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lvwSearchDetails = New System.Windows.Forms.ListView
        Me._lvwSearchDetails_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_7 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_8 = New System.Windows.Forms.ColumnHeader
        Me._lvwSearchDetails_ColumnHeader_9 = New System.Windows.Forms.ColumnHeader
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cmdMarkAll = New System.Windows.Forms.Button
        Me.cmdReconcile = New System.Windows.Forms.Button
        Me.cmdMark = New System.Windows.Forms.Button
        Me.cmdDrill = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblDateTo = New System.Windows.Forms.Label
        Me.lblMarkedStatus = New System.Windows.Forms.Label
        Me.lblMonth = New System.Windows.Forms.Label
        Me.lblTotalMarkedLabel = New System.Windows.Forms.Label
        Me.lblTotalMarked = New System.Windows.Forms.Label
        Me.lblBank = New System.Windows.Forms.Label
        Me.lblOpeningBalance = New System.Windows.Forms.Label
        Me.lblTotalReconciled = New System.Windows.Forms.Label
        Me.lblTotalUnReconciled = New System.Windows.Forms.Label
        Me.lblClosingBalance = New System.Windows.Forms.Label
        Me.lblTotalUnreconciledLabel = New System.Windows.Forms.Label
        Me.lblTotalReconciledLabel = New System.Windows.Forms.Label
        Me.lblOpeningBalanceLabel = New System.Windows.Forms.Label
        Me.lblClosingBalanceLabel = New System.Windows.Forms.Label
        Me.lblBankStatementBalance = New System.Windows.Forms.Label
        Me.lblClosed = New System.Windows.Forms.Label
        Me.txtDateTo = New System.Windows.Forms.TextBox
        Me.txtBankStatementBalance = New System.Windows.Forms.TextBox
        Me.cboMonth = New System.Windows.Forms.ComboBox
        Me.cboMarkedStatus = New System.Windows.Forms.ComboBox
        Me.uctBankAccount = New UserControls.BankAccount
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.stbStatus.SuspendLayout()
        CType(Me.imgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'uctPMResizer
        '
        Me.uctPMResizer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMResizer.Location = New System.Drawing.Point(560, 0)
        Me.uctPMResizer.Name = "uctPMResizer"
        Me.uctPMResizer.Size = New System.Drawing.Size(32, 30)
        Me.uctPMResizer.TabIndex = 12
        Me.uctPMResizer.Visible = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(794, 304)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 15
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 356)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.ShowItemToolTips = True
        Me.stbStatus.Size = New System.Drawing.Size(792, 22)
        Me.stbStatus.TabIndex = 17
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
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(775, 22)
        Me._stbStatus_Panel1.Tag = ""
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'imglImages
        '
        Me.imglImages.ImageStream = CType(resources.GetObject("imglImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imglImages.Tag = "Sirius For Broking Rules"
        Me.imglImages.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.imglImages.Images.SetKeyName(0, "check")
        Me.imglImages.Images.SetKeyName(1, "reconcile")
        '
        'imgImage
        '
        Me.imgImage.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgImage.Image = CType(resources.GetObject("imgImage.Image"), System.Drawing.Image)
        Me.imgImage.Location = New System.Drawing.Point(802, 81)
        Me.imgImage.Name = "imgImage"
        Me.imgImage.Size = New System.Drawing.Size(32, 32)
        Me.imgImage.TabIndex = 18
        Me.imgImage.TabStop = False
        '
        'lvwSearchDetails
        '
        Me.lvwSearchDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwSearchDetails.BackColor = System.Drawing.SystemColors.Window
        Me.lvwSearchDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwSearchDetails.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwSearchDetails_ColumnHeader_1, Me._lvwSearchDetails_ColumnHeader_2, Me._lvwSearchDetails_ColumnHeader_3, Me._lvwSearchDetails_ColumnHeader_4, Me._lvwSearchDetails_ColumnHeader_5, Me._lvwSearchDetails_ColumnHeader_6, Me._lvwSearchDetails_ColumnHeader_7, Me._lvwSearchDetails_ColumnHeader_8, Me._lvwSearchDetails_ColumnHeader_9})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwSearchDetails, True)
        Me.lvwSearchDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwSearchDetails.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwSearchDetails.FullRowSelect = True
        Me.lvwSearchDetails.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwSearchDetails, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.LargeImageList = Me.imglImages
        Me.lvwSearchDetails.Location = New System.Drawing.Point(0, 0)
        Me.lvwSearchDetails.Name = "lvwSearchDetails"
        Me.lvwSearchDetails.Size = New System.Drawing.Size(792, 160)
        Me.listViewHelper1.SetSmallIcons(Me.lvwSearchDetails, "")
        Me.lvwSearchDetails.SmallImageList = Me.imglImages
        Me.listViewHelper1.SetSorted(Me.lvwSearchDetails, False)
        Me.listViewHelper1.SetSortKey(Me.lvwSearchDetails, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwSearchDetails, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwSearchDetails.TabIndex = 10
        Me.lvwSearchDetails.UseCompatibleStateImageBehavior = False
        Me.lvwSearchDetails.View = System.Windows.Forms.View.Details
        '
        '_lvwSearchDetails_ColumnHeader_1
        '
        Me._lvwSearchDetails_ColumnHeader_1.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_1.Text = " "
        Me._lvwSearchDetails_ColumnHeader_1.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_2
        '
        Me._lvwSearchDetails_ColumnHeader_2.Tag = "DATESORT"
        Me._lvwSearchDetails_ColumnHeader_2.Text = ""
        Me._lvwSearchDetails_ColumnHeader_2.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_3
        '
        Me._lvwSearchDetails_ColumnHeader_3.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_3.Text = ""
        Me._lvwSearchDetails_ColumnHeader_3.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_4
        '
        Me._lvwSearchDetails_ColumnHeader_4.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_4.Text = ""
        Me._lvwSearchDetails_ColumnHeader_4.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_5
        '
        Me._lvwSearchDetails_ColumnHeader_5.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_5.Text = ""
        Me._lvwSearchDetails_ColumnHeader_5.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_6
        '
        Me._lvwSearchDetails_ColumnHeader_6.Tag = "DATESORT"
        Me._lvwSearchDetails_ColumnHeader_6.Text = ""
        Me._lvwSearchDetails_ColumnHeader_6.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_7
        '
        Me._lvwSearchDetails_ColumnHeader_7.Tag = "VALUESORT"
        Me._lvwSearchDetails_ColumnHeader_7.Text = ""
        Me._lvwSearchDetails_ColumnHeader_7.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_8
        '
        Me._lvwSearchDetails_ColumnHeader_8.Tag = ""
        Me._lvwSearchDetails_ColumnHeader_8.Text = ""
        Me._lvwSearchDetails_ColumnHeader_8.Width = 97
        '
        '_lvwSearchDetails_ColumnHeader_9
        '
        Me._lvwSearchDetails_ColumnHeader_9.Tag = "DATESORT"
        Me._lvwSearchDetails_ColumnHeader_9.Text = ""
        Me._lvwSearchDetails_ColumnHeader_9.Width = 97
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmdMarkAll)
        Me.Panel1.Controls.Add(Me.cmdReconcile)
        Me.Panel1.Controls.Add(Me.cmdMark)
        Me.Panel1.Controls.Add(Me.cmdDrill)
        Me.Panel1.Controls.Add(Me.cmdCancel)
        Me.Panel1.Controls.Add(Me.cmdOK)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 322)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(792, 34)
        Me.Panel1.TabIndex = 19
        '
        'cmdMarkAll
        '
        Me.cmdMarkAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdMarkAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMarkAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMarkAll.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMarkAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMarkAll.Location = New System.Drawing.Point(167, 6)
        Me.cmdMarkAll.Name = "cmdMarkAll"
        Me.cmdMarkAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMarkAll.Size = New System.Drawing.Size(73, 22)
        Me.cmdMarkAll.TabIndex = 17
        Me.cmdMarkAll.Text = "M&ark All"
        Me.cmdMarkAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMarkAll.UseVisualStyleBackColor = False
        '
        'cmdReconcile
        '
        Me.cmdReconcile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdReconcile.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReconcile.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReconcile.Enabled = False
        Me.cmdReconcile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReconcile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReconcile.Location = New System.Drawing.Point(7, 6)
        Me.cmdReconcile.Name = "cmdReconcile"
        Me.cmdReconcile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReconcile.Size = New System.Drawing.Size(73, 22)
        Me.cmdReconcile.TabIndex = 15
        Me.cmdReconcile.Text = "&Reconcile"
        Me.cmdReconcile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReconcile.UseVisualStyleBackColor = False
        '
        'cmdMark
        '
        Me.cmdMark.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdMark.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMark.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMark.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdMark.Enabled = False
        Me.cmdMark.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMark.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMark.Location = New System.Drawing.Point(87, 6)
        Me.cmdMark.Name = "cmdMark"
        Me.cmdMark.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMark.Size = New System.Drawing.Size(73, 22)
        Me.cmdMark.TabIndex = 16
        Me.cmdMark.Text = "&Mark"
        Me.cmdMark.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMark.UseVisualStyleBackColor = False
        '
        'cmdDrill
        '
        Me.cmdDrill.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDrill.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDrill.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDrill.Enabled = False
        Me.cmdDrill.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDrill.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDrill.Location = New System.Drawing.Point(247, 6)
        Me.cmdDrill.Name = "cmdDrill"
        Me.cmdDrill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDrill.Size = New System.Drawing.Size(73, 22)
        Me.cmdDrill.TabIndex = 18
        Me.cmdDrill.Text = "&Drill"
        Me.cmdDrill.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDrill.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(713, 6)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 20
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(634, 6)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 19
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(792, 356)
        Me.Panel2.TabIndex = 20
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lvwSearchDetails)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 158)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(792, 198)
        Me.Panel3.TabIndex = 21
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.tabMainTab)
        Me.Panel4.Controls.Add(Me.Panel5)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(792, 158)
        Me.Panel4.TabIndex = 22
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(230, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(0, 0)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(698, 158)
        Me.tabMainTab.TabIndex = 18
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDateTo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblMarkedStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblMonth)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTotalMarkedLabel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTotalMarked)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBank)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblOpeningBalance)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTotalReconciled)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTotalUnReconciled)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClosingBalance)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTotalUnreconciledLabel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTotalReconciledLabel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblOpeningBalanceLabel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClosingBalanceLabel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBankStatementBalance)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblClosed)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDateTo)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBankStatementBalance)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboMonth)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboMarkedStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctBankAccount)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(690, 132)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = " 1 - Details"
        '
        'lblDateTo
        '
        Me.lblDateTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateTo.Location = New System.Drawing.Point(12, 46)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateTo.Size = New System.Drawing.Size(57, 17)
        Me.lblDateTo.TabIndex = 18
        Me.lblDateTo.Text = "Date To:"
        '
        'lblMarkedStatus
        '
        Me.lblMarkedStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblMarkedStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMarkedStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMarkedStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMarkedStatus.Location = New System.Drawing.Point(288, 16)
        Me.lblMarkedStatus.Name = "lblMarkedStatus"
        Me.lblMarkedStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMarkedStatus.Size = New System.Drawing.Size(81, 17)
        Me.lblMarkedStatus.TabIndex = 19
        Me.lblMarkedStatus.Text = "Reconciled ?"
        '
        'lblMonth
        '
        Me.lblMonth.BackColor = System.Drawing.SystemColors.Control
        Me.lblMonth.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMonth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMonth.Location = New System.Drawing.Point(288, 46)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMonth.Size = New System.Drawing.Size(41, 17)
        Me.lblMonth.TabIndex = 20
        Me.lblMonth.Text = "Month:"
        '
        'lblTotalMarkedLabel
        '
        Me.lblTotalMarkedLabel.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalMarkedLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalMarkedLabel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalMarkedLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalMarkedLabel.Location = New System.Drawing.Point(12, 84)
        Me.lblTotalMarkedLabel.Name = "lblTotalMarkedLabel"
        Me.lblTotalMarkedLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalMarkedLabel.Size = New System.Drawing.Size(86, 19)
        Me.lblTotalMarkedLabel.TabIndex = 21
        Me.lblTotalMarkedLabel.Text = "Total Marked:"
        '
        'lblTotalMarked
        '
        Me.lblTotalMarked.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalMarked.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalMarked.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalMarked.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalMarked.Location = New System.Drawing.Point(96, 84)
        Me.lblTotalMarked.Name = "lblTotalMarked"
        Me.lblTotalMarked.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalMarked.Size = New System.Drawing.Size(115, 25)
        Me.lblTotalMarked.TabIndex = 22
        Me.lblTotalMarked.Text = "00.00"
        Me.lblTotalMarked.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblBank
        '
        Me.lblBank.BackColor = System.Drawing.SystemColors.Control
        Me.lblBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBank.Location = New System.Drawing.Point(12, 14)
        Me.lblBank.Name = "lblBank"
        Me.lblBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBank.Size = New System.Drawing.Size(55, 17)
        Me.lblBank.TabIndex = 23
        Me.lblBank.Text = "Bank:"
        '
        'lblOpeningBalance
        '
        Me.lblOpeningBalance.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOpeningBalance.BackColor = System.Drawing.Color.Transparent
        Me.lblOpeningBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOpeningBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOpeningBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOpeningBalance.Location = New System.Drawing.Point(554, 12)
        Me.lblOpeningBalance.Name = "lblOpeningBalance"
        Me.lblOpeningBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOpeningBalance.Size = New System.Drawing.Size(130, 17)
        Me.lblOpeningBalance.TabIndex = 28
        Me.lblOpeningBalance.Text = "0.00"
        Me.lblOpeningBalance.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTotalReconciled
        '
        Me.lblTotalReconciled.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalReconciled.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalReconciled.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalReconciled.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalReconciled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalReconciled.Location = New System.Drawing.Point(551, 36)
        Me.lblTotalReconciled.Name = "lblTotalReconciled"
        Me.lblTotalReconciled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalReconciled.Size = New System.Drawing.Size(133, 17)
        Me.lblTotalReconciled.TabIndex = 29
        Me.lblTotalReconciled.Text = "0.00"
        Me.lblTotalReconciled.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTotalUnReconciled
        '
        Me.lblTotalUnReconciled.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalUnReconciled.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalUnReconciled.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalUnReconciled.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalUnReconciled.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalUnReconciled.Location = New System.Drawing.Point(551, 60)
        Me.lblTotalUnReconciled.Name = "lblTotalUnReconciled"
        Me.lblTotalUnReconciled.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalUnReconciled.Size = New System.Drawing.Size(138, 15)
        Me.lblTotalUnReconciled.TabIndex = 31
        Me.lblTotalUnReconciled.Text = "0.00"
        Me.lblTotalUnReconciled.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblClosingBalance
        '
        Me.lblClosingBalance.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblClosingBalance.BackColor = System.Drawing.Color.Transparent
        Me.lblClosingBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClosingBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClosingBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClosingBalance.Location = New System.Drawing.Point(548, 84)
        Me.lblClosingBalance.Name = "lblClosingBalance"
        Me.lblClosingBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClosingBalance.Size = New System.Drawing.Size(136, 17)
        Me.lblClosingBalance.TabIndex = 31
        Me.lblClosingBalance.Text = "0.00"
        Me.lblClosingBalance.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTotalUnreconciledLabel
        '
        Me.lblTotalUnreconciledLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalUnreconciledLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalUnreconciledLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalUnreconciledLabel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalUnreconciledLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalUnreconciledLabel.Location = New System.Drawing.Point(440, 60)
        Me.lblTotalUnreconciledLabel.Name = "lblTotalUnreconciledLabel"
        Me.lblTotalUnreconciledLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalUnreconciledLabel.Size = New System.Drawing.Size(118, 17)
        Me.lblTotalUnreconciledLabel.TabIndex = 24
        Me.lblTotalUnreconciledLabel.Text = "Total Unreconciled:"
        '
        'lblTotalReconciledLabel
        '
        Me.lblTotalReconciledLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotalReconciledLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblTotalReconciledLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalReconciledLabel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalReconciledLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalReconciledLabel.Location = New System.Drawing.Point(440, 36)
        Me.lblTotalReconciledLabel.Name = "lblTotalReconciledLabel"
        Me.lblTotalReconciledLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalReconciledLabel.Size = New System.Drawing.Size(121, 17)
        Me.lblTotalReconciledLabel.TabIndex = 25
        Me.lblTotalReconciledLabel.Text = "Total Reconciled:"
        '
        'lblOpeningBalanceLabel
        '
        Me.lblOpeningBalanceLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOpeningBalanceLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblOpeningBalanceLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOpeningBalanceLabel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOpeningBalanceLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOpeningBalanceLabel.Location = New System.Drawing.Point(440, 12)
        Me.lblOpeningBalanceLabel.Name = "lblOpeningBalanceLabel"
        Me.lblOpeningBalanceLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOpeningBalanceLabel.Size = New System.Drawing.Size(109, 17)
        Me.lblOpeningBalanceLabel.TabIndex = 26
        Me.lblOpeningBalanceLabel.Text = "Opening Balance:"
        '
        'lblClosingBalanceLabel
        '
        Me.lblClosingBalanceLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblClosingBalanceLabel.BackColor = System.Drawing.Color.Transparent
        Me.lblClosingBalanceLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClosingBalanceLabel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClosingBalanceLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClosingBalanceLabel.Location = New System.Drawing.Point(442, 84)
        Me.lblClosingBalanceLabel.Name = "lblClosingBalanceLabel"
        Me.lblClosingBalanceLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClosingBalanceLabel.Size = New System.Drawing.Size(129, 15)
        Me.lblClosingBalanceLabel.TabIndex = 27
        Me.lblClosingBalanceLabel.Text = "Closing Balance:"
        '
        'lblBankStatementBalance
        '
        Me.lblBankStatementBalance.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankStatementBalance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankStatementBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankStatementBalance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankStatementBalance.Location = New System.Drawing.Point(217, 92)
        Me.lblBankStatementBalance.Name = "lblBankStatementBalance"
        Me.lblBankStatementBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankStatementBalance.Size = New System.Drawing.Size(105, 27)
        Me.lblBankStatementBalance.TabIndex = 32
        Me.lblBankStatementBalance.Text = "Bank Statement Balance Amount:"
        '
        'lblClosed
        '
        Me.lblClosed.AutoSize = True
        Me.lblClosed.BackColor = System.Drawing.SystemColors.Control
        Me.lblClosed.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClosed.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClosed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClosed.Location = New System.Drawing.Point(217, 80)
        Me.lblClosed.Name = "lblClosed"
        Me.lblClosed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClosed.Size = New System.Drawing.Size(46, 13)
        Me.lblClosed.TabIndex = 33
        Me.lblClosed.Text = "Closed"
        '
        'txtDateTo
        '
        Me.txtDateTo.AcceptsReturn = True
        Me.txtDateTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateTo.Location = New System.Drawing.Point(72, 42)
        Me.txtDateTo.MaxLength = 0
        Me.txtDateTo.Name = "txtDateTo"
        Me.txtDateTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateTo.Size = New System.Drawing.Size(161, 21)
        Me.txtDateTo.TabIndex = 1
        '
        'txtBankStatementBalance
        '
        Me.txtBankStatementBalance.AcceptsReturn = True
        Me.txtBankStatementBalance.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBankStatementBalance.BackColor = System.Drawing.SystemColors.Window
        Me.txtBankStatementBalance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBankStatementBalance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankStatementBalance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBankStatementBalance.Location = New System.Drawing.Point(328, 92)
        Me.txtBankStatementBalance.MaxLength = 0
        Me.txtBankStatementBalance.Name = "txtBankStatementBalance"
        Me.txtBankStatementBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBankStatementBalance.Size = New System.Drawing.Size(111, 21)
        Me.txtBankStatementBalance.TabIndex = 4
        '
        'cboMonth
        '
        Me.cboMonth.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMonth.BackColor = System.Drawing.SystemColors.Window
        Me.cboMonth.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMonth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMonth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMonth.Location = New System.Drawing.Point(338, 42)
        Me.cboMonth.Name = "cboMonth"
        Me.cboMonth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMonth.Size = New System.Drawing.Size(97, 21)
        Me.cboMonth.TabIndex = 3
        '
        'cboMarkedStatus
        '
        Me.cboMarkedStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMarkedStatus.BackColor = System.Drawing.SystemColors.Window
        Me.cboMarkedStatus.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboMarkedStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMarkedStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMarkedStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboMarkedStatus.Location = New System.Drawing.Point(370, 12)
        Me.cboMarkedStatus.Name = "cboMarkedStatus"
        Me.cboMarkedStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboMarkedStatus.Size = New System.Drawing.Size(65, 21)
        Me.cboMarkedStatus.TabIndex = 2
        '
        'uctBankAccount
        '
        Me.uctBankAccount.DefaultId = "0"
        Me.uctBankAccount.FirstItem = ""
        Me.uctBankAccount.Id = 0
        Me.uctBankAccount.ListIndex = -1
        Me.uctBankAccount.Location = New System.Drawing.Point(72, 12)
        Me.uctBankAccount.Name = "uctBankAccount"
        Me.uctBankAccount.Size = New System.Drawing.Size(161, 21)
        Me.uctBankAccount.TabIndex = 0
        Me.uctBankAccount.ToolTipText = ""
        Me.uctBankAccount.WhatsThisHelpID = 0
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.cmdReport)
        Me.Panel5.Controls.Add(Me.cmdNewSearch)
        Me.Panel5.Controls.Add(Me.cmdFindNow)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel5.Location = New System.Drawing.Point(698, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(94, 158)
        Me.Panel5.TabIndex = 24
        '
        'cmdReport
        '
        Me.cmdReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Location = New System.Drawing.Point(3, 96)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(83, 22)
        Me.cmdReport.TabIndex = 23
        Me.cmdReport.Text = "&Report"
        Me.cmdReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(4, 39)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(83, 22)
        Me.cmdNewSearch.TabIndex = 22
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(3, 12)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(83, 22)
        Me.cmdFindNow.TabIndex = 21
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(792, 378)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.uctPMResizer)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.stbStatus)
        Me.Controls.Add(Me.imgImage)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(191, 280)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Insurer/Agent Payment"
        Me.stbStatus.ResumeLayout(False)
        Me.stbStatus.PerformLayout()
        CType(Me.imgImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Sub lvwSearchDetails_InitializeColumnKeys()
        Me._lvwSearchDetails_ColumnHeader_1.Name = ""
        Me._lvwSearchDetails_ColumnHeader_2.Name = ""
        Me._lvwSearchDetails_ColumnHeader_3.Name = ""
        Me._lvwSearchDetails_ColumnHeader_4.Name = ""
        Me._lvwSearchDetails_ColumnHeader_5.Name = ""
        Me._lvwSearchDetails_ColumnHeader_6.Name = ""
        Me._lvwSearchDetails_ColumnHeader_7.Name = ""
        Me._lvwSearchDetails_ColumnHeader_8.Name = ""
        Me._lvwSearchDetails_ColumnHeader_9.Name = ""
    End Sub
    Private WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents cmdMarkAll As System.Windows.Forms.Button
    Public WithEvents cmdReconcile As System.Windows.Forms.Button
    Public WithEvents cmdMark As System.Windows.Forms.Button
    Public WithEvents cmdDrill As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Public WithEvents lvwSearchDetails As System.Windows.Forms.ListView
    Private WithEvents _lvwSearchDetails_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwSearchDetails_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Public WithEvents tabMainTab As System.Windows.Forms.TabControl
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lblDateTo As System.Windows.Forms.Label
    Public WithEvents lblMarkedStatus As System.Windows.Forms.Label
    Public WithEvents lblMonth As System.Windows.Forms.Label
    Public WithEvents lblTotalMarkedLabel As System.Windows.Forms.Label
    Public WithEvents lblTotalMarked As System.Windows.Forms.Label
    Public WithEvents lblBank As System.Windows.Forms.Label
    Public WithEvents lblOpeningBalance As System.Windows.Forms.Label
    Public WithEvents lblTotalReconciled As System.Windows.Forms.Label
    Public WithEvents lblTotalUnReconciled As System.Windows.Forms.Label
    Public WithEvents lblClosingBalance As System.Windows.Forms.Label
    Public WithEvents lblTotalUnreconciledLabel As System.Windows.Forms.Label
    Public WithEvents lblTotalReconciledLabel As System.Windows.Forms.Label
    Public WithEvents lblOpeningBalanceLabel As System.Windows.Forms.Label
    Public WithEvents lblClosingBalanceLabel As System.Windows.Forms.Label
    Public WithEvents lblBankStatementBalance As System.Windows.Forms.Label
    Public WithEvents lblClosed As System.Windows.Forms.Label
    Public WithEvents txtDateTo As System.Windows.Forms.TextBox
    Public WithEvents txtBankStatementBalance As System.Windows.Forms.TextBox
    Public WithEvents cboMonth As System.Windows.Forms.ComboBox
    Public WithEvents cboMarkedStatus As System.Windows.Forms.ComboBox
    Public WithEvents uctBankAccount As UserControls.BankAccount
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmdNewSearch As System.Windows.Forms.Button
    Public WithEvents cmdFindNow As System.Windows.Forms.Button
#End Region
End Class