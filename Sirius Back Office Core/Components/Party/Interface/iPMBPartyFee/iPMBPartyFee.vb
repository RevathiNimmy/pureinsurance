Option Strict Off
Option Explicit On
Imports System

Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 05/05/1999
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBPartyFee"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    '***************************************
    '***************************************
    ' AUS005 Changes
    Public Const kTableCurrency As String = "Currency"
    Public Const kTableProduct As String = "Product"
    Public Const kTableRiskTypeGroup As String = "Risk_Type_Group"
    Public Const kTablePerilGroup As String = "Peril_Group"
    Public Const kTableTaxGroup As String = "Tax_Group"
    Public Const kTableMakeLiveOptions = "MakeLiveOptions"
    Public Const kTableDoPaymentTerms = "DoPaymentTerms"

    Public Const ACDetailKey As Integer = 0
    Public Const ACDetailCode As Integer = 1
    Public Const ACDetailDesc As Integer = 2

    Public Const ACFeeAmountId As Integer = 0
    Public Const ACFeeAmountProductId As Integer = 1
    Public Const ACFeeAmountRiskTypeGroupId As Integer = 2
    Public Const ACFeeAmountPerilGroupId As Integer = 3
    Public Const ACFeeAmountFeePercentage As Integer = 4
    Public Const ACFeeAmountFeeAmount As Integer = 5
    Public Const ACFeeAmountCurrencyId As Integer = 6
    Public Const ACFeeAmountTransactionSubType As Integer = 7
    Public Const ACFeeAmountEffectiveDate As Integer = 8
    Public Const ACFeeAmountTaxGroupId As Integer = 9
    Public Const ACFeeAmountIsTaxAppliedToCr As Integer = 10
    Public Const ACFeeAmountIncludeIns As Integer = 11
    Public Const ACFeeAmountSpread As Integer = 12
    Public Const kFeeAmountMakeLiveOptions = 13
    Public Const kFeeAmountPaymentTerm = 14
    Public Const kFeeAmountNetPremiumWithTax = 15
    Public Const kFeeAmountApplyProrated = 16
    Public Const kFeeAmountTransactionTypeID = 17
    Public Const kFeeAmountOverrideRateAmount = 18
    Public Const kFeeAmountUseWhenDeleted = 19
    Public Const kFeeItemCalculationBasis = 28
    Public Const kFeeItemPaymentTerms = 29
    Public Const kFeeItemMakeLiveOptions = 30
    Public Const kFeeItemApplyProRated = 31
    Public Const kFeeItemProRataRate = 32
    Public Const kFeeItemIsOverridden = 33

    '***************************************
    '***************************************

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 305
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACCaptionName As Integer = 103
    Public Const ACCaptionPercentage As Integer = 104
    Public Const ACCaptionAmount As Integer = 105
    Public Const ACCaptionCommission As Integer = 106
    Public Const ACCaptionCommissionPercentage As Integer = 107
    Public Const ACCaptionCommissionAmount As Integer = 108
    Public Const ACCaptionDisplayOnQuotes As Integer = 109
    Public Const ACCaptionScheme As Integer = 110
    Public Const ACCaptionCurrency As Integer = 111
    'GW 210504 PN013 - extra constants added
    Public Const ACCaptionProduct As Integer = 112
    Public Const ACCaptionIsTaxable As Integer = 113
    Public Const ACCaptionTransactionType As Integer = 114
    Public Const ACCaptionEffectiveDate As Integer = 115
    Public Const ACCaptionFSATypeOfSale As Integer = 306

    Public Const ScreenHelpID As Integer = 40000

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

    '040506 Datasure
    Public Const ACTaxGroup As Integer = 304

    Public Enum eUFeesColumns
        Col_FeeType = 0
        Col_Premium = 1
        Col_UPerc = 2
        Col_UAmount = 3
        Col_IsValue = 4
        Col_IsAmmended = 5
    End Enum

    Public Const ACTransTypeBlank As Integer = 0
    Public Const ACTransTypeCancel As Integer = 1
    Public Const ACTransTypeMTA As Integer = 2
    Public Const ACTransTypeNewBusiness As Integer = 3
    Public Const ACTransTypeRenewal As Integer = 4
    Public Const ACTransTypeReInstatement As Integer = 5

    Public Const ACTransTypeCaptionCancel As String = "Cancel Policy"
    Public Const ACTransTypeCaptionMTA As String = "MTA"
    Public Const ACTransTypeCaptionNewBusiness As String = "New Business"
    Public Const ACTransTypeCaptionRenewal As String = "Renewals"
    Public Const ACTransTypeCaptionReInstatement As String = "Reinstate Policy"

    'Images
    Public Const ksAddressImage As String = "AddressImage"
    Public Const ksContactImage As String = "ContactImage"
    Public Const ksPolicyImage As String = "PolicyImage"


    ' Menus


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.

    <ThreadStatic()> _
 Public g_iSourceID As Integer

    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.

    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    Public Const ACExtraSchemeID As Integer = 0
    Public Const ACExtraSchemeDesc As Integer = 1

    Public Sub Main()

    End Sub

    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub

End Module