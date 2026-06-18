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
    ' Date: 25/06/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRPartyIN.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All SIRPartyIN SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRPartyIN"
    Public Const ACGetAllDetailsSQL As String = "spe_party_insurer_saa"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRPartyINID"
    Public Const ACCheckIDSQL As String = "spe_SIRPartyIN_check_id"

    ' Select next available shortname from agent table
    Public Const ACGetNextRefStored As Boolean = True
    Public Const ACGetNextRefName As String = "SelectNextShortname"
    Public Const ACGetNextRefSQL As String = "spu_Next_Insurer_Shortname_sel"

    ' Get ABI codes from GII
    Public Const ACGetABICodesStored As Boolean = False
    Public Const ACGetABICodesName As String = "SelectABICodes"
    Public Const ACGetABICodesSQL As String = "SELECT DISTINCT abi_81_insurer," & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "description" & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "FROM gis_insurer" & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "WHERE abi_81_insurer > ''" & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "ORDER BY description"
    ' Select Terms of payment SQL
    Public Const ACGetTermsOfPayment As Boolean = True
    Public Const ACGetTermsOfPaymentName As String = "GetTermsOfPaymetId"
    Public Const ACGetTermsOfPaymentSQL As String = "spu_terms_of_payment_sel"

    'Select risk code details
    Public Const ACGetRiskCodeDetailsStored As Boolean = True
    Public Const ACGetRiskCodeDetailsName As String = "GetRiskCodeDetails"
    Public Const ACGetRiskCodeDetailsSQL As String = "spu_party_insurer_risk_sel"

    'Update risk code details
    Public Const ACUpdateRiskCodeDetailsStored As Boolean = True
    Public Const ACUpdateRiskCodeDetailsName As String = "UpdateRiskCodeDetails"
    Public Const ACUpdateRiskCodeDetailsSQL As String = "spu_party_insurer_risk_upd"

    Public Const ACGetCertYearStored As Boolean = True
    Public Const ACGetCertYearName As String = "GetCertYear"
    Public Const ACGetCertYearSQL As String = "spu_Get_agent_Cert_Year"

    Public Const ACUpdateCertYearStored As Boolean = True
    Public Const ACUpdateCertYearName As String = "UpdateCertYear"
    Public Const ACUpdateCertYearSQL As String = "spu_Update_agent_Cert_Year"

    Public Const ACGetSubAgentDetailStored As Boolean = True
    Public Const ACGetSubAgentDetailName As String = "Get Sub Agent Detail"
    Public Const ACGetSubAgentDetailSQL As String = "spu_Get_Sub_Agent_Detail"

    Public Const ACDelAddressStored As Boolean = True
    Public Const ACDelAddressName = "DeleteAddresses"
    Public Const ACDelAddressSQL As String = "spe_Delete_Addresses"

End Module