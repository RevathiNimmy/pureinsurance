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
	' Date: 07/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIREvent.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Get All Text File Descriptions SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "GetTextFileDescription"
	Public Const ACGetAllDetailsSQL As String = "{call spu_text_file_description_sel (?)}"
	
	' Delete All Text File Descriptions SQL
	Public Const ACDeleteAllDetailsStored As Boolean = True
	Public Const ACDeleteAllDetailsName As String = "DeleteTextFileDescription"
	Public Const ACDeleteAllDetailsSQL As String = "{call spu_text_file_description_del (?)}"
	
	' Insert Text File Description SQL
	Public Const ACInsertTFDStored As Boolean = True
	Public Const ACInsertTFDName As String = "InsertTextFileDescription"
	Public Const ACInsertTFDSQL As String = "{call spe_text_file_description_add (?,?,?,?)}"
End Module