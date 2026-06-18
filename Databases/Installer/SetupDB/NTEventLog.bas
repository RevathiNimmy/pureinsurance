Attribute VB_Name = "MNTEventLog"
' Module:   Write to the NT Application Event Log
' Shared:   Yes (RESTRICTED)
' Needs:    gSWLibrary
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE PURE DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
' In order for this to work properly, message DLLs have to be provided
' to cover all possible category and event IDs written to the log.
' Pure have two standard DLLs for this purpose which basically do nothing
' except ensure that the log doesn't get cluttered up with error messages
' like "can't find description for this event ID".
'
' The filenames are:
'
'   cPMEventLogCat.dll
'   cPMEventLogMsg.dll
'
' The registry keys that need to be set are:
'
'   [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application\Pure]
'   CategoryCount = REG_DWORD: 00000004
'   CategoryMessageFile = REG_EXPAND_SZ: <path>\cPMEventLogCat.dll
'   EventMessageFile = REG_EXPAND_SZ: <path>\cPMEventLogMsg.dll
'   TypesSupported = REG_DWORD: 00000007
'
Option Explicit

' Pure standard severity levels - to be used as the event log category ID.
Public Enum ESeverityLevels
    knLogFatal = 1
    knLogError = 2
    knLogWarning = 3
    knLogOnError = 4
    knLogInfo = 5
    knLogDebug1 = 6
    knLogDebug2 = 7
    knLogDebug3 = 8
    knLogDebug4 = 9
End Enum

' Hard-coded Pure standard message source used for all events logged.
Private Const ksEventLogSource = "Pure"

' Hard-coded Pure standard event ID to be used for all events logged.
Private Const DEFAULT_EVENT_ID = &H60000001

' Windows API declarations.
Private Const EVENTLOG_ERROR_TYPE = 1
Private Const EVENTLOG_WARNING_TYPE = 2
Private Const EVENTLOG_INFORMATION_TYPE = 4
Private Const EVENTLOG_AUDIT_SUCCESS = 8
Private Const EVENTLOG_AUDIT_FAILURE = 16

Private Const FIRST_ATTEMPT = 16
Private Const TOKEN_QUERY = 8
Private Const HEAP_GENERATE_EXCEPTIONS = 4
Private Const HEAP_ZERO_MEMORY = 8

Private Const VER_PLATFORM_WIN32s = 0
Private Const VER_PLATFORM_WIN32_WINDOWS = 1
Private Const VER_PLATFORM_WIN32_NT = 2

Private Type OSVERSIONINFO
    dwOSVersionInfoSize As Long
    dwMajorVersion As Long
    dwMinorVersion As Long
    dwBuildNumber As Long
    dwPlatformId As Long
    szCSDVersion As String * 128
End Type

Private Enum TOKEN_INFORMATION_CLASS
    TokenUser = 1
    TokenGroups
    TokenPrivileges
    TokenOwner
    TokenPrimaryGroup
    TokenDefaultDacl
    TokenSource
    TokenType
    TokenImpersonationLevel
    TokenStatistics
    TokenRestrictedSids
End Enum

Private Const knStringMaxLength = 32767
Private Const knNumStrings = 1 ' number of elements in TLPCSTRArray
Private Type TLPCSTRArray
    s0 As String
End Type

Private Type SID_AND_ATTRIBUTES
    lpSID As Long
    hAttributes As Long
End Type

Private Type TOKEN_USER
    xSID As SID_AND_ATTRIBUTES
    sPadding As String * 120 ' required due to a bug in GetTokenInformation()
End Type

Private Declare Function GetVersionEx Lib "kernel32" Alias "GetVersionExA" _
    (ByRef lpVersionInformation As OSVERSIONINFO) As Long

Private Declare Function RegisterEventSource Lib "advapi32" Alias "RegisterEventSourceA" _
    (ByVal lpUNCServerName As String, ByVal lpSourceName As String) As Long

Private Declare Function DeregisterEventSource Lib "advapi32" _
    (ByVal hEventLog As Long) As Long

Private Declare Function ReportEvent Lib "advapi32" Alias "ReportEventA" _
    (ByVal hEventLog As Long, ByVal wType As Long, _
    ByVal wCategory As Long, ByVal dwEventID As Long, _
    ByVal lpUserSid As Long, ByVal wNumStrings As Long, _
    ByVal dwDataSize As Long, ByRef lpStrings As Any, _
    ByRef lpRawData As Any) As Long

Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" _
    (ByVal lpTo As Long, ByVal lpFrom As Long, ByVal dwSize As Long)

Private Declare Function GetProcessHeap Lib "kernel32" () As Long

Private Declare Function IsValidSid Lib "advapi32.dll" _
    (ByVal lpSID As Long) As Long

Private Declare Function GetLengthSid Lib "advapi32.dll" _
    (ByVal lpSID As Long) As Long

Private Declare Function HeapAlloc Lib "kernel32" _
    (ByVal hHeap As Long, ByVal dwFlags As Long, ByVal dwBytes As Long) As Long

Private Declare Function HeapFree Lib "kernel32" _
    (ByVal hHeap As Long, ByVal dwFlags As Long, ByVal lpMem As Long) As Long

Private Declare Function CloseHandle Lib "kernel32" _
    (ByVal hObject As Long) As Long

Private Declare Function OpenProcessToken Lib "advapi32" _
    (ByVal hProcess As Long, ByVal dwDesiredAccess As Long, ByRef hToken As Long) As Long

Private Declare Function GetCurrentProcess Lib "kernel32" () As Long

Private Declare Function GetTokenInformation Lib "advapi32.dll" _
    (ByVal hToken As Long, _
    ByVal eTokenInformationClass As TOKEN_INFORMATION_CLASS, _
    ByRef uTokenInformation As Any, _
    ByVal nTokenInformationLength As Long, _
    ByRef nReturnLength As Long) As Long

' Error declarations.
Private Const ksErrSource = "iSWErrorHandler.MNTEventLog"
Private Const knErrBadSID = vbObjectError + 1
Private Const ksErrBadSID = "Cannot read the current process SID."

' Returns True if event logging is available on this machine.
Public Function LoggingAvailable() As Boolean

    Dim xVersionInfo As OSVERSIONINFO

    ' Event logging is available only on Windows NT.
    xVersionInfo.dwOSVersionInfoSize = Len(xVersionInfo)
    GetVersionEx xVersionInfo
    LoggingAvailable = (xVersionInfo.dwPlatformId = VER_PLATFORM_WIN32_NT)

End Function

' Writes an arbitary block of text to the local machine application event log.
' It throws a VB error if anything goes wrong.
Public Sub LogEvent(ByVal nSeverity As ESeverityLevels, ByVal sText As String)

    Dim hEventLog As Long
    Dim nReturn As Long
    Dim xStrings As TLPCSTRArray
    Dim lpSID As Long
    Dim nErrNumber As Long
    Dim sErrSource As String
    Dim sErrDescription As String

    On Error GoTo EH_Handler

    ' Open a handle to the local machine application event log
    ' using our own private message source.
    hEventLog = RegisterEventSource(vbNullString, ksEventLogSource)
    If hEventLog = 0 Then
        RaiseWindowsError Err.LastDllError
    End If

    ' Trim the message text to the maximum allowed length and
    ' force it into the required data structure.
    sText = RTrim$(Left$(sText, knStringMaxLength))
    xStrings.s0 = sText

    ' Get the SID of the current process, as a pointer to an
    ' allocated memory block.
    lpSID = GetSID()

    ' Write an event to the log.
    nReturn = ReportEvent(hEventLog, EVENTLOG_ERROR_TYPE, _
        nSeverity, DEFAULT_EVENT_ID, lpSID, knNumStrings, 0&, xStrings, ByVal 0&)
    If nReturn = 0 Then
        RaiseWindowsError Err.LastDllError
    End If

    ' Deallocate the SID memory block.
    FreeSID lpSID

    ' Close the event log handle.
    nReturn = DeregisterEventSource(hEventLog)
    If nReturn = 0 Then
        RaiseWindowsError Err.LastDllError
    End If

    Exit Sub

EH_Handler:
    ' Release all pointers and handles before aborting.
    nErrNumber = Err.Number
    sErrSource = Err.Source
    sErrDescription = Err.Description
    On Error Resume Next

    If lpSID <> 0 Then
        FreeSID lpSID
    End If
    If hEventLog <> 0 Then
        DeregisterEventSource hEventLog
    End If

    On Error GoTo 0
    Err.Raise nErrNumber, sErrSource, sErrDescription

End Sub

Private Function GetSID() As Long

    Dim nReturn As Long
    Dim hToken As Long
    Dim xTokenUser As TOKEN_USER
    Dim nTokenUserLength As Long
    Dim lpSID As Long
    Dim nSIDLength As Long
    Dim lpSID2 As Long
    Dim nErrNumber As Long
    Dim sErrSource As String
    Dim sErrDescription As String

    On Error GoTo EH_Handler
    GetSID = 0

    ' Open a handle to the process token.
    nReturn = OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY, hToken)
    If nReturn = 0 Then
        RaiseWindowsError Err.LastDllError
    End If

    ' Get a pointer to the SID for this token.
    nReturn = GetTokenInformation(hToken, TokenUser, xTokenUser, Len(xTokenUser), nTokenUserLength)
    If nReturn = 0 Then
        RaiseWindowsError Err.LastDllError
    End If

    ' Validate the SID pointer.
    lpSID = xTokenUser.xSID.lpSID
    If lpSID = 0 Then
        Err.Raise knErrBadSID, ksErrSource, ksErrBadSID
    End If
    nReturn = IsValidSid(lpSID)
    If nReturn = 0 Then
        Err.Raise knErrBadSID, ksErrSource, ksErrBadSID
    End If
    nSIDLength = GetLengthSid(lpSID)

    ' Allocate a block of memory to copy the SID to, because
    ' closing the token handle deallocates the original SID.
    lpSID2 = HeapAlloc(GetProcessHeap(), HEAP_GENERATE_EXCEPTIONS Or HEAP_ZERO_MEMORY, nSIDLength)
    If lpSID2 = 0 Then
        Err.Raise knErrBadSID, ksErrSource, ksErrBadSID
    End If

    ' Copy the SID to the new memory location.
    CopyMemory lpSID2, lpSID, nSIDLength

    ' Close the handle to the process token.
    nReturn = CloseHandle(hToken)
    If nReturn = 0 Then
        RaiseWindowsError Err.LastDllError
    End If

    ' Return the pointer to the memory that is still allocated.
    GetSID = lpSID2
    Exit Function

EH_Handler:
    ' Release all pointers and handles before aborting.
    nErrNumber = Err.Number
    sErrSource = Err.Source
    sErrDescription = Err.Description
    On Error Resume Next

    If hToken <> 0 Then
        CloseHandle hToken
    End If

    On Error GoTo 0
    Err.Raise nErrNumber, sErrSource, sErrDescription

End Function

Private Sub FreeSID(ByVal lpSID As Long)

    Dim nReturn As Long

    ' Free the memory used to store the SID.
    nReturn = HeapFree(GetProcessHeap(), HEAP_GENERATE_EXCEPTIONS, lpSID)
    If nReturn = 0 Then
        RaiseWindowsError Err.LastDllError
    End If

End Sub
