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
    ' Date: 24/08/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              bCLMThirdParty.Business class.
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    ' Select All CLMThirdPartyRecovery SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllCLMThirdPartyRecovery"
    Public Const ACGetAllDetailsSQL As String = "{call spu_GetAllTPDetails(?)}"

    ' Select All CLMThirdPartyRecoveryTypes SQL
    Public Const ACSalvageRecoveryTypesStored As Boolean = True
    Public Const ACSalvageRecoveryTypesName As String = "SelectAllCLMThirdPartyRecoveryTypes"
    Public Const ACSalvageRecoveryTypesSQL As String = "{Call spu_TP_Recovery_Type}"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckCLMThirdPartyRecoveryDelete"
    Public Const ACCheckIDSQL As String = "{call spu_TP_chk_del_id(?)}"

    ' Select CLMCoinsuranceRecoveries SQL
    Public Const ACGetCoInsurerDetailsStored As Boolean = True
    Public Const ACGetCoInsurerDetailsName As String = "SelectCLMCoinsuranceRecoveries"
    Public Const ACGetCoInsurerDetailsSQL As String = "{call spu_get_TP_coins_details(?)}"

    ' Select CLMReinsuranceRecoveries SQL
    Public Const ACGetReInsurerDetailsStored As Boolean = True
    Public Const ACGetReInsurerDetailsName As String = "SelectCLMReinsuranceRecoveries"
    Public Const ACGetReInsurerDetailsSQL As String = "{call spu_get_TP_reins_details(?)}"

    ' Select CLMPeril SQL
    Public Const ACGetPerilDetailsStored As Boolean = True
    Public Const ACGetPerilDetailsName As String = "SelectCLMPeril"
    Public Const ACGetPerilDetailsSQL As String = "{call spu_get_TP_Peril_details(?)}"

    ' Select CLMDefaultCurrencyID SQL
    Public Const ACGetDefaultCurrencyIDStored As Boolean = True
    Public Const ACGetDefaultCurrencyIDName As String = "SelectCLMDefaultCurrencyID"
    Public Const ACGetDefaultCurrencyIDSQL As String = "{call spu_get_tp_CurrencyID(?)}"

    ' Check RecoverytypeID SQL
    Public Const ACCheckRecoverytypeIDStored As Boolean = True
    Public Const ACCheckRecoverytypeIDName As String = "CheckCLMThirdPartyRecoverytypeID"
    Public Const ACCheckRecoverytypeIDSQL As String = "{call spu_TPRecovery_Type(?,?)}"

    ' Delete any Work Claim records
    Public Const ACDeleteWorkClaimStored As Boolean = True
    Public Const ACDeleteWorkClaimName As String = "Delete Work Claim"
    Public Const ACDeleteWorkClaimSQL As String = "{call spu_delete_work_claim (?,?)}"

    ' JMK 25/05/2001 Find out whether Claim was previously Info Only status
    Public Const ACGetInfoOnlyStatusStored As Boolean = True
    Public Const ACGetInfoOnlyStatusName As String = "GetInfoOnlyStatus"
    Public Const ACGetInfoOnlyStatusSQL As String = "{call spu_get_claim_info_only_status(?)}"

    'RWH(15/06/01)
    Public Const ACGetOriginalClaimIDStored As Boolean = False
    Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
    Public Const ACGetOriginalClaimIDSQL As String = "SELECT Original_Claim_id FROM work_claim" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "WHERE claim_id = {claim_id}"


    'RWH(Added functionality to close claim.
    Public Const ACCloseClaimSQL As String = "{call spu_CloseClaim(?)}"
    Public Const ACCloseClaimName As String = "CloseClaimName"
    Public Const ACCloseClaimStored As Boolean = True

    Public Const ACGetCurrentReserveRecoverySQL As String = "{call spu_GetCurrentReserveRecovery(?)}"
    Public Const ACGetCurrentReserveRecoveryName As String = "GetCurrentReserveRecovery"
    Public Const ACGetCurrentReserveRecoveryStored As Boolean = True

    ' Alix Bergeret - 15/05/2003 - Load tax types and bands
    Public Const ACGetTaxTypesBandsStored As Boolean = True
    Public Const ACGetTaxTypesBandsName As String = "GetTaxTypesBands"
    Public Const ACGetTaxTypesBandsSQL As String = "{call spu_Get_Tax_Types_and_Bands}"

    'Get Policy Detail
    Public Const ACGetClientPolicyDetailsStored As Boolean = True
    Public Const ACGetClientPolicyDetailsName As String = "GetClientPolicyDetails"
    Public Const ACGetClientPolicyDetailsSQL As String = "spu_get_client_policy_details"
End Module