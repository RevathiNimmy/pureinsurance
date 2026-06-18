Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 22 Oct 2009
	'
	' Description: Main interface.
	'
	' Edit History:
	' Cash Deposit
	'
	' Created By : Renuka
	'
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "frmInterface"
	
	Private m_iLanguageID As Integer
	Private m_lSourceID As Integer
	Private m_iUserId As Integer
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_sStepStatus As String = ""
	
	Private m_bOKCLICK As Boolean
	Private m_sCallingAppName As String = ""
	
	Private m_lTranCurrencyId As Integer
	Private m_lSelPartyType As MainModule.ENBGPartyType
	
	Private m_dtCoverFromDate As Date
	
	Private m_lSelectedArrayIndexOnTag As Integer
	Private m_lListSelectedItem As Integer
	Private m_lSelectedTag As Integer
	' Stores the return value for the a function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	Dim m_bIsInitialised As Boolean
	
	Private m_lSelectedCashDepositID As Integer
	Private m_lSelectedAccountID As Integer
	Private m_sSelectedCDRef As String = ""
	Private m_lInsuranceFolderCnt As Integer
	Private m_lProductId As Integer
	Private m_sBusinessTypeCode As String = ""
	Private m_lPartyCnt As Integer
	Private m_lAgentCnt As Integer
	Private m_sPartyCode As String = ""
	Private m_sPartyName As String = ""
	Private m_sAgentCode As String = ""
	Private m_sAgentName As String = ""
	Private m_sAgentType As String = ""
	Private m_vCashDepositDetails( ,  ) As Object
	
	Private m_lInsuranceFileCnt As Integer
	Private m_dtPolicyIssueDate As Date
    Private m_crTotalPremium As Decimal
    'developer guide no. 101
    Private m_vPrePayment As Object
	Private m_lPaymentAccountID As Integer
	Private m_iDebitAgainst As Integer
	Private m_vCreditTransactions( ,  ) As Object
	Private Const ACLockName As String = "CashDepositAccount"
	
	'Start - Prakash - PN 65557
	Private m_crEffectiveBasePremium As Decimal
	Private m_lBaseCurrencyID As Integer
	Private m_sBaseCurrencyCode As String = ""
	Private m_lTransactionCurrencyID As Integer
	Private m_sTransactionCurrencyCode As String = ""
	'End - Prakash - PN 65557
	
	'Start - Prakash - PN 65531
	Private m_crEffectivePremium As Decimal
	Private m_crLeadAgentCommission As Decimal
	Private m_crLeadAgentTax As Decimal
	
	Public Property LeadAgentCommission() As Decimal
		Get
			Return m_crLeadAgentCommission
		End Get
		Set(ByVal Value As Decimal)
			m_crLeadAgentCommission = Value
		End Set
	End Property
	
	
	Public Property LeadAgentTax() As Decimal
		Get
			Return m_crLeadAgentTax
		End Get
		Set(ByVal Value As Decimal)
			m_crLeadAgentTax = Value
		End Set
	End Property
	'End - Prakash - PN 65531
	
	Public Property PolicyIssueDate() As Date
		Get
			Return m_dtPolicyIssueDate
		End Get
		Set(ByVal Value As Date)
			m_dtPolicyIssueDate = Value
		End Set
	End Property
	
    'developer guide no. 101
    Public Property PrePayment() As Object
        Get
            Return m_vPrePayment
        End Get
        Set(ByVal Value As Object) 'developer guide no. 101

            m_vPrePayment = Value
        End Set
    End Property
	
	
	Public Property PaymentAccountID() As Integer
		Get
			Return m_lPaymentAccountID
		End Get
		Set(ByVal Value As Integer)
			m_lPaymentAccountID = Value
		End Set
	End Property
	
	
	Public Property DebitAgainst() As Integer
		Get
			Return m_iDebitAgainst
		End Get
		Set(ByVal Value As Integer)
			m_iDebitAgainst = Value
		End Set
	End Property
	
	
	Public Property CreditTransactions() As Object
		Get
			Return VB6.CopyArray(m_vCreditTransactions)
		End Get
		Set(ByVal Value As Object)
			m_vCreditTransactions = Value
		End Set
	End Property
	
	Public Property OKCLICK() As Boolean
		Get
			Return m_bOKCLICK
		End Get
		Set(ByVal Value As Boolean)
			m_bOKCLICK = Value
		End Set
	End Property
	
	
	Public Property CoverFromDate() As Date
		Get
			Return m_dtCoverFromDate
		End Get
		Set(ByVal Value As Date)
			m_dtCoverFromDate = Value
		End Set
	End Property
	
	
	Public Property TotalPremium() As Decimal
		Get
			Return m_crTotalPremium
		End Get
		Set(ByVal Value As Decimal)
			m_crTotalPremium = Value
		End Set
	End Property
	Public Property TranCurrencyId() As Integer
		Get
			Return m_lTranCurrencyId
		End Get
		Set(ByVal Value As Integer)
			m_lTranCurrencyId = Value
		End Set
	End Property
	'
	
	Public Property SelPartyType() As MainModule.ENBGPartyType
		Get
			Return m_lSelPartyType
		End Get
		Set(ByVal Value As MainModule.ENBGPartyType)
			m_lSelPartyType = Value
		End Set
	End Property
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			m_lNavigate = Value
			
		End Set
	End Property
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			m_lProcessMode = Value
		End Set
	End Property
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			m_sTransactionType = Value
		End Set
	End Property
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	Public Property InsuranceFileCnt() As Integer
		Get
			Dim result As Integer = 0
			Return result
		End Get
		Set(ByVal Value As Integer)
			m_lInsuranceFileCnt = Value
		End Set
	End Property
	'UPGRADE_NOTE: (7001) The following declaration (get ListSelectedItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ListSelectedItem() As Integer
		'Return m_lListSelectedItem
	'End Function
	'UPGRADE_NOTE: (7001) The following declaration (let ListSelectedItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub ListSelectedItem(ByVal Value As Integer)
		'm_lListSelectedItem = Value
	'End Sub
	
	Private Function GetCDsForPolicy() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetCDsForPolicy"
        Dim lPartyCnt As Integer
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue

		
		If m_lSelPartyType = MainModule.ENBGPartyType.Client Then
			optClient.Checked = True
			lPartyCnt = m_lPartyCnt
			txtPartyName.Text = m_sPartyName
			txtPartyCode.Text = m_sPartyCode
			lPartyCnt = m_lPartyCnt
			'Start - Prakash - PN 65531
			m_crEffectivePremium = m_crTotalPremium
			'End - Prakash - PN 65531
		ElseIf m_lSelPartyType = MainModule.ENBGPartyType.agent Then 
			optAgent.Checked = True
			lPartyCnt = m_lAgentCnt
			txtPartyName.Text = m_sAgentName
			txtPartyCode.Text = m_sAgentCode
			'Start - Prakash - PN 65531
			If m_sAgentType.Trim().ToLower() = "broker" Or m_sAgentType.Trim().ToLower() = "intermed" Then
				m_crEffectivePremium = m_crTotalPremium - m_crLeadAgentCommission - m_crLeadAgentTax
			Else
				m_crEffectivePremium = m_crTotalPremium
			End If
			'End - Prakash - PN 65531
		End If
		
		'Start - Prakash - PN 65557
		'CashDeposit process uses amounts in base currency.So convert the policy premium to base currency

		m_lReturn = m_oBusiness.ConvertPolicyAmountToBaseCurrency(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_crPolicyAmount:=m_crEffectivePremium, v_crBaseAmount:=m_crEffectiveBasePremium, v_lBaseCurrencyID:=m_lBaseCurrencyID, v_sBaseCurrencyCode:=m_sBaseCurrencyCode, v_lTransactionCurrencyID:=m_lTransactionCurrencyID, v_sTransactionCurrencyCode:=m_sTransactionCurrencyCode)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "Failed to execute bSIRPolicyCashDeposit.ConvertPolicyAmountToBaseCurrency", gPMConstants.PMELogLevel.PMLogError)
		End If
		'End - Prakash - PN 65557
		
		If m_crTotalPremium > 0 Then
			'Start - Prakash - PN 65557 - changed value for total premium paramater

			m_lReturn = m_oBusiness.GetCDsForPolicy(v_lPartyCnt:=lPartyCnt, v_lProductId:=m_lProductId, v_lSourceId:=m_lSourceID, v_crTotalPremium:=m_crEffectiveBasePremium, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_vPrePayment:=m_vPrePayment, v_dtCoverStartDate:=m_dtCoverFromDate, v_dtPolicyIssueDate:=m_dtPolicyIssueDate, r_vCashDepositDetails:=m_vCashDepositDetails)
			'End - Prakash - PN 65557 - changed value for total premium paramater
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "Failed to execute bSIRPolicyCashDeposit.GetCDsForPolicy", gPMConstants.PMELogLevel.PMLogError)
			End If
		ElseIf m_crTotalPremium < 0 Then 

			m_lReturn = m_oBusiness.GetCDPaymentHistoryForPolicy(v_lPartyCnt:=lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_vCashDepositDetails:=m_vCashDepositDetails)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "Failed to execute bSIRPolicyCashDeposit.GetCDPaymentHistoryForPolicy", gPMConstants.PMELogLevel.PMLogError)
			End If
		End If
		

		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		


		

		End Try
		Return result
	End Function
	
	Private Function GetReceiptsForAllocation() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetReceiptsForAllocation"
        Dim lPartyCnt As Integer
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue

		
		If m_lSelPartyType = MainModule.ENBGPartyType.Client Then
			lPartyCnt = m_lPartyCnt
		ElseIf m_lSelPartyType = MainModule.ENBGPartyType.agent Then 
			lPartyCnt = m_lAgentCnt
		End If
		
		If m_crTotalPremium > 0 Then
			'Start - Prakash - PN 65557 - changed value for total premium paramater

			m_lReturn = m_oBusiness.GetCDReceiptsForAllocation(v_lCashDepositId:=m_lSelectedCashDepositID, v_crTotalPremium:=m_crEffectiveBasePremium, v_vPrePayment:=m_vPrePayment, v_dtCoverStartDate:=m_dtCoverFromDate, v_dtPolicyIssueDate:=m_dtPolicyIssueDate, r_vReceiptDetails:=m_vCreditTransactions)
			'End - Prakash - PN 65557 - changed value for total premium paramater
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "Failed to execute bSIRPolicyCashDeposit.GetCDReceiptsForAllocation", gPMConstants.PMELogLevel.PMLogError)
			End If
			'Start - Prakash - PN 65554 -Code commented accoring to new functionality. It can be removed.
			'    ElseIf m_crTotalPremium < 0 Then
			'        'Start - Prakash - PN 65557 - changed value for total premium paramater
			'        m_lReturn = m_oBusiness.GetCDRecieptsForRefund( _
			''                                                 v_lCashDepositId:=m_lSelectedCashDepositID, _
			''                                                 v_crTotalPremium:=m_crEffectiveBasePremium, _
			''                                                 r_vReceiptDetails:=m_vCreditTransactions)
			'        'Start - Prakash - PN 65557 - changed value for total premium paramater
			'        If m_lReturn <> PMTrue Then
			'            RaiseError kMethodName, "Failed to execute bSIRPolicyCashDeposit.GetCDRecieptsForRefund", PMLogError
			'        End If
			'End - Prakash - PN 65554
		End If
		

		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		


		

		End Try
		Return result
	End Function
	
	Public Function GetBusiness() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "GetBusiness"
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = CType(GetCDsForPolicy(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetCDsForPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	Private Function PopulateCDDetailsList() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "PopulateCDDetailsList"
		
        Dim oListItem As ListViewItem
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		
		
		If gPMFunctions.IsArrayEmpty(m_vCashDepositDetails) Then
			lvwCDList.Items.Clear()
			Return result
		End If
		
		'Set max rows to number of addresses - though must be at least 5
		lvwCDList.Items.Clear()
		SetupPolicyCashDepositDetailsListView()
		
		For iCount As Integer = m_vCashDepositDetails.GetLowerBound(1) To m_vCashDepositDetails.GetUpperBound(1)
			'Set oListItem = lvwCDList.ListItems.Add(Text:=Trim(m_vCashDepositDetails(ENCashDeposit.CDID, iCount)), SmallIcon:="history")
			oListItem = lvwCDList.Items.Add("")
			ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexCDRef).Text = CStr(m_vCashDepositDetails(MainModule.ENCashDeposit.CDRef, iCount)).Trim()
			ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexBalance).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(CStr(m_vCashDepositDetails(MainModule.ENCashDeposit.Balance, iCount)).Trim())))
			
            'developer guide no. 40
            ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexCreatedDate).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=gPMFunctions.ToSafeDate(CStr(m_vCashDepositDetails(MainModule.ENCashDeposit.DateCreated, iCount).Trim())))
			ListViewHelper.GetListViewSubItem(oListItem, kCashDepositColHIndexAccountID).Text = CStr(m_vCashDepositDetails(MainModule.ENCashDeposit.AccountID, iCount)).Trim()
			
			oListItem.Tag = CStr(m_vCashDepositDetails(MainModule.ENCashDeposit.CDID, iCount)).Trim()
		Next iCount
		
		

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		


		

		End Try
		Return result
	End Function
	
	Public Function BusinessToInterface() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "BusinessToInterface"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = CType(PopulateCDDetailsList(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "PopulateCDDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	'Start - Renuka - Changes according to the WPR85 process sheet updation - deleted and added some code
	Public Function InterfaceToData() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "InterfaceToData"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		If m_lSelectedCashDepositID > 0 Then
			m_lReturn = CType(GetReceiptsForAllocation(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

				If m_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_lPaymentAccountID, v_lUserID:=m_iUserId) <> gPMConstants.PMEReturnCode.PMTrue Then
					MessageBox.Show("Failed to unlock KeyName: " & ACLockName & "for " & CStr(m_lPaymentAccountID), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
				End If
				gPMFunctions.RaiseError(kMethodName, "GetReceiptsForAllocation Failed", gPMConstants.PMELogLevel.PMLogError)
			Else
				'Start - Prakash - PN 65554
				If m_crTotalPremium > 0 Then
					If Information.IsArray(m_vCreditTransactions) Then
						m_lPaymentAccountID = CInt(m_vCreditTransactions(0, 0))
					Else

						If m_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_lPaymentAccountID, v_lUserID:=m_iUserId) <> gPMConstants.PMEReturnCode.PMTrue Then
							MessageBox.Show("Failed to unlock KeyName: " & ACLockName & "for " & CStr(m_lPaymentAccountID), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
						End If
						gPMFunctions.RaiseError(kMethodName, "Unable to get allocation details for " & m_sSelectedCDRef, gPMConstants.PMELogLevel.PMLogError)
					End If
				ElseIf m_crTotalPremium < 0 Then 
					m_lPaymentAccountID = m_lSelectedAccountID
				End If
				'End - Prakash -PN 65554
				
				m_iDebitAgainst = gPMConstants.PMDebitAgainst.PMDebitAgainstCashDeposit
			End If
		Else
			MessageBox.Show("Select atleast one Cash Deposit to proceed further.", "Payment - Cash Depsosit", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	'End - Renuka - Changes according to the WPR85 process sheet updation
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_bOKCLICK = False
		'Hide the Form
		Me.Visible = False
		
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		' Click event of the OK button.
        Dim Item As ListViewItem = lvwCDList.FocusedItem
        Dim iSelectedItemCount As Integer
        Dim iCurrentSelectedIndex As Integer = 0
        Dim iSelectedIndex As Integer = 0
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK


            If lvwCDList.Items.Count = 0 Then
                MessageBox.Show("Select atleast one Cash Deposit to proceed further.", "Payment - Cash Depsosit", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                If lvwCDList.SelectedItems.Count = 0 Or lvwCDList.FocusedItem Is Nothing Then
                    lvwCDList.Items(0).Focused = True
                    lvwCDList.Items(0).Selected = True
                End If
            End If

            'developer guide no. moved the following code from the event handler lvwCDList_ItemCheck
            'start'
           

            If Item.Checked Then


                iCurrentSelectedIndex = Item.Index + 1

                For lListCount As Integer = 1 To lvwCDList.Items.Count
                    If lvwCDList.Items.Item(lListCount - 1).Checked Then
                        If lvwCDList.Items.Item(lListCount - 1).Index + 1 <> iCurrentSelectedIndex Then
                            iSelectedIndex = lvwCDList.Items.Item(lListCount - 1).Index + 1
                        End If
                        iSelectedItemCount += 1
                    End If
                Next

                If iSelectedItemCount > 1 Then
                    Item.Checked = False
                    Item.Selected = False
                    lvwCDList.Items.Item(iSelectedIndex - 1).Checked = True
                    lvwCDList.Items.Item(iSelectedIndex - 1).Selected = True
                    MessageBox.Show("Not more than one CD can be selected", "Payment - Cash Depsosit", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    m_lSelectedCashDepositID = gPMFunctions.ToSafeLong(Convert.ToString(Item.Tag))
                    m_lSelectedAccountID = CInt(Item.SubItems.Item(kCashDepositColHIndexAccountID).Text.Trim())
                    m_sSelectedCDRef = Item.SubItems.Item(kCashDepositColHIndexCDRef).Text.Trim()
                    Item.Checked = True
                    Item.Selected = True
                End If

            Else
                m_lSelectedCashDepositID = 0
                m_lSelectedAccountID = 0
                Item.Selected = False
            End If
            'End;'			

            m_lReturn = CType(ValidateForm(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lReturn = InterfaceToData()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            ElseIf (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                ' Everything OK, so we can hide the interface.
                m_bOKCLICK = True

                Me.Hide()
            End If

        Catch excep As System.Exception


            m_bOKCLICK = False
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
		
	End Sub
	Public Function Initialise() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "Initialise"
        Dim temp_m_oBusiness As Object = Nothing
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Check if already initialised
		If m_bIsInitialised Then
			Return result
		End If
		
		' Create an instance of the object manager.
		g_oObjectManager = New bObjectManager.ObjectManager()
		
		' Call the initialise method.
		m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' If UserID is 0 assume that user cancelled logon
		If g_oObjectManager.UserID = 0 Then
			' Exit application
			result = gPMConstants.PMEReturnCode.PMFalse
			Exit Function
		End If
		
		' Store the language ID from the object manager to the public variables,
		' to enable us to use them throughout the object.
		With g_oObjectManager
			m_iLanguageID = .LanguageID
			m_lSourceID = .SourceID
			m_iUserId = .UserID
		End With
		
		' Set the mouse pointer to busy.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		' Get an instance of the business object via the public object manager.

		m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRCashDeposit.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
		m_oBusiness = temp_m_oBusiness
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRCashDeposit.Business Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' hold Initialised status
		m_bIsInitialised = True
		
		
		Catch ex As Exception
		
		' Do Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		' Set the mouse pointer to normal.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Const kMethodName As String = "Form_Load"
		
		Try 
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			m_lReturn = CType(Initialise(), gPMConstants.PMEReturnCode)
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				gPMFunctions.RaiseError(kMethodName, "Initialise failed", gPMConstants.PMELogLevel.PMLogError)
				Exit Sub
			End If
			
			' Set the interface default values.
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				gPMFunctions.RaiseError(kMethodName, "SetupForm failed", gPMConstants.PMELogLevel.PMLogError)
				Exit Sub
			End If
			
			
			' Set the interface default values.
			m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				gPMFunctions.RaiseError(kMethodName, "SetupForm failed", gPMConstants.PMELogLevel.PMLogError)
				Exit Sub
			End If
			
			m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				gPMFunctions.RaiseError(kMethodName, "SetupForm failed", gPMConstants.PMELogLevel.PMLogError)
				Exit Sub
			End If
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
	End Sub
	
	Private Function SetInterfaceDefaults() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "SetInterfaceDefaults"
        Dim vPolicyDetails(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_lReturn = m_oBusiness.GetPolicyDetailsForCashDeposit(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vPolicyDetails:=vPolicyDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute bSIRPolicyCashDeposit.GetPolicyDetailsForCashDeposit", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vPolicyDetails) Then
                'Give Proper Error Message
            Else

                m_lInsuranceFolderCnt = gPMFunctions.ToSafeLong(vPolicyDetails(MainModule.ENPolicyDetails.InsuranceFolderCnt, vPolicyDetails.GetLowerBound(1)))

                m_lProductId = gPMFunctions.ToSafeLong(vPolicyDetails(MainModule.ENPolicyDetails.ProductId, vPolicyDetails.GetLowerBound(1)))

                m_sBusinessTypeCode = gPMFunctions.ToSafeString(vPolicyDetails(MainModule.ENPolicyDetails.BusinessType, vPolicyDetails.GetLowerBound(1)))

                m_lPartyCnt = gPMFunctions.ToSafeLong(vPolicyDetails(MainModule.ENPolicyDetails.PartyCnt, vPolicyDetails.GetLowerBound(1)))

                m_sPartyCode = gPMFunctions.ToSafeString(vPolicyDetails(MainModule.ENPolicyDetails.PartyCode, vPolicyDetails.GetLowerBound(1)))

                m_sPartyName = gPMFunctions.ToSafeString(vPolicyDetails(MainModule.ENPolicyDetails.PartyName, vPolicyDetails.GetLowerBound(1)))

                m_lAgentCnt = gPMFunctions.ToSafeLong(vPolicyDetails(MainModule.ENPolicyDetails.AgentCnt, vPolicyDetails.GetLowerBound(1)))

                m_sAgentCode = gPMFunctions.ToSafeString(vPolicyDetails(MainModule.ENPolicyDetails.AgentCode, vPolicyDetails.GetLowerBound(1)))

                m_sAgentName = gPMFunctions.ToSafeString(vPolicyDetails(MainModule.ENPolicyDetails.AgentName, vPolicyDetails.GetLowerBound(1)))

                m_sAgentType = gPMFunctions.ToSafeString(vPolicyDetails(MainModule.ENPolicyDetails.AgentType, vPolicyDetails.GetLowerBound(1)))
            End If

            optAgent.Visible = True
            optClient.Visible = True
            If m_sBusinessTypeCode.Trim().ToLower() = "direct" Then
                optClient.Visible = True
                optClient.Checked = True
                optAgent.Visible = False
                optClient.Left = optAgent.Left
                txtPartyName.Text = m_sPartyName
                txtPartyCode.Text = m_sPartyCode
            ElseIf m_sBusinessTypeCode.Trim().ToLower() <> "direct" Then
                If m_sAgentType.Trim().ToLower() = "broker" Then
                    optClient.Visible = False
                    optAgent.Visible = True
                    optAgent.Checked = True
                    txtPartyName.Text = m_sAgentName
                    txtPartyCode.Text = m_sAgentCode
                ElseIf m_sAgentType.Trim().ToLower() = "comm acc" Then
                    txtPartyName.Text = m_sPartyName
                    txtPartyCode.Text = m_sPartyCode
                    optClient.Visible = True
                    optClient.Checked = True
                    optAgent.Visible = False
                    optClient.Left = optAgent.Left
                ElseIf m_sAgentType.Trim().ToLower() = "intermed" Then
                    txtPartyName.Text = m_sAgentName
                    txtPartyCode.Text = m_sAgentCode
                    optClient.Visible = True
                    optAgent.Visible = True
                    optAgent.Checked = True
                    SelPartyType = MainModule.ENBGPartyType.agent
                End If
            End If

            m_lReturn = SetupPolicyCashDepositDetailsListView()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupPolicyCashDepositDetailsListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
		Return result
	End Function
	
	Private Function ValidateForm() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "ValidateForm"
        Dim crAvaliableBalance, crRunningBalance As Decimal
        Dim bUnLock As Boolean
        Dim sLockedBy As String = ""
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            'Start - Renuka - Changes according to the WPR85 process sheet updation

            'End - Renuka - Changes according to the WPR85 process sheet updation


            If m_lSelectedCashDepositID = 0 Then
                MessageBox.Show("Select atleast one Cash Deposit to proceed further.", "Payment - Cash Depsosit", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
                'Start - Renuka - Changes according to the WPR85 process sheet updation
            Else

                m_lReturn = m_oBusiness.LockKey(v_sKeyName:=ACLockName.Trim(), v_lKeyValue:=m_lSelectedAccountID, v_lUserID:=m_iUserId, r_sLockedBy:=sLockedBy)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If m_crTotalPremium > 0 Then
                        m_lReturn = CType(GetBalanceForCD(crAvaliableBalance, crRunningBalance), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bUnLock = True
                            gPMFunctions.RaiseError(kMethodName, "GetBalanceForCD Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        'Start - Prakash - PN 65557
                        If crAvaliableBalance < m_crEffectiveBasePremium Then
                            'End - Prakash - PN 65557
                            'Start - Prakash - PN 65447 -Changing the error message
                            MessageBox.Show("The selected Cash Deposit Account does not have sufficient available balance to process this transaction", "Payment - Cash Depsosit", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            'End - Prakash - PN 65447
                            lvwCDList.FocusedItem.Checked = False
                            bUnLock = True
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Return result
                        End If
                        'Start - Prakash - PN 65557
                        'developer guide no. 142
                        If m_vPrePayment = "1" And crRunningBalance < m_crEffectiveBasePremium Then
                            'End - Prakash - PN 65557
                            'Start - Prakash - PN 65447 -Changing the error message
                            MessageBox.Show("The selected Cash Deposit Account does not have sufficient running balance to process this transaction", "Payment - Cash Depsosit", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            'End - Prakash - PN 65447
                            lvwCDList.FocusedItem.Checked = False
                            bUnLock = True
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Return result
                        End If
                    End If
                Else
                    If sLockedBy = "ERROR" Then
                        MessageBox.Show("Failed to lock CashDeposit : " & m_sSelectedCDRef, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ElseIf sLockedBy <> "" Then
                        MessageBox.Show("The CashDeposit : " & m_sSelectedCDRef & " is being locked by " & sLockedBy & ". Please try again later.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
                'End - Renuka - Changes according to the WPR85 process sheet updation
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally
            If bUnLock Then

                If m_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_lPaymentAccountID, v_lUserID:=m_iUserId) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & "for " & CStr(m_lPaymentAccountID), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If



        End Try
		Return result
	End Function
	
	Private Sub lvwCDList_ItemCheck(ByVal eventSender As Object, ByVal eventArgs As ItemCheckEventArgs) Handles lvwCDList.ItemCheck
        'developer guide no. moved the code to cmdOk_click event handler because this logic is not needed here 
        'its required when we click OK button instead. Working as per the desired functionality.
        
	End Sub
	
	Private Sub lvwCDList_ItemClick(ByVal Item As ListViewItem)
		m_lListSelectedItem = Item.Index + 1 - 1
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub optAgent_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optAgent.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			Const kMethodName As String = "optAgent_Click"
			
			Try
			
			
			m_lSelPartyType = MainModule.ENBGPartyType.agent
			m_lReturn = CType(GetCDsForPolicy(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetValidBgsOnPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetValidBgsOnPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			

			Catch ex As Exception
			
			' DO Not Call any functions before here or the error will be lost
			iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CInt(""), excep:=ex)
			
			' If you want to rollback a transaction or something, do it here
			Finally


            End Try
        End If
	End Sub
	
	Private Sub optClient_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optClient.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			Const kMethodName As String = "optAgent_Click"
			
			Try
			
			
			m_lSelPartyType = MainModule.ENBGPartyType.Client
			m_lReturn = CType(GetCDsForPolicy(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetValidBgsOnPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			
			m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "GetValidBgsOnPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
			

			Catch ex As Exception
			
			' DO Not Call any functions before here or the error will be lost
			iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMELogLevel.PMLogError, excep:=ex)
			
			' If you want to rollback a transaction or something, do it here
			Finally
			
		

            End Try
        End If
	End Sub
	
	Private Function SetupPolicyCashDepositDetailsListView() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetupPolicyCashDepositDetailsListView"
		
		Dim lColWidth As Integer
		Dim sCaption As String = ""
		
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		lvwCDList.Columns.Clear()
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwSelect, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		
		lvwCDList.Columns.Insert(kCashDepositColHIndexSelect, "", sCaption, CInt(VB6.TwipsToPixelsX(1000)), HorizontalAlignment.Left, -1)
		

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwCDNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwCDList.Columns.Insert(kCashDepositColHIndexCDRef, "", sCaption, CInt(VB6.TwipsToPixelsX(3000)), HorizontalAlignment.Left, -1)


        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwAvailableBal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwCDList.Columns.Insert(kCashDepositColHIndexBalance, "", sCaption, CInt(VB6.TwipsToPixelsX(3000)), HorizontalAlignment.Left, -1)


        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwDateCreated, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		
		lvwCDList.Columns.Insert(kCashDepositColHIndexCreatedDate, "", sCaption, CInt(VB6.TwipsToPixelsX(3000)), HorizontalAlignment.Left, -1)
		lvwCDList.Columns.Insert(kCashDepositColHIndexAccountID, "", "Account ID", CInt(VB6.TwipsToPixelsX(10)), HorizontalAlignment.Left, -1)
		
		lvwCDList.Columns.Item(kCashDepositColHIndexAccountID).Width = CInt(0)
		lvwCDList.LabelEdit = False
		

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	'Start - Renuka - Changes according to the WPR85 process sheet updation
	Private Function GetBalanceForCD(ByRef crAvaliableBalance As Decimal, ByRef crRunningBalance As Decimal) As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "GetBalanceForCD"
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		m_lReturn = m_oBusiness.GetBalanceForCD(v_lCashDepositId:=m_lSelectedCashDepositID, v_dtCoverStartDate:=m_dtCoverFromDate, v_dtPolicyIssueDate:=m_dtPolicyIssueDate, v_vPrePayment:=m_vPrePayment, v_crAvaliableBalance:=crAvaliableBalance, v_crRunningBalance:=crRunningBalance)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "Failed to execute bSIRPolicyCashDeposit.GetBalanceForCD", gPMConstants.PMELogLevel.PMLogError)
		End If

		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		



		End Try
		Return result
	End Function
	'End - Renuka - Changes according to the WPR85 process sheet updation

    Private Sub lvwCDList_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwCDList.ItemChecked
        Dim lSlectedItemCtr As Integer = 0
        For Each lvItem As ListViewItem In lvwCDList.Items
            If lvItem.Checked Then
                lSlectedItemCtr += 1
                If lSlectedItemCtr > 1 Then
                    MessageBox.Show("Not more than one CD can be selected", "Payment - Cash Depsosit", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    lvItem.Checked = False
                    Exit Sub
                End If
                lvItem.Selected = True
                lvItem.Focused = True
            End If
        Next
    End Sub
End Class
