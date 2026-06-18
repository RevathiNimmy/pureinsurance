Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 08/08/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTDocument.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Document SQL
	Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectAllDocument"
    'Developer Guide No 39
    'Public Const ACGetDetailsSQL As String = "{call spu_ACT_select_Document (?)}"
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_Document"
	
	' Select All Document SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllDocument"
    'Public Const ACGetAllDetailsSQL As String = "{call spu_ACT_select_all_Document}"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_select_all_Document"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckDocumentID"
    'Public Const ACCheckIDSQL As String = "{call spu_ACT_check_Document (?)}"
    Public Const ACCheckIDSQL As String = "spu_ACT_check_Document"
	
	' Add Document SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddDocument"
	'sj 01/08/2002 - start
	'Public Const ACAddSQL = "{call spu_ACT_add_Document (?,?,?,?,?,?,?,?,?,?,?,?)}"
    'Public Const ACAddSQL As String = "{call spu_ACT_add_Document (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"
    Public Const ACAddSQL As String = "spu_ACT_add_Document"
	'sj 01/08/2002 - end
	
	' Delete Document SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteDocument"
    'Developer Guide No 39
    'Public Const ACDeleteSQL As String = "{call spu_ACT_delete_Document (?)}"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_Document"
	
	' Update Document SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateDocument"
	'sj 01/08/2002 - start
    'Public Const ACUpdateSQL = "{call spu_ACT_update_Document (?,?,?,?,?,?,?,?,?,?,?,?)}"
    'Developer Guide NO 39
    'Public Const ACUpdateSQL As String = "{call spu_ACT_update_Document (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}"
    Public Const ACUpdateSQL As String = "spu_ACT_update_Document"
	'sj 01/08/2002 - end
	
	Public Const ACACTNumberGroupSelStored As Boolean = True
	Public Const ACACTNumberGroupSelName As String = "ACTNumberGroupSel"
    'Developer Guide NO 39
    'Public Const ACACTNumberGroupSelSQL As String = "{call spe_ACTNumber_Group_sel (?)}"
    Public Const ACACTNumberGroupSelSQL As String = "spe_ACTNumber_Group_sel"
	
	Public Const ACACTNumberRangeSelStored As Boolean = True
	Public Const ACACTNumberRangeSelName As String = "ACTNumberRangeSel"
    'Developer Guide NO 39
    'Public Const ACACTNumberRangeSelSQL As String = "{call spe_ACTNumber_Range_sel (?)}"
    Public Const ACACTNumberRangeSelSQL As String = "spe_ACTNumber_Range_sel"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module