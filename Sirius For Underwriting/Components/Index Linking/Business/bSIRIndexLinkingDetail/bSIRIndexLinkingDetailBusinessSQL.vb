Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 09/06/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRIndexLinkingDetail.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select COI Arrangement SQL
	Public Const ACSaaIndexLinkingDetailStored As Boolean = True
    Public Const ACSaaIndexLinkingDetailName As String = "SaaIndexLinkingDetail"

    'Developer Guide No.: 39
    Public Const ACSaaIndexLinkingDetailSQL As String = "spu_index_linking_detail_saa"
	
	Public Const ACDelIndexLinkingDetailStored As Boolean = True
	Public Const ACDelIndexLinkingDetailName As String = "DelIndexLinkingDetail"

    'Developer Guide No.: 39
    Public Const ACDelIndexLinkingDetailSQL As String = "spu_index_linking_detail_del"
	
	Public Const ACAddIndexLinkingDetailStored As Boolean = True
	Public Const ACAddIndexLinkingDetailName As String = "AddIndexLinkingDetail"

    'Developer Guide No.: 39
    Public Const ACAddIndexLinkingDetailSQL As String = "spe_index_linking_detail_add"
End Module