VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ISQLScriptEvents"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' Class:    SPP file execution events
' Shared:   PublicNotCreatable, Yes (RESTRICTED)
'
' This interface provides a faster alternative to handling the events.
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE SIRIUS DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
Option Explicit

Public Sub ProcessFile(ByVal sFile As String)

End Sub

Public Sub ProcessStatement(ByVal nStatement As Long, ByVal sSQL As String, ByRef r_bProcess As Boolean)

End Sub

Public Sub PromptForValue(ByVal sPromptText As String, ByVal vDefault As Variant, ByRef r_vValue As Variant)

End Sub
