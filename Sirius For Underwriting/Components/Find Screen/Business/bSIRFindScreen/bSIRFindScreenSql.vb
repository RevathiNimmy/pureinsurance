Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module FindRiskTypeSql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Class Name: FindPolicyTypeSQL
	'
	' Date: 27th September 1996
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate an FindPolicyType
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectEvent"
	' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"
	
	
	Public Const ACSelectStored As Boolean = False
	Public Const ACSelectName As String = "SelectTypes"
	Public Const ACSelectSQL As String = "SELECT DISTINCT gs.gis_screen_id," & Strings.Chr(13) & Strings.Chr(10) &  _
	                                     "c.Caption" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                     "FROM Gis_screen gs," & Strings.Chr(13) & Strings.Chr(10) &  _
	                                     "pmcaption c," & Strings.Chr(13) & Strings.Chr(10) &  _
	                                     "risk_type_GIS_screen rtgs" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                     "WHERE gs.caption_id = c.caption_id" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                     "AND c.language_id = {language_id}" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                     "AND gs.is_deleted = 0" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                     "AND gs.gis_screen_id = rtgs.gis_screen_id" & Strings.Chr(13) & Strings.Chr(10) &  _
	                                     "AND rtgs.risk_type_id = {risk_type_id}"
End Module