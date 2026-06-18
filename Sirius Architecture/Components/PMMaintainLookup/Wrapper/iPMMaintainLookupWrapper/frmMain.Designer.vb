<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdMaintain As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents imgIcons As System.Windows.Forms.ImageList
	Private WithEvents _lvwTables_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTables As System.Windows.Forms.ListView
	Public WithEvents cboPMProducts As PMLookupControl.cboPMLookup
	Public WithEvents lblPMProduc As System.Windows.Forms.Label
	Public WithEvents lblMaintainableTables As System.Windows.Forms.Label
	Public WithEvents imgIcon As System.Windows.Forms.PictureBox

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdMaintain = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.imgIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.lvwTables = New System.Windows.Forms.ListView
        Me._lvwTables_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me.cboPMProducts = New PMLookupControl.cboPMLookup
        Me.lblPMProduc = New System.Windows.Forms.Label
        Me.lblMaintainableTables = New System.Windows.Forms.Label
        Me.imgIcon = New System.Windows.Forms.PictureBox
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdMaintain
        '
        Me.cmdMaintain.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMaintain.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMaintain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMaintain.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMaintain.Location = New System.Drawing.Point(8, 320)
        Me.cmdMaintain.Name = "cmdMaintain"
        Me.cmdMaintain.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMaintain.Size = New System.Drawing.Size(73, 22)
        Me.cmdMaintain.TabIndex = 4
        Me.cmdMaintain.Text = "&Maintain"
        Me.cmdMaintain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdMaintain.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(272, 320)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 5
        Me.cmdOK.Text = "&Exit"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'imgIcons
        '
        Me.imgIcons.ImageStream = CType(resources.GetObject("imgIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgIcons.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.imgIcons.Images.SetKeyName(0, "table")
        '
        'lvwTables
        '
        Me.lvwTables.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTables.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwTables.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTables_ColumnHeader_1})
        Me.lvwTables.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTables.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwTables.HideSelection = False
        Me.lvwTables.LargeImageList = Me.imgIcons
        Me.lvwTables.Location = New System.Drawing.Point(8, 72)
        Me.lvwTables.MultiSelect = False
        Me.lvwTables.Name = "lvwTables"
        Me.lvwTables.Size = New System.Drawing.Size(337, 241)
        Me.lvwTables.SmallImageList = Me.imgIcons
        Me.lvwTables.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwTables.TabIndex = 3
        Me.lvwTables.UseCompatibleStateImageBehavior = False
        Me.lvwTables.View = System.Windows.Forms.View.Details
        '
        '_lvwTables_ColumnHeader_1
        '
        Me._lvwTables_ColumnHeader_1.Text = "Tables"
        Me._lvwTables_ColumnHeader_1.Width = 334
        '
        'cboPMProducts
        '
        Me.cboPMProducts.DefaultItemId = 0
        Me.cboPMProducts.FirstItem = ""
        Me.cboPMProducts.ItemId = 0
        Me.cboPMProducts.ListIndex = -1
        Me.cboPMProducts.Location = New System.Drawing.Point(8, 24)
        Me.cboPMProducts.Name = "cboPMProducts"
        Me.cboPMProducts.PMLookupProductFamily = 1
        Me.cboPMProducts.SingleItemId = 0
        Me.cboPMProducts.Size = New System.Drawing.Size(161, 21)
        Me.cboPMProducts.Sorted = True
        Me.cboPMProducts.TabIndex = 1
        Me.cboPMProducts.TableName = "PMProduct"
        Me.cboPMProducts.ToolTipText = ""
        Me.cboPMProducts.WhereClause = ""
        '
        'lblPMProduc
        '
        Me.lblPMProduc.AutoSize = True
        Me.lblPMProduc.BackColor = System.Drawing.SystemColors.Control
        Me.lblPMProduc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPMProduc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPMProduc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPMProduc.Location = New System.Drawing.Point(8, 8)
        Me.lblPMProduc.Name = "lblPMProduc"
        Me.lblPMProduc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPMProduc.Size = New System.Drawing.Size(115, 13)
        Me.lblPMProduc.TabIndex = 0
        Me.lblPMProduc.Text = "PM Product Database:"
        '
        'lblMaintainableTables
        '
        Me.lblMaintainableTables.AutoSize = True
        Me.lblMaintainableTables.BackColor = System.Drawing.SystemColors.Control
        Me.lblMaintainableTables.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMaintainableTables.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaintainableTables.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMaintainableTables.Location = New System.Drawing.Point(8, 56)
        Me.lblMaintainableTables.Name = "lblMaintainableTables"
        Me.lblMaintainableTables.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMaintainableTables.Size = New System.Drawing.Size(105, 13)
        Me.lblMaintainableTables.TabIndex = 2
        Me.lblMaintainableTables.Text = "Maintainable Tables:"
        '
        'imgIcon
        '
        Me.imgIcon.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgIcon.Image = CType(resources.GetObject("imgIcon.Image"), System.Drawing.Image)
        Me.imgIcon.Location = New System.Drawing.Point(312, 16)
        Me.imgIcon.Name = "imgIcon"
        Me.imgIcon.Size = New System.Drawing.Size(32, 32)
        Me.imgIcon.TabIndex = 6
        Me.imgIcon.TabStop = False
        '
        'frmMain
        '
        Me.AcceptButton = Me.cmdMaintain
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(353, 351)
        Me.Controls.Add(Me.cmdMaintain)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lvwTables)
        Me.Controls.Add(Me.cboPMProducts)
        Me.Controls.Add(Me.lblPMProduc)
        Me.Controls.Add(Me.lblMaintainableTables)
        Me.Controls.Add(Me.imgIcon)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PM Lookup Tables"
        CType(Me.imgIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class