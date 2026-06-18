Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 05 July 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' RFC 17061998 - Server Printer Properties added.
	' DAK200100 - Get the process id
	' ***************************************************************** '
	
	
	' Main global constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bClientManager"
	
    ' Username and Password
    
    Public g_sUsername As String = ""
    
    Public g_sPassword As String = ""
	
    ' Language ID.
    
    Public g_iLanguageID As Integer
	
    ' Source ID
    
    Public g_iSourceID As Integer
	
    ' Country ID
    
    Public g_iCountryID As Integer
	
    ' Log Level
    
    Public g_iLogLevel As Integer
	
    ' Currency
    
    Public g_iCurrencyID As Integer
	
    ' Calling App Name
    
    Public g_sCallingAppName As String = ""
	
    ' Party Count for this User
    
    Public g_lPartyCnt As Integer
	
    ' User ID
    
    Public g_iUserID As Integer
	
    ' User Server Printer properties
    
    Public g_sServerPrinter As String = ""
    
    Public g_iIsPrinterChangeable As Integer
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'DAK200100
	Declare Function GetCurrentProcessId Lib "kernel32" () As Integer
	
    Public Sub Main()



        ' Default LanguageID, SourceID
        g_iLanguageID = 1
        g_iSourceID = 1

    End Sub

	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
	End Sub
End Module