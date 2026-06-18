Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 09 July 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' RFC 19/08/1998 - Version & Date of Architecture Added.
	' ***************************************************************** '
	
	
#Const TEST = False
	
	
	' Main global constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iLogonStatusManager"
	
	' Date of Architecture Displayed in Help About
	Public Const ACArchitectureDate As String = "24/05/2004"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
    ' Public Username.
    
    Public g_sUserName As String = ""
    'Public g_sSourceName As String = ""
	
    ' Public Password.
    Public g_sPassword As String = ""
	
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
	' RDC 29112002
	Public g_iCountryID As Integer
	
    ' Public Logon Time.
    
    Public g_dLogonTime As Date
	
    ' Public instance of the logon manager.
    
    Public g_oLogonManager As iLogonManager.LogonManager
	
    ' Local instance of the interface form.
    
    Public g_frmInterface As Form
	' end of global declarations
	
	Sub Main_Renamed()
		
		' Main entry point for the component
#If TEST = True Then

		Call TEST
#End If
		
	End Sub
	
	
	'' ***************************************************************** '
	'' Name: LogMessage
	''
	'' Description: Dummy LogMessage function.
	''              All code uses LogMessagePopup to display errors.
	''              This function is used as some of the functions in
	''              GeneralFunc.bas use LogMessage().
	''
	'' ***************************************************************** '
	'Public Sub LogMessage(iType As Integer, sMsg As String, Optional vApp, Optional vClass, Optional vMethod, Optional vErrNo, Optional vErrDesc)
	'
	'    LogMessagePopup _
	''        iType:=iType, _
	''        sMsg:=sMsg, _
	''        vApp:=vApp, _
	''        vClass:=vClass, _
	''        vMethod:=vMethod, _
	''        vErrNo:=vErrNo, _
	''        vErrDesc:=vErrDesc
	'
	'End Sub
	
	Sub TEST()
		
		
		Dim oLogonStatusManager As New LogonStatusManager
		
		Dim lError As Integer = oLogonStatusManager.Start()
		
		
		
	End Sub
End Module