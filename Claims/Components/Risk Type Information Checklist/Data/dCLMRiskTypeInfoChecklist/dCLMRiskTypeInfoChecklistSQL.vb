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
	' Class Name:   SQL
	' Description:  Contains the SQL Statements required by the
	'               CLMRTInfoChkLst class.
	' Author:       SK
	' Date:         06/07/2000
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Add AddCLMRiskType_ExpSer SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCLMRiskType_ExpSer"
    'Developer Guide no. 39
    'start
    Public Const ACAddSQL As String = "spu_Rsk_Type_Exp_Ser_add"
	
	' Add DeleteCLMRiskType_ExpSer SQL
	Public Const ACDelStored As Boolean = True
	Public Const ACDelName As String = "DeleteCLMRiskType_ExpSer"
    Public Const ACDelSQL As String = "spu_Rsk_Type_Exp_Ser_del"
    'end
End Module