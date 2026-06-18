Option Strict Off
Option Explicit On
Imports System
Module PMTaskGroupSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMTaskGroupSQL
	'
	' Date: 17 October 1996
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate a PMTaskGroup
	'
	' Edit History:
	'RFC300399 - Change to call Stored Procedures using ODBC syntax.
	' DAK070999 - Replace ACUpdateSQL with stored procedure call
	' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
	' DAK211299 - Remove PMWrk_Task_Group_Category
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectPMUserGroup"
	' Public Const ACSelectSQL = "SELECT * FROM PMUser_Group WHERE PMUser_id = {PMUser_id}"
	
	' Select PMTaskGroup SQL
	'RFC300399
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectPMTaskGroup"
	Public Const ACGetDetailsSQL As String = "spu_pmwrk_task_group"
	
	' Select All PMTaskGroup SQL
	'RFC300399
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllPMTaskGroup"
    'Developer Guide No. 39
    'Public Const ACGetAllDetailsSQL As String = "{call spu_pmwrk_task_group_all (?,?)}"
    Public Const ACGetAllDetailsSQL As String = "spu_pmwrk_task_group_all"

    ' Select Every PMTask SQL
    'RFC300399
    Public Const ACGetEveryTaskStored As Boolean = True
    Public Const ACGetEveryTaskName As String = "SelectEveryTask"
    'Developer Guide No. 39
    'Public Const ACGetEveryTaskSQL As String = "{call spu_pmwrk_task_group_everyone (?,?)}"
    Public Const ACGetEveryTaskSQL As String = "spu_pmwrk_task_group_everyone"

    ' Add PMTaskGroup SQL
    'RFC300399
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPMTaskGroup"
    'DAK041099
    'DAK211299
    'Developer Guide No. 39
    'Public Const ACAddSQL As String = "{call spu_pmwrk_task_group_add (?,?,?,?,?,?)}"
    Public Const ACAddSQL As String = "spu_pmwrk_task_group_add"

    ' Delete PMTaskGroup SQL
    Public Const ACDeleteStored As Boolean = False
    Public Const ACDeleteName As String = "DeletePMTaskGroup"
    Public Const ACDeleteSQL As String = "UPDATE pmwrk_task_group SET is_deleted = 1 " & _
                                         "WHERE pmwrk_task_group_id = {pmwrk_task_group_id} "

    ' Update PMTaskGroup SQL
    'DAK070999
    'Public Const ACUpdateStored = False
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePMTaskGroup"
    'DAK041099
    'DAK211299
    'Developer Guide No. 39
    'Public Const ACUpdateSQL As String = "{call spu_pmwrk_task_group_upd (?,?,?,?,?,?,?)}"
    Public Const ACUpdateSQL As String = "spu_pmwrk_task_group_upd"
	'Public Const ACUpdateSQL = "UPDATE pmwrk_task_group SET caption_id = {caption_id}, " & _
	'"code = {code}, " & _
	'"description = {description}, " & _
	'"is_deleted = {is_deleted}, " & _
	'"effective_date = {effective_date}, " & _
	'"display_icon = {display_icon} " & _
	'"WHERE pmwrk_task_group_id = {pmwrk_task_group_id} "
	
	' Delete All Group Tasks SQL
	Public Const ACDeleteGroupTasksStored As Boolean = False
	Public Const ACDeleteGroupTasksName As String = "DeleteGroupTasks"
	Public Const ACDeleteGroupTasksSQL As String = "DELETE FROM pmwrk_task_group_task Where pmwrk_task_group_id = {pmwrk_task_group_id}"
	
	' Add Group Task SQL
	Public Const ACAddGroupTaskStored As Boolean = False
	Public Const ACAddGroupTaskName As String = "AddGroupTask"
	Public Const ACAddGroupTaskSQL As String = "INSERT INTO pmwrk_task_group_task (pmwrk_task_group_id, pmwrk_task_id, display_sequence_num) " &  _
	                                           "SELECT {pmwrk_task_group_id}, {pmwrk_task_id}, {display_sequence_num}"
End Module