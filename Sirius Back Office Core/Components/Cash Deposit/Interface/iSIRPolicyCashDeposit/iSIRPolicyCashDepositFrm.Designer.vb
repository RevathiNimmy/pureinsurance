<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
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
	Public WithEvents txtPartyCode As System.Windows.Forms.TextBox
	Public WithEvents txtPartyName As System.Windows.Forms.TextBox
	Public WithEvents lvwCDList As System.Windows.Forms.ListView
	Public WithEvents lblPartyCode As System.Windows.Forms.Label
	Public WithEvents lblPartyName As System.Windows.Forms.Label
	Public WithEvents frmCD As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents optAgent As System.Windows.Forms.RadioButton
	Public WithEvents optClient As System.Windows.Forms.RadioButton
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.frmCD = New System.Windows.Forms.GroupBox
        Me.txtPartyCode = New System.Windows.Forms.TextBox
        Me.txtPartyName = New System.Windows.Forms.TextBox
        Me.lvwCDList = New System.Windows.Forms.ListView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.lblPartyCode = New System.Windows.Forms.Label
        Me.lblPartyName = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.optAgent = New System.Windows.Forms.RadioButton
        Me.optClient = New System.Windows.Forms.RadioButton
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.frmCD.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'frmCD
        '
        Me.frmCD.BackColor = System.Drawing.SystemColors.Control
        Me.frmCD.Controls.Add(Me.txtPartyCode)
        Me.frmCD.Controls.Add(Me.txtPartyName)
        Me.frmCD.Controls.Add(Me.lvwCDList)
        Me.frmCD.Controls.Add(Me.lblPartyCode)
        Me.frmCD.Controls.Add(Me.lblPartyName)
        Me.frmCD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frmCD.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmCD.Location = New System.Drawing.Point(6, 36)
        Me.frmCD.Name = "frmCD"
        Me.frmCD.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmCD.Size = New System.Drawing.Size(671, 333)
        Me.frmCD.TabIndex = 4
        Me.frmCD.TabStop = False
        Me.frmCD.Text = "Cash Deposits"
        '
        'txtPartyCode
        '
        Me.txtPartyCode.AcceptsReturn = True
        Me.txtPartyCode.BackColor = System.Drawing.SystemColors.Menu
        Me.txtPartyCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyCode.Location = New System.Drawing.Point(86, 24)
        Me.txtPartyCode.MaxLength = 0
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.ReadOnly = True
        Me.txtPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyCode.Size = New System.Drawing.Size(125, 20)
        Me.txtPartyCode.TabIndex = 6
        '
        'txtPartyName
        '
        Me.txtPartyName.AcceptsReturn = True
        Me.txtPartyName.BackColor = System.Drawing.SystemColors.Menu
        Me.txtPartyName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartyName.Location = New System.Drawing.Point(312, 24)
        Me.txtPartyName.MaxLength = 0
        Me.txtPartyName.Name = "txtPartyName"
        Me.txtPartyName.ReadOnly = True
        Me.txtPartyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPartyName.Size = New System.Drawing.Size(255, 20)
        Me.txtPartyName.TabIndex = 5
        '
        'lvwCDList
        '
        Me.lvwCDList.BackColor = System.Drawing.SystemColors.Window
        Me.lvwCDList.CheckBoxes = True
        Me.listViewHelper1.SetColumnHeaderIcons(Me.lvwCDList, "")
        Me.listViewHelper1.SetCorrectEventsBehavior(Me.lvwCDList, True)
        Me.lvwCDList.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwCDList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwCDList.FullRowSelect = True
        Me.lvwCDList.HideSelection = False
        Me.listViewHelper1.SetItemClickMethod(Me.lvwCDList, "lvwCDList_ItemClick")
        Me.listViewHelper1.SetLargeIcons(Me.lvwCDList, "")
        Me.lvwCDList.LargeImageList = Me.ImageList1
        Me.lvwCDList.Location = New System.Drawing.Point(6, 52)
        Me.lvwCDList.MultiSelect = False
        Me.lvwCDList.Name = "lvwCDList"
        Me.lvwCDList.Size = New System.Drawing.Size(659, 275)
        Me.listViewHelper1.SetSmallIcons(Me.lvwCDList, "")
        Me.lvwCDList.SmallImageList = Me.ImageList1
        Me.listViewHelper1.SetSorted(Me.lvwCDList, True)
        Me.lvwCDList.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.listViewHelper1.SetSortKey(Me.lvwCDList, 0)
        Me.listViewHelper1.SetSortOrder(Me.lvwCDList, System.Windows.Forms.SortOrder.Ascending)
        Me.lvwCDList.TabIndex = 9
        Me.lvwCDList.UseCompatibleStateImageBehavior = False
        Me.lvwCDList.View = System.Windows.Forms.View.Details
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
        '
        'lblPartyCode
        '
        Me.lblPartyCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyCode.Location = New System.Drawing.Point(16, 26)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyCode.Size = New System.Drawing.Size(71, 17)
        Me.lblPartyCode.TabIndex = 8
        Me.lblPartyCode.Text = "Party Code:"
        '
        'lblPartyName
        '
        Me.lblPartyName.BackColor = System.Drawing.SystemColors.Control
        Me.lblPartyName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPartyName.Location = New System.Drawing.Point(238, 26)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPartyName.Size = New System.Drawing.Size(75, 17)
        Me.lblPartyName.TabIndex = 7
        Me.lblPartyName.Text = "Party Name:"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(588, 376)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(89, 23)
        Me.cmdCancel.TabIndex = 0
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(496, 376)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(89, 23)
        Me.cmdOk.TabIndex = 3
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'optAgent
        '
        Me.optAgent.BackColor = System.Drawing.SystemColors.Control
        Me.optAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.optAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optAgent.Location = New System.Drawing.Point(14, 10)
        Me.optAgent.Name = "optAgent"
        Me.optAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optAgent.Size = New System.Drawing.Size(72, 20)
        Me.optAgent.TabIndex = 2
        Me.optAgent.TabStop = True
        Me.optAgent.Text = "Agent"
        Me.optAgent.UseVisualStyleBackColor = False
        '
        'optClient
        '
        Me.optClient.BackColor = System.Drawing.SystemColors.Control
        Me.optClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.optClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optClient.Location = New System.Drawing.Point(92, 10)
        Me.optClient.Name = "optClient"
        Me.optClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optClient.Size = New System.Drawing.Size(64, 20)
        Me.optClient.TabIndex = 1
        Me.optClient.TabStop = True
        Me.optClient.Text = "Client"
        Me.optClient.UseVisualStyleBackColor = False
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(682, 403)
        Me.Controls.Add(Me.frmCD)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.optAgent)
        Me.Controls.Add(Me.optClient)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Make Live - Cash Deposit"
        Me.frmCD.ResumeLayout(False)
        Me.frmCD.PerformLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class