<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmClonedRIManual
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
        'This call is required by the Windows Form Designer.
        isInitializingComponent = True
        InitializeComponent()
        isInitializingComponent = False
		Form_Initialize_renamed()
    End Sub
    Private Sub Ctx_unresolved_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Ctx_unresolved.Opening
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        Ctx_unresolved.Items.Clear()
        'We are moving the submenus from original menu to the context menu before displaying it
        For Each item As System.Windows.Forms.ToolStripItem In (mnuPopup).DropDownItems
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            Ctx_unresolved.Items.Add(item)
        Next item
        e.Cancel = False
    End Sub
    Private Sub Ctx_unresolved_Closing(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosingEventArgs) Handles Ctx_unresolved.Closing
        Dim list As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) = New System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem)()
        'We are moving the submenus the context menu back to the original menu after displaying
        For Each item As System.Windows.Forms.ToolStripItem In Ctx_unresolved.Items
            list.Add(item)
        Next item
        For Each item As System.Windows.Forms.ToolStripItem In list
            mnuPopup.DropDownItems.Add(item)
        Next item
    End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents _lvwPolicies_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwPolicies_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwPolicies_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwPolicies_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwPolicies_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwPolicies_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwPolicies As System.Windows.Forms.ListView
	Public WithEvents fmeDeferredRI As System.Windows.Forms.GroupBox
	Public WithEvents cmdSelectAll As System.Windows.Forms.Button
	Public WithEvents cmdAccept As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdAmend As System.Windows.Forms.Button
	Public WithEvents _stbStatus_Panel1 As System.Windows.Forms.ToolStripStatusLabel
	Public WithEvents stbStatus As System.Windows.Forms.StatusStrip
    Public WithEvents Ctx_unresolved As System.Windows.Forms.ContextMenuStrip
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmClonedRIManual))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fmeDeferredRI = New System.Windows.Forms.GroupBox()
        Me.lvwPolicies = New System.Windows.Forms.ListView()
        Me._lvwPolicies_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me._lvwPolicies_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.cmdSelectAll = New System.Windows.Forms.Button()
        Me.cmdAccept = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdAmend = New System.Windows.Forms.Button()
        Me.stbStatus = New System.Windows.Forms.StatusStrip()
        Me._stbStatus_Panel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Ctx_unresolved = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuPopup = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPopupAmend = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPopupAccept = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuPopupDivider = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPopupSelectAll = New System.Windows.Forms.ToolStripMenuItem
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.fmeDeferredRI.SuspendLayout
        Me.stbStatus.SuspendLayout

        Me.SuspendLayout
        '
        'fmeDeferredRI
        '
        Me.fmeDeferredRI.BackColor = System.Drawing.SystemColors.Control
        Me.fmeDeferredRI.Controls.Add(Me.lvwPolicies)
        Me.fmeDeferredRI.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.fmeDeferredRI.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fmeDeferredRI.Location = New System.Drawing.Point(8, 6)
        Me.fmeDeferredRI.Name = "fmeDeferredRI"
        Me.fmeDeferredRI.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fmeDeferredRI.Size = New System.Drawing.Size(751, 406)
        Me.fmeDeferredRI.TabIndex = 0
        Me.fmeDeferredRI.TabStop = false
        Me.fmeDeferredRI.Text = "Policies"
        '
        'lvwPolicies
        '
        Me.lvwPolicies.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPolicies.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwPolicies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwPolicies_ColumnHeader_1, Me._lvwPolicies_ColumnHeader_2, Me._lvwPolicies_ColumnHeader_3, Me._lvwPolicies_ColumnHeader_4, Me._lvwPolicies_ColumnHeader_5, Me._lvwPolicies_ColumnHeader_6})
        Me.lvwPolicies.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lvwPolicies.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPolicies.FullRowSelect = true
        Me.lvwPolicies.Location = New System.Drawing.Point(12, 21)
        Me.lvwPolicies.Name = "lvwPolicies"
        Me.lvwPolicies.Size = New System.Drawing.Size(726, 371)
        Me.lvwPolicies.TabIndex = 1
        Me.lvwPolicies.UseCompatibleStateImageBehavior = false
        Me.lvwPolicies.View = System.Windows.Forms.View.Details
        '
        '_lvwPolicies_ColumnHeader_1
        '
        Me._lvwPolicies_ColumnHeader_1.Text = "Branch"
        Me._lvwPolicies_ColumnHeader_1.Width = 165
        '
        '_lvwPolicies_ColumnHeader_2
        '
        Me._lvwPolicies_ColumnHeader_2.Text = "Deferred RI Status"
        Me._lvwPolicies_ColumnHeader_2.Width = 294
        '
        '_lvwPolicies_ColumnHeader_3
        '
        Me._lvwPolicies_ColumnHeader_3.Text = "Policy Number"
        Me._lvwPolicies_ColumnHeader_3.Width = 236
        '
        '_lvwPolicies_ColumnHeader_4
        '
        Me._lvwPolicies_ColumnHeader_4.Text = "Client Code"
        Me._lvwPolicies_ColumnHeader_4.Width = 236
        '
        '_lvwPolicies_ColumnHeader_5
        '
        Me._lvwPolicies_ColumnHeader_5.Text = "Client Name"
        Me._lvwPolicies_ColumnHeader_5.Width = 294
        '
        '_lvwPolicies_ColumnHeader_6
        '
        Me._lvwPolicies_ColumnHeader_6.Text = "Product"
        Me._lvwPolicies_ColumnHeader_6.Width = 236
        '
        'cmdSelectAll
        '
        Me.cmdSelectAll.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelectAll.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelectAll.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdSelectAll.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelectAll.Location = New System.Drawing.Point(169, 421)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelectAll.Size = New System.Drawing.Size(73, 23)
        Me.cmdSelectAll.TabIndex = 4
        Me.cmdSelectAll.Text = "&Select All"
        Me.cmdSelectAll.UseVisualStyleBackColor = false
        '
        'cmdAccept
        '
        Me.cmdAccept.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAccept.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAccept.Enabled = false
        Me.cmdAccept.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdAccept.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAccept.Location = New System.Drawing.Point(89, 421)
        Me.cmdAccept.Name = "cmdAccept"
        Me.cmdAccept.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAccept.Size = New System.Drawing.Size(73, 23)
        Me.cmdAccept.TabIndex = 3
        Me.cmdAccept.Text = "Acc&ept"
        Me.cmdAccept.UseVisualStyleBackColor = false
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(687, 421)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 23)
        Me.cmdOK.TabIndex = 5
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = false
        '
        'cmdAmend
        '
        Me.cmdAmend.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAmend.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAmend.Enabled = false
        Me.cmdAmend.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cmdAmend.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAmend.Location = New System.Drawing.Point(9, 421)
        Me.cmdAmend.Name = "cmdAmend"
        Me.cmdAmend.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAmend.Size = New System.Drawing.Size(73, 23)
        Me.cmdAmend.TabIndex = 2
        Me.cmdAmend.Text = "&Amend"
        Me.cmdAmend.UseVisualStyleBackColor = false
        '
        'stbStatus
        '
        Me.stbStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.stbStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._stbStatus_Panel1})
        Me.stbStatus.Location = New System.Drawing.Point(0, 451)
        Me.stbStatus.Name = "stbStatus"
        Me.stbStatus.Size = New System.Drawing.Size(771, 22)
        Me.stbStatus.TabIndex = 6
        '
        '_stbStatus_Panel1
        '
        Me._stbStatus_Panel1.AutoSize = false
        Me._stbStatus_Panel1.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)  _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)  _
            Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom),System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me._stbStatus_Panel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter
        Me._stbStatus_Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me._stbStatus_Panel1.Name = "_stbStatus_Panel1"
        Me._stbStatus_Panel1.Size = New System.Drawing.Size(756, 22)
        Me._stbStatus_Panel1.Spring = true
        Me._stbStatus_Panel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        
        '
        'Ctx_unresolved
        '
        Me.Ctx_unresolved.Name = "Ctx_unresolved"
        Me.Ctx_unresolved.Size = New System.Drawing.Size(61, 4)
        '
        'mnuPopup
        '
        Me.mnuPopup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPopupAmend, Me.mnuPopupAccept, Me.mnuPopupDivider, Me.mnuPopupSelectAll})
        Me.mnuPopup.Name = "mnuPopup"
        Me.mnuPopup.Size = New System.Drawing.Size(49, 20)
        Me.mnuPopup.Text = "Popup"
        '
        'mnuPopupAmend
        '
        Me.mnuPopupAmend.Name = "mnuPopupAmend"
        Me.mnuPopupAmend.Size = New System.Drawing.Size(117, 22)
        Me.mnuPopupAmend.Text = "&Amend"
        '
        'mnuPopupAccept
        '
        Me.mnuPopupAccept.Name = "mnuPopupAccept"
        Me.mnuPopupAccept.Size = New System.Drawing.Size(117, 22)
        Me.mnuPopupAccept.Text = "Acc&ept"
        '
        'mnuPopupDivider
        '
        Me.mnuPopupDivider.Name = "mnuPopupDivider"
        Me.mnuPopupDivider.Size = New System.Drawing.Size(114, 6)
        '
        'mnuPopupSelectAll
        '
        Me.mnuPopupSelectAll.Name = "mnuPopupSelectAll"
        Me.mnuPopupSelectAll.Size = New System.Drawing.Size(117, 22)
        Me.mnuPopupSelectAll.Text = "&Select All"
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuPopup})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(771, 24)
        Me.MainMenu1.TabIndex = 7

        '
        'frmClonedRIManual
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(771, 473)
        Me.Controls.Add(Me.fmeDeferredRI)
        Me.Controls.Add(Me.cmdSelectAll)
        Me.Controls.Add(Me.cmdAccept)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdAmend)
        Me.Controls.Add(Me.stbStatus)

        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(16, 29)
        Me.MinimizeBox = false
        Me.Name = "frmClonedRIManual"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = false
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Amend RI Cloned Transfer Policies"
        Me.fmeDeferredRI.ResumeLayout(false)
        Me.stbStatus.ResumeLayout(false)
        Me.stbStatus.PerformLayout
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(false)
        Me.PerformLayout

    End Sub
    Public WithEvents mnuPopup As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopupAmend As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopupAccept As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuPopupDivider As System.Windows.Forms.ToolStripSeparator
    Public WithEvents mnuPopupSelectAll As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
#End Region 
End Class