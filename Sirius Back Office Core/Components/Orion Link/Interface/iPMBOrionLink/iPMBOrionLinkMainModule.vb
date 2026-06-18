Option Strict Off
Option Explicit On
Imports System
Imports System.Collections
Imports System.IO
Module MainModule
	
	Private Const ACClass As String = "MainModule"
	
	Public Const ACApp As String = "iPMBOrionLink"
	
    'eck PN6169
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
End Module