Option Strict Off
Option Explicit On

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
	' Date:  02/09/2000
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bCLMChangeClaimStatus"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'************
	' MEvans : 09-10-2003 : Continuation Tasks
	' step instance data array positions.
	Public Const ACStepInstanceId As Integer = 0
	Public Const ACStepInstanceTaskInstanceCnt As Integer = 1
	Public Const ACStepInstanceStepId As Integer = 2
	'************
	
	Public Const kSysOptDMEInstalled As Integer = 10
	Public Const kSysOptPaymentRefCheck As Integer = 5040
	
    Public Const kTaxTypeArrayPosCode As Integer = 1
    Public Const kTaxTypeArrayPosTaxAmount As Integer = 2
    Public Const kClaimDetailPostClaimsTaxes As Integer = 11

    Sub Main_Renamed()

    End Sub
End Module