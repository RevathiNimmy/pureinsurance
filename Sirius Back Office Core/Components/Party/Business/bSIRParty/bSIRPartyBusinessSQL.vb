Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 12/10/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRParty.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Select All SIRParty SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRParty"
    'Developer Guide No. 39 
    Public Const ACGetAllDetailsSQL As String = "spe_Party_saa"
    'Add Accident details
    Public Const ACAddAccidentDetailsStored As Boolean = True
    Public Const ACAddAccidentDetailsName As String = "Add Accident Details"
    Public Const ACAddAccidentDetailsSQL As String = "spe_previous_accidents_add"
    'Add Conviction details
    Public Const ACAddConvictionDetailsStored As Boolean = True
    Public Const ACAddConvictionDetailsName As String = "Add Conviction Details"
    Public Const ACAddConvictionDetailsSQL As String = "spe_party_conviction_add"
    'Add Party Supplier details
    Public Const ACAddPartySupplierDetailsStored As Boolean = True
    Public Const ACAddPartySupplierDetailsName As String = "Add PartySupplier Details"
    Public Const ACAddPartySupplierDetailsSQL As String = "spu_SAM_CLM_party_Supplier_add"


    ' Check ID SQL
    Public Const ACPartyVariantAddressSelectStored As Boolean = True
    Public Const ACPartyVariantAddressSelectName As String = "PartyVariantAddressSelect"
    'Developer Guide No. 39

    Public Const ACPartyVariantAddressSelectSQL As String = "spu_party_variant_address_sel"

    Public Const ACPartyVariantAddressSelectAllStored As Boolean = True
    Public Const ACPartyVariantAddressSelectAllName As String = "PartyVariantAddressSelectAll"
    'Developer Guide No. 39
    Public Const ACPartyVariantAddressSelectAllSQL As String = "spu_party_variant_address_selall"

    Public Const ACPartyVariantAddressCommitStored As Boolean = True
    Public Const ACPartyVariantAddressCommitName As String = "PartyVariantAddressUpdate"
    'Developer Guide No. 39
    Public Const ACPartyVariantAddressCommitSQL As String = "spu_party_variant_address_commit"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyID"
    'Developer Guide No. 39
    Public Const ACCheckIDSQL As String = "spe_SIRParty_check_id"

    ' Get Party Count SQL
    'Thinh Nguyen 21/06/2002 (start) - this won't work if we have apostrophe in shortname
    Public Const ACGetPartyCntStored As Boolean = True
    Public Const ACGetPartyCntName As String = "GETPARTYCNT"
    'Developer Guide No. 39
    Public Const ACGetPartyCntSQL As String = "spu_GetPartyByShortName" ' Get Agent details SQL
    'Thinh Nguyen 21/06/2002 (end) - this won't work if we have apostrophe in shortname

    ' Get Agent details SQL
    Public Const ACGetAgentDetailsStored As Boolean = False
    Public Const ACGetAgentDetailsName As String = "GETAGENTDETAILS"
    Public Const ACGetAgentDetailsSQL As String = "SELECT shortname, resolved_name FROM party WHERE party_cnt = "

    ' Get PartyStructureID SQL
    Public Const ACGetPartyStructureIDStored As Boolean = False
    Public Const ACGetPartyStructureIDName As String = "GetPartyStructureID"
    Public Const ACGetPartyStructureIDSQL As String = "SELECT party_structure_id FROM Party WHERE party_cnt = {party_cnt}"

    ' Get Employer details SQL
    Public Const ACGetEmployerDetailsStored As Boolean = False
    Public Const ACGetEmployerDetailsName As String = "GETEmployerDETAILS"
    Public Const ACGetEmployerDetailsSQL As String = "SELECT shortname FROM party WHERE party_cnt = "

    ' Get Party Type SQL
    Public Const ACGetPartyTypeStored As Boolean = False
    Public Const ACGetPartyTypeName As String = "GETPARTYTYPE"
    Public Const ACGetPartyTypeSQL As String = "SELECT description, code FROM party_type WHERE party_type_id = "

    ' Get Party Fee Details SQL
    Public Const ACGetFeeDetailsStored As Boolean = True
    Public Const ACGetFeeDetailsName As String = "GetFeeDetails"
    'Developer Guide No. 39
    Public Const ACGetFeeDetailsSQL As String = "spe_fee_amounts_sel"

    'DC 04/08/00
    Public Const ACAddContactStored As Boolean = True
    Public Const ACAddContactName As String = "AddContact"
    'Developer Guide No. 39
    Public Const ACAddContactSQL As String = "spe_contact_add"

    'DJM 24/05/2002
    Public Const ACUpdContactStored As Boolean = True
    Public Const ACUpdContactName As String = "UpdContact"
    'Developer Guide No. 39
    Public Const ACUpdContactSQL As String = "spe_contact_Upd"

    'DJM 24/05/2002
    Public Const ACDelContactStored As Boolean = True
    Public Const ACDelContactName As String = "DelContact"
    'Developer Guide No. 39
    Public Const ACDelContactSQL As String = "spe_contact_Del"

    'DJM 24/05/2002
    Public Const ACDelPartyUsageStored As Boolean = True
    Public Const ACDelPartyUsageName As String = "DelPartyUsage"
    'Developer Guide No. 39
    Public Const ACDelPartyUsageSQL As String = "spe_Party_Contact_Usage_del"

    'DJM 24/05/2002
    Public Const ACAddPartyUsageStored As Boolean = True
    Public Const ACAddPartyUsageName As String = "AddPartyUsage"
    'Developer Guide No. 39
    Public Const ACAddPartyUsageSQL As String = "spe_Party_Contact_Usage_add"

    ' CTAF 220800
    'Developer Guide No. 39
    Public Const ACConvertSQL As String = "spu_convert_party"
    Public Const ACConvertSQLName As String = "ConvertParty"
    Public Const ACConvertSQLStored As Boolean = True

    ' Get Addresses
    Public Const ACGetAddressesStored As Boolean = True
    Public Const ACGetAddressesName As String = "Get Addresses"
    'Developer Guide No. 39
    Public Const ACGetAddressesSQL As String = "spu_get_addresses_for_party"

    ' RDC 20042004 for GetBaseCurrencyID
    Public Const ACGetBaseCurrencyStored As Boolean = True
    Public Const ACGetBaseCurrencyName As String = "GetBaseCurrency"
    'Developer Guide No. 39
    Public Const ACGetBaseCurrencySQL As String = "spu_get_source_base_currency"

    'FSA Phase III

    Public Const ACUpdateTobLetterStored As Boolean = False
    Public Const ACUpdateTobLetterName As String = "UpdateTobLetter"
    Public Const ACUpdateTobLetterSQL As String = "Update Party set tob_letter = {today} where party_cnt = {party_cnt}"

    'Identification of Duplicate Customer

    Public Const ACGetDuplicateClientStored As Boolean = True
    Public Const ACGetDuplicateClient As String = "Get Duplicate Party"
    Public Const ACGetDuplicateClientSQL As String = "spu_party_matchbyshortname"

    'Check if account code exists
    Public Const ACCheckDupRefStored As Boolean = True
    Public Const ACCheckDupRefName As String = "CheckIfAccountCodeExists"
    'Developer Guide No. 39
    Public Const ACCheckDupRefSQL As String = "spu_CheckShortName"

    'Check if Party Account exists
    Public Const ACCheckClientAccount As Boolean = True
    Public Const ACCheckClientAccountName As String = "CheckIfPartyAccountExists"
    'Developer Guide No. 39
    Public Const ACCheckClientAccountSQL As String = "spu_CheckPartyAccountExists"

    '******************************************************************************
    Public Const kGetPartyDetailsName As String = "Get Additional Party Details"
    Public Const kGetPartyDetailsSQL As String = "spu_SIR_Party_Details_Select"

    Public Const kUpdatePartyDetailsName As String = "Update Additional Party Details"
    Public Const kUpdatePartyDetailsSQL As String = "spu_SIR_Party_Details_Update"
    '******************************************************************************
    'AR20050823 - PN24332
    'Get the GIS Policy Link for a Party
    Public Const ACPartyGetGisPolicyLinkSP As Boolean = True
    Public Const ACPartyGetGisPolicyLinkName As String = "spu_Party_Get_GIS_Policy_Link"
    'Developer Guide No. 39
    Public Const ACPartyGetGisPolicyLinkSQL As String = "spu_Party_Get_GIS_Policy_Link"

    'Add tax details info
    Public Const kUpdatePartyTaxDetailsName As String = "Update Tax details"
    Public Const kUpdatePartyTaxDetailsSQL As String = "spu_SAM_CLM_Party_Details_Update"
    'Add Other Party Info
    Public Const kAddOtherPartyDetailsName As String = "Add other party Info"
    Public Const kAddOtherPartyDetailsSQL As String = "spe_party_other_upd"

    'Update Other Party Info
    Public Const kUpdateOtherPartyName As String = "Update Other Party Details"
    Public Const kUpdateOtherPartySQL As String = "spe_party_other_upd"

    'Update Other Party Conviction Details
    Public Const kUpdateOPConvictionName As String = "Update Other Party Coviction Details" 'GAURAV
    Public Const kUpdateOPConvictionSQL As String = "spe_party_conviction_upd"

    ' Update Previous Accident Details
    Public Const kUpdateOPAccidentName As String = "Update Other Party Accident Details" 'GAURAV
    Public Const kUpdateOPAccidentSQL As String = "spu_SAM_previous_accidents_upd"

    'SET - PN32665 - delete all objects for the given gis_policy_link_id
    Public Const ACDelGisObjectSP As Boolean = True
    Public Const ACDelGisObjectName As String = "spu_SirRen_DeleteGisObject"
    'Developer Guide No. 39
    Public Const ACDelGisObjectSQL As String = "spu_SirRen_DeleteGisObject"

    'Get Previously attached Party Builder Data Model if it differs from current one
    Public Const ACIsScreenDataModelChangedStored As Boolean = True
    Public Const ACIsScreenDataModelChangedName As String = "spu_SIR_Is_Screen_Data_Model_Changed"
    'Developer Guide No. 39
    Public Const ACIsScreenDataModelChangedSQL As String = "spu_SIR_Is_Screen_Data_Model_Changed"

    'Deletes all corresponding GIS data for a GIS Policy Link Id
    Public Const ACDeleteCustomDataStored As Boolean = True
    Public Const ACDeleteCustomDataName As String = "spu_SIR_Delete_GIS_Data"
    'Developer Guide No. 39
    Public Const ACDeleteCustomDataSQL As String = "spu_SIR_Delete_GIS_Data"

    Public Const ACGetPartyContacts As Boolean = False
    Public Const ACGetPartyContactsName As String = "GetPartyContacts"
    Public Const ACGetPartyContactsSQL As String = "select Typecode from clientcontacts where clientid = {party_cnt}"

    Public Const ACGetPartyPolicies As Boolean = True
    Public Const ACGetPartyPoliciesName As String = "GetPartyPolicies"
    Public Const ACGetPartyPoliciesSQL As String = "spu_SAM_Get_Party_Policies_Latest_Version"

    'Add Party History
    Public Const kAddPartyHistoryName As String = "Add Party Hitory"
    Public Const kAddPartyHistorySQL As String = "spu_Party_History_Add"

    Public Const kGetPartyGISDetailName As String = "Get Party GISDetails"
    Public Const kGetPartyGISDetailSQL As String = "spu_Party_Get_GIS_Details"

    Public Const kGetAllPartiesName As String = "Get All Parties"
    Public Const kGetAllPartiesSQl As String = "spu_PartyCnt_SelectAll"

End Module