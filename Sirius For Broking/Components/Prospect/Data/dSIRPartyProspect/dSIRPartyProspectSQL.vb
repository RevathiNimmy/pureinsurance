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
	' Date: 27/04/1999
	'
	' Description: Contains the SQL Statements required by the
	'              SIRPartyProspect class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select SIRPartyProspect SQL
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleSIRPartyProspect"
    'developer guide no. 39 (Guide)
    Public Const ACSelectSingleSQL As String = "spe_party_prospect_sel"
	
	' Add SIRPartyProspect SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddSIRPartyProspect"
    'developer guide no. 39 (Guide)
    Public Const ACAddSQL As String = "spe_party_prospect_add"
	
	' Delete SIRPartyProspect SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteSIRPartyProspect"
    'developer guide no. 39 (Guide)
    Public Const ACDeleteSQL As String = "spe_party_prospect_del"
	
	' Update SIRPartyProspect SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateSIRPartyProspect"
    'developer guide no. 39 (Guide)
    Public Const ACUpdateSQL As String = "spe_party_prospect_upd"
End Module