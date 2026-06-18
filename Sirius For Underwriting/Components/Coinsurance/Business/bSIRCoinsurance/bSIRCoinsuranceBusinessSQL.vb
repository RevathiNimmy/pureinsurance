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
	'              bSIRCoinsurance.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select COI Arrangement SQL
	Public Const ACSelectCOIArrangementStored As Boolean = True
	Public Const ACSelectCOIArrangementName As String = "SelectCOIArrangement"
    'developer guide no 39. 
    Public Const ACSelectCOIArrangementSQL As String = "spe_COI_Arrangement_sel"
	
	' Select COI Default SQL
	Public Const ACSelectCOIDefaultStored As Boolean = True
	Public Const ACSelectCOIDefaultName As String = "SelectCOIDefault"
    'developer guide no 39. 
    Public Const ACSelectCOIDefaultSQL As String = "spu_COI_Default_saa"
	
	' Select COI Value SQL
	Public Const ACSelectCOIValueStored As Boolean = True
	Public Const ACSelectCOIValueName As String = "SelectCOIValue"
    'developer guide no 39. 
    Public Const ACSelectCOIValueSQL As String = "spu_COI_Value_saa"
	
	' Delete COI Arrangement SQL
	Public Const ACDeleteCOIArrangementStored As Boolean = True
	Public Const ACDeleteCOIArrangementName As String = "DeleteCOIArrangement"
    'developer guide no 39. 
    Public Const ACDeleteCOIArrangementSQL As String = "spe_COI_Arrangement_del"
	
	' Delete COI Value SQL
	Public Const ACDeleteCOIValueStored As Boolean = True
	Public Const ACDeleteCOIValueName As String = "DeleteCOIValue"
    'developer guide no 39.
    Public Const ACDeleteCOIValueSQL As String = "spu_COI_Value_del"
	
	' Insert COI Arrangement SQL
	Public Const ACInsertCOIArrangementStored As Boolean = True
	Public Const ACInsertCOIArrangementName As String = "InsertCOIArrangement"
    'developer guide no 39. 
    Public Const ACInsertCOIArrangementSQL As String = "spe_COI_Arrangement_add"
	
	' Insert COI Value SQL
	Public Const ACInsertCOIValueStored As Boolean = True
	Public Const ACInsertCOIValueName As String = "InsertCOIValue"
    'developer guide no 39. 
    Public Const ACInsertCOIValueSQL As String = "spe_COI_Value_add"
End Module