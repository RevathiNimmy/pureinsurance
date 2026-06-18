Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 20/10/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bACTBudgetDetail.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select ACTBudgetDetail SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectAllACTBudgetDetail"
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_Budget_Detail"
	
	' Select All ACTBudgetDetail SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllACTBudgetDetail"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_select_all_Budget_Detail"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckACTBudgetDetailID"
    Public Const ACCheckIDSQL As String = "spu_ACT_check_Budget_Detail"
	
	' Add ACTBudgetDetail SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddACTBudgetDetail"
    Public Const ACAddSQL As String = "spu_ACT_add_Budget_Detail"
	
	' Delete ACTBudgetDetail SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteACTBudgetDetail"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_Budget_Detail"
	
	' Update ACTBudgetDetail SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateACTBudgetDetail"
    Public Const ACUpdateSQL As String = "spu_ACT_update_Budget_Detail"
	
	' Get Account Name
	Public Const ACGetAccNameStored As Boolean = True
	Public Const ACGetAccNameName As String = "GetAccName"
    Public Const ACGetAccNameSQL As String = "spu_ACT_Do_GetAccName"
	
	' Delete Budget Details SQL
	Public Const ACDeleteBudgetDetailsStored As Boolean = True
	Public Const ACDeleteBudgetDetailsName As String = "DeleteACTBudgetDetails"
	Public Const ACDeleteBudgetDetailsSQL As String = "spu_ACT_delete_Budget_Details"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module