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
	' Date: 29/04/1999
	'
	' Description: Contains the SQL Statements required by the
	'              SIRProspectCamp class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select SIRProspectCamp SQL
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleSIRProspectCamp"
    'developer guide no. 39 (Guide)
    Public Const ACSelectSingleSQL As String = "spe_prospect_campaign_sel"
	
	' Add SIRProspectCamp SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddSIRProspectCamp"
    'developer guide no. 39 (Guide)
    Public Const ACAddSQL As String = "spe_prospect_campaign_add"
	
	' Delete SIRProspectCamp SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteSIRProspectCamp"
    'developer guide no. 39 (Guide)
    Public Const ACDeleteSQL As String = "spe_prospect_campaign_del"
	
	' Update SIRProspectCamp SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateSIRProspectCamp"
    'developer guide no. 39 (Guide)
    Public Const ACUpdateSQL As String = "spe_prospect_campaign_upd"
End Module