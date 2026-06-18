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
	' Date: 08/10/2001
	'
	' Description: Contains the SQL Statements required by the
	'              bCLMCheckUnpaidPremium.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Example Select using Stored Procedure
	' Public Const ACSelectCOIArrangementStored = True
	' Public Const ACSelectCOIArrangementName = "SelectCOIArrangement"
	' Public Const ACSelectCOIArrangementSQL = "{call spe_COI_Arrangement_sel (?)}"
	
	
	' Get Premium Payment information
	Public Const ACSelectPremiumPaymentsStatusStored As Boolean = True
	Public Const ACSelectPremiumPaymentsStatusName As String = "PremiumPaymentsStatus"
	Public Const ACSelectPremiumPaymentsStatusSQL As String = "spu_get_Premium_Payment_Status"
	
	' Get Transactions for Policy
	Public Const ACSelectTransactionsForPolicyStored As Boolean = True
	Public Const ACSelectTransactionsForPolicyName As String = "TransactionsForPolicy"
	Public Const ACSelectTransactionsForPolicySQL As String = "spu_get_Transactions_For_Policy"
	
	' ' Get Instalments for Policy
	' Public Const ACSelectInstalmentsForPolicyStored = True
	' Public Const ACSelectInstalmentsForPolicyName = "TransactionsForPolicy"
	' 'Public Const ACSelectInstalmentsForPolicySQL = "{call spu_get_Instalments_For_Policy (?)}"
	' Public Const ACSelectInstalmentsForPolicySQL = "spu_Temp_get_Instalments_For_Policy"
	
	' Get Original Claim ID (needed for unlocking)
	Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
	Public Const ACGetOriginalClaimIDSQL As String = "spu_CLM_Get_Base_Claim"
	
	' Delete any Work Claim records
	Public Const ACDeleteClaimStored As Boolean = True
	Public Const ACDeleteClaimName As String = "Delete Claim"
	Public Const ACDeleteClaimSQL As String = "spu_delete_claim"
End Module