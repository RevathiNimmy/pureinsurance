Option Strict Off
Option Explicit On
Imports System
Imports System.Globalization
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
    '******************************************************************************
    ' Module Name:      MainModule
    ' History:          Created 22 Aug 2000
    ' Description:      Main module containing public variable/constants.
    '******************************************************************************

    ' Main public constant for all functions to identify which application this is
    Public Const ACApp As String = "uctPMUFeesControl"

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public contants used for the start and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1
    Public Const ACReserveFrame As Integer = 125
    Public Const ACEditButton As Integer = 205

    'Constants used for resizing & positioning the user control

    ' list view column headers
    Public Const kFeeColHFeeType As Integer = 1
    Public Const kFeeColHCurrency As Integer = 2
    Public Const kFeeColHAppliedTo As Integer = 3
    Public Const kFeeColHPremium As Integer = 4
    Public Const kFeeColHRate As Integer = 5
    Public Const kFeeColHFeeAmount As Integer = 6
    Public Const kFeeColHTaxAmount As Integer = 7
    Public Const kFeeColHTotalAmount As Integer = 8
    Public Const kFeeColHTaxGroup As Integer = 9
    Public Const kFeeColHIncludeIns As Integer = 10
    Public Const kFeeColHSpread As Integer = 11

    ' fee details array positions
    Public Const kFeeItemResolvedName As Integer = 0
    Public Const kFeeItemProductId As Integer = 1
    Public Const kFeeItemProductDesc As Integer = 2
    Public Const kFeeItemTransactionTypeId As Integer = 3
    Public Const kFeeItemTransactionTypeDesc As Integer = 4
    Public Const kFeeItemFeeAmountId As Integer = 5
    Public Const kFeeItemPartyCnt As Integer = 6
    Public Const kFeeItemFeePercentage As Integer = 7
    Public Const kFeeItemFeeAmount As Integer = 8
    Public Const kFeeItemEffectiveDate As Integer = 9
    Public Const kFeeItemTaxGroupId As Integer = 10
    Public Const kFeeItemFeeCharge As Integer = 11
    Public Const kFeeItemCurrencyId As Integer = 12
    Public Const kFeeItemCurrencyDesc As Integer = 13
    Public Const kFeeItemPerilGroupId As Integer = 14
    Public Const kFeeItemPerilGroupDesc As Integer = 15
    Public Const kFeeItemRiskGroupId As Integer = 16
    Public Const kFeeItemRiskGroupDesc As Integer = 17
    Public Const kFeeItemCurrencyIsoCode As Integer = 18
    Public Const kFeeItemCompanyId As Integer = 19
    Public Const kFeeItemCurrencyAmount As Integer = 20
    Public Const kFeeItemTaxAmount As Integer = 21
    Public Const kFeeItemTaxGroupDescription As Integer = 22
    Public Const kFeeitemPolicyFeeUId As Integer = 23
    Public Const kFeeItemFeePremium As Integer = 24
    Public Const kFeeItemIncludeIns As Integer = 25
    Public Const kFeeItemSpread As Integer = 26
    Public Const kFeeItemTransCurrencyISOCode As Integer = 27
    Public Const kFeeItemFeeTypePercent As Integer = 28
    Public Const kFeeItemCalculationBasis As Integer = 29
    Public Const kFeeItemPaymentTerms As Integer = 30
    Public Const kFeeItemMakeLiveOptions As Integer = 31
    Public Const kFeeItemApplyProRated As Integer = 32
    Public Const kFeeItemProRataRate As Integer = 33
    Public Const kFeeItemIsOverridden As Integer= 34

    ' policy details array positions
    Public Const kPolicyCoverStartDate As Integer = 0
    Public Const kPolicyThisPremium As Integer = 1
    Public Const kPolicyExpiryDate As Integer = 2
    Public Const kPolicyAnnualPremium As Integer = 3

    Public Const kTableCurrency As String = "Currency"
    Public Const kTableTaxGroup As String = "Tax_Group"

    Public Const kModeRecalculate As Integer = 1

    Public Const kRiskDetailItemThisPremium As Integer = 18

    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    Public Function IsValidCurrency(ByRef cValue As String) As Integer
        ' Function to test value supplied is a valid currency value
        Dim result As Integer = 0
        Try

            Dim curTemp As Decimal

            ' set the default return value
            result = gPMConstants.PMEReturnCode.PMFalse

            ' perform basic checks
            If cValue.Length < 1 Then Return result
            Dim dbNumericTemp As Double
            If Not Double.TryParse(cValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then Return result

            ' test by converting value to a currency
            curTemp = CDec(cValue)

            ' worked fine

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return result
        End Try
    End Function
End Module