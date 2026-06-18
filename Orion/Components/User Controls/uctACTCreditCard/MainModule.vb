Option Strict Off
Option Explicit On
Imports System
Module MainModule
	'
	' History : CJB 04/01/2005 User Control created for Retail Logic Integration
	'
	
	Private Const ACClass As String = "MainModule"
	Public Const ACApp As String = "uctACTCreditCard"
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bACTCreditCard.Business
End Module