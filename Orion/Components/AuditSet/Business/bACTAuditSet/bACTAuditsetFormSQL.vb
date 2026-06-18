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
	' Date: 08/08/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTAuditset.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Auditset SQL
	Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectAuditset"
    'developer guide no 39
    'Public Const ACGetDetailsSQL As String = "{call spu_ACT_select_AuditSet (?)}"
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_AuditSet"
	
	' Select All Auditset SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllAuditset"
    'developer guide no 39
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_select_all_AuditSet"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckAuditsetID"
    'developer guide no 39
    Public Const ACCheckIDSQL As String = "spu_ACT_check_AuditSet"
	
	' Add Auditset SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddAuditset"
    'developer guide no 39
    Public Const ACAddSQL As String = "spu_ACT_add_AuditSet"
	
	' Delete Auditset SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteAuditset"
    'developer guide no 39
    Public Const ACDeleteSQL As String = "spu_ACT_delete_AuditSet"
	
	' Update Auditset SQL
	Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateAuditset"
    'developer guide no 39
    Public Const ACUpdateSQL As String = "spu_ACT_update_AuditSet"
	
	' Select All Auditset SQL
	Public Const ACGetAuditSetbyCashListStored As Boolean = True
    Public Const ACGetAuditSetbyCashListName As String = "SelectAllAuditset"
    'developer guide no 39
    Public Const ACGetAuditSetbyCashListSQL As String = "spu_ACT_selall_AuditSet"

End Module