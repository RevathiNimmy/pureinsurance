Option Strict Off
Option Explicit On
Imports System

Imports SharedFiles

Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 14052003
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History: Ajit Kumar  - Created
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iCLMAuthorisePayments"


    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACErrCancelLose As Integer = 500
    Public Const ACErrCancel As Integer = 501
    Public Const ACErrCancelDetails As Integer = 502

    Public Const ACWarnDecline As Integer = 504
    Public Const ACWarnAuthorise As Integer = 505

    ' Menus

    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'Array positions
    Public Const ACColClaimId As Integer = 0
    Public Const ACColClaimNumber As Integer = 1
    Public Const ACColPolicyNumber As Integer = 2
    Public Const ACColClientName As Integer = 3
    Public Const ACColPaymentAmount As Integer = 4
    Public Const ACColPaymentDate As Integer = 5
    Public Const ACColCreatedBy As Integer = 6
    Public Const ACColStatus As Integer = 7
    Public Const ACColPaymentID As Integer = 8
    Public Const ACColOriginalUserID As Integer = 9

    Public Const ACColPaymentPayeePartyCnt As Integer = 10
    Public Const ACCOlPaymentCurrencyId As Integer = 11
    Public Const ACColPaymentDocumentId As Integer = 12
    Public Const ACColPaymentMediaTypeID As Integer = 13
    Public Const ACColPaymentPayeeAccountId As Integer = 14

    Public Const ACColIsReferredforRecommendationId As Integer = 10
    Public Const ACColRecommenderId As Integer = 11
    Public Const ACColProductIDId As Integer = 12
    Public Const KACCOlPayeeName As Integer = 17
    Public Const ACCOlClaimPaymentCurrencyId As Integer = 19


    'Broking & Underwriting
    Public Const ACIClmPolicyId As Integer = 0
    Public Const ACIClmClaimId As Integer = 1
    Public Const ACIClmDescription As Integer = 2
    Public Const ACIClmClaimNumber As Integer = 3
    Public Const ACIClmPolicyNumber As Integer = 4

    'Broking Only
    Public Const ACIClmClaimDate As Integer = 5
    Public Const ACIClmClientName As Integer = 6
    Public Const ACIClmStatusId As Integer = 7
    Public Const ACIClmHandler As Integer = 8
    Public Const ACIClmRiskTypeId As Integer = 9

    'Underwriting Only
    Public Const ACIClmRiskTypeIdU As Integer = 5
    Public Const ACIClmShortnameU As Integer = 6
    Public Const ACIClmClaimDateU As Integer = 7
    Public Const ACIClmClientNameU As Integer = 8
    Public Const ACIClmStatusIdU As Integer = 9
    Public Const ACIClmHandlerU As Integer = 10

    'AK 060603
    Public Const ACModeAuthorise As Integer = 11


    Public Const ACHasClaimPaymentauthority As Integer = 12
    Public Const ACClaimPaymentAmount As Integer = 13
    Public Const ACClaimPaymentCurrencyID As Integer = 17

    Public Const ACIsRecommender As Integer = 57
    Public Const ACRecommenderCurrency As Integer = 58
    Public Const ACRecommenderCurrAmount As Integer = 59


    ' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer

    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer

    <ThreadStatic()> _
    Public g_iUserID As Integer

    Public Const g_sUserAuthorityDenial As String = "This user does not have authority to perform this task"

    Public Const g_sDisabled As String = "Disabled"

    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sProduct As String = ""

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    Public Const kClaimPaymentAccountDetailsClaimPaymentId As Integer = 0
    Public Const kClaimPaymentAccountDetailsTotalPaymentAmount As Integer = 1
    Public Const kClaimPaymentAccountDetailsAccountId As Integer = 2
    Public Const kClaimPaymentAccountDetailsCurrencyId As Integer = 3
    Public Const kClaimPaymentAccountDetailsMediaTypeID As Integer = 4
    Public Const kClaimPaymentAccountDetailsDocumentID As Integer = 18
    Public Const kClaimPaymentAccountDetailsSourceId As Integer = 19

    Public Const kClaimPaymentAuthProcessRecommend As Integer = 0
    Public Const kClaimPaymentAuthProcessAuthorise As Integer = 1


    Public Sub Main_Renamed()
        '   This is used for testing purposes
        'Developer Guide No. 88
        Dim o As Object
        Dim vKeyArray(,) As Object

        Dim lReturn As gPMConstants.PMEReturnCode = CType(CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            o.CallingAppName = "TEST"
            lReturn = CType(o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)

            lReturn = CType(o.Start(), gPMConstants.PMEReturnCode)

            lReturn = CType(o.GetKeys(vKeyArray), gPMConstants.PMEReturnCode)
            o.Dispose()

        End If

        'End

    End Sub
End Module