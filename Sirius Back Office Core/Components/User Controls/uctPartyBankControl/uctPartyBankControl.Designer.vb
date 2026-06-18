<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPartyBankControl
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializecmdPrevious()
        InitializecmdNext()
        UserControl_Initialize()
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
    Friend WithEvents cmdBankSelectAll As System.Windows.Forms.Button
    Friend WithEvents cmdBankDelete As System.Windows.Forms.Button
    Friend WithEvents uctAnchor As uSIRCommonControls.uctAnchor
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents cmdBankActiveInactive As System.Windows.Forms.Button
    Friend WithEvents cmdBankEdit As System.Windows.Forms.Button
    Friend WithEvents cmdBankAdd As System.Windows.Forms.Button
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_16 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_17 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_18 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_19 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsList_ColumnHeader_20 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwBankDetailsList As System.Windows.Forms.ListView
    Friend WithEvents fraBankDetails As System.Windows.Forms.GroupBox
    Friend WithEvents _cmdNext_0 As System.Windows.Forms.Button
    Friend WithEvents _tabPartyBank_TabPage0 As System.Windows.Forms.TabPage
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents _lvwBankDetailsHistory_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwBankDetailsHistory As System.Windows.Forms.ListView
    Friend WithEvents _cmdPrevious_0 As System.Windows.Forms.Button
    Friend WithEvents _tabPartyBank_TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents tabPartyBank As System.Windows.Forms.TabControl
    Friend cmdNext(0) As System.Windows.Forms.Button
    Friend cmdPrevious(0) As System.Windows.Forms.Button
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    '<System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPartyBankControl))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabPartyBank = New System.Windows.Forms.TabControl()
        Me._tabPartyBank_TabPage0 = New System.Windows.Forms.TabPage()
        Me.fraBankDetails = New System.Windows.Forms.GroupBox()
        Me.cmdBankSelectAll = New System.Windows.Forms.Button()
        Me.cmdBankDelete = New System.Windows.Forms.Button()
        Me.uctAnchor = New uSIRCommonControls.uctAnchor()
        Me.cmdBankActiveInactive = New System.Windows.Forms.Button()
        Me.cmdBankEdit = New System.Windows.Forms.Button()
        Me.cmdBankAdd = New System.Windows.Forms.Button()
        Me.lvwBankDetailsList = New System.Windows.Forms.ListView()
        Me._lvwBankDetailsList_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_18 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_19 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_20 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsList_ColumnHeader_21 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me._cmdNext_0 = New System.Windows.Forms.Button()
        Me._tabPartyBank_TabPage1 = New System.Windows.Forms.TabPage()
        Me.lvwBankDetailsHistory = New System.Windows.Forms.ListView()
        Me._lvwBankDetailsHistory_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwBankDetailsHistory_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._cmdPrevious_0 = New System.Windows.Forms.Button()
        Me.tabPartyBank.SuspendLayout()
        Me._tabPartyBank_TabPage0.SuspendLayout()
        Me.fraBankDetails.SuspendLayout()
        Me._tabPartyBank_TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabPartyBank
        '
        Me.tabPartyBank.Controls.Add(Me._tabPartyBank_TabPage0)
        Me.tabPartyBank.Controls.Add(Me._tabPartyBank_TabPage1)
        Me.tabPartyBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabPartyBank.ItemSize = New System.Drawing.Size(316, 18)
        Me.tabPartyBank.Location = New System.Drawing.Point(2, 2)
        Me.tabPartyBank.Multiline = True
        Me.tabPartyBank.Name = "tabPartyBank"
        Me.tabPartyBank.SelectedIndex = 0
        Me.tabPartyBank.ShowToolTips = True
        Me.tabPartyBank.Size = New System.Drawing.Size(686, 366)
        Me.tabPartyBank.TabIndex = 0
        '
        '_tabPartyBank_TabPage0
        '
        Me._tabPartyBank_TabPage0.Controls.Add(Me.fraBankDetails)
        Me._tabPartyBank_TabPage0.Controls.Add(Me._cmdNext_0)
        Me._tabPartyBank_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabPartyBank_TabPage0.Name = "_tabPartyBank_TabPage0"
        Me._tabPartyBank_TabPage0.Size = New System.Drawing.Size(678, 340)
        Me._tabPartyBank_TabPage0.TabIndex = 0
        Me._tabPartyBank_TabPage0.Text = "Payment Details"
        '
        'fraBankDetails
        '
        Me.fraBankDetails.BackColor = System.Drawing.SystemColors.Control
        Me.fraBankDetails.Controls.Add(Me.cmdBankSelectAll)
        Me.fraBankDetails.Controls.Add(Me.cmdBankDelete)
        Me.fraBankDetails.Controls.Add(Me.uctAnchor)
        Me.fraBankDetails.Controls.Add(Me.cmdBankActiveInactive)
        Me.fraBankDetails.Controls.Add(Me.cmdBankEdit)
        Me.fraBankDetails.Controls.Add(Me.cmdBankAdd)
        Me.fraBankDetails.Controls.Add(Me.lvwBankDetailsList)
        Me.fraBankDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraBankDetails.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraBankDetails.Location = New System.Drawing.Point(4, 4)
        Me.fraBankDetails.Name = "fraBankDetails"
        Me.fraBankDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraBankDetails.Size = New System.Drawing.Size(671, 305)
        Me.fraBankDetails.TabIndex = 1
        Me.fraBankDetails.TabStop = False
        Me.fraBankDetails.Text = "Bank Details"
        '
        'cmdBankSelectAll
        '
        Me.cmdBankSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBankSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBankSelectAll.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBankSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBankSelectAll.Location = New System.Drawing.Point(432, 277)
        Me.cmdBankSelectAll.Name = "cmdBankSelectAll"
        Me.cmdBankSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBankSelectAll.Size = New System.Drawing.Size(91, 23)
        Me.cmdBankSelectAll.TabIndex = 8
        Me.cmdBankSelectAll.Text = "&Select All"
        Me.cmdBankSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBankSelectAll.UseVisualStyleBackColor = False
        '
        'cmdBankDelete
        '
        Me.cmdBankDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBankDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBankDelete.Enabled = False
        Me.cmdBankDelete.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBankDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBankDelete.Location = New System.Drawing.Point(206, 277)
        Me.cmdBankDelete.Name = "cmdBankDelete"
        Me.cmdBankDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBankDelete.Size = New System.Drawing.Size(91, 23)
        Me.cmdBankDelete.TabIndex = 7
        Me.cmdBankDelete.Text = "&Delete"
        Me.cmdBankDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBankDelete.UseVisualStyleBackColor = False
        '
        'uctAnchor
        '
        Me.uctAnchor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAnchor.Location = New System.Drawing.Point(282, 284)
        Me.uctAnchor.Name = "uctAnchor"
        Me.uctAnchor.Size = New System.Drawing.Size(44, 16)
        Me.uctAnchor.TabIndex = 9
        Me.uctAnchor.Visible = False
        '
        'cmdBankActiveInactive
        '
        Me.cmdBankActiveInactive.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBankActiveInactive.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBankActiveInactive.Enabled = False
        Me.cmdBankActiveInactive.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBankActiveInactive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBankActiveInactive.Location = New System.Drawing.Point(332, 277)
        Me.cmdBankActiveInactive.Name = "cmdBankActiveInactive"
        Me.cmdBankActiveInactive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBankActiveInactive.Size = New System.Drawing.Size(91, 23)
        Me.cmdBankActiveInactive.TabIndex = 4
        Me.cmdBankActiveInactive.Text = "&Inactive"
        Me.cmdBankActiveInactive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBankActiveInactive.UseVisualStyleBackColor = False
        '
        'cmdBankEdit
        '
        Me.cmdBankEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBankEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBankEdit.Enabled = False
        Me.cmdBankEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBankEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBankEdit.Location = New System.Drawing.Point(106, 277)
        Me.cmdBankEdit.Name = "cmdBankEdit"
        Me.cmdBankEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBankEdit.Size = New System.Drawing.Size(91, 23)
        Me.cmdBankEdit.TabIndex = 3
        Me.cmdBankEdit.Text = "&Edit"
        Me.cmdBankEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBankEdit.UseVisualStyleBackColor = False
        '
        'cmdBankAdd
        '
        Me.cmdBankAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBankAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBankAdd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBankAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBankAdd.Location = New System.Drawing.Point(6, 277)
        Me.cmdBankAdd.Name = "cmdBankAdd"
        Me.cmdBankAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBankAdd.Size = New System.Drawing.Size(91, 23)
        Me.cmdBankAdd.TabIndex = 2
        Me.cmdBankAdd.Text = "&Add"
        Me.cmdBankAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBankAdd.UseVisualStyleBackColor = False
        '
        'lvwBankDetailsList
        '
        Me.lvwBankDetailsList.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBankDetailsList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwBankDetailsList_ColumnHeader_1, Me._lvwBankDetailsList_ColumnHeader_2, Me._lvwBankDetailsList_ColumnHeader_3, Me._lvwBankDetailsList_ColumnHeader_4, Me._lvwBankDetailsList_ColumnHeader_5, Me._lvwBankDetailsList_ColumnHeader_6, Me._lvwBankDetailsList_ColumnHeader_7, Me._lvwBankDetailsList_ColumnHeader_8, Me._lvwBankDetailsList_ColumnHeader_9, Me._lvwBankDetailsList_ColumnHeader_10, Me._lvwBankDetailsList_ColumnHeader_11, Me._lvwBankDetailsList_ColumnHeader_12, Me._lvwBankDetailsList_ColumnHeader_13, Me._lvwBankDetailsList_ColumnHeader_14, Me._lvwBankDetailsList_ColumnHeader_15, Me._lvwBankDetailsList_ColumnHeader_16, Me._lvwBankDetailsList_ColumnHeader_17, Me._lvwBankDetailsList_ColumnHeader_18, Me._lvwBankDetailsList_ColumnHeader_19, Me._lvwBankDetailsList_ColumnHeader_20, Me._lvwBankDetailsList_ColumnHeader_21})
        Me.lvwBankDetailsList.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBankDetailsList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBankDetailsList.FullRowSelect = True
        Me.lvwBankDetailsList.HideSelection = False
        Me.lvwBankDetailsList.LargeImageList = Me.ImageList1
        Me.lvwBankDetailsList.Location = New System.Drawing.Point(8, 14)
        Me.lvwBankDetailsList.Name = "lvwBankDetailsList"
        Me.lvwBankDetailsList.Size = New System.Drawing.Size(652, 257)
        Me.lvwBankDetailsList.SmallImageList = Me.ImageList1
        Me.lvwBankDetailsList.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwBankDetailsList.TabIndex = 5
        Me.lvwBankDetailsList.UseCompatibleStateImageBehavior = False
        Me.lvwBankDetailsList.View = System.Windows.Forms.View.Details
        '
        '_lvwBankDetailsList_ColumnHeader_1
        '
        Me._lvwBankDetailsList_ColumnHeader_1.Text = "Payment Type"
        Me._lvwBankDetailsList_ColumnHeader_1.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_2
        '
        Me._lvwBankDetailsList_ColumnHeader_2.Text = "Account Type"
        Me._lvwBankDetailsList_ColumnHeader_2.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_3
        '
        Me._lvwBankDetailsList_ColumnHeader_3.Text = "Account Holder Name"
        Me._lvwBankDetailsList_ColumnHeader_3.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_4
        '
        Me._lvwBankDetailsList_ColumnHeader_4.Text = "Account Number"
        Me._lvwBankDetailsList_ColumnHeader_4.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_5
        '
        Me._lvwBankDetailsList_ColumnHeader_5.Text = "Bank Branch Code"
        Me._lvwBankDetailsList_ColumnHeader_5.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_6
        '
        Me._lvwBankDetailsList_ColumnHeader_6.Text = "BIC"
        Me._lvwBankDetailsList_ColumnHeader_6.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_7
        '
        Me._lvwBankDetailsList_ColumnHeader_7.Text = "IBAN"
        '
        '_lvwBankDetailsList_ColumnHeader_8
        '
        Me._lvwBankDetailsList_ColumnHeader_8.Text = "Bank Branch"
        Me._lvwBankDetailsList_ColumnHeader_8.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_9
        '
        Me._lvwBankDetailsList_ColumnHeader_9.Text = "Bank Name"
        Me._lvwBankDetailsList_ColumnHeader_9.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_10
        '
        Me._lvwBankDetailsList_ColumnHeader_10.Text = "Expiry Date"
        Me._lvwBankDetailsList_ColumnHeader_10.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_11
        '
        Me._lvwBankDetailsList_ColumnHeader_11.Text = "Start Date"
        Me._lvwBankDetailsList_ColumnHeader_11.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_12
        '
        Me._lvwBankDetailsList_ColumnHeader_12.Text = "Issue Number"
        Me._lvwBankDetailsList_ColumnHeader_12.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_13
        '
        Me._lvwBankDetailsList_ColumnHeader_13.Text = "Manual Authorisation"
        Me._lvwBankDetailsList_ColumnHeader_13.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_14
        '
        Me._lvwBankDetailsList_ColumnHeader_14.Text = "Authorisation Code"
        Me._lvwBankDetailsList_ColumnHeader_14.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_15
        '
        Me._lvwBankDetailsList_ColumnHeader_15.Text = "No/Name Street"
        Me._lvwBankDetailsList_ColumnHeader_15.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_16
        '
        Me._lvwBankDetailsList_ColumnHeader_16.Text = "Locality"
        Me._lvwBankDetailsList_ColumnHeader_16.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_17
        '
        Me._lvwBankDetailsList_ColumnHeader_17.Text = "Post Town"
        Me._lvwBankDetailsList_ColumnHeader_17.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_18
        '
        Me._lvwBankDetailsList_ColumnHeader_18.Text = "County"
        Me._lvwBankDetailsList_ColumnHeader_18.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_19
        '
        Me._lvwBankDetailsList_ColumnHeader_19.Text = "Post Code"
        Me._lvwBankDetailsList_ColumnHeader_19.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_20
        '
        Me._lvwBankDetailsList_ColumnHeader_20.Text = "Country"
        Me._lvwBankDetailsList_ColumnHeader_20.Width = 97
        '
        '_lvwBankDetailsList_ColumnHeader_21
        '
        Me._lvwBankDetailsList_ColumnHeader_21.Text = "Is Default"
        Me._lvwBankDetailsList_ColumnHeader_21.Width = 50
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "add")
        Me.ImageList1.Images.SetKeyName(1, "history")
        Me.ImageList1.Images.SetKeyName(2, "edited")
        Me.ImageList1.Images.SetKeyName(3, "delete")
        Me.ImageList1.Images.SetKeyName(4, "saved")
        Me.ImageList1.Images.SetKeyName(5, "Inactive")
        '
        '_cmdNext_0
        '
        Me._cmdNext_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdNext_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdNext_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdNext_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdNext_0.Location = New System.Drawing.Point(436, 314)
        Me._cmdNext_0.Name = "_cmdNext_0"
        Me._cmdNext_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdNext_0.Size = New System.Drawing.Size(29, 19)
        Me._cmdNext_0.TabIndex = 9
        Me._cmdNext_0.Text = ">>"
        Me._cmdNext_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdNext_0.UseVisualStyleBackColor = False
        Me._cmdNext_0.Visible = False
        '
        '_tabPartyBank_TabPage1
        '
        Me._tabPartyBank_TabPage1.Controls.Add(Me.lvwBankDetailsHistory)
        Me._tabPartyBank_TabPage1.Controls.Add(Me._cmdPrevious_0)
        Me._tabPartyBank_TabPage1.Location = New System.Drawing.Point(4, 22)
        Me._tabPartyBank_TabPage1.Name = "_tabPartyBank_TabPage1"
        Me._tabPartyBank_TabPage1.Size = New System.Drawing.Size(678, 340)
        Me._tabPartyBank_TabPage1.TabIndex = 1
        Me._tabPartyBank_TabPage1.Text = "History"
        '
        'lvwBankDetailsHistory
        '
        Me.lvwBankDetailsHistory.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBankDetailsHistory.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwBankDetailsHistory_ColumnHeader_1, Me._lvwBankDetailsHistory_ColumnHeader_2, Me._lvwBankDetailsHistory_ColumnHeader_3, Me._lvwBankDetailsHistory_ColumnHeader_4, Me._lvwBankDetailsHistory_ColumnHeader_5, Me._lvwBankDetailsHistory_ColumnHeader_6, Me._lvwBankDetailsHistory_ColumnHeader_7, Me._lvwBankDetailsHistory_ColumnHeader_8, Me._lvwBankDetailsHistory_ColumnHeader_9, Me._lvwBankDetailsHistory_ColumnHeader_10, Me._lvwBankDetailsHistory_ColumnHeader_11, Me._lvwBankDetailsHistory_ColumnHeader_12})
        Me.lvwBankDetailsHistory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBankDetailsHistory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBankDetailsHistory.FullRowSelect = True
        Me.lvwBankDetailsHistory.LargeImageList = Me.ImageList1
        Me.lvwBankDetailsHistory.Location = New System.Drawing.Point(6, 8)
        Me.lvwBankDetailsHistory.Name = "lvwBankDetailsHistory"
        Me.lvwBankDetailsHistory.Size = New System.Drawing.Size(669, 303)
        Me.lvwBankDetailsHistory.SmallImageList = Me.ImageList1
        Me.lvwBankDetailsHistory.TabIndex = 6
        Me.lvwBankDetailsHistory.UseCompatibleStateImageBehavior = False
        Me.lvwBankDetailsHistory.View = System.Windows.Forms.View.Details
        '
        '_lvwBankDetailsHistory_ColumnHeader_1
        '
        Me._lvwBankDetailsHistory_ColumnHeader_1.Text = "Action Code"
        Me._lvwBankDetailsHistory_ColumnHeader_1.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_2
        '
        Me._lvwBankDetailsHistory_ColumnHeader_2.Text = "Date"
        Me._lvwBankDetailsHistory_ColumnHeader_2.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_3
        '
        Me._lvwBankDetailsHistory_ColumnHeader_3.Text = "Bank Name"
        Me._lvwBankDetailsHistory_ColumnHeader_3.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_4
        '
        Me._lvwBankDetailsHistory_ColumnHeader_4.Text = "Branch"
        Me._lvwBankDetailsHistory_ColumnHeader_4.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_5
        '
        Me._lvwBankDetailsHistory_ColumnHeader_5.Text = "Account Name"
        Me._lvwBankDetailsHistory_ColumnHeader_5.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_6
        '
        Me._lvwBankDetailsHistory_ColumnHeader_6.Text = "Sort Code"
        Me._lvwBankDetailsHistory_ColumnHeader_6.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_7
        '
        Me._lvwBankDetailsHistory_ColumnHeader_7.Text = "Account Number"
        Me._lvwBankDetailsHistory_ColumnHeader_7.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_8
        '
        Me._lvwBankDetailsHistory_ColumnHeader_8.Text = "BIC"
        Me._lvwBankDetailsHistory_ColumnHeader_8.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_9
        '
        Me._lvwBankDetailsHistory_ColumnHeader_9.Text = "IBAN"
        Me._lvwBankDetailsHistory_ColumnHeader_9.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_10
        '
        Me._lvwBankDetailsHistory_ColumnHeader_10.Text = "User"
        Me._lvwBankDetailsHistory_ColumnHeader_10.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_11
        '
        Me._lvwBankDetailsHistory_ColumnHeader_11.Text = "No & Street Name"
        Me._lvwBankDetailsHistory_ColumnHeader_11.Width = 97
        '
        '_lvwBankDetailsHistory_ColumnHeader_12
        '
        Me._lvwBankDetailsHistory_ColumnHeader_12.Text = "Postcode"
        Me._lvwBankDetailsHistory_ColumnHeader_12.Width = 97
        '
        '_cmdPrevious_0
        '
        Me._cmdPrevious_0.BackColor = System.Drawing.SystemColors.Control
        Me._cmdPrevious_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._cmdPrevious_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cmdPrevious_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me._cmdPrevious_0.Location = New System.Drawing.Point(5400, 314)
        Me._cmdPrevious_0.Name = "_cmdPrevious_0"
        Me._cmdPrevious_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cmdPrevious_0.Size = New System.Drawing.Size(29, 19)
        Me._cmdPrevious_0.TabIndex = 10
        Me._cmdPrevious_0.Text = "<<"
        Me._cmdPrevious_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me._cmdPrevious_0.UseVisualStyleBackColor = False
        Me._cmdPrevious_0.Visible = False
        '
        'uctPartyBankControl
        '
        Me.Controls.Add(Me.tabPartyBank)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "uctPartyBankControl"
        Me.Size = New System.Drawing.Size(691, 369)
        Me.tabPartyBank.ResumeLayout(False)
        Me._tabPartyBank_TabPage0.ResumeLayout(False)
        Me.fraBankDetails.ResumeLayout(False)
        Me._tabPartyBank_TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub InitializecmdPrevious()
        Me.cmdPrevious(0) = _cmdPrevious_0
    End Sub
    Sub InitializecmdNext()
        Me.cmdNext(0) = _cmdNext_0
    End Sub

    Friend WithEvents _lvwBankDetailsList_ColumnHeader_21 As ColumnHeader
#End Region
#Region "Upgrade Support"
    <System.Runtime.InteropServices.ProgId("RefreshBankDetailsEventArgs_NET.RefreshBankDetailsEventArgs")> _
    Public NotInheritable Class RefreshBankDetailsEventArgs
        Inherits System.EventArgs
        Public vBankDetails As Object
        Public Sub New(ByRef vBankDetails As Object)
            MyBase.New()
            Me.vBankDetails = vBankDetails
        End Sub
    End Class
#End Region
End Class
