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
	Public Const ACApp As String = "bSIRRIBandVersion"

	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"


	' Constants for ri band rate array contents
	Public Const ACRRIBandId As Integer = 0
	Public Const ACRCode As Integer = 1
	Public Const ACRCaptionId As Integer = 2
	Public Const ACRDescription As Integer = 3
	Public Const ACREffectiveDate As Integer = 4
	Public Const ACRDateForTreaty As Integer = 5
	Public Const ACRXOLTreaty As Integer = 6
	Public Const ACRProportionalRICal As Integer = 7
	Public Const ACRUseAnniversaryDate As Integer = 8

	Public Const ACRDateForTreatyID As Integer = 9
	Public Const ACRXOLID As Integer = 10
	Public Const ACRPRICALID As Integer = 11

	' 020506 Datasure
	Public Const ACRMaxArray As Integer = 11
End Module