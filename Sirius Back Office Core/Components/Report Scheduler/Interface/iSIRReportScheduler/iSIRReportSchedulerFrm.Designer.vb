<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportSchedulerDetail
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
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents chkOutputAsCSV As System.Windows.Forms.CheckBox
    Public WithEvents chkArchieveAsPDF As System.Windows.Forms.CheckBox
    Public WithEvents chkOutputAsPDF As System.Windows.Forms.CheckBox
    Public WithEvents txtReportName As System.Windows.Forms.TextBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents lvwReportSchedulerDetail As System.Windows.Forms.ListView
    Public WithEvents cboFrequency As PMLookupControl.cboPMLookup
    Public WithEvents lblFrequency As System.Windows.Forms.Label
    Public WithEvents lblReportName As System.Windows.Forms.Label
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
     Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.chkOutputAsCSV = New System.Windows.Forms.CheckBox
        Me.chkArchieveAsPDF = New System.Windows.Forms.CheckBox
        Me.chkOutputAsPDF = New System.Windows.Forms.CheckBox
        Me.txtReportName = New System.Windows.Forms.TextBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.lvwReportSchedulerDetail = New System.Windows.Forms.ListView
        Me.cboFrequency = New PMLookupControl.cboPMLookup
        Me.lblFrequency = New System.Windows.Forms.Label
        Me.lblReportName = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.lblSeprateBy = New System.Windows.Forms.Label
        Me.cboSeprateBy = New System.Windows.Forms.ComboBox
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(497, 415)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 7
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'chkOutputAsCSV
        '
        Me.chkOutputAsCSV.BackColor = System.Drawing.SystemColors.Control
        Me.chkOutputAsCSV.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOutputAsCSV.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.chkOutputAsCSV.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOutputAsCSV.Location = New System.Drawing.Point(376, 27)
        Me.chkOutputAsCSV.Name = "chkOutputAsCSV"
        Me.chkOutputAsCSV.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOutputAsCSV.Size = New System.Drawing.Size(137, 26)
        Me.chkOutputAsCSV.TabIndex = 5
        Me.chkOutputAsCSV.Text = "Output as Excel"
        Me.chkOutputAsCSV.UseVisualStyleBackColor = False
        '
        'chkArchieveAsPDF
        '
        Me.chkArchieveAsPDF.BackColor = System.Drawing.SystemColors.Control
        Me.chkArchieveAsPDF.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkArchieveAsPDF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkArchieveAsPDF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkArchieveAsPDF.Location = New System.Drawing.Point(376, 51)
        Me.chkArchieveAsPDF.Name = "chkArchieveAsPDF"
        Me.chkArchieveAsPDF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkArchieveAsPDF.Size = New System.Drawing.Size(137, 21)
        Me.chkArchieveAsPDF.TabIndex = 4
        Me.chkArchieveAsPDF.Text = "Archive Report"
        Me.chkArchieveAsPDF.UseVisualStyleBackColor = False
        '
        'chkOutputAsPDF
        '
        Me.chkOutputAsPDF.BackColor = System.Drawing.SystemColors.Control
        Me.chkOutputAsPDF.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkOutputAsPDF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOutputAsPDF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOutputAsPDF.Location = New System.Drawing.Point(376, 8)
        Me.chkOutputAsPDF.Name = "chkOutputAsPDF"
        Me.chkOutputAsPDF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkOutputAsPDF.Size = New System.Drawing.Size(137, 20)
        Me.chkOutputAsPDF.TabIndex = 3
        Me.chkOutputAsPDF.Text = "Output as PDF"
        Me.chkOutputAsPDF.UseVisualStyleBackColor = False
        '
        'txtReportName
        '
        Me.txtReportName.AcceptsReturn = True
        Me.txtReportName.BackColor = System.Drawing.SystemColors.Window
        Me.txtReportName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReportName.Enabled = False
        Me.txtReportName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReportName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReportName.Location = New System.Drawing.Point(128, 8)
        Me.txtReportName.MaxLength = 0
        Me.txtReportName.Name = "txtReportName"
        Me.txtReportName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReportName.Size = New System.Drawing.Size(225, 20)
        Me.txtReportName.TabIndex = 1
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(412, 417)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 6
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lvwReportSchedulerDetail
        '
        Me.lvwReportSchedulerDetail.AllowColumnReorder = True
        Me.lvwReportSchedulerDetail.BackColor = System.Drawing.SystemColors.Window
        Me.lvwReportSchedulerDetail.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwReportSchedulerDetail.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwReportSchedulerDetail, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwReportSchedulerDetail, True)
        Me.lvwReportSchedulerDetail.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwReportSchedulerDetail.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwReportSchedulerDetail.FullRowSelect = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwReportSchedulerDetail, "lvwReportSchedulerDetail_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwReportSchedulerDetail, "")
        Me.lvwReportSchedulerDetail.Location = New System.Drawing.Point(9, 86)
        Me.lvwReportSchedulerDetail.Name = "lvwReportSchedulerDetail"
        Me.lvwReportSchedulerDetail.Size = New System.Drawing.Size(565, 324)
        Me.listViewHelper1.SetSmallIcons(Me.lvwReportSchedulerDetail, "")
        Me.listViewHelper1.SetSorted(Me.lvwReportSchedulerDetail, False)
        Me.listViewHelper1.SetSortKey(Me.lvwReportSchedulerDetail, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwReportSchedulerDetail, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwReportSchedulerDetail.TabIndex = 0
        Me.lvwReportSchedulerDetail.UseCompatibleStateImageBehavior = False
        Me.lvwReportSchedulerDetail.View = System.Windows.Forms.View.Details
        '
        'cboFrequency
        '
        Me.cboFrequency.DefaultItemId = 0
        Me.cboFrequency.FirstItem = ""
        Me.cboFrequency.ItemId = 0
        Me.cboFrequency.ListIndex = -1
        Me.cboFrequency.Location = New System.Drawing.Point(128, 32)
        Me.cboFrequency.Name = "cboFrequency"
        Me.cboFrequency.PMLookupProductFamily = 1
        Me.cboFrequency.SingleItemId = 0
        Me.cboFrequency.Size = New System.Drawing.Size(225, 21)
        Me.cboFrequency.Sorted = True
        Me.cboFrequency.TabIndex = 2
        Me.cboFrequency.TableName = "Scheduled_Report_Frequency"
        Me.cboFrequency.ToolTipText = ""
        Me.cboFrequency.WhereClause = ""
        '
        'lblFrequency
        '
        Me.lblFrequency.AutoSize = True
        Me.lblFrequency.BackColor = System.Drawing.SystemColors.Control
        Me.lblFrequency.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFrequency.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrequency.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFrequency.Location = New System.Drawing.Point(16, 32)
        Me.lblFrequency.Name = "lblFrequency"
        Me.lblFrequency.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFrequency.Size = New System.Drawing.Size(60, 13)
        Me.lblFrequency.TabIndex = 9
        Me.lblFrequency.Text = "Frequency:"
        '
        'lblReportName
        '
        Me.lblReportName.AutoSize = True
        Me.lblReportName.BackColor = System.Drawing.SystemColors.Control
        Me.lblReportName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReportName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReportName.Location = New System.Drawing.Point(16, 8)
        Me.lblReportName.Name = "lblReportName"
        Me.lblReportName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReportName.Size = New System.Drawing.Size(73, 13)
        Me.lblReportName.TabIndex = 8
        Me.lblReportName.Text = "Report Name:"
        '
        'lblSeprateBy
        '
        Me.lblSeprateBy.AutoSize = True
        Me.lblSeprateBy.BackColor = System.Drawing.SystemColors.Control
        Me.lblSeprateBy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSeprateBy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeprateBy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSeprateBy.Location = New System.Drawing.Point(16, 59)
        Me.lblSeprateBy.Name = "lblSeprateBy"
        Me.lblSeprateBy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSeprateBy.Size = New System.Drawing.Size(68, 13)
        Me.lblSeprateBy.TabIndex = 11
        Me.lblSeprateBy.Text = "Separate By:"
        Me.lblSeprateBy.Visible = False
        '
        'cboSeprateBy
        '
        Me.cboSeprateBy.FormattingEnabled = True
        Me.cboSeprateBy.Location = New System.Drawing.Point(128, 58)
        Me.cboSeprateBy.Name = "cboSeprateBy"
        Me.cboSeprateBy.Size = New System.Drawing.Size(224, 21)
        Me.cboSeprateBy.TabIndex = 12
        Me.cboSeprateBy.Visible = False
        '
        'frmReportSchedulerDetail
        '
        Me.AcceptButton = Me.cmdCancel
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(582, 449)
        Me.Controls.Add(Me.cboSeprateBy)
        Me.Controls.Add(Me.lblSeprateBy)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.chkOutputAsCSV)
        Me.Controls.Add(Me.chkArchieveAsPDF)
        Me.Controls.Add(Me.chkOutputAsPDF)
        Me.Controls.Add(Me.txtReportName)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lvwReportSchedulerDetail)
        Me.Controls.Add(Me.cboFrequency)
        Me.Controls.Add(Me.lblFrequency)
        Me.Controls.Add(Me.lblReportName)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReportSchedulerDetail"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Report Scheduler Detail"
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents lblSeprateBy As System.Windows.Forms.Label
    Friend WithEvents cboSeprateBy As System.Windows.Forms.ComboBox
#End Region
End Class