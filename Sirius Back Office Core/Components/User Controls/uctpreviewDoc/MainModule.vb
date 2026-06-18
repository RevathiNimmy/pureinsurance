Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "uctPreviewDoc"
	Private Const ACClass As String = "MainModule"
	
    ' Public instance of the object manager.

    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module