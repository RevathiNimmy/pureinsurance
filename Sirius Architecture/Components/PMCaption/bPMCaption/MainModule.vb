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
	' Date:  17 October 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bPMCaption"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	' UserID
	
    ' CTAF 181200
    'developers guid no. 39
    Public Const ACGetCaptionDescSQL As String = "spu_pm_caption_desc"
	Public Const ACGetCaptionDescName As String = "GetCaptionDesc"
	Public Const ACGetCaptionDescStored As Boolean = True

    'developers guid no. 39
    Public Const ACGetCaptionSQL As String = "spu_pm_caption"
	Public Const ACGetCaptionName As String = "GetCaption"
	Public Const ACGetCaptionStored As Boolean = True
	
	'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)
	Public Const kUSLangId As Integer = 2
	Public Const kUKLangId As Integer = 1
	'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)
End Module