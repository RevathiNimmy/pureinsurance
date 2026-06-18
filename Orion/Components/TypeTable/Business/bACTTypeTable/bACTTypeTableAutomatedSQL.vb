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
	' Date: 14/07/1997
	'
	' Description: Contains the SQL Statements required by the 
	'              bACTTypeTable.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select TypeTable SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
    Public Const ACAutoGetDetailsName As String = "SelectAllTypeTable"
    'developer guide no. 39
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_{NewTable}"
	
	' Select All TypeTable SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllTypeTable"
    'developer guide no. 39
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_{NewTable}"
	
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckTypeTableID"
    'developer guide no. 39
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_{NewTable}"
	
	' Add TypeTable SQL
	Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddTypeTable"
    'developer guide no. 39
    Public Const ACAutoAddSQL As String = "spu_ACT_add_{NewTable}"
	
	' Delete TypeTable SQL
	Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeleteTypeTable"
    'developer guide no. 39
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_{NewTable}"
	
	' Update TypeTable SQL
	Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdateTypeTable"
    'developer guide no. 39
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_{NewTable}"
    'developer guide no. 29
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module