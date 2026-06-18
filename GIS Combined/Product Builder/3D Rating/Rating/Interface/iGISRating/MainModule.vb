Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "iGISRating"
	
	Public Const ACClass As String = "MainModule"
	
    ' Object Manager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Business object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bGISRating.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lGISSchemeID As Integer
	
	' Array values
	Public Const ACGRTArrayRateTypeID As Integer = 0
	Public Const ACGRTArrayDescription As Integer = 1
	Public Const ACGRTArrayCode1 As Integer = 2
	Public Const ACGRTArrayDescription1 As Integer = 3
	Public Const ACGRTArrayCode2 As Integer = 4
	Public Const ACGRTArrayDescription2 As Integer = 5
	Public Const ACGRTArrayCode3 As Integer = 6
	Public Const ACGRTArrayDescription3 As Integer = 7
End Module