Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	'
	' Module Name   : MainModule
	'
	' Date          : 24-10-2002
	'
	' Author        : Ram Chandrabose
	'
	' Description   : Main module containing public variable/constants.
	'
	' Edit History  :
	' RAM20021024   : Created
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMURiskWrapper"
	
	Public Const SW_SHOWDEFAULT As Integer = 10
	Public Const SWP_NOMOVE As Integer = 2
	Public Const SWP_NOSIZE As Integer = 1
	Public Const FLAGS As Integer = SWP_NOMOVE Or SWP_NOSIZE
	Public Const HWND_TOPMOST As Integer = -1
	Public Const HWND_NOTOPMOST As Integer = -2

	Public Const SWP_SHOWWINDOW As Integer = &H40s
	Public Const SW_SHOW As Integer = 5
	Public Const SW_SHOWNORMAL As Integer = 1
	Public Const SW_SHOWMAXIMIZED As Integer = 3
	
	Public Declare Function FindWindow Lib "user32"  Alias "FindWindowA"(ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
	
	Public Declare Function ShowWindow Lib "user32" (ByVal hwnd As Integer, ByVal nCmdShow As Integer) As Integer
	
	
	Public Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
	
	Public Declare Function SetFocus Lib "user32" (ByVal hwnd As Integer) As Integer
	
	Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Integer) As Integer
End Module