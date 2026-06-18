Option Strict Off
Option Explicit On
Imports System
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
	Public Const ACApp As String = "bSIRTaxBandRate"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' Constants for tax band rate array contents
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
    Public Const kACRIsRIPaymentsRecoveries As Integer = 45
	' 020506 Datasure
    Public Const ACRMaxArray As Integer = 45
End Module