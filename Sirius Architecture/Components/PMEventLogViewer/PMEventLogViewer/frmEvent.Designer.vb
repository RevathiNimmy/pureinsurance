<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEvent
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
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents imgInfo As System.Windows.Forms.PictureBox
	Public WithEvents imgWarning As System.Windows.Forms.PictureBox
	Public WithEvents imgError As System.Windows.Forms.PictureBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents lblEventDataLabel As System.Windows.Forms.Label
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents lblEventDataText As System.Windows.Forms.Label
	Public WithEvents Label9 As System.Windows.Forms.Label
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents lblTimestamp As System.Windows.Forms.Label
	Public WithEvents lblCategory As System.Windows.Forms.Label
	Public WithEvents lblEventType As System.Windows.Forms.Label
	Public WithEvents lblComputer As System.Windows.Forms.Label
	Public WithEvents lblEventData As System.Windows.Forms.Label
	Public WithEvents lblSource As System.Windows.Forms.Label
	Public WithEvents lblCategoryString As System.Windows.Forms.Label
	Public WithEvents lblEventID As System.Windows.Forms.Label
	Public WithEvents cmdUp As System.Windows.Forms.Button
	Public WithEvents txtDescription As System.Windows.Forms.TextBox
	Public WithEvents txtEventDataText As System.Windows.Forms.TextBox
	Public WithEvents cmdDown As System.Windows.Forms.Button
	Private WithEvents _tabMain_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMain As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEvent))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOk = New System.Windows.Forms.Button
        Me.cmdUp = New System.Windows.Forms.Button
        Me.cmdDown = New System.Windows.Forms.Button
        Me.tabMain = New System.Windows.Forms.TabControl
        Me._tabMain_TabPage0 = New System.Windows.Forms.TabPage
        Me.imgInfo = New System.Windows.Forms.PictureBox
        Me.imgWarning = New System.Windows.Forms.PictureBox
        Me.imgError = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblEventDataLabel = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.lblEventDataText = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.lblTimestamp = New System.Windows.Forms.Label
        Me.lblCategory = New System.Windows.Forms.Label
        Me.lblEventType = New System.Windows.Forms.Label
        Me.lblComputer = New System.Windows.Forms.Label
        Me.lblEventData = New System.Windows.Forms.Label
        Me.lblSource = New System.Windows.Forms.Label
        Me.lblCategoryString = New System.Windows.Forms.Label
        Me.lblEventID = New System.Windows.Forms.Label
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.txtEventDataText = New System.Windows.Forms.TextBox
        Me.tabMain.SuspendLayout()
        Me._tabMain_TabPage0.SuspendLayout()
        CType(Me.imgInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgWarning, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgError, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(354, 416)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(69, 25)
        Me.cmdOk.TabIndex = 1
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdOk, "Exit")
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'cmdUp
        '
        Me.cmdUp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUp.Font = New System.Drawing.Font("Webdings", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.cmdUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUp.Location = New System.Drawing.Point(384, 8)
        Me.cmdUp.Name = "cmdUp"
        Me.cmdUp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUp.Size = New System.Drawing.Size(25, 25)
        Me.cmdUp.TabIndex = 12
        Me.cmdUp.Text = "5"
        Me.cmdUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdUp, "View previous event")
        Me.cmdUp.UseVisualStyleBackColor = False
        '
        'cmdDown
        '
        Me.cmdDown.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDown.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDown.Font = New System.Drawing.Font("Webdings", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.cmdDown.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDown.Location = New System.Drawing.Point(384, 36)
        Me.cmdDown.Name = "cmdDown"
        Me.cmdDown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDown.Size = New System.Drawing.Size(25, 25)
        Me.cmdDown.TabIndex = 23
        Me.cmdDown.Text = "6"
        Me.cmdDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolTip1.SetToolTip(Me.cmdDown, "View next event")
        Me.cmdDown.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me._tabMain_TabPage0)
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(414, 18)
        Me.tabMain.Location = New System.Drawing.Point(8, 8)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(419, 405)
        Me.tabMain.TabIndex = 0
        '
        '_tabMain_TabPage0
        '
        Me._tabMain_TabPage0.Controls.Add(Me.imgInfo)
        Me._tabMain_TabPage0.Controls.Add(Me.imgWarning)
        Me._tabMain_TabPage0.Controls.Add(Me.imgError)
        Me._tabMain_TabPage0.Controls.Add(Me.Label1)
        Me._tabMain_TabPage0.Controls.Add(Me.Label2)
        Me._tabMain_TabPage0.Controls.Add(Me.Label3)
        Me._tabMain_TabPage0.Controls.Add(Me.Label4)
        Me._tabMain_TabPage0.Controls.Add(Me.Label5)
        Me._tabMain_TabPage0.Controls.Add(Me.lblEventDataLabel)
        Me._tabMain_TabPage0.Controls.Add(Me.Label7)
        Me._tabMain_TabPage0.Controls.Add(Me.lblEventDataText)
        Me._tabMain_TabPage0.Controls.Add(Me.Label9)
        Me._tabMain_TabPage0.Controls.Add(Me.Label10)
        Me._tabMain_TabPage0.Controls.Add(Me.lblTimestamp)
        Me._tabMain_TabPage0.Controls.Add(Me.lblCategory)
        Me._tabMain_TabPage0.Controls.Add(Me.lblEventType)
        Me._tabMain_TabPage0.Controls.Add(Me.lblComputer)
        Me._tabMain_TabPage0.Controls.Add(Me.lblEventData)
        Me._tabMain_TabPage0.Controls.Add(Me.lblSource)
        Me._tabMain_TabPage0.Controls.Add(Me.lblCategoryString)
        Me._tabMain_TabPage0.Controls.Add(Me.lblEventID)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdUp)
        Me._tabMain_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMain_TabPage0.Controls.Add(Me.txtEventDataText)
        Me._tabMain_TabPage0.Controls.Add(Me.cmdDown)
        Me._tabMain_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMain_TabPage0.Name = "_tabMain_TabPage0"
        Me._tabMain_TabPage0.Size = New System.Drawing.Size(411, 379)
        Me._tabMain_TabPage0.TabIndex = 0
        Me._tabMain_TabPage0.Text = "Event details"
        '
        'imgInfo
        '
        Me.imgInfo.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgInfo.Image = CType(resources.GetObject("imgInfo.Image"), System.Drawing.Image)
        Me.imgInfo.Location = New System.Drawing.Point(8, 16)
        Me.imgInfo.Name = "imgInfo"
        Me.imgInfo.Size = New System.Drawing.Size(32, 32)
        Me.imgInfo.TabIndex = 0
        Me.imgInfo.TabStop = False
        Me.imgInfo.Visible = False
        '
        'imgWarning
        '
        Me.imgWarning.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgWarning.Image = CType(resources.GetObject("imgWarning.Image"), System.Drawing.Image)
        Me.imgWarning.Location = New System.Drawing.Point(8, 16)
        Me.imgWarning.Name = "imgWarning"
        Me.imgWarning.Size = New System.Drawing.Size(32, 32)
        Me.imgWarning.TabIndex = 1
        Me.imgWarning.TabStop = False
        Me.imgWarning.Visible = False
        '
        'imgError
        '
        Me.imgError.Cursor = System.Windows.Forms.Cursors.Default
        Me.imgError.Image = CType(resources.GetObject("imgError.Image"), System.Drawing.Image)
        Me.imgError.Location = New System.Drawing.Point(8, 16)
        Me.imgError.Name = "imgError"
        Me.imgError.Size = New System.Drawing.Size(32, 32)
        Me.imgError.TabIndex = 2
        Me.imgError.TabStop = False
        Me.imgError.Visible = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(41, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Timestamp"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(211, 92)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Source"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(41, 92)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(57, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Computer"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(41, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Event type"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(16, 128)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(61, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Description"
        '
        'lblEventDataLabel
        '
        Me.lblEventDataLabel.BackColor = System.Drawing.SystemColors.Control
        Me.lblEventDataLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEventDataLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEventDataLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEventDataLabel.Location = New System.Drawing.Point(16, 232)
        Me.lblEventDataLabel.Name = "lblEventDataLabel"
        Me.lblEventDataLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEventDataLabel.Size = New System.Drawing.Size(57, 13)
        Me.lblEventDataLabel.TabIndex = 7
        Me.lblEventDataLabel.Text = "Event data"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(41, 40)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(49, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Category"
        '
        'lblEventDataText
        '
        Me.lblEventDataText.BackColor = System.Drawing.SystemColors.Control
        Me.lblEventDataText.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEventDataText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEventDataText.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEventDataText.Location = New System.Drawing.Point(16, 268)
        Me.lblEventDataText.Name = "lblEventDataText"
        Me.lblEventDataText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEventDataText.Size = New System.Drawing.Size(81, 13)
        Me.lblEventDataText.TabIndex = 9
        Me.lblEventDataText.Text = "Event data text"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(211, 40)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(88, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Category string"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(211, 60)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(53, 13)
        Me.Label10.TabIndex = 11
        Me.Label10.Text = "Event ID"
        '
        'lblTimestamp
        '
        Me.lblTimestamp.BackColor = System.Drawing.SystemColors.Control
        Me.lblTimestamp.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTimestamp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTimestamp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTimestamp.Location = New System.Drawing.Point(105, 20)
        Me.lblTimestamp.Name = "lblTimestamp"
        Me.lblTimestamp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTimestamp.Size = New System.Drawing.Size(261, 17)
        Me.lblTimestamp.TabIndex = 13
        Me.lblTimestamp.Text = "lblTimestamp"
        '
        'lblCategory
        '
        Me.lblCategory.BackColor = System.Drawing.SystemColors.Control
        Me.lblCategory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCategory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCategory.Location = New System.Drawing.Point(108, 39)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCategory.Size = New System.Drawing.Size(97, 17)
        Me.lblCategory.TabIndex = 14
        Me.lblCategory.Text = "lblCategory"
        '
        'lblEventType
        '
        Me.lblEventType.BackColor = System.Drawing.SystemColors.Control
        Me.lblEventType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblEventType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEventType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEventType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEventType.Location = New System.Drawing.Point(108, 60)
        Me.lblEventType.Name = "lblEventType"
        Me.lblEventType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEventType.Size = New System.Drawing.Size(97, 17)
        Me.lblEventType.TabIndex = 15
        Me.lblEventType.Text = "lblEventType"
        '
        'lblComputer
        '
        Me.lblComputer.BackColor = System.Drawing.SystemColors.Control
        Me.lblComputer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblComputer.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblComputer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComputer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblComputer.Location = New System.Drawing.Point(108, 92)
        Me.lblComputer.Name = "lblComputer"
        Me.lblComputer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblComputer.Size = New System.Drawing.Size(97, 17)
        Me.lblComputer.TabIndex = 16
        Me.lblComputer.Text = "lblComputer"
        '
        'lblEventData
        '
        Me.lblEventData.BackColor = System.Drawing.SystemColors.Control
        Me.lblEventData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblEventData.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEventData.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEventData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEventData.Location = New System.Drawing.Point(16, 246)
        Me.lblEventData.Name = "lblEventData"
        Me.lblEventData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEventData.Size = New System.Drawing.Size(377, 17)
        Me.lblEventData.TabIndex = 17
        Me.lblEventData.Text = "lblEventData"
        '
        'lblSource
        '
        Me.lblSource.BackColor = System.Drawing.SystemColors.Control
        Me.lblSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSource.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSource.Location = New System.Drawing.Point(281, 92)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSource.Size = New System.Drawing.Size(121, 17)
        Me.lblSource.TabIndex = 18
        Me.lblSource.Text = "lblSource"
        '
        'lblCategoryString
        '
        Me.lblCategoryString.BackColor = System.Drawing.SystemColors.Control
        Me.lblCategoryString.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCategoryString.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCategoryString.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCategoryString.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCategoryString.Location = New System.Drawing.Point(305, 39)
        Me.lblCategoryString.Name = "lblCategoryString"
        Me.lblCategoryString.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCategoryString.Size = New System.Drawing.Size(73, 17)
        Me.lblCategoryString.TabIndex = 19
        Me.lblCategoryString.Text = "lblCategoryString"
        '
        'lblEventID
        '
        Me.lblEventID.BackColor = System.Drawing.SystemColors.Control
        Me.lblEventID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblEventID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEventID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEventID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEventID.Location = New System.Drawing.Point(281, 60)
        Me.lblEventID.Name = "lblEventID"
        Me.lblEventID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEventID.Size = New System.Drawing.Size(97, 17)
        Me.lblEventID.TabIndex = 20
        Me.lblEventID.Text = "lblEventID"
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(16, 140)
        Me.txtDescription.MaxLength = 0
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(377, 85)
        Me.txtDescription.TabIndex = 21
        Me.txtDescription.Text = "txtDescription" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'txtEventDataText
        '
        Me.txtEventDataText.AcceptsReturn = True
        Me.txtEventDataText.BackColor = System.Drawing.SystemColors.Window
        Me.txtEventDataText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEventDataText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEventDataText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEventDataText.Location = New System.Drawing.Point(16, 280)
        Me.txtEventDataText.MaxLength = 0
        Me.txtEventDataText.Multiline = True
        Me.txtEventDataText.Name = "txtEventDataText"
        Me.txtEventDataText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEventDataText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtEventDataText.Size = New System.Drawing.Size(377, 85)
        Me.txtEventDataText.TabIndex = 22
        Me.txtEventDataText.Text = "txtEventDataText" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'frmEvent
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(430, 447)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.tabMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEvent"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "View event"
        Me.tabMain.ResumeLayout(False)
        Me._tabMain_TabPage0.ResumeLayout(False)
        Me._tabMain_TabPage0.PerformLayout()
        CType(Me.imgInfo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgWarning, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgError, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class