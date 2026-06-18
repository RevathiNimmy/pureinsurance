Attribute VB_Name = "MDLLFileSystem"
' Class:    File system functions
' Shared:   Yes (RESTRICTED)
' Needs:    MDLLString
'
' THIS CODE IMPLEMENTS CORRESPONDING FUNCTIONS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE CHANGING THE INTERNALS OF THE DLL.
'
Option Explicit

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Public Enumerations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' Cannot use a real enumeration otherwise the DLL would not compile!
Public Const CSIDL_PROFILE = &H28&                      ' %userprofile%
Public Const CSIDL_APPDATA = &H1A&                      ' %userprofile%\Application Data (ROAMING)
Public Const CSIDL_COOKIES = &H21&                      ' %userprofile%\Cookies
Public Const CSIDL_DESKTOP = &H0&                       ' %userprofile%\Desktop
Public Const CSIDL_DESKTOPDIRECTORY = &H10&             ' %userprofile%\Desktop
Public Const CSIDL_FAVORITES = &H6&                     ' %userprofile%\Favorites
Public Const CSIDL_LOCAL_APPDATA = &H1C&                ' %userprofile%\Local Settings\Application Data (NON_ROAMING)
Public Const CSIDL_CDBURN_AREA = &H3B&                  ' %userprofile%\Local Settings\Application Data\Microsoft\CD Burning
Public Const CSIDL_HISTORY = &H22&                      ' %userprofile%\Local Settings\History
Public Const CSIDL_INTERNET_CACHE = &H20&               ' %userprofile%\Local Settings\Temporary Internet Files
Public Const CSIDL_PERSONAL = &H5&                      ' %userprofile%\My Documents
Public Const CSIDL_MYMUSIC = &HD&                       ' %userprofile%\My Documents\My Music
Public Const CSIDL_MYPICTURES = &H27&                   ' %userprofile%\My Documents\My Pictures
Public Const CSIDL_NETHOOD = &H13&                      ' %userprofile%\NetHood
Public Const CSIDL_PRINTHOOD = &H1B&                    ' %userprofile%\PrintHood
Public Const CSIDL_RECENT = &H8&                        ' %userprofile%\Recent
Public Const CSIDL_SENDTO = &H9&                        ' %userprofile%\SendTo
Public Const CSIDL_STARTMENU = &HB&                     ' %userprofile%\Start Menu
Public Const CSIDL_PROGRAMS = &H2&                      ' %userprofile%\Start Menu\Programs
Public Const CSIDL_ADMINTOOLS = &H30&                   ' %userprofile%\Start Menu\Programs\Administrative Tools
Public Const CSIDL_STARTUP = &H7&                       ' %userprofile%\Start Menu\Programs\Startup
Public Const CSIDL_TEMPLATES = &H15&                    ' %userprofile%\Templates

Public Const CSIDL_COMMON_APPDATA = &H23&               ' %allusersprofile%\Application Data
Public Const CSIDL_COMMON_DESKTOPDIRECTORY = &H19&      ' %allusersprofile%\Desktop
Public Const CSIDL_COMMON_DOCUMENTS = &H2E&             ' %allusersprofile%\Documents
Public Const CSIDL_COMMON_MUSIC = &H35&                 ' %allusersprofile%\Documents\My Music
Public Const CSIDL_COMMON_PICTURES = &H36&              ' %allusersprofile%\Documents\My Pictures
Public Const CSIDL_COMMON_VIDEO = &H37&                 ' %allusersprofile%\Documents\My Videos
Public Const CSIDL_COMMON_FAVORITES = &H1F&             ' %allusersprofile%\Favorites
Public Const CSIDL_COMMON_STARTMENU = &H16&             ' %allusersprofile%\Start Menu
Public Const CSIDL_COMMON_PROGRAMS = &H17&              ' %allusersprofile%\Start Menu\Programs
Public Const CSIDL_COMMON_ADMINTOOLS = &H2F&            ' %allusersprofile%\Start Menu\Programs\Administrative Tools
Public Const CSIDL_COMMON_STARTUP = &H18&               ' %allusersprofile%\Start Menu\Programs\Startup
Public Const CSIDL_COMMON_TEMPLATES = &H2D&             ' %allusersprofile%\Templates

Public Const CSIDL_PROGRAM_FILES = &H26&                ' %programfiles%
Public Const CSIDL_PROGRAM_FILES_COMMON = &H2B&         ' %programfiles%\Common Files

Public Const CSIDL_WINDOWS = &H24&                      ' %windir%
Public Const CSIDL_FONTS = &H14&                        ' %windir%\Fonts
Public Const CSIDL_RESOURCES = &H38&                    ' %windir%\resources
Public Const CSIDL_RESOURCES_LOCALIZED = &H39&          ' %windir%\resources\<langID>
Public Const CSIDL_SYSTEM = &H25&                       ' %windir%\system32

