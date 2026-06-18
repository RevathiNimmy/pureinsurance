Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 05/10/2009
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History: Sankar Chellathurai
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iSIRFindCashDeptMaintenance"
	
    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	
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
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    ' MKW 190503 PN2032 START
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
	
	Private m_lReturn As Integer
	' MKW 190503 PN2032 END
	
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Buttons
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACClient As Integer = 202
	Public Const ACAgent As Integer = 203
	Public Const ACBank As Integer = 204
	Public Const ACCDNumber As Integer = 205
	Public Const ACFindNowButton As Integer = 206
	Public Const ACNewSearchButton As Integer = 207
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACClearDetailsTitle As Integer = 302
	Public Const ACClearDetails As Integer = 303
	Public Const ACStatusSearching As Integer = 304
	Public Const ACStatusFound As Integer = 305
	Public Const ACAllTypes As Integer = 306
	Public Const ACBusinessFailTitle As Integer = 307
	Public Const ACBusinessFail As Integer = 308
	
	Public Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Public Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066

    

End Module