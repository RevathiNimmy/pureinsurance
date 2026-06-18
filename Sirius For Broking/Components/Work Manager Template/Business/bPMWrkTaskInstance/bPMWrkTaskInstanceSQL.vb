Option Strict Off
Option Explicit On
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
    ' ***************************************************************** '

    'SQL Statements

    'developer guide no. 39
    'start
    ' Select PMWrkTaskInstance SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSinglePMWrkTaskInstance"
    Public Const ACSelectSingleSQL As String = "spe_PMWrk_Task_Instance_Temp_sel"

    ' Add PMWrkTaskInstance SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPMWrkTaskInstanceTemp"

    'PN15774 Extra Parameter
    Public Const ACAddSQL As String = "spe_PMWrk_Task_Instance_Temp_add"
    Public Const ACAddSQLInstance As String = "spe_PMWrk_Task_Instance_add"


    ' Delete PMWrkTaskInstance SQL
    ' Note: This will also delte any Task Insr Keys & Task Inst Log entries.
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePMWrkTaskInstance"
    Public Const ACDeleteSQL As String = "spu_pmwrk_task_inst_del"

    ' Update PMWrkTaskInstance SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePMWrkTaskInstance"

    Public Const ACUpdateSQL As String = "spe_PMWrk_Task_Instance_upd"

    ' Validate PMWrkTaskGroup SQL
    Public Const ACValidateTaskGroupStored As Boolean = True
    Public Const ACValidateTaskGroupName As String = "ValidateTaskGroup"
    Public Const ACValidateTaskGroupSQL As String = "spu_PMWrk_Task_Group_val"

    ' Add Task Instance Key SQL
    Public Const ACAddTaskInstKeyStored As Boolean = True
    Public Const ACAddTaskInstKeyName As String = "AddTaskInstKey"
    Public Const ACAddTaskInstKeySQL As String = "spu_PMWrk_Task_Inst_Key_add"

    ' Get Task Instance Keys SQL
    Public Const ACGetTaskInstKeysStored As Boolean = True
    Public Const ACGetTaskInstKeysName As String = "GetTaskInstKeys"
    Public Const ACGetTaskInstKeysSQL As String = "spu_pmwrk_task_inst_keys_saa"

    ' Auto Delete PMWrkTaskInstance SQL
    Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "AutoDeletePMWrkTaskInstance"
    Public Const ACAutoDeleteSQL As String = "spu_pmwrk_task_inst_auto_del"

    '2006 Diary Days
    Public Const ACGetDiaryDaysStored As Boolean = False
    Public Const ACGetDiaryDaysName As String = "GetDiaryDays"
    Public Const ACGetDiaryDaysSQL As String = "SELECT no_of_days,description FROM Diary_Days where is_deleted = 0" 'PN27272
End Module