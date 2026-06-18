Option Strict Off
Option Explicit On
Imports System
Module PMWrkTaskInstanceSQL
    ' ***************************************************************** '
    ' Class Name: PMWrkTaskInstanceSQL
    '
    ' Date: 26/10/1998
    '
    ' Description: SQL and Stored procedures
    '
    ' Edit History:
    ' DAK141299 - add is_visible column to task instance
    ' CJB280605 - PN22000 Do not select deleted diary_days!
    ' ***************************************************************** '

    'SQL Statements

    ' Select PMWrkTaskInstance SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSinglePMWrkTaskInstance"
    'developer guide no. 39
    Public Const ACSelectSingleSQL As String = "spe_PMWrk_Task_Instance_sel"

    ' Add PMWrkTaskInstance SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPMWrkTaskInstance"
    'DAK141299
    ' AMB 17/01/2003 - Workflow_information column added to PMWrk_Task_Instance table
    Public Const ACAddSQL As String = "spe_PMWrk_Task_Instance_add"

    ' Delete PMWrkTaskInstance SQL
    ' Note: This will also delte any Task Insr Keys & Task Inst Log entries.
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePMWrkTaskInstance"
    'developer guide no. 39
    Public Const ACDeleteSQL As String = "spu_pmwrk_task_inst_del"

    ' Update PMWrkTaskInstance SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePMWrkTaskInstance"
    'DAK141299
    ' AMB 17/01/2003 - Workflow_information column added to PMWrk_Task_Instance table
    Public Const ACUpdateSQL As String = "spe_PMWrk_Task_Instance_upd"

    ' Validate PMWrkTaskGroup SQL
    Public Const ACValidateTaskGroupStored As Boolean = True
    Public Const ACValidateTaskGroupName As String = "ValidateTaskGroup"
    'developer guide no. 39
    Public Const ACValidateTaskGroupSQL As String = "spu_PMWrk_Task_Group_val"

    ' Add Task Instance Key SQL
    Public Const ACAddTaskInstKeyStored As Boolean = True
    Public Const ACAddTaskInstKeyName As String = "AddTaskInstKey"
    'developer guide no. 39
    Public Const ACAddTaskInstKeySQL As String = "spu_PMWrk_Task_Inst_Key_add"

    ' Get Task Instance Keys SQL
    Public Const ACGetTaskInstKeysStored As Boolean = True
    Public Const ACGetTaskInstKeysName As String = "GetTaskInstKeys"
    'developer guide no. 39
    Public Const ACGetTaskInstKeysSQL As String = "spu_pmwrk_task_inst_keys_saa"

    ' Auto Delete PMWrkTaskInstance SQL
    Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "AutoDeletePMWrkTaskInstance"
    'developer guide no. 39
    Public Const ACAutoDeleteSQL As String = "spu_pmwrk_task_inst_auto_del"

    ' RAM20020715 : Multiple Tasks
    ' Reassign Multiple Tasks
    Public Const ACUpdateReAssignTasksStored As Boolean = True
    Public Const ACUpdateReAssignTasksName As String = "UpdateReAssignMultipleTasks"
    'developer guide no. 39
    Public Const ACUpdateReAssignTasksSQL As String = "spe_PMWrk_Task_Instance_Multiple_upd"

    ' RDC 16082002 get users for group
    Public Const ACGetUsersForGroupStored As Boolean = True
    Public Const ACGetUsersForGroupName As String = "GetUsersForGroup"
    'developer guide no. 39
    Public Const ACGetUsersForGroupSQL As String = "spu_PMWrk_Task_Get_Group_Users"

    ' RDC 06092002 checking user is group supervisor
    Public Const ACCheckIsSupervisorStored As Boolean = True
    Public Const ACCheckIsSupervisorName As String = "CheckIsSupervisor"
    'developer guide no. 39
    Public Const ACCheckIsSupervisorSQL As String = "spu_pmwrk_check_is_supervisor"

    ' RDC 09092002 get user groups that have the supplied task in their assigned tasks groups
    Public Const ACGetTaskUserGroupsStored As Boolean = True
    Public Const ACGetTaskUserGroupsName As String = "GetTaskUserGroups"
    'developer guide no. 39
    Public Const ACGetTaskUserGroupsSQL As String = "spu_get_task_user_groups"

    ' RDC 13092002
    Public Const ACCheckIsSysAdminStored As Boolean = True
    Public Const ACCheckIsSysAdminName As String = "CheckIsSysAdmin"
    'developer guide no. 39
    Public Const ACCheckIsSysAdminSQL As String = "spu_pmuser_is_sysadmin"

    '2005 Diary Days
    Public Const ACGetDiaryDaysStored As Boolean = False
    Public Const ACGetDiaryDaysName As String = "GetDiaryDays"
    Public Const ACGetDiaryDaysSQL As String = "SELECT no_of_days,description FROM Diary_Days where is_deleted = 0" 'PN22000

    Public Const ACGetUserPartyCntStored As Boolean = True
    Public Const ACGetUserPartyCntName As String = "GetUserPartyCnt"
    'developer guide no. 39
    Public Const ACGetUserPartyCntSQL As String = "spu_get_pmuser_party"

    Public Const KSelExternalWorkFlowConfiguration_UsergroupStored As Boolean = True
    Public Const KSelExternalWorkFlowConfiguration_UsergroupName As String = "SelExternalWorkFlowConfiguration_Usergroups"
    Public Const KSelExternalWorkFlowConfiguration_UsergroupsSQL As String = "Spu_sir_external_workflow_usergroups_sel"

    Public Const KFilteredKeyArrayStored As Boolean = True
    Public Const KFilteredKeyArrayStoredName As String = "FilterKeyArray"
    Public Const KFilteredKeyArrayStoredSQL As String = "spu_sir_get_key_settings"


    Public Const kBackgroundJobAddStored As Boolean = True
    Public Const kBackgroundJobAddName As String = "CreateBackgroundJob"
    Public Const kBackgroundJobAddSQL As String = "spu_SIR_Background_Job_add"

End Module