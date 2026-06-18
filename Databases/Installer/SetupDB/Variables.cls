VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CVariables"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    Collection of variables and their values
' Shared:   Yes (RESTRICTED)
' Needs:    CVariable
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE SIRIUS DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
' Generic module to handle an unlimited collection of variables of
' any primitive type. It makes no assumptions about how they are used.
'
Option Explicit

Private m_colVariables As Collection
Private m_bCaseSensitive As Boolean
Private m_bRequireDeclaration As Boolean
Private m_bRequireInitialisation As Boolean

' Error declarations.
Const ksErrSource = "iSWSQLScript.CVariables"
Const knErrCannotRead = vbObjectError + 1
Const knErrCannotWrite = vbObjectError + 2
Const ksErrCannotRead = " must be defined before it can be read."
Const ksErrCannotWrite = " must be defined before it can be written to."

Public Sub Add(ByVal sName As String, ByVal vValue As Variant)

Dim var As New CVariable

    var.Name = sName
    var.Value = vValue
    m_colVariables.Add var, MakeKey(sName)

End Sub

Public Property Get Count() As Long
    Count = m_colVariables.Count
End Property

' Attributes: Default
Public Property Get Item(ByVal sName As String) As Variant
Attribute Item.VB_UserMemId = 0

    On Error Resume Next

    If m_bRequireDeclaration Or m_bRequireInitialisation Then
        ' Assume variable already exists. Throw an error if it doesn't.
        Item = m_colVariables.Item(MakeKey(sName)).Value
        If Err.Number Then
            On Error GoTo 0
            Err.Raise knErrCannotRead, ksErrSource, sName & ksErrCannotRead
        End If
    Else
        ' Return Empty if variable doesn't exist yet.
        Item = m_colVariables.Item(MakeKey(sName)).Value
        If Err.Number Then
            Item = Empty
            Err.Clear
        End If
    End If

End Property

' Attributes: Default
Public Property Let Item(ByVal sName As String, ByVal vValue As Variant)

    On Error Resume Next

    If m_bRequireDeclaration Then
        ' Assume variable already exists. Throw an error if it doesn't.
        m_colVariables.Item(MakeKey(sName)).Value = vValue
        If Err.Number Then
            On Error GoTo 0
            Err.Raise knErrCannotWrite, ksErrSource, sName & ksErrCannotWrite
        End If
    Else
        ' Create variable if it doesn't exist.
        m_colVariables.Item(MakeKey(sName)).Value = vValue
        If Err Then
            Add sName, vValue
            Err.Clear
        End If
    End If

End Property

' Attributes: Hidden, Enumerator
Public Function NewEnum() As IUnknown
Attribute NewEnum.VB_UserMemId = -4
Attribute NewEnum.VB_MemberFlags = "40"
    Set NewEnum = m_colVariables.[_NewEnum]
End Function

Public Sub Remove(ByVal sName As String)
    m_colVariables.Remove MakeKey(sName)
End Sub

Public Property Get CaseSensitive() As Boolean
    CaseSensitive = m_bCaseSensitive
End Property

Public Property Let CaseSensitive(ByVal bCaseSensitive As Boolean)
    m_bCaseSensitive = bCaseSensitive
End Property

Public Property Get RequireDeclaration() As Boolean
    RequireDeclaration = m_bRequireDeclaration
End Property

Public Property Let RequireDeclaration(ByVal bRequireDeclaration As Boolean)
    m_bRequireDeclaration = bRequireDeclaration
End Property

Public Property Get RequireInitialisation() As Boolean
    RequireInitialisation = m_bRequireInitialisation
End Property

Public Property Let RequireInitialisation(ByVal bRequireInitialisation As Boolean)
    m_bRequireInitialisation = bRequireInitialisation
End Property

Private Sub Class_Initialize()
    Set m_colVariables = New Collection
    m_bCaseSensitive = False
    m_bRequireDeclaration = False
    m_bRequireInitialisation = False
End Sub

Private Sub Class_Terminate()
    Set m_colVariables = Nothing
End Sub

Private Function MakeKey(ByVal sName As String) As String
    If m_bCaseSensitive Then
        MakeKey = sName
    Else
        MakeKey = UCase$(sName)
    End If
End Function
