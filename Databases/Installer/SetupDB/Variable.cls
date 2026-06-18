VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CVariable"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    A variable and its value
' Shared:   Yes (RESTRICTED)
' Needs:    Nothing
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE SIRIUS DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
' Generic module to handle an unlimited collection of variables of
' any primitive type. It makes no assumptions about how they are used.
'
Option Explicit

Private m_sName As String
Private m_vValue As Variant

Public Property Get Name() As String
    Name = m_sName
End Property

Public Property Let Name(ByVal sName As String)
    m_sName = sName
End Property

Public Property Get Value() As Variant
    Value = m_vValue
End Property

Public Property Let Value(ByVal vValue As Variant)
    m_vValue = vValue
End Property
