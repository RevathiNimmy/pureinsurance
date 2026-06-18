Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
Module MainModule
	''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery reinsurance
	' ***************************************************************** '
	' Module Name: MainModule
	
	' Description: Main module containing public variable/constants.
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMListReceipts"
	Public Const PMKeyNameNavigatorTitle1 As String = "List Payments"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	Public Const kACPaymentID As Integer = 0
	Public Const kACDate As Integer = 1
	Public Const kACResolvedName As Integer = 2
	Public Const kACPayee As Integer = 3
	Public Const kACAmount As Integer = 4
	Public Const kACCurrency As Integer = 5
	Public Const kACLossAmount As Integer = 6
	Public Const kACBaseAmount As Integer = 7
	Public Const kACPaymentCurrencyID As Integer = 8
	Public Const kACLossCurrencyID As Integer = 9
	Public Const kACBaseCurrencyID As Integer = 10
	Public Const kACTaxAmount As Integer = 11
	
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
    'developer guide no. (So that can be used everywhere)
    Public m_ofrmInterface As frmInterface
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	''End (Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery reinsurance
End Module