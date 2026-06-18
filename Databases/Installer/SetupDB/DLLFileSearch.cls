VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CFileSearch"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    Supports multiple simultaneous directory searches
' Shared:   Yes (RESTRICTED)
' Needs:    Nothing
'
' THIS CLASS IS A COPY OF THE CORRESPONDING CLASS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE ALSO CHANGING THE DLL.
'
Option Explicit

' Public enumerations.
Public Enum EAttributes
    knAttributeReadOnly = &H1&
    knAttributeHidden = &H2&
    knAttributeSystem = &H4&
    knAttributeVolumeLabel = &H8&
    knAttributeDirectory = &H10&
    knAttributeArchived = &H20&
    knAttributeEncrypted = &H40&
    knAttributeNormal = &H80&
    knAttributeTemporary = &H100&
    knAttributeSparseFile = &H200&
    knAttributeReparsePoint = &H400&
    knAttributeCompressed = &H800&
    knAttributeOffline = &H1000&
    knAttributeNotContentIndexed = &H2000&
End Enum

' Properties.
Private m_bErrorOnFileNotFound As Boolean
Private m_bErrorOnPathNotFound As Boolean

' API declarations.
Private Const ERROR_FILE_NOT_FOUND = 2
Private Const ERROR_PATH_NOT_FOUND = 3
Private Const ERROR_NO_MORE_FILES = 18
Private Const INVALID_HANDLE_VALUE = -1
Private Const MAX_PATH = 260

Private Type FILETIME
    dwLowDateTime As Long
    dwHighDateTime As Long
End Type

Private Type SYSTEMTIME
    wYear As Integer
    wMonth As Integer
    wDayOfWeek As Integer
    wDay As Integer
    wHour As Integer
    wMinute As Integer
    wSecond As Integer
    wMilliseconds As Integer
End Type

Private Type WIN32_FIND_DATA
    dwFileAttributes As Long
    ftCreationTime As FILETIME
    ftLastAccessTime As FILETIME
    ftLastWriteTime As FILETIME
    nFileSizeHigh As Long
    nFileSizeLow As Long
    dwReserved0 As Long
    dwReserved1 As Long
    cFileName As String * MAX_PATH
    cAlternate As String * 14
End Type

Private Declare Function API_FindFirstFile Lib "kernel32" Alias "FindFirstFileA" _
    (ByVal lpFileName As String, _
    ByRef lpFindFileData As WIN32_FIND_DATA) As Long

Private Declare Function API_FindNextFile Lib "kernel32" Alias "FindNextFileA" _
    (ByVal hFindFile As Long, _
    ByRef lpFindFileData As WIN32_FIND_DATA) As Long

Private Declare Function API_FindClose Lib "kernel32" Alias "FindClose" _
    (ByVal hFindFile As Long) As Long

Private Declare Function API_FileTimeToLocalFileTime Lib "kernel32" Alias "FileTimeToLocalFileTime" _
    (ByRef lpFileTime As FILETIME, _
    ByRef lpLocalFileTime As FILETIME) As Long

Private Declare Function API_FileTimeToSystemTime Lib "kernel32" Alias "FileTimeToSystemTime" _
    (ByRef lpFileTime As FILETIME, _
    ByRef lpSystemTime As SYSTEMTIME) As Long

' Private variables.
Private m_hFindFile As Long
Private m_xFindFileData As WIN32_FIND_DATA
Private m_sFindFolder As String

' Error declarations.
Const ksErrSource = "gSWLibrary.CFileSearch"
Const knErrNotYetOpen = vbObjectError + 1
Const ksErrNotYetOpen = "A file search is not currently running."

Public Property Get ErrorOnFileNotFound() As Boolean
    ErrorOnFileNotFound = m_bErrorOnFileNotFound
End Property

Public Property Let ErrorOnFileNotFound(ByVal bErrorOnFileNotFound As Boolean)
    m_bErrorOnFileNotFound = bErrorOnFileNotFound
End Property

Public Property Get ErrorOnPathNotFound() As Boolean
    ErrorOnPathNotFound = m_bErrorOnPathNotFound
End Property

Public Property Let ErrorOnPathNotFound(ByVal bErrorOnPathNotFound As Boolean)
    m_bErrorOnPathNotFound = bErrorOnPathNotFound
End Property

Public Sub FindFirst(ByVal sPattern As String)

    Dim nReturn As Long

    ' If a search is open, force it closed.
    If Searching Then
        On Error Resume Next
        FindClose
        On Error GoTo 0
    End If

    ' parse out the containing folder for use later on
    m_sFindFolder = ParsePathFile(sPattern, "")

    nReturn = API_FindFirstFile(sPattern, m_xFindFileData)

    If nReturn = INVALID_HANDLE_VALUE Then
        nReturn = Err.LastDllError
        Select Case nReturn
        Case ERROR_FILE_NOT_FOUND, ERROR_NO_MORE_FILES
            If Not m_bErrorOnFileNotFound Then
                ' file not found - leave search closed
                nReturn = 0
            End If
        Case ERROR_PATH_NOT_FOUND
            If Not m_bErrorOnPathNotFound Then
                ' path not found - leave search closed
                nReturn = 0
            End If
        End Select
        If nReturn <> 0 Then
            ' unexpected error - raise it
            RaiseWindowsError nReturn
        End If
    Else
        ' open search
        m_hFindFile = nReturn
    End If

End Sub

Public Sub FindNext()

    Dim nReturn As Long

    If Not Searching Then
        Err.Raise knErrNotYetOpen, ksErrSource, ksErrNotYetOpen
    End If

    nReturn = API_FindNextFile(m_hFindFile, m_xFindFileData)

    If nReturn = 0 Then
        nReturn = Err.LastDllError
        Select Case nReturn
        Case ERROR_NO_MORE_FILES
            ' no more files - close search
            FindClose
        Case Else
            ' unexpected error - raise it
            RaiseWindowsError nReturn
        End Select
    End If

End Sub

' Finish current search in order to start a new one.
' Included for completeness but never required.
' Attributes: Hidden
Public Sub FindClose()
Attribute FindClose.VB_MemberFlags = "40"

    Dim nReturn As Long

    If Not Searching Then
        Exit Sub
    End If

    nReturn = API_FindClose(m_hFindFile)

    If nReturn = 0 Then
        nReturn = Err.LastDllError
        ' unexpected error - raise it
        RaiseWindowsError nReturn
    Else
        ' close search
        m_hFindFile = 0
    End If

End Sub

Public Property Get Searching() As Boolean

    Searching = (m_hFindFile <> 0)

End Property

Public Property Get Name() As String

    If m_hFindFile <> 0 Then
        Name = Trim$(RemoveTZ(m_xFindFileData.cFileName))
    Else
        Name = ""
    End If

End Property

Public Property Get ShortName() As String

    If m_hFindFile <> 0 Then
        ShortName = Trim$(RemoveTZ(m_xFindFileData.cAlternate))
    Else
        ShortName = ""
    End If

End Property

Public Property Get FullyQualifiedName() As String

    If m_hFindFile <> 0 Then
        FullyQualifiedName = AddSlash(m_sFindFolder) & Name
    Else
        FullyQualifiedName = ""
    End If

End Property

Public Property Get Extension() As String

    Dim sExtension As String

    ParseFileExt Name, sExtension
    Extension = sExtension

End Property

Public Property Get Attributes() As EAttributes

    If m_hFindFile <> 0 Then
        Attributes = m_xFindFileData.dwFileAttributes
    Else
        Attributes = 0
    End If

End Property

Public Property Get Size() As Long

    If m_hFindFile <> 0 Then
        Size = m_xFindFileData.nFileSizeLow
    Else
        Size = 0
    End If

End Property

Public Property Get TimeCreated() As Variant

    If m_hFindFile <> 0 Then
        TimeCreated = FileTimeToVBDate(m_xFindFileData.ftCreationTime, True)
    Else
        TimeCreated = Empty
    End If

End Property

Public Property Get TimeLastAccessed() As Variant

    If m_hFindFile <> 0 Then
        TimeLastAccessed = FileTimeToVBDate(m_xFindFileData.ftLastAccessTime, True)
    Else
        TimeLastAccessed = Empty
    End If

End Property

Public Property Get TimeLastModified() As Variant

    If m_hFindFile <> 0 Then
        TimeLastModified = FileTimeToVBDate(m_xFindFileData.ftLastWriteTime, True)
    Else
        TimeLastModified = Empty
    End If

End Property

Public Property Get TimeCreatedUTC() As Variant

    If m_hFindFile <> 0 Then
        TimeCreatedUTC = FileTimeToVBDate(m_xFindFileData.ftCreationTime, False)
    Else
        TimeCreatedUTC = Empty
    End If

End Property

Public Property Get TimeLastAccessedUTC() As Variant

    If m_hFindFile <> 0 Then
        TimeLastAccessedUTC = FileTimeToVBDate(m_xFindFileData.ftLastAccessTime, False)
    Else
        TimeLastAccessedUTC = Empty
    End If

End Property

Public Property Get TimeLastModifiedUTC() As Variant

    If m_hFindFile <> 0 Then
        TimeLastModifiedUTC = FileTimeToVBDate(m_xFindFileData.ftLastWriteTime, False)
    Else
        TimeLastModifiedUTC = Empty
    End If

End Property

Public Function IsOfType(ByVal nAttributes As EAttributes) As Boolean

    IsOfType = ((Attributes And nAttributes) <> 0)

End Function

' Returns True for any type of file, but not a folder or a volume label.
Public Property Get IsFile() As Boolean

    IsFile = Not IsOfType(knAttributeDirectory Or knAttributeVolumeLabel)

End Property

' Returns True for a genuine folder, but not the . or .. pseudo-entries.
Public Property Get IsFolder() As Boolean

    Dim sName As String

    IsFolder = False
    If IsOfType(knAttributeDirectory) Then
        sName = Name
        If sName = "." Then
        ElseIf sName = ".." Then
        Else
            IsFolder = True
        End If
    End If

End Property

Private Sub Class_Initialize()

    ' Start with a closed search.
    m_hFindFile = 0

    ' Set defaults to emulate previous behaviour.
    m_bErrorOnFileNotFound = True
    m_bErrorOnPathNotFound = True

End Sub

Private Sub Class_Terminate()

    ' If a search is open, force it closed.
    If Searching Then
        On Error Resume Next
        FindClose
        On Error GoTo 0
    End If

End Sub

Private Function FileTimeToVBDate(ByRef xFileTime As FILETIME, _
    ByVal bLocal As Boolean) As Variant

    Dim xLocalFileTime As FILETIME
    Dim xSystemTime As SYSTEMTIME
    Dim nReturn As Long

    FileTimeToVBDate = Empty

    If bLocal Then
        nReturn = API_FileTimeToLocalFileTime(xFileTime, xLocalFileTime)
        If nReturn = 0 Then
            Exit Function
        End If
        nReturn = API_FileTimeToSystemTime(xLocalFileTime, xSystemTime)
        If nReturn = 0 Then
            Exit Function
        End If
    Else
        nReturn = API_FileTimeToSystemTime(xFileTime, xSystemTime)
        If nReturn = 0 Then
            Exit Function
        End If
    End If

    On Error Resume Next
    FileTimeToVBDate = _
        DateSerial(xSystemTime.wYear, xSystemTime.wMonth, xSystemTime.wDay) + _
        TimeSerial(xSystemTime.wHour, xSystemTime.wMinute, xSystemTime.wSecond)
    Err.Clear

End Function
