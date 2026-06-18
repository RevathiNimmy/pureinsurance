Option Strict Off
Option Explicit On
Imports System
Module FindPartySql

    ' ***************************************************************** '
    ' Class Name: FindPartySQL
    '
    ' Date: 27th September 1996
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL) manipulate an FindParty
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectEvent"
    ' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"

    ' Select FindParty by shortname SQL
    Public Const ACPartyLikeShortNameStored As Boolean = True
    Public Const ACPartyLikeShortNameName As String = "FindPartyLikeShortName"
    'developer guide no. 39 (Guide)
    Public Const ACPartyLikeShortNameSQL As String = "spu_FindParty_like_shortname"

    ' Select party_cnt_from_shortname SQL
    Public Const ACPartyFromShortNameStored As Boolean = True
    Public Const ACPartyFromShortNameName As String = "SelectPartyFromShortName"
    'developer guide no. 39 (Guide)
    Public Const ACPartyFromShortNameSQL As String = "spu_select_id_from_shortname"

    'select Select party_cnt_from_name SQL
    Public Const ACPartyFromNameStored As Boolean = True
    Public Const ACPartyFromNameName As String = "SelectPartyFromName"
    'developer guide no. 39 (Guide)
    Public Const ACPartyFromNameSQL As String = "spu_select_id_from_name"

    'select Select party_cnt_from_name SQL
    Public Const ACPartyFromCntStored As Boolean = True
    Public Const ACPartyFromCntName As String = "SelectPartyFromCnt"
    'developer guide no. 39 (Guide)
    Public Const ACPartyFromCntSQL As String = "spu_select_name_from_id"

    'Select party_cnt_from_name SQL
    Public Const ACSelectAccountBalStored As Boolean = True
    Public Const ACSelectAccountBalName As String = "SelectAccountBal"
    'developer guide no. 39 (Guide)
    Public Const ACSelectAccountBalSQL As String = "spu_SAM_ACT_Select_AccountBal"

    'Select party_cnt_from_name SQL
    Public Const ACGetClaimIncurredStored As Boolean = True
    Public Const ACGetClaimIncurredName As String = "GetClaimIncurred"
    'developer guide no. 39 (Guide)
    Public Const ACGetClaimIncurredSQL As String = "spu_Select_Incurred_on_all_claims"

    'TMP Select Sub Agent Allow Consolidated Commission
    Public Const ACPartyCommFromShortNameStored As Boolean = True
    Public Const ACPartyCommFromShortNameName As String = "SelectPartyFromCnt"
    'developer guide no. 39 (Guide)
    Public Const ACPartyCommFromShortNameSQL As String = "spu_select_allow_commission_from_shortname"

    'select party details fromquery
    Public Const ACPartyFromQueryStored As Boolean = False
    Public Const ACPartyFromQueryName As String = "SelectPartyFromQuery"
    Public Const ACPartyFromQuerySQL As String = "{}"

    ' Select party with no filter (stored procedure)
    Public Const ACPartyNoFilterStored As Boolean = True
    Public Const ACPartyNoFilterName As String = "FindPartyNoFilter"
    Public Const ACPartyNoFilterSQL As String = "spu_FindParty_NoFilter"

    ' Get Live Transaction Details SQL
    'eck210102 This code assumed that transaction related to a policy
    'Public Const ACGetLiveTransactionDetailsSQL = "SELECT distinct td.insurance_ref " & _
    ''                "FROM insurance_file ifi, transdetail td " & _
    ''                "WHERE td.insurance_ref = ifi.insurance_ref " & _
    ''                "AND ifi.insured_cnt = {party_cnt} " & _
    ''                "AND td.fully_matched = 0"
    'DC120104 PN9544 -correct way account table now links to party table
    'Public Const ACGetLiveTransactionDetailsSQL = "SELECT distinct td.transdetail_id " & _
    ''                "FROM party p, account a, transdetail td " & _
    ''                "WHERE p.party_Cnt = {party_cnt} " & _
    ''                "AND a.account_key = ((p.source_id - 1) * 268435456) + p.party_id " & _
    ''                "AND a.account_Id = td.account_id " & _
    ''                "AND td.fully_matched = 0"

    Public Const ACGetLiveTransactionDetailsSQL As String = "SELECT distinct td.transdetail_id " & _
                                                            "FROM party p, account a, transdetail td " & _
                                                            "WHERE p.party_Cnt = {party_cnt} " & _
                                                            "AND a.account_key = p.party_cnt " & _
                                                            "AND a.account_Id = td.account_id " & _
                                                            "AND td.fully_matched = 0"

    ' Get Live Policy Details SQL
    Public Const ACGetLivePolicyDetailsStored As Boolean = False
    Public Const ACGetLivePolicyDetailsName As String = "GetLivePolicyDetails"
    Public Const ACGetLivePolicyDetailsSQL As String = "SELECT ifi.insurance_file_cnt " & _
                                                       "FROM insurance_folder ifo, insurance_file ifi " & _
                                                       "WHERE ifo.insurance_holder_cnt = {party_cnt} " & _
                                                       "AND ifo.insurance_folder_cnt = ifi.insurance_folder_cnt " & _
                                                       "AND ifi.insurance_file_status_id IS NULL " & _
                                                       "AND ifi.policy_ignore IS NULL " & _
                                                       "AND ifi.policy_version = ( " & _
                                                       "SELECT max(ifi1.policy_version) FROM insurance_folder ifo1, insurance_file ifi1 " & _
                                                       "WHERE ifo1.insurance_holder_cnt = {party_cnt} AND ifo1.insurance_folder_cnt = ifi1.insurance_folder_cnt) "






    ' Get Shared Premium Details SQL
    Public Const ACGetSharedPremiumDetailsStored As Boolean = False
    Public Const ACGetSharedPremiumDetailsName As String = "GetSharedPremiumDetails"
    Public Const ACGetSharedPremiumDetailsSQL As String = "SELECT ifi.insurance_file_cnt " & _
                                                          "FROM policy_shared_premiums psp, insurance_file ifi, insurance_folder ifo " & _
                                                          "WHERE psp.party_cnt = {party_cnt} " & _
                                                          "AND psp.insurance_file_cnt = ifi.insurance_file_cnt " & _
                                                          "AND ifi.insurance_file_status_id IS NULL " & _
                                                          "AND ifo.insurance_folder_cnt = ifi.insurance_folder_cnt " & _
                                                          "AND ifo.insurance_holder_cnt <> {party_cnt}"

    ' 'Delete' Party SQL
    Public Const ACDeletePartyStored As Boolean = False
    Public Const ACDeletePartyName As String = "DeleteParty"
    Public Const ACDeletePartySQL As String = "UPDATE party " & _
                                              "SET is_deleted = 1 WHERE party_cnt = {party_cnt}"


    ' 'Undelete' Party SQL
    Public Const ACUndeletePartyStored As Boolean = False
    Public Const ACUndeletePartyName As String = "DeleteParty"
    Public Const ACUndeletePartySQL As String = "UPDATE party " & _
                                                "SET is_deleted = 0 WHERE party_cnt = {party_cnt}"

    ' Get associated client count
    Public Const ACGetAssociatedClientCountStored As Boolean = True
    Public Const ACGetAssociatedClientCountName As String = "GetAssociatedClientCount"
    'developer guide no. 39 (Guide)
    Public Const ACGetAssociatedClientCountSQL As String = "spu_select_associated_client_count"

    'DC091204
    ' Get All Visible Agent Types
    Public Const ACGetVisibleAgentTypesStored As Boolean = True
    Public Const ACGetVisibleAgentTypesName As String = "GetAssociatedClientCount"
    'developer guide no. 39 (Guide)
    Public Const ACGetVisibleAgentTypesSQL As String = "spu_get_visible_agent_types"


    Public Const kGetPartyTypeByCodeName As String = "Get Party Type Details By Code"
    Public Const kGetPartyTypeByCodeSQL As String = "spu_SIR_Get_Party_Type_By_Code"

    Public Const kGetClientBlackListingReasonName As String = "returns the blacklist reason for the specified personal client"
    Public Const kGetClientBlackListingReasonSQL As String = "spu_SIR_Party_BlackList_Reason_Sel"

    Public Const ACCheckInsurerAccessStored As Boolean = True
    Public Const ACCheckInsurerAccessName As String = "CheckInsurerAccess"
    'developer guide no. 39 (Guide)
    Public Const ACCheckInsurerAccessSQL As String = "spu_pmuser_insurer_access"

    Public Const kGetInsurerTypeStored As Boolean = True
    Public Const kGetInsurerTypeName As String = "GetInsurerType"
    Public Const kGetInsurerTypeSQL As String = "spu_Get_Insurer_Type"

    Public Const kCheckReceivesCorrespondenceStored As Boolean = False
    Public Const kCheckReceivesCorrespondenceName As String = "CheckReceivesCorrespondence"
    Public Const kCheckReceivesCorrespondenceSQL As String = "select receives_client_correspondence  from party_agent where party_cnt = {party_cnt}"

    Public Const kCheckOtherPartyBranchRecordsStored As Boolean = False
    Public Const kCheckOtherPartyBranchRecordsName As String = "CheckOtherPartyBranchRecords"
    Public Const kCheckOtherPartyBranchRecordsSQL As String = "SELECT COUNT(1) " & "FROM other_party_branch"

End Module