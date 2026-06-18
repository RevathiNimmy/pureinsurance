Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
    Public Const ACApp As String = "iPMBDocSpooler"

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iUserId As Integer

    ' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iTask As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sUserName As String = ""
End Module