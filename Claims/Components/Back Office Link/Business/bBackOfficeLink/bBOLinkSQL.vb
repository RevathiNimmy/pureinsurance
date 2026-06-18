Option Strict Off
Option Explicit On
Module bBOLInkSQL
    ' Select GetClientDetails SQL
    Public Const ACGetSiriusProdStored As Boolean = True
    Public Const ACGetSiriusProdName As String = "GetSiriusProd"
    Public Const ACGetSiriusProdSQL As String = "spu_get_sirius_prod"


    ' Find Insurance from built query.
    Public Const ACInsFileSearchStored As Boolean = False
    Public Const ACInsFileSearchName As String = "GetPolicyList"

    ' Select GetPartyAdds SQL
    Public Const ACGetPartyAddsStored As Boolean = True
    Public Const ACGetPartyAddsName As String = "GetPartyAdds"
    Public Const ACGetPartyAddsSQL As String = "spu_get_party_adds"

    ' Select GetPartyContacts SQL
    Public Const ACGetPartyContactsStored As Boolean = True
    Public Const ACGetPartyContactsName As String = "GetPartyContacts"
    Public Const ACGetPartyContactsSQL As String = "spu_get_party_contacts"

    ' Select GetClientDetails SQL
    Public Const ACGetClientDetailsStored As Boolean = True
    Public Const ACGetClientDetailsName As String = "GetClientDetails"
    Public Const ACGetClientDetailsSQL As String = "spu_get_client_details"

    ' Select GetInsurerDetails SQL
    Public Const ACGetInsurerDetailsStored As Boolean = True
    Public Const ACGetInsurerDetailsName As String = "GetInsurerDetails"
    Public Const ACGetInsurerDetailsSQL As String = "spu_get_insurer_details"

    ' Select GetRiskDetails SQL
    Public Const ACGetRiskDetailsStored As Boolean = True
    Public Const ACGetRiskDetailsName As String = "GetRiskDetailsBroking"
    Public Const ACGetRiskDetailsSQL As String = "spu_get_risk_details"

    ' Select GetRiskDetails SQL
    Public Const ACGetRiskDetailsUStored As Boolean = True
    Public Const ACGetRiskDetailsUName As String = "GetRiskDetailsUnderWriting"
    Public Const ACGetRiskDetailsUSQL As String = "spu_get_risk_details_U"

    ' Select GetRiskDetails SQL
    Public Const ACGetRiskDetailsOpenClaimUStored As Boolean = True
    Public Const ACGetRiskDetailsOpenClaimUName As String = "GetRiskDetailsUnderWritingOpenClaim"
    Public Const ACGetRiskDetailsOpenClaimUSQL As String = "spu_get_risk_details_open_claim_U"

    ' Select GetRiskDesc SQL
    Public Const ACGetRiskDescStored As Boolean = True
    Public Const ACGetRiskDescName As String = "GetRiskDesc"
    Public Const ACGetRiskDescSQL As String = "spu_get_risk_desc"

    ' Select Get Policy Details SQL
    Public Const ACGetPolicyDetailsStored As Boolean = True
    Public Const ACGetPolicyDetailsName As String = "GetPolicyDetails"
    Public Const ACGetPolicyDetailsSQL As String = "spu_get_policy_details"

    'get latest version of policy
    Public Const ACGetLatestVersionPolicyStored As Boolean = True
    Public Const ACGetLatestVersionPolicyName As String = "GetLatestVersionPolicy"
    Public Const ACGetLatestVersionPolicySQL As String = "spu_GetPolicy_U"
    Public Const ACGetClientDetailsUStored As Boolean = True
    Public Const ACGetClientDetailsUName As String = "GetClientDetailsUnderwriting"
    Public Const ACGetClientDetailsUSQL As String = "spu_get_client_details_U"

    ' Find Insurance from Search Index (Using GIS Stored Procedure)
    Public Const ACInsLikeIndexGISSearchStored As Boolean = True
    Public Const ACInsLikeIndexGISSearchName As String = "FindInsuranceLikeIndexGISSearch"
    Public Const ACInsLikeIndexGISSearchSQL As String = "spu_gis_search_property_find"

    Public Const ACGisSearchPropertyGisStored As Boolean = True
    Public Const ACGisSearchPropertyGisName As String = "Gis Search Property Gis For Risk"
    Public Const ACGisSearchPropertyGisSQL As String = "spu_gis_search_property_risk"

    Public Const ACGetPolicyForClaimDateStored As Boolean = True
    Public Const ACGetPolicyForClaimDateName As String = "Get Policy For Claim Date"
    Public Const ACGetPolicyForClaimDateSQL As String = "spu_Get_Policy_For_Claim_Date"

End Module