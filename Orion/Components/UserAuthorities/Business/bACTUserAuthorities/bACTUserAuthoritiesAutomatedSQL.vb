Option Strict Off
Option Explicit On
Imports System
Module AutomatedSQL
	' ***************************************************************** '
	' Class Name: AutomatedSQL
	'
	' Date: 14/02/2000
	'
	' Description: Contains the SQL Statements required by the 
	'              bACTUserAuthorities.Automated class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All ACTUserAuthorities SQL
	' Public Const ACGetAllDetailsStored = True
	' Public Const ACGetAllDetailsName = "SelectAllACTUserAuthorities"
	' Public Const ACGetAllDetailsSQL = "{call spe_ACTUserAuthorities_saa}"
	
	' Check ID SQL
	' Public Const ACCheckIDStored = True
	' Public Const ACCheckIDName = "CheckACTUserAuthoritiesID"
	' Public Const ACCheckIDSQL = "{call spe_ACTUserAuthorities_check_id (?)}"
End Module