Attribute VB_Name = "MSettings"
' Module:   General Swift registry settings
' Shared:   Yes
' Needs:    gSWLibrary
'
Option Explicit

' Standard registry folders.
Public Const ksRegPathPMRoot = "Software\PM"
Public Const ksRegPathSwift = ksRegPathPMRoot & "\Swift"
Public Const ksRegPathDataBase = "SOFTWARE\Pure\Architecture\Server"
Public Const ksRegPathApplications = ksRegPathSwift & "\Applications"
Public Const ksRegPathAssumptions = ksRegPathSwift & "\Assumptions"

Const ksColours = ksRegPathSwift & "\Colours"
Const ksFolders = ksRegPathSwift & "\Folders"

' Returns the application version properties in human-readable form.
' This function cannot go into a DLL for obvious reasons.
Public Function AppVersion() As String

    AppVersion = App.Major & "." & Format$(App.Minor, "00") & "." & Format$(App.Revision, "0000")

End Function

Public Function ColourErrorBackground() As Long

    ColourErrorBackground = RegRead(HKEY_CURRENT_USER, ksColours, "ErrorBackground", vbRed)

End Function

Public Function ColourErrorForeground() As Long

    ColourErrorForeground = RegRead(HKEY_CURRENT_USER, ksColours, "ErrorForeground", vbRed)

End Function

Public Function ColourMandatoryBackground() As Long

    ColourMandatoryBackground = RegRead(HKEY_CURRENT_USER, ksColours, "MandatoryBackground", vbRed)

End Function

Public Function ColourMandatoryForeground() As Long

    ColourMandatoryForeground = RegRead(HKEY_CURRENT_USER, ksColours, "MandatoryForeground", vbRed)

End Function

Public Function FolderBase() As String

    FolderBase = RegRead(HKEY_LOCAL_MACHINE, ksFolders, "Base", "")

End Function

Public Function FolderPrograms() As String

    Dim sFolder As String
    sFolder = RegRead(HKEY_LOCAL_MACHINE, ksFolders, "Programs", "")
    If sFolder <> "" Then
        FolderPrograms = sFolder
    Else
        FolderPrograms = AddSlash(FolderBase()) & "Bin"
    End If

End Function

Public Function FolderHelp() As String

    Dim sFolder As String
    sFolder = RegRead(HKEY_LOCAL_MACHINE, ksFolders, "Help", "")
    If sFolder <> "" Then
        FolderHelp = sFolder
    Else
        FolderHelp = AddSlash(FolderBase()) & "Help"
    End If

End Function

Public Function FolderReports() As String

    Dim sFolder As String
    sFolder = RegRead(HKEY_LOCAL_MACHINE, ksFolders, "Reports", "")
    If sFolder <> "" Then
        FolderReports = sFolder
    Else
        FolderReports = AddSlash(FolderBase()) & "Reports"
    End If

End Function

Public Function FolderLogs() As String

    Dim sFolder As String
    sFolder = RegRead(HKEY_LOCAL_MACHINE, ksFolders, "Logs", "")
    If sFolder <> "" Then
        FolderLogs = sFolder
    Else
        FolderLogs = AddSlash(FolderBase()) & "Logs"
    End If

End Function

Public Function FolderSQL() As String

    Dim sFolder As String
    sFolder = RegRead(HKEY_LOCAL_MACHINE, ksFolders, "SQL", "")
    If sFolder <> "" Then
        FolderSQL = sFolder
    Else
        FolderSQL = AddSlash(FolderBase()) & "Bin"
    End If

End Function

Public Function FolderUserDocuments() As String

    Dim sFolder As String
    sFolder = RegRead(HKEY_LOCAL_MACHINE, ksFolders, "UserDocuments", "")
    If sFolder <> "" Then
        FolderUserDocuments = sFolder
    Else
        FolderUserDocuments = AddSlash(FolderBase()) & "Documents"
    End If

End Function

Public Function FolderScannedDocuments() As String

    Dim sFolder As String
    sFolder = RegRead(HKEY_LOCAL_MACHINE, ksFolders, "ScannedDocuments", "")
    If sFolder <> "" Then
        FolderScannedDocuments = sFolder
    Else
        FolderScannedDocuments = FolderUserDocuments()
    End If

End Function
