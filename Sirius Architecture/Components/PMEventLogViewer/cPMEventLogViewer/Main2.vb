Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Module modMain
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' Used to get error messages directly from the
	'    system instead of hard-coding them
	Private Const FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000
	Private Const FORMAT_MESSAGE_IGNORE_INSERTS As Integer = &H200
	Private Const FORMAT_MESSAGE_FROM_HMODULE As Integer = &H800
	Private Const LOAD_LIBRARY_AS_DATAFILE As Integer = 2
	Private Const MAX_PATH As Integer = 260
	
	' Custom error messages for this app
	Public Const ERR_REG_EVENT_SOURCE As Integer = 9001
	Public Const ERR_REPORT_EVENT As Integer = 9002
	Public Const ERR_DEREG_EVENT_SOURCE As Integer = 9003
	Public Const ERR_NO_CREATE_KEY As Integer = 9004
	Public Const ERR_NO_OPEN_KEY As Integer = 9005
	Public Const ERR_NO_SET_FIRST_VALUE As Integer = 9006
	Public Const ERR_NO_SET_SECOND_VALUE As Integer = 9007
	Public Const ERR_NO_CLOSE_KEY As Integer = 9008
	
	' Status Codes
	Private Const INVALID_HANDLE_VALUE As Integer = -1
	Public Const ERROR_SUCCESS As Integer = 0
	
	' Upper and lower bounds of network errors
	Private Const NERR_BASE As Integer = 2100
	Private Const MAX_NERR As Integer = NERR_BASE + 899
	
	' Upper and lower bounds of Internet errors
	Private Const INTERNET_ERROR_BASE As Integer = 12000
	Private Const INTERNET_ERROR_LAST As Integer = INTERNET_ERROR_BASE + 171
	
	Private Declare Function GetShortPathName Lib "Kernel32"  Alias "GetShortPathNameA"(ByVal lpszLongPath As String, ByVal lpszShortPath As String, ByVal nBufferLength As Integer) As Integer
	
	Private Declare Function FormatMessage Lib "Kernel32"  Alias "FormatMessageA"(ByVal dwFlags As Integer, ByVal lpSource As Integer, ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByVal lpBuffer As String, ByVal nSize As Integer, ByRef Arguments As Integer) As Integer
	
	Private Declare Function LoadLibraryEx Lib "Kernel32"  Alias "LoadLibraryExA"(ByVal lpLibFileName As String, ByVal hFile As Integer, ByVal dwFlags As Integer) As Integer
	
	Private Declare Function FreeLibrary Lib "Kernel32" (ByVal hLibModule As Integer) As Integer
	
	Private Declare Function MultiByteToWideChar Lib "Kernel32" (ByVal CodePage As Integer, ByVal dwFlags As Integer, ByVal lpMultiByteStr As Integer, ByVal cchMultiByte As Integer, ByVal lpWideCharStr As Integer, ByVal cchWideChar As Integer) As Integer
	
	Private Declare Function lstrcpyA Lib "Kernel32" (ByVal lpString1 As Integer, ByVal lpString2 As Integer) As Integer
	
	Private Declare Function lstrlenA Lib "Kernel32" (ByVal lpString As Integer) As Integer
	
	Private Const CP_ACP As Integer = 0 ' ANSI code page
	
	' The types of events that can be logged.
	Public Const EVENTLOG_SUCCESS As Integer = &H0s
	Public Const EVENTLOG_ERROR_TYPE As Integer = &H1s
	Public Const EVENTLOG_WARNING_TYPE As Integer = &H2s
	Public Const EVENTLOG_INFORMATION_TYPE As Integer = &H4s
	Public Const EVENTLOG_AUDIT_SUCCESS As Integer = &H8s
	Public Const EVENTLOG_AUDIT_FAILURE As Integer = &H10s
	
	' Constants for different types of events
	Public Const Event_Type_Info As String = "Information"
	Public Const Event_Type_Warning As String = "Warning"
	Public Const Event_Type_Error As String = "Error"
	Public Const Event_Type_Success_Audit As String = "Success Audit"
	Public Const Event_Type_Failure_Audit As String = "Failure Audit"
	
	' Constants for the type of log you want to look
	'   at.  Note that user program will need to use
	'   these constants also
	Public Const EVNT_SYSTEM As String = "System"
	Public Const EVNT_APP As String = "Application"
	Public Const EVNT_SECURITY As String = "Security"
	
	' Custom error return codes for the various class modules
	Public Const ERR_LOGTYPE_NOT_SET As Integer = 1011
	Public Const ERR_SOURCENAME_NOT_SET As Integer = 1012
	Public Const ERR_BAD_INDEX As Integer = 1013
	Public Const ERR_FAILED_OPEN_REGISTRY_KEY As Integer = 1014
	Public Const ERR_FAILED_READ_REGISTRY_KEY As Integer = 1015
	Public Const ERR_RESOURCE_DATA_NOT_FOUND As Integer = 1016
	Public Const ERR_READING_EVENT_LOG As Integer = 1017
	Public Const ERR_LOG_NOT_OPENED As Integer = 1018
	Public Const ERR_FAILED_SET_LOG_TYPE As Integer = 1019
	Public Const ERR_FAILED_FORMAT_MSG As Integer = 1020
	Public Const ERR_BAD_SERVER_NAME As Integer = 1021
	Public Const ERR_READ_EVENT_RECORD As Integer = 1022
	
	' API calls that need to be public within program
	Public Declare Sub CopyMem Lib "Kernel32"  Alias "RtlCopyMem"(ByVal pTo As Integer, ByVal uFrom As Integer, ByVal lSize As Integer)
	Public Declare Sub MoveMem Lib "Kernel32"  Alias "RtlMoveMemory"(ByVal pTo As Integer, ByVal uFrom As Integer, ByVal lSize As Integer)
	
	' Eventlog Status Codes
	Public Const ERROR_FILE_NOT_FOUND As Integer = 2
	Public Const ERROR_PATH_NOT_FOUND As Integer = 3
	Public Const ERROR_TOO_MANY_OPEN_FILES As Integer = 4
	Public Const ERROR_ACCESS_DENIED As Integer = 5
	Public Const ERROR_INVALID_HANDLE As Integer = 6
	Public Const ERROR_ARENA_TRASHED As Integer = 7
	Public Const ERROR_NOT_ENOUGH_MEMORY As Integer = 8
	Public Const ERROR_OUTOFMEMORY As Integer = 14
	Public Const ERROR_HANDLE_EOF As Integer = 38
	Public Const ERROR_INVALID_PARAMETER As Integer = 87
	Public Const ERROR_INSUFFICIENT_BUFFER As Integer = 122
	Public Const ERROR_INVALID_NAME As Integer = 123
	Public Const ERROR_ALREADY_EXISTS As Integer = 183
	Public Const ERROR_MORE_DATA As Integer = 234
	Public Const ERROR_NO_MORE_ITEMS As Integer = 259
	Public Const ERROR_MR_MID_NOT_FOUND As Integer = 317
	Public Const ERROR_NOACCESS As Integer = 998
	Public Const ERROR_BADDB As Integer = 1009
	Public Const ERROR_BADKEY As Integer = 1010
	Public Const ERROR_CANTOPEN As Integer = 1011
	Public Const ERROR_CANTREAD As Integer = 1012
	Public Const ERROR_CANTWRITE As Integer = 1013
	Public Const ERROR_EVENTLOG_FILE_CORRUPT As Integer = 1500
	Public Const ERROR_EVENTLOG_CANT_START As Integer = 1501
	Public Const ERROR_LOG_FILE_FULL As Integer = 1502
	Public Const ERROR_EVENTLOG_FILE_CHANGED As Integer = 1503
	Public Const RPC_S_SERVER_UNAVAILABLE As Integer = 1722
	Public Const ERROR_RESOURCE_DATA_NOT_FOUND As Integer = 1812
	
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
	
	Private Structure SYSTEMTIME
		Dim wYear As Integer
		Dim wMonth As Integer
		Dim wDayOfWeek As Integer
		Dim wDay As Integer
		Dim wHour As Integer
		Dim wMinute As Integer
		Dim wSecond As Integer
        Dim wMilliseconds As Integer
        Private Function GetDateTime() As DateTime
            Return New DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds)
        End Function
        Public Sub LoadDateTime(ByVal dt As DateTime)
            wYear = dt.Year
            wMonth = dt.Month
            wDayOfWeek = CInt(dt.DayOfWeek)
            wDay = dt.Day
            wHour = dt.Hour
            wMinute = dt.Minute
            wSecond = dt.Second
            wMilliseconds = dt.Millisecond
        End Sub
        Public Sub GetLocalTime()
            LoadDateTime(DateTime.Now)
        End Sub
        Public Sub GetSystemTime()
            LoadDateTime(DateTime.UtcNow)
        End Sub
        Public Function GetFileTime(ByRef ft As FILETIME) As Long
            Dim dt As DateTime = GetDateTime()
            Dim fileTime As Long = dt.ToFileTime()

            ft.dwLowDateTime = CInt(fileTime And &H7FFFFFFF)
            ft.dwHighDateTime = CInt(fileTime >> 32)

            Return fileTime
        End Function
    End Structure
	
	


	Private Declare Function FileTimeToLocalFileTime Lib "Kernel32" (ByRef lpFileTime As FILETIME, ByRef lpLocalFileTime As FILETIME) As Integer
	


	Private Declare Function FileTimeToSystemTime Lib "Kernel32" (ByRef lpFileTime As FILETIME, ByRef lpSystemTime As SYSTEMTIME) As Integer
	

	Private Declare Function VariantTimeToSystemTime Lib "oleaut32" (ByVal vtime As Double, ByRef psystime As SYSTEMTIME) As Integer
	

	Private Declare Function SystemTimeToVariantTime Lib "oleaut32" (ByRef psystime As SYSTEMTIME, ByRef pvtime As Double) As Integer
	
	Public Function LoWord(ByVal lngNumber As Integer) As Integer
		Return lngNumber And &HFFFF
	End Function
	
	Public Function HiWord(ByRef lngNumber As Integer) As Integer
		Return CInt(lngNumber / &H10000)
	End Function
	
	Public Function DateToLocalDate(ByVal xi_dteTimes As Date) As Date
        Dim p_typSystemTime As New SYSTEMTIME
        p_typSystemTime.GetLocalTime()
		Dim p_typFileTime As New FILETIME
		Dim p_typLocalFileTime As New FILETIME
		Dim p_dblReturn As Double
		
        'VariantTimeToSystemTime(xi_dteTimes.ToOADate, p_typSystemTime)
		p_typSystemTime.GetFileTime(p_typFileTime)
		FileTimeToLocalFileTime(p_typFileTime, p_typLocalFileTime)
		FileTimeToSystemTime(p_typLocalFileTime, p_typSystemTime)
		SystemTimeToVariantTime(p_typSystemTime, p_dblReturn)
		Return DateTime.FromOADate(p_dblReturn)
		
	End Function
	
	Public Function GetStrFromPtrA(ByVal xi_lngPtrString As Integer, Optional ByVal xi_lngStrLen As Integer = 0) As String
		
		Dim result As String = String.Empty
		Dim p_lngNumChars As Integer
		
		If xi_lngStrLen = 0 Then
			Dim handle As GCHandle = GCHandle.Alloc(xi_lngPtrString, GCHandleType.Pinned)
			Try 
				Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
				p_lngNumChars = lstrlenA(tmpPtr)
			Finally 
				handle.Free()
			End Try
		Else
			p_lngNumChars = xi_lngStrLen
		End If
		
		result = New String(" "c, p_lngNumChars)
        Dim handle2 As GCHandle = GCHandle.Alloc(xi_lngPtrString, GCHandleType.Pinned)
        'Dim handle3 As GCHandle = GCHandle.Alloc(StrPtr(result), GCHandleType.Pinned)
        Dim handle3 As GCHandle = GCHandle.Alloc(UpgradeStubs.VBA__HiddenModule.StrPtr(result), GCHandleType.Pinned)

		Try 
			Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()

			Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
			MultiByteToWideChar(CP_ACP, 0, tmpPtr2, p_lngNumChars, tmpPtr3, p_lngNumChars)
		Finally 
			handle2.Free()
			handle3.Free()
		End Try
		
		Return result
	End Function
	
	Public Function ClearLastCrLf(ByVal xi_strTmpMsg As String) As String
		
		
		' Make a temporary copy of the passed string
		Dim p_strTmpMsg As String = xi_strTmpMsg.Trim()
		
		' Trim any trailing nulls
		If p_strTmpMsg.Substring(p_strTmpMsg.Length - 1) = Strings.Chr(0).ToString() Then
			p_strTmpMsg = p_strTmpMsg.Substring(0, p_strTmpMsg.Length - 1)
		End If
		
		' Trim any trailing Cr's, Lf's, or CrLf's
		If p_strTmpMsg.Substring(p_strTmpMsg.Length - 2) = Strings.Chr(13) & Strings.Chr(10) Then
			p_strTmpMsg = p_strTmpMsg.Substring(0, p_strTmpMsg.Length - 2)
		ElseIf p_strTmpMsg.Substring(p_strTmpMsg.Length - 1) = Constants.vbLf Then 
			p_strTmpMsg = p_strTmpMsg.Substring(0, p_strTmpMsg.Length - 1)
		ElseIf p_strTmpMsg.Substring(p_strTmpMsg.Length - 1) = Strings.Chr(13) Then 
			p_strTmpMsg = p_strTmpMsg.Substring(0, p_strTmpMsg.Length - 1)
		End If
		
		' Set the return value
		Return p_strTmpMsg
		
	End Function
	
	Public Function GetShortName(ByVal xi_strLongPathName As String) As String
		
		Dim p_strBuffer As String = New String(" "c, MAX_PATH)
		If xi_strLongPathName.EndsWith("\") Then
			xi_strLongPathName = xi_strLongPathName.Substring(0, xi_strLongPathName.Length - 1)
		End If
		
		Dim p_lngRtn As Integer = GetShortPathName(lpszLongPath:=xi_strLongPathName, lpszShortPath:=p_strBuffer, nBufferLength:=p_strBuffer.Length)
		If p_lngRtn > 0 Then
			Return p_strBuffer.Substring(0, Math.Min(p_strBuffer.Length, p_lngRtn)).Trim()
		Else
			Return ""
		End If
	End Function
	
	Public Function ReturnApiErrString(ByRef ErrorCode As Integer) As String
		
		Dim p_strBuffer As String = ""
		Dim p_lngHwndModule, p_lngFlags As Integer
		

		Try  ' Don't accept an error here
			
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
					
					' Allocate the string, then get the system to tell us the error
					' message associated with this error number
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
				
				' Strip the last null, then the last CrLf  pair if it exists
				p_strBuffer = p_strBuffer.Substring(0, p_strBuffer.IndexOf(Strings.Chr(0)))
				If p_strBuffer.Substring(p_strBuffer.Length - 2) = Strings.Chr(13).ToString() & Strings.Chr(10).ToString() Then
					p_strBuffer = p_strBuffer.Substring(0, p_strBuffer.Length - 2)
				End If
			End If
			
			' Set the return value
			Return p_strBuffer
		
		Catch exc As System.Exception

		End Try
		
	End Function
End Module