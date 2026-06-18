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
	'              bACTTypeTables.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select TypeTable SQL
	Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectTypeTable"
    'developer guide no. 39
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_TypeTable"
	
	' Select All TypeTable SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllTypeTable"
    'developer guide no. 39
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_TypeTable"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckTypeTableID"
    'developer guide no. 39
    Public Const ACCheckIDSQL As String = "spu_ACT_check_TypeTable"
	
	' Add TypeTable SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddTypeTable"
    'developer guide no. 39
    Public Const ACAddSQL As String = "spu_ACT_add_TypeTable"
	
	' Delete TypeTable SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteTypeTable"
    'developer guide no. 39
    Public Const ACDeleteSQL As String = "spu_ACT_delete_TypeTable"
	
	' DeleteAll TypeTable SQL
	Public Const ACDeleteAllStored As Boolean = True
    Public Const ACDeleteAllName As String = "DeleteAllTypeTable"
    'developer guide no. 39
    Public Const ACDeleteAllSQL As String = "spu_ACT_delAll_TypeTable"
	
	' Update TypeTable SQL
	Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateTypeTable"
    'developer guide no. 39
    Public Const ACUpdateSQL As String = "spu_ACT_update_TypeTable"
    'developer guide no. 29
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module