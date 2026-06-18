Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports SharedFiles

Module ADVReg
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
	Public Const KEY_QUERY_VALUE As Integer = &H1s
	Public Const KEY_SET_VALUE As Integer = &H2s
	Public Const KEY_CREATE_SUB_KEY As Integer = &H4s
	Public Const KEY_ENUMERATE_SUB_KEYS As Integer = &H8s
	Public Const KEY_NOTIFY As Integer = &H10s
	Public Const KEY_CREATE_LINK As Integer = &H20s
	Public Const KEY_ALL_ACCESS As Integer = ((STANDARD_RIGHTS_ALL Or KEY_QUERY_VALUE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY Or KEY_CREATE_LINK) And (Not SYNCHRONIZE))
	
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
		Dim TStr1 As String = ""
		
		Dim lKeyValue, lDataTypeValue As Integer
		Dim td As Double
		

		Try 
			
			Dim TStr2 As New StringBuilder
			
			Dim lResult As Integer = RegOpenKey(Group, Section, lKeyValue)
			Dim sValue As String = New String(" "c, 2048)
			Dim lValueLength As Integer = sValue.Length
			lResult = RegQueryValueEx(lKeyValue, Key, 0, lDataTypeValue, sValue, lValueLength)
			
			If (lResult = 0) And (Information.Err().Number = 0) Then
				
                If lDataTypeValue = gPMConstants.REG_DWORD Then
                    td = Strings.Asc(sValue.Substring(0, 1)(0)) + &H100 * Strings.Asc(sValue.Substring(1, 1)(0)) + &H10000 * Strings.Asc(sValue.Substring(2, 1)(0)) + &H1000000 * Strings.Asc(sValue.Substring(3, 1)(0))
                    sValue = StringsHelper.Format(td, "000")
                End If
				
                If lDataTypeValue = gPMConstants.REG_BINARY Then

                    ' Return a binary field as a hex string (2 chars per byte)
                    TStr2 = New StringBuilder("")
                    For i As Integer = 1 To lValueLength
                        TStr1 = Strings.Asc(Mid(sValue, i, 1)(0)).ToString("X")
                        If TStr1.Length = 1 Then TStr1 = "0" & TStr1
                        TStr2.Append(TStr1)
                    Next
                    sValue = TStr2.ToString()
                Else
                    sValue = sValue.Substring(0, lValueLength - 1)
                End If
			Else
				sValue = "Not Found"
			End If
			
			lResult = RegCloseKey(lKeyValue)
			Return sValue
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
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
	Public Function ReadRegistryGetAll(ByVal Group As Integer, ByVal Section As String, ByRef Idx As Integer) As Object
		
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
	
	' This routine allows you to write values into the entire Registry, it currently
	' only handles string and double word values.
	'
	' Example
	' WriteRegistry HKEY_CURRENT_USER, "SOFTWARE\My Name\My App\", "NewSubKey", ValString, "NewValueHere"
	' WriteRegistry HKEY_CURRENT_USER, "SOFTWARE\My Name\My App\", "NewSubKey", ValDWord, "31"
	
	Public Sub WriteRegistry(ByVal Group As Integer, ByVal Section As String, ByVal Key As String, ByVal ValType As InTypes, ByVal Value As String)
		
		Dim lKeyValue, InLen, lNewVal As Integer
		Dim sNewVal As String = ""
		

		Try 
			
			Dim lResult As Integer = RegCreateKey(Group, Section, lKeyValue)
			
			If ValType = InTypes.ValDWord Then
				
				lNewVal = CInt(Value)
				InLen = 4
				lResult = RegSetValueExLong(lKeyValue, Key, 0, ValType, lNewVal, InLen)
				
			Else
				
				If ValType = InTypes.ValString Then
					Value = Value & Strings.Chr(0).ToString()
				End If
				
				sNewVal = Value
				InLen = sNewVal.Length
				lResult = RegSetValueExString(lKeyValue, Key, 0, 1, sNewVal, InLen)
				
			End If
			
			lResult = RegFlushKey(lKeyValue)
			lResult = RegCloseKey(lKeyValue)
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Sub
	
	' This routine deletes a specified key (and all its subkeys and values if on Win95) from the registry.
	' Be very careful using this function.
	'
	' Example
	' DeleteSubkey HKEY_CURRENT_USER, "Software\My Name\My App"
	'
	Public Function DeleteSubkey(ByVal Group As Integer, ByVal Section As String) As String
		Dim lKeyValue As Integer

		Try 
			
			Dim sSubKey As String = ReadRegistryGetSubkey(Group, Section, 0)
			Do Until sSubKey = "Not Found"
				DeleteSubkey(Group, Section & "\" & sSubKey)
				sSubKey = ReadRegistryGetSubkey(Group, Section, 0)
			Loop 
			
			Dim lResult As Integer = RegOpenKeyEx(Group, Strings.Chr(0), 0, KEY_ALL_ACCESS, lKeyValue)
			lResult = RegDeleteKey(lKeyValue, Section)
			lResult = RegCloseKey(lKeyValue)
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
	End Function
	
	' This routine deletes a specified value from below a specified subkey.
	' Be very careful using this function.
	'
	' Example
	' DeleteValue HKEY_CURRENT_USER, "Software\My Name\My App", "NewSubKey"
	'
	Public Function DeleteValue(ByVal Group As Integer, ByVal Section As String, ByVal Key As String) As String
		Dim lKeyValue As Integer

		Try 
			Dim lResult As Integer = RegOpenKey(Group, Section, lKeyValue)
			lResult = RegDeleteValue(lKeyValue, Key)
			lResult = RegCloseKey(lKeyValue)
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
	End Function
End Module