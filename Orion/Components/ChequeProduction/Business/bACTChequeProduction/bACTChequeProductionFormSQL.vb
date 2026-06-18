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
    ' Date: 07/02/01
    '
    ' Description: Contains the SQL Statements required by the
    '              bACTChequeProduction.Form class.
    '
    ' Edit History:
    ' ***************************************************************** '


    'SQL Statements


    ' Select Cheques SQL
    Public Const ACSelectAllStored As Boolean = True
    Public Const ACSelectAllName As String = "SelectPrintedCheque"
    Public Const ACSelectAllSQL As String = "spu_ACT_select_Cheque"

    ' Select By Bank SQL
    Public Const ACSelectBankStored As Boolean = True
    Public Const ACSelectBankName As String = "SelectPrintedCheque"
    Public Const ACSelectBankSQL As String = "spu_ACT_select_Bank_Cheque"

    ' Add Cheque SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCheque"
    'Developer Guide No 39
    'Starts
    Public Const ACAddSQL As String = "spu_ACT_add_Cheque"

    ' Delete Transdetail SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteCheque"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_Cheque"

    ' Update Cheque SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCheque"
    Public Const ACUpdateSQL As String = "spu_ACT_update_Cheque"

    ' Update Cash List SQL
    Public Const ACUpdateCashListStored As Boolean = True
    Public Const ACUpdateCashListName As String = "UpdateCashList"
    Public Const ACUpdateCashListSQL As String = "spu_ACT_update_CashlistItem_Cheque"

    ' Select PartyCnt from Party & Source IDs
    Public Const ACGetPartyCntFromKeyStored As Boolean = False
    Public Const ACGetPartyCntFromKeyName As String = "PartyKeyFromPartyCnt"
    Public Const ACGetPartyCntFromKeySQL As String = "SELECT party_cnt FROM Party WHERE source_id = {source_id} AND party_id = {party_id}"

    ' Check Duplicate Cheque SQL
    Public Const ACCheckDuplicateChequeStored As Boolean = True
    Public Const ACCheckDuplicateChequeName As String = "CheckDuplicateCheque"
    Public Const ACCheckDuplicateChequeSQL As String = "spu_ACT_Check_Duplicate_Cheque"


    ' Update Cheque Printed SQL
    Public Const ACUpdateChequePrintedStored As Boolean = True
    Public Const ACUpdateChequePrintedName As String = "UpdateChequePrinted"
    Public Const ACUpdateChequePrintedSQL As String = "spu_ACT_Update_Cheque_Printed"
    'Ends
    'Get Start Cheque Number
    Public Const ACGetBankStartChequeNumberStored As Boolean = True
    Public Const ACGetBankStartChequeNumberName As String = "GetStartChequeNumber"
    Public Const ACGetBankStartChequeNumberSQL As String = "spu_get_bank_start_chequenumber"

    'Get Bank Highest Issued Cheque Number
    Public Const ACGetBankHighestIssuedChequeNumberStored As Boolean = True
    Public Const ACGetBankHighestIssuedChequeNumberName As String = "GetHighestIssuedChequeNumber"
    Public Const ACGetBankHighestIssuedChequeNumberSQL As String = "spu_get_bank_highest_issued_chequenumber"

    'Select User Authorities
    Public Const ACCanOverrideChequeNumberStored As Boolean = True
    Public Const ACCanOverrideChequeNumberName As String = "CanOverrideChequeNumber"
    Public Const ACCanOverrideChequeNumberSQL As String = "spe_User_Authorities_Sel"

    'Select Bank Cheque Sequence
    Public Const ACSelBankChequeSequenceStored As Boolean = True
    Public Const ACSelBankChequeSequenceName As String = "SelBankChequeSequence"
    Public Const ACSelBankChequeSequenceSQL As String = "spu_get_bank_cheque_sequence"

    'PN 47972 Get user Printer from PMuser table
    Public Const ACGetUserPrinterStored As Boolean = True
    Public Const ACGetUserPrinterName As String = "Get User Printer Name"
    Public Const ACGetUserPrinterSQL As String = "spu_Get_User_Printer"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module