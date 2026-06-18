Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "iPMSystem"
	
	'Captions
	'Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACText1Caption As Integer = 101
	Public Const ACText2Caption As Integer = 102
	Public Const ACCurrencyCaption As Integer = 103
	'Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	'Messages
	Public Const ACBusinessFailTitle As Integer = 300
	Public Const ACBusinessFail As Integer = 301
	Public Const ACCancelDetailsTitle As Integer = 302
	Public Const ACCancelDetails As Integer = 303
	
    'Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    'Public source and language ID's from the Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUserName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
End Module