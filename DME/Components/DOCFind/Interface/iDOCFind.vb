Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {17/2/98}
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iDOCFind"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	' Documaster access level
	Public g_iDOCaccesslevel As Integer
	
	'Start folders for search
	Public g_lStartFoldNum As Integer
	Public g_sStartFoldName As String = ""
	
	'store found docs
	Public g_vDocsFound As Object
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	Private m_lReturn As Integer
	
	
	Sub Main_Renamed()
		'test code
		Dim v As Object
		
		
        Dim i As New Interface_Renamed
		
		m_lReturn = i.Initialise(7)
		
		m_lReturn = i.Find(25329, "ark", v)
		
		
		
	End Sub
End Module