'Add GetResData in the project
Option Strict Off
Option Explicit On
Imports System
'Developer Guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 31/01/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBPartyCM"
	
	
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
	Public Const ACNewButton As Integer = 204
	Public Const ACDeleteButton As Integer = 205
	Public Const ACUnDeleteButton As Integer = 206
	Public Const ACUpdateButton As Integer = 207
	Public Const ACAddButton As Integer = 208
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACSaveChanges As Integer = 304
	
	' Menus
	
	' Dirty status
	Public Const ACFlagSame As Integer = 0
	Public Const ACFlagChanged As Integer = 1
	Public Const ACFlagNew As Integer = 2
	
	' Array positions
	Public Const ACArrayPartyCnt As Integer = 0
	Public Const ACArrayShortName As Integer = 1
	Public Const ACArrayName As Integer = 2
	Public Const ACArrayPartyTypeID As Integer = 3
	Public Const ACArrayIsDeleted As Integer = 4
	Public Const ACArrayPartyFlag As Integer = 5
	'eck300101
	Public Const ACArrayPartySourceID As Integer = 6
	'pkh 01/10/2002
	Public Const ACArrayBranchName As Integer = 7
	
	
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'Modified,added HelpContextID as per vbcode
    Public Const HelpContextID As Integer = 210
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	Sub Main_Renamed()
		
    End Sub

End Module