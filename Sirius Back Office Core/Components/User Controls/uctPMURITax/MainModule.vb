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
	Public Const ACApp As String = "uctPMURITaxControl"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public contants used for the start and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	Public Const ACReserveFrame As Integer = 125
	Public Const ACEditButton As Integer = 205
	
	'Constants used for resizing & positioning the user control
	Public Const ACListViewTop As Integer = 240
	Public Const ACCtrlVerticalSpacing As Integer = 120
	Public Const ACCommandButtonWidth As Integer = 1095
	Public Const ACCommandButtonHeight As Integer = 330
	
	' list view column constants
	Public Const kLRITaxColHeaderTaxGroup As Integer = 1
	Public Const kRITaxColHeaderSequence As Integer = 2
	Public Const kRITaxColHeaderTaxBand As Integer = 3
	Public Const kRITaxColHeaderTaxAmount As Integer = 4
	Public Const kRITaxColHeaderCalculationBasis As Integer = 5
	Public Const kRITaxColHeaderRate As Integer = 6
	Public Const kRITaxColHeaderClassOfBusiness As Integer = 7
	Public Const kRITaxColHeaderCountry As Integer = 8
	Public Const kRITaxColHeaderState As Integer = 9
	Public Const kRITaxColHeaderIsNotInclude As Integer = 10
	Public Const kRITaxColHeaderIncludeIns As Integer = 11
	Public Const kRITaxColHeaderSpread As Integer = 12
	Public Const kRITaxColHeaderApplyTaxBy As Integer = 13 '(RC)
	
	
	
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
	Public Const ACRIsAppliedToClnt As Integer = 31
	Public Const ACRIncludeIns As Integer = 32
	Public Const ACRSpread As Integer = 33
	Public Const ACRApplyTaxBy As Integer = 34 '(RC)
	
	' Calculation basis constants
	Public Const ACCalcBasisPremium As Integer = 0
	Public Const ACCalcBasisSumInsured As Integer = 1
	Public Const ACCalcBasisSumInsuredChange As Integer = 2
	Public Const ACCalcBasisRunningTotal As Integer = 3
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