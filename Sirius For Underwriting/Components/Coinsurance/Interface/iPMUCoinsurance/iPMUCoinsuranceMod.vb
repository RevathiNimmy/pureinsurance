Option Strict Off
Option Explicit On
Imports System
'developer guide no 129. 
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
	Public Const ACApp As String = "iPMUCoinsurance"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	
	Public Const ACCInsuranceFile As Integer = 111
	Public Const ACCIsRecoverd As Integer = 112
	Public Const ACCIsSurcharged As Integer = 113
	Public Const ACCAllocatedPercent As Integer = 114
	Public Const ACCDefault As Integer = 115
	Public Const ACHCoinsurer As Integer = 116
	Public Const ACHArrangementRef As Integer = 117
	Public Const ACHSharePercent As Integer = 118
	Public Const ACHCommissionPercent As Integer = 119
	Public Const ACLCoinsurer As Integer = 120
	Public Const ACLArrangementRef As Integer = 121
	Public Const ACLSharePercent As Integer = 122
	Public Const ACLCommissionPercent As Integer = 123
	
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
	
	' Constants for the search data array indexes.
	Public Const ACAInsuranceFileCnt As Integer = 0
	Public Const ACAIsRecovered As Integer = 1
	Public Const ACAIsSurcharged As Integer = 2
	Public Const ACACOIDefaultId As Integer = 3
	
	Public Const ACVInsuranceFileCnt As Integer = 0
	Public Const ACVCOIValueId As Integer = 1
	Public Const ACVPartyCnt As Integer = 2
	Public Const ACVArrangementRef As Integer = 3
	Public Const ACVSharePercent As Integer = 4
	Public Const ACVSharePremium As Integer = 5
	Public Const ACVCommissionPercent As Integer = 6
	Public Const ACVCommissionValue As Integer = 7
	Public Const ACVSurchargePercent As Integer = 8
	Public Const ACVSurchargeValue As Integer = 9
	Public Const ACVIsStandardSurcharge As Integer = 10
	Public Const ACVPremiumTaxRecoveryPercent As Integer = 11
	Public Const ACVPremiumTaxRecoveryValue As Integer = 12
	Public Const ACVIsManualPremiumTax As Integer = 13
	Public Const ACVPartyName As Integer = 14
	
	Public Const ACDCOIDefaultId As Integer = 0
	Public Const ACDCode As Integer = 1
	Public Const ACDCaption As Integer = 2
	Public Const ACDDescription As Integer = 3
	Public Const ACDSourceId As Integer = 4
	Public Const ACDIsRecovered As Integer = 5
	Public Const ACDIsRecoveredOverrideable As Integer = 6
	Public Const ACDIsSurcharged As Integer = 7
	Public Const ACDIsSurchargedOverrideable As Integer = 8
	Public Const ACDStandardSurchargePercent As Integer = 9
	Public Const ACDCompulsoryCOIPartyCnt As Integer = 10
	Public Const ACDCompulsoryCOIComPercent As Integer = 11
	Public Const ACDEffectiveDate As Integer = 12
	Public Const ACDIsDeleted As Integer = 13
	
	
	
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
	
	' KB 100801 required for help text, screenhelpid will need to be amended
	' when text available
	Public Const ScreenHelpID As Integer = 4001
	'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Sub Main_Renamed()
		
	End Sub
End Module