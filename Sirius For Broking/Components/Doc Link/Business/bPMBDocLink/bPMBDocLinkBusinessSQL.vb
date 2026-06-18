Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date:  01/02/2001
	'
	' Created By: Ajit Kumar
	'
	' Description: Contains the SQL Statements required by the
	'              bPMBDocLink.Business class.
	'
	' Edit History:
	'   26/06/2002 SJP  - Merged from Carole Nash into Broking
	' RAM20040224       - Code changes related to PN Issue 7408
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select All PMBDocLink SQL
	Public Const ACGetAllDocLinkStored As Boolean = True
	Public Const ACGetAllDocLinkName As String = "SelectAllPMBDocLink"
    'Developer Guide No 39
    Public Const ACGetAllDocLinkSQL As String = "spu_PMB_Doc_Link_sel"
	
	' Add PMBDocLink SQL
	Public Const ACInsDocLinkStored As Boolean = True
	Public Const ACInsDocLinkName As String = "AddPMBDocLink"
    'Developer Guide No 39
    Public Const ACInsDocLinkSQL As String = "spu_PMB_Doc_Link_Add"
	
	' Delete PMBDocLink SQL
	Public Const ACDelDocLinkStored As Boolean = True
	Public Const ACDelDocLinkName As String = "DeletePMBDocLink"
    'Developer Guide No 39
    Public Const ACDelDocLinkSQL As String = "spu_PMB_Doc_Link_Del"
	
	' Extract Risk Groups
	Public Const ACGetRiskGroupStored As Boolean = True
	Public Const ACGetRiskGroupName As String = "GetRiskGroup"
    'Developer Guide No 39
    Public Const ACGetRiskGroupSQL As String = "spu_PMB_Risk_Group_Sel"
	
	' Extract GIS Schemes
	Public Const ACGetGISSchemeStored As Boolean = True
	Public Const ACGetGISSchemeName As String = "GetGISSchemes"
    'Developer Guide No 39
    Public Const ACGetGISSchemeSQL As String = "spu_PMB_Scheme_Sel"
	
	'DC040702
	' Extract Agents
	Public Const ACGetAgentStored As Boolean = True
	Public Const ACGetAgentName As String = "GetAgents"
    'Developer Guide No 39
    Public Const ACGetAgentSQL As String = "spu_PMB_Agent_Sel"
	
	' Extract processes
	Public Const ACGetProcessStored As Boolean = True
	Public Const ACGetProcessName As String = "GetProcess"
    'Developer Guide No 39
    Public Const ACGetProcessSQL As String = "spu_PMB_Process_Type_Sel"
	
	' Extract Document Type
	Public Const ACGetDocTypeStored As Boolean = True
	Public Const ACGetDocTypeName As String = "GetDocType"
    'Developer Guide No 39
    Public Const ACGetDocTypeSQL As String = "spu_PMB_Doc_Type_Sel"
	
	' Extract Document Template
	Public Const ACGetDocTempStored As Boolean = True
	Public Const ACGetDocTempName As String = "GetDocTemp"
    'Developer Guide No 39
    Public Const ACGetDocTempSQL As String = "spu_PMB_Doc_Temp_Sel"
	
	'   SP 25/6/02 - added this for schemes.
	Public Const ACDocTemplateGetStored As Boolean = True
	Public Const ACDocTemplateGetName As String = "GetDocTemplate"
    ' CJB 300802 - Increased param count to 3 to fix parallel maint bug
    'Developer Guide No 39
    Public Const ACDocTemplateGetSQL As String = "spu_PMB_Doc_Temp_Get"
	
	'Renewal Printing
	'Update PMB_Doc_Link
	Public Const ACAddDocLinkStored As Boolean = True
	Public Const ACAddDocLinkName As String = "AddDocLink"
	Public Const ACAddDocLinkSQL As String = "spu_SIR_AddDocLink"
	
	'Update PMB_Doc_Link Records
	Public Const ACUpdateDocLinkStored As Boolean = True
	Public Const ACUpdateDocLinkName As String = "UpdateDocLink"
	Public Const ACUpdateDocLinkSQL As String = "spu_SIR_UpdDocLink"
	
	'Update PMB_Doc_Link Records
	Public Const ACDeleteDocLinkStored As Boolean = True
	Public Const ACDeleteDocLinkName As String = "DeleteDocLink"
	Public Const ACDeleteDocLinkSQL As String = "spu_SIR_DelDocLink"
	
	'Extract PMB_Doc_Link Records
	Public Const ACGetDocLinkStored As Boolean = True
	Public Const ACGetDocLinkName As String = "GetSFIDocLinks"
	Public Const ACGetDocLinkSQL As String = "spu_SIR_GetSFIDocLinks"
	
	'Extract PMB_Doc_Link DocumentTemplates
	Public Const ACGetSFIDocumnetTemplatesStored As Boolean = True
	Public Const ACGetSFIDocumnetTemplatesName As String = "GetSFIDocumnetTemplates"
	Public Const ACGetSFIDocumnetTemplatesSQL As String = "spu_SIR_GetSFIDocumnetTemplates"
	
	'Extract PMB_Doc_Link DocumentTemplatesForProcessType
	Public Const ACGetSFIDocumnetTemplatesForProcessTypeStored As Boolean = True
	Public Const ACGetSFIDocumnetTemplatesForProcessTypeName As String = "GetSFIDocumnetTemplatesForProcessType"
	Public Const ACGetSFIDocumnetTemplatesForProcessTypeSQL As String = "spu_SIR_GetSFIDocumnetTemplatesForProcessType"
	
	'Extract LookUp Values
	Public Const ACGetLookUpListStored As Boolean = True
	Public Const ACGetLookUpListName As String = "GetLookUpList"
	Public Const ACGetLookUpListSQL As String = "spu_Get_LookUp_List"
	
	Public Const ACGetProcessTypeStored As Boolean = True
	Public Const ACGetProcessTypeName As String = "GetProcessType"
	Public Const ACGetProcessTypeSQL As String = "spu_Get_Process_Type"
	
	Public Const ACGetProcessTypeDocsStored As Boolean = True
	Public Const ACGetProcessTypeDocsName As String = "GetProcessTypesDocsForFunctionalArea"
	Public Const ACGetProcessTypeDocsSQL As String = "spu_get_process_types_docs_ForFunctionalArea"
End Module