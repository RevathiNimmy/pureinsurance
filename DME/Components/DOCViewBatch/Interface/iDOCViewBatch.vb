Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {TodaysDate}
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iDOCViewBatch"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' Username.
	Public g_sUsername As String = ""
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
	' User ID
	Public g_iUserID As Integer
	
	' Calling Application
	Public g_sCallingAppName As String = ""
	' Source ID
	Public g_iSourceID As Integer
	' Language ID
	Public g_iLanguageID As Integer
	' Currency ID
	Public g_iCurrencyID As Integer
	' LogLevel
	Public g_iLogLevel As Integer
	
	
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
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	'Public g_iSourceID As Integer
	'Public g_iLanguageID As Integer
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business class
	Public g_oViewBatch As Object ' bDOCViewBatch.Form
	
	' Public instance of the viewer object
	Public g_oViewer As iDOCViewer.Interface_Renamed
	
	Private m_lReturn As Integer
	
	Public bIsStandAlone As Boolean
	
	Public lNumberDocuments As Integer
	
	' Current user and log level
	Public sCurrentUserName As String = ""
	Public iCurrentLogLevel As Integer
	
	' Directory set as the scan directory (in the registry)
	Public sScanDirectory As String = ""
	
	Public Const ROOT_NODE As Integer = 1
	
	' Constants for the buttons
    Public Const DOC_VB_TOOLBAR_EXPANDALL As Integer = 0
    Public Const DOC_VB_TOOLBAR_DELETEALL As Integer = 1
    Public Const DOC_VB_TOOLBAR_VIEWPAGE As Integer = 2
    Public Const DOC_VB_TOOLBAR_DELETE As Integer = 3
    Public Const DOC_VB_TOOLBAR_COMMIT As Integer = 4
    Public Const DOC_VB_TOOLBAR_PAGELEFT As Integer = 5
    Public Const DOC_VB_TOOLBAR_PAGERIGHT As Integer = 6
End Module