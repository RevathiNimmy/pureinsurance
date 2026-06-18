Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
'developer guide no. 129
Imports SharedFiles
Module RebootPC
	' ***************************************************************** '
	' Module Name: RebootPC
	'
	' Date: 12/02/1999
	'
	' Description: Contains a Method to reboot and/or logoff a PC
	'              which will work on Win95 & NT.
	'
	' Edit History:
	' ***************************************************************** '
	
	Public Const ACClass As String = "RebootPC"
	
	Private Structure LUID
		Dim UsedPart As Integer
		Dim IgnoredForNowHigh32BitPart As Integer
	End Structure
	
	Private Structure TOKEN_PRIVILEGES
		Dim PrivilegeCount As Integer
		Dim TheLuid As LUID
		Dim Attributes As Integer
		Public Shared Function CreateInstance() As TOKEN_PRIVILEGES
			Dim result As New TOKEN_PRIVILEGES
			Return result
		End Function
	End Structure
	
	' Beginning of Code
	Private Const EWX_LogOff As Integer = 0
	Private Const EWX_SHUTDOWN As Integer = 1
	Private Const EWX_FORCE As Integer = 4
	Private Const EWX_REBOOT As Integer = 2
	
	Private Declare Function ExitWindowsEx Lib "user32" (ByVal dwOptions As Integer, ByVal dwReserved As Integer) As Integer
	
	Private Declare Function GetCurrentProcess Lib "kernel32" () As Integer
	Private Declare Function OpenProcessToken Lib "advapi32" (ByVal ProcessHandle As Integer, ByVal DesiredAccess As Integer, ByRef TokenHandle As Integer) As Integer

	Private Declare Function LookupPrivilegeValue Lib "advapi32"  Alias "LookupPrivilegeValueA"(ByVal lpSystemName As String, ByVal lpName As String, ByRef lpLuid As LUID) As Integer


	Private Declare Function AdjustTokenPrivileges Lib "advapi32" (ByVal TokenHandle As Integer, ByVal DisableAllPrivileges As Integer, ByRef NewState As TOKEN_PRIVILEGES, ByVal BufferLength As Integer, ByRef PreviousState As TOKEN_PRIVILEGES, ByRef ReturnLength As Integer) As Integer
	
	Private Sub AdjustToken()
		
		Const TOKEN_ADJUST_PRIVILEGES As Integer = &H20s
		Const TOKEN_QUERY As Integer = &H8s
		Const SE_PRIVILEGE_ENABLED As Integer = &H2s
		Dim hdlTokenHandle As Integer
		Dim tmpLuid As New LUID
		Dim tkp As TOKEN_PRIVILEGES = TOKEN_PRIVILEGES.CreateInstance()
		Dim tkpNewButIgnored As TOKEN_PRIVILEGES = TOKEN_PRIVILEGES.CreateInstance()
		Dim lBufferNeeded As Integer
		
		Dim hdlProcessHandle As Integer = GetCurrentProcess()
		OpenProcessToken(hdlProcessHandle, TOKEN_ADJUST_PRIVILEGES Or TOKEN_QUERY, hdlTokenHandle)
		
		' Get the LUID for shutdown privilege.
		LookupPrivilegeValue("", "SeShutdownPrivilege", tmpLuid)
		
		tkp.PrivilegeCount = 1 ' One privilege to set
		tkp.TheLuid = tmpLuid
		tkp.Attributes = SE_PRIVILEGE_ENABLED
		
		' Enable the shutdown privilege in the access token of this
		' process.

		AdjustTokenPrivileges(hdlTokenHandle, False, tkp, Marshal.SizeOf(tkpNewButIgnored), tkpNewButIgnored, lBufferNeeded)
		
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdForceShutdown_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub cmdForceShutdown_Click()
		'
		'ExitWindowsEx (EWX_SHUTDOWN Or EWX_FORCE Or EWX_REBOOT), &HFFFF
		'
	'End Sub
	
	' ***************************************************************** '
	' Name: ShutDownPC
	'
	' Description: Logs Off or Reboots a PC.
	'
	'
	' ***************************************************************** '
	Public Sub ShutDownPC(ByRef v_bLogoffOnly As Boolean)
		
		Try 
			
			' Get the Correct Privilages
			AdjustToken()
			
			' Logoff OR Reboot the PC
			' Note: Do not use the Force option as shown in the example as this
			' will NOT give the user an oportunity to save their data.
			' ExitWindowsEx (EWX_SHUTDOWN Or EWX_FORCE Or EWX_REBOOT), &HFFFF
			
			If v_bLogoffOnly Then
				ExitWindowsEx(EWX_LogOff, &HFFFFs)
			Else
				ExitWindowsEx(EWX_REBOOT, &HFFFFs)
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShutDownPCFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShutDownPC", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
End Module
