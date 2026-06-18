Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	Public Const ACApp As String = "bSIRDocManagerWrapper"
	
	Public Declare Function FindWindow Lib "user32.dll"  Alias "FindWindowA"(ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
	Declare Function IsWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer
	
	Public Const IsBusinessObject As Integer = 1
End Module