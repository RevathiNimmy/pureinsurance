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
    Public WithEvents uctCashDepositControl As uctCashDepositControl.CashDeposit
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdNewSearch As System.Windows.Forms.Button
	Public WithEvents cmdFindNow As System.Windows.Forms.Button
	Public WithEvents cboBankName As PMLookupControl.cboPMLookup
	Public WithEvents cmdAgent As System.Windows.Forms.Button
	Public WithEvents cmdClient As System.Windows.Forms.Button
	Public WithEvents txtCashDepositNumber As System.Windows.Forms.TextBox
	Public WithEvents txtAgent As System.Windows.Forms.TextBox
	Public WithEvents txtClient As System.Windows.Forms.TextBox
	Public WithEvents lblBankName As System.Windows.Forms.Label
	Public WithEvents lblDepositNumber As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdNewSearch = New System.Windows.Forms.Button
        Me.cmdFindNow = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.cboBankName = New PMLookupControl.cboPMLookup
        Me.cmdAgent = New System.Windows.Forms.Button
        Me.cmdClient = New System.Windows.Forms.Button
        Me.txtCashDepositNumber = New System.Windows.Forms.TextBox
        Me.txtAgent = New System.Windows.Forms.TextBox
        Me.txtClient = New System.Windows.Forms.TextBox
        Me.lblBankName = New System.Windows.Forms.Label
        Me.lblDepositNumber = New System.Windows.Forms.Label
        Me.uctCashDepositControl = New uctCashDepositControl.CashDeposit
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(546, 532)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(84, 26)
        Me.cmdCancel.TabIndex = 11
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
        Me.cmdOK.Location = New System.Drawing.Point(444, 532)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(84, 26)
        Me.cmdOK.TabIndex = 10
        Me.cmdOK.Text = "OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdNewSearch
        '
        Me.cmdNewSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewSearch.Location = New System.Drawing.Point(561, 65)
        Me.cmdNewSearch.Name = "cmdNewSearch"
        Me.cmdNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewSearch.Size = New System.Drawing.Size(84, 26)
        Me.cmdNewSearch.TabIndex = 9
        Me.cmdNewSearch.Text = "New Search"
        Me.cmdNewSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdNewSearch.UseVisualStyleBackColor = False
        '
        'cmdFindNow
        '
        Me.cmdFindNow.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNow.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNow.Enabled = False
        Me.cmdFindNow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNow.Location = New System.Drawing.Point(561, 25)
        Me.cmdFindNow.Name = "cmdFindNow"
        Me.cmdFindNow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNow.Size = New System.Drawing.Size(84, 26)
        Me.cmdFindNow.TabIndex = 8
        Me.cmdFindNow.Text = "Find Now"
        Me.cmdFindNow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdFindNow.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cboBankName)
        Me.Frame1.Controls.Add(Me.cmdAgent)
        Me.Frame1.Controls.Add(Me.cmdClient)
        Me.Frame1.Controls.Add(Me.txtCashDepositNumber)
        Me.Frame1.Controls.Add(Me.txtAgent)
        Me.Frame1.Controls.Add(Me.txtClient)
        Me.Frame1.Controls.Add(Me.lblBankName)
        Me.Frame1.Controls.Add(Me.lblDepositNumber)
        Me.Frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(24, 16)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(521, 80)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        '
        'cboBankName
        '
        Me.cboBankName.DefaultItemId = 0
        Me.cboBankName.FirstItem = ""
        Me.cboBankName.ItemId = 0
        Me.cboBankName.ListIndex = -1
        Me.cboBankName.Location = New System.Drawing.Point(376, 48)
        Me.cboBankName.Name = "cboBankName"
        Me.cboBankName.PMLookupProductFamily = 1
        Me.cboBankName.SingleItemId = 0
        Me.cboBankName.Size = New System.Drawing.Size(137, 21)
        Me.cboBankName.Sorted = True
        Me.cboBankName.TabIndex = 13
        Me.cboBankName.TableName = "CashListItem_Bank"
        Me.cboBankName.ToolTipText = ""
        Me.cboBankName.WhereClause = ""
        '
        'cmdAgent
        '
        Me.cmdAgent.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAgent.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAgent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAgent.Location = New System.Drawing.Point(11, 49)
        Me.cmdAgent.Name = "cmdAgent"
        Me.cmdAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAgent.Size = New System.Drawing.Size(81, 20)
        Me.cmdAgent.TabIndex = 2
        Me.cmdAgent.Text = "Agent"
        Me.cmdAgent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdAgent.UseVisualStyleBackColor = False
        '
        'cmdClient
        '
        Me.cmdClient.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClient.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClient.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClient.Location = New System.Drawing.Point(10, 20)
        Me.cmdClient.Name = "cmdClient"
        Me.cmdClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClient.Size = New System.Drawing.Size(81, 20)
        Me.cmdClient.TabIndex = 5
        Me.cmdClient.Text = "Client"
        Me.cmdClient.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdClient.UseVisualStyleBackColor = False
        '
        'txtCashDepositNumber
        '
        Me.txtCashDepositNumber.AcceptsReturn = True
        Me.txtCashDepositNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtCashDepositNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCashDepositNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCashDepositNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCashDepositNumber.Location = New System.Drawing.Point(375, 19)
        Me.txtCashDepositNumber.MaxLength = 0
        Me.txtCashDepositNumber.Name = "txtCashDepositNumber"
        Me.txtCashDepositNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCashDepositNumber.Size = New System.Drawing.Size(137, 20)
        Me.txtCashDepositNumber.TabIndex = 4
        '
        'txtAgent
        '
        Me.txtAgent.AcceptsReturn = True
        Me.txtAgent.BackColor = System.Drawing.SystemColors.Window
        Me.txtAgent.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAgent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAgent.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAgent.Location = New System.Drawing.Point(105, 49)
        Me.txtAgent.MaxLength = 0
        Me.txtAgent.Name = "txtAgent"
        Me.txtAgent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAgent.Size = New System.Drawing.Size(137, 20)
        Me.txtAgent.TabIndex = 3
        '
        'txtClient
        '
        Me.txtClient.AcceptsReturn = True
        Me.txtClient.BackColor = System.Drawing.SystemColors.Window
        Me.txtClient.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClient.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClient.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClient.Location = New System.Drawing.Point(105, 19)
        Me.txtClient.MaxLength = 0
        Me.txtClient.Name = "txtClient"
        Me.txtClient.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClient.Size = New System.Drawing.Size(137, 20)
        Me.txtClient.TabIndex = 1
        '
        'lblBankName
        '
        Me.lblBankName.AutoSize = True
        Me.lblBankName.BackColor = System.Drawing.SystemColors.Control
        Me.lblBankName.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBankName.Location = New System.Drawing.Point(260, 50)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBankName.Size = New System.Drawing.Size(63, 13)
        Me.lblBankName.TabIndex = 6
        Me.lblBankName.Text = "Bank Name"
        '
        'lblDepositNumber
        '
        Me.lblDepositNumber.AutoSize = True
        Me.lblDepositNumber.BackColor = System.Drawing.SystemColors.Control
        Me.lblDepositNumber.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDepositNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDepositNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDepositNumber.Location = New System.Drawing.Point(260, 20)
        Me.lblDepositNumber.Name = "lblDepositNumber"
        Me.lblDepositNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDepositNumber.Size = New System.Drawing.Size(110, 13)
        Me.lblDepositNumber.TabIndex = 7
        Me.lblDepositNumber.Text = "Cash Deposit Number"
        '
        'uctCashDepositControl
        '
        Me.uctCashDepositControl.BankCode = ""
        Me.uctCashDepositControl.CashDepositRef = ""
        Me.uctCashDepositControl.FindCashDepositRef = ""
        Me.uctCashDepositControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uctCashDepositControl.FromAgentOrClientMaintenance = False
        Me.uctCashDepositControl.IsClient = False
        Me.uctCashDepositControl.IsFind = False
        Me.uctCashDepositControl.Location = New System.Drawing.Point(8, 112)
        Me.uctCashDepositControl.Name = "uctCashDepositControl"
        Me.uctCashDepositControl.PartyCnt = 0
        Me.uctCashDepositControl.PartyCode = ""
        Me.uctCashDepositControl.PartyName = ""
        Me.uctCashDepositControl.Size = New System.Drawing.Size(649, 417)
        Me.uctCashDepositControl.TabIndex = 12
        Me.uctCashDepositControl.Task = 0
        '
        'frmInterface
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(659, 573)
        Me.Controls.Add(Me.uctCashDepositControl)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdNewSearch)
        Me.Controls.Add(Me.cmdFindNow)
        Me.Controls.Add(Me.Frame1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Find Cash Deposit Account"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class