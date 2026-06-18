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
    '              bSIRPartyEX.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All SIRPartyEX SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyEX"
    Public Const ACGetAllDetailsSQL As String = "spe_Party_Agent_sel"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyEXID"
    Public Const ACCheckIDSQL As String = "spe_SIRPartyEX_check_id"

    ' Select next available shortname from agent table
    Public Const ACGetNextRefStored As Boolean = True
    Public Const ACGetNextRefName As String = "SelectNextShortname"
    Public Const ACGetNextRefSQL As String = "spu_Next_Agent_Shortname_sel"

    ' PW150702 - Select all available doc types that can be suppressed
    Public Const ACGetAvailableDocsStored As Boolean = True
    Public Const ACGetAvailableDocsName As String = "SelectAvailableDocs"
    Public Const ACGetAvailableDocsSQL As String = "spu_get_process_types_docs"

    ' PW150702 - Select docs that have been suppressed
    Public Const ACGetSuppressedDocsStored As Boolean = True
    Public Const ACGetSuppressedDocsName As String = "SelectSuppressedDocs"
    Public Const ACGetSuppressedDocsSQL As String = "spu_get_agent_docs"

    ' PW150702 - Delete suppressed docs list
    Public Const ACDelSuppressedDocsStored As Boolean = True
    Public Const ACDelSuppressedDocsName As String = "DeleteSuppressedDocs"
    Public Const ACDelSuppressedDocsSQL As String = "spu_del_agent_docs"

    ' PW150702 - Delete suppressed docs list
    Public Const ACAddSuppressedDocsStored As Boolean = True
    Public Const ACAddSuppressedDocsName As String = "AddSuppressedDocs"
    Public Const ACAddSuppressedDocsSQL As String = "spu_add_agent_docs"

    ' SD 14/08/2002 - Get a party name from the shortname
    Public Const ACGetPartyNameFromShortnameStored As Boolean = True
    Public Const ACGetPartyNameFromShortnameName As String = "GetPartyNameFromShortname"
    Public Const ACGetPartyNameFromShortnameSQL As String = "spu_get_party_name_from_Shortname"

    'DC220803 -PS253 -fsa compliance
    Public Const ACGetAgentRiskGroupsStored As Boolean = True
    Public Const ACGetAgentRiskGroupsName As String = "SelectAgentRiskGroups"
    Public Const ACGetAgentRiskGroupsSQL As String = "spu_get_agent_risk_groups"

    Public Const ACAddAgentRiskGroupStored As Boolean = True
    Public Const ACAddAgentRiskGroupName As String = "AddAgentRiskGroup"
    Public Const ACAddAgentRiskGroupSQL As String = "spu_add_agent_risk_group"

    Public Const ACDeleteAgentRiskGroupStored As Boolean = True
    Public Const ACDeleteAgentRiskGroupName As String = "DeleteAgentRiskGroups"
    Public Const ACDeleteAgentRiskGroupSQL As String = "spu_delete_agent_risk_group"

    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    Public Const ACUpdateAgencyNumberStored As Boolean = True
    Public Const ACUpdateAgencyNumberName As String = "UpdateAgencyNumber"
    Public Const ACUpdateAgencyNumberSQL As String = "spu_Party_Extra_ups"

    Public Const ACUpdateFeesPartyExtraStored As Boolean = True
    Public Const ACUpdateFeesPartyExtraName As String = "UpdateFeesPartyExtra"
    Public Const ACUpdateFeesPartyExtraSQL As String = "spu_fees_update_Party_Extra"

    Public Const ACDelAddressStored As Boolean = True
    Public Const ACDelAddressName = "DeleteAddresses"
    Public Const ACDelAddressSQL As String = "spe_Delete_Addresses"

End Module