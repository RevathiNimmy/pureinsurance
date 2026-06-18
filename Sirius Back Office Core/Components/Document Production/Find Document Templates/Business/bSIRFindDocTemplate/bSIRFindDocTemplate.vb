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
	' Date:  27th September 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRFindDocTemplate"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Constants for the search data array indexes.
	'EK 140199 Bug 206 List replaced to be compatible with interface
	
	Public Const ACIInsFileId As Integer = 0
	Public Const ACIInsFileSourceId As Integer = 1
	Public Const ACIInsFileCnt As Integer = 2
	Public Const ACIInsReference As Integer = 3
	Public Const ACIInsFolderName As Integer = 4
	Public Const ACIInsFileType As Integer = 5
	Public Const ACIInsuredLongName As Integer = 6
	Public Const ACIInsuredShortName As Integer = 7
	Public Const ACIInsuredId As Integer = 8
	Public Const ACIInsuredSourceId As Integer = 9
	Public Const ACILastModified As Integer = 10
	Public Const ACIInsHolderCnt As Integer = 11
	Public Const ACIInsFolderCnt As Integer = 12
	Public Const ACIProductID As Integer = 13
	Public Const ACIProductCode As Integer = 14
	Public Const ACIProductName As Integer = 15
	Public Const ACILeadAgentCnt As Integer = 16
	Public Const ACIDateCreated As Integer = 17
	Public Const ACIRegistration As Integer = 18 ' Tom 02/11/98
	
	'RWH(26/09/2000) RSAIB Process 28.
	'AJM 08/03/01 - get document description also
	'PSL 10/09/2003 Issue 6535 only four params
	Public Const ACGetDocumentTemplateSQL As String = "spu_get_document_template_saa"
	Public Const ACGetDocumentTemplateName As String = "GetDocumentTemplate"
	
	Public Const ACGetFutureDatedTemplateSQL As String = "spu_get_futuredated_template"
	Public Const ACGetFutureDatedTemplateName As String = "GetFutureDatedTemplate"
	Public Const ACGetFutureDatedTemplateStored As Boolean = True
	
	'Start -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)
	Public Enum ENClauseType
		ProductType = 1
		RiskType = 2
	End Enum
	'End -(Arul Stephen)-(TechSpec WR6 ClauseGrouping.doc)-(5.1.2)
	
	Sub Main_Renamed()
		
		' Main entry point for the component
		
	End Sub
End Module