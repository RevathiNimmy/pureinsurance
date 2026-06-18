Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 23/07/1999
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0
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
	Private m_iBankID As Integer
	Private m_sCode As String = ""
	Private m_sBranchCode As String = ""
	Private m_sBankName As String = ""
	Private m_iHeadOffice As Integer
	Private m_sHeadOfficeName As String = ""
	Private m_sBankAddress1 As String = ""
	Private m_sBankAddress2 As String = ""
	Private m_sBankAddress3 As String = ""
	Private m_sBankAddress4 As String = ""
	Private m_sBankPostalCode As String = ""
	Private m_iBankCountry As Integer
	Private m_sBankPhoneAreaCode As String = ""
	Private m_sBankPhoneNumber As String = ""
	Private m_sBankPhoneExtension As String = ""
	Private m_sBankFaxAreaCode As String = ""
	Private m_sBankFaxNumber As String = ""
	Private m_sBankFaxExtension As String = ""
	Private m_sComments As String = ""
	Private m_vAccounts( ,  ) As Object
	Private m_lBankAccountID As Integer
	Private m_bIsNRMA As Boolean
	Private m_bIsMultiCompany As Boolean
	' {* USER DEFINED CODE (End) *}
	
	'DM 08082006 PN29948
	Private m_iCountryID As Integer
	
	'MKR 27/10/2004 PN 13451
	Private m_sOldCodeReference As String = ""
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iACTBank.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Declare an instance of the contact interface.

	Private m_oBankAccount As iACTBankAccount.Interface_Renamed
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	Private m_iBankAccountType As Integer
	Private m_sUniqueId As String = ""
	Private m_sScreenHierarchy As String = ""

	' Stores the details from the business object.

	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)


	' PUBLIC Property Procedures (Begin)
	Public WriteOnly Property IsNRMA() As Boolean
		Set(ByVal Value As Boolean)
			
			m_bIsNRMA = Value
			
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
	' {* USER DEFINED CODE (Begin) *}
	'VB 16/02/2005 PN-2074 Added BankCode Property
	'VB End


	Public Property BankID() As Integer
		Get
			Return m_iBankID
		End Get
		Set(ByVal Value As Integer)
			m_iBankID = Value
		End Set
	End Property

	Public Property BankCode() As String
		Get
			Return m_sCode
		End Get
		Set(ByVal Value As String)
			m_sCode = Value
		End Set
	End Property

	Public Property UniqueId() As String
		Get
			Return m_sUniqueId
		End Get
		Set(ByVal value As String)
			m_sUniqueId = value
		End Set
	End Property

	Public Property ScreenHierarchy() As String
		Get
			Return m_sScreenHierarchy
		End Get
		Set(ByVal value As String)
			m_sScreenHierarchy = value
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
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			m_oFormFields = New iPMFormControl.FormFields()
			
			m_oFormFields.LanguageID = g_iLanguageID
			
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringUpper, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBankName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBranchCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress1, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress2, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress3, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress4, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPostalCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAddressCountry, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPhoneAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPhoneNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPhoneExtension, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFaxAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFaxNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtComments, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
            If (cboBankAccountType.Visible) Then
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboBankAccountType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringUpper, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            End If
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
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
			

			m_lReturn = m_oBusiness.GetDetails(vBankID:=m_iBankID)
			
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
            'Modified as
            Me.cboBankAccountType.FirstItem = ""
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Assign the details to the interface.
			
			m_lReturn = m_oFormFields.FormatControl(txtCode, m_sCode)
			m_lReturn = m_oFormFields.FormatControl(txtBankName, m_sBankName)
			m_lReturn = m_oFormFields.FormatControl(txtBranchCode, m_sBranchCode)
			m_lReturn = m_oFormFields.FormatControl(txtAddress1, m_sBankAddress1)
			m_lReturn = m_oFormFields.FormatControl(txtAddress2, m_sBankAddress2)
			m_lReturn = m_oFormFields.FormatControl(txtAddress3, m_sBankAddress3)
			m_lReturn = m_oFormFields.FormatControl(txtAddress4, m_sBankAddress4)
			m_lReturn = m_oFormFields.FormatControl(txtPostalCode, m_sBankPostalCode)
			m_lReturn = m_oFormFields.FormatControl(cboAddressCountry, m_iBankCountry)
			m_lReturn = m_oFormFields.FormatControl(txtPhoneAreaCode, m_sBankPhoneAreaCode)
			m_lReturn = m_oFormFields.FormatControl(txtPhoneNumber, m_sBankPhoneNumber)
			m_lReturn = m_oFormFields.FormatControl(txtPhoneExtension, m_sBankPhoneExtension)
			m_lReturn = m_oFormFields.FormatControl(txtFaxAreaCode, m_sBankFaxAreaCode)
			m_lReturn = m_oFormFields.FormatControl(txtFaxNumber, m_sBankFaxNumber)
			m_lReturn = m_oFormFields.FormatControl(txtComments, m_sComments)
            'ToDoList 
            'm_lReturn = m_oFormFields.FormatControl(cboBankAccountType, gPMFunctions.ToSafeInteger(m_iBankAccountType))
			pnlHeadOffice.Tag = CStr(m_iHeadOffice)


            'Developer Guide no.51
            pnlHeadOffice.Name = m_sHeadOfficeName
            lblHeadOffice.Text = m_sHeadOfficeName
			'MKR 27/10/2004 PN 13451 -- just incase if user wish to change the short code
			m_sOldCodeReference = m_sCode
			
			PopulateAccounts()
			
			
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
	' ***************************************************************** '
	Public Function InterfaceToBusiness() As Integer
		
		Dim result As Integer = 0
		Dim lBusinessDataID As Integer
		Dim sUniqueId As String = ""
		Dim sScreenHierarchy As String = ""

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
			If String.IsNullOrEmpty(m_sUniqueId) Then
				m_sUniqueId = GetUniqueID()
			End If
			sScreenHierarchy = $"Bank({m_sCode.Trim()})"
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Inform the business object with a new data item.

					' {* USER DEFINED CODE (Begin) *}

					m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vBankID:=m_iBankID, vCode:=m_sCode, vBranchCode:=m_sBranchCode, vBankName:=m_sBankName, vHeadOffice:=m_iHeadOffice, vBankAddress1:=m_sBankAddress1, vBankAddress2:=m_sBankAddress2, vBankAddress3:=m_sBankAddress3, vBankAddress4:=m_sBankAddress4, vBankPostalCode:=m_sBankPostalCode, vBankCountry:=m_iBankCountry, vBankPhoneAreaCode:=m_sBankPhoneAreaCode, vBankPhoneNumber:=m_sBankPhoneNumber, vBankPhoneExtension:=m_sBankPhoneExtension, vBankFaxAreaCode:=m_sBankFaxAreaCode, vBankFaxNumber:=m_sBankFaxNumber, vBankFaxExtension:=m_sBankFaxExtension, vComments:=m_sComments, vBankAccountType:=m_iBankAccountType, vUniqueId:=m_sUniqueId, vScreenHierarchy:=sScreenHierarchy)
					' {* USER DEFINED CODE (End) *}

				Case gPMConstants.PMEComponentAction.PMEdit
					' Inform the business object with an updated data item.

					' {* USER DEFINED CODE (Begin) *}

					m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vBankID:=m_iBankID, vCode:=m_sCode, vBranchCode:=m_sBranchCode, vBankName:=m_sBankName, vHeadOffice:=m_iHeadOffice, vBankAddress1:=m_sBankAddress1, vBankAddress2:=m_sBankAddress2, vBankAddress3:=m_sBankAddress3, vBankAddress4:=m_sBankAddress4, vBankPostalCode:=m_sBankPostalCode, vBankCountry:=m_iBankCountry, vBankPhoneAreaCode:=m_sBankPhoneAreaCode, vBankPhoneNumber:=m_sBankPhoneNumber, vBankPhoneExtension:=m_sBankPhoneExtension, vBankFaxAreaCode:=m_sBankFaxAreaCode, vBankFaxNumber:=m_sBankFaxNumber, vBankFaxExtension:=m_sBankFaxExtension, vComments:=m_sComments, vBankAccountType:=m_iBankAccountType, vUniqueId:=m_sUniqueId, vScreenHierarchy:=sScreenHierarchy)
					' {* USER DEFINED CODE (End) *}
			End Select
			
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
			
			m_lReturn = GetLookupValues()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			m_lReturn = GetLookupDetails(sLookupTable:=gPMConstants.PMLookupCountry, ctlLookup:=cboAddressCountry)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'PWC 01/10/2002 - Ensure that the default country is set
			'm_oFormFields.FormatControl cboAddressCountry, g_oObjectManager.CountryID
			'PN29948 Changed to display base country of branch in which user have logged in
			m_oFormFields.FormatControl(cboAddressCountry, m_iCountryID)
			
			' Get all of the lookup details.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to retreive all of the lookup
			' descriptions for a given lookup type.
			' The GetLookupDetails function will allow you to do this.
			'
			' Example:-
			'
			'    m_lReturn& = GetLookupDetails( _
			''        sLookupTable:=PMLookupCodeName, _
			''        ctlLookup:=cmbCodeName)
			'
			'    ' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
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
	' ***************************************************************** '
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			

			m_lReturn = m_oBusiness.GetNext(vBankID:=m_iBankID, vCode:=m_sCode, vBranchCode:=m_sBranchCode, vBankName:=m_sBankName, vHeadOffice:=m_iHeadOffice, vBankAddress1:=m_sBankAddress1, vBankAddress2:=m_sBankAddress2, vBankAddress3:=m_sBankAddress3, vBankAddress4:=m_sBankAddress4, vBankPostalCode:=m_sBankPostalCode, vBankCountry:=m_iBankCountry, vBankPhoneAreaCode:=m_sBankPhoneAreaCode, vBankPhoneNumber:=m_sBankPhoneNumber, vBankPhoneExtension:=m_sBankPhoneExtension, vBankFaxAreaCode:=m_sBankFaxAreaCode, vBankFaxNumber:=m_sBankFaxNumber, vBankFaxExtension:=m_sBankFaxExtension, vComments:=m_sComments, vBankAccountType:=m_iBankAccountType)
			
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			'Get additional details required for display that not stored on this
			'record

			m_lReturn = m_oBusiness.GetOtherDetails(vHeadOfficeId:=m_iHeadOffice, vHeadOfficeName:=m_sHeadOfficeName)
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the Account details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
			
			'Get accounts for the Bank

			m_lReturn = m_oBusiness.GetAccountDetails(vBankID:=m_iBankID, vAccounts:=m_vAccounts)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the account details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
			End If
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
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
			'    m_lReturn& = m_oFormFields.UnformatControl(txtName)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			

			m_sCode = CStr(m_oFormFields.UnformatControl(txtCode))

			m_sBankName = CStr(m_oFormFields.UnformatControl(txtBankName))

			m_sBranchCode = CStr(m_oFormFields.UnformatControl(txtBranchCode))

			m_sBankAddress1 = CStr(m_oFormFields.UnformatControl(txtAddress1))

			m_sBankAddress2 = CStr(m_oFormFields.UnformatControl(txtAddress2))

			m_sBankAddress3 = CStr(m_oFormFields.UnformatControl(txtAddress3))

			m_sBankAddress4 = CStr(m_oFormFields.UnformatControl(txtAddress4))

			m_sBankPostalCode = CStr(m_oFormFields.UnformatControl(txtPostalCode))

            'developer guide no. 250
            m_iBankCountry = VB6.GetItemData(cboAddressCountry, cboAddressCountry.SelectedIndex)

			m_sBankPhoneAreaCode = CStr(m_oFormFields.UnformatControl(txtPhoneAreaCode))

			m_sBankPhoneNumber = CStr(m_oFormFields.UnformatControl(txtPhoneNumber))

			m_sBankPhoneExtension = CStr(m_oFormFields.UnformatControl(txtPhoneExtension))

			m_sBankFaxAreaCode = CStr(m_oFormFields.UnformatControl(txtFaxAreaCode))

			m_sBankFaxNumber = CStr(m_oFormFields.UnformatControl(txtFaxNumber))

			m_sComments = CStr(m_oFormFields.UnformatControl(txtComments))

            If (cboBankAccountType.Visible) Then
                m_iBankAccountType = CInt(m_oFormFields.UnformatControl(cboBankAccountType))
            End If
			If Convert.ToString(pnlHeadOffice.Tag) <> "" Then
				m_iHeadOffice = CInt(Convert.ToString(pnlHeadOffice.Tag))
			End If
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateAccounts
	'
	' Description: Fills the grid control with Account details
	' ***************************************************************** '
	Private Sub PopulateAccounts()
		
		Dim oListItem As ListViewItem
		Dim bProcess As Boolean
		
		Const ACCompanyId As Integer = 7
		
		Try 
			
			If Not Information.IsArray(m_vAccounts) Then
				cmdEditAcc.Enabled = False
				cmdDeleteAcc.Enabled = False
				Exit Sub
			End If
			
			lvwAccounts.Items.Clear()
			
			' Assign the details to the interface.
			For lCount As Integer = m_vAccounts.GetLowerBound(1) To m_vAccounts.GetUpperBound(1)
				
				'SJ 20/07/2004 - start
				bProcess = True
				If m_bIsMultiCompany And CDbl(m_vAccounts(ACCompanyId, lCount)) <> g_iSourceID Then
					bProcess = False
				End If
				
				If bProcess Then
					'SJ 20/07/2004 - end
					' Add a new list item and set icon

                    oListItem = lvwAccounts.Items.Add(CStr(m_vAccounts(1, lCount)).Trim(), "AccountImage")
					
					' PWF 26/09/2002 - Added additional columns to display branch/subbranch
					' Assign details to other the columns
					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAccounts(2, lCount)).Trim()
					ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAccounts(3, lCount)).Trim()
					ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vAccounts(4, lCount)).Trim()
					ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vAccounts(5, lCount)).Trim()
					ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vAccounts(6, lCount)).Trim()
					
					' Store the Account_cnt
					oListItem.Tag = CStr(m_vAccounts(0, lCount)).Trim()
				End If
				
			Next lCount
			
            'developer guide no. 170
            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwAccounts)
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccountss", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
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
			
			'PSL 20/02/2003 NRMA do't have postcodes
			If m_bIsNRMA Then
				lblPostalCode.Visible = False
				txtPostalCode.Visible = False
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
			
			'MKW020703 PN4519 START - Allow accounts to be added on bank account creation.
			'eck310801 - Don't allow addition of accounts until Bank has been saved
			'    If m_iTask = PMAdd Then
			'       tabMainTab.TabVisible(2) = False
			'    End If
			'MKW020703 PN4519 END
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			' DD 13/08/2002: Added default country
			'm_oFormFields.FormatControl cboAddressCountry, g_oObjectManager.CountryID
			'PN29948 Changed to display base country of branch in which user have logged in

			m_lReturn = m_oBusiness.GetBranchBaseCountry(v_lSourceID:=g_oObjectManager.SourceID, r_iCountryID:=m_iCountryID)
			m_oFormFields.FormatControl(cboAddressCountry, m_iCountryID)
			
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
		Const CDetailsTab As Integer = 0
		Const CCommentsTab As Integer = 1
		Const CAccountsTab As Integer = 2
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
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
			
			'MKW020703 PN4519 START - Allow accounts to be added on bank account creation.
			'DD 09/07/2002: Added to clean up the interface on Add/Edit
			'    If Task = PMAdd Then
			'        ReDim m_ctlTabFirstLast(ACControlStart To ACControlEnd, _
			''                                CDetailsTab To CAccountsTab - 1)
			'        cmdNext(1).Visible = False
			'    Else
			m_ctlTabFirstLast = Array.CreateInstance(GetType(Control), New Integer(){ACControlEnd - ACControlStart + 1, CAccountsTab - CDetailsTab + 1}, New Integer(){ACControlStart, CDetailsTab})
			cmdNext(1).Visible = True
			'    End If
			'MKW020703 PN4519 END
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			
			m_ctlTabFirstLast(ACControlStart, CDetailsTab) = txtCode
			m_ctlTabFirstLast(ACControlEnd, CDetailsTab) = txtFaxNumber
			m_ctlTabFirstLast(ACControlStart, CCommentsTab) = txtComments
			m_ctlTabFirstLast(ACControlEnd, CCommentsTab) = txtComments
			'MKW020703 PN4519 START - Allow accounts to be added on bank account creation.
			'DD 09/07/2002: Added to clean up the interface on Add/Edit
			'    If Task = PMEdit Then
			'        Set m_ctlTabFirstLast(ACControlStart, CAccountsTab) = lvwAccounts
			'        Set m_ctlTabFirstLast(ACControlEnd, CAccountsTab) = lvwAccounts
			'    End If
			'MKW020703 PN4519 END
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
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
			

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblBranchCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			

            lblAddress1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblAddress2.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblAddress3.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblAddress4.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle8, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblPostalCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle9, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblAddressCountry.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle10, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblAreaCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle11, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle12, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblExtension.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle13, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblTelephone.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle14, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblFax.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle15, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            cmdHeadOffice.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHeadOfficeButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

            lblBankAccountType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLabTitle16, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
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
	Private Function GetLookupValues() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Gets all of the lookup values.
			
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Get all of the lookup values.

					m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					'
				Case gPMConstants.PMEComponentAction.PMEdit
					' Get all of the lookup values with the correct
					' effective date.

					m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
					
				Case gPMConstants.PMEComponentAction.PMView
					' Get lookup values for viewing only.

					m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			End Select
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
				
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLookupDetails
	'
	' Description: Gets all of the lookup details using the lookup
	'              values, then assigns them to the control passed.
	'
    ' ***************************************************************** '
    'Developer guide no.30
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.


            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)

                ' Add the details to the control.

                'Developer Guide no.30
                'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))
                'ctlLookup.ItemData(NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
                Dim NewIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(Trim(m_vLookupDetails(ACDetailDesc, lCntr)), m_vLookupDetails(ACDetailKey, lCntr)))
                'SP150998 - compare long value not string
                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'Developer Guide no. 28
                        ctlLookup.SelectedIndex = NewIndex
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

                'Developer Guide no.30
                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'SJ 20/07/2004 - start
    ' ***************************************************************** '
    ' Name: CheckMultiCompany (Standard Method)
    '
    ' Description: Checks hidden options to see if this is a multi company environment
    '
    ' ***************************************************************** '
    Private Function CheckMultiCompany() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vValue As Byte

            m_bIsMultiCompany = False

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vValue))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option  " & gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMultiCompany")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If vValue = 1 Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableBranchSelectAtLogon, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option  " & gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMultiCompany")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If vValue = 1 Then
                    m_bIsMultiCompany = True
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMultiCompany Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMultiCompany", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'SJ 20/07/2004 - end

    ' PRIVATE Methods (End)
    Private Function ValidateOK() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lPartyCnt As Integer
        Dim nCounter As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'MKR 27/10/2004 PN 13451 -- Checking for duplicate Bank Short Code
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then 'In Add Mode

                m_lReturn = m_oBusiness.GetBankId(vBankRef:=txtCode.Text.Trim(), vBankID:=lPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to access business object.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lPartyCnt <> 0 Then
                    MessageBox.Show("Bank Short Code already exists.", "Bank Short Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit And txtCode.Text.Trim().ToUpper() <> m_sOldCodeReference.Trim().ToUpper() Then
                'Checking that new short code doesn't currently exist.

                m_lReturn = m_oBusiness.GetBankId(vBankRef:=txtCode.Text.Trim(), vBankID:=lPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to access business object.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lPartyCnt <> 0 Then
                    MessageBox.Show("Bank Short Code already exists.", "Bank Short Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If MessageBox.Show("Warning! You are about to change the Bank Short Code. Do you wish to do so?", "Bank Short Code Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    txtCode.Text = m_sOldCodeReference.Trim()
                End If

            End If

            txtCode.Text = Trim(txtCode.Text)

            For nCounter = 0 To Len(txtCode.Text)
                Dim sAplhaNumeric As String
                Dim nASCIICode As Integer

                sAplhaNumeric = Mid(txtCode.Text, nCounter + 1, 1)

                If (sAplhaNumeric <> "") Then
                    nASCIICode = Asc(sAplhaNumeric)
                    If Not (((nASCIICode >= 48 AndAlso nASCIICode <= 57) OrElse (nASCIICode >= 65 AndAlso nASCIICode <= 90) OrElse (nASCIICode >= 97 AndAlso nASCIICode <= 122))) Then
                        MessageBox.Show("Bank Short Code can only have Alpha-Numeric characters.", "Bank Short Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        If m_sOldCodeReference.Trim().Length > 0 Then
                            txtCode.Text = m_sOldCodeReference.Trim()
                        Else
                            txtCode.Text = ""
                        End If
                        result = PMEReturnCode.PMFalse
                        m_lStatus = PMEReturnCode.PMCancel
                        Exit For
                    End If
                    End If
            Next nCounter

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdAddAcc_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAcc.Click


        'Create Account component if not already done so
        If m_oBankAccount Is Nothing Then

            ' Get an instance of the contactinterface object via
            ' the public object manager.
            Dim temp_m_oBankAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBankAccount, sClassName:="iACTBankAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oBankAccount = temp_m_oBankAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bank account component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If
        End If
        m_lReturn = m_oBankAccount.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If


        'set the contact id

        m_oBankAccount.BankAccountId = 0

        m_oBankAccount.BankID = m_iBankID
        'sw g_iSourceId shoudl be used to find company ID

        m_oBankAccount.CompanyID = g_iSourceID

		m_oBankAccount.BankAccountType = cboBankAccountType.ItemId

		m_oBankAccount.UniqueId = m_sUniqueId

		m_lReturn = m_oBankAccount.Start()

		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If

		If Not String.IsNullOrEmpty(m_oBankAccount.UniqueId) Then
			m_sUniqueId = m_oBankAccount.UniqueId
		End If

		'If not cancelled, edit grid

		If m_oBankAccount.Status = gPMConstants.PMEReturnCode.PMCancel Then
            Exit Sub
        End If


        'Reset Interface
        cmdEditAcc.Enabled = False
        cmdDeleteAcc.Enabled = False
        Me.Refresh()

        'DJM 16/02/2004 PN10120 : Display bank account code and number the correct way around.


        Dim oListItem As ListViewItem = lvwAccounts.Items.Add(m_oBankAccount.Code, AccountImage)

        ' Column 2

        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oBankAccount.BankAccountNo

        ' Column 3

        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oBankAccount.BankAccountName

        ' Column 4

        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oBankAccount.Description


        oListItem.Tag = m_oBankAccount.BankAccountId
    End Sub

    Private Sub cmdDeleteAcc_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAcc.Click
        
        'MKR 27/10/2004 -- Check applied to avoid run time error
        If lvwAccounts.Items.Count < 1 Then
            Exit Sub
        End If

        'Create address component if not already done so
        If m_oBankAccount Is Nothing Then

            ' Get an instance of the contactinterface object via
            ' the public object manager.
            Dim temp_m_oBankAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBankAccount, sClassName:="iACTBankAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oBankAccount = temp_m_oBankAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bank account component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If
        End If
        m_lReturn = m_oBankAccount.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMDelete)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        'eck040601 replace cInt with cLng
        m_lBankAccountID = Convert.ToString(lvwAccounts.Items.Item(lvwAccounts.FocusedItem.Index).Tag)

        'set the account id

        m_oBankAccount.BankAccountId = m_lBankAccountID

        m_oBankAccount.BankAccountType = cboBankAccountType.ItemId

		m_oBankAccount.UniqueId = m_sUniqueId
		m_oBankAccount.BankCode = txtCode.Text.Trim()
		m_lReturn = m_oBankAccount.Start()

		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If



		'If not cancelled, edit grid

		If m_oBankAccount.Status = gPMConstants.PMEReturnCode.PMCancel Then
            Exit Sub
        End If


        'Reset Interface
        cmdEditAcc.Enabled = False
        cmdDeleteAcc.Enabled = False

        'Re-Fill the Account listview
        lvwAccounts.Items.RemoveAt(lvwAccounts.FocusedItem.Index)

    End Sub

    Private Sub cmdEditAcc_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAcc.Click
       Dim oListItem As ListViewItem

        Try

            'Set row to be deleted - if a valid one selected
            If lvwAccounts.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oBankAccount Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oBankAccount As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oBankAccount, sClassName:="iACTBankAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oBankAccount = temp_m_oBankAccount

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bank account component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oBankAccount.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If lvwAccounts.Items.Count > 0 And lvwAccounts.FocusedItem Is Nothing Then
                lvwAccounts.Items(0).Focused = True
            End If

            'eck040601 replace cInt with cLng           
            m_lBankAccountID = Convert.ToString(lvwAccounts.Items.Item(lvwAccounts.FocusedItem.Index).Tag)

            'set the account id

            m_oBankAccount.BankID = m_iBankID
            'sw g_iSourceId shoudl be used to find company ID

            m_oBankAccount.CompanyID = g_iSourceID

            m_oBankAccount.BankAccountId = m_lBankAccountID

            m_oBankAccount.BankAccountType = cboBankAccountType.ItemId
			m_oBankAccount.BankCode = txtCode.Text.Trim()

			m_lReturn = m_oBankAccount.Start()

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If

			If Not String.IsNullOrEmpty(m_oBankAccount.UniqueId) Then
				m_sUniqueId = m_oBankAccount.UniqueId
			End If

			'If not cancelled, edit grid

			If m_oBankAccount.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If


            'Reset Interface
            cmdEditAcc.Enabled = False
            cmdDeleteAcc.Enabled = False

            oListItem = lvwAccounts.FocusedItem

            'DJM 16/02/2004 PN10120 : Display bank account code and number the correct way around.

            oListItem.Text = m_oBankAccount.Code
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oBankAccount.BankAccountNo

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oBankAccount.BankAccountName

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oBankAccount.Description

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAcc_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAcc_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub cmdHeadOffice_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHeadOffice.Click
        Dim vHeadOffice As String = ""
        Dim vName As String = ""
        Dim vKeyArray(,) As Object

        
        Dim oFindBank As Object


        Try

            oFindBank = CreateLateBoundObject("iACTFindBank.Interface_Renamed")

            m_lErrorNumber = CType(oFindBank, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindBank.Dispose()
                oFindBank = Nothing
                Throw New Exception()
            End If

            oFindBank.CallingAppName = "iACTBank.Interface"

            m_lErrorNumber = oFindBank.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindBank.Dispose()
                oFindBank = Nothing
                Throw New Exception()
            End If
            'Exclude current record from list
            With oFindBank
                .OmitBankID = m_iBankID
                'PWC30092002 - Issue351 - Disable the 'Edit' and 'New' options of the
                'FindBank interface to stop reentrant code
                .DisableCommands()
            End With

            m_lErrorNumber = oFindBank.Start()

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindBank.Dispose()
                oFindBank = Nothing
                Throw New Exception()
            End If
            If oFindBank.Status = gPMConstants.PMEReturnCode.PMOK Then

                m_lErrorNumber = oFindBank.GetKeys(vKeyArray)

                If Information.IsArray(vKeyArray) Then

                    For i As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                        Select Case vKeyArray(0, i)
                            Case PMNavKeyConst.ACTKeyNameAccountID

                                vHeadOffice = CStr(vKeyArray(1, i))
                            Case PMNavKeyConst.ACTKeyNameAccountName

                                vName = CStr(vKeyArray(1, i))
                        End Select
                    Next i
                End If

            Else
                'eck310801
                If oFindBank.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    oFindBank.Dispose()
                    oFindBank = Nothing
                    Exit Sub
                Else
                    Throw New Exception()
                End If
            End If

		oFindBank.Dispose()


            oFindBank = Nothing
            pnlHeadOffice.Tag = vHeadOffice



            'Developer Guide no.51
            pnlHeadOffice.Name = vName
            lblHeadOffice.Text = vName
        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHeadOfficeLookUp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_1.Click, _cmdPrevious_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch




            Exit Sub
        End Try


    End Sub


    ' PRIVATE Events (Begin)

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTBank.Form", vInstanceManager:="ClientManager")
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
            m_oGeneral = New iACTBank.General()

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

            'SJ 20/07/2004 - start
            'Check to see if we are running in a multi company environment
            m_lReturn = CheckMultiCompany()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            'SJ 20/07/2004 - end

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

            cmdEditAcc.Enabled = False
            cmdDeleteAcc.Enabled = False

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

        cboBankAccountType.Select()

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
                    'developer guide no. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
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
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub lvwAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAccounts.Click
        cmdEditAcc.Enabled = True
        cmdDeleteAcc.Enabled = True
    End Sub

    Private Sub lvwAccounts_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAccounts.DoubleClick
        Dim oListItem As ListViewItem

        Try

            'Set row to be deleted - if a valid one selected
            If lvwAccounts.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oBankAccount Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oBankAccount As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oBankAccount, sClassName:="iACTBankAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oBankAccount = temp_m_oBankAccount

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bank account component", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwAccounts_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oBankAccount.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            'eck040601 replace cInt with cLng
            m_lBankAccountID = Convert.ToString(lvwAccounts.Items.Item(lvwAccounts.FocusedItem.Index).Tag)

            'set the contact id

            m_oBankAccount.BankAccountId = m_lBankAccountID
			m_oBankAccount.BankCode = txtCode.Text.Trim()

			m_lReturn = m_oBankAccount.Start()

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If

			If Not String.IsNullOrEmpty(m_oBankAccount.UniqueId) Then
				m_sUniqueId = m_oBankAccount.UniqueId
			End If

			'If not cancelled, edit grid

			If m_oBankAccount.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If


            'Reset Interface
            cmdEditAcc.Enabled = False
            cmdDeleteAcc.Enabled = False

            oListItem = lvwAccounts.FocusedItem


			oListItem.Text = m_oBankAccount.Code
			' Column 2

			ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oBankAccount.BankAccountNo

			' Column 3

			ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oBankAccount.BankAccountName

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oBankAccount.Description

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="lvwAccount_DblClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwDblClick_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try


    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
                    VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
                Else
                    VB6.SetDefault(cmdOK, True)
                End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

				' Set focus to the first control on the tab.
				If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch





            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: UpdateBankAccounts
    '
    ' Description: Updates the Bank Account records after the bank
    '              account record has been written.
    '
    ' History: 18/10/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateBankAccounts() As Integer

        Dim result As Integer = 0
		Dim lBankAccountID As Integer
		Dim screenHierarchy As String = ""

		Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwAccounts.Items.Count < 1 Then
                Return result
            End If

            ' Go through each account
            For iLoop1 As Integer = 1 To lvwAccounts.Items.Count

				' Get the bank account id
				lBankAccountID = Convert.ToString(lvwAccounts.Items.Item(iLoop1 - 1).Tag) '
				screenHierarchy = $"Bank({txtCode.Text.Trim})\Accounts({lvwAccounts.Items.Item(iLoop1 - 1).SubItems(1).Text.Trim()})"
				' Get the bank account

				m_lReturn = m_oBusiness.UpdateBank(v_iBankID:=m_iBankID, v_lBankAccountID:=lBankAccountID, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=screenHierarchy)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateBank " & m_iBankID, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBankAccounts", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateBankAccounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBankAccounts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

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
            'Validate some  stuff
            m_lReturn = ValidateOK()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' Update the bank accounts
                m_lReturn = UpdateBankAccounts()

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

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_1.Click, _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch




            Exit Sub
        End Try


    End Sub

    'MKR 27/10/2004 PN 13712
    Private Sub txtComments_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComments.Enter

        VB6.SetDefault(cmdNext(1), False)

    End Sub

    Private Sub txtComments_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtComments.Leave

        VB6.SetDefault(cmdNext(1), True)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: DeleteBankAccounts
    '
    ' Description: Deletes the bank accounts attached if user cancels
    '              when adding new bank.
    '
    ' History: 18/06/2008 Gautam Poddar - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteBankAccounts() As Integer

        Dim result As Integer = 0
        Dim lBankAccountID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwAccounts.Items.Count < 1 Then
                Return result
            End If

            'Create address component if not already done so
            If m_oBusiness Is Nothing Then

                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_m_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTBank.Form", vInstanceManager:="ClientManager")
                m_oBusiness = temp_m_oBusiness

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bACTBank.Form component", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteBankAccounts", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If

            ' Go through each account
            For iLoop1 As Integer = 1 To lvwAccounts.Items.Count

                ' Get the bank account id
                lBankAccountID = Convert.ToString(lvwAccounts.Items.Item(iLoop1 - 1).Tag)

                ' Delete the bank account

                m_lReturn = m_oBusiness.DeleteBankAccount(v_lBankAccountID:=lBankAccountID)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete bank account " & lBankAccountID, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteBankAccounts", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteBankAccounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteBankAccounts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

	Private Sub frmInterface_Click(sender As Object, e As EventArgs) Handles Me.Click

	End Sub
End Class
