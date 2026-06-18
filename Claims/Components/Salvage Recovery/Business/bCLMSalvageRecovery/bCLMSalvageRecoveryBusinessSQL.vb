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
    '              bCLMSalvageRecovery.Business class.
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    ' Select All CLMSalvageRecovery SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllCLMSalvageRecovery"
    Public Const ACGetAllDetailsSQL As String = "{call spu_GetAllSalvageDetails(?)}"

    ' Select All CLMSalvageRecoveryTypes SQL
    Public Const ACSalvageRecoveryTypesStored As Boolean = True
    Public Const ACSalvageRecoveryTypesName As String = "SelectAllCLMSalvageRecoveryTypes"
    Public Const ACSalvageRecoveryTypesSQL As String = "{Call spu_Salvage_Recovery_Type}"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckCLMSalvageRecoveryDelete"
    Public Const ACCheckIDSQL As String = "{call spu_Sal_chk_del_id(?)}"

    ' Select CLMCoinsuranceRecoveries SQL
    Public Const ACGetCoInsurerDetailsStored As Boolean = True
    Public Const ACGetCoInsurerDetailsName As String = "SelectCLMCoinsuranceRecoveries"
    Public Const ACGetCoInsurerDetailsSQL As String = "{call spu_get_sal_coins_details(?)}"

    ' Select CLMReinsuranceRecoveries SQL
    Public Const ACGetReInsurerDetailsStored As Boolean = True
    Public Const ACGetReInsurerDetailsName As String = "SelectCLMReinsuranceRecoveries"
    Public Const ACGetReInsurerDetailsSQL As String = "{call spu_get_sal_reins_details(?)}"

    ' Select CLMPeril SQL
    Public Const ACGetPerilDetailsStored As Boolean = True
    Public Const ACGetPerilDetailsName As String = "SelectCLMPeril"
    Public Const ACGetPerilDetailsSQL As String = "{call spu_get_Peril_details(?)}"

    ' Select CLMDefaultCurrencyID SQL
    Public Const ACGetDefaultCurrencyIDStored As Boolean = True
    Public Const ACGetDefaultCurrencyIDName As String = "SelectCLMDefaultCurrencyID"
    Public Const ACGetDefaultCurrencyIDSQL As String = "{call spu_get_CurrencyID(?)}"

    ' Check RecoverytypeID SQL
    Public Const ACCheckRecoverytypeIDStored As Boolean = True
    Public Const ACCheckRecoverytypeIDName As String = "CheckCLMSalvageRecoverytypeID"
    Public Const ACCheckRecoverytypeIDSQL As String = "{call spu_Check_Recovery_Type (?,?)}"

    ' Delete any Work Claim records
    Public Const ACDeleteWorkClaimStored As Boolean = True
    Public Const ACDeleteWorkClaimName As String = "Delete Work Claim"
    Public Const ACDeleteWorkClaimSQL As String = "{call spu_delete_work_claim (?,?)}"

    'get info only flag from work_claim table
    Public Const ACGetInfoOnlyFlagStored As Boolean = False
    Public Const ACGetInfoOnlyFlagName As String = "get info only flag"
    Public Const ACGetInfoOnlyFlagSQL As String = "SELECT Info_only FROM work_claim WHERE claim_id = {claim_id}"

    'RWH(15/06/01)
    Public Const ACGetOriginalClaimIDStored As Boolean = False
    Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
    Public Const ACGetOriginalClaimIDSQL As String = "SELECT Original_Claim_id FROM work_claim" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "WHERE claim_id = {claim_id}"


    ' JMK 25/05/2001 Find out whether Claim was previously Info Only status
    Public Const ACGetInfoOnlyStatusStored As Boolean = True
    Public Const ACGetInfoOnlyStatusName As String = "GetInfoOnlyStatus"
    Public Const ACGetInfoOnlyStatusSQL As String = "{call spu_get_claim_info_only_status(?)}"

    'SP080102 - Merge catch up
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