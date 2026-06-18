Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
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
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMUScreenControl"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACColumnHeader1 As Integer = 102
	Public Const ACColumnHeader2 As Integer = 103
	Public Const ACColumnHeader3 As Integer = 104
	
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
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'Array constants for the Risk
	Public Const ACRRiskId As Integer = 0
	Public Const ACRRiskStatusId As Integer = 1
	Public Const ACRRiskFolderCnt As Integer = 2
	Public Const ACRAccumulationId As Integer = 3
	Public Const ACRRiskTypeId As Integer = 4
	Public Const ACRDescription As Integer = 5
	Public Const ACRSequenceNumber As Integer = 6
	Public Const ACRSumInsuredRequested As Integer = 7
	Public Const ACRInceptionDate As Integer = 8
	Public Const ACRExpiryDate As Integer = 0
	Public Const ACRIsNotIndexLinked As Integer = 10
	Public Const ACRIsAccumulated As Integer = 11
	Public Const ACRLapsedReasonId As Integer = 12
	Public Const ACRLapsedDate As Integer = 13
	Public Const ACRLapsedDescription As Integer = 14
	Public Const ACRVarDataRef As Integer = 15
	Public Const ACRTotalSumInsured As Integer = 16
	Public Const ACRTotalAnnualPremium As Integer = 17
	Public Const ACRTotalThisPremium As Integer = 18
	Public Const ACRIsRiAtRiskLevel As Integer = 19
	Public Const ACRIsAutoReinsured As Integer = 20
	Public Const ACRGISScreenId As Integer = 21
	Public Const ACREMLPercentage As Integer = 22
	' PW181102
	Public Const ACRRiskNumber As Integer = 23
	Public Const ACRVariationNumber As Integer = 24
	Public Const ACRIsRiskSelected As Integer = 25
	Public Const ACRCoverage As Integer = 26
	Public Const ACRInsuredItem As Integer = 27
	Public Const ACRExtensions As Integer = 28
	' PW150503 - add 'package' field
	Public Const ACRPackage As Integer = 29
	
	'AC 18/09/2003 CQ17 Added NCD and Excess fields
	Public Const ACRNCD As Integer = 30
	Public Const ACRExcess As Integer = 31
	
	'Array constants for the Data Dictionary
	Public Const ACOGISObjectId As Integer = 0
	Public Const ACOGISDataModelId As Integer = 1
	Public Const ACOObjectName As Integer = 2
	Public Const ACOTableName As Integer = 3
	Public Const ACOMaxInstances As Integer = 4
	Public Const ACOIsQuoteObject As Integer = 5
	Public Const ACOParentObjectId As Integer = 6
	Public Const ACOPolarisObjectId As Integer = 7
	Public Const ACOIsSelectable As Integer = 8
	Public Const ACOIsNonGIS As Integer = 9
	Public Const ACPGISPropertyId As Integer = 10
	Public Const ACPGISObjectId As Integer = 11
	Public Const ACPPropertyName As Integer = 12
	Public Const ACPColumnName As Integer = 13
	Public Const ACPDataType As Integer = 14
	Public Const ACPIsInputProperty As Integer = 15
	Public Const ACPIsIdentifyingProperty As Integer = 16
	Public Const ACPIsPrimaryKey As Integer = 17
	'GSD
	'Public Const ACPGISListId = 18
	Public Const ACPPolarisPropertyId As Integer = 18
	Public Const ACPIsDeleted As Integer = 19
	Public Const ACPIsSearchProperty As Integer = 20
	'Public Const ACPLookupTableName = 22
	'Public Const ACPPartyTypeId = 23
	'Public Const ACPSumInsuredType = 24
	'Public Const ACPStdWordingType = 25
	'Public Const ACPGISUserDefHeaderId = 26
	'Public Const ACPProductId = 27
	Public Const ACPEditFlags As Integer = 21
	Public Const ACPSpecialsType As Integer = 22
	Public Const ACPSpecialsTypeReference As Integer = 23
	
	Public Const ACO2GISObjectId As Integer = 24
	Public Const ACUParent As Integer = 25
	
	' CTAF 20020809 - Merged in from CNIC - start
	Public Const ACArrayRiskGroupID As Integer = 0
	Public Const ACArrayCaptionID As Integer = 1
	Public Const ACArrayCode As Integer = 2
	Public Const ACArrayDescription As Integer = 3
	Public Const ACArrayIsDeleted As Integer = 4
	Public Const ACArrayEffectiveDate As Integer = 5
	Public Const ACArrayRiskScreenType As Integer = 6
	Public Const ACArrayABICode As Integer = 7
	Public Const ACArrayPQRiskScreenType As Integer = 8
	Public Const ACArrayMax As Integer = 8
	' CTAF 20020809 - Merged in from CNIC - end
	
	' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sUsername As String = "" 'DJM 03/04/2002

    ' Public instance of the object manager.

    'Public g_oObjectManager As bObjectManager.ObjectManager

    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oGIS As Object
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	
	
	Sub Main_Renamed()
		
	End Sub
End Module
