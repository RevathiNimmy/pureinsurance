Option Strict Off
Option Explicit On
Imports System
Module AutomatedSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: AutomatedSQL
	'
	' Date: 20/10/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bACTBudget.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'developer guide no.39
    'start
	' Select ACTBudget SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
	Public Const ACAutoGetDetailsName As String = "SelectAllACTBudget"
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_Budget"
	
	' Select All ACTBudget SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
	Public Const ACAutoGetAllDetailsName As String = "SelectAllACTBudget"
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_Budget"
	
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
	Public Const ACAutoCheckIDName As String = "CheckACTBudgetID"
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_Budget"
	
	' Add ACTBudget SQL
	Public Const ACAutoAddStored As Boolean = True
	Public Const ACAutoAddName As String = "AddACTBudget"
    Public Const ACAutoAddSQL As String = "spu_ACT_add_Budget"
	
	' Delete ACTBudget SQL
	Public Const ACAutoDeleteStored As Boolean = True
	Public Const ACAutoDeleteName As String = "DeleteACTBudget"
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_Budget"
	
	' Update ACTBudget SQL
	Public Const ACAutoUpdateStored As Boolean = True
	Public Const ACAutoUpdateName As String = "UpdateACTBudget"
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_Budget"
    'end

End Module