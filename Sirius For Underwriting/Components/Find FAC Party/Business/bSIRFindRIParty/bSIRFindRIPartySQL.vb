Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	
	Public Const ACGetRIPartyName As String = "GetPartyInsurerDetails"
	Public Const ACGetRIPartySQL As String = "Spu_Sir_GetPartyInsurerDetails"
	Public Const ACGetRIPartyStored As Boolean = True
	
	Public Const ACGetBrokerParticipantsName As String = "GetBrokerParticipants"
	Public Const ACGetBrokerParticipantsSQL As String = "Spu_Sir_GetBrokerParticipants"
	Public Const ACGetBrokerParticipantsStored As Boolean = True
	
	Public Const ACAddBrokerParticipantsName As String = "AddBrokerParticipants"
	Public Const ACAddBrokerParticipantsSQL As String = "Spu_Sir_AddBrokerParticipants"
	Public Const ACAddBrokerParticipantsStored As Boolean = True
	
	Public Const ACDelBrokerParticipantsName As String = "DelBrokerParticipants"
	Public Const ACDelBrokerParticipantsSQL As String = "Spu_Sir_DelBrokerParticipants"
	Public Const ACDelBrokerParticipantsStored As Boolean = True
	
	Public Const ACAddPlacementRiLinesName As String = "AddPlacementRiLines"
	Public Const ACAddPlacementRiLinesSQL As String = "spu_RI_Arrangement_Line_add"
	Public Const ACAddPlacementRiLinesStored As Boolean = True
	
	'For Claim
	Public Const ACAddClaimPlacementRiLinesName As String = "AddClaimPlacementRiLines"
	Public Const ACAddClaimPlacementRiLinesSQL As String = "spu_Claim_Ri_Arrangement_Line_add"
	Public Const ACAddClaimPlacementRiLinesStored As Boolean = True
	
	Public Const ACGetClaimGroupedRiLinesName As String = "GetClaimGroupedRiLines"
	Public Const ACGetClaimGroupedRiLinesSQL As String = "Spu_Sir_GetClaimGroupedRiArangementLines"
	Public Const ACGetClaimGroupedRiLinesStored As Boolean = True
	
	Public Const ACUpdatePlacementRiLinesName As String = "UpdPlacementRiLines"
	Public Const ACUpdatePlacementRiLinesSQL As String = "spu_RI_Arrangement_Line_upd_RI2007"
	Public Const ACUpdatePlacementRiLinesStored As Boolean = True
	
	Public Const ACUpdRiArrangementLineGroupingName As String = ""
	Public Const ACUpdRiArrangementLineGroupingSQL As String = "Spu_Sir_Update_RiArrangementLine_Grouping"
	Public Const ACUpdRiArrangementLineGroupingStored As Boolean = True
	
	Public Const ACDelPlacementRiLinesName As String = "Delete_RI2007ArrangementLines"
	Public Const ACDelPlacementRiLinesSQL As String = "Spu_Sir_Delete_RI2007ArrangementLines"
	Public Const ACDelPlacementRiLinesStored As Boolean = True
	
	Public Const ACGetGroupedRiLinesName As String = "GetGroupedRiLines"
	Public Const ACGetGroupedRiLinesSQL As String = "Spu_Sir_GetGroupedRiArangementLines"
	Public Const ACGetGroupedRiLinesStored As Boolean = True
	
	Public Const ACUpdateClaimPlacementRiLinesName As String = "UpdClaimPlacementRiLines"
	Public Const ACUpdateClaimPlacementRiLinesSQL As String = "spu_Claim_RI_Arrangement_Line_upd_RI007"
	Public Const ACUpdateClaimPlacementRiLinesStored As Boolean = True

    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module