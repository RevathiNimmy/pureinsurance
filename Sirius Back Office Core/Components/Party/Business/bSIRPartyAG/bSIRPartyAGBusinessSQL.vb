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
    ' Date: 12/10/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPartyAG.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All SIRPartyAG SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyAG"
    'Developer Guide No 39
    Public Const ACGetAllDetailsSQL As String = "spe_Party_Agent_sel"

    ' PN 21971 - Check Reg. Number
    Public Const ACCheckAgentRegNumberStored As Boolean = True
    Public Const ACCheckAgentRegNumberName As String = "CheckAgentRegNumber"
    'Developer Guide No 39
    Public Const ACCheckAgentRegNumberSQL As String = "spu_CheckAgentRegNumber"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyAGID"
    'Developer Guide No 39
    Public Const ACCheckIDSQL As String = "spe_SIRPartyAG_check_id"

    ' Select next available shortname from agent table
    Public Const ACGetNextRefStored As Boolean = True
    Public Const ACGetNextRefName As String = "SelectNextShortname"
    'Developer Guide No 39
    Public Const ACGetNextRefSQL As String = "spu_Next_Agent_Shortname_sel"

    ' PW150702 - Select all available doc types that can be suppressed
    Public Const ACGetAvailableDocsStored As Boolean = True
    Public Const ACGetAvailableDocsName As String = "SelectAvailableDocs"
    'Developer Guide No 39
    Public Const ACGetAvailableDocsSQL As String = "spu_get_process_types_docs"

    ' PW150702 - Select docs that have been suppressed
    Public Const ACGetSuppressedDocsStored As Boolean = True
    Public Const ACGetSuppressedDocsName As String = "SelectSuppressedDocs"
    'Developer Guide No 39
    Public Const ACGetSuppressedDocsSQL As String = "spu_get_agent_docs"

    ' PW150702 - Delete suppressed docs list
    Public Const ACDelSuppressedDocsStored As Boolean = True
    Public Const ACDelSuppressedDocsName As String = "DeleteSuppressedDocs"
    'Developer Guide No 39
    Public Const ACDelSuppressedDocsSQL As String = "spu_del_agent_docs"

    ' PW150702 - Delete suppressed docs list
    Public Const ACAddSuppressedDocsStored As Boolean = True
    Public Const ACAddSuppressedDocsName As String = "AddSuppressedDocs"
    'Developer Guide No 39
    Public Const ACAddSuppressedDocsSQL As String = "spu_add_agent_docs"

    ' SD 14/08/2002 - Get a party name from the shortname
    Public Const ACGetPartyNameFromShortnameStored As Boolean = True
    Public Const ACGetPartyNameFromShortnameName As String = "GetPartyNameFromShortname"
    'Developer Guide No 39
    Public Const ACGetPartyNameFromShortnameSQL As String = "spu_get_party_name_from_Shortname"

    'DC220803 -PS253 -fsa compliance
    Public Const ACGetAgentRiskGroupsStored As Boolean = True
    Public Const ACGetAgentRiskGroupsName As String = "SelectAgentRiskGroups"
    'Developer Guide No 39
    Public Const ACGetAgentRiskGroupsSQL As String = "spu_get_agent_risk_groups"

    Public Const ACAddAgentRiskGroupStored As Boolean = True
    Public Const ACAddAgentRiskGroupName As String = "AddAgentRiskGroup"
    'Developer Guide No 39
    Public Const ACAddAgentRiskGroupSQL As String = "spu_add_agent_risk_group"

    Public Const ACDeleteAgentRiskGroupStored As Boolean = True
    Public Const ACDeleteAgentRiskGroupName As String = "DeleteAgentRiskGroups"
    'Developer Guide No 39
    Public Const ACDeleteAgentRiskGroupSQL As String = "spu_delete_agent_risk_group"


    Public Const ACGetSourceInfoStored As Boolean = False
    Public Const ACGetSourceInfoSQL As String = "SELECT base_currency_id,country_id from Source where Source_id={BranchId}"
    Public Const ACGetSourceInfoName As String = "Get Source Information"

    Public Const ACGetPartyAgentOriginStored As Boolean = False
    Public Const ACGetPartyAgentOriginSQL As String = "Select party_agent_origin_id from party_agent_origin where Code LIKE 'DATA CONV%'"
    Public Const ACGetPartyAgentOriginName As String = "Party Origin ID"

    Public Const ACGetPartyTypeIDForAgentStored As Boolean = False
    Public Const ACGetPartyTypeIDForAgentSQL As String = "Select Party_type_id from party_type where code='AG'"
    Public Const ACGetPartyTypeIDForAgentName As String = "Get Party Type For Agent"

    Public Const ACGetContactTypeIDStored As Boolean = False
    Public Const ACGetContactTypeIDSQL As String = "SELECT contact_type_id FROM contact_type WHERE code like 'Telephone%'"
    Public Const ACGetContactTypeIDName As String = "Get Contact Type"

    Public Const ACGetCorrespondenceAddressUsageTypeIDStored As Boolean = False
    Public Const ACGetCorrespondenceAddressUsageTypeIDSQL As String = "SELECT address_usage_type_id FROM Address_Usage_Type WHERE code like '3131 XCO%'"
    Public Const ACGetCorrespondenceAddressUsageTypeIDName As String = "Address Usage Type"

    Public Const ACPartyContactUsageStored As Boolean = False
    Public Const ACPartyContactUsageSQL As String = "spu_Party_Contact_Usage_add"
    Public Const ACPartyContactUsageName As String = "spu_Party_Contact_Usage_add"

    Public Const ACGetIsAgentExistsSQL As String = "Select 1 from  Party where ShortName={Code}"
    Public Const ACGetIsAgentExistsStored As Boolean = False
    Public Const ACGetIsAgentExistsName As String = "IsAgentExists"

    Public Const ACUpdateSourceIDforAgentImportSQL As String = "spu_UpdateSourceIdForAgentImport"
    Public Const ACUpdateSourceIDforAgentImportStored As Boolean = False
    Public Const ACUpdateSourceIDforAgentImportName As String = "UpdateSourceIDforAgentImport"

    ' SAGICOR - WPR 14: Get Users for the selected Agent
    Public Const ACGetAgentUserStored As Boolean = True
    Public Const ACGetAgentUserName As String = "SelectAgencyUser"
    Public Const ACGetAgentUserSQL As String = "spu_SIR_Select_AgencyUser"

    Public Const ACGetAgencyUserSourceStored As Boolean = True
    Public Const ACGetAgencyUserSourcerName As String = "SelectAgencyUserSource"
    Public Const ACGetAgencyUserSourceSQL As String = "spe_Agent_PLLSource"

    Public Const ACGetAgencyUserProductStored As Boolean = True
    Public Const ACGetAgencyUserProductName As String = "SelectAgencyUserProduct"
    Public Const ACGetAgencyUserProductSQL As String = "spu_SIR_SelectAll_AgencyProduct"

    Public Const ACSaveSourceIDforAgentStored As Boolean = True
    Public Const ACSaveSourceIDforAgentName As String = "SaveAgencyUserSource"
    Public Const ACSaveSourceIDforAgentSQL As String = "spe_Agent_PLSSource"

    Public Const ACSaveProductIDforAgentStored As Boolean = True
    Public Const ACSaveProductIDforAgentName As String = "SaveAgencyUserProduct"
    Public Const ACSaveProductIDforAgentSQL As String = "spu_sir_add_agencyproduct"

    Public Const ACDeleteSourceIDforAgentStored As Boolean = True
    Public Const ACDeleteSourceIDforAgentName As String = "DeleteAgencyUserSource"
    Public Const ACDeleteSourceIDforAgentSQL As String = "spe_Agent_PLDSource"

    Public Const ACDeleteProductIDforAgentStored As Boolean = True
    Public Const ACDeleteProductIDforAgentName As String = "DeleteAgencyUserProduct"
    Public Const ACDeleteProductIDforAgentSQL As String = "spu_SIR_Delete_AgencyProduct"

    Public Const ACGetCertYearStored As Boolean = True
    Public Const ACGetCertYearName As String = "GetCertYear"
    Public Const ACGetCertYearSQL As String = "spu_Get_agent_Cert_Year"

    Public Const ACUpdateCertYearStored As Boolean = True
    Public Const ACUpdateCertYearName As String = "UpdateCertYear"
    Public Const ACUpdateCertYearSQL As String = "spu_Update_agent_Cert_Year"

    Public Const ACGetSubAgentDetailStored As Boolean = True
    Public Const ACGetSubAgentDetailName As String = "Get Sub Agent Detail"
    Public Const ACGetSubAgentDetailSQL As String = "spu_Get_Sub_Agent_Detail"

    Public Const ACGetBankAccountForCurrencyStored = True
    Public Const ACGetBankAccountForCurrencyName = "SelBankAccountForCurrency"
    Public Const ACGetBankAccountForCurrencySQL = "spu_Sel_BankAccount_ForCurrency"

    ' Update Commission Level for agent
    Public Const ACUpdateCommissionLevelStored As Boolean = True
    Public Const ACUpdateCommissionLevelName As String = "UpdateCommissionLevel"
    Public Const ACUpdateCommissionLevelSQL As String = "spu_Update_Agent_Commission_Level"

    Public Const ACDelAddressStored As Boolean = True
    Public Const ACDelAddressName = "DeleteAddresses"
    Public Const ACDelAddressSQL As String = "spe_Delete_Addresses"

End Module
