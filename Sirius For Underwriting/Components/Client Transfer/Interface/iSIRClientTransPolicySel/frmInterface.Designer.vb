<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
    Public WithEvents txtClientFrom As System.Windows.Forms.TextBox
    Public WithEvents txtCLientTo As System.Windows.Forms.TextBox
    Public WithEvents lblFromClient As System.Windows.Forms.Label
    Public WithEvents lblClientTo As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOk As System.Windows.Forms.Button
    Private WithEvents _lvwPolicies_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicies_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicies_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicies_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicies_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicies_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicies_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicies_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicies_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
    Private WithEvents _lvwPolicies_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
    Public WithEvents lvwPolicies As System.Windows.Forms.ListView
    Public WithEvents fraPolicies As System.Windows.Forms.GroupBox
    Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.txtClientFrom = New System.Windows.Forms.TextBox()
        Me.txtCLientTo = New System.Windows.Forms.TextBox()
        Me.lblFromClient = New System.Windows.Forms.Label()
        Me.lblClientTo = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.fraPolicies = New System.Windows.Forms.GroupBox()
        Me.lvwPolicies = New System.Windows.Forms.ListView()
        Me._lvwPolicies_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me._lvwPolicies_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Frame1.SuspendLayout()
        Me.fraPolicies.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        ' 
        ' Frame1
        ' 
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtClientFrom)
        Me.Frame1.Controls.Add(Me.txtCLientTo)
        Me.Frame1.Controls.Add(Me.lblFromClient)
        Me.Frame1.Controls.Add(Me.lblClientTo)
        Me.Frame1.Enabled = True
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(2, -2)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(669, 49)
        Me.Frame1.TabIndex = 4
        Me.Frame1.Visible = True
        ' 
        ' txtClientFrom
        ' 
        Me.txtClientFrom.AcceptsReturn = True
        Me.txtClientFrom.BackColor = System.Drawing.SystemColors.Menu
        Me.txtClientFrom.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClientFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClientFrom.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClientFrom.Location = New System.Drawing.Point(100, 16)
        Me.txtClientFrom.MaxLength = 0
        Me.txtClientFrom.Name = "txtClientFrom"
        Me.txtClientFrom.ReadOnly = True
        Me.txtClientFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClientFrom.Size = New System.Drawing.Size(157, 21)
        Me.txtClientFrom.TabIndex = 6
        ' 
        'txtCLientTo
        ' 
        Me.txtCLientTo.AcceptsReturn = True
        Me.txtCLientTo.BackColor = System.Drawing.SystemColors.Menu
        Me.txtCLientTo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCLientTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCLientTo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCLientTo.Location = New System.Drawing.Point(354, 16)
        Me.txtCLientTo.MaxLength = 0
        Me.txtCLientTo.Name = "txtCLientTo"
        Me.txtCLientTo.ReadOnly = True
        Me.txtCLientTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCLientTo.Size = New System.Drawing.Size(165, 21)
        Me.txtCLientTo.TabIndex = 5
        ' 
        'lblFromClient
        ' 
        Me.lblFromClient.BackColor = System.Drawing.SystemColors.Control
        Me.lblFromClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFromClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFromClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFromClient.Location = New System.Drawing.Point(12, 18)
        Me.lblFromClient.Name = "lblFromClient"
        Me.lblFromClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFromClient.Size = New System.Drawing.Size(77, 21)
        Me.lblFromClient.TabIndex = 8
        Me.lblFromClient.Text = "Client From:"
        ' 
        'lblClientTo
        ' 
        Me.lblClientTo.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientTo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientTo.Location = New System.Drawing.Point(278, 18)
        Me.lblClientTo.Name = "lblClientTo"
        Me.lblClientTo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientTo.Size = New System.Drawing.Size(77, 19)
        Me.lblClientTo.TabIndex = 7
        Me.lblClientTo.Text = "Client To:"
        ' 
        'cmdCancel
        ' 
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(594, 486)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 23)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        ' 
        'cmdOk
        ' 
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(516, 486)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(73, 23)
        Me.cmdOk.TabIndex = 2
        Me.cmdOk.Text = "Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        ' 
        'fraPolicies
        ' 
        Me.fraPolicies.BackColor = System.Drawing.SystemColors.Control
        Me.fraPolicies.Controls.Add(Me.lvwPolicies)
        Me.fraPolicies.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPolicies.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPolicies.Location = New System.Drawing.Point(2, 44)
        Me.fraPolicies.Name = "fraPolicies"
        Me.fraPolicies.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPolicies.Size = New System.Drawing.Size(669, 437)
        Me.fraPolicies.TabIndex = 0
        Me.fraPolicies.TabStop = False
        Me.fraPolicies.Text = "Policies"
        ' 
        'lvwPolicies
        ' 
        Me.lvwPolicies.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicies.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPolicies, "")
        Me.lvwPolicies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicies_ColumnHeader_1, Me._lvwPolicies_ColumnHeader_2, Me._lvwPolicies_ColumnHeader_3, Me._lvwPolicies_ColumnHeader_4, Me._lvwPolicies_ColumnHeader_5, Me._lvwPolicies_ColumnHeader_6, Me._lvwPolicies_ColumnHeader_7, Me._lvwPolicies_ColumnHeader_8, Me._lvwPolicies_ColumnHeader_9, Me._lvwPolicies_ColumnHeader_10, Me._lvwPolicies_ColumnHeader_11})
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPolicies, False)
        Me.lvwPolicies.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPolicies.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicies.FullRowSelect = True
        Me.lvwPolicies.GridLines = True
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPolicies, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPolicies, "")
        Me.lvwPolicies.Location = New System.Drawing.Point(6, 14)
        Me.lvwPolicies.Name = "lvwPolicies"
        Me.lvwPolicies.Size = New System.Drawing.Size(657, 417)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPolicies, "")
        Me.listViewHelper1.SetSorted(Me.lvwPolicies, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPolicies, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPolicies, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPolicies.TabIndex = 1
        Me.lvwPolicies.UseCompatibleStateImageBehavior = False
        Me.lvwPolicies.View = System.Windows.Forms.View.Details
        ' 
        '_lvwPolicies_ColumnHeader_1
        ' 
        Me._lvwPolicies_ColumnHeader_1.Text = "Policy Number"
        Me._lvwPolicies_ColumnHeader_1.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_2
        ' 
        Me._lvwPolicies_ColumnHeader_2.Text = "Policy Type"
        Me._lvwPolicies_ColumnHeader_2.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_3
        ' 
        Me._lvwPolicies_ColumnHeader_3.Text = "Product Name"
        Me._lvwPolicies_ColumnHeader_3.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_4
        ' 
        Me._lvwPolicies_ColumnHeader_4.Text = "Regarding"
        Me._lvwPolicies_ColumnHeader_4.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_5
        ' 
        Me._lvwPolicies_ColumnHeader_5.Text = "Renewal Date"
        Me._lvwPolicies_ColumnHeader_5.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_6
        ' 
        Me._lvwPolicies_ColumnHeader_6.Text = "Agent"
        Me._lvwPolicies_ColumnHeader_6.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_7
        ' 
        Me._lvwPolicies_ColumnHeader_7.Text = "Premium"
        Me._lvwPolicies_ColumnHeader_7.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_8
        ' 
        Me._lvwPolicies_ColumnHeader_8.Text = "Policy Status"
        Me._lvwPolicies_ColumnHeader_8.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_9
        ' 
        Me._lvwPolicies_ColumnHeader_9.Text = "Risk Type Description"
        Me._lvwPolicies_ColumnHeader_9.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_10
        ' 
        Me._lvwPolicies_ColumnHeader_10.Text = "Event Description"
        Me._lvwPolicies_ColumnHeader_10.Width = 94
        ' 
        '_lvwPolicies_ColumnHeader_11
        '
        Me._lvwPolicies_ColumnHeader_11.Text = "ActivePlansCount"
        Me._lvwPolicies_ColumnHeader_11.Width = 94
        '
        'frmInterface
        ' 
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(676, 514)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.fraPolicies)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Client Portfolio Transfer - Select Policies"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.fraPolicies.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _lvwPolicies_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
#End Region
End Class