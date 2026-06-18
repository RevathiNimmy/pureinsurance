Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 28/08/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTPeriodEnd"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
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
	
	' Menus
	
	
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
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    'Product Family Name for Help
    Public g_sUsername As String
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ScreenHelpID As Integer = 27000
	
	' Constants to define print operations
	' (Taken from bSIRPrintReport)
	'SD 23/07/2002 Duplicate since NavKeyConstants.bas added
	'Public Const AC_VIEW_ONLY = 0
	'Public Const AC_PRINT_ONLY = 1
	'Public Const AC_PRINT_AND_VIEW = 2
	
	' Report array index's
	Public Const ACReportID As Integer = 0
	Public Const ACReportCaptionID As Integer = 1
	Public Const ACReportCode As Integer = 2
	Public Const ACReportDescription As Integer = 3
	Public Const ACReportFilename As Integer = 4
	Public Const ACReportPeriodEnd As Integer = 5
	Public Const ACReportYearEnd As Integer = 6
	
	
	Sub Main_Renamed()
		
	End Sub
End Module