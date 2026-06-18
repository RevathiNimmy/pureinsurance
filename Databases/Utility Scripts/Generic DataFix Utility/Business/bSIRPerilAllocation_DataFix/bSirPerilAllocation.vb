Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  27/10/1997
	'
	' Description: Main Module.
	'
	' Edit History: TF27101997 - Created
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSirPerilAllocation"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	Public Const ACOPolicyBinderId As Integer = 0
	Public Const ACOOutputId As Integer = 1
	Public Const ACODeclineReason As Integer = 2
	Public Const ACOReferReason As Integer = 3
	Public Const ACOMessage As Integer = 4
	Public Const ACOPolicyRatingSectionType As Integer = 5
	Public Const ACORiskRatingSectionType As Integer = 6
	Public Const ACOSumInsured As Integer = 7
	Public Const ACOPremium As Integer = 8
	Public Const ACORate As Integer = 9
	Public Const ACOOriginalPremium As Integer = 10
	Public Const ACOOriginalFlag As Integer = 11
	Public Const ACORateTypeId As Integer = 12
	Public Const ACOCountryId As Integer = 13
	Public Const ACOStateId As Integer = 14
	Public Const ACOAutoCalculated As Integer = 15
    Public Const ACOEarningPatternID As Integer = 16
    ' WPR53
    Public Const ACODisableOriginalProRata As Integer = 17
    Public Const ACODisableNewProRata As Integer = 18
	' PW191102 - add constants for rating section type tax rate array
	Public Const ACRTaxBandID As Integer = 0
	Public Const ACRTaxRate As Integer = 1
	Public Const ACRTaxPercentShare As Integer = 2
	
	Sub Main_Renamed()
		
	End Sub
End Module