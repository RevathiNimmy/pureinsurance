Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  03/01/2001
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions to identify which application this is.
    Public Const ACApp As String = "bSIRRITax"

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"


    ' Constants for the search data array indexes.
    Public Const ACRParentCnt As Integer = 0
    Public Const ACRTaxBandId As Integer = 1
    Public Const ACRPremium As Integer = 2
    Public Const ACRTaxRate As Integer = 3
    Public Const ACRTaxValue As Integer = 4
    Public Const ACRIsValue As Integer = 5
    Public Const ACRIsManuallyChanged As Integer = 6
    Public Const ACRDescription As Integer = 7
    Public Const ACRIsNotAppliedToClient As Integer = 8
    Public Const ACRIsDeleted As Integer = 9
    Public Const ACRBasisValue As Integer = 10
    Public Const ACRCalcBasis As Integer = 11
    Public Const ACRSumInsured As Integer = 12
    Public Const ACRIsSIRounded As Integer = 13
    Public Const ACRCurrencyID As Integer = 14
    Public Const ACRCurrencyName As Integer = 15
    Public Const ACRAllowTaxCredit As Integer = 16
    Public Const ACROriginalSumInsured As Integer = 17
    Public Const ACRSumInsuredChange As Integer = 18
    Public Const ACRTaxGroupID As Integer = 19
    Public Const ACRTaxGroup As Integer = 20
    Public Const ACRSequence As Integer = 21
    Public Const ACRCountryID As Integer = 22
    Public Const ACRCountry As Integer = 23
    Public Const ACRStateID As Integer = 24
    Public Const ACRState As Integer = 25
    Public Const ACRClassOfBusinessID As Integer = 26
    Public Const ACRClassOfBusiness As Integer = 27
    Public Const ACRRunningTotal As Integer = 28
    Public Const ACRPrimaryKeyTaxCnt As Integer = 29
    Public Const ACRIsNotApplied As Integer = 31
    Public Const ACRIncludeIns As Integer = 32
    Public Const ACRSpread As Integer = 33
    Public Const ACRApplyTaxBy As Integer = 34 '(RC)

    ' Calculation basis constants
    Public Const ACCalcBasisPremium As Integer = 0
    Public Const ACCalcBasisSumInsured As Integer = 1
    Public Const ACCalcBasisSumInsuredChange As Integer = 2
    Public Const ACCalcBasisRunningTotal As Integer = 3


    ' System options
    Public Const ACApplyTaxesSystemOptionNumber As Integer = 1007
End Module