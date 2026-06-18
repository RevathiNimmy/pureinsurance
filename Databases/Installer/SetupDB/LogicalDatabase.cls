VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CLogicalDatabase"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    Logical database
' Shared:   No
'
' Holds information about one logical database, its
' currently-installed version, and all available installable
' versions.
'
Option Explicit

' Logical database name as used in the database and SourceSafe.
Private m_sName As String
' Recognised alternative name or abbreviation.
Private m_sAbbreviation As String
' Description to be displayed on screen.
Private m_sDescription As String
' Actual folder name found on disk (NOT a complete filespec!).
Private m_sFolder As String
' Highest version encountered so far while reading in the upgrade order file.
Private m_sMaxOrderVersion As String
' Version read from the database. Blank if not yet installed.
Private m_sCurrentVersion As String
' Sorted array (descending) of creatable versions from disk.
Private m_colCreatableVersions As Collection
' Sorted array (descending) of upgradable versions from disk.
Private m_colUpgradableVersions As Collection
' Version requested by the command line.
Private m_sCommandLineVersion As String
' Version that was finally selected to install.
Private m_sVersion As String

Public Property Get Name() As String
    Name = m_sName
End Property

Public Property Let Name(ByVal sName As String)
    m_sName = sName
End Property

Public Property Get Abbreviation() As String
    Abbreviation = m_sAbbreviation
End Property

Public Property Let Abbreviation(ByVal sAbbreviation As String)
    m_sAbbreviation = sAbbreviation
End Property

Public Property Get Description() As String
    Description = m_sDescription
End Property

Public Property Let Description(ByVal sDescription As String)
    m_sDescription = sDescription
End Property

Public Property Get Folder() As String
    Folder = m_sFolder
End Property

Public Property Let Folder(ByVal sFolder As String)
    m_sFolder = sFolder
End Property

Public Property Get MaxOrderVersion() As String
    MaxOrderVersion = m_sMaxOrderVersion
End Property

Public Property Let MaxOrderVersion(ByVal sMaxOrderVersion As String)
    m_sMaxOrderVersion = sMaxOrderVersion
End Property

Public Property Get CurrentVersion() As String
    CurrentVersion = m_sCurrentVersion
End Property

Public Property Let CurrentVersion(ByVal sCurrentVersion As String)
    m_sCurrentVersion = sCurrentVersion
End Property

Public Property Get CommandLineVersion() As String
    CommandLineVersion = m_sCommandLineVersion
End Property

Public Property Let CommandLineVersion(ByVal sCommandLineVersion As String)
    m_sCommandLineVersion = sCommandLineVersion
End Property

Public Property Let CreatableVersion(ByVal sCreatableVersion As String)
    AddVersion m_colCreatableVersions, sCreatableVersion
End Property

Public Property Let UpgradableVersion(ByVal sUpgradableVersion As String)
    AddVersion m_colUpgradableVersions, sUpgradableVersion
End Property

Public Property Get Version() As String
    Version = m_sVersion
End Property

Public Property Let Version(ByVal sVersion As String)
    m_sVersion = sVersion
End Property

' Encapsulates test done all over the code.
Public Property Get NeedsCreating() As Boolean

    NeedsCreating = (m_sCurrentVersion = "")

End Property

' Retrieves the correct list of installable versions.
Public Property Get InstallableVersions() As Collection

    If NeedsCreating Then
        Set InstallableVersions = m_colCreatableVersions
    Else
        Set InstallableVersions = m_colUpgradableVersions
    End If

End Property

Public Property Get DefaultVersion() As String

    Dim sMaxVersion As String

    If InstallableVersions.Count > 0 Then
        sMaxVersion = InstallableVersions.Item(1)
    Else
        sMaxVersion = ""
    End If

    If VersionLessThan(m_sCurrentVersion, sMaxVersion) Then
        DefaultVersion = sMaxVersion
    Else
        DefaultVersion = ""
    End If

End Property

' Returns True if the requested version can be installed
' (i.e. Folder exists and Version exists in the correct place).
Public Function VersionCanBeInstalled() As Boolean

    Dim vsVersion As Variant ' String
    Dim bFound As Boolean

    VersionCanBeInstalled = False

    ' If the logical database folder is not on the CD,
    ' then the version obviously cannot be installed.
    If m_sFolder = "" Then
        Exit Function
    End If

    ' If there are no installable versions on the CD,
    ' then the version obviously cannot be installed.
    If InstallableVersions.Count = 0 Then
        Exit Function
    End If

    ' Try a fast search for the version in the collection.
    ' If found, then return True straight away.
    On Error Resume Next
    InstallableVersions.Item m_sVersion
    bFound = (Err.Number = 0)
    Err.Clear
    If bFound Then
        VersionCanBeInstalled = True
        Exit Function
    End If

    ' Try a manual search, in case the version was specified
    ' on the command line in a slightly different format to
    ' the folder name on the CD. *IN ADDITION*, if the version
    ' property DOES NOT exactly match an installable version,
    ' correct it here, because all processing code assumes that
    ' it does match.
    bFound = False
    For Each vsVersion In InstallableVersions
        If VersionEqual(vsVersion, m_sVersion) Then
            bFound = True
            m_sVersion = vsVersion
            Exit For
        End If
    Next
    VersionCanBeInstalled = bFound

End Function

Private Sub Class_Initialize()

    Set m_colCreatableVersions = New Collection
    Set m_colUpgradableVersions = New Collection

End Sub

Private Sub Class_Terminate()

    Set m_colCreatableVersions = Nothing
    Set m_colUpgradableVersions = Nothing

End Sub

' Adds a version into a collection in descending numerical order.
Private Sub AddVersion(ByVal colVersions As Collection, ByVal sNewVersion As String)

    Dim vsVersion As Variant ' String
    Dim bInserted As Boolean

    ' If new version is blank, then don't add it to the list.
    sNewVersion = Trim$(sNewVersion)
    If sNewVersion = "" Then
        Exit Sub
    End If

    ' If list is empty, then just add the version.
    If colVersions.Count = 0 Then
        colVersions.Add _
            Item:=sNewVersion, _
            Key:=sNewVersion
        Exit Sub
    End If

    bInserted = False
    For Each vsVersion In colVersions
        Select Case VersionCompare(vsVersion, sNewVersion)
        Case knVersion2GreaterThan1
            ' Greater than this version, insert it above this version.
            colVersions.Add _
                Item:=sNewVersion, _
                Key:=sNewVersion, _
                Before:=vsVersion
            bInserted = True
            Exit For
        Case knVersion1GreaterThan2
            ' Carry on searching.
        Case Else ' knVersionEqual
            ' Already in the list, don't add it again.
            bInserted = True
            Exit For
        End Select
    Next

    If Not bInserted Then
        ' Must be the last item, insert at the end.
        colVersions.Add _
            Item:=sNewVersion, _
            Key:=sNewVersion
    End If

End Sub
