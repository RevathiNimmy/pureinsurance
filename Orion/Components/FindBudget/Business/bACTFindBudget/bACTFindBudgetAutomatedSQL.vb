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
	'              bACTFindBudget.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select ACTFindBudget SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
	Public Const ACAutoGetDetailsName As String = "SelectAllACTFindBudget"
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_{NewTable}"

    ' Select All ACTFindBudget SQL
    Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllACTFindBudget"
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_{NewTable}"

    ' Check ID SQL
    Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckACTFindBudgetID"
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_{NewTable}"

    ' Add ACTFindBudget SQL
    Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddACTFindBudget"
    Public Const ACAutoAddSQL As String = "spu_ACT_add_{NewTable}"

    ' Delete ACTFindBudget SQL
    Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeleteACTFindBudget"
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_{NewTable}"

    ' Update ACTFindBudget SQL
    Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdateACTFindBudget"
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_{NewTable}"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module