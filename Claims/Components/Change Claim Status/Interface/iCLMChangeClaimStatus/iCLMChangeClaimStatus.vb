Option Strict Off
Option Explicit On
Imports System

'Developer Guide No.: 129
Imports SharedFiles

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
	Public Const ACApp As String = "iCLMChangeClaimStatus"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	' Form
	' Buttons
	
	' Messages
	Public Const ACInvalidStatusTitle As Integer = 300
	Public Const ACInvalidStatus As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	'AK 060603
	Public Const ACModeAuthorise As gPMConstants.PMEComponentAction = gPMConstants.PMEComponentAction.PMReverse
	
	'constants for setting 'is_referred'
	Public Const ACModeProcessed As Integer = 0
	Public Const ACModeReferred As Integer = 1
	
	Public Const ACModeRecommend As Integer = 1
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
    'AK 07052003 - for Claim Authority level
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lInsuranceFilecnt As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lInsuranceFoldercnt As Integer
	
	Public Const kClaimPaymentAccountDetailsClaimPaymentId As Integer = 0
	Public Const kClaimPaymentAccountDetailsTotalPaymentAmount As Integer = 1
	Public Const kClaimPaymentAccountDetailsAccountId As Integer = 2
	Public Const kClaimPaymentAccountDetailsCurrencyId As Integer = 3
	Public Const kClaimPaymentAccountDetailsMediaTypeID As Integer = 4
	Public Const kClaimPaymentAccountDetailsSourceId As Integer = 19
	
	
	
	Sub Main_Renamed()
		
	End Sub
End Module