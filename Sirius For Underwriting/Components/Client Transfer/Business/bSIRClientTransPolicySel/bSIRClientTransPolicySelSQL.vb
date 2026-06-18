Option Strict Off
Option Explicit On
Imports System
Module bSIRClientTransPolcySelSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	' ***************************************************************** '
	' Class Name: bSIRClientTransPolicySel
	'
	' Date: 13/10/2008
	'
	' Description: Contains the SQL Statements to (Stored Procedures)
	'
	' Edit History: Saurabh Agrawal
	' *****************************************************************
	
	
	
	Public Const ACTransferpolicies As String = "spu_SIR_Transfer_Policy"
	Public Const ACTransferPoliciesName As String = "Transfer Policies"
	Public Const ACTransferPoliciesStored As Boolean = True
End Module