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
	' Class Name    :BusinessSQL
	' Date          :16/08/2000
	' Description   :Contains the SQL Statements required by the
	'               bCLMResvDefn.Business class.
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All GetClmForResvType SQL
	Public Const ACGetClmForResvTypeStored As Boolean = True
	Public Const ACGetClmForResvTypeName As String = "GetClmForResvType"
    Public Const ACGetClmForResvTypeSQL As String = "spu_get_clm_for_resv_type"
	
	' Select All ResvTypes SQL
	Public Const ACGetReserveTypesStored As Boolean = True
	Public Const ACGetReserveTypesName As String = "GetResvTypes"
    Public Const ACGetReserveTypesSQL As String = "spu_get_resv_types"
	
	' Select All ChkResvTypeNameExists SQL
	Public Const ACChkResvTypeNameExistsStored As Boolean = True
	Public Const ACChkResvTypeNameExistsName As String = "ChkResvTypeNameExists"
    Public Const ACChkResvTypeNameExistsSQL As String = "spu_chk_resv_type_name_exists"
	
	
	
	
	'*******************************
	'' Check ID SQL
	'Public Const ACCheckIDStored = True
	'Public Const ACCheckIDName = "CheckCLMResvDefnID"
	'Public Const ACCheckIDSQL = "{call spe_CLMResvDefn_check_id (?)}"
End Module