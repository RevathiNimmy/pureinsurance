<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
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
	Public dlgHelpOpen As System.Windows.Forms.OpenFileDialog
	Public dlgHelpSave As System.Windows.Forms.SaveFileDialog
	Public dlgHelpFont As System.Windows.Forms.FontDialog
	Public dlgHelpColor As System.Windows.Forms.ColorDialog
	Public dlgHelpPrint As System.Windows.Forms.PrintDialog
	Public WithEvents cmdHelp As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents txtAdPostcode As System.Windows.Forms.TextBox
	Public WithEvents txtAdReference As System.Windows.Forms.TextBox
	Public WithEvents cboAddUsageType As PMLookupControl.cboPMLookup
	Public WithEvents lblAddressUsageType As System.Windows.Forms.Label
	Public WithEvents lbAdReference As System.Windows.Forms.Label
	Public WithEvents lblAdPostcode As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents uctAdd As PMAddressControl.uctPMAddressControl
	Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents tabMainTab As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dlgHelpOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgHelpSave = New System.Windows.Forms.SaveFileDialog
        Me.dlgHelpFont = New System.Windows.Forms.FontDialog
        Me.dlgHelpColor = New System.Windows.Forms.ColorDialog
        Me.dlgHelpPrint = New System.Windows.Forms.PrintDialog
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.tabMainTab = New System.Windows.Forms.TabControl
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtAdPostcode = New System.Windows.Forms.TextBox
        Me.txtAdReference = New System.Windows.Forms.TextBox
        Me.cboAddUsageType = New PMLookupControl.cboPMLookup
        Me.lblAddressUsageType = New System.Windows.Forms.Label
        Me.lbAdReference = New System.Windows.Forms.Label
        Me.lblAdPostcode = New System.Windows.Forms.Label
        Me.uctAdd = New PMAddressControl.uctPMAddressControl
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdHelp
        '
        Me.cmdHelp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdHelp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdHelp.Location = New System.Drawing.Point(394, 303)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdHelp.Size = New System.Drawing.Size(73, 22)
        Me.cmdHelp.TabIndex = 2
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdHelp.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(314, 303)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(234, 303)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMainTab.ItemSize = New System.Drawing.Size(91, 18)
        Me.tabMainTab.Location = New System.Drawing.Point(6, 6)
        Me.tabMainTab.Multiline = True
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(465, 294)
        Me.tabMainTab.TabIndex = 3
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.Frame1)
        Me._tabMainTab_TabPage0.Controls.Add(Me.uctAdd)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(457, 268)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "&1 - Address"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtAdPostcode)
        Me.Frame1.Controls.Add(Me.txtAdReference)
        Me.Frame1.Controls.Add(Me.cboAddUsageType)
        Me.Frame1.Controls.Add(Me.lblAddressUsageType)
        Me.Frame1.Controls.Add(Me.lbAdReference)
        Me.Frame1.Controls.Add(Me.lblAdPostcode)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(10, 4)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(441, 97)
        Me.Frame1.TabIndex = 4
        Me.Frame1.TabStop = False
        '
        'txtAdPostcode
        '
        Me.txtAdPostcode.AcceptsReturn = True
        Me.txtAdPostcode.BackColor = System.Drawing.SystemColors.Control
        Me.txtAdPostcode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAdPostcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdPostcode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAdPostcode.Location = New System.Drawing.Point(136, 42)
        Me.txtAdPostcode.MaxLength = 0
        Me.txtAdPostcode.Name = "txtAdPostcode"
        Me.txtAdPostcode.ReadOnly = True
        Me.txtAdPostcode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAdPostcode.Size = New System.Drawing.Size(81, 20)
        Me.txtAdPostcode.TabIndex = 7
        Me.txtAdPostcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdReference
        '
        Me.txtAdReference.AcceptsReturn = True
        Me.txtAdReference.BackColor = System.Drawing.SystemColors.Control
        Me.txtAdReference.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAdReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdReference.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAdReference.Location = New System.Drawing.Point(136, 18)
        Me.txtAdReference.MaxLength = 0
        Me.txtAdReference.Name = "txtAdReference"
        Me.txtAdReference.ReadOnly = True
        Me.txtAdReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAdReference.Size = New System.Drawing.Size(289, 20)
        Me.txtAdReference.TabIndex = 10
        Me.txtAdReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboAddUsageType
        '
        Me.cboAddUsageType.DefaultItemId = 0
        Me.cboAddUsageType.ItemId = 0
        Me.cboAddUsageType.ListIndex = -1
        Me.cboAddUsageType.Location = New System.Drawing.Point(136, 66)
        Me.cboAddUsageType.Name = "cboAddUsageType"
        Me.cboAddUsageType.PMLookupProductFamily = 9
        Me.cboAddUsageType.SingleItemId = 0
        Me.cboAddUsageType.Size = New System.Drawing.Size(177, 21)
        Me.cboAddUsageType.Sorted = True
        Me.cboAddUsageType.TabIndex = 11
        Me.cboAddUsageType.TableName = "address_usage_type"
        Me.cboAddUsageType.ToolTipText = ""
        Me.cboAddUsageType.WhereClause = ""
        '
        'lblAddressUsageType
        '
        Me.lblAddressUsageType.AutoSize = True
        Me.lblAddressUsageType.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddressUsageType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddressUsageType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddressUsageType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddressUsageType.Location = New System.Drawing.Point(16, 70)
        Me.lblAddressUsageType.Name = "lblAddressUsageType"
        Me.lblAddressUsageType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddressUsageType.Size = New System.Drawing.Size(34, 13)
        Me.lblAddressUsageType.TabIndex = 9
        Me.lblAddressUsageType.Text = "Type:"
        '
        'lbAdReference
        '
        Me.lbAdReference.AutoSize = True
        Me.lbAdReference.BackColor = System.Drawing.SystemColors.Control
        Me.lbAdReference.Cursor = System.Windows.Forms.Cursors.Default
        Me.lbAdReference.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbAdReference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbAdReference.Location = New System.Drawing.Point(16, 22)
        Me.lbAdReference.Name = "lbAdReference"
        Me.lbAdReference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lbAdReference.Size = New System.Drawing.Size(60, 13)
        Me.lbAdReference.TabIndex = 6
        Me.lbAdReference.Text = "Reference:"
        '
        'lblAdPostcode
        '
        Me.lblAdPostcode.AutoSize = True
        Me.lblAdPostcode.BackColor = System.Drawing.SystemColors.Control
        Me.lblAdPostcode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAdPostcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdPostcode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAdPostcode.Location = New System.Drawing.Point(16, 46)
        Me.lblAdPostcode.Name = "lblAdPostcode"
        Me.lblAdPostcode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAdPostcode.Size = New System.Drawing.Size(55, 13)
        Me.lblAdPostcode.TabIndex = 5
        Me.lblAdPostcode.Text = "Postcode:"
        '
        'uctAdd
        '
        Me.uctAdd.AddressLine1 = ""
        Me.uctAdd.AddressLine2 = ""
        Me.uctAdd.AddressLine3 = ""
        Me.uctAdd.AddressLine4 = ""
        Me.uctAdd.Caption = ""
        Me.uctAdd.CaptionAddress1 = "No. && street name:"
        Me.uctAdd.CaptionAddress2 = "Locality:"
        Me.uctAdd.CaptionAddress3 = "Town:"
        Me.uctAdd.CaptionAddress4 = "County:"
        Me.uctAdd.CaptionCountry = "Country:"
        Me.uctAdd.CaptionFontBoldAddress1 = False
        Me.uctAdd.CaptionFontBoldPostCode = False
        Me.uctAdd.CaptionPostCode = "Postcode:"
        Me.uctAdd.ClearButtonCaption = "X"
        Me.uctAdd.ClearButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAdd.ClearButtonLeft = 6015
        Me.uctAdd.ClearButtonWidth = 360
        Me.uctAdd.CountryId = 0
        Me.uctAdd.FaceFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAdd.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAdd.IsCountryRequired = 1
        Me.uctAdd.IsPostCodeRequired = 1
        Me.uctAdd.Location = New System.Drawing.Point(10, 104)
        Me.uctAdd.Name = "uctAdd"
        Me.uctAdd.Organisation = ""
        Me.uctAdd.PMAddressCnt = 0
        Me.uctAdd.PMDatabaseID = 9
        Me.uctAdd.PostCode = ""
        Me.uctAdd.QAS2PMAddress1 = "2,12"
        Me.uctAdd.QAS2PMAddress2 = "8"
        Me.uctAdd.QAS2PMAddress3 = "9"
        Me.uctAdd.QAS2PMAddress4 = ""
        Me.uctAdd.QASDatabaseID = 2
        Me.uctAdd.SearchButtonCaption = ".."
        Me.uctAdd.SearchButtonFont = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctAdd.SearchButtonHeight = 285
        Me.uctAdd.SearchButtonLeft = 5580
        Me.uctAdd.SearchButtonTop = 1530
        Me.uctAdd.SearchButtonWidth = 360
        Me.uctAdd.Size = New System.Drawing.Size(441, 156)
        Me.uctAdd.TabIndex = 8
        Me.uctAdd.WarningMessage = ""
        '
        'frmInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(473, 331)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabMainTab)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Address"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class