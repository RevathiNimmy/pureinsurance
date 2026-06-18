Option Strict Off
Option Explicit On
Imports System
Module PMTaskSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMTaskSQL
	'
	' Date: 17 October 1996
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate an PMTask
	'
	' Edit History:
	'RFC300399 - Change to use ODBC Call syntax for stored procedures.
	'DAK200999 - New column, display icon, added to PMWrk_Task table.
	'DAK041099 - New fields for view only task, tasks linked to objects,
	'            and whether task can be run directly from available tasks
	'            bar.
	'DAK211299 - Add task category
	' RAW 14/02/2003 : ISS2153 : added new nav_xml_file column
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectPMTask"
	' Public Const ACSelectSQL = "SELECT * FROM PMTask WHERE PMTask_id = {PMTask_id}"
	
	' Select PMTask SQL
	'RFC300399
	Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectPMTask"
    'Developer Guide No. 39
    Public Const ACGetDetailsSQL As String = "spu_pmwrk_task_sel"

    ' Select All PMTasks SQL
    'RFC300399
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllPMTask"
    'Developer Guide No. 39
    Public Const ACGetAllDetailsSQL As String = "spu_pmwrk_task_all"

    ' Add PMTask SQL
    'RFC300399
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddPMTask"
    'DAK200999
    'DAK041099
    'DAK211299
    ' RAW 14/02/2003 : ISS2153 : added new nav_xml_file column
    'Developer Guide No. 39
    Public Const ACAddSQL As String = "spu_pmwrk_task_add"


    ' Delete PMTask SQL
    Public Const ACDeleteStored As Boolean = False
    Public Const ACDeleteName As String = "DeletePMTask"
    Public Const ACDeleteSQL As String = "UPDATE pmwrk_task SET is_deleted = 1 " & _
                                         "WHERE pmwrk_task_id = {pmwrk_task_id} "

    ' Update PMTask SQL
    'RFC300399
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePMTask"
    'DAK200999
    'DAK041099
    'DAK211299
    ' RAW 14/02/2003 : ISS2153 : added new nav_xml_file column
    'Developer Guide No. 39
    Public Const ACUpdateSQL As String = "spu_pmwrk_task_upd"
	
	
	Public Const ACUserAuthorityTorunTaskSQL As String = "spu_GetUserAuthorityTask"
	Public Const ACUserAuthorityTorunTaskName As String = "GetUserAuthorityForTask"
End Module