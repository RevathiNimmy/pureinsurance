Option Strict Off
Option Explicit On
Imports System
Module PMProductSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMProductSQL
	'
	' Date: 08 October 1999
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate a PMProduct
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectPMUserGroup"
	' Public Const ACSelectSQL = "SELECT * FROM PMUser_Group WHERE PMUser_id = {PMUser_id}"
	
	' Select PMProduct by code SQL
	Public Const ACGetPMProductByCodeStored As Boolean = False
	Public Const ACGetPMProductByCodeName As String = "SelectPMProduct"
	Public Const ACGetPMProductByCodeSQL As String = "SELECT pmproduct_id FROM PMProduct WHERE code = {code}"
End Module