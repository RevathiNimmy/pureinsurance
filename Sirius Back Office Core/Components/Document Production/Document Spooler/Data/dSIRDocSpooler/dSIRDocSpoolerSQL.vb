Option Strict Off
Option Explicit On
Imports System
Module SQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: SQL
	'
	' Date: 07/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              SIRDocSpooler class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select SIRDocSpooler SQL
    'Developer Guide No 39

	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleSIRDocumentTemplate"

    Public Const ACSelectSingleSQL As String = "spe_document_spooler_sel"
	
	' Add SIRDocSpooler SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddSIRDocumentTemplate"
	'sj 15/10/2002 - start
	' PW230403 - add new fields required for Document Issuance changes
	'DC240603 -ISS4097 -added new parameter for source id
    Public Const ACAddSQL As String = "spe_document_spooler_add"
	'sj 15/10/2002 - end
	' Delete SIRDocSpooler SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteSIRDocumentTemplate"
  
    Public Const ACDeleteSQL As String = "spe_document_spooler_del"
	
	' Update SIRDocSpooler SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateSIRDocumentTemplate"
	' PW230403 - add new fields required for Document Issuance changes
    'DC240603 -ISS4097 -added new parameter for source id
    Public Const ACUpdateSQL As String = "spe_document_spooler_upd"
End Module