Attribute VB_Name = "MVersions"
' Module:   Version string comparison routines
' Shared:   Yes
' Needs:    gSWLibrary
'
Option Explicit

#Const knVersionEqual = 0
#Const knVersion1GreaterThan2 = 0
#Const knVersion2GreaterThan1 = 0

Public Enum EVersionComparisons
    knVersionEqual
    knVersion1GreaterThan2
    knVersion2GreaterThan1
End Enum

Public Function VersionCompare( _
    ByVal sVersion1 As String, _
    ByVal sVersion2 As String) As EVersionComparisons

    Dim nNumber1 As Long
    Dim nNumber2 As Long
    Dim lCount As Long

    sVersion1 = Trim$(sVersion1)
    sVersion2 = Trim$(sVersion2)
    VersionCompare = knVersionEqual
    
    lCount = 0
    Do Until sVersion1 = "" And sVersion2 = ""
        nNumber1 = ToLong(ParseSep(sVersion1, sVersion1, "."))
        nNumber2 = ToLong(ParseSep(sVersion2, sVersion2, "."))
        
        lCount = lCount + 1
        
        'This if statement and everything in it can be deleted once no one is on 1900 or lower.
        If lCount = 1 Then
            'On the first loop check the major version.
            'Versions 1810, 1811 and 1900 come before versions 1090, 1100 and 1110.
            If nNumber1 = 1090 Or nNumber1 = 1100 Or nNumber1 = 1110 Then
                If nNumber2 = 1810 Or nNumber2 = 1811 Or nNumber2 = 1900 Then
                    VersionCompare = knVersion1GreaterThan2
                    Exit Do
                End If
            End If
        
            If nNumber1 = 1810 Or nNumber1 = 1811 Or nNumber1 = 1900 Then
                If nNumber2 = 1090 Or nNumber2 = 1100 Or nNumber2 = 1110 Then
                    VersionCompare = knVersion2GreaterThan1
                    Exit Do
                End If
            End If
        End If
        
        If nNumber1 > nNumber2 Then
            VersionCompare = knVersion1GreaterThan2
            Exit Do
        ElseIf nNumber1 < nNumber2 Then
            VersionCompare = knVersion2GreaterThan1
            Exit Do
        End If
    Loop

End Function

Public Function VersionEqual( _
    ByVal sVersion1 As String, _
    ByVal sVersion2 As String) As Boolean

    VersionEqual = (VersionCompare(sVersion1, sVersion2) = knVersionEqual)

End Function

Public Function VersionGreaterThan( _
    ByVal sVersion1 As String, _
    ByVal sVersion2 As String) As Boolean

    VersionGreaterThan = (VersionCompare(sVersion1, sVersion2) = knVersion1GreaterThan2)

End Function

Public Function VersionLessThan( _
    ByVal sVersion1 As String, _
    ByVal sVersion2 As String) As Boolean

    VersionLessThan = (VersionCompare(sVersion1, sVersion2) = knVersion2GreaterThan1)

End Function
