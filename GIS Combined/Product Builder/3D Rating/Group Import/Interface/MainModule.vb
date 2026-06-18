Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Private Const ACClass As String = "MainModule"
	Public Const ACApp As String = "iGisGroupImport"
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bGisGroupImport.Business
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
End Module