Option Strict Off
Option Explicit On
Imports System
Module BusinessMain
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: BusinesMain
	'
	' Date:  04-12-2002
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bPMPackageStep"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "BusinessMain"
	
	' Initialisation variable stores
	
	
	'******************
	' Authority Rules Constants
	
	' Context Constants
	Public Const ACAuthLevelContextLossSchedulePayment As Integer = 0
	Public Const ACAuthLevelContextPurchaseOrderProduction As Integer = 1
	Public Const ACAuthLevelContextDebtRecovery As Integer = 2
	
	' Process Type
	Public Const ACAuthLevelProcessTypeClaims As Integer = 1
	
	'******************
	
	' Step Data Array Position Constants
	Public Const ACStepDataStepItemId As Integer = 0
	Public Const ACStepDataClaimPartyId As Integer = 1
	Public Const ACStepDataClaimDebtId As Integer = 2
	Public Const ACStepDataTaskDescription As Integer = 3
	Public Const ACStepDataTaskDuration As Integer = 4
	
	' Step User Group Array Position Constants
	Public Const ACStepGroupUserStepItemId As Integer = 0
	Public Const ACStepGroupUserUserGroupId As Integer = 1
	Public Const ACStepGroupUserUserId As Integer = 2
	Public Const ACStepGroupUserBranchID As Integer = 3
	
	' Step table and assoc document template keys
	Public Const ACStepWorkflowStepId As Integer = 0
	Public Const ACStepWorkflowId As Integer = 1
	Public Const ACStepStepOrder As Integer = 2
	Public Const ACStepStepCode As Integer = 3
	Public Const ACStepStepDescription As Integer = 4
	Public Const ACStepEffectiveDate As Integer = 5
	Public Const ACStepIsDeleted As Integer = 6
	Public Const ACStepTaskGroupId As Integer = 7
	Public Const ACStepTaskId As Integer = 8
	Public Const ACStepUserGroupId As Integer = 9
	Public Const ACStepUserId As Integer = 10
	Public Const ACStepStepDaysDuration As Integer = 11
	Public Const ACStepCompleteNextWorkflowStepId As Integer = 12
	Public Const ACStepOverdueNextWorkflowStepId As Integer = 13
	Public Const ACStepExecutableTask As Integer = 14
	Public Const ACStepTaskActionTypeId As Integer = 15
	Public Const ACStepEventTypeId As Integer = 16
	Public Const ACStepEventDescription As Integer = 17
	Public Const ACStepEventLogSubjectId As Integer = 18
	Public Const ACStepTaskDescription As Integer = 19
	Public Const ACStepDocTemplateCode As Integer = 20
	Public Const ACStepDocumentTemplateId As Integer = 21
	Public Const ACStepSGUPMUserGroupId As Integer = 22
	Public Const ACStepSGUUserId As Integer = 23
	Public Const ACStepSDClaimPartyId As Integer = 24
	Public Const ACStepSDClaimDebtId As Integer = 25
	Public Const ACStepSDTaskDescription As Integer = 26
	Public Const ACStepSDTaskDuration As Integer = 27
	Public Const ACStepDocumentTypeId As Integer = 28
	Public Const ACStepDocumentDescription As Integer = 29
	Public Const ACStepTaskDefaultCompletionTaskOutcome As Integer = 30
	Public Const ACStepEventLogClaimPartyId As Integer = 31
	Public Const ACStepEventLogClaimDebtId As Integer = 32
	Public Const ACStepEventLogPartyCnt As Integer = 33
	Public Const ACStepEventLogInsuranceFolderCnt As Integer = 34
	Public Const ACStepEventLogInsuranceFileCnt As Integer = 35
	Public Const ACStepEventLogClaimCnt As Integer = 36
	Public Const ACStepPrevTaskInstanceCustomer As Integer = 37
	Public Const ACStepTaskIsUrgent As Integer = 38
	Public Const ACStepTaskCustomer As Integer = 39
	Public Const ACStepTaskWorkflow As Integer = 40
	Public Const ACStepExecutableComponent As Integer = 41
	Public Const ACStepEventTypeCode As Integer = 42
	Public Const ACStepEventClaimPerilId As Integer = 43
	Public Const ACStepBranchId As Integer = 44
	Public Const ACStepSGUBranchId As Integer = 45
	
	Public Const ACProcessTaskImmediately As Integer = 0
	Public Const ACExecuteTask As Integer = 1
	Public Const ACTaskStatusCompleted As Integer = 3
	Public Const ACDocumentProductionComponent As String = "bPMProduceDocument.NavigatorV3"
	
	
	'Printing Modes
	'Public Const ACNormalMode = 0
	'Public Const ACMergeMode = 1
	'Public Const ACPrintMode = 2
	'Public Const ACPrintSilentMode = 3
	'Public Const ACSpoolDocMode = 4
	'Public Const ACSpoolReportMode = 5
End Module