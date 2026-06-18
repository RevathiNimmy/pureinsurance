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
	Public Const ACApp As String = "iPMUListScreens"
	
	
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
	Public Const ACDeleteButton As Integer = 204
	Public Const ACUndeleteButton As Integer = 205
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACInvalidDMTitle As Integer = 304
	Public Const ACInvalidDM As Integer = 305
	
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
	Public Const ACSGISScreenId As Integer = 0
	Public Const ACSGISDataModelId As Integer = 1
	Public Const ACSCaptionId As Integer = 2
	Public Const ACSCode As Integer = 3
	Public Const ACSDescription As Integer = 4
	Public Const ACSIsDeleted As Integer = 5
	Public Const ACSEffectiveDate As Integer = 6
	Public Const ACSParentId As Integer = 7
	Public Const ACSIsMaintainable As Integer = 8
	Public Const ACSDMCode As Integer = 9
	Public Const ACSDMDescription As Integer = 10
	
	Public Const ACDateColumn As Integer = 2
	
	' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oGIS As Object
	
	
	
	
	Sub Main_Renamed()
		
    End Sub

    

End Module