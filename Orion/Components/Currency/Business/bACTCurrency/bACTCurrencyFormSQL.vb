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
	' Date: 11/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTCurrency.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Currency SQL
	Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectCurrency"
    'developer guide no. 39
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_currency"
	
	' Select Currency by ISO code SQL
	Public Const ACGetDetailsByCodeStored As Boolean = True
    Public Const ACGetDetailsByCodeName As String = "SelectCurrency"
    'developer guide no. 39
    Public Const ACGetDetailsByCodeSQL As String = "spu_ACT_select_CurrencyCode"
	
	' Select All Currency SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllCurrency"
    'developer guide no. 39
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_currency"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckCurrencyID"
    'developer guide no. 39
    Public Const ACCheckIDSQL As String = "spu_ACT_check_currency"
	
	' Add Currency SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCurrency"
    'developer guide no. 39
    Public Const ACAddSQL As String = "spu_ACT_add_currency"
	
	' Delete Currency SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteCurrency"
    'developer guide no. 39
    Public Const ACDeleteSQL As String = "spu_ACT_delete_currency"
	
	' Update Currency SQL
	Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCurrency"
    'developer guide no. 39
    Public Const ACUpdateSQL As String = "spu_ACT_update_currency"
	
	'GetSystemCurrency
	Public Const ACGetSystemCurrencyStored As Boolean = True
    Public Const ACGetSystemCurrencyName As String = "GetSystemCurrency"
    'developer guide no. 39
    Public Const ACGetSystemCurrencySQL As String = "spu_ACT_GetSystemCurrency"
    'developer guide no. 29(No Solutions)
    'Shared Sub New()
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module