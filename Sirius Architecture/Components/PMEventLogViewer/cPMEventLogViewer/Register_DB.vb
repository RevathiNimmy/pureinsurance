Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
<System.Runtime.InteropServices.ProgId("cRegistryDB_NET.cRegistryDB")> _
Public NotInheritable Class cRegistryDB 
	' ************************************************************
	' Copyright ? 1999-2001 Slightly Tilted Software
	' All rights reserved
	' You're absolutely free to use these resources within your
	'     own applications, but you may not redistribute them
	'     (as source) in any manner whatsoever, whether for profit
	'     or not.
	' The only legitimate source for the original source code is
	'     at the VBPJ site and my own web site at:
	'     http://www.SlightlyTiltedSoftware.com
	' ************************************************************
	
	' ---------------------------------------------
	' Module    : Register_DB.cls
	' By        : L.J. Johnson       Date: 04-28-2001
	' Comments  : Retrieve registry entries for the
	'           :     EventMessageFile or PamameterMessageLog
	'           : Get the Event Log Names for this
	'           :     system
	' ---------------------------------------------
	
	' ************************************************
	' Added to replace global variables 18/09/2003
	' Username.
	Private m_sUsername As String = ""
	
	' Password.
	Private m_sPassword As String = ""
	
	' User ID
	Private m_iUserID As Integer
	
	' Calling Application
	Private m_sCallingAppName As String = ""
	' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	' ************************************************
	
	
	' -------------------------------------------------
	' For keeping track of our own error info
	' -------------------------------------------------
	Private m_lngErrNumber As Integer
	Private m_strErrSource As String = ""
	Private m_strErrDesc As String = ""
	
	' --------------------------------------------
	'
	' --------------------------------------------
	Private Const ERROR_SUCCESS As Integer = 0
	Private Const ERR_NO_OPEN_KEY As Integer = 9005
	Private Const ERR_NO_CLOSE_KEY As Integer = 9008
	
	' --------------------------------------------
	' Used as part of error string return
	' --------------------------------------------
	Private Const REG_SOURCENAME As String = "ReadEventLogs.RegistryDB."
	
	' --------------------------------------------
	' Type of item to look up
	' --------------------------------------------
	Public Enum enmMessageLookup
		EventMessage = 0
		ParameterMessage = 1
		CategoryMessage = 2
	End Enum
	
	' --------------------------------------------
	' Module-level variables used to feed the Property
	'   Get functions
	' --------------------------------------------
	Private m_strLogType As String = Nothing
	Private m_strSourceName As String = Nothing
	Private m_strRegistryEntry As String = ""
	
	' --------------------------------------------
	'
	' --------------------------------------------
	Private Structure FILETIME
		Dim dwLowDateTime As Integer
		Dim dwHighDateTime As Integer
		Public Function CompareFileTime(ByRef ft As FILETIME) As Integer
			Dim result As Integer = dwHighDateTime.CompareTo(ft.dwHighDateTime)
			
			If result = 0 Then
				result = dwLowDateTime.CompareTo(ft.dwLowDateTime)
			End If
			
			Return result
		End Function
	End Structure
	
	' --------------------------------------------
	' These 32-bit API calls are used only in this
	'   class module
	' --------------------------------------------
	Private Declare Function RegConnectRegistry Lib "advapi32.dll"  Alias "RegConnectRegistryA"(ByVal lpMachineName As String, ByVal hKey As Integer, ByRef phkResult As Integer) As Integer
	Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer
	Private Declare Function RegOpenKeyEx Lib "advapi32"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	Private Declare Function RegQueryValueEx Lib "advapi32"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByRef lpReserved As Integer, ByRef lptype As Integer, ByVal lpData As Integer, ByRef lpcbData As Integer) As Integer

	Private Declare Function RegEnumKeyEx Lib "advapi32.dll"  Alias "RegEnumKeyExA"(ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByRef lpcbName As Integer, ByVal lpReserved As Integer, ByVal lpClass As String, ByRef lpcbClass As Integer, ByRef lpftLastWriteTime As FILETIME) As Integer
	Private Declare Function RegEnumKey Lib "advapi32.dll"  Alias "RegEnumKeyA"(ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByRef cbName As Integer) As Integer
	
	' --------------------------------------------
	' These constants for main registry keys are used
	'   only in this class module
	' --------------------------------------------
	Private Const HKEY_CLASSES_ROOT As Integer = &H80000000
	Private Const HKEY_CURRENT_USER As Integer = &H80000001
	Private Const HKEY_LOCAL_MACHINE As Integer = &H80000002
	Private Const HKEY_USERS As Integer = &H80000003
	Private Const HKEY_PERFORMANCE_DATA As Integer = &H80000004
	Private Const HKEY_CURRENT_CONFIG As Integer = &H80000005
	Private Const HKEY_DYN_DATA As Integer = &H80000006
	
	' --------------------------------------------
	' Read/Write permissions for registry -- used only
	'   in this class module
	' --------------------------------------------
	Private Const KEY_QUERY_VALUE As Integer = &H1
	Private Const KEY_SET_VALUE As Integer = &H2
	Private Const KEY_CREATE_SUB_KEY As Integer = &H4
	Private Const KEY_ENUMERATE_SUB_KEYS As Integer = &H8
	Private Const KEY_NOTIFY As Integer = &H10
	Private Const KEY_CREATE_LINK As Integer = &H20
	Private Const READ_CONTROL As Integer = &H20000
	Private Const WRITE_DAC As Integer = &H40000
	Private Const WRITE_OWNER As Integer = &H80000
	Private Const SYNCHRONIZE As Integer = &H100000
	Private Const STANDARD_RIGHTS_REQUIRED As Integer = &HF0000
	Private Const STANDARD_RIGHTS_READ As Integer = READ_CONTROL
	Private Const STANDARD_RIGHTS_WRITE As Integer = READ_CONTROL
	Private Const STANDARD_RIGHTS_EXECUTE As Integer = READ_CONTROL
    'developer guide no. Modified by Alkesh starts
    Private Const KEY_READ As Integer = STANDARD_RIGHTS_READ Or KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY
    Private Const KEY_WRITE As Integer = STANDARD_RIGHTS_WRITE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY
    'developer guide no. Modified by Alkesh ends
	Private Const KEY_EXECUTE As Integer = KEY_READ
	
	Private Const REG_EXPAND_SZ As Integer = 2
	Private Const REG_DWORD As Integer = 4
	Private Const REG_MULTI_SZ As Integer = 7
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Function GetSourceNames
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 06/02/2001 -- 14:13:11
	' Inputs       : ByVal xi_strLogName:String -
	'              : Optional ByVal xi_strServerName:String = "" -
	' Outputs      : N/A
	' Description  :
	'              :
	'              :
	' *******************************************************
	Public Function GetSourceNames(ByVal xi_strLogName As String, Optional ByVal xi_strServerName As String = "") As Object
		Dim p_avntSourceNames() As String
		Dim p_lngRtn, p_lngKeyHandle, p_lngRemoteHandle As Integer
		Const BASE_DIR As String = "System\CurrentControlSet\Services\EventLog\"
		
		' ------------------------------------------
		'
		' ------------------------------------------
		If xi_strServerName.Trim().Length > 0 Then
			If Not xi_strServerName.StartsWith("\\") Then
				xi_strServerName = xi_strServerName & "\\"
			End If
			
			p_lngRtn = RegConnectRegistry(lpMachineName:=xi_strServerName, hKey:=HKEY_LOCAL_MACHINE, phkResult:=p_lngRemoteHandle)
		Else
			p_lngRemoteHandle = 0
		End If
		
		' ------------------------------------------
		' Set the full subkey name
		' ------------------------------------------
		Dim p_strSubKey As String = BASE_DIR & xi_strLogName
		
		' ------------------------------------------
		' Open the key so we can enumerate it's keys
		' ------------------------------------------
		If p_lngRemoteHandle = 0 Then
			p_lngRtn = RegOpenKeyEx(hKey:=HKEY_LOCAL_MACHINE, lpSubKey:=p_strSubKey, ulOptions:=0, samDesired:=KEY_READ, phkResult:=p_lngKeyHandle)
		Else
			p_lngRtn = RegOpenKeyEx(hKey:=p_lngRemoteHandle, lpSubKey:=p_strSubKey, ulOptions:=0, samDesired:=KEY_READ, phkResult:=p_lngKeyHandle)
		End If
		If p_lngRtn <> ERROR_SUCCESS Then
			m_lngErrNumber = (ERR_NO_OPEN_KEY + Constants.vbObjectError)
			m_strErrSource = REG_SOURCENAME & "GetSourceNames"
			m_strErrDesc = "Could not OPEN the key, " & p_strSubKey &  _
			               ", in the registry" & ".  The error was: " &  _
			               ReturnApiErrString(p_lngRtn)
			Exit Function
		End If
		
		' ------------------------------------------
		' Get the current values for 'Sources'
		' ------------------------------------------
		Dim p_strMulti As String = New String(" "c, 2048 * 10)
		Dim p_lngLenMulti As Integer = p_strMulti.Length
		Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(p_strMulti)
		Try 
			p_lngRtn = RegQueryValueEx(hKey:=p_lngKeyHandle, lpValueName:="Sources", lpReserved:=0, lptype:=REG_MULTI_SZ, lpData:=tmpPtr, lpcbData:=p_lngLenMulti)
			p_strMulti = Marshal.PtrToStringAnsi(tmpPtr)
		Finally 
			Marshal.FreeHGlobal(tmpPtr)
		End Try
		If p_lngRtn <> ERROR_SUCCESS Then
			m_lngErrNumber = (ERR_NO_OPEN_KEY + Constants.vbObjectError)
			m_strErrSource = REG_SOURCENAME & "GetSourceNames"
			m_strErrDesc = "Could not READ the value, 'Sources', " &  _
			               "in the registry at HKLM\" & p_strSubKey & "." & Strings.Chr(13) & Strings.Chr(10) & "The error was: " &  _
			               ReturnApiErrString(p_lngRtn)
			Exit Function
		Else
			p_strMulti = p_strMulti.Substring(0, p_lngLenMulti)
			p_avntSourceNames = p_strMulti.Split(New String(){Strings.Chr(0).ToString()}, StringSplitOptions.None)
		End If
		
		p_lngRtn = RegCloseKey(p_lngKeyHandle)
		
		Return p_avntSourceNames
		
	End Function
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Function GetLogNames
	' Written By   : L.J. Johnson
	' Programmer   : L.J. Johnson [Slightly Tilted Software]
	' Date Writen  : 04/28/2001 -- 18:05:15
	' Inputs       : N/A
	' Outputs      : Variant --
	' Description  : Get the names of the event logs available
	'              :     on this system
	'              : Applies to W2K+, defaults for NT 4.x
	' Called By    : *****
	' *******************************************************
	Public Function GetLogNames(Optional ByVal xi_strServerName As String = "") As Object
		Dim result As Object = Nothing
		Dim p_lngRtn, p_lngLenKeyName, p_lngIndex, p_lngNumItems, p_lngKeyHandle, p_lngRemoteHandle As Integer
		Dim p_strKeyName As String = ""
		Dim p_typLastWriteTime As New FILETIME
		Dim p_vntNames() As Object
		
		Const BASE_DIR As String = "System\CurrentControlSet\Services\EventLog"
		Const NO_MORE_ITEMS As Integer = 259
		
		' ------------------------------------------
		' Set the full subkey name
		' ------------------------------------------
		Dim p_strSubKey As String = BASE_DIR
		
		' ------------------------------------------
		'
		' ------------------------------------------
		If xi_strServerName.Trim().Length > 0 Then
			If Not xi_strServerName.StartsWith("\\") Then
				xi_strServerName = xi_strServerName & "\\"
			End If
			
			p_lngRtn = RegConnectRegistry(lpMachineName:=xi_strServerName, hKey:=HKEY_LOCAL_MACHINE, phkResult:=p_lngRemoteHandle)
		Else
			p_lngRemoteHandle = 0
		End If
		
		' ------------------------------------------
		' Open the key so we can enumerate it's keys
		' ------------------------------------------
		If p_lngRemoteHandle = 0 Then
			p_lngRtn = RegOpenKeyEx(hKey:=HKEY_LOCAL_MACHINE, lpSubKey:=p_strSubKey, ulOptions:=0, samDesired:=KEY_READ, phkResult:=p_lngKeyHandle)
		Else
			p_lngRtn = RegOpenKeyEx(hKey:=p_lngRemoteHandle, lpSubKey:=p_strSubKey, ulOptions:=0, samDesired:=KEY_READ, phkResult:=p_lngKeyHandle)
		End If
		If p_lngRtn <> ERROR_SUCCESS Then
			m_lngErrNumber = (ERR_NO_OPEN_KEY + Constants.vbObjectError)
			m_strErrSource = REG_SOURCENAME & "RegisterEventSource"
			m_strErrDesc = "Could not OPEN the list of event log names in " &  _
			               BASE_DIR & ".  The error was: " &  _
			               ReturnApiErrString(p_lngRtn)
			Return result
		End If
		
		' ------------------------------------------
		' Now, enumerate them
		' ------------------------------------------
		Do 
			p_lngLenKeyName = 255
			p_strKeyName = New String(Strings.Chr(0), p_lngLenKeyName)
			p_lngRtn = RegEnumKey(hKey:=p_lngKeyHandle, dwIndex:=p_lngIndex, lpName:=p_strKeyName, cbName:=p_lngLenKeyName)
			
			If p_lngRtn = NO_MORE_ITEMS Then
				result = p_vntNames
				GoTo CloseKey
				
			ElseIf p_lngRtn = ERROR_SUCCESS Then 
				ReDim Preserve p_vntNames(p_lngNumItems)

				p_vntNames(p_lngNumItems) = p_strKeyName.Substring(0, Math.Min(p_strKeyName.Length, p_lngLenKeyName))
				p_lngNumItems += 1
				p_lngIndex += 1
				
			Else
				m_lngErrNumber = (Information.Err().LastDllError + Constants.vbObjectError)
				m_strErrSource = REG_SOURCENAME & "GetLogName"
                m_strErrDesc = "Error retriving event log names, " & _
                               ReturnApiErrString(Information.Err().LastDllError)
                'NIIT - Replaced with the Migrated code 1144 
                'Throw New System.Exception((Number:=m_lngErrNumber).ToString() + ", " + Description:=m_strErrDesc + ", " + Source:=m_strErrSource)
                Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)

			End If
			
		Loop While p_lngRtn <> NO_MORE_ITEMS
		
CloseKey: 
		If p_lngRemoteHandle <> 0 Then
			p_lngRtn = RegCloseKey(p_lngRemoteHandle)
		End If
		
		' ------------------------------------------
		' Always close the key you opened
		' ------------------------------------------
		p_lngRtn = RegCloseKey(p_lngKeyHandle)
		
		If p_lngRtn <> ERROR_SUCCESS Then
			m_lngErrNumber = (ERR_NO_CLOSE_KEY + Constants.vbObjectError)
			m_strErrSource = REG_SOURCENAME & "RegisterEventSource"
			m_strErrDesc = "Could not CLOSE the key, " &  _
			               BASE_DIR & ".  The error was: " &  _
			               ReturnApiErrString(p_lngRtn)
		End If
		
		Return result
	End Function
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Sub RegistryEntry
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 11:55:54
	' Inputs       : xi_enmMsgType:Long -
	' Outputs      : N/A
	' Description  : Get the registry entry for EventMessageFile or
	'              :     PamameterMessageLog entry --
	'              : Have to set the Log Type and SourceName before
	'              :     calling this Property
	' *******************************************************
	Public Sub RegistryEntry(ByVal xi_enmMsgType As enmMessageLookup)

		Dim resume2 As Boolean = True
		Try  ' Don't accept an error here
			Dim p_lngKeyType, p_lngOpenedKey As Integer
            Dim p_strValueName As String
			
			' -----------------------------------------------------
			' Set the value for the Value Name
			' -----------------------------------------------------
			If xi_enmMsgType = enmMessageLookup.ParameterMessage Then
				p_strValueName = "ParameterMessageFile"
			ElseIf xi_enmMsgType = enmMessageLookup.CategoryMessage Then 
				p_strValueName = "CategoryMessageFile"
			Else
				p_strValueName = "EventMessageFile"
			End If
			
			' -----------------------------------------------------
			' Check to make sure that the 2 variables have been set
			' -----------------------------------------------------
			If m_strLogType.Trim().Length = 0 Or m_strLogType = CStr(&HFFFFFFFF) Then
				m_lngErrNumber = (ERR_LOGTYPE_NOT_SET + Constants.vbObjectError)
				m_strErrSource = REG_SOURCENAME & "RegistryEntry"
				m_strErrDesc = "The Log Type has not been set."

				resume2 = False
				Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
				
			ElseIf m_strSourceName.Trim().Length = 0 Then 
				m_lngErrNumber = (ERR_SOURCENAME_NOT_SET + Constants.vbObjectError)
				m_strErrSource = REG_SOURCENAME & "RegistryEntry"
				m_strErrDesc = "The Source Name has not been set."

				resume2 = False
				Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
				
			End If
			
			' --------------------------------------------------
			' Open key
			' --------------------------------------------------
			Dim p_strKey As String = "SYSTEM\CurrentControlSet\Services\EventLog\" & m_strLogType & "\" & m_strSourceName
			Dim p_lngRtn As Integer = RegOpenKeyEx(HKEY_LOCAL_MACHINE, p_strKey, 0, KEY_READ, p_lngOpenedKey)
			
			If p_lngRtn <> ERROR_SUCCESS Then
				m_lngErrNumber = (ERR_FAILED_OPEN_REGISTRY_KEY + Constants.vbObjectError)
				m_strErrSource = REG_SOURCENAME & "RegistryEntry"
				m_strErrDesc = "Error opening Registry Key (RegOpenKeyEx), " & "'" & p_strKey & "'" & "." & Strings.Chr(13) & Strings.Chr(10) & "The error was: " & ReturnApiErrString(p_lngRtn)

				resume2 = False
				'Debug.Print m_strErrDesc
				Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
			End If
			
			' --------------------------------------------------
			' Set up buffer for data to be returned in.
			' --------------------------------------------------
			Dim p_lngLenReturnedData As Integer = 255
			Dim p_strReturnString As String = New String(" "c, p_lngLenReturnedData)
			
			' --------------------------------------------------
			' Read key -- try again if buffer isn't big enough
			' --------------------------------------------------
ValueTryAgain: 
			Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(p_strReturnString)
			Try 
				p_lngRtn = RegQueryValueEx(p_lngOpenedKey, p_strValueName, 0, p_lngKeyType, tmpPtr, p_lngLenReturnedData)
				p_strReturnString = Marshal.PtrToStringAnsi(tmpPtr)
			Finally 
				Marshal.FreeHGlobal(tmpPtr)
			End Try
			
			If p_lngRtn = ERROR_MORE_DATA Then
				p_strReturnString = New String(" "c, p_lngLenReturnedData)
				GoTo ValueTryAgain
				
			ElseIf p_lngRtn <> ERROR_SUCCESS Then 
				m_lngErrNumber = (ERR_FAILED_READ_REGISTRY_KEY + Constants.vbObjectError)
				m_strErrSource = REG_SOURCENAME & "RegistryEntry"
				m_strErrDesc = "Error querying the Registry Entry value (RegQueryValueEx), " & "'" & p_strKey & "'" & " for the Value Name, " & "'" & p_strValueName & "'." & Strings.Chr(13) & Strings.Chr(10) & "The error was: " & ReturnApiErrString(p_lngRtn)
				'Debug.Print m_strErrDesc

				resume2 = False
				Throw New System.Exception(m_lngErrNumber.ToString() + ", " + m_strErrSource + ", " + m_strErrDesc)
				
			Else
				m_strRegistryEntry = p_strReturnString.Substring(0, p_lngLenReturnedData - 1)
				
			End If
			
			
			' --------------------------------------------------
			' Always close opened keys!
			' --------------------------------------------------
			p_lngRtn = RegCloseKey(p_lngOpenedKey)
		
		Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
			If Not resume2 Then
				Throw exc
			End If
		End Try
	
	End Sub
	
	' ***********************************************************************
	' ***********************************************************************
	' ****                     Read/Write Properties                     ****
	' ***********************************************************************
	' ***********************************************************************
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Let LogType
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 11:53:37
	' Inputs       : ByVal xi_strLogType:String -
	' Outputs      : N/A
	' Description  : Get/Let the log type
	' *******************************************************
	
	Public Property LogType() As String
		Get
			Return m_strLogType
		End Get
		Set(ByVal Value As String)
			m_strLogType = Value
		End Set
	End Property
	
	' *******************************************************
	' Routine Name : (PUBLIC in CLASS) Property Let SourceName
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 11:53:12
	' Inputs       : ByVal xi_strSourceName:String -
	' Outputs      : N/A
	' Description  : Get/Let the Source name
	' *******************************************************
	
	Public Property SourceName() As String
		Get
			Return m_strSourceName
		End Get
		Set(ByVal Value As String)
			m_strSourceName = Value
		End Set
	End Property
	
	' ***********************************************************************
	' ***********************************************************************
	' ****                        READ-ONLY Properties                   ****
	' ***********************************************************************
	' ***********************************************************************
	
	' -------------------------------------------------
	' Get the Value string for the registry value entry
	' -------------------------------------------------
	Public ReadOnly Property RegistryEntryText() As String
		Get
			Return m_strRegistryEntry
		End Get
	End Property
	
	' ***********************************************************************
	' ***********************************************************************
	' ****                           CLASS Properties                    ****
	' ***********************************************************************
	' ***********************************************************************
	
	' *******************************************************
	' Routine Name : (PRIVATE in CLASS) Sub Class_Initialize
	' Written By   : L.J  Johnson
	' Programmer   : L.J  Johnson [Slightly Tilted Software]
	' Date Writen  : 05/29/2001 -- 11:48:30
	' Inputs       : N/A
	' Outputs      : N/A
	' Description  : Set 2 strings to zero-lengths on initialization
	' *******************************************************
End Class
