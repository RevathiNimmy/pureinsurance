Option Strict Off
Option Explicit On
Imports System
Module bSIRBankGuaranteeSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	' ***************************************************************** '
	' Class Name: bSIRBankGuaranteeSQL
	'
	' Date: 15/08/2007
	'
	' Description: Contains the SQL Statements to (Stored Procedures)
	'
	' Edit History: Gaurav Arora
	' ***************************************************************** '
	
	Public Const ACSELBankGuaranteeDetailsSQL As String = "spu_PartyBG_Details_Sel"
	Public Const ACSELBankGuaranteeDetailsName As String = "Select Bank Guarantee Details"
	
	Public Const ACSELBankGuaranteePolicySQL As String = "spu_get_policies_on_BG_for_id"
	Public Const ACSELBankGuaranteePolicyName As String = "Select Policy Details"
	
	Public Const ACADDBankGuaranteeDetailsSQL As String = "spu_PartyBG_Details_Add"
	Public Const ACADDBankGuaranteeDetailsName As String = "Add Bank Guarantee Details"
	
	Public Const ACUPDBankGuaranteeDetailsSQL As String = "spu_PartyBG_Details_Upd"
	Public Const ACUPDBankGuaranteeDetailsName As String = "Update Bank Guarantee Details"
	
	Public Const ACDELBankGuaranteeDetailsSQL As String = "spu_BankGuarantee_Details_DelUndel"
	Public Const ACDELBankGuaranteeDetailsName As String = "Delete Bank Guarantee Details"
	
	Public Const ACGetSystemCurrencySQL As String = "spu_ACT_GetSystemCurrency"
	Public Const ACGetSystemCurrencyName As String = "Get System Currency Details"
	
	'Public Const ACADDBankGuaranteeHistorySQL = "spu_BankGuarantee_History_Add"
	'Public Const ACADDBankGuaranteeHistoryName = "Add Bank Guarantee History"
	
	'Public Const ACSELBankGuaranteeHistorySQL = "spu_BankGuarantee_History_Sel"
	'Public Const ACSELBankGuaranteeHistoryName = "Get Bank Guarantee History"
	
	Public Const ACSELBankGuaranteeBranchesSQL As String = "spu_partyBG_Branches_Sel"
	Public Const ACSELBankGuaranteeBranchesName As String = "Get Bank Guarantee Branches"
	
	Public Const ACSELBankGuaranteeProductsSQL As String = "spu_partyBG_Products_Sel"
	Public Const ACSELBankGuaranteeProductsName As String = "Get Bank Guarantee Products"
	
	Public Const ACSELBankGuaranteeDetailsByIdSQL As String = "spu_BankGuarantee_Details_ByID"
	Public Const ACSELBankGuaranteeDetailsByIdName As String = "Select Bank Guarantee Details By ID"
	
	Public Const ACGetLookupsByEffectiveDateName As String = "Returns lookups by effective date"
	Public Const ACGetLookupsByEffectiveDateSQL As String = "spu_SIR_Get_Lookup_Values_By_Effective_Date"
	
	Public Const ACADDBranchSQL As String = "spu_BankGuarantee_PLSBranch"
	Public Const ACADDBranchName As String = "Add Branch to BG"
	
	Public Const ACADDProductName As String = "Add Product to BG"
	Public Const ACADDProductSQL As String = "spu_BankGuarantee_PLSProduct"
	
	Public Const ACDELBranchSQL As String = "spu_BankGuarantee_PLDBranch"
	Public Const ACDELBranchName As String = "Delete Branch to BG"
	
	Public Const ACUPDBGStatusSQL As String = "spu_Update_BG_Status"
	Public Const ACUPDBGStatusName As String = "Delete Branch to BG"
	
	Public Const ACDELProductName As String = "Delete Product to BG"
	Public Const ACDELProductSQL As String = "spu_BankGuarantee_PLDProduct"
	
	Public Const ACSELValidBGsForPolicySQL As String = "spu_get_BGs_for_policy"
	Public Const ACSELValidBGsForPolicyName As String = "Select Valid Bank Guarantee Details"
	
	Public Const ACADDPolicyBankGuaranteeSQL As String = "spu_PolicyBG_Details_Add"
	Public Const ACADDPolicyBankGuaranteeName As String = "Make Live Policy on BG"
	
	Public Const ACSELBankGuaranteePolicyForReceiptSQL As String = "spu_get_policies_on_BG_for_receipt"
	Public Const ACSELBankGuaranteePolicyForReceiptName As String = "Select BG Policy Details For Receipt"
	
	Public Const ACSELCashListItemIdsForBGSQL As String = "spu_get_cashlistitem_for_bg"
	Public Const ACSELCashListItemIdsForBGName As String = "Select Cash List Items for BG"
	
	Public Const ACUPDCashListItemIdsForBGSQL As String = "spu_update_cashlistitem_for_bg"
	Public Const ACUPDCashListItemIdsForBGName As String = "Select Cash List Items for BG"
End Module