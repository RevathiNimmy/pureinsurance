Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 10/05/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iGISMaintainDataDictionary"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACDataModel As Integer = 102
	Public Const ACObjectsTables As Integer = 103
	Public Const ACPropertiesColumns As Integer = 104
	Public Const ACObject As Integer = 105
	Public Const ACTableName As Integer = 106
	Public Const ACProperty As Integer = 107
	Public Const ACColumnName As Integer = 108
	Public Const ACStatus As Integer = 109
	Public Const ACShowKeys As Integer = 110
	Public Const ACSPropertyType As Integer = 111
	Public Const ACSPropertySpecial As Integer = 112
	
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACApplyButton As Integer = 206
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	'Listview object tags
	Public Const ACTagGisModelType As Integer = 0
	Public Const ACTagGisObjectId As Integer = 1
	
	' Menus
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	Public Const ACOpenFolder As String = "Open"
	Public Const ACClosedFolder As String = "Closed"
	
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUserName As String = ""
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	Sub Main_Renamed()
		
	End Sub
End Module