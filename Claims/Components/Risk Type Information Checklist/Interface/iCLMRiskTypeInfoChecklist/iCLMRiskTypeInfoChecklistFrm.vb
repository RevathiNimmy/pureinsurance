Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Sudhanshu Behera on 6/2/2010 7:38:52 PM refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	' Date: 10/07/2000
	' Description: Main interface.
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	Private sTitle As String = ""
	Private sMessage As String = ""
	
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	'Private m_sStepStatus As String
	Private m_lErrorNumber As Integer
	
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_iRskTypeChkLstVal As CheckState
	
	' Declare an instance of the general interface object.
	Private m_oGeneral As iCLMRiskTypeInfoChecklist.General
	
	' Declare an instance of the Business object.
	'Private m_oBusiness As bCLMRTInfoChkLst.Business
	Private m_oBusiness As Object
	
	
	' Lookup detail contants.
	Const ACDetailKey As Integer = 0
	Const ACDetailDesc As Integer = 1
	Const ACDetailCode As Integer = 2
	
	
	
	
	' Declare an instance of the COLLECTION object.
	'Private m_oCollection As bCLMRTInfoChkLst.CLMRiskTypeInfoChecklists
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	' Variables to store the lookup values/details.
	Private m_vLookupValues( ,  ) As Object
	Private m_vLookupDetails( ,  ) As Object
	
	' Stores the return value for the a
	' function call.
	
	' declarations of the variables
	
	Private m_sRisk_Type_Id As Integer
	Private m_sExpert_Service_Id As String = ""
	
	'Constants for Labels
	Private Const LBL_RSKTYP As Integer = 0
	Private Const LBL_DESC As Integer = 1
	'Constants for ListBox
	Private Const LST_LOOKUP_INFOCHKLST As Integer = 0
	Private Const LST_RSKTYP_INFOCHKLST As Integer = 1
	'Constants for MOVE command buttons
	Private Const CMD_ADD As Integer = 0
	Private Const CMD_DEL As Integer = 1
	Private Const CMD_ADDALL As Integer = 2
	Private Const CMD_DELALL As Integer = 3
	'Constants for Regular command buttons
	Private Const CMD_APPLY As Integer = 0
	Private Const CMD_OK As Integer = 1
	Private Const CMD_CANCEL As Integer = 2
	Private Const CMD_HELP As Integer = 3
	
	''''Dim m_oBusiness As bCLMRTInfoChkLst.Business
	Dim m_lReturn As gPMConstants.PMEReturnCode
	'result arrray for Risk Type Combo Box
	Dim m_vRiskTypeArray( ,  ) As Object
	'result arrray for Risk Type Expert Service List Box
	Dim m_vRiskTypeESArray( ,  ) As Object
	'result arrray for Expert Service List Box
	Dim m_vExpSerArray( ,  ) As Object
	
	
	'Holds the CURRENT row count of the Number of rows in the collection
	Private m_lRowCnt As Integer
	Private m_vRisk_type_id As Integer
	
	
	
	' Control array to store the first and last
	' text box controls for each tab.
	Private m_ctlTabFirstLast( ,  ) As Control
	
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
	
	'##ModelId=3963AA3902C8

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
	
	'##ModelId=3963AA390306
	'##ModelId=3963AA390304
	Public Property Risk_Type_Id() As Integer
		Get
			Return m_sRisk_Type_Id
		End Get
		Set(ByVal Value As Integer)
			Value = Risk_Type_Id
		End Set
	End Property
	
	'##ModelId=3963AA390318
	'##ModelId=3963AA39030E
	Public Property Expert_Service_Id() As Integer
		Get
			Return CInt(m_sExpert_Service_Id)
		End Get
		Set(ByVal Value As Integer)
			Value = Expert_Service_Id
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
	'##ModelId=3963AA390319
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
			'        m_lReturn = m_oFormFields.AddNewFormField( '                       ctlControl:=<Control Name>, '                       lFieldType:=<PM field type>, '                       lFormat:=<PM format string>, '                       lMandatory:=<PMMandatory or PMNonMandatory)
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
	'##ModelId=3963AA390322
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name:     PrepareForm (Private)
	'
	' Description:  Depending on the mode of operation enable & disable the
	'               controls
	' ***************************************************************** '
	Private Function PrepareForm(ByVal vMode As Integer) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			''    Select Case vMode
			''
			''            Case g_nADDMODE
			
			cmdMove(CMD_ADD).Enabled = True
			cmdMove(CMD_DEL).Enabled = True
			cmdMove(CMD_ADDALL).Enabled = True
			cmdMove(CMD_DELALL).Enabled = True
			cmdButton(CMD_APPLY).Enabled = False
			cmdButton(CMD_OK).Enabled = True
			cmdButton(CMD_CANCEL).Enabled = True
			cmdButton(CMD_HELP).Enabled = True
			
			cboRskType.Enabled = True
			
			''                txtOpenClaim(g_nCLAIM_STATUS).Text = "Provisional Open Claim"
			
			''    End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failure to PrepareForm", vApp:=ACApp, vClass:=ACClass, vMethod:="PrepareForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
	'##ModelId=3963AA39032C
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
			'    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
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
	'##ModelId=3963AA39032D
	Public Function InterfaceToBusiness() As Integer
		
		Dim result As Integer = 0
		Dim lBusinessDataID As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Update the business object.
			
			' Assign the details from the interface to the data storage.
			'm_lReturn& = InterfaceToData()
			
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
					'            m_lReturn& = m_oBusiness.EditAdd(lRow:=lBusinessDataID&,)
					' {* USER DEFINED CODE (End) *}
					
				Case gPMConstants.PMEComponentAction.PMEdit
					' Inform the business object with an updated data item.
					
					' {* USER DEFINED CODE (Begin) *}
					'            m_lReturn& = m_oBusiness.EditUpdate(lRow:=lBusinessDataID&, )
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
	
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: BusinessToData
	'
	' Description: Updates the data storage from the business object.
	'
	' ***************************************************************** '
	'##ModelId=3963AA390340
	Private Function BusinessToData() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Assign the details to the data storage.
			
			' {* USER DEFINED CODE (Begin) *}
			
			'm_lReturn& = m_oBusiness.GetNext()
			'm_lReturn& = g_oBusiness.GetNext()
			
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
	'##ModelId=3963AA390341

	'Private Function InterfaceToData() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'
			' Update the data storage.
			'
			' {* USER DEFINED CODE (Begin) *}
			'
			' ************************************************************
			' Enter your code here to assign all of the details from the
			' interface to the data storage.
			''
			' Example:-
			''
			'    m_DName$ = trim$(txtName.Text)
			'    m_DDate = CDate(txtDate.Text)
			'    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
			'    m_lReturn& = m_oFormFields.UnformatControl(txtName)
			''
			' NOTE: Replace this section with your new code.
			' ************************************************************
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
		'End Try
	'End Function
	' ***************************************************************** '
	' Name:     PopulateRiskTypeCbo (Private)
	'
	' Description:  Depending upon the Primary Cause Selected Secondary
	'               causes are filled into the combo
	'
	' ***************************************************************** '
	Private Function PopulateRiskTypeCbo() As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			cboRskType.Items.Clear()
			'get the upper bound of the no. of rows
			For ncount As Integer = 0 To m_vRiskTypeArray.GetUpperBound(1)
				
				''        If m_vRiskTypeArray(1, ncount) = cboRskType.ItemData(cboRskType.ListIndex) Then
				
				Dim cboRskType_NewIndex As Integer = -1
				cboRskType_NewIndex = cboRskType.Items.Add(CStr(m_vRiskTypeArray(2, ncount)))
				'DC140201 changed to use description and not code
				VB6.SetItemData(cboRskType, cboRskType_NewIndex, CInt(m_vRiskTypeArray(0, ncount)))
				
				''        End If
			Next 
			'Default select the first item of the combo
			cboRskType.SelectedIndex = 0
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process PopulateRiskTypeCbo", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateRiskTypeCbo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result


			
			Return result
		End Try
	End Function
	' ***************************************************************** '
	' Name:         PopRiskTypeExpSerLstbx (Private)
	' Description:  This procedure is called when ever the form is loaded
	'               OR the value in Risk Type combo is changed
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	Private Function PopRiskTypeExpSerLstbx() As Integer
		
		Dim result As Integer = 0
		Dim vExp_ser_id As Integer
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Initialize the 'm_lRowCnt' variable to zero,
			'since the collection is empty
			m_lRowCnt = 0
			
			lstInfoChkLst(LST_RSKTYP_INFOCHKLST).Items.Clear()
			'    m_oBusiness.ClearColl
			'get the upper bound of the no. of rows in the array
			For ncount As Integer = 0 To m_vRiskTypeESArray.GetUpperBound(1)
				m_lRowCnt += 1
				'populate the RHS list box + RCnt
				Dim lstInfoChkLst_NewIndex As Integer = -1
				lstInfoChkLst_NewIndex = lstInfoChkLst(LST_RSKTYP_INFOCHKLST).Items.Add(CStr(m_vRiskTypeESArray(2, ncount)) & "                                                                 " & CStr(m_lRowCnt))
				VB6.SetItemData(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), lstInfoChkLst_NewIndex, CInt(m_vRiskTypeESArray(1, ncount)))
				
				m_vRisk_type_id = VB6.GetItemData(cboRskType, cboRskType.SelectedIndex)
				'Assign the ITEMDATA to variable
				vExp_ser_id = VB6.GetItemData(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), lstInfoChkLst_NewIndex)
				
				
				'we pass true cos the status for this record is PMView
				'cos it is already saved to the database
				'        m_lReturn = m_oBusiness.EditAdd(m_lRowCnt, True, True, , vExp_ser_id, m_vRisk_type_id)

				m_lReturn = g_oBusiness.EditAdd(m_lRowCnt, True, True,  , vExp_ser_id, m_vRisk_type_id)
			Next 
			
			'    lstInfoChkLst(LST_RSKTYP_INFOCHKLST).ListIndex = 0
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process PopRiskTypeExpSerLstbx", vApp:=ACApp, vClass:=ACClass, vMethod:="PopRiskTypeExpSerLstbx", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name:         PopExpSerLstbx (Private)
	' Description:
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	Private Function PopExpSerLstbx() As Integer
		
		Dim result As Integer = 0
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lstInfoChkLst(LST_LOOKUP_INFOCHKLST).Items.Clear()
			'get the upper bound of the no. of rows
			For ncount As Integer = 0 To m_vExpSerArray.GetUpperBound(1)
				
				Dim lstInfoChkLst_NewIndex As Integer = -1
				lstInfoChkLst_NewIndex = lstInfoChkLst(LST_LOOKUP_INFOCHKLST).Items.Add(CStr(m_vExpSerArray(1, ncount)))
				VB6.SetItemData(lstInfoChkLst(LST_LOOKUP_INFOCHKLST), lstInfoChkLst_NewIndex, CInt(m_vExpSerArray(0, ncount)))
				
			Next 
			
			'    lstInfoChkLst(LST_LOOKUP_INFOCHKLST).ListIndex = 0
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process PopExpSerLstbx", vApp:=ACApp, vClass:=ACClass, vMethod:="PopExpSerLstbx", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name:         SetInterfaceDefaults
	' Description:  Sets all of the interface default values and prepares
	'               the form for data entry or for View depending on the Mode
	' Date:         07/07/2000
	' Author:       SK
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
			
			''    m_lReturn& = PrepareForm(g_nPMMode)
			m_lReturn = CType(PrepareForm(1), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			''    m_lReturn& = SetFirstLastControls()       See later SK
			
			''    If (m_lReturn& <> PMTrue) Then
			''        SetInterfaceDefaults = PMFalse
			''        Exit Function
			''    End If
			
			' Set any other default values to the interface.
			'######################################################################################
			'-------------------------------------
			'Get an instance of the object manager, which is a global variable
			' & will not be required to be set again in this project
			''Set g_oObjectManager = New bObjectManager.ObjectManager
			
			'Get an instance of the BO's BUSINESS class
			Dim temp_g_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bCLMRTInfoChkLst.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			g_oBusiness = temp_g_oBusiness
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lReturn = gPMConstants.PMEReturnCode.PMFalse
				Return result
			End If
			'*************************************************************

			CType(g_oBusiness, SSP.S4I.Interfaces.IBusiness).Initialise(g_oObjectManager.UserName, g_oObjectManager.Password, g_oObjectManager.UserID, g_oObjectManager.SourceID, g_oObjectManager.LanguageID, g_oObjectManager.CurrencyID, g_oObjectManager.LogLevel, "Salvage")
			
			
			'Public g_oBackOffice As bBackOfficeLink.bBOLink
			'Get an instance of the BO's BUSINESS class
			Dim temp_g_oBackOffice As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_g_oBackOffice, "bBackOfficeLink.bBOLink", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			g_oBackOffice = temp_g_oBackOffice
			
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lReturn = gPMConstants.PMEReturnCode.PMFalse
				Return result
			End If
			'*************************************************************
			
			''commented till the time the Risk_Type is converted to a Lookup table
			
			'Call the BO's method to Selects all the
			'Risk Type records from the Risk_Type Table

			m_lReturn = g_oBusiness.SelectRiskType(m_vRiskTypeArray)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				
				m_lReturn = gPMConstants.PMEReturnCode.PMTrue
				Return result
				
			ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then 
				
				Return gPMConstants.PMEReturnCode.PMFalse
				
			End If
			
			'Populate the Risk Type combo using the above returned records
			m_lReturn = CType(PopulateRiskTypeCbo(), gPMConstants.PMEReturnCode)
			
			
			
			'Default select the first item of the combo
			
			
			''    'Call the BO's method to Selects all the
			''    'Risk Type records from the Risk_Type Table
			''    m_lReturn& = g_oBusiness.SelectRiskType(m_vRiskTypeArray)
			''    If (m_lReturn& = PMNotFound) Then
			''
			''        m_lReturn& = PMTrue
			''        Exit Function
			''
			''    ElseIf (m_lReturn& <> PMTrue) Then
			''        SetInterfaceDefaults = PMFalse
			''        Exit Function
			''    End If
			''
			''    'Populate the Risk Type combo using the above returned records
			''    m_lReturn& = PopulateRiskTypeCbo()
			''
			'    'Used to revert back to the previous combo value
			'    m_iRskTypeCboIndex = cboRskType.ListIndex
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'DC140201 no longer need to display description as selection now using description and not code
			''Populate the Risk Type text box
			'If g_oBackOffice.Sirius_Product = BROKING Then
			'
			'        txtRskType = m_vLookupDetails(1, 0)
			'ElseIf g_oBackOffice.Sirius_Product = UNDERWRITING Then
			'
			'    txtRskType = m_vRiskTypeArray(2, cboRskType.ListIndex)
			'End If
			
			'**************************************************************
			'its ok if we dont call the is routine now cause it is being called again
			'in the RiskType combos change event which is being triggered
			'when the screen loads
			''    m_lReturn& = PopulateScreen()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Set this flag to false once the Risk type has changed
			m_bChecked = False
			
			'    m_lrsk_type_id = cboRskType.ItemData(cboRskType.ListIndex)
			''Call the BO's method to Selects all the
			''Expert Services for the specified Risk Type
			'    m_lReturn& = g_oBusiness.SelRiskTypeExpSer(m_vRiskTypeESArray, _
			''                                                m_lrsk_type_id)
			'    If (m_lReturn& <> PMTrue) Then
			'        SetInterfaceDefaults = PMFalse
			'        Exit Function
			'    End If
			'
			'
			''Populate the Risk Type Expert Service Listbox using the above returned records
			'    m_lReturn& = PopRiskTypeExpSerLstbx()
			'**************************************************************
			'Call the BO's method to Selects all existing
			'Expert Services excluding the ones for the specified risk type
			'**************************************************************
			
			'######################################################################################
			
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
	'##ModelId=3963AA390354

	'Private Function SetFirstLastControls() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Initialise the control array with the number of
			' tabs which contain data entry fields on (Remember
			' that arrays start from zero, therefore you must
			' subtract one from the number of tabs).
			''ReDim m_ctlTabFirstLast(1, 6)
			'
			' Set the first and last data entry controls for
			' all of the tabs.
			'
			' {* USER DEFINED CODE (Begin) *}
			'
			' ************************************************************
			' Enter your code here to set the first and last data entry
			' controls for all of the tabs.
			''
			' Example:-
			''
			'    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
			'    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
			''
			' NOTE: Replace this section with your new code.
			' ************************************************************
			'
			' {* USER DEFINED CODE (End) *}
			'
			'Return result
		'
		'Catch 
			'
			'
			'
			'
			'
			' Log Error.
			'
			'
			'Return gPMConstants.PMEReturnCode.PMError
		'End Try
		'
	'End Function
	
	' ***************************************************************** '
	' Name:         DisplayCaptions
	' Description:  Display all language specific captions.
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Display all language specific captions.
			

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Check for an error.
			If Me.Text = "" Then
				' Failed to get data from the resource file.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
				
				Return result
			End If
			
			'Caption of Label - Risk Type

            SSTab1.SelectedTab.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			
			'Caption of Label - Risk Type


            Label1(LBL_RSKTYP).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'DC140201 description no longer required as now used in dropdown list
			'    'Caption of Label - Description
			'    Label1(LBL_DESC).Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACDescription, _
			''        iDataType:=PMResString)
			
			'Caption of Label - InfoChkList frame

            Frame1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInfoChkList, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Caption of APPLY Button


            cmdButton(CMD_APPLY).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAPPLYButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Caption of OK Button


            cmdButton(CMD_OK).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Caption of Cancel Button


            cmdButton(CMD_CANCEL).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Caption of Help Button


            cmdButton(CMD_HELP).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cboRskType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRskType.SelectedIndexChanged
		Dim Response As DialogResult
		Dim sMsg1, sMsg2, sMsgHeading As String
		m_lReturn = gPMConstants.PMEReturnCode.PMTrue
		
		If Not m_bDataChanged Then
			
			m_lReturn = CType(PopulateScreen(), gPMConstants.PMEReturnCode)
			
			'iRiskId = cboRskType.ItemData(cboRskType.ListIndex)
			If Information.IsArray(m_vRiskTypeArray) Then
				For iCtr As Integer = m_vRiskTypeArray.GetLowerBound(0) To cboRskType.Items.Count - 1
					If m_lrsk_type_id = CDbl(m_vRiskTypeArray(0, iCtr)) Then
						chkInfoCheckList.CheckState = CInt(m_vRiskTypeArray(3, iCtr))
						Exit For
					End If
				Next iCtr
			End If
		Else
			If Not m_bChecked Then

                sMsg1 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSaveChangesMsg1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMsg2 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSaveChangesMsg2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMsgHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSaveChangesTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				Response = MessageBox.Show(sMsg1 & Strings.Chr(13).ToString() & sMsg2, sMsgHeading, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2)
				If Response = System.Windows.Forms.DialogResult.Yes Then
					m_lReturn = CType(PopulateScreen(), gPMConstants.PMEReturnCode)
					
					'disable the Apply button
					cmdButton(CMD_APPLY).Enabled = False
				Else
					'this flag is set to true so that this Msg Box doesn't
					'pop up twice as the CLICK event gets triggered again
					'as soon as we change the combo's listindex property
					m_bChecked = True
					cboRskType.SelectedIndex = m_iRskTypeCboIndex
				End If
			Else
				m_bChecked = False
			End If
		End If
	End Sub
	
	Private Sub chkInfoCheckList_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkInfoCheckList.Enter
		
		cmdButton(CMD_APPLY).Enabled = True
		m_bDataChanged = True
		
	End Sub
	
	Private Sub cmdButton_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdButton_2.Click, _cmdButton_1.Click, _cmdButton_3.Click, _cmdButton_0.Click
		Dim Index As Integer = Array.IndexOf(cmdButton, eventSender)
		
		Try 
			
			
			Select Case Index
				Case CMD_APPLY
					
					m_lReturn = CType(Apply(), gPMConstants.PMEReturnCode)
					
				Case CMD_OK
					
					m_lReturn = CType(OK(), gPMConstants.PMEReturnCode)
					
				Case CMD_CANCEL
					
					m_lReturn = CType(Cancel(), gPMConstants.PMEReturnCode)
					
				Case CMD_HELP
					'do nothing
					
			End Select
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	Private Sub cmdMove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdMove_0.Click, _cmdMove_1.Click, _cmdMove_2.Click, _cmdMove_3.Click
		Dim Index As Integer = Array.IndexOf(cmdMove, eventSender)
		
		Dim vExp_ser_id As Integer
		'Dim m_vRisk_type_id As Variant
		Dim RCnt As String = ""
		
		
		m_lReturn = gPMConstants.PMEReturnCode.PMFalse
		m_vRisk_type_id = VB6.GetItemData(cboRskType, cboRskType.SelectedIndex)
		
		
		Select Case Index
			Case CMD_ADD
				
				If (ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_LOOKUP_INFOCHKLST))) > -1 Then
					m_lRowCnt += 1
					'Add ITEM to the RHS listbox + the Row Count
					Dim lstInfoChkLst_NewIndex As Integer = -1
					lstInfoChkLst_NewIndex = lstInfoChkLst(LST_RSKTYP_INFOCHKLST).Items.Add(VB6.GetItemString(lstInfoChkLst(LST_LOOKUP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_LOOKUP_INFOCHKLST))) & "                                                                 " & CStr(m_lRowCnt))
					'Add ITEMDATA to the RHS listbox
					VB6.SetItemData(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), lstInfoChkLst_NewIndex, VB6.GetItemData(lstInfoChkLst(LST_LOOKUP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_LOOKUP_INFOCHKLST))))
					'Assign the ITEMDATA to variable
					vExp_ser_id = VB6.GetItemData(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), lstInfoChkLst_NewIndex)
					'Remove ITEM from LHS list
					lstInfoChkLst(LST_LOOKUP_INFOCHKLST).Items.RemoveAt(CShort(ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_LOOKUP_INFOCHKLST))))
					

					m_lReturn = g_oBusiness.EditAdd(m_lRowCnt, False, False,  , vExp_ser_id, m_vRisk_type_id)
					If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
						cmdButton(CMD_APPLY).Enabled = True
						m_bDataChanged = True
					End If
					
				End If
				
				
			Case CMD_DEL
                'Modified as per runtime test,insert this line in if condition
                'RCnt = VB6.GetItemString(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))).Substring(VB6.GetItemString(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))).Length - 2).Trim()
				
				'Move the item from LHS lstbx to RHS lstbx
				If (ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))) > -1 Then
                    'Modified,insert this line in if condition
                    RCnt = VB6.GetItemString(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))).Substring(VB6.GetItemString(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))).Length - 2).Trim()
                    Dim lstInfoChkLst_NewIndex2 As Integer = -1
					lstInfoChkLst_NewIndex2 = lstInfoChkLst(LST_LOOKUP_INFOCHKLST).Items.Add(VB6.GetItemString(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))))
					VB6.SetItemData(lstInfoChkLst(LST_LOOKUP_INFOCHKLST), lstInfoChkLst_NewIndex2, VB6.GetItemData(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))))
					lstInfoChkLst(LST_RSKTYP_INFOCHKLST).Items.RemoveAt(CShort(ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))))

					m_lReturn = g_oBusiness.EditDelete(RCnt)
					If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
						cmdButton(CMD_APPLY).Enabled = True
						m_bDataChanged = True
					End If
				End If
				
			Case CMD_ADDALL
				
				For iCount As Integer = 0 To lstInfoChkLst(LST_LOOKUP_INFOCHKLST).Items.Count - 1
                    'Modified by Sudhanshu Behera on 6/3/2010 11:36:34 AM refer developer guide no. 29
                    Dim lstInfoChkLst_NewIndex As Integer
					m_lRowCnt += 1
					'Select the record from the LHS list box that we are moving
					'i.e. set its list index
					ListBoxHelper.SetSelectedIndex(lstInfoChkLst(LST_LOOKUP_INFOCHKLST), iCount)
					'Add ITEM to the RHS listbox
					lstInfoChkLst_NewIndex = lstInfoChkLst(LST_RSKTYP_INFOCHKLST).Items.Add(VB6.GetItemString(lstInfoChkLst(LST_LOOKUP_INFOCHKLST), iCount) & "                                                                 " & CStr(m_lRowCnt))
					'Add ITEMDATA to the RHS listbox
					VB6.SetItemData(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), lstInfoChkLst_NewIndex, VB6.GetItemData(lstInfoChkLst(LST_LOOKUP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_LOOKUP_INFOCHKLST))))
					vExp_ser_id = VB6.GetItemData(lstInfoChkLst(LST_LOOKUP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_LOOKUP_INFOCHKLST)))
					

					m_lReturn = g_oBusiness.EditAdd(m_lRowCnt, False, False,  , vExp_ser_id, m_vRisk_type_id)
				Next iCount
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					cmdButton(CMD_APPLY).Enabled = True
					m_bDataChanged = True
				End If
				
				'Clear the LHS List Box
				lstInfoChkLst(LST_LOOKUP_INFOCHKLST).Items.Clear()
				
				
			Case CMD_DELALL
                'Modified by Sudhanshu Behera on 6/3/2010 11:38:19 AM refer developer guide no. 29
                Dim lstInfoChkLst_NewIndex2 As Integer
				
				For iCount As Integer = 0 To lstInfoChkLst(LST_RSKTYP_INFOCHKLST).Items.Count - 1
					'Select the record from the RHS list box that we are moving
					'i.e. set its list index
					ListBoxHelper.SetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), iCount)
					RCnt = VB6.GetItemString(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))).Substring(VB6.GetItemString(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))).Length - 2).Trim()
					'Add ITEM to the LHS listbox
					lstInfoChkLst_NewIndex2 = lstInfoChkLst(LST_LOOKUP_INFOCHKLST).Items.Add(VB6.GetItemString(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))))
					'Add ITEMDATA to the LHS listbox
					VB6.SetItemData(lstInfoChkLst(LST_LOOKUP_INFOCHKLST), lstInfoChkLst_NewIndex2, VB6.GetItemData(lstInfoChkLst(LST_RSKTYP_INFOCHKLST), ListBoxHelper.GetSelectedIndex(lstInfoChkLst(LST_RSKTYP_INFOCHKLST))))
					'            m_lReturn = m_oBusiness.EditDelete(RCnt)

					m_lReturn = g_oBusiness.EditDelete(RCnt)
				Next iCount
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					cmdButton(CMD_APPLY).Enabled = True
					m_bDataChanged = True
				End If
				
				lstInfoChkLst(LST_RSKTYP_INFOCHKLST).Items.Clear()
				
		End Select
		
	End Sub
	
	'PRIVATE Events (Begin)
	' ***************************************************************** '
	' Name:         FormIntialise
	' Description:  Intialise all required details of the form
	' Date:         11/07/2000
	' Author:       SK
	' ***************************************************************** '
	Private Sub Form_Initialize_Renamed()
		
		Try 
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
			
			' Create an instance of the general interface object.
			m_oGeneral = New iCLMRiskTypeInfoChecklist.General()
			
			' Call the initialise method passing this interface
			' and the business object as parameters.
			m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	' ***************************************************************** '
	' Name:         FormLoad
	' Description:  Intialise all required details of the form
	' Date:         11/07/2000
	' Author:       SK
	' ***************************************************************** '

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

		Try 
			
			' Check if we have had an error so far.
			' Possibly creating the business object.
			If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
				' We have already encountered an error
				Exit Sub
			End If
			'When the Mode is Add
			'----------------------
			'----------------------
			'#  m_lReturn& = SetFieldValidation
			
			''''    m_lReturn = PrepareForm(CInt(g_nPMMode))    SK
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			'Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			
			'Set the Object Manager
			g_oObjectManager = New bObjectManager.ObjectManager()
			
			'm_lReturn& = ObjMgr.Initialise(sCallingAppName:="bCLMRTInfoChkLst")
			m_lReturn = g_oObjectManager.Initialise(sCallingAppName:="bCLMRTInfoChkLst")
			
			
			'    'Set the Business object
			'    Set m_oBusiness = New bCLMRTInfoChkLst.Business
			'
			'    m_oBusiness.Initialise g_oObjectManager.UserName, g_oObjectManager.Password, g_oObjectManager.UserID, g_oObjectManager.SourceID, g_oObjectManager.LanguageID, g_oObjectManager.CurrencyID, g_oObjectManager.LogLevel, "Salvage"
			
			'Set the interface default values.
			m_lReturn = SetInterfaceDefaults()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			
			
			' Gets the interface details to be displayed.
			m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Set the mouse pointer to normal.
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
				Exit Sub
			End If
			
			''SK populate the combo, text box & the 2 list boxes
			
			''                m_lReturn = BusinessToInterface(g_sClaimNo)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				Exit Sub
			End If
			
			''        Case g_nREADMODE
			''
			''                'g_sClaimNo = "P111/005"         'Sample Value for testing
			''
			''                m_lReturn = BusinessToInterface(g_sClaimNo)
			''
			''                If (m_lReturn& <> PMTrue) Then
			''                    m_lErrorNumber& = PMFalse
			''                    Exit Sub
			''                End If
			
			'Call BusinessToData(g_sClaimNo)
			'Call DisplayPolicyDetails
			
			''    End Select
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			
		End Try
		
	End Sub
	
    Private Const vbFormCode As Integer = 0
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Modified by Sudhanshu Behera on 6/3/2010 10:48:28 AM refer developer guide no. 19 (no solution)
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                'm_lReturn& = m_oGeneral.ProcessCommand()
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
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
            ' m_lReturn& = m_oBusiness.Terminate()
            'm_lReturn& = g_oBusiness.Terminate()

            ' Check for errors.
            

            ' Destroy the instance of the business object
            ' from memory.
            'Set m_oBusiness = Nothing
            'Set g_oBusiness = Nothing

            '    ' Terminate the form control object.
            '    m_lReturn& = m_oFormFields.Terminate()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

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
		
		'Dim iCtrlDown  As Integer
		'
		'Const ACCtrlMask = 2
		'
		'    On Error GoTo Err_FormKeyDown
		'
		'    ' Set the control key value.
		'    iCtrlDown = (Shift And ACCtrlMask) > 0
		'
		'    With tabMainTab
		'        ' Check the key pressed.
		'        Select Case KeyCode
		'            Case vbKeyPageUp
		'                ' Page Up key has been pressed.
		'
		'                ' Check if the control key has also
		'                ' been pressed.
		'                If (iCtrlDown) Then
		'                    ' Display the first tab.
		'                    .Tab = 0
		'                Else
		'                    ' Check we are not on the
		'                    ' first tab.
		'                    If (.Tab > 0) Then
		'                        ' Display the previous tab.
		'                        .Tab = .Tab - 1
		'                    End If
		'                End If
		'
		'            Case vbKeyPageDown
		'                ' Page Down key has been pressed.
		'
		'                ' Check if the control key has also
		'                ' been pressed.
		'                If (iCtrlDown) Then
		'                    ' Display the last tab.
		'                    .Tab = .Tabs - 1
		'                Else
		'                    ' Check we are not on the
		'                    ' last tab.
		'                    If (.Tab < (.Tabs - 1)) Then
		'                        ' Display the next tab.
		'                        .Tab = .Tab + 1
		'                    End If
		'                End If
		'
		'            Case vbKeyHome
		'                ' Home key has been pressed.
		'
		'                ' Check if the control key has also
		'                ' been pressed.
		'                If (iCtrlDown) Then
		'                    ' Set focus the the start control on
		'                    ' the tab.
		'                    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
		'                         m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
		'                    End If
		'                End If
		'
		'            Case vbKeyEnd
		'                ' End key has been pressed.
		'
		'                ' Check if the control key has also
		'                ' been pressed.
		'                If (iCtrlDown) Then
		'                    ' Set focus the the start control on
		'                    ' the tab.
		'                    If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
		'                         m_ctlTabFirstLast(ACControlEnd, .Tab).SetFocus
		'                    End If
		'                End If
		'        End Select
		'    End With
		'
		'    Exit Sub
		
		
		
		
		Exit Sub
		
	End Sub
	
	'Private Sub tabMainTab_Click(PreviousTab As Integer)
	'
	'    On Error GoTo Err_tabMainTabClick
	'
	'    With tabMainTab
	'        ' Set the default button.
	'        If (.Tab < cmdNext.Count) Then
	'            cmdNext(.Tab).Default = True
	'        Else
	'            cmdOK.Default = True
	'        End If
	'
	'        ' Now I know this is crap, this goes against
	'        ' all my principles, but for some reason when
	'        ' using the mouse to select a tab the setfocus
	'        ' code below doesn't work. The cursor sticks,
	'        ' and you can't tab off. Therefore I've used
	'        ' this to get around the problem.
	'        DoEvents
	'
	'        ' Set focus to the first control on the tab.
	'        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
	'            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
	'        End If
	'    End With
	'
	'    Exit Sub
	'
	'
	'Err_tabMainTabClick:
	'
	'    Exit Sub
	'
	'End Sub
	
	'##ModelId=3963AA39037F

	'Private Function GetDetails() As Integer

		'Try 
			' calls a stored proceudre in the business object that fetches the
			' records for the list view
		'
		'Catch 
			'
			' paste the log message error
		'End Try
		'
	'End Function
	
	' ***************************************************************** '
	' Name:         PopulateScreen (Public)
	' Description:  used to populate the Desc text box, & both the
	'               list boxes
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	Private Function PopulateScreen() As Integer
		
		Dim result As Integer = 0
		Try 
			
			'initialize the row count
			m_lRowCnt = 0
			m_bDataChanged = False
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			''Populate the Risk Type text box
			'    txtRskType = m_vRiskTypeArray(2, cboRskType.ListIndex)
			
			'Clear both the list boxes before populating them
			lstInfoChkLst(LST_LOOKUP_INFOCHKLST).Items.Clear()
			lstInfoChkLst(LST_RSKTYP_INFOCHKLST).Items.Clear()
			
			'get the Risk Type Id for the item specified in the combo
			m_lrsk_type_id = VB6.GetItemData(cboRskType, cboRskType.SelectedIndex)
			'Call the BO's method to Selects all the
			'Expert Services for the specified Risk Type in combo

			m_lReturn = g_oBusiness.SelRiskTypeExpSer(m_vRiskTypeESArray, m_lrsk_type_id)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				
			ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then 
				Return result
			End If
			
			'Used to revert back to the previous combo value
			m_iRskTypeCboIndex = cboRskType.SelectedIndex
			
			
			'Clear the collection whenever fresh values are
			'are comming into the list boxes ie. when the combo
			'values are changed

			g_oBusiness.ClearColl()
			
			'Check if any records are returned
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				'Only if records are returned
				'Populate the Risk Type Expert Service Listbox using
				'the above returned records
				m_lReturn = CType(PopRiskTypeExpSerLstbx(), gPMConstants.PMEReturnCode)
				'        PopulateScreen = PMFalse
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				'        Exit Function
			End If
			
			
			
			
			' Constants in m_vRiskTypeArray array
            'Const RskTypeESId_Field As Integer = 0
            'Const ESId_Field As Integer = 1
            'Const Desc_Field As Integer = 2
			
			'Populate the Description text box
			'DC140201 do not need to display description as description now used for selection
			'If g_oBackOffice.Sirius_Product = BROKING Then
			'
			'    txtRskType = m_vLookupDetails(ACDetailDesc, cboRskType.ListIndex)
			'
			'ElseIf g_oBackOffice.Sirius_Product = UNDERWRITING Then
			'
			'    txtRskType = m_vRiskTypeArray(Desc_Field, cboRskType.ListIndex)
			'
			'End If
			'*************************************************************
			'Call the BO's method to Selects all the
			'Expert Services EXCLUDING the ones in Risk Type Expert Services List Box

			m_lReturn = g_oBusiness.SelExpSer(m_vExpSerArray, m_lrsk_type_id)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				Return result
				
				
			ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then 
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			
			'Populate the Expert Service Listbox using
			'the above returned records
			m_lReturn = CType(PopExpSerLstbx(), gPMConstants.PMEReturnCode)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Populate Screen", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	' Name:         Apply (Private)
	' Description:  Procedure called when the Apply button is clicked
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	Private Function Apply() As Integer
		
		Try 
			
			'm_lReturn = m_oBusiness.Update

			m_lReturn = g_oBusiness.Update
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				cmdButton(CMD_APPLY).Enabled = False
				m_bDataChanged = False
				m_bChecked = False
			End If
			
			m_iRskTypeChkLstVal = chkInfoCheckList.CheckState

			m_lReturn = g_oBusiness.UpdateRiskTypeInfoChkLst(m_lrsk_type_id, m_iRskTypeChkLstVal)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			'''''''''''''''
			'Call the BO's method to Selects all the
			'Expert Services for the specified Risk Type in combo

			m_lReturn = g_oBusiness.SelRiskTypeExpSer(m_vRiskTypeESArray, m_lrsk_type_id)
			
			
			'Clear the collection whenever fresh values are
			'are comming into the list boxes ie. when the combo
			'values are changed

			g_oBusiness.ClearColl()
			
			'    'once the collection is cleared, set the 'm_lRowCnt' variable to zero
			'    m_lRowCnt = 0
			
			'Check if any records are returned
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				'Only if records are returned
				'Populate the Risk Type Expert Service Listbox using
				'the above returned records
				m_lReturn = CType(PopRiskTypeExpSerLstbx(), gPMConstants.PMEReturnCode)
				'        PopulateScreen = PMFalse
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				'        Exit Function
			End If
			
			'After applying the changes to the database it is
			'very essential to set 'm_lRowCnt' variable to the
			'actual count of the items in the collection

			m_lRowCnt = g_oBusiness.CollCount
			
			
			
			Return m_lReturn
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
	End Function
	' ***************************************************************** '
	' Name:         OK (Private)
	' Description:  Procedure called when the OK button is clicked
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	Private Function OK() As Integer
		'    Dim sMsg As String
		'    Dim sMsgHeading As String
		'    Dim Response As Variant
		
		Try 
			
			'prompt only if form is dirty
			If m_bDataChanged Then
				
				''    sMsg = iPMFunc.GetResData(iLangID:=g_iLanguageID%, _
				'''                    lID:=ACExitSaveChangesMsg, _
				'''                    iDataType:=PMResString)
				''    sMsgHeading = iPMFunc.GetResData(iLangID:=g_iLanguageID%, _
				'''                    lID:=ACSaveChangesTitle, _
				'''                    iDataType:=PMResString)
				''
				''    Response = MsgBox(sMsg, vbYesNo + vbDefaultButton1, sMsgHeading)
				''    If Response = vbYes Then
				m_lReturn = Apply()
				Me.Close()
				''    Else
				''        Unload Me
				''    End If
			Else
				
				m_lStatus = gPMConstants.PMEReturnCode.PMOK
				
				Me.Close()
			End If
			
			
			Return m_lReturn
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
	End Function
	' ***************************************************************** '
	' Name:         Cancel (Private)
	' Description:  Procedure called when the Cancel button is clicked
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	Private Function Cancel() As Integer
		
		Dim sMsg1, sMsg2, sMsgHeading As String
		Dim Response As DialogResult
		
		
		Try 
			'prompt only if form is dirty
			If m_bDataChanged Then
				

                sMsg1 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExitSaveChangesMsg1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMsg2 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACExitSaveChangesMsg2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMsgHeading = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSaveChangesTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
				
				Response = MessageBox.Show(sMsg1 & Strings.Chr(13).ToString() & sMsg2, sMsgHeading, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
				If Response = System.Windows.Forms.DialogResult.Yes Then
					Me.Close()
					'    Else
					'        Unload Me
				End If
			Else
				
				m_lStatus = gPMConstants.PMEReturnCode.PMCancel
				
				Me.Close()
			End If
			
			'    Unload Me
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			
		End Try
	End Function
	
	'***************************************************************** '
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
			
			m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Get all of the lookup details.
			
			m_lReturn = CType(GetLookupDetails(sLookupTable:="Risk_Type", ctlLookup:=cboRskType), gPMConstants.PMEReturnCode)
			
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			
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
			' Get all of the lookup values.

			m_lReturn = g_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
			
			
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
    'Developer Guide no. 153
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        '' Lookup detail contants.
        'Const ACDetailKey = 0
        'Const ACDetailDesc = 1


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
                'DC140201 use description rather than code

                'Developer Guide no. 153
                'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


                'Developer Guide no. 153
                'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
                Dim NewIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(Trim(m_vLookupDetails(ACDetailDesc, lCntr)), CInt(m_vLookupDetails(ACDetailKey, lCntr))))
                'SP150998 - compare long value not string
                ' Check if this is the selected index.
                '        If (m_vLookupValues(ACValueID, lRow&) = _
                ''        CLng(m_vLookupDetails(ACDetailKey, lCntr&))) Then
                '            ctlLookup.ListIndex = ctlLookup.NewIndex
                '        End If

                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'Developer Guide no. 153
                        'ctlLookup.ListIndex = ctlLookup.NewIndex
                        ctlLookup.SelectedIndex = NewIndex
                    End If
                End If

            Next lCntr

            '    ' Check if the selected index is blank. If so,
            '    ' we set the controls index to zero.
            '    If (m_vLookupValues(ACValueID, lRow&) = "") Then
            '        ctlLookup.ListIndex = 0
            '    End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
