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
	'              bACTAuditset.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACAutoSelectStored = False
	' Public Const ACAutoSelectName = "SelectRisk"
	' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Auditset SQL
	Public Const ACAutoGetDetailsStored As Boolean = True
	Public Const ACAutoGetDetailsName As String = "SelectAllAuditset"
	Public Const ACAutoGetDetailsSQL As String = "{call spu_ACT_select_AuditSet (?)}"
	
	' Select All Auditset SQL
	Public Const ACAutoGetAllDetailsStored As Boolean = True
	Public Const ACAutoGetAllDetailsName As String = "SelectAllAuditset"
	Public Const ACAutoGetAllDetailsSQL As String = "{call spu_ACT_select_all_AuditSet}"
	
	' Check ID SQL
	Public Const ACAutoCheckIDStored As Boolean = True
	Public Const ACAutoCheckIDName As String = "CheckAuditsetID"
	Public Const ACAutoCheckIDSQL As String = "{call spu_ACT_check_AuditSet (?)}"
	
	' Add Auditset SQL
	Public Const ACAutoAddStored As Boolean = True
	Public Const ACAutoAddName As String = "AddAuditset"
	Public Const ACAutoAddSQL As String = "{call spu_ACT_add_AuditSet (?,?,?,?,?,?,?,?,?,?,?,?)}"
	
	' Delete Auditset SQL
	Public Const ACAutoDeleteStored As Boolean = True
	Public Const ACAutoDeleteName As String = "DeleteAuditset"
	Public Const ACAutoDeleteSQL As String = "{call spu_ACT_delete_AuditSet (?)}"
	
	' Update Auditset SQL
	Public Const ACAutoUpdateStored As Boolean = True
	Public Const ACAutoUpdateName As String = "UpdateAuditset"
	Public Const ACAutoUpdateSQL As String = "{call spu_ACT_update_AuditSet (?,?,?,?,?,?,?,?,?,?,?,?)}"
    
End Module