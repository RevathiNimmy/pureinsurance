Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  01/02/2001
	'
	' Created By: Ajit Kumar
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	'   26/06/2002 SJP  - Merged from Carole Nash into Broking
	' RAM20040225       - Code changes related to PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBDocLink"
	
	
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
	
	' Positions in the array
	Public Const ACArrayDocLinkID As Integer = 0
	Public Const ACArrayGISSchemeDesc As Integer = 1
	Public Const ACArrayProcessDesc As Integer = 2
	Public Const ACArrayDocTypeDesc As Integer = 3
	Public Const ACArrayDocTempDesc As Integer = 4
	Public Const ACArrayIsDeleted As Integer = 5
	Public Const ACArrayAgentDesc As Integer = 6
	Public Const ACArraySpoolDocument As Integer = 7
	Public Const ACArrayDocTemplateID As Integer = 8
	Public Const ACArrayDocSchemeVer As Integer = 9
	Public Const ACArrayAutoArchiveDocument As Integer = 10
	Public Const ACArraylGISSchemeID As Integer = 11
	Public Const ACArraylProcessID As Integer = 12
	Public Const ACArraylDocumentTypeID As Integer = 13
	Public Const ACArraylAgentID As Integer = 14
	Public Const ACArrayDocLinkMaxCol As Integer = 14
	
	Public Const ACArrayGISSchemeID As Integer = 0
	Public Const ACArrayGISScheme As Integer = 1
	
	Public Const ACArrayProcessID As Integer = 0
	Public Const ACArrayProcess As Integer = 1
	
	Public Const ACArrayAgentID As Integer = 0
	Public Const ACArrayAgent As Integer = 1
	
	Public Const ACArrayDocTypeID As Integer = 0
	Public Const ACArrayDocType As Integer = 1
	
	Public Const ACArrayDocTempID As Integer = 0
	Public Const ACArrayDocTemp As Integer = 1
	Public Const ACArrayDocTempDocTypeID As Integer = 2
	
	' RAM20040225 : Declared the following variable.
	'               PN Issue 6151, 6748, 7408, 10286 (1.8.5 Catch-up)
    ' Declare an instance of the Business object (moved from frmInterface)
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oBusiness As Object
End Module