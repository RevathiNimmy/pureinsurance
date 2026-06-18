Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Friend Partial Class frmBankAccountRuleEdit
	Inherits System.Windows.Forms.Form
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_sBankAccountName As String = ""
	Private m_lBankAccountId As Integer
	Private m_lBankAccountRuleID As Integer
	Private m_bProcessComplete As Boolean
	Private m_sBankAccountNo As String = ""
	Private m_oBusiness As Object
	Private m_lErrorNumber As Integer
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Const ACClass As String = "frmBankAccountRuleEdit"
	
	Public Property Task() As Integer
		Get
			
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iTask = Value
			
		End Set
	End Property
	
	Public Property BankAccountNo() As String
		Get
			
			Return m_sBankAccountNo
			
		End Get
		Set(ByVal Value As String)
			
			m_sBankAccountNo = Value
			
		End Set
	End Property
	Public Property BankAccountName() As String
		Get
			
			Return m_sBankAccountName
			
		End Get
		Set(ByVal Value As String)
			
			m_sBankAccountName = Value
			
		End Set
	End Property
	
	Public Property BankAccountId() As String
		Get
			
			Return CStr(m_lBankAccountId)
			
		End Get
		Set(ByVal Value As String)
			
			m_lBankAccountId = CInt(Value)
			
		End Set
	End Property
	
	Public Property BankAccountRuleID() As Integer
		Get
			
			Return m_lBankAccountRuleID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lBankAccountRuleID = Value
			
		End Set
	End Property
	
	
	Public Property Business() As Object
		Get
			
			Return m_oBusiness
			
		End Get
		Set(ByVal Value As Object)
			
			m_oBusiness = Value
			
		End Set
	End Property
	
	Public Property ProcessComplete() As Boolean
		Get
			Return m_bProcessComplete
		End Get
		Set(ByVal Value As Boolean)
			m_bProcessComplete = Value
		End Set
	End Property
	
	Public Sub SetCaptions()
		

        'developer guide no. 51 (guide)
        pnlBankAccount.Name = m_sBankAccountName.Trim() & " / " & m_sBankAccountNo.Trim()
		
		Select Case m_iTask
			Case gPMConstants.PMEComponentAction.PMAdd
				Me.Text = "Add Bank Account Rule"
			Case gPMConstants.PMEComponentAction.PMView
				Me.Text = "View Bank Account Rule"
			Case gPMConstants.PMEComponentAction.PMEdit
				Me.Text = "Edit Bank Acount Rule"
		End Select
		
	End Sub
	
	Public Function DisplayRule(ByVal v_lMediaTypeID As Integer, ByVal v_iMatchToTransdetail As Integer, ByVal v_iMatchAccountCode As Integer, ByVal v_iCodeIsMerchantNumber As Integer, ByVal v_iMatchBatchNumber As Integer, ByVal v_iBatchIsRemitCode As Integer, ByVal v_iMatchChequeNumber As Integer, ByVal v_iMatchAmount As Integer, ByVal v_iMatchDate As Integer, ByVal v_iSkipIfReasonNull As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			cboPMLookupMediaType.ItemId = v_lMediaTypeID
			optTransactionRule(v_iMatchToTransdetail).Checked = True
			chkMatchCode.CheckState = v_iMatchAccountCode
			chkMatchBatchReference.CheckState = v_iMatchBatchNumber
			chkMatchChequeNumber.CheckState = v_iMatchChequeNumber
			chkMatchAmount.CheckState = v_iMatchAmount
			chkMatchDate.CheckState = v_iMatchDate
			chkSkip.CheckState = v_iSkipIfReasonNull
			chkCodeIsMerchantNumber.CheckState = v_iCodeIsMerchantNumber
			chkReferenceIsRemitCode.CheckState = v_iBatchIsRemitCode
			
			'call click events for validation
			chkCodeIsMerchantNumber_CheckStateChanged(chkCodeIsMerchantNumber, New EventArgs())
			optTransactionRule_CheckedChanged(optTransactionRule(v_iMatchToTransdetail), New EventArgs())
			
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				Frame1.Enabled = False
				cboPMLookupMediaType.Enabled = False
				fraRuleType.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayRule", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub chkCodeIsMerchantNumber_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCodeIsMerchantNumber.CheckStateChanged
		
		If chkCodeIsMerchantNumber.CheckState = CheckState.Checked Then
			chkCodeIsMerchantNumber.Text = "Code is Merchant Number"
		Else
			chkCodeIsMerchantNumber.Text = "Code is Account Code"
		End If
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_bProcessComplete = False
		Me.Hide()
		Exit Sub
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		Dim lNewRuleID As Integer
		Dim iTransactionRule As Integer
		
		Try 
			
			If optTransactionRule(0).Checked Then
				iTransactionRule = 0
			ElseIf optTransactionRule(1).Checked Then 
				iTransactionRule = 1
			Else
				iTransactionRule = 2
			End If
			
			
			Select Case Task
				Case gPMConstants.PMEComponentAction.PMAdd

					m_lReturn = m_oBusiness.AddBankAccountRule(lNewRuleID, m_lBankAccountId, Me.cboPMLookupMediaType.ItemId, iTransactionRule, chkMatchCode.CheckState, chkCodeIsMerchantNumber.CheckState, chkMatchBatchReference.CheckState, chkReferenceIsRemitCode.CheckState, chkMatchChequeNumber.CheckState, chkMatchAmount.CheckState, chkMatchDate.CheckState, chkSkip.CheckState, v_iActive:=1)
					
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						MessageBox.Show("Error adding new rule to the database", "Add Rule", MessageBoxButtons.OK)
						m_bProcessComplete = False
						Me.Hide()
						Exit Sub
					Else
						m_bProcessComplete = True
						Me.Hide()
						Exit Sub
					End If
					
					
				Case gPMConstants.PMEComponentAction.PMEdit

					m_lReturn = m_oBusiness.UpdateBankAccountRule(m_lBankAccountRuleID, m_lBankAccountId, cboPMLookupMediaType.ItemId, iTransactionRule, chkMatchCode.CheckState, chkCodeIsMerchantNumber.CheckState, chkMatchBatchReference.CheckState, chkReferenceIsRemitCode.CheckState, chkMatchChequeNumber.CheckState, chkMatchAmount.CheckState, chkMatchDate.CheckState, chkSkip.CheckState, v_iActive:=1)
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						MessageBox.Show("Error updating the database record", "Add Rule", MessageBoxButtons.OK)
						m_bProcessComplete = False
						Me.Hide()
						Exit Sub
					Else
						m_bProcessComplete = True
						Me.Hide()
						Exit Sub
					End If
				Case gPMConstants.PMEComponentAction.PMView
					Me.Hide()
					Exit Sub
					
			End Select
		
		Catch excep As System.Exception
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayRule", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			
		End Try
		
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub optTransactionRule_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optTransactionRule_2.CheckedChanged, _optTransactionRule_1.CheckedChanged, _optTransactionRule_0.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			Dim Index As Integer = Array.IndexOf(optTransactionRule, eventSender)
			Select Case Index
				Case 1, 2
					chkMatchChequeNumber.CheckState = CheckState.Unchecked
					chkReferenceIsRemitCode.CheckState = CheckState.Unchecked
					chkMatchChequeNumber.Enabled = False
					chkReferenceIsRemitCode.Enabled = False
				Case 0
					chkMatchChequeNumber.Enabled = True
					chkReferenceIsRemitCode.Enabled = True
			End Select
		End If
	End Sub
End Class
