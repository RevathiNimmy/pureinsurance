Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 08/08/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTCashListPost.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	
	' Select OS Transdetail SQL
	Public Const ACGetOSDetailsStored As Boolean = True
    Public Const ACGetOSDetailsName As String = "SelectTransdetail"
    'developer guide no.39
    Public Const ACGetOSDetailsSQL As String = "spu_ACT_select_TransDetail_OS"
	
	'eck310102
	Public Const ACGetPlansStored As Boolean = True
    Public Const ACGetPlansName As String = "GetPlan"
    'developer guide no.39
    Public Const ACGetPlansSQL As String = "spu_ACT_Get_PFPremiumFinance"
	
	Public Const SelPlanInstalmentsStored As Boolean = True
    Public Const SelPlanInstalmentsName As String = "GetInstalment"
    'developer guide no.39
    Public Const SelPlanInstalmentsSQL As String = "spe_PFInstalments_sel"
	
	Public Const PayPlanInstalmentStored As Boolean = True
    Public Const PayPlanInstalmentName As String = "PayInstalment"
    'developer guide no.39
    Public Const PayPlanInstalmentSQL As String = "spu_Pay_PFInstalment"
	
    Public Const ACGetPaymentStatusCodeName As String = "GetPaymentStatusCode"
    'developer guide no.39
    Public Const ACGetPaymentStatusCodeSQL As String = "spu_ACT_Get_Payment_Status_Code"
	
	Public Const ACGetAuditSetTypeStored As Boolean = True
    Public Const ACGetAuditSetTypeName As String = "GetAuditSetType"
    'developer guide no.39
    Public Const ACGetAuditSetTypeSQL As String = "spu_ACT_Select_AuditSetType"
	
	Public Const ACUpdateApproveAuditSetStored As Boolean = True
    Public Const ACUpdateApproveAuditSetName As String = "Approve AuditSetType"
    'developer guide no.39
    Public Const ACUpdateApproveAuditSetSQL As String = "spu_ACT_Do_Approve_AuditSet"
	
    'DJM 09/12/2003
    'developer guide no.39
    Public Const ACGetAllocationTotalSQL As String = "spu_ACT_Select_AllocationTotal_By_Allocation_ID"
	
	'Check for Duplicacy of document_ref
	Public Const ACGetDOCRefSQL As String = "Select * from Document where Document_Ref ={Doc_ref} and company_id={Company_id}"
	
	'Start - Sankar - Bank Guarantee Bug Fixing
    Public Const ACUpdateBGAvailableLimitName As String = "UpdateBGAvailableLimit"
    'developer guide no.39
    Public Const ACUpdateBGAvailableLimitSQL As String = "spu_SAM_Update_Available_Balance_with_BGKey"
	'End - Sankar - Bank Guarantee Bug Fixing
	Public Const ACGetPostingStatusForCashListItemStored As Boolean = True
	Public Const ACGetPostingStatusForCashListItemName As String = "Get Posting Status For CashListItem"
    Public Const ACGetPostingStatusForCashListItemSQL As String = "spu_ACT_Get_PostingStatusForCashListItem"

    Public Const kGetTaxTransdetailidStored As Boolean = True
    Public Const kGetTaxTransdetailidName As String = "Get Tax Transaction"
    Public Const kGetTaxTransdetailidSQL As String = "spu_ACT_Get_Cashlist_Tax"

    Public Const kGetPostingStatusForCashListStored As Boolean = True
    Public Const kGetPostingStatusForCashListName As String = "Get Posting Status For CashListItem"
    Public Const kGetPostingStatusForCashListSQL As String = "spu_ACT_Get_PostingStatusForCashList"

    Public Const kAddRIPaymentRecieptStored As Boolean = True
    Public Const kAddRIPaymentRecieptName As String = "AddRIPaymentReciept"
    Public Const kAddRIPaymentRecieptSQL As String = "spu_act_add_reisurer_payment"

    Public Const kGetAccountIdFormTaxBandIDStored As Boolean = True
    Public Const kGetAccountIdFormTaxBandIDName As String = "GetAccountIdFormTaxBandId"
    Public Const kGetAccountIdFormTaxBandIDSQL As String = "spu_Get_AccountId_Form_TaxBandId"

    Public Const kGetParentNodeIdForTaxStored As Boolean = True
    Public Const kGetParentNodeIdForTaxName As String = "GetParentNodeIdForTax"
    Public Const kGetParentNodeIdForTaxSQL As String = "spu_Get_Parent_Node_ID_For_Tax"

    Public Const kDoAccountExistsStored As Boolean = True
    Public Const kDoAccountExistsName As String = "DoAccountExists"
    Public Const kDoAccountExistsSQL As String = "spu_Get_AccountIdFromShortCode"

    Public Const kGetFinancerAccountIdStored As Boolean = True
    Public Const kGetFinancerAccountIdName As String = "GetFinancerAccountID"
    Public Const kGetFinancerAccountIdSQL As String = "spu_Get_Financer_Account_ID"

    Public Const kGetPolicyTransdetailStored As Boolean = True
    Public Const kGetPolicyTransdetailName As String = "GetPolicyTransdetail"
    Public Const kGetPolicyTransdetailSQL As String = "spu_Get_Policy_Transdetail"

    Public Const ACGetPlanOutstandingAmountSQL As String = "spu_get_plantransaction_outstanding_amount"
    Public Const ACGetPlanOutstandingAmountName As String = "GetPlanOutstandingAmount"

End Module