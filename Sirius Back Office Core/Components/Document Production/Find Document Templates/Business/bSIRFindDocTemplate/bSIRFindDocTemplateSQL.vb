Option Strict Off
Option Explicit On
Imports System
Module FindDocTemplateSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FindDocTemplateSQL
	'
	' Date: 23rd October 1996
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate an FindInsurance
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectEvent"
	' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"
	
	' Find Insurance from built query.
	Public Const ACDocTemplateFromQueryStored As Boolean = False
	Public Const ACDocTemplateFromQueryName As String = "FindDocTemplateQuery"
	Public Const ACDocTemplateFromQuerySQL As String = "{}"
	
	' Find Templates from risk type.
	' PW190603 - CQ1228 - add code and source parameters
	Public Const ACDocTemplateFromRiskTypeStored As Boolean = True
	Public Const ACDocTemplateFromRiskTypeName As String = "DocTemplateFromRiskType"
	Public Const ACDocTemplateFromRiskTypeSQL As String = "spu_Risk_Type_Linked_Clauses_Sel"
	
	' CTAF 20030624 - Can't seem to branch or roll back this component so created alternate
	'                 constants for 1.8.6. This SQL.BAS is shared, but the Form.cls ISNT'
	'                 so consider this when making changes in here...
	Public Const ACDocTemplateFromRiskTypeStored186 As Boolean = True
	Public Const ACDocTemplateFromRiskTypeName186 As String = "DocTemplateFromRiskType"
	Public Const ACDocTemplateFromRiskTypeSQL186 As String = "spu_Risk_Type_Linked_Clauses_Sel"
	
	
	' Find Templates from product.
	Public Const ACDocTemplateFromProductStored As Boolean = True
	Public Const ACDocTemplateFromProductName As String = "DocTemplateFromProduct"
	Public Const ACDocTemplateFromProductSQL As String = "spu_Product_Linked_Clauses_Sel"
	
	' Find branch for insurance file count
	Public Const ACGetInsuranceFileStored As Boolean = True
	Public Const ACGetInsuranceFileName As String = "GetInsuranceFile"
	Public Const ACGetInsuranceFileSQL As String = "spe_Insurance_File_Sel"
	
	'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)-(5.2.1)
	Public Const ACSELProductTypeLinkedClausesSQL As String = "Spu_Product_Linked_Clauses_Sel"
	Public Const ACSELProductTypeLinkedClausesSQLName As String = "Select the Product Type with Linked Clause details"
	
	Public Const ACSELRisktypeTypeLinkedClausesSQL As String = "Spu_Risk_Type_Linked_Clauses_Sel"
	Public Const ACSELRisktypeTypeLinkedClausesSQLName As String = "Select the Risk Type with Linked Clause details"
	Public Const AcSELRiskORProductStored As Boolean = True
	
	'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)-(5.2.1)
End Module