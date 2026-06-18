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
	Public Const ACApp As String = "iDOCInformation"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	Public lDocNum As Integer
	
	' {* USER DEFINED CODE (End) *}
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_sUserName As String = ""
	Public g_sPassword As String = ""
	Public g_iUserID As Integer
	
	' Document being processed
	Public g_lDocNum As Integer
	
	'New Doc Name
	Public g_sNewName As String = ""
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Return value from function calls
	Private m_lReturn As Integer
	
	'PN -54730
	Public Const HWND_TOPMOST As Integer = -1
	Public Const SWP_NOMOVE As Integer = &H2s
	Public Const SWP_NOSIZE As Integer = &H1s
	'API to put a window on top of all screen 33223
	Public Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
	
	
	Sub Main_Renamed()
		
	End Sub
	'PN -54730
	Public Function SetWinPos(ByRef lHWnd As Integer) As Boolean
		' Run the API SetWindowPos function
		Dim result As Boolean = False
		If SetWindowPos(lHWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE + SWP_NOSIZE) Then
			' If the function is greater than 0 (FALSE) then the operation was successful.  Return a True for to indicate such.
			result = True ' True or false can be returned from this function, but not absolutely necessary
		End If
		Return result
	End Function
End Module