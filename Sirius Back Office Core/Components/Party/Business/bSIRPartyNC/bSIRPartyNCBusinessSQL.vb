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
	' Date: 25/06/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRPartyNC.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All SIRPartyNC SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyNC"
    Public Const ACGetAllDetailsSQL As String = "spe_party_insurer_saa"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckSIRPartyNCID"
    Public Const ACCheckIDSQL As String = "spe_SIRPartyNC_check_id"
	
	' Select next available shortname from agent table
	Public Const ACGetNextRefStored As Boolean = True
	Public Const ACGetNextRefName As String = "SelectNextShortname"
    Public Const ACGetNextRefSQL As String = "spu_Next_Insurer_Shortname_sel"
End Module