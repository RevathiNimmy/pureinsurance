Option Strict Off
Option Explicit On
Module ControlTransClaimsSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: ControlTransSQL
    '
    ' Date: 13/03/2001
    '
    ' Description: Contains the SQL Statements to manipulate the Stats and
    '              Transaction Export tables
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements


    Public Const ACAddStatsFolderName As String = "AddStatsFolderClaims"
    Public Const ACAddStatsFolderSQL As String = "spu_add_stats_folder_claims"

    Public Const ACGetStatsFolderName As String = "GetWorkStatsFolderClaims"
    Public Const ACGetStatsFolderSQL As String = "spu_CLM_Get_Stats_Folder"

    Public Const ACAddStatsDetailsName As String = "AddStatsDetailsClaims"
    Public Const ACAddStatsDetailsSQL As String = "spu_add_stats_details_claims"

    Public Const ACAddExportFolderName As String = "AddExportFolderClaims"
    Public Const ACAddExportFolderSQL As String = "spu_add_trans_export_folder_clm"

    Public Const ACAddExportDetailsName As String = "AddExportDetailsClaims"
    Public Const ACAddExportDetailsSQL As String = "spu_add_trans_export_detail_clm"

    Public Const ACFinaliseStatsName As String = "Perform the actions that used to take place in copy work details to live"
    Public Const ACFinaliseStatsSQl As String = "spu_CLM_Finalise_stats"

    Public Const ACAddTransDetailsStored As Boolean = True
    Public Const ACAddTransDetailsName As String = "AddTransdetail"
    Public Const ACAddTransDetailsSQL As String = "spu_ACT_add_TransDetail"

    Public Const ACSelLedgerIDName As String = "SelectLedgerID"
    Public Const ACSelLedgerIDSQL As String = "spu_CLM_Get_Account_Ledger_Details"

    Public Const ACAddDocumentStored As Boolean = True
    Public Const ACAddDocumentName As String = "AddDocument"
    Public Const ACAddDocumentSQL As String = "spu_ACT_add_Document"

    Public Const ACGetStatsFolderDetailsName As String = "GetStatsFolderDetails"
    Public Const ACGetStatsFolderDetailsSQL As String = "spu_CLM_Get_Stats_Folder_Details"

    Public Const kGetAccountIdName As String = "GetAccountID for either account key or short code"
    Public Const kGetAccountIdSQL As String = "spu_CLM_Get_Account_ID"

    Public Const ACSelClaimRefName As String = "GetClaimReferenceNumber"
    Public Const ACSelClaimRefSQL As String = "spu_CLM_Get_Claim_Details"

    Public Const ACUpdTransExportFolderName As String = "UpdateTransactionExportFolder"
    Public Const ACUpdTransExportFolderSQL As String = "spu_ACT_Update_Accounts_Export_Status"

    'RWH(05/07/01)Add stats for coinsurance to work tables.
    Public Const ACAddStatsCoinsStored As Boolean = True
    Public Const ACAddStatsCoinsName As String = "AddStatsCoins"
    Public Const ACAddStatsCoinsSQL As String = "spu_add_claims_stats_details_coins"

    'RWH(05/07/01)Add stats for reinsurance to work tables.
    Public Const ACAddStatsReinsStored As Boolean = True
    Public Const ACAddStatsReinsName As String = "AddStatsReins"
    Public Const ACAddStatsReinsSQL As String = "spu_add_claims_stats_details_reins"

    'RWH(16/07/01)Add transactions for claims.
    Public Const ACAddTransClaimsStored As Boolean = True
    Public Const ACAddTransClaimsName As String = "AddTransClaimsControl"
    Public Const ACAddTransClaimsSQL As String = "spu_add_trans_claims_control"

    Public Const kGetStatsFolderForClaimName As String = "AddTransClaimsControl"
    Public Const kGetStatsFolderForClaimSQL As String = "spu_CLM_Get_Stats_Folder_For_Claim"
End Module