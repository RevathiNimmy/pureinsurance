Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {17/2/98}
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMAbout"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	Declare Function ShellExecute Lib "shell32.dll"  Alias "ShellExecuteA"(ByVal hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
	
	' RDC 13062002 now in gPMConstants.bas ################################## START
	'Public Const HKEY_LOCAL_MACHINE = &H80000002
	
	'Public Const REG_SZ As Long = 1
	'Public Const KEY_ALL_ACCESS = &H3F
	
	'Public Const ERROR_NONE = 0
	'Public Const ERROR_BADKEY = 2
	
	'Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias _
	'"RegOpenKeyExA" (ByVal hKey As Long, ByVal lpSubKey As String, _
	'ByVal ulOptions As Long, ByVal samDesired As Long, phkResult As _
	'Long) As Long
	
	Declare Function RegQueryValueEx Lib "advapi32.dll"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
	
	'Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Long) As Long
	' RDC 13062002 now in gPMConstants.bas ################################## END
	
	' General Icons
	
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
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
	
    ' Version information
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sTitle As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sVersionNumber As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sVersionDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sLicensee As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCopyright As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sComponent As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sSupportEmail As String = ""
	
	Private m_lReturn As Integer
	

	Public Sub Main()
		
	End Sub

	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
	End Sub
End Module