Option Strict Off
Option Explicit On
Module BusinessSQL

    ' adds a claim party link record for the specified party cnt / claim id
    'developer guide no.39
    Public Const ACAddClaimPartyLinkSQL As String = "spu_CLM_Claim_Party_Link_Add"
    Public Const ACAddClaimPartyLinkName As String = "adds a claim party link for the specified claim id / party cnt"

    ' deletes a claim party link record for the specified party cnt / claim id
    'developer guide no.39
    Public Const ACDeleteClaimPartyLinkSQL As String = "spu_CLM_Claim_Party_Link_Delete"
    Public Const ACDeleteClaimPartyLinkName As String = "deletes a claim party link for the specified claim id / party cnt"
End Module