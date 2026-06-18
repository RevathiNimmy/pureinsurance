<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterface
#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		isInitializingComponent = True
		InitializeComponent()
		isInitializingComponent = False
		InitializeoptView()
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents cmdApply As System.Windows.Forms.Button
	Public WithEvents lblExcess As System.Windows.Forms.Label
	Public WithEvents lblViewOptions As System.Windows.Forms.Label
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdAssign As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents txtTotal As System.Windows.Forms.TextBox
	Public WithEvents txtSubtotal As System.Windows.Forms.TextBox
	Public WithEvents txtExtras As System.Windows.Forms.TextBox
	Public WithEvents lblTotal As System.Windows.Forms.Label
	Public WithEvents lblExtras As System.Windows.Forms.Label
	Public WithEvents lblSubTotal As System.Windows.Forms.Label
	Public WithEvents lblPaymentAmount As System.Windows.Forms.Label
	Public WithEvents fraTotals As System.Windows.Forms.GroupBox
	Private WithEvents _optView_1 As System.Windows.Forms.RadioButton
	Private WithEvents _optView_0 As System.Windows.Forms.RadioButton
	Public WithEvents cboFilter As System.Windows.Forms.ComboBox
	Public WithEvents lblFilter As System.Windows.Forms.Label
	Public WithEvents fraViewOptions As System.Windows.Forms.GroupBox
	Public WithEvents txtExcess As System.Windows.Forms.TextBox
	Public WithEvents lvwItems As System.Windows.Forms.ListView
	Private WithEvents _tabLossSchedule_TabPage0 As System.Windows.Forms.TabPage
	Private WithEvents _tabLossSchedule_TabPage1 As System.Windows.Forms.TabPage
	Private WithEvents _tabLossSchedule_TabPage2 As System.Windows.Forms.TabPage
	Public WithEvents tabLossSchedule As System.Windows.Forms.TabControl
	Public optView(1) As System.Windows.Forms.RadioButton
	Private WithEvents listViewHelper1 As Artinsoft.VB6.Gui.ListViewHelper
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	 Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterface))
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOK = New System.Windows.Forms.Button
		Me.cmdApply = New System.Windows.Forms.Button
		Me.tabLossSchedule = New System.Windows.Forms.TabControl
		Me._tabLossSchedule_TabPage0 = New System.Windows.Forms.TabPage
		Me.lblExcess = New System.Windows.Forms.Label
		Me.lblViewOptions = New System.Windows.Forms.Label
		Me.cmdEdit = New System.Windows.Forms.Button
		Me.cmdAssign = New System.Windows.Forms.Button
		Me.cmdAdd = New System.Windows.Forms.Button
		Me.fraTotals = New System.Windows.Forms.GroupBox
		Me.txtTotal = New System.Windows.Forms.TextBox
		Me.txtSubtotal = New System.Windows.Forms.TextBox
		Me.txtExtras = New System.Windows.Forms.TextBox
		Me.lblTotal = New System.Windows.Forms.Label
		Me.lblExtras = New System.Windows.Forms.Label
		Me.lblSubTotal = New System.Windows.Forms.Label
		Me.lblPaymentAmount = New System.Windows.Forms.Label
		Me.fraViewOptions = New System.Windows.Forms.GroupBox
		Me._optView_1 = New System.Windows.Forms.RadioButton
		Me._optView_0 = New System.Windows.Forms.RadioButton
		Me.cboFilter = New System.Windows.Forms.ComboBox
		Me.lblFilter = New System.Windows.Forms.Label
		Me.txtExcess = New System.Windows.Forms.TextBox
		Me.lvwItems = New System.Windows.Forms.ListView
		Me._tabLossSchedule_TabPage1 = New System.Windows.Forms.TabPage
		Me._tabLossSchedule_TabPage2 = New System.Windows.Forms.TabPage
		Me.tabLossSchedule.SuspendLayout()
		Me._tabLossSchedule_TabPage0.SuspendLayout()
		Me.fraTotals.SuspendLayout()
		Me.fraViewOptions.SuspendLayout()
		Me.SuspendLayout()
		Me.listViewHelper1 = New Artinsoft.VB6.Gui.ListViewHelper(Me.components)
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).BeginInit()
		' 
		' cmdCancel
		' 
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Location = New System.Drawing.Point(656, 520)
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
		Me.cmdCancel.TabIndex = 3
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdOK
		' 
		Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOK.CausesValidation = True
		Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOK.Enabled = True
		Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOK.Location = New System.Drawing.Point(576, 520)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOK.Size = New System.Drawing.Size(73, 22)
		Me.cmdOK.TabIndex = 2
		Me.cmdOK.TabStop = True
		Me.cmdOK.Text = "Ok"
		Me.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdApply
		' 
		Me.cmdApply.BackColor = System.Drawing.SystemColors.Control
		Me.cmdApply.CausesValidation = True
		Me.cmdApply.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdApply.Enabled = True
		Me.cmdApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdApply.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdApply.Location = New System.Drawing.Point(496, 520)
		Me.cmdApply.Name = "cmdApply"
		Me.cmdApply.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdApply.Size = New System.Drawing.Size(73, 22)
		Me.cmdApply.TabIndex = 1
		Me.cmdApply.TabStop = True
		Me.cmdApply.Text = "Apply"
		Me.cmdApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' tabLossSchedule
		' 
		Me.tabLossSchedule.Alignment = System.Windows.Forms.TabAlignment.Top
		Me.tabLossSchedule.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.tabLossSchedule.Controls.Add(Me._tabLossSchedule_TabPage0)
		Me.tabLossSchedule.Controls.Add(Me._tabLossSchedule_TabPage1)
		Me.tabLossSchedule.Controls.Add(Me._tabLossSchedule_TabPage2)
		Me.tabLossSchedule.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.tabLossSchedule.ItemSize = New System.Drawing.Size(239, 18)
		Me.tabLossSchedule.Location = New System.Drawing.Point(0, 32)
		Me.tabLossSchedule.Multiline = True
		Me.tabLossSchedule.Name = "tabLossSchedule"
		Me.tabLossSchedule.Size = New System.Drawing.Size(725, 477)
		Me.tabLossSchedule.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
		Me.tabLossSchedule.TabIndex = 0
		' 
		' _tabLossSchedule_TabPage0
		' 
		Me._tabLossSchedule_TabPage0.Controls.Add(Me.lblExcess)
		Me._tabLossSchedule_TabPage0.Controls.Add(Me.lblViewOptions)
		Me._tabLossSchedule_TabPage0.Controls.Add(Me.cmdEdit)
		Me._tabLossSchedule_TabPage0.Controls.Add(Me.cmdAssign)
		Me._tabLossSchedule_TabPage0.Controls.Add(Me.cmdAdd)
		Me._tabLossSchedule_TabPage0.Controls.Add(Me.fraTotals)
		Me._tabLossSchedule_TabPage0.Controls.Add(Me.fraViewOptions)
		Me._tabLossSchedule_TabPage0.Controls.Add(Me.txtExcess)
		Me._tabLossSchedule_TabPage0.Controls.Add(Me.lvwItems)
		Me._tabLossSchedule_TabPage0.Text = "&1 - Loss Schedule"
		' 
		' lblExcess
		' 
		Me.lblExcess.AutoSize = True
		Me.lblExcess.BackColor = System.Drawing.SystemColors.Control
		Me.lblExcess.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblExcess.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblExcess.Enabled = True
		Me.lblExcess.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblExcess.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblExcess.Location = New System.Drawing.Point(8, 15)
		Me.lblExcess.Name = "lblExcess"
		Me.lblExcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblExcess.Size = New System.Drawing.Size(205, 13)
		Me.lblExcess.TabIndex = 18
		Me.lblExcess.Text = "Excess For This Loss Schedule"
		Me.lblExcess.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblExcess.UseMnemonic = True
		Me.lblExcess.Visible = True
		' 
		' lblViewOptions
		' 
		Me.lblViewOptions.AutoSize = True
		Me.lblViewOptions.BackColor = System.Drawing.SystemColors.Control
		Me.lblViewOptions.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblViewOptions.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblViewOptions.Enabled = True
		Me.lblViewOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblViewOptions.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblViewOptions.Location = New System.Drawing.Point(288, 15)
		Me.lblViewOptions.Name = "lblViewOptions"
		Me.lblViewOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblViewOptions.Size = New System.Drawing.Size(70, 13)
		Me.lblViewOptions.TabIndex = 20
		Me.lblViewOptions.Text = "View Options"
		Me.lblViewOptions.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblViewOptions.UseMnemonic = True
		Me.lblViewOptions.Visible = True
		' 
		' cmdEdit
		' 
		Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
		Me.cmdEdit.CausesValidation = True
		Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdEdit.Enabled = True
		Me.cmdEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdEdit.Location = New System.Drawing.Point(640, 420)
		Me.cmdEdit.Name = "cmdEdit"
		Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdEdit.Size = New System.Drawing.Size(73, 22)
		Me.cmdEdit.TabIndex = 4
		Me.cmdEdit.TabStop = True
		Me.cmdEdit.Text = "Edit"
		Me.cmdEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAssign
		' 
		Me.cmdAssign.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAssign.CausesValidation = True
		Me.cmdAssign.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAssign.Enabled = True
		Me.cmdAssign.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAssign.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAssign.Location = New System.Drawing.Point(480, 420)
		Me.cmdAssign.Name = "cmdAssign"
		Me.cmdAssign.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAssign.Size = New System.Drawing.Size(73, 22)
		Me.cmdAssign.TabIndex = 5
		Me.cmdAssign.TabStop = True
		Me.cmdAssign.Text = "Assign"
		Me.cmdAssign.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAssign.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' cmdAdd
		' 
		Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdd.CausesValidation = True
		Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdd.Enabled = True
		Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdd.Location = New System.Drawing.Point(560, 420)
		Me.cmdAdd.Name = "cmdAdd"
		Me.cmdAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdd.Size = New System.Drawing.Size(73, 22)
		Me.cmdAdd.TabIndex = 6
		Me.cmdAdd.TabStop = True
		Me.cmdAdd.Text = "Add"
		Me.cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
		' 
		' fraTotals
		' 
		Me.fraTotals.BackColor = System.Drawing.SystemColors.Control
		Me.fraTotals.Controls.Add(Me.txtTotal)
		Me.fraTotals.Controls.Add(Me.txtSubtotal)
		Me.fraTotals.Controls.Add(Me.txtExtras)
		Me.fraTotals.Controls.Add(Me.lblTotal)
		Me.fraTotals.Controls.Add(Me.lblExtras)
		Me.fraTotals.Controls.Add(Me.lblSubTotal)
		Me.fraTotals.Controls.Add(Me.lblPaymentAmount)
		Me.fraTotals.Enabled = True
		Me.fraTotals.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraTotals.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraTotals.Location = New System.Drawing.Point(392, 292)
		Me.fraTotals.Name = "fraTotals"
		Me.fraTotals.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraTotals.Size = New System.Drawing.Size(321, 121)
		Me.fraTotals.TabIndex = 7
		Me.fraTotals.Text = "Totals"
		Me.fraTotals.Visible = True
		' 
		' txtTotal
		' 
		Me.txtTotal.AcceptsReturn = True
		Me.txtTotal.AutoSize = False
		Me.txtTotal.BackColor = System.Drawing.SystemColors.Control
		Me.txtTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTotal.CausesValidation = True
		Me.txtTotal.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTotal.Enabled = False
		Me.txtTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtTotal.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTotal.HideSelection = True
		Me.txtTotal.Location = New System.Drawing.Point(168, 88)
		Me.txtTotal.MaxLength = 0
		Me.txtTotal.Multiline = False
		Me.txtTotal.Name = "txtTotal"
		Me.txtTotal.ReadOnly = False
		Me.txtTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTotal.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTotal.Size = New System.Drawing.Size(137, 21)
		Me.txtTotal.TabIndex = 14
		Me.txtTotal.TabStop = True
		Me.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTotal.Visible = True
		' 
		' txtSubtotal
		' 
		Me.txtSubtotal.AcceptsReturn = True
		Me.txtSubtotal.AutoSize = False
		Me.txtSubtotal.BackColor = System.Drawing.SystemColors.Control
		Me.txtSubtotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtSubtotal.CausesValidation = True
		Me.txtSubtotal.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtSubtotal.Enabled = False
		Me.txtSubtotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtSubtotal.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtSubtotal.HideSelection = True
		Me.txtSubtotal.Location = New System.Drawing.Point(168, 40)
		Me.txtSubtotal.MaxLength = 0
		Me.txtSubtotal.Multiline = False
		Me.txtSubtotal.Name = "txtSubtotal"
		Me.txtSubtotal.ReadOnly = False
		Me.txtSubtotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtSubtotal.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtSubtotal.Size = New System.Drawing.Size(137, 21)
		Me.txtSubtotal.TabIndex = 13
		Me.txtSubtotal.TabStop = True
		Me.txtSubtotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtSubtotal.Visible = True
		' 
		' txtExtras
		' 
		Me.txtExtras.AcceptsReturn = True
		Me.txtExtras.AutoSize = False
		Me.txtExtras.BackColor = System.Drawing.SystemColors.Control
		Me.txtExtras.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtExtras.CausesValidation = True
		Me.txtExtras.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtExtras.Enabled = False
		Me.txtExtras.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtExtras.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtExtras.HideSelection = True
		Me.txtExtras.Location = New System.Drawing.Point(168, 64)
		Me.txtExtras.MaxLength = 0
		Me.txtExtras.Multiline = False
		Me.txtExtras.Name = "txtExtras"
		Me.txtExtras.ReadOnly = False
		Me.txtExtras.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtExtras.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtExtras.Size = New System.Drawing.Size(137, 21)
		Me.txtExtras.TabIndex = 12
		Me.txtExtras.TabStop = True
		Me.txtExtras.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtExtras.Visible = True
		' 
		' lblTotal
		' 
		Me.lblTotal.AutoSize = False
		Me.lblTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTotal.Enabled = True
		Me.lblTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTotal.Location = New System.Drawing.Point(40, 88)
		Me.lblTotal.Name = "lblTotal"
		Me.lblTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTotal.Size = New System.Drawing.Size(117, 17)
		Me.lblTotal.TabIndex = 11
		Me.lblTotal.Text = "Total"
		Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblTotal.UseMnemonic = True
		Me.lblTotal.Visible = True
		' 
		' lblExtras
		' 
		Me.lblExtras.AutoSize = False
		Me.lblExtras.BackColor = System.Drawing.SystemColors.Control
		Me.lblExtras.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblExtras.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblExtras.Enabled = True
		Me.lblExtras.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblExtras.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblExtras.Location = New System.Drawing.Point(40, 64)
		Me.lblExtras.Name = "lblExtras"
		Me.lblExtras.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblExtras.Size = New System.Drawing.Size(117, 17)
		Me.lblExtras.TabIndex = 10
		Me.lblExtras.Text = "Extras"
		Me.lblExtras.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblExtras.UseMnemonic = True
		Me.lblExtras.Visible = True
		' 
		' lblSubTotal
		' 
		Me.lblSubTotal.AutoSize = False
		Me.lblSubTotal.BackColor = System.Drawing.SystemColors.Control
		Me.lblSubTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblSubTotal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblSubTotal.Enabled = True
		Me.lblSubTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblSubTotal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblSubTotal.Location = New System.Drawing.Point(40, 40)
		Me.lblSubTotal.Name = "lblSubTotal"
		Me.lblSubTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblSubTotal.Size = New System.Drawing.Size(117, 17)
		Me.lblSubTotal.TabIndex = 9
		Me.lblSubTotal.Text = "Sub-Total"
		Me.lblSubTotal.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblSubTotal.UseMnemonic = True
		Me.lblSubTotal.Visible = True
		' 
		' lblPaymentAmount
		' 
		Me.lblPaymentAmount.AutoSize = False
		Me.lblPaymentAmount.BackColor = System.Drawing.SystemColors.Control
		Me.lblPaymentAmount.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPaymentAmount.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPaymentAmount.Enabled = True
		Me.lblPaymentAmount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblPaymentAmount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPaymentAmount.Location = New System.Drawing.Point(40, 16)
		Me.lblPaymentAmount.Name = "lblPaymentAmount"
		Me.lblPaymentAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPaymentAmount.Size = New System.Drawing.Size(117, 17)
		Me.lblPaymentAmount.TabIndex = 8
		Me.lblPaymentAmount.Text = "Payment Amount"
		Me.lblPaymentAmount.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblPaymentAmount.UseMnemonic = True
		Me.lblPaymentAmount.Visible = True
		' 
		' fraViewOptions
		' 
		Me.fraViewOptions.BackColor = System.Drawing.SystemColors.Control
		Me.fraViewOptions.Controls.Add(Me._optView_1)
		Me.fraViewOptions.Controls.Add(Me._optView_0)
		Me.fraViewOptions.Controls.Add(Me.cboFilter)
		Me.fraViewOptions.Controls.Add(Me.lblFilter)
		Me.fraViewOptions.Enabled = True
		Me.fraViewOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.fraViewOptions.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraViewOptions.Location = New System.Drawing.Point(352, 4)
		Me.fraViewOptions.Name = "fraViewOptions"
		Me.fraViewOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraViewOptions.Size = New System.Drawing.Size(357, 32)
		Me.fraViewOptions.TabIndex = 15
		Me.fraViewOptions.Visible = True
		' 
		' _optView_1
		' 
		Me._optView_1.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optView_1.BackColor = System.Drawing.SystemColors.Control
		Me._optView_1.CausesValidation = True
		Me._optView_1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optView_1.Checked = False
		Me._optView_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._optView_1.Enabled = True
		Me._optView_1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._optView_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optView_1.Location = New System.Drawing.Point(72, 12)
		Me._optView_1.Name = "_optView_1"
		Me._optView_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optView_1.Size = New System.Drawing.Size(97, 13)
		Me._optView_1.TabIndex = 23
		Me._optView_1.TabStop = True
		Me._optView_1.Text = "Compressed"
		Me._optView_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optView_1.Visible = True
		' 
		' _optView_0
		' 
		Me._optView_0.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optView_0.BackColor = System.Drawing.SystemColors.Control
		Me._optView_0.CausesValidation = True
		Me._optView_0.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optView_0.Checked = True
		Me._optView_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._optView_0.Enabled = True
		Me._optView_0.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me._optView_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optView_0.Location = New System.Drawing.Point(8, 12)
		Me._optView_0.Name = "_optView_0"
		Me._optView_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optView_0.Size = New System.Drawing.Size(81, 13)
		Me._optView_0.TabIndex = 22
		Me._optView_0.TabStop = True
		Me._optView_0.Text = "Normal"
		Me._optView_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optView_0.Visible = True
		' 
		' cboFilter
		' 
		Me.cboFilter.BackColor = System.Drawing.SystemColors.Window
		Me.cboFilter.CausesValidation = True
		Me.cboFilter.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboFilter.Enabled = True
		Me.cboFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.cboFilter.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboFilter.IntegralHeight = True
		Me.cboFilter.Location = New System.Drawing.Point(216, 8)
		Me.cboFilter.Name = "cboFilter"
		Me.cboFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboFilter.Size = New System.Drawing.Size(136, 21)
		Me.cboFilter.Sorted = False
		Me.cboFilter.TabIndex = 16
		Me.cboFilter.TabStop = True
		Me.cboFilter.Visible = True
		' 
		' lblFilter
		' 
		Me.lblFilter.AutoSize = True
		Me.lblFilter.BackColor = System.Drawing.SystemColors.Control
		Me.lblFilter.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblFilter.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblFilter.Enabled = True
		Me.lblFilter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lblFilter.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblFilter.Location = New System.Drawing.Point(184, 12)
		Me.lblFilter.Name = "lblFilter"
		Me.lblFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblFilter.Size = New System.Drawing.Size(28, 13)
		Me.lblFilter.TabIndex = 21
		Me.lblFilter.Text = "Filter"
		Me.lblFilter.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblFilter.UseMnemonic = True
		Me.lblFilter.Visible = True
		' 
		' txtExcess
		' 
		Me.txtExcess.AcceptsReturn = True
		Me.txtExcess.AutoSize = False
		Me.txtExcess.BackColor = System.Drawing.SystemColors.Window
		Me.txtExcess.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtExcess.CausesValidation = True
		Me.txtExcess.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtExcess.Enabled = False
		Me.txtExcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.txtExcess.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtExcess.HideSelection = True
		Me.txtExcess.Location = New System.Drawing.Point(218, 13)
		Me.txtExcess.MaxLength = 0
		Me.txtExcess.Multiline = False
		Me.txtExcess.Name = "txtExcess"
		Me.txtExcess.ReadOnly = False
		Me.txtExcess.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtExcess.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtExcess.Size = New System.Drawing.Size(65, 19)
		Me.txtExcess.TabIndex = 17
		Me.txtExcess.TabStop = True
		Me.txtExcess.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtExcess.Visible = True
		' 
		' lvwItems
		' 
		Me.lvwItems.BackColor = System.Drawing.SystemColors.Window
		Me.lvwItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.lvwItems.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwItems.HideSelection = False
		Me.lvwItems.LabelEdit = True
		Me.lvwItems.LabelWrap = True
		Me.lvwItems.Location = New System.Drawing.Point(8, 36)
		Me.lvwItems.MultiSelect = True
		Me.lvwItems.Name = "lvwItems"
		Me.lvwItems.Size = New System.Drawing.Size(705, 249)
		Me.lvwItems.TabIndex = 19
		Me.lvwItems.View = System.Windows.Forms.View.Details
		' 
		' _tabLossSchedule_TabPage1
		' 
		Me._tabLossSchedule_TabPage1.Text = "&2 - Client"
		' 
		' _tabLossSchedule_TabPage2
		' 
		Me._tabLossSchedule_TabPage2.Text = "&3 - Policy"
		' 
		' frmInterface
		' 
		Me.AcceptButton = Me.cmdCancel
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ClientSize = New System.Drawing.Size(729, 543)
		Me.ControlBox = True
		Me.Controls.Add(Me.cmdCancel)
		Me.Controls.Add(Me.cmdOK)
		Me.Controls.Add(Me.cmdApply)
		Me.Controls.Add(Me.tabLossSchedule)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Enabled = True
		Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.HelpButton = False
		Me.KeyPreview = False
		Me.Location = New System.Drawing.Point(4, 23)
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Name = "frmInterface"
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Text = "Form1"
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		CType(Me.listViewHelper1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tabLossSchedule.ResumeLayout(False)
		Me._tabLossSchedule_TabPage0.ResumeLayout(False)
		Me.fraTotals.ResumeLayout(False)
		Me.fraViewOptions.ResumeLayout(False)
		Me.ResumeLayout(False)
	End Sub
	Sub InitializeoptView()
		Me.optView(1) = _optView_1
		Me.optView(0) = _optView_0
	End Sub
#End Region 
End Class