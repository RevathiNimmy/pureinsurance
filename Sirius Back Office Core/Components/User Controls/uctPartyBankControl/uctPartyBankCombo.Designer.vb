<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class uctPartyBankCombo
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		UserControl_InitProperties()
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
	Friend WithEvents cboAccountType As System.Windows.Forms.ComboBox
	Friend WithEvents cmdEditPaymentType As System.Windows.Forms.Button
	Friend WithEvents cmdAddPaymentType As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(uctPartyBankCombo))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cboAccountType = New System.Windows.Forms.ComboBox
		Me.cmdEditPaymentType = New System.Windows.Forms.Button
		Me.cmdAddPaymentType = New System.Windows.Forms.Button
		Me.SuspendLayout()
		' 
		' cboAccountType
		' 
		Me.cboAccountType.BackColor = System.Drawing.SystemColors.Window
		Me.cboAccountType.CausesValidation = True
		Me.cboAccountType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboAccountType.Enabled = True
		Me.cboAccountType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboAccountType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboAccountType.IntegralHeight = True
		Me.cboAccountType.Location = New System.Drawing.Point(4, 4)
		Me.cboAccountType.Name = "cboAccountType"
		Me.cboAccountType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboAccountType.Size = New System.Drawing.Size(221, 21)
		Me.cboAccountType.Sorted = False
		Me.cboAccountType.TabIndex = 2
		Me.cboAccountType.TabStop = True
		Me.cboAccountType.Visible = True
		' 
		' cmdEditPaymentType
		' 
		Me.cmdEditPaymentType.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEditPaymentType.CausesValidation = True
		Me.cmdEditPaymentType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEditPaymentType.Enabled = True
		Me.cmdEditPaymentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEditPaymentType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEditPaymentType.Location = New System.Drawing.Point(300, 6)
		Me.cmdEditPaymentType.Name = "cmdEditPaymentType"
		Me.cmdEditPaymentType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEditPaymentType.Size = New System.Drawing.Size(69, 19)
		Me.cmdEditPaymentType.TabIndex = 1
		Me.cmdEditPaymentType.TabStop = True
		Me.cmdEditPaymentType.Text = "&Edit"
		Me.cmdEditPaymentType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEditPaymentType.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAddPaymentType
		' 
		Me.cmdAddPaymentType.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAddPaymentType.CausesValidation = True
		Me.cmdAddPaymentType.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAddPaymentType.Enabled = True
		Me.cmdAddPaymentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAddPaymentType.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAddPaymentType.Location = New System.Drawing.Point(230, 6)
		Me.cmdAddPaymentType.Name = "cmdAddPaymentType"
		Me.cmdAddPaymentType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAddPaymentType.Size = New System.Drawing.Size(69, 19)
		Me.cmdAddPaymentType.TabIndex = 0
		Me.cmdAddPaymentType.TabStop = True
		Me.cmdAddPaymentType.Text = "&Add New"
		Me.cmdAddPaymentType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAddPaymentType.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' uctPartyBankCombo
		' 
		Me.ClientSize = New System.Drawing.Size(379, 29)
		Me.Controls.Add(Me.cboAccountType)
		Me.Controls.Add(Me.cmdEditPaymentType)
		Me.Controls.Add(Me.cmdAddPaymentType)
		MyBase.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		MyBase.Location = New System.Drawing.Point(0, 0)
		MyBase.Name = "uctPartyBankCombo"
		Me.ResumeLayout(False)
	End Sub
#End Region 
#Region "Upgrade Support"
	<System.Runtime.InteropServices.ProgId("EditPartyBankItemEventArgs_NET.EditPartyBankItemEventArgs")> _
	Public NotInheritable Class EditPartyBankItemEventArgs
		Inherits System.EventArgs
		Public vBankDetails As Object
		Public Sub New(ByVal vBankDetails As Object)
			MyBase.New()
			Me.vBankDetails = vBankDetails
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("AddPartyBankItemEventArgs_NET.AddPartyBankItemEventArgs")> _
	Public NotInheritable Class AddPartyBankItemEventArgs
		Inherits System.EventArgs
		Public vBankDetails As Object
		Public Sub New(ByVal vBankDetails As Object)
			MyBase.New()
			Me.vBankDetails = vBankDetails
		End Sub
	End Class
	<System.Runtime.InteropServices.ProgId("ComboChangeEventArgs_NET.ComboChangeEventArgs")> _
	Public NotInheritable Class ComboChangeEventArgs
		Inherits System.EventArgs
		Public lSelItemID As Integer
		Public Sub New(ByRef lSelItemID As Integer)
			MyBase.New()
			Me.lSelItemID = lSelItemID
		End Sub
	End Class
#End Region 
End Class