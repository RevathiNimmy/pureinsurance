Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 05/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRProspect.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Delete All Prospect Policies
	Public Const ACDeletePoliciesStored As Boolean = False
	Public Const ACDeletePoliciesName As String = "DeletePolicies"
	Public Const ACDeletePoliciesSQL As String = "Delete from prospect_policy where party_cnt = {party_cnt}"
	
	' Insert Prospect Policies
	Public Const ACInsertPoliciesStored As Boolean = True
    Public Const ACInsertPoliciesName As String = "InsertPolicies"

    'Developer Guide No 39
    Public Const ACInsertPoliciesSQL As String = "spe_prospect_policy_add"
End Module