Option Strict Off
Option Explicit On
Imports System
Module InterfaceMain
	
	' ***************************************************************** '
	' Module Name: InterfaceMain
	'
	' Date:
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "iPMEventTask"
	
	' Public source and language ID's from the Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Sub Main_Renamed()
		
	End Sub
End Module