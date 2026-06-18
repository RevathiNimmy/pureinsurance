Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	
	Public Const ACApp As String = "iSIRClientTransClientSel"
	
	' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' MKW 190503 PN2032 START
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
	
	Private m_lReturn As Integer
End Module