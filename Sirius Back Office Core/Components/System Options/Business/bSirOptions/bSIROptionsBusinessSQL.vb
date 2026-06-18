Option Strict Off
Option Explicit On

Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 07/05/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIROptions.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All System Options SQL
    Public Const ACGetAllDetailsStored As Boolean = False
    Public Const ACGetAllDetailsName As String = "SelectAllSystemOptions"
    Public Const ACGetAllDetailsSQL As String = "select option_number, value, description from system_options " &
                                                "where branch_id = {branch_id} order by option_number"

    ' Delete System Options SQL
    Public Const ACDeleteDetailsStored As Boolean = False
    Public Const ACDeleteDetailsName As String = "DeleteSystemOptions"
    Public Const ACDeleteDetailsSQL As String = "delete from system_options " &
                                                "where branch_id = {branch_id}"

    ' Insert System Options SQL
    Public Const ACInsertDetailsStored As Boolean = False
    Public Const ACInsertDetailsName As String = "InsertSystemOptions"
    Public Const ACInsertDetailsSQL As String = "insert into system_options (branch_id, option_number, value, description) values ({branch_id},{option_number},{value},{description})"

    ' Select One System Option SQL
    Public Const ACGetOneOptionStored As Boolean = False
    Public Const ACGetOneOptionName As String = "SelectOneOption"
    Public Const ACGetOneOptionSQL As String = "select value from system_options " &
                                               "where branch_id = {branch_id} and option_number = {option_number}"

    'CT /12/00 Select System Option for all sources SQL
    Public Const ACGetOptionAllSourcesStored As Boolean = False
    Public Const ACGetOptionAllSourcesName As String = "SelectOptionAllSources"
    Public Const ACGetOptionAllSourcesSQL As String = "select branch_id, value from system_options " &
                                                      "where option_number = {option_number}"

    ' Select All Parties SQL
    Public Const ACGetAllPartiesStored As Boolean = False
    Public Const ACGetAllPartiesName As String = "SelectAllParties"
    Public Const ACGetAllPartiesSQL As String = "select party_cnt, shortname from party " &
                                                "where source_id = {source_id} order by party_cnt"

    ' Select All Policies SQL
    Public Const ACGetAllPoliciesStored As Boolean = False
    Public Const ACGetAllPoliciesName As String = "SelectAllPolicies"

    'RAM20021217 - Added the code to return Insurance File Cnt as well
    'DC140606 PN28895 only check on policy branch as client may be on a different branch
    Public Const ACGetAllPoliciesSQL As String = "select  p.party_cnt, p.shortname, " &
                                                 "ifi.insurance_folder_cnt, ifo.code, ifo.Description, ifi.insurance_file_cnt " &
                                                 "from insurance_file ifi " &
                                                 "join party p on ifi.insured_cnt = p.party_cnt " &
                                                 "join insurance_folder ifo on ifi.insurance_folder_cnt = ifo.insurance_folder_cnt " &
                                                 "Where ifi.source_id = {source_id} order by 1, 3"

    'DC140606 PN28895 only check on policy branch as a claim is linked to a policy, the client may be on a different branch
    Public Const ACGetAllClaimsStored As Boolean = False
    Public Const ACGetAllClaimsName As String = "SelectAllClaims"
    Public Const ACGetAllClaimsSQL As String = "SELECT i.insured_cnt, p.shortname, " &
                                               "c.claim_id, c.claim_number, c.description " &
                                               "FROM claim c " &
                                               "JOIN insurance_file i ON c.policy_id = i.insurance_file_cnt " &
                                               "JOIN party p ON i.insured_cnt = p.party_cnt " &
                                               "WHERE i.source_id = {source_id} " &
                                               "ORDER BY c.claim_id"

    Public Const ACGetAllSourceStored As Boolean = False
    Public Const ACGetAllSourceName As String = "SelectAllSource"
    Public Const ACGetAllSourceSQL As String = "select source_id, code, description from source"

    ' Get the list of user groups
    Public Const ACGetUserGroupName As String = "Get User Groups"
    Public Const ACGetUserGroupSQL As String = "spu_get_valid_user_groups"

    Public Const ACGetSystemOptionConfigurationStored As Boolean = True
    Public Const ACGetSystemOptionConfigurationName As String = "GetSystemOptionConfiguration"
    Public Const ACGetSystemOptionConfigurationSQL As String = "spu_SIR_system_option_configuration_sel"

    Public Const ACUpdateClaimsWpFieldsStored As Boolean = True
    Public Const ACUpdateClaimsWpFieldsName As String = "UpdateWpFields_SystemOptions"
    Public Const ACUpdateClaimsWpFieldsSQL As String = "spu_UpdateWpFields_SystemOptions"

    Public Const ACGetMultiBranchStored As Boolean = False
    Public Const ACGetMultiBranchName As String = "SelectAllSource"
    Public Const ACGetMultiBranchSQL As String = "select value from hidden_options where option_number = 16 and branch_id =1"

    Public Const ACGetAllCaseNumberingSchemeStored As Boolean = True
    Public Const ACGetAllCaseNumberingSchemeName As String = "SelectAllCaseNumberingScheme"
    Public Const ACGetAllCaseNumberingSchemeSQL As String = "spu_get_all_casenumbering_scheme"

    Public Const ACGetAllCaseGISScreenStored As Boolean = True
    Public Const ACGetAllCaseGISScreenName As String = "SelectAllCaseGISScreen"
    Public Const ACGetAllCaseGISScreenSQL As String = "spu_get_GIS_screens"

    'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.3.1)
    Public Const ACUpdateExistingPCResolvedNamesFieldsStored As Boolean = True
    Public Const ACUpdateExistingPCResolvedNamesName As String = "UpdateResolvedNames_SystemOptions"
    Public Const ACUpdateExistingPCResolvedNamesSQL As String = "spu_UpdateResolvedNames_SystemOptions"
    'End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.3.1)

    Public Const ACMultipleQuoteVersionStored As Boolean = False
    Public Const ACMultipleQuoteVersionName As String = "QuoteVersionMultipleRecords"
    Public Const ACMultipleQuoteVersionSQL As String = "SELECT count(quote_version) FROM Insurance_File where quote_version >1"

    Public Const ACGetTaxGroupForClaimsStored As Boolean = True
    Public Const ACGetTaxGroupForClaimsName As String = "SelectTaxGroupForClaims"
    Public Const ACGetTaxGroupForClaimsSQL As String = "spu_SIR_Get_Tax_Group_For_Claims"

    Public Const ACGetTALLCurrencyNameStored As Boolean = True
    Public Const ACGetALLCurrencyName As String = "SelectCurrencyName"
    Public Const ACGetALLCurrencySQL As String = "spu_currency_selall"

End Module