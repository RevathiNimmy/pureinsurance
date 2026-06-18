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
    ' Date: 31/03/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              bSirOrionLink.Form class.
    '
    ' Edit History:
    '   14/02/2003 : ISS2153 : Added sql constants for GetAccountFromPartyCnt
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select Party & Source IDs from Party Shortname
    Public Const ACGetPartyKeyFromShortnameStored As Boolean = False
    Public Const ACGetPartyKeyFromShortnameName As String = "PartyKeyFromShortname"
    Public Const ACGetPartyKeyFromShortnameSQL As String = "SELECT party_id, source_id, party_cnt FROM Party WHERE shortname = {shortname}"

    ' Select Party & Source IDs from PartyCnt
    Public Const ACGetPartyKeyFromPartyCntStored As Boolean = False
    Public Const ACGetPartyKeyFromPartyCntName As String = "PartyKeyFromPartyCnt"
    Public Const ACGetPartyKeyFromPartyCntSQL As String = "SELECT party_id, source_id FROM Party WHERE party_cnt = {party_cnt}"

    ' Select PartyCnt from Party & Source IDs
    Public Const ACGetPartyCntFromKeyStored As Boolean = False
    Public Const ACGetPartyCntFromKeyName As String = "PartyKeyFromPartyCnt"
    Public Const ACGetPartyCntFromKeySQL As String = "SELECT party_cnt FROM Party WHERE source_id = {source_id} AND party_id = {party_id}"

    ' Update Accounts Export Status SQL
    Public Const ACUpdateAccExportStatusStored As Boolean = True
    Public Const ACUpdateAccExportStatusName As String = "UpdateAccountsExportStatus"
    'Developer Guide No 39
    Public Const ACUpdateAccExportStatusSQL As String = "spu_sir_upd_acc_export_status"

    ' Update Accounts Export Status on the Journal_Export_Folder table SQL
    Public Const ACUpdateJournalExportStatusStored As Boolean = True
    Public Const ACUpdateJournalExportStatusName As String = "UpdateJournalExportStatus"
    'Developer Guide No 39
    Public Const ACUpdateJournalExportStatusSQL As String = "spu_SIR_Update_Journal_Export_Status"

    ' Get AccountID from PartyCnt
    'PN6169 pass company_id as parameter
    Public Const ACGetAccountFromPartyCntStored As Boolean = True
    Public Const ACGetAccountFromPartyCntName As String = "GetAccountFromPartyCnt"
    'Developer Guide No 39
    Public Const ACGetAccountFromPartyCntSQL As String = "spu_ACT_GetAccountFromPartyCnt"

    ' Get Account Status from AccountID MKW 101203 PN9003
    Public Const ACGetAccountStatusFromAccountIDStored As Boolean = False
    Public Const ACGetAccountStatusFromAccountIDName As String = "AccountStatusFromAccountID"
    Public Const ACGetAccountStatusFromAccountIDSQL As String = "select accountstatus_id from account where account_id={account_id}"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module