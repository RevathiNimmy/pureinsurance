Option Strict Off
Option Explicit On
Imports System
Module SQLConstants
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Public Const ACGetDocumentTemplateDescStored As Boolean = True
	Public Const ACGetDocumentTemplateDescName As String = "SelectAllDocumentTemplate"
	Public Const ACGetDocumentTemplateDescSQL As String = "spu_get_document_description"
End Module