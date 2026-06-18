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
	' Date:  02/09/2000
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRRenSelection"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Const PMKeyNameInsFileCnt As String = "insurance_file_cnt"
    Public Const ACTKeyNameDocumentID As String = "document_id"
    Public Const PMKeyNameInsFolderCnt As String = "insurance_folder_cnt"
    Public Const PMKeyNamePartyCnt As String = "party_cnt"

    Sub Main_Renamed()

    End Sub
End Module