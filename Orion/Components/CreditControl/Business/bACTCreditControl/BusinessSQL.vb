Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date:
	'
	' Description: Contains the SQL Statements required by the
	'              business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Const kGetALLPMWrkTaskGroupTasksName As String = "Returns all effective PMWrkTasks for all effective PMWrkTaskGroups"
	Public Const kGetALLPMWrkTaskGroupTasksSQL As String = "spu_ACT_PMwrk_Task_Group_Tasks_Select"
	
	Public Const kGetALLPMWrkTaskGroupPMUserGroupsName As String = "Returns all effective PMUSerGroups for all effective PMWrkTaskGroups"
	Public Const kGetALLPMWrkTaskGroupPMUserGroupsSQL As String = "spu_ACT_PMwrk_Task_Group_PMUserGroup_Select"
	
	Public Const kGetAccountCreditAmountsName As String = "Returns the sum amount of all unallocated credits on the specified account"
	Public Const kGetAccountCreditAmountsSQL As String = "spu_ACT_Get_Account_UnallocatedCreditAmount"
	
	Public Const kGetInstalmentImportInsuranceFileStatuses As String = "spu_ACT_Get_Instalment_Import_File_Insurance_File_Statuses"
	
	Public Const kAddCreditControlInsuranceFileStatus As String = "spu_ACT_Credit_Control_Rule_Insurance_File_Status_Add"
	
	Public Const kDeleteCreditControlRuleInsuranceFileStatus As String = "spu_ACT_Credit_Control_Rule_Insurance_File_Status_Delete"
	
	Public Const kGetCreditControlRuleInsuranceFileStatuses As String = "spu_ACT_Credit_Control_Rule_Insurance_File_Status_Select"
	
	Public Const kGetNextAvailableInstalmentFailureCount As String = "spu_ACT_Credit_Control_Rule_Get_Selected_Instalment_Failure_Counts"
	
	Public Const kGetInsuranceFileStatusSQL As String = "spu_SIR_GetInsuranceFileStatus"
	
    Public Const kGetOutstandingBalanceAndTransactionsForInsuranceFolderSQL As String = "spu_ACT_Get_Outstanding_Transactions_For_Insurance_Folder"

    Public Const kCheckForRenewedPolicySQL As String = "spu_SIR_CheckForRenewedPolicy"
    Public Const kCheckAutoCancellationDocument As String = "spu_Act_Check_AutoCancellation_Document"

    Public Const kGetOutstandingBalanceAndTransactionsForSIP As String = "spu_ACT_Get_TransDetails_For_CreditControl"

End Module