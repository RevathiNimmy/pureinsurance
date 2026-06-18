VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CParameters"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private m_col As Collection

' Parse a complete parameter string into the collection.
Public Sub Parse(ByVal sParameters As String)

    Dim sParameter As String
    Dim sName As String
    Dim sValue As String
    Dim oParameter As CParameter

    ' Parse each name/value pair in turn and add to the parameters collection.
    ' Stop when we run out of parameters. Use the existing ParseCSV() function
    ' to maintain backward compatibility with the command line of previous versions
    ' of setupdb. If the equals sign does not exist within a parameter, assume the
    ' name is blank, not the value. This enables us to easily tell the differemce
    ' between old and new command lines.
    sParameters = Trim$(sParameters)
    Do Until sParameters = ""
        sParameter = Trim$(ParseCSV(sParameters, sParameters))
        sName = Trim$(ParseSep(sParameter, sValue, "=", True))
        sValue = Trim$(sValue)
        Set oParameter = New CParameter
        oParameter.Text = sParameter
        oParameter.Name = sName
        oParameter.Value = sValue
        m_col.Add oParameter
        Set oParameter = Nothing
    Loop

End Sub

' Destructive read of named parameter out of the collection.
Public Function ReadByName(ByVal sName As String, _
    Optional ByRef r_bExists As Boolean) As String

    Dim oParameter As CParameter
    Dim iIndex As Long

    ReadByName = ""
    r_bExists = False
    sName = LCase$(Trim$(sName))
    iIndex = 0
    For Each oParameter In m_col
        iIndex = iIndex + 1
        If oParameter.NameTest = sName Then
            ReadByName = oParameter.Value
            r_bExists = True
            Exit For
        End If
    Next
    If r_bExists Then
        m_col.Remove iIndex
    End If

End Function

' Destructive read of the first parameter out of the collection.
Public Function ReadFirst(Optional ByRef r_bExists As Boolean) As String

    Dim oParameter As CParameter

    ReadFirst = ""
    r_bExists = False
    If m_col.Count > 0 Then
        Set oParameter = m_col.Item(1)
        ReadFirst = oParameter.Value
        Set oParameter = Nothing
        r_bExists = True
        m_col.Remove 1
    End If

End Function

' Retrieves data for display by the {CTRL+F12} debugging key.
Public Function DebugMessagePopup() As String

    Dim oParameter As CParameter

    For Each oParameter In m_col
        If DebugMessagePopup <> "" Then DebugMessagePopup = DebugMessagePopup & ", "
        DebugMessagePopup = DebugMessagePopup & oParameter.Text
    Next

End Function

' Attributes: Default
Public Property Get Item(ByVal iIndex As Long) As String
Attribute Item.VB_UserMemId = 0
    Item = m_col.Item(iIndex)
End Property

' Attributes: Enumerator, Hidden
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
