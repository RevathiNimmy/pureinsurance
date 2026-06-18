Option Strict Off
Option Explicit On
Imports System
Module MainModule
	Public Const ACApp As String = "ClientManagerWrapper"
	
	Private Const ACClass As String = "MainModule"
	
    ' Instance of object manager
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module