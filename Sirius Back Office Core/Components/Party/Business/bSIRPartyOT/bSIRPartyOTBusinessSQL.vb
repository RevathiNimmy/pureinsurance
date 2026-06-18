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
    ' Date: 08/10/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPartyOT.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'developer guide no.39
    'start
    ' Select All SIRPartyOT SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyOT"
    Public Const ACGetAllDetailsSQL As String = "spe_Address_saa"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyOTID"
    Public Const ACCheckIDSQL As String = "spe_SIRPartyOT_check_id"

    ' Get Convictions SQL
    Public Const ACGetConvictionsStored As Boolean = True
    Public Const ACGetConvictionsName As String = "Get Convictions"
    Public Const ACGetConvictionsSQL As String = "spe_party_conviction_saa"

    ' Delete Convictions SQL
    Public Const ACDeleteConvictionsStored As Boolean = True
    Public Const ACDeleteConvictionsName As String = "Delete Convictions"
    Public Const ACDeleteConvictionsSQL As String = "spe_party_conviction_dar"

    ' Add Convictions SQL
    Public Const ACAddConvictionsStored As Boolean = True
    Public Const ACAddConvictionsName As String = "Add Convictions"
    '                                             "?,?,?,?,?)}"
    Public Const ACAddConvictionsSQL As String = "spe_party_conviction_add"

    ' Get Accidents SQL
    Public Const ACGetAccidentsStored As Boolean = True
    Public Const ACGetAccidentsName As String = "Get Accidents"
    Public Const ACGetAccidentsSQL As String = "spe_previous_accidents_saa"

    ' Delete Accidents SQL
    Public Const ACDeleteAccidentsStored As Boolean = True
    Public Const ACDeleteAccidentsName As String = "Delete Accidents"
    Public Const ACDeleteAccidentsSQL As String = "spe_previous_accidents_dar"

    ' Add Accidents SQL
    Public Const ACAddAccidentsStored As Boolean = True
    Public Const ACAddAccidentsName As String = "Add Accidents"
    Public Const ACAddAccidentsSQL As String = "spe_previous_accidents_add"

    'SD 17/09/2002 Add Supplier party type changes
    ' Add Party Supplier Business
    Public Const ACAddPartSuppBussStored As Boolean = True
    Public Const ACAddPartSuppBussName As String = "Add Party Supplier Business"
    Public Const ACAddPartSuppBussSQL As String = "spu_PartySupplierBusiness_add"
    'Update party_other Table With three fields Vivek
    Public Const ACAddPartyOtherStored As Boolean = True
    Public Const ACAddPartyOtherName As String = "Add Party Supplier Business"
    Public Const ACAddPartyOtherSQL As String = "spu_PartyOther_Update"
    '-----------------------------------------------------------------------
    Public Const kGetPartySupplierBusinessSQL As String = "spu_SAM_GetPartySupplier_details"
    Public Const kGetPartySupplierBusinessName As String = "returns party supplier business details"

    Public Const kGetOtherPartyDetailsSQL As String = "spu_SAM_GetOtherParty_details"
    Public Const kGetOtherPartyDetailsName As String = "returns Three fields from party_other"

    Public Const kGetOtherPartyAdditionalDetailsSQL As String = "spu_SAM_GetOtherPartyAdditional_details"
    Public Const kGetOtherPartyAdditionalDetailsName As String = "returns additional party details"

    ' Get Party System Type Code SQL
    Public Const ACGetPartySystemTypeCodeName As String = "GetPartySystemTypeCode"
    Public Const ACGetPartySystemTypeCodeSQL As String = "spu_Get_Party_System_Type_Code"

    'Get Party Type details
    Public Const AC_SQL_PARTYTYPESELECT_NAME As String = "Get Party Type Details"
    Public Const AC_SQL_PARTYTYPESELECT_SQL As String = "spu_party_type_sel"
    Public Const AC_SQL_PARTYTYPESELECT_SP As Boolean = True

    Public Const ACDelPartSupplierBussStored As Boolean = True
    Public Const ACDelPartSupplierBussName As String = "Delete Party Supplier Business"
    Public Const ACDelPartSupplierBussSQL As String = "spu_PartySupplierBusiness_del"

    Public Const ACDelAddressStored As Boolean = True
    Public Const ACDelAddressName = "DeleteAddresses"
    Public Const ACDelAddressSQL As String = "spe_Delete_Addresses"
    'end
End Module