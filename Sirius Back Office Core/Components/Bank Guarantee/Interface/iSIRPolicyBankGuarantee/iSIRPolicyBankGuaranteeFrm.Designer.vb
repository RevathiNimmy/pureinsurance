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
	Private WithEvents _lvwBGList_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBGList_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBGList_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBGList_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Private WithEvents _lvwBGList_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwBGList As System.Windows.Forms.ListView
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents optAgent As System.Windows.Forms.RadioButton
	Public WithEvents optClient As System.Windows.Forms.RadioButton
	Public WithEvents ImageList1 As System.Windows.Forms.ImageList
    'Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtPartyCode = New System.Windows.Forms.TextBox
        Me.txtPartyName = New System.Windows.Forms.TextBox
        Me.lvwBGList = New System.Windows.Forms.ListView
        Me._lvwBGList_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwBGList_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwBGList_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwBGList_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me._lvwBGList_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
        Me._lvwBGList_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.optAgent = New System.Windows.Forms.RadioButton
        Me.optClient = New System.Windows.Forms.RadioButton
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtPartyCode)
        Me.Frame1.Controls.Add(Me.txtPartyName)
        Me.Frame1.Controls.Add(Me.lvwBGList)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Controls.Add(Me.Label2)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(6, 36)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(671, 333)
        Me.Frame1.TabIndex = 4
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Bank Guarantees"
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
        Me.txtPartyCode.Text = "ADMGA"
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
        Me.txtPartyName.Text = "Gaurav Arora"
        '
        'lvwBGList
        '
        Me.lvwBGList.BackColor = System.Drawing.SystemColors.Window
        Me.lvwBGList.CheckBoxes = True
        Me.lvwBGList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwBGList_ColumnHeader_1, Me._lvwBGList_ColumnHeader_2, Me._lvwBGList_ColumnHeader_3, Me._lvwBGList_ColumnHeader_4, Me._lvwBGList_ColumnHeader_5, Me._lvwBGList_ColumnHeader_6})
        Me.lvwBGList.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwBGList.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwBGList.FullRowSelect = True
        Me.lvwBGList.HideSelection = False
        Me.lvwBGList.LargeImageList = Me.ImageList1
        Me.lvwBGList.Location = New System.Drawing.Point(6, 52)
        Me.lvwBGList.MultiSelect = False
        Me.lvwBGList.Name = "lvwBGList"
        Me.lvwBGList.Size = New System.Drawing.Size(659, 275)
        Me.lvwBGList.SmallImageList = Me.ImageList1
        Me.lvwBGList.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwBGList.TabIndex = 9
        Me.lvwBGList.UseCompatibleStateImageBehavior = False
        Me.lvwBGList.View = System.Windows.Forms.View.Details
        '
        '_lvwBGList_ColumnHeader_1
        '
        Me._lvwBGList_ColumnHeader_1.Text = "Bank Id"
        Me._lvwBGList_ColumnHeader_1.Width = 80
        '
        '_lvwBGList_ColumnHeader_2
        '
        Me._lvwBGList_ColumnHeader_2.DisplayIndex = 2
        Me._lvwBGList_ColumnHeader_2.Text = "BG Number"
        Me._lvwBGList_ColumnHeader_2.Width = 100
        '
        '_lvwBGList_ColumnHeader_3
        '
        Me._lvwBGList_ColumnHeader_3.Text = "Available Limit"
        Me._lvwBGList_ColumnHeader_3.Width = 97
        '
        '_lvwBGList_ColumnHeader_4
        '
        Me._lvwBGList_ColumnHeader_4.Text = "Expiry Date"
        Me._lvwBGList_ColumnHeader_4.Width = 97
        '
        '_lvwBGList_ColumnHeader_5
        '
        Me._lvwBGList_ColumnHeader_5.Text = "Due Date"
        Me._lvwBGList_ColumnHeader_5.Width = 97
        '
        '_lvwBGList_ColumnHeader_6
        '
        Me._lvwBGList_ColumnHeader_6.DisplayIndex = 1
        Me._lvwBGList_ColumnHeader_6.Text = "Bank Name"
        Me._lvwBGList_ColumnHeader_6.Width = 120
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
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(71, 17)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Party Code:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(238, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(75, 17)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Party Name:"
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
        Me.optAgent.Size = New System.Drawing.Size(66, 20)
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
        Me.optClient.Location = New System.Drawing.Point(86, 10)
        Me.optClient.Name = "optClient"
        Me.optClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optClient.Size = New System.Drawing.Size(68, 20)
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
        Me.Controls.Add(Me.Frame1)
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
        Me.Text = "Make Live - Bank Guarantee"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _lvwBGList_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
#End Region 
End Class