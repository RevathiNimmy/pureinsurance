Option Strict Off
Option Explicit On
Imports System

'Developer Guide No.: 129
Imports SharedFiles

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
    'developer guide no.50
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_ofrmInterface As frmInterface
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMInfoChklst"
	
	
    'Store RecoveryType Ids
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vClaim_Id() As Object
	
	' Public interface constants used when
	' retrieving data from the resource file.
    'developer guide no. 107
    <ThreadStatic()> _
 Public vTableArray() As Object
	'array holding the return value from database
	'used to populate the listview,
    'could be later used to populate Addrequirement screen
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vExpServ As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtDateReqt As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtTimeReqt As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtDateCrit As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtTimeCrit As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtDateRecv As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtTimeRecv As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bAddExpSer As Boolean
	
	Public Const ACDateConversion As String = "short date"
	Public Const ACTimeConversion As String = "HH:MM"
	
	'Error Messages
	Public Const ACMandatoryFieldMsg As Integer = 304
	Public Const ACInvalidDateMsg As Integer = 315
	Public Const ACCannotBeGreaterThanLossDateMsg As Integer = 316
	Public Const ACInvalidTimeMsg As Integer = 317
	Public Const ACCannotBeGreaterThanTodaysDateMsg As Integer = 318
	Public Const ACInvaildTimeMsg As Integer = 319
	Public Const ACCannotBeGreaterThanReportedDateMsg As Integer = 320
	Public Const ACCannotBeGreaterThanTodaysDateMsg2 As Integer = 321
	
	' General Icons
	
	' Form
	'AJM (25/07/2001) - Add 'Edit' captions for edit mode
	
	Public Const ACEditServiceTitle As Integer = 98 'Edit service
	Public Const ACEditRequirementTitle As Integer = 99 'Edit requirement
	Public Const ACMainInterfaceTitle As Integer = 100 'Information Checklist By SV
	Public Const ACAddServiceTitle As Integer = 101 'Add Service By SV
	Public Const ACAddRequirementTitle As Integer = 102 'Add Requirement By SV
	Public Const ACMenuFile As Integer = 103 'File  By SV
	Public Const ACMenuNavigator As Integer = 104 'Return To Navigator By SV
	
	Public Const ACTabTitle As Integer = 105 '&1 - General By SV
	
	Public Const ACClaimNumber As Integer = 106 'Claim Number By SV
	Public Const ACColHeader1 As Integer = 322 'Service/Requirement
	Public Const ACColHeader2 As Integer = 107 'Description By SV
	Public Const ACColHeader3 As Integer = 108 'Type By SV
	Public Const ACColHeader4 As Integer = 109 'Reference By SV
	Public Const ACColHeader5 As Integer = 110 'Date Critical By SV
	Public Const ACColHeader6 As Integer = 123 'Date Received
	
	Public Const ACServicelbl As Integer = 114 'Service: By SV
	
	Public Const ACDateRequested As Integer = 116 'Date Requested: By SV
	Public Const ACDateReceived As Integer = 117 'Date Received: By SV
	Public Const ACDateCritical As Integer = 118 'Date Critical : By SV
	
	Public Const ACContactlbl As Integer = 119 'Contact : By SV
	Public Const ACDescription As Integer = 120 'Description : By SV
	Public Const ACRequirement As Integer = 121 'Requirement : By SV
	Public Const ACReference As Integer = 122 'Reference : By SV
	Public Const ACMandatoryMain As Integer = 313 'Mandatory By SV
	Public Const ACMandatoryMainTitle As Integer = 314 'Mandatory Title By SV
	
	' Buttons
	Public Const ACOKButton As Integer = 202 ' OK Button By SV
	Public Const ACCancelButton As Integer = 201 'Cancel Button By SV
	Public Const ACHelpButton As Integer = 200 'Help Button By SV
	Public Const ACPartyButton As Integer = 115 'Party... By SV
	
	Public Const ACAddRequirementButton As Integer = 111 'Command Button:Add&Requirement By SV
	Public Const ACAddServiceButton As Integer = 112 'Command Button:Add&Service By SV
	Public Const ACEditButton As Integer = 113 'Command Button:&Edit By SV
	
	'Public Const ACAddButton = 204
	'Public Const ACEditButton = 205
	'Public Const ACDeleteButton = 206
	
	'Public Const ACRecoveryType = 207
	
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	'Public Const ACMandatoryFieldMsg = 304      'being used IC
	'Public Const ACInvalidNumberMsg = 305
	'Public Const ACInvalidCurrencyMsg = 306
	'Public Const ACInvalidCurrencyDecimalPointsMsg = 307
	'Public Const ACInvalidExccoveryType = 310
	'Public Const ACInvalidRevisedReserve = 311
	'Public Const ACInvalidShangeRateMsg = 308
	'Public Const ACInvalidPositiveNumbers = 309
	'Public Const ACInvalidRealvageAmount = 312
	
	
	'Lookup
	'Public Const ACLookupFailTitle = 308
	'Public Const ACLookupFail = 309
	
	' Menus
	
	' Constants for the search data array indexes for perils.
	Public Const ACServType As Integer = 0
	Public Const ACDesc As Integer = 1
	Public Const ACCESId As Integer = 2
	Public Const ACESId As Integer = 3
	Public Const ACPrtyClmId As Integer = 4
	Public Const ACService As Integer = 5
	Public Const ACRef As Integer = 6
	Public Const ACContact As Integer = 7
	Public Const ACDateReq As Integer = 8
	Public Const ACDateCrit As Integer = 9
	Public Const ACDateRecv As Integer = 10
	
	'JMK 21/05/2001 - Service_Type: Requirement = 1, Service = 2
	Public Const ACTypeServ As Integer = 2
	Public Const ACTypeReqmnt As Integer = 1
	
	
	Public Const ACDescId As Integer = 0
	Public Const ACIPerilTypeId As Integer = 1
	
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
	Public Const ACIClaim_Id As Integer = 0
	
	
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
	
    'DC281100
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_nPMMode As Integer

	Public g_nREADMODE As Integer = gPMConstants.PMEComponentAction.PMView '0

	Public g_nADDMODE As Integer = gPMConstants.PMEComponentAction.PMAdd '1

	Public g_nEDITMODE As Integer = gPMConstants.PMEComponentAction.PMEdit '2
	'DC281100
	
	'RWH(06/03/2001) Doc Generation modes.
	Public Const ACPrintMode As Integer = 2
	Public Const ACPrintSilentMode As Integer = 3
	Public Const ACSpoolSilentMode As Integer = 4
	
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	'Instance of Bussiness Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bCLMInfoChklst.Business
	
    ' Stores the search data from the lookup business object for currency,coinsurancetreatment.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vLookupArray As Object
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    'RWH(15/06/01)
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bPrevInfoOnlyStatus As Boolean
	
	Sub Main_Renamed()
		
	End Sub
End Module