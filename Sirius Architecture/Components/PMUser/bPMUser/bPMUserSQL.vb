Option Strict Off
Option Explicit On
Imports System
Module PMUserSQL
    ' ***************************************************************** '
    ' Class Name: PMUserSQL
    '
    ' Date: 17 October 1996
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL) manipulate an PMUser
    '
    ' Edit History:
    ' RFC290398 - LoggedOnAtClient added, Timestamp removed.
    ' RFC270398 - Logon and Logoff added.
    ' RFC250398 - Stored Procedure Calls replaced by embedded sql to
    ' RFC250398 - allow Architecture components to work in db's that
    ' RFC250398 - don't support stored procedures.
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectPMUser"
    ' Public Const ACSelectSQL = "SELECT * FROM PMUser WHERE PMUser_id = {PMUser_id}"

    ' Logon SQL
    Public Const ACLogonStored As Boolean = False
    Public Const ACLogonName As String = "LogonPMUser"
    Public Const ACLogonSQL As String = "UPDATE pmuser SET " &
                                        "logged_on_at_client = {logged_on_at_client}, " &
                                        "lastlogin = {lastlogin} " &
                                        "WHERE username = {username}"

    ' Logoff SQL
    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)
    Public Const ACLogoffStored As Boolean = False
    Public Const ACLogoffName As String = "LogoffPMUser"
    Public Const ACLogoffSQL As String = "UPDATE pmuser SET " &
                                         "logged_on_at_client = NULL, " &
                                         "user_config_xml_dataset = {user_config_xml_dataset} " &
                                         "WHERE username = {username}"
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.6.1)

    ' Select PMUser SQL
    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.2)
    Public Const ACGetDetailsStored As Boolean = False
    Public Const ACGetDetailsName As String = "SelectPMUser"
    Public Const ACGetDetailsSQL As String = "SELECT user_id, language_id, username, secure_password, password_change_date, " &
                                             "date_created, lastlogin, logged_on_at_client, party_cnt, is_pmb_link_required, server_printer, is_printer_changeable, is_deleted, effective_date, email_address, " &
                                             "full_name, signature_file, title, initials, telephone_number, extension_number, mobile_number, fax_number, job_title_id, claim_handler_id, party_handler_id, other_party_id, " &
                                             "alternative_identifier, job_basis, percent_hours_worked, sirius_user, date_deleted, user_config_xml_dataset ,is_locked,incorrect_attempt_count,is_temp_password,password ,sso_preferred_username " &
                                             "FROM pmuser WHERE user_id = {user_id}"
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.2)

    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.2)
    ' Select All PMUsers SQL
    Public Const ACGetAllDetailsStored As Boolean = False
    Public Const ACGetAllDetailsName As String = "SelectAllPMUser"
    Public Const ACGetAllDetailsSQL As String = "SELECT user_id, language_id, username, secure_password, password_change_date, " &
                                                "date_created, lastlogin, logged_on_at_client, party_cnt, is_pmb_link_required, server_printer, is_printer_changeable, is_deleted, effective_date, email_address, " &
                                                "full_name, signature_file, title, initials, telephone_number, extension_number, mobile_number, fax_number, job_title_id, claim_handler_id, party_handler_id, other_party_id, " &
                                                "alternative_identifier, job_basis, percent_hours_worked, sirius_user, date_deleted, user_config_xml_dataset ,is_locked,incorrect_attempt_count,is_temp_password,password,sso_preferred_username " &
                                                "FROM pmuser ORDER BY is_deleted ASC, username ASC"
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.5.2)

    ' Add PMUser SQL
    Public Const ACAddStored As Boolean = False
    Public Const ACAddName As String = "AddPMUser"
    Public Const ACAddSQL As String = "INSERT INTO pmuser (user_id, party_cnt, language_id, username, password,secure_password, password_change_date, " &
                                      "date_created, is_deleted, lastlogin, effective_date, is_pmb_link_required, server_printer, is_printer_changeable, email_address, " &
                                      "full_name, signature_file, title, initials, telephone_number, extension_number, mobile_number, fax_number, job_title_id, claim_handler_id, party_handler_id, other_party_id, " &
                                      "alternative_identifier, job_basis, percent_hours_worked, sirius_user, date_deleted,is_temp_password,ModifiedBy,UniqueId,ScreenHierarchy,sso_preferred_username) " &
                                      "SELECT MAX(user_id) + 1, {party_cnt}, {language_id}, {username}, {password}, {secure_password}, {password_change_date}, " &
                                      "{date_created}, {is_deleted}, {lastlogin}, {effective_date}, {is_pmb_link_required}, {server_printer}, {is_printer_changeable}, {email_address}, " &
                                      "{full_name}, {signature_file}, {title}, {initials}, {telephone_number}, {extension_number}, {mobile_number}, {fax_number}, {job_title_id}, {claim_handler_id}, {party_handler_id}, {other_party_id}, " &
                                      "{alternative_identifier}, {job_basis}, {percent_hours_worked}, {sirius_user}, {date_deleted},{is_temp_password}, " &
                                      "{modified_by},{UniqueId}, {ScreenHierarchy} ,{sso_preferred_username} " &
                                      " FROM PMUser"

    ' Delete PMUser SQL
    Public Const ACDeleteStored As Boolean = False
    Public Const ACDeleteName As String = "DeletePMUser"
    Public Const ACDeleteSQL As String = "UPDATE pmuser SET is_deleted = 1 " &
                                         "WHERE user_id = {user_id} "

    ' Update PMUser SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdatePMUser"
    'DC050903
    Public Const ACUpdateSQL As String = "spu_Update_User_Record"

    'DAK150500
    ' Select PMUser id by name SQL
    Public Const ACSelectUserByNameStored As Boolean = False
    Public Const ACSelectUserByNameName As String = "SelectUserByName"
    Public Const ACSelectUserByNameSQL As String = "SELECT user_id " &
                                                   "FROM pmuser WHERE username = {username} "

    'RFC190700
    ' Select PMUser id by PartyCnt SQL
    Public Const ACSelectUserByPartyStored As Boolean = False
    Public Const ACSelectUserByPartyName As String = "SelectUserByParty"
    Public Const ACSelectUserByPartySQL As String = "SELECT user_id , username " &
                                                    "FROM pmuser WHERE party_cnt = {party_cnt} "

    ' RDC 01102002 system option
    Public Const ACGetSystemOptionStored As Boolean = True
    Public Const ACGetSystemOptionName As String = "GetSystemOption"
    'Developer Guide No. 39
    Public Const ACGetSystemOptionSQL As String = "spu_get_system_option"

    'DC100903
    Public Const ACGetUserSourceInfoStored As Boolean = True
    Public Const ACGetUserSourceInfoName As String = "GetUserSourceInfo"
    'Developer Guide No. 39
    Public Const ACGetUserSourceInfoSQL As String = "spu_get_pmuser_source_info"

    'DC100903
    Public Const ACGetUserRiskGroupInfoStored As Boolean = True
    Public Const ACGetUserRiskGroupInfoName As String = "GetUserRiskGroupInfo"
    'Developer Guide No. 39
    Public Const ACGetUserRiskGroupInfoSQL As String = "spu_get_pmuser_riskgroup_info"

    Public Const ACAddUserRiskGroupInfoStored As Boolean = True
    Public Const ACAddUserRiskGroupInfoName As String = "AddUserRiskGroupInfo"
    'Developer Guide No. 39
    Public Const ACAddUserRiskGroupInfoSQL As String = "spu_add_pmuser_riskgroup_info"

    Public Const ACUpdateUserRiskGroupInfoStored As Boolean = True
    Public Const ACUpdateUserRiskGroupInfoName As String = "UpdateUserRiskGroupInfo"
    'Developer Guide No. 39
    Public Const ACUpdateUserRiskGroupInfoSQL As String = "spu_update_pmuser_riskgroup_info"
    Public Const ACAddUserSourceInfoStored As Boolean = True
    Public Const ACAddUserSourceInfoName As String = "AddUserSourceInfo"
    'Developer Guide No. 39
    Public Const ACAddUserSourceInfoSQL As String = "spu_add_pmuser_source_info"

    Public Const ACDeleteUserSourceInfoStored As Boolean = True
    Public Const ACDeleteUserSourceInfoName As String = "DeleteUserSourceInfo"
    'Developer Guide No. 39
    Public Const ACDeleteUserSourceInfoSQL As String = "spu_delete_pmuser_source_info"

    Public Const ACGetUserPartyInfoStored As Boolean = True
    Public Const ACGetUserPartyInfoName As String = "GetUserPartyInfo"
    'Developer Guide No. 39
    Public Const ACGetUserPartyInfoSQL As String = "spu_get_pmuser_party_info"

    Public Const ACGetClaimHandlersStored As Boolean = True
    Public Const ACGetClaimHandlersName As String = "GetClaimHandlers"
    'Developer Guide No. 39
    Public Const ACGetClaimHandlersSQL As String = "spu_get_claim_handlers"

    Public Const ACGetPartyStored As Boolean = True
    Public Const ACGetPartyName As String = "GetClaimHandlers"
    'Developer Guide No. 39
    Public Const ACGetPartySQL As String = "spu_get_party_details"

    '(RC) WR34
    Public Const ACGetOtherPartyStored As Boolean = True
    Public Const ACGetOtherPartyName As String = "GetOtherParty"
    'Developer Guide No. 39
    Public Const ACGetOtherPartySQL As String = "spu_get_otherparty_details"

    'MKW111103 PN8249
    Public Const ACGetUserStatusStored As Boolean = False
    Public Const ACGetUserStatusName As String = "GetRiskCodes"
    Public Const ACGetUserStatusSQL As String = "select fsa_user_status_id,description from FSA_User_Status"

    'Windows unified login
    Public Const ACDeleteUserMappingStored As Boolean = False
    Public Const ACDeleteUserMappingName As String = "DeleteUserMapping"
    Public Const ACDeleteUserMappingSQL As String = "update PMUser " & "set alternative_identifier = null," &
                                                    "ModifiedBy = {modified_by}," &
                                                    "UniqueId = {unique_id}," &
                                                    "ScreenHierarchy = 'User(' + username + ')' "

    Public Const ACUpdateProductOptionStored As Boolean = True
    Public Const ACUpdateProductOptionName As String = "UpdateProductOption"
    'Developer Guide No. 39
    Public Const ACUpdateProductOptionSQL As String = "spu_Product_Options_Update"

    Public Const ACUpdateUserMappingStored As Boolean = True
    Public Const ACUpdateUserMappingName As String = "UpdateUserMapping"
    'Developer Guide No. 39
    Public Const ACUpdateUserMappingSQL As String = "spu_update_pmuser_mapping"

    Public Const ACGetUsersWithNoPasswordStored As Boolean = True
    Public Const ACGetUsersWithNoPasswordName As String = "GetUsersWithNoPassword"
    'Developer Guide No. 39
    Public Const ACGetUsersWithNoPasswordSQL As String = "spu_GetUsersWithoutPassword"

    Public Const ACGetUserAdminStatusStored As Boolean = True
    Public Const ACGetUserAdminStatusName As String = "GetUserAdminStatus"
    'Developer Guide No. 39
    Public Const ACGetUserAdminStatusSQL As String = "spu_is_system_administrator"

    Public Const ACGetAdminUserCountStored As Boolean = True
    Public Const ACGetAdminUserCountName As String = "GetAdminUserCount"
    'Developer Guide No. 39
    Public Const ACGetAdminUserCountSQL As String = "spu_GetNumberOfSystemAdministrator"

    Public Const ACUpdateIncorrectAttemptsAndLockUnlockStored As Boolean = True
    Public Const ACUpdateIncorrectAttemptsAndLockUnlockName As String = "UpdateIncorrectAttemptsAndLockUnlock"
    Public Const ACUpdateIncorrectAttemptsAndLockUnlockSQL As String = "spu_PM_User_Set_Attempts_and_Lock"


    Public Const ACGetOtherPartySourceStored As Boolean = True
    Public Const ACGetOtherPartySourceName As String = "GetOtherPartySource"
    Public Const ACGetOtherPartySourceSQL As String = "spu_other_party_PLLSource"


    Public Const ACGetAgentSourceStored As Boolean = True
    Public Const ACGetAgentSourceName As String = "GetAgentSource"
    Public Const ACGetAgentSourceSQL As String = "spe_Agent_PLLSource"

    Public Const ACPasswordHistoryAddStored As Boolean = True
    Public Const ACPasswordHistoryAddName As String = "PasswordHistoryAdd"
    Public Const ACPasswordHistoryAddSQL As String = "Spu_SIR_PasswordHistory_add"

    Public Const ACCheckLogonStored As Boolean = True
    Public Const ACCheckLogonName As String = "Check_Logon"
    Public Const ACCheckLogonSQL As String = "spu_check_Logon"

    Public Const kCheckAlternativeLogonSQL As String = "spu_Check_Alternate_Logon"
    ' ***************************************************************** '
    ' Old Stored Procedure Calls

    '' Select Valid PMUser SQL
    'Public Const kCheckLogonStored = True
    'Public Const kCheckLogonName = "CheckLogon"
    'Public Const kCheckLogonSQL = "{call spu_select_user (?,?)}"
    '
    '' Select PMUser SQL
    'Public Const ACGetDetailsStored = True
    'Public Const ACGetDetailsName = "SelectPMUser"
    'Public Const ACGetDetailsSQL = "{call spu_select_PMUser_by_id(?)}"
    '
    '' Select All PMUsers SQL
    'Public Const ACGetAllDetailsStored = True
    'Public Const ACGetAllDetailsName = "SelectAllPMUser"
    'Public Const ACGetAllDetailsSQL = "{call spu_select_all_pmuser}"
    '
    '' Add PMUser SQL
    'Public Const ACAddStored = True
    'Public Const ACAddName = "AddPMUser"
    'Public Const ACAddSQL = "{call spu_add_PMUser (?,?,?,?,?,?,?,?,?,?)}"
    '
    '' Delete PMUser SQL
    'Public Const ACDeleteStored = True
    'Public Const ACDeleteName = "DeletePMUser"
    'Public Const ACDeleteSQL = "{call spu_delete_PMUser (?)}"
    '
    '' Update PMUser SQL
    'Public Const ACUpdateStored = True
    'Public Const ACUpdateName = "UpdatePMUser"
    'Public Const ACUpdateSQL = "{call spu_update_PMUser (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"
    ' ***************************************************************** '
End Module
