Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	
	' jmf : 30-06-2003
	Public Const ACPMWrk_Get_Due_TasksStored As Boolean = True
	Public Const ACPMWrk_Get_Due_TasksName As String = "PMWrk_Get_Due_Tasks"
	Public Const ACPMWrk_Get_Due_TasksSQL As String = "{call spe_PMWrk_Get_Due_Tasks}"
	
	' jmf : 30-06-2003
	Public Const ACPMWrk_Task_Action_Type_GetSP As Boolean = True
	Public Const ACPMWrk_Task_Action_Type_GetName As String = "PMWrk_Task_Action_Type_Get"
	Public Const ACPMWrk_Task_Action_Type_GetSQL As String = "{call spu_PMWrk_Task_Action_Type_Get}"
	
	' jmf : 30-06-2003
	Public Const ACPMEvent_Task_FindSP As Boolean = True
	Public Const ACPMEvent_Task_FindName As String = "PMWrk_Task_Event_Task_Find"
	Public Const ACPMEvent_Task_FindSQL As String = "{call spu_PMEvent_Task_Find}"
	
	' Returns lookup details for all the PMUSer Records
	Public Const ACGetPMUserDetailsName As String = "Returns lookup details for all PMUser Records"
	Public Const ACGetPMUserDetailsSQL As String = "{call spu_PM_Get_PMUsers}"
	
	' Returns all entries from PMWrk_Task_Group_Task_Action
	Public Const ACGetTaskGroupTaskActionName As String = "Returns entries from PMWrk_Task_Group_Task_Action"
	Public Const ACGetTaskGroupTaskActionSQL As String = "{call spu_PM_Task_Group_Task_Action_Select}"
	
	' Returns all entries from PMWrk_Task_Group_Task
	Public Const ACGetTaskGroupTaskName As String = "Returns entries from PMWrk_Task_Group_Task"
	Public Const ACGetTaskGroupTaskSQL As String = "{call spu_PM_Task_Group_Task_Select}"
	
	' Returns all entries from PMUser_Group_User
	Public Const ACGetPMUserGroupUsersName As String = "Returns entries from PMUser_Group_User"
	Public Const ACGetPMUserGroupUsersSQL As String = "{call spu_PM_Get_PMUser_Group_Users}"
	
	' Returns all entries from PMUser_GRoup_Activity
	Public Const ACGetTaskGroupUserGroupsName As String = "Returns entries from PMUser_Group_Activity"
	Public Const ACGetTaskGroupUserGroupsSQL As String = "{call spu_PM_Get_Task_Group_UserGroups}"
	
	' Returns all entries from PMwrk_Task_Action_type_Outcome
	Public Const ACGetTaskActionTypeOutcomesName As String = "Returns entries from PMwrk_Task_ACtion_type_Outcome"
	Public Const ACGetTaskActionTypeOutcomesSQL As String = "{call spu_PM_Get_Task_Action_Type_Outcome}"
	
	' Returns all event task details for the specified pmwrk_task_instance_cnt
	Public Const ACGetEventTaskName As String = "Returns all event task details for the specified pmwrk_task_instance_cnt"
	Public Const ACGetEventTaskSQL As String = "{call spu_PM_EventTask_Select(?)}"
	
	' Updates the specified events associated task
	Public Const ACUpdateEventTaskName As String = "updates the specified events associated task"
	Public Const ACUpdateEventTaskSQL As String = "{call spu_PM_Update_Event_Task(?,?,?,?,?)}"
End Module