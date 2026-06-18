Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 09/06/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iPMURITax"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public interface constants used when retrieving data from the resource file.
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
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
	
	' Calculation basis constants
	Public Const ACCalcBasisPremium As Integer = 0
	Public Const ACCalcBasisSumInsured As Integer = 1
	Public Const ACCalcBasisSumInsuredChange As Integer = 2
	Public Const ACCalcBasisRunningTotal As Integer = 3
	
    ' Public source and language ID's from the Object Manager.
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
	
    ' Determine if these are risk level or policy level taxes
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bRiskFlag As Boolean
End Module