<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStep
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
    Public WithEvents cboWriteOffReason As PMLookupControl.cboPMLookup
    Public WithEvents txtOutstandingBalanceWriteOffToleranceAmount As System.Windows.Forms.TextBox
    Public WithEvents lblWriteOffReason As System.Windows.Forms.Label
    Public WithEvents lblOutstandingBalanceWriteOffTolerance As System.Windows.Forms.Label
    Public WithEvents fraWriteOffTolerance As System.Windows.Forms.GroupBox
    Public WithEvents cboAutoCancellationDocument2 As System.Windows.Forms.ComboBox
    Public WithEvents txtAutoCancelDoc2Trigger As System.Windows.Forms.TextBox
    Public WithEvents cboAutoCancellationDocument1 As System.Windows.Forms.ComboBox
    Public WithEvents txtAutoCancelDoc1Trigger As System.Windows.Forms.TextBox
    Public WithEvents fraOutstandingBalanceMinimum As System.Windows.Forms.Label
    Public WithEvents lblDocument1 As System.Windows.Forms.Label
    Public WithEvents fraAutoCancellationDocuments As System.Windows.Forms.GroupBox
    Public WithEvents chkCheckAutoCancel As System.Windows.Forms.CheckBox
    Public WithEvents chkRunAutoCancel As System.Windows.Forms.CheckBox
    Public WithEvents chkAutoLapseRenewal As System.Windows.Forms.CheckBox
    Public WithEvents chkStopAccount As System.Windows.Forms.CheckBox
    Public WithEvents fraAutoCancellationActions As System.Windows.Forms.GroupBox
    Public WithEvents cboInstalmentFailureCount As System.Windows.Forms.ComboBox
    Public WithEvents lblInstalmentFailureCount As System.Windows.Forms.Label
    Public WithEvents fraTriggers As System.Windows.Forms.GroupBox
    Public WithEvents chkJumpToNextStep As System.Windows.Forms.CheckBox
	Public WithEvents cboClientLetter As System.Windows.Forms.ComboBox
	Public WithEvents txtElapsedDays As System.Windows.Forms.TextBox
	Public WithEvents txtPolicyAmt As System.Windows.Forms.TextBox
	Public WithEvents lblPolicy As System.Windows.Forms.Label
	Public WithEvents fraTolerance As System.Windows.Forms.GroupBox
	Public WithEvents lblClientLetter As System.Windows.Forms.Label
	Public WithEvents lblElapsedDays As System.Windows.Forms.Label
	Public WithEvents fraDirectCustomers As System.Windows.Forms.GroupBox
	Public WithEvents txtStepDescription As System.Windows.Forms.TextBox
	Public WithEvents cboUserGroup As System.Windows.Forms.ComboBox
	Public WithEvents cboTask As System.Windows.Forms.ComboBox
	Public WithEvents cboTaskGroup As System.Windows.Forms.ComboBox
	Public WithEvents cboPMLookupUserGroup As PMLookupControl.cboPMLookup
	Public WithEvents cboPMLookupWrkTask As PMLookupControl.cboPMLookup
	Public WithEvents cboPMLookupActionType As PMLookupControl.cboPMLookup
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblAllocateToUserGroup As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents lblTaskGroup As System.Windows.Forms.Label
	Public WithEvents lblActiontype As System.Windows.Forms.Label
	Public WithEvents lblUserGroup As System.Windows.Forms.Label
	Public WithEvents lblTask As System.Windows.Forms.Label
	Public WithEvents fraOther As System.Windows.Forms.GroupBox
	Public WithEvents txtTolPercent2 As System.Windows.Forms.TextBox
	Public WithEvents txtTolPercent1 As System.Windows.Forms.TextBox
	Public WithEvents lblTolePercent2 As System.Windows.Forms.Label
	Public WithEvents lblTolePercent1 As System.Windows.Forms.Label
	Public WithEvents fraTolerancePercentages As System.Windows.Forms.GroupBox
	Public WithEvents txtRecurringDays As System.Windows.Forms.TextBox
	Public WithEvents chkReprint As System.Windows.Forms.CheckBox
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents fraRecurring As System.Windows.Forms.GroupBox
	Public WithEvents txtOffHoldStep As System.Windows.Forms.TextBox
	Public WithEvents txtPreviousStep As System.Windows.Forms.TextBox
	Public WithEvents txtNextStep As System.Windows.Forms.TextBox
	Public WithEvents txtStep As System.Windows.Forms.TextBox
	Public WithEvents lblOffHold As System.Windows.Forms.Label
	Public WithEvents lblPreviousStep As System.Windows.Forms.Label
	Public WithEvents lblNextStep As System.Windows.Forms.Label
	Public WithEvents lblStep As System.Windows.Forms.Label
	Public WithEvents fraStepOrder As System.Windows.Forms.GroupBox
	Public WithEvents txtHelper As System.Windows.Forms.TextBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cboPMLookupUserGroup2 As PMLookupControl.cboPMLookup
	Public WithEvents cboPMLookupWrkTask2 As PMLookupControl.cboPMLookup
	Public WithEvents cboPMLookupActionType2 As PMLookupControl.cboPMLookup
	Public WithEvents lblTask2 As System.Windows.Forms.Label
	Public WithEvents lblUserGroup2 As System.Windows.Forms.Label
	Public WithEvents lblActiontype2 As System.Windows.Forms.Label
	Public WithEvents fraOther2 As System.Windows.Forms.GroupBox
	Public WithEvents cboClientLetter2 As System.Windows.Forms.ComboBox
	Public WithEvents cboOIPLetter2 As System.Windows.Forms.ComboBox
	Public WithEvents lblOIPLetter2 As System.Windows.Forms.Label
	Public WithEvents lblClientLetter2 As System.Windows.Forms.Label
	Public WithEvents fraPrint2 As System.Windows.Forms.GroupBox
	Public WithEvents cboPMLookupBrokerReport As PMLookupControl.cboPMLookup
	Public WithEvents cboOIPLetter As System.Windows.Forms.ComboBox
	Public WithEvents lblBrokerReport As System.Windows.Forms.Label
	Public WithEvents lblOIPLetter As System.Windows.Forms.Label
	Public WithEvents fraPrint As System.Windows.Forms.GroupBox
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraAutoCancellationActions = New System.Windows.Forms.GroupBox
        Me.fraWriteOffTolerance = New System.Windows.Forms.GroupBox
        Me.cboWriteOffReason = New PMLookupControl.cboPMLookup
        Me.txtOutstandingBalanceWriteOffToleranceAmount = New System.Windows.Forms.TextBox
        Me.lblWriteOffReason = New System.Windows.Forms.Label
        Me.lblOutstandingBalanceWriteOffTolerance = New System.Windows.Forms.Label
        Me.fraAutoCancellationDocuments = New System.Windows.Forms.GroupBox
        Me.cboAutoCancellationDocument2 = New System.Windows.Forms.ComboBox
        Me.txtAutoCancelDoc2Trigger = New System.Windows.Forms.TextBox
        Me.cboAutoCancellationDocument1 = New System.Windows.Forms.ComboBox
        Me.txtAutoCancelDoc1Trigger = New System.Windows.Forms.TextBox
        Me.fraOutstandingBalanceMinimum = New System.Windows.Forms.Label
        Me.lblDocument1 = New System.Windows.Forms.Label
        Me.chkCheckAutoCancel = New System.Windows.Forms.CheckBox
        Me.chkRunAutoCancel = New System.Windows.Forms.CheckBox
        Me.chkAutoLapseRenewal = New System.Windows.Forms.CheckBox
        Me.chkStopAccount = New System.Windows.Forms.CheckBox
        Me.fraTriggers = New System.Windows.Forms.GroupBox
        Me.cboInstalmentFailureCount = New System.Windows.Forms.ComboBox
        Me.lblInstalmentFailureCount = New System.Windows.Forms.Label
        Me.fraDirectCustomers = New System.Windows.Forms.GroupBox
        Me.chkJumpToNextStep = New System.Windows.Forms.CheckBox
        Me.cboClientLetter = New System.Windows.Forms.ComboBox
        Me.txtElapsedDays = New System.Windows.Forms.TextBox
        Me.fraTolerance = New System.Windows.Forms.GroupBox
        Me.txtPolicyAmt = New System.Windows.Forms.TextBox
        Me.lblPolicy = New System.Windows.Forms.Label
        Me.lblClientLetter = New System.Windows.Forms.Label
        Me.lblElapsedDays = New System.Windows.Forms.Label
        Me.fraOther = New System.Windows.Forms.GroupBox
        Me.txtStepDescription = New System.Windows.Forms.TextBox
        Me.cboUserGroup = New System.Windows.Forms.ComboBox
        Me.cboTask = New System.Windows.Forms.ComboBox
        Me.cboTaskGroup = New System.Windows.Forms.ComboBox
        Me.cboPMLookupUserGroup = New PMLookupControl.cboPMLookup
        Me.cboPMLookupWrkTask = New PMLookupControl.cboPMLookup
        Me.cboPMLookupActionType = New PMLookupControl.cboPMLookup
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblAllocateToUserGroup = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblTaskGroup = New System.Windows.Forms.Label
        Me.lblActiontype = New System.Windows.Forms.Label
        Me.lblUserGroup = New System.Windows.Forms.Label
        Me.lblTask = New System.Windows.Forms.Label
        Me.fraTolerancePercentages = New System.Windows.Forms.GroupBox
        Me.txtTolPercent2 = New System.Windows.Forms.TextBox
        Me.txtTolPercent1 = New System.Windows.Forms.TextBox
        Me.lblTolePercent2 = New System.Windows.Forms.Label
        Me.lblTolePercent1 = New System.Windows.Forms.Label
        Me.fraRecurring = New System.Windows.Forms.GroupBox
        Me.txtRecurringDays = New System.Windows.Forms.TextBox
        Me.chkReprint = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.fraStepOrder = New System.Windows.Forms.GroupBox
        Me.txtOffHoldStep = New System.Windows.Forms.TextBox
        Me.txtPreviousStep = New System.Windows.Forms.TextBox
        Me.txtNextStep = New System.Windows.Forms.TextBox
        Me.txtStep = New System.Windows.Forms.TextBox
        Me.lblOffHold = New System.Windows.Forms.Label
        Me.lblPreviousStep = New System.Windows.Forms.Label
        Me.lblNextStep = New System.Windows.Forms.Label
        Me.lblStep = New System.Windows.Forms.Label
        Me.txtHelper = New System.Windows.Forms.TextBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.fraOther2 = New System.Windows.Forms.GroupBox
        Me.cboPMLookupUserGroup2 = New PMLookupControl.cboPMLookup
        Me.cboPMLookupWrkTask2 = New PMLookupControl.cboPMLookup
        Me.cboPMLookupActionType2 = New PMLookupControl.cboPMLookup
        Me.lblTask2 = New System.Windows.Forms.Label
        Me.lblUserGroup2 = New System.Windows.Forms.Label
        Me.lblActiontype2 = New System.Windows.Forms.Label
        Me.fraPrint2 = New System.Windows.Forms.GroupBox
        Me.cboClientLetter2 = New System.Windows.Forms.ComboBox
        Me.cboOIPLetter2 = New System.Windows.Forms.ComboBox
        Me.lblOIPLetter2 = New System.Windows.Forms.Label
        Me.lblClientLetter2 = New System.Windows.Forms.Label
        Me.fraPrint = New System.Windows.Forms.GroupBox
        Me.cboPMLookupBrokerReport = New PMLookupControl.cboPMLookup
        Me.cboOIPLetter = New System.Windows.Forms.ComboBox
        Me.lblBrokerReport = New System.Windows.Forms.Label
        Me.lblOIPLetter = New System.Windows.Forms.Label
        Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
        Me.ssTabBroker = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.chkJumptonextstepBroker = New System.Windows.Forms.CheckBox
        Me.fraToleranceBroker = New System.Windows.Forms.GroupBox
        Me.txtAccountAmt = New System.Windows.Forms.TextBox
        Me.lblAccount = New System.Windows.Forms.Label
        Me.cboBrokerLetter = New System.Windows.Forms.ComboBox
        Me.lblBrokerLetter = New System.Windows.Forms.Label
        Me.txtBrokerDays = New System.Windows.Forms.TextBox
        Me.lblBroketrDays = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.chkJumptonextstepBrokerSingleInst = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtAccountAmtSingleInst = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cboBrokerLetterSingleInst = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtBrokerDaysSingleInst = New System.Windows.Forms.TextBox
        Me.lblBrokerDaysSingleInst = New System.Windows.Forms.Label
        Me.fraAutoCancellationActions.SuspendLayout()
        Me.fraWriteOffTolerance.SuspendLayout()
        Me.fraAutoCancellationDocuments.SuspendLayout()
        Me.fraTriggers.SuspendLayout()
        Me.fraDirectCustomers.SuspendLayout()
        Me.fraTolerance.SuspendLayout()
        Me.fraOther.SuspendLayout()
        Me.fraTolerancePercentages.SuspendLayout()
        Me.fraRecurring.SuspendLayout()
        Me.fraStepOrder.SuspendLayout()
        Me.fraOther2.SuspendLayout()
        Me.fraPrint2.SuspendLayout()
        Me.fraPrint.SuspendLayout()
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ssTabBroker.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.fraToleranceBroker.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraAutoCancellationActions
        '
        Me.fraAutoCancellationActions.BackColor = System.Drawing.SystemColors.Control
        Me.fraAutoCancellationActions.Controls.Add(Me.fraWriteOffTolerance)
        Me.fraAutoCancellationActions.Controls.Add(Me.fraAutoCancellationDocuments)
        Me.fraAutoCancellationActions.Controls.Add(Me.chkCheckAutoCancel)
        Me.fraAutoCancellationActions.Controls.Add(Me.chkRunAutoCancel)
        Me.fraAutoCancellationActions.Controls.Add(Me.chkAutoLapseRenewal)
        Me.fraAutoCancellationActions.Controls.Add(Me.chkStopAccount)
        Me.fraAutoCancellationActions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAutoCancellationActions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAutoCancellationActions.Location = New System.Drawing.Point(8, 284)
        Me.fraAutoCancellationActions.Name = "fraAutoCancellationActions"
        Me.fraAutoCancellationActions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAutoCancellationActions.Size = New System.Drawing.Size(649, 49)
        Me.fraAutoCancellationActions.TabIndex = 29
        Me.fraAutoCancellationActions.TabStop = False
        Me.fraAutoCancellationActions.Text = "Auto Cancellation"
        '
        'fraWriteOffTolerance
        '
        Me.fraWriteOffTolerance.BackColor = System.Drawing.SystemColors.Control
        Me.fraWriteOffTolerance.Controls.Add(Me.cboWriteOffReason)
        Me.fraWriteOffTolerance.Controls.Add(Me.txtOutstandingBalanceWriteOffToleranceAmount)
        Me.fraWriteOffTolerance.Controls.Add(Me.lblWriteOffReason)
        Me.fraWriteOffTolerance.Controls.Add(Me.lblOutstandingBalanceWriteOffTolerance)
        Me.fraWriteOffTolerance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraWriteOffTolerance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraWriteOffTolerance.Location = New System.Drawing.Point(8, 48)
        Me.fraWriteOffTolerance.Name = "fraWriteOffTolerance"
        Me.fraWriteOffTolerance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraWriteOffTolerance.Size = New System.Drawing.Size(633, 49)
        Me.fraWriteOffTolerance.TabIndex = 33
        Me.fraWriteOffTolerance.TabStop = False
        Me.fraWriteOffTolerance.Text = "Write off any outstanding balance if the amount is within the specified tolerance" & _
    " ( + OR - ) "
        '
        'cboWriteOffReason
        '
        Me.cboWriteOffReason.DefaultItemId = 0
        Me.cboWriteOffReason.FirstItem = "(None)"
        Me.cboWriteOffReason.ItemId = 0
        Me.cboWriteOffReason.ListIndex = -1
        Me.cboWriteOffReason.Location = New System.Drawing.Point(384, 17)
        Me.cboWriteOffReason.Name = "cboWriteOffReason"
        Me.cboWriteOffReason.PMLookupProductFamily = 1
        Me.cboWriteOffReason.SingleItemId = 0
        Me.cboWriteOffReason.Size = New System.Drawing.Size(241, 21)
        Me.cboWriteOffReason.Sorted = True
        Me.cboWriteOffReason.TabIndex = 37
        Me.cboWriteOffReason.TableName = "Write_Off_Reason"
        Me.cboWriteOffReason.ToolTipText = ""
        Me.cboWriteOffReason.WhereClause = ""
        '
        'txtOutstandingBalanceWriteOffToleranceAmount
        '
        Me.txtOutstandingBalanceWriteOffToleranceAmount.AcceptsReturn = True
        Me.txtOutstandingBalanceWriteOffToleranceAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtOutstandingBalanceWriteOffToleranceAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOutstandingBalanceWriteOffToleranceAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutstandingBalanceWriteOffToleranceAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOutstandingBalanceWriteOffToleranceAmount.Location = New System.Drawing.Point(120, 18)
        Me.txtOutstandingBalanceWriteOffToleranceAmount.MaxLength = 0
        Me.txtOutstandingBalanceWriteOffToleranceAmount.Name = "txtOutstandingBalanceWriteOffToleranceAmount"
        Me.txtOutstandingBalanceWriteOffToleranceAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOutstandingBalanceWriteOffToleranceAmount.Size = New System.Drawing.Size(145, 20)
        Me.txtOutstandingBalanceWriteOffToleranceAmount.TabIndex = 35
        '
        'lblWriteOffReason
        '
        Me.lblWriteOffReason.AutoSize = True
        Me.lblWriteOffReason.BackColor = System.Drawing.SystemColors.Control
        Me.lblWriteOffReason.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWriteOffReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWriteOffReason.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblWriteOffReason.Location = New System.Drawing.Point(280, 21)
        Me.lblWriteOffReason.Name = "lblWriteOffReason"
        Me.lblWriteOffReason.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWriteOffReason.Size = New System.Drawing.Size(92, 13)
        Me.lblWriteOffReason.TabIndex = 36
        Me.lblWriteOffReason.Text = "Write Off Reason:"
        '
        'lblOutstandingBalanceWriteOffTolerance
        '
        Me.lblOutstandingBalanceWriteOffTolerance.AutoSize = True
        Me.lblOutstandingBalanceWriteOffTolerance.BackColor = System.Drawing.SystemColors.Control
        Me.lblOutstandingBalanceWriteOffTolerance.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOutstandingBalanceWriteOffTolerance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutstandingBalanceWriteOffTolerance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOutstandingBalanceWriteOffTolerance.Location = New System.Drawing.Point(8, 21)
        Me.lblOutstandingBalanceWriteOffTolerance.Name = "lblOutstandingBalanceWriteOffTolerance"
        Me.lblOutstandingBalanceWriteOffTolerance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOutstandingBalanceWriteOffTolerance.Size = New System.Drawing.Size(97, 13)
        Me.lblOutstandingBalanceWriteOffTolerance.TabIndex = 34
        Me.lblOutstandingBalanceWriteOffTolerance.Text = "Tolerance Amount:"
        '
        'fraAutoCancellationDocuments
        '
        Me.fraAutoCancellationDocuments.BackColor = System.Drawing.SystemColors.Control
        Me.fraAutoCancellationDocuments.Controls.Add(Me.cboAutoCancellationDocument2)
        Me.fraAutoCancellationDocuments.Controls.Add(Me.txtAutoCancelDoc2Trigger)
        Me.fraAutoCancellationDocuments.Controls.Add(Me.cboAutoCancellationDocument1)
        Me.fraAutoCancellationDocuments.Controls.Add(Me.txtAutoCancelDoc1Trigger)
        Me.fraAutoCancellationDocuments.Controls.Add(Me.fraOutstandingBalanceMinimum)
        Me.fraAutoCancellationDocuments.Controls.Add(Me.lblDocument1)
        Me.fraAutoCancellationDocuments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraAutoCancellationDocuments.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraAutoCancellationDocuments.Location = New System.Drawing.Point(8, 96)
        Me.fraAutoCancellationDocuments.Name = "fraAutoCancellationDocuments"
        Me.fraAutoCancellationDocuments.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraAutoCancellationDocuments.Size = New System.Drawing.Size(633, 73)
        Me.fraAutoCancellationDocuments.TabIndex = 38
        Me.fraAutoCancellationDocuments.TabStop = False
        Me.fraAutoCancellationDocuments.Text = "On auto cancellation generate the relevant letter"
        '
        'cboAutoCancellationDocument2
        '
        Me.cboAutoCancellationDocument2.BackColor = System.Drawing.SystemColors.Window
        Me.cboAutoCancellationDocument2.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAutoCancellationDocument2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAutoCancellationDocument2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAutoCancellationDocument2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAutoCancellationDocument2.Location = New System.Drawing.Point(288, 43)
        Me.cboAutoCancellationDocument2.Name = "cboAutoCancellationDocument2"
        Me.cboAutoCancellationDocument2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAutoCancellationDocument2.Size = New System.Drawing.Size(337, 21)
        Me.cboAutoCancellationDocument2.TabIndex = 44
        '
        'txtAutoCancelDoc2Trigger
        '
        Me.txtAutoCancelDoc2Trigger.AcceptsReturn = True
        Me.txtAutoCancelDoc2Trigger.BackColor = System.Drawing.SystemColors.Window
        Me.txtAutoCancelDoc2Trigger.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAutoCancelDoc2Trigger.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAutoCancelDoc2Trigger.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAutoCancelDoc2Trigger.Location = New System.Drawing.Point(160, 43)
        Me.txtAutoCancelDoc2Trigger.MaxLength = 0
        Me.txtAutoCancelDoc2Trigger.Name = "txtAutoCancelDoc2Trigger"
        Me.txtAutoCancelDoc2Trigger.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAutoCancelDoc2Trigger.Size = New System.Drawing.Size(121, 20)
        Me.txtAutoCancelDoc2Trigger.TabIndex = 43
        '
        'cboAutoCancellationDocument1
        '
        Me.cboAutoCancellationDocument1.BackColor = System.Drawing.SystemColors.Window
        Me.cboAutoCancellationDocument1.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboAutoCancellationDocument1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAutoCancellationDocument1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAutoCancellationDocument1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboAutoCancellationDocument1.Location = New System.Drawing.Point(288, 16)
        Me.cboAutoCancellationDocument1.Name = "cboAutoCancellationDocument1"
        Me.cboAutoCancellationDocument1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboAutoCancellationDocument1.Size = New System.Drawing.Size(337, 21)
        Me.cboAutoCancellationDocument1.TabIndex = 41
        '
        'txtAutoCancelDoc1Trigger
        '
        Me.txtAutoCancelDoc1Trigger.AcceptsReturn = True
        Me.txtAutoCancelDoc1Trigger.BackColor = System.Drawing.SystemColors.Window
        Me.txtAutoCancelDoc1Trigger.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAutoCancelDoc1Trigger.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAutoCancelDoc1Trigger.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAutoCancelDoc1Trigger.Location = New System.Drawing.Point(160, 16)
        Me.txtAutoCancelDoc1Trigger.MaxLength = 0
        Me.txtAutoCancelDoc1Trigger.Name = "txtAutoCancelDoc1Trigger"
        Me.txtAutoCancelDoc1Trigger.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAutoCancelDoc1Trigger.Size = New System.Drawing.Size(121, 20)
        Me.txtAutoCancelDoc1Trigger.TabIndex = 40
        '
        'fraOutstandingBalanceMinimum
        '
        Me.fraOutstandingBalanceMinimum.AutoSize = True
        Me.fraOutstandingBalanceMinimum.BackColor = System.Drawing.SystemColors.Control
        Me.fraOutstandingBalanceMinimum.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraOutstandingBalanceMinimum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOutstandingBalanceMinimum.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOutstandingBalanceMinimum.Location = New System.Drawing.Point(8, 47)
        Me.fraOutstandingBalanceMinimum.Name = "fraOutstandingBalanceMinimum"
        Me.fraOutstandingBalanceMinimum.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOutstandingBalanceMinimum.Size = New System.Drawing.Size(130, 13)
        Me.fraOutstandingBalanceMinimum.TabIndex = 42
        Me.fraOutstandingBalanceMinimum.Text = "If Outstanding Balance <="
        '
        'lblDocument1
        '
        Me.lblDocument1.AutoSize = True
        Me.lblDocument1.BackColor = System.Drawing.SystemColors.Control
        Me.lblDocument1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDocument1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocument1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocument1.Location = New System.Drawing.Point(8, 20)
        Me.lblDocument1.Name = "lblDocument1"
        Me.lblDocument1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDocument1.Size = New System.Drawing.Size(127, 13)
        Me.lblDocument1.TabIndex = 39
        Me.lblDocument1.Text = "If Oustanding Balance >="
        '
        'chkCheckAutoCancel
        '
        Me.chkCheckAutoCancel.BackColor = System.Drawing.SystemColors.Control
        Me.chkCheckAutoCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCheckAutoCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCheckAutoCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCheckAutoCancel.Location = New System.Drawing.Point(8, 16)
        Me.chkCheckAutoCancel.Name = "chkCheckAutoCancel"
        Me.chkCheckAutoCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkCheckAutoCancel.Size = New System.Drawing.Size(197, 29)
        Me.chkCheckAutoCancel.TabIndex = 30
        Me.chkCheckAutoCancel.Tag = "CAP;522"
        Me.chkCheckAutoCancel.Text = "*{Check Auto-Cancel Rules}"
        Me.chkCheckAutoCancel.UseVisualStyleBackColor = False
        '
        'chkRunAutoCancel
        '
        Me.chkRunAutoCancel.BackColor = System.Drawing.SystemColors.Control
        Me.chkRunAutoCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkRunAutoCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRunAutoCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRunAutoCancel.Location = New System.Drawing.Point(241, 16)
        Me.chkRunAutoCancel.Name = "chkRunAutoCancel"
        Me.chkRunAutoCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkRunAutoCancel.Size = New System.Drawing.Size(183, 29)
        Me.chkRunAutoCancel.TabIndex = 31
        Me.chkRunAutoCancel.Tag = "CAP;523"
        Me.chkRunAutoCancel.Text = "*{Run Auto-Cancel Rules}"
        Me.chkRunAutoCancel.UseVisualStyleBackColor = False
        '
        'chkAutoLapseRenewal
        '
        Me.chkAutoLapseRenewal.BackColor = System.Drawing.SystemColors.Control
        Me.chkAutoLapseRenewal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAutoLapseRenewal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoLapseRenewal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAutoLapseRenewal.Location = New System.Drawing.Point(24, 16)
        Me.chkAutoLapseRenewal.Name = "chkAutoLapseRenewal"
        Me.chkAutoLapseRenewal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAutoLapseRenewal.Size = New System.Drawing.Size(197, 29)
        Me.chkAutoLapseRenewal.TabIndex = 89
        Me.chkAutoLapseRenewal.Tag = "CAP;730"
        Me.chkAutoLapseRenewal.Text = "*{Auto Lapse Renewal}"
        Me.chkAutoLapseRenewal.UseVisualStyleBackColor = False
        '
        'chkStopAccount
        '
        Me.chkStopAccount.BackColor = System.Drawing.SystemColors.Control
        Me.chkStopAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkStopAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkStopAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkStopAccount.Location = New System.Drawing.Point(456, 16)
        Me.chkStopAccount.Name = "chkStopAccount"
        Me.chkStopAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkStopAccount.Size = New System.Drawing.Size(169, 29)
        Me.chkStopAccount.TabIndex = 32
        Me.chkStopAccount.Tag = "CAP;728"
        Me.chkStopAccount.Text = "Stop Account"
        Me.chkStopAccount.UseVisualStyleBackColor = False
        '
        'fraTriggers
        '
        Me.fraTriggers.BackColor = System.Drawing.SystemColors.Control
        Me.fraTriggers.Controls.Add(Me.cboInstalmentFailureCount)
        Me.fraTriggers.Controls.Add(Me.lblInstalmentFailureCount)
        Me.fraTriggers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTriggers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTriggers.Location = New System.Drawing.Point(288, 104)
        Me.fraTriggers.Name = "fraTriggers"
        Me.fraTriggers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTriggers.Size = New System.Drawing.Size(233, 49)
        Me.fraTriggers.TabIndex = 10
        Me.fraTriggers.TabStop = False
        Me.fraTriggers.Text = "Step Triggers"
        '
        'cboInstalmentFailureCount
        '
        Me.cboInstalmentFailureCount.BackColor = System.Drawing.SystemColors.Window
        Me.cboInstalmentFailureCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboInstalmentFailureCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboInstalmentFailureCount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboInstalmentFailureCount.Location = New System.Drawing.Point(168, 18)
        Me.cboInstalmentFailureCount.Name = "cboInstalmentFailureCount"
        Me.cboInstalmentFailureCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboInstalmentFailureCount.Size = New System.Drawing.Size(57, 21)
        Me.cboInstalmentFailureCount.TabIndex = 12
        Me.cboInstalmentFailureCount.Text = "Combo1"
        '
        'lblInstalmentFailureCount
        '
        Me.lblInstalmentFailureCount.AutoSize = True
        Me.lblInstalmentFailureCount.BackColor = System.Drawing.SystemColors.Control
        Me.lblInstalmentFailureCount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInstalmentFailureCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstalmentFailureCount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInstalmentFailureCount.Location = New System.Drawing.Point(16, 21)
        Me.lblInstalmentFailureCount.Name = "lblInstalmentFailureCount"
        Me.lblInstalmentFailureCount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInstalmentFailureCount.Size = New System.Drawing.Size(123, 13)
        Me.lblInstalmentFailureCount.TabIndex = 11
        Me.lblInstalmentFailureCount.Text = "Instalment Failure Count:"
        '
        'fraDirectCustomers
        '
        Me.fraDirectCustomers.BackColor = System.Drawing.SystemColors.Control
        Me.fraDirectCustomers.Controls.Add(Me.chkJumpToNextStep)
        Me.fraDirectCustomers.Controls.Add(Me.cboClientLetter)
        Me.fraDirectCustomers.Controls.Add(Me.txtElapsedDays)
        Me.fraDirectCustomers.Controls.Add(Me.fraTolerance)
        Me.fraDirectCustomers.Controls.Add(Me.lblClientLetter)
        Me.fraDirectCustomers.Controls.Add(Me.lblElapsedDays)
        Me.fraDirectCustomers.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDirectCustomers.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDirectCustomers.Location = New System.Drawing.Point(8, 60)
        Me.fraDirectCustomers.Name = "fraDirectCustomers"
        Me.fraDirectCustomers.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDirectCustomers.Size = New System.Drawing.Size(649, 97)
        Me.fraDirectCustomers.TabIndex = 13
        Me.fraDirectCustomers.TabStop = False
        Me.fraDirectCustomers.Text = "Direct Business"
        '
        'chkJumpToNextStep
        '
        Me.chkJumpToNextStep.BackColor = System.Drawing.SystemColors.Control
        Me.chkJumpToNextStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkJumpToNextStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkJumpToNextStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkJumpToNextStep.Location = New System.Drawing.Point(18, 44)
        Me.chkJumpToNextStep.Name = "chkJumpToNextStep"
        Me.chkJumpToNextStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkJumpToNextStep.Size = New System.Drawing.Size(161, 21)
        Me.chkJumpToNextStep.TabIndex = 16
        Me.chkJumpToNextStep.Tag = "CAP;512"
        Me.chkJumpToNextStep.Text = "*{Jump To Next Step}"
        Me.chkJumpToNextStep.UseVisualStyleBackColor = False
        '
        'cboClientLetter
        '
        Me.cboClientLetter.BackColor = System.Drawing.SystemColors.Window
        Me.cboClientLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClientLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboClientLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClientLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboClientLetter.Location = New System.Drawing.Point(304, 18)
        Me.cboClientLetter.Name = "cboClientLetter"
        Me.cboClientLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClientLetter.Size = New System.Drawing.Size(329, 21)
        Me.cboClientLetter.TabIndex = 15
        '
        'txtElapsedDays
        '
        Me.txtElapsedDays.AcceptsReturn = True
        Me.txtElapsedDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtElapsedDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtElapsedDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtElapsedDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtElapsedDays.Location = New System.Drawing.Point(126, 18)
        Me.txtElapsedDays.MaxLength = 2
        Me.txtElapsedDays.Name = "txtElapsedDays"
        Me.txtElapsedDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtElapsedDays.Size = New System.Drawing.Size(65, 20)
        Me.txtElapsedDays.TabIndex = 18
        '
        'fraTolerance
        '
        Me.fraTolerance.BackColor = System.Drawing.SystemColors.Control
        Me.fraTolerance.Controls.Add(Me.txtPolicyAmt)
        Me.fraTolerance.Controls.Add(Me.lblPolicy)
        Me.fraTolerance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTolerance.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTolerance.Location = New System.Drawing.Point(7, 44)
        Me.fraTolerance.Name = "fraTolerance"
        Me.fraTolerance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTolerance.Size = New System.Drawing.Size(252, 47)
        Me.fraTolerance.TabIndex = 71
        Me.fraTolerance.TabStop = False
        Me.fraTolerance.Tag = "CAP;514"
        Me.fraTolerance.Text = "*{Tolerance Amount}"
        '
        'txtPolicyAmt
        '
        Me.txtPolicyAmt.AcceptsReturn = True
        Me.txtPolicyAmt.BackColor = System.Drawing.SystemColors.Window
        Me.txtPolicyAmt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPolicyAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPolicyAmt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPolicyAmt.Location = New System.Drawing.Point(112, 16)
        Me.txtPolicyAmt.MaxLength = 50
        Me.txtPolicyAmt.Name = "txtPolicyAmt"
        Me.txtPolicyAmt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPolicyAmt.Size = New System.Drawing.Size(129, 20)
        Me.txtPolicyAmt.TabIndex = 20
        Me.txtPolicyAmt.Tag = "FMT;$"
        '
        'lblPolicy
        '
        Me.lblPolicy.AutoSize = True
        Me.lblPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.lblPolicy.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPolicy.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPolicy.Location = New System.Drawing.Point(48, 18)
        Me.lblPolicy.Name = "lblPolicy"
        Me.lblPolicy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPolicy.Size = New System.Drawing.Size(50, 13)
        Me.lblPolicy.TabIndex = 19
        Me.lblPolicy.Tag = "CAP;515"
        Me.lblPolicy.Text = "*{Policy:}"
        Me.lblPolicy.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblClientLetter
        '
        Me.lblClientLetter.AutoSize = True
        Me.lblClientLetter.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientLetter.Location = New System.Drawing.Point(202, 21)
        Me.lblClientLetter.Name = "lblClientLetter"
        Me.lblClientLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientLetter.Size = New System.Drawing.Size(78, 13)
        Me.lblClientLetter.TabIndex = 14
        Me.lblClientLetter.Tag = "CAP;518"
        Me.lblClientLetter.Text = "*{Client Letter:}"
        '
        'lblElapsedDays
        '
        Me.lblElapsedDays.AutoSize = True
        Me.lblElapsedDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblElapsedDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblElapsedDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblElapsedDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblElapsedDays.Location = New System.Drawing.Point(18, 21)
        Me.lblElapsedDays.Name = "lblElapsedDays"
        Me.lblElapsedDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblElapsedDays.Size = New System.Drawing.Size(87, 13)
        Me.lblElapsedDays.TabIndex = 17
        Me.lblElapsedDays.Tag = "CAP;509"
        Me.lblElapsedDays.Text = "*{Elapsed Days:}"
        '
        'fraOther
        '
        Me.fraOther.BackColor = System.Drawing.SystemColors.Control
        Me.fraOther.Controls.Add(Me.txtStepDescription)
        Me.fraOther.Controls.Add(Me.cboUserGroup)
        Me.fraOther.Controls.Add(Me.cboTask)
        Me.fraOther.Controls.Add(Me.cboTaskGroup)
        Me.fraOther.Controls.Add(Me.cboPMLookupUserGroup)
        Me.fraOther.Controls.Add(Me.cboPMLookupWrkTask)
        Me.fraOther.Controls.Add(Me.cboPMLookupActionType)
        Me.fraOther.Controls.Add(Me.lblDescription)
        Me.fraOther.Controls.Add(Me.lblAllocateToUserGroup)
        Me.fraOther.Controls.Add(Me.Label3)
        Me.fraOther.Controls.Add(Me.lblTaskGroup)
        Me.fraOther.Controls.Add(Me.lblActiontype)
        Me.fraOther.Controls.Add(Me.lblUserGroup)
        Me.fraOther.Controls.Add(Me.lblTask)
        Me.fraOther.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOther.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOther.Location = New System.Drawing.Point(8, 336)
        Me.fraOther.Name = "fraOther"
        Me.fraOther.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOther.Size = New System.Drawing.Size(649, 109)
        Me.fraOther.TabIndex = 45
        Me.fraOther.TabStop = False
        Me.fraOther.Tag = "CAP;521"
        Me.fraOther.Text = "Create Task"
        '
        'txtStepDescription
        '
        Me.txtStepDescription.AcceptsReturn = True
        Me.txtStepDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtStepDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStepDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStepDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStepDescription.Location = New System.Drawing.Point(96, 78)
        Me.txtStepDescription.MaxLength = 255
        Me.txtStepDescription.Name = "txtStepDescription"
        Me.txtStepDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStepDescription.Size = New System.Drawing.Size(537, 20)
        Me.txtStepDescription.TabIndex = 53
        '
        'cboUserGroup
        '
        Me.cboUserGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUserGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboUserGroup.Location = New System.Drawing.Point(96, 47)
        Me.cboUserGroup.Name = "cboUserGroup"
        Me.cboUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboUserGroup.Size = New System.Drawing.Size(241, 21)
        Me.cboUserGroup.TabIndex = 51
        '
        'cboTask
        '
        Me.cboTask.BackColor = System.Drawing.SystemColors.Window
        Me.cboTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTask.Enabled = False
        Me.cboTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTask.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTask.Location = New System.Drawing.Point(392, 16)
        Me.cboTask.Name = "cboTask"
        Me.cboTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTask.Size = New System.Drawing.Size(241, 21)
        Me.cboTask.TabIndex = 49
        '
        'cboTaskGroup
        '
        Me.cboTaskGroup.BackColor = System.Drawing.SystemColors.Window
        Me.cboTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTaskGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTaskGroup.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTaskGroup.Location = New System.Drawing.Point(96, 16)
        Me.cboTaskGroup.Name = "cboTaskGroup"
        Me.cboTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboTaskGroup.Size = New System.Drawing.Size(241, 21)
        Me.cboTaskGroup.TabIndex = 47
        '
        'cboPMLookupUserGroup
        '
        Me.cboPMLookupUserGroup.DefaultItemId = 0
        Me.cboPMLookupUserGroup.FirstItem = "("""")"
        Me.cboPMLookupUserGroup.ItemId = 0
        Me.cboPMLookupUserGroup.ListIndex = -1
        Me.cboPMLookupUserGroup.Location = New System.Drawing.Point(160, 160)
        Me.cboPMLookupUserGroup.Name = "cboPMLookupUserGroup"
        Me.cboPMLookupUserGroup.PMLookupProductFamily = 1
        Me.cboPMLookupUserGroup.SingleItemId = 0
        Me.cboPMLookupUserGroup.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupUserGroup.Sorted = True
        Me.cboPMLookupUserGroup.TabIndex = 85
        Me.cboPMLookupUserGroup.TableName = "PMUser_Group"
        Me.cboPMLookupUserGroup.TabStop = False
        Me.cboPMLookupUserGroup.ToolTipText = ""
        Me.cboPMLookupUserGroup.WhereClause = ""
        '
        'cboPMLookupWrkTask
        '
        Me.cboPMLookupWrkTask.DefaultItemId = 0
        Me.cboPMLookupWrkTask.FirstItem = "("""")"
        Me.cboPMLookupWrkTask.ItemId = 0
        Me.cboPMLookupWrkTask.ListIndex = -1
        Me.cboPMLookupWrkTask.Location = New System.Drawing.Point(160, 160)
        Me.cboPMLookupWrkTask.Name = "cboPMLookupWrkTask"
        Me.cboPMLookupWrkTask.PMLookupProductFamily = 2
        Me.cboPMLookupWrkTask.SingleItemId = 0
        Me.cboPMLookupWrkTask.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupWrkTask.Sorted = True
        Me.cboPMLookupWrkTask.TabIndex = 86
        Me.cboPMLookupWrkTask.TableName = "PMWrk_Task"
        Me.cboPMLookupWrkTask.TabStop = False
        Me.cboPMLookupWrkTask.ToolTipText = ""
        Me.cboPMLookupWrkTask.WhereClause = ""
        '
        'cboPMLookupActionType
        '
        Me.cboPMLookupActionType.DefaultItemId = 0
        Me.cboPMLookupActionType.FirstItem = "("""")"
        Me.cboPMLookupActionType.ItemId = 0
        Me.cboPMLookupActionType.ListIndex = -1
        Me.cboPMLookupActionType.Location = New System.Drawing.Point(160, 160)
        Me.cboPMLookupActionType.Name = "cboPMLookupActionType"
        Me.cboPMLookupActionType.PMLookupProductFamily = 1
        Me.cboPMLookupActionType.SingleItemId = 0
        Me.cboPMLookupActionType.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupActionType.Sorted = True
        Me.cboPMLookupActionType.TabIndex = 84
        Me.cboPMLookupActionType.TableName = "PMWrk_Task_Action_type"
        Me.cboPMLookupActionType.TabStop = False
        Me.cboPMLookupActionType.ToolTipText = ""
        Me.cboPMLookupActionType.WhereClause = ""
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(17, 80)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 52
        Me.lblDescription.Text = "Description:"
        '
        'lblAllocateToUserGroup
        '
        Me.lblAllocateToUserGroup.AutoSize = True
        Me.lblAllocateToUserGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblAllocateToUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAllocateToUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAllocateToUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAllocateToUserGroup.Location = New System.Drawing.Point(16, 51)
        Me.lblAllocateToUserGroup.Name = "lblAllocateToUserGroup"
        Me.lblAllocateToUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAllocateToUserGroup.Size = New System.Drawing.Size(64, 13)
        Me.lblAllocateToUserGroup.TabIndex = 50
        Me.lblAllocateToUserGroup.Text = "User Group:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(352, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 48
        Me.Label3.Text = "Task:"
        '
        'lblTaskGroup
        '
        Me.lblTaskGroup.AutoSize = True
        Me.lblTaskGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblTaskGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTaskGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTaskGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTaskGroup.Location = New System.Drawing.Point(16, 20)
        Me.lblTaskGroup.Name = "lblTaskGroup"
        Me.lblTaskGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTaskGroup.Size = New System.Drawing.Size(66, 13)
        Me.lblTaskGroup.TabIndex = 46
        Me.lblTaskGroup.Text = "Task Group:"
        '
        'lblActiontype
        '
        Me.lblActiontype.AutoSize = True
        Me.lblActiontype.BackColor = System.Drawing.SystemColors.Control
        Me.lblActiontype.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblActiontype.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActiontype.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActiontype.Location = New System.Drawing.Point(8, 160)
        Me.lblActiontype.Name = "lblActiontype"
        Me.lblActiontype.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblActiontype.Size = New System.Drawing.Size(79, 13)
        Me.lblActiontype.TabIndex = 83
        Me.lblActiontype.Tag = "CAP;525"
        Me.lblActiontype.Text = "*{Action Type:}"
        '
        'lblUserGroup
        '
        Me.lblUserGroup.AutoSize = True
        Me.lblUserGroup.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserGroup.Location = New System.Drawing.Point(8, 160)
        Me.lblUserGroup.Name = "lblUserGroup"
        Me.lblUserGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserGroup.Size = New System.Drawing.Size(94, 13)
        Me.lblUserGroup.TabIndex = 82
        Me.lblUserGroup.Tag = "CAP;525"
        Me.lblUserGroup.Text = "*{For User Group:}"
        '
        'lblTask
        '
        Me.lblTask.AutoSize = True
        Me.lblTask.BackColor = System.Drawing.SystemColors.Control
        Me.lblTask.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTask.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTask.Location = New System.Drawing.Point(8, 160)
        Me.lblTask.Name = "lblTask"
        Me.lblTask.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTask.Size = New System.Drawing.Size(153, 13)
        Me.lblTask.TabIndex = 87
        Me.lblTask.Tag = "CAP;524"
        Me.lblTask.Text = "*{Create Work manager Task:}"
        '
        'fraTolerancePercentages
        '
        Me.fraTolerancePercentages.BackColor = System.Drawing.SystemColors.Control
        Me.fraTolerancePercentages.Controls.Add(Me.txtTolPercent2)
        Me.fraTolerancePercentages.Controls.Add(Me.txtTolPercent1)
        Me.fraTolerancePercentages.Controls.Add(Me.lblTolePercent2)
        Me.fraTolerancePercentages.Controls.Add(Me.lblTolePercent1)
        Me.fraTolerancePercentages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTolerancePercentages.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTolerancePercentages.Location = New System.Drawing.Point(8, 106)
        Me.fraTolerancePercentages.Name = "fraTolerancePercentages"
        Me.fraTolerancePercentages.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTolerancePercentages.Size = New System.Drawing.Size(649, 49)
        Me.fraTolerancePercentages.TabIndex = 72
        Me.fraTolerancePercentages.TabStop = False
        Me.fraTolerancePercentages.Tag = "CAP;514"
        Me.fraTolerancePercentages.Text = "*{Tolerance Percentages}"
        '
        'txtTolPercent2
        '
        Me.txtTolPercent2.AcceptsReturn = True
        Me.txtTolPercent2.BackColor = System.Drawing.SystemColors.Window
        Me.txtTolPercent2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTolPercent2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTolPercent2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTolPercent2.Location = New System.Drawing.Point(568, 16)
        Me.txtTolPercent2.MaxLength = 50
        Me.txtTolPercent2.Name = "txtTolPercent2"
        Me.txtTolPercent2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTolPercent2.Size = New System.Drawing.Size(73, 20)
        Me.txtTolPercent2.TabIndex = 74
        Me.txtTolPercent2.Tag = "FMT;$"
        '
        'txtTolPercent1
        '
        Me.txtTolPercent1.AcceptsReturn = True
        Me.txtTolPercent1.BackColor = System.Drawing.SystemColors.Window
        Me.txtTolPercent1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTolPercent1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTolPercent1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTolPercent1.Location = New System.Drawing.Point(248, 16)
        Me.txtTolPercent1.MaxLength = 50
        Me.txtTolPercent1.Name = "txtTolPercent1"
        Me.txtTolPercent1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTolPercent1.Size = New System.Drawing.Size(65, 20)
        Me.txtTolPercent1.TabIndex = 73
        Me.txtTolPercent1.Tag = "FMT;$"
        '
        'lblTolePercent2
        '
        Me.lblTolePercent2.BackColor = System.Drawing.SystemColors.Control
        Me.lblTolePercent2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTolePercent2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTolePercent2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTolePercent2.Location = New System.Drawing.Point(344, 24)
        Me.lblTolePercent2.Name = "lblTolePercent2"
        Me.lblTolePercent2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTolePercent2.Size = New System.Drawing.Size(193, 17)
        Me.lblTolePercent2.TabIndex = 76
        Me.lblTolePercent2.Tag = "CAP;515"
        Me.lblTolePercent2.Text = "*{Sub-step 2 (less than):}"
        '
        'lblTolePercent1
        '
        Me.lblTolePercent1.BackColor = System.Drawing.SystemColors.Control
        Me.lblTolePercent1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTolePercent1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTolePercent1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTolePercent1.Location = New System.Drawing.Point(8, 24)
        Me.lblTolePercent1.Name = "lblTolePercent1"
        Me.lblTolePercent1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTolePercent1.Size = New System.Drawing.Size(193, 17)
        Me.lblTolePercent1.TabIndex = 75
        Me.lblTolePercent1.Tag = "CAP;515"
        Me.lblTolePercent1.Text = "*{Sub-step 1 (less than):}"
        '
        'fraRecurring
        '
        Me.fraRecurring.BackColor = System.Drawing.SystemColors.Control
        Me.fraRecurring.Controls.Add(Me.txtRecurringDays)
        Me.fraRecurring.Controls.Add(Me.chkReprint)
        Me.fraRecurring.Controls.Add(Me.Label1)
        Me.fraRecurring.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRecurring.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraRecurring.Location = New System.Drawing.Point(8, 449)
        Me.fraRecurring.Name = "fraRecurring"
        Me.fraRecurring.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraRecurring.Size = New System.Drawing.Size(649, 49)
        Me.fraRecurring.TabIndex = 54
        Me.fraRecurring.TabStop = False
        Me.fraRecurring.Tag = "CAP;526"
        Me.fraRecurring.Text = "*{Recurring}"
        '
        'txtRecurringDays
        '
        Me.txtRecurringDays.AcceptsReturn = True
        Me.txtRecurringDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtRecurringDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRecurringDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRecurringDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRecurringDays.Location = New System.Drawing.Point(244, 20)
        Me.txtRecurringDays.MaxLength = 2
        Me.txtRecurringDays.Name = "txtRecurringDays"
        Me.txtRecurringDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRecurringDays.Size = New System.Drawing.Size(49, 20)
        Me.txtRecurringDays.TabIndex = 56
        '
        'chkReprint
        '
        Me.chkReprint.BackColor = System.Drawing.SystemColors.Control
        Me.chkReprint.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkReprint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReprint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkReprint.Location = New System.Drawing.Point(364, 17)
        Me.chkReprint.Name = "chkReprint"
        Me.chkReprint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkReprint.Size = New System.Drawing.Size(161, 25)
        Me.chkReprint.TabIndex = 57
        Me.chkReprint.Tag = "CAP;528"
        Me.chkReprint.Text = "*{Reprint Letters}"
        Me.chkReprint.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(116, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(121, 17)
        Me.Label1.TabIndex = 55
        Me.Label1.Tag = "CAP;527"
        Me.Label1.Text = "*{Recurring Days:}"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'fraStepOrder
        '
        Me.fraStepOrder.BackColor = System.Drawing.SystemColors.Control
        Me.fraStepOrder.Controls.Add(Me.txtOffHoldStep)
        Me.fraStepOrder.Controls.Add(Me.txtPreviousStep)
        Me.fraStepOrder.Controls.Add(Me.txtNextStep)
        Me.fraStepOrder.Controls.Add(Me.txtStep)
        Me.fraStepOrder.Controls.Add(Me.lblOffHold)
        Me.fraStepOrder.Controls.Add(Me.lblPreviousStep)
        Me.fraStepOrder.Controls.Add(Me.lblNextStep)
        Me.fraStepOrder.Controls.Add(Me.lblStep)
        Me.fraStepOrder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraStepOrder.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraStepOrder.Location = New System.Drawing.Point(8, 6)
        Me.fraStepOrder.Name = "fraStepOrder"
        Me.fraStepOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraStepOrder.Size = New System.Drawing.Size(649, 49)
        Me.fraStepOrder.TabIndex = 1
        Me.fraStepOrder.TabStop = False
        Me.fraStepOrder.Tag = "CAP;503"
        Me.fraStepOrder.Text = "*{Step Order}"
        '
        'txtOffHoldStep
        '
        Me.txtOffHoldStep.AcceptsReturn = True
        Me.txtOffHoldStep.BackColor = System.Drawing.SystemColors.Window
        Me.txtOffHoldStep.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOffHoldStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOffHoldStep.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOffHoldStep.Location = New System.Drawing.Point(512, 20)
        Me.txtOffHoldStep.MaxLength = 2
        Me.txtOffHoldStep.Name = "txtOffHoldStep"
        Me.txtOffHoldStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOffHoldStep.Size = New System.Drawing.Size(41, 20)
        Me.txtOffHoldStep.TabIndex = 9
        '
        'txtPreviousStep
        '
        Me.txtPreviousStep.AcceptsReturn = True
        Me.txtPreviousStep.BackColor = System.Drawing.SystemColors.Window
        Me.txtPreviousStep.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPreviousStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPreviousStep.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPreviousStep.Location = New System.Drawing.Point(368, 20)
        Me.txtPreviousStep.MaxLength = 2
        Me.txtPreviousStep.Name = "txtPreviousStep"
        Me.txtPreviousStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPreviousStep.Size = New System.Drawing.Size(41, 20)
        Me.txtPreviousStep.TabIndex = 7
        '
        'txtNextStep
        '
        Me.txtNextStep.AcceptsReturn = True
        Me.txtNextStep.BackColor = System.Drawing.SystemColors.Window
        Me.txtNextStep.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNextStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNextStep.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNextStep.Location = New System.Drawing.Point(216, 20)
        Me.txtNextStep.MaxLength = 2
        Me.txtNextStep.Name = "txtNextStep"
        Me.txtNextStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNextStep.Size = New System.Drawing.Size(41, 20)
        Me.txtNextStep.TabIndex = 5
        '
        'txtStep
        '
        Me.txtStep.AcceptsReturn = True
        Me.txtStep.BackColor = System.Drawing.SystemColors.Window
        Me.txtStep.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStep.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStep.Location = New System.Drawing.Point(72, 20)
        Me.txtStep.MaxLength = 2
        Me.txtStep.Name = "txtStep"
        Me.txtStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStep.Size = New System.Drawing.Size(41, 20)
        Me.txtStep.TabIndex = 3
        Me.txtStep.Tag = "F;M;"
        '
        'lblOffHold
        '
        Me.lblOffHold.AutoSize = True
        Me.lblOffHold.BackColor = System.Drawing.SystemColors.Control
        Me.lblOffHold.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOffHold.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOffHold.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOffHold.Location = New System.Drawing.Point(415, 21)
        Me.lblOffHold.Name = "lblOffHold"
        Me.lblOffHold.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOffHold.Size = New System.Drawing.Size(86, 13)
        Me.lblOffHold.TabIndex = 8
        Me.lblOffHold.Tag = "CAP;507"
        Me.lblOffHold.Text = "*{Off-Hold Step:}"
        Me.lblOffHold.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblPreviousStep
        '
        Me.lblPreviousStep.AutoSize = True
        Me.lblPreviousStep.BackColor = System.Drawing.SystemColors.Control
        Me.lblPreviousStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPreviousStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreviousStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPreviousStep.Location = New System.Drawing.Point(256, 21)
        Me.lblPreviousStep.Name = "lblPreviousStep"
        Me.lblPreviousStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPreviousStep.Size = New System.Drawing.Size(88, 13)
        Me.lblPreviousStep.TabIndex = 6
        Me.lblPreviousStep.Tag = "CAP;506"
        Me.lblPreviousStep.Text = "*{Previous Step:}"
        Me.lblPreviousStep.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblNextStep
        '
        Me.lblNextStep.AutoSize = True
        Me.lblNextStep.BackColor = System.Drawing.SystemColors.Control
        Me.lblNextStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblNextStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNextStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblNextStep.Location = New System.Drawing.Point(127, 21)
        Me.lblNextStep.Name = "lblNextStep"
        Me.lblNextStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblNextStep.Size = New System.Drawing.Size(69, 13)
        Me.lblNextStep.TabIndex = 4
        Me.lblNextStep.Tag = "CAP;505"
        Me.lblNextStep.Text = "*{Next Step:}"
        Me.lblNextStep.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblStep
        '
        Me.lblStep.AutoSize = True
        Me.lblStep.BackColor = System.Drawing.SystemColors.Control
        Me.lblStep.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStep.Location = New System.Drawing.Point(13, 21)
        Me.lblStep.Name = "lblStep"
        Me.lblStep.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStep.Size = New System.Drawing.Size(52, 13)
        Me.lblStep.TabIndex = 2
        Me.lblStep.Tag = "CAP;504"
        Me.lblStep.Text = "*{Step:}"
        Me.lblStep.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtHelper
        '
        Me.txtHelper.AcceptsReturn = True
        Me.txtHelper.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtHelper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtHelper.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHelper.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHelper.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHelper.Location = New System.Drawing.Point(8, 448)
        Me.txtHelper.MaxLength = 0
        Me.txtHelper.Multiline = True
        Me.txtHelper.Name = "txtHelper"
        Me.txtHelper.ReadOnly = True
        Me.txtHelper.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHelper.Size = New System.Drawing.Size(125, 29)
        Me.txtHelper.TabIndex = 88
        Me.txtHelper.Visible = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(504, 504)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 24)
        Me.cmdOK.TabIndex = 58
        Me.cmdOK.Tag = "CAP;209"
        Me.cmdOK.Text = "*{&OK}"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(584, 504)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 24)
        Me.cmdCancel.TabIndex = 59
        Me.cmdCancel.Tag = "CAP;210"
        Me.cmdCancel.Text = "*{&Cancel}"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'fraOther2
        '
        Me.fraOther2.BackColor = System.Drawing.SystemColors.Control
        Me.fraOther2.Controls.Add(Me.cboPMLookupUserGroup2)
        Me.fraOther2.Controls.Add(Me.cboPMLookupWrkTask2)
        Me.fraOther2.Controls.Add(Me.cboPMLookupActionType2)
        Me.fraOther2.Controls.Add(Me.lblTask2)
        Me.fraOther2.Controls.Add(Me.lblUserGroup2)
        Me.fraOther2.Controls.Add(Me.lblActiontype2)
        Me.fraOther2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraOther2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraOther2.Location = New System.Drawing.Point(336, 10)
        Me.fraOther2.Name = "fraOther2"
        Me.fraOther2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraOther2.Size = New System.Drawing.Size(321, 115)
        Me.fraOther2.TabIndex = 0
        Me.fraOther2.TabStop = False
        Me.fraOther2.Tag = "CAP;723"
        Me.fraOther2.Text = "*{Other Actions}"
        Me.fraOther2.Visible = False
        '
        'cboPMLookupUserGroup2
        '
        Me.cboPMLookupUserGroup2.DefaultItemId = 0
        Me.cboPMLookupUserGroup2.FirstItem = "("""")"
        Me.cboPMLookupUserGroup2.ItemId = 0
        Me.cboPMLookupUserGroup2.ListIndex = -1
        Me.cboPMLookupUserGroup2.Location = New System.Drawing.Point(160, 48)
        Me.cboPMLookupUserGroup2.Name = "cboPMLookupUserGroup2"
        Me.cboPMLookupUserGroup2.PMLookupProductFamily = 1
        Me.cboPMLookupUserGroup2.SingleItemId = 0
        Me.cboPMLookupUserGroup2.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupUserGroup2.Sorted = True
        Me.cboPMLookupUserGroup2.TabIndex = 63
        Me.cboPMLookupUserGroup2.TableName = "PMUser_Group"
        Me.cboPMLookupUserGroup2.ToolTipText = ""
        Me.cboPMLookupUserGroup2.WhereClause = ""
        '
        'cboPMLookupWrkTask2
        '
        Me.cboPMLookupWrkTask2.DefaultItemId = 0
        Me.cboPMLookupWrkTask2.FirstItem = "("""")"
        Me.cboPMLookupWrkTask2.ItemId = 0
        Me.cboPMLookupWrkTask2.ListIndex = -1
        Me.cboPMLookupWrkTask2.Location = New System.Drawing.Point(160, 16)
        Me.cboPMLookupWrkTask2.Name = "cboPMLookupWrkTask2"
        Me.cboPMLookupWrkTask2.PMLookupProductFamily = 2
        Me.cboPMLookupWrkTask2.SingleItemId = 0
        Me.cboPMLookupWrkTask2.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupWrkTask2.Sorted = True
        Me.cboPMLookupWrkTask2.TabIndex = 61
        Me.cboPMLookupWrkTask2.TableName = "PMWrk_Task"
        Me.cboPMLookupWrkTask2.ToolTipText = ""
        Me.cboPMLookupWrkTask2.WhereClause = ""
        '
        'cboPMLookupActionType2
        '
        Me.cboPMLookupActionType2.DefaultItemId = 0
        Me.cboPMLookupActionType2.FirstItem = "("""")"
        Me.cboPMLookupActionType2.ItemId = 0
        Me.cboPMLookupActionType2.ListIndex = -1
        Me.cboPMLookupActionType2.Location = New System.Drawing.Point(160, 80)
        Me.cboPMLookupActionType2.Name = "cboPMLookupActionType2"
        Me.cboPMLookupActionType2.PMLookupProductFamily = 1
        Me.cboPMLookupActionType2.SingleItemId = 0
        Me.cboPMLookupActionType2.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupActionType2.Sorted = True
        Me.cboPMLookupActionType2.TabIndex = 64
        Me.cboPMLookupActionType2.TableName = "PMWrk_Task_Action_type"
        Me.cboPMLookupActionType2.ToolTipText = ""
        Me.cboPMLookupActionType2.WhereClause = ""
        '
        'lblTask2
        '
        Me.lblTask2.BackColor = System.Drawing.SystemColors.Control
        Me.lblTask2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTask2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTask2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTask2.Location = New System.Drawing.Point(8, 16)
        Me.lblTask2.Name = "lblTask2"
        Me.lblTask2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTask2.Size = New System.Drawing.Size(145, 25)
        Me.lblTask2.TabIndex = 60
        Me.lblTask2.Tag = "CAP;524"
        Me.lblTask2.Text = "*{Create Work manager Task:}"
        '
        'lblUserGroup2
        '
        Me.lblUserGroup2.BackColor = System.Drawing.SystemColors.Control
        Me.lblUserGroup2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUserGroup2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserGroup2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUserGroup2.Location = New System.Drawing.Point(8, 48)
        Me.lblUserGroup2.Name = "lblUserGroup2"
        Me.lblUserGroup2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUserGroup2.Size = New System.Drawing.Size(177, 17)
        Me.lblUserGroup2.TabIndex = 62
        Me.lblUserGroup2.Tag = "CAP;525"
        Me.lblUserGroup2.Text = "*{For User Group:}"
        '
        'lblActiontype2
        '
        Me.lblActiontype2.BackColor = System.Drawing.SystemColors.Control
        Me.lblActiontype2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblActiontype2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActiontype2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblActiontype2.Location = New System.Drawing.Point(8, 80)
        Me.lblActiontype2.Name = "lblActiontype2"
        Me.lblActiontype2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblActiontype2.Size = New System.Drawing.Size(177, 17)
        Me.lblActiontype2.TabIndex = 65
        Me.lblActiontype2.Tag = "CAP;525"
        Me.lblActiontype2.Text = "*{Action Type:}"
        '
        'fraPrint2
        '
        Me.fraPrint2.BackColor = System.Drawing.SystemColors.Control
        Me.fraPrint2.Controls.Add(Me.cboClientLetter2)
        Me.fraPrint2.Controls.Add(Me.cboOIPLetter2)
        Me.fraPrint2.Controls.Add(Me.lblOIPLetter2)
        Me.fraPrint2.Controls.Add(Me.lblClientLetter2)
        Me.fraPrint2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPrint2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPrint2.Location = New System.Drawing.Point(336, 18)
        Me.fraPrint2.Name = "fraPrint2"
        Me.fraPrint2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPrint2.Size = New System.Drawing.Size(321, 89)
        Me.fraPrint2.TabIndex = 66
        Me.fraPrint2.TabStop = False
        Me.fraPrint2.Tag = "CAP;722"
        Me.fraPrint2.Text = "*{Print}"
        Me.fraPrint2.Visible = False
        '
        'cboClientLetter2
        '
        Me.cboClientLetter2.BackColor = System.Drawing.SystemColors.Window
        Me.cboClientLetter2.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboClientLetter2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboClientLetter2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboClientLetter2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboClientLetter2.Location = New System.Drawing.Point(160, 32)
        Me.cboClientLetter2.Name = "cboClientLetter2"
        Me.cboClientLetter2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboClientLetter2.Size = New System.Drawing.Size(153, 21)
        Me.cboClientLetter2.TabIndex = 68
        '
        'cboOIPLetter2
        '
        Me.cboOIPLetter2.BackColor = System.Drawing.SystemColors.Window
        Me.cboOIPLetter2.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOIPLetter2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOIPLetter2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOIPLetter2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOIPLetter2.Location = New System.Drawing.Point(160, 56)
        Me.cboOIPLetter2.Name = "cboOIPLetter2"
        Me.cboOIPLetter2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOIPLetter2.Size = New System.Drawing.Size(153, 21)
        Me.cboOIPLetter2.TabIndex = 70
        '
        'lblOIPLetter2
        '
        Me.lblOIPLetter2.BackColor = System.Drawing.SystemColors.Control
        Me.lblOIPLetter2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOIPLetter2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOIPLetter2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOIPLetter2.Location = New System.Drawing.Point(8, 52)
        Me.lblOIPLetter2.Name = "lblOIPLetter2"
        Me.lblOIPLetter2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOIPLetter2.Size = New System.Drawing.Size(97, 17)
        Me.lblOIPLetter2.TabIndex = 69
        Me.lblOIPLetter2.Tag = "CAP;519"
        Me.lblOIPLetter2.Text = "*{OIP Letter:}"
        '
        'lblClientLetter2
        '
        Me.lblClientLetter2.BackColor = System.Drawing.SystemColors.Control
        Me.lblClientLetter2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblClientLetter2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClientLetter2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblClientLetter2.Location = New System.Drawing.Point(8, 19)
        Me.lblClientLetter2.Name = "lblClientLetter2"
        Me.lblClientLetter2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblClientLetter2.Size = New System.Drawing.Size(97, 17)
        Me.lblClientLetter2.TabIndex = 67
        Me.lblClientLetter2.Tag = "CAP;518"
        Me.lblClientLetter2.Text = "*{Client Letter:}"
        '
        'fraPrint
        '
        Me.fraPrint.BackColor = System.Drawing.SystemColors.Control
        Me.fraPrint.Controls.Add(Me.cboPMLookupBrokerReport)
        Me.fraPrint.Controls.Add(Me.cboOIPLetter)
        Me.fraPrint.Controls.Add(Me.lblBrokerReport)
        Me.fraPrint.Controls.Add(Me.lblOIPLetter)
        Me.fraPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPrint.Location = New System.Drawing.Point(8, 210)
        Me.fraPrint.Name = "fraPrint"
        Me.fraPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPrint.Size = New System.Drawing.Size(649, 49)
        Me.fraPrint.TabIndex = 77
        Me.fraPrint.TabStop = False
        Me.fraPrint.Tag = "CAP;517"
        Me.fraPrint.Text = "*{Print}"
        Me.fraPrint.Visible = False
        '
        'cboPMLookupBrokerReport
        '
        Me.cboPMLookupBrokerReport.DefaultItemId = 0
        Me.cboPMLookupBrokerReport.FirstItem = "("""")"
        Me.cboPMLookupBrokerReport.ItemId = 0
        Me.cboPMLookupBrokerReport.ListIndex = -1
        Me.cboPMLookupBrokerReport.Location = New System.Drawing.Point(480, 16)
        Me.cboPMLookupBrokerReport.Name = "cboPMLookupBrokerReport"
        Me.cboPMLookupBrokerReport.PMLookupProductFamily = 2
        Me.cboPMLookupBrokerReport.SingleItemId = 0
        Me.cboPMLookupBrokerReport.Size = New System.Drawing.Size(153, 21)
        Me.cboPMLookupBrokerReport.Sorted = True
        Me.cboPMLookupBrokerReport.TabIndex = 78
        Me.cboPMLookupBrokerReport.TableName = "Report"
        Me.cboPMLookupBrokerReport.ToolTipText = ""
        Me.cboPMLookupBrokerReport.Visible = False
        Me.cboPMLookupBrokerReport.WhereClause = ""
        '
        'cboOIPLetter
        '
        Me.cboOIPLetter.BackColor = System.Drawing.SystemColors.Window
        Me.cboOIPLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboOIPLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOIPLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOIPLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboOIPLetter.Location = New System.Drawing.Point(480, 16)
        Me.cboOIPLetter.Name = "cboOIPLetter"
        Me.cboOIPLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOIPLetter.Size = New System.Drawing.Size(153, 21)
        Me.cboOIPLetter.TabIndex = 79
        Me.cboOIPLetter.Visible = False
        '
        'lblBrokerReport
        '
        Me.lblBrokerReport.BackColor = System.Drawing.SystemColors.Control
        Me.lblBrokerReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBrokerReport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBrokerReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrokerReport.Location = New System.Drawing.Point(488, 16)
        Me.lblBrokerReport.Name = "lblBrokerReport"
        Me.lblBrokerReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBrokerReport.Size = New System.Drawing.Size(121, 17)
        Me.lblBrokerReport.TabIndex = 80
        Me.lblBrokerReport.Tag = "CAP;520"
        Me.lblBrokerReport.Text = "*{Broker Report:}"
        Me.lblBrokerReport.Visible = False
        '
        'lblOIPLetter
        '
        Me.lblOIPLetter.BackColor = System.Drawing.SystemColors.Control
        Me.lblOIPLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOIPLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOIPLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOIPLetter.Location = New System.Drawing.Point(504, 18)
        Me.lblOIPLetter.Name = "lblOIPLetter"
        Me.lblOIPLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOIPLetter.Size = New System.Drawing.Size(97, 17)
        Me.lblOIPLetter.TabIndex = 81
        Me.lblOIPLetter.Tag = "CAP;519"
        Me.lblOIPLetter.Text = "*{OIP Letter:}"
        Me.lblOIPLetter.Visible = False
        '
        'ssTabBroker
        '
        Me.ssTabBroker.Controls.Add(Me.TabPage1)
        Me.ssTabBroker.Controls.Add(Me.TabPage2)
        Me.ssTabBroker.Location = New System.Drawing.Point(7, 162)
        Me.ssTabBroker.Name = "ssTabBroker"
        Me.ssTabBroker.SelectedIndex = 0
        Me.ssTabBroker.Size = New System.Drawing.Size(650, 116)
        Me.ssTabBroker.TabIndex = 89
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.chkJumptonextstepBroker)
        Me.TabPage1.Controls.Add(Me.fraToleranceBroker)
        Me.TabPage1.Controls.Add(Me.cboBrokerLetter)
        Me.TabPage1.Controls.Add(Me.lblBrokerLetter)
        Me.TabPage1.Controls.Add(Me.txtBrokerDays)
        Me.TabPage1.Controls.Add(Me.lblBroketrDays)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(642, 90)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Broker Business"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'chkJumptonextstepBroker
        '
        Me.chkJumptonextstepBroker.AutoSize = True
        Me.chkJumptonextstepBroker.Location = New System.Drawing.Point(13, 12)
        Me.chkJumptonextstepBroker.Name = "chkJumptonextstepBroker"
        Me.chkJumptonextstepBroker.Size = New System.Drawing.Size(131, 17)
        Me.chkJumptonextstepBroker.TabIndex = 29
        Me.chkJumptonextstepBroker.Text = "Jump to Next Step"
        Me.chkJumptonextstepBroker.UseVisualStyleBackColor = True
        '
        'fraToleranceBroker
        '
        Me.fraToleranceBroker.BackColor = System.Drawing.SystemColors.Control
        Me.fraToleranceBroker.Controls.Add(Me.txtAccountAmt)
        Me.fraToleranceBroker.Controls.Add(Me.lblAccount)
        Me.fraToleranceBroker.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraToleranceBroker.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraToleranceBroker.Location = New System.Drawing.Point(9, 40)
        Me.fraToleranceBroker.Name = "fraToleranceBroker"
        Me.fraToleranceBroker.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraToleranceBroker.Size = New System.Drawing.Size(252, 47)
        Me.fraToleranceBroker.TabIndex = 28
        Me.fraToleranceBroker.TabStop = False
        Me.fraToleranceBroker.Tag = "CAP;514"
        Me.fraToleranceBroker.Text = "*{Tolerance Amount}"
        '
        'txtAccountAmt
        '
        Me.txtAccountAmt.AcceptsReturn = True
        Me.txtAccountAmt.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountAmt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountAmt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountAmt.Location = New System.Drawing.Point(112, 16)
        Me.txtAccountAmt.MaxLength = 50
        Me.txtAccountAmt.Name = "txtAccountAmt"
        Me.txtAccountAmt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountAmt.Size = New System.Drawing.Size(129, 20)
        Me.txtAccountAmt.TabIndex = 28
        Me.txtAccountAmt.Tag = "FMT;$"
        '
        'lblAccount
        '
        Me.lblAccount.AutoSize = True
        Me.lblAccount.BackColor = System.Drawing.SystemColors.Control
        Me.lblAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAccount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAccount.Location = New System.Drawing.Point(17, 18)
        Me.lblAccount.Name = "lblAccount"
        Me.lblAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAccount.Size = New System.Drawing.Size(62, 13)
        Me.lblAccount.TabIndex = 27
        Me.lblAccount.Tag = "CAP;516"
        Me.lblAccount.Text = "*{Account:}"
        '
        'cboBrokerLetter
        '
        Me.cboBrokerLetter.BackColor = System.Drawing.SystemColors.Window
        Me.cboBrokerLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBrokerLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBrokerLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBrokerLetter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBrokerLetter.Location = New System.Drawing.Point(360, 56)
        Me.cboBrokerLetter.Name = "cboBrokerLetter"
        Me.cboBrokerLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBrokerLetter.Size = New System.Drawing.Size(276, 21)
        Me.cboBrokerLetter.TabIndex = 27
        '
        'lblBrokerLetter
        '
        Me.lblBrokerLetter.AutoSize = True
        Me.lblBrokerLetter.BackColor = System.Drawing.SystemColors.Control
        Me.lblBrokerLetter.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBrokerLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBrokerLetter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrokerLetter.Location = New System.Drawing.Point(267, 62)
        Me.lblBrokerLetter.Name = "lblBrokerLetter"
        Me.lblBrokerLetter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBrokerLetter.Size = New System.Drawing.Size(83, 13)
        Me.lblBrokerLetter.TabIndex = 26
        Me.lblBrokerLetter.Tag = "CAP;724"
        Me.lblBrokerLetter.Text = "*{Broker Letter:}"
        '
        'txtBrokerDays
        '
        Me.txtBrokerDays.AcceptsReturn = True
        Me.txtBrokerDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtBrokerDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBrokerDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBrokerDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBrokerDays.Location = New System.Drawing.Point(361, 10)
        Me.txtBrokerDays.MaxLength = 2
        Me.txtBrokerDays.Name = "txtBrokerDays"
        Me.txtBrokerDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBrokerDays.Size = New System.Drawing.Size(65, 20)
        Me.txtBrokerDays.TabIndex = 25
        '
        'lblBroketrDays
        '
        Me.lblBroketrDays.AutoSize = True
        Me.lblBroketrDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblBroketrDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBroketrDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBroketrDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBroketrDays.Location = New System.Drawing.Point(230, 12)
        Me.lblBroketrDays.Name = "lblBroketrDays"
        Me.lblBroketrDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBroketrDays.Size = New System.Drawing.Size(80, 13)
        Me.lblBroketrDays.TabIndex = 24
        Me.lblBroketrDays.Tag = "CAP;510"
        Me.lblBroketrDays.Text = "*{Broker Days:}"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.chkJumptonextstepBrokerSingleInst)
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Controls.Add(Me.cboBrokerLetterSingleInst)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.txtBrokerDaysSingleInst)
        Me.TabPage2.Controls.Add(Me.lblBrokerDaysSingleInst)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(642, 90)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Single - Instalment Broker"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'chkJumptonextstepBrokerSingleInst
        '
        Me.chkJumptonextstepBrokerSingleInst.AutoSize = True
        Me.chkJumptonextstepBrokerSingleInst.Location = New System.Drawing.Point(13, 12)
        Me.chkJumptonextstepBrokerSingleInst.Name = "chkJumptonextstepBrokerSingleInst"
        Me.chkJumptonextstepBrokerSingleInst.Size = New System.Drawing.Size(131, 17)
        Me.chkJumptonextstepBrokerSingleInst.TabIndex = 35
        Me.chkJumptonextstepBrokerSingleInst.Text = "Jump to Next Step"
        Me.chkJumptonextstepBrokerSingleInst.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.txtAccountAmtSingleInst)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox1.Location = New System.Drawing.Point(9, 40)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox1.Size = New System.Drawing.Size(252, 47)
        Me.GroupBox1.TabIndex = 34
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Tag = "CAP;514"
        Me.GroupBox1.Text = "*{Tolerance Amount}"
        '
        'txtAccountAmtSingleInst
        '
        Me.txtAccountAmtSingleInst.AcceptsReturn = True
        Me.txtAccountAmtSingleInst.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccountAmtSingleInst.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccountAmtSingleInst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountAmtSingleInst.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccountAmtSingleInst.Location = New System.Drawing.Point(112, 16)
        Me.txtAccountAmtSingleInst.MaxLength = 50
        Me.txtAccountAmtSingleInst.Name = "txtAccountAmtSingleInst"
        Me.txtAccountAmtSingleInst.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccountAmtSingleInst.Size = New System.Drawing.Size(129, 20)
        Me.txtAccountAmtSingleInst.TabIndex = 28
        Me.txtAccountAmtSingleInst.Tag = "FMT;$"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(17, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 27
        Me.Label2.Tag = "CAP;516"
        Me.Label2.Text = "*{Account:}"
        '
        'cboBrokerLetterSingleInst
        '
        Me.cboBrokerLetterSingleInst.BackColor = System.Drawing.SystemColors.Window
        Me.cboBrokerLetterSingleInst.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBrokerLetterSingleInst.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBrokerLetterSingleInst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBrokerLetterSingleInst.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBrokerLetterSingleInst.Location = New System.Drawing.Point(360, 56)
        Me.cboBrokerLetterSingleInst.Name = "cboBrokerLetterSingleInst"
        Me.cboBrokerLetterSingleInst.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBrokerLetterSingleInst.Size = New System.Drawing.Size(276, 21)
        Me.cboBrokerLetterSingleInst.TabIndex = 33
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(267, 62)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(83, 13)
        Me.Label4.TabIndex = 32
        Me.Label4.Tag = "CAP;724"
        Me.Label4.Text = "*{Broker Letter:}"
        '
        'txtBrokerDaysSingleInst
        '
        Me.txtBrokerDaysSingleInst.AcceptsReturn = True
        Me.txtBrokerDaysSingleInst.BackColor = System.Drawing.SystemColors.Window
        Me.txtBrokerDaysSingleInst.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBrokerDaysSingleInst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBrokerDaysSingleInst.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBrokerDaysSingleInst.Location = New System.Drawing.Point(361, 10)
        Me.txtBrokerDaysSingleInst.MaxLength = 2
        Me.txtBrokerDaysSingleInst.Name = "txtBrokerDaysSingleInst"
        Me.txtBrokerDaysSingleInst.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBrokerDaysSingleInst.Size = New System.Drawing.Size(65, 20)
        Me.txtBrokerDaysSingleInst.TabIndex = 31
        '
        'lblBrokerDaysSingleInst
        '
        Me.lblBrokerDaysSingleInst.AutoSize = True
        Me.lblBrokerDaysSingleInst.BackColor = System.Drawing.SystemColors.Control
        Me.lblBrokerDaysSingleInst.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBrokerDaysSingleInst.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBrokerDaysSingleInst.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBrokerDaysSingleInst.Location = New System.Drawing.Point(226, 13)
        Me.lblBrokerDaysSingleInst.Name = "lblBrokerDaysSingleInst"
        Me.lblBrokerDaysSingleInst.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBrokerDaysSingleInst.Size = New System.Drawing.Size(124, 13)
        Me.lblBrokerDaysSingleInst.TabIndex = 30
        Me.lblBrokerDaysSingleInst.Tag = "CAP;510"
        Me.lblBrokerDaysSingleInst.Text = "Elapsed Days after jump:"
        '
        'frmStep
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(673, 533)
        Me.ControlBox = False
        Me.Controls.Add(Me.ssTabBroker)
        Me.Controls.Add(Me.fraAutoCancellationActions)
        Me.Controls.Add(Me.fraTriggers)
        Me.Controls.Add(Me.fraDirectCustomers)
        Me.Controls.Add(Me.fraOther)
        Me.Controls.Add(Me.fraTolerancePercentages)
        Me.Controls.Add(Me.fraRecurring)
        Me.Controls.Add(Me.fraStepOrder)
        Me.Controls.Add(Me.txtHelper)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.fraOther2)
        Me.Controls.Add(Me.fraPrint2)
        Me.Controls.Add(Me.fraPrint)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStep"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "*{Add/Edit Step}"
        Me.fraAutoCancellationActions.ResumeLayout(False)
        Me.fraWriteOffTolerance.ResumeLayout(False)
        Me.fraWriteOffTolerance.PerformLayout()
        Me.fraAutoCancellationDocuments.ResumeLayout(False)
        Me.fraAutoCancellationDocuments.PerformLayout()
        Me.fraTriggers.ResumeLayout(False)
        Me.fraTriggers.PerformLayout()
        Me.fraDirectCustomers.ResumeLayout(False)
        Me.fraDirectCustomers.PerformLayout()
        Me.fraTolerance.ResumeLayout(False)
        Me.fraTolerance.PerformLayout()
        Me.fraOther.ResumeLayout(False)
        Me.fraOther.PerformLayout()
        Me.fraTolerancePercentages.ResumeLayout(False)
        Me.fraTolerancePercentages.PerformLayout()
        Me.fraRecurring.ResumeLayout(False)
        Me.fraRecurring.PerformLayout()
        Me.fraStepOrder.ResumeLayout(False)
        Me.fraStepOrder.PerformLayout()
        Me.fraOther2.ResumeLayout(False)
        Me.fraPrint2.ResumeLayout(False)
        Me.fraPrint.ResumeLayout(False)
        CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ssTabBroker.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.fraToleranceBroker.ResumeLayout(False)
        Me.fraToleranceBroker.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ssTabBroker As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents fraToleranceBroker As System.Windows.Forms.GroupBox
    Public WithEvents txtAccountAmt As System.Windows.Forms.TextBox
    Public WithEvents lblAccount As System.Windows.Forms.Label
    Public WithEvents cboBrokerLetter As System.Windows.Forms.ComboBox
    Public WithEvents lblBrokerLetter As System.Windows.Forms.Label
    Public WithEvents txtBrokerDays As System.Windows.Forms.TextBox
    Public WithEvents lblBroketrDays As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents chkJumptonextstepBroker As System.Windows.Forms.CheckBox
    Friend WithEvents chkJumptonextstepBrokerSingleInst As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents txtAccountAmtSingleInst As System.Windows.Forms.TextBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents cboBrokerLetterSingleInst As System.Windows.Forms.ComboBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents txtBrokerDaysSingleInst As System.Windows.Forms.TextBox
    Public WithEvents lblBrokerDaysSingleInst As System.Windows.Forms.Label
#End Region 
End Class