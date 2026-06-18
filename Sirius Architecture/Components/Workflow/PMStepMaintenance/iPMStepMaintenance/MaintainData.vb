Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles


Friend NotInheritable Class MaintainData 
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MaintainData"
	
	'***************************
	'*** Default Properties ****
	Private m_lId As Integer
	Private m_sCode As String = ""
	Private m_sDescription As String = ""
	Private m_bIsDeleted As Boolean
	Private m_dtEffectiveDate As Date
	Private m_bItemIsLive As Boolean
	Private m_bSaveOriginal As Boolean
	Private m_bItemUpdated As Boolean
	'***************************
	
	'*** custom properties ****
	Private m_lWorkflowId As Integer
	Private m_lStepOrder As Integer
	Private m_lTaskGroupId As Integer
	Private m_lTaskId As Integer
	Private m_lPMUserGroupId As Integer
	Private m_lUserid As Integer
	Private m_lStepDaysDuration As Integer
	Private m_lCompleteNextWorkflowStepId As Integer
	Private m_lOverduenextWorkflowStepId As Integer
	Private m_bExecutableTask As Boolean
	Private m_lTaskActionTypeId As Integer
	Private m_lEventTypeId As Integer
	Private m_sEventDescription As String = ""
	Private m_sTaskDescription As String = ""
	Private m_lEventlogsubjectId As Integer
	Private m_lReplacedItemId As Integer
	Private m_bIsUrgent As Boolean
	Private m_sCustomer As String = ""
	Private m_sWorkflow As String = ""
	Private m_lBranchId As Integer
	
	'**********************
	' Original Properties
	'**********************
	'*** Default Properties ****
	Private m_lOrigId As Integer
	Private m_sOrigCode As String = ""
	Private m_sOrigDescription As String = ""
	Private m_bOrigIsDeleted As Boolean
	Private m_dtOrigEffectiveDate As Date
	
	'*** custom properties ****
	Private m_lOrigWorkflowId As Integer
	Private m_lOrigStepOrder As Integer
	Private m_sOrigStepDescription As String = ""
	Private m_lOrigTaskGroupId As Integer
	Private m_lOrigTaskId As Integer
	Private m_lOrigPMUserGroupId As Integer
	Private m_lOrigUserid As Integer
	Private m_lOrigStepDaysDuration As Integer
	Private m_lOrigCompleteNextWorkflowStepId As Integer
	Private m_lOrigOverduenextWorkflowStepId As Integer
	Private m_bOrigExecutableTask As Boolean
	Private m_lOrigTaskActionTypeid As Integer
	Private m_lOrigEventTypeId As Integer
	Private m_sOrigEventDescription As String = ""
	Private m_sOrigTaskDescription As String = ""
	Private m_lOrigEventLogSubjectId As Integer
	Private m_bUpdateThisItem As Boolean
	Private m_bOrigIsUrgent As Boolean
	Private m_sOrigCustomer As String = ""
	Private m_sOrigWorkflow As String = ""
	Private m_lOrigBranchId As Integer
	
	'**********************
	
	'***************************
	'*** Default Properties ****
	'***************************
	
	
	
	Public Property Id() As Integer
		Get
			Return m_lId
		End Get
		Set(ByVal Value As Integer)
			m_lId = Value
		End Set
	End Property
	'************************
	
	Public Property Code() As String
		Get
			Return m_sCode
		End Get
		Set(ByVal Value As String)
			m_sCode = Value
		End Set
	End Property
	'************************
	Public Property description() As String
		Get
			Return m_sDescription
		End Get
		Set(ByVal Value As String)
			m_sDescription = Value
		End Set
	End Property
	'************************
	
	Public Property IsDeleted() As Boolean
		Get
			Return m_bIsDeleted
		End Get
		Set(ByVal Value As Boolean)
			m_bIsDeleted = Value
		End Set
	End Property
	' ************************
	Public Property EffectiveDate() As Date
		Get
			Return m_dtEffectiveDate
		End Get
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	
	'**************************
	'*** custom properties ****
	Public Property BranchId() As Integer
		Get
			Return m_lBranchId
		End Get
		Set(ByVal Value As Integer)
			m_lBranchId = Value
		End Set
	End Property
	'**************************
	Public Property UpdateThisItem() As Boolean
		Get
			Return m_bUpdateThisItem
		End Get
		Set(ByVal Value As Boolean)
			m_bUpdateThisItem = Value
		End Set
	End Property
	'**************************
	Public Property ReplacedItemId() As Integer
		Get
			Return m_lReplacedItemId
		End Get
		Set(ByVal Value As Integer)
			m_lReplacedItemId = Value
		End Set
	End Property
	'**************************
	Public Property TaskDescription() As String
		Get
			Return m_sTaskDescription
		End Get
		Set(ByVal Value As String)
			m_sTaskDescription = Value
		End Set
	End Property
	'**************************
	Public Property EventDescription() As String
		Get
			Return m_sEventDescription
		End Get
		Set(ByVal Value As String)
			m_sEventDescription = Value
		End Set
	End Property
	'**********************
	Public Property ExecutableTask() As Boolean
		Get
			Return m_bExecutableTask
		End Get
		Set(ByVal Value As Boolean)
			m_bExecutableTask = Value
		End Set
	End Property
	'**********************
	Public Property EventTypeId() As Integer
		Get
			Return m_lEventTypeId
		End Get
		Set(ByVal Value As Integer)
			m_lEventTypeId = Value
		End Set
	End Property
	'**********************
	Public Property EventLogSubjectId() As Integer
		Get
			Return m_lEventlogsubjectId
		End Get
		Set(ByVal Value As Integer)
			m_lEventlogsubjectId = Value
		End Set
	End Property
	'**********************
	Public Property TaskActionTypeid() As Integer
		Get
			Return m_lTaskActionTypeId
		End Get
		Set(ByVal Value As Integer)
			m_lTaskActionTypeId = Value
		End Set
	End Property
	'**********************
	Public Property OverdueNextWorkflowStepId() As Integer
		Get
			Return m_lOverduenextWorkflowStepId
		End Get
		Set(ByVal Value As Integer)
			m_lOverduenextWorkflowStepId = Value
		End Set
	End Property
	'**********************
	Public Property CompleteNextWorkflowStepId() As Integer
		Get
			Return m_lCompleteNextWorkflowStepId
		End Get
		Set(ByVal Value As Integer)
			m_lCompleteNextWorkflowStepId = Value
		End Set
	End Property
	'**********************
	Public Property StepDaysDuration() As Integer
		Get
			Return m_lStepDaysDuration
		End Get
		Set(ByVal Value As Integer)
			m_lStepDaysDuration = Value
		End Set
	End Property
	'**********************
	Public Property UserId() As Integer
		Get
			Return m_lUserid
		End Get
		Set(ByVal Value As Integer)
			m_lUserid = Value
		End Set
	End Property
	'**********************
	Public Property PMUserGroupId() As Integer
		Get
			Return m_lPMUserGroupId
		End Get
		Set(ByVal Value As Integer)
			m_lPMUserGroupId = Value
		End Set
	End Property
	'**********************
	Public Property WorkflowId() As Integer
		Get
			Return m_lWorkflowId
		End Get
		Set(ByVal Value As Integer)
			m_lWorkflowId = Value
		End Set
	End Property
	'**********************
	Public Property StepOrder() As Integer
		Get
			Return m_lStepOrder
		End Get
		Set(ByVal Value As Integer)
			m_lStepOrder = Value
		End Set
	End Property
	'**********************
	Public Property TaskGroupId() As Integer
		Get
			Return m_lTaskGroupId
		End Get
		Set(ByVal Value As Integer)
			m_lTaskGroupId = Value
		End Set
	End Property
	'**********************
	Public Property TaskId() As Integer
		Get
			Return m_lTaskId
		End Get
		Set(ByVal Value As Integer)
			m_lTaskId = Value
		End Set
	End Property
	'************************
	'************************
	
	Public Property ItemIsLive() As Boolean
		Get
			Return m_bItemIsLive
		End Get
		Set(ByVal Value As Boolean)
			m_bItemIsLive = Value
		End Set
	End Property
	'************************
	
	Public Property IsUrgent() As Boolean
		Get
			Return m_bIsUrgent
		End Get
		Set(ByVal Value As Boolean)
			m_bIsUrgent = Value
		End Set
	End Property
	'********************
	Public Property Customer() As String
		Get
			Return m_sCustomer
		End Get
		Set(ByVal Value As String)
			m_sCustomer = Value
		End Set
	End Property
	'********************
	Public Property Workflow() As String
		Get
			Return m_sWorkflow
		End Get
		Set(ByVal Value As String)
			m_sWorkflow = Value
		End Set
	End Property
	'********************
	
	Public ReadOnly Property ItemUpdated() As Boolean
		Get
			
			If m_lOrigWorkflowId = m_lWorkflowId And m_lOrigStepOrder = m_lStepOrder And m_sDescription = m_sDescription And m_lOrigTaskGroupId = m_lTaskGroupId And m_lOrigTaskId = m_lTaskId And m_lOrigPMUserGroupId = m_lPMUserGroupId And m_lOrigUserid = m_lUserid And m_lOrigStepDaysDuration = m_lStepDaysDuration And m_lOrigCompleteNextWorkflowStepId = m_lCompleteNextWorkflowStepId And m_lOrigOverduenextWorkflowStepId = m_lOverduenextWorkflowStepId And m_bOrigExecutableTask = m_bExecutableTask And m_lOrigTaskActionTypeid = m_lTaskActionTypeId And m_lOrigEventTypeId = m_lEventTypeId And m_sOrigEventDescription = m_sEventDescription And m_sOrigTaskDescription = m_sTaskDescription And m_lOrigEventLogSubjectId = m_lEventlogsubjectId And m_bOrigIsUrgent = m_bIsUrgent And m_sOrigCustomer = m_sCustomer And m_sOrigWorkflow = m_sWorkflow And m_lOrigBranchId = m_lBranchId And Not m_bUpdateThisItem Then
				
				m_bItemUpdated = False
			Else
				m_bItemUpdated = True
			End If
			
			Return m_bItemUpdated
			
		End Get
	End Property
	'************************
	
	' ***************************************************************** '
	' Name: Copy
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 24-06-2003 : workflow
	' ***************************************************************** '
	Public Function Copy(ByVal v_oMaintainData As MaintainData) As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "Copy"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' generic data
			m_lId = v_oMaintainData.Id
			m_sCode = v_oMaintainData.Code
			m_sDescription = v_oMaintainData.description
			m_bIsDeleted = v_oMaintainData.IsDeleted
			m_dtEffectiveDate = v_oMaintainData.EffectiveDate
			m_bItemIsLive = v_oMaintainData.ItemIsLive
			m_lWorkflowId = v_oMaintainData.WorkflowId
			m_lStepOrder = v_oMaintainData.StepOrder
			m_lTaskGroupId = v_oMaintainData.TaskGroupId
			m_lTaskId = v_oMaintainData.TaskId
			m_lPMUserGroupId = v_oMaintainData.PMUserGroupId
			m_lUserid = v_oMaintainData.UserId
			m_lStepDaysDuration = v_oMaintainData.StepDaysDuration
			m_lCompleteNextWorkflowStepId = v_oMaintainData.CompleteNextWorkflowStepId
			m_lOverduenextWorkflowStepId = v_oMaintainData.OverdueNextWorkflowStepId
			m_bExecutableTask = v_oMaintainData.ExecutableTask
			m_lTaskActionTypeId = v_oMaintainData.TaskActionTypeid
			m_lEventTypeId = v_oMaintainData.EventTypeId
			m_sEventDescription = v_oMaintainData.EventDescription
			m_sTaskDescription = v_oMaintainData.TaskDescription
			m_lEventlogsubjectId = v_oMaintainData.EventLogSubjectId
			m_bUpdateThisItem = v_oMaintainData.UpdateThisItem
			m_bIsUrgent = v_oMaintainData.IsUrgent
			m_sCustomer = v_oMaintainData.Customer
			m_sWorkflow = v_oMaintainData.Workflow
			m_lBranchId = v_oMaintainData.BranchId
			
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
	' Name: CopyToOriginalData
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 25-06-2003 : workflow
	' ***************************************************************** '
	Public Function CopyToOriginalData() As Integer
		
		Dim result As Integer = 0
		Const sFunctionName As String = "CopyToOriginalData"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' default values
			m_sOrigDescription = m_sDescription
			m_bOrigIsDeleted = m_bIsDeleted
			m_dtOrigEffectiveDate = m_dtEffectiveDate
			
			' custom entries
			m_lOrigWorkflowId = m_lWorkflowId
			m_lOrigStepOrder = m_lStepOrder
			m_lOrigTaskGroupId = m_lTaskGroupId
			m_lOrigTaskId = m_lTaskId
			m_lOrigPMUserGroupId = m_lPMUserGroupId
			m_lOrigUserid = m_lUserid
			m_lOrigStepDaysDuration = m_lStepDaysDuration
			m_lOrigCompleteNextWorkflowStepId = m_lCompleteNextWorkflowStepId
			m_lOrigOverduenextWorkflowStepId = m_lOverduenextWorkflowStepId
			m_bOrigExecutableTask = m_bExecutableTask
			m_lOrigTaskActionTypeid = m_lTaskActionTypeId
			m_lOrigEventTypeId = m_lEventTypeId
			m_sOrigEventDescription = m_sEventDescription
			m_sOrigTaskDescription = m_sTaskDescription
			m_lOrigEventLogSubjectId = m_lEventlogsubjectId
			m_bOrigIsUrgent = m_bIsUrgent
			m_sOrigCustomer = m_sCustomer
			m_sOrigWorkflow = m_sWorkflow
			m_lOrigBranchId = m_lBranchId
			
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
