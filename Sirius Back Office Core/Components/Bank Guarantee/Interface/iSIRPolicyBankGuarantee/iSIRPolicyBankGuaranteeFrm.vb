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
    '?   The above tab will allow users to select the BG for making live the transaction(s). Based on the Agent Type selected (lead agents only), user will have the option to select either the client or agent BG’s.
    'o   If Agent Type is Broker then only Agent BG’s will be available.
    'o   If Agent Type is commission agent then only Client BG’s will be available.
    'o   If Intermediary then both BG’s will be available.
    '?   In case of “Broker” and “Commission Agent”, the Bank Guarantee tab will default the relevant BG’s of the Agent as per the rules detailed below
    '?   In case of “Intermediary”, user will have the choice to use either client or agent BG. Upon selecting the BG details are required to be displayed in the Grid.
    '?   In case of Direct Business, only the Client BG(s) are required to be displayed.


    '5.5.1   Rules/Validations for displaying the BG’s on the Bank Guarantee Tab
    '
    '•   Validate the Product, i.e. to retrieve only those BG(s), which are applicable based on the configuration in Bank Guarantee Maintenance
    '
    '•   Validate the Branch, i.e. to retrieve only those BG(s), which are applicable based on the configuration in Bank Guarantee Maintenance. Requirement is to validate the transaction branch
    '
    '•   Validation for BG start and Expiry date. On Bank Guarantee Tab, based on the cover from date of the policy, only those BG(s) are required to be retrieved which have the cover from date in between the BG Start and Expiry date, inclusive of both the dates
    '
    '•   Bank Guarantees, with BG Status as “Active” are required to be retrieved
    '
    '•   All the above four are required to be validated together, for displaying the applicable BG(s) on the tab
    '
    '5.5.2   Rules/Validations for Selecting BG on the Bank Guarantee Tab
    '
    '•   Only one BG can be selected for completing the transaction and making live the policy transaction
    '•   In case user has made a wrong selection, user will have to un-select and then re-select another BG
    '•   Upon attempting to select another BG, a message is required to be displayed “Only one Bank Guarantee can be selected”
    '5.5.3   Rules/Validations for Calculation the Due Date of BG
    'The due date of the BG cannot be earlier than the last date of the Next month on which the policy is issued.
    '5.5.4   Rules/Validations for “Make Live” for BG
    'Upon clicking the “Make Live” option, payment mode selected is “Bank Guarantee”, following validations are required to be checked, when the Hidden Product Option “Enable Pre-Payment Functionality” is enabled
    '
    '•   BG Available limit is sufficient for policy issuance i.e. the Gross Premium amount should be <= BG Available Limit, only in this scenario transaction to be converted as “Live”. Account posting are detailed in the 4.5 section
    '•   In case the BG Available Limit is not sufficient a message to be generated “The selected Bank Guarantee does not have sufficient available limit to process this transaction”
    '•   The BG Available Limit to reduced based on the premium amount used for transaction
    '
    'In case the Hidden Product Option “Enable Pre-payment functionality” is not enabled, then following validations is required to be checked
    '•   The BG Available Limit to reduced based on the premium amount used for transaction




    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17th August 2006
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' Bank Gaurantee Work
    '
    ' Created By :    Gaurav
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
	
	Private m_lSelectedItem As Integer
	
	Private m_sAgentType As String = ""
	Private m_sPolicyRef As String = ""
	Private m_bIsInitialised As Integer
	
	Private m_sBusinessTypeCode As String = ""
	
	Private m_lAgentCnt As Integer 'lLeadAgentCnt
	Private m_sPartyCode As String = ""
	Private m_sPartyName As String = ""
	
	Private m_sAgentCode As String = ""
	Private m_sAgentName As String = ""
	
	Private m_lPartyCnt As Integer 'PartyCnt
	
	Private m_lProductId As Integer 'm_lProductId
	Private m_lTranCurrencyId As Integer 'txtCurrency.Text
	Private m_vBankGuaranteeDetails( ,  ) As Object
	Private m_lSelPartyType As MainModule.ENBGPartyType
	
	Private m_lInsuranceFileCnt As Integer 'InsuranceFileCnt
	Private m_crTotalPremium As Decimal 'm_crTotalPolicyAmount
	Private m_lSelBGId As Integer 'Let the selected bg_id of list
	Private m_dtCoverFromDate As Date 'txtCoverFromDate
	
	
	Private m_lSelectedArrayIndexOnTag As Integer
	Private m_lListSelectedItem As Integer
	Private m_lSelectedTag As Integer
	
	' Stores the return value for the a function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	'Start - Sankar - Bank Guarantee Bug Fixing
	Private m_dtDuedate As Date
	'End - Sankar - Bank Guarantee Bug Fixing
	
	'Start - Sankar - Bank Guarantee Bug Fixing
	Public Property DueDate() As Date
		Get
			Return m_dtDuedate
		End Get
		Set(ByVal Value As Date)
			m_dtDuedate = Value
		End Set
	End Property
	'End - Sankar - Bank Guarantee Bug Fixing
	
	
	Public Property OKCLICK() As Boolean
		Get
			Return m_bOKCLICK
		End Get
		Set(ByVal Value As Boolean)
			m_bOKCLICK = Value
		End Set
	End Property
	
	
	Public Property BusinessTypeCode() As String
		Get
			Return m_sBusinessTypeCode
		End Get
		Set(ByVal Value As String)
			m_sBusinessTypeCode = Value
		End Set
	End Property
	
	
	Public Property CoverFromDate() As Decimal
		Get
			Return m_dtCoverFromDate.ToOADate
		End Get
		Set(ByVal Value As Decimal)
			m_dtCoverFromDate = DateTime.FromOADate(Value)
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
	
	
	Public Property ProductId() As Integer
		Get
			Return m_lProductId
		End Get
		Set(ByVal Value As Integer)
			m_lProductId = Value
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
	
	
	Public Property SelPartyType() As MainModule.ENBGPartyType
		Get
			Return m_lSelPartyType
		End Get
		Set(ByVal Value As MainModule.ENBGPartyType)
			m_lSelPartyType = Value
		End Set
	End Property
	
	
	Public Property AgentName() As String
		Get
			Return m_sAgentName
		End Get
		Set(ByVal Value As String)
			m_sAgentName = Value
		End Set
	End Property
	
	
	Public Property AgentCode() As String
		Get
			Return m_sAgentCode
		End Get
		Set(ByVal Value As String)
			m_sAgentCode = Value
		End Set
	End Property
	
	
	Public Property PartyName() As String
		Get
			Return m_sPartyName
		End Get
		Set(ByVal Value As String)
			m_sPartyName = Value
		End Set
	End Property
	
	
	Public Property PartyCode() As String
		Get
			Return m_sPartyCode
		End Get
		Set(ByVal Value As String)
			m_sPartyCode = Value
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
	
	Public WriteOnly Property AgentCnt() As Integer
		Set(ByVal Value As Integer)
			m_lAgentCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property PartyCnt() As Integer
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	
	Public Property AgentType() As String
		Get
			Return m_sAgentType
		End Get
		Set(ByVal Value As String)
			m_sAgentType = Value
		End Set
	End Property
	
	
	Private Property ListSelectedItem() As Integer
		Get
			Return m_lListSelectedItem
		End Get
		Set(ByVal Value As Integer)
			m_lListSelectedItem = Value
		End Set
	End Property
	
	Private ReadOnly Property SelectedArrayIndexOnTag() As Integer
		Get
			Dim result As Integer = 0
			If ListSelectedItem >= 0 Then

				m_lSelectedTag = Convert.ToString(lvwBGList.Items.Item(ListSelectedItem).Tag)
				
				m_lReturn = CType(SearchArrayIndexOnTag(lSelectedTag:=m_lSelectedTag, lSelectedArrayIndex:=m_lSelectedArrayIndexOnTag, lColumnId:=MainModule.ENBankGuarantee.RowIndex), gPMConstants.PMEReturnCode)
				
				result = m_lSelectedArrayIndexOnTag
			End If
			Return result
		End Get
	End Property
	
	Private Function SearchArrayIndexOnTag(ByVal lSelectedTag As Integer, ByRef lSelectedArrayIndex As Integer, ByVal lColumnId As MainModule.ENBankGuarantee) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SearchArrayIndexOnTag"
		
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		
		For lArrayCount As Integer = m_vBankGuaranteeDetails.GetLowerBound(1) To m_vBankGuaranteeDetails.GetUpperBound(1)
			If CDbl(m_vBankGuaranteeDetails(lColumnId, lArrayCount)) = lSelectedTag Then
				lSelectedArrayIndex = lArrayCount
				Exit For
			End If
		Next 
		

		
		Catch ex As Exception
		' Do Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally



		End Try
		Return result
	End Function
	
	Private Function BuildArrayIndex() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "BuildArrayIndex"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		If Information.IsArray(m_vBankGuaranteeDetails) Then
			For lBounds As Integer = m_vBankGuaranteeDetails.GetLowerBound(1) To m_vBankGuaranteeDetails.GetUpperBound(1)
				m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowIndex, lBounds) = lBounds
			Next lBounds
		End If
		
		
		

		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		



		End Try
		Return result
	End Function
	
	
	Private Function GetValidBgsOnPolicy() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetValidBgsOnPolicy"
		'Start - Sankar - Bank Guarantee Bug Fixing
		Dim dtDueDate As Date
		'End - Sankar - Bank Guarantee Bug Fixing
        Dim lPartyCnt As Integer
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue

		
		If m_lSelPartyType = MainModule.ENBGPartyType.Client Then
			optClient.Checked = True
			lPartyCnt = m_lPartyCnt
			txtPartyName.Text = m_sPartyName
			txtPartyCode.Text = m_sPartyCode
		ElseIf m_lSelPartyType = MainModule.ENBGPartyType.agent Then 
			optAgent.Checked = True
			lPartyCnt = m_lAgentCnt
			txtPartyName.Text = m_sAgentName
			txtPartyCode.Text = m_sAgentCode
		End If
		
		

		m_lReturn = m_oBusiness.GetValidBgsOnPolicy(lProductId:=m_lProductId, lSourceId:=m_lSourceID, dtCoverFromDate:=m_dtCoverFromDate, lInsuranceFileCnt:=m_lInsuranceFileCnt, lTransactionCurrencyId:=m_lTranCurrencyId, lPartyCnt:=lPartyCnt, crTotalPremium:=m_crTotalPremium, vBankGuaranteeDetails:=m_vBankGuaranteeDetails, r_dtDueDate:=dtDueDate) 'Sankar - Added DueDate
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "bSIRBankGuarantee.GetValidBgsOnPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
		ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then 
			'Start - Sankar - Bank Guarantee Bug Fixing
			DueDate = dtDueDate
			'End - Sankar - Bank Guarantee Bug Fixing
			m_lReturn = CType(BuildArrayIndex(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				gPMFunctions.RaiseError(kMethodName, "BuildArrayIndex Failed", gPMConstants.PMELogLevel.PMLogError)
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
	
	Public Function GetBusiness() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "GetBusiness"
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = CType(GetValidBgsOnPolicy(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetValidBgsOnPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
	
		End Try
		Return result
	End Function
    Private m_bItemAdded As Boolean = False
	Private Function PopulateBGDetailsList() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "PopulateBGDetailsList"
        Dim oListItem As ListViewItem
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		
		
		If gPMFunctions.IsArrayEmpty(m_vBankGuaranteeDetails) Then
			lvwBGList.Items.Clear()
			Return result
		End If
		
		'Set max rows to number of addresses - though must be at least 5
		lvwBGList.Items.Clear()
		
		For i As Integer = m_vBankGuaranteeDetails.GetLowerBound(1) To m_vBankGuaranteeDetails.GetUpperBound(1)
            m_bItemAdded = True
            oListItem = lvwBGList.Items.Add(CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankNameId, i)).Trim(), "history")

			ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBGRef).Text = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)).Trim()
			ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexAvailableBalance).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.AvailableBal, i)).Trim())))
			
			ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexExpiryDate).Text = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.ExpiryDate, i)).Trim()
            'Start - Sankar - Bank Guarantee Bug Fixing
            'developer guide no. 40
            ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexDueDate).Text = DueDate

            ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBankName).Text = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankName, i)).Trim()

			'End - Sankar - Bank Guarantee Bug Fixing
			oListItem.Tag = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowIndex, i))
			
		Next i





		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		


		

		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: BusinessToInterface
	'
	' Description: Updates all interface details from the business
	'              object.
	'
	' ***************************************************************** '
	Public Function BusinessToInterface() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "BusinessToInterface"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
        m_lReturn = CType(PopulateBGDetailsList(), gPMConstants.PMEReturnCode)
        m_bItemAdded = False
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "PopulateBGDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		
		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: BusinessToInterface
	'
	' Description: Updates all interface details from the business
	'              object.
	'
	' ***************************************************************** '
	Public Function InterfaceToData() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "InterfaceToData"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		For lCount As Integer = 1 To lvwBGList.Items.Count
			If lvwBGList.Items.Item(lCount - 1).Checked Then
				ListSelectedItem = lCount - 1
				m_lSelBGId = CInt(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, SelectedArrayIndexOnTag))
			End If
		Next 
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		
		End Try
		Return result
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_bOKCLICK = False
		'Hide the Form
		Me.Visible = False
		
	End Sub
	
	Private Function DataToBusiness() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "DataToBusiness"
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		m_lReturn = m_oBusiness.UpdateBGForPolicy(lInsuranceFileCnt:=m_lInsuranceFileCnt, crAmount:=m_crTotalPremium, lBGId:=m_lSelBGId, dtCoverFromDate:=m_dtCoverFromDate)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "m_oBusiness.UpdateBGForPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		

		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		



		End Try
		Return result
	End Function
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			
			m_lReturn = InterfaceToData()
			
			' Check the return value.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			m_lReturn = CType(ValidateForm(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			m_lReturn = DataToBusiness()
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
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
	
	'Private Function GetAccountID() As Long
	'On Error GoTo Ere_GetAccountId
	'
	'
	'    Dim vResultArray As Variant
	'    GetAccountID = PMTrue
	'
	'    ' Get the details from the business object.
	'    If optAgent.value = True Then
	'        m_lReturn& = m_oBusiness.GetAccountID(m_lAgentCnt, vResultArray)
	'    Else
	'        m_lReturn& = m_oBusiness.GetAccountID(m_lPartyCnt, vResultArray)
	'    End If
	'    If m_lReturn& <> PMTrue Then
	'        GetAccountID = PMFalse
	'        Exit Function
	'    End If
	'
	'    If (Not IsArray(vResultArray)) Then
	'        GetAccountID = PMFalse
	'        Exit Function
	'    End If
	'
	'    m_lAccountId = ToSafeLong(vResultArray(0, 0))
	'
	'
	'Exit Function
	'
	'Ere_GetAccountId:
	'' Log Error.
	'    LogMessage _
	''        iType:=PMLogOnError, _
	''        sMsg:="Failed to get the AccountID", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:="GetAccountID", _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'End Function
	'
	' ***************************************************************** '
	' Name: Initialise
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : Gaurav Arora : 06-03-2008 :
	' ***************************************************************** '
	Public Function Initialise() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "Initialise"
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Check if already initialised
		If m_bIsInitialised Then
			Return result
		End If
		
		'Set m_colPaymentItems = New Collection
		
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
        Dim temp_m_oBusiness As Object = Nothing
		m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRBankGuarantee.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
		m_oBusiness = temp_m_oBusiness
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRBankGuarantee.Business Failed", gPMConstants.PMELogLevel.PMLogError)
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
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		optAgent.Visible = True
		optClient.Visible = True
		If m_sBusinessTypeCode.Trim().ToLower() = "direct" Then
			optClient.Visible = True
			optAgent.Visible = False
			optClient.Left = optAgent.Left
			txtPartyName.Text = m_sPartyName
			txtPartyCode.Text = m_sPartyCode
		ElseIf m_sBusinessTypeCode.Trim().ToLower() <> "direct" Then 
			If m_sAgentType.ToLower() = "broker" Then
				optClient.Visible = False
				optAgent.Visible = True
				optAgent.Checked = True
				txtPartyName.Text = m_sAgentName
				txtPartyCode.Text = m_sAgentCode
				
			ElseIf m_sAgentType.ToLower() = "comm acc" Then 
				txtPartyName.Text = m_sPartyName
				txtPartyCode.Text = m_sPartyCode
				optClient.Visible = True
				optClient.Checked = True
				optAgent.Visible = False
				optClient.Left = optAgent.Left
			ElseIf m_sAgentType.ToLower() = "intermed" Then 
				txtPartyName.Text = m_sAgentName
				txtPartyCode.Text = m_sAgentCode
				optClient.Visible = True
				optAgent.Visible = True
				optAgent.Checked = True
				SelPartyType = MainModule.ENBGPartyType.agent
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
	
	' ***************************************************************** '
	'
	' Name: ValidateForm
	'
	' Description:
	'
	' ***************************************************************** '
	Private Function ValidateForm() As Integer
		Dim result As Integer = 0
		Const kMethodName As String = "ValidateForm"
		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		If m_lSelBGId = 0 Then
			MessageBox.Show("Select atleast one Bank Guarantee to proceed further.", "Payment - Bank Guarantee", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		

		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		Finally
		



		End Try
		Return result
	End Function
	
	'Public Function GetAccountDetails(ByVal lPartyCnt As Long, _
	''                                  ByRef r_vAccountBalance As Variant, _
	''                                  Optional ByRef r_vResultArray As Variant) As Long
	'On Error GoTo Err_GetAccountDetails
	'
	'
	'Dim sSQL As String
	'Dim vResultArray As Variant
	'Dim vAccountID As Variant
	'Dim oAccount As Object
	'GetAccountDetails = PMTrue
	'
	'    m_lReturn = m_oBusiness.GetAccountID(v_lPartyCnt:=lPartyCnt, _
	''                                        r_vResults:=vResultArray)
	'    If m_lReturn <> PMTrue Then
	'        GetAccountDetails = PMFalse
	'        Exit Function
	'    End If
	'
	'    vAccountID = vResultArray(0, 0)
	'
	'    m_lReturn& = g_oObjectManager.GetInstance( _
	''        oObject:=m_oAccount, _
	''        sClassName:="bActAccount.Form", _
	''        vInstanceManager:=PMGetViaClientManager)
	'
	'    If (m_lReturn <> PMTrue) Then
	'        GetAccountDetails = PMFalse
	'        Exit Function
	'    End If
	'
	'    m_lReturn = m_oAccount.GetAccountBalance( _
	''                        r_vdAccountBalance:=r_vAccountBalance, _
	''                        v_vAccountID:=vAccountID, _
	''                        r_vResultArray:=r_vResultArray)
	'
	'    If (m_lReturn <> PMTrue) Then
	'        GetAccountDetails = PMFalse
	'        Exit Function
	'    End If
	'
	'Exit Function
	'
	'Err_GetAccountDetails:
	'
	'    ' Log Error Message
	'    LogMessage _
	''        iType:=PMLogOnError, _
	''        sMsg:="GetAccountdetails Failed", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:="GetAccountDetails", _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'
	'End Function
	
    Private Sub lvwBGList_ItemCheck(ByVal eventSender As Object, ByVal eventArgs As ItemCheckEventArgs) Handles lvwBGList.ItemCheck
        'developer guide no. commenting this as logically its not required, working as per the desired functionality
        
    End Sub
	
	Private Sub lvwBGList_ItemClick(ByVal Item As ListViewItem)
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
			m_lReturn = CType(GetValidBgsOnPolicy(), gPMConstants.PMEReturnCode)
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
			m_lReturn = CType(GetValidBgsOnPolicy(), gPMConstants.PMEReturnCode)
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

    Private Sub lvwBGList_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwBGList.ItemChecked
        If e.Item.Index < 0 Or m_bItemAdded Then
            Exit Sub
        End If

        Dim Item As ListViewItem = lvwBGList.Items(e.Item.Index)
        RemoveHandler lvwBGList.ItemChecked, AddressOf lvwBGList_ItemChecked
        For lListCount As Integer = 1 To lvwBGList.Items.Count
            If lvwBGList.Items.Item(lListCount - 1).Checked Then
                lvwBGList.Items.Item(lListCount - 1).Checked = False
            End If
        Next

        Item.Checked = True
        Item.Selected = True
        AddHandler lvwBGList.ItemChecked, AddressOf lvwBGList_ItemChecked
    End Sub
End Class
