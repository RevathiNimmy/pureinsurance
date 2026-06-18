Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
'Modified by Sudhanshu Behera on 5/21/2010 6:03:29 PM refer developer guide no. 129
Imports SharedFiles
 Public Module ADVReg
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	'Security Mask constants
	Public Const READ_CONTROL As Integer = &H20000
	Public Const SYNCHRONIZE As Integer = &H100000
	Public Const STANDARD_RIGHTS_ALL As Integer = &H1F0000
	Public Const STANDARD_RIGHTS_READ As Integer = READ_CONTROL
	Public Const STANDARD_RIGHTS_WRITE As Integer = READ_CONTROL
	Public Const KEY_QUERY_VALUE As Integer = &H1s
	Public Const KEY_SET_VALUE As Integer = &H2s
	Public Const KEY_CREATE_SUB_KEY As Integer = &H4s
	Public Const KEY_ENUMERATE_SUB_KEYS As Integer = &H8s
	Public Const KEY_NOTIFY As Integer = &H10s
	Public Const KEY_CREATE_LINK As Integer = &H20s
	' RDC 13062002 moved to gPMConstants
	'Public Const KEY_ALL_ACCESS = ((STANDARD_RIGHTS_ALL Or KEY_QUERY_VALUE Or _
	'KEY_SET_VALUE Or KEY_CREATE_SUB_KEY Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY Or _
	'KEY_CREATE_LINK) And (Not SYNCHRONIZE))
	
	Public Const KEY_READ As Integer = ((STANDARD_RIGHTS_READ Or KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY) And (Not SYNCHRONIZE))
	
	Public Const KEY_EXECUTE As Integer = ((KEY_READ) And (Not SYNCHRONIZE))
	Public Const KEY_WRITE As Integer = ((STANDARD_RIGHTS_WRITE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY) And (Not SYNCHRONIZE))
	
	' Possible registry data types
	Public Enum InTypes
		ValNull = 0
		ValString = 1
		ValXString = 2
		ValBinary = 3
		ValDWord = 4
		ValLink = 6
		ValMultiString = 7
		ValResList = 8
	End Enum
	
	' RDC 13062002 moved to gPMConstants
	' Registry value type definitions
	'Public Const REG_NONE As Long = 0
	'Public Const REG_SZ As Long = 1
	'Public Const REG_EXPAND_SZ As Long = 2
	'Public Const REG_BINARY As Long = 3
	'Public Const REG_DWORD As Long = 4
	'Public Const REG_LINK As Long = 6
	'Public Const REG_MULTI_SZ As Long = 7
	'Public Const REG_RESOURCE_LIST As Long = 8
	
	' RDC 13062002 moved to gPMConstants
	' Registry section definitions
	'Public Const HKEY_CLASSES_ROOT = &H80000000
	'Public Const HKEY_CURRENT_USER = &H80000001
	'Public Const HKEY_LOCAL_MACHINE = &H80000002
	'Public Const HKEY_USERS = &H80000003
	'Public Const HKEY_PERFORMANCE_DATA = &H80000004
	'Public Const HKEY_CURRENT_CONFIG = &H80000005
	'Public Const HKEY_DYN_DATA = &H80000006
	
	' Codes returned by Reg API calls
	Private Const ERROR_NONE As Integer = 0
	Private Const ERROR_BADDB As Integer = 1
	Private Const ERROR_BADKEY As Integer = 2
	Private Const ERROR_CANTOPEN As Integer = 3
	Private Const ERROR_CANTREAD As Integer = 4
	Private Const ERROR_CANTWRITE As Integer = 5
	Private Const ERROR_OUTOFMEMORY As Integer = 6
	Private Const ERROR_INVALID_PARAMETER As Integer = 7
	Private Const ERROR_ACCESS_DENIED As Integer = 8
	Private Const ERROR_INVALID_PARAMETERS As Integer = 87
	Private Const ERROR_NO_MORE_ITEMS As Integer = 259
	
	' Registry API functions used in this module (there are more of them)
	Private Declare Function RegOpenKey Lib "advapi32.dll"  Alias "RegOpenKeyA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByRef phkResult As Integer) As Integer
	Private Declare Function RegOpenKeyEx Lib "advapi32.dll"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	Private Declare Function RegQueryValueEx Lib "advapi32.dll"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
	Private Declare Function RegEnumValue Lib "advapi32.dll"  Alias "RegEnumValueA"(ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpValueName As String, ByRef lpcbValueName As Integer, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
	Private Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
	Private Declare Function RegCreateKey Lib "advapi32.dll"  Alias "RegCreateKeyA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByRef phkResult As Integer) As Integer
	Private Declare Function RegSetValueExString Lib "advapi32.dll"  Alias "RegSetValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByVal lpValue As String, ByVal cbData As Integer) As Integer
	Private Declare Function RegSetValueExLong Lib "advapi32.dll"  Alias "RegSetValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByRef lpValue As Integer, ByVal cbData As Integer) As Integer
	Private Declare Function RegFlushKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
	Private Declare Function RegEnumKey Lib "advapi32.dll"  Alias "RegEnumKeyA"(ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByVal cbName As Integer) As Integer
	Private Declare Function RegDeleteKey Lib "advapi32.dll"  Alias "RegDeleteKeyA"(ByVal hKey As Integer, ByVal lpSubKey As String) As Integer
	Private Declare Function RegDeleteValue Lib "advapi32.dll"  Alias "RegDeleteValueA"(ByVal hKey As Integer, ByVal lpValueName As String) As Integer
	
	' This routine allows you to get values from anywhere in the Registry, it currently
	' only handles string, double word and binary values. Binary values are returned as
	' hex strings.
	'
	' Example
	' Text1.Text = ReadRegistry(HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "DefaultUserName")
	'
	Public Function ReadRegistry(ByVal Group As Integer, ByVal Section As String, ByVal Key As String) As String
		Dim lReturn As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim sValue As String = ""
		Try

			lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=Group, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=Key, r_sSettingValue:=sValue), gPMConstants.PMEReturnCode)

			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return "Not Found"
			End If
			Return sValue

		Catch exc As System.Exception
			Return "Not Found"
		End Try

	End Function
	
	
	
	' This routine enumerates the subkeys under any given key
	' Call repeatedly until "Not Found" is returned - store values in array or something
	'
	' Example - this example just adds all the subkeys to a string - you will probably want to
	' save then into an array or something.
	'
	' Dim Res As String
	' Dim i As Long
	' Res = ReadRegistryGetSubkey(HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\", i)
	' Do Until Res = "Not Found"
	'   Text1.Text = Text1.Text & " " & Res
	'   i = i + 1
	'   Res = ReadRegistryGetSubkey(HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\", i)
	' Loop
	
	Public Function ReadRegistryGetSubkey(ByVal Group As Integer, ByVal Section As String, ByRef Idx As Integer) As String
		
        Dim lKeyValue As Integer

		Try 
			
			Dim lResult As Integer = RegOpenKey(Group, Section, lKeyValue)
			Dim sValue As String = New String(" "c, 2048)
			Dim lValueLength As Integer = sValue.Length
			lResult = RegEnumKey(lKeyValue, Idx, sValue, lValueLength)
			
			If (lResult = 0) And (Information.Err().Number = 0) Then
				sValue = sValue.Substring(0, sValue.IndexOf(Strings.Chr(0).ToString()))
			Else
				sValue = "Not Found"
			End If
			
			lResult = RegCloseKey(lKeyValue)
			Return sValue
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Function
	' This routine allows you to get all the values from anywhere in the Registry under any
	' given subkey, it currently only returns string and double word values.
	'
	' Example - returns list of names/values to multiline text box
	' Dim Res As Variant
	' Dim i As Long
	' Res = ReadRegistryGetAll(HKEY_CURRENT_USER, "Software\Microsoft\Notepad", i)
	' Do Until Res(2) = "Not Found"
	'    Text1.Text = Text1.Text & Chr(13) & Chr(10) & Res(1) & " " & Res(2)
	'    i = i + 1
	'    Res = ReadRegistryGetAll(HKEY_CURRENT_USER, "Software\Microsoft\Notepad", i)
	' Loop
	'
	Public Function ReadRegistryGetAll(ByVal Group As Integer, ByVal Section As String, ByRef Idx As Integer) As Object()
		
		Dim lKeyValue, lDataTypeValue As Integer
		Dim td As Double
		

		Try 
			
			'Initialise variables
			Dim lResult As Integer = RegOpenKey(Group, Section, lKeyValue)
			Dim sValue As String = New String(" "c, 2048)
			Dim sValueName As String = New String(" "c, 2048)
			Dim lValueLength As Integer = sValue.Length
			Dim lValueNameLength As Integer = sValueName.Length
			lResult = RegEnumValue(lKeyValue, Idx, sValueName, lValueNameLength, 0, lDataTypeValue, sValue, lValueLength)
			
			If (lResult = 0) And (Information.Err().Number = 0) Then
				If lDataTypeValue = gPMConstants.REG_DWORD Then
					td = Strings.Asc(sValue.Substring(0, 1)(0)) + &H100 * Strings.Asc(sValue.Substring(1, 1)(0)) + &H10000 * Strings.Asc(sValue.Substring(2, 1)(0)) + &H1000000 * Strings.Asc(sValue.Substring(3, 1)(0))
					sValue = StringsHelper.Format(td, "000")
				End If
				sValue = sValue.Substring(0, lValueLength - 1)
				sValueName = sValueName.Substring(0, lValueNameLength)
			Else
				sValue = "Not Found"
			End If
			
			lResult = RegCloseKey(lKeyValue)
			
			' Return the datatype, value name and value as an array

			Return New Object(){lDataTypeValue, sValueName, sValue}
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Function
    'NIIT - Replaced with the Migrated code 1144 
    'Shared Sub New()
    '	MainModule.JustForInvokeMain()
    'End Sub
End Module
