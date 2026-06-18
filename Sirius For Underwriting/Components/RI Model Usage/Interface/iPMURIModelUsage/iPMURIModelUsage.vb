Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
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
	Public Const ACApp As String = "iPMURIModelUsage"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	'JMK 23/10/2001
	Public Const ACInterfaceTitleInsurer As Integer = 103
	Public Const ACTabTitleInsurer As Integer = 104
	
	Public Const ACCProduct As Integer = 111
	Public Const ACCRiskType As Integer = 112
	Public Const ACHRIBand As Integer = 113
	Public Const ACHRIModel As Integer = 114
	Public Const ACHDescription As Integer = 115
	Public Const ACCRIBand As Integer = 116
	Public Const ACCRIModel As Integer = 117
	Public Const ACCDescription As Integer = 118
	Public Const ACHIsDeleted As Integer = 119
	Public Const ACCIsDeleted As Integer = 120
	Public Const ACHEffectiveDate As Integer = 121
	Public Const ACCEffectiveDate As Integer = 122
	Public Const ACHExpiryDate As Integer = 127
	Public Const ACCExpiryDate As Integer = 128
	Public Const ACCRIPortfolio As Integer = 129
	Public Const ACHRIPortfolio As Integer = 130
	
	'JMK 23/10/2001
	Public Const ACHBand As Integer = 123
	Public Const ACHInsurerModel As Integer = 124
	Public Const ACCBand As Integer = 125
	Public Const ACCInsurerModel As Integer = 126
	
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
	Public Const ACMandatoryField As Integer = 304
	Public Const ACCantDeleteModel As Integer = 305
	
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
	
	' Constants for the search data array indexes.
	Public Const ACRProductId As Integer = 0
	Public Const ACRRiskTypeId As Integer = 0
	Public Const ACRRIModelBand As Integer = 1
	Public Const ACRRIModelId As Integer = 2
	Public Const ACRDescription As Integer = 3
	Public Const ACRRIModelDescription As Integer = 4
	Public Const ACRIsDeleted As Integer = 5
	Public Const ACREffectiveDate As Integer = 6
	Public Const ACRExpiryDate As Integer = 7
	Public Const ACRTransferFromModelID As Integer = 8
	Public Const ACRRiskTypeRIModelUsageCnt As Integer = 9
	Public Const ACRItemStatus As Integer = 10
	Public Const ACRRIBandDescription As Integer = 11
	Public Const ACRMax As Integer = 11
	
	Public Const ACItemStatus_Unchanged As Integer = 0
	Public Const ACItemStatus_Changed As Integer = 1
	Public Const ACItemStatus_Added As Integer = 2
	Public Const ACItemStatus_Deleted As Integer = 3
	
	Public Enum enumRIModelData
		edRIModelID = 0
		edCode = 1
		edDesc = 2
		edIsDeleted = 3
		edEffectiveDate = 4
		edIsDeferred = 5
	End Enum
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oGIS As Object
	
	' KB 020801 Required for help text link
	
	Public Const ScreenHelpID As Integer = 4093
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	
	Sub Main_Renamed()
		
    End Sub
    
End Module