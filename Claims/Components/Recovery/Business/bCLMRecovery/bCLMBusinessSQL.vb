Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Class Name:  BusinessSQL
	' Date:        24/08/2000
	' Description: Contains the SQL Statements required by the
	'              bRecovery.Business class.
	' ***************************************************************** '
	
	
	' ***************************************************************** '
	' RECOVERIES
	' ***************************************************************** '
	Public Const ACGetRecoveryStored As Boolean = True
	Public Const ACGetRecoveryName As String = "SelectAllRecovery"
	Public Const ACGetRecoverySQL As String = "spu_recovery_saa"
	
	Public Const ACAddRecoveryStored As Boolean = True
	Public Const ACAddRecoveryName As String = "AddRecovery"
	Public Const ACAddRecoverySQL As String = "spu_recovery_add"
	
	Public Const ACDeleteRecoveryStored As Boolean = True
	Public Const ACDeleteRecoveryName As String = "DeleteRecovery"
	Public Const ACDeleteRecoverySQL As String = "spu_recovery_del"
	
	Public Const ACUpdateRecoveryStored As Boolean = True
	Public Const ACUpdateRecoveryName As String = "UpdateRecovery"
	Public Const ACUpdateRecoverySQL As String = "spu_recovery_upd"
	
	Public Const ACBalanceRecoveryStored As Boolean = True
	Public Const ACBalanceRecoveryName As String = "BalanceAllRecovery"
	Public Const ACBalanceRecoverySQL As String = "spu_recovery_balance"
	
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	Public Const ACUpdatePartyLinkStored As Boolean = True
	Public Const ACUpdatePartyLinkName As String = "UpdatePartyLink"
	Public Const ACUpdatePartyLinkSQL As String = "spu_Recovery_Party_Link_upd"
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
	
	' ***************************************************************** '
	' PAYMENTS
	' ***************************************************************** '
	Public Const ACAddPaymentStored As Boolean = True
	Public Const ACAddPaymentName As String = "AddPayment"
	Public Const ACAddPaymentSQL As String = "spu_payment_add"
	
	Public Const ACDelPaymentStored As Boolean = True
	Public Const ACDelPaymentName As String = "AddPayment"
	Public Const ACDelPaymentSQL As String = "spu_payment_del"
	
	
	' ***************************************************************** '
	' RECEIPTS
	' ***************************************************************** '
	Public Const ACAddReceiptStored As Boolean = True
	Public Const ACAddReceiptName As String = "AddReceipt"
	Public Const ACAddReceiptSQL As String = "spu_receipt_add"
	
	Public Const ACDelReceiptStored As Boolean = True
	Public Const ACDelReceiptName As String = "AddReceipt"
	Public Const ACDelReceiptSQL As String = "spu_receipt_del"
	
	Public Const ACUpdReceiptStored As Boolean = True
	Public Const ACUpdReceiptName As String = "AddReceipt"
	Public Const ACUpdReceiptSQL As String = "spu_receipt_upd"
	
	
	' ***************************************************************** '
	' COINSURANCE / REINSURANCE
	' ***************************************************************** '
	
	' Add reinsurance values
	Public Const ACAddReinsuranceDetailsSQL As String = "spu_claims_recovery_reins_allocate"
	Public Const ACAddReinsuranceDetailsName As String = "AddReinsuranceDetails"
	Public Const ACAddReinsuranceDetailsStored As Boolean = True
	
	' Select all recovery coinsurance details
	Public Const ACGetCoinsurerDetailsSQL As String = "spu_claims_recovery_coins_saa"
	Public Const ACGetCoinsurerDetailsName As String = "SelectCoinsuranceRecoveries"
	Public Const ACGetCoinsurerDetailsStored As Boolean = True
	
	' Select all recovery reinsurance details
	Public Const ACGetReinsurerDetailsSQL As String = "spu_claims_recovery_reins_saa"
	Public Const ACGetReinsurerDetailsName As String = "SelectReinsuranceRecoveries"
	Public Const ACGetReinsurerDetailsStored As Boolean = True
	
	
	' ***************************************************************** '
	' CLAIMS
	' ***************************************************************** '
	' Close the claim
	Public Const ACCloseClaimSQL As String = "spu_CloseClaim"
	Public Const ACCloseClaimName As String = "CloseClaimName"
	Public Const ACCloseClaimStored As Boolean = True
	
	' Delete any  Claim records
	Public Const ACDeleteClaimSQL As String = "spu_delete_claim"
	Public Const ACDeleteClaimName As String = "Delete  Claim"
	Public Const ACDeleteClaimStored As Boolean = True
	
	Public Const ACGetClientAgentSQL As String = "spu_claim_get_clientagent"
	Public Const ACGetClientAgentName As String = "GetClaimClientAgent"
	Public Const ACGetClientAgentStored As Boolean = True
	
	' Get current outstanding reserve and recovery amounts prior to close claim
	Public Const ACGetCurrentReserveRecoverySQL As String = "spu_GetCurrentReserveRecovery"
	Public Const ACGetCurrentReserveRecoveryName As String = "GetCurrentReserveRecovery"
	Public Const ACGetCurrentReserveRecoveryStored As Boolean = True
	
	' Get original claim id from  claim
	Public Const ACGetOriginalClaimIDSQL As String = "spu_CLM_Get_Base_Claim"
	Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
	
	
	' Select CLMPeril SQL
	Public Const ACGetPerilDetailsSQL As String = "spu_get_Peril_details"
	Public Const ACGetPerilDetailsName As String = "SelectCLMPeril"
	Public Const ACGetPerilDetailsStored As Boolean = True
	
	Public Const ACGetRecoveryTypesSQL As String = "spu_recovery_type_saa"
	Public Const ACGetRecoveryTypesName As String = "GetRecoveryTypes"
	Public Const ACGetRecoveryTypesStored As Boolean = True
	
	' ***************************************************************** '
	' TAXES
	' ***************************************************************** '
	' Load tax types and bands
	Public Const ACGetTaxTypesBandsSQL As String = "spu_Get_Tax_Types_and_Bands"
	Public Const ACGetTaxTypesBandsName As String = "GetTaxTypesBands"
	Public Const ACGetTaxTypesBandsStored As Boolean = True
	
	'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.3.5.6.1)
	Public Const ACGetAttachedPartiesOnClaimSQL As String = "spu_Get_Attached_Parties_On_Claim"
	Public Const ACGetAttachedPartiesOnClaimName As String = "GetAttachedPartiesOnClaim"
	Public Const ACGetAttachedPartiesOnClaimStored As Boolean = True
	'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.3.5.6.1)
End Module