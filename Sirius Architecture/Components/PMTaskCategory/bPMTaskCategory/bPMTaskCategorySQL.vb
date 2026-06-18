Option Strict Off
Option Explicit On
Module PMTaskCategorySQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: PMTaskCategorySQL
    '
    ' Date: 08 October 1999
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL) manipulate a PMTaskCategory
    '
    ' Edit History:
    ' DAK131299 - Add licence key
    ' DAK050100 - Get ICCS
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectPMUserGroup"
    ' Public Const ACSelectSQL = "SELECT * FROM PMUser_Group WHERE PMUser_id = {PMUser_id}"

    ' Select PMTaskCategory SQL
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectPMTaskCategory"
    Public Const ACGetDetailsSQL As String = "spu_pmwrk_task_category"

    ' Select All PMTaskCategory SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllPMTaskCategory"
    Public Const ACGetAllDetailsSQL As String = "spu_pmwrk_task_category_all"

    ' Add PMTaskCategory SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPMTaskCategory"
    'DAK131299
    Public Const ACAddSQL As String = "spu_pmwrk_task_category_add"

    ' Delete PMTaskCategory SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeletePMTaskCategory"
    Public Const ACDeleteSQL As String = "spu_pmwrk_task_category_del"

    ' Update PMTaskCategory SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePMTaskCategory"
    'DAK131299
    Public Const ACUpdateSQL As String = "spu_pmwrk_task_category_upd"

    ' Count Active Task Instances For Category SQL
    Public Const ACCountCategoryTasksStored As Boolean = True
    Public Const ACCountCategoryTasksName As String = "CountCategoryTasks"
    Public Const ACCountCategoryTasksSQL As String = "spu_pmwrk_task_category_in_prog"

    ' Get ICCS
    Public Const ACGetICCSStored As Boolean = True
    Public Const ACGetICCSName As String = "GetICCS"
    Public Const ACGetICCSSQL As String = "spu_pm_iccs"
End Module