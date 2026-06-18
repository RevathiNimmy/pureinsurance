VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CUpgradeOrderRow"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private m_sName As String
Private m_sVersion As String

Public Property Get Name() As String
    Name = m_sName
End Property

Public Property Let Name(ByVal sName As String)
    m_sName = sName
End Property

Public Property Get Version() As String
    Version = m_sVersion
End Property

Public Property Let Version(ByVal sVersion As String)
    m_sVersion = sVersion
End Property
