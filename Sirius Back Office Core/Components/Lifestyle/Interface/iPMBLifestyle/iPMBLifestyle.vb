Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
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
    Private Const ACClass As String = "frmInterface"
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
	Private m_sStepStatus As String = ""
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_lPartyCnt As Integer
	Private m_lPartyLifestyleID As Integer
	
	Private m_sPartyName As String = ""
	Private m_lCategory As Integer
	Private m_dtDateOfBirth As Date
	Private m_sGenderCode As String = ""
	Private m_sOccupationCode As String = ""
	Private m_sSecondaryOccupationCode As String = ""
	Private m_iIsSmoker As Integer
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iPMBLifestyle.General
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Declare an instance of the Gemini List Manager
	'PN9578 ECK 170304 Replaced with Sirius dropdown control
	'Private m_oGEMListManager As iGEMListManager.Interface
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
	Private Const POLARIS_GENDER_CODE As String = "524308"
	Private Const POLARIS_OCCUPATION As String = "10616834"
	
	
	
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	Public Property PartyCnt() As Integer
		Get
			Return m_lPartyCnt
		End Get
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	Public Property PartyLifestyleID() As Integer
		Get
			Return m_lPartyLifestyleID
		End Get
		Set(ByVal Value As Integer)
			m_lPartyLifestyleID = Value
		End Set
	End Property
	
	Public ReadOnly Property PartyName() As String
		Get
			Return m_sPartyName
		End Get
	End Property
	Public ReadOnly Property Category() As Integer
		Get
			Return m_lCategory
		End Get
	End Property
	Public ReadOnly Property DateOfBirth() As Date
		Get
			Return m_dtDateOfBirth
		End Get
	End Property
	Public ReadOnly Property GenderCode() As String
		Get
			Return m_sGenderCode
		End Get
	End Property
	Public ReadOnly Property OccupationCode() As String
		Get
			Return m_sOccupationCode
		End Get
	End Property
	Public ReadOnly Property SecondaryOccupationCode() As String
		Get
			Return m_sSecondaryOccupationCode
		End Get
	End Property
	Public ReadOnly Property IsSmoker() As Integer
		Get
			Return m_iIsSmoker
		End Get
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
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDOB, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
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
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			

			m_lReturn = m_oBusiness.GetDetails(vLockMode:=gPMConstants.PMELockMode.PMNoLock, vPartyCnt:=PartyCnt, vPartyLifestyleID:=PartyLifestyleID)
			
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
	' Name: GetIndexForText
	'
	' Description: Gets the index in the passed combo box for the text.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetIndexForText) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetIndexForText(ByVal v_cboCombo As ComboBox, ByVal v_sText As String, ByRef r_lIndex As Integer) As Integer
		'
		'Dim result As Integer = 0
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Default to not found
			'r_lIndex = -1
			'
			'For 'iLoop1 As Integer = 0 To v_cboCombo.Items.Count - 1
				'If VB6.GetItemString(v_cboCombo, iLoop1).Trim() = v_sText.Trim() Then
					'r_lIndex = iLoop1
					'Exit For
				'End If
			'Next iLoop1
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetIndexForText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIndexForText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	
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
			
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sPartyName)
			m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDOB, vControlValue:=m_dtDateOfBirth)
			
			' - 1 because we're not showing "Insured"
			cboCategory.SelectedIndex = m_lCategory - 2
			
			'PN9578 ECK 170304 Replaced with Sirius dropdown control
			'    m_lReturn& = GetIndexForText(v_cboCombo:=cboGenderCode, _
			''                    v_sText:=m_sGenderCode$, _
			''                    r_lIndex:=lIndex)
			'    cboGenderCode.ListIndex = lIndex
			'    m_lReturn& = GetIndexForText(v_cboCombo:=cboOccupationCode, _
			''                                 v_sText:=m_sOccupationCode$, _
			''                                 r_lIndex:=lIndex)
			'    cboOccupationCode.ListIndex = lIndex
			'
			'
			'    m_lReturn& = GetIndexForText(v_cboCombo:=cboSecOccCode, _
			''                                 v_sText:=m_sSecondaryOccupationCode, _
			''                                 r_lIndex:=lIndex)
			'    cboSecOccCode.ListIndex = lIndex
			ddOccupation.Text = m_sOccupationCode
			ddSecondaryOccupation.Text = m_sSecondaryOccupationCode
			ddGender.Text = m_sGenderCode
			
			chkSmoker.CheckState = m_iIsSmoker
			
			' {* USER DEFINED CODE (End) *}
			
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
					
					' {* USER DEFINED CODE (Begin) *}

					m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vPartyLifestyleID:=m_lPartyLifestyleID, vName:=m_sPartyName, vCategory:=m_lCategory, vDateOfBirth:=m_dtDateOfBirth, vGenderCode:=m_sGenderCode, vOccupationCode:=m_sOccupationCode, vSecondaryOccupationCode:=m_sSecondaryOccupationCode, vIsSmoker:=m_iIsSmoker)
					
					' {* USER DEFINED CODE (End) *}
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Inform the business object with an updated data item.
					
					' {* USER DEFINED CODE (Begin) *}

					m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vPartyLifestyleID:=m_lPartyLifestyleID, vName:=m_sPartyName, vCategory:=m_lCategory, vDateOfBirth:=m_dtDateOfBirth, vGenderCode:=m_sGenderCode, vOccupationCode:=m_sOccupationCode, vSecondaryOccupationCode:=m_sSecondaryOccupationCode, vIsSmoker:=m_iIsSmoker)
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
			

			m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vPartyLifestyleID:=m_lPartyLifestyleID, vName:=m_sPartyName, vCategory:=m_lCategory, vDateOfBirth:=m_dtDateOfBirth, vGenderCode:=m_sGenderCode, vOccupationCode:=m_sOccupationCode, vSecondaryOccupationCode:=m_sSecondaryOccupationCode, vIsSmoker:=m_iIsSmoker)
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
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
			
			' Name
			m_sPartyName = txtName.Text.Trim()
			
			' Category
			'DJM 14/05/2002 : If no category is selected then default to Undefined Relationship
			If cboCategory.SelectedIndex = -1 Then
				m_lCategory = 11 'Default to Undefined Relationship
			Else
				' + 2 because we're not showing "Insured"
				' and listindex starts at 0
				m_lCategory = cboCategory.SelectedIndex + 2
			End If
			
			' Date of Birth

			m_dtDateOfBirth = CDate(m_oFormFields.UnformatControl(txtDOB))
			
			'PN9578 ECK 170304 Replaced with Sirius dropdown control
			' Gender
			'   m_sGenderCode = cboGenderCode.Text
			'    ' Occupation
			'    m_sOccupationCode = cboOccupationCode.Text
			'
			'    ' 2ndary Occupation
			'    m_sSecondaryOccupationCode = cboSecOccCode.Text
			m_sOccupationCode = ddOccupation.Text
			m_sSecondaryOccupationCode = ddSecondaryOccupation.Text
			m_sGenderCode = ddGender.Text
			'
			' Is Smoker
			m_iIsSmoker = chkSmoker.CheckState
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ************************************************************************** '
	'
	' Fills combo box with polaris list via ComponentManager
	'
	' History: original by SJ
	'          modified heavily for S4B by CF - 7/5/99
	'
	' ************************************************************************** '
	'PN9578 ECK 170304 Replaced with Sirius dropdown control
	'Public Function FillCombo(cboControl As ComboBox, _
	''                          bRefill As Boolean, _
	''                          sPropertyID As String) As Long
	'
	'Dim vListArray As Variant
	'
	'Dim lPropertyID As Long
	'Dim iPropertyType As Integer
	'Dim sTableName As String
	'Dim sFieldName As String
	'
	'Dim sMatchString As String
	'Dim lNumItems As Long
	'Dim lItem As Long
	'
	'Dim lReturn As Long
	'Dim sText As String
	'
	'    On Error GoTo Err_FillCombo
	'
	'    FillCombo& = PMTrue
	'    sMatchString = ""
	'
	'    With cboControl
	'
	'        ' Save text
	'        sText = .Text
	'
	'        If (Len(sText) = 0) Then
	'            m_lReturn& = m_oGEMListManager.PopulateListControl( _
	''                            v_sPropertyId:=sPropertyID, _
	''                            r_oControl:=cboControl)
	'            If (m_lReturn& <> PMTrue) Then
	'                FillCombo = PMFalse
	'                Exit Function
	'            End If
	'            Exit Function
	'        End If
	'
	'        ' If it's not a refill, it only needs filling once
	'        If (bRefill = False) And (.ListCount > 0) Then
	'            ' Return successful
	'            Exit Function
	'        End If
	'
	'        ' If it's a refill, then only return matching items
	'        If (bRefill = True) Then
	'            sMatchString = .Text
	'        End If
	'
	'        ' Get the List from the list manager
	'        If sMatchString <> "" Then
	'
	'            lReturn& = m_oGEMListManager.GetList( _
	''                v_sPropertyId:=sPropertyID, _
	''                r_vListData:=vListArray, _
	''                v_vSearchString:=sMatchString)
	'        Else
	'
	'            lReturn& = m_oGEMListManager.GetList( _
	''                v_sPropertyId:=CStr(lPropertyID), _
	''                r_vListData:=vListArray)
	'
	'        End If
	'
	'        If (lReturn& <> PMTrue) Then
	'            FillCombo& = PMFalse
	'
	'            ' Log Error.
	'            LogMessage _
	''                iType:=PMLogOnError, _
	''                sMsg:="Failed to get list from List Manager", _
	''                vApp:=ACApp, _
	''                vClass:=ACClass, _
	''                vMethod:="FillCombo", _
	''                vErrNo:=Err.Number, _
	''                vErrDesc:=Err.Description
	'
	'            Exit Function
	'        End If
	'
	'        ' Put the list into the Array
	'        lNumItems = UBound(vListArray)
	'        If IsArray(vListArray) = True Then
	'            .Clear
	'            .AddItem " "
	'
	'            For lItem& = 0 To lNumItems&
	'            .AddItem Trim$(vListArray(lItem&))
	'
	'            Next
	'        End If
	'
	'        'sj 15/02/99 - end
	'
	'        ' Restore text
	'        If .style = vbComboDropdown Then
	'            .Text = sText
	'        End If
	'
	'    End With
	'
	'    Exit Function
	'
	'Err_FillCombo:
	'
	'    FillCombo& = PMError
	'
	'    ' Log Error.
	'    LogMessage _
	''        iType:=PMLogOnError, _
	''        sMsg:="FillCombo Failed", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:="FillCombo", _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'
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
			
			' Clear the category list view
			cboCategory.Items.Clear()
			' We dont show Insured
			'cboCategory.AddItem "Insured"
			cboCategory.Items.Add("Spouse")
			cboCategory.Items.Add("First Child")
			cboCategory.Items.Add("Second Child")
			cboCategory.Items.Add("Third Child")
			cboCategory.Items.Add("Fourth Child")
			cboCategory.Items.Add("Fifth Child")
			cboCategory.Items.Add("Sixth Child")
			cboCategory.Items.Add("Other Child")
			cboCategory.Items.Add("Partner")
			cboCategory.Items.Add("Undefined Relationship")
			'PN9578 ECK 170304 Replaced with Sirius dropdown control
			'    m_lReturn& = m_oGEMListManager.PopulateListControl( _
			''                    v_sPropertyID:=POLARIS_GENDER_CODE, _
			''                    r_oControl:=cboGenderCode)
			'    If (m_lReturn& <> PMTrue) Then
			'        SetInterfaceDefaults = PMFalse
			'        Exit Function
			'    End If
			'
			'    m_lReturn& = m_oGEMListManager.PopulateListControl( _
			''                    v_sPropertyID:=POLARIS_OCCUPATION, _
			''                    r_oControl:=cboOccupationCode)
			'    If (m_lReturn& <> PMTrue) Then
			'        SetInterfaceDefaults = PMFalse
			'        Exit Function
			'    End If
			'
			'    cboOccupationCode.ListIndex = -1
			'
			'    m_lReturn& = m_oGEMListManager.PopulateListControl( _
			''                    v_sPropertyID:=POLARIS_OCCUPATION, _
			''                    r_oControl:=cboSecOccCode)
			'    If (m_lReturn& <> PMTrue) Then
			'        SetInterfaceDefaults = PMFalse
			'        Exit Function
			'    End If
			'
			'    cboSecOccCode.ListIndex = -1
			ddOccupation.Enabled = True
			ddSecondaryOccupation.Enabled = True
			
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
			
			m_ctlTabFirstLast(ACControlStart, 0) = txtName
			m_ctlTabFirstLast(ACControlEnd, 0) = chkSmoker
			
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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
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


            lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            lblDOB.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionDOB, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            lblCategory.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCategory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            lblGenderCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionGenderCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            lblOccupationCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionOccCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            lblSecOccCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSecOccCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            lblSmoker.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionSmoker, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

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


            ' Gets all of the lookup values.

            '    ' Check the task.
            '    Select Case (m_iTask)
            '        Case PMAdd
            '            ' Get all of the lookup values.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAll, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMEdit
            '            ' Get all of the lookup values with the correct
            '            ' effective date.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupAllEffective, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '
            '        Case PMView
            '            ' Get lookup values for viewing only.
            '            m_lReturn& = m_oBusiness.GetLookupValues( _
            ''                iLookupType:=PMLookupSingle, _
            ''                vTableArray:=m_vLookupValues, _
            ''                iLanguageID:=g_iLanguageID%, _
            ''                vResultArray:=m_vLookupDetails)
            '    End Select

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        GetLookupValues = PMFalse
            '
            '        ' Log Error.
            '        LogMessagePopup _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to get the lookup values from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetLookupValues"
            '
            '        Exit Function
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

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
    ' PRIVATE Methods (End)
    'PN9578 ECK 170304 Replaced with Sirius dropdown control
    'Private Sub cboOccupationCode_DropDown()
    '
    '    m_lReturn& = FillCombo(cboControl:=cboOccupationCode, _
    ''                           bRefill:=True, _
    ''                           sPropertyID:=POLARIS_OCCUPATION)
    '
    'End Sub
    '
    'Private Sub cboSecOccCode_DropDown()
    '
    '    m_lReturn& = FillCombo(cboControl:=cboSecOccCode, _
    ''                           bRefill:=True, _
    ''                           sPropertyID:=POLARIS_OCCUPATION)
    '
    'End Sub

    Private Sub ddGender_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddGender.GotFocus
        If ddGender.Text = "" Then
            m_lReturn = HighlightContol(ddGender, optBoolDropDown:=True)
        End If

    End Sub

    Private Sub ddGender_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddGender.LostFocus
        m_lReturn = ValidateListField(ddGender)

    End Sub

    Private Sub ddOccupation_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddOccupation.GotFocus

        If ddOccupation.Text = "" Then
            m_lReturn = HighlightContol(ddOccupation, optBoolDropDown:=True)
        End If

    End Sub

    Private Sub ddOccupation_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddOccupation.LostFocus
        m_lReturn = ValidateListField(ddOccupation)

    End Sub

    Private Sub ddSecondaryOccupation_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddSecondaryOccupation.GotFocus
        If ddSecondaryOccupation.Text = "" Then
            m_lReturn = HighlightContol(ddSecondaryOccupation, optBoolDropDown:=True)
        End If

    End Sub

    Private Sub ddSecondaryOccupation_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddSecondaryOccupation.LostFocus
        m_lReturn = ValidateListField(ddSecondaryOccupation)

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
            'developer guide no. 218
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRLifeStyle.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMBLifestyle.General()

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

            'PN9578 ECK 170304 Replaced with Sirius dropdown control

            ' Initialise Gemini List Manager
            '    Set m_oGEMListManager = New iGEMListManager.Interface
            '
            '    m_lReturn& = m_oGEMListManager.Initialise()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '        Exit Sub
            '    End If
            '
            '    m_lReturn& = m_oGEMListManager.CheckListVersions()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '        Exit Sub
            '    End If
            Dim temp_g_oListManager As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oListManager, sClassName:="iGEMListManager.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            PMBGeneralFunc.g_oListManager = temp_g_oListManager

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iGEMListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'm_lReturn = g_oListManager.Initialise()


            m_lReturn = PMBGeneralFunc.g_oListManager.CheckListVersions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get latest list manager files.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            'PN9578 end

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
            'PN9578 ECK 170304 Replaced with Sirius dropdown control
            ' Terminate the Gemini list manager

            '    m_lReturn& = m_oGEMListManager.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '    End If
            '
            '    ' Destroy the instance of the list manager
            '    Set m_oGEMListManager = Nothing
            If Not (PMBGeneralFunc.g_oListManager Is Nothing) Then


                PMBGeneralFunc.g_oListManager.Dispose()
                ' Destroy the instance of the object manager
                ' from memory.
                PMBGeneralFunc.g_oListManager = Nothing

            End If

            'PN9578End

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
		Catch 
			
			Exit Sub
		End Try
		
		
	End Sub
	
	Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged
		
		'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
	Private isInitializingComponent As Boolean
	Private Sub txtDOB_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDOB.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
	End Sub
	
	Private Sub txtDOB_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDOB.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDOB)
	End Sub
	
	Private Sub txtDOB_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDOB.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDOB)
	End Sub
	
	Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtName)
	End Sub
	
	Private Sub txtName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtName)
	End Sub
End Class
