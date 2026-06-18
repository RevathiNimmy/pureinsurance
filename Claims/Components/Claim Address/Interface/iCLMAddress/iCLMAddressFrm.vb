Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 02/07/1998
	'
	' Description: Main interface.
	'
	' Edit History:
	'   SP161198 - Update Address User control (uses QAS now) and remove
	'   terminate call which was a temporary work around an old bug.
	'
	'   Ram 17-11-200 - Set the uctAdd (uctPMAddressControl) Control's
	'                   IsCountryRequired  Value to 1  (Previous Value = 0)
	' ***************************************************************** '
    'developer guide no.7
    Public Const vbFormCode As Integer = 0
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' Status members
	Private m_sProcessStatus As New FixedLengthString(2)
	Private m_sMapStatus As New FixedLengthString(2)
	Private m_sStepStatus As New FixedLengthString(2)
	
	' Form Constants for Captions
	
	Const ACInterfaceCaption As Integer = 100
	Const ACMainTabTitle0 As Integer = 101
	Const ACMainTabTitle1 As Integer = 102
	Const ACConPostCodeCaption As Integer = 103
	Const ACConReferenceCaption As Integer = 104
	Const AClbAdReferenceCaption As Integer = 105
	Const ACAdPostcodeCaption As Integer = 106
	
	
	' Button Constants for Captions
	
	Const ACHelpCaption As Integer = 200
	Const ACCancelCaption As Integer = 201
	Const ACOKCaption As Integer = 202
	Const ACNextCaption0 As Integer = 203
	Const ACEditConCaption As Integer = 204
	Const ACDeleteConCaption As Integer = 205
	Const ACAddConCaption As Integer = 206
	
	
	' Message Constants for Captions
	
	
	' Constants for internal return flags
	Const ACAddressAdd As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
	Const ACAddressDoNotAdd As gPMConstants.PMEReturnCode = 2
	Const ACAddressCancel As gPMConstants.PMEReturnCode = 3
	Const ACAddressEdit As gPMConstants.PMEReturnCode = 4
	
	Private m_lAddressCnt As Integer
	Private m_sPostalCode As String = ""
	Private m_sAddress1 As String = ""
	Private m_sAddress2 As String = ""
	Private m_sAddress3 As String = ""
	Private m_sAddress4 As String = ""
	Private m_vContacts As Object
	
	'TN20010402 Start
	' CountryID
	Private m_lCountryID As Integer
	'TN20010402 End
	
	' CF30499 Address Usage Type
	Private m_lAddressUsageTypeID As Integer
	Private m_sAddressUsageType As String = ""
	'JMK 05/06/2001
	Private Const ACAddressUsageTypeID As Integer = 4
	'AR20050303 - PN15644
	Private m_lAddressId As Integer
	'AR20050303 - PN15644 Flag whether user has manually altered the address
	Private m_sInitial_AddressLine1 As String = ""
	Private m_sInitial_AddressLine2 As String = ""
	Private m_sInitial_AddressLine3 As String = ""
	Private m_sInitial_AddressLine4 As String = ""
	Private m_sInitial_PostCode As String = ""
	Private m_lInitial_CountryId As Integer
	Private m_bAddressChanged As Boolean
	
	' Declare an instance of the contact interface.
	Private m_oContact As Object
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iCLMAddress.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast() As Control
	
	'DJM 07/05/2002 : Added new property
	Private m_sShortname As String = ""
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	
	

	'Private Sub Status(ByVal Value As Integer)
		'
		' Standard Property.
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property Task() As Integer
		Get
			
			' Return the objects task.
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the objects task.
			m_iTask = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the navigate flag.
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the process mode.
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the type of business.
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			' Standard Property.
			
			' Set the effective date.
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	Public ReadOnly Property StepStatus() As String
		Get
			
			' Standard Property.
			
			' Return the Steps Status
			Return m_sStepStatus.Value
			
		End Get
	End Property
	
	' {* USER DEFINED CODE (Begin) *}
	'AR20050303 - PN15644
	
	Public Property AddressId() As Integer
		Get
			
			Return m_lAddressId
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lAddressId = Value
			
		End Set
	End Property
	
	'AR20050404 - PN15644
	
	Public Property AddressChanged() As Boolean
		Get
			
			Return m_bAddressChanged
			
		End Get
		Set(ByVal Value As Boolean)
			
			m_bAddressChanged = Value
			
		End Set
	End Property
	
	Public Property AddressCnt() As Integer
		Get
			
			' Return the objects parameter value.
			Return m_lAddressCnt
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the objects parameter value.
			m_lAddressCnt = Value
			
		End Set
	End Property
	Public Property Address1() As String
		Get
			
			' Return the objects parameter value.
			Return m_sAddress1
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sAddress1 = Value
			
		End Set
	End Property
	Public Property Address2() As String
		Get
			
			' Return the objects parameter value.
			Return m_sAddress2
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sAddress2 = Value
			
		End Set
	End Property
	Public Property Address3() As String
		Get
			
			' Return the objects parameter value.
			Return m_sAddress3
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sAddress3 = Value
			
		End Set
	End Property
	Public Property Address4() As String
		Get
			
			' Return the objects parameter value.
			Return m_sAddress4
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sAddress4 = Value
			
		End Set
	End Property
	Public Property PostalCode() As String
		Get
			
			' Return the objects parameter value.
			Return m_sPostalCode
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sPostalCode = Value
			
		End Set
	End Property
	
	' CF30499 - Address Usage Type
	Public Property AddressUsageTypeID() As Integer
		Get
			Return m_lAddressUsageTypeID
		End Get
		Set(ByVal Value As Integer)
			m_lAddressUsageTypeID = Value
		End Set
	End Property
	
	Public Property AddressUsageType() As String
		Get
			Return m_sAddressUsageType
		End Get
		Set(ByVal Value As String)
			m_sAddressUsageType = Value
		End Set
	End Property
	Public Property CountryID() As Integer
		Get
			Return m_lCountryID
		End Get
		Set(ByVal Value As Integer)
			m_lCountryID = Value
		End Set
	End Property
	
	Public WriteOnly Property Shortname() As String
		Set(ByVal Value As String)
			' Set the objects parameter value.
			m_sShortname = Value
		End Set
	End Property
	
	' ***************************************************************** '
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************** '
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
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
			
			
			
			' Error Section.
			
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

			m_lReturn = m_oBusiness.GetDetails(vAddressCnt:=m_lAddressCnt)
			
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
			
			
			
			' Error Section.
			
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
			
			' {* USER DEFINED CODE (Begin) *}
			'Fill the Address control
			uctAdd.PostCode = m_sPostalCode
			uctAdd.AddressLine1 = m_sAddress1
			uctAdd.AddressLine2 = m_sAddress2
			uctAdd.AddressLine3 = m_sAddress3
			uctAdd.AddressLine4 = m_sAddress4
			uctAdd.CountryId = m_lCountryID
			
			'AR20050404 - PN15664 Store initial values
			m_sInitial_AddressLine1 = m_sAddress1
			m_sInitial_AddressLine2 = m_sAddress2
			m_sInitial_AddressLine3 = m_sAddress3
			m_sInitial_AddressLine4 = m_sAddress4
			m_sInitial_PostCode = m_sPostalCode
			m_lInitial_CountryId = m_lCountryID
			m_bAddressChanged = False
			
			'DJM 30/04/2002 : Underwriting don't want to be able to change Address Usage Type
			'Default to correspondence address
			cboAddUsageType.ItemId = ACAddressUsageTypeID
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Inform the business object with a new data item.
					'AR20050303 - PN15644 Add performed in iOpenClaim
					'            m_lReturn& = m_oBusiness.EditAdd(lRow:=lBusinessDataID&, _
					''                                            vAddress1:=m_sAddress1, _
					''                                            vAddress2:=m_sAddress2, _
					''                                            vAddress3:=m_sAddress3, _
					''                                            vAddress4:=m_sAddress4, _
					''                                            vPostalCode:=m_sPostalCode, _
					''                                            vCountryID:=m_lCountryID, _
					''                                            vAddressUsageTypeID:=m_lAddressUsageTypeID)
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Inform the business object with an updated data item.
					'AR20050303 - PN15644 Pass in m_lAddressID and Update flag

					m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vCountryID:=m_lCountryID, vAddressUsageTypeID:=m_lAddressUsageTypeID, vAddressID:=m_lAddressId, vUpdateGlobalAddress:=m_bAddressChanged)
			End Select
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: SetStatus (Standard Method)
	'
	' Description: Set the Process, Map and Step status.
	' Note:        A Property Get is provided for the Step Status only
	'              as this is the only one which this component can
	'              alter directly.
	' ***************************************************************** '
	Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the current Status settings.
			m_sProcessStatus.Value = sProcessStatus.Trim()
			m_sMapStatus.Value = sMapStatus.Trim()
			m_sStepStatus.Value = sStepStatus.Trim()
			
			Return result
		
		Catch excep As System.Exception
			
			
            ' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PUBLIC Methods (End)
	
	' ***************************************************************** '
	' Name: ValidateOK
	'
	' Description: This validates according to whether the address
	' already exists or not
	'
	' ***************************************************************** '

	'Private Function ValidateOK() As Integer
		'
		'Dim result As Integer = 0
		'Dim sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode, sTmp As String
		'
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				'
				'If uctAdd.PMAddressCnt > 0 Then
					'Address to add was got from database
					'must check if its changed so get it

					'm_oBusiness.AddressCnt = uctAdd.PMAddressCnt
					'

					'm_lReturn = m_oBusiness.GetDetails(vAddressCnt:=uctAdd.PMAddressCnt)
					'
					'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						'Return gPMConstants.PMEReturnCode.PMFalse
					'End If
					'

					'm_lReturn = m_oBusiness.getnext(vAddress1:=sAddress1, vAddress2:=sAddress2, vAddress3:=sAddress3, vAddress4:=sAddress4, vPostalCode:=sPostalCode)
					'
					'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						'Return gPMConstants.PMEReturnCode.PMFalse
					'End If
					'
					'Has user changed anything
					'If (sAddress1.Trim() <> uctAdd.AddressLine1.Trim()) Or (sAddress2.Trim() <> uctAdd.AddressLine2.Trim()) Or (sAddress3.Trim() <> uctAdd.AddressLine3.Trim()) Or (sAddress4.Trim() <> uctAdd.AddressLine4.Trim()) Or (sPostalCode.Trim() <> uctAdd.PostCode.Trim()) Then
						'Something has changed
						'sTmp = CStr(MessageBox.Show("This address already exists and is in use by other parties, " &  _
						'       "yet you have made changes." & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() &  _
						'       "Do you wish these changes to apply to all other parties use of " &  _
						'       "this address ?" &  _
						'       Strings.Chr(10).ToString() & "(Select 'No' and a new address will be created just for " &  _
						'       "this party)", "Confirm Address Add", MessageBoxButtons.YesNoCancel))
						'
						'Select Case sTmp
							'Case CStr(System.Windows.Forms.DialogResult.Yes)
								'
								'result = ACAddressEdit
								'Effectively we are doing an edit, so change the task
								'm_iTask = gPMConstants.PMEComponentAction.PMEdit
								'
							'Case CStr(System.Windows.Forms.DialogResult.No)
								'
								'result = ACAddressAdd
								'Clear out stuff as we'll be doing an add

								'm_lReturn = m_oBusiness.Clear()

								'm_oBusiness.AddressCnt = 0
								'
							'Case Else
								'
								'result = ACAddressCancel

								'm_lReturn = m_oBusiness.Clear()
								'
						'End Select
					'Else
						'
						'result = ACAddressDoNotAdd
						'
					'End If
				'Else
					'dont already exist, so add it
					'result = ACAddressAdd
				'End If
				'
			'ElseIf (m_iTask = gPMConstants.PMEComponentAction.PMEdit) Then 
				'
				'Has user changed anything
				'If (m_sAddress1.Trim() <> uctAdd.AddressLine1.Trim()) Or (m_sAddress2.Trim() <> uctAdd.AddressLine2.Trim()) Or (m_sAddress3.Trim() <> uctAdd.AddressLine3.Trim()) Or (m_sAddress4.Trim() <> uctAdd.AddressLine4.Trim()) Or (m_sPostalCode.Trim() <> uctAdd.PostCode.Trim()) Then
					'Something has changed
					'Check if anyone else uses this address
					'sTmp = CStr(MessageBox.Show("This address may be in use by other parties, " &  _
					'       "yet you have made changes." & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() &  _
					'       "Do you wish these changes to apply to all other parties use of " &  _
					'       "this address ?" &  _
					'       Strings.Chr(10).ToString() & "If not, delete this address and add it correctly.", "Confirm Address Edit", MessageBoxButtons.YesNo))
					'
					'Select Case sTmp
						'Case CStr(System.Windows.Forms.DialogResult.Yes)
							'
							'result = ACAddressEdit
							'
						'Case CStr(System.Windows.Forms.DialogResult.No)
							'
							'result = ACAddressCancel
							'
					'End Select
				'End If
				'
			'End If
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error Message
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Dim sTmp As String = ""
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage.
			
			'DJM 07/05/2002 : Added vAddressUsageTypeID as a parameter

			m_lReturn = m_oBusiness.getnext(vAddress1:=m_sAddress1, vAddress2:=m_sAddress2, vAddress3:=m_sAddress3, vAddress4:=m_sAddress4, vPostalCode:=m_sPostalCode, vCountryID:=m_lCountryID, vAddressUsageTypeID:=m_lAddressUsageTypeID, vAddressID:=m_lAddressId) 'TN20010402 add country id
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			

			m_lReturn = m_oBusiness.GetContacts(vContacts:=m_vContacts)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the contact details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
		
		' Temp string to hold postcode before validation
		Dim result As Integer = 0
		Dim sTempPostalCode As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			sTempPostalCode = uctAdd.PostCode.Trim()
			m_sAddress1 = uctAdd.AddressLine1.Trim()
			m_sAddress2 = uctAdd.AddressLine2.Trim()
			m_sAddress3 = uctAdd.AddressLine3.Trim()
			m_sAddress4 = uctAdd.AddressLine4.Trim()
			
			' CF 080199 - Shuffle up address if line 2 is missing
			If m_sAddress2 = "" Then
				m_sAddress2 = m_sAddress3
				m_sAddress3 = m_sAddress4
				m_sAddress4 = ""
			End If
			'JMK 05/06/2001
			m_lCountryID = uctAdd.CountryId
			
			' CF 080199 - Fix postcode if QAS has returned it with an
			'             inappropriate number of spaces
			m_lReturn = FormatPostCode(v_sInString:=sTempPostalCode, r_sOutString:=m_sPostalCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' CF 300499 - Address usage type
			m_lAddressUsageTypeID = cboAddUsageType.ItemId
			m_sAddressUsageType = cboAddUsageType.ItemCaption
			
			'AR20050404 - PN15664 Flag if the user has changed the address
			If m_sAddress1 <> m_sInitial_AddressLine1 Or m_sAddress2 <> m_sInitial_AddressLine2 Or m_sAddress3 <> m_sInitial_AddressLine3 Or m_sAddress4 <> m_sInitial_AddressLine4 Or m_sPostalCode <> m_sInitial_PostCode Or (m_lCountryID <> m_lInitial_CountryId) Then
				m_bAddressChanged = True
			End If
			
			' ************************************************************
			' Enter your code here to assign all of the details from the
			' interface to the data storage.
			'
			' Example:-
			'
			'    m_DName$ = trim$(txtName.Text)
			'    m_DDate = CDate(txtDate.Text)
			'    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
			'    m_lReturn& = m_oFormFields.UnformatControl(txtName)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			
			'AR20050303 - PN15644 Removed
			'tabMainTab.TabVisible(1) = False
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
			' If in add mode then default to correspondance address
			
			'DJM 07/05/2002 : Underwriting don't want to be able to change Address Usage Type
			
			cboAddUsageType.Enabled = False
			
			'Default to correspondence address
			cboAddUsageType.ItemId = ACAddressUsageTypeID
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			' Display all language specific captions
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lbAdReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClbAdReferenceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

            lblAdPostcode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdPostcodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
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
			
			
			
			' Error Section.
			
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
			' Error Section.
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
			' Error Section.
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
	' PRIVATE Methods (End)
	
	
	
	Private Sub cboAddUsageType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAddUsageType.Click
		
        Dim vIResult(,) As Object
		
		'Check if changed
		If m_lAddressUsageTypeID <> cboAddUsageType.ItemId Then
			
			'If different then lookup address type code

			m_lReturn = m_oBusiness.GetPartyAddress(vIResult, m_sShortname, cboAddUsageType.ItemId)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				'change addtype property
				m_lAddressUsageTypeID = cboAddUsageType.ItemId
				m_sAddressUsageType = cboAddUsageType.ItemCaption
				
				'copy address to data

                m_sAddress1 = CStr(vIResult(1, 0))

                m_sAddress2 = CStr(vIResult(2, 0))

                m_sAddress3 = CStr(vIResult(3, 0))

                m_sAddress4 = CStr(vIResult(4, 0))

                m_sPostalCode = CStr(vIResult(5, 0))
                'AR20050303 - PN15644

                m_lAddressId = CInt(vIResult(6, 0))
				'Update interface
				DataToInterface()
			Else
				cboAddUsageType.ItemId = m_lAddressUsageTypeID
			End If
			
		End If
		
	End Sub
	
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		' Fire up the help screen
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)
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
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMAddress.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iCLMAddress.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
			
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
			
			
			
			' Error Section
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		
		' Forms load event.
		
		Try 
            cboAddUsageType.FirstItem = ""
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
			
			' Set the status for the business object.
			'    m_lReturn& = m_obusiness.SetStatus( _
			'sProcessStatus:=m_sProcessStatus$, _
			'sMapStatus:=m_sMapStatus$, _
			'sStepStatus:=m_sStepStatus$)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the status for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
				
				Exit Sub
			End If
			
			' Set the business keys.
			' {* USER DEFINED CODE (Begin) *}

			m_oBusiness.AddressCnt = m_lAddressCnt
			
			' {* USER DEFINED CODE (End) *}
			
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
			
			' Validate fields using Forms Control
			m_lReturn = SetFieldValidation()
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
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

            ' Terminate the contact object (is used)
            If Not (m_oContact Is Nothing) Then


		m_oContact.Dispose()

               

                ' Destroy the instance of the contact object
                ' from memory.
                m_oContact = Nothing

            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try
		
	End Sub
	
	
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Dim vAddressCode As Object
		
		' Click event of the OK button.
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			
			' Check mandatory controls have been entered into.
			m_lReturn = m_oFormFields.CheckMandatoryControls()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'DJM 01/05/2002 : Don't care about country as we're broking
			
			'TN20010403 Start

			m_lReturn = m_oBusiness.GetCountryDetail(v_lcountryid:=uctAdd.CountryId, v_sFieldName:="code", r_vResult:=vAddressCode)
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get country code", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Exit Sub
			End If
			

			If CStr(vAddressCode).Trim() = "GBR" Then
				
				m_lReturn = CheckValidPostCode(v_sPostCode:=uctAdd.PostCode)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					MessageBox.Show("The post code '" & uctAdd.PostCode & "' is not of a valid format.", "Post Code - " & uctAdd.PostCode, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					Exit Sub
				End If
			End If
			
			'RWH(24/08/01) Remove 2nd round of Postcode checking.
			'Validate the fields in the address control too
			'sp todo - remove this when validation goes into Address User Control
			If uctAdd.AddressLine1.Trim() = "" Then
				MessageBox.Show("You must supply a first address line", Application.ProductName)
				uctAdd.Focus()
				SSTabHelper.SetSelectedIndex(tabMainTab, 0)
				Exit Sub
			End If
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = m_oGeneral.ProcessCommand()
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				
				'update the contact cnt property

				m_lAddressCnt = m_oBusiness.AddressCnt
				
				' Everything OK, so we can hide the interface.
				Me.Hide()
				
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	
	Public Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		result = gPMConstants.PMEReturnCode.PMTrue
		
		uctAdd.PostCode = m_sPostalCode
		uctAdd.AddressLine1 = m_sAddress1
		uctAdd.AddressLine2 = m_sAddress2
		uctAdd.AddressLine3 = m_sAddress3
		uctAdd.AddressLine4 = m_sAddress4
		uctAdd.CountryId = m_lCountryID
		
		'AR20050404 - PN15664 Store initial values
		m_sInitial_AddressLine1 = m_sAddress1
		m_sInitial_AddressLine2 = m_sAddress2
		m_sInitial_AddressLine3 = m_sAddress3
		m_sInitial_AddressLine4 = m_sAddress4
		m_sInitial_PostCode = m_sPostalCode
		m_lInitial_CountryId = m_lCountryID
		m_bAddressChanged = False
		
		Return result
	End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        'developer guide no.293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub
End Class