<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmInterface
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        lvwCommLevel_InitializeColumnKeys()
        Form_Initialize_Renamed()
    End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lvwCommissionLevel = New System.Windows.Forms.ListView()
        Me._lvwCommissionLevel_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommissionLevel_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommissionLevel_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommissionLevel_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwCommissionLevel_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog()
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog()
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog()
        Me.tabDetailTab = New System.Windows.Forms.TabControl()
        Me._tabDetailTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.lblCommissionLevel = New System.Windows.Forms.Label()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.cboCommissionLevel = New PMLookupControl.cboPMLookup()
        Me.cmdDetailOK = New System.Windows.Forms.Button()
        Me.cmdDetailCancel = New System.Windows.Forms.Button()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me.chkIsDeleted = New System.Windows.Forms.CheckBox()
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.tabDetailTab.SuspendLayout()
        Me._tabDetailTab_TabPage0.SuspendLayout()
        Me.tabMainTab.SuspendLayout()
        Me.SuspendLayout()
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwCommissionLevel)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdEdit)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(492, 251)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - Commission"
        '
        'lvwCommissionLevel
        '
        Me.lvwCommissionLevel.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCommissionLevel.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwCommissionLevel.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwCommissionLevel_ColumnHeader_1, Me._lvwCommissionLevel_ColumnHeader_2, Me._lvwCommissionLevel_ColumnHeader_3, Me._lvwCommissionLevel_ColumnHeader_4, Me._lvwCommissionLevel_ColumnHeader_5})
        Me.lvwCommissionLevel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCommissionLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCommissionLevel.FullRowSelect = True
        Me.lvwCommissionLevel.HideSelection = False
        Me.lvwCommissionLevel.LargeImageList = Me.ImageList1
        Me.lvwCommissionLevel.Location = New System.Drawing.Point(8, 11)
        Me.lvwCommissionLevel.Name = "lvwCommissionLevel"
        Me.lvwCommissionLevel.Size = New System.Drawing.Size(480, 180)
        Me.lvwCommissionLevel.SmallImageList = Me.ImageList1
        Me.lvwCommissionLevel.TabIndex = 19
        Me.lvwCommissionLevel.UseCompatibleStateImageBehavior = False
        Me.lvwCommissionLevel.View = System.Windows.Forms.View.Details
        '
        '_lvwCommissionLevel_ColumnHeader_1
        '
        Me._lvwCommissionLevel_ColumnHeader_1.Text = "Commission Level"
        Me._lvwCommissionLevel_ColumnHeader_1.Width = 250
        '
        '_lvwCommissionLevel_ColumnHeader_2
        '
        Me._lvwCommissionLevel_ColumnHeader_2.Text = "Description"
        Me._lvwCommissionLevel_ColumnHeader_2.Width = 201
        '
        '_lvwCommissionLevel_ColumnHeader_3
        '
        Me._lvwCommissionLevel_ColumnHeader_3.Text = "Effective Date"
        Me._lvwCommissionLevel_ColumnHeader_3.Width = 481
        '
        '_lvwCommissionLevel_ColumnHeader_4
        '
        Me._lvwCommissionLevel_ColumnHeader_4.Text = "Is Deleted"
        Me._lvwCommissionLevel_ColumnHeader_4.Width = 80
        '
        '_lvwCommissionLevel_ColumnHeader_5
        '
        Me._lvwCommissionLevel_ColumnHeader_5.Text = "Agent_Commission_Level_Id"
        Me._lvwCommissionLevel_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "FindImage")
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(398, 197)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(73, 22)
        Me.cmdDelete.TabIndex = 1
        Me.cmdDelete.Text = "Dele&te"
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(237, 197)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 0
        Me.cmdEdit.Text = "&Edit..."
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        Me.cmdEdit.Visible = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(316, 197)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
        Me.cmdAdd.TabIndex = 20
        Me.cmdAdd.Text = "&New"
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(8, 296)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 15
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        Me.cmdHelp.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(435, 296)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 14
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'tabDetailTab
        '
        Me.tabDetailTab.Controls.Add(Me._tabDetailTab_TabPage0)
        Me.tabDetailTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabDetailTab.ItemSize = New System.Drawing.Size(600, 19)
        Me.tabDetailTab.Location = New System.Drawing.Point(94, 12)
        Me.tabDetailTab.Multiline = True
        Me.tabDetailTab.Name = "tabDetailTab"
        Me.tabDetailTab.SelectedIndex = 0
        Me.tabDetailTab.Size = New System.Drawing.Size(400, 191)
        Me.tabDetailTab.TabIndex = 16
        Me.tabDetailTab.Visible = False
        '
        '_tabDetailTab_TabPage0
        '
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblCommissionLevel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cboCommissionLevel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailOK)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.cmdDetailCancel)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabDetailTab_TabPage0.Controls.Add(Me.chkIsDeleted)
        Me._tabDetailTab_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._tabDetailTab_TabPage0.Name = "_tabDetailTab_TabPage0"
        Me._tabDetailTab_TabPage0.Size = New System.Drawing.Size(392, 164)
        Me._tabDetailTab_TabPage0.TabIndex = 0
        Me._tabDetailTab_TabPage0.Text = "&2 - Commission Level"
        '
        'lblCommissionLevel
        '
        Me.lblCommissionLevel.BackColor = System.Drawing.SystemColors.Control
        Me.lblCommissionLevel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCommissionLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCommissionLevel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommissionLevel.Location = New System.Drawing.Point(13, 18)
        Me.lblCommissionLevel.Name = "lblCommissionLevel"
        Me.lblCommissionLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCommissionLevel.Size = New System.Drawing.Size(104, 17)
        Me.lblCommissionLevel.TabIndex = 15
        Me.lblCommissionLevel.Text = "Commission Level:"
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(13, 42)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(104, 17)
        Me.lblEffectiveDate.TabIndex = 17
        Me.lblEffectiveDate.Text = "Effective Date:"
        '
        'cboCommissionLevel
        '
        Me.cboCommissionLevel.DefaultItemId = 0
        Me.cboCommissionLevel.FirstItem = "(none)"
        Me.cboCommissionLevel.ItemId = 0
        Me.cboCommissionLevel.ListIndex = -1
        Me.cboCommissionLevel.Location = New System.Drawing.Point(123, 14)
        Me.cboCommissionLevel.Name = "cboCommissionLevel"
        Me.cboCommissionLevel.PMLookupProductFamily = 2
        Me.cboCommissionLevel.SingleItemId = 0
        Me.cboCommissionLevel.Size = New System.Drawing.Size(240, 21)
        Me.cboCommissionLevel.SortColumnName = ""
        Me.cboCommissionLevel.Sorted = True
        Me.cboCommissionLevel.TabIndex = 25
        Me.cboCommissionLevel.TableName = "Commission_Level"
        Me.cboCommissionLevel.ToolTipText = ""
        Me.cboCommissionLevel.WhereClause = ""
        '
        'cmdDetailOK
        '
        Me.cmdDetailOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailOK.Location = New System.Drawing.Point(224, 105)
        Me.cmdDetailOK.Name = "cmdDetailOK"
        Me.cmdDetailOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailOK.TabIndex = 6
        Me.cmdDetailOK.Text = "OK"
        Me.cmdDetailOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailOK.UseVisualStyleBackColor = False
        '
        'cmdDetailCancel
        '
        Me.cmdDetailCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDetailCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDetailCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDetailCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDetailCancel.Location = New System.Drawing.Point(303, 105)
        Me.cmdDetailCancel.Name = "cmdDetailCancel"
        Me.cmdDetailCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDetailCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdDetailCancel.TabIndex = 7
        Me.cmdDetailCancel.Text = "Cancel"
        Me.cmdDetailCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDetailCancel.UseVisualStyleBackColor = False
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(123, 42)
        Me.txtEffectiveDate.MaxLength = 0
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(160, 20)
        Me.txtEffectiveDate.TabIndex = 4
        '
        'chkIsDeleted
        '
        Me.chkIsDeleted.BackColor = System.Drawing.SystemColors.Control
        Me.chkIsDeleted.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkIsDeleted.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIsDeleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIsDeleted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIsDeleted.Location = New System.Drawing.Point(16, 68)
        Me.chkIsDeleted.Name = "chkIsDeleted"
        Me.chkIsDeleted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIsDeleted.Size = New System.Drawing.Size(158, 21)
        Me.chkIsDeleted.TabIndex = 3
        Me.chkIsDeleted.Text = "Is deleted:"
        Me.chkIsDeleted.UseVisualStyleBackColor = False
        Me.chkIsDeleted.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(356, 296)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 13
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(600, 19)
        Me.tabMainTab.Location = New System.Drawing.Point(12, 12)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(500, 278)
        Me.tabMainTab.TabIndex = 17
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(524, 328)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabDetailTab)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Commission Level"
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.tabDetailTab.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.ResumeLayout(False)
        Me._tabDetailTab_TabPage0.PerformLayout()
        Me.tabMainTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Sub lvwCommLevel_InitializeColumnKeys()
        Me._lvwCommissionLevel_ColumnHeader_1.Name = ""
        Me._lvwCommissionLevel_ColumnHeader_2.Name = ""
        Me._lvwCommissionLevel_ColumnHeader_3.Name = ""
    End Sub

    Private WithEvents _tabMainTab_TabPage0 As TabPage
    Public WithEvents ImageList1 As ImageList
    Public WithEvents cmdDelete As Button
    Public WithEvents cmdEdit As Button
    Public WithEvents cmdAdd As Button
    Public WithEvents cmdHelp As Button
    Public WithEvents cmdCancel As Button
    Public WithEvents dlgHelpOpen As OpenFileDialog
    Public WithEvents dlgHelpSave As SaveFileDialog
    Public WithEvents dlgHelpFont As FontDialog
    Public WithEvents tabDetailTab As TabControl
    Private WithEvents _tabDetailTab_TabPage0 As TabPage
    Public WithEvents lblCommissionLevel As Label
    Public WithEvents lblEffectiveDate As Label
    Public WithEvents cboCommissionLevel As PMLookupControl.cboPMLookup
    Public WithEvents cmdDetailOK As Button
    Public WithEvents cmdDetailCancel As Button
    Public WithEvents txtEffectiveDate As TextBox
    Public WithEvents chkIsDeleted As CheckBox
    Public WithEvents dlgHelpPrint As PrintDialog
    Public WithEvents cmdOK As Button
    Public WithEvents ToolTip1 As ToolTip
    Public WithEvents tabMainTab As TabControl
    Public WithEvents dlgHelpColor As ColorDialog
    Public WithEvents lvwCommissionLevel As ListView
    Private WithEvents _lvwCommissionLevel_ColumnHeader_1 As ColumnHeader
    Private WithEvents _lvwCommissionLevel_ColumnHeader_2 As ColumnHeader
    Private WithEvents _lvwCommissionLevel_ColumnHeader_3 As ColumnHeader
    Private WithEvents _lvwCommissionLevel_ColumnHeader_4 As ColumnHeader
    Private WithEvents _lvwCommissionLevel_ColumnHeader_5 As ColumnHeader
#End Region
End Class