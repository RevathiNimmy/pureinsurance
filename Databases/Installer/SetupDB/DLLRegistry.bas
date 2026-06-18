Attribute VB_Name = "MDLLRegistry"
' Module:   Registry & INI Access Functions
' Shared:   Yes (RESTRICTED)
' Needs:    MDLLDataTypes
'
' THIS CODE IMPLEMENTS CORRESPONDING FUNCTIONS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE CHANGING THE INTERNALS OF THE DLL.
'
Option Explicit

' Registry Terminology:
'       <Hive>\<Folder>\<Folder>\<Folder>\<Attribute>=<Value>
'
' INI File Terminology:
'       <Hive>.ini
'       [<Folder>\<Folder>\<Folder>]
'       <Attribute>=<Value>

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Public Enumerations
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' Cannot use a real enumeration otherwise the DLL would not compile!
Public Const HKEY_CLASSES_ROOT = &H80000000
Public Const HKEY_CURRENT_CONFIG = &H80000005
Public Const HKEY_CURRENT_USER = &H80000001
Public Const HKEY_DYN_DATA = &H80000006
Public Const HKEY_LOCAL_MACHINE = &H80000002
Public Const HKEY_PERFORMANCE_DATA = &H80000004
Public Const HKEY_USERS = &H80000003

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Private
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' API Declarations
Private Declare Function RegOpenKeyEx _
    Lib "advapi32.dll" Alias "RegOpenKeyExA" _
    (ByVal hKey As Long, ByVal lpSubKey As String, ByVal ulOptions As Long, ByVal samDesired As Long, ByRef phkResult As Long) As Long

Private Declare Function RegCreateKeyEx _
    Lib "advapi32.dll" Alias "RegCreateKeyExA" _
    (ByVal hKey As Long, ByVal lpSubKey As String, ByVal Reserved As Long, ByVal lpClass As String, ByVal dwOptions As Long, ByVal samDesired As Long, ByVal lpSecurityAttributes As Long, ByRef phkResult As Long, ByRef lpdwDisposition As Long) As Long

Private Declare Function RegQueryValueEx _
    Lib "advapi32.dll" Alias "RegQueryValueExA" _
    (ByVal hKey As Long, ByVal lpValueName As String, ByVal lpReserved As Long, ByRef lpType As Long, ByRef lpData As Any, ByRef lpcbData As Long) As Long

Private Declare Function RegSetValueEx _
    Lib "advapi32.dll" Alias "RegSetValueExA" _
    (ByVal hKey As Long, ByVal lpValueName As String, ByVal Reserved As Long, ByVal dwType As Long, ByRef lpData As Any, ByVal cbData As Long) As Long

Private Declare Function RegDeleteKey _
    Lib "advapi32.dll" Alias "RegDeleteKeyA" _
    (ByVal hKey As Long, ByVal lpSubKey As String) As Long

Private Declare Function RegDeleteValue _
    Lib "advapi32.dll" Alias "RegDeleteValueA" _
    (ByVal hKey As Long, ByVal lpValueName As String) As Long

Private Declare Function RegCloseKey _
    Lib "advapi32.dll" _
    (ByVal hKey As Long) As Long

Private Declare Function GetPrivateProfileString _
    Lib "kernel32" Alias "GetPrivateProfileStringA" _
    (ByVal lpszSection As String, ByVal lpszEntry As String, ByVal lpszDefault As String, ByVal lpszReturnBuffer As String, ByVal cbReturnBuffer As Integer, ByVal lpszFilename As String) As Integer

Private Declare Function WritePrivateProfileString _
    Lib "kernel32" Alias "WritePrivateProfileStringA" _
    (ByVal lpszSection As String, ByVal lpszEntry As String, ByVal lpszString As String, ByVal lpszFilename As String) As Integer

' Registry data types.
Private Const REG_BINARY = 3
Private Const REG_DWORD = 4
Private Const REG_DWORD_BIG_ENDIAN = 5
Private Const REG_DWORD_LITTLE_ENDIAN = 4
Private Const REG_LINK = 6
Private Const REG_NONE = 0
Private Const REG_SZ = 1
Private Const REG_EXPAND_SZ = 2

