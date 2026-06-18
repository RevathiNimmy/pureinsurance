Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles


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
	
	' buttons captions
	Public Const ACResDataInterfaceButtonOK As Integer = 102
	Public Const ACResDataInterfaceButtonCancel As Integer = 103
	Public Const ACResDataInterfaceButtonApply As Integer = 104
	Public Const ACResDataInterfaceButtonAdd As Integer = 105
	Public Const ACResDataInterfaceButtonEdit As Integer = 106
	Public Const ACResDataInterfaceButtonDelete As Integer = 107
	Public Const ACResDataInterfaceButtonView As Integer = 108
	Public Const ACResDataInterfaceButtonUndelete As Integer = 122
	
	' list view headers
	Public Const ACResDataInterfaceListViewHeaderCode As Integer = 109
	Public Const ACResDataInterfaceListViewHeaderDescription As Integer = 110
	Public Const ACResDataInterfaceListViewHeaderTemplate As Integer = 111
	Public Const ACResDataInterfaceListViewHeaderEffectiveDate As Integer = 112
	Public Const ACResDataInterfaceListViewHeaderStepOrder As Integer = 136
	
	'*********************************
	' frmdetails constants
	'*********************************
	' form title
	Public Const ACResDataDetailsTitle As Integer = 120
	
	' tab title
	Public Const ACResDataDetailsTabActionType As Integer = 121
	
	' frmdetails labels
	Public Const ACResDataDetailsLabelDescription As Integer = 113
	Public Const ACResDataDetailsLabelEffectiveDate As Integer = 114
	Public Const ACResDataDetailsLabelDueDays As Integer = 115
	Public Const ACResDataDetailsLabelTemplate As Integer = 116
	Public Const ACResDataDetailsLabelOutcomeEditable As Integer = 117
	
	' frmdetails buttons captions
	Public Const ACResDataDetailsButtonOutcomes As Integer = 118
	Public Const ACResDataDetailsButtonOK As Integer = 119
	Public Const ACResDataDetailsButtonCancel As Integer = 123
	Public Const ACResDataDetailsButtonClear As Integer = 127
	
	Public Const ACResDataDetailsMessageInvalidDate As Integer = 124
	Public Const ACResDataDetailsMessageInvalidDueDays As Integer = 125
	Public Const ACResDataDetailsMessageInvalidCode As Integer = 126
	
	'*********************************
	' frmoutcomes constants
	'*********************************
	' form title
	Public Const ACResDataOutcomesTitle As Integer = 128
	
	' tab title
	Public Const ACResDataOutcomesTabOutcomes As Integer = 129
	
	' buttons
	Public Const ACResDataOutcomesButtonCancel As Integer = 130
	Public Const ACResDataOutcomesButtonOK As Integer = 131
	
	' messages
	Public Const ACMessageDeleteStepNotAllowed As Integer = 132
	
	Public Const ACMessageTitleMandatoryFieldChecks As Integer = 133
	Public Const ACMessageMandatoryChecksFailed As Integer = 134
	Public Const ACResDataDetailsMessageInvalidStepDaysDuration As Integer = 135
	
	'********************************************************************
	'********************************************************************
	'********************************************************************
	
	Public Const ACListViewColIndexCode As Integer = 1
	Public Const ACListViewColIndexDescription As Integer = 2
	Public Const ACListViewColIndexEffectiveDate As Integer = 3
	Public Const ACListViewColIndexStepOrder As Integer = 4
	
	Public Const ACListViewSubItemIndexDescription As Integer = 1
	Public Const ACListViewSubItemIndexEffectiveDate As Integer = 2
	Public Const ACListViewSubItemIndexStepOrder As Integer = 3
	
	Public Const ACListViewColKeyCode As String = "Code"
	Public Const ACListViewColKeyDescription As String = "Description"
	Public Const ACListViewColKeyEffectiveDate As String = "EffectiveDate"
	Public Const ACListViewColKeyStepOrder As String = "StepOrder"
	
	' List View Column Tag Types
	Public Const ACListViewTagTypeString As String = "String"
	Public Const ACListViewTagTypeNumber As String = "Number"
	Public Const ACListViewTagTypeImage As String = "Image"
	Public Const ACListViewTagTypeDate As String = "Date"
	
	Public Const ACMaintainDataWorkflowStepId As Integer = 0
	Public Const ACMaintainDataWorkflowId As Integer = 1
	Public Const ACMaintainDataStepOrder As Integer = 2
	Public Const ACMaintainDataStepCode As Integer = 3
	Public Const ACMaintainDataStepDescription As Integer = 4
	Public Const ACMaintainDataEffectiveDate As Integer = 5
	Public Const ACMaintainDataIsDeleted As Integer = 6
	Public Const ACMaintainDataTaskGroupId As Integer = 7
	Public Const ACMaintainDataTaskId As Integer = 8
	Public Const ACMaintainDataPMUserGroupId As Integer = 9
	Public Const ACMaintainDataUserId As Integer = 10
	Public Const ACMaintainDataStepDayDuration As Integer = 11
	Public Const ACMaintainDataCompleteNextWorkflowStepId As Integer = 12
	Public Const ACMaintainDataOverdueNextWorkflowStepId As Integer = 13
	Public Const ACMaintainDataExecutableTask As Integer = 14
	Public Const ACMaintainDataTaskActionTypeId As Integer = 15
	Public Const ACMaintainDataEventTypeId As Integer = 16
	Public Const ACMaintainDataEventDescription As Integer = 17
	Public Const ACMaintainDataEventLogSubjectId As Integer = 18
	Public Const ACMaintainDataTaskDescription As Integer = 19
	Public Const ACMaintainDataIsUrgent As Integer = 20
	Public Const ACMaintainDataCustomer As Integer = 21
	Public Const ACMaintainDataWorkflow As Integer = 22
	Public Const ACMaintainDataBranchId As Integer = 23
	
	
	'Public Const ACDataPosId = 0
	'Public Const ACDataPosCode = 1
	'Public Const ACDataPosDescription = 2
	'Public Const ACDataPosIsDeleted = 3
	'Public Const ACDataPosEffectiveDate = 4
	'Public Const ACDataPosDueDays = 5
	'Public Const ACDataPosDocumentTemplateCode = 6
	'Public Const ACDataPosOutcomeNotEditable = 7
	
	' Actions
	Public Const ACActionView As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMView
	Public Const ACActionEdit As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMAdd
	Public Const ACActionAdd As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMEdit
	
	
	' Document Template Array Fields
	Public Const ACDocTemplateId As Integer = 0
	Public Const ACDocTemplateDescription As Integer = 1
	Public Const ACDocTemplateCode As Integer = 2
	
	' array items
	Public Const ACDetailKey As Integer = 0
	Public Const ACDetailDesc As Integer = 1
	Public Const ACDetailCode As Integer = 2
	
	' Return Constants
	Public Const ACReturnOk As Integer = 1
	Public Const ACReturnCancel As Integer = 2
	
	
	'*******************
	' Lookup contants. *
	'*******************
	Public Const ACValueTableName As Integer = 0
	Public Const ACValueID As Integer = 1
	Public Const ACValueStartPos As Integer = 2
	Public Const ACValueNumber As Integer = 3
	
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
	Public Const ACUserGroup As Integer = 3
	Public Const ACUser As Integer = 4
	
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
	
	
	Public Const ACWorkflowStepSelectNone As String = "<none>"
	
	
	' Valid User Branch Data Array Positions
	Public Const ACValidBranchUserGroupId As Integer = 0
	Public Const ACValidBranchUserId As Integer = 1
	Public Const ACValidBranchBranchId As Integer = 2
	Public Const ACValidBranchBranchDescription As Integer = 3
End Module