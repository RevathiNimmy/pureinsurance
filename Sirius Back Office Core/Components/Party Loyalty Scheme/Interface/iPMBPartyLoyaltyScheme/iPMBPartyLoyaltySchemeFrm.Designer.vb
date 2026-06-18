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
	Public WithEvents cboPMLookupLoyaltyScheme As PMLookupControl.cboPMLookup
	Public WithEvents txtEndDate As System.Windows.Forms.TextBox
	Public WithEvents txtOtherRef As System.Windows.Forms.TextBox
	Public WithEvents txtMainMemberNumber As System.Windows.Forms.TextBox
	Public WithEvents txtMemberNumber As System.Windows.Forms.TextBox
	Public WithEvents txtStartDate As System.Windows.Forms.TextBox
	Public WithEvents chkActive As System.Windows.Forms.CheckBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents lblEndDate As System.Windows.Forms.Label
	Public WithEvents lblOtherRef As System.Windows.Forms.Label
	Public WithEvents lblMainMemberNumber As System.Windows.Forms.Label
	Public WithEvents lblLoyaltyScheme As System.Windows.Forms.Label
	Public WithEvents lblStartDate As System.Windows.Forms.Label
	Public WithEvents lblMemberNumber As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboPMLookupLoyaltyScheme = New PMLookupControl.cboPMLookup
        Me.txtEndDate = New System.Windows.Forms.TextBox
        Me.txtOtherRef = New System.Windows.Forms.TextBox
        Me.txtMainMemberNumber = New System.Windows.Forms.TextBox
        Me.txtMemberNumber = New System.Windows.Forms.TextBox
        Me.txtStartDate = New System.Windows.Forms.TextBox
        Me.chkActive = New System.Windows.Forms.CheckBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.lblEndDate = New System.Windows.Forms.Label
        Me.lblOtherRef = New System.Windows.Forms.Label
        Me.lblMainMemberNumber = New System.Windows.Forms.Label
        Me.lblLoyaltyScheme = New System.Windows.Forms.Label
        Me.lblStartDate = New System.Windows.Forms.Label
        Me.lblMemberNumber = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cboPMLookupLoyaltyScheme
        '
        Me.cboPMLookupLoyaltyScheme.DefaultItemId = 0
        Me.cboPMLookupLoyaltyScheme.FirstItem = ""
        Me.cboPMLookupLoyaltyScheme.ItemId = 0
        Me.cboPMLookupLoyaltyScheme.ListIndex = -1
        Me.cboPMLookupLoyaltyScheme.Location = New System.Drawing.Point(200, 16)
        Me.cboPMLookupLoyaltyScheme.Name = "cboPMLookupLoyaltyScheme"
        Me.cboPMLookupLoyaltyScheme.PMLookupProductFamily = 1
        Me.cboPMLookupLoyaltyScheme.SingleItemId = 0
        Me.cboPMLookupLoyaltyScheme.Size = New System.Drawing.Size(241, 21)
        Me.cboPMLookupLoyaltyScheme.Sorted = True
        Me.cboPMLookupLoyaltyScheme.TabIndex = 0
        Me.cboPMLookupLoyaltyScheme.TableName = "Loyalty_Scheme"
        Me.cboPMLookupLoyaltyScheme.Tag = "F;"
        Me.cboPMLookupLoyaltyScheme.ToolTipText = ""
        Me.cboPMLookupLoyaltyScheme.WhereClause = ""
        '
        'txtEndDate
        '
        Me.txtEndDate.AcceptsReturn = True
        Me.txtEndDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEndDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEndDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEndDate.Location = New System.Drawing.Point(200, 144)
        Me.txtEndDate.MaxLength = 0
        Me.txtEndDate.Name = "txtEndDate"
        Me.txtEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEndDate.Size = New System.Drawing.Size(169, 20)
        Me.txtEndDate.TabIndex = 4
        Me.txtEndDate.Tag = "F;FMT;DT"
        '
        'txtOtherRef
        '
        Me.txtOtherRef.AcceptsReturn = True
        Me.txtOtherRef.BackColor = System.Drawing.SystemColors.Window
        Me.txtOtherRef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOtherRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOtherRef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOtherRef.Location = New System.Drawing.Point(200, 80)
        Me.txtOtherRef.MaxLength = 0
        Me.txtOtherRef.Name = "txtOtherRef"
        Me.txtOtherRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOtherRef.Size = New System.Drawing.Size(241, 20)
        Me.txtOtherRef.TabIndex = 2
        Me.txtOtherRef.Tag = "F;"
        '
        'txtMainMemberNumber
        '
        Me.txtMainMemberNumber.AcceptsReturn = True
        Me.txtMainMemberNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtMainMemberNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMainMemberNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMainMemberNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMainMemberNumber.Location = New System.Drawing.Point(200, 176)
        Me.txtMainMemberNumber.MaxLength = 0
        Me.txtMainMemberNumber.Name = "txtMainMemberNumber"
        Me.txtMainMemberNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMainMemberNumber.Size = New System.Drawing.Size(241, 20)
        Me.txtMainMemberNumber.TabIndex = 5
        Me.txtMainMemberNumber.Tag = "F;"
        '
        'txtMemberNumber
        '
        Me.txtMemberNumber.AcceptsReturn = True
        Me.txtMemberNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtMemberNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMemberNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMemberNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMemberNumber.Location = New System.Drawing.Point(200, 48)
        Me.txtMemberNumber.MaxLength = 0
        Me.txtMemberNumber.Name = "txtMemberNumber"
        Me.txtMemberNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMemberNumber.Size = New System.Drawing.Size(241, 20)
        Me.txtMemberNumber.TabIndex = 1
        Me.txtMemberNumber.Tag = "F;"
        '
        'txtStartDate
        '
        Me.txtStartDate.AcceptsReturn = True
        Me.txtStartDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartDate.Location = New System.Drawing.Point(200, 112)
        Me.txtStartDate.MaxLength = 0
        Me.txtStartDate.Name = "txtStartDate"
        Me.txtStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartDate.Size = New System.Drawing.Size(169, 20)
        Me.txtStartDate.TabIndex = 3
        Me.txtStartDate.Tag = "F;FMT;DT"
        '
        'chkActive
        '
        Me.chkActive.BackColor = System.Drawing.SystemColors.Control
        Me.chkActive.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkActive.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkActive.Location = New System.Drawing.Point(200, 208)
        Me.chkActive.Name = "chkActive"
        Me.chkActive.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkActive.Size = New System.Drawing.Size(93, 17)
        Me.chkActive.TabIndex = 6
        Me.chkActive.Tag = "CAP;108;F;"
        Me.chkActive.Text = "{Active}"
        Me.chkActive.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(368, 248)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Tag = "CAP;201"
        Me.cmdCancel.Text = "{&Cancel}"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(288, 248)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 7
        Me.cmdOK.Tag = "CAP;200"
        Me.cmdOK.Text = "{&OK}"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'lblEndDate
        '
        Me.lblEndDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEndDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEndDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEndDate.Location = New System.Drawing.Point(16, 144)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEndDate.Size = New System.Drawing.Size(89, 17)
        Me.lblEndDate.TabIndex = 14
        Me.lblEndDate.Tag = "CAP;106"
        Me.lblEndDate.Text = "{End Date:}"
        '
        'lblOtherRef
        '
        Me.lblOtherRef.BackColor = System.Drawing.SystemColors.Control
        Me.lblOtherRef.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOtherRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOtherRef.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOtherRef.Location = New System.Drawing.Point(16, 80)
        Me.lblOtherRef.Name = "lblOtherRef"
        Me.lblOtherRef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOtherRef.Size = New System.Drawing.Size(177, 17)
        Me.lblOtherRef.TabIndex = 13
        Me.lblOtherRef.Tag = "CAP;104"
        Me.lblOtherRef.Text = "{Other Reference:}"
        '
        'lblMainMemberNumber
        '
        Me.lblMainMemberNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblMainMemberNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMainMemberNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMainMemberNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMainMemberNumber.Location = New System.Drawing.Point(16, 176)
        Me.lblMainMemberNumber.Name = "lblMainMemberNumber"
        Me.lblMainMemberNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMainMemberNumber.Size = New System.Drawing.Size(177, 17)
        Me.lblMainMemberNumber.TabIndex = 12
        Me.lblMainMemberNumber.Tag = "CAP;107"
        Me.lblMainMemberNumber.Text = "{Main Member:}"
        '
        'lblLoyaltyScheme
        '
        Me.lblLoyaltyScheme.BackColor = System.Drawing.SystemColors.Control
        Me.lblLoyaltyScheme.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoyaltyScheme.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoyaltyScheme.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLoyaltyScheme.Location = New System.Drawing.Point(16, 16)
        Me.lblLoyaltyScheme.Name = "lblLoyaltyScheme"
        Me.lblLoyaltyScheme.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoyaltyScheme.Size = New System.Drawing.Size(121, 17)
        Me.lblLoyaltyScheme.TabIndex = 11
        Me.lblLoyaltyScheme.Tag = "CAP;102"
        Me.lblLoyaltyScheme.Text = "{Loyalty Scheme:}"
        '
        'lblStartDate
        '
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(16, 112)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(89, 17)
        Me.lblStartDate.TabIndex = 10
        Me.lblStartDate.Tag = "CAP;105"
        Me.lblStartDate.Text = "{Start Date:}"
        '
        'lblMemberNumber
        '
        Me.lblMemberNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblMemberNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMemberNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMemberNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMemberNumber.Location = New System.Drawing.Point(16, 48)
        Me.lblMemberNumber.Name = "lblMemberNumber"
        Me.lblMemberNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMemberNumber.Size = New System.Drawing.Size(177, 17)
        Me.lblMemberNumber.TabIndex = 9
        Me.lblMemberNumber.Tag = "CAP;103"
        Me.lblMemberNumber.Text = "{Membership Number:}"
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(465, 282)
        Me.Controls.Add(Me.cboPMLookupLoyaltyScheme)
        Me.Controls.Add(Me.txtEndDate)
        Me.Controls.Add(Me.txtOtherRef)
        Me.Controls.Add(Me.txtMainMemberNumber)
        Me.Controls.Add(Me.txtMemberNumber)
        Me.Controls.Add(Me.txtStartDate)
        Me.Controls.Add(Me.chkActive)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.lblEndDate)
        Me.Controls.Add(Me.lblOtherRef)
        Me.Controls.Add(Me.lblMainMemberNumber)
        Me.Controls.Add(Me.lblLoyaltyScheme)
        Me.Controls.Add(Me.lblStartDate)
        Me.Controls.Add(Me.lblMemberNumber)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(203, 163)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Tag = "CAP;100"
        Me.Text = "{Loyalty Scheme Membership}"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class