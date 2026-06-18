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
	' Class Name    :BusinessSQL
	' Date          :16/08/2000
	' Description   :Contains the SQL Statements required by the
	'               bCLMPerilType.Business class.
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	Public Const ACCheckAgentCancelledSP As Boolean = True
    Public Const ACCheckAgentCancelledName As String = "CheckAgentCancelled"
    'developer guide no. 39 
    Public Const ACCheckAgentCancelledSQL As String = "spu_check_agent_cancelled"
	
	Public Const ACSelProductStored As Boolean = True
	Public Const ACSelProductName As String = "SelProduct"
    Public Const ACSelProductSQL As String = "spe_Product_Sel"
	
	Public Const ACClaimCurrencyStored As Boolean = True
	Public Const ACClaimCurrencyName As String = "ClaimCurrencyByRefs"
    Public Const ACClaimCurrencySQL As String = "spu_get_claim_currency_by_refs"
	
	Public Const ACUpdatePaymentRatesStored As Boolean = True
	Public Const ACUpdatePaymentRatesName As String = "UpdatePaymentRates"
    Public Const ACUpdatePaymentRatesSQL As String = "spu_update_payment_rates"
	
	Public Const ACUpdateReceiptRatesStored As Boolean = True
	Public Const ACUpdateReceiptRatesName As String = "UpdateReceiptRates"
    Public Const ACUpdateReceiptRatesSQL As String = "spu_update_receipt_rates"
	
	'PS344
	Public Const ACRejectPaymentStored As Boolean = True
	Public Const ACRejectPaymentName As String = "RejectPayment"
    Public Const ACRejectPaymentSQL As String = "spu_reject_payment"
	
	Public Const ACSelectPaymentDetailsStored As Boolean = True
	Public Const ACSelectPaymentDetailsName As String = "SelectPaymentDetails"
    Public Const ACSelectPaymentDetailsSQL As String = "spu_select_payment_details"
	
	'AR20050427 - PN19582
	Public Const ACSelectMediaTypeStored As Boolean = True
	Public Const ACSelectMediaTypeName As String = "SelectMediaType"
    Public Const ACSelectMediaTypeSQL As String = "spu_ACT_Select_MediaType_ByCode"
	
	Public Const kGetClaimBaseCurrencyDetailsName As String = "Returns the base currency id of the branch that is associated with the claims policy version"
    Public Const kGetClaimBaseCurrencyDetailsSQL As String = "spu_CLM_Get_Claim_Base_Currency_Details"
	
	'eck 11/2005
	Public Const kGetCoInsurerDetailsName As String = "Returns CoInsurer Breakdown"
	Public Const kGetCoInsurerDetailsSQL As String = "spu_CLM_Get_CoInsurer_Split"
	
	Public Const kUpdatePaymentDocumentIdStored As Boolean = False
	Public Const kUpdatePaymentDocumentIdName As String = "UpdateClaimPaymentDocumentId"
	Public Const kUpdatePaymentDocumentIdSQL As String = "Update claim_payment SET document_id = {document_id} WHERE claim_payment_id = {claim_payment_id}"
End Module