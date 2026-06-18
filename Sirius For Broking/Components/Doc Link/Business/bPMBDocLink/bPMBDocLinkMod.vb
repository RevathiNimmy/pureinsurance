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
	' Description: Main Module.
	'
	' Edit History:
	'   26/06/2002 SJP - Merged from Carole Nash into Broking
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bPMBDocLink"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	'Public g_sUsername As String * 12
	
	' Password.
	'Public g_sPassword As String * 30
	
	' User ID
	'Public g_iUserID As Integer
	
	' Calling Application
	'Public g_sCallingAppName As String
	' Source ID
	'Public g_iSourceID As Integer
	' Language ID
	'Public g_iLanguageID As Integer
	' Currency ID
	'Public g_iCurrencyID As Integer
	' LogLevel
	'Public g_iLogLevel As Integer
	
	' Positions in the array
	Public Const ACArrayDocLinkID As Integer = 0
	Public Const ACArrayGISSchemeDesc As Integer = 1
	Public Const ACArrayProcessDesc As Integer = 2
	Public Const ACArrayDocTypeDesc As Integer = 3
	Public Const ACArrayDocTempDesc As Integer = 4
	Public Const ACArrayIsDeleted As Integer = 5
	Public Const ACArrayAgentDesc As Integer = 6
	Public Const ACArraySpoolDocument As Integer = 7
	Public Const ACArrayDocTempID As Integer = 8
	Public Const ACArrayDocSchemeVer As Integer = 9
	Public Const ACArrayAutoArchiveDocument As Integer = 10
	Public Const ACArraylGISSchemeID As Integer = 11
	Public Const ACArraylProcessID As Integer = 12
	Public Const ACArraylDocumentTypeID As Integer = 13
	Public Const ACArraylAgentID As Integer = 14
End Module