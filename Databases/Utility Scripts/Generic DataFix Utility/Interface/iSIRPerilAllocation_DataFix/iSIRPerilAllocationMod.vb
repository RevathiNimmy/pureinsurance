Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 01/12/1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' RAW 10/06/2003 : CR23 : added PremiumOverrideEventLevel option (1030)
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMUPerilAllocation"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    '*********************
    ' AUS005 Constants
    Public Const kRecalculateModeRatings As Integer = 1
    Public Const kRecalculateModeFees As Integer = 2
    Public Const kRecalculateModeRITax As Integer = 3

    Public Const kControlModeLoad As Integer = 1
    Public Const kControlModeRecalculate As Integer = 2
    '*********************

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabCaption As Integer = 101
    Public Const ACPolicyHolderLabel As Integer = 102
    Public Const ACPolicyRefLabel As Integer = 103
    Public Const ACRatingSectionGridColumn As Integer = 104
    Public Const ACPolicySectionGridColumn As Integer = 105
    Public Const ACRateTypeGridColumn As Integer = 106
    Public Const ACRateGridColumn As Integer = 107
    Public Const ACSumInsuredGridColumn As Integer = 108
    Public Const ACPremiumGridColumn As Integer = 109
    Public Const ACThisPremiumGridColumn As Integer = 110
    Public Const ACTotalPremiumLabel As Integer = 111
    Public Const ACReturnPremiumLabel As Integer = 112
    Public Const ACNetPremiumLabel As Integer = 113

    ' PW251102
    ' PS411
    Public Const ACOldAnnualPremiumLabel As Integer = 114
    Public Const ACNewAnnualPremiumLabel As Integer = 115
    Public Const ACPremiumDueLabel As Integer = 116
    Public Const ACNetTotalLabel As Integer = 117
    Public Const ACTaxTotalLabel As Integer = 118
    Public Const ACGrossTotalLabel As Integer = 119

    Public Const ACDetailTitle As Integer = 121
    Public Const ACDetailTabCaption As Integer = 122
    Public Const ACRatingSectionTypeLabel As Integer = 123
    Public Const ACPolicySectionTypeLabel As Integer = 124
    Public Const ACRateTypeLabel As Integer = 125
    Public Const ACRateLabel As Integer = 126
    Public Const ACSumInsuredLabel As Integer = 127
    Public Const ACPremiumLabel As Integer = 128
    Public Const ACThisPremiumLabel As Integer = 129
    Public Const ACEarningPatternLabel As Integer = 305

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACDeleteButton As Integer = 206

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACEarningPatternGridColumn As Integer = 304

    ' Menus
    Public Const ACMenuParty As Integer = 400
    Public Const ACMenuPartyAdd As Integer = 401
    Public Const ACMenuPartyRemove As Integer = 402


    ' Constants to define result array positions
    Public Const ACProductDescription As Integer = 0
    Public Const ACInsuranceRef As Integer = 1
    Public Const ACQuoteRef As Integer = 2
    Public Const ACPolicyHolderName As Integer = 3
    Public Const ACInsuredName As Integer = 4
    Public Const ACCoverFromDate As Integer = 5
    Public Const ACCoverToDate As Integer = 6
    Public Const ACInceptionDate As Integer = 7
    Public Const ACIssueDate As Integer = 8
    Public Const ACPremium As Integer = 9
    Public Const ACCurrencyCode As Integer = 10


    'Constants to define Listview column positions
    Public Const ACRatingSectionTypeCol As Integer = 0
    Public Const ACPolicySectionTypeCol As Integer = 1
    Public Const ACRateTypeCol As Integer = 2
    Public Const ACRateCol As Integer = 3
    Public Const ACSumInsuredCol As Integer = 4
    Public Const ACThisPremiumCol As Integer = 5
    Public Const ACPremiumCol As Integer = 6
    Public Const ACCountryCol As Integer = 7
    Public Const ACStateCol As Integer = 8
    Public Const ACRatingSectionIdCol As Integer = 9
    Public Const ACRatingSectionTypeIdCol As Integer = 10
    Public Const ACPolicySectionTypeIdCol As Integer = 11
    Public Const ACRateTypeIdCol As Integer = 12
    Public Const ACOriginalFlagCol As Integer = 13
    Public Const ACDefinedCurrencyID As Integer = 14
    Public Const ACCountryIDCol As Integer = 15
    Public Const ACStateIDCol As Integer = 16
    Public Const ACIsAmendedCol As Integer = 17
    Public Const ACCalculatedPremiumCol As Integer = 18
    Public Const ACOverrideReasonCol As Integer = 19
    Public Const ACAutoCalculatedCol As Integer = 20
    Public Const ACEarningPatternCol As Integer = 1
    Public Const ACEarningPatternIDCol As Integer = 11
    Public Const ACEarningPatternArrPos As Integer = 21
    Public Const ACEarningPatternIDArrPos As Integer = 22
    Public Const ACIsLeviTax As Integer = 23

    ' PW191102 - constants for Risk Tax Array
    ' PS411
    Public Const ACRRiskCnt As Integer = 0
    Public Const ACRTaxBandId As Integer = 1
    Public Const ACRPremium As Integer = 2
    Public Const ACRPercentage As Integer = 3
    Public Const ACRValue As Integer = 4
    Public Const ACRIsValue As Integer = 5
    Public Const ACRIsManuallyChanged As Integer = 6
    Public Const ACRDescription As Integer = 7

    ' {* USER DEFINED CODE (End) *}

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

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
 Public g_iUserID As Integer ' RAW 10/06/2003 : CR23 : added
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sPassword As String = ""

    ' Public array for storing data for the grid.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_vGridData As Object
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_vRatingSectionType As Object

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' KB 130801 required for help text, screenhelpid will need to be amended
    ' when text available
    Public Const ScreenHelpID As Integer = 4001
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    ' PW191102 - When taxes required option
    ' PS411
    Public Const ACOptWhenTaxesRequired As Integer = 1007
    ' RAW 10/06/2003 : CR23 : added
    Public Const ACOptPremiumOverrideEventLevel As Integer = 1030
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public m_vCoverDates As Object

    Sub Main_Renamed()

    End Sub
End Module