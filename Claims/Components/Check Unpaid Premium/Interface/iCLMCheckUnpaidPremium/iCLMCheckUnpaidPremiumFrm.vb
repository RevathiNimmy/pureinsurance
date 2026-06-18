Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 09/06/1999
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
    'developer guide no.7
    Public Const vbFormCode As Integer = 0
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lInsuranceFileCnt As Integer
	Private m_lInsuranceFolderCnt As Integer
	Private m_sInsReference As String = ""
	Private m_lPartyCnt As Integer
	Private m_lItemsFound As Integer
	'Variables to store data taken from the List View
	Private m_iAction As Integer
	Private m_lInsCnt As Integer
	Private m_sClientName As String = ""
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iCLMCheckUnpaidPremium.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Stores the return value for a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' To store query results
	Private m_vPremiumPaymentsArray( ,  ) As Object
	Private m_vTransactionsForPolicyArray( ,  ) As Object
	Private m_vInstalmentsForPolicyArray( ,  ) As Object
	
	' PaymentType
	Private m_sPaymentType As String = ""
	' PaidUpToDate
	Private m_lPaidUpToDate As Integer
	Private m_dInstalmentDebt As Double
	
	
	
	Private m_bEvent As Boolean
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
	'Thinh Nguyen 11/04/2002 (start)
	Public Property TransactionsForPolicyArray() As Object
		Get
			Return VB6.CopyArray(m_vTransactionsForPolicyArray)
		End Get
		Set(ByVal Value As Object)
			m_vTransactionsForPolicyArray = Value
		End Set
	End Property
	'Thinh Nguyen 11/04/2002 (start)
	
	Public Property PaymentType() As String
		Get
			Return m_sPaymentType
		End Get
		Set(ByVal Value As String)
			m_sPaymentType = Value
		End Set
	End Property
	
	Public Property InstalmentDebt() As Double
		Get
			Return m_dInstalmentDebt
		End Get
		Set(ByVal Value As Double)
			m_dInstalmentDebt = Value
		End Set
	End Property
	
	Public Property PaidUpToDate() As Integer
		Get
			Return m_lPaidUpToDate
		End Get
		Set(ByVal Value As Integer)
			m_lPaidUpToDate = Value
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
	
	' {* USER DEFINED CODE (Begin) *}
	
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	
	' PRIVATE Property Procedures (Begin)
	

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
	
	Public Property TransactionType() As String
		Get
			Return m_sTransactionType
		End Get
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
	'
	' Name: UnlockClaim
	'
	' Description:
	'
	' History:  17/09/2000 Tomo - Created.
	'           29/10/2001 JMK  - Copied from Open Claim to use here
	' ***************************************************************** '
	Public Function UnlockClaim(ByVal v_lOriginalClaimID As Integer) As Integer
		Dim result As Integer = 0
		Dim oPMLock As bPMLock.User
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get bPMLock
			Dim temp_oPMLock As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oPMLock = temp_oPMLock
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Return result
			End If
			

			m_lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock the screen", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
				
			End If
			
			oPMLock = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************** '
	Private Function SetFieldValidation() As Integer
		
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
			''                       lMandatory:=<PMNonMandatory or PMNonMandatory)
			'
			'        'Error checking
			'        If m_lReturn <> PMTrue Then
			'          SetFieldValidation = PMFalse
			'          Exit Function
			'        End If
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			'    m_lReturn = m_oFormFields.AddNewFormField( _
			''                           ctlControl:=pnlClaimDate, _
			''                           lFieldType:=PMDate, _
			''                           lFormat:=PMFormatDateLong, _
			''                           lMandatory:=PMNonMandatory)
			
			'    'Error checking
			'     If m_lReturn <> PMTrue Then
			'       SetFieldValidation = PMFalse
			'       Exit Function
			'     End If
			
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
	' Name: DisplayPayments
	'
	' Description: Populates the list view with search results
	'
	' ***************************************************************** '
	Public Function DisplayPayments() As Integer
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
		
		Const ACFindImage As String = "FindImage"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			' Assign the details to the interface.
			If PaymentType = "Instalments" Then
				lvwTransactions.Visible = False
				lvwInstalments.Top = VB6.TwipsToPixelsY(1440)
				lvwInstalments.Visible = True
				lvwInstalments.Items.Clear()
				'Do we have an array?
				If Not Information.IsArray(m_vTransactionsForPolicyArray) Then
					' Log Error.
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Transactions for this Policy not found", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayPayments", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					Return gPMConstants.PMEReturnCode.PMNotFound
				Else
					' Default, display only outstanding payments
					For lRow As Integer = m_vInstalmentsForPolicyArray.GetLowerBound(1) To m_vInstalmentsForPolicyArray.GetUpperBound(1)
						If (CDbl(m_vInstalmentsForPolicyArray(ACAIBranch, lRow)) <> 0) And ((CStr(m_vInstalmentsForPolicyArray(ACAIStatus, lRow)).Trim() = "Pending") And (chkOutstandingOnly.CheckState = CheckState.Checked)) Or (chkOutstandingOnly.CheckState = CheckState.Unchecked) Then
							
							' Assign the details to the first column.
							
							' Column 1 Branch

                            'developer guide no.49
                            oListItem = lvwInstalments.Items.Add(CStr(m_vInstalmentsForPolicyArray(ACAIBranch, lRow)).Trim(), ACFindImage)
							
							' Assign details to the other columns
							
							' Column 2 Account
                            oListItem.SubItems.Add(1).Text = CStr(m_vInstalmentsForPolicyArray(ACAIAccount, lRow)).Trim()
							
							'Column 3 Document Ref
                            oListItem.SubItems.Add(2).Text = CStr(m_vInstalmentsForPolicyArray(ACAIDocRef, lRow)).Trim()
							
							' Column 4 Instalment Number
                            oListItem.SubItems.Add(3).Text = CStr(m_vInstalmentsForPolicyArray(ACAIInstalmentNo, lRow))
							
							' Column 5 Instalment Amount
                            oListItem.SubItems.Add(4).Text = StringsHelper.Format(m_vInstalmentsForPolicyArray(ACAIInstalmentAmount, lRow), "#0.00")
							
							' Column 6 Due Date
                            oListItem.SubItems.Add(5).Text = CDate(m_vInstalmentsForPolicyArray(ACAIDueDate, lRow)).ToString("dd MMM yyyy")
							
							' Column 7 Transaction Code
                            oListItem.SubItems.Add(6).Text = CStr(m_vInstalmentsForPolicyArray(ACAITransactionCode, lRow)).Trim()
							
							' Column 8 Status
                            oListItem.SubItems.Add(7).Text = CStr(m_vInstalmentsForPolicyArray(ACAIStatus, lRow)).Trim()
							
							' Column 9 Posted Date
                            oListItem.SubItems.Add(8).Text = CDate(m_vInstalmentsForPolicyArray(ACAIPostedDate, lRow)).ToString("dd MMM yyyy")
							
							' {* USER DEFINED CODE (End) *}
							
							' Set the tag property with the index of
							' the search data storage.
							oListItem.Tag = CStr(lRow)
							
							' Refresh the first X amount of rows, to
							' allow the user to see the results instantly.
							If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
								' Select the first item.
								lvwInstalments.Items.Item(0).Selected = True
								
								' Refresh the initial results.
								lvwInstalments.Refresh()
							End If
						End If
					Next lRow
				End If
			Else
				lvwInstalments.Visible = False
				lvwTransactions.Top = VB6.TwipsToPixelsY(1440)
				lvwTransactions.Visible = True
				lvwTransactions.Items.Clear()
				'Do we have an array?
				If Not Information.IsArray(m_vTransactionsForPolicyArray) Then
					' Log Error.
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Transactions for this Policy not found", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayPayments", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					Return gPMConstants.PMEReturnCode.PMNotFound
				Else
					For lRow As Integer = m_vTransactionsForPolicyArray.GetLowerBound(1) To m_vTransactionsForPolicyArray.GetUpperBound(1)
						If (CDbl(m_vTransactionsForPolicyArray(ACATBranch, lRow)) <> 0) And ((CDbl(m_vTransactionsForPolicyArray(ACATOSAmount, lRow)) <> 0) And (chkOutstandingOnly.CheckState = CheckState.Checked)) Or (chkOutstandingOnly.CheckState = CheckState.Unchecked) Then
							
							' Assign the details to the first column.
							
							' Column 1 Branch

                            'developer guide no.49
                            oListItem = lvwTransactions.Items.Add(CStr(m_vTransactionsForPolicyArray(ACATBranch, lRow)).Trim(), ACFindImage)
							
							' Assign details to the other columns
							
							' Column 2 Account
                            'ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vTransactionsForPolicyArray(ACATAccount, lRow)).Trim()
                            oListItem.SubItems.Add(1).Text = CStr(m_vTransactionsForPolicyArray(ACATAccount, lRow)).Trim()
							
							'Column 3 Document Ref
                            oListItem.SubItems.Add(2).Text = CStr(m_vTransactionsForPolicyArray(ACATDocRef, lRow)).Trim()
							
							' Column 4 Transaction Date
                            oListItem.SubItems.Add(3).Text = CDate(m_vTransactionsForPolicyArray(ACATTransDate, lRow)).ToString("dd MMM yyyy")
							
							' Column 5 Amount
                            oListItem.SubItems.Add(4).Text = StringsHelper.Format(m_vTransactionsForPolicyArray(ACATAmount, lRow), "#0.00")
							
							' Column 6 O/S Amount
                            oListItem.SubItems.Add(5).Text = StringsHelper.Format(m_vTransactionsForPolicyArray(ACATOSAmount, lRow), "#0.00")
							
							' Column 7 Document Type
                            oListItem.SubItems.Add(6).Text = CStr(m_vTransactionsForPolicyArray(ACATDocType, lRow)).Trim()
							' {* USER DEFINED CODE (End) *}
							
							' Set the tag property with the index of
							' the search data storage.
							oListItem.Tag = CStr(lRow)
							
							' Refresh the first X amount of rows, to
							' allow the user to see the results instantly.
							If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
								' Select the first item.
								lvwTransactions.Items.Item(0).Selected = True
								
								' Refresh the initial results.
								lvwTransactions.Refresh()
							End If
						End If
					Next lRow
				End If
			End If
			Return result
		
		Catch excep As System.Exception
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate list view", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayPayments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			' Find out if premium payments are up to date

			m_lReturn = m_oBusiness.GetPremiumPaymentsStatus(g_sClaimNo, m_vPremiumPaymentsArray)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness - GetPremiumPaymentsStatus")
				
				Return result
				
			End If
			
			' First, initialise form properties
			' ...when PaidUpToDate = 1, the form will not be displayed
			PaymentType = CStr(m_vPremiumPaymentsArray(ACAPaymentType, 0))
			PaidUpToDate = CInt(m_vPremiumPaymentsArray(ACAPaidUpToDate, 0))
			g_sPolicyNo = CStr(m_vPremiumPaymentsArray(ACAPolicyNum, 0))
			g_vClaimDate = CStr(m_vPremiumPaymentsArray(ACAClaimDate, 0))
			
			' KG 04/07/03
			InstalmentDebt = CDbl(m_vPremiumPaymentsArray(ACAInstalmentDebt, 0))
			
			' If Client has PaidUpToDate...
			If m_lPaidUpToDate = 1 Then
				' Set the interface status.
				m_lStatus = gPMConstants.PMEReturnCode.PMOK
				' No need to do anything else
			Else
				' get outstanding transactions/instalments
				If PaymentType = "Instalments" Then

					m_lReturn = m_oBusiness.GetInstalmentsForPolicy(g_sPolicyNo, m_vInstalmentsForPolicyArray)
					
					' Check for errors
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						' Failed to get details.
						result = gPMConstants.PMEReturnCode.PMFalse
						
						' Log Error.
						iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness - GetInstalmentsForPolicy")
					End If
				Else

                    m_lReturn = m_oBusiness.GetTransactionsForPolicy(g_sPolicyNo, m_vTransactionsForPolicyArray)
                    'developer guide no.(Added the condition for unpaid transactions)
                    'Thinh Nguyen 11/04/2002 (start) - only do the rest if we have transactions
                    If Not Information.IsArray(m_vTransactionsForPolicyArray) Then
                        ' Don't bother with displaying interface if Premium has been paid...
                        m_lPaidUpToDate = 1
                        m_lStatus = gPMConstants.PMEReturnCode.PMOK
                    End If
                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to get details.
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness - GetTransactionsForPolicy")
                    End If

                    End If
                End If
                ' {* USER DEFINED CODE (End) *}
                Return result

		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
		
		'Dim oListItem As ListItem
		'Dim lRow As Long
		'
		'Const ACFindImage = "FindImage"
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			
			' Update the interface details.
			
			' Assign the details from the business object
			' to the data storage.
			m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Assign the details to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign the all of the interface
			' details from the business object, using the FormatField
			' function for any type conversion.
			'
			' Example:-
			'
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			' {* USER DEFINED CODE (Begin) *}
			
			Me.Text = Me.Text & ": " & PaymentType

            'developer guide no.26
            'start
            plblPolicyNumber.Text = g_sPolicyNo

            plblClaimNumber.Text = g_sClaimNo

            plblClient.Text = g_sClientName

            plblClaimDate.Text = DateTime.Parse(g_vClaimDate).ToString("D")
			
			' KG 04/07/03
            pLblOverdueInstalments.Text = InstalmentDebt
            'end

			' Assign the details to the interface.
			m_lReturn = CType(DisplayPayments(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' {* USER DEFINED CODE (End) *}
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
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

	'Private Function InterfaceToData() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'
			' {* USER DEFINED CODE (End) *}
			'
			'Return gPMConstants.PMEReturnCode.PMTrue
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result


			'
			'Return result
		'End Try
	'End Function
	
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
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the status of the Navigate button.
			
			m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
            ' {* USER DEFINED CODE (Begin) *}
            'developer guide no.303
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwTransactions.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            lvwTransactions.FullRowSelect = True
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
            'developer guide no.303
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwInstalments.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            lvwInstalments.FullRowSelect = True
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			chkOutstandingOnly.CheckState = CheckState.Checked
			
			' Set the column widths for the Transactions list.
			lvwTransactions.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(500))
			lvwTransactions.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwTransactions.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwTransactions.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1100))
			lvwTransactions.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1000))
			lvwTransactions.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1000))
			lvwTransactions.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(1500))
			
			' Set the column widths for the Instalments list.
			lvwInstalments.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(500))
			lvwInstalments.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwInstalments.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwInstalments.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(400))
			lvwInstalments.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1000))
			lvwInstalments.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1100))
			lvwInstalments.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwInstalments.Columns.Item(7).Width = CInt(VB6.TwipsToPixelsX(1300))
			lvwInstalments.Columns.Item(8).Width = CInt(VB6.TwipsToPixelsX(1100))
			
			If Task = gPMConstants.PMEComponentAction.PMView Then
				'cmdAdd.Enabled = False
			Else
				'cmdAdd.Enabled = True
			End If
			
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
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			
			
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
			

            lblPolicyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblClaimNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'    cmdContinuePayment.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACContinueButton, _
			''        iDataType:=PMResString)
			'
			'    cmdAbortPayment.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACAbortButton, _
			''        iDataType:=PMResString)
			

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			


            lvwTransactions.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwTransactions.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwTransactions.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTDocRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwTransactions.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTransDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwTransactions.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwTransactions.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTOSAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwTransactions.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTDocType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwInstalments.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwInstalments.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwInstalments.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIDocRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwInstalments.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIInstalNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwInstalments.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIInstalAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwInstalments.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIDueDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwInstalments.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACITransCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwInstalments.Columns.Item(7).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			


            lvwInstalments.Columns.Item(8).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIPostedDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub chkOutstandingOnly_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkOutstandingOnly.CheckStateChanged

        If Not m_vTransactionsForPolicyArray Is Nothing Then
            m_lReturn = CType(DisplayPayments(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process Outstanding Only checkbox", vApp:=ACApp, vClass:=ACClass, vMethod:="chkOutstandingOnly_Click")
            End If
        End If
    End Sub
	
	Private Sub cmdAbortPayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAbortPayment.Click
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Abort command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAbortPayment_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdContinuePayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdContinuePayment.Click
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				Me.Hide()
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Continue command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdContinuePayment_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	
	' PRIVATE Methods (End)
	
	' PRIVATE Events (Begin)
	
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		
        ' Fire up the help screen
        'developer guide no.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)

		
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		Dim sMessage, sTitle As String
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMCheckUnpaidPremium.Business", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailMsg, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iCLMCheckUnpaidPremium.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			' Create an instance of the form control object.
			m_oFormFields = New iPMFormControl.FormFields()
			
			' Set language
			m_oFormFields.LanguageID = g_iLanguageID
			
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
    Private Sub frmInterface_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Process already done in LoadInterface of Class Interface_Renamed Line no.
    End Sub
    'This is called at the time of load in VB so has to converted to funtion so that is can be called
    Public Sub frmInterfaceLoad()

        ' Forms load event.

        Try

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
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

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
				m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
				
				' Check the return value.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'developer guide no.7
                    eventArgs.Cancel = True
					Cancel = 1
					
					' Set the mouse pointer to normal.
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
					Exit Sub
				End If
			End If
			
			' Terminate the general object.
            m_oGeneral.Dispose()
			
			
			
			' Destroy the instance of the general object
			' from memory.
			m_oGeneral = Nothing
			
			' Terminate the business object

		m_oBusiness.Dispose()
			
			
			
			' Destroy the instance of the business object
			' from memory.
			m_oBusiness = Nothing
			
			' Terminate the form control object.
		m_oFormFields.Dispose()
			
			
			
			' Destroy the instance of the form control object
			' from memory.
			m_oFormFields = Nothing
			
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
	' ***************************************************************** '
	' Name: DisableInterface
	'
	' Description: Disables parts of the interface while a search is
	'              in progress.
	'
	' ***************************************************************** '

	'Private Function DisableInterface(ByRef bDisable As Boolean) As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'
			'Return gPMConstants.PMEReturnCode.PMTrue
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section.
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		Dim iCtrlDown As Integer
		
		Const ACCtrlMask As Integer = 2
		
		Try 
			
			' Set the control key value.
			iCtrlDown = (Shift And ACCtrlMask) > 0
		
		Catch 
			
			
			
			
			Exit Sub
		End Try
		
		
	End Sub

	'Private Sub cmdNavigate_Click()
		'
		' Click event of the Navigate button.
		'
		'Try 
			'
			' Set the interface status.
			'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
			'
			' Process the next set of actions depending
			' upon the interface task etc.
			'm_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			'
			' Check the return value.
			'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				'Me.Hide()
			'End If
		'
		'Catch excep As System.Exception
			'
			'
			'
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub

    
End Class
