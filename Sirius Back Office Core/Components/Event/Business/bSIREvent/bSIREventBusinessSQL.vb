Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 07/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIREvent.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All SIREvent SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllSIREvent"
	Public Const ACGetAllDetailsSQL As String = "spe_event_log_saa"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckSIREventID"
	Public Const ACCheckIDSQL As String = "spe_event_log_check_id"
	
	' Copy By Document SQL
	Public Const ACCopyByDocumentCntStored As Boolean = True
	Public Const ACCopyByDocumentCntName As String = "CopyByDocument"
	Public Const ACCopyByDocumentCntSQL As String = "spe_event_log_copy_by_document_cnt"
End Module