Public Const CSIDL_ALTSTARTUP = &H1D&                   ' non localized startup
Public Const CSIDL_BITBUCKET = &HA&                     ' <desktop>\Recycle Bin
Public Const CSIDL_COMMON_ALTSTARTUP = &H1E&            ' non localized common startup
Public Const CSIDL_COMMON_OEM_LINKS = &H3A&             ' Links to All Users OEM specific apps
Public Const CSIDL_COMPUTERSNEARME = &H3D&              ' Computers Near Me (computed from Workgroup membership)
Public Const CSIDL_CONNECTIONS = &H31&                  ' Network and Dial-up Connections
Public Const CSIDL_CONTROLS = &H3&                      ' My Computer\Control Panel
Public Const CSIDL_DRIVES = &H11&                       ' My Computer
Public Const CSIDL_INTERNET = &H1&                      ' Internet Explorer (icon on desktop)
Public Const CSIDL_MYDOCUMENTS = &HC&                   ' logical "My Documents" desktop icon
Public Const CSIDL_MYVIDEO = &HE&                       ' "My Videos" folder
Public Const CSIDL_NETWORK = &H12&                      ' Network Neighborhood (My Network Places)
Public Const CSIDL_PRINTERS = &H4&                      ' My Computer\Printers
Public Const CSIDL_PROGRAM_FILES_COMMONX86 = &H2C&      ' x86 Program Files\Common on RISC
Public Const CSIDL_PROGRAM_FILESX86 = &H2A&             ' x86 Program Files on RISC
Public Const CSIDL_SYSTEMX86 = &H29&                    ' x86 system directory on RISC

Public Const CSIDL_FLAG_CREATE = &H8000&                ' combine with CSIDL_ value to force folder creation in SHGetFolderPath()
Public Const CSIDL_FLAG_DONT_VERIFY = &H4000&           ' combine with CSIDL_ value to return an unverified folder path
Public Const CSIDL_FLAG_NO_ALIAS = &H1000&              ' combine with CSIDL_ value to insure non-alias versions of the pidl
Public Const CSIDL_FLAG_PER_USER_INIT = &H800&          ' combine with CSIDL_ value to indicate per-user init (eg. upgrade)
Public Const CSIDL_FLAG_MASK = &HFF00&                  ' mask for all possible flag values

Public Const SHGFP_TYPE_CURRENT = 0 ' current value for user, verify it exists
Public Const SHGFP_TYPE_DEFAULT = 1 ' default value, may not exist

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Private
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' API Declarations
Private Const MAX_PATH = 260

Private Const S_OK = 0
'Private Const S_FALSE = 1
'Private Const E_UNEXPECTED = &H8000FFFF
'Private Const E_NOTIMPL = &H80004001
'Private Const E_OUTOFMEMORY = &H8007000E
'Private Const E_INVALIDARG = &H80070057
'Private Const E_NOINTERFACE = &H80004002
'Private Const E_POINTER = &H80004003
'Private Const E_HANDLE = &H80070006
'Private Const E_ABORT = &H80004004
'Private Const E_FAIL = &H80004005
'Private Const E_ACCESSDENIED = &H80070005

' API functions.
Private Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" _
    (ByVal lpszSysPath As String, ByVal cbSysPath As Integer) As Integer
Private Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" _
    (ByVal lpszSysPath As String, ByVal cbSysPath As Integer) As Integer
Private Declare Function GetTempPath Lib "kernel32" Alias "GetTempPathA" _
    (ByVal nBufferLength As Long, ByVal lpBuffer As String) As Long
Private Declare Function GetTempFileName Lib "kernel32" Alias "GetTempFileNameA" _
    (ByVal lpszPath As String, ByVal lpPrefixString As String, ByVal uUnique As Long, ByVal lpTempFileName As String) As Long
Private Declare Function SHGetFolderPath Lib "shell32.dll" Alias "SHGetFolderPathA" _
    (ByVal hWndOwner As Long, ByVal nFolder As Long, ByVal hToken As Long, ByVal dwFlags As Long, ByVal pszPath As String) As Long

' Fast check for the existence of a file.
Public Function FileExists(ByVal sFile As String) As Boolean

    Dim nAttributes As Integer
    Dim bFolder As Boolean
    Dim bVolumeLabel As Boolean
    Dim bAlias As Boolean

    On Error Resume Next
    nAttributes = GetAttr(sFile)
    If Err = 0 Then
        ' Return True only if it's actually a file.
        bFolder = (nAttributes And vbDirectory) <> 0
        bVolumeLabel = (nAttributes And vbVolume) <> 0
        bAlias = (nAttributes And vbAlias) <> 0
        FileExists = Not bFolder And Not bVolumeLabel And Not bAlias
    Else
        FileExists = False
    End If
    On Error GoTo 0

End Function

' Fast check for the existence of a folder.
Public Function FolderExists(ByVal sFolder As String) As Boolean

    Dim nAttributes As Integer
    Dim bFolder As Boolean
    Dim bVolumeLabel As Boolean
    Dim bAlias As Boolean

    On Error Resume Next
    nAttributes = GetAttr(sFolder)
    If Err = 0 Then
        ' Return True only if it's actually a folder.
        bFolder = (nAttributes And vbDirectory) <> 0
        bVolumeLabel = (nAttributes And vbVolume) <> 0
        bAlias = (nAttributes And vbAlias) <> 0
        FolderExists = bFolder And Not bVolumeLabel And Not bAlias
    Else
        FolderExists = False
    End If
    On Error GoTo 0

End Function

Public Function FolderWindows() As String

    Const knReturnLength = 255
    Dim sReturnValue As String
    Dim nBytesCopied As Integer

    sReturnValue = String$(knReturnLength, 0)
    nBytesCopied = GetWindowsDirectory(sReturnValue, knReturnLength)
    If nBytesCopied > 0 Then
        sReturnValue = Left$(sReturnValue, nBytesCopied)
    Else
        sReturnValue = ""
    End If
    FolderWindows = Trim$(sReturnValue)

End Function

Public Function FolderWindowsSystem() As String

    Const knReturnLength = 255
    Dim sReturnValue As String
    Dim nBytesCopied As Integer

    sReturnValue = String$(knReturnLength, 0)
    nBytesCopied = GetSystemDirectory(sReturnValue, knReturnLength)
    If nBytesCopied > 0 Then
        sReturnValue = Left$(sReturnValue, nBytesCopied)
    Else
        sReturnValue = ""
    End If
    FolderWindowsSystem = Trim$(sReturnValue)

End Function

Public Function FolderTemporary() As String

    Const knReturnLength = 255
    Dim sReturnValue As String
    Dim nBytesCopied As Integer

    sReturnValue = String$(knReturnLength, 0)
    nBytesCopied = GetTempPath(knReturnLength, sReturnValue)
    If nBytesCopied > 0 Then
        sReturnValue = Left$(sReturnValue, nBytesCopied)
    Else
        sReturnValue = ""
    End If
    FolderTemporary = RemoveSlash(Trim$(sReturnValue))

End Function

Public Function GetNewTempFileName(ByVal sPrefix As String) As String

    Const knLenBuffer = 255
    Dim sBuffer As String
    Dim sFile As String
    Dim sPath As String
    Dim nBytesCopied As Long
    Dim uUnique As Long

    sBuffer = String$(knLenBuffer, 0)
    nBytesCopied = GetTempPath(knLenBuffer, sBuffer)
    If nBytesCopied > 0 Then
        sPath = Left$(sBuffer, nBytesCopied)
    Else
        sPath = "."
    End If

    uUnique = GetTempFileName(sPath, sPrefix, 0, sBuffer)
    sFile = Trim$(RemoveTZ(sBuffer))

    GetNewTempFileName = sFile

End Function

Public Function FolderSpecial(ByVal nFolder As Long, _
    Optional ByVal nFlags As Long = SHGFP_TYPE_CURRENT) As String

    Dim sPath As String * MAX_PATH
    Dim hResult As Long

    hResult = SHGetFolderPath(0, nFolder, 0, nFlags, sPath)

    If hResult = S_OK Then
        FolderSpecial = Trim$(RemoveTZ(sPath))
    End If

End Function

' Ensures that a complete path spec exists on disk, creating individual directories as required.
Public Sub CreateWholePath(ByVal sPath As String)

    Dim sPathAlreadyExists As String
    Dim sPathNeedsCreating As String
    Dim sFolderToCreate As String

    sPathAlreadyExists = sPath
    sPathNeedsCreating = ""
    Do Until FolderExists(sPathAlreadyExists)
        sPathAlreadyExists = RemoveSlash(ParseSepRev(sPathAlreadyExists, sFolderToCreate, "\", True) & "\")
        If sFolderToCreate <> "" Then
            If sPathNeedsCreating <> "" Then
                sPathNeedsCreating = AddSlash(sFolderToCreate) & sPathNeedsCreating
            Else
                sPathNeedsCreating = sFolderToCreate
            End If
        End If
    Loop

    Do Until sPathNeedsCreating = ""
        sFolderToCreate = ParseSep(sPathNeedsCreating, sPathNeedsCreating, "\")
        sPathAlreadyExists = AddSlash(sPathAlreadyExists) & sFolderToCreate
        MkDir sPathAlreadyExists
    Loop

End Sub
