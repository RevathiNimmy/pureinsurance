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
	' Date: 11/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTCurrency.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select Currency SQL
    'developer guide no. 39

	Public Const ACAutoGetDetailsStored As Boolean = True
    Public Const ACAutoGetDetailsName As String = "SelectAllCurrency"
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_currency"
	
	' Select All Currency SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllCurrency"
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_currency"
	
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckCurrencyID"
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_currency"
	
	' Add Currency SQL
	Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddCurrency"
    Public Const ACAutoAddSQL As String = "spu_ACT_add_currency"
	
	' Delete Currency SQL
	Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeleteCurrency"
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_currency"
	
	' Update Currency SQL
	Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdateCurrency"
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_currency"
    ' developer guide no. 29(No Solutions)
    'Shared Sub New()
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module