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
	Public Const ACApp As String = "iPMUPrimCauseRiskType"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	Public Const ACCPrimaryCause As Integer = 111
	Public Const ACHCode As Integer = 113
	Public Const ACHDescription As Integer = 114
	Public Const ACHEffectiveDate As Integer = 115
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACApplyButton As Integer = 203
	Public Const ACSelectButton As Integer = 204
	Public Const ACUnselectButton As Integer = 205
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
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
	Public Const ACTPPrimaryCauseId As Integer = 0
	Public Const ACTPRiskTypeGroupId As Integer = 1
	
	
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
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	
	Sub Main_Renamed()
		
	End Sub
End Module