' Not all of these constants are used at present.
' They may be used in the future for NT etc.
Private Const SYNCHRONIZE = &H100000
Private Const STANDARD_RIGHTS_READ = &H20000
Private Const STANDARD_RIGHTS_WRITE = &H20000
Private Const STANDARD_RIGHTS_EXECUTE = &H20000
Private Const STANDARD_RIGHTS_REQUIRED = &HF0000
Private Const STANDARD_RIGHTS_ALL = &H1F0000

Private Const KEY_QUERY_VALUE = &H1
Private Const KEY_SET_VALUE = &H2
Private Const KEY_CREATE_SUB_KEY = &H4
Private Const KEY_ENUMERATE_SUB_KEYS = &H8
Private Const KEY_NOTIFY = &H10
Private Const KEY_CREATE_LINK = &H20
Private Const KEY_READ = ((STANDARD_RIGHTS_READ Or KEY_QUERY_VALUE Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY) And (Not SYNCHRONIZE))
Private Const KEY_WRITE = ((STANDARD_RIGHTS_WRITE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY) And (Not SYNCHRONIZE))
Private Const KEY_EXECUTE = (KEY_READ)
Private Const KEY_ALL_ACCESS = ((STANDARD_RIGHTS_ALL Or KEY_QUERY_VALUE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY Or KEY_CREATE_LINK) And (Not SYNCHRONIZE))

Private Const ERROR_SUCCESS = 0&
Private Const ERROR_FILE_NOT_FOUND = 2&
Private Const REG_CREATED_NEW_KEY = &H1&
Private Const REG_OPENED_EXISTING_KEY = &H2&
Private Const REG_OPTION_NON_VOLATILE = 0
Private Const REG_OPTION_VOLATILE = 1

' Errors raised.
Const ksErrSource = "gSWLibrary.GRegistry"
Const knErrBadHiveOrFolder = 5
Const ksErrBadHiveOrFolder = "Hive or folder not specified."
Const knErrUnSupportedRegType = 13
Const ksErrUnSupportedRegType = "Unsupported registry data type."
Const knErrUnSupportedVBType = 13
Const ksErrUnSupportedVBType = "Unsupported VB data type."

' Reads the value of a specified registry attribute. If attribute is blank,
' it reads the default value for the folder. If the folder/attribute
' does not currently exist, the default is returned.
Public Function RegRead(ByVal lHive As Long, _
    ByVal sFolder As String, _
    ByVal sAttribute As String, _
    ByVal vDefault As Variant) As Variant

    Dim nError As Long
    Dim hFolder As Long
    Dim dwType As Long
    Dim cbData As Long

    RegRead = vDefault

    If lHive = 0 Or sFolder = "" Then
        Err.Raise knErrBadHiveOrFolder, ksErrSource, ksErrBadHiveOrFolder
    End If

    ' Open the folder handle.
    nError = RegOpenKeyEx(lHive, sFolder, 0&, KEY_QUERY_VALUE, hFolder)
    If nError <> ERROR_SUCCESS Or hFolder = 0 Then
        GoTo EX_NormalExit
    End If

    ' Read the attribute type and size.
    nError = RegQueryValueEx(hFolder, sAttribute, 0&, dwType, ByVal 0&, cbData)
    If nError <> ERROR_SUCCESS Or cbData = 0 Then
        GoTo EX_NormalExit
    End If

    ' Read the attribute value in the correct manner for its data type.
    Select Case dwType

    Case REG_DWORD, REG_DWORD_LITTLE_ENDIAN
        ' Read as a long integer.
        Dim lValue As Long
        nError = RegQueryValueEx(hFolder, sAttribute, 0&, dwType, lValue, cbData)
        If nError <> ERROR_SUCCESS Then
            GoTo EX_NormalExit
        End If
        RegRead = lValue

    Case REG_SZ, REG_EXPAND_SZ
        ' Read as a string.
        Dim sValue As String
        sValue = String$(cbData, 0)
        nError = RegQueryValueEx(hFolder, sAttribute, 0&, dwType, ByVal sValue, cbData)
        If nError <> ERROR_SUCCESS Then
            GoTo EX_NormalExit
        End If
        RegRead = Left$(sValue, cbData - 1)

    Case REG_BINARY
        ' Read as a byte array.
        Dim abyValue() As Byte
        Dim vValue As Variant
        ReDim abyValue(0 To cbData - 1)
        nError = RegQueryValueEx(hFolder, sAttribute, 0&, dwType, abyValue(0), cbData)
        If nError <> ERROR_SUCCESS Then
            GoTo EX_NormalExit
        End If
        vValue = abyValue
        RegRead = vValue

    Case Else ' REG_DWORD_BIG_ENDIAN, REG_LINK, REG_MULTI_SZ, REG_NONE, REG_RESOURCE_LIST
        ' Cannot interpret this - return the default instead.
        #If SiriusDatabaseInstaller Then
            RegCloseKey hFolder
            Err.Raise knErrUnSupportedRegType, ksErrSource, ksErrUnSupportedRegType
        #End If

    End Select

EX_NormalExit:
    ' Close the folder handle if necessary.
    If hFolder <> 0 Then
        RegCloseKey hFolder
    End If

    #If SiriusDatabaseInstaller Then
        If nError <> ERROR_SUCCESS And nError <> ERROR_FILE_NOT_FOUND Then
            RaiseWindowsError nError
        End If
    #End If

End Function

' Writes the value of a specified registry attribute. If attribute is blank,
' it writes the default value for the folder. If the folder/attribute
' does not currently exist, they are created.
Public Sub RegWrite(ByVal lHive As Long, _
    ByVal sFolder As String, _
    ByVal sAttribute As String, _
    ByVal vValue As Variant)

    Dim nError As Long
    Dim hFolder As Long
    Dim dwDisposition As Long

    If lHive = 0 Or sFolder = "" Then
        Err.Raise knErrBadHiveOrFolder, ksErrSource, ksErrBadHiveOrFolder
    End If

    ' Open the folder handle, creating any bits that don't exist.
    nError = RegCreateKeyEx(lHive, sFolder, 0&, vbNullString, REG_OPTION_NON_VOLATILE, KEY_SET_VALUE, 0&, hFolder, dwDisposition)
    If nError <> ERROR_SUCCESS Or hFolder = 0 Then
        GoTo EX_NormalExit
    End If

    ' Write the attribute value in the correct manner for its data type.
    Select Case VarType(vValue)

    Case vbBoolean
        ' Write as a long integer.
        Dim bValue As Long
        bValue = ToLong(vValue)
        nError = RegSetValueEx(hFolder, sAttribute, 0&, REG_DWORD, bValue, 4)

    Case vbByte, vbInteger, vbLong
        ' Write as a long integer.
        Dim lValue As Long
        lValue = ToLongFixed(vValue)
        nError = RegSetValueEx(hFolder, sAttribute, 0&, REG_DWORD, lValue, 4)

    Case vbString, vbDouble, vbDate, vbCurrency, vbSingle, vbNull, vbEmpty
        ' Write as a string.
        Dim sValue As String
        sValue = ToStringFixed(vValue)
        nError = RegSetValueEx(hFolder, sAttribute, 0&, REG_SZ, ByVal sValue, Len(sValue) + 1)

    Case vbArray + vbByte
        ' Write as a byte array.
        Dim abyValue() As Byte
        abyValue = vValue
        nError = RegSetValueEx(hFolder, sAttribute, 0&, REG_BINARY, abyValue(LBound(abyValue)), UBound(abyValue) - LBound(abyValue) + 1)

    Case Else
        ' Data type not supported, raise an error.
        RegCloseKey hFolder
        Err.Raise knErrUnSupportedVBType, ksErrSource, ksErrUnSupportedVBType

    End Select

EX_NormalExit:
    ' Close the folder handle if necessary.
    If hFolder <> 0 Then
        RegCloseKey hFolder
    End If

    #If SiriusDatabaseInstaller Then
        If nError <> ERROR_SUCCESS Then
            RaiseWindowsError nError
        End If
    #End If

End Sub

' Deletes the specified registry attribute. If attribute is blank,
' it blanks out the default value for the folder. If the folder/attribute
' does not currently exist, nothing happens.
Public Sub RegDelete(ByVal lHive As Long, _
    ByVal sFolder As String, _
    ByVal sAttribute As String)

    Dim nError As Long
    Dim hFolder As Long

    If lHive = 0 Or sFolder = "" Then
        Err.Raise knErrBadHiveOrFolder, ksErrSource, ksErrBadHiveOrFolder
    End If

    ' Open the folder handle.
    nError = RegOpenKeyEx(lHive, sFolder, 0&, KEY_SET_VALUE, hFolder)
    If nError <> ERROR_SUCCESS Or hFolder = 0 Then
        GoTo EX_NormalExit
    End If

    ' Delete the attribute.
    RegDeleteValue hFolder, sAttribute

EX_NormalExit:
    ' Close the folder handle if necessary.
    If hFolder <> 0 Then
        RegCloseKey hFolder
    End If

    #If SiriusDatabaseInstaller Then
        If nError <> ERROR_SUCCESS And nError <> ERROR_FILE_NOT_FOUND Then
            RaiseWindowsError nError
        End If
    #End If

End Sub

' Detects whether the specified folder exists.
Public Function RegFolderExists(ByVal lHive As Long, _
    ByVal sFolder As String) As Boolean

    Dim nError As Long
    Dim hFolder As Long

    RegFolderExists = False

    If lHive = 0 Or sFolder = "" Then
        Err.Raise knErrBadHiveOrFolder, ksErrSource, ksErrBadHiveOrFolder
    End If

    ' Try to open the folder handle.
    nError = RegOpenKeyEx(lHive, sFolder, 0&, KEY_QUERY_VALUE, hFolder)
    RegFolderExists = (nError = ERROR_SUCCESS And hFolder <> 0)

EX_NormalExit:
    ' Close the folder handle if necessary.
    If hFolder <> 0 Then
        RegCloseKey hFolder
    End If

    #If SiriusDatabaseInstaller Then
        If nError <> ERROR_SUCCESS And nError <> ERROR_FILE_NOT_FOUND Then
            RaiseWindowsError nError
        End If
    #End If

End Function

' Deletes the folder specified in the third parameter. The
' folder must not have any subfolders for this to work.
Public Sub RegFolderDelete(ByVal lHive As Long, _
    ByVal sFolder As String, _
    ByVal sFolderDelete As String)

    Dim nError As Long
    Dim hFolder As Long

    If lHive = 0 Or sFolder = "" Then
        Err.Raise knErrBadHiveOrFolder, ksErrSource, ksErrBadHiveOrFolder
    End If

    ' Open the folder handle.
    nError = RegOpenKeyEx(lHive, sFolder, 0&, KEY_SET_VALUE, hFolder)
    If nError <> ERROR_SUCCESS Or hFolder = 0 Then
        GoTo EX_NormalExit
    End If

    ' Delete the folder.
    RegDeleteKey hFolder, sFolderDelete

EX_NormalExit:
    ' Close the folder handle if necessary.
    If hFolder <> 0 Then
        RegCloseKey hFolder
    End If

    #If SiriusDatabaseInstaller Then
        If nError <> ERROR_SUCCESS And nError <> ERROR_FILE_NOT_FOUND Then
            RaiseWindowsError nError
        End If
    #End If

End Sub

' Reads the value of a specified INI attribute. If attribute is blank,
' it reads the default value for the folder. The value is coerced into
' the same data type as the default supplied. If the folder/attribute
' does not currently exist, the default is returned.
Public Function IniRead(ByVal sHive As String, _
    ByVal sFolder As String, _
    ByVal sAttribute As String, _
    ByVal vDefault As Variant) As Variant

    Const knReturnLength = 255
    Dim sReturnValue As String
    Dim nBytesCopied As Integer
    Dim sDefault As String
    Dim sValue As String
    Dim vValue As Variant

    IniRead = vDefault

    If sHive = "" Or sFolder = "" Then
        Err.Raise knErrBadHiveOrFolder, ksErrSource, ksErrBadHiveOrFolder
    ElseIf sAttribute = "" Then
        sAttribute = "@"    ' emulate the registry "default folder value"
    End If

    sDefault = ToStringFixed(vDefault)
    sReturnValue = String$(knReturnLength, vbNullChar)

    nBytesCopied = GetPrivateProfileString(sFolder, sAttribute, sDefault, sReturnValue, knReturnLength, sHive)

    ' I've noticed recently that the function is returning nBytesCopied = 0!
    ' We must therefore scan for a zero manually if this is the case.
    If nBytesCopied = 0 Then
        nBytesCopied = InStr(sReturnValue, vbNullChar) - 1
    End If
    If nBytesCopied >= 0 Then
        sValue = Left$(sReturnValue, nBytesCopied)
    Else
        sValue = sReturnValue
    End If

    Select Case VarType(vDefault)
    Case vbBoolean
        ' Use international conversion for this one
        ' because it can interpret both strings and integers.
        vValue = ToBoolean(sValue)
    Case vbByte
        vValue = ToByteFixed(sValue)
    Case vbInteger
        vValue = ToIntegerFixed(sValue)
    Case vbLong
        vValue = ToLongFixed(sValue)
    Case vbDouble
        vValue = ToDoubleFixed(sValue)
    Case vbCurrency
        vValue = ToCurrencyFixed(sValue)
    Case vbDate
        vValue = ToDateTimeFixed(sValue)
    Case vbString
        vValue = sValue
    Case Else
        ' Unable to deduce return type, just return the string.
        vValue = sValue
    End Select
    IniRead = vValue

End Function

' Writes the value of a specified INI attribute. If attribute is blank,
' it writes the default value for the folder. If the folder/attribute
' does not currently exist, they are created.
Public Sub IniWrite(ByVal sHive As String, _
    ByVal sFolder As String, _
    ByVal sAttribute As String, _
    ByVal vValue As Variant)

    Dim sValue As String

    If sHive = "" Or sFolder = "" Then
        Err.Raise knErrBadHiveOrFolder, ksErrSource, ksErrBadHiveOrFolder
    ElseIf sAttribute = "" Then
        sAttribute = "@"    ' emulate the registry "default folder value"
    End If

    sValue = ToStringFixed(vValue)

    WritePrivateProfileString sFolder, sAttribute, sValue, sHive

End Sub

' Deletes the specified INI attribute. If attribute is blank,
' it blanks out the default value for the folder. If the folder/attribute
' does not currently exist, nothing happens.
Public Sub IniDelete(ByVal sHive As String, _
    ByVal sFolder As String, _
    ByVal sAttribute As String)

    If sHive = "" Or sFolder = "" Then
        Err.Raise knErrBadHiveOrFolder, ksErrSource, ksErrBadHiveOrFolder
    ElseIf sAttribute = "" Then
        sAttribute = "@"    ' emulate the registry "default folder value"
    End If

    WritePrivateProfileString sFolder, sAttribute, vbNullString, sHive

End Sub

' Deletes the specified INI folder. This will not delete any
' subfolders, they must be deleted individually.
Public Sub IniFolderDelete(ByVal sHive As String, _
    ByVal sFolder As String)

    If sHive = "" Or sFolder = "" Then
        Err.Raise knErrBadHiveOrFolder, ksErrSource, ksErrBadHiveOrFolder
    End If

    WritePrivateProfileString sFolder, vbNullString, vbNullString, sHive

End Sub
