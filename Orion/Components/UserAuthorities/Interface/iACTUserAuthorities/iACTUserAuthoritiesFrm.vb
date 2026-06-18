Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 14/02/2000
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_vResults( ,  ) As Object
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iACTUserAuthorities.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	Private m_sStepStatus As String = ""
    'developer guide no.7
    Private Const vbFormCode As Integer = 0
	' Currently selected user id
	Private m_iUserID As Integer
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	
	
	Public Property StepStatus() As String
		Get
			Return m_sStepStatus
		End Get
		Set(ByVal Value As String)
			m_sStepStatus = Value
		End Set
	End Property
	
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
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
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Status(ByVal Value As Integer)
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
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
	' PRIVATE Property Procedures (End)
	
	' PUBLIC Methods (Begin)
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************** '
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign all of the controls to
			' PMFormControl
			'
			' Example:-
			'
			'        ' Pass control and required settings to FormControl
			'        m_lReturn = m_oFormFields.AddNewFormField( _
			''                       ctlControl:=<Control Name>, _
			''                       lFieldType:=<PM field type>, _
			''                       lFormat:=<PM format string>, _
			''                       lMandatory:=<PMMandatory or PMNonMandatory)
			'
			'        'Error checking
			'        If m_lReturn <> PMTrue Then
			'          SetFieldValidation = PMFalse
			'          Exit Function
			'        End If
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			

			m_lReturn = m_oBusiness.GetDetails()
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: RefreshList
	'
	' Description:
	'
	' ***************************************************************** '
	Private Function RefreshList() As Integer
		
		Dim result As Integer = 0
		Dim lstItem As ListViewItem
		Dim sKey, sText As String
		Dim iLoop1, iUserID As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear the list
			lvwResults.Items.Clear()
			
			For iLoop1 = m_vResults.GetLowerBound(1) To m_vResults.GetUpperBound(1)
				
				' Username
				sKey = "U" & CStr(m_vResults(ACArrayUserID, iLoop1))
				
				iUserID = CInt(m_vResults(ACArrayUserID, iLoop1))
				cboUser.UserID = iUserID
				sText = cboUser.ItemUsername
				
				' Store the username
				m_vResults(ACArrayUserName, iLoop1) = sText
				
				lstItem = lvwResults.Items.Add(sKey, sText, "")
				
				' Write Off
				sText = CStr(m_vResults(ACArrayCanWriteOff, iLoop1))
				If sText = "1" Then
					ListViewHelper.GetListViewSubItem(lstItem, 1).Text = "Yes"
					ListViewHelper.GetListViewSubItem(lstItem, 2).Text = StringsHelper.Format(m_vResults(ACArrayWriteOffAmount, iLoop1), "#,##0.00")
				Else
					ListViewHelper.GetListViewSubItem(lstItem, 1).Text = "No"
					ListViewHelper.GetListViewSubItem(lstItem, 2).Text = StringsHelper.Format(0, "#,##,0.00")
				End If
				' Unrestricted Enquiry
				sText = CStr(m_vResults(ACArrayUnrestrictedEnquiry, iLoop1))
				If sText = "1" Then
					sText = "Yes"
				Else
					sText = "No"
				End If
				ListViewHelper.GetListViewSubItem(lstItem, 3).Text = sText
				' Unrestricted Update
				sText = CStr(m_vResults(ACArrayUnrestrictedUpdate, iLoop1))
				If sText = "1" Then
					sText = "Yes"
				Else
					sText = "No"
				End If
				ListViewHelper.GetListViewSubItem(lstItem, 4).Text = sText
				
				'Override Currency Date
				sText = CStr(m_vResults(ACArrayOverrideDate, iLoop1))
				If sText = "1" Then
					sText = "Yes"
				Else
					sText = "No"
				End If
				ListViewHelper.GetListViewSubItem(lstItem, 5).Text = sText
				
				'Override Currency Rate
				sText = CStr(m_vResults(ACArrayOverrideRate, iLoop1))
				If sText = "1" Then
					sText = "Yes"
				Else
					sText = "No"
				End If
				ListViewHelper.GetListViewSubItem(lstItem, 6).Text = sText
				
				' AMB 05/02/2003 - Added fields
				' Transaction Write Off
				If CStr(m_vResults(ACArrayHasTransWriteOff, iLoop1)) = "1" Then
					ListViewHelper.GetListViewSubItem(lstItem, 7).Text = "Yes"
					' Transaction Write Off Amount
					ListViewHelper.GetListViewSubItem(lstItem, 8).Text = StringsHelper.Format(m_vResults(ACArrayTransWriteOffAmount, iLoop1), "#,##0.00")
				Else
					ListViewHelper.GetListViewSubItem(lstItem, 7).Text = "No"
				End If
				' has refund authority
				If CStr(m_vResults(ACArrayHasRefundAuthority, iLoop1)) = "1" Then
					ListViewHelper.GetListViewSubItem(lstItem, 9).Text = "Yes"
				Else
					ListViewHelper.GetListViewSubItem(lstItem, 9).Text = "No"
				End If
				' has transfer authority
				If CStr(m_vResults(ACArrayHasTransferAuthority, iLoop1)) = "1" Then
					ListViewHelper.GetListViewSubItem(lstItem, 10).Text = "Yes"
				Else
					ListViewHelper.GetListViewSubItem(lstItem, 10).Text = "No"
				End If
				
				If CStr(m_vResults(ACArrayHasPaymentsAuthority, iLoop1)) = "1" Then
					ListViewHelper.GetListViewSubItem(lstItem, 11).Text = "Yes"
					ListViewHelper.GetListViewSubItem(lstItem, 12).Text = StringsHelper.Format(m_vResults(ACArrayPaymentsAmount, iLoop1), "#,##0.00")
				Else
					ListViewHelper.GetListViewSubItem(lstItem, 11).Text = "No"
					ListViewHelper.GetListViewSubItem(lstItem, 12).Text = StringsHelper.Format(0, "#,##,0.00")
				End If
				
				If CStr(m_vResults(ACArrayHasClaimPaymentsAuthority, iLoop1)) = "1" Then
					ListViewHelper.GetListViewSubItem(lstItem, 13).Text = "Yes"
					ListViewHelper.GetListViewSubItem(lstItem, 14).Text = StringsHelper.Format(m_vResults(ACArrayClaimPaymentsAmount, iLoop1), "#,##0.00")
				Else
					ListViewHelper.GetListViewSubItem(lstItem, 13).Text = "No"
					ListViewHelper.GetListViewSubItem(lstItem, 14).Text = StringsHelper.Format(0, "#,##,0.00")
				End If
			Next iLoop1
			
			lblUserCount.Text = CStr(iLoop1) & " user(s)"
			
            'm_lReturn = ListViewAutoSize(lvwList:=lvwResults)
            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwResults)
#If CODEBASE = 18 Then
			lvwResults.Columns.Item(7).Width = CInt(0)
			lvwResults.Columns.Item(8).Width = CInt(0)
			lvwResults.Columns.Item(9).Width = CInt(0)
			lvwResults.Columns.Item(10).Width = CInt(0)
#End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
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
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the interface details.
			
			' Assign the details from the business object
			' to the data storage.
			m_lReturn = BusinessToData()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Assign the details to the interface.
			
			
			
			If Not Information.IsArray(m_vResults) Then
				MessageBox.Show("There are no users on the system at present. There should be at least 'sirius'." & Environment.NewLine & "Please contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			
			m_lReturn = RefreshList()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToBusiness
	'
	' Description: Updates all business members from the interface
	'              details.
	'
	' AMB 05/02/2003 - Added fields for IAG 220 Manage Debtors development
	' ***************************************************************** '
	Public Function InterfaceToBusiness() As Integer
		
		Dim result As Integer = 0
		Dim lBusinessDataID As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the business object.
			
			' Assign the details from the interface to the data storage.
			m_lReturn = InterfaceToData()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the business data ID to one because we are only
			' dealing with one record item only.
			lBusinessDataID = 1
			
			For iLoop1 As Integer = m_vResults.GetLowerBound(1) To m_vResults.GetUpperBound(1)
				
				'Check the task.
				Select Case (m_iTask)
					Case gPMConstants.PMEComponentAction.PMAdd
						' Inform the business object with a new data item.
						
						' {* USER DEFINED CODE (Begin) *}
						'm_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID&, )
						' {* USER DEFINED CODE (End) *}
						
					Case gPMConstants.PMEComponentAction.PMEdit
						' Inform the business object with an updated data item.

						m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vUserID:=m_vResults(ACArrayUserID, iLoop1), vHasWriteOffAuthority:=m_vResults(ACArrayCanWriteOff, iLoop1), vWriteOffAmount:=m_vResults(ACArrayWriteOffAmount, iLoop1), vHasUnrestrictedEnquiry:=m_vResults(ACArrayUnrestrictedEnquiry, iLoop1), vHasUnrestrictedUpdate:=m_vResults(ACArrayUnrestrictedUpdate, iLoop1), vFeeDiscount:=m_vResults(ACArrayFeeDiscount, iLoop1), vHasTransWriteOffAuthority:=m_vResults(ACArrayHasTransWriteOff, iLoop1), vTransWriteOffAmount:=m_vResults(ACArrayTransWriteOffAmount, iLoop1), vHasRefundAuthority:=m_vResults(ACArrayHasRefundAuthority, iLoop1), vHasTransferAuthority:=m_vResults(ACArrayHasTransferAuthority, iLoop1), vHasPaymentsAuthority:=m_vResults(ACArrayHasPaymentsAuthority, iLoop1), vPaymentsAmount:=m_vResults(ACArrayPaymentsAmount, iLoop1), vHasClaimPaymentsAuthority:=m_vResults(ACArrayHasClaimPaymentsAuthority, iLoop1), vClaimPaymentsAmount:=m_vResults(ACArrayClaimPaymentsAmount, iLoop1), vOverrideDate:=m_vResults(ACArrayOverrideDate, iLoop1), vOverrideRate:=m_vResults(ACArrayOverrideRate, iLoop1), vOverridePrePolicyDate:=m_vResults(ACArrayOverridePrePolicyDate, iLoop1), vOverridePrePolicyRate:=m_vResults(ACArrayOverridePrePolicyRate, iLoop1), vWriteOffCurrencyID:=m_vResults(ACArrayWriteOffCurrencyID, iLoop1), vTransWriteOffCurrencyID:=m_vResults(ACArrayTransWriteOffCurrencyID, iLoop1), vPaymentsCurrencyID:=m_vResults(ACArrayPaymentsCurrencyID, iLoop1), vClaimPaymentsCurrencyID:=m_vResults(ACArrayClaimPaymentsCurrencyID, iLoop1))
						
						' Next row
						lBusinessDataID += 1
						
				End Select
				
			Next iLoop1
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DisplayLookupDetails
	'
	' Description: Displays all of the lookup details using the lookup
	'              values/details.
	'
	' ***************************************************************** '
	Public Function DisplayLookupDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the lookup values.
			
			'm_lReturn = GetLookupValues()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get all of the lookup details.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to retreive all of the lookup
			' descriptions for a given lookup type.
			' The GetLookupDetails function will allow you to do this.
			'
			' Example:-
			'
			'    m_lReturn = GetLookupDetails( _
			''        sLookupTable:=PMLookupCodeName, _
			''        ctlLookup:=cmbCodeName)
			'
			'    ' Check for errors.
			'    If (m_lReturn <> PMTrue) Then
			'        DisplayLookupDetails = PMFalse
			'        Exit Function
			'    End If
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' AMB 05/02/2003 - Added fields for IAG 220 Manage Debtors development
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		'User Authority Variables
		Dim result As Integer = 0
		Dim vUserID As Object
		
		Dim vUnrestrictedEnquiry, vUnrestrictedUpdate, vOverrideDate, vOverrideRate, vOverridePrePolicyDate, vOverridePrePolicyRate As Object
		
		Dim vHasRefundAuthority, vHasTransferAuthority As Object
		
		Dim vHasPaymentsAuthority, vPaymentsCurrencyID, vPaymentsAmount As Object
		
		Dim vHasWriteOffAuthority, vWriteOffCurrencyID, vWriteOffAmount, vHasTransWriteOffAuthority, vTransWriteOffCurrencyID, vTransWriteOffAmount As Object
		
		Dim vHasClaimPaymentsAuthority, vClaimPaymentsCurrencyID, vClaimPaymentsAmount As Object
		
		Dim vFeeDiscount As Object
		
		Dim iIndex, iSystemCurrencyID As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			iIndex = 0
			

			Do While (m_oBusiness.GetNext(vUserID:=vUserID, vHasWriteOffAuthority:=vHasWriteOffAuthority, vWriteOffAmount:=vWriteOffAmount, vHasUnrestrictedEnquiry:=vUnrestrictedEnquiry, vHasUnrestrictedUpdate:=vUnrestrictedUpdate, vFeeDiscount:=vFeeDiscount, vHasTransWriteOffAuthority:=vHasTransWriteOffAuthority, vTransWriteOffAmount:=vTransWriteOffAmount, vHasRefundAuthority:=vHasRefundAuthority, vHasTransferAuthority:=vHasTransferAuthority, vHasPaymentsAuthority:=vHasPaymentsAuthority, vPaymentsAmount:=vPaymentsAmount, vHasClaimPaymentsAuthority:=vHasClaimPaymentsAuthority, vClaimPaymentsAmount:=vClaimPaymentsAmount, vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate, vOverridePrePolicyDate:=vOverridePrePolicyDate, vOverridePrePolicyRate:=vOverridePrePolicyRate, vWriteOffCurrencyID:=vWriteOffCurrencyID, vTransWriteOffCurrencyID:=vTransWriteOffCurrencyID, vPaymentsCurrencyID:=vPaymentsCurrencyID, vClaimPaymentsCurrencyID:=vClaimPaymentsCurrencyID) <> gPMConstants.PMEReturnCode.PMEOF)
				
				If Not Information.IsArray(m_vResults) Then
					ReDim m_vResults(ACArrayNumberOfElements, iIndex)
				Else
					ReDim Preserve m_vResults(ACArrayNumberOfElements, iIndex)
				End If
				
				' Store the results

				m_vResults(ACArrayUserID, iIndex) = vUserID

				m_vResults(ACArrayCanWriteOff, iIndex) = vHasWriteOffAuthority

				m_vResults(ACArrayWriteOffAmount, iIndex) = vWriteOffAmount
				' blank username for now
				m_vResults(ACArrayUserName, iIndex) = ""

				m_vResults(ACArrayUnrestrictedEnquiry, iIndex) = vUnrestrictedEnquiry

				m_vResults(ACArrayUnrestrictedUpdate, iIndex) = vUnrestrictedUpdate

				m_vResults(ACArrayFeeDiscount, iIndex) = vFeeDiscount

				m_vResults(ACArrayHasTransWriteOff, iIndex) = vHasTransWriteOffAuthority

				m_vResults(ACArrayTransWriteOffAmount, iIndex) = vTransWriteOffAmount

				m_vResults(ACArrayHasRefundAuthority, iIndex) = vHasRefundAuthority

				m_vResults(ACArrayHasTransferAuthority, iIndex) = vHasTransferAuthority

				m_vResults(ACArrayHasPaymentsAuthority, iIndex) = vHasPaymentsAuthority

				m_vResults(ACArrayPaymentsAmount, iIndex) = vPaymentsAmount

				m_vResults(ACArrayHasClaimPaymentsAuthority, iIndex) = vHasClaimPaymentsAuthority

				m_vResults(ACArrayClaimPaymentsAmount, iIndex) = vClaimPaymentsAmount

				m_vResults(ACArrayOverrideDate, iIndex) = vOverrideDate

				m_vResults(ACArrayOverrideRate, iIndex) = vOverrideRate

				m_vResults(ACArrayOverridePrePolicyDate, iIndex) = vOverridePrePolicyDate

				m_vResults(ACArrayOverridePrePolicyRate, iIndex) = vOverridePrePolicyRate

				If CDbl(vWriteOffCurrencyID) = 0 Then
					If iSystemCurrencyID = 0 Then
						m_lReturn = GetSystemCurrencyID(iSystemCurrencyID)
					End If

					vWriteOffCurrencyID = iSystemCurrencyID
				End If

				m_vResults(ACArrayWriteOffCurrencyID, iIndex) = vWriteOffCurrencyID

				If CDbl(vTransWriteOffCurrencyID) = 0 Then
					If iSystemCurrencyID = 0 Then
						m_lReturn = GetSystemCurrencyID(iSystemCurrencyID)
					End If

					vTransWriteOffCurrencyID = iSystemCurrencyID
				End If

				m_vResults(ACArrayTransWriteOffCurrencyID, iIndex) = vTransWriteOffCurrencyID

				If CDbl(vPaymentsCurrencyID) = 0 Then
					If iSystemCurrencyID = 0 Then
						m_lReturn = GetSystemCurrencyID(iSystemCurrencyID)
					End If

					vPaymentsCurrencyID = iSystemCurrencyID
				End If

				m_vResults(ACArrayPaymentsCurrencyID, iIndex) = vPaymentsCurrencyID

				If CDbl(vClaimPaymentsCurrencyID) = 0 Then
					If iSystemCurrencyID = 0 Then
						m_lReturn = GetSystemCurrencyID(iSystemCurrencyID)
					End If

					vClaimPaymentsCurrencyID = iSystemCurrencyID
				End If

				m_vResults(ACArrayClaimPaymentsCurrencyID, iIndex) = vClaimPaymentsCurrencyID
				
				iIndex += 1
			Loop 
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToData
	'
	' Description: Updates the data storage from the interface details.
	'
	' ***************************************************************** '
	Private Function InterfaceToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			' Update the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign all of the details from the
			' interface to the data storage.
			'
			' Example:-
			'
			'    m_DName$ = trim$(txtName.Text)
			'    m_DDate = CDate(txtDate.Text)
			'    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
			'    m_lReturn = m_oFormFields.UnformatControl(txtName)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' CTAF 140200 - Do nothing here as the data's all in an array already.
			
			' {* USER DEFINED CODE (End) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			iPMFunc.CenterForm(Me)
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the status of the Navigate button.
			Select Case (m_lNavigate)
				Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
					cmdNavigate.Visible = True
					cmdNavigate.Enabled = True
					
				Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
					cmdNavigate.Visible = True
					cmdNavigate.Enabled = False
					
				Case Else
					cmdNavigate.Visible = False
			End Select
			
			m_lReturn = SetFirstLastControls()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwResults.Handle.ToInt32(), v_vShowRowSelect:=True)
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetFirstLastControls
	'
	' Description: Sets the first and last data entry controls for
	'              each tab to the control array, for use with the
	'              keyboard navigation.
	'
	' ***************************************************************** '
	Private Function SetFirstLastControls() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			'ReDim m_ctlTabFirstLast(1, )
			
			' Set the first and last data entry controls for
			' all of the tabs.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to set the first and last data entry
			' controls for all of the tabs.
			'
			' Example:-
			'
			'    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
			'    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Description: Display all language specific captions.
	'
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to display all language specific
			' captions.
			' The GetResData function will allow you to do this.
			'
			' Example:-
			'
			'    lblDesc.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACDesc, _
			''        iDataType:=PMResString)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupValues
	'
	' Description: Gets all of the lookup values, ready to be used by
	'              the lookup function.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetLookupValues() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Gets all of the lookup values.
			'
			' Check the task.
			'Select Case (m_iTask)
				'Case gPMConstants.PMEComponentAction.PMAdd
					' Get all of the lookup values.

					'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					'
				'Case gPMConstants.PMEComponentAction.PMEdit
					' Get all of the lookup values with the correct
					' effective date.

					'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					'
				'Case gPMConstants.PMEComponentAction.PMView
					' Get lookup values for viewing only.

					'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			'End Select
			'
			' Check for errors.
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				' Log Error.
				'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
				'
				'Return result
			'End If
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name: GetLookupDetails
	'
	' Description: Gets all of the lookup details using the lookup
	'              values, then assigns them to the control passed.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
		'
		'Dim result As Integer = 0
		'Dim lRow As Integer
		'Dim bFoundMatch As Boolean
		'
		' Lookup value contants.
		'Const ACValueTableName As Integer = 0
		'Const ACValueID As Integer = 1
		'Const ACValueStartPos As Integer = 2
		'Const ACValueNumber As Integer = 3
		'
		' Lookup detail contants.
		'Const ACDetailKey As Integer = 0
		'Const ACDetailDesc As Integer = 1
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Get the lookup values.
			'
			'bFoundMatch = False
			'
			'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
				' Check for a match of the table name.
				'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
					' Found a match
					'bFoundMatch = True
					'Exit For
				'End If
			'Next lRow
			'
			' Check if there has been a table match.
			'If Not bFoundMatch Then
				'result = gPMConstants.PMEReturnCode.PMFalse
				'
				' Log Error.
				'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
				'
				'Return result
			'End If
			'
			' Using the lookup values, populate the control with
			' the details from the lookup details array.
			'
			'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
				' Add the details to the control.

				'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


				'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
				'
				'SP150998 - compare long value not string
				' Check if this is the selected index.
				'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
					'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


						'ctlLookup.ListIndex = ctlLookup.NewIndex
					'End If
				'End If
				'
			'Next lCntr
			'
			' Check if the selected index is blank. If so,
			' we set the controls index to zero.
			'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

				'ctlLookup.ListIndex = 0
			'End If
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	' ***************************************************************** '
	'
	' Name: GetIndexForID
	'
	' Description: Gets the index associated with the passed user id
	'
	' History: 14/02/2000 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function GetIndexForID(ByVal v_iUserID As Integer, ByRef r_iIndex As Integer) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			For iLoop1 As Integer = m_vResults.GetLowerBound(1) To m_vResults.GetUpperBound(1)
				If v_iUserID = CDbl(m_vResults(0, iLoop1)) Then
					r_iIndex = iLoop1
					Exit For
				End If
			Next iLoop1
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetIndexForID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIndexForID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	'
	' Name: ProcessEdit
	'
	' Description:
	'
	' History: 14/02/2000 CTAF - Created.
	'
	' AMB 05/02/2003 - Added fields for IAG 220 Manage Debtors development
	' ***************************************************************** '
	Private Function ProcessEdit() As Integer
		
		Dim result As Integer = 0
		Dim frmAuth As frmAuthorities
		Dim iIndex As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Create a new instance of the authorites form
			frmAuth = New frmAuthorities()
			
			m_lReturn = GetIndexForID(v_iUserID:=m_iUserID, r_iIndex:=iIndex)
			
			' Set some properties
			With frmAuth
				.UserID = m_iUserID
				.UserName = CStr(m_vResults(ACArrayUserName, iIndex))
				.CanWriteOff = CInt(m_vResults(ACArrayCanWriteOff, iIndex))
				.WriteOffAmount = CDbl(m_vResults(ACArrayWriteOffAmount, iIndex))
				.UnrestrictedEnquiry = CInt(m_vResults(ACArrayUnrestrictedEnquiry, iIndex))
				.UnrestrictedUpdate = CInt(m_vResults(ACArrayUnrestrictedUpdate, iIndex))

                '.set_FeeDiscount(m_vResults(ACArrayFeeDiscount, iIndex))
                .FeeDiscount = m_vResults(ACArrayFeeDiscount, iIndex)
				.HasTransWriteOffAuthority = CInt(m_vResults(ACArrayHasTransWriteOff, iIndex))
				.TransWriteOffAmount = CDbl(m_vResults(ACArrayTransWriteOffAmount, iIndex))
				.HasRefundAuthority = CInt(m_vResults(ACArrayHasRefundAuthority, iIndex))
				.HasTransferAuthority = CInt(m_vResults(ACArrayHasTransferAuthority, iIndex))
				.HasPaymentsAuthority = CInt(m_vResults(ACArrayHasPaymentsAuthority, iIndex))
				.HasClaimPaymentsAuthority = CInt(m_vResults(ACArrayHasClaimPaymentsAuthority, iIndex))
				.PaymentsAmount = CDbl(m_vResults(ACArrayPaymentsAmount, iIndex))
				.ClaimPaymentsAmount = CDbl(m_vResults(ACArrayClaimPaymentsAmount, iIndex))
				.OverrideDate = CInt(m_vResults(ACArrayOverrideDate, iIndex))
				.OverrideRate = CInt(m_vResults(ACArrayOverrideRate, iIndex))
				.OverridePrePolicyDate = CInt(m_vResults(ACArrayOverridePrePolicyDate, iIndex))
				.OverridePrePolicyRate = CInt(m_vResults(ACArrayOverridePrePolicyRate, iIndex))
				.WriteOffCurrencyID = CInt(m_vResults(ACArrayWriteOffCurrencyID, iIndex))
				.TransWriteOffCurrencyID = CInt(m_vResults(ACArrayTransWriteOffCurrencyID, iIndex))
				.PaymentsCurrencyID = CInt(m_vResults(ACArrayPaymentsCurrencyID, iIndex))
				.ClaimPaymentsCurrencyID = CInt(m_vResults(ACArrayClaimPaymentsCurrencyID, iIndex))
			End With
			
			' Show it
			frmAuth.ShowDialog()
			
			' Get the values if needed
			If frmAuth.Status = gPMConstants.PMEReturnCode.PMOK Then
				m_vResults(ACArrayCanWriteOff, iIndex) = frmAuth.CanWriteOff
				m_vResults(ACArrayWriteOffAmount, iIndex) = frmAuth.WriteOffAmount
				m_vResults(ACArrayUnrestrictedEnquiry, iIndex) = frmAuth.UnrestrictedEnquiry
				m_vResults(ACArrayUnrestrictedUpdate, iIndex) = frmAuth.UnrestrictedUpdate

				m_vResults(ACArrayFeeDiscount, iIndex) = frmAuth.FeeDiscount
				m_vResults(ACArrayHasTransWriteOff, iIndex) = frmAuth.HasTransWriteOffAuthority
				m_vResults(ACArrayTransWriteOffAmount, iIndex) = frmAuth.TransWriteOffAmount
				m_vResults(ACArrayHasRefundAuthority, iIndex) = frmAuth.HasRefundAuthority
				m_vResults(ACArrayHasTransferAuthority, iIndex) = frmAuth.HasTransferAuthority
				m_vResults(ACArrayHasPaymentsAuthority, iIndex) = frmAuth.HasPaymentsAuthority
				m_vResults(ACArrayHasClaimPaymentsAuthority, iIndex) = frmAuth.HasClaimPaymentsAuthority
				m_vResults(ACArrayPaymentsAmount, iIndex) = frmAuth.PaymentsAmount
				m_vResults(ACArrayClaimPaymentsAmount, iIndex) = frmAuth.ClaimPaymentsAmount
				m_vResults(ACArrayOverrideDate, iIndex) = frmAuth.OverrideDate
				m_vResults(ACArrayOverrideRate, iIndex) = frmAuth.OverrideRate
				m_vResults(ACArrayOverridePrePolicyDate, iIndex) = frmAuth.OverridePrePolicyDate
				m_vResults(ACArrayOverridePrePolicyRate, iIndex) = frmAuth.OverridePrePolicyRate
				m_vResults(ACArrayWriteOffCurrencyID, iIndex) = frmAuth.WriteOffCurrencyID
				m_vResults(ACArrayTransWriteOffCurrencyID, iIndex) = frmAuth.TransWriteOffCurrencyID
				m_vResults(ACArrayPaymentsCurrencyID, iIndex) = frmAuth.PaymentsCurrencyID
				m_vResults(ACArrayClaimPaymentsCurrencyID, iIndex) = frmAuth.ClaimPaymentsCurrencyID
				
				' Refresh the list
				m_lReturn = RefreshList()
				
			End If
			
			' Remove it
			frmAuth = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' PRIVATE Methods (End)
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
		m_lReturn = ProcessEdit()
		
	End Sub
	
	' PRIVATE Events (Begin)
	
	Private Sub Form_Initialize_Renamed()
		
		Dim sMessage, sTitle As String
		
		' Forms initialise event.
		
		Try 
			
			iPMFunc.ShowFormInTaskBar_Attach()
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

				sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				

				sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iACTUserAuthorities.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Forms load event.
		
		Try 
			
			iPMFunc.ShowFormInTaskBar_Detach()
			
			' Check if we have had an error so far.
			' Possibly creating the business object.
			If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
				' We have already encountered an error,
				' so we MUST exit now.
				Exit Sub
			End If
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Set the process modes for the busines object.

			m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				
				Exit Sub
			End If
			
			' Set the business keys.
			' {* USER DEFINED CODE (Begin) *}
			' {* USER DEFINED CODE (End) *}
			
			' Validate fields using Forms Control
			m_lReturn = SetFieldValidation()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			' Set the interface default values.
			m_lReturn = SetInterfaceDefaults()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			' Gets the interface details to be displayed.
			m_lReturn = m_oGeneral.GetInterfaceDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Check if the interface has been terminated by means
			' other than pressing the command buttons.

			If UnloadMode <> vbFormCode Then
				' Process the next set of actions depending
				' upon the interface task etc.
				m_lReturn = m_oGeneral.ProcessCommand()
				
				' Check the return value.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Do not procced with the interface termination.
					Cancel = 1
                    eventArgs.cancel = True
					' Set the mouse pointer to normal.
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
					Exit Sub
				End If
			End If
			
			' Terminate the general object.
		m_oGeneral.Dispose()
			
			' Check for errors.
			
			
			' Destroy the instance of the general object
			' from memory.
			m_oGeneral = Nothing
			
			' Terminate the business object

		m_oBusiness.Dispose()
			
			' Check for errors.
			
			
			' Destroy the instance of the business object
			' from memory.
			m_oBusiness = Nothing
			
			' Reset the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
		
		Catch excep As System.Exception
			
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			eventArgs.Cancel = Cancel <> 0
		End Try
		
	End Sub
	
	Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		Dim iCtrlDown As Integer
		
		Const ACCtrlMask As Integer = 2
		
		Try 
			
			' Set the control key value.
			iCtrlDown = (Shift And ACCtrlMask) > 0
			
			With tabMainTab
				' Check the key pressed.
				Select Case KeyCode
					Case Keys.PageUp
						' Page Up key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the first tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, 0)
						Else
							' Check we are not on the
							' first tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
								' Display the previous tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
							End If
						End If
						
					Case Keys.PageDown
						' Page Down key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Display the last tab.
							SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
						Else
							' Check we are not on the
							' last tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
								' Display the next tab.
								SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
							End If
						End If
						
					Case Keys.Home
						' Home key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
						
					Case Keys.End
						' End key has been pressed.
						
						' Check if the control key has also
						' been pressed.
						If iCtrlDown Then
							' Set focus the the start control on
							' the tab.
							If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
								m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
							End If
						End If
				End Select
			End With
		
		Catch 
			
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub lvwResults_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwResults.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwResults.Columns(eventArgs.Column)
		
		Try 
			
			' Sort the columns
			With lvwResults
				
				ListViewHelper.SetSortedProperty(lvwResults, False)
				
				If ListViewHelper.GetSortOrderProperty(lvwResults) = SortOrder.Ascending Then
					ListViewHelper.SetSortOrderProperty(lvwResults, SortOrder.Descending)
				Else
					ListViewHelper.SetSortOrderProperty(lvwResults, SortOrder.Ascending)
				End If
				
				ListViewHelper.SetSortKeyProperty(lvwResults, ColumnHeader.Index + 1 - 1)
				
				ListViewHelper.SetSortedProperty(lvwResults, True)
				
			End With
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort list view.", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwResults_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub lvwResults_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwResults.DoubleClick
		
		If cmdEdit.Enabled Then
			cmdEdit_Click(cmdEdit, New EventArgs())
		End If
		
	End Sub
	
	Private Sub lvwResults_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwResults.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		
		' See where the button was clicked
		Dim lstItem As ListViewItem = lvwResults.GetItemAt(x, y)
		
		If Not (lstItem Is Nothing) Then
			' On an item so get the user id
			m_iUserID = CInt(lstItem.Name.Substring(lstItem.Name.Length - (Strings.Len(lstItem.Name) - 1)))
			' and enable edit
			cmdEdit.Enabled = True
		Else
			' No where in particular so disable the edit button
			cmdEdit.Enabled = False
		End If
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = m_oGeneral.ProcessCommand()
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = m_oGeneral.ProcessCommand()
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
		
		' Click event of the Navigate button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = m_oGeneral.ProcessCommand()
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdNext_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub cmdNext_Click(ByRef Index As Integer)
		'
		'Try 
			'
			' Change to the next tab.
			'If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
				'SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
			'End If
			'
			' Set focus to the first control on the tab.
			'If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
				'm_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
			'End If
		'
		'Catch 
			'
			'
			'
			'
			'Exit Sub
		'End Try
		'
		'
	'End Sub
	' PRIVATE Events (End)
	
	Public Function GetSystemCurrencyID(ByRef r_iCurrencyId As Integer) As Integer
		Dim result As Integer = 0
		
		Dim oCurrency As bACTCurrency.Form
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim temp_oCurrency As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oCurrency, "bACTCurrency.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oCurrency = temp_oCurrency
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = oCurrency.GetSystemCurrency(r_iCurrencyId:=r_iCurrencyId)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

            oCurrency.Dispose()
            oCurrency = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemCurrencyID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
