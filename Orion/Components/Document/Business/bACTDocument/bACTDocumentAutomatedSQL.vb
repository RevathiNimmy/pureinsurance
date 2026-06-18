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
	' Date: 08/08/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTDocument.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Document SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
    Public Const ACAutoGetDetailsName As String = "SelectAllDocument"
    'Developer Guide No 39
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_Document"
	
	' Select All Document SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
	Public Const ACAutoGetAllDetailsName As String = "SelectAllDocument"
    'Developer Guide No 39
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_Document"
	
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
	Public Const ACAutoCheckIDName As String = "CheckDocumentID"
    'Developer Guide No 39
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_Document"
	
	' Add Document SQL
	Public Const ACAutoAddStored As Boolean = True
	Public Const ACAutoAddName As String = "AddDocument"
    'Developer Guide No 39
    Public Const ACAutoAddSQL As String = "spu_ACT_add_Document"
	
	' Delete Document SQL
	Public Const ACAutoDeleteStored As Boolean = True
	Public Const ACAutoDeleteName As String = "DeleteDocument"
    'Developer Guide No 39
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_Document"
	
	' Update Document SQL
	Public Const ACAutoUpdateStored As Boolean = True
	Public Const ACAutoUpdateName As String = "UpdateDocument"
    'Developer Guide No 39
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_Document"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module