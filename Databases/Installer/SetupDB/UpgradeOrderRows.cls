VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CUpgradeOrderRows"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private m_col As Collection

Public Sub Add(ByVal sName As String, ByVal sVersion As String)

    Dim oRow As CUpgradeOrderRow

    Set oRow = New CUpgradeOrderRow
    oRow.Name = sName
    oRow.Version = sVersion
    m_col.Add oRow

End Sub

Public Property Get Count() As Long
    Count = m_col.Count
End Property

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
