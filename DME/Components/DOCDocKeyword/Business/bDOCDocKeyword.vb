Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Data
Imports System.Data.SqlClient
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  16/01/1998
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bDOCDocKeyword"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	Public g_sUsername As String = ""
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
	' User ID
	Public g_iUserID As Integer
	
	' Calling Application
	Public g_sCallingAppName As String = ""
	' Source ID
	Public g_iSourceID As Integer
	' Language ID
	Public g_iLanguageID As Integer
	' Currency ID
	Public g_iCurrencyID As Integer
	' LogLevel
	Public g_iLogLevel As Integer
	Public m_lreturn As Integer
	Sub Main_Renamed()
		
		'    Optional vDocKeywordID As Variant, _
		''    Optional vKeywordID As Variant, _
		''    Optional vDocNum As Variant, _
		''    Optional vUserName As Variant, _
		''    Optional vCreateDate As Variant) As Long
		'     sUserName As String, _
		''    sPassword As String, _
		''    iUserID As Integer, _
		''    iSourceID As Integer, _
		''    iLanguageID As Integer, _
		''    iCurrencyID As Integer, _
		''    iLogLevel As Integer, _
		''    sCallingAppName As String, _
		'
	End Sub
End Module