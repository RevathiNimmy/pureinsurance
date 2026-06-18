Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date:
	'
	' Description: Main interface form.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmInterface"
	
	'********************************
	'Process Mode Variables
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
	Private m_lProcessMode As gPMConstants.PMEProcessMode
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	'********************************
	
	'********************************
	' General Property variables
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lPMAuthorityLevel As Integer
	Private m_bError As Integer

	Private m_oBusiness As bPMEventTask.Business
	Private m_lReturn As Integer
	Private m_bInterfaceError As Boolean
	'********************************
	
	'*****************************************************************
	' retained for consistency with existing interface in           '*
	' pmwrkmanager                                                  '*
	Private m_lPMWrkTaskInstanceCnt As Integer
	Private m_sTaskCustomer As String = ""
	Private m_sTaskDescription As String = ""
	Private m_dtTaskDueDate As Date
	Private m_iTaskIsUrgent As Integer
	Private m_iTaskStatus As Integer
	Private m_lAllocateToUserGroupId As Integer
	Private m_lAllocateToUserId As Integer
	Private m_lPMNavProcessID As Integer
	Private m_sComponentObjectName As String = ""
	Private m_sComponentClassName As String = ""
	Private m_lDisplayIcon As Integer
	Private m_iIsViewOnlyTask As Integer
	Private m_sLinkedObjectName As String = ""
	Private m_sLinkedClassName As String = ""
	Private m_sLinkedCaption As String = ""
	Private m_sWorkflowInformation As String = ""
	' end of retained for consistency with existing interface in    '*
	' pmwrkmanager                                                  '*
	'*****************************************************************
	
	Private m_vLookupDetails( ,  ) As Object
	Private m_vLookupTables( ,  ) As Object
	Private m_vPMUsers As Object
	Private m_vTaskGroupTask( ,  ) As Object
	Private m_vTaskGroupTaskAction( ,  ) As Object
	Private m_sPrevTaskGroup As String = ""
	Private m_sPrevTask As String = ""
	Private m_vTaskActionDueDays() As Object
	Private m_sPrevTaskAction As String = ""
	Private m_vPMUserGroupUsers( ,  ) As Object
	Private m_sPrevUserGroup As String = ""
	Private m_vTaskGroupUserGroups( ,  ) As Object
	Private m_vTaskActionTypeOutcomes( ,  ) As Object
	Private m_vTaskEvent( ,  ) As Object
	Private m_bUseWorkTables As Boolean
	
	'******************
	' Task Info
	Private m_lTaskGroupId As Integer
	Private m_lTaskId As Integer
	Private m_lTaskActionTypeId As Integer
	Private m_lTaskAllocateToUserGroupId As Integer
	Private m_lTaskAllocateToUserId As Integer
	Private m_sTaskWorkflowInformation As String = ""
	Private m_dtTaskOutcomeDate As Date
	Private m_lTaskOutcomeId As Integer
	'******************
	
	'******************
	' Event Info
	Private m_lEventCnt As Integer
	Private m_lPartyCnt As Integer
	Private m_lEventType As Integer
	Private m_sEventDescription As String = ""
	Private m_lEventlogsubjectId As Integer
	
	Private m_dtEventDate As Date
	Private m_iEventUserId As Integer
	
	Private m_lInsuranceFolderCnt As Integer
	Private m_lInsuranceFileCnt As Integer
	Private m_lClaimCnt As Integer
	Private m_lAccountKey As Integer
	Private m_lDocumentCnt As Integer
	Private m_lOldAddressCnt As Integer
	Private m_lNewAddressCnt As Integer
	Private m_lCampaignId As Integer
	Private m_lDocumentTypeId As Integer
	Private m_lReportTypeId As Integer
	Private m_lOldPartyTypeId As Integer
	Private m_lDocumentTemplateId As Integer
	Private m_sOutputMediaCode As String = ""
	'******************
	
	Public WriteOnly Property PartyCnt() As Integer
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property InsuranceFolderCnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFolderCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property InsuranceFIlecnt() As Integer
		Set(ByVal Value As Integer)
			m_lInsuranceFileCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property ClaimId() As Integer
		Set(ByVal Value As Integer)
			m_lClaimCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property AccountKey() As Integer
		Set(ByVal Value As Integer)
			m_lAccountKey = Value
		End Set
	End Property
	
	Public WriteOnly Property UseWorkTables() As Boolean
		Set(ByVal Value As Boolean)
			m_bUseWorkTables = Value
		End Set
	End Property
	
	'********************************
	' General Interface Properties
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			m_sCallingAppName = Value
		End Set
	End Property
	Public WriteOnly Property PMAuthoritylevel() As Integer
		Set(ByVal Value As Integer)
			m_lPMAuthorityLevel = Value
		End Set
	End Property
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	Public ReadOnly Property Error_Renamed() As Integer
		Get
			Return m_bError
		End Get
	End Property
	'********************************
	
	'********************************
	' Process Mode Variables
	Public WriteOnly Property Task() As Integer
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
	'********************************
	'*****************************************************************
	' retained for consistency with existing interface in           '*
	' pmwrkmanager                                                  '* '*
	Public Property PMWrkTaskInstanceCnt() As Integer
		Get '*
			Return m_lPMWrkTaskInstanceCnt '*
		End Get
		Set(ByVal Value As Integer)
			m_lPMWrkTaskInstanceCnt = Value '*
		End Set
	End Property '*
	Public ReadOnly Property Customer() As String
		Get '*
			Return m_sTaskCustomer '*
		End Get
	End Property '*
	Public ReadOnly Property description() As String
		Get '*
			Return m_sTaskDescription '*
		End Get
	End Property '*
	Public ReadOnly Property DueDate() As Date
		Get '*
			Return m_dtTaskDueDate '*
		End Get
	End Property '*
	Public ReadOnly Property IsUrgent() As Integer
		Get '*
			Return m_iTaskIsUrgent '*
		End Get
	End Property '*
	Public ReadOnly Property TaskStatus() As Integer
		Get '*
			Return m_iTaskStatus '*
		End Get
	End Property '*
	Public ReadOnly Property PMUserGroupId() As Integer
		Get '*
			Return m_lAllocateToUserGroupId '*
		End Get
	End Property '*
	Public ReadOnly Property PMUserId() As Integer
		Get '*
			Return m_lAllocateToUserId '*
		End Get
	End Property '*
	Public ReadOnly Property PMNavProcessId() As Integer
		Get '*
			Return m_lPMNavProcessID '*
		End Get
	End Property '*
	Public ReadOnly Property ComponentObjectName() As String
		Get '*
			Return m_sComponentObjectName '*
		End Get
	End Property '*
	Public ReadOnly Property ComponentClassName() As String
		Get '*
			Return m_sComponentClassName '*
		End Get
	End Property '*
	Public ReadOnly Property DisplayIcon() As Integer
		Get '*
			Return m_lDisplayIcon '*
		End Get
	End Property '*
	Public ReadOnly Property IsViewOnlyTask() As Integer
		Get '*
			Return m_iIsViewOnlyTask '*
		End Get
	End Property '*
	Public ReadOnly Property LinkedObjectName() As String
		Get '*
			Return m_sLinkedObjectName '*
		End Get
	End Property '*
	Public ReadOnly Property LinkedClassName() As String
		Get '*
			Return m_sLinkedClassName '*
		End Get
	End Property '*
	Public ReadOnly Property LinkedCaption() As String
		Get '*
			Return m_sLinkedCaption '*
		End Get
	End Property '*
	Public ReadOnly Property WorkflowInformation() As String
		Get '*
			Return m_sWorkflowInformation '*
		End Get
	End Property '*
	'*****************************************************************
	' end of retained for consistency with existing interface in    '*
	' pmwrkmanager                                                  '*
	'*****************************************************************
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	Private Sub cboActionType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboActionType.SelectedIndexChanged
		m_lReturn = SetupTaskCombos(ACTaskAction)
	End Sub
	Private Sub cboEventType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboEventType.SelectedIndexChanged
		m_lReturn = SetupFrames()
	End Sub
	
	Private Sub cboTask_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTask.SelectedIndexChanged
		m_lReturn = SetupTaskCombos(ACTask)
		m_sPrevTask = cboTask.Text
	End Sub
	
	Private Sub cboTaskGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaskGroup.SelectedIndexChanged
		m_lReturn = SetupTaskCombos(ACTaskGroup)
		m_sPrevTaskGroup = cboTaskGroup.Text
		
		' ensure mandatory fields are indicated
		If m_sPrevTaskGroup <> "" Then
			SetMandatoryFields()
		End If
		
	End Sub
	
	Private Sub cboUserGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUserGroup.SelectedIndexChanged
		m_lReturn = SetupAllocateToFrame()
		m_sPrevUserGroup = cboUserGroup.Text
	End Sub
	
	Private Sub chkComplete_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkComplete.CheckStateChanged
		m_lReturn = SetupCompleteFrame()
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lReturn = ActionCancel()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lReturn = ActionOK()
	End Sub
	
	
	
	
	
	
	
	
	
	
	Private Sub cmdTaskLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTaskLog.Click
		m_lReturn = ActionTaskLog()
	End Sub
	
	' ***************************************************************** '
	' Name: Form_Initialize
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '
	Private Sub Form_Initialize_Renamed()
		
		Const sFunction As String = "Form_Initialize"
		
		Try 
			
			' initialise form error indicator
			m_bError = False
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			If g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMEventTask.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
				m_oBusiness = temp_m_oBusiness
				
				' interface error shut down
				m_bError = False
				
				' Failed to get an instance of the business object.
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & "Failed to create business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=New Exception(Information.Err().Description))
				
				' reset mouse pointer
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
			Else
				m_oBusiness = temp_m_oBusiness
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=excep)
			
			' reset mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: Form_QueryUnload
	'
	' Description: Determines whether any actions need to take place
	'               before unload.
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	' ***************************************************************** '
	' Name: Form_Unload
	'
	' Description: Destroys all object references
	'
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		Const sFunctionName As String = "Form_Unload"
		
		Try 
			
			' Terminate the business object

		m_oBusiness.Dispose()
            ' destroy object reference
			m_oBusiness = Nothing
		
		Catch excep As System.Exception
			
			
			
			m_bInterfaceError = True
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: Form_Load
	'
	' Parameters: N/A
	'
	' Description: Sets up the form, populates controls, etc
	'
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Const sFunctionName As String = "Form_Load"
		
		Try 
			
			' set up interface
			m_lReturn = SetUpForm()
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			m_bInterfaceError = True
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: SetUpForm
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 23-06-2003 : WorkFlow
	' ***************************************************************** '
	Private Function SetUpForm() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetUpForm"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_lReturn = GetResourceData()
			
			m_lReturn = GetData()
			
			m_lReturn = PopulateForm()
			
			m_lReturn = SetupFields()
			
			m_lReturn = SetupButtons()
			
			m_lReturn = SetupFrames()
			
			m_lReturn = SetMandatoryFields()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetResString
	'
	' Parameters: n/a
	'
	' Description: Returns string item from resource file
	'
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '
	Private Function GetResString(ByVal v_lItemId As Integer) As String
		
		Dim result As String = String.Empty
		Const sFunctionName As String = "GetResString"
		
		Dim sReturn As String = ""
		
		Try 
			
			' always want to return a string

			sReturn = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=v_lItemId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
			
			
			Return sReturn
		
		Catch excep As System.Exception
			
			
			
			result = "Error"
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: LogMsg
	'
	' Parameters: n/a
	'
	' Description: Wrapper for default LogMessageToFile function
	'
	' History:
	'           Created : MEvans : 28-05-2003 : 223
	' ***************************************************************** '
	Private Sub LogMsg(ByVal v_sMsg As String, ByVal v_sMethod As String)
		
		Const sFunctionName As String = "LogMsg"
		
		Try 
			
			gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=v_sMethod & ":" & v_sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sMethod)
		
		Catch excep As System.Exception
			
			
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'******************************
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: GetResourceData
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 23-06-2003 : WorkFlow
	' ***************************************************************** '
	Private Function GetResourceData() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetResourceData"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' form
			Me.Text = GetResString(ACResDataInterfaceTitle)
			
			' tab
			tabMainTab.SelectedTab.Text = GetResString(ACResDataInterfaceTabActionTypes)
			
			' frames
			
			
			
			
			
			
			cmdOK.Text = GetResString(ACResDataInterfaceButtonOK)
			cmdCancel.Text = GetResString(ACResDataInterfaceButtonCancel)
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	
	' ***************************************************************** '
	' Name: ConvertFromNullValues
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 05-06-2003 : 223
	' ***************************************************************** '
	Private Function ConvertFromNullValues(ByRef r_vValue As String, ByVal v_iDataType As Integer) As String
		
		Dim result As String = String.Empty
		Const sFunctionName As String = "ConvertFromNullValues"
		
		Try 
			
			result = CStr(gPMConstants.PMEReturnCode.PMTrue)
			
			
			
			Select Case v_iDataType
				Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMCurrency, gPMConstants.PMEDataType.PMBoolean, gPMConstants.PMEDataType.PMDate
					If r_vValue Is DBNull.Value.ToString() Or r_vValue = "" Then
						Return CStr(0)
					Else
						Return r_vValue
					End If
					
				Case Else
					Return r_vValue
					
			End Select
		
		Catch 
		End Try
		
		
		
		result = CStr(gPMConstants.PMEReturnCode.PMError)
		
		'******************************
		' Log Error.
        gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
		'*******************************
		
		Return result
		
	End Function
	
	
	' ***************************************************************** '
	' Name: SetupButtons
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 23-06-2003 : Workflow
	' ***************************************************************** '
	Private Function SetupButtons() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetupButtons"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			cmdTaskLog.Enabled = False
			
			If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
				cmdTaskLog.Enabled = True
			End If
			
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				cmdOK.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ActionOK
	'
	' Parameters: n/a
	'
	' Description: Perform any actions required by ok button
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	Private Function ActionOK() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ActionOK"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' if the form data passes all the validation checks
			If ValidateChecks() = gPMConstants.PMEReturnCode.PMTrue Then
				
				' perform update / create
				If ProcessUpdates() = gPMConstants.PMEReturnCode.PMTrue Then
					
					m_lStatus = gPMConstants.PMEReturnCode.PMOK
					
					Me.Close()
				Else
					result = gPMConstants.PMEReturnCode.PMFalse
					
					' Log Error.
					gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to process updates", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
					
				End If
				
			Else
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ActionCancel
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 07-07-2003 : workflow
	' ***************************************************************** '
	Private Function ActionCancel() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ActionCancel"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			Me.Close()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ConvertToNullValues
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 05-06-2003 : 223
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (ConvertToNullValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ConvertToNullValues(ByRef r_vValue As gPMConstants.PMEReturnCode, ByVal v_iDataType As Integer) As gPMConstants.PMEReturnCode
		'
		'Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		'Const sFunctionName As String = "ConvertToNullValues"
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'
			'Select Case v_iDataType
				'Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMCurrency
					'If r_vValue = gPMConstants.PMEReturnCode.PMFalse Then

						'r_vValue = Nothing
					'End If
					'
				'Case gPMConstants.PMEDataType.PMString
					'If r_vValue = "" Then

						'r_vValue = Nothing
					'End If
					'
			'End Select
			'
			'
			'Return r_vValue
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result
			'
		'End Try
	'End Function
	
	
	' ***************************************************************** '
	' Name: GetLookups
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 05-06-2003 : 223
	' ***************************************************************** '
	Private Function GetLookups() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetLookups"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			ReDim m_vLookupTables(3, 6)
			
			m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = ACLookupTablePMWrkTaskGroup
			m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = ACLookupTablePMWrkTask
			m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = ACLookupTablePMUserGroup
			m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = ACLookupTableEventType
			m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 4) = ACLookupTableEventLogSubject
			m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 5) = ACLookupTablePMWrkTaskActionType
			m_vLookupTables(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 6) = ACLookupTableTaskOutcome
			

			If m_oBusiness.GetLookupValues(r_vTableArray:=m_vLookupTables, r_vResultArray:=m_vLookupDetails) <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateForm
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 30-06-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateForm() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateForm"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_iTask = gPMConstants.PMEComponentAction.PMEdit Or m_iTask = gPMConstants.PMEComponentAction.PMView Then
				
				PopulateEventTaskDetails()
				
			Else
				
				' populate lookup combos and selected values
				If PopulateLookups() <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse
				End If
				
				txtEventDate.Text = DateTime.Now.ToString("dd/MM/yyyy")
				txtEventUser.Text = g_oObjectManager.UserName
				
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateEventTaskDetails
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 02-07-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateEventTaskDetails() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateEventTaskDetails"
		
        Dim bIsUrgent As Boolean
        Dim lTaskActionTypeId, lTaskStatus As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If Information.IsArray(m_vTaskEvent) Then
				
				' populate form fields
				
				txtEventType.Text = CStr(m_vTaskEvent(ACEventTask_SelectedEventTypeDescription, 0))
				txtEventSubject.Text = CStr(m_vTaskEvent(ACEventTask_SelectedEventLogSubjectDescription, 0))
				txtEventUser.Text = CStr(m_vTaskEvent(ACEventTask_EventUsername, 0))
				txtEventDescription.Text = CStr(m_vTaskEvent(ACEventTask_EventDescription, 0))
				txtTaskGroup.Text = CStr(m_vTaskEvent(ACEventTask_SelectedTaskGroupDescription, 0))
				txtTask.Text = CStr(m_vTaskEvent(ACEventTask_SelectedTaskDescription, 0))
				txtTaskDescription.Text = CStr(m_vTaskEvent(ACEventTask_TaskDescription, 0))
				txtClient.Text = CStr(m_vTaskEvent(ACEventTask_Customer, 0))
				txtWorkflow.Text = CStr(m_vTaskEvent(ACEventTask_WorkflowInformation, 0))
				txtAllocateToUserGroup.Text = CStr(m_vTaskEvent(ACEventTask_TaskAllocateToUsergroupDescription, 0))
				txtAllocateToUser.Text = CStr(m_vTaskEvent(ACEventTask_TaskAllocateToUsernameDescription, 0))
				txtActionType.Text = CStr(m_vTaskEvent(ACEventTask_SelectedTaskActionTypeDescription, 0))
				
				' get fields for event update
				m_lEventCnt = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_EventCnt, 0)), gPMConstants.PMEDataType.PMLong))
				m_lPartyCnt = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_PartyCnt, 0)), gPMConstants.PMEDataType.PMLong))
				m_lEventType = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_EventTypeId, 0)), gPMConstants.PMEDataType.PMLong))
				m_lInsuranceFolderCnt = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_InsuranceFolderCnt, 0)), gPMConstants.PMEDataType.PMLong))
				m_lInsuranceFileCnt = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_InsuranceFileCnt, 0)), gPMConstants.PMEDataType.PMLong))
				m_lClaimCnt = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_ClaimCnt, 0)), gPMConstants.PMEDataType.PMLong))
				m_lDocumentCnt = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_DocumentCnt, 0)), gPMConstants.PMEDataType.PMLong))
				m_lOldAddressCnt = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_OldAddressCnt, 0)), gPMConstants.PMEDataType.PMLong))
				m_lNewAddressCnt = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_NewAddressCnt, 0)), gPMConstants.PMEDataType.PMLong))
				m_lCampaignId = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_CampaignId, 0)), gPMConstants.PMEDataType.PMLong))
				m_lDocumentTypeId = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_DocumentTypeId, 0)), gPMConstants.PMEDataType.PMLong))
				m_lReportTypeId = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_ReportTypeId, 0)), gPMConstants.PMEDataType.PMLong))
				m_lOldPartyTypeId = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_OldPartyTypeId, 0)), gPMConstants.PMEDataType.PMLong))
				m_lDocumentTemplateId = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_DocumentTemplateId, 0)), gPMConstants.PMEDataType.PMLong))
				m_sOutputMediaCode = CStr(m_vTaskEvent(ACEventTask_OutputMediaCode, 0))
				m_dtEventDate = CDate(m_vTaskEvent(ACEventTask_EventDate, 0))
				m_iEventUserId = CInt(m_vTaskEvent(ACEventTask_UserId, 0))
				
				' format date / time fields fields
				If Information.IsDate(m_vTaskEvent(ACEventTask_EventDate, 0)) Then
					txtEventDate.Text = CDate(m_vTaskEvent(ACEventTask_EventDate, 0)).ToString("dd/MMM/yyyy")
				End If
				
				If Information.IsDate(m_vTaskEvent(ACEventTask_TaskDueDate, 0)) Then
					txtTaskDueDate.Text = CDate(m_vTaskEvent(ACEventTask_TaskDueDate, 0)).ToString("dd/MMM/yyyy")
					txtTaskDueDateTime.Text = CDate(m_vTaskEvent(ACEventTask_TaskDueDate, 0)).ToString("HH:MM:SS")
				End If
				
				' format check box fields
				bIsUrgent = CBool(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_IsUrgent, 0)), gPMConstants.PMEDataType.PMBoolean))
				If bIsUrgent Then
					chkTaskUrgent.CheckState = CheckState.Checked
				Else
					chkTaskUrgent.CheckState = CheckState.Unchecked
				End If
				
				lTaskStatus = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_TaskStatus, 0)), gPMConstants.PMEDataType.PMBoolean))
				If lTaskStatus = ACTaskStatusComplete Then
					' if this item is completed then swap to view mode
					' as a completed task cannot be edited
					m_iTask = gPMConstants.PMEComponentAction.PMView
					
					' set other complete frame values.
					chkComplete.CheckState = CheckState.Checked
					
					txtOutcome.Text = ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_TaskOutcomeDescription, 0)), gPMConstants.PMEDataType.PMString)
					
					If Information.IsDate(m_vTaskEvent(ACEventTask_TaskOutcomeDate, 0)) And CStr(m_vTaskEvent(ACEventTask_TaskOutcomeDate, 0)) <> "00:00:00" Then
						txtOutcomeDate.Text = CDate(m_vTaskEvent(ACEventTask_TaskOutcomeDate, 0)).ToString("dd/MMM/yyyy")
						txtOutcomeDateTime.Text = CDate(m_vTaskEvent(ACEventTask_TaskOutcomeDate, 0)).ToString("HH:MM:SS")
					End If
					
				Else
					chkComplete.CheckState = CheckState.Unchecked
				End If
				
				lTaskActionTypeId = CInt(m_vTaskEvent(ACEventTask_PMWrkTaskActionTypeid, 0))
				m_lReturn = PopulateOutcomeCbo(lTaskActionTypeId)
				
				'************************
				' set up default return values for work manager event call
				' included to keep component compatible with existing work manager code...
				'
				' can be updated in edit mode so will need to updated later in the
				' process....
				m_sTaskCustomer = CStr(m_vTaskEvent(ACEventTask_Customer, 0))
				m_sTaskDescription = CStr(m_vTaskEvent(ACEventTask_TaskDescription, 0))
				m_dtTaskDueDate = CDate(m_vTaskEvent(ACEventTask_TaskDueDate, 0))
				m_iTaskIsUrgent = CInt(m_vTaskEvent(ACEventTask_IsUrgent, 0))
				m_iTaskStatus = CInt(m_vTaskEvent(ACEventTask_TaskStatus, 0))
				'
				' cant be updated
				m_lPMWrkTaskInstanceCnt = CInt(m_vTaskEvent(ACEventTask_PMWrkTaskInstanceCnt, 0))
				m_lAllocateToUserGroupId = CInt(m_vTaskEvent(ACEventTask_AllocateToPMUserGroupId, 0))
				m_lAllocateToUserId = CInt(ConvertFromNullValues(CStr(m_vTaskEvent(ACEventTask_AllocateToUserId, 0)), gPMConstants.PMEDataType.PMLong))
				m_lPMNavProcessID = CInt(m_vTaskEvent(ACEventTask_PMNavProcessId, 0))
				m_sComponentObjectName = CStr(m_vTaskEvent(ACEventTask_ComponentObjectName, 0))
				m_sComponentClassName = CStr(m_vTaskEvent(ACEventTask_ComponentClassName, 0))
				m_lDisplayIcon = CInt(m_vTaskEvent(ACEventTask_DisplayIcon, 0))
				m_iIsViewOnlyTask = CInt(m_vTaskEvent(ACEventTask_IsViewOnlyTask, 0))
				m_sLinkedObjectName = CStr(m_vTaskEvent(ACEventTask_LinkedObjectName, 0))
				m_sLinkedClassName = CStr(m_vTaskEvent(ACEventTask_LinkedClassName, 0))
				m_sLinkedCaption = CStr(m_vTaskEvent(ACEventTask_linkedcaptionid, 0))
				m_sWorkflowInformation = CStr(m_vTaskEvent(ACEventTask_WorkflowInformation, 0))
				'************************
				
			Else
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			Return result


			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetLookupDetails
	'
	' Description: Gets all of the lookup details using the lookup
	'              values, then assigns them to the control passed.
	'               Selects the specified row
	'
	' ***************************************************************** '
	
	Private Function GetLookupDetails(ByVal v_sLookupTable As String, ByRef r_octlLookup As ComboBox, Optional ByVal v_lSelectedItemId As Integer = 0) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetLookupDetails"
		
		' Lookup value contants.
		Const ACValueTableName As Integer = 0
        'Const ACValueID As Integer = 1         ''Unused Local Variable
		Const ACValueStartPos As Integer = 2
		Const ACValueNumber As Integer = 3
		
		Dim lRow As Integer
		Dim bFoundMatch As Boolean
		Dim lItemIndex, lItemFoundIndex, lIndex As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the lookup values.
			
			r_octlLookup.Items.Clear()
			
			bFoundMatch = False
			
			For lRow = m_vLookupTables.GetLowerBound(1) To m_vLookupTables.GetUpperBound(1)
				' Check for a match of the table name.
				If CStr(m_vLookupTables(ACValueTableName, lRow)).Trim() = v_sLookupTable.Trim() Then
					' Found a match
					bFoundMatch = True
					Exit For
				End If
			Next lRow
			
			' Check if there has been a table match.
			If Not bFoundMatch Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & v_sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
				
				Return result
			End If
			
			' Using the lookup values, populate the control with
			' the details from the lookup details array.
			lItemIndex = -1
			lItemFoundIndex = -1
			
			lIndex = 0
			
			For lCntr As Integer = CInt(m_vLookupTables(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupTables(ACValueStartPos, lRow)) + CDbl(m_vLookupTables(ACValueNumber, lRow))) - 1)
				
				r_octlLookup.Items.Add(CStr(m_vLookupDetails(ACDetailDesc, lCntr)))
				VB6.SetItemData(r_octlLookup, lIndex, CInt(m_vLookupDetails(ACDetailKey, lCntr)))
				
				
				If CDbl(m_vLookupDetails(ACDetailKey, lCntr)) = v_lSelectedItemId Then
					lItemFoundIndex = lItemIndex
				End If
				
				Debug.WriteLine(VB6.GetItemString(r_octlLookup, lIndex) & ":" & CStr(lIndex))
				
				lIndex += 1
				lItemIndex += 1
				
			Next lCntr
			
			' set the item we want to display in the list
			If lItemFoundIndex <> -1 Then
				r_octlLookup.SelectedIndex = lItemFoundIndex
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			Return result


			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: PopulateLookups
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 30-06-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateLookups() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateLookups"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Populate Lookup Values
			
			' Event Type
			m_lReturn = GetLookupDetails(v_sLookupTable:=ACLookupTableEventType, r_octlLookup:=cboEventType)
			
			' Event Subject
			m_lReturn = GetLookupDetails(v_sLookupTable:=ACLookupTableEventLogSubject, r_octlLookup:=cboEventSubject)
			
			' Task Group
			m_lReturn = GetLookupDetails(v_sLookupTable:=ACLookupTablePMWrkTaskGroup, r_octlLookup:=cboTaskGroup)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetPMUsers
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function GetPMUsers() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetPMUsers"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If m_oBusiness.GetPMUser(r_vResults:=m_vPMUsers) <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Interface
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetTaskGroupTask
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function GetTaskGroupTask() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetTaskGroupTask"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			

			If m_oBusiness.GetTaskGroupTask(r_vResults:=m_vTaskGroupTask) <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetTaskGroupTaskAction
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function GetTaskGroupTaskAction() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetTaskGroupTaskAction"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If m_oBusiness.GetTaskGroupTaskAction(r_vResults:=m_vTaskGroupTaskAction) <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetData
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function GetData() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetData"
		
		Dim bError As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			bError = False
			
			If Not bError Then
				If m_lPMWrkTaskInstanceCnt <> 0 Then
					If GetEventTask() <> gPMConstants.PMEReturnCode.PMTrue Then
						result = gPMConstants.PMEReturnCode.PMFalse
					End If
				End If
			End If
			
			' Lookups
			If Not bError Then
				If GetLookups() <> gPMConstants.PMEReturnCode.PMTrue Then
					bError = True
				End If
			End If
			
			' PMUsers
			If Not bError Then
				If GetPMUsers() <> gPMConstants.PMEReturnCode.PMTrue Then
					bError = True
				End If
			End If
			
			' Task Group Tasks
			If Not bError Then
				If GetTaskGroupTask() <> gPMConstants.PMEReturnCode.PMTrue Then
					bError = True
				End If
			End If
			
			' Task Group Task Actions
			If Not bError Then
				If GetTaskGroupTaskAction() <> gPMConstants.PMEReturnCode.PMTrue Then
					bError = True
				End If
			End If
			
			' PMUser Group Users
			If Not bError Then
				If GetPMUserGroupUsers() <> gPMConstants.PMEReturnCode.PMTrue Then
					bError = True
				End If
			End If
			
			' Task Group User Groups
			If Not bError Then
				If GetTaskGroupUserGroups() <> gPMConstants.PMEReturnCode.PMTrue Then
					bError = True
				End If
			End If
			
			' Task Action Type Outcomes
			If Not bError Then
				If GetTaskActionTypeOutcomes() <> gPMConstants.PMEReturnCode.PMTrue Then
					bError = True
				End If
			End If
			
			If bError Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetupFrames
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function SetupFrames() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetupFrames"
		
		Dim sEventTypeCode As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			SetupTaskFrame()
			
			SetupCompleteFrame()
			
			SetupAllocateToFrame()
			
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				fraEvent.Enabled = False
				fraTask.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetupTaskFrame
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function SetupTaskFrame() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetupTaskFrame"
		
		Dim sEventTypeCode As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				
				' defaults
				fraTask.Enabled = False
				
				' determine if fratask should be enabled
				If cboEventType.Text <> "" Then
					
					' get event type code
					If GetLookupItem(v_sLookupTable:=ACLookupTableEventType, r_sItemDesc:=cboEventType.Text, r_sItemCode:=sEventTypeCode, r_lItemId:=0) = gPMConstants.PMEReturnCode.PMTrue Then
						
						' if event type code is task related
						If sEventTypeCode = ACEventTypeCodeClaimDebtTask Or sEventTypeCode = ACEventTypeCodeClientTask Or sEventTypeCode = ACEventTypeCodePolicyTask Or sEventTypeCode = ACEventTypeCodeClaimTask Then
							
							fraTask.Enabled = True
							
						End If
						
					End If
				End If
				
				' reset the task frame
				If Not fraTask.Enabled Then
					
					cboTaskGroup.SelectedIndex = -1
					SetupTaskCombos(ACTaskGroup)
					txtTaskDueDate.Text = ""
					txtTaskDueDateTime.Text = ""
					txtTaskDescription.Text = ""
					chkTaskUrgent.CheckState = CheckState.Unchecked
					
					m_sPrevTaskAction = ""
					m_sPrevTask = ""
					m_sPrevTaskGroup = ""
					
				End If
				
			ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then 
				
				If txtTaskGroup.Text <> "" Then
					fraTask.Enabled = True
				End If
			End If
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	'***************************************************************** '
	' Name: SetupCompleteFrame
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function SetupCompleteFrame() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetupCompleteFrame"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
				
				If fraTask.Enabled Then
					
					' default
					fraComplete.Enabled = False
					
					' determine if fracomplete should be enabled
					If chkComplete.CheckState = CheckState.Checked Then
						fraComplete.Enabled = True
						
						' default these values when complete is checked..
						txtOutcomeDate.Text = DateTime.Now.ToString("dd/MM/yyyy")
						txtOutcomeDateTime.Text = DateTime.Now.ToString("HH:MM:SS")
					Else
						txtOutcomeDate.Text = ""
						txtOutcomeDateTime.Text = ""
						cboOutcome.SelectedIndex = -1
					End If
					
				Else
					chkComplete.CheckState = CheckState.Unchecked
					txtOutcomeDate.Text = ""
					txtOutcomeDateTime.Text = ""
					cboOutcome.SelectedIndex = -1
				End If
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetLookupItem
	'
	' Parameters: n/a
	'
	' Description: Returns the code for a specifed item description
	'                  in a specified lookup table..
	'
	' History:
	'           Created : MEvans : 06-06-2003 : 223
	' ***************************************************************** '
	Private Function GetLookupItem(ByVal v_sLookupTable As String, ByVal r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetLookupItem"
		
		Dim lRow As Integer
		Dim bFoundMatch As Boolean
		Dim sCode As String = ""
		
		Dim llBound, lUBound As Integer
		Dim v_vLookupItem As String = ""
		Dim lLookupItem As Integer
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Lookup value contants.
			Const ACValueTableName As Integer = 0
            'Const ACValueID As Integer = 1         ''Unused Local Variable
			Const ACValueStartPos As Integer = 2
			Const ACValueNumber As Integer = 3
			
			Const ACDetailKey As Integer = 0
			Const ACDetailDesc As Integer = 1
			Const ACDetailCode As Integer = 2
			
			' Initilisation
			bFoundMatch = False
			
			For lRow = m_vLookupTables.GetLowerBound(1) To m_vLookupTables.GetUpperBound(1)
				
				' Check for a match of the table name.
				If CStr(m_vLookupTables(ACValueTableName, lRow)).Trim() = v_sLookupTable.Trim() Then
					bFoundMatch = True
					Exit For
				End If
				
			Next lRow
			
			If bFoundMatch Then
				' get array boundaries for specified table
				llBound = CInt(m_vLookupTables(ACValueStartPos, lRow))
				
				lUBound = CInt((CDbl(m_vLookupTables(ACValueStartPos, lRow)) + CDbl(m_vLookupTables(ACValueNumber, lRow))) - 1)
				
				
				' set lookup properties
				If r_lItemId <> 0 Then
					v_vLookupItem = CStr(r_lItemId)
					lLookupItem = 0
					
				ElseIf r_sItemDesc <> "" Then 
					v_vLookupItem = r_sItemDesc
					lLookupItem = 1
					
				ElseIf r_sItemCode <> "" Then 
					v_vLookupItem = r_sItemCode
					lLookupItem = 2
				End If
				
				' loop around the available items for the specified table
				For lCntr As Integer = llBound To lUBound
					
					' get the code for the specified lookup items key
					If CStr(m_vLookupDetails(lLookupItem, lCntr)).Trim() = v_vLookupItem Then
						
						' return the requested code, id, description
						r_sItemDesc = CStr(m_vLookupDetails(ACDetailDesc, lCntr)).Trim()
						r_sItemCode = CStr(m_vLookupDetails(ACDetailCode, lCntr)).Trim()
						r_lItemId = CInt(CStr(m_vLookupDetails(ACDetailKey, lCntr)).Trim())
						
						Exit For
					End If
					
				Next lCntr
				
			End If
			
			' if we dont find the code then log an error
			If r_sItemCode = "" Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
				'******************************
				' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find code for lookuptable:" & v_sLookupTable & "and lookup Item:" & v_vLookupItem, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
				'*******************************
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: SetupTaskCombos
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function SetupTaskCombos(ByVal v_lItemChangedLevel As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetupTaskCombos"
		
		Dim lTaskGroupId, lTaskActionTypeId, lTaskId As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' get task group id
			If cboTaskGroup.Text <> "" Then
				m_lReturn = GetLookupItem(v_sLookupTable:=ACLookupTablePMWrkTaskGroup, r_sItemDesc:=cboTaskGroup.Text, r_sItemCode:="", r_lItemId:=lTaskGroupId)
			End If
			
			' get task id
			If cboTask.Text <> "" Then
				m_lReturn = GetLookupItem(v_sLookupTable:=ACLookupTablePMWrkTask, r_sItemDesc:=cboTask.Text, r_sItemCode:="", r_lItemId:=lTaskId)
			End If
			
			' get task action type id
			If cboActionType.Text <> "" Then
				m_lReturn = GetLookupItem(v_sLookupTable:=ACLookupTablePMWrkTaskActionType, r_sItemDesc:=cboActionType.Text, r_sItemCode:="", r_lItemId:=lTaskActionTypeId)
			End If
			
			
			Select Case v_lItemChangedLevel
				Case ACTaskGroup
					
					' if a new item has been selected
					If m_sPrevTaskGroup <> cboTaskGroup.Text Then
						
						If cboTaskGroup.Text <> "" Then
							
							' populate task
							If PopulateTaskCbo(v_lTaskGroupId:=lTaskGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
								result = gPMConstants.PMEReturnCode.PMFalse
							End If
							
							' populate user groups
							If PopulateUserGroupscbo(v_lTaskGroupId:=lTaskGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
								result = gPMConstants.PMEReturnCode.PMFalse
							End If
							
						Else
							' clear down any selected task, and
							' any associated fields
							
							' task group related
							cboUserGroup.SelectedIndex = -1
							cboUserGroup.Enabled = False
							
							' task group related
							cboTask.Items.Clear()
							cboTask.Enabled = False
							
							' task related
							cboActionType.Items.Clear()
							cboActionType.Enabled = False
							m_sPrevTask = ""
							
							' action type related
							cboOutcome.SelectedIndex = -1
							cboOutcome.Enabled = False
							m_sPrevTaskAction = ""
							txtTaskDueDate.Text = ""
							txtTaskDueDateTime.Text = ""
							
						End If
						
					End If
					
				Case ACTask
					
					' if a new item has been selected
					If m_sPrevTask <> cboTask.Text Then
						If cboTask.Text <> "" Then
							If PopulateTaskActionTypeCbo(v_lTaskGroupId:=lTaskGroupId, v_lTaskId:=lTaskId) <> gPMConstants.PMEReturnCode.PMTrue Then
								
								result = gPMConstants.PMEReturnCode.PMFalse
							End If
						Else
							' clear down any selected task action types
							
							' task related
							cboActionType.Items.Clear()
							cboActionType.Enabled = False
							
							' action type related
							cboOutcome.SelectedIndex = -1
							cboOutcome.Enabled = False
							m_sPrevTaskAction = ""
							txtTaskDueDate.Text = ""
							txtTaskDueDateTime.Text = ""
							
						End If
					End If
					
				Case ACTaskAction
					
					If m_sPrevTaskAction <> cboActionType.Text Then
						
						If cboActionType.Text <> "" Then
							If Information.IsArray(m_vTaskActionDueDays) Then
								
								txtTaskDueDate.Text = DateTime.Now.AddDays(CDbl(m_vTaskActionDueDays(cboActionType.SelectedIndex))).ToString("dd/MM/yyyy")
								txtTaskDueDateTime.Text = DateTime.Now.ToString("HH:MM:SS")
								
							End If
							
							' populate user groups
							If PopulateOutcomeCbo(v_lTaskActionTypeId:=lTaskActionTypeId) <> gPMConstants.PMEReturnCode.PMTrue Then
								result = gPMConstants.PMEReturnCode.PMFalse
							End If
							
						Else
							' action type related
							cboOutcome.SelectedIndex = -1
							cboOutcome.Enabled = False
							txtTaskDueDate.Text = ""
							txtTaskDueDateTime.Text = ""
						End If
						
					End If
					
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Interface
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateTaskCbo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateTaskCbo(ByVal v_lTaskGroupId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateTaskCbo"
		
		Dim llBound, lUBound, lTaskId, lTaskGroupId As Integer
		Dim sTaskDescription As String = ""
		Dim lIndex As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' initialisation
			m_sPrevTask = ""
			m_sPrevTaskAction = ""
			
			cboTask.Items.Clear()
			cboActionType.Items.Clear()
			
			lIndex = 0
			
			' if we have a selected task group
			If cboTaskGroup.Text <> "" Then
				
				' if we have an array of task group tasks
				If Information.IsArray(m_vTaskGroupTask) Then
					
					' get array boundaries
					llBound = m_vTaskGroupTask.GetLowerBound(1)
					lUBound = m_vTaskGroupTask.GetUpperBound(1)
					
					' for each item in the array
					For lItem As Integer = llBound To lUBound
						
						' get item details
						lTaskGroupId = CInt(m_vTaskGroupTask(ACTaskGroupTaskGroupId, lItem))
						lTaskId = CInt(m_vTaskGroupTask(ACTaskgroupTaskId, lItem))
						sTaskDescription = CStr(m_vTaskGroupTask(ACTaskGroupTaskDescription, lItem))
						
						' if task group matches the selected task group
						If lTaskGroupId = v_lTaskGroupId Then
							
							' add task to combo
							cboTask.Items.Insert(lIndex, sTaskDescription)
							VB6.SetItemData(cboTask, lIndex, lTaskId)
							lIndex += 1
							
						End If
						
					Next lItem
					
				End If
				
			End If
			
			cboTask.Enabled = Not (lIndex = 0)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateTaskActionTypeCbo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateTaskActionTypeCbo(ByVal v_lTaskGroupId As Integer, ByVal v_lTaskId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateTaskActionTypeCbo"
		
		Dim lTaskGroupId, lTaskId As Integer
		Dim sTaskActionType As String = ""
        Dim lIndex, llBound, lUBound, lTaskActionTypeId, lTaskActionDueDays As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' initialisation
			txtTaskDueDate.Text = ""
			txtTaskDueDateTime.Text = ""
			m_sPrevTaskAction = ""
			cboActionType.Items.Clear()
			lIndex = 0
			
			' if we have a selected task group
			If cboTask.Text <> "" Then
				
				' if we have an array of task group tasks
				If Information.IsArray(m_vTaskGroupTaskAction) Then
					
					' get array boundaries
					llBound = m_vTaskGroupTaskAction.GetLowerBound(1)
					lUBound = m_vTaskGroupTaskAction.GetUpperBound(1)
					
					' for each item in the array
					For lItem As Integer = llBound To lUBound
						
						' get item details
						lTaskGroupId = CInt(m_vTaskGroupTaskAction(ACTaskGroupTaskActionTaskGroupId, lItem))
						lTaskId = CInt(m_vTaskGroupTaskAction(ACTaskGroupTaskActionTaskId, lItem))
						lTaskActionTypeId = CInt(m_vTaskGroupTaskAction(ACTaskGroupTaskActionTypeId, lItem))
						sTaskActionType = CStr(m_vTaskGroupTaskAction(ACTaskGroupTaskActionDescription, lItem))
						lTaskActionDueDays = CInt(ConvertFromNullValues(CStr(m_vTaskGroupTaskAction(ACTaskGroupTaskActionDueDays, lItem)), gPMConstants.PMEDataType.PMLong))
						
						' if task group matches the selected task group
						If lTaskGroupId = v_lTaskGroupId And lTaskId = v_lTaskId Then
							
							' add task to combo
							cboActionType.Items.Insert(lIndex, sTaskActionType)
							VB6.SetItemData(cboActionType, lIndex, lTaskActionTypeId)
							
							' generate the due days array
							If Information.IsArray(m_vTaskActionDueDays) Then
								ReDim Preserve m_vTaskActionDueDays(lIndex)
							Else
								ReDim m_vTaskActionDueDays(lIndex)
							End If
							
							m_vTaskActionDueDays(lIndex) = lTaskActionDueDays
							
							lIndex += 1
						End If
						
					Next lItem
					
				End If
				
			End If
			
			cboActionType.Enabled = Not (lIndex = 0)
			
			Return result
		
		Catch excep As System.Exception
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	
	
	' ***************************************************************** '
	' Name: GetPMUserGroupUsers
	'
	' Parameters: n/a
	'
	' Description: Returns all entries from PMUser_Group_User
	'
	' History:
	'           Created : MEvans : 01-07-2003: workflow
	' ***************************************************************** '
	Private Function GetPMUserGroupUsers() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetPMUserGroupUsers"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If m_oBusiness.GetPMUserGroupUsers(r_vResults:=m_vPMUserGroupUsers) <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: SetupAllocateToFrame
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function SetupAllocateToFrame() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetupAllocateToFrame"
		
		Dim lUserGroupId As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				
				If fraTask.Enabled Then
					
					If cboUserGroup.Text <> "" Then
						
						If m_sPrevUserGroup <> cboUserGroup.Text Then
							
							If GetLookupItem(v_sLookupTable:=ACLookupTablePMUserGroup, r_sItemDesc:=cboUserGroup.Text, r_sItemCode:="", r_lItemId:=lUserGroupId) = gPMConstants.PMEReturnCode.PMTrue Then
								
								If Information.IsArray(m_vPMUserGroupUsers) Then
									
									If PopulateAllocateToUserCombo(v_lPMUserGroupId:=lUserGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
										result = gPMConstants.PMEReturnCode.PMFalse
									End If
									
								End If
								
							End If
							
						End If
						
					Else
						cboUser.SelectedIndex = -1
						cboUser.Enabled = False
					End If
					
				Else
					cboUserGroup.SelectedIndex = -1
					cboUser.SelectedIndex = -1
					cboUser.Enabled = False
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: PopulateAllocateToUserCombo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateAllocateToUserCombo(ByVal v_lPMUserGroupId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateAllocateToUserCombo"
		
		Dim lIndex, llBound, lUBound, lPMUserGroupId, lPMUserId As Integer
		Dim sUsername As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' defaults
			lIndex = 0
			cboUser.Items.Clear()
			
			' if we have a selected task group
			If cboUserGroup.Text <> "" Then
				
				' if we have an array of task group tasks
				If Information.IsArray(m_vPMUserGroupUsers) Then
					
					' get array boundaries
					llBound = m_vPMUserGroupUsers.GetLowerBound(1)
					lUBound = m_vPMUserGroupUsers.GetUpperBound(1)
					
					' for each item in the array
					For lItem As Integer = llBound To lUBound
						
						' get item details
						lPMUserGroupId = CInt(m_vPMUserGroupUsers(ACPMUserGroupId, lItem))
						lPMUserId = CInt(m_vPMUserGroupUsers(ACPMUserId, lItem))
						sUsername = CStr(m_vPMUserGroupUsers(ACPMUsername, lItem))
						
						' if task group matches the selected task group
						If lPMUserGroupId = v_lPMUserGroupId Then
							
							' if we have items in the list
							' add a grouping item to cover all users.
							If lIndex = 0 Then
								cboUser.Items.Insert(lIndex, "(Any Group Member)")
								VB6.SetItemData(cboUser, lIndex, 0)
								lIndex = 1
							End If
							
							' add task to combo
							cboUser.Items.Insert(lIndex, sUsername)
							VB6.SetItemData(cboUser, lIndex, lPMUserId)
							lIndex += 1
							
						End If
						
					Next lItem
					
				End If
				
			End If
			
			If lIndex = 0 Then
				cboUser.Enabled = False
			Else
				cboUser.Enabled = True
				
				' select the first grouping row automatically
				cboUser.SelectedIndex = 0
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	
	' ***************************************************************** '
	' Name: ValidateChecks
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function ValidateChecks() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ValidateChecks"
		
		Dim bValidationFailed As Boolean
		Dim sMessage, sMessageTitle As String
		Dim dtTaskDueDate As Date
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			bValidationFailed = False
			
			sMessageTitle = GetResString(ACResDataInterfaceTitle)
			
			' if the task frame is enabled
			If fraTask.Enabled Then
				
				' if a task group is selected
				If cboTaskGroup.Text <> "" Then
					
					sMessage = GetResString(ACResDataMessageInvalidTaskDueDate)
					
					' validate task due date
					If Not bValidationFailed Then
						If txtTaskDueDate.Text = "" Then
							MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
							txtTaskDueDate.Focus()
							bValidationFailed = True
						End If
					End If
					
					If Not bValidationFailed Then
						If Not Information.IsDate(txtTaskDueDate.Text) Then
							MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
							txtTaskDueDate.Focus()
							bValidationFailed = True
						End If
					End If
					
					sMessage = GetResString(ACResDataMessageInvalidTaskDueDateTime)
					
					If Not bValidationFailed Then
						If txtTaskDueDateTime.Text = "" Then
							MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
							txtTaskDueDateTime.Focus()
							bValidationFailed = True
						End If
					End If
					
					If Not bValidationFailed Then
						If Not Information.IsDate(txtTaskDueDateTime.Text) Then
							MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
							txtTaskDueDateTime.Focus()
							bValidationFailed = True
						End If
					End If
					
					If Not bValidationFailed Then
						
						dtTaskDueDate = CDate(txtTaskDueDate.Text).AddDays(CDate(txtTaskDueDateTime.Text).ToOADate())
						
						If dtTaskDueDate <= DateTime.Now Then
							
							sMessage = GetResString(ACResDateMessageTaskDueDateInPast)
							
							If MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
								
								chkComplete.CheckState = CheckState.Checked
								
								m_lReturn = SetupCompleteFrame()
								
								If cboOutcome.Text = "" Then
									
									sMessage = GetResString(ACResDataMessageCompletedTaskOutcomeMandatory)
									MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
									cboOutcome.Focus()
									bValidationFailed = True
									
								End If
							Else
								result = gPMConstants.PMEReturnCode.PMFalse
							End If
							
						End If
						
					End If
				End If
			End If
			
			If bValidationFailed Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetTaskGroupUserGroups
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function GetTaskGroupUserGroups() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetTaskGroupUserGroups"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If m_oBusiness.GetTaskGroupUserGroups(r_vResults:=m_vTaskGroupUserGroups) <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: PopulateUserGroupscbo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateUserGroupscbo(ByVal v_lTaskGroupId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateUserGroupscbo"
		
		Dim lIndex, llBound, lUBound, lTaskGroupId, lUserGroupId As Integer
		Dim sUserGroup As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			cboUserGroup.Items.Clear()
			
			lIndex = 0
			
			' if we have a selected task group
			If cboTaskGroup.Text <> "" Then
				
				' if we have an array
				If Information.IsArray(m_vTaskGroupUserGroups) Then
					
					' get array boundaries
					llBound = m_vTaskGroupUserGroups.GetLowerBound(1)
					lUBound = m_vTaskGroupUserGroups.GetUpperBound(1)
					
					' for each item in the array
					For lItem As Integer = llBound To lUBound
						
						' get item details
						lTaskGroupId = CInt(m_vTaskGroupUserGroups(ACTaskGroupUserGroup_TaskGroupId, lItem))
						lUserGroupId = CInt(m_vTaskGroupUserGroups(ACTaskGroupUserGroup_UserGroupId, lItem))
						sUserGroup = CStr(m_vTaskGroupUserGroups(ACTaskGroupUserGroup_UserGroupDescription, lItem))
						
						' if task group matches the selected task group
						If lTaskGroupId = v_lTaskGroupId Then
							
							' add user group to combo
							cboUserGroup.Items.Insert(lIndex, sUserGroup)
							VB6.SetItemData(cboUserGroup, lIndex, lUserGroupId)
							lIndex += 1
							
						End If
						
					Next lItem
					
				End If
				
			End If
			
			cboUserGroup.Enabled = Not (lIndex = 0)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ProcessUpdates
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function ProcessUpdates() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ProcessUpdates"
		
		Dim lTaskInstanceCnt, lEventCnt As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If GetFormData() = gPMConstants.PMEReturnCode.PMTrue Then
				
				' create item
				If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
					

					If m_oBusiness.CreateEventTask(r_lTaskInstanceCnt:=lTaskInstanceCnt, v_lTaskGroupId:=m_lTaskGroupId, v_iTaskIsVisible:=gPMConstants.PMEReturnCode.PMTrue, v_lTaskId:=m_lTaskId, v_lTaskActionTypeId:=m_lTaskActionTypeId, v_sTaskCustomer:=m_sTaskCustomer, v_dtTaskDueDate:=m_dtTaskDueDate, v_lTaskPMUserGroupID:=m_lTaskAllocateToUserGroupId, v_sTaskDescription:=m_sTaskDescription, v_iTaskStatus:=m_iTaskStatus, v_iTaskIsUrgent:=m_iTaskIsUrgent, v_sTaskWorkflowInformation:=m_sTaskWorkflowInformation, v_iTaskUserID:=m_lTaskAllocateToUserId, v_dtTaskOutcomeDate:=m_dtTaskOutcomeDate, v_lTaskOutcomeId:=m_lTaskOutcomeId, v_vTaskInstanceKeyArray:="", r_vEventCnt:=lEventCnt, v_vEventPartyCnt:=m_lPartyCnt, v_vEventInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_vEventInsuranceFileCnt:=m_lInsuranceFileCnt, v_vEventClaimCnt:=m_lClaimCnt, v_vEventDocumentCnt:=0, v_vEventOldAddressCnt:=0, v_vEventNewAddressCnt:=0, v_vEventCampaignId:=0, v_vEventDocumentType:=0, v_vEventReportType:=0, v_vEventType:=m_lEventType, v_vEventDescription:=m_sEventDescription, v_vEventOldPartyTypeID:=0, v_vEventLogSubjectId:=m_lEventlogsubjectId, v_vEventTypeCode:="", v_vEventAccountKey:=m_lAccountKey, v_vEventDocumentTemplateID:=0, v_vEventOutputMediaCode:="", v_lEventClaimDebtID:=0) <> gPMConstants.PMEReturnCode.PMTrue Then
						
						' log error
						result = gPMConstants.PMEReturnCode.PMFalse
						
						LogMsg(v_sMsg:="Failed to create event task", v_sMethod:=sFunctionName)
						
					Else
						m_lPMWrkTaskInstanceCnt = lTaskInstanceCnt
					End If
					
				ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then 
					

					If m_oBusiness.AmendEventTask(v_lTaskInstanceCnt:=m_lPMWrkTaskInstanceCnt, v_sTaskCustomer:=m_sTaskDescription, v_dtTaskDueDate:=m_dtTaskDueDate, v_sTaskDescription:=m_sTaskDescription, v_iTaskStatus:=m_iTaskStatus, v_iTaskIsUrgent:=m_iTaskIsUrgent, v_sTaskWorkflowInformation:=m_sTaskWorkflowInformation, v_dtTaskOutcomeDate:=m_dtTaskOutcomeDate, v_lTaskOutcomeId:=m_lTaskOutcomeId, v_vEventCnt:=m_lEventCnt, v_vEventPartyCnt:=m_lPartyCnt, v_vEventInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_vEventInsuranceFileCnt:=m_lInsuranceFileCnt, v_vEventClaimCnt:=m_lClaimCnt, v_vEventDocumentCnt:=m_lDocumentCnt, v_vEventOldAddressCnt:=m_lOldAddressCnt, v_vEventNewAddressCnt:=m_lNewAddressCnt, v_vEventCampaignId:=m_lCampaignId, v_vEventDocumentType:=m_lDocumentTypeId, v_vEventReportType:=m_lReportTypeId, v_vEventType:=m_lEventType, v_vEventUserId:=m_iEventUserId, v_vEventDate:=m_dtEventDate, v_vEventDescription:=m_sEventDescription, v_vEventOldPartyTypeID:=m_lOldPartyTypeId, v_vEventDocumentTemplateID:=m_lDocumentTemplateId, v_vEventOutputMediaCode:=m_sOutputMediaCode) <> gPMConstants.PMEReturnCode.PMTrue Then
						
						' log error
						result = gPMConstants.PMEReturnCode.PMFalse
						
						LogMsg(v_sMsg:="Failed to update event task", v_sMethod:=sFunctionName)
						
					End If
					
				End If
				
			Else
				
				' Log Error
				result = gPMConstants.PMEReturnCode.PMFalse
				
				LogMsg(v_sMsg:="Failed to retrieve data from form", v_sMethod:=sFunctionName)
				
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetTaskActionTypeOutcomes
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function GetTaskActionTypeOutcomes() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetTaskActionTypeOutcomes"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If m_oBusiness.GetTaskActionTypeOutcomes(r_vResults:=m_vTaskActionTypeOutcomes) <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateOutcomeCbo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 01-07-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateOutcomeCbo(ByVal v_lTaskActionTypeId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateOutcomeCbo"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Dim sTaskOutcome As String = ""
            Dim lTaskOutcomeId, lTaskActionTypeId, lIndex, llBound, lUBound As Integer
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' initialisation
			cboOutcome.Items.Clear()
			lIndex = 0
			
			' if we have a selected task action type
			If cboActionType.Text <> "" Or txtActionType.Text <> "" Then
				
				' if we have an array of task outcomes
				If Information.IsArray(m_vTaskActionTypeOutcomes) Then
					
					' get array boundaries
					llBound = m_vTaskActionTypeOutcomes.GetLowerBound(1)
					lUBound = m_vTaskActionTypeOutcomes.GetUpperBound(1)
					
					' for each item in the array
					For lItem As Integer = llBound To lUBound
						
						' get item details
						lTaskActionTypeId = CInt(m_vTaskActionTypeOutcomes(ACTaskActionOutcome_TaskActionTypeId, lItem))
						lTaskOutcomeId = CInt(m_vTaskActionTypeOutcomes(ACTaskActionOutcome_TaskOutcomeId, lItem))
						sTaskOutcome = CStr(m_vTaskActionTypeOutcomes(ACTaskActionOutcome_TaskOutcome, lItem))
						
						' if task action type matches the selected task action type
						If lTaskActionTypeId = v_lTaskActionTypeId Then
							
							' add outcome to combo
							cboOutcome.Items.Insert(lIndex, sTaskOutcome)
							VB6.SetItemData(cboOutcome, lIndex, lTaskOutcomeId)
							lIndex += 1
							
						End If
						
					Next lItem
					
				End If
				
			End If
			
			cboOutcome.Enabled = Not (lIndex = 0)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetEventTask
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 02-07-2003 : workflow
	' ***************************************************************** '
	Private Function GetEventTask() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetEventTask"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If m_oBusiness.GetSpecifiedEventTask(r_vResults:=m_vTaskEvent, v_lTaskInstanceCnt:=m_lPMWrkTaskInstanceCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SizeHiddenTextField
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 02-07-2003 : workflow
	' ***************************************************************** '
	Private Sub SizeHiddenTextField(ByRef r_oTxtField As TextBox, ByRef r_ocboField As ComboBox)
		
		Const sFunctionName As String = "SizeHiddenTextField"
		
		Try 
			
			r_oTxtField.Width = r_ocboField.Width
			r_oTxtField.Height = r_ocboField.Height
			r_oTxtField.Left = r_ocboField.Left
			r_oTxtField.Top = r_ocboField.Top
		
		Catch excep As System.Exception
			
			
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Exit Sub
			
		End Try
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: SetupFields
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 02-07-2003 : workflow
	' ***************************************************************** '
	Private Function SetupFields() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetupFields"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			Select Case m_iTask
				Case gPMConstants.PMEComponentAction.PMEdit
					SwitchReadOnlyCombosForTextAlternatives(v_bReadOnlyCombos:=True)
					
				Case gPMConstants.PMEComponentAction.PMView
					SwitchReadOnlyCombosForTextAlternatives(v_bReadOnlyCombos:=True)
					
					
					
				Case gPMConstants.PMEComponentAction.PMAdd
					
					
			End Select
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SwitchReadOnlyCombosForTextAlternatives
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 02-07-2003 : workflow
	' ***************************************************************** '
	Private Sub SwitchReadOnlyCombosForTextAlternatives(ByVal v_bReadOnlyCombos As Boolean)
		
		Const sFunctionName As String = "SwitchReadOnlyCombosForTextAlternatives"
		
		Try 
			
			' size hidden fields
			SizeHiddenTextField(txtEventType, cboEventType)
			SizeHiddenTextField(txtEventSubject, cboEventSubject)
			SizeHiddenTextField(txtTaskGroup, cboTaskGroup)
			SizeHiddenTextField(txtTask, cboTask)
			SizeHiddenTextField(txtAllocateToUserGroup, cboUserGroup)
			SizeHiddenTextField(txtAllocateToUser, cboUser)
			SizeHiddenTextField(txtOutcome, cboOutcome)
			SizeHiddenTextField(txtActionType, cboActionType)
			
			' hide combos and replace with appropriate txt controls
			cboEventType.Visible = Not v_bReadOnlyCombos
			cboEventSubject.Visible = Not v_bReadOnlyCombos
			cboTaskGroup.Visible = Not v_bReadOnlyCombos
			cboTask.Visible = Not v_bReadOnlyCombos
			cboUserGroup.Visible = Not v_bReadOnlyCombos
			cboUser.Visible = Not v_bReadOnlyCombos
			
			txtEventType.Visible = v_bReadOnlyCombos
			txtEventSubject.Visible = v_bReadOnlyCombos
			txtTaskGroup.Visible = v_bReadOnlyCombos
			txtTask.Visible = v_bReadOnlyCombos
			txtAllocateToUserGroup.Visible = v_bReadOnlyCombos
			txtAllocateToUser.Visible = v_bReadOnlyCombos
			txtActionType.Visible = v_bReadOnlyCombos
			
			' disable text alternatives
			txtEventType.Enabled = False
			txtEventSubject.Enabled = False
			txtTaskGroup.Enabled = False
			txtTask.Enabled = False
			txtAllocateToUserGroup.Enabled = False
			txtAllocateToUser.Enabled = False
			txtActionType.Enabled = False
			
			If m_iTask = gPMConstants.PMEComponentAction.PMView Then
				cboOutcome.Visible = False
				txtOutcome.Visible = True
			End If
		
		Catch excep As System.Exception
			
			
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: ActionTaskLog
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 02-07-2003 : workflow
	' ***************************************************************** '
	Private Function ActionTaskLog() As Integer
		Dim result As Integer = 0
        Const sFunctionName As String = "ActionTaskLog"
		Dim oInterface As iPMWrkTaskInstLog.Interface_Renamed
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' create an instance of the task log component
			Dim temp_oInterface As Object
			If g_oObjectManager.GetInstance(temp_oInterface, sClassName:="iPMWrkTaskInstLog.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface) = gPMConstants.PMEReturnCode.PMTrue Then
				oInterface = temp_oInterface
				
				' set required properties

				oInterface.CallingAppName = ACApp

				oInterface.PMWrkTaskInstanceCnt = m_lPMWrkTaskInstanceCnt
				

				If oInterface.SetProcessModes(vTask:=m_iTask) = gPMConstants.PMEReturnCode.PMTrue Then
					

					If oInterface.Start <> gPMConstants.PMEReturnCode.PMTrue Then
						' Log Error.
						result = gPMConstants.PMEReturnCode.PMFalse
						
						LogMsg(v_sMsg:="Failed to start iPMWrkTaskInstLog.Interface", v_sMethod:=sFunctionName)
						
					End If
					
				Else
					
					' Log Error.
					result = gPMConstants.PMEReturnCode.PMFalse
					
					LogMsg(v_sMsg:="Failed to set process modes for iPMWrkTaskInstLog.Interface", v_sMethod:=sFunctionName)
					
				End If
				
			Else
				oInterface = temp_oInterface
				' Log Error.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				LogMsg(v_sMsg:="Failed to create instance of iPMWrkTaskInstLog.Interface", v_sMethod:=sFunctionName)
				
			End If
			
			
			
			oInterface = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetFormData
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 03-07-2003 : workflow
	' ***************************************************************** '
	Private Function GetFormData() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "GetFormData"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' set defaults
			m_dtTaskDueDate = #12/30/1899#
			m_dtTaskOutcomeDate = #12/30/1899#
			
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				
				' only get task related data if there is any to get
				If fraTask.Enabled Then
					
					'******************
					' Task Info
					If cboTaskGroup.SelectedIndex <> -1 Then
						m_lTaskGroupId = VB6.GetItemData(cboTaskGroup, cboTaskGroup.SelectedIndex)
					End If
					If cboTask.SelectedIndex <> -1 Then
						m_lTaskId = VB6.GetItemData(cboTask, cboTask.SelectedIndex)
					End If
					
					If cboActionType.SelectedIndex <> -1 Then
						m_lTaskActionTypeId = VB6.GetItemData(cboActionType, cboActionType.SelectedIndex)
					End If
					
					If cboUserGroup.SelectedIndex <> -1 Then
						m_lTaskAllocateToUserGroupId = VB6.GetItemData(cboUserGroup, cboUserGroup.SelectedIndex)
					End If
					
					If cboUser.SelectedIndex <> -1 Then
						m_lTaskAllocateToUserId = VB6.GetItemData(cboUser, cboUser.SelectedIndex)
					End If
					'******************
				End If
				
				'******************
				' Event Info
				If cboEventSubject.SelectedIndex <> -1 Then
					m_lEventlogsubjectId = VB6.GetItemData(cboEventSubject, cboEventSubject.SelectedIndex)
				End If
				
				If cboEventType.SelectedIndex <> -1 Then
					m_lEventType = VB6.GetItemData(cboEventType, cboEventType.SelectedIndex)
				End If
				'******************
				
			End If
			
			' data valid for both add and edit
			' only get task related data if there is any to get
			If fraTask.Enabled Then
				
				If chkTaskUrgent.CheckState = CheckState.Checked Then
					m_iTaskIsUrgent = gPMConstants.PMEReturnCode.PMTrue
				Else
					m_iTaskIsUrgent = gPMConstants.PMEReturnCode.PMFalse
				End If
				
				m_sTaskWorkflowInformation = txtWorkflow.Text
				
				
				m_sTaskDescription = txtTaskDescription.Text
				m_sTaskCustomer = txtClient.Text
				
				If chkComplete.CheckState = CheckState.Checked Then
					
					m_iTaskStatus = ACTaskStatusComplete
					
					If cboOutcome.SelectedIndex <> -1 Then
						m_lTaskOutcomeId = VB6.GetItemData(cboOutcome, cboOutcome.SelectedIndex)
					End If
					
					If Information.IsDate(txtOutcomeDate.Text) Then
						m_dtTaskOutcomeDate = CDate(txtOutcomeDate.Text)
						If Information.IsDate(txtOutcomeDateTime.Text) Then
							m_dtTaskOutcomeDate = m_dtTaskOutcomeDate.AddDays(CDate(txtOutcomeDateTime.Text).ToOADate())
						End If
					Else
						m_dtTaskDueDate = #12/30/1899#
					End If
					
				Else
					If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
						m_iTaskStatus = ACTaskStatusNew
					Else
						m_iTaskStatus = CInt(m_vTaskEvent(ACEventTask_TaskStatus, 0))
					End If
				End If
				
				If Information.IsDate(txtTaskDueDate.Text) Then
					m_dtTaskDueDate = CDate(txtTaskDueDate.Text)
					If Information.IsDate(txtTaskDueDateTime.Text) Then
						m_dtTaskDueDate = m_dtTaskDueDate.AddDays(CDate(txtTaskDueDateTime.Text).ToOADate())
					End If
				End If
				
			End If
			
			m_sEventDescription = txtEventDescription.Text
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			


			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetMandatoryFields
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 03-07-2003 : workflow
	' ***************************************************************** '
	Private Function SetMandatoryFields() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetMandatoryFields"
		
		Dim bTaskDataRequired As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lblEventType.Font = VB6.FontChangeBold(lblEventType.Font, True)
			
			bTaskDataRequired = cboTaskGroup.SelectedIndex <> -1
			
			lblTask.Font = VB6.FontChangeBold(lblTask.Font, bTaskDataRequired)
			lblTaskDueDate.Font = VB6.FontChangeBold(lblTaskDueDate.Font, bTaskDataRequired)
			lblAllocateToUserGroup.Font = VB6.FontChangeBold(lblAllocateToUserGroup.Font, bTaskDataRequired)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
End Class