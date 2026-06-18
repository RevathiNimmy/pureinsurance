Option Strict Off
Option Explicit On
Imports System
Module InterfaceConst
	
	'********************************************************************
	'******** Resource Data *********************************************
	'********************************************************************
	
	'*********************************
	' frminterface constants
	'*********************************
	
	' form title
	Public Const ACResDataInterfaceTitle As Integer = 100
	
	' tab title
	Public Const ACResDataInterfaceTabActionTypes As Integer = 101
	
	' frame event
	Public Const ACResDataInterfaceFrameEvent As Integer = 102
	
	' frame event field labels
	Public Const ACResDataInterfaceLabelEventType As Integer = 103
	Public Const ACResDataInterfaceLabelEventDate As Integer = 104
	Public Const ACResDataInterfaceLabelEventSubject As Integer = 105
	Public Const ACResDataInterfaceLabelEventUser As Integer = 106
	Public Const ACResDataInterfaceLabelEventDescription As Integer = 107
	
	' frame task
	Public Const ACResDataInterfaceFrameTask As Integer = 108
	
	' frame task field labels
	Public Const ACResDataInterfaceLabelTaskTaskGroup As Integer = 109
	Public Const ACResDataInterfaceLabelTaskTask As Integer = 110
	Public Const ACResDataInterfaceLabelTaskActionType As Integer = 111
	Public Const ACResDataInterfaceLabelTaskDueDate As Integer = 112
	Public Const ACResDataInterfaceLabelTaskUrgent As Integer = 113
	Public Const ACResDataInterfaceLabelTaskDescription As Integer = 122
	
	' frame allocate to
	Public Const ACResDataInterfaceFrameAllocateTo As Integer = 114
	
	' frame allocate to field labels
	Public Const ACResDataInterfaceLabelAllocateToUserGroup As Integer = 109
	Public Const ACResDataInterfaceLabelAllocateToUser As Integer = 109
	
	Public Const ACResDataInterfaceFrameComplete As Integer = 117
	
	Public Const ACResDataInterfaceLabelCompleteOutcome As Integer = 118
	Public Const ACResDataInterfaceLabelCompleteOutcomeDate As Integer = 119
	
	' buttons captions
	Public Const ACResDataInterfaceButtonOK As Integer = 120
	Public Const ACResDataInterfaceButtonCancel As Integer = 121
	
	
	' messages
	Public Const ACResDataMessageInvalidTaskDueDate As Integer = 123
	Public Const ACResDataMessageInvalidTaskDueDateTime As Integer = 124
	Public Const ACResDateMessageTaskDueDateInPast As Integer = 125
	Public Const ACResDataMessageCompletedTaskOutcomeMandatory As Integer = 126
	
	
	
	
	'********************************************************************
	'********************************************************************
	'********************************************************************
	
	'*******************
	' Lookup contants. *
	'*******************
	Public Const ACValueTableName As Integer = 0
	Public Const ACValueID As Integer = 1
	Public Const ACValueStartPos As Integer = 2
	Public Const ACValueNumber As Integer = 3
	
	Public Const ACDetailKey As Integer = 0
	Public Const ACDetailDesc As Integer = 1
	Public Const ACDetailCode As Integer = 2
	
	
	' Maintain Data Array Positions
	Public Const ACMaintainDataPosId As Integer = 0
	Public Const ACMaintainDataPosCode As Integer = 1
	Public Const ACMaintainDataPosDescription As Integer = 2
	Public Const ACMaintainDataPosCompletionTaskId As Integer = 3
	Public Const ACMaintainDataPosIncompleteTaskId As Integer = 4
	Public Const ACMaintainDataPosActionRequired As Integer = 5
	Public Const ACMaintainDataPosCompletionCode As Integer = 6
	Public Const ACMaintainDataPosCompletionDescription As Integer = 7
	Public Const ACMaintainDataPosIncompleteCode As Integer = 8
	Public Const ACMaintainDataPosIncompleteDescription As Integer = 9
	
	' Actions
	Public Const ACActionView As Integer = 0
	Public Const ACActionEdit As Integer = 1
	Public Const ACActionAdd As Integer = 2
	
	
	
	' Return Constants
	Public Const ACReturnOk As Integer = 1
	Public Const ACReturnCancel As Integer = 2
	
	' Task Outcome DAta Array Positions
	Public Const ACTaskOutcomePosId As Integer = 0
	Public Const ACTaskOutcomePosDescription As Integer = 1
	Public Const ACTaskOutcomePosCode As Integer = 2
	
	' Lookup table name constancts
	Public Const ACLookupTablePMWrkTaskGroup As String = "PMWrk_Task_Group"
	Public Const ACLookupTablePMWrkTask As String = "PMWrk_Task"
	Public Const ACLookupTablePMUserGroup As String = "PMUser_Group"
	Public Const ACLookupTableEventType As String = "Event_Type"
	Public Const ACLookupTableEventLogSubject As String = "Event_Log_Subject"
	Public Const ACLookupTablePMWrkTaskActionType As String = "PMWrk_Task_Action_Type"
	Public Const ACLookupTableTaskOutcome As String = "Task_Outcome"
	
	' Event Type Codes
	Public Const ACEventTypeCodeClientTask As String = "CLNT_TASK"
	Public Const ACEventTypeCodePolicyTask As String = "POL_TASK"
	Public Const ACEventTypeCodeClaimDebtTask As String = "CLDBT_TASK"
	Public Const ACEventTypeCodeClaimTask As String = "CLM_TASK"
	
	Public Const ACTaskGroup As Integer = 0
	Public Const ACTask As Integer = 1
	Public Const ACTaskAction As Integer = 2
	
	' Task Group Task Data Array Positions
	Public Const ACTaskGroupTaskGroupId As Integer = 0
	Public Const ACTaskgroupTaskId As Integer = 1
	Public Const ACTaskGroupTaskDescription As Integer = 2
	
	' Task Group Task Action Data Array Positions
	Public Const ACTaskGroupTaskActionTaskGroupId As Integer = 0
	Public Const ACTaskGroupTaskActionTaskId As Integer = 1
	Public Const ACTaskGroupTaskActionTypeId As Integer = 2
	Public Const ACTaskGroupTaskActionDescription As Integer = 3
	Public Const ACTaskGroupTaskActionDueDays As Integer = 4
	
	
	' PMuser Group User Data Array Positions
	Public Const ACPMUserGroupId As Integer = 0
	Public Const ACPMUserId As Integer = 1
	Public Const ACPMUsername As Integer = 2
	
	
	Public Const ACTaskGroupUserGroup_TaskGroupId As Integer = 0
	Public Const ACTaskGroupUserGroup_UserGroupId As Integer = 1
	Public Const ACTaskGroupUserGroup_UserGroupDescription As Integer = 2
	
	' PMWrk_Task_Action_Outcome Data Array Positions
	Public Const ACTaskActionOutcome_TaskActionTypeId As Integer = 0
	Public Const ACTaskActionOutcome_TaskOutcomeId As Integer = 1
	Public Const ACTaskActionOutcome_TaskOutcome As Integer = 2
	
	' Event Task Data Array Positions
	Public Const ACEventTask_EventCnt As Integer = 0
	Public Const ACEventTask_EventTypeId As Integer = 1
	Public Const ACEventTask_EventLogSubjectId As Integer = 2
	Public Const ACEventTask_UserId As Integer = 3
	Public Const ACEventTask_EventDate As Integer = 4
	Public Const ACEventTask_EventDescription As Integer = 5
	Public Const ACEventTask_PMWrkTaskGroupId As Integer = 6
	Public Const ACEventTask_PMWrkTaskId As Integer = 7
	Public Const ACEventTask_PMWrkTaskActionTypeid As Integer = 8
	Public Const ACEventTask_TaskDueDate As Integer = 9
	Public Const ACEventTask_TaskDescription As Integer = 10
	Public Const ACEventTask_IsUrgent As Integer = 11
	Public Const ACEventTask_Customer As Integer = 12
	Public Const ACEventTask_WorkflowInformation As Integer = 13
	Public Const ACEventTask_AllocateToPMUserGroupId As Integer = 14
	Public Const ACEventTask_AllocateToUserId As Integer = 15
	Public Const ACEventTask_TaskStatus As Integer = 16
	Public Const ACEventTask_TaskOutcomeId As Integer = 17
	Public Const ACEventTask_TaskOutcomeDate As Integer = 18
	Public Const ACEventTask_PMNavProcessId As Integer = 19
	Public Const ACEventTask_ComponentObjectName As Integer = 20
	Public Const ACEventTask_ComponentClassName As Integer = 21
	Public Const ACEventTask_DisplayIcon As Integer = 22
	Public Const ACEventTask_IsViewOnlyTask As Integer = 23
	Public Const ACEventTask_LinkedObjectName As Integer = 24
	Public Const ACEventTask_LinkedClassName As Integer = 25
	Public Const ACEventTask_linkedcaptionid As Integer = 26
	Public Const ACEventTask_SelectedEventTypeDescription As Integer = 27
	Public Const ACEventTask_SelectedEventLogSubjectDescription As Integer = 28
	Public Const ACEventTask_SelectedTaskGroupDescription As Integer = 29
	Public Const ACEventTask_SelectedTaskDescription As Integer = 30
	Public Const ACEventTask_SelectedTaskActionTypeDescription As Integer = 31
	Public Const ACEventTask_TaskOutcomeDescription As Integer = 32
	Public Const ACEventTask_EventUsername As Integer = 33
	Public Const ACEventTask_TaskAllocateToUsernameDescription As Integer = 34
	Public Const ACEventTask_TaskAllocateToUsergroupDescription As Integer = 35
	Public Const ACEventTask_PMWrkTaskInstanceCnt As Integer = 36
	
	' included specifically to pass these details to the update
	' method because if they are not passed they are updated
	' to nulls..
	Public Const ACEventTask_PartyCnt As Integer = 37
	Public Const ACEventTask_InsuranceFolderCnt As Integer = 38
	Public Const ACEventTask_InsuranceFileCnt As Integer = 39
	Public Const ACEventTask_ClaimCnt As Integer = 40
	Public Const ACEventTask_DocumentCnt As Integer = 41
	Public Const ACEventTask_OldAddressCnt As Integer = 42
	Public Const ACEventTask_NewAddressCnt As Integer = 43
	Public Const ACEventTask_CampaignId As Integer = 44
	Public Const ACEventTask_DocumentTypeId As Integer = 45
	Public Const ACEventTask_ReportTypeId As Integer = 46
	Public Const ACEventTask_OldPartyTypeId As Integer = 47
	Public Const ACEventTask_DocumentTemplateId As Integer = 48
	Public Const ACEventTask_OutputMediaCode As Integer = 49
	
	
	
	' Task Status Id
	Public Const ACTaskStatusNew As Integer = 0
	Public Const ACTaskStatusInProgress As Integer = 1
	Public Const ACTaskStatusInComplete As Integer = 2
	Public Const ACTaskStatusComplete As Integer = 3
End Module