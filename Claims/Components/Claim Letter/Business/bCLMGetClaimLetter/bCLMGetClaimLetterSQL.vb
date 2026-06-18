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
	' Date: 06/10/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bCLMGetDocument.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' AM 11/12/00 Get client name from claim for work manager tasks
	Public Const ACGetClientAndPolicyIDStored As Boolean = True
	Public Const ACGetClientAndPolicyID As String = "GetClaimCliPolID"
	Public Const ACGetClientAndPolicyIDSQL As String = "{call spu_get_claim_clipol_id(?)}"
End Module