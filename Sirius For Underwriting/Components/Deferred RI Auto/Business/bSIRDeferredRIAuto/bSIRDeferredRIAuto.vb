Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
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
	' Date:  26/09/00
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRRenewal"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	'Constants for Renewal Statuses
	Public Const ACAwaitManReview As Integer = 1
	Public Const ACAwaitRenewalPrint As Integer = 2
	Public Const ACAwaitManRatingFail As Integer = 3
	Public Const ACPolicyDetailsChanged As Integer = 4
	Public Const ACAwaitUpdate As Integer = 5
	Public Const ACAwaitManRating As Integer = 6
	
	Sub Main_Renamed()
		
		
	End Sub
End Module