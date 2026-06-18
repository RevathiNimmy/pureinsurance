Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule


    Public Const ACApp As String = "iSIRClientTransClientSel"

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.

    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iUserID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_oPMUser As Object




    '**********************************************
    ' list view Policy level constants
    '**********************************************
    Public Const kPolicyColHIndexNumber As Integer = 0
    Public Const kPolicyColHIndexType As Integer = 1
    Public Const kPolicyColHIndexProductName As Integer = 2
    Public Const kPolicyColHIndexRegarding As Integer = 3
    Public Const kPolicyColHIndexrenewalDate As Integer = 4
    Public Const kPolicyColHIndexagent As Integer = 5
    Public Const kPolicyColHIndexPremium As Integer = 6
    Public Const kPolicyColHIndexStatus As Integer = 7
    Public Const kPolicyColHIndexRiskDescription As Integer = 8
    Public Const kPolicyColHIndexEventDescription As Integer = 9
    Public Const kPolicyColHIndexActivePlansCount As Integer = 10
    '**********************************************
    ' list view Policy column Headers
    '**********************************************
    Public Const kRegKeyConstLvwPolicyNumber As Integer = 300
    Public Const kRegKeyConstLvwPolicyType As Integer = 301
    Public Const kRegKeyConstLvwProductName As Integer = 302
    Public Const kRegKeyConstLvwRegarding As Integer = 303
    Public Const kRegKeyConstLvwRenewalDate As Integer = 304
    Public Const kRegKeyConstLvwAgent As Integer = 305
    Public Const kRegKeyConstLvwPremium As Integer = 306
    Public Const kRegKeyConstLvwPolicyStatus As Integer = 307
    Public Const kRegKeyConstRiskTypeDescription As Integer = 308
    Public Const kRegKeyConstEventDescription As Integer = 309
    Public Const kRegKeyConstActivePlansCount As Integer = 310

    '**********************************************
    ' list view Policy constants
    '**********************************************
    Public Const kPolicySelectionColHIndexPolicyNumber As Integer = 0
    Public Const kPolicySelectionColHIndexPolicyType As Integer = 1
    Public Const kPolicySelectionColHIndexProductName As Integer = 2
    Public Const kPolicySelectionColHIndexRegarding As Integer = 3
    Public Const kPolicySelectionColHIndexRenewalDate As Integer = 4
    Public Const kPolicySelectionColHIndexAgent As Integer = 5
    Public Const kPolicySelectionColHIndexPremium As Integer = 6
    Public Const kPolicySelectionColHIndexPolicyStatus As Integer = 7
    Public Const kPolicySelectionColHIndexRiskTypeDescription As Integer = 8
    Public Const kPolicySelectionColHIndexEventDescription As Integer = 9
    Public Const kPolicySelectionColHIndexActivePlansCount As Integer = 10

    Public Enum ENPolicy
        InsuranceFileId = 0
        SourceID = 1
        InsuranceFileCnt = 2
        InsuranceRef = 3
        InsuranceFolderCode = 4
        TypeCode = 5
        InsuredName = 6
        shortname = 7
        PartyId = 8
        PartySourceId = 9
        RenewalDate = 10
        InsuranceHolderCnt = 11
        InsuranceFolderCnt = 12
        ProductId = 13
        ProductCode = 14
        Description = 15
        AgentCnt = 16
        DateCreated = 17
        Status = 18
        AgentName = 19
        Premium = 20
        PolicyTypeId = 21
        PolicyType = 22
        TypeDesc = 23
        NoOfClaims = 26
        AnniversaryCopy = 27
        EventDesciption = 28
        ActivePlansCount = 30
    End Enum



    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303



End Module