Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  08/10/1998
	'
	' Description: Main Module.
	'
	' Edit History:
	'   07062002 SJP - Created
	'   21062002 SJP - Added constants to file
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRHandlerUpdate"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    Sub Main_Renamed()


    End Sub
End Module