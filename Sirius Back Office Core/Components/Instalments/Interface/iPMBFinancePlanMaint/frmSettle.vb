Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Partial Friend Class frmSettle
    Inherits System.Windows.Forms.Form
	
	'================================
	'Private Variables for Properties
	'================================
	Private m_bOK As Boolean
	Private m_vFinancePlan( ,  ) As Object
	Private m_crSettleAmount As Decimal
	Private m_crSettleRefund As Decimal
	Private m_sSettleFormatted As String = ""
	Private _m_oFormFields As iPMFormControl.FormFields = Nothing
	Private Property m_oFormFields() As iPMFormControl.FormFields
		Get
			If _m_oFormFields Is Nothing Then
				_m_oFormFields = New iPMFormControl.FormFields()
			End If
			Return _m_oFormFields
		End Get
		Set(ByVal Value As iPMFormControl.FormFields)
			_m_oFormFields = value
		End Set
	End Property
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'===========================
	'Read-only Public Properties
	'===========================
	Public ReadOnly Property SettleAmount() As Decimal
		Get
			Return m_crSettleAmount
		End Get
	End Property
	Public ReadOnly Property SettleRefund() As Decimal
		Get
			Return m_crSettleRefund
		End Get
	End Property
	Public ReadOnly Property OK() As Boolean
		Get
			Return m_bOK
		End Get
	End Property
	'============================
	'Read/Write Public Properties
	'============================
	Public Property FinancePlan() As Object
		Get
			Return VB6.CopyArray(m_vFinancePlan)
		End Get
		Set(ByVal Value As Object)
			m_vFinancePlan = Value
		End Set
	End Property
	
	'===============
	'Private Methods
	'===============
	Private Sub Recalc()

        m_lReturn = g_oBusiness.SettlePlanCalculate(v_vPremiumFinance:=m_vFinancePlan, r_crSettlement:=m_crSettleAmount, r_crRefund:=m_crSettleRefund, r_dtNextInstalmentDate:=#12/30/1899#, r_dtNextInstalmentDatePlus1:=#12/30/1899#, r_dtLastInstalmentDate:=#12/30/1899#, r_dtLastPaidInstalmentDate:=#12/30/1899#, r_vSettlementFormatted:=m_sSettleFormatted)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", frmSettle, Settle Plan Calculate Error")
			m_bOK = False
			Me.Hide()
			Exit Sub
		End If
		
		lblSettle.Text = "The Settlement Figure for the above date will " & "be " & m_sSettleFormatted & "."
	End Sub
	
	'===========
	'Form Events
	'===========
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_bOK = False
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		If MessageBox.Show("Are you sure you wish to settle this plan using the " & "calculated figures?", "Settle Plan", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
			m_bOK = True
			Me.Hide()
		End If
	End Sub
	

	Private Sub frmSettle_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		iPMFunc.CenterForm(Me)
		m_oFormFields.AddNewFormField(txtSettleDate, gPMConstants.PMEFormatStyle.PMFormatDateShort, gPMConstants.PMEDataType.PMLong,  , gPMConstants.PMEMandatoryStatus.PMMandatory)
		m_oFormFields.FormatControl(txtSettleDate, DateTime.Today.AddMonths(1))
		Recalc()
	End Sub
	Private Sub txtSettleDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSettleDate.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSettleDate)
	End Sub
	Private Sub txtSettleDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSettleDate.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSettleDate)
		Recalc()
	End Sub
End Class