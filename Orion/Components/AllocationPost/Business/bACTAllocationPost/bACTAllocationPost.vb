Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  27th September 1996
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' RAW 12/03/2003 : ISS2893 : change sql for ReverseAllocations
    ' RAW 15/05/2003 : CQ954 : added new sql
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '



    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bAllocationPost"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.

    ' Password.

    ' Calling Application

    ' Source ID

    ' Language ID

    ' Log Level

    ' Currency ID


    'SQL Statements sw payment maintenance 14-11-2002
    ' RAW 12/03/2003 : ISS2893 : increased parameters to 3 and added return value
    'developer guide no. 39
    Public Const ACReverseAllocationSQL As String = "spu_ACT_Do_Allocation_Reversal"
    Public Const ACReverseAllocationName As String = "DoReverseAllocation"
    Public Const ACReverseAllocationStored As Boolean = True

    ' RAW 12/03/2003 : ISS2893 : increased parameters to 3
    'SQL Statements sw payment maintenance 14-11-2002
    'developer guide no. 39
    Public Const ACSelectReverseAllocationSQL As String = "spu_ACT_Select_ReverseAllocation"
    Public Const ACSelectReverseAllocationName As String = "SelectReverseAllocation"

    ' RAW 15/05/2003 : CQ954 : added
    'developer guide no. 39
    Public Const ACDoAllocationDetailPairsExistSQL As String = "spu_ACT_Get_DoAllocationDetailPairsExist"
    Public Const ACDoAllocationDetailPairsExistName As String = "DoAllocationDetailPairsExist"
    Public Const ACDoAllocationDetailPairsExistStored As Boolean = True

    ' Alix - 03/03/2003
    ' RAW 12/03/2003 : ISS2893 : removed
    'Public Const ACSelectReverseAllocationTransDetailIDName = "SelectReverseAllocationFromTransDetailID"

    'SD 09/01/2003
    Public Const ACCreditControlOptionNo As String = "5001"
    Public Const ACValueWhenCreditControlSet As String = "1"
    'developer guide no. 39
    Public Const ACReAddCredControlItemSQL As String = "spu_ACT_ReAdd_Credit_Control_Item"
    Public Const ACReAddCredControlItemName As String = "ReAddCreditControlItem"
    Public Const ACReAddCredControlItemStored As Boolean = True

    'SMJB CQ2189 13/08/03
    'developer guide no. 39
    Public Const ACGetDebitAllocationIDSQL As String = "spu_ACT_Get_DR_Allocation_Detail_IDs"
    Public Const ACGetDebitAllocationIDName As String = "GetDebitAllocationIDs"
    Public Const ACGetDebitAllocationIDStored As Boolean = True

    'developer guide no. 39
    Public Const ACGetSWDDocumentsInAllocationSQL As String = "spu_ACT_Get_SWD_Documents_In_Allocation"
    Public Const ACGetSWDDocumentsInAllocationName As String = "GetSWDDocumentsInAllocation"
    Public Const ACGetSWDDocumentsInAllocationStored As Boolean = True

    'FSA Phase 3.2

    Public Const ACGetTransdetailIDsInAllocationSQL As String = "SELECT transdetail_id from allocationdetail where allocation_id = {allocation_id}"
    Public Const ACGetTransdetailIDsInAllocationName As String = "GetTransdetailIdsInAllocation"
    Public Const ACGetTransdetailIDsInAllocationStored As Boolean = False
	
	Public Const ACInsertVoidReversalDetailSQL As String = "Insert into VOID_REVERSE_TRANSACTION_LOG_DETAIL(reverse_transaction_log_id,allocation_id,account_id,is_reverse_allocated) " &
															"Values({LogId} ,{Allocation_id},{account_id},NULL)"
	Public Const ACInsertVoidReversalDetailName As String = "InsertVoidReversalDetail"
    Public Const ACInsertVoidReversalDetailStored As Boolean = False

    Sub Main_Renamed()

        ' Main entry point for the component
        'Test

    End Sub
End Module