Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 09/06/1999
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMUTaxBandRate"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102

    Public Const ACCTaxBandId As Integer = 111
    Public Const ACHCode As Integer = 112
    Public Const ACHCaptionId As Integer = 113
    Public Const ACHDescription As Integer = 114
    Public Const ACHEffectiveDate As Integer = 115
    Public Const ACHIsDeleted As Integer = 116
    Public Const ACHIsValue As Integer = 117
    Public Const ACHRate As Integer = 118
    Public Const ACCCode As Integer = 119
    Public Const ACCCaptionId As Integer = 120
    Public Const ACCDescription As Integer = 121
    Public Const ACCEffectiveDate As Integer = 122
    Public Const ACCIsDeleted As Integer = 123
    Public Const ACCIsValue As Integer = 124
    Public Const ACCRate As Integer = 125
    Public Const ACCCurrency As Integer = 126

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307

    ' Menus


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Constants for the search data array indexes.
    Public Const ACRTaxBandId As Integer = 0
    Public Const ACRTaxBandRateId As Integer = 1
    Public Const ACRCode As Integer = 2
    Public Const ACRCaptionId As Integer = 3
    Public Const ACRDescription As Integer = 4
    Public Const ACREffectiveDate As Integer = 5
    Public Const ACRIsDeleted As Integer = 6
    Public Const ACRIsValue As Integer = 7
    Public Const ACRRate As Integer = 8
    Public Const ACRCalcBasis As Integer = 9
    Public Const ACRSumInsuredValue As Integer = 10
    Public Const ACRNB As Integer = 11
    Public Const ACRAMTA As Integer = 12
    Public Const ACRRMTA As Integer = 13
    Public Const ACRCANC As Integer = 14
    Public Const ACRREN As Integer = 15
    Public Const ACRSumInsuredRounded As Integer = 16
    Public Const ACRCurrencyID As Integer = 17
    Public Const ACRAllowTaxCredit As Integer = 18
    Public Const ACRCountryID As Integer = 19
    Public Const ACRStateID As Integer = 20
    Public Const ACRCOBID As Integer = 21
    Public Const ACRCOBDesc As Integer = 22
    Public Const ACRCountryDesc As Integer = 23
    Public Const ACRStateDesc As Integer = 24
    Public Const ACRTTRI As Integer = 25
    Public Const ACRTTRIC As Integer = 26
    Public Const ACRTTAC As Integer = 27
    Public Const ACRTTF As Integer = 28
    Public Const ACRTTCP As Integer = 29
    Public Const ACRTTCS As Integer = 30
    Public Const ACRTTCR As Integer = 31
    Public Const ACRTTIC As Integer = 32
    Public Const ACRMTAThresholdDate As Integer = 33
    Public Const ACRIsPassedToInsurer As Integer = 34
    Public Const ACRTTI As Integer = 35
    ' 020506 Datasure
    Public Const ACRTTE As Integer = 36
    Public Const ACRRiskGroupId As Integer = 37
    Public Const ACRRiskCodeId As Integer = 38
    Public Const ACRCOBRatingSectionId As Integer = 39
    Public Const ACRRiskGroupDesc As Integer = 40
    Public Const ACRRiskCodeDesc As Integer = 41
    Public Const ACRCOBRatingSecDesc As Integer = 42
    Public Const ACRUseForRefundWhenExpired As Integer = 43
    Public Const ACRUseForBackdatedNB As Integer = 44
    Public Const kIsRIPaymentsRecoveries As Integer = 45
    ' 020506 Datasure
    Public Const ACRMaxArray As Integer = 45 '43 (RC)


    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sUnderwritingOrAgency As String = ""
    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oGIS As Object

    ' KB 130801 required for help text, screenhelpid will need to be amended
    ' when text available
    Public Const ScreenHelpID As Integer = 4001
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Sub Main_Renamed()

    End Sub
End Module