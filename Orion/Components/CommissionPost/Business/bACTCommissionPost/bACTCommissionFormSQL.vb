Option Strict Off
Option Explicit On
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
    '              bACTCashListItem.Form class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Select Commission Amount SQLs

    Public Const ACSelectCommissionForDebitStored As Boolean = False
    Public Const ACSelectCommissionForDebitName As String = "SelectCommissionForDebit"

    'eck290601
    'Developer Guide No 39
    'Start
    'PN11230 eck 25032004 - use the stored procedure !!!
    Public Const ACSelectCommissionForEffectiveStored As Boolean = True
    Public Const ACSelectCommissionForEffectiveName As String = "SelectCommissionForEffective"
    Public Const ACSelectCommissionForEffectiveSQL As String = "spu_ACT_Sel_Comm_for_effective"

    Public Const ACSelectCommissionForClientStored As Boolean = True
    Public Const ACSelectCommissionForClientName As String = "SelectCommissionForClient"
    'AR20041119 - PN11221
    Public Const ACSelectCommissionForClientSQL As String = "spu_ACT_Sel_Comm_for_client"
    'eck100203
    Public Const ACSelectCommissionForClientDIDStored As Boolean = True
    Public Const ACSelectCommissionForClientDIDName As String = "SelectCommissionForClientDID"
    Public Const ACSelectCommissionForClientDIDSQL As String = "spu_ACT_Sel_Comm_for_client_did"

    'DC260606 Datasure
    Public Const ACSelectCommissionTaxStored As Boolean = True
    Public Const ACSelectCommissionTaxName As String = "SelectCommissionTax"
    Public Const ACSelectCommissionTaxSQL As String = "spu_ACT_Sel_Comm_Tax"

    Public Const ACSelectCommissionForInsurerStored As Boolean = True
    Public Const ACSelectCommissionForInsurerName As String = "SelectCommissionForInsurer"
    Public Const ACSelectCommissionForInsurerSQL As String = "spu_ACT_Sel_Comm_for_insurer"

    Public Const ACSelectCommissionForDIDStored As Boolean = True
    Public Const ACSelectCommissionForDIDName As String = "SelectCommissionForDID"
    Public Const ACSelectCommissionForDIDSQL As String = "spu_ACT_Sel_Comm_for_DID"


    Public Const ACSelectIsCommissionEarnedStored As Boolean = True
    Public Const ACSelectIsCommissionEarnedName As String = "SelectIsCommissionEarned"
    Public Const ACSelectIsCommissionEarnedSQL As String = "spu_ACT_Sel_Comm_Earned"

    'eck040501
    Public Const ACSelectHasCommissionMovedStored As Boolean = True
    Public Const ACSelectHasCommissionMovedName As String = "SelectHasCommissionMoved"
    Public Const ACSelectHasCommissionMovedSQL As String = "spu_ACT_Sel_Commission_Moved"

    'DC260606
    Public Const ACSelectHasTaxMovedStored As Boolean = True
    Public Const ACSelectHasTaxMovedName As String = "SelectHasCommissionMoved"
    Public Const ACSelectHasTaxMovedSQL As String = "spu_ACT_Sel_Tax_Moved"

    'eck241001
    Public Const ACSelectIsInsurerStored As Boolean = True
    Public Const ACSelectIsInsurerName As String = "SelectIsInsurer"
    Public Const ACSelectIsInsurerSQL As String = "spu_ACT_Do_IsInsurer"

    'PN11230 eck250302 (was 'eck241001)
    Public Const ACSelectBrokeragePaymentsStored As Boolean = True
    Public Const ACSelectBrokeragePaymentsName As String = "SelectBrokeragePayments"
    Public Const ACSelectBrokeragePaymentsSQL As String = "spu_ACT_Sel_Paid_Brokerage_Trans"

    'TR09012003 - TS219 Auto Batch Payment Run
    Public Const ACSelectFilteredTransDetailsStored As Boolean = True
    Public Const ACSelectFilteredTransDetailsName As String = "SelectFilteredTransDetails"
    Public Const ACSelectFilteredTransDetailsSQL As String = "spu_ACT_Select_TransDetail_Filter"

    Public Const ACUpdateTransDetailsIsPaidStored As Boolean = True
    Public Const ACUpdateTransDetailsIsPaidName As String = "UpdateTransDetailIsPaidStatus"
    Public Const ACUpdateTransDetailsIsPaidSQL As String = "spu_ACT_Update_TransDetail_IsPaid_Only"

    Public Const ACGetAccountDetailsStored As Boolean = True
    Public Const ACGetAccountDetailsName As String = "SelectAllAccount"
    Public Const ACGetAccountDetailsSQL As String = "spu_ACT_Select_Account"

    Public Const ACGetCashListItemPayTypeStored As Boolean = True
    Public Const ACGetCashListItemPayTypeName As String = "GetCashListItemPaymentTypeID"
    Public Const ACGetCashListItemPayTypeSQL As String = "spu_ACT_Get_CashListItem_PaymentType"

    Public Const ACGetAllocationStatusStored As Boolean = True
    Public Const ACGetAllocationStatusName As String = "GetAllocationStatusID"
    Public Const ACGetAllocationStatusSQL As String = "spu_ACT_Select_AllocationStatusID"

    Public Const ACGetCashListItemPayStatusStored As Boolean = True
    Public Const ACGetCashListItemPayStatusName As String = "GetCashListItemPaymentTypeID"
    Public Const ACGetCashListItemPayStatusSQL As String = "spu_ACT_Select_CashListItem_PaymentStatusID"

    'AAB-24-September-2003
    Public Const ACGetSuspenseDetailsSQL As String = "spu_ACT_Get_Suspended_Trans_Info"
    Public Const ACGetSuspenseDetailsName As String = "GetSuspendedTransDetailInfo"
    Public Const ACGetSuspenseDetailsStored As Boolean = True
    'Ends

    Public Const ACGetCommPayTransDetailTypeSQL As String = "SELECT transdetail_type_id FROM transdetail_type WHERE code = 'COMMPAY'"
    Public Const ACGetCommPayTransDetailTypeName As String = "GetCommPayTransDetailType"
    Public Const ACGetCommPayTransDetailTypeStored As Boolean = False

    'DC260606 Datasure
    Public Const ACGetTaxPayTransDetailTypeSQL As String = "SELECT transdetail_type_id FROM transdetail_type WHERE code = 'TAXPAY'"
    Public Const ACGetTaxPayTransDetailTypeName As String = "GetTaxPayTransDetailType"
    Public Const ACGetTaxPayTransDetailTypeStored As Boolean = False

    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module