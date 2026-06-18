Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module FindProductTypeSql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Class Name: FindPolicyTypeSQL
	'
	' Date: 27th September 1996
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate an FindPolicyType
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectEvent"
	' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"
	
	
	Public Const ACSelectStored As Boolean = False
	Public Const ACSelectName As String = "SelectTypes"
    Public Const ACSelectSQL As String = "select pt.product_id," & Strings.Chr(13) & Strings.Chr(10) & _
                                         "c.Caption" & Strings.Chr(13) & Strings.Chr(10) & _
                                         "from product pt," & Strings.Chr(13) & Strings.Chr(10) & _
                                         "pmcaption c" & Strings.Chr(13) & Strings.Chr(10) & _
                                         "Where pt.caption_id = c.caption_id" & Strings.Chr(13) & Strings.Chr(10) & _
                                         "and c.language_id = {language_id}" & Strings.Chr(13) & Strings.Chr(10) & _
                                         "and pt.is_deleted = 0" & Strings.Chr(13) & Strings.Chr(10) & _
                                         "and (pt.product_id in " & _
                                         "(SELECT product_id from Product_source where source_id not in " & _
                                         "(select Source_id from PMUser_Source where user_id={user_id})) or {user_id}=0) " & _
                                             "Order by 2"
	'Tomo310801 - Was ordering by pt.description, but that may not be the same as the caption...
	'And this change was never released...
	
	Public Const ACProductByAgentStored As Boolean = True
    Public Const ACProductByAgentName As String = "ProductByAgent"
    'Modified by arun.kumar on 6/20/2010 4:09:09 PM refer developer guide no 39. 
    'Public Const ACProductByAgentSQL As String = "{call spu_agent_product_usage_sel(?)}"
    Public Const ACProductByAgentSQL As String = "spu_agent_product_usage_sel"
End Module