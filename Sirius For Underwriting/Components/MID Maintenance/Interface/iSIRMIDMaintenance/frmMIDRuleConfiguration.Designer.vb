<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMIDRuleConfiguration
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cboTaskGroup = New System.Windows.Forms.ComboBox()
        Me.cboUserGroup = New System.Windows.Forms.ComboBox()
        Me.cboMIDType = New System.Windows.Forms.ComboBox()
        Me.chkTestIndicator = New System.Windows.Forms.CheckBox()
        Me.lblCurrentFileSequenceNumber = New System.Windows.Forms.Label()
        Me.lblFileSequenceNumberStart = New System.Windows.Forms.Label()
        Me.lblFileName = New System.Windows.Forms.Label()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.lblTaskGroup = New System.Windows.Forms.Label()
        Me.lblUserGroup = New System.Windows.Forms.Label()
        Me.lblSiteNumber = New System.Windows.Forms.Label()
        Me.txtSiteNumber = New System.Windows.Forms.TextBox()
        Me.lblDelegatedAuthorityID = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.txtDelegatedAuthorityID = New System.Windows.Forms.TextBox()
        Me.tabMainTab = New System.Windows.Forms.TabControl()
        Me._tabMainTab_TabPage0 = New System.Windows.Forms.TabPage()
        Me.txtFileSequenceNumberStart = New System.Windows.Forms.TextBox()
        Me.txtCurrentfileSequenceNumber = New System.Windows.Forms.TextBox()
        Me.cboSupplierType = New PMLookupControl.cboPMLookup()
        Me.lblInsurerID = New System.Windows.Forms.Label()
        Me.txtInsurerID = New System.Windows.Forms.TextBox()
        Me.lblSupplierID = New System.Windows.Forms.Label()
        Me.txtSupplierID = New System.Windows.Forms.TextBox()
        Me.lblSupplierType = New System.Windows.Forms.Label()
        Me.lblMIDType = New System.Windows.Forms.Label()
        Me.lblExpiryDate = New System.Windows.Forms.Label()
        Me.txtExpiryDate = New System.Windows.Forms.TextBox()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.txtStartDate = New System.Windows.Forms.TextBox()
        Me.lblEffectiveDate = New System.Windows.Forms.Label()
        Me.txtEffectiveDate = New System.Windows.Forms.TextBox()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.tabMainTab.SuspendLayout()
        Me._tabMainTab_TabPage0.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(507, 455)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Tag = ""
        Me.cmdOK.Text = "Ok"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cboTaskGroup
        '
        Me.cboTaskGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaskGroup.FormattingEnabled = True
        Me.cboTaskGroup.Location = New System.Drawing.Point(139, 306)
        Me.cboTaskGroup.Name = "cboTaskGroup"
        Me.cboTaskGroup.Size = New System.Drawing.Size(180, 21)
        Me.cboTaskGroup.TabIndex = 12
        '
        'cboUserGroup
        '
        Me.cboUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUserGroup.FormattingEnabled = True
        Me.cboUserGroup.Location = New System.Drawing.Point(139, 333)
        Me.cboUserGroup.Name = "cboUserGroup"
        Me.cboUserGroup.Size = New System.Drawing.Size(180, 21)
        Me.cboUserGroup.TabIndex = 13
        '
        'cboMIDType
        '
        Me.cboMIDType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMIDType.FormattingEnabled = True
        Me.cboMIDType.Items.AddRange(New Object() {"MID1", "MID2"})
        Me.cboMIDType.Location = New System.Drawing.Point(139, 150)
        Me.cboMIDType.Name = "cboMIDType"
        Me.cboMIDType.Size = New System.Drawing.Size(178, 21)
        Me.cboMIDType.TabIndex = 5
        '
        'chkTestIndicator
        '
        Me.chkTestIndicator.AutoSize = True
        Me.chkTestIndicator.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkTestIndicator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTestIndicator.Location = New System.Drawing.Point(404, 22)
        Me.chkTestIndicator.Name = "chkTestIndicator"
        Me.chkTestIndicator.Size = New System.Drawing.Size(91, 17)
        Me.chkTestIndicator.TabIndex = 15
        Me.chkTestIndicator.Text = "Test Indicator"
        Me.chkTestIndicator.UseVisualStyleBackColor = True
        '
        'lblCurrentFileSequenceNumber
        '
        Me.lblCurrentFileSequenceNumber.AutoSize = True
        Me.lblCurrentFileSequenceNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrentFileSequenceNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentFileSequenceNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentFileSequenceNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCurrentFileSequenceNumber.Location = New System.Drawing.Point(326, 75)
        Me.lblCurrentFileSequenceNumber.Name = "lblCurrentFileSequenceNumber"
        Me.lblCurrentFileSequenceNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentFileSequenceNumber.Size = New System.Drawing.Size(152, 13)
        Me.lblCurrentFileSequenceNumber.TabIndex = 35
        Me.lblCurrentFileSequenceNumber.Tag = ""
        Me.lblCurrentFileSequenceNumber.Text = "Current File Sequence Number"
        '
        'lblFileSequenceNumberStart
        '
        Me.lblFileSequenceNumberStart.AutoSize = True
        Me.lblFileSequenceNumberStart.BackColor = System.Drawing.SystemColors.Control
        Me.lblFileSequenceNumberStart.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileSequenceNumberStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileSequenceNumberStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFileSequenceNumberStart.Location = New System.Drawing.Point(338, 49)
        Me.lblFileSequenceNumberStart.Name = "lblFileSequenceNumberStart"
        Me.lblFileSequenceNumberStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileSequenceNumberStart.Size = New System.Drawing.Size(140, 13)
        Me.lblFileSequenceNumberStart.TabIndex = 33
        Me.lblFileSequenceNumberStart.Tag = ""
        Me.lblFileSequenceNumberStart.Text = "File Sequence Number Start"
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.BackColor = System.Drawing.SystemColors.Control
        Me.lblFileName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFileName.Location = New System.Drawing.Point(72, 360)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFileName.Size = New System.Drawing.Size(54, 13)
        Me.lblFileName.TabIndex = 31
        Me.lblFileName.Tag = ""
        Me.lblFileName.Text = "File Name"
        '
        'txtFileName
        '
        Me.txtFileName.AcceptsReturn = True
        Me.txtFileName.BackColor = System.Drawing.SystemColors.Window
        Me.txtFileName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFileName.Location = New System.Drawing.Point(139, 360)
        Me.txtFileName.MaxLength = 50
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtFileName.Size = New System.Drawing.Size(178, 20)
        Me.txtFileName.TabIndex = 14
        Me.txtFileName.Tag = ""
        '
        'lblTaskGroup
        '
        Me.lblTaskGroup.AutoSize = True
        Me.lblTaskGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskGroup.Location = New System.Drawing.Point(68, 306)
        Me.lblTaskGroup.Name = "lblTaskGroup"
        Me.lblTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskGroup.Size = New System.Drawing.Size(63, 13)
        Me.lblTaskGroup.TabIndex = 29
        Me.lblTaskGroup.Tag = ""
        Me.lblTaskGroup.Text = "Task Group"
        Me.lblTaskGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblUserGroup
        '
        Me.lblUserGroup.AutoSize = True
        Me.lblUserGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserGroup.Location = New System.Drawing.Point(68, 333)
        Me.lblUserGroup.Name = "lblUserGroup"
        Me.lblUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserGroup.Size = New System.Drawing.Size(61, 13)
        Me.lblUserGroup.TabIndex = 27
        Me.lblUserGroup.Tag = ""
        Me.lblUserGroup.Text = "User Group"
        '
        'lblSiteNumber
        '
        Me.lblSiteNumber.AutoSize = True
        Me.lblSiteNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblSiteNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSiteNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSiteNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSiteNumber.Location = New System.Drawing.Point(68, 283)
        Me.lblSiteNumber.Name = "lblSiteNumber"
        Me.lblSiteNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSiteNumber.Size = New System.Drawing.Size(65, 13)
        Me.lblSiteNumber.TabIndex = 23
        Me.lblSiteNumber.Tag = ""
        Me.lblSiteNumber.Text = "Site Number"
        '
        'txtSiteNumber
        '
        Me.txtSiteNumber.AcceptsReturn = True
        Me.txtSiteNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtSiteNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSiteNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSiteNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSiteNumber.Location = New System.Drawing.Point(139, 280)
        Me.txtSiteNumber.MaxLength = 3
        Me.txtSiteNumber.Name = "txtSiteNumber"
        Me.txtSiteNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSiteNumber.Size = New System.Drawing.Size(180, 20)
        Me.txtSiteNumber.TabIndex = 10
        Me.txtSiteNumber.Tag = ""
        '
        'lblDelegatedAuthorityID
        '
        Me.lblDelegatedAuthorityID.AutoSize = True
        Me.lblDelegatedAuthorityID.BackColor = System.Drawing.SystemColors.Control
        Me.lblDelegatedAuthorityID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDelegatedAuthorityID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDelegatedAuthorityID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDelegatedAuthorityID.Location = New System.Drawing.Point(19, 257)
        Me.lblDelegatedAuthorityID.Name = "lblDelegatedAuthorityID"
        Me.lblDelegatedAuthorityID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDelegatedAuthorityID.Size = New System.Drawing.Size(114, 13)
        Me.lblDelegatedAuthorityID.TabIndex = 21
        Me.lblDelegatedAuthorityID.Tag = ""
        Me.lblDelegatedAuthorityID.Text = "Delegated Authority ID"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(586, 455)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Tag = ""
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'txtDelegatedAuthorityID
        '
        Me.txtDelegatedAuthorityID.AcceptsReturn = True
        Me.txtDelegatedAuthorityID.BackColor = System.Drawing.SystemColors.Window
        Me.txtDelegatedAuthorityID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDelegatedAuthorityID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDelegatedAuthorityID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDelegatedAuthorityID.Location = New System.Drawing.Point(139, 254)
        Me.txtDelegatedAuthorityID.MaxLength = 3
        Me.txtDelegatedAuthorityID.Name = "txtDelegatedAuthorityID"
        Me.txtDelegatedAuthorityID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDelegatedAuthorityID.Size = New System.Drawing.Size(180, 20)
        Me.txtDelegatedAuthorityID.TabIndex = 9
        Me.txtDelegatedAuthorityID.Tag = ""
        '
        'tabMainTab
        '
        Me.tabMainTab.Controls.Add(Me._tabMainTab_TabPage0)
        Me.tabMainTab.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.tabMainTab.Location = New System.Drawing.Point(2, 1)
        Me.tabMainTab.Name = "tabMainTab"
        Me.tabMainTab.SelectedIndex = 0
        Me.tabMainTab.Size = New System.Drawing.Size(686, 440)
        Me.tabMainTab.TabIndex = 0
        '
        '_tabMainTab_TabPage0
        '
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtFileSequenceNumberStart)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCurrentfileSequenceNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboSupplierType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboTaskGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboUserGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.cboMIDType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.chkTestIndicator)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCurrentFileSequenceNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblFileSequenceNumberStart)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblFileName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtFileName)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblTaskGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblUserGroup)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSiteNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtSiteNumber)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDelegatedAuthorityID)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDelegatedAuthorityID)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblInsurerID)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtInsurerID)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSupplierID)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtSupplierID)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblSupplierType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblMIDType)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblExpiryDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtExpiryDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblStartDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtStartDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtEffectiveDate)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtDescription)
        Me._tabMainTab_TabPage0.Controls.Add(Me.lblCode)
        Me._tabMainTab_TabPage0.Controls.Add(Me.txtCode)
        Me._tabMainTab_TabPage0.Location = New System.Drawing.Point(4, 22)
        Me._tabMainTab_TabPage0.Name = "_tabMainTab_TabPage0"
        Me._tabMainTab_TabPage0.Padding = New System.Windows.Forms.Padding(3)
        Me._tabMainTab_TabPage0.Size = New System.Drawing.Size(678, 414)
        Me._tabMainTab_TabPage0.TabIndex = 0
        Me._tabMainTab_TabPage0.Text = "1 - MID Rule"
        Me._tabMainTab_TabPage0.UseVisualStyleBackColor = True
        '
        'txtFileSequenceNumberStart
        '
        Me.txtFileSequenceNumberStart.Location = New System.Drawing.Point(502, 46)
        Me.txtFileSequenceNumberStart.Name = "txtFileSequenceNumberStart"
        Me.txtFileSequenceNumberStart.Size = New System.Drawing.Size(167, 21)
        Me.txtFileSequenceNumberStart.TabIndex = 16
        '
        'txtCurrentfileSequenceNumber
        '
        Me.txtCurrentfileSequenceNumber.Enabled = False
        Me.txtCurrentfileSequenceNumber.Location = New System.Drawing.Point(502, 72)
        Me.txtCurrentfileSequenceNumber.Name = "txtCurrentfileSequenceNumber"
        Me.txtCurrentfileSequenceNumber.Size = New System.Drawing.Size(167, 21)
        Me.txtCurrentfileSequenceNumber.TabIndex = 17
        '
        'cboSupplierType
        '
        Me.cboSupplierType.DefaultItemId = 0
        Me.cboSupplierType.FirstItem = ""
        Me.cboSupplierType.ItemId = 0
        Me.cboSupplierType.ListIndex = -1
        Me.cboSupplierType.Location = New System.Drawing.Point(139, 175)
        Me.cboSupplierType.Name = "cboSupplierType"
        Me.cboSupplierType.PMLookupProductFamily = 1
        Me.cboSupplierType.SingleItemId = 0
        Me.cboSupplierType.Size = New System.Drawing.Size(180, 21)
        Me.cboSupplierType.SortColumnName = ""
        Me.cboSupplierType.Sorted = True
        Me.cboSupplierType.TabIndex = 6
        Me.cboSupplierType.TableName = "Supplier_type"
        Me.cboSupplierType.ToolTipText = ""
        Me.cboSupplierType.WhereClause = ""
        '
        'lblInsurerID
        '
        Me.lblInsurerID.AutoSize = True
        Me.lblInsurerID.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsurerID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsurerID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsurerID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInsurerID.Location = New System.Drawing.Point(80, 231)
        Me.lblInsurerID.Name = "lblInsurerID"
        Me.lblInsurerID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsurerID.Size = New System.Drawing.Size(53, 13)
        Me.lblInsurerID.TabIndex = 19
        Me.lblInsurerID.Tag = ""
        Me.lblInsurerID.Text = "Insurer ID"
        '
        'txtInsurerID
        '
        Me.txtInsurerID.AcceptsReturn = True
        Me.txtInsurerID.BackColor = System.Drawing.SystemColors.Window
        Me.txtInsurerID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtInsurerID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsurerID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtInsurerID.Location = New System.Drawing.Point(139, 228)
        Me.txtInsurerID.MaxLength = 3
        Me.txtInsurerID.Name = "txtInsurerID"
        Me.txtInsurerID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInsurerID.Size = New System.Drawing.Size(180, 20)
        Me.txtInsurerID.TabIndex = 8
        Me.txtInsurerID.Tag = ""
        '
        'lblSupplierID
        '
        Me.lblSupplierID.AutoSize = True
        Me.lblSupplierID.BackColor = System.Drawing.SystemColors.Control
        Me.lblSupplierID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSupplierID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSupplierID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSupplierID.Location = New System.Drawing.Point(74, 204)
        Me.lblSupplierID.Name = "lblSupplierID"
        Me.lblSupplierID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSupplierID.Size = New System.Drawing.Size(59, 13)
        Me.lblSupplierID.TabIndex = 17
        Me.lblSupplierID.Tag = ""
        Me.lblSupplierID.Text = "Supplier ID"
        '
        'txtSupplierID
        '
        Me.txtSupplierID.AcceptsReturn = True
        Me.txtSupplierID.BackColor = System.Drawing.SystemColors.Window
        Me.txtSupplierID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSupplierID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSupplierID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSupplierID.Location = New System.Drawing.Point(139, 202)
        Me.txtSupplierID.MaxLength = 3
        Me.txtSupplierID.Name = "txtSupplierID"
        Me.txtSupplierID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSupplierID.Size = New System.Drawing.Size(180, 20)
        Me.txtSupplierID.TabIndex = 7
        Me.txtSupplierID.Tag = ""
        '
        'lblSupplierType
        '
        Me.lblSupplierType.AutoSize = True
        Me.lblSupplierType.BackColor = System.Drawing.SystemColors.Control
        Me.lblSupplierType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSupplierType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSupplierType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSupplierType.Location = New System.Drawing.Point(61, 179)
        Me.lblSupplierType.Name = "lblSupplierType"
        Me.lblSupplierType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSupplierType.Size = New System.Drawing.Size(72, 13)
        Me.lblSupplierType.TabIndex = 15
        Me.lblSupplierType.Tag = ""
        Me.lblSupplierType.Text = "Supplier Type"
        '
        'lblMIDType
        '
        Me.lblMIDType.AutoSize = True
        Me.lblMIDType.BackColor = System.Drawing.SystemColors.Control
        Me.lblMIDType.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMIDType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMIDType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMIDType.Location = New System.Drawing.Point(59, 153)
        Me.lblMIDType.Name = "lblMIDType"
        Me.lblMIDType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMIDType.Size = New System.Drawing.Size(74, 13)
        Me.lblMIDType.TabIndex = 13
        Me.lblMIDType.Tag = ""
        Me.lblMIDType.Text = "MID1 or MID2"
        '
        'lblExpiryDate
        '
        Me.lblExpiryDate.AutoSize = True
        Me.lblExpiryDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblExpiryDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiryDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExpiryDate.Location = New System.Drawing.Point(72, 127)
        Me.lblExpiryDate.Name = "lblExpiryDate"
        Me.lblExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblExpiryDate.Size = New System.Drawing.Size(61, 13)
        Me.lblExpiryDate.TabIndex = 11
        Me.lblExpiryDate.Tag = ""
        Me.lblExpiryDate.Text = "Expiry Date"
        '
        'txtExpiryDate
        '
        Me.txtExpiryDate.AcceptsReturn = True
        Me.txtExpiryDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtExpiryDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtExpiryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpiryDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExpiryDate.Location = New System.Drawing.Point(139, 124)
        Me.txtExpiryDate.MaxLength = 50
        Me.txtExpiryDate.Name = "txtExpiryDate"
        Me.txtExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtExpiryDate.Size = New System.Drawing.Size(178, 20)
        Me.txtExpiryDate.TabIndex = 4
        Me.txtExpiryDate.Tag = ""
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(78, 101)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(55, 13)
        Me.lblStartDate.TabIndex = 9
        Me.lblStartDate.Tag = ""
        Me.lblStartDate.Text = "Start Date"
        '
        'txtStartDate
        '
        Me.txtStartDate.AcceptsReturn = True
        Me.txtStartDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartDate.Location = New System.Drawing.Point(139, 98)
        Me.txtStartDate.MaxLength = 50
        Me.txtStartDate.Name = "txtStartDate"
        Me.txtStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartDate.Size = New System.Drawing.Size(178, 20)
        Me.txtStartDate.TabIndex = 3
        Me.txtStartDate.Tag = ""
        '
        'lblEffectiveDate
        '
        Me.lblEffectiveDate.AutoSize = True
        Me.lblEffectiveDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEffectiveDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEffectiveDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEffectiveDate.Location = New System.Drawing.Point(58, 72)
        Me.lblEffectiveDate.Name = "lblEffectiveDate"
        Me.lblEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEffectiveDate.Size = New System.Drawing.Size(75, 13)
        Me.lblEffectiveDate.TabIndex = 7
        Me.lblEffectiveDate.Tag = ""
        Me.lblEffectiveDate.Text = "Effective Date"
        '
        'txtEffectiveDate
        '
        Me.txtEffectiveDate.AcceptsReturn = True
        Me.txtEffectiveDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEffectiveDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEffectiveDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEffectiveDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEffectiveDate.Location = New System.Drawing.Point(139, 72)
        Me.txtEffectiveDate.MaxLength = 50
        Me.txtEffectiveDate.Name = "txtEffectiveDate"
        Me.txtEffectiveDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEffectiveDate.Size = New System.Drawing.Size(178, 20)
        Me.txtEffectiveDate.TabIndex = 2
        Me.txtEffectiveDate.Tag = ""
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(73, 49)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(60, 13)
        Me.lblDescription.TabIndex = 5
        Me.lblDescription.Tag = ""
        Me.lblDescription.Text = "Description"
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDescription.Location = New System.Drawing.Point(139, 46)
        Me.txtDescription.MaxLength = 50
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDescription.Size = New System.Drawing.Size(178, 20)
        Me.txtDescription.TabIndex = 1
        Me.txtDescription.Tag = ""
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.SystemColors.Control
        Me.lblCode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCode.Location = New System.Drawing.Point(101, 23)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCode.Size = New System.Drawing.Size(32, 13)
        Me.lblCode.TabIndex = 3
        Me.lblCode.Tag = ""
        Me.lblCode.Text = "Code"
        '
        'txtCode
        '
        Me.txtCode.AcceptsReturn = True
        Me.txtCode.BackColor = System.Drawing.SystemColors.Window
        Me.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCode.Location = New System.Drawing.Point(139, 20)
        Me.txtCode.MaxLength = 10
        Me.txtCode.Name = "txtCode"
        Me.txtCode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCode.Size = New System.Drawing.Size(178, 20)
        Me.txtCode.TabIndex = 0
        Me.txtCode.Tag = ""
        '
        'frmMIDRuleConfiguration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(690, 501)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.tabMainTab)
        Me.MaximizeBox = False
        Me.Name = "frmMIDRuleConfiguration"
        Me.Text = "MID Rule"
        Me.tabMainTab.ResumeLayout(False)
        Me._tabMainTab_TabPage0.ResumeLayout(False)
        Me._tabMainTab_TabPage0.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Private WithEvents cboTaskGroup As System.Windows.Forms.ComboBox
    Private WithEvents cboUserGroup As System.Windows.Forms.ComboBox
    Private WithEvents cboMIDType As System.Windows.Forms.ComboBox
    Private WithEvents chkTestIndicator As System.Windows.Forms.CheckBox
    Public WithEvents lblCurrentFileSequenceNumber As System.Windows.Forms.Label
    Public WithEvents lblFileSequenceNumberStart As System.Windows.Forms.Label
    Public WithEvents lblFileName As System.Windows.Forms.Label
    Public WithEvents txtFileName As System.Windows.Forms.TextBox
    Public WithEvents lblTaskGroup As System.Windows.Forms.Label
    Public WithEvents lblUserGroup As System.Windows.Forms.Label
    Public WithEvents lblSiteNumber As System.Windows.Forms.Label
    Public WithEvents txtSiteNumber As System.Windows.Forms.TextBox
    Public WithEvents lblDelegatedAuthorityID As System.Windows.Forms.Label
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents txtDelegatedAuthorityID As System.Windows.Forms.TextBox
    Private WithEvents tabMainTab As System.Windows.Forms.TabControl
    Private WithEvents _tabMainTab_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents lblInsurerID As System.Windows.Forms.Label
    Public WithEvents txtInsurerID As System.Windows.Forms.TextBox
    Public WithEvents lblSupplierID As System.Windows.Forms.Label
    Public WithEvents txtSupplierID As System.Windows.Forms.TextBox
    Public WithEvents lblSupplierType As System.Windows.Forms.Label
    Public WithEvents lblMIDType As System.Windows.Forms.Label
    Public WithEvents lblExpiryDate As System.Windows.Forms.Label
    Public WithEvents txtExpiryDate As System.Windows.Forms.TextBox
    Public WithEvents lblStartDate As System.Windows.Forms.Label
    Public WithEvents txtStartDate As System.Windows.Forms.TextBox
    Public WithEvents lblEffectiveDate As System.Windows.Forms.Label
    Public WithEvents txtEffectiveDate As System.Windows.Forms.TextBox
    Public WithEvents lblDescription As System.Windows.Forms.Label
    Public WithEvents txtDescription As System.Windows.Forms.TextBox
    Public WithEvents lblCode As System.Windows.Forms.Label
    Public WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents cboSupplierType As PMLookupControl.cboPMLookup
    Friend WithEvents txtFileSequenceNumberStart As System.Windows.Forms.TextBox
    Public WithEvents txtCurrentfileSequenceNumber As System.Windows.Forms.TextBox
End Class
