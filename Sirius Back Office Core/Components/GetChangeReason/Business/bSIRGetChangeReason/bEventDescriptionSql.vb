Option Strict Off
Option Explicit On
Imports System
Module bEventDescriptionSql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Class Name: FindAccountSQL
	'
	' Date: 01 April 1997
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Embedded SQL)
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectEvent"
	' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"
	
	'Get Reason
	'sj 11/06/2004 - Add extra parameter
	Public Const ACMTAEventDescriptionStored As Boolean = True
	Public Const ACMTAEventDescriptionName As String = "Select"
	Public Const ACMTAEventDescriptionSQL As String = "spu_SIR_MTAEventDescription_Sel"
	
	Public Const ACClaimEventDescriptionStored As Boolean = True
	Public Const ACClaimEventDescriptionName As String = "Select"
	Public Const ACClaimEventDescriptionSQL As String = "spu_SIR_ClaimEventDescription_Sel"
End Module