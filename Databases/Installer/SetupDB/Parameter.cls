VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CParameter"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private m_sText As String
Private m_sTextTest As String
Private m_sName As String
Private m_sNameTest As String
Private m_sValue As String

Public Property Get Text() As String
    Text = m_sText
End Property

Public Property Get TextTest() As String
    TextTest = m_sTextTest
End Property

Public Property Let Text(ByVal sText As String)
    m_sText = sText
    m_sTextTest = LCase$(Trim$(sText))
End Property

Public Property Get Name() As String
    Name = m_sName
End Property

Public Property Get NameTest() As String
    NameTest = m_sNameTest
End Property

Public Property Let Name(ByVal sName As String)
    m_sName = sName
    m_sNameTest = LCase$(Trim$(sName))
End Property

Public Property Get Value() As String
    Value = m_sValue
End Property

Public Property Let Value(ByVal sValue As String)
    m_sValue = sValue
End Property
