Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
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
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	Private m_oObjectManager As bObjectManager.ObjectManager
	
	' Declare an instance of the data object.

	Private m_oBusiness As bSIRPerilTypeUsage.Business
	
	'Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	
	Private m_lRatingSectionType As Integer
	
	Private m_dtEffectiveDate As Date
	Private m_lEarningPatternId As Integer
	
	Private m_lReturn As Integer
	Private m_iTask As Integer
	Private m_sUniqueId As String
	Private m_sScreenHierarchy As String

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

	' {* USER DEFINED CODE (Begin) *}
	Public WriteOnly Property RatingSectionTypeId() As Integer
		Set(ByVal Value As Integer)
			m_lRatingSectionType = Value
		End Set
	End Property

	Public WriteOnly Property UniqueId() As String
		Set(ByVal Value As String)
			m_sUniqueId = Value
		End Set
	End Property

	Public WriteOnly Property ScreenHierarchy() As String
		Set(ByVal Value As String)
			m_sScreenHierarchy = Value
		End Set
	End Property

	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)



	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Description: Display all language specific captions.
	'
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "DisplayCaptions"
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Display all language specific captions.
		
		'This is how it should be done.
		'Don't forget to update it with all the controls on this control

        'developer guide no. 243
        fraEarningPatternDetails.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEarningPatternDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		

        'developer guide no. 243
        lblEarningPattern.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEarningPattern, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		

        'developer guide no. 243
        lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		
		

		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		' Do any tidy up, e.g. Set x = Nothing here
		

		
		' This is for debugging only

		

		End Try
		Return result
	End Function
	
	Private Sub cboEarningPattern_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboEarningPattern.Click
		
		txtEffectiveDate.Enabled = Not (cboEarningPattern.ItemCode = "DAILY")
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
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
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		If Validate_Renamed() = gPMConstants.PMEReturnCode.PMFalse Then
			Exit Sub
		End If
		
		m_lReturn = InterfaceToBusiness()
		
		' Check the return value.
		If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
			' Everything OK, so we can hide the interface.
			Me.Hide()
		End If
		
		Exit Sub
		
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
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPerilTypeUsage.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.
				'        sTitle$ = iPMFunc.GetResData( _
				''            iLangID:=g_iLanguageID%, _
				''            lId:=ACBusinessFailTitle, _
				''            iDataType:=PMResString)
				'
				'        sMessage$ = iPMFunc.GetResData( _
				''            iLangID:=g_iLanguageID%, _
				''            lId:=ACBusinessFail, _
				''            iDataType:=PMResString)
				
				' Display message.
				'        MsgBox sMessage$, vbCritical, sTitle$
				
				Exit Sub
			End If
			
			' Create an instance of the general interface object.
			'    Set m_oGeneral = New iPMUIndexLinkingDetail.General
			'
			'    ' Call the initialise method passing this interface
			'    ' and the business object as parameters.
			'    m_lReturn& = m_oGeneral.Initialise( _
			''        frmInterface:=Me, _
			''        oBusiness:=m_oBusiness)
			'
			'    ' Check for errors.
			'    If (m_lReturn& <> PMTrue) Then
			'        m_lErrorNumber& = PMFalse
			'        Exit Sub
			'    End If
			
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

            Me.cboEarningPattern.FirstItem = ""
            Me.cboEarningPattern.DefaultItemId = 1
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
			
			' Gets the interface details to be displayed.
			m_lReturn = GetInterfaceDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the interface details.
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
			
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub


			
		End Try
		
	End Sub
	
	
	Public Function Validate_Renamed() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Validate"
		Dim vbResult As DialogResult
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If cboEarningPattern.ItemId = ACEPFully Then
			
			vbResult = MessageBox.Show("Warning - once applied these changes cannot be undone - Do you wish to continue?", "Earning Pattern", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
			
			If vbResult = System.Windows.Forms.DialogResult.No Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
		End If
		
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		' Do any tidy up, e.g. Set x = Nothing here
		
'		Return result
		
		' This is for debugging only
'		Resume 
		
'		Return result
		End Try
		Return result
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
			
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				cmdOk.Enabled = False
				cboEarningPattern.Enabled = False
                cboEarningPattern.BackColor = SystemColors.Control
				txtEffectiveDate.Enabled = False
				
			Else
				
				If m_lEarningPatternId = 2 Then
					cmdOk.Enabled = False
					cboEarningPattern.Enabled = False
                    cboEarningPattern.BackColor = SystemColors.Control
					txtEffectiveDate.Enabled = False
					
				ElseIf m_lEarningPatternId = 1 Then 
					cmdOk.Enabled = True
					cboEarningPattern.Enabled = True
                    cboEarningPattern.BackColor = Color.White
					txtEffectiveDate.Enabled = False
				Else
					cmdOk.Enabled = True
					cboEarningPattern.Enabled = True
                    cboEarningPattern.BackColor = Color.White
					txtEffectiveDate.Value = DateTime.Today
					txtEffectiveDate.Enabled = Not (cboEarningPattern.ItemCode = "DAILY")
				End If
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetInterfaceDetails
	'
	' Description: Gets the interface details and sets the appropriate
	'              sytle.
	'
	' ***************************************************************** '
	Public Function GetInterfaceDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check the task.
			If m_iTask = gPMConstants.PMEComponentAction.PMEdit Or m_iTask = gPMConstants.PMEComponentAction.PMView Then
				' Get the interface details from the
				' business object.
				m_lReturn = GetBusiness()
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to get the details.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				' Assign the details from the business object
				' to the interface.
				
				m_lReturn = BusinessToInterface()
				
				' Check for errors
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Failed to assign the details.
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			
			' Check the task.
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				' Disable the interface to only allow viewing.
				m_lReturn = DisableForm(lDisabled:=True)
				
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
			cboEarningPattern.Enabled = Not lDisabled
			txtEffectiveDate.Enabled = Not lDisabled
			cmdOk.Enabled = Not lDisabled
			'cmdCancel.Enabled = Not lDisabled&
			
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
			

			m_lReturn = m_oBusiness.GetEarningPattern(v_lRatingSectionTypeId:=m_lRatingSectionType, r_lEarningPatternId:=m_lEarningPatternId, r_dtEffectiveDate:=m_dtEffectiveDate)
			
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

			If Not (Convert.IsDBNull(m_lEarningPatternId) Or IsNothing(m_lEarningPatternId)) Then
				cboEarningPattern.ItemId = m_lEarningPatternId
				txtEffectiveDate.Value = gPMFunctions.ToSafeDate(m_dtEffectiveDate)
			Else
				cboEarningPattern.ItemId = ACEPDaily
				txtEffectiveDate.Value = DateTime.Today
			End If
			
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
		Dim lEarningPatternId As Integer
		Dim dEffectiveDate As Date

		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lEarningPatternId = gPMFunctions.ToSafeLong(cboEarningPattern.ItemId)
			dEffectiveDate = gPMFunctions.ToSafeDate(txtEffectiveDate.Value)



			m_lReturn = m_oBusiness.UpdateEarningPattern(v_lRatingSectionTypeId:=m_lRatingSectionType, v_lEarningPatternId:=lEarningPatternId, v_dEffectiveDate:=dEffectiveDate, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

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
End Class
