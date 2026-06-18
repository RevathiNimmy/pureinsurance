Option Strict Off
Option Explicit On
Module SQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: SQL
    '
    ' Date: {TodaysDate}
    '
    ' Description: Contains the SQL Statements required by the
    '              OpenClaim class.
    '
    ' Edit History: Written by Sravan Kumar.G
    '               Pandu-Added Parameters for New Fields
    ' ***************************************************************** '

    'SQL Statements

    ' Select Claim SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleClaim"
    Public Const ACSelectSingleSQL As String = "spu_Claim_sel"

    ' Add Claim SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddClaim"
    'RWH(10/11/2000) Added extra param to pass in generated claim no.
    'CMG(SJP) 21/02/2003 Added extra param to pass in branch id for system option query
    'DD 29/03/2004 Added Underwriting Year ID
    'S4B Claim Enhancements R&D 2005
    Public Const ACAddSQL As String = "spu_Claim_add"

    ' Delete Claim SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteClaim"
    Public Const ACDeleteSQL As String = "spu_Claim_del"

    ' Update Claim SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateClaim"
    'RWH(14/11/2000) Added extra param to pass in generated claim no.
    'DD 29/03/2004 Added Underwriting Year ID
    'S4B Claim Enhancements R&D 2005
    Public Const ACUpdateSQL As String = "spu_Claim_upd"

    'DN 27/03/01 Select policy Description for use in documaster
    Public Const ACGetPartyCntStored As Boolean = True
    Public Const ACGetPartyCntName As String = "GetPartyCnt"
    Public Const ACGetPartyCntSQL As String = "spu_CLM_Get_Party_From_Shortname"

    'DJM 19/09/2003 : Added script to get InsuranceFileCnt.
    Public Const ACGetInsuranceFolderCntStored As Boolean = True
    Public Const ACGetInsuranceFolderCntName As String = "GetInsuranceFolderCnt"
    Public Const ACGetInsuranceFolderCntSQL As String = "spu_CLM_Get_Insurance_File_Details"

    'DN 27/03/01 Select Claim Number for use in Documaster
    Public Const ACGetClaimNoStored As Boolean = True
    Public Const ACGetClaimNoName As String = "GetClaimNo"
    Public Const ACGetClaimNoSQL As String = "spu_get_claimnumber"

    'DC150402 -Start
    Public Const ACGetClaimCommentsStored As Boolean = True
    Public Const ACGetClaimComments As String = "GetClaimCommemts"
    Public Const ACGetClaimCommentsSQL As String = "spu_claim_comments_sel"

    Public Const ACDeleteClaimCommentsStored As Boolean = True
    Public Const ACDeleteClaimCommentsName As String = "DeleteClaimComments"
    Public Const ACDeleteClaimCommentsSQL As String = "spu_claim_comments_del"

    Public Const ACAddClaimCommentsStored As Boolean = True
    Public Const ACAddClaimCommentsName As String = "AddClaimComments"
    Public Const ACAddClaimCommentsSQL As String = "spu_claim_comments_add"

    Public Const ACUpdateClaimPolicyDetailsStored As Boolean = True
    Public Const ACUpdateClaimPolicyDetailsName As String = "UpdateClaimPolicyDetails"
    Public Const ACUpdateClaimPolicyDetailsSQL As String = "spu_claim_policy_upd"

    'DC150402 -End
End Module