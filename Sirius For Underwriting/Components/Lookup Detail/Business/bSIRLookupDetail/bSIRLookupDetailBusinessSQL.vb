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
	' Date: 05/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRNarr.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All Lookup Details SQL
	Public Const ACGetLookupDetailsStored As Boolean = True
	Public Const ACGetLookupDetailsName As String = "SelectLookupDetails"
	Public Const ACGetLookupDetailsSQL As String = "{call spu_lookup_detail_saa (?)}"
	
	' Delete All Lookup Details SQL
	Public Const ACUpdateLookupDetailsStored As Boolean = True
	Public Const ACUpdateLookupDetailsName As String = "DeleteLookupDetails"
	Public Const ACUpdateLookupDetailsSQL As String = "{call spe_lookup_Detail_upd (?,?,?,?,?,?,?,?)}"
	
	' Insert All Lookup Details SQL
	Public Const ACInsertLookupDetailsStored As Boolean = True
	Public Const ACInsertLookupDetailsName As String = "InsertLookupDetails"
	Public Const ACInsertLookupDetailsSQL As String = "{call spe_lookup_Detail_add (?,?,?,?,?,?,?,?)}"
End Module