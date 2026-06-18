Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
' developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 06/10/1998
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
    'DEEPAK_COMMENT: Replaced iPMFunc.GetResData with GetResData in the whole document
	
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
	
	' Status members
	Private m_sProcessStatus As New FixedLengthString(2)
	Private m_sMapStatus As New FixedLengthString(2)
	Private m_sStepStatus As New FixedLengthString(2)
	
	' {* USER DEFINED CODE (Begin) *}
	' Form Constants for Captions
	
	Const ACInterfaceCaption As Integer = 100
	Const ACMainTabTitle0 As Integer = 101
	Const ACExtensionCaption As Integer = 102
	Const ACNumberCaption As Integer = 103
	Const ACAreaCodeCaption As Integer = 104
	Const ACDescriptionCaption As Integer = 105
	Const ACContactTypeCaption As Integer = 106
	Const ACAdPostcodeCaption As Integer = 107
	Const AClbAdReferenceCaption As Integer = 108
	
	
	' Button Constants for Captions
	
	Const ACHelpCaption As Integer = 200
	Const ACCancelCaption As Integer = 201
	Const ACOKCaption As Integer = 202
	Const ACNextCaption0 As Integer = 203
	
	Private m_lContactCnt As Integer
	Private m_iContactTypeID As Integer
	Private m_sContactType As String = ""
	Private m_sDescription As String = ""
	Private m_sAreaCode As String = ""
	Private m_sNumber As String = ""
	Private m_sExtension As String = ""
	'DC 28/06/00
	Private m_vContactTypes( ,  ) As Object
	Private m_vContactTypeId As Object
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iPMBContact.General
	
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
	Private m_ctlTabFirstLast( ,  ) As Control

	Private m_vUnderwritingOrBroking As String = ""
	Private m_sUniqueId As String = ""
	Private m_sScreenHeirarchy As String = ""

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
	
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
	Public Property ContactCnt() As Integer
		Get
			
			' Return the objects parameter value.
			Return m_lContactCnt
			
		End Get
		Set(ByVal Value As Integer)
			
			' Set the objects parameter value.
			m_lContactCnt = Value
			
		End Set
	End Property
	Public Property AreaCode() As String
		Get
			
			Return m_sAreaCode
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sAreaCode = Value
			
		End Set
	End Property
	
	Public Property Number() As String
		Get
			
			Return m_sNumber
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sNumber = Value
			
		End Set
	End Property
	
	Public Property ContactType() As String
		Get
			
			Return m_sContactType
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sContactType = Value
			
		End Set
	End Property
	
	Public Property Description() As String
		Get
			
			Return m_sDescription
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sDescription = Value
			
		End Set
	End Property

	Public Property Extension() As String
		Get

			Return m_sExtension

		End Get
		Set(ByVal Value As String)

			' Set the objects parameter value.
			m_sExtension = Value

		End Set
	End Property

	Public Property UniqueId() As String
		Get
			Return m_sUniqueId
		End Get
		Set(ByVal Value As String)
			m_sUniqueId = Value
		End Set
	End Property

	Public Property ScreenHeirarchy() As String
		Get
			Return m_sScreenHeirarchy
		End Get
		Set(ByVal Value As String)
			m_sScreenHeirarchy = Value
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
			
			'contact type
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cmbContactType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Decription type
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'AreaCode type
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAreaCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Number type
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Extension type
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExtension, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
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
			
			' {* USER DEFINED CODE (Begin) *}
			

			m_lReturn = m_oBusiness.GetDetails(vContactCnt:=m_lContactCnt)
			
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
			
			' CTAF 021100 - We want to display the contact type too
			For iTemp As Integer = 0 To cmbContactType.Items.Count - 1
				If VB6.GetItemData(cmbContactType, iTemp) = m_iContactTypeID Then
					cmbContactType.SelectedIndex = iTemp
					Exit For
				End If
			Next iTemp
			
			'    cmbContactType.ListIndex = m_iContactTypeID% - 1
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sDescription)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAreaCode, vControlValue:=m_sAreaCode)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtNumber, vControlValue:=m_sNumber)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtExtension, vControlValue:=m_sExtension)
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the data.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
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
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			
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
			If m_sScreenHeirarchy <> "" Then
				m_sScreenHeirarchy = m_sScreenHeirarchy & "/" & $"ContactType({m_sContactType})"
			End If
			' Check the task.
			Select Case (m_iTask)
				Case gPMConstants.PMEComponentAction.PMAdd
					' Inform the business object with a new data item.

					' {* USER DEFINED CODE (Begin) *}

					m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vContactTypeID:=m_iContactTypeID, vDescription:=m_sDescription, vAreaCode:=m_sAreaCode, vNumber:=m_sNumber, vExtension:=m_sExtension, sUniqueId:=m_sUniqueId, sScreenHeirarchy:=m_sScreenHeirarchy)
					' {* USER DEFINED CODE (End) *}

				Case gPMConstants.PMEComponentAction.PMEdit
					' Inform the business object with an updated data item.

					' {* USER DEFINED CODE (Begin) *}

					m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vContactTypeID:=m_iContactTypeID, vDescription:=m_sDescription, vAreaCode:=m_sAreaCode, vNumber:=m_sNumber, vExtension:=m_sExtension, sUniqueId:=m_sUniqueId, sScreenHeirarchy:=m_sScreenHeirarchy)
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
			
			
			
			' Error Section.
			
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
			
			' Get all of the lookup details.
			
			' {* USER DEFINED CODE (Begin) *}
			'DC 28/06/00 removed
			'    m_lReturn& = GetLookupDetails(sLookupTable:=SIRLookupContactType, _
			''                                ctlLookup:=cmbContactType)
			' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        DisplayLookupDetails = PMFalse
			'        Exit Function
			'    End If
			
			'DC 28/06/00
			'Get contact types

			m_lReturn = m_oBusiness.GetContactTypes(vContactTypes:=m_vContactTypes)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the contact types from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
			End If
			
			'DC 28/06/00
			'   Fill the Contact Type Combo Box
			PopulateContactTypes()
			
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
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			

			m_lReturn = m_oBusiness.GetNext(vContactTypeID:=m_iContactTypeID, vDescription:=m_sDescription, vAreaCode:=m_sAreaCode, vNumber:=m_sNumber, vExtension:=m_sExtension)
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
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
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the data storage.
			'contact type
			'DC 03/08/00
			'check if there has been a contact type selected
			If cmbContactType.SelectedIndex <> -1 Then
				m_iContactTypeID = VB6.GetItemData(cmbContactType, cmbContactType.SelectedIndex)
			End If
			
			m_sContactType = cmbContactType.Text
			
			'description

			m_sDescription = CStr(m_oFormFields.UnformatControl(txtDescription))
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'area code

			m_sAreaCode = CStr(m_oFormFields.UnformatControl(txtAreaCode))
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'number

			m_sNumber = CStr(m_oFormFields.UnformatControl(txtNumber))
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'extension

            'commented follwoing line to fix issue 2228
            'm_sExtension = CStr(m_oFormFields.UnformatControl(txtExtension))
            m_sExtension = CStr(txtExtension.Text)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
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
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the status of the Navigate button.
			'    Select Case (m_lNavigate&)
			'        Case PMNavigateEnabled
			'            cmdNavigate.Visible = True
			'            cmdNavigate.Enabled = True
			'
			'        Case PMNavigateDisabled
			'            cmdNavigate.Visible = True
			'            cmdNavigate.Enabled = False
			'
			'        Case Else
			'            cmdNavigate.Visible = False
			'    End Select
			
			m_lReturn = SetFirstLastControls()
			
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
			m_ctlTabFirstLast(ACControlStart, 0) = cmbContactType
			m_ctlTabFirstLast(ACControlEnd, 0) = txtExtension
			
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
			
			
			' Display all language specific captions

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblExtension.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExtensionCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNumberCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblAreaCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAreaCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDescriptionCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblContactType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACContactTypeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lblAdPostcode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdPostcodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			

			lbAdReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClbAdReferenceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			


			cmdNext(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNextCaption0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			Return result
		
		Catch excep As System.Exception
			'    'sp todo - delete from here onwards
			'    Me.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACInterfaceTitle, _
			''        iDataType:=PMResString)
			'
			'    ' Check for an error.
			'    If (Me.Caption = "") Then
			'        ' Failed to get data from the resource file.
			'        DisplayCaptions = PMFalse
			'
			'        ' Log Error.
			'        LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
			''            "Please check the file exists and the correct captions are available", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="DisplayCaptions"
			'
			'        Exit Function
			'    End If
			'
			'    cmdOK.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACOKButton, _
			''        iDataType:=PMResString)
			'
			'    cmdCancel.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACCancelButton, _
			''        iDataType:=PMResString)
			'
			'    cmdHelp.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACHelpButton, _
			''        iDataType:=PMResString)
			'
			'    cmdNavigate.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACNavigateButton, _
			''        iDataType:=PMResString)
			'
			'    tabMainTab.TabCaption(0) = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACTabTitle1, _
			''        iDataType:=PMResString)
			'
			'    ' {* USER DEFINED CODE (Begin) *}
			'
			'    ' ************************************************************
			'    ' Enter your code here to display all language specific
			'    ' captions.
			'    ' The GetResData function will allow you to do this.
			'    '
			'    ' Example:-
			'    '
			'    '    lblDesc.Caption = iPMFunc.GetResData( _
			''    '        iLangID:=g_iLanguageID%, _
			''    '        lID:=ACDesc, _
			''    '        iDataType:=PMResString)
			'    '
			'    ' NOTE: Replace this section with your new code.
			'    ' ************************************************************
			'
			'    ' {* USER DEFINED CODE (End) *}
			'
			'    Exit Function
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateContactTypes
	'
	' Description: Populate Contact Types
	'
	' ***************************************************************** '
	Private Function PopulateContactTypes() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear Contact Type List
			cmbContactType.Items.Clear()
			cmbContactType.SelectedIndex = -1
			
			' Check if there is a list
			If Not Information.IsArray(m_vContactTypes) Then
				Return result
			End If
			
			' Assign the details to the interface.
			For lRow As Integer = m_vContactTypes.GetLowerBound(1) To m_vContactTypes.GetUpperBound(1)
				
				Dim cmbContactType_NewIndex As Integer = -1
				cmbContactType_NewIndex = cmbContactType.Items.Add(CStr(m_vContactTypes(1, lRow)))
				VB6.SetItemData(cmbContactType, cmbContactType_NewIndex, CInt(m_vContactTypes(0, lRow)))
				
				
				If CStr(m_vContactTypes(2, lRow)).Trim() = "TELEPHONE" Then
					cmbContactType.SelectedIndex = cmbContactType_NewIndex
					cmbContactType.Text = CStr(m_vContactTypes(1, lRow))
				End If
				
				
			Next lRow
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Populate Contact Types", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContactTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
				'        If (m_vLookupValues(ACValueID, lRow&) = _
				''        CLng(m_vLookupDetails(ACDetailKey, lCntr&))) Then
				'            ctlLookup.ListIndex = ctlLookup.NewIndex
				'        End If
				'
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
	
	
	Private Sub cmbContactType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbContactType.Enter
		
		m_lReturn = m_oFormFields.GotFocus(cmbContactType)
		
	End Sub
	
	
	Private Sub cmbContactType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbContactType.Leave
		
		m_lReturn = m_oFormFields.LostFocus(cmbContactType)
		
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
            Dim temp_m_oBusiness As Object = Nothing
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRContact.Business", vInstanceManager:="ClientManager")
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
			m_oGeneral = New iPMBContact.General()
			
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
			
			' Underwriting or broking
			m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, 1, m_vUnderwritingOrBroking)
			
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
			'    m_lReturn& = m_oBusiness.SetStatus( _
			''        sProcessStatus:=m_sProcessStatus$, _
			''        sMapStatus:=m_sMapStatus$, _
			''        sStepStatus:=m_sStepStatus$)
			
			' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        ' Failed to process the interface.
			'        m_lErrorNumber& = PMFalse
			'
			'        ' Log Error Message
			'        LogMessage _
			''            iType:=PMLogOnError, _
			''            sMsg:="Failed to set the status for the business object", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="Form_Load"
			'
			'        Exit Sub
			'    End If
			
			' Set the business keys.
			' {* USER DEFINED CODE (Begin) *}

			m_oBusiness.ContactCnt = m_lContactCnt
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
                    Cancel = 1
                    'developer guide no. 7
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



            ' Error Section.

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
			
			' Check mandatory controls have been entered into.
			m_lReturn = m_oFormFields.CheckMandatoryControls()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If

			' Check for contact number
			If VB6.GetItemData(cmbContactType, cmbContactType.SelectedIndex) = 1 OrElse VB6.GetItemData(cmbContactType, cmbContactType.SelectedIndex) = 2 OrElse VB6.GetItemData(cmbContactType, cmbContactType.SelectedIndex) = 4 Then
				Dim MatchPhoneNumberPattern As String = "^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$"
				If Not Regex.IsMatch(txtNumber.Text, MatchPhoneNumberPattern) Then
					MessageBox.Show("Enter Correct Number", "Incorrect Number Entered", MessageBoxButtons.OK, MessageBoxIcon.Error)
					m_lReturn = gPMConstants.PMEReturnCode.PMFalse
				End If
			End If

			If VB6.GetItemData(cmbContactType, cmbContactType.SelectedIndex) = 3 OrElse VB6.GetItemData(cmbContactType, cmbContactType.SelectedIndex) = 8 Then
				m_lReturn = gPMFunctions.IsValidEmail(txtNumber.Text)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					If (VB6.GetItemData(cmbContactType, cmbContactType.SelectedIndex) = 3) Then
						MessageBox.Show("Please Enter correct Internet Electronic Mail Address", "Incorrect Email Entered", MessageBoxButtons.OK, MessageBoxIcon.Error)
					Else
						MessageBox.Show("Please Enter correct Main Email Contact", "Incorrect Email Entered", MessageBoxButtons.OK, MessageBoxIcon.Error)
					End If
					txtNumber.Focus()
					m_lReturn = gPMConstants.PMEReturnCode.PMFalse
				End If
			End If

			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				'update the contact cnt property

				m_lContactCnt = m_oBusiness.ContactCnt
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
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
			'm_lReturn = m_oGeneral.ProcessCommand()
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
			' Error Section.
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub
	
	Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_0.Click
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
	
	Private Sub txtAreaCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAreaCode.Enter
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAreaCode)
		
	End Sub
	
	
	Private Sub txtAreaCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAreaCode.Leave
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAreaCode)
		
	End Sub
	
	
	' PRIVATE Events (End)
	Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
		
	End Sub
	
	
	Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
		
	End Sub
	
	
	Private Sub txtExtension_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExtension.Enter
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtExtension)
		
	End Sub
	
	Private Sub txtExtension_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExtension.Leave
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtExtension)
		
	End Sub
	
	
	Private Sub txtNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNumber.Enter
		
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtNumber)
		
	End Sub

	Private Sub txtNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNumber.Leave
		
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtNumber)
		
	End Sub
End Class
