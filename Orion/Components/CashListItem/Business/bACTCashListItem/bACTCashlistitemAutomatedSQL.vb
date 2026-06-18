Option Strict Off
Option Explicit On
Module AutomatedSQL
    ' ***************************************************************** '
    ' Class Name: AutomatedSQL
    '
    ' Date: 07/04/1998
    '
    ' Description: Contains the SQL Statements required by the 
    '              bACTCashlistitem.Automated class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACAutoSelectStored = False
    ' Public Const ACAutoSelectName = "SelectRisk"
    ' Public Const ACAutoSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select Cashlistitem SQL
    Public Const ACAutoGetDetailsStored As Boolean = True
    Public Const ACAutoGetDetailsName As String = "SelectAllCashlistitem"
    'developer guide no.39
    Public Const ACAutoGetDetailsSQL As String = "spu_ACT_select_{NewTable}"

    ' Select All Cashlistitem SQL
    Public Const ACAutoGetAllDetailsStored As Boolean = True
    Public Const ACAutoGetAllDetailsName As String = "SelectAllCashlistitem"
    'developer guide no.39
    Public Const ACAutoGetAllDetailsSQL As String = "spu_ACT_select_all_{NewTable}"
    ' Check ID SQL
    Public Const ACAutoCheckIDStored As Boolean = True
    Public Const ACAutoCheckIDName As String = "CheckCashlistitemID"
    'developer guide no.39
    Public Const ACAutoCheckIDSQL As String = "spu_ACT_check_{NewTable}"

    ' Add Cashlistitem SQL
    Public Const ACAutoAddStored As Boolean = True
    Public Const ACAutoAddName As String = "AddCashlistitem"
    'developer guide no.39
    Public Const ACAutoAddSQL As String = "spu_ACT_add_{NewTable}"

    ' Delete Cashlistitem SQL
    Public Const ACAutoDeleteStored As Boolean = True
    Public Const ACAutoDeleteName As String = "DeleteCashlistitem"
    'developer guide no.39
    Public Const ACAutoDeleteSQL As String = "spu_ACT_delete_{NewTable}"

    ' Update Cashlistitem SQL
    Public Const ACAutoUpdateStored As Boolean = True
    Public Const ACAutoUpdateName As String = "UpdateCashlistitem"
    'developer guide no.39
    Public Const ACAutoUpdateSQL As String = "spu_ACT_update_{NewTable}"

    'Get Document Details from Transdetail
    Public Const ACGetDocumentFromTransdetailSQL As String = "spu_ACT_Get_Document_From_Transdetail"
    Public Const ACGetDocumentFromTransdetailName As String = "GetDocumentFromTransdetail"
    Public Const ACGetDocumentFromTransdetailStored As Boolean = True

    ' Get Policy Details From Claim Payment
    Public Const ACGetPolicyDetailsFromClaimPaymentStored As Boolean = True
    Public Const ACGetPolicyDetailsFromClaimPaymentName As String = "GetPolicyDetailsFromClaimPayment"
    Public Const ACGetPolicyDetailsFromClaimPaymentSQL As String = "spu_CLM_Get_Policy_Details_From_Claim_Payment"

End Module