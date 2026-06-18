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
	
	' Select All SIREvent SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIREvent"
    'developer guide no. 39
    Public Const ACGetAllDetailsSQL As String = "spe_event_log_saa"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIREventID"
    'developer guide no. 39
    Public Const ACCheckIDSQL As String = "spe_event_log_check_id"
	
	'DJM 24/10/2003
	'Select all of the slot and file numbers for a particular entity.
	Public Const ACTextFilesSelAllStored As Boolean = True
    Public Const ACTextFilesSelAllName As String = "TextFilesSelAll"
    'developer guide no. 39
    Public Const ACTextFilesSelAllSQL As String = "spu_Text_Files_Sel_All"
	
	'DJM 24/10/2003
	'Create new text files for copied policy in same slots as the original.
	Public Const ACTextFilesCopyStored As Boolean = True
    Public Const ACTextFilesCopyName As String = "TextFilesCopy"
    'developer guide no. 39
    Public Const ACTextFilesCopySQL As String = "spu_Text_Files_Copy"
	
	'DJM 25/02/2004
	'Get the default document template
	Public Const ACTextFileTemplateStored As Boolean = True
    Public Const ACTextFileTemplateName As String = "TextFileTemplate"
    'developer guide no. 39
    Public Const ACTextFileTemplateSQL As String = "spu_text_file_template"
End Module