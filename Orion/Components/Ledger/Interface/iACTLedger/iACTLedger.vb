Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 11th July 1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTLedger"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	Public Const ACListTitle1 As Integer = 102
	Public Const ACListTitle2 As Integer = 103
	
	' Details Form
	Public Const ACDetailsTabTitle1 As Integer = 104
	
	Public Const ACNameCaption As Integer = 105
	Public Const ACTypeCaption As Integer = 106
	Public Const ACShortNameCaption As Integer = 107
	Public Const ACMappingCaption As Integer = 108
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACRemoveButton As Integer = 205
	Public Const ACEditButton As Integer = 206
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACShortNameErrorTitle As Integer = 304
	Public Const ACShortNameError As Integer = 305
	
	' Menus
	
	
	' Constants for the List data array subscripts.
	'BB
	
	Public Const ACSubLedgerID As Integer = 0
	Public Const ACSubCompanyID As Integer = 1
	Public Const ACSubLedgerName As Integer = 2
	Public Const ACSubLedgerShortName As Integer = 3
	Public Const ACSubMappingID As Integer = 4
	Public Const ACSubLedgerTypeID As Integer = 5
	Public Const ACSubLedgerType As Integer = 6
	Public Const ACSubIsDeletable As Integer = 7
	
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
	
	Public Const ScreenHelpID As Integer = 34000
	

	Public Sub Main()
		
	End Sub
    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
	End Sub
End Module