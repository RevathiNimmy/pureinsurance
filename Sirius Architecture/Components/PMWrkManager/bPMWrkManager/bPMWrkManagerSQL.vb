Option Strict Off
Option Explicit On
Imports System
Module PMWrkManagerSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: PMWrkManagerSQL
    '
    ' Date: 08/10/1998
    '
    ' Description: SQL and Stored procedures
    '
    ' Edit History:
    ' DAK141299 - Get is_visible column from scheduled tasks
    ' ***************************************************************** '

    'SQL Statements

    ' ***************************************************************** '
    ' Scheduled Tasks SQL

    ' Get Scheduled Tasks
    Public Const ACGetSchedTasksStored As Boolean = True
    Public Const ACGetSchedTasksName As String = "GetScheduledTasks"
    Public Const ACGetSchedTasksSQL As String = "spu_PM_Get_Scheduled_Tasks"
    ' Get Batch Tasks
    Public Const kACGetBatchTasksStored As Boolean = True
    Public Const kACGetBatchTasksName As String = "GetBatchTasks"
    Public Const kACGetBatchTasksSQL As String = "spu_PM_Get_Batch_Tasks"
    'DAK110500
    'mkw100204 PN9978 Added client shortcode
    Public Const ACGetSchedTasksSelectSQL As String = "SELECT ti.pmwrk_task_instance_cnt , " &  _
	                                                  " ti.is_urgent , " &  _
	                                                  " ti.task_status , " &  _
	                                                  " t.type_of_task , " &  _
	                                                  " t.is_system_task , " &  _
	                                                  " ti.task_due_date , " &  _
	                                                  " ti.customer , " &  _
	                                                  " ti.description , " &  _
	                                                  " ti.pmuser_group_id , " &  _
	                                                  " ti.user_id , " &  _
	                                                  " t.pmnav_process_id , " &  _
	                                                  " t.component_object_name , " &  _
	                                                  " t.component_class_name , " &  _
	                                                  " t.display_icon , " &  _
	                                                  " t.is_view_only_task , " &  _
	                                                  " t.linked_object_name , " &  _
	                                                  " t.linked_class_name , " &  _
	                                                  " t.linked_caption_id , " &  _
	                                                  " ti.is_visible , " &  _
	                                                  " p.file_name as nav_xml_file, " &  _
                                                      "c.shortname "
    '    " t.nav_xml_file "
    'DAK110500
    ' RDC 27082002 updated
    ' RDC 24072003 added pmnavxm_process
    'Public Const ACGetSchedTasksFromSQL = _
    ''    "FROM       pmwrk_task_instance ti With(Index(I__PMWrk_Task_Instance__pmwrk_task_id)), " & _
    ''    "           pmwrk_task t, pmnavxm_process p "

    'mkw100204 PN9978 Retrieve client party of task (associated by keys)
    Public Const ACGetSchedTasksFromSQL As String = "FROM pmwrk_task_instance AS ti " &  _
	                                                "JOIN PMWrk_Task AS t      ON t.pmwrk_task_id = ti.pmwrk_task_id " &  _
	                                                "LEFT OUTER JOIN PMNavXM_Process AS p ON p.pmnavxm_process_id = t.pmnavxm_process_id " &  _
	                                                "left outer join pmwrk_task_inst_key v on pmnav_key_id=2 and v.pmwrk_task_instance_cnt=ti.pmwrk_task_instance_cnt " &  _
                                                    "left outer join party c on c.party_cnt=v.key_value "

    ' RDC 278082002 new SQL
    Public Const ACGetSchedTasksFromSQLStatusInd As String = "FROM       pmwrk_task_instance ti, " &  _
                                                             "           pmwrk_task t "

    Public Const ACGetTasksByKeyFromSQL As String = "FROM       PMNav_Key nk " &  _
	                                                "JOIN       PMWrk_Task_Inst_Key tik " &  _
	                                                "ON         nk.pmnav_key_id = tik.pmnav_key_id " &  _
	                                                "JOIN       PMWrk_Task_Instance ti " &  _
	                                                "ON         tik.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt " &  _
	                                                "JOIN       PMWrk_Task t " &  _
                                                    "ON         ti.pmwrk_task_id = t.pmwrk_task_id "
    'DAK110500
    ' RDC 24072003 add pmnavxm_process_id
    Public Const ACGetSchedTasksWhereSQL As String = " WHERE " '     ti.pmwrk_task_id = t.pmwrk_task_id " & |    '"AND        t.pmnavxm_process_id = p.pmnavxm_process_id "
    Public Const ACgetTasksByKeyWhereSQL As String = "WHERE nk.name = {key_name} " &  _
                                                     "AND tik.key_value = {key_value} "

    'Public Const ACGetSchedTasksStatusSQL = "AND ti.task_status = {task_status} "
    'Public Const ACGetSchedTasksGroupSQL = "AND ti.pmuser_group_id = {pmuser_group_id} "
    'Public Const ACGetSchedTasksUserSQL = "AND ((ti.user_id = {user_id}) OR (ti.user_id IS NULL)) "
    'Public Const ACGetSchedTasksLimitDateSQL = "AND ti.task_due_date <= {due_date_limit} "
    'Public Const ACGetSchedTasksOmitSQL = "AND ti.task_status <> {task_status} "
    'Public Const ACGetSchedTasksOmitSystemSQL = "AND t.is_system_task = 0 "
    'Public Const ACGetSchedTasksOnlySystemSQL = "AND t.is_system_task = 1 "
    'Public Const ACGetSchedTasksOrderBySQL = "ORDER BY ti.task_due_date ASC"

    Public Const ACGetSchedTasksCurrentUserSQL As String = "(ti.user_id = {current_user_id} AND ti.is_visible = 0) OR ("
    Public Const ACGetSchedTasksStatusSQL As String = "ti.task_status = {task_status} AND "
    Public Const ACGetSchedTasksGroupSQL As String = "ti.pmuser_group_id = {pmuser_group_id} AND "
    Public Const ACGetSchedTasksUserSQL As String = "((ti.user_id = {user_id}) OR (ti.user_id IS NULL)) AND "
    Public Const ACGetSchedTasksLimitDateSQL As String = "ti.task_due_date <= {due_date_limit} AND "
    Public Const ACGetSchedTasksOmitSQL As String = "ti.task_status <> {task_status} AND "
    Public Const ACGetSchedTasksOmitSystemSQL As String = "t.is_system_task = 0 AND "
    Public Const ACGetSchedTasksOnlySystemSQL As String = "t.is_system_task = 1 AND "
    Public Const ACGetSchedTasksCloseBracketSQL As String = ") "
    Public Const ACGetSchedTasksOrderBySQL As String = "ORDER BY ti.task_due_date ASC"
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Get Available Tasks
    Public Const ACGetAvailTasksStored As Boolean = True
    Public Const ACGetAvailTasksName As String = "GetAvailableTasks"
    'developers guide no 39
    'Public Const ACGetAvailTasksSQL As String = "{call spu_pmwrk_users_tasks_sel (?,?,?)}"
    Public Const ACGetAvailTasksSQL As String = "spu_pmwrk_users_tasks_sel"
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Quick Start SQL

    ' Add Quick Start Task SQL
    'developer guide no. 39
    Public Const ACAddQuickStartTaskStored As Boolean = True
    Public Const ACAddQuickStartTaskName As String = "AddQuickStartTask"
    Public Const ACAddQuickStartTaskSQL As String = "spe_PMWrk_User_Quick_Start_add"

    'Delete Quick Start Task SQL
    Public Const ACDelSingleQuickStartTasksStored As Boolean = True
    Public Const ACDelSingleQuickStartTasksName As String = "DeleteQuickStartTask"
    Public Const ACDelSingleQuickStartTasksSQL As String = "spe_PMWrk_User_Quick_Start_delete"


    ' Delete All Quick Start Tasks SQL
    Public Const ACDelQuickStartTasksStored As Boolean = True
    Public Const ACDelQuickStartTasksName As String = "DelQuickStartTask"
    Public Const ACDelQuickStartTasksSQL As String = "spu_PMWrk_Users_Quick_Start_dar"

    ' Get All Quick Start Tasks SQL
    Public Const ACGetQuickStartTasksStored As Boolean = True
    Public Const ACGetQuickStartTasksName As String = "GetQuickStartTask"
    Public Const ACGetQuickStartTasksSQL As String = "spu_PMWrk_Users_Quick_Start_saa"
    ' ***************************************************************** '

    'DJM 10/03/2004
    Public Const ACGetDefaultUserGroupForTaskGroupStored As Boolean = True
    Public Const ACGetDefaultUserGroupForTaskGroupName As String = "GetDefaultUserGroupForTaskGroup"
    Public Const ACGetDefaultUserGroupForTaskGroupSQL As String = "spu_PM_Get_Default_UserGroup_For_TaskGroup"

    ' WPR 29
    Public Const ACGetAgentTaskStored As Boolean = True
    Public Const ACGetAgentTaskName As String = "GetAgentTasks"
    Public Const ACGetAgentTaskSQL As String = "spu_GetAgents_for_Tasks"
    ' RnDWPR03-Batch Management
    Public Const kACGetViewBatchProcessStatusAuthoritySQL As String = "spe_User_Authorities_Sel"
    Public Const kACGetViewBatchProcessStatusAuthorityName As String = "selectUserAuthority"
    Public Const kACGetViewBatchProcessStatusAuthorityStored As Boolean = True

End Module
