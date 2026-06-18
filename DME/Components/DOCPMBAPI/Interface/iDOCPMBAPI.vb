Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	'Include for Sirius Compliance
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iDOCPMBAPI"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	

	Public g_oBusiness As bDOCPMBAPI.Form
	Public g_iLanguageID As Integer
	Public g_iSourceID As Integer
	Public g_oObjectManager As bObjectManager.ObjectManager
	Public g_oPMBLog As Object
End Module