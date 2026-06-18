Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    '
    ' Description: Contains the SQL Statements required by the
    '              bSirOrionUpdate.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements


    ' Select Party & Source IDs from Party Shortname
    Public Const ACGetPartyKeyFromShortnameStored As Boolean = False
    Public Const ACGetPartyKeyFromShortnameName As String = "PartyKeyFromShortname"
    Public Const ACGetPartyKeyFromShortnameSQL As String = "SELECT party_id, source_id FROM Party WHERE shortname = {shortname}"

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
    'Modified by Deepak Sharma on 5/13/2010 12:36:59 PM refer developer guide no. 39(Guide)
    'Public Const ACUpdateAccExportStatusSQL As String = "{call spu_sir_upd_acc_export_status (?,?)}"
    Public Const ACUpdateAccExportStatusSQL As String = "spu_sir_upd_acc_export_status"
End Module