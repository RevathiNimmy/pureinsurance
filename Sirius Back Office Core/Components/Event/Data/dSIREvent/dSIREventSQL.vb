Option Strict Off
Option Explicit On
Imports System
Module SQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: SQL
	'
	' Date: 07/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              SIREvent class.
	'
	' Edit History:
	'
	' CJB 07/04/2004 Add ShortDescription on add proc
	'
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select SIREvent SQL
    'developer guide no. 39 (Guide)
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleSIREvent"
    Public Const ACSelectSingleSQL As String = "spe_event_log_sel"

	
	' Add SIREvent SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddSIREvent"
	'sj 30/09/2002 - start
	'Public Const ACAddSQL = "{call spe_event_log_add (?,?,?,?,?,?,?,?,?,?," & _
	''                                                 "?,?,?,?,?,?)}"
	'Public Const ACAddSQL = "{call spe_event_log_add (?,?,?,?,?,?,?,?,?,?," & _
	''                                                 "?,?,?,?,?,?,?,?,?)}"
	'sj 30/09/2002 - end
	'sj 15/09/2003 - Start
	'2005 StickyNotes - 2 new parameters
	Public Const ACAddSQL As String = "spe_event_log_add"
	'sj 15/09/2003 - End
	
	' Delete SIREvent SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteSIREvent"
    Public Const ACDeleteSQL As String = "spe_event_log_del"
	
	' Update SIREvent SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateSIREvent"
	'sj 30/09/2002 - start
	'Public Const ACUpdateSQL = "{call spe_event_log_upd (?,?,?,?,?,?,?,?,?," & _
	''                                                    "?,?,?,?,?,?)}"
	'2005 StickyNotes - 2 new parameters
    Public Const ACUpdateSQL As String = "spe_event_log_upd"
	'sj 30/09/2002 - end
	
	' Select SIREvent SQL
	Public Const ACUpdateEventLogClaimPolicyStored As Boolean = True
	Public Const ACUpdateEventLogClaimPolicyName As String = "UpdateEventLogClaimChanged"
	Public Const ACUpdateEventLogClaimPolicySQL As String = "spu_event_log_claim_policy_upd"
End Module