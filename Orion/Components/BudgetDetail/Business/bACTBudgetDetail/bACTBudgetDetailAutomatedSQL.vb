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
	'              bACTBudgetDetail.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select ACTBudgetDetail SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
	Public Const ACAutoGetDetailsName As String = "SelectAllACTBudgetDetail"
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_Budget_Detail"

    ' Select All ACTBudgetDetail SQL
    Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllACTBudgetDetail"
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_Budget_Detail"

    ' Check ID SQL
    Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckACTBudgetDetailID"
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_Budget_Detail"

    ' Add ACTBudgetDetail SQL
    Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddACTBudgetDetail"
    Public Const ACAutoAddSQL As String = "spu_ACT_add_Budget_Detail"

    ' Delete ACTBudgetDetail SQL
    Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeleteACTBudgetDetail"
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_Budget_Detail"

    ' Update ACTBudgetDetail SQL
    Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdateACTBudgetDetail"
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_Budget_Detail"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module