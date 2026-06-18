Attribute VB_Name = "MWorkstationInfo"
' Module:   Workstation information functions
' Shared:   Yes
' Needs:    gSWLibrary
'
Option Explicit

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Public Declarations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Public Enum EWindowsPlatforms
    knPlatformWin32s  ' VER_PLATFORM_WIN32s = 0
    knPlatformWin9x   ' VER_PLATFORM_WIN32_WINDOWS = 1
    knPlatformWinNT   ' VER_PLATFORM_WIN32_NT = 2
End Enum

Public Type TWindowsVersion
    Platform As EWindowsPlatforms
    Major As Long
    Minor As Long
    Build As Long
    Text As String
    ServicePack As String
End Type

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Private Declarations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Private Const WTS_CURRENT_SERVER_HANDLE = 0
Private Const WTS_CURRENT_SESSION_HANDLE = -1

Private Const RETURN_FAIL = 0

Private Const MAX_COMPUTERNAME_LENGTH = 31
Private Const UNLEN = 256

Private Enum EXTENDED_NAME_FORMAT
    NameUnknown = 0
    NameFullyQualifiedDN = 1
    NameSamCompatible = 2
    NameDisplay = 3
    NameUniqueId = 6
    NameCanonical = 7
    NameUserPrincipal = 8
    NameCanonicalEx = 9
    NameServicePrincipal = 10
End Enum

Private Enum WTS_INFO_CLASS
    WTSInitialProgram
    WTSApplicationName
    WTSWorkingDirectory
    WTSOEMId
    WTSSessionID
    WTSUserName
    WTSWinStationName
    WTSDomainName
    WTSConnectState
    WTSClientBuildNumber
    WTSClientName
    WTSClientDirectory
    WTSClientProductId
    WTSClientHardwareId
    WTSClientAddress
    WTSClientDisplay
    WTSClientProtocolType
End Enum

Private Type OSVERSIONINFO
    dwOSVersionInfoSize As Long
    dwMajorVersion As Long
    dwMinorVersion As Long
    dwBuildNumber As Long
    dwPlatformId As Long
    szCSDVersion As String * 128
End Type

Private Declare Function GetVersionEx Lib "kernel32.dll" Alias "GetVersionExA" _
    (ByRef lpOSVersionInfo As OSVERSIONINFO) As Long
Private Declare Function GetComputerName Lib "kernel32" Alias "GetComputerNameA" _
    (ByVal lpBuffer As String, nSize As Long) As Long
Private Declare Function GetUserName Lib "advapi32.dll" Alias "GetUserNameA" _
    (ByVal lpBuffer As String, nSize As Long) As Long
Private Declare Function GetUserNameEx Lib "secur32.dll" Alias "GetUserNameExA" _
    (ByVal lpType As EXTENDED_NAME_FORMAT, ByVal lpName As String, ByRef lpwLength As Long) As Long

Private Declare Function WTSQuerySessionInformation Lib "wtsapi32.dll" Alias "WTSQuerySessionInformationA" _
    (ByVal hServer As Long, ByVal hSessionID As Long, ByVal lWSI As WTS_INFO_CLASS, ByRef lptBuffer As Long, ByRef lBytes As Long) As Long

Private Declare Sub WTSFreeMemory Lib "wtsapi32.dll" (ByVal pMemory As Long)

Private Declare Sub CopyMemoryFromAddress Lib "kernel32" Alias "RtlMoveMemory" _
   (ByRef lpDestination As Any, _
    ByVal lplpSource As Long, _
    ByVal length As Long)

' Returns Windows version information.
Public Function GetWindowsVersion() As TWindowsVersion

    Dim lpOSVersionInfo As OSVERSIONINFO

    lpOSVersionInfo.dwOSVersionInfoSize = Len(lpOSVersionInfo)
    GetVersionEx lpOSVersionInfo
    GetWindowsVersion.Platform = lpOSVersionInfo.dwPlatformId
    GetWindowsVersion.Major = lpOSVersionInfo.dwMajorVersion
    GetWindowsVersion.Minor = lpOSVersionInfo.dwMinorVersion
    GetWindowsVersion.Build = lpOSVersionInfo.dwBuildNumber
    If GetWindowsVersion.Platform = knPlatformWin9x Then
        GetWindowsVersion.Build = GetWindowsVersion.Build And &HFFFF&
    End If
    GetWindowsVersion.Text = _
        GetWindowsVersion.Major & "." & _
        GetWindowsVersion.Minor & "." & _
        GetWindowsVersion.Build
    GetWindowsVersion.ServicePack = Trim$(RemoveTZ(lpOSVersionInfo.szCSDVersion))

End Function

' Returns the Windows Computer Name. This function guarantees
' that the returned name will be at most 31 characters long.
Public Function GetWindowsComputerName() As String

    Dim bSuccess As Long
    Dim sComputerName As String

    GetWindowsComputerName = ""

    sComputerName = String$(MAX_COMPUTERNAME_LENGTH + 1, 0)
    bSuccess = GetComputerName(sComputerName, MAX_COMPUTERNAME_LENGTH + 1)
    If bSuccess Then
        ' Truncate the string to the correct length just in case
        ' the API function does something unexpected.
        GetWindowsComputerName = Left$(RemoveTZ(sComputerName), 31)
    End If

End Function

' Returns the Windows User Name. This function guarantees
' that the returned name will be at most 255 characters long.
Public Function GetWindowsUserName() As String

    Dim bSuccess As Long
    Dim sUserName As String

    GetWindowsUserName = ""

    sUserName = String$(UNLEN + 1, 0)
    bSuccess = GetUserName(sUserName, UNLEN + 1)
    If bSuccess Then
        ' Although the API documentation specifies 256 chars as
        ' the maximum, the most practical size for storing in the
        ' database is 255. We therefore truncate the string to
        ' this length as a safety measure.
        GetWindowsUserName = Left$(RemoveTZ(sUserName), 255)
    End If

End Function

' Returns the fully-qualified Domain User Name. The maximum length
' returned does not appear to be documented in MSDN.
' Note that this function requires secur32.dll to exist on the machine.
' This is *normally* only included with WinNT 5.0 or above, but may exist
' on older versions as part of service pack installs etc. Regardless,
' if the DLL cannot be found or the API function does not exist in it,
' this function silently returns an empty string and does not error.
Public Function GetDomainUserName() As String

    Dim bSuccess As Long
    Dim sUserName As String
    Dim nSize As Long

    On Error GoTo EH_Handler
    GetDomainUserName = ""

    GetUserNameEx NameSamCompatible, "", nSize
    sUserName = String$(nSize, 0)
    bSuccess = GetUserNameEx(NameSamCompatible, sUserName, nSize)
    If bSuccess Then
        GetDomainUserName = RemoveTZ(sUserName)
    End If

EH_Handler:
    ' This works because VB6 does not discover whether the API
    ' function exists or not until the first time it's called.

End Function

' Returns the ID of the current Terminal Services session, or zero
' if it is not running.
' Note that this function requires wtsapi32.dll to exist on the machine.
' This is *normally* only included with WinNT 5.0 or above, but may exist
' on older versions as part of service pack installs etc. Regardless,
' if the DLL cannot be found or the API function does not exist in it,
' this function silently returns zero and does not error.
Public Function GetTerminalServicesSessionID() As Long

    Dim nLength As Long
    Dim lpBuffer As Long
    Dim lSessionID As Long
    Dim nReturn As Long

    On Error GoTo EH_Handler
    GetTerminalServicesSessionID = 0

    nReturn = WTSQuerySessionInformation( _
        WTS_CURRENT_SERVER_HANDLE, _
        WTS_CURRENT_SESSION_HANDLE, _
        WTSSessionID, _
        lpBuffer, _
        nLength)

    If nReturn <> RETURN_FAIL Then
        CopyMemoryFromAddress lSessionID, lpBuffer, nLength
        WTSFreeMemory lpBuffer
        GetTerminalServicesSessionID = lSessionID
    End If

EH_Handler:
    ' This works because VB6 does not discover whether the API
    ' function exists or not until the first time it's called.

End Function
