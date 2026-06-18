Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  04/06/1998
	'
	' Description: Main Module.
	'
	' Edit History: TF040698 - Created
	' ***************************************************************** '
	
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "PMAddressControl"
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public interface constants used when retrieving data from the resource file.
	
	Public Const ACCaptionAddress1 As Integer = 101
	Public Const ACCaptionAddress2 As Integer = 102
	Public Const ACCaptionAddress3 As Integer = 103
	Public Const ACCaptionAddress4 As Integer = 104
	Public Const ACCaptionPostCode As Integer = 105
	Public Const ACCaptionCountry As Integer = 106
	
	Public Const ACUSCountryCode As String = "USA"
	Public Const ACUKCountryCode As String = "GBR"
	
	
	' Constants for country configuration array
	Public Const ACCCountryID As Integer = 0
	Public Const ACCISOCode As Integer = 1
	Public Const ACCAddressLine1Caption As Integer = 2
	Public Const ACCAddressLine2Caption As Integer = 3
	Public Const ACCAddressLine3Caption As Integer = 4
	Public Const ACCAddressLine4Caption As Integer = 5
	Public Const ACCIsStateLookup As Integer = 6
	Public Const ACCPostcodeCaption As Integer = 7
	Public Const ACCPostcodeVisibility As Integer = 8
	
	
	' Public source and language ID's from the Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iCountryId As Integer
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	Sub Main_Renamed()
		
	End Sub
End Module