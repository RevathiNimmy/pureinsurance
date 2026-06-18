VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CLogicalDatabases"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    Logical databases collection
' Shared:   No
'
' The list of logical database versions currently installed,
' plus all installable logical databases and versions found
' on the CD. It is designed to be populated just once from
' an XML file, then updated with extra information as it is
' discovered during the course of the program.
'
Option Explicit

Private m_col As Collection

' Error declarations.
Const ksErrSource = ksProjectName '& ".CLogicalDatabases"
Const knErrBadParameters = 5
Const ksErrBadParameters = "Valid name and description required for logical database."

' Add new object from the XML definition.
Public Sub Add(ByVal sName As String, _
    ByVal sAbbreviation As String, _
    ByVal sDescription As String)

    Dim oTask As CLogicalDatabase

    ' Validate data first.
    If sName = "" Or sDescription = "" Then
        Err.Raise knErrBadParameters, ksErrSource, ksErrBadParameters
    End If

    ' Add it to the collection.
    Set oTask = New CLogicalDatabase
    oTask.Name = sName
    oTask.Abbreviation = sAbbreviation
    oTask.Description = sDescription
    m_col.Add Item:=oTask, Key:=UCase$(sName)
    Set oTask = Nothing

End Sub

Public Sub Append(ByVal oTask As CLogicalDatabase)
    m_col.Add Item:=oTask, Key:=UCase$(oTask.Name)
End Sub

Public Property Get Count() As Long
    Count = m_col.Count
End Property

' Easy collection lookup using the logical database
' name or abbreviation. Returns Nothing if search failed.
Public Function Find(ByVal sSearch As String) As CLogicalDatabase

    Dim oLoop As CLogicalDatabase

    ' Fail gracefully with blank value.
    On Error Resume Next
    Set Find = Nothing

    ' Search is always case-insensitive.
    sSearch = UCase$(Trim$(sSearch))
    If sSearch = "" Then Exit Function

    ' Try a fast search on the collection key first.
    Set Find = m_col.Item(sSearch)
    Err.Clear

    ' If that fails, try a manual search on the abbreviation.
    If Find Is Nothing Then
        For Each oLoop In m_col
            If UCase$(oLoop.Abbreviation) = sSearch Then
                Set Find = oLoop
                Exit For
            End If
        Next
    End If

End Function

' Attributes: Default
Public Property Get Item(ByVal sName As String) As CLogicalDatabase
Attribute Item.VB_UserMemId = 0
    Set Item = m_col.Item(UCase$(sName))
End Property

' Attributes: Enumerated, Hidden
Public Function NewEnum() As IUnknown
Attribute NewEnum.VB_UserMemId = -4
Attribute NewEnum.VB_MemberFlags = "40"
    Set NewEnum = m_col.[_NewEnum]
End Function

Private Sub Class_Initialize()
    Set m_col = New Collection
End Sub

Private Sub Class_Terminate()
    Set m_col = Nothing
End Sub
