Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmDetails
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmDetails
	'
	' Date: {TodaysDate}
	'
	' Description: Interface for Item Details Tab.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmDetails"
	
	
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
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Form Control
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_iCompanyID As Integer
	Private m_sCode As New FixedLengthString(10)
	Private m_sDescription As New FixedLengthString(255)
	Private m_lCaptionID As Integer
	Private m_iParentID As Integer
	Private m_sRegNo1 As New FixedLengthString(30)
	Private m_sRegNo2 As New FixedLengthString(30)
	Private m_iCurrencyID As Integer
	Private m_sAddress1 As New FixedLengthString(40)
	Private m_sAddress2 As New FixedLengthString(40)
	Private m_sAddress3 As New FixedLengthString(40)
	Private m_sAddress4 As New FixedLengthString(40)
	Private m_sPostalCode As New FixedLengthString(20)
	Private m_iCountryID As Integer
	Private m_sPhoneAreaCode As New FixedLengthString(10)
	Private m_sPhoneNumber As New FixedLengthString(15)
	Private m_sPhoneExtension As New FixedLengthString(6)
	Private m_sFaxAreaCode As New FixedLengthString(10)
	Private m_sFaxNumber As New FixedLengthString(15)
	Private m_sFaxExtension As New FixedLengthString(6)
    'Developer Guide No.7
    Private Const vbFormCode As Integer = 0
	'DC 31/01/00
	'Added following
	Private m_sEmail As New FixedLengthString(50)
	Private m_sVatNo As New FixedLengthString(20)
	Private m_sSenderMailboxId As New FixedLengthString(14)
	Private m_sBrokerABIId As New FixedLengthString(6)
	Private m_iUserLicenceId As Integer
	Private m_iPMCompanyNumber As Integer
	Private m_sDefaultIndicator As New FixedLengthString(1)
	
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
	
	
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	
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
	
	' {* USER DEFINED CODE (Begin) *}
	' ************************************************************
	' Enter your code here to expose the form data fields as properties
	' These are used to pass the data in and out of the form from/to
	' the list array rather than using the business object directly
	' ************************************************************
	'Public Property Let Var(xVar As Type)
	'
	'    ' Set the objects Var
	'    m_xVar = xVar
	'
	'End Property
	'
	'Public Property Get Var() As Type
	'
	'    ' Return the objects Var
	'    Var = m_xVar
	'
	'End Property
	
	Public Property CompanyID() As Integer
		Get
			
			' Return the objects Company ID
			Return m_iCompanyID
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the objects Company ID
			m_iCompanyID = Value
			
		End Set
	End Property
	
	
	Public Property Code() As String
		Get
			
			' Return the objects Code
			Return m_sCode.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects Code
			m_sCode.Value = Value
			
		End Set
	End Property
	
	
	Public Property Description() As String
		Get
			
			' Return the objects Description
			Return m_sDescription.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects Description
			m_sDescription.Value = Value
			
		End Set
	End Property
	
	
	Public Property CaptionID() As Integer
		Get
			
			' Return the objects Caption ID
			Return m_lCaptionID
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the objects Caption ID
			m_lCaptionID = Value
			
		End Set
	End Property
	
	
	Public Property ParentID() As Integer
		Get
			
			' Return the objects ParentID
			Return m_iParentID
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the objects ParentID
			m_iParentID = Value
			
		End Set
	End Property
	
	
	Public Property RegNo1() As String
		Get
			
			' Return the objects RegNo1
			Return m_sRegNo1.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects RegNo1
			m_sRegNo1.Value = Value
			
		End Set
	End Property
	
	
	Public Property RegNo2() As String
		Get
			
			' Return the objects RegNo2
			Return m_sRegNo2.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects RegNo2
			m_sRegNo2.Value = Value
			
		End Set
	End Property
	
	
	Public Property CurrencyID() As Integer
		Get
			
			Return m_iCurrencyID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iCurrencyID = Value
			
		End Set
	End Property
	
	
	Public Property Address1() As String
		Get
			
			' Return the objects Address1
			Return m_sAddress1.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects Address1
			m_sAddress1.Value = Value
			
		End Set
	End Property
	
	
	Public Property Address2() As String
		Get
			
			' Return the objects Address2
			Return m_sAddress2.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects Address2
			m_sAddress2.Value = Value
			
		End Set
	End Property
	
	
	Public Property Address3() As String
		Get
			
			' Return the objects Address3
			Return m_sAddress3.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects Address3
			m_sAddress3.Value = Value
			
		End Set
	End Property
	
	
	Public Property Address4() As String
		Get
			
			' Return the objects Address4
			Return m_sAddress4.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects Address4
			m_sAddress4.Value = Value
			
		End Set
	End Property
	
	
	Public Property PostalCode() As String
		Get
			
			' Return the objects PostalCode
			Return m_sPostalCode.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects PostalCode
			m_sPostalCode.Value = Value
			
		End Set
	End Property
	
	
	Public Property CountryID() As Integer
		Get
			
			' Return the objects CountryID
			Return m_iCountryID
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the objects CountryID
			m_iCountryID = Value
			
		End Set
	End Property
	
	
	Public Property PhoneAreaCode() As String
		Get
			
			' Return the objects PhoneAreaCode
			Return m_sPhoneAreaCode.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects PhoneAreaCode
			m_sPhoneAreaCode.Value = Value
			
		End Set
	End Property
	
	
	Public Property PhoneNumber() As String
		Get
			
			' Return the objects PhoneNumber
			Return m_sPhoneNumber.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects PhoneNumber
			m_sPhoneNumber.Value = Value
			
		End Set
	End Property
	
	
	Public Property PhoneExtension() As String
		Get
			
			' Return the objects PhoneExtension
			Return m_sPhoneExtension.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects PhoneExtension
			m_sPhoneExtension.Value = Value
			
		End Set
	End Property
	
	
	Public Property FaxAreaCode() As String
		Get
			
			' Return the objects FaxAreaCode
			Return m_sFaxAreaCode.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects FaxAreaCode
			m_sFaxAreaCode.Value = Value
			
		End Set
	End Property
	
	
	Public Property FaxNumber() As String
		Get
			
			' Return the objects FaxNumber
			Return m_sFaxNumber.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects FaxNumber
			m_sFaxNumber.Value = Value
			
		End Set
	End Property
	
	
	Public Property FaxExtension() As String
		Get
			
			' Return the objects FaxExtension
			Return m_sFaxExtension.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects FaxExtension
			m_sFaxExtension.Value = Value
			
		End Set
	End Property
	' DC 31/01/00
	' DC 31/01/00
	Public Property Email() As String
		Get
			
			' Return the objects Email
			Return m_sEmail.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects Email
			m_sEmail.Value = Value
			
		End Set
	End Property
	' DC 31/01/00
	' DC 31/01/00
	Public Property VatNo() As String
		Get
			
			' Return the objects VatNo
			Return m_sVatNo.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects VatNo
			m_sVatNo.Value = Value
			
		End Set
	End Property
	' DC 31/01/00
	' DC 31/01/00
	Public Property SenderMailboxId() As String
		Get
			
			' Return the objects SenderMailboxId
			Return m_sSenderMailboxId.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects SenderMailboxId
			m_sSenderMailboxId.Value = Value
			
		End Set
	End Property
	' DC 31/01/00
	' DC 31/01/00
	Public Property BrokerABIId() As String
		Get
			
			' Return the objects BrokerABIId
			Return m_sBrokerABIId.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects BrokerABIId
			m_sBrokerABIId.Value = Value
			
		End Set
	End Property
	' DC 31/01/00
	' DC 31/01/00
	Public Property UserLicenceId() As Integer
		Get
			
			' Return the objects UserLicenceId
			Return m_iUserLicenceId
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the objects UserLicenceId
			m_iUserLicenceId = Value
			
		End Set
	End Property
	' DC 31/01/00
	' DC 31/01/00
	Public Property PMCompanyNumber() As Integer
		Get
			
			' Return the objects PMCompanyNumber
			Return m_iPMCompanyNumber
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the objects PMCompanyNumber
			m_iPMCompanyNumber = Value
			
		End Set
	End Property
	' DC 31/01/00
	' DC 31/01/00
	Public Property DefaultIndicator() As String
		Get
			
			' Return the objects DefaultIndicator
			Return m_sDefaultIndicator.Value
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects DefaultIndicator
			m_sDefaultIndicator.Value = Value
			
		End Set
	End Property
	' PRIVATE Property Procedures (End)
	
	' PUBLIC Methods (Begin)
	
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Entry point for any initialisation code for this
	'              object.
	'
	' ***************************************************************** '
	Public Function Initialise(ByRef oBusiness As Object) As Integer



		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Store the instance of the business object
			' into the memberfor use by lookup
			m_oBusiness = oBusiness
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: Load (Standard Method)
	'
	' Description: Load Interface defaults and get details replaces
	'              form load event
	'
	' ***************************************************************** '
	Public Function Load_Renamed() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Set the interface default values.
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Gets the interface details to be displayed.
			m_lReturn = CType(GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name: ShowForm (Standard Method)
	'
	' Description: Show the form
	'
	' ***************************************************************** '
	Public Function ShowForm(ByRef lDisplayState As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Show the the form, allow user input etc.
			VB6.ShowForm(Me, lDisplayState)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************************
	'
	' Name: SetFieldValidation
	'
	' Description: Sets the rules for validating fields.
	'
	' ***************************************************************************
	Public Function SetFieldValidation() As Integer
		
		Dim result As Integer = 0
		Try 
			
			m_oFormFields = New iPMFormControl.FormFields()
			
			m_oFormFields.LanguageID = g_iLanguageID
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add the controls...
			
			' Short Code
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Description
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Reg number 1
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRegNo1, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Reg number 2
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRegNo2, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Address 1
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress1, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Address 2
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress2, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Address 3
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress3, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Address 4
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAddress4, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Post Code
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPostalCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Phone Area
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPhoneAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Phone Number
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPhoneNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Phone Extension
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPhoneExtension, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Fax Area
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFaxAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Fax Number
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFaxNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Fax Extension
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFaxExtension, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' DC 31/01/00
			' Email
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEmail, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' DC 31/01/00
			' VatNo
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtVatNo, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' DC 31/01/00
			' SenderMailboxId
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSenderMailboxId, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' DC 31/01/00
			' BrokerABIId
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBrokerABIId, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' DC 31/01/00
			' UserLicenceId
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtUserLicenceId, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' DC 31/01/00
			' PMCompanyNumber
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPMCompanyNumber, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' DC 31/01/00
			' DefaultIndicator
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDefaultIndicator, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' PUBLIC Methods (End)
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: PropertiesToInterface
	'
	' Description: Updates all interface details from the Form properties
	'
	' ***************************************************************** '
	Private Function PropertiesToInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to assign the all of the interface
			' details from the business object, using the FormatField
			' function for any type conversion.
			'
			' Example:-
			'
			'    txtDesc.Text = FormatField( _
			''        iFormatType:=PMFormatString, _
			''        vFieldValue:=m_sDesc$)
			'
			'    optChoice.Value = CBool(FormatField( _
			''        iFormatType:=PMFormatBoolean, _
			''        vFieldValue:=m_iChoice%))
			'
			'    txtDate.Text = FormatField( _
			''        iFormatType:=PMFormatDateLong, _
			''        vFieldValue:=m_dtDate)
			'
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			txtCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sCode.Value)
			
			txtDescription.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sDescription.Value)
			
			txtRegNo1.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sRegNo1.Value)
			
			txtRegNo2.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sRegNo2.Value)
			
			If m_iCurrencyID <> 0 Then
				uctCurrency.CurrencyId = m_iCurrencyID
			End If
			
			txtAddress1.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sAddress1.Value)
			
			txtAddress2.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sAddress2.Value)
			
			txtAddress3.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sAddress3.Value)
			
			txtAddress4.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sAddress4.Value)
			
			txtPostalCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPostalCode.Value)
			
			txtPhoneAreaCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPhoneAreaCode.Value)
			
			txtPhoneNumber.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPhoneNumber.Value)
			
			txtPhoneExtension.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPhoneExtension.Value)
			
			txtFaxAreaCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sFaxAreaCode.Value)
			
			txtFaxNumber.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sFaxNumber.Value)
			
			txtFaxExtension.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sFaxExtension.Value)
			
			' DC 31/01/00
			txtEmail.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sEmail.Value)
			
			' DC 31/01/00
			txtVatNo.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sVatNo.Value)
			
			' DC 31/01/00
			txtSenderMailboxId.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sSenderMailboxId.Value)
			
			' DC 31/01/00
			txtBrokerABIId.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sBrokerABIId.Value)
			
			' DC 31/01/00
			txtUserLicenceId.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatInteger, vFieldValue:=CStr(m_iUserLicenceId))
			
			' DC 31/01/00
			txtPMCompanyNumber.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatInteger, vFieldValue:=CStr(m_iPMCompanyNumber))
			
			' DC 31/01/00
			txtDefaultIndicator.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sDefaultIndicator.Value)
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the properties", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: InterfaceToProperties
	'
	' Description: Updates the Properties from the interface details.
	'
	' ***************************************************************** '
	Private Function InterfaceToProperties() As Integer
		
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
			'    m_sName$ = trim$(txtName.Text)
			'    m_dtDate = CDate(txtDate.Text)
			'    m_iCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			m_sCode.Value = txtCode.Text.Trim()
			m_sDescription.Value = txtDescription.Text.Trim()
			'BB Remove "Subsidiary of" combo
			'm_iParentID = cmbParent.ItemData(cmbParent.ListIndex)
			m_sRegNo1.Value = txtRegNo1.Text.Trim()
			m_sRegNo2.Value = txtRegNo2.Text.Trim()
			m_iCurrencyID = uctCurrency.CurrencyId
			m_sAddress1.Value = txtAddress1.Text.Trim()
			m_sAddress2.Value = txtAddress2.Text.Trim()
			m_sAddress3.Value = txtAddress3.Text.Trim()
			m_sAddress4.Value = txtAddress4.Text.Trim()
			m_sPostalCode.Value = txtPostalCode.Text.Trim()
			m_iCountryID = VB6.GetItemData(cmbCountry, cmbCountry.SelectedIndex)
			m_sPhoneAreaCode.Value = txtPhoneAreaCode.Text.Trim()
			m_sPhoneNumber.Value = txtPhoneNumber.Text.Trim()
			m_sPhoneExtension.Value = txtPhoneExtension.Text.Trim()
			m_sFaxAreaCode.Value = txtFaxAreaCode.Text.Trim()
			m_sFaxNumber.Value = txtFaxNumber.Text.Trim()
			m_sFaxExtension.Value = txtFaxExtension.Text.Trim()
			
			' DC 31/01/00
			m_sEmail.Value = txtEmail.Text.Trim()
			m_sVatNo.Value = txtVatNo.Text.Trim()
			m_sSenderMailboxId.Value = txtSenderMailboxId.Text.Trim()
			m_sBrokerABIId.Value = txtBrokerABIId.Text.Trim()
			'DC 19/04/00
			'Should have been checking if True and not False

			If Not True Or Convert.IsDBNull(txtUserLicenceId.Text) Or IsNothing(txtUserLicenceId.Text) Then
				txtUserLicenceId.Text = CStr(0)
			End If
			m_iUserLicenceId = CInt(txtUserLicenceId.Text)
			'DC 19/04/00
			'Should have been checking if True and not False

			If Not True Or Convert.IsDBNull(txtPMCompanyNumber.Text) Or IsNothing(txtPMCompanyNumber.Text) Then
				txtPMCompanyNumber.Text = CStr(0)
			End If
			m_iPMCompanyNumber = CInt(txtPMCompanyNumber.Text)
			m_sDefaultIndicator.Value = txtDefaultIndicator.Text.Trim()
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the form properties", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
			
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
			
			m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			' {* USER DEFINED CODE (End) *}
			
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			' DC 31/01/00
			ReDim m_ctlTabFirstLast(1, 2)
			
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
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtCode
			m_ctlTabFirstLast(ACControlEnd, 0) = txtRegNo2
			m_ctlTabFirstLast(ACControlStart, 1) = txtAddress1
			m_ctlTabFirstLast(ACControlEnd, 1) = txtFaxExtension
			
			' DC 31/01/00
			m_ctlTabFirstLast(ACControlStart, 2) = txtEmail
			m_ctlTabFirstLast(ACControlEnd, 2) = txtDefaultIndicator
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
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
			

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
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

			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACMainTabTitle0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACMainTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblExtension.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACExtensionCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblAddress1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddress1Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblAddress2.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddress2Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblAddress3.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddress3Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblAddress4.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddress4Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblPostalCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACPostalCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblCountry.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCountryCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblPhone.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACPhoneCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblFax.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACFaxCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblAreaCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAreaCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACNumberCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDescriptionCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			'BB Remove "Subsidiary of" combo and label
			
			'    lblParent.Caption = iPMFunc.GetResData( _
			''          iLangID:=g_iLanguageID%, _
			''          lID:=ACParentCaption, _
			''          iDataType:=PMResString)
			

			lblRegNo1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACRegNo1Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblRegNo2.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACRegNo2Caption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblBaseCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBaseCurrencyCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' DC 31/01/00

			lblEmail.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEmailCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' DC 31/01/00

			lblVatNo.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACVatNoCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' DC 31/01/00

			lblSenderMailboxId.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACSenderMailboxIdCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' DC 31/01/00

			lblBrokerABIId.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACBrokerABIIdCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' DC 31/01/00

			lblUserLicenceId.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACUserLicenceIdCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' DC 31/01/00

			lblPMCompanyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACPMCompanyNumberCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			' DC 31/01/00

			lblDefaultIndicator.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACDefaultIndicatorCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
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
	' Name: GetInterfaceDetails
	'
	' Description: Gets the interface details and sets the appropriate
	'              style.
	'
	' ***************************************************************** '
	Private Function GetInterfaceDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView
					' Assign the details from the form properties
					' to the interface.
					m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)
					
					' Check for errors
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						' Failed to assign the details.
						Return gPMConstants.PMEReturnCode.PMFalse
					End If
			End Select
			
			' Display all of the lookup details.
			m_lReturn = CType(DisplayLookupDetails(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Check the task.
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				' Disable the interface to only allow viewing.
				m_lReturn = CType(DisableForm(lDisabled:=True), gPMConstants.PMEReturnCode)
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to disable the interface
					result = gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
	Private Function DisplayLookupDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the lookup values.
			
			m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
			
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
			
			m_lReturn = CType(GetLookupDetails(sLookupTable:=gPMConstants.PMLookupCountry, ctlLookup:=cmbCountry), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			
			
			
			' Error Section.
			
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
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer
        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail constants.
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

                'Developer guide No.29
                Dim NewIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr)))
                ' Check if this is the selected index.
                If CBool(CStr((CStr(m_vLookupValues(ACValueID, lRow))) = CStr(m_vLookupDetails(ACDetailKey, lCntr)))) Then


                    'Developer Guide No.28
                    ctlLookup.SelectedIndex = NewIndex
                End If
            Next lCntr

            ' If nothing yet selected (index = -1)
            ' select first item (index = 0)

            'Developer Guide No.28
            If ctlLookup.SelectedIndex < 0 Then
                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.
                    If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Form hasn't been cancelled
                        ' Update the properties from the interface.
                        m_lReturn = CType(InterfaceToProperties(), gPMConstants.PMEReturnCode)

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the properties", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                            Return result
                        End If
                    End If

            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set all of the forms controls to the disable state.

            For Each ctlFormControl As Control In ContainerHelper.Controls(Me)
                ' Check the type of the control.
                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is RadioButton) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                End If
            Next ctlFormControl

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen relating to the tab show
        If SSTabHelper.GetSelectedIndex(tabMainTab) = 0 Then
            'Developer Guide No.184
            PMHelpFunc.g_sProductFamily = g_sProductFamily
            m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID1), gPMConstants.PMEReturnCode)

        ElseIf (SSTabHelper.GetSelectedIndex(tabMainTab) = 1) Then
            'Developer Guide No.184
            m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID2), gPMConstants.PMEReturnCode)
        End If
    End Sub


    Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

    End Sub

    ' PRIVATE Methods (End)


    ' PRIVATE Events (Begin)
    Private Sub frmDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
                m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

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

            ' Destroy this instance of the business object
            ' from memory.
            m_oBusiness = Nothing

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

    Private Sub frmDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
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



            ' Error Section.

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



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = m_oFormFields.CheckMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
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
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

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

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

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



            ' Error Section

            Exit Sub
        End Try


    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (Text1_Change) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Text1_Change()
    '
    'End Sub

    Private Sub txtAddress1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress1.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAddress1)
    End Sub

    Private Sub txtAddress2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress2.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAddress2)
    End Sub

    Private Sub txtAddress3_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress3.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAddress3)
    End Sub

    Private Sub txtAddress4_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress4.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAddress4)
    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter
        iPMFunc.SelectText(txtCode)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
        iPMFunc.SelectText(txtDescription)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtFaxAreaCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxAreaCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFaxAreaCode)
    End Sub

    Private Sub txtFaxExtension_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxExtension.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFaxExtension)
    End Sub

    Private Sub txtFaxNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFaxNumber)
    End Sub

    Private Sub txtPhoneAreaCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneAreaCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPhoneAreaCode)
    End Sub

    Private Sub txtPhoneExtension_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneExtension.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPhoneExtension)
    End Sub

    Private Sub txtPhoneNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPhoneNumber)
    End Sub

    Private Sub txtPostalCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPostalCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPostalCode)
    End Sub

    Private Sub txtRegNo1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegNo1.Enter
        iPMFunc.SelectText(txtRegNo1)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRegNo1)
    End Sub
    Private Sub txtRegNo1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegNo1.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRegNo1)
    End Sub

    Private Sub txtRegNo2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegNo2.Enter
        iPMFunc.SelectText(txtRegNo2)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRegNo2)
    End Sub
    Private Sub txtAddress1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress1.Enter
        iPMFunc.SelectText(txtAddress1)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAddress1)
    End Sub
    Private Sub txtAddress2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress2.Enter
        iPMFunc.SelectText(txtAddress2)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAddress2)
    End Sub
    Private Sub txtAddress3_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress3.Enter
        iPMFunc.SelectText(txtAddress3)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAddress3)
    End Sub
    Private Sub txtAddress4_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAddress4.Enter
        iPMFunc.SelectText(txtAddress4)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAddress4)
    End Sub
    Private Sub txtPostalCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPostalCode.Enter
        iPMFunc.SelectText(txtPostalCode)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPostalCode)
    End Sub
    Private Sub txtPhoneAreaCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneAreaCode.Enter
        iPMFunc.SelectText(txtPhoneAreaCode)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPhoneAreaCode)
    End Sub
    Private Sub txtPhoneNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneNumber.Enter
        iPMFunc.SelectText(txtPhoneNumber)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPhoneNumber)
    End Sub
    Private Sub txtPhoneExtension_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPhoneExtension.Enter
        iPMFunc.SelectText(txtPhoneExtension)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPhoneExtension)
    End Sub
    Private Sub txtFaxAreaCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxAreaCode.Enter
        iPMFunc.SelectText(txtFaxAreaCode)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFaxAreaCode)
    End Sub
    Private Sub txtFaxNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxNumber.Enter
        iPMFunc.SelectText(txtFaxNumber)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFaxNumber)
    End Sub
    Private Sub txtFaxExtension_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFaxExtension.Enter
        iPMFunc.SelectText(txtFaxExtension)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFaxExtension)
    End Sub
    Private Sub txtRegNo2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegNo2.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRegNo2)
    End Sub
    ' DC 31/01/00
    Private Sub txtEmail_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEmail.Enter
        iPMFunc.SelectText(txtEmail)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEmail)
    End Sub
    ' DC 31/01/00
    Private Sub txtEmail_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEmail.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEmail)
    End Sub
    ' DC 31/01/00
    Private Sub txtVatNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtVatNo.Enter
        iPMFunc.SelectText(txtVatNo)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtVatNo)
    End Sub
    ' DC 31/01/00
    Private Sub txtVatNo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtVatNo.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtVatNo)
    End Sub
    ' DC 31/01/00
    Private Sub txtSenderMailboxId_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSenderMailboxId.Enter
        iPMFunc.SelectText(txtSenderMailboxId)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSenderMailboxId)
    End Sub
    ' DC 31/01/00
    Private Sub txtSenderMailboxId_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSenderMailboxId.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSenderMailboxId)
    End Sub
    ' DC 31/01/00
    Private Sub txtBrokerABIId_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBrokerABIId.Enter
        iPMFunc.SelectText(txtBrokerABIId)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtBrokerABIId)
    End Sub
    ' DC 31/01/00
    Private Sub txtBrokerABIId_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBrokerABIId.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtBrokerABIId)
    End Sub
    ' DC 31/01/00
    Private Sub txtUserLicenceId_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtUserLicenceId.Enter
        iPMFunc.SelectText(txtUserLicenceId)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtUserLicenceId)
    End Sub
    ' DC 31/01/00
    Private Sub txtUserLicenceId_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtUserLicenceId.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtUserLicenceId)
    End Sub
    ' DC 31/01/00
    Private Sub txtPMCompanyNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMCompanyNumber.Enter
        iPMFunc.SelectText(txtPMCompanyNumber)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPMCompanyNumber)
    End Sub
    ' DC 31/01/00
    Private Sub txtPMCompanyNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPMCompanyNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPMCompanyNumber)
    End Sub
    ' DC 31/01/00
    Private Sub txtDefaultIndicator_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDefaultIndicator.Enter
        iPMFunc.SelectText(txtDefaultIndicator)
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDefaultIndicator)
    End Sub
    ' DC 31/01/00
    Private Sub txtDefaultIndicator_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDefaultIndicator.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDefaultIndicator)
    End Sub
End Class
