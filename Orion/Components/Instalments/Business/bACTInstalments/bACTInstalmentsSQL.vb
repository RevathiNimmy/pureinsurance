Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
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
	' Date: 31/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTCompany.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    'Developer Guide No 39
    'Start
	' Select Account From Party
	Public Const ACGetAccountFromPartyStored As Boolean = True
	Public Const ACGetAccountFromPartyName As String = "GetACcountIDFromPartyCnt"
	'PN6169 pass extra parameter
    Public Const ACGetAccountFromPartySQL As String = "spu_ACT_Get_AccountID_From_PartyCnt"

    ' Select Account From TransDetail
    Public Const ACGetTransDetailStored As Boolean = True
    Public Const ACGetTransDetailName As String = "GetAccountIDFromTransdetail"
    Public Const ACGetTransDetailSQL As String = "spu_ACT_Select_TransDetail"

    ' Select Account From ShortCode
    Public Const ACGetAccountFromShortCodeStored As Boolean = True
    Public Const ACGetAccountFromShortCodeName As String = "GetAccountIDFromShortCode"
    Public Const ACGetAccountFromShortCodeSQL As String = "spu_ACT_Get_AccountID_From_ShortCode"

    Public Const ACGetCCIFromInstalmentSQL As String = "spu_ACT_Select_Credit_Control_Item_For_Plan"
    Public Const ACGetCCIFromInstalmentName As String = "GetCCIFromInstalment"
    Public Const ACGetCCIFromInstalmentStored As Boolean = True

    'sw Accounting for ISF 02/07/2003
    Public Const ACCancelPlanInsFeesSplitByCOBSQL As String = "spu_ACT_CancelPlan_InsFees_Split_By_COB"
    Public Const ACCancelPlanInsFeesSplitByCOBName As String = "CancelPlanWhenInstalmentFeesSplitByCOB"

    'sw Accounting for ISF 02/07/2003
    Public Const ACCopyStatsFolderAndSettleSQL As String = "spu_ACT_Copy_Stats_Details"
    Public Const ACCopyStatsFolderAndSettleName As String = "CopyStatsFolderAndSettle"

    'SMJB 17/09/03 CQ 1733
    Public Const ACSelectCreditTransDetailName As String = "GetCRTransDetail"
    Public Const ACSelectCreditTransDetailSQL As String = "spu_ACT_Get_CR_TransDetail_For_Instalment_Settlement"
    Public Const ACSelectCreditTransDetailStored As Boolean = True

    ' START CHANGES - Changed By: AAB  - Changed On: 19-September-2003   ****
    ' To support RI Dripping
    Public Const ACGetRISuspenseInfoStored As Boolean = True
    Public Const ACGetRISuspenseInfoName As String = "Get ReInsurance Suspense Flag and Account"
    Public Const ACGetRISuspenseInfoSQL As String = "spu_PFGetRISuspenseInfo"

    Public Const ACGetSuspendedPartiesStored As Boolean = True
    Public Const ACGetSuspendedPartiesName As String = "Get ReInsuarnce & FAC Parties to use for suspense"
    Public Const ACGetSuspendedPartiesSQL As String = "spu_PFGetRIAccountInfo"

    Public Const ACGetSuspendedTransactionsStored As Boolean = True
    Public Const ACGetSuspendedTransactionsName As String = "Get ReInsurance Suspended Transactions For a specific party"
    Public Const ACGetSuspendedTransactionsSQL As String = "spu_PFGetPartyTransactionsToSuspend"

    Public Const ACCreatePFAccountsTransactionsStored As Boolean = True
    Public Const ACCreatePFAccountsTransactionsName As String = "Insert Into PF_Accounts_Transactions Table"
    Public Const ACCreatePFAccountsTransactionsSQL As String = "spu_PFAccountsTransactions_Add"

    Public Const ACUpdatePFAccountsTransactionsStored As Boolean = True
    Public Const ACUpdatePFAccountsTransactionsName As String = "Update PF_Accounts_Transactions Table"
    Public Const ACUpdatePFAccountsTransactionsSQL As String = "spu_PFAccountsTransactions_Update"

    Public Const ACGetPFFromInstalmentsIDStored As Boolean = True
    Public Const ACGetPFFromInstalmentsIDSQL As String = "spu_ACT_GetPFFromInstalmentsID"
    Public Const ACGetPFFromInstalmentsIDName As String = "Get PF Scheme Proeprties for the Instalment ID"

    'FSA Phase 3.2 Removed Transaction Type Parameter
    Public Const ACGetSuspenseDetailsSQL As String = "spu_ACT_GetDetailsFor_PFTransactions"
    Public Const ACGetSuspenseDetailsName As String = "Get Details for releasing all transaction types"
    Public Const ACGetSuspenseDetailsStored As Boolean = True

    Public Const ACGetLasInstalmentIDStored As Boolean = True
    Public Const ACGetLasInstalmentIDName As String = "Get Last Instalment ID for given PFCnt & Version"
    Public Const ACGetLasInstalmentIDSQL As String = "spu_PFGetLastInstalmentID"
    'FSA Phase 3.2
    Public Const ACGetTransdetailTypeIDStored As Boolean = False
    Public Const ACGetTransdetailTypeIDName As String = "Get Transdetail Type for Suspended Commission"
    Public Const ACGetTransdetailTypeIDSQL As String = "SELECT transdetail_type_id from transdetail_type where code = 'COMSUSP'"

    Public Const ACGetSourceIDFromPlanStored As Boolean = False
    Public Const ACGetSourceIDFromPlanName As String = "GetSourceIDFromPlan"
    Public Const ACGetSourceIDFromPlanSQL As String = "SELECT" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "    source_id," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "    sub_branch_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "FROM PFPremiumFinance" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "WHERE pfprem_finance_cnt = {pfprem_finance_cnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "AND pfprem_finance_version = {pfprem_finance_version}"

    Public Const kGetTransDetailDocumentTypeName As String = "Get the document type for the specified transdetail id"
    Public Const kGetTransDetailDocumentTypeSQL As String = "spu_ACT_Get_TransDetail_DocumentType"

    Public Const ACGetAmountNotIncludedInInstalmentStored As Boolean = True
	Public Const ACGetAmountNotIncludedInInstalmentName As String = "Get Amount Not Included In Instalment"
    Public Const ACGetAmountNotIncludedInInstalmentSQL As String = "spu_Get_Amount_Not_Included_In_Instalment"
	
    Public Const ACGetOutstandingTaxAmount As String = "Spu_ACT_Get_Outstanding_Tax_Amount"
	Public Const ACGetOutstandingTaxAmountName As String = "Get Outstanding Tax Amount"
	
	Public Const ACGetRISUSPTransdetailTypeIDStored As Boolean = True
	Public Const ACGetRISUSPTransdetailTypeIDName As String = "Get Transdetail Type for Suspended Reinsurance"
    Public Const ACGetRISUSPTransdetailTypeIDSQL As String = "spu_ACT_Get_TransDetail_Type_Id"

    Public Const ACGetCurrentPeriodFromLegderStored As Boolean = True
    Public Const ACGetCurrentPeriodFromLegderName As String = "Get Current Period From Legder"
    Public Const ACGetCurrentPeriodFromLegderSQL As String = "spu_ACT_Get_Period_Id_From_ledger"

    Public Const ACGetAccountDetailsForInsuranceStored As Boolean = True
    Public Const ACGetAccountDetailsForInsuranceFileName As String = "Get Account Details For InsuranceFile"
    Public Const ACGetAccountDetailsForInsuranceFileSQL As String = "spu_Get_Account_Details_For_InsuranceFile"

    Public Const ACSettlePlanCalculateRefundSQL As String = "spu_PFPremiumFinance_GetRefundAmount"
    Public Const ACSettlePlanCalculateRefundFileName As String = "Get plan oustanding amount"

    Public Const KGetInsuranceFileTransactionTypeSQL As String = "spu_PFGetInsuranceFile_TransactionType"
    Public Const KGetInsuranceFileTransactionTypeName As String = "GetInsuranceFileTransactionType"

    Public Const ACGetPeriodFromInsuranceFileIDStored As Boolean = False
    Public Const ACGetPeriodFromInsuranceFileIDName As String = "Get period from insurance file"
    Public Const ACGetPeriodFromInsuranceFileIDSQL As String = "select ISNULL(posting_period_id,0) from Insurance_File where Insurance_file_cnt= {InsuranceFileCnt} "

    'Ends
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module
