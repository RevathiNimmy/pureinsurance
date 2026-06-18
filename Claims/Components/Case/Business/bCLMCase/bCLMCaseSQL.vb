Option Strict Off
Option Explicit On
Module bCLMCaseSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Class Name: CLMCaseSQL
    '
    ' Date: 15/08/2007
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL)
    '
    ' Edit History:VB
    ' ***************************************************************** '

    Public Const kFindClaimName As String = "returns the claims that match the specified search parameters"
    Public Const kFindClaimSQL As String = "spu_Get_Case_Claim_Details"

    '====================

    ' Get datamodel code for claim
    Public Const kFindClaimCaseName As String = "Find Claim Case"
    Public Const kFindClaimCaseSQL As String = "spu_CLM_Case_Select"
    '
    Public Const kSaveCaseName As String = "Save Case"
    Public Const kSaveCaseSQL As String = "spu_CLM_Case_Add"

    Public Const kGetCaseClaimLinksName As String = "Get Case Claim Link"
    Public Const kGetCaseClaimLinksSQL As String = "spu_Get_Case_Claim_Link"

    Public Const kUpdateClaimCaseLinkName As String = "Update claim base case Id"
    Public Const kUpdateClaimCaseLinkSQL As String = "spu_Case_Update_Claim_CaseLink"

    Public Const kCaseHistoryName As String = "Case History"
    Public Const kCaseHistorySQL As String = "spu_Get_Case_History"

    Public Const kCloseCaseName As String = "Close Case"
    Public Const kCloseCaseSQL As String = "spu_Update_Case_Progress_Status"

    Public Const kGetEventTypeName As String = "Event Type"
    Public Const kGetEventTypeSQL As String = "spu_ACT_Spoke_Get_EventTypeIDFromCode"

    'Get Previously attached Case Builder Data Model if it differs from current one
    Public Const ACIsScreenDataModelChangedName As String = "spu_SIR_Is_Case_Screen_Data_Model_Changed"
    Public Const ACIsScreenDataModelChangedSQL As String = "spu_SIR_Is_Case_Screen_Data_Model_Changed"

    'Deletes all corresponding GIS data for a GIS Policy Link Id
    Public Const ACDeleteCustomDataName As String = "spu_SIR_Delete_GIS_Data"
    Public Const ACDeleteCustomDataSQL As String = "spu_SIR_Delete_GIS_Data"

    Public Const kGetCaseDetailsSQL As String = "spu_Get_Case_Details"
    Public Const kGetCaseDetailsName As String = "Get Case Details"

    Public Const kCleanUpDirtyCaseName As String = "cleans up dirty Cases"
    Public Const kCleanUpDirtyCaseSQL As String = "spu_CLM_Clean_Up_Dirty_Cases"

    Public Const kCopyCaseName As String = "Copy the Case Details and return the new Case id"
    Public Const kCopyCaseSQL As String = "spu_CLM_Copy_Case"

    ' Get datamodel code for Case
    Public Const ACGetDatamodeCodeforCaseStored As Boolean = True
    Public Const ACGetDatamodeCodeforCaseName As String = "Get Datamodel Code for Case"
    Public Const ACGetDatamodeCodeforCaseSQL As String = "spu_CLM_Get_DataModel_Code_for_CASE"

    ' returns the gis policy link id etc for the specified Case
    Public Const ACGetGisPolicyLinkDetailsName As String = "Get Gis Policy Link Details for Case"
    Public Const ACGetGisPolicyLinkDetailsSQL As String = "spu_CLM_Get_GIS_Policy_Link_Details_for_CASE"

    Public Const ACGISCopyDatasetStart As String = "spg_"
    Public Const ACGISCopyDatasetEnd As String = "_copy_dataset"

    Public Const ACUpdateGisPolicyLinkDetailsName As String = "Get Gis Policy Link Details for Case"
    Public Const ACUpdateGisPolicyLinkDetailsSQL As String = "spu_CLM_Update_GIS_Policy_Link_Details_for_CASE"
End Module