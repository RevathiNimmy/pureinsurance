Attribute VB_Name = "MDLLExecute"
' Class:    Functions to run applications or open documents
' Shared:   Yes (RESTRICTED)
' Needs:    Nothing
'
' THIS CODE IMPLEMENTS CORRESPONDING FUNCTIONS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE CHANGING THE INTERNALS OF THE DLL.
'
Option Explicit

Private Type STARTUPINFO
    cb As Long
    lpReserved As String
    lpDesktop As String
    lpTitle As String
    dwX As Long
    dwY As Long
    dwXSize As Long
    dwYSize As Long
    dwXCountChars As Long
    dwYCountChars As Long
    dwFillAttribute As Long
    dwFlags As Long
    wShowWindow As Integer
    cbReserved2 As Integer
    lpReserved2 As Long
    hStdInput As Long
    hStdOutput As Long
    hStdError As Long
End Type

Private Type PROCESS_INFORMATION
    hProcess As Long
    hThread As Long
    dwProcessID As Long
    dwThreadID As Long
End Type

Private Type SHELLEXECUTEINFO
    cbSize As Long
    fMask As Long
    hWnd As Long
    lpVerb As String
    lpFile As String
    lpParameters As String
    lpDirectory As String
    nShow As Long
    hInstApp As Long
    ' Optional fields
    lpIDList As Long
    lpClass As String
    hkeyClass As Long
    dwHotKey As Long
    hIcon As Long
    hProcess As Long
End Type

Private Declare Function CreateProcess Lib "kernel32" Alias "CreateProcessA" _
    (ByVal lpApplicationName As Long, _
    ByVal lpszCommandLine As String, _
    ByVal lpProcessAttributes As Long, _
    ByVal lpThreadAttributes As Long, _
    ByVal bInheritHandles As Long, _
    ByVal dwCreationFlags As Long, _
    ByVal lpEnvironment As Long, _
    ByVal lpszCurrentDirectory As String, _
    ByRef lpStartupInfo As STARTUPINFO, _
    ByRef lpProcessInformation As PROCESS_INFORMATION) As Long

Private Declare Function ShellExecuteEx Lib "shell32.dll" _
    (ByRef lpExecInfo As SHELLEXECUTEINFO) As Long

Private Declare Function WaitForSingleObject Lib "kernel32" _
    (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long

Private Declare Function CloseHandle Lib "kernel32" _
    (ByVal hObject As Long) As Long

Private Declare Function FormatMessage Lib "kernel32" Alias "FormatMessageA" _
    (ByVal dwFlags As Long, ByRef lpSource As Any, ByVal dwMessageID As Long, ByVal dwLanguageID As Long, ByVal lpBuffer As String, ByVal nSize As Long, ByRef Arguments As Long) As Long

Private Declare Function LoadLibraryEx Lib "kernel32" Alias "LoadLibraryExA" _
    (ByVal lpLibFileName As String, ByVal hFile As Long, ByVal dwFlags As Long) As Long

Private Declare Function FreeLibrary Lib "kernel32" _
    (ByVal hLibModule As Long) As Long

Private Const CREATE_NEW_CONSOLE = &H10&
Private Const CREATE_SEPARATE_WOW_VDM = &H800&
Private Const CREATE_SHARED_WOW_VDM = &H1000&
Private Const INFINITE = &HFFFFFFFF
Private Const NORMAL_PRIORITY_CLASS = &H20&
Private Const SEE_MASK_FLAG_DDEWAIT = &H100&
Private Const SEE_MASK_NOCLOSEPROCESS = &H40&
Private Const STARTF_USESHOWWINDOW = &H1&
Private Const LOAD_LIBRARY_AS_DATAFILE = 2&

Private Const FORMAT_MESSAGE_ALLOCATE_BUFFER = &H100
Private Const FORMAT_MESSAGE_ARGUMENT_ARRAY = &H2000
Private Const FORMAT_MESSAGE_FROM_HMODULE = &H800
Private Const FORMAT_MESSAGE_FROM_STRING = &H400
Private Const FORMAT_MESSAGE_FROM_SYSTEM = &H1000
Private Const FORMAT_MESSAGE_IGNORE_INSERTS = &H200
Private Const FORMAT_MESSAGE_MAX_WIDTH_MASK = &HFF

Private Const NETWORK_ERROR_BASE = 2100&
Private Const NETWORK_ERROR_LAST = NETWORK_ERROR_BASE + 899&
Private Const INTERNET_ERROR_BASE = 12000&
Private Const INTERNET_ERROR_LAST = INTERNET_ERROR_BASE + 171&

' NB: These constants are not actually used anywhere, but
' including this lot here is good documentation should we
' require them later on.
'                                       'New Win State  Active Win  VBA Constant
'                                       '-------------  ----------  ------------
Private Const SW_HIDE = 0               'hidden         new         vbHide
Private Const SW_NORMAL = 1             'normal         new         vbNormalFocus
Private Const SW_RESTORE = 9            'normal         new
Private Const SW_MINIMIZE = 6           'minimized      top-level   vbMinimizedNoFocus
Private Const SW_MAXIMIZE = 3           'maximized      new         vbMaximizedFocus
Private Const SW_SHOW = 5               'show           new
Private Const SW_SHOWNORMAL = 1         'normal         new         vbNormalFocus
Private Const SW_SHOWMINIMIZED = 2      'minimized      new         vbMinimizedFocus
Private Const SW_SHOWMAXIMIZED = 3      'maximized      new         vbMaximizedFocus
Private Const SW_SHOWNOACTIVATE = 4     'normal         previous    vbNormalNoFocus
Private Const SW_SHOWMINNOACTIVE = 7    'minimized      previous
Private Const SW_SHOWNA = 8             'normal         previous

Public Sub StartExecutable(ByVal sCommandLine As String, _
    Optional ByVal nWindowStyle As VBA.VbAppWinStyle = vbNormalFocus, _
    Optional ByVal bWaitUntilFinished As Boolean = False, _
    Optional ByVal sWorkingFolder As String = "", _
    Optional ByVal bNewConsole As Boolean = False, _
    Optional ByVal bForceSeparateVDM As Boolean = False, _
    Optional ByVal bForceSharedVDM As Boolean = False)

    Dim dwCreationFlags As Long
    Dim lpStartupInfo As STARTUPINFO
    Dim lpProcessInformation As PROCESS_INFORMATION
    Dim bSuccess As Long

    ' Initialize required variables.
    With lpStartupInfo
        .cb = Len(lpStartupInfo)
        .dwFlags = STARTF_USESHOWWINDOW
        .wShowWindow = nWindowStyle
    End With
    dwCreationFlags = NORMAL_PRIORITY_CLASS Or _
        IIf(bNewConsole, CREATE_NEW_CONSOLE, 0) Or _
        IIf(bForceSeparateVDM, CREATE_SEPARATE_WOW_VDM, 0) Or _
        IIf(bForceSharedVDM, CREATE_SHARED_WOW_VDM, 0)

    ' Create the process.
    If sWorkingFolder = "" Then
        bSuccess = CreateProcess(0, _
            sCommandLine, _
            0, _
            0, _
            1, _
            dwCreationFlags, _
            0, _
            vbNullString, _
            lpStartupInfo, _
            lpProcessInformation)
    Else
        bSuccess = CreateProcess(0, _
            sCommandLine, _
            0, _
            0, _
            1, _
            dwCreationFlags, _
            0, _
            sWorkingFolder, _
            lpStartupInfo, _
            lpProcessInformation)
    End If
    If bSuccess = 0 Then
        RaiseWindowsError Err.LastDllError
    End If

    If bWaitUntilFinished Then
        ' Wait for the process to finish.
        bSuccess = WaitForSingleObject(lpProcessInformation.hProcess, INFINITE)
        If bSuccess = 0 Then
            RaiseWindowsError Err.LastDllError
        End If
    End If

    ' Close the process handle.
    bSuccess = CloseHandle(lpProcessInformation.hProcess)

End Sub

Public Sub OpenDocument(ByVal sFileSpec As String, _
    Optional ByVal sAction As String = "", _
    Optional ByVal nWindowStyle As VBA.VbAppWinStyle = vbNormalFocus, _
    Optional ByVal bWaitUntilFinished As Boolean = False, _
    Optional ByVal sWorkingFolder As String = "", _
    Optional ByVal sParameters As String = "")

    Dim lpExecInfo As SHELLEXECUTEINFO
    Dim bSuccess As Long

    ' Initialize required variables.
    With lpExecInfo
        .cbSize = Len(lpExecInfo)
        .fMask = SEE_MASK_NOCLOSEPROCESS Or _
            IIf(bWaitUntilFinished, SEE_MASK_FLAG_DDEWAIT, 0)
        If sAction = "" Then
            .lpVerb = vbNullString
        Else
            .lpVerb = sAction
        End If
        .lpFile = sFileSpec
        .lpParameters = sParameters
        .lpDirectory = sWorkingFolder
        .nShow = nWindowStyle
    End With

    ' Open the document.
    bSuccess = ShellExecuteEx(lpExecInfo)
    If bSuccess = 0 Then
        RaiseWindowsError Err.LastDllError
    End If

    If bWaitUntilFinished Then
        ' Wait for the process to finish.
        bSuccess = WaitForSingleObject(lpExecInfo.hProcess, INFINITE)
        If bSuccess = 0 Then
            RaiseWindowsError Err.LastDllError
        End If
    End If

    ' Close the process handle.
    bSuccess = CloseHandle(lpExecInfo.hProcess)

End Sub

' Takes a Windows error code and raises a VB error from it.
' You can obtain this code from the Err.LastDLLError property.
Public Sub RaiseWindowsError(ByVal nErrorNumber As Long)

    Const knLenDescription = 255

    Dim hLibrary As Long
    Dim nReturn As Long
    Dim sErrorSource As String
    Dim sErrorDescription As String

    ' Safety check first. A valid error number will never be zero.
    If nErrorNumber = 0 Then Exit Sub

    ' Make sure the DLL containing the error description is physically
    ' loaded into memory first. Normal Windows errors are permanently
    ' available and don't need loading. If the load library call fails,
    ' the handle will be zero, and the error will most likely end up being
    ' an "Unrecognised Error".
    Select Case nErrorNumber
    Case NETWORK_ERROR_BASE To NETWORK_ERROR_LAST
        hLibrary = LoadLibraryEx("netmsg.dll", 0&, LOAD_LIBRARY_AS_DATAFILE)
        sErrorSource = "Windows Networking"
    Case INTERNET_ERROR_BASE To INTERNET_ERROR_LAST
        hLibrary = LoadLibraryEx("Wininet.dll", 0&, LOAD_LIBRARY_AS_DATAFILE)
        sErrorSource = "Windows Internet"
    Case Else
        hLibrary = 0
        sErrorSource = "Windows"
    End Select

    ' Get the error message text.
    sErrorDescription = String$(knLenDescription, 0)
    nReturn = FormatMessage( _
        FORMAT_MESSAGE_FROM_SYSTEM Or FORMAT_MESSAGE_FROM_HMODULE Or FORMAT_MESSAGE_IGNORE_INSERTS, _
        ByVal hLibrary, nErrorNumber, 0&, sErrorDescription, knLenDescription, ByVal 0&)
    If nReturn <> 0 Then
        sErrorDescription = RemoveChars(RemoveTZ(sErrorDescription), vbCrLf)
    Else
        sErrorDescription = "Error 0x" & Hex$(nErrorNumber) & " - no description available."
    End If

    ' Unload the DLL if one was loaded.
    If hLibrary <> 0 Then
        FreeLibrary hLibrary
    End If

    ' Translate the error number to the normal VB equivalent if there is one.
    ' This enables error handlers to check for only one error number for both
    ' VB and API function calls.
    Select Case nErrorNumber
    Case 1 ' ERROR_INVALID_FUNCTION
        nErrorNumber = 5
    Case 2 ' ERROR_FILE_NOT_FOUND
        nErrorNumber = 53
    Case 3 ' ERROR_PATH_NOT_FOUND
        nErrorNumber = 76
    Case 4 ' ERROR_TOO_MANY_OPEN_FILES
        nErrorNumber = 67
    Case 5 ' ERROR_ACCESS_DENIED
        nErrorNumber = 70
    Case 8 ' ERROR_NOT_ENOUGH_MEMORY
        nErrorNumber = 7
    Case 14 ' ERROR_OUTOFMEMORY
        nErrorNumber = 7
    Case 17 ' ERROR_NOT_SAME_DEVICE
        nErrorNumber = 74
    Case 20 ' ERROR_BAD_UNIT
        nErrorNumber = 68
    Case 21 ' ERROR_NOT_READY
        nErrorNumber = 71
    Case 38 ' ERROR_HANDLE_EOF
        nErrorNumber = 62
    Case 39 ' ERROR_HANDLE_DISK_FULL
        nErrorNumber = 61
    Case 80 ' ERROR_FILE_EXISTS
        nErrorNumber = 58
    Case 112 ' ERROR_DISK_FULL
        nErrorNumber = 61
    Case Else
        ' Set the "user-defined error" flag bits to make sure
        ' it doesn't clash with valid VB error numbers.
        nErrorNumber = nErrorNumber Or vbObjectError
    End Select

    ' Raise the error.
    Err.Raise nErrorNumber, sErrorSource, sErrorDescription

End Sub
