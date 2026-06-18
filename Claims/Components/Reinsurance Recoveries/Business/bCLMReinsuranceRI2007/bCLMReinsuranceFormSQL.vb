Option Strict Off
Option Explicit On
Module FormSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '

    ' Claims
    Public Const ACDeleteClaimName As String = "Delete Work Claim"
    Public Const ACDeleteClaimSQL As String = "spu_delete_claim"
    Public Const ACDeleteClaimStored As Boolean = True

    Public Const ACGetClaimRiskStatusName As String = "GetClaimRiskStatus"
    Public Const ACGetClaimRiskStatusSQL As String = "spu_CLM_risk_status_sel2"
    Public Const ACGetClaimRiskStatusStored As Boolean = True

    ''PN 61908
    Public Const ACGetClaimTransType As String = "GetClaimTransType"
    Public Const ACGetClaimTransTypeSQL As String = "spu_SAM_CLM_Get_Claim_Transaction_Type"
    Public Const ACGetClaimTransTypeStored As Boolean = True
    ''PN 61908

    Public Const ACGetInfoOnlyStatusName As String = "GetInfoOnlyStatus"
    Public Const ACGetInfoOnlyStatusSQL As String = "spu_get_claim_info_only_status"
    Public Const ACGetInfoOnlyStatusStored As Boolean = True

    Public Const ACGetOriginalClaimIDName As String = "Get Original Claim ID"
    Public Const ACGetOriginalClaimIDSQL As String = "spu_CLM_Get_Base_Claim"

    ' Claims RI Arrangement
    Public Const ACCopyRIName As String = "CopyRI"
    Public Const ACCopyRISQL As String = "spu_copy_reinsurance_details_to_claim_RI2007"
    Public Const ACCopyRIStored As Boolean = True

    Public Const ACSelectRIArrangementName As String = "SelectRIArrangements"
    Public Const ACSelectRIArrangementSQL As String = "spu_Claim_RI_Arrangement_saa"
    Public Const ACSelectRIArrangementStored As Boolean = True

    Public Const ACSelectRIArrangementBandsName As String = "SelectRIArrangementBands"
    Public Const ACSelectRIArrangementBandsSQL As String = "spu_Claim_RI_Arrangement_sel_bands"
    Public Const ACSelectRIArrangementBandsStored As Boolean = True

    Public Const ACUpdateRIArrangementName As String = "UpdateRIArrangement"
    Public Const ACUpdateRIArrangementSQL As String = "spu_Claim_RI_Arrangement_upd"
    Public Const ACUpdateRIArrangementStored As Boolean = True

    'Claims RI Arrangement Line
    Public Const ACInsertRIArrangementLineName As String = "Insert RI Arrangement"
    Public Const ACInsertRIArrangementLineSQL As String = "spu_Claim_RI_Arrangement_Line_add"
    Public Const ACInsertRIArrangementLineStored As Boolean = True

    'Public Const ACSelectRIArrangementLineName = "Select RI Arrangement"
    'Public Const ACSelectRIArrangementLineSQL = "spu_Claim_RI_Arrangement_Line_saa"
    'Public Const ACSelectRIArrangementLineStored = True

    Public Const ACSelectRIArrangementLineRI2007Name As String = "Select RI Arrangement"
    Public Const ACSelectRIArrangementLineRI2007SQL As String = "spu_Claim_RI_Arrangement_Line_saa_RI2007"
    Public Const ACSelectRIArrangementLineRI2007Stored As Boolean = True


    Public Const ACUpdateRIArrangementLineName As String = "Update RI Arrangement"
    Public Const ACUpdateRIArrangementLineSQL As String = "spu_Claim_RI_Arrangement_Line_upd"
    Public Const ACUpdateRIArrangementLineStored As Boolean = True

    ' Treaty details
    Public Const ACSelectTreatyName As String = "SelectTreaty"
    Public Const ACSelectTreatySQL As String = "spu_Treaty_sel"
    Public Const ACSelectTreatyStored As Boolean = True

    Public Const ACAddBrokerParticipantsName As String = "AddBrokerParticipants"
    Public Const ACAddBrokerParticipantsSQL As String = "Spu_Sir_AddBrokerParticipants"
    Public Const ACAddBrokerParticipantsStored As Boolean = True

    Public Const ACDeleteRIArrangementLineRI2007Name As String = "DeleteRIArrangementLinesRI2007"
    Public Const ACDeleteRIArrangementLineRI2007SQL As String = "spu_Claim_RI_Arrangement_Line_Del_RI2007"
    Public Const ACDeleteRIArrangementLineRI2007Stored As Boolean = True

    Public Const ACUpdRIArrangementVersionName As String = "UpdRIArrangementVersion"
    Public Const ACUpdRIArrangementVersionSQL As String = "spu_Claim_RI_Arrangement_Version_upd"
    Public Const ACUpdRIArrangementVersionStored As Boolean = True
End Module