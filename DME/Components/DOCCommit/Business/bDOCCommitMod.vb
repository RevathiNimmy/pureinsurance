Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  03/12/97
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bDOCCommit"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
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
	' Access level
	Public g_iAccessLevel As Integer
	' Admin level
	Public g_iAdminLevel As Integer
	
	' Stores property value indicating total documents to commit.
	Public g_lDocsTotal As Integer
	
	' Stores property value indicating how many documents have faield to commit.
	Public g_lDocsFailed As Integer
	
	' Stores property value indicating how many documents have been commited.
	Public g_lDocsDone As Integer
	
	' Stores property value indicating what state the business is in. Also
	'used to start the commit procedure.
	Public g_iRunStatus As Integer
	
	' Stores property value indicating the default annotation.
	Public g_sBatchAnnotation As String = ""
	
	' Stores property value holding where the scanned documents are stored.
	Public g_sScanDirectory As String = ""
	
	' DocuMaster Database Class
#If PD_EARLYBOUND = 1 Then

	Public g_oMainDB As dPMDAO.Database
#Else
	Public g_oMainDB As dPMDAO.Database
#End If
	
	' Scan Database Class
#If PD_EARLYBOUND = 1 Then

	Public g_oScanDB As dPMDAO.Database
#Else
	Public g_oScanDB As dPMDAO.Database
#End If
	
	' Server side business object

	Public g_oCommitServer As bDOCCommitServer.Commit
	
	' Public instance of the object manager.
#If PD_EARLYBOUND = 1 Then

	Public g_oObjectManager As bObjectManager.ObjectManager
#Else
	Public g_oObjectManager As bObjectManager.ObjectManager
#End If
	
	' Variable to store the Task Cnt
	Public g_vTaskCnt As Byte
End Module