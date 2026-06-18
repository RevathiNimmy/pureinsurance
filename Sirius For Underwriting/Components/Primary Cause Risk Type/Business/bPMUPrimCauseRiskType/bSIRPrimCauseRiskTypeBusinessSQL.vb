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
	'              bSIRPrimCauseRiskType.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select
    'developer guide no. 39
    'start

	Public Const ACSelectPrimaryCauseRiskTypeGroupStored As Boolean = True
	Public Const ACSelectPrimaryCauseRiskTypeGroupName As String = "SelectPrimaryCauseRiskTypeGroup"
    Public Const ACSelectPrimaryCauseRiskTypeGroupSQL As String = "spu_CLM_Get_PrimCause_RiskTypeGrp"
	
	' Delete
	Public Const ACDeletePrimaryCauseRiskTypeGroupStored As Boolean = True
	Public Const ACDeletePrimaryCauseRiskTypeGroupName As String = "DeletePrimaryCauseRiskTypeGroup"
    Public Const ACDeletePrimaryCauseRiskTypeGroupSQL As String = "spu_CLM_Delete_PrimCause_RiskTypeGrp"
	
	' Insert
	Public Const ACInsertPrimaryCauseRiskTypeGroupStored As Boolean = True
	Public Const ACInsertPrimaryCauseRiskTypeGroupName As String = "InsertPrimaryCauseRiskTypeGroup"
    Public Const ACInsertPrimaryCauseRiskTypeGroupSQL As String = "spu_CLM_Add_PrimCause_RiskTypeGrp"
	
	
	Public Const ACSaaRiskTypeGroupStored As Boolean = True
	Public Const ACSaaRiskTypeGroupName As String = "SelAllRiskTypeGroup"
    Public Const ACSaaRiskTypeGroupSQL As String = "spe_Risk_Type_Group_saa"
End Module