Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 20/09/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMPaymentProcess"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	'' Messages
	'Public Const ACInvalidStatusTitle = 300
	'Public Const ACInvalidStatus = 301
	'Public Const ACBusinessFailTitle = 302
	'Public Const ACBusinessFail = 303
	
	
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
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no.107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	Public Const kClaimPaymentAccountDetailsClaimPaymentId As Integer = 0
	Public Const kClaimPaymentAccountDetailsTotalPaymentAmount As Integer = 1
	Public Const kClaimPaymentAccountDetailsAccountId As Integer = 2
	Public Const kClaimPaymentAccountDetailsCurrencyId As Integer = 3
	Public Const kClaimPaymentAccountDetailsMediaTypeID As Integer = 4
	Public Const kClaimPaymentAccountDetailsSourceId As Integer = 19
	
	Sub Main_Renamed()
		
	End Sub
End Module