Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 20/09/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMCreateClaimDiary"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	' Form
	' Buttons
	' Messages
	' Menus
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_sUserName As String = ""
	Public g_sPassword As String = ""
	Public g_iUserID As Integer
	Public g_iSourceID As Integer
	Public g_iCurrencyID As Integer
	Public g_iLanguageID As Integer
	Public g_iLogLevel As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Sub Main_Renamed()
		
	End Sub
End Module