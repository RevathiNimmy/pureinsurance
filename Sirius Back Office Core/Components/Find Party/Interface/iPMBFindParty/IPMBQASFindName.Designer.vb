<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQASFindName
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
	Public WithEvents uctPMAddressControl1 As PMAddressControl.uctPMAddressControl
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtOrgName As System.Windows.Forms.TextBox
	Public WithEvents txtSurname As System.Windows.Forms.TextBox
	Public WithEvents txtForename As System.Windows.Forms.TextBox
	Public WithEvents txtInitial As System.Windows.Forms.TextBox
	Public WithEvents txtTitle As System.Windows.Forms.TextBox
	Public WithEvents lblorgname As System.Windows.Forms.Label
	Public WithEvents lblsurname As System.Windows.Forms.Label
	Public WithEvents lblinitial As System.Windows.Forms.Label
	Public WithEvents lblForename As System.Windows.Forms.Label
	Public WithEvents lbltitle As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Private WithEvents _TabStrip1_Tab1 As System.Windows.Forms.TabPage
	Public WithEvents TabStrip1_Tabs As System.Windows.Forms.TabControl.TabPageCollection
	Public WithEvents TabStrip1 As System.Windows.Forms.TabControl
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmQASFindName))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uctPMAddressControl1 = New PMAddressControl.uctPMAddressControl
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtOrgName = New System.Windows.Forms.TextBox
        Me.txtSurname = New System.Windows.Forms.TextBox
        Me.txtForename = New System.Windows.Forms.TextBox
        Me.txtInitial = New System.Windows.Forms.TextBox
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.lblorgname = New System.Windows.Forms.Label
        Me.lblsurname = New System.Windows.Forms.Label
        Me.lblinitial = New System.Windows.Forms.Label
        Me.lblForename = New System.Windows.Forms.Label
        Me.lbltitle = New System.Windows.Forms.Label
        Me.TabStrip1 = New System.Windows.Forms.TabControl
        Me._TabStrip1_Tab1 = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.Frame1.SuspendLayout()
        Me.TabStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'uctPMAddressControl1
        '
        Me.uctPMAddressControl1.AddressLine1 = ""
        Me.uctPMAddressControl1.AddressLine2 = ""
        Me.uctPMAddressControl1.AddressLine3 = ""
        Me.uctPMAddressControl1.AddressLine4 = ""
        Me.uctPMAddressControl1.Caption = ""
        Me.uctPMAddressControl1.CaptionAddress1 = "No. && street name:"
        Me.uctPMAddressControl1.CaptionAddress2 = "Locality:"
        Me.uctPMAddressControl1.CaptionAddress3 = "Town:"
        Me.uctPMAddressControl1.CaptionAddress4 = "County:"
        Me.uctPMAddressControl1.CaptionCountry = "Country:"
        Me.uctPMAddressControl1.CaptionFontBoldAddress1 = False
        Me.uctPMAddressControl1.CaptionFontBoldPostCode = False
        Me.uctPMAddressControl1.CaptionPostCode = "Postcode:"
        Me.uctPMAddressControl1.ClearButtonCaption = "X"
        Me.uctPMAddressControl1.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.ClearButtonLeft = 4935
        Me.uctPMAddressControl1.ClearButtonWidth = 360
        Me.uctPMAddressControl1.CountryId = 0
        Me.uctPMAddressControl1.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.IsCountryRequired = 0
        Me.uctPMAddressControl1.IsPostCodeRequired = 1
        Me.uctPMAddressControl1.Location = New System.Drawing.Point(24, 40)
        Me.uctPMAddressControl1.Name = "uctPMAddressControl1"
        Me.uctPMAddressControl1.Organisation = ""
        Me.uctPMAddressControl1.PMAddressCnt = 0
        Me.uctPMAddressControl1.PMDatabaseID = 0
        Me.uctPMAddressControl1.PostCode = ""
        Me.uctPMAddressControl1.QAS2PMAddress1 = "3,4,2,6,5"
        Me.uctPMAddressControl1.QAS2PMAddress2 = "8,7"
        Me.uctPMAddressControl1.QAS2PMAddress3 = "9"
        Me.uctPMAddressControl1.QAS2PMAddress4 = ""
        Me.uctPMAddressControl1.QASDatabaseID = 3
        Me.uctPMAddressControl1.SearchButtonCaption = ".."
        Me.uctPMAddressControl1.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctPMAddressControl1.SearchButtonHeight = 285
        Me.uctPMAddressControl1.SearchButtonLeft = 4500
        Me.uctPMAddressControl1.SearchButtonTop = 1545
        Me.uctPMAddressControl1.SearchButtonWidth = 360
        Me.uctPMAddressControl1.Size = New System.Drawing.Size(369, 129)
        Me.uctPMAddressControl1.TabIndex = 15
        Me.uctPMAddressControl1.WarningMessage = ""
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(336, 344)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 6
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(248, 344)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 5
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtOrgName)
        Me.Frame1.Controls.Add(Me.txtSurname)
        Me.Frame1.Controls.Add(Me.txtForename)
        Me.Frame1.Controls.Add(Me.txtInitial)
        Me.Frame1.Controls.Add(Me.txtTitle)
        Me.Frame1.Controls.Add(Me.lblorgname)
        Me.Frame1.Controls.Add(Me.lblsurname)
        Me.Frame1.Controls.Add(Me.lblinitial)
        Me.Frame1.Controls.Add(Me.lblForename)
        Me.Frame1.Controls.Add(Me.lbltitle)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(24, 192)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(369, 129)
        Me.Frame1.TabIndex = 9
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Returned Name Data"
        '
        'txtOrgName
        '
        Me.txtOrgName.AcceptsReturn = True
        Me.txtOrgName.BackColor = System.Drawing.SystemColors.Window
        Me.txtOrgName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOrgName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrgName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOrgName.Location = New System.Drawing.Point(136, 24)
        Me.txtOrgName.MaxLength = 0
        Me.txtOrgName.Name = "txtOrgName"
        Me.txtOrgName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOrgName.Size = New System.Drawing.Size(153, 19)
        Me.txtOrgName.TabIndex = 1
        '
        'txtSurname
        '
        Me.txtSurname.AcceptsReturn = True
        Me.txtSurname.BackColor = System.Drawing.SystemColors.Window
        Me.txtSurname.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSurname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSurname.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSurname.Location = New System.Drawing.Point(136, 96)
        Me.txtSurname.MaxLength = 0
        Me.txtSurname.Name = "txtSurname"
        Me.txtSurname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSurname.Size = New System.Drawing.Size(153, 19)
        Me.txtSurname.TabIndex = 4
        '
        'txtForename
        '
        Me.txtForename.AcceptsReturn = True
        Me.txtForename.BackColor = System.Drawing.SystemColors.Window
        Me.txtForename.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtForename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtForename.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtForename.Location = New System.Drawing.Point(136, 48)
        Me.txtForename.MaxLength = 0
        Me.txtForename.Name = "txtForename"
        Me.txtForename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtForename.Size = New System.Drawing.Size(153, 19)
        Me.txtForename.TabIndex = 2
        '
        'txtInitial
        '
        Me.txtInitial.AcceptsReturn = True
        Me.txtInitial.BackColor = System.Drawing.SystemColors.Window
        Me.txtInitial.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInitial.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInitial.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInitial.Location = New System.Drawing.Point(136, 72)
        Me.txtInitial.MaxLength = 0
        Me.txtInitial.Name = "txtInitial"
        Me.txtInitial.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInitial.Size = New System.Drawing.Size(81, 19)
        Me.txtInitial.TabIndex = 3
        '
        'txtTitle
        '
        Me.txtTitle.AcceptsReturn = True
        Me.txtTitle.BackColor = System.Drawing.SystemColors.Window
        Me.txtTitle.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTitle.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTitle.Location = New System.Drawing.Point(136, 24)
        Me.txtTitle.MaxLength = 0
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTitle.Size = New System.Drawing.Size(81, 19)
        Me.txtTitle.TabIndex = 0
        '
        'lblorgname
        '
        Me.lblorgname.BackColor = System.Drawing.SystemColors.Control
        Me.lblorgname.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblorgname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblorgname.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblorgname.Location = New System.Drawing.Point(16, 24)
        Me.lblorgname.Name = "lblorgname"
        Me.lblorgname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblorgname.Size = New System.Drawing.Size(89, 25)
        Me.lblorgname.TabIndex = 14
        Me.lblorgname.Text = "Trading Name"
        '
        'lblsurname
        '
        Me.lblsurname.BackColor = System.Drawing.SystemColors.Control
        Me.lblsurname.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblsurname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblsurname.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblsurname.Location = New System.Drawing.Point(16, 96)
        Me.lblsurname.Name = "lblsurname"
        Me.lblsurname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblsurname.Size = New System.Drawing.Size(97, 17)
        Me.lblsurname.TabIndex = 13
        Me.lblsurname.Text = "Surname:"
        '
        'lblinitial
        '
        Me.lblinitial.BackColor = System.Drawing.SystemColors.Control
        Me.lblinitial.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblinitial.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblinitial.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblinitial.Location = New System.Drawing.Point(16, 72)
        Me.lblinitial.Name = "lblinitial"
        Me.lblinitial.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblinitial.Size = New System.Drawing.Size(97, 17)
        Me.lblinitial.TabIndex = 12
        Me.lblinitial.Text = "Initial:"
        '
        'lblForename
        '
        Me.lblForename.BackColor = System.Drawing.SystemColors.Control
        Me.lblForename.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblForename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblForename.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblForename.Location = New System.Drawing.Point(16, 48)
        Me.lblForename.Name = "lblForename"
        Me.lblForename.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblForename.Size = New System.Drawing.Size(89, 17)
        Me.lblForename.TabIndex = 11
        Me.lblForename.Text = "Forename:"
        '
        'lbltitle
        '
        Me.lbltitle.BackColor = System.Drawing.SystemColors.Control
        Me.lbltitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbltitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbltitle.Location = New System.Drawing.Point(16, 24)
        Me.lbltitle.Name = "lbltitle"
        Me.lbltitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbltitle.Size = New System.Drawing.Size(89, 17)
        Me.lbltitle.TabIndex = 10
        Me.lbltitle.Text = "Title:"
        '
        'TabStrip1
        '
        Me.TabStrip1.Controls.Add(Me._TabStrip1_Tab1)
        Me.TabStrip1.Location = New System.Drawing.Point(8, 8)
        Me.TabStrip1.Name = "TabStrip1"
        Me.TabStrip1.SelectedIndex = 0
        Me.TabStrip1.Size = New System.Drawing.Size(401, 329)
        Me.TabStrip1.TabIndex = 7
        '
        '_TabStrip1_Tab1
        '
        Me._TabStrip1_Tab1.Location = New System.Drawing.Point(4, 22)
        Me._TabStrip1_Tab1.Name = "_TabStrip1_Tab1"
        Me._TabStrip1_Tab1.Size = New System.Drawing.Size(393, 303)
        Me._TabStrip1_Tab1.TabIndex = 0
        Me._TabStrip1_Tab1.Tag = ""
        Me._TabStrip1_Tab1.Text = "&1 - Search"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(40, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(113, 33)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Label1"
        '
        'frmQASFindName
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(418, 373)
        Me.Controls.Add(Me.uctPMAddressControl1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.TabStrip1)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmQASFindName"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Find Name and Address"
        Me.Frame1.ResumeLayout(False)
        Me.TabStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class