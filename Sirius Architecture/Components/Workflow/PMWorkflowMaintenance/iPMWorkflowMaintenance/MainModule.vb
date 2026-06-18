Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  22/01/2003
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Const ACApp As String = "WorkflowMaintenance"
	
	' Username.
	Public g_sUsername As String = ""
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
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
	' UserID
	Public g_iUserID As Integer
	
	Public Const ACCancelDetailsTitleText As String = "Cancel Details"
	Public Const ACCancelDetailsText As String = "Cancelling will lose any changes" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to cancel?"
	
	Public Const ACSaveDetailsTitleText As String = "Save Details"
	Public Const ACSaveDetailsText As String = "Details have changed" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to save?"
	
	Public Const ACBusinessFailTitleText As String = "Business Object"
	Public Const ACBusinessFailText As String = "Unable to gain access to the business object" & Strings.Chr(13) & Strings.Chr(10) & "Please try later"
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Const USRAddPackage As Integer = 0
	Public Const USREditPackage As Integer = 1
End Module