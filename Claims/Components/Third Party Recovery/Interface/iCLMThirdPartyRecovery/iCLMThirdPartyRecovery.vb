Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 11/08/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMThirdPartyRecovery"
	
	
    'Store RecoveryType Ids
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vRecoveryTypeID(,) As Object
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lButton As Integer
	
	' Public interface constants used when
	' retrieving data from the resource file.
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vLookupValues As Object
	' General Icons
	
	'RWH(06/04/2001) Transaction Type constants.
	Public Const ACTransTypeClaimOpen As String = "C_CO" 'Open claim
	Public Const ACTransTypeClaimPay As String = "C_CP" 'Claim payment
	Public Const ACTransTypeClaimRevise As String = "C_CR" 'Claim revision (maintain claim)
	Public Const ACTransTypeClaimSalvage As String = "C_SA" 'Claim salvage
	Public Const ACTransTypeClaimRecovery As String = "C_RV" 'Claim recovery
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100 'Third Party Recovery
	Public Const ACThirdPartyReceipt As Integer = 101 'Third Party Receipt
	Public Const ACThirdPartyDetails As Integer = 118 'Third Party Details
	Public Const ACSelectPeril As Integer = 122 'Select Peril
	
	Public Const ACTabTitle1 As Integer = 111 '&1-Recovery Amounts
	Public Const ACTabTitle2 As Integer = 112 '&2-CoInsurance Amounts
	Public Const ACTabTitle3 As Integer = 113 '&3-ReInsurance Amounts
	Public Const ACTabTitleGeneral As Integer = 119 '&1-General
	
	Public Const ACClaimNumber As Integer = 102 'Claim Number
	Public Const ACPerilType As Integer = 103 'Peril Type
	Public Const ACCurrency As Integer = 104 'Currency
	Public Const ACExchangeRate As Integer = 105 'Exchange Rate
	Public Const ACThirdPartyType As Integer = 106
	Public Const ACInitialReserve As Integer = 107
	Public Const ACRevisedReserve As Integer = 108
	Public Const ACNewReserve As Integer = 109
	Public Const ACThirdPartyAmount As Integer = 110
	Public Const ACShare As Integer = 114
	Public Const ACCoInsuranceTreatment As Integer = 115
	Public Const ACCoInsurer As Integer = 116
	Public Const ACReInsurer As Integer = 117
	Public Const ACCurrentReserve As Integer = 129
	
	'TN20010906
	Public Const ACReceivedToDate As Integer = 124
	
	Public Const ACNewReserve_U As Integer = 109
	Public Const ACInitialReserve_U As Integer = 107
	Public Const ACCurrentReserve_U As Integer = 123
	Public Const ACRevisedReserve_U As Integer = 108
	
	Public Const ACPeril As Integer = 120
	Public Const ACDescription As Integer = 121
	
	'JMK 14/11/2001
	Public Const ACTabTitle3Insurer As Integer = 125
	Public Const ACInsurer As Integer = 126
	
	' Buttons
	Public Const ACOKButton As Integer = 202
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 200
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	Public Const ACSelectButton As Integer = 211
	
	Public Const ACRecoveryType As Integer = 207
	
	'Constants For Third Party Details Screen
	Public Const ACThirdPartyInitialReserve As Integer = 212
	Public Const ACThirdPartyRevisedReserve As Integer = 209
	Public Const ACThirdPartyThirdPartyAmount As Integer = 210
	
	'DC050802
	Public Const ACThirdPartyInitialReserve_U As Integer = 208
	
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACMandatoryFieldMsg As Integer = 304
	Public Const ACInvalidNumberMsg As Integer = 305
	Public Const ACInvalidCurrencyMsg As Integer = 306
	Public Const ACInvalidCurrencyDecimalPointsMsg As Integer = 307
	Public Const ACInvalidExchangeRateMsg As Integer = 308
	Public Const ACInvalidPositiveNumbers As Integer = 309
	Public Const ACInvalidRecoveryType As Integer = 310
	Public Const ACInvalidRevisedReserve As Integer = 311
	Public Const ACInvalidThirdPartyAmount As Integer = 312
	Public Const ACRecoverTypeExists As Integer = 313
	Public Const ACInvalidDeleteOperation As Integer = 314
	Public Const ACInvalidRevisedReserveRecdNull As Integer = 315
	Public Const ACInvalidRRRecdToDate As Integer = 316
	Public Const ACInvalidRR As Integer = 317
	'RWH(08/10/01) Constants for Close Claim messages.
	Public Const ACFailedToGetCurrentReserve As Integer = 318
	Public Const ACFailedToCloseClaim As Integer = 319
	Public Const ACInvalidAction As Integer = 320
	Public Const ACGenralDetailsFrame As Integer = 321
	Public Const ACCloseClaimTitle As Integer = 322
	Public Const ACCloseClaimDetail As Integer = 323
	
	' Alix - 21/05/2003 - VAT on claim payments
	Public Const ACTaxType As Integer = 328
	Public Const ACTaxBand As Integer = 329
	Public Const ACTaxAmount As Integer = 330
	Public Const ACNetAmount As Integer = 331
	
	Public Const ACThirdPartyRevisedReserveLoss As Integer = 332
	Public Const ACLossCurrency As Integer = 333
	Public Const ACReceiptCurrency As Integer = 334
	Public Const ACReceiptToLossRate As Integer = 335
	Public Const ACRevisedReserveLoss As Integer = 336
	
	'DC050801 added as underwriting to stay as was, but modified for broking
	Public Const ACFailedToGetCurrentReserve_U As Integer = 318
	
	'Lookup
	'Public Const ACLookupFailTitle = 308
	'Public Const ACLookupFail = 309
	
	' Menus
	
	' Constants for the search data array indexes for perils.
	Public Const ACIPerilId As Integer = 0
	Public Const ACIPerilTypeId As Integer = 1
	Public Const ACIPeril As Integer = 2
	Public Const ACIDescription As Integer = 3
	
	' Constants for the search data array indexes for Co_Insurers.
	Public Const ACICoInsurerId As Integer = 0
	Public Const ACICoInsurerName As Integer = 1
	Public Const ACICoShare As Integer = 2
	
	' Constants for the search data array indexes for Re_Insurers.
	Public Const ACIReInsurerId As Integer = 0
	Public Const ACIReInsurerName As Integer = 1
	Public Const ACIReShare As Integer = 2
	
	
	' Constants for the search data array indexes for RecoveryIds.
	Public Const ACIRecoveryID As Integer = 0
	Public Const ACIRecoveryTypeID As Integer = 0
	
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'Constants To Identify Table
	Public Const ACRecovery As Integer = 0
	Public Const ACReceipt As Integer = 1
	Public Const ACPayment As Integer = 2
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    'Instance of Bussiness Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	
	'Instance of Bussiness Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness1 As bCLMThirdParty.Business
	
	
    'Get All Recovery Types for Third Party
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vThirdPartyRecoveryTypes As Object
	
    ' Stores the search data from the lookup business object for currency,coinsurancetreatment.
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vLookupDetails As Object
	
	'Count For No of Objects in Collection
	'Public g_lCount As Long
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Sub Main_Renamed()
		
	End Sub
End Module