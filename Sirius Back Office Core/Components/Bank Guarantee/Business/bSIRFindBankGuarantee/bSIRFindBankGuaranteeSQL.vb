Option Strict Off
Option Explicit On
Imports System
Module bSIRFindBankGuaranteeSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	' ***************************************************************** '
	' Class Name: FindClaimSQL
	'
	' Date: 15/07/2000
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate an FindClaim
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	Public Const kGetBankGuarenteeDetailsName As String = "Checks if a claim is set as information only"
	Public Const kGetBankGuarenteeDetailsSQL As String = "spu_bank_guarantee_sel"
	
	Public Const ACSELBankGuaranteeBranchesSQL As String = "spu_partyBG_Branches_Sel"
	Public Const ACSELBankGuaranteeBranchesName As String = "Get Bank Guarantee Branches"
	
	Public Const ACSELBankGuaranteeProductsSQL As String = "spu_partyBG_Products_Sel"
	Public Const ACSELBankGuaranteeProductsName As String = "Get Bank Guarantee Products"
	
	Public Const ACGetLookupsByEffectiveDateName As String = "Returns lookups by effective date"
	Public Const ACGetLookupsByEffectiveDateSQL As String = "spu_SIR_Get_Lookup_Values_By_Effective_Date"
	
	Public Const ACGetValidPartyBGDetailsName As String = ""
	Public Const ACGetValidPartyBGDetailsSQL As String = "spu_party_bg_details_sel"
	
	'Start - Sankar - Bank Guarantee Bug Fixing
	Public Const ACGetLookupShortnameSQL As String = "spu_SAM_Get_Party_ShortName"
	Public Const ACGetLookupShortnameName As String = "Get Party Short Name"
	'End - Sankar - Bank Guarantee Bug Fixing
End Module