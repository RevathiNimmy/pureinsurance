Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Module MainModule
	Public Const ACApp As String = "iPMBPartyWrapper"
	Public Const ACClass As String = "MainModule"
	
	Public Const ACPartyGC As String = "GC"
	Public Const ACPartyCC As String = "CC"
	Public Const ACPartyPC As String = "PC"
	
    ' Instance of object manager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module