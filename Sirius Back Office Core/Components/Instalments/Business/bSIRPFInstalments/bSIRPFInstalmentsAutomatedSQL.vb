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
	' Date: {TodaysDate}
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRPFInstalments.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All PFInstalments SQL
	' Public Const ACGetAllDetailsStored = True
	' Public Const ACGetAllDetailsName = "SelectAllPFInstalments"
	' Public Const ACGetAllDetailsSQL = "{call spe_PFInstalments_saa}"
	
	' Check ID SQL
	' Public Const ACCheckIDStored = True
	' Public Const ACCheckIDName = "CheckPFInstalmentsID"
	' Public Const ACCheckIDSQL = "{call spe_PFInstalments_check_id (?)}"
End Module