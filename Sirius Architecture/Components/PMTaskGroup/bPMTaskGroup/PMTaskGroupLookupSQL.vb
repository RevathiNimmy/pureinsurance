Option Strict Off
Option Explicit On
Imports System
Module PMTaskGroupLookupSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMTaskGroupLookupSQL
	'
	' Date: 22 October 1998
	'
	' Description: Contains the SQL/Stored Procedures used by the
	'              Lookup class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' All User Lookup SQL
	Public Const ACAllTaskGroupLookupStored As Boolean = True
    Public Const ACAllTaskGroupLookupName As String = "AllTaskGroupLookup"
    'Developer Guide No.39
    Public Const ACAllTaskGroupLookupSQL As String = "spu_pmwrk_all_task_group_sel"

    ' Group All User Lookup SQL
    Public Const ACGroupAllTaskGroupLookupStored As Boolean = True
    Public Const ACGroupAllTaskGroupLookupName As String = "GroupAllTaskGroupLookup"
    'Developer Guide No.39
    Public Const ACGroupAllTaskGroupLookupSQL As String = "spu_pmwrk_task_group_sel"

    ' All Tasks Lookup SQL
    Public Const ACAllTasksLookupStored As Boolean = True
    Public Const ACAllTasksLookupName As String = "TasksLookup"
    'Developer Guide No. 39
    Public Const ACAllTasksLookupSQL As String = "spu_pmwrk_task_sel_all"

    ' All Tasks in a Group Lookup SQL
    Public Const ACTaskGroupTasksLookupStored As Boolean = True
    Public Const ACTaskGroupTasksLookupName As String = "TaskGroupTasksLookup"
    'Developer Guide No. 39
    Public Const ACTaskGroupTasksLookupSQL As String = "spu_pmwrk_task_group_tasks_sel"
End Module