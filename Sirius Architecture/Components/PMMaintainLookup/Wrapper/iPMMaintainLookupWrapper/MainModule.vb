Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' Edit History:
	' DAK251199 - Make business object global
	' ***************************************************************** '
	
	Public Const ACApp As String = "iPMMaintainLookupWrapper"
	
    ' Instance of the main form
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_frmMain As frmMain

    ' Instance of object manager
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Instance of the business
    'DAK251199

    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oBusiness As bPMMaintainLookupWrapper.Business
End Module