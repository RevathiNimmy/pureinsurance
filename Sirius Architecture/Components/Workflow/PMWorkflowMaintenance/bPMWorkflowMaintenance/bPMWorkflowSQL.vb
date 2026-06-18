Option Strict Off
Option Explicit On
Imports System
Module PMWorkflowSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: PMWorkflowSQL
	'
	' Date: 22/01/2003
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate a PMPackage
	'
	' Edit History:
	'               AMB 22/01/2003 - Created
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectPMUserGroup"
	' Public Const ACSelectSQL = "SELECT * FROM PMUser_Group WHERE PMUser_id = {PMUser_id}"
	
	' Select PMPackage SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "SelectPMPackage"
	Public Const ACGetDetailsSQL As String = "{call spu_pmwrk_workflow_sel (?)}"
	
	' Select by 'code' field  SQL
	Public Const ACGetDetailsByCodeStored As Boolean = True
	Public Const ACGetDetailsByCodeName As String = "SelectByCodePMPackage"
	Public Const ACGetDetailsByCodeSQL As String = "{call spu_pmwrk_workflow_sel_code (?)}"
	
	' Select All PMPackage SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllPMPackage"
	Public Const ACGetAllDetailsSQL As String = "{call spu_pmwrk_workflow_selall}"
	
	' Add PMPackage SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddPMPackage"
	Public Const ACAddSQL As String = "{call spu_pmwrk_workflow_add (?,?,?,?,?,?)}"
	
	' Delete PMPackage SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeletePMPackage"
	Public Const ACDeleteSQL As String = "{call spu_pmwrk_workflow_del (?)}"
	
	' Update PMPackage SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdatePMPackage"
	Public Const ACUpdateSQL As String = "{call spu_pmwrk_workflow_upd (?,?,?,?,?,?)}"
End Module