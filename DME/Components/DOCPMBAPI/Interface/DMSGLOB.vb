Option Strict Off
Option Explicit On
Imports System
Module DMSGLOB
	
	'needed for database version checking
	Public Const DDB_VERSION As Integer = 1
	Public Const HDB_VERSION As Integer = 2
	
	Public g_iLoggedIn As Integer
	Public VERSIONNUMBER As String = ""
End Module