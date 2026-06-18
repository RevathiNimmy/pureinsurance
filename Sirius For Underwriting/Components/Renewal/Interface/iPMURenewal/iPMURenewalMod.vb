Option Strict Off
Option Explicit On
Imports System

Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 25/09/2000
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMURenewal"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons
    ' Form
    ' Buttons

    ' Messages
    Public Const ACInvalidStatusTitle As Integer = 300
    Public Const ACInvalidStatus As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACStatusSearching As Integer = 304
    Public Const ACStatusFound As Integer = 305
    Public Const ACRerateNow As Integer = 306
    Public Const ACReprintNow As Integer = 307
    Public Const ACAmmendQuery As Integer = 308
    Public Const ACValidationTitle As Integer = 309
    Public Const ACMandatoryStartDate As Integer = 310
    Public Const ACMandatoryExpiryDate As Integer = 311
    Public Const ACMandatoryStartGreaterThanExpiry As Integer = 312
    Public Const ACLapseTitle As Integer = 313
    Public Const ACLapseComplete As Integer = 314
    Public Const ACAmendTitle As Integer = 315
    Public Const ACAmendProcessLaunchFail As Integer = 316
    Public Const ACAmendSetKeysFail As Integer = 317
    Public Const ACAmendSetProcessModesFail As Integer = 318
    Public Const ACAmendProcessFail As Integer = 319
    Public Const ACDateColumn As Integer = 2
    'VB 21/02/2005 PN-18895 Added constant for RenewalDate column
    Public Const ACRenewalDate As Integer = 5

    ' Menus



    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"


    ' Constants for the Renewals search data array indexes.
    Public Const ACIRenewalStatusId As Integer = 0
    Public Const ACIRenewalProduct As Integer = 1
    Public Const ACIRenewalInsuranceHolder As Integer = 2
    Public Const ACIRenewalShortname As Integer = 3
    Public Const ACIRenewalPartyType As Integer = 4
    Public Const ACIRenewalLivePolicy As Integer = 5
    Public Const ACIRenewalPolicyCnt As Integer = 6
    Public Const ACIRenewalPolicy As Integer = 7
    Public Const ACIRenewalInsuranceFolder As Integer = 8
    Public Const ACIRenewalInsuranceStructID As Integer = 9
    Public Const ACIRenewalStatusTypeId As Integer = 10
    Public Const ACIRenewalStatusType As Integer = 11
    Public Const ACIRenewalCriticalDate As Integer = 12
    Public Const ACIRenewalLivePolicyCnt As Integer = 13
    Public Const ACIRenewalCoverStartDate As Integer = 14
    Public Const ACIRenewalExpiryDate As Integer = 15
    Public Const ACIRenewalAgentCnt As Integer = 16
    Public Const ACIRenewalProductId As Integer = 17
    Public Const ACIRenewalDate As Integer = 18
    Public Const ACIRenewalLeadAgentCode As Integer = 19
    Public Const ACIRenewalAccHandlerCode As Integer = 20
    Public Const ACIRenewalSourceCode As Integer = 21
    Public Const ACIRenewalClaimsIndicator As Integer = 22
    Public Const ACIRenewalClosedBranch As Integer = 23
    Public Const ACIRenewalIsInTransferMode As Integer = 24
    Public Const ACIRenewalTransferToPartyCnt As Integer = 25
    Public Const ACIRenewalTransferToShortname As Integer = 26
    Public Const ACIRenewalProductIsTrueMonthlyPolicy As Integer = 27
    Public Const ACIRenewalAnniversaryCopy As Integer = 28
    Public Const ACIRenewalExceptionReason As Integer = 31
    Public Const ACIRenewalRENProductId As Integer = 32
    Public Const ACIRenewalPaymentMethod As Integer = 33
    'Constants for Renewal Statuses
    Public Const ACAwaitManReview As Integer = 1
    Public Const ACAwaitRenewalPrint As Integer = 2
    Public Const ACAwaitManRatingFail As Integer = 3
    Public Const ACPolicyDetailsChanged As Integer = 4
    Public Const ACAwaitUpdate As Integer = 5
    Public Const ACAwaitManRating As Integer = 6

    'Constants for Business Types. RWH(05/10/2000)
    Public Const ACBusinessTypeQuote As Integer = 1
    Public Const ACBusinessTypePolicy As Integer = 2
    Public Const ACBusinessTypeProvClaim As Integer = 3
    Public Const ACBusinessTypeFullClaim As Integer = 4

    'Constants for Lapse reasons search data array indexes.
    Public Const ACLapseReasonID As Integer = 0
    Public Const ACLapseReason As Integer = 1

    'RWH(12/02/2001) Doc Generation modes.
    Public Const ACPrintMode As Integer = 2
    Public Const ACPrintSilentMode As Integer = 3
    Public Const ACSpoolSilentMode As Integer = 4

    'DN 21/02/03
    Public Const ACDOCTypeDebitNote As Integer = 3
    Public Const ACDocTypeSchedule As Integer = 4
    Public Const ACDocTypeCertificate As Integer = 5
    Public Const ACDocTypeLapse As Integer = 8

    'PSL 6535 10/09/2003 neew Renewal Debit Note doc type
    Public Const ACDocTypeRenewalDebitNote As Integer = 14

    ' Public source and language ID's from the
    ' Object Manager.

    <ThreadStatic()> _
 Public g_iSourceID As Integer

    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    <ThreadStatic()> _
 Public g_lInsuranceFileCnt As Integer

    ' Public instance of the object manager.

    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the renewal business object.

    <ThreadStatic()> _
 Public g_oRenewal As Object

    <ThreadStatic()> _
 Public g_lReasonID As Integer

    <ThreadStatic()> _
 Public g_sReasonDesc As String = ""

    Sub Main_Renamed()

    End Sub
End Module