Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles


Friend Partial Class frmDetails
	Inherits System.Windows.Forms.Form
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmDetails"
	
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
	
	Private m_oMaintainData As MaintainData
	Private m_lActionType As gPMConstants.PMEComponentAction
	Private m_lReturnType As Integer
	Private m_vCodes As Object
	
	'********************************
	'Process Mode Variables
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	'********************************
	
    Private m_vLookupDetails As Object
    Private m_vLookupTables As Object
	Private m_vPMUsers As Object
    Private m_vTaskGroupTask As Object
    Private m_vTaskGroupTaskAction As Object
	Private m_vTaskActionDueDays() As Object
    Private m_vPMUserGroupUsers As Object
    Private m_vTaskGroupUserGroups As Object
	Private m_vTaskActionTypeOutcomes As Object
	Private m_vTaskEvent As Object
    Private m_vWorkflowSteps As Object
	Private m_sPrevTaskGroup As String = ""
	Private m_sPrevTask As String = ""
	Private m_sPrevTaskAction As String = ""
	Private m_sPrevUserGroup As String = ""
	Private m_sPrevUser As String = ""
	Private m_bUseWorkTables As Boolean
    Private m_vValidUserBranches As Object
	
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
	' Data Arrays
	Public WriteOnly Property ValidUserBranches() As Object()
		Set(ByVal Value() As Object)
			m_vValidUserBranches = Value
		End Set
	End Property
	Public WriteOnly Property Codes() As Object
		Set(ByVal Value As Object)


			m_vCodes = Value
		End Set
	End Property
	Public WriteOnly Property WorkflowSteps() As Object()
		Set(ByVal Value() As Object)
			m_vWorkflowSteps = Value
		End Set
	End Property
	Public WriteOnly Property LookupDetails() As Object()
		Set(ByVal Value() As Object)
			m_vLookupDetails = Value
		End Set
	End Property
	Public WriteOnly Property LookupTables() As Object()
		Set(ByVal Value() As Object)
			m_vLookupTables = Value
		End Set
	End Property
	Public WriteOnly Property PMUsers() As Object
		Set(ByVal Value As Object)


			m_vPMUsers = Value
		End Set
	End Property
	Public WriteOnly Property TaskGroupTask() As Object()
		Set(ByVal Value() As Object)
			m_vTaskGroupTask = Value
		End Set
	End Property
	Public WriteOnly Property TaskGroupTaskAction() As Object()
		Set(ByVal Value() As Object)
			m_vTaskGroupTaskAction = Value
		End Set
	End Property
	Public WriteOnly Property PMUserGroupUsers() As Object()
		Set(ByVal Value() As Object)
			m_vPMUserGroupUsers = Value
		End Set
	End Property
	Public WriteOnly Property TaskGroupUserGroups() As Object()
		Set(ByVal Value() As Object)
			m_vTaskGroupUserGroups = Value
		End Set
	End Property
	Public WriteOnly Property TaskEvent() As Object
		Set(ByVal Value As Object)


			m_vTaskEvent = Value
		End Set
	End Property
	'***********************
	Public Property ActionType() As Integer
		Get
			Return m_lActionType
		End Get
		Set(ByVal Value As Integer)
			m_lActionType = Value
		End Set
	End Property
	'***********************
	
	Public Property MaintainData() As MaintainData
		Get
			Return m_oMaintainData
		End Get
		Set(ByVal Value As MaintainData)
			m_oMaintainData = Value
		End Set
	End Property
	'***********************
	Public ReadOnly Property ReturnType() As Integer
		Get
			Return m_lReturnType
		End Get
	End Property
	'***********************
	Public ReadOnly Property Error_Renamed() As Boolean
		Get
			Return m_bError
		End Get
	End Property
	'***********************
	
	
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
			
			Me.Text = GetResString(ACResDataDetailsTitle)
			tabMainTab.SelectedTab.Text = GetResString(ACResDataDetailsTabActionType)
			
			cmdOK.Text = GetResString(ACResDataDetailsButtonOK)
			cmdCancel.Text = GetResString(ACResDataDetailsButtonCancel)
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	Private Sub cboUser_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboUser.SelectedIndexChanged
		m_lReturn = SetupAllocateToFrame(ACUser)
		m_sPrevUser = cboUser.Text
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lReturnType = ACReturnCancel
		Me.Close()
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
			
			m_lReturn = SetMandatoryFields()
			
			m_lReturn = SetReadOnlyFields()
			
			m_lReturn = PopulateForm()
			
			m_lReturn = SetupButtons()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
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
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateForm() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateForm"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If PopulateLookups() <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = PopulateCbo(m_vWorkflowSteps, cboInTime)
			m_lReturn = PopulateCbo(m_vWorkflowSteps, cboOverdue)
			
			If m_lActionType = gPMConstants.PMEComponentAction.PMAdd Or m_lActionType = gPMConstants.PMEComponentAction.PMView Then
				
				txtCode.Text = m_oMaintainData.Code
				txtDescription.Text = m_oMaintainData.description
				txtEffectiveDate.Text = DateTimeHelper.ToString(m_oMaintainData.EffectiveDate)
				
				m_lReturn = SelectcboItem(cboTaskGroup, m_oMaintainData.TaskGroupId)
				m_lReturn = SelectcboItem(cboTask, m_oMaintainData.TaskId)
				m_lReturn = SelectcboItem(cboActionType, m_oMaintainData.TaskActionTypeid)
				
				m_lReturn = SelectcboItem(cboUserGroup, m_oMaintainData.PMUserGroupId)
				m_lReturn = SelectcboItem(cboUser, m_oMaintainData.UserId)
				
				m_lReturn = SelectcboItem(cboEventType, m_oMaintainData.EventTypeId)
				m_lReturn = SelectcboItem(cboEventSubject, m_oMaintainData.EventLogSubjectId)
				
				m_lReturn = SelectcboItem(cboOverdue, m_oMaintainData.OverdueNextWorkflowStepId)
				m_lReturn = SelectcboItem(cboInTime, m_oMaintainData.CompleteNextWorkflowStepId)
				
				m_lReturn = SelectcboItem(cboBranch, m_oMaintainData.BranchId)
				
				txtStepDaysDuration.Text = CStr(m_oMaintainData.StepDaysDuration)
				
				If m_oMaintainData.ExecutableTask Then
					chkExecutable.CheckState = CheckState.Checked
				Else
					chkExecutable.CheckState = CheckState.Unchecked
				End If
				
				txtEventDescription.Text = m_oMaintainData.EventDescription
				
				txtTaskDescription.Text = m_oMaintainData.TaskDescription
				
				If m_oMaintainData.IsUrgent Then
					chkTaskUrgent.CheckState = CheckState.Checked
				Else
					chkTaskUrgent.CheckState = CheckState.Unchecked
				End If
				
				txtClient.Text = m_oMaintainData.Customer
				txtWorkflow.Text = m_oMaintainData.Workflow
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: ActionOK
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003: workflow
	' ***************************************************************** '
	Private Function ActionOK() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ActionOK"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If ValidateMandatoryFields() = gPMConstants.PMEReturnCode.PMTrue Then
				
				If PopulateObject() = gPMConstants.PMEReturnCode.PMTrue Then
					m_lReturnType = ACReturnOk
					Me.Close()
				End If
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	
	
	' ***************************************************************** '
	' Name: PopulateCbo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 03-06-2003 : 223
	' ***************************************************************** '
	Private Function PopulateCbo(ByRef r_vDataArray( ,  ) As Object, ByRef r_oObject As ComboBox) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateCbo"
		
		Dim llBound, lUBound As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' clear down the combo
			r_oObject.Items.Clear()
			
			' if we have a populated array
			If Information.IsArray(r_vDataArray) Then
				
				' get array boundaries
				llBound = r_vDataArray.GetLowerBound(1)
				lUBound = r_vDataArray.GetUpperBound(1)
				
				' for each item in the array
				For lItem As Integer = llBound To lUBound
					
					' add the item to the specified combo

					r_oObject.Items.Add(CStr(r_vDataArray(ACDetailDesc, lItem)).Trim())

					VB6.SetItemData(r_oObject, lItem, CInt(r_vDataArray(ACDetailKey, lItem)))
					
				Next lItem
				
				' enable the combo
				r_oObject.Enabled = True
				
			Else
				' no data so disable the combo
				r_oObject.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetLookupItem
	'
	' Parameters: n/a
	'
	' Description: Returns all the item information for a specifed item
	'                  in a specified array
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (GetArrayItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetArrayItem(ByVal v_vArray( ,  ) As Object, ByVal r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "GetArrayItem"
		'
		'
		'Dim v_vLookupItem As String = ""
		'Dim lLookupItem, llBound, lUBound As Integer
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' set lookup properties
			'If r_lItemId <> 0 Then
				'v_vLookupItem = CStr(r_lItemId)
				'lLookupItem = 0
				'
			'ElseIf r_sItemDesc <> "" Then 
				'v_vLookupItem = r_sItemDesc
				'lLookupItem = 1
				'
			'ElseIf r_sItemCode <> "" Then 
				'v_vLookupItem = r_sItemCode
				'lLookupItem = 2
			'End If
			'
			'
			'llBound = v_vArray.GetLowerBound(1)
			'lUBound = v_vArray.GetUpperBound(1)
			'
			' loop around the available items in the specified array
			'For 'lItem As Integer = llBound To lUBound
				'
				' look for a match

				'If CStr(v_vArray(lLookupItem, lItem)).Trim() = v_vLookupItem Then
					'
					' return the requested code, id, description

					'r_sItemDesc = CStr(v_vArray(ACDetailDesc, lItem)).Trim()

					'r_sItemCode = CStr(v_vArray(ACDetailCode, lItem)).Trim()

					'r_lItemId = CInt(CStr(v_vArray(ACDetailKey, lItem)).Trim())
					'
					'Exit For
				'End If
				'
			'Next lItem
			'
			' if we dont find the values specified then return false
			'If r_sItemCode = "" Then
				'
				'result = gPMConstants.PMEReturnCode.PMFalse
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
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result


			'Return result
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name: PopulateObject
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Private Function PopulateObject() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateObject"
		
		Dim sMessageTitle, sMessage As String
		Dim bError As Boolean
		Dim sCode As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' set the default message title
			sMessageTitle = GetResString(ACResDataDetailsTitle)
			
			' Code
			' if we are in add action then
			If m_lActionType = gPMConstants.PMEComponentAction.PMEdit Then

				If ItemInArray(r_vArray:=m_vCodes, v_sItemValue:=txtCode.Text) <> gPMConstants.PMEReturnCode.PMTrue Then
					sMessage = GetResString(ACResDataDetailsMessageInvalidCode)
					MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
					txtCode.Focus()
					bError = True
				End If
			End If
			
			If Not bError Then
				m_oMaintainData.Code = txtCode.Text
			End If
			
			' Description
			If Not bError Then
				m_oMaintainData.description = txtDescription.Text
			End If
			
			' Effective Date
			If Not bError Then
				If Information.IsDate(txtEffectiveDate.Text) Then
					m_oMaintainData.EffectiveDate = CDate(txtEffectiveDate.Text)
				Else
					sMessage = GetResString(ACResDataDetailsMessageInvalidDate)
					MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
					txtEffectiveDate.Focus()
					bError = True
				End If
			End If
			
			If Not bError Then
				If cboTaskGroup.SelectedIndex <> -1 Then
					m_oMaintainData.TaskGroupId = VB6.GetItemData(cboTaskGroup, cboTaskGroup.SelectedIndex)
				Else
					m_oMaintainData.TaskGroupId = -1
				End If
			End If
			
			If Not bError Then
				If cboTask.SelectedIndex <> -1 Then
					m_oMaintainData.TaskId = VB6.GetItemData(cboTask, cboTask.SelectedIndex)
				Else
					m_oMaintainData.TaskId = -1
				End If
			End If
			
			If Not bError Then
				If cboActionType.SelectedIndex <> -1 Then
					m_oMaintainData.TaskActionTypeid = VB6.GetItemData(cboActionType, cboActionType.SelectedIndex)
				Else
					m_oMaintainData.TaskActionTypeid = -1
				End If
			End If
			
			If Not bError Then
				
				Dim dbNumericTemp As Double
				If Double.TryParse(txtStepDaysDuration.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
					If CDbl(txtStepDaysDuration.Text) < 0 Or CDbl(txtStepDaysDuration.Text) > 365 Then
						sMessage = GetResString(ACResDataDetailsMessageInvalidStepDaysDuration)
						MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
						txtStepDaysDuration.Focus()
						bError = True
					End If
				Else
					sMessage = GetResString(ACResDataDetailsMessageInvalidStepDaysDuration)
					MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
					txtStepDaysDuration.Focus()
					bError = True
				End If
				
				m_oMaintainData.StepDaysDuration = CInt(txtStepDaysDuration.Text)
				
			End If
			
			If Not bError Then
				m_oMaintainData.ExecutableTask = (chkExecutable.CheckState = CheckState.Checked)
			End If
			
			If Not bError Then
				If cboUserGroup.SelectedIndex <> -1 Then
					m_oMaintainData.PMUserGroupId = VB6.GetItemData(cboUserGroup, cboUserGroup.SelectedIndex)
				Else
					m_oMaintainData.PMUserGroupId = -1
				End If
			End If
			
			If Not bError Then
				If cboUser.SelectedIndex <> -1 Then
					m_oMaintainData.UserId = VB6.GetItemData(cboUser, cboUser.SelectedIndex)
				Else
					m_oMaintainData.UserId = -1
				End If
			End If
			
			If Not bError Then
				If cboEventType.SelectedIndex <> -1 Then
					m_oMaintainData.EventTypeId = VB6.GetItemData(cboEventType, cboEventType.SelectedIndex)
				Else
					m_oMaintainData.EventTypeId = -1
				End If
			End If
			
			If Not bError Then
				If cboEventSubject.SelectedIndex <> -1 Then
					m_oMaintainData.EventLogSubjectId = VB6.GetItemData(cboEventSubject, cboEventSubject.SelectedIndex)
				Else
					m_oMaintainData.EventLogSubjectId = -1
				End If
			End If
			
			If Not bError Then
				m_oMaintainData.EventDescription = txtEventDescription.Text
			End If
			
			If Not bError Then
				m_oMaintainData.TaskDescription = txtTaskDescription.Text
			End If
			
			If Not bError Then
				If cboInTime.SelectedIndex <> -1 Then
					m_oMaintainData.CompleteNextWorkflowStepId = VB6.GetItemData(cboInTime, cboInTime.SelectedIndex)
				Else
					m_oMaintainData.CompleteNextWorkflowStepId = -1
				End If
			End If
			
			If Not bError Then
				If cboOverdue.SelectedIndex <> -1 Then
					m_oMaintainData.OverdueNextWorkflowStepId = VB6.GetItemData(cboOverdue, cboOverdue.SelectedIndex)
				Else
					m_oMaintainData.OverdueNextWorkflowStepId = -1
				End If
			End If
			
			If Not bError Then
				m_oMaintainData.IsUrgent = (chkTaskUrgent.CheckState = CheckState.Checked)
			End If
			
			If Not bError Then
				m_oMaintainData.Customer = txtClient.Text
			End If
			
			If Not bError Then
				m_oMaintainData.Workflow = txtWorkflow.Text
			End If
			
			If Not bError Then
				If cboBranch.SelectedIndex <> -1 Then
					m_oMaintainData.BranchId = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
				Else
					m_oMaintainData.BranchId = -1
				End If
			End If
			
			If m_lActionType = gPMConstants.PMEComponentAction.PMEdit Then
				' copies the current functional values to the original properties
				' so comparisons can determine if any values have been updated
				m_lReturn = m_oMaintainData.CopyToOriginalData()
			End If
			
			If bError Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


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
	Private Function ConvertFromNullValues(ByRef r_vValue As gPMConstants.PMEReturnCode, ByVal v_iDataType As Integer) As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Const sFunctionName As String = "ConvertFromNullValues"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			
			Select Case v_iDataType
				Case gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMCurrency, gPMConstants.PMEDataType.PMBoolean, gPMConstants.PMEDataType.PMDate

                    '--TODO check at runtime
                    If r_vValue Or r_vValue = "" Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    Else
                        Return r_vValue
                    End If
					
				Case Else
					Return r_vValue
					
			End Select
		
		Catch 
		End Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		'******************************
		' Log Error.
        gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
		'*******************************
		
		Return result
		
	End Function
	
	' ***************************************************************** '
	' Name:SetMandatoryFields
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Private Function SetMandatoryFields() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetMandatoryFields"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lblCode.Font = VB6.FontChangeBold(lblCode.Font, True)
			lblDescription.Font = VB6.FontChangeBold(lblDescription.Font, True)
			lblEffectiveDate.Font = VB6.FontChangeBold(lblEffectiveDate.Font, True)
			lblTaskGroup.Font = VB6.FontChangeBold(lblTaskGroup.Font, True)
			lblTask.Font = VB6.FontChangeBold(lblTask.Font, True)
			lblAllocateToUserGroup.Font = VB6.FontChangeBold(lblAllocateToUserGroup.Font, True)
			lblStepDaysDuration.Font = VB6.FontChangeBold(lblStepDaysDuration.Font, True)
			lblInTimeStep.Font = VB6.FontChangeBold(lblInTimeStep.Font, True)
			lblOverDueStep.Font = VB6.FontChangeBold(lblOverDueStep.Font, True)
			lblEventType.Font = VB6.FontChangeBold(lblEventType.Font, True)
			lblEventSubject.Font = VB6.FontChangeBold(lblEventSubject.Font, True)
			lblBranch.Font = VB6.FontChangeBold(lblBranch.Font, True)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: SetReadOnlyFields
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Private Function SetReadOnlyFields() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetReadOnlyFields"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			Select Case m_lActionType
				Case ACActionEdit
					txtCode.Enabled = False
					
				Case ACActionView
					txtCode.Enabled = False
					txtDescription.Enabled = False
					txtEffectiveDate.Enabled = False
					
					
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ItemInArray
	'
	' Parameters: n/a
	'
	' Description: Checks the array to see if the specified item
	'                 already exists
	' History:
	'           Created : MEvans : 03-06-2003 : 223
	' ***************************************************************** '
	Private Function ItemInArray(ByRef r_vArray() As Object, ByVal v_sItemValue As String) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ItemInArray"
		
		Dim llBound, lUBound As Integer
		Dim bItemExists As Boolean
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' create new collection
			If Information.IsArray(r_vArray) Then
				
				' get array boundaries
				llBound = r_vArray.GetLowerBound(0)
				lUBound = r_vArray.GetUpperBound(0)
				
				' for each item in the array
				For lItem As Integer = llBound To lUBound
					
					' check if we match the values from the array specified

					If CStr(r_vArray(lItem)).Trim() = v_sItemValue.Trim() Then
						' indicate item already exists
						bItemExists = True
						
						' quit loop
						lItem = lUBound
					End If
					
				Next lItem
				
			End If
			
			' if the item doesnt already exist
			' add it to the array
			If bItemExists Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	
	Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
		If Information.IsDate(txtEffectiveDate.Text) Then
			txtEffectiveDate.Text = CDate(txtEffectiveDate.Text).ToString("dd/MM/yyyy")
		End If
	End Sub
	
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
		m_lReturn = SetupAllocateToFrame(ACUserGroup)
		m_sPrevUserGroup = cboUserGroup.Text
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lReturn = ActionOK()
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
                gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & "Failed to create business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=New Exception(Information.Err().Description))
				
				' reset mouse pointer
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				
			Else
				m_oBusiness = temp_m_oBusiness
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=excep)
			
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
	Private Sub frmDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
	Private Sub frmDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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

	Private Sub frmDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Exit Sub
			
		End Try
		
	End Sub
	
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
	'UPGRADE_NOTE: (7001) The following declaration (LogMsg) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub LogMsg(ByVal v_sMsg As String, ByVal v_sMethod As String)
		'
		'Const sFunctionName As String = "LogMsg"
		'
		'Try 
			'
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=v_sMethod & ":" & v_sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sMethod)
		'
		'Catch excep As System.Exception
			'
			'
			'
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'******************************
			'
			'Exit Sub
			'
		'End Try
		'
	'End Sub
	
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
			
			If m_lActionType = gPMConstants.PMEComponentAction.PMView Then
				cmdOK.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
	'UPGRADE_NOTE: (7001) The following declaration (ActionCancel) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ActionCancel() As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "ActionCancel"
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'm_lStatus = gPMConstants.PMEReturnCode.PMCancel
			'
			'Me.Close()
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result
			'
		'End Try
	'End Function
	
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
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
			
			SetupAllocateToFrame(ACUserGroup)
			
			If m_lActionType = gPMConstants.PMEComponentAction.PMView Then
				
				fraWorkflowStep.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
				
				'        ' determine if fratask should be enabled
				'        If cboEventType.Text <> "" Then
				'
				'            ' get event type code
				'            If GetLookupItem(v_sLookupTable:=ACLookupTableEventType, _
				''                             r_sItemDesc:=cboEventType, _
				''                             r_sItemCode:=sEventTypeCode, _
				''                             r_lItemId:=0) = PMTrue Then
				'
				'                ' if event type code is task related
				'                If sEventTypeCode = ACEventTypeCodeClaimDebtTask Or _
				''                    sEventTypeCode = ACEventTypeCodeClientTask Or _
				''                     sEventTypeCode = ACEventTypeCodePolicyTask Or _
				''                      sEventTypeCode = ACEventTypeCodeClaimTask Then
				'
				'                     fraTask.Enabled = True
				'
				'                End If
				'
				'            End If
				'        End If
				
				' reset the task frame
				'        If fraTask.Enabled = False Then
				'
				'            cboTaskGroup.ListIndex = -1
				'            SetupTaskCombos (ACTaskGroup)
				'
				'            m_sPrevTaskAction = ""
				'            m_sPrevTask = ""
				'            m_sPrevTaskGroup = ""
				'
				'        End If
				'
			ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then 
				
				'        If txtTaskGroup <> "" Then
				'            fraTask.Enabled = True
				'        End If
			End If
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
            'Const ACValueID As Integer = 1             ''Unused Local Variable
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
				gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find code for lookuptable:" & v_sLookupTable & "and lookup Item:" & v_vLookupItem, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:= New Exception(Information.Err().Description))
				'*******************************
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
			If cboTaskGroup.Text.Trim() <> "" Then
				m_lReturn = GetLookupItem(v_sLookupTable:=ACLookupTablePMWrkTaskGroup, r_sItemDesc:=cboTaskGroup.Text.Trim(), r_sItemCode:="", r_lItemId:=lTaskGroupId)
			End If
			
			' get task id
			If cboTask.Text.Trim() <> "" Then
				m_lReturn = GetLookupItem(v_sLookupTable:=ACLookupTablePMWrkTask, r_sItemDesc:=cboTask.Text.Trim(), r_sItemCode:="", r_lItemId:=lTaskId)
			End If
			
			' get task action type id
			If cboActionType.Text.Trim() <> "" Then
				m_lReturn = GetLookupItem(v_sLookupTable:=ACLookupTablePMWrkTaskActionType, r_sItemDesc:=cboActionType.Text.Trim(), r_sItemCode:="", r_lItemId:=lTaskActionTypeId)
			End If
			
			
			Select Case v_lItemChangedLevel
				Case ACTaskGroup
					
					' if a new item has been selected
					If m_sPrevTaskGroup <> cboTaskGroup.Text Then
						
						If cboTaskGroup.Text.Trim() <> "" Then
							
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
							m_sPrevTaskAction = ""
							
						End If
						
					End If
					
				Case ACTask
					
					' if a new item has been selected
					If m_sPrevTask <> cboTask.Text Then
						If cboTask.Text.Trim() <> "" Then
							If PopulateTaskActionTypeCbo(v_lTaskGroupId:=lTaskGroupId, v_lTaskId:=lTaskId) <> gPMConstants.PMEReturnCode.PMTrue Then
								
								result = gPMConstants.PMEReturnCode.PMFalse
							End If
						Else
							' clear down any selected task action types
							
							' task related
							cboActionType.Items.Clear()
							cboActionType.Enabled = False
							
							' action type related
							m_sPrevTaskAction = ""
							
						End If
					End If
					
				Case ACTaskAction
					
					' if the task action type has actually changed
					If m_sPrevTaskAction <> cboActionType.Text Then
						
						' if a task action type has been selected
						If cboActionType.Text.Trim() <> "" Then
							If Information.IsArray(m_vTaskActionDueDays) Then
								
								' default the number of step days
								txtStepDaysDuration.Text = CStr(m_vTaskActionDueDays(cboActionType.SelectedIndex))
								
							End If
							
						Else
							' otherwise specify 0 days..
							txtStepDaysDuration.Text = "0"
						End If
						
					End If
					
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Interface
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
						lTaskActionDueDays = ConvertFromNullValues(m_vTaskGroupTaskAction(ACTaskGroupTaskActionDueDays, lItem), gPMConstants.PMEDataType.PMLong)
						
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


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
	Private Function SetupAllocateToFrame(ByVal v_lItemChangedLevel As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SetupAllocateToFrame"
		
		Dim lUserGroupId, lUser As Integer
		Dim sUser As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If fraTask.Enabled Then
				
				
				Select Case v_lItemChangedLevel
					Case ACUserGroup
						
						If cboUserGroup.Text <> "" Then
							
							If m_sPrevUserGroup <> cboUserGroup.Text Then
								
								If GetLookupItem(v_sLookupTable:=ACLookupTablePMUserGroup, r_sItemDesc:=cboUserGroup.Text, r_sItemCode:="", r_lItemId:=lUserGroupId) = gPMConstants.PMEReturnCode.PMTrue Then
									
									If Information.IsArray(m_vPMUserGroupUsers) Then
										
										If PopulateAllocateToUserCombo(v_lPMUserGroupId:=lUserGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
											result = gPMConstants.PMEReturnCode.PMFalse
										End If
										
									End If
									
									If Information.IsArray(m_vValidUserBranches) Then
										If PopulateBranchcbo(v_lUserGroupId:=lUserGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
											result = gPMConstants.PMEReturnCode.PMFalse
										End If
									End If
									
								End If
								
							End If
							
						Else
							cboUser.SelectedIndex = -1
							cboUser.Enabled = False
						End If
						
					Case ACUser
						
						If m_sPrevUser <> cboUser.Text Then
							
							' get user group and user id
							lUserGroupId = VB6.GetItemData(cboUserGroup, cboUserGroup.SelectedIndex)
							lUser = VB6.GetItemData(cboUser, cboUser.SelectedIndex)
							
							If lUser = 0 Then
								sUser = ""
							Else
								sUser = CStr(lUser)
							End If
							
							If Information.IsArray(m_vValidUserBranches) Then
								If PopulateBranchcbo(v_lUserGroupId:=lUserGroupId, v_sUser:=sUser) <> gPMConstants.PMEReturnCode.PMTrue Then
									result = gPMConstants.PMEReturnCode.PMFalse
								End If
							End If
							
						End If
						
				End Select
				
			Else
				cboUserGroup.SelectedIndex = -1
				cboUser.SelectedIndex = -1
				cboUser.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
		Dim sUserName As String = ""
		
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
						sUserName = CStr(m_vPMUserGroupUsers(ACPMUsername, lItem))
						
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
							cboUser.Items.Insert(lIndex, sUserName)
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
	'UPGRADE_NOTE: (7001) The following declaration (ValidateChecks) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ValidateChecks() As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "ValidateChecks"
		'
		'Dim bValidationFailed As Boolean
		'Dim sMessage, sMessageTitle As String
		'Dim dtTaskDueDate As Date
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'bValidationFailed = False
			'
			'sMessageTitle = GetResString(ACResDataInterfaceTitle)
			'
			'If bValidationFailed Then
				'result = gPMConstants.PMEReturnCode.PMFalse
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
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result
			'
		'End Try
	'End Function
	
	
	
	
	
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
			
			m_sPrevUserGroup = ""
			
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
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
	'UPGRADE_NOTE: (7001) The following declaration (SetupFields) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function SetupFields() As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "SetupFields"
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'
			'Select Case m_iTask
				'Case gPMConstants.PMEComponentAction.PMEdit
					'SwitchReadOnlyCombosForTextAlternatives(v_bReadOnlyCombos:=True)
					'
				'Case gPMConstants.PMEComponentAction.PMView
					'SwitchReadOnlyCombosForTextAlternatives(v_bReadOnlyCombos:=True)
					'
					'
					'
				'Case gPMConstants.PMEComponentAction.PMAdd
					'
					'
			'End Select
			'
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result
			'
		'End Try
	'End Function
	
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
		
		Catch excep As System.Exception
			
			
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Exit Sub
			
		End Try
		
	End Sub
	
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
	'UPGRADE_NOTE: (7001) The following declaration (GetFormData) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetFormData() As Integer
		'
		'Dim result As Integer = 0
		'Const sFunctionName As String = "GetFormData"
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				'
				' only get task related data if there is any to get
				'If fraTask.Enabled Then
					'
					'******************
					' Task Info
					'If cboTaskGroup.SelectedIndex <> -1 Then
						'm_lTaskGroupId = VB6.GetItemData(cboTaskGroup, cboTaskGroup.SelectedIndex)
					'End If
					'If cboTask.SelectedIndex <> -1 Then
						'm_lTaskId = VB6.GetItemData(cboTask, cboTask.SelectedIndex)
					'End If
					'
					'If cboActionType.SelectedIndex <> -1 Then
						'm_lTaskActionTypeId = VB6.GetItemData(cboActionType, cboActionType.SelectedIndex)
					'End If
					'
					'If cboUserGroup.SelectedIndex <> -1 Then
						'm_lTaskAllocateToUserGroupId = VB6.GetItemData(cboUserGroup, cboUserGroup.SelectedIndex)
					'End If
					'
					'If cboUser.SelectedIndex <> -1 Then
						'm_lTaskAllocateToUserId = VB6.GetItemData(cboUser, cboUser.SelectedIndex)
					'End If
					'******************
				'End If
				'
				'******************
				' Event Info
				'If cboEventSubject.SelectedIndex <> -1 Then
					'm_lEventlogsubjectId = VB6.GetItemData(cboEventSubject, cboEventSubject.SelectedIndex)
				'End If
				'
				'If cboEventType.SelectedIndex <> -1 Then
					'm_lEventType = VB6.GetItemData(cboEventType, cboEventType.SelectedIndex)
				'End If
				'******************
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
			'******************************
			' Log Error.
			'gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
			'*******************************
			'
			'Return result
			'


			'Return result
		'End Try
	'End Function
	
	' ***************************************************************** '
	' Name: SelectcboItem
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 03-10-2003 : 229
	' ***************************************************************** '
	Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "SelectcboItem"
		
		Dim bItemNotFound As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' if the item id is valid
			If v_lSelectedId <> -1 Then
				
				' if there are some list items to select from
				If r_oCbo.Items.Count <> 0 Then
					
					bItemNotFound = True
					
					' for each item in the list
					For lItem As Integer = 0 To r_oCbo.Items.Count
						' search the item data array for a match
						If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then
							
							' found a match - select the item
							r_oCbo.SelectedIndex = lItem
							bItemNotFound = False
							Exit For
						End If
						
					Next lItem
				End If
			End If
			
			If bItemNotFound Then
				
				' log that we havent found the specified item
				gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find item with id:" & CStr(v_lSelectedId) &  _
				                              " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: ValidateMandatoryFields
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 03-10-2003 : 229
	' ***************************************************************** '
	Private Function ValidateMandatoryFields() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "ValidateMandatoryFields"
		
		Dim bValid As Boolean
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			bValid = True
			
			If bValid Then
				bValid = MandatoryFieldPopulated(txtCode)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(txtDescription)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(txtEffectiveDate)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(cboTaskGroup)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(cboTask)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(txtStepDaysDuration)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(cboUserGroup)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(cboBranch)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(cboEventType)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(cboEventSubject)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(cboInTime)
			End If
			
			If bValid Then
				bValid = MandatoryFieldPopulated(cboOverdue)
			End If
			
			If Not bValid Then
				
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: MandatoryFieldPopulated
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 03-10-2003 : 229
	' ***************************************************************** '
	Private Function MandatoryFieldPopulated(ByRef r_oControl As Control) As Boolean
		
		Dim result As Boolean = False
		Const sFunctionName As String = "MandatoryFieldPopulated"
		
		Dim bReturn As Boolean
		Dim sMessage, sMessageTitle As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			bReturn = True
			
			If TypeOf r_oControl Is ComboBox Then
				If CType(r_oControl, ComboBox).SelectedIndex = -1 Then
					bReturn = False
					If r_oControl.Enabled Then
						r_oControl.Focus()
					End If
				End If
			End If
			
			If TypeOf r_oControl Is TextBox Then
				If CType(r_oControl, TextBox).Text.Trim() = "" Then
					bReturn = False
				End If
			End If
			
			If Not bReturn Then
				
				sMessageTitle = GetResString(ACMessageTitleMandatoryFieldChecks)
				sMessage = GetResString(ACMessageMandatoryChecksFailed)
				MessageBox.Show(sMessage, sMessageTitle, MessageBoxButtons.OK)
				
				If ControlHelper.GetEnabled(r_oControl) Then
					r_oControl.Focus()
				End If
				
			End If
			
			
			Return bReturn
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result


			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: PopulateBranchcbo
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 20-10-2003 : Continuation Tasks
	' ***************************************************************** '
	Private Function PopulateBranchcbo(ByVal v_lUserGroupId As Integer, Optional ByVal v_sUser As String = "") As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "PopulateBranchcbo"
		
		Dim llBound, lUBound, lIndex As Integer
		
		Dim lUserGroupId As Integer
		Dim sUserId, sBranch As String
		Dim lBranchId As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' initialisation
			cboBranch.Items.Clear()
			
			lIndex = 0
			
			' if we have a selected user group
			If cboUserGroup.Text <> "" Then
				
				' if we have an array of task group branches
				If Information.IsArray(m_vValidUserBranches) Then
					
					' get array boundaries
					llBound = m_vValidUserBranches.GetLowerBound(1)
					lUBound = m_vValidUserBranches.GetUpperBound(1)
					
					' for each item in the array
					For lItem As Integer = llBound To lUBound
						
						' get item details
						lUserGroupId = CInt(m_vValidUserBranches(ACValidBranchUserGroupId, lItem))
						sUserId = CStr(m_vValidUserBranches(ACValidBranchUserId, lItem))
						lBranchId = CInt(m_vValidUserBranches(ACValidBranchBranchId, lItem))
						sBranch = CStr(m_vValidUserBranches(ACValidBranchBranchDescription, lItem)).Trim()
						
						' if user group matches the selected user group
						If lUserGroupId = v_lUserGroupId Then
							
							If v_sUser <> "" Then
								
								' match on a specific user as well as the user group
								If v_sUser = sUserId Then
									
									' add branch to combo
									cboBranch.Items.Insert(lIndex, sBranch)
									VB6.SetItemData(cboBranch, lIndex, lBranchId)
									lIndex += 1
								End If
								
							Else
								
								' check if branch already added
								' as it could have been present on multiple users
								If Not (cboItemExists(cboBranch, lBranchId) = gPMConstants.PMEReturnCode.PMTrue) Then
									
									' add task to combo
									cboBranch.Items.Insert(lIndex, sBranch)
									VB6.SetItemData(cboBranch, lIndex, lBranchId)
									lIndex += 1
								End If
								
							End If
							
						End If
						
					Next lItem
					
				End If
				
			End If
			
			cboBranch.Enabled = Not (lIndex = 0)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: cboItemExists
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 20-10-2003 : Continuations Tasks
	' ***************************************************************** '
	Private Function cboItemExists(ByVal v_oControl As ComboBox, ByVal v_lItemId As Integer) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "cboItemExists"
		
		Dim bFound As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			bFound = False
			
			For lItem As Integer = 1 To v_oControl.Items.Count
				If VB6.GetItemData(v_oControl, lItem - 1) = v_lItemId Then
					bFound = True
					Exit For
				End If
			Next 
			
			If Not bFound Then
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			'******************************
			' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
			'*******************************
			
			Return result
			
		End Try
	End Function
End Class
