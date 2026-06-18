<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSetListColumnOrder
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
	Public WithEvents cmdDown As System.Windows.Forms.Button
	Public WithEvents cmdUp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents ListViewSetListColumnOrder As System.Windows.Forms.ListView
	Private WithEvents commandButtonHelper1 As Artinsoft.VB6.Gui.CommandButtonHelper
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSetListColumnOrder))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdDown = New System.Windows.Forms.Button
        Me.cmdUp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.ListViewSetListColumnOrder = New System.Windows.Forms.ListView
        Me.commandButtonHelper1 = New Artinsoft.VB6.Gui.CommandButtonHelper(Me.components)
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdDown
        '
        Me.cmdDown.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdDown, True)
        Me.cmdDown.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdDown, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdDown, Nothing)
        Me.cmdDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDown.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDown.Image = CType(resources.GetObject("cmdDown.Image"), System.Drawing.Image)
        Me.cmdDown.Location = New System.Drawing.Point(355, 65)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdDown, System.Drawing.Color.Silver)
        Me.cmdDown.Name = "cmdDown"
        Me.cmdDown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDown.Size = New System.Drawing.Size(30, 33)
        Me.commandButtonHelper1.SetStyle(Me.cmdDown, 1)
        Me.cmdDown.TabIndex = 4
        Me.cmdDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDown.UseVisualStyleBackColor = False
        '
        'cmdUp
        '
        Me.cmdUp.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdUp, True)
        Me.cmdUp.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdUp, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdUp, Nothing)
        Me.cmdUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUp.Image = CType(resources.GetObject("cmdUp.Image"), System.Drawing.Image)
        Me.cmdUp.Location = New System.Drawing.Point(355, 29)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdUp, System.Drawing.Color.Silver)
        Me.cmdUp.Name = "cmdUp"
        Me.cmdUp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUp.Size = New System.Drawing.Size(30, 33)
        Me.commandButtonHelper1.SetStyle(Me.cmdUp, 1)
        Me.cmdUp.TabIndex = 3
        Me.cmdUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdUp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdCancel, True)
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdCancel, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdCancel, Nothing)
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(276, 194)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdCancel, System.Drawing.Color.Silver)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdCancel, 0)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.commandButtonHelper1.SetCorrectEventsBehavior(Me.cmdOK, True)
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.commandButtonHelper1.SetDisabledPicture(Me.cmdOK, Nothing)
        Me.commandButtonHelper1.SetDownPicture(Me.cmdOK, Nothing)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(196, 194)
        Me.commandButtonHelper1.SetMaskColor(Me.cmdOK, System.Drawing.Color.Silver)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.commandButtonHelper1.SetStyle(Me.cmdOK, 0)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'ListViewSetListColumnOrder
        '
        Me.ListViewSetListColumnOrder.BackColor = System.Drawing.SystemColors.Window
        'Me.listViewHelper1.SetColumnHeaderIcons(Me.ListViewSetListColumnOrder, "")
        'Me.listViewHelper1.SetCorrectEventsBehavior(Me.ListViewSetListColumnOrder, False)
        Me.ListViewSetListColumnOrder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListViewSetListColumnOrder.ForeColor = System.Drawing.SystemColors.WindowText
        'Me.listViewHelper1.SetItemClickMethod(Me.ListViewSetListColumnOrder, "")
        'Me.listViewHelper1.SetLargeIcons(Me.ListViewSetListColumnOrder, "")
        Me.ListViewSetListColumnOrder.Location = New System.Drawing.Point(30, 29)
        Me.ListViewSetListColumnOrder.MultiSelect = False
        Me.ListViewSetListColumnOrder.Name = "ListViewSetListColumnOrder"
        Me.ListViewSetListColumnOrder.Size = New System.Drawing.Size(319, 161)
        'Me.listViewHelper1.SetSmallIcons(Me.ListViewSetListColumnOrder, "")
        ' Me.listViewHelper1.SetSorted(Me.ListViewSetListColumnOrder, True)
        'Me.ListViewSetListColumnOrder.Sorting = System.Windows.Forms.SortOrder.Ascending
        'Me.listViewHelper1.SetSortKey(Me.ListViewSetListColumnOrder, 0)
        ' Me.listViewHelper1.SetSortOrder(Me.ListViewSetListColumnOrder, System.Windows.Forms.SortOrder.Ascending)
        Me.ListViewSetListColumnOrder.TabIndex = 2
        Me.ListViewSetListColumnOrder.UseCompatibleStateImageBehavior = False
        Me.ListViewSetListColumnOrder.View = System.Windows.Forms.View.Details
        '
        'frmSetListColumnOrder
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(401, 235)
        Me.Controls.Add(Me.cmdDown)
        Me.Controls.Add(Me.cmdUp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.ListViewSetListColumnOrder)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(307, 275)
        Me.Name = "frmSetListColumnOrder"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Set List Column Order"
        CType(Me.commandButtonHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class