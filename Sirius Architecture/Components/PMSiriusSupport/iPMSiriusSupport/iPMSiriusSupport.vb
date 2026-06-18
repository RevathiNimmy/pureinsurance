Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "iPMSiriusSupport"
	Private Const ACClass As String = "MainModule"
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPassword As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
	
	Sub Main_Renamed()
	End Sub
End Module