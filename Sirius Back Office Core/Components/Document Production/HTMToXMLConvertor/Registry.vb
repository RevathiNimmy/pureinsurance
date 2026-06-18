Option Strict Off
Option Explicit On
Imports SharedFiles
Module Registry
	' ********************************************************************* '
	'
	' Module : ADVReg
	'
	' Description : Advanced Registry Functions
	'
	' ********************************************************************* '
	'Security Mask constants
	Public Const READ_CONTROL As Integer = &H20000
	Public Const SYNCHRONIZE As Integer = &H100000
	Public Const STANDARD_RIGHTS_ALL As Integer = &H1F0000
	Public Const STANDARD_RIGHTS_READ As Integer = READ_CONTROL
	Public Const STANDARD_RIGHTS_WRITE As Integer = READ_CONTROL
	Public Const KEY_QUERY_VALUE As Short = &H1s
	Public Const KEY_SET_VALUE As Short = &H2s
	Public Const KEY_CREATE_SUB_KEY As Short = &H4s
	Public Const KEY_ENUMERATE_SUB_KEYS As Short = &H8s
	Public Const KEY_NOTIFY As Short = &H10s
	Public Const KEY_CREATE_LINK As Short = &H20s
	Public Const KEY_ALL_ACCESS As Boolean = ((STANDARD_RIGHTS_ALL Or KEY_QUERY_VALUE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY Or KEY_CREATE_LINK) And (Not SYNCHRONIZE))
	
	Public Const KEY_READ As Boolean = ((STANDARD_RIGHTS_READ Or KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY) And (Not SYNCHRONIZE))
	
	Public Const KEY_EXECUTE As Boolean = ((KEY_READ) And (Not SYNCHRONIZE))
	Public Const KEY_WRITE As Boolean = ((STANDARD_RIGHTS_WRITE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY) And (Not SYNCHRONIZE))
	
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
	
	'SD 02/08/2002 START Scalability changes -remove dupliacte declarations
	
	' Registry value type definitions
	Public Const REG_NONE As Integer = 0
	'Public Const REG_SZ As Long = 1
	Public Const REG_EXPAND_SZ As Integer = 2
	Public Const REG_BINARY As Integer = 3
    Public Const REG_DWORD As Long = 4
	Public Const REG_LINK As Integer = 6
	Public Const REG_MULTI_SZ As Integer = 7
	Public Const REG_RESOURCE_LIST As Integer = 8
	
	' Registry section definitions
	Public Const HKEY_CLASSES_ROOT As Integer = &H80000000
	'Public Const HKEY_CURRENT_USER = &H80000001
	'Public Const HKEY_LOCAL_MACHINE = &H80000002
	Public Const HKEY_USERS As Integer = &H80000003
	Public Const HKEY_PERFORMANCE_DATA As Integer = &H80000004
	Public Const HKEY_CURRENT_CONFIG As Integer = &H80000005
	Public Const HKEY_DYN_DATA As Integer = &H80000006
	
	'SD 02/08/2002 END Scalability changes -remove dupliacte declarations
	
	' Codes returned by Reg API calls
	Private Const ERROR_NONE As Short = 0
	Private Const ERROR_BADDB As Short = 1
	Private Const ERROR_BADKEY As Short = 2
	Private Const ERROR_CANTOPEN As Short = 3
	Private Const ERROR_CANTREAD As Short = 4
	Private Const ERROR_CANTWRITE As Short = 5
	Private Const ERROR_OUTOFMEMORY As Short = 6
	Private Const ERROR_INVALID_PARAMETER As Short = 7
	Private Const ERROR_ACCESS_DENIED As Short = 8
	Private Const ERROR_INVALID_PARAMETERS As Short = 87
	Private Const ERROR_NO_MORE_ITEMS As Short = 259
	
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
	
	' ********************************************************************* '
	'
	' Function : WriteRegistry
	'
	' Description : This routine allows you to write values into the entire Registry, it currently
	'               only handles string and double word values.
	'HKEY_LOCAL_MACHINE\Software\PM\SiriusSolutions\Setup\DocumentsConvertedToXML=1
	'' Example:  WriteRegistry HKEY_CURRENT_USER, "SOFTWARE\My Name\My App\", "NewSubKey", ValString, "NewValueHere"
	'           WriteRegistry HKEY_CURRENT_USER, "SOFTWARE\My Name\My App\", "NewSubKey", ValDWord, "31"
	' ********************************************************************* '
    Public Sub WriteRegistry(ByVal Group As Integer, ByVal Section As String, ByVal Key As String, ByVal ValType As InTypes, ByVal Value As Object)

        Dim lResult As Integer
        Dim lKeyValue As Integer
        Dim InLen As Integer
        Dim lNewVal As Integer
        Dim sNewVal As String

        Try


            lResult = RegCreateKey(Group, Section, lKeyValue)

            If ValType = InTypes.ValDWord Then

                'UPGRADE_WARNING: Couldn't resolve default property of object Value. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                lNewVal = CInt(Value)
                InLen = 4
                lResult = RegSetValueExLong(lKeyValue, Key, 0, ValType, lNewVal, InLen)

            Else

                If ValType = InTypes.ValString Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object Value. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Value = Value + Chr(0)
                End If

                'UPGRADE_WARNING: Couldn't resolve default property of object Value. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sNewVal = Value
                InLen = Len(sNewVal)
                lResult = RegSetValueExString(lKeyValue, Key, 0, 1, sNewVal, InLen)

            End If

            lResult = RegFlushKey(lKeyValue)
            lResult = RegCloseKey(lKeyValue)
        Catch ex As Exception

        End Try

    End Sub
End Module
