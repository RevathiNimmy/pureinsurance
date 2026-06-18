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
	' Date: 05/05/1999
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
    Private Const vbFormCode As Integer = 0
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
	Private m_sStepStatus As String = ""
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_sDescription As Object
	Private m_sReference As Object
	Private m_cSumInsured As Object
	Private m_dtDateAdded As Object
	Private m_dtDateDeleted As Object
	Private m_iIsValuationRequired As String = ""
	Private m_dtValuationDate As Object
	
	Private m_iIsValuation As Integer
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iGISSumInsured.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	'Private m_oBusiness As bSIRIPTExtras.Business
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues As Object
	Private m_vLookupDetails As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	Public Property Description() As Object
		Get
			Return m_sDescription
		End Get
		Set(ByVal Value As Object)


			m_sDescription = Value
		End Set
	End Property
	
	Public Property Reference() As Object
		Get
			Return m_sReference
		End Get
		Set(ByVal Value As Object)


			m_sReference = Value
		End Set
	End Property
	
	Public Property SumInsured() As Object
		Get
			Return m_cSumInsured
		End Get
		Set(ByVal Value As Object)


			m_cSumInsured = Value
		End Set
	End Property
	
	Public Property DateAdded() As Object
		Get
			Return m_dtDateAdded
		End Get
		Set(ByVal Value As Object)


			m_dtDateAdded = Value
		End Set
	End Property
	
	Public Property DateDeleted() As Object
		Get
			Return m_dtDateDeleted
		End Get
		Set(ByVal Value As Object)


			m_dtDateDeleted = Value
		End Set
	End Property
	
	Public Property IsValuationRequired() As String
		Get
			Return m_iIsValuationRequired
		End Get
		Set(ByVal Value As String)

			m_iIsValuationRequired = CStr(Value)
		End Set
	End Property
	
	Public Property ValuationDate() As Object
		Get
			Return m_dtValuationDate
		End Get
		Set(ByVal Value As Object)


			m_dtValuationDate = Value
		End Set
	End Property
	
	Public Property IsValuation() As Integer
		Get
			Return m_iIsValuation
		End Get
		Set(ByVal Value As Integer)
			m_iIsValuation = Value
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
	
	
	Public Property StepStatus() As String
		Get
			
			Return m_sStepStatus
			
		End Get
		Set(ByVal Value As String)
			
			m_sStepStatus = Value
			
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
		Dim lMandatory As gPMConstants.PMEMandatoryStatus
		
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
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSumInsured, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateAdded, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If m_iTask = gPMConstants.PMEComponentAction.PMDelete Then
				lMandatory = gPMConstants.PMEMandatoryStatus.PMMandatory
			Else
				lMandatory = gPMConstants.PMEMandatoryStatus.PMNonMandatory
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateDeleted, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=lMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtValuationDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
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
			
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			
			'    m_lReturn& = m_oBusiness.GetDetails(vExtras:=m_vExtras)
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors
			'    If (m_lReturn& <> PMTrue) Then
			'        ' Failed to get details.
			'        GetBusiness = PMFalse
			'
			'        ' Log Error.
			'        LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Failed to get details from the business object", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="GetBusiness"
			'
			'        Exit Function
			'    End If
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
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
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_dtDDate)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			

			If Convert.IsDBNull(m_sDescription) Or IsNothing(m_sDescription) Then
				txtDescription.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sDescription)
			End If
			

			If Convert.IsDBNull(m_sReference) Or IsNothing(m_sReference) Then
				txtReference.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtReference, vControlValue:=m_sReference)
			End If
			

			If Convert.IsDBNull(m_cSumInsured) Or IsNothing(m_cSumInsured) Then
				txtSumInsured.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSumInsured, vControlValue:=m_cSumInsured)
			End If
			

			If Convert.IsDBNull(m_dtDateAdded) Or IsNothing(m_dtDateAdded) Then
				txtDateAdded.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDateAdded, vControlValue:=m_dtDateAdded)
			End If
			

			If Convert.IsDBNull(m_dtDateDeleted) Or IsNothing(m_dtDateDeleted) Then
				txtDateDeleted.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDateDeleted, vControlValue:=m_dtDateDeleted)
			End If
			

			If Convert.IsDBNull(m_iIsValuationRequired) Or IsNothing(m_iIsValuationRequired) Then
				chkIsValuationRequired.CheckState = CheckState.Unchecked
				txtValuationDate.Enabled = False
			Else
				If m_iIsValuationRequired = "Yes" Then
					chkIsValuationRequired.CheckState = CheckState.Checked
				End If
			End If
			

			If Convert.IsDBNull(m_dtValuationDate) Or IsNothing(m_dtValuationDate) Then
				txtValuationDate.Text = ""
			Else
				m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtValuationDate, vControlValue:=m_dtValuationDate)
			End If
			
			' {* USER DEFINED CODE (End) *}
			
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				m_lReturn = CType(DisableForm(lDisabled:=True), gPMConstants.PMEReturnCode)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
			
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

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the business object.
			
			' Assign the details from the interface to the data storage.
			m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
	Private Function InterfaceToData() As Integer
		
		Dim result As Integer = 0
		Dim sTemp As String = ""

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
			'    m_DDate = CDate(txtDescription.Text)
			'    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
			'    m_lReturn& = m_oFormFields.UnformatControl(txtName)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			


			m_sDescription = m_oFormFields.UnformatControl(txtDescription)


			m_sReference = m_oFormFields.UnformatControl(txtReference)


			m_cSumInsured = m_oFormFields.UnformatControl(txtSumInsured)


			m_dtDateAdded = m_oFormFields.UnformatControl(txtDateAdded)


			m_dtDateDeleted = m_oFormFields.UnformatControl(txtDateDeleted)
			
			If chkIsValuationRequired.CheckState = CheckState.Checked Then
				m_iIsValuationRequired = "Yes"
			Else
				m_iIsValuationRequired = "No"
			End If
			


			m_dtValuationDate = m_oFormFields.UnformatControl(txtValuationDate)
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
			
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
			
			If IsValuation = 0 Then
				chkIsValuationRequired.Visible = False
				lblValuationDate.Visible = False
				txtValuationDate.Visible = False
			End If
			
			If m_iTask = gPMConstants.PMEComponentAction.PMDelete Then
				txtDescription.Enabled = False
				txtReference.Enabled = False
				txtDateAdded.Enabled = False
				txtSumInsured.Enabled = False
				chkIsValuationRequired.Enabled = False
				txtValuationDate.Enabled = False
			Else
				txtDateDeleted.Enabled = False
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			ReDim m_ctlTabFirstLast(1, 0)
			
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
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtDescription
			m_ctlTabFirstLast(ACControlEnd, 0) = txtValuationDate
			
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
			

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
				                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

			SSTabHelper.SetTabEnabled(tabMainTab, 0, True)
			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
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
			

			lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCaptionDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCaptionReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblSumInsured.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCaptionSumInsured, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblDateAdded.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCaptionDateAdded, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblDateDeleted.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCaptionDateDeleted, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			chkIsValuationRequired.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCaptionIsValuationRequired, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblValuationDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCaptionValuationDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
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

			For	Each ctlFormControl As Control In ContainerHelper.Controls(Me)
				' Check the type of the control.
				If TypeOf ctlFormControl Is TextBox Then
					ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				ElseIf (TypeOf ctlFormControl Is ComboBox) Then 
					ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				ElseIf (TypeOf ctlFormControl Is CheckBox) Then 
					ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				ElseIf (TypeOf ctlFormControl Is RadioButton) Then 
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
	
	' ***************************************************************** '
	'
	' Name: ValidateForm
	'
	' Description:
	'
	' History: 07/09/1999 Tomo - Created.
	'
	' ***************************************************************** '
	Private Function ValidateForm() As Integer
		
		Dim result As Integer = 0
		Dim cTemp As Decimal
		Dim dTemp As Double
		

        'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_ValidateForm)")
		
		result = gPMConstants.PMEReturnCode.PMTrue
		

		dTemp = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtSumInsured))
		
		Try 
			
			cTemp = dTemp
		
		Catch 
			MessageBox.Show("Sum Insured is too large - please re-enter", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
			txtSumInsured.Focus()
			Return gPMConstants.PMEReturnCode.PMFalse
		End Try
		
		
		Return result
		
Err_ValidateForm: 
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	' PRIVATE Methods (End)
	
	' PRIVATE Events (Begin)
	
	Private Sub chkIsValuationRequired_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsValuationRequired.CheckStateChanged
		
		If chkIsValuationRequired.CheckState = CheckState.Checked Then
			txtValuationDate.Enabled = True
		Else
			txtValuationDate.Text = ""
			txtValuationDate.Enabled = False
		End If
		
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		

		' Forms initialise event.
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			'    m_lReturn& = g_oObjectManager.GetInstance( _
			'oObject:=m_oBusiness, _
			'sClassName:="bSIRIPTExtras.Business", _
			'vInstanceManager:=PMGetViaClientManager)
			
			' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        ' Failed to get an instance of the business object.
			'        m_lErrorNumber& = PMFalse
			'
			'        ' Display error stating the problem.
			'
			'        ' Get description from the resource file.
			'        sTitle$ = iPMFunc.GetResData( _
			''            iLangID:=g_iLanguageID%, _
			''            lID:=ACBusinessFailTitle, _
			''            iDateDeleted:=PMResString)
			'
			'        sMessage$ = iPMFunc.GetResData( _
			''            iLangID:=g_iLanguageID%, _
			''            lID:=ACBusinessFail, _
			''            iDateDeleted:=PMResString)
			'
			'        ' Display message.
			'        MsgBox sMessage$, vbCritical, sTitle$
			'
			'        Exit Sub
			'    End If
			
			' Create an instance of the general interface object.
			m_oGeneral = New iGISSumInsured.General()
			
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
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
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
			'    m_lReturn& = m_oBusiness.SetProcessModes( _
			'vTask:=CVar(m_iTask%), _
			'vNavigate:=CVar(m_lNavigate&), _
			'vProcessMode:=CVar(m_lProcessMode&), _
			'vTransactionType:=CVar(m_sTransactionType$), _
			'vEffectiveDate:=CVar(m_dtEffectiveDate))
			
			' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        ' Failed to process the interface.
			'        m_lErrorNumber& = PMFalse
			'
			'        ' Log Error Message
			'        LogMessage _
			''            iType:=PMLogOnError, _
			''            sMsg:="Failed to set the process modes for the business object", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="Form_Load"
			'
			'        Exit Sub
			'    End If
			
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

            ' developer guide no. 19
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'Cancel = 1
                    eventArgs.Cancel = True
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
			
			'    ' Terminate the business object
			'    m_lReturn& = m_oBusiness.Terminate()
			'
			'    ' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        m_lErrorNumber& = PMFalse
			'
			'        ' Log Error.
			'        LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Failed to terminate the business object", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="Form_QueryUnload"
			'    End If
			'
			'    ' Destroy the instance of the business object
			'    ' from memory.
			'    Set m_oBusiness = Nothing
			
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

            'developer guide no.293

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
		Catch 
			
			
			
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		

		'Try 
			'
			'    With tabMainTab
			'        ' Set the default button.
			'        If (.Tab < cmdNext.Count) Then
			'            cmdNext(.Tab).Default = True
			'        Else
			'            cmdOK.Default = True
			'        End If
			''
			'        ' Now I know this is crap, this goes against
			'        ' all my principles, but for some reason when
			'        ' using the mouse to select a tab the setfocus
			'        ' code below doesn't work. The cursor sticks,
			'        ' and you can't tab off. Therefore I've used
			'        ' this to get around the problem.
			'        DoEvents
			''
			'        ' Set focus to the first control on the tab.
			'        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
			'            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
			'        End If
			'    End With
		'
		'Catch 
			'
			'
			'
			'
			'
			'tabMainTabPreviousTab = tabMainTab.SelectedIndex
		'End Try
		
	End Sub
	
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
			
			' validate data entered.
			m_lReturn = CType(ValidateForm(), gPMConstants.PMEReturnCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
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
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
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
			m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
			
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
	
	Private Sub txtDateAdded_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateAdded.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateAdded)
	End Sub
	
	Private Sub txtDateAdded_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateAdded.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateAdded)
	End Sub
	
	Private Sub txtDateDeleted_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateDeleted.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateDeleted)
	End Sub
	
	Private Sub txtDateDeleted_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateDeleted.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateDeleted)
	End Sub
	
	Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
	End Sub
	
	Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
	End Sub
	
	Private Sub txtReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReference.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtReference)
	End Sub
	
	Private Sub txtReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReference.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtReference)
	End Sub
	
	Private Sub txtSumInsured_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSumInsured.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSumInsured)
	End Sub
	
	Private Sub txtSumInsured_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSumInsured.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSumInsured)
	End Sub
	
	Private Sub txtValuationDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValuationDate.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtValuationDate)
	End Sub
	
	Private Sub txtValuationDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValuationDate.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtValuationDate)
	End Sub
	
	' PRIVATE Events (End)
End Class
