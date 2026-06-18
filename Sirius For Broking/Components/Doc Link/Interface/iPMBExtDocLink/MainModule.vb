Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "iPMBExtDocLink"
	Public Const ACClass As String = "MainModule"
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module