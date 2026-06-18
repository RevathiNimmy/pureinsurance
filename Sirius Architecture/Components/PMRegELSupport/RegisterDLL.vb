Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Runtime.InteropServices
Imports SharedFiles
Module RegisterDLL
	
	' Used in setting registry values
	Private Const REG_EXPAND_SZ As Integer = 2 ' Unicode nul terminated string (with environment variable references)
	Private Const REG_DWORD As Integer = 4 ' 32-bit number
	
	' Read/Write permissions:
	Private Const KEY_QUERY_VALUE As Integer = &H1
	Private Const KEY_SET_VALUE As Integer = &H2
	Private Const KEY_CREATE_SUB_KEY As Integer = &H4
	Private Const KEY_ENUMERATE_SUB_KEYS As Integer = &H8
	Private Const KEY_NOTIFY As Integer = &H10
	Private Const READ_CONTROL As Integer = &H20000
	Private Const STANDARD_RIGHTS_READ As Integer = READ_CONTROL
	Private Const STANDARD_RIGHTS_WRITE As Integer = READ_CONTROL
	Private Const KEY_READ As Integer = STANDARD_RIGHTS_READ Or KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY
	Private Const KEY_WRITE As Integer = STANDARD_RIGHTS_WRITE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY
	
	' Used by RegCreateKeyEx API function
	Private Const REG_OPTION_NON_VOLATILE As Integer = 0
	Private Const HKEY_LOCAL_MACHINE As Integer = &H80000002
	
	Private Const EVENTLOG_ERROR_TYPE As Integer = 1
	Private Const EVENTLOG_WARNING_TYPE As Integer = 2
	Private Const EVENTLOG_INFORMATION_TYPE As Integer = 4
	
	' Used as part of error string return
	Private Const REG_SOURCENAME As String = "WriteEvents.RegisterEvntSource."
	
	Private Const ERROR_SUCCESS As Integer = 0
	
	Private Const ERR_NO_OPEN_KEY As Integer = 9005
	Private Const ERR_NO_CLOSE_KEY As Integer = 9008
	
	Private Const INTERNET_ERROR_BASE As Integer = 12000
	Private Const INTERNET_ERROR_LAST As Integer = INTERNET_ERROR_BASE + 171
	
	Private Const FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000
	Private Const FORMAT_MESSAGE_IGNORE_INSERTS As Integer = &H200
	Private Const FORMAT_MESSAGE_FROM_HMODULE As Integer = &H800
	Private Const LOAD_LIBRARY_AS_DATAFILE As Integer = 2
	
	Public Const MESSAGE_FILE As String = "cPMEventLogMsg.dll"
	Public Const CATEGORY_FILE As String = "cPMEventLogCat.dll"
	Private Const LOG_FILE_NAME As String = "Application"
	
	Private Const NUM_CATEGORIES As Integer = 4
	
	Private Const NERR_BASE As Integer = 2100
	Private Const MAX_NERR As Integer = NERR_BASE + 899
	
	Private Const MSG_TITLE As String = "Sirius"
	Private Const APP_NAME As String = "Sirius"
	
	Private m_strLocal As String = ""
	Private m_strSaveDrive As String = ""
	Private m_strSaveDir As String = ""
	
	' Used to return errors from RegisterEvntSource
	Private m_lngErrNum As Integer
	Private m_strErrSource As String = ""
	Private m_strErrDesc As String = ""
	
	' Used in RegCreateKeyEx API call
	Private Structure SECURITY_ATTRIBUTES
		Dim length As Integer
		Dim SecurityDescriptor As Integer
		Dim InheritHandle As Integer
	End Structure
	
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
	
	' Declare the API calls to Open, Close, Create, and Set Values in the registry
	Private Declare Function RegOpenKeyEx Lib "advapi32"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	
	Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer
	

	Private Declare Function RegCreateKeyEx Lib "advapi32"  Alias "RegCreateKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal Reserved As Integer, ByVal lpClass As String, ByVal dwOptions As Integer, ByVal samDesired As Integer, ByRef lpSecurityAttributes As SECURITY_ATTRIBUTES, ByRef phkResult As Integer, ByRef lpdwDisposition As Integer) As Integer
	
	Private Declare Function RegDeleteKey Lib "advapi32.dll"  Alias "RegDeleteKeyA"(ByVal hKey As Integer, ByVal lpSubKey As String) As Integer
	

	Private Declare Function RegEnumKeyEx Lib "advapi32.dll"  Alias "RegEnumKeyExA"(ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByRef lpcbName As Integer, ByVal lpReserved As Integer, ByVal lpClass As String, ByRef lpcbClass As Integer, ByRef lpftLastWriteTime As FILETIME) As Integer
	
	Private Declare Function RegEnumKey Lib "advapi32.dll"  Alias "RegEnumKeyA"(ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByRef cbName As Integer) As Integer
	
	Private Declare Function RegSetValueEx Lib "advapi32"  Alias "RegSetValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByVal lpData As Integer, ByVal cbData As Integer) As Integer
	
	Private Declare Function RegFlushKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
	
	Private Declare Function RegQueryValueEx Lib "advapi32.dll"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As Integer, ByRef lpcbData As Integer) As Integer
	
	Private Declare Function LoadLibraryEx Lib "kernel32"  Alias "LoadLibraryExA"(ByVal lpLibFileName As String, ByVal hFile As Integer, ByVal dwFlags As Integer) As Integer
	
	Private Declare Function FormatMessage Lib "kernel32"  Alias "FormatMessageA"(ByVal dwFlags As Integer, ByVal lpSource As Integer, ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByVal lpBuffer As String, ByVal nSize As Integer, ByRef Arguments As Integer) As Integer
	
	Private Declare Function FreeLibrary Lib "kernel32" (ByVal hLibModule As Integer) As Integer
	
	Public Function RegisterEventLogDLLs(ByVal sPath As String) As Integer
		
		Dim result As Integer = 0
		Dim p_strMsg As String = ""

		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' register
			RegisterEventSource(MESSAGE_FILE, CATEGORY_FILE, sPath, APP_NAME, LOG_FILE_NAME, NUM_CATEGORIES)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (GetLogNames) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetLogNames() As Object
		'
		'Dim result As Object = Nothing
		'Dim p_lngLenKeyName, p_lngIndex, p_lngNumItems, p_lngKeyHandle As Integer
		'Dim p_strKeyName As String = ""
		'Dim p_typLastWriteTime As New FILETIME
		'Dim p_vntNames() As Object
		'
		'Const BASE_DIR As String = "System\CurrentControlSet\Services\EventLog"
		'Const NO_MORE_ITEMS As Integer = 259
		'
		' Set the full subkey name
		'Dim p_strSubKey As String = BASE_DIR
		'
		' Open the key so we can enumerate it's keys
		'Dim p_lngRtn As Integer = RegOpenKeyEx(hKey:=HKEY_LOCAL_MACHINE, lpSubKey:=p_strSubKey, ulOptions:=0, samDesired:=KEY_READ, phkResult:=p_lngKeyHandle)
		'
		'If p_lngRtn <> ERROR_SUCCESS Then
			'm_lngErrNum = (ERR_NO_OPEN_KEY + Constants.vbObjectError)
			'm_strErrSource = REG_SOURCENAME & "RegisterEventSource"
			'm_strErrDesc = "Could not OPEN the list of event log names in " &  _
			'               BASE_DIR & ".  The error was: " &  _
			'               ReturnApiErrString(p_lngRtn)
			'Return result
		'End If
		'
		' Now, enumerate them
		'Do 
			'p_lngLenKeyName = 255
			'p_strKeyName = New String(Strings.Chr(0), p_lngLenKeyName)
			'p_lngRtn = RegEnumKey(hKey:=p_lngKeyHandle, dwIndex:=p_lngIndex, lpName:=p_strKeyName, cbName:=p_lngLenKeyName)
			'
			'If p_lngRtn = NO_MORE_ITEMS Then
				'result = p_vntNames
				'GoTo CloseKey
				'
			'ElseIf p_lngRtn = ERROR_SUCCESS Then 
				''ReDim Preserve p_vntNames(p_lngNumItems)

				'p_vntNames(p_lngNumItems) = p_strKeyName.Substring(0, Math.Min(p_strKeyName.Length, p_lngLenKeyName))
				'p_lngNumItems += 1
				'p_lngIndex += 1
			'Else
				'm_lngErrNum = (Information.Err().LastDllError + Constants.vbObjectError)
				'm_strErrSource = REG_SOURCENAME & "GetLogName"
				'm_strErrDesc = "Error retriving event log names, " &  _
				'               ReturnApiErrString(Information.Err().LastDllError)
				'Throw New System.Exception((Number:=m_lngErrNum).ToString() + ", " + Description:=m_strErrDesc + ", " + Source:=m_strErrSource)
			'End If
			'
		'Loop While p_lngRtn <> NO_MORE_ITEMS
		'
'CloseKey: '
		'
		' Always close the key you opened
		'p_lngRtn = RegCloseKey(p_lngKeyHandle)
		'
		'If p_lngRtn <> ERROR_SUCCESS Then
			'm_lngErrNum = (ERR_NO_CLOSE_KEY + Constants.vbObjectError)
			'm_strErrSource = REG_SOURCENAME & "RegisterEventSource"
			'm_strErrDesc = "Could not CLOSE the key, " &  _
			'               BASE_DIR & ".  The error was: " &  _
			'               ReturnApiErrString(p_lngRtn)
		'End If
		'
		'Return result
	'End Function
	
	Private Function RegisterEventSource(ByVal xi_strMessageFile As String, ByVal xi_strCategoryFile As String, ByVal xi_strFilePath As String, ByVal xi_strAppName As String, ByVal xi_strLogName As String, Optional ByVal xi_lngNumCategories As Integer = 0) As Integer
		
		Dim result As Integer = 0
		Dim p_lngRtn, p_lngDisposition As Integer
		Dim p_strSubKey As String = ""
		Dim p_lngResKey, p_lngKeyHandle As Integer
		Dim p_strValData As String = ""
		Dim p_lngPtrValData, p_lngLenData As Integer
        Dim p_strValueName As String
        Dim p_lngSuccess As Integer
		Dim p_typSecAttrib As New SECURITY_ATTRIBUTES
		
		Const BASE_DIR As String = "System\CurrentControlSet\Services\EventLog\"
		
		 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the full subkey name and default the return value to TRUE
			p_strSubKey = BASE_DIR & xi_strLogName & "\" & xi_strAppName
			
			' Create the key -- this will work even if the  key already exists!
			p_lngRtn = RegCreateKeyEx(hKey:=HKEY_LOCAL_MACHINE, lpSubKey:=p_strSubKey, Reserved:=0, lpClass:="", dwOptions:=REG_OPTION_NON_VOLATILE, samDesired:=KEY_WRITE, lpSecurityAttributes:=p_typSecAttrib, phkResult:=p_lngResKey, lpdwDisposition:=p_lngDisposition)
			
			If p_lngRtn = ERROR_SUCCESS Then
				' p_lngDisposition = REG_CREATED_NEW_KEY means
				'   that a new key was created --
				' p_lngDisposition = REG_OPENED_EXISTING_KEY
				'   means that we opened an existing key.
				' We don't care here, but it could matter in
				'   other places
				p_lngRtn = RegCloseKey(hKey:=p_lngResKey)
			Else
				Return result
			End If
			
			' Open the key so we can set 4 values
			p_lngRtn = RegOpenKeyEx(hKey:=HKEY_LOCAL_MACHINE, lpSubKey:=p_strSubKey, ulOptions:=0, samDesired:=KEY_WRITE, phkResult:=p_lngKeyHandle)
			
			If p_lngRtn <> ERROR_SUCCESS Then
				Return result
			End If
			
			' Set the FIRST value, EventMessageFile
			p_strValueName = "EventMessageFile"
			
			If Not xi_strFilePath.Trim().EndsWith("\") Then
				xi_strFilePath = xi_strFilePath.Trim() & "\"
			End If
			
			p_strValData = xi_strFilePath & xi_strMessageFile
			p_lngLenData = p_strValData.Length
			
			Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(p_strValData)
			Try 
				p_lngRtn = RegSetValueEx(hKey:=p_lngKeyHandle, lpValueName:=p_strValueName, Reserved:=0, dwType:=REG_EXPAND_SZ, lpData:=tmpPtr, cbData:=p_lngLenData)
				p_strValData = Marshal.PtrToStringAnsi(tmpPtr)
			Finally 
				Marshal.FreeHGlobal(tmpPtr)
			End Try
			
			If p_lngRtn <> ERROR_SUCCESS Then
				Return result
			End If
			
			' Set the SECOND & THIRD values, CategoryMessageFile and CatetoryCount
			p_strValueName = "CategoryMessageFile"
			
			If Not xi_strFilePath.Trim().EndsWith("\") Then
				xi_strFilePath = xi_strFilePath.Trim() & "\"
			End If
			
			p_strValData = xi_strFilePath & xi_strCategoryFile
			p_lngLenData = p_strValData.Length
			
			Dim tmpPtr2 As IntPtr = Marshal.StringToHGlobalAnsi(p_strValData)
			Try 
				p_lngRtn = RegSetValueEx(hKey:=p_lngKeyHandle, lpValueName:=p_strValueName, Reserved:=0, dwType:=REG_EXPAND_SZ, lpData:=tmpPtr2, cbData:=p_lngLenData)
				p_strValData = Marshal.PtrToStringAnsi(tmpPtr2)
			Finally 
				Marshal.FreeHGlobal(tmpPtr2)
			End Try
			
			If p_lngRtn <> ERROR_SUCCESS Then
				Return result
			End If
			
			p_strValueName = "CategoryCount"
			
			Dim handle3 As GCHandle = GCHandle.Alloc(xi_lngNumCategories, GCHandleType.Pinned)
			Try 
				Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()

				p_lngRtn = RegSetValueEx(hKey:=p_lngKeyHandle, lpValueName:=p_strValueName, Reserved:=0, dwType:=REG_DWORD, lpData:=tmpPtr3, cbData:=Marshal.SizeOf(xi_lngNumCategories))
				xi_lngNumCategories = Marshal.ReadInt32(tmpPtr3)
			Finally 
				handle3.Free()
			End Try
			
			If p_lngRtn <> ERROR_SUCCESS Then
				Return result
			End If
			
			' Set the FOURTH value, TypesSupported
			p_strValueName = "TypesSupported"
			
			p_lngPtrValData = EVENTLOG_ERROR_TYPE Or EVENTLOG_WARNING_TYPE Or EVENTLOG_INFORMATION_TYPE
			

			p_lngLenData = Marshal.SizeOf(p_lngPtrValData)
			
			Dim handle4 As GCHandle = GCHandle.Alloc(p_lngPtrValData, GCHandleType.Pinned)
			Try 
				Dim tmpPtr4 As IntPtr = handle4.AddrOfPinnedObject()
				p_lngRtn = RegSetValueEx(hKey:=p_lngKeyHandle, lpValueName:=p_strValueName, Reserved:=0, dwType:=REG_DWORD, lpData:=tmpPtr4, cbData:=p_lngLenData)
				p_lngPtrValData = Marshal.ReadInt32(tmpPtr4)
			Finally 
				handle4.Free()
			End Try
			
			If p_lngRtn <> ERROR_SUCCESS Then
				Return result
			End If
			
			' Always close the key you opened
			p_lngRtn = RegFlushKey(p_lngKeyHandle)
			p_lngRtn = RegCloseKey(p_lngKeyHandle)
			
			If p_lngRtn <> ERROR_SUCCESS Then
				m_lngErrNum = (ERR_NO_CLOSE_KEY + Constants.vbObjectError)
				m_strErrSource = REG_SOURCENAME & "RegisterEventSource"
				m_strErrDesc = "Could not CLOSE the key, " &  _
				               xi_strAppName & ", in the registry under " &  _
				               BASE_DIR & xi_strLogName & ".  The error was: " &  _
				               ReturnApiErrString(p_lngRtn)
				'### Err.Raise m_lngErrNum, m_strErrSource, m_strErrDesc
			End If
			
			' Flush it
			If p_lngSuccess Then
				p_lngRtn = RegFlushKey(p_lngKeyHandle)
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		
	End Function
	
	Private Function ReturnApiErrString(ByRef ErrorCode As Integer) As String
		
		Dim p_strBuffer As String = ""
		Dim p_lngHwndModule, p_lngFlags As Integer
		

		  ' don't accept an error here
			
			' Separate handling for network errors netmsg.dll
			If ErrorCode >= NERR_BASE And ErrorCode <= MAX_NERR Then
				
				p_lngHwndModule = LoadLibraryEx(lpLibFileName:="netmsg.dll", hFile:=0, dwFlags:=LOAD_LIBRARY_AS_DATAFILE)
				
				If p_lngHwndModule <> 0 Then
					
					p_lngFlags = FORMAT_MESSAGE_FROM_SYSTEM Or FORMAT_MESSAGE_IGNORE_INSERTS Or FORMAT_MESSAGE_FROM_HMODULE
					
					' Allocate the string, then get the system to tell us the error
					'     message associated with this error number
					p_strBuffer = New String(Strings.Chr(0), 256)
					
					Dim handle As GCHandle = GCHandle.Alloc(p_lngHwndModule, GCHandleType.Pinned)
					Try 
						Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
						FormatMessage(dwFlags:=p_lngFlags, lpSource:=tmpPtr, dwMessageId:=ErrorCode, dwLanguageId:=0, lpBuffer:=p_strBuffer, nSize:=p_strBuffer.Length, Arguments:=0)
						p_lngHwndModule = Marshal.ReadInt32(tmpPtr)
					Finally 
						handle.Free()
					End Try
					
					' Strip the last null, then the last CrLf pair if it exists
					p_strBuffer = p_strBuffer.Substring(0, p_strBuffer.IndexOf(Strings.Chr(0)))
					If p_strBuffer.Substring(p_strBuffer.Length - 2) = Strings.Chr(13).ToString() & Strings.Chr(10).ToString() Then
						p_strBuffer = p_strBuffer.Substring(0, p_strBuffer.Length - 2)
					End If
					
					FreeLibrary(hLibModule:=p_lngHwndModule)
				End If
				
				' Separate handling for Wininet error Wininet.dll
			ElseIf ErrorCode >= INTERNET_ERROR_BASE And ErrorCode <= INTERNET_ERROR_LAST Then 
				
				' Load the library
				p_lngHwndModule = LoadLibraryEx(lpLibFileName:="Wininet.dll", hFile:=0, dwFlags:=LOAD_LIBRARY_AS_DATAFILE)
				
				If p_lngHwndModule <> 0 Then
					
					p_lngFlags = FORMAT_MESSAGE_FROM_SYSTEM Or FORMAT_MESSAGE_IGNORE_INSERTS Or FORMAT_MESSAGE_FROM_HMODULE
					
					' Allocate the string, then get the system to tell us
					' the error message associated with this error number
					p_strBuffer = New String(Strings.Chr(0), 256)
					Dim handle2 As GCHandle = GCHandle.Alloc(p_lngHwndModule, GCHandleType.Pinned)
					Try 
						Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
						FormatMessage(dwFlags:=p_lngFlags, lpSource:=tmpPtr2, dwMessageId:=ErrorCode, dwLanguageId:=0, lpBuffer:=p_strBuffer, nSize:=p_strBuffer.Length, Arguments:=0)
						p_lngHwndModule = Marshal.ReadInt32(tmpPtr2)
					Finally 
						handle2.Free()
					End Try
					
					' Strip the last null, then the last CrLf pair if it exists
					p_strBuffer = p_strBuffer.Substring(0, p_strBuffer.IndexOf(Strings.Chr(0)))
					If p_strBuffer.Substring(p_strBuffer.Length - 2) = Strings.Chr(13).ToString() & Strings.Chr(10).ToString() Then
						p_strBuffer = p_strBuffer.Substring(0, p_strBuffer.Length - 2)
					End If
					
					FreeLibrary(hLibModule:=p_lngHwndModule)
				End If
				
				' Wasn't Wininet or NetMsg, so do the standard API error look-up
			Else
				' Allocate the string, then get the system to tell us the
				' error message associated with this error number
				p_strBuffer = New String(Strings.Chr(0), 256)
				p_lngFlags = FORMAT_MESSAGE_FROM_SYSTEM Or FORMAT_MESSAGE_IGNORE_INSERTS
				
				Dim handle3 As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
				Try 
					Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()
					FormatMessage(dwFlags:=p_lngFlags, lpSource:=tmpPtr3, dwMessageId:=ErrorCode, dwLanguageId:=0, lpBuffer:=p_strBuffer, nSize:=p_strBuffer.Length, Arguments:=0)
				Finally 
					handle3.Free()
				End Try
				
				' Strip the last null, then the last CrLf pair if it exists
				p_strBuffer = p_strBuffer.Substring(0, p_strBuffer.IndexOf(Strings.Chr(0)))
				If p_strBuffer.Substring(p_strBuffer.Length - 2) = Strings.Chr(13).ToString() & Strings.Chr(10).ToString() Then
					p_strBuffer = p_strBuffer.Substring(0, p_strBuffer.Length - 2)
				End If
			End If
			
			' Set the return value
			Return p_strBuffer
		
		
	End Function
End Module

