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
	' Date: 08/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTCompanyCurrency.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select ACTCompanyCurrency SQL
    'developer guide no.39
    'start
	Public Const ACAutoGetDetailsStored As Boolean = True
	Public Const ACAutoGetDetailsName As String = "SelectAllACTCompanyCurrency"
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_CompanyCurrency"
	
	' Select All ACTCompanyCurrency SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
	Public Const ACAutoGetAllDetailsName As String = "SelectAllACTCompanyCurrency"
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_CompanyCurrency"
	
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
	Public Const ACAutoCheckIDName As String = "CheckACTCompanyCurrencyID"
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_CompanyCurrency_id"
	
	' Add ACTCompanyCurrency SQL
	Public Const ACAutoAddStored As Boolean = True
	Public Const ACAutoAddName As String = "AddACTCompanyCurrency"
    Public Const ACAutoAddSQL As String = "spu_ACT_add_CompanyCurrency"
	
	' Delete ACTCompanyCurrency SQL
	Public Const ACAutoDeleteStored As Boolean = True
	Public Const ACAutoDeleteName As String = "DeleteACTCompanyCurrency"
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_CompanyCurrency"
	
	' Update ACTCompanyCurrency SQL
	Public Const ACAutoUpdateStored As Boolean = True
	Public Const ACAutoUpdateName As String = "UpdateACTCompanyCurrency"
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_CompanyCurrency"
    'developer guide no. 29(No Solutions)
    'Shared Sub New()
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module