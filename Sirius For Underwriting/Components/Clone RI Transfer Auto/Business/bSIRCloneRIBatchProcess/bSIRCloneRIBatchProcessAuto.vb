Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  26/09/00
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRCloneRIBatchProcess"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"





    'Constants for Renewal Statuses
    Public Const ACAwaitManReview As Integer = 1
    Public Const ACAwaitRenewalPrint As Integer = 2
    Public Const ACAwaitManRatingFail As Integer = 3
    Public Const ACPolicyDetailsChanged As Integer = 4
    Public Const ACAwaitUpdate As Integer = 5
    Public Const ACAwaitManRating As Integer = 6


    Public Enum ClaimVersionsEnum
        DBClaimID
        DBPolicyID
        DBPolicyNumber
        DBClaimNumber
        DBRiskTypeID
        DBClientID
        DBClientName
        DBClaimClonedRIUsageID
        DBOldInsuranceFileCnt
        DBNewInsuranceFileCnt
        DBOldRiskCnt
        DBNewRiskCnt
        DBStatus
        DBTransactionCode
        DBShortCode
        DBPartyName
        ' Max count for array manipulation
        DBRIMax = ClaimVersionsEnum.DBPartyName
    End Enum

    Sub Main_Renamed()


    End Sub
End Module