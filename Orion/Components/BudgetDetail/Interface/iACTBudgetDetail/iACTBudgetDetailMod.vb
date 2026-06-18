Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles

Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 27/10/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTBudgetDetail"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACBudgetRef As Integer = 102
	Public Const ACYear As Integer = 103
	Public Const ACApportion As Integer = 104
	Public Const ACColAccount As Integer = 105
	Public Const ACColAnnual As Integer = 106
	
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
	
	' Grid Constants
	Public Const ACGridBudgetID As Integer = 0
	Public Const ACGridBudgetSequence As Integer = 1
	Public Const ACGridPeriodID As Integer = 2
	Public Const ACGridAccountID As Integer = 3
	Public Const ACGridBudgetAmount As Integer = 4
	Public Const ACGridBudgetDetailID As Integer = 5
	Public Const ACGridRows As Integer = ACGridBudgetDetailID + 1
	
	' Array constants
	Public Const ACAccountID As Integer = 1
	Public Const ACBudgetDetailID As Integer = 2
	
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
	
	Public g_iCurrencyID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	'Screen Help Context ID
	Public Const ScreenHelpID As Integer = 25000
	
	Sub Main_Renamed()
		
	End Sub
End Module