<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents lblReserveTypeName As System.Windows.Forms.Label
	Public WithEvents lblReserveTypeDescription As System.Windows.Forms.Label
	Public WithEvents lblIncludeIntotal As System.Windows.Forms.Label
	Public WithEvents lblMainReserve As System.Windows.Forms.Label
	Public WithEvents lvwPerilTypeReservetype As System.Windows.Forms.ListView
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmbReserveTypeName As System.Windows.Forms.ComboBox
	Public WithEvents txtReserveTypeDescription As System.Windows.Forms.TextBox
	Public WithEvents chkIncludeIntotal As System.Windows.Forms.CheckBox
	Public WithEvents chkMainReserve As System.Windows.Forms.CheckBox
	Public WithEvents cmdModify As System.Windows.Forms.Button
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	Dim Private tabMainTabPreviousTab As Integer
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.lblReserveTypeName = New System.Windows.Forms.Label
        Me.lblReserveTypeDescription = New System.Windows.Forms.Label
        Me.lblIncludeIntotal = New System.Windows.Forms.Label
        Me.lblMainReserve = New System.Windows.Forms.Label
        Me.lvwPerilTypeReservetype = New System.Windows.Forms.ListView
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmbReserveTypeName = New System.Windows.Forms.ComboBox
        Me.txtReserveTypeDescription = New System.Windows.Forms.TextBox
        Me.chkIncludeIntotal = New System.Windows.Forms.CheckBox
        Me.chkMainReserve = New System.Windows.Forms.CheckBox
        Me.cmdModify = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(155, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 8)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(473, 355)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblReserveTypeName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblReserveTypeDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblIncludeIntotal)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblMainReserve)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lvwPerilTypeReservetype)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAdd)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdDelete)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmbReserveTypeName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtReserveTypeDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkIncludeIntotal)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkMainReserve)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdModify)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(465, 329)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "Tab 0"
        '
        'lblReserveTypeName
        '
        Me.lblReserveTypeName.BackColor = System.Drawing.SystemColors.Control
        Me.lblReserveTypeName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReserveTypeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReserveTypeName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReserveTypeName.Location = New System.Drawing.Point(32, 19)
        Me.lblReserveTypeName.Name = "lblReserveTypeName"
        Me.lblReserveTypeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReserveTypeName.Size = New System.Drawing.Size(67, 16)
        Me.lblReserveTypeName.TabIndex = 0
        Me.lblReserveTypeName.Text = "Label1"
        '
        'lblReserveTypeDescription
        '
        Me.lblReserveTypeDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblReserveTypeDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReserveTypeDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReserveTypeDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReserveTypeDescription.Location = New System.Drawing.Point(32, 43)
        Me.lblReserveTypeDescription.Name = "lblReserveTypeDescription"
        Me.lblReserveTypeDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReserveTypeDescription.Size = New System.Drawing.Size(53, 15)
        Me.lblReserveTypeDescription.TabIndex = 2
        Me.lblReserveTypeDescription.Text = "Label2"
        Me.lblReserveTypeDescription.Visible = False
        '
        'lblIncludeIntotal
        '
        Me.lblIncludeIntotal.BackColor = System.Drawing.SystemColors.Control
        Me.lblIncludeIntotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblIncludeIntotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIncludeIntotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIncludeIntotal.Location = New System.Drawing.Point(32, 64)
        Me.lblIncludeIntotal.Name = "lblIncludeIntotal"
        Me.lblIncludeIntotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIncludeIntotal.Size = New System.Drawing.Size(98, 18)
        Me.lblIncludeIntotal.TabIndex = 4
        Me.lblIncludeIntotal.Text = "Label3"
        '
        'lblMainReserve
        '
        Me.lblMainReserve.BackColor = System.Drawing.SystemColors.Control
        Me.lblMainReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMainReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMainReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMainReserve.Location = New System.Drawing.Point(32, 86)
        Me.lblMainReserve.Name = "lblMainReserve"
        Me.lblMainReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMainReserve.Size = New System.Drawing.Size(98, 19)
        Me.lblMainReserve.TabIndex = 6
        Me.lblMainReserve.Text = "Label4"
        '
        'lvwPerilTypeReservetype
        '
        Me.lvwPerilTypeReservetype.BackColor = System.Drawing.SystemColors.Window
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwPerilTypeReservetype, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwPerilTypeReservetype, False)
        Me.lvwPerilTypeReservetype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPerilTypeReservetype.ForeColor = System.Drawing.SystemColors.WindowText
        Me.listViewHelper1.SetItemClickMethod(Me.lvwPerilTypeReservetype, "")
        Me.listViewHelper1.SetLargeIcons(Me.lvwPerilTypeReservetype, "")
        Me.lvwPerilTypeReservetype.Location = New System.Drawing.Point(10, 108)
        Me.lvwPerilTypeReservetype.Name = "lvwPerilTypeReservetype"
        Me.lvwPerilTypeReservetype.Size = New System.Drawing.Size(453, 215)
        Me.listViewHelper1.SetSmallIcons(Me.lvwPerilTypeReservetype, "")
        Me.listViewHelper1.SetSorted(Me.lvwPerilTypeReservetype, False)
        Me.listViewHelper1.SetSortKey(Me.lvwPerilTypeReservetype, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwPerilTypeReservetype, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwPerilTypeReservetype.TabIndex = 11
        Me.lvwPerilTypeReservetype.UseCompatibleStateImageBehavior = False
        Me.lvwPerilTypeReservetype.View = System.Windows.Forms.View.Details
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.Enabled = False
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Location = New System.Drawing.Point(385, 10)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdd.Size = New System.Drawing.Size(77, 25)
        Me.cmdAdd.TabIndex = 8
        Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(385, 64)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(77, 25)
        Me.cmdDelete.TabIndex = 10
        Me.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmbReserveTypeName
        '
        Me.cmbReserveTypeName.BackColor = System.Drawing.SystemColors.Window
        Me.cmbReserveTypeName.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbReserveTypeName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReserveTypeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbReserveTypeName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbReserveTypeName.Location = New System.Drawing.Point(136, 16)
        Me.cmbReserveTypeName.Name = "cmbReserveTypeName"
        Me.cmbReserveTypeName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbReserveTypeName.Size = New System.Drawing.Size(217, 21)
        Me.cmbReserveTypeName.TabIndex = 1
        '
        'txtReserveTypeDescription
        '
        Me.txtReserveTypeDescription.AcceptsReturn = True
        Me.txtReserveTypeDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtReserveTypeDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtReserveTypeDescription.Enabled = False
        Me.txtReserveTypeDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReserveTypeDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtReserveTypeDescription.Location = New System.Drawing.Point(136, 43)
        Me.txtReserveTypeDescription.MaxLength = 0
        Me.txtReserveTypeDescription.Name = "txtReserveTypeDescription"
        Me.txtReserveTypeDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtReserveTypeDescription.Size = New System.Drawing.Size(217, 20)
        Me.txtReserveTypeDescription.TabIndex = 3
        Me.txtReserveTypeDescription.Visible = False
        '
        'chkIncludeIntotal
        '
        Me.chkIncludeIntotal.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeIntotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeIntotal.Enabled = False
        Me.chkIncludeIntotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeIntotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeIntotal.Location = New System.Drawing.Point(136, 64)
        Me.chkIncludeIntotal.Name = "chkIncludeIntotal"
        Me.chkIncludeIntotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeIntotal.Size = New System.Drawing.Size(113, 23)
        Me.chkIncludeIntotal.TabIndex = 5
        Me.chkIncludeIntotal.UseVisualStyleBackColor = False
        '
        'chkMainReserve
        '
        Me.chkMainReserve.BackColor = System.Drawing.SystemColors.Control
        Me.chkMainReserve.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMainReserve.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMainReserve.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMainReserve.Location = New System.Drawing.Point(136, 83)
        Me.chkMainReserve.Name = "chkMainReserve"
        Me.chkMainReserve.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMainReserve.Size = New System.Drawing.Size(99, 21)
        Me.chkMainReserve.TabIndex = 7
        Me.chkMainReserve.UseVisualStyleBackColor = False
        '
        'cmdModify
        '
        Me.cmdModify.BackColor = System.Drawing.SystemColors.Control
        Me.cmdModify.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdModify.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdModify.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdModify.Location = New System.Drawing.Point(385, 37)
        Me.cmdModify.Name = "cmdModify"
        Me.cmdModify.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdModify.Size = New System.Drawing.Size(77, 25)
        Me.cmdModify.TabIndex = 9
        Me.cmdModify.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdModify.UseVisualStyleBackColor = False
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(400, 363)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 3
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(320, 364)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(240, 364)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(483, 392)
        Me.Controls.Add(Me.tabMainTab)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Form1"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class