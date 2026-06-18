Option Strict Off
Option Explicit On
Imports System
Module PMTaskGroupCategorySQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMTaskGroupCategorySQL
	'
	' Date: 08 October 1999
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate a PMTaskGroupCategory
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectPMUserGroup"
	' Public Const ACSelectSQL = "SELECT * FROM PMUser_Group WHERE PMUser_id = {PMUser_id}"
	
	' Select PMTaskGroupCategory SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectPMTaskGroupCategory"
	Public Const ACGetDetailsSQL As String = "{call spu_pmwrk_task_group_category}"
	
	' Select All PMTaskGroupCategory SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllPMTaskGroupCategory"
	Public Const ACGetAllDetailsSQL As String = "{call spu_pmwrk_task_group_cat_all (?,?)}"
	
	' Add PMTaskGroupCategory SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddPMTaskGroupCategory"
	Public Const ACAddSQL As String = "{call spu_pmwrk_task_group_cat_add (?,?,?,?,?,?,?,?,?)}"
	
	' Delete PMTaskGroupCategory SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeletePMTaskGroupCategory"
	Public Const ACDeleteSQL As String = "{call spu_pmwrk_task_group_cat_del (?)}"
	
	' Update PMTaskGroupCategory SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdatePMTaskGroupCategory"
	Public Const ACUpdateSQL As String = "{call spu_pmwrk_task_group_cat_upd (?,?,?,?,?,?,?,?,?,?)}"
	
	' Count Active Task Instances For Category SQL
	Public Const ACCountCategoryTasksStored As Boolean = True
	Public Const ACCountCategoryTasksName As String = "CountCategoryTasks"
	Public Const ACCountCategoryTasksSQL As String = "{call spu_pmwrk_task_group_cat_prog (?)}"
End Module