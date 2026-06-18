<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		Form_Initialize_Renamed()
	End Sub
    Private Sub ReleaseResources(ByVal eventSender As Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Closed
        Dispose(True)
    End Sub
    Dim fTerminateCalled_Form_Terminate_Renamed As Boolean
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
     Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not fTerminateCalled_Form_Terminate_Renamed Then
                fTerminateCalled_Form_Terminate_Renamed = True
                Form_Terminate_Renamed()
            End If
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblBGRef As System.Windows.Forms.Label
	Public WithEvents cboBGStatus As PMLookupControl.cboPMLookup
	Public WithEvents cmdPolicy As System.Windows.Forms.Button
	Public WithEvents CmdClient As System.Windows.Forms.Button
	Public WithEvents txtBGHolder As System.Windows.Forms.TextBox
	Public WithEvents txtPolicy As System.Windows.Forms.TextBox
	Public WithEvents txtBGRef As System.Windows.Forms.TextBox
	Public WithEvents txtAgent As System.Windows.Forms.TextBox
	Public WithEvents cmdAgent As System.Windows.Forms.Button
	Public WithEvents cmdBank As System.Windows.Forms.Button
	Public WithEvents txtBank As System.Windows.Forms.TextBox
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
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
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdEdit = New System.Windows.Forms.Button
        Me.cmdNew = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblBGRef = New System.Windows.Forms.Label
        Me.cboBGStatus = New PMLookupControl.cboPMLookup
        Me.cmdPolicy = New System.Windows.Forms.Button
        Me.CmdClient = New System.Windows.Forms.Button
        Me.txtBGHolder = New System.Windows.Forms.TextBox
        Me.txtPolicy = New System.Windows.Forms.TextBox
        Me.txtBGRef = New System.Windows.Forms.TextBox
        Me.txtAgent = New System.Windows.Forms.TextBox
        Me.cmdAgent = New System.Windows.Forms.Button
        Me.cmdBank = New System.Windows.Forms.Button
        Me.txtBank = New System.Windows.Forms.TextBox
        Me.lvwsearchdetails = New System.Windows.Forms.ListView
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ImageList1.Images.SetKeyName(0, "add")
        Me.ImageList1.Images.SetKeyName(1, "list")
        Me.ImageList1.Images.SetKeyName(2, "edited")
        Me.ImageList1.Images.SetKeyName(3, "delete")
        Me.ImageList1.Images.SetKeyName(4, "saved")
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(684, 56)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(81, 22)
        Me.cmdNewSearch.TabIndex = 7
        Me.cmdNewSearch.TabStop = False
        Me.cmdNewSearch.Text = "Ne&w Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(684, 30)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(81, 22)
        Me.cmdFindNow.TabIndex = 6
        Me.cmdFindNow.TabStop = False
        Me.cmdFindNow.Text = "F&ind Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(692, 423)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 10
        Me.cmdCancel.TabStop = False
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Enabled = False
        Me.cmdEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(614, 423)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
        Me.cmdEdit.TabIndex = 9
        Me.cmdEdit.TabStop = False
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdNew
        '
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNew.Enabled = False
        Me.cmdNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNew.Location = New System.Drawing.Point(536, 423)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNew.Size = New System.Drawing.Size(73, 22)
        Me.cmdNew.TabIndex = 8
        Me.cmdNew.TabStop = False
        Me.cmdNew.Text = "&Add"
        Me.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(221, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(8, 10)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(671, 141)
        Me.tabMainTab.TabIndex = 11
        Me.tabMainTab.TabStop = False
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.Label1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblBGRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboBGStatus)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.CmdClient)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBGHolder)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtPolicy)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBGRef)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtAgent)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdAgent)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cmdBank)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtBank)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(663, 115)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - General"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(398, 62)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "BG Status:"
        '
        'lblBGRef
        '
        Me.lblBGRef.AutoSize = True
        Me.lblBGRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblBGRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBGRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBGRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBGRef.Location = New System.Drawing.Point(340, 14)
        Me.lblBGRef.Name = "lblBGRef"
        Me.lblBGRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBGRef.Size = New System.Drawing.Size(128, 13)
        Me.lblBGRef.TabIndex = 12
        Me.lblBGRef.Text = "&Bank Guarentee Ref:"
        '
        'cboBGStatus
        '
        Me.cboBGStatus.DefaultItemId = 0
        Me.cboBGStatus.FirstItem = ""
        Me.cboBGStatus.ItemId = 0
        Me.cboBGStatus.ListIndex = -1
        Me.cboBGStatus.Location = New System.Drawing.Point(464, 60)
        Me.cboBGStatus.Name = "cboBGStatus"
        Me.cboBGStatus.PMLookupProductFamily = 1
        Me.cboBGStatus.SingleItemId = 0
        Me.cboBGStatus.Size = New System.Drawing.Size(195, 21)
        Me.cboBGStatus.Sorted = True
        Me.cboBGStatus.TabIndex = 5
        Me.cboBGStatus.TableName = "bg_status"
        Me.cboBGStatus.ToolTipText = ""
        Me.cboBGStatus.WhereClause = ""
        '
        'cmdPolicy
        '
        Me.cmdPolicy.AutoSize = True
        Me.cmdPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdPolicy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdPolicy.Location = New System.Drawing.Point(16, 60)
        Me.cmdPolicy.Name = "cmdPolicy"
        Me.cmdPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdPolicy.Size = New System.Drawing.Size(97, 23)
        Me.cmdPolicy.TabIndex = 13
        Me.cmdPolicy.Text = "Insurance File"
        Me.cmdPolicy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdPolicy.UseVisualStyleBackColor = False
        '
        'CmdClient
        '
        Me.CmdClient.BackColor = System.Drawing.SystemColors.Control
        Me.CmdClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.CmdClient.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CmdClient.Location = New System.Drawing.Point(16, 12)
        Me.CmdClient.Name = "CmdClient"
        Me.CmdClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CmdClient.Size = New System.Drawing.Size(90, 19)
        Me.CmdClient.TabIndex = 14
        Me.CmdClient.Text = "Client"
        Me.CmdClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.CmdClient.UseVisualStyleBackColor = False
        '
        'txtBGHolder
        '
        Me.txtBGHolder.AcceptsReturn = True
        Me.txtBGHolder.BackColor = System.Drawing.SystemColors.Window
        Me.txtBGHolder.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBGHolder.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBGHolder.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBGHolder.Location = New System.Drawing.Point(120, 12)
        Me.txtBGHolder.MaxLength = 0
        Me.txtBGHolder.Name = "txtBGHolder"
        Me.txtBGHolder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBGHolder.Size = New System.Drawing.Size(137, 21)
        Me.txtBGHolder.TabIndex = 0
        '
        'txtPolicy
        '
        Me.txtPolicy.AcceptsReturn = True
        Me.txtPolicy.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicy.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicy.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicy.Location = New System.Drawing.Point(120, 60)
        Me.txtPolicy.MaxLength = 30
        Me.txtPolicy.Name = "txtPolicy"
        Me.txtPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicy.Size = New System.Drawing.Size(201, 21)
        Me.txtPolicy.TabIndex = 2
        '
        'txtBGRef
        '
        Me.txtBGRef.AcceptsReturn = True
        Me.txtBGRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtBGRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBGRef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBGRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBGRef.Location = New System.Drawing.Point(464, 12)
        Me.txtBGRef.MaxLength = 30
        Me.txtBGRef.Name = "txtBGRef"
        Me.txtBGRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBGRef.Size = New System.Drawing.Size(195, 21)
        Me.txtBGRef.TabIndex = 3
        '
        'txtAgent
        '
        Me.txtAgent.AcceptsReturn = True
        Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgent.Location = New System.Drawing.Point(120, 36)
        Me.txtAgent.MaxLength = 0
        Me.txtAgent.Name = "txtAgent"
        Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgent.Size = New System.Drawing.Size(137, 21)
        Me.txtAgent.TabIndex = 1
        '
        'cmdAgent
        '
        Me.cmdAgent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgent.Location = New System.Drawing.Point(16, 36)
        Me.cmdAgent.Name = "cmdAgent"
        Me.cmdAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgent.Size = New System.Drawing.Size(90, 19)
        Me.cmdAgent.TabIndex = 15
        Me.cmdAgent.Text = "Agent"
        Me.cmdAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgent.UseVisualStyleBackColor = False
        '
        'cmdBank
        '
        Me.cmdBank.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBank.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBank.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBank.Location = New System.Drawing.Point(368, 34)
        Me.cmdBank.Name = "cmdBank"
        Me.cmdBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBank.Size = New System.Drawing.Size(90, 19)
        Me.cmdBank.TabIndex = 18
        Me.cmdBank.Text = "Bank Name"
        Me.cmdBank.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdBank.UseVisualStyleBackColor = False
        '
        'txtBank
        '
        Me.txtBank.AcceptsReturn = True
        Me.txtBank.BackColor = System.Drawing.SystemColors.Window
        Me.txtBank.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBank.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBank.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBank.Location = New System.Drawing.Point(464, 36)
        Me.txtBank.MaxLength = 0
        Me.txtBank.Name = "txtBank"
        Me.txtBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBank.Size = New System.Drawing.Size(195, 21)
        Me.txtBank.TabIndex = 4
        '
        'lvwsearchdetails
        '
        Me.lvwsearchdetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwsearchdetails.FullRowSelect = True
        Me.lvwsearchdetails.LargeImageList = Me.ImageList1
        Me.lvwsearchdetails.Location = New System.Drawing.Point(6, 150)
        Me.lvwsearchdetails.Name = "lvwsearchdetails"
        Me.lvwsearchdetails.Size = New System.Drawing.Size(757, 267)
        Me.lvwsearchdetails.SmallImageList = Me.ImageList1
        Me.lvwsearchdetails.TabIndex = 12
        Me.lvwsearchdetails.UseCompatibleStateImageBehavior = False
        Me.lvwsearchdetails.View = System.Windows.Forms.View.Details
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdFindNow
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(774, 455)
        Me.Controls.Add(Me.lvwsearchdetails)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdNew)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(10, 29)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Find Bank Guarantee"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvwsearchdetails As System.Windows.Forms.ListView
#End Region 
End Class