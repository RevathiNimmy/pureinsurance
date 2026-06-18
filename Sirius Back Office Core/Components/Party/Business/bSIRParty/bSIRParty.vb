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
	' Date:  12/10/1998
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRParty"
	
	' Constant for the functions to identify
	' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Const AC_PARTY_IsBordereauxAccount As Short = 0
    Public Const AC_PARTY_PremiumManagerId As Short = 1
	
	Sub Main_Renamed()


    End Sub
End Module