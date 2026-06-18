Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
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
	Public Const ACApp As String = "iPMURiskTypeRILimits"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	'JMK 22/10/2001 display Insurer/Reinsurer
	Public Const ACInterfaceTitleInsurer As Integer = 103
	Public Const ACTabTitle1Insurer As Integer = 104
	
	Public Const ACCRiskType As Integer = 111
	Public Const ACHBreakdown As Integer = 112
	Public Const ACHValue As Integer = 113
	Public Const ACCBreakdown As Integer = 114
	Public Const ACCValue As Integer = 115
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACEditButton As Integer = 204
	
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
	Public Const ACRRiskTypeId As Integer = 0
	Public Const ACRRILimitId As Integer = 1
	Public Const ACRGISPropertyId As Integer = 2
	Public Const ACRGISObjectName As Integer = 3
	Public Const ACRGISPropertyName As Integer = 4
	
	Public Const ACVRiskTypeId As Integer = 0
	Public Const ACVGISHeaderIndId1 As Integer = 1
	Public Const ACVGISHeaderIndId2 As Integer = 2
	Public Const ACVGISHeaderIndId3 As Integer = 3
	Public Const ACVValue As Integer = 4
	
	
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
	
	' KB 160801 required for help text, screenhelpid will need to be amended
	' when text available
	Public Const ScreenHelpID As Integer = 4001
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Sub Main_Renamed()
		
    End Sub
    
End Module