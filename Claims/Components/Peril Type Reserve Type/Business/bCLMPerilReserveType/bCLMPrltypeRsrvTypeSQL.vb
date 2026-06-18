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
	' Description   :Contains the SQL Statements required by the
	'                  bCLMDefnFlds.Business class.
	' Edit History: DG
	' ***************************************************************** '
	
	' Get All Reserve Types
	Public Const ACGetAllReserveTypesStored As Boolean = True
    Public Const ACGetAllReserveTypesName As String = "GetAllReserveTypes"
    'Developer Guide No:39
    Public Const ACGetAllReserveTypesSQL As String = "spu_get_all_reserve_types"
	
	' Check if Reserve type is defined for a Peril Type
	Public Const ACChckRsrvTypExstInPrlRsrTypStored As Boolean = True
    Public Const ACChckRsrvTypExstInPrlRsrTypName As String = "ChckRsrvTypExstInPrlRsrTyp"
    'Developer Guide No:39
    Public Const ACChckRsrvTypExstInPrlRsrTypSQL As String = "spu_ChckRsrvTypExstInPrlRsrTyp"
	
	' Get reserve types for peril types
	Public Const ACGetRsrvTypForPrlTypStored As Boolean = True
    Public Const ACGetRsrvTypForPrlTypName As String = "GetRsrvTypForPrlTyp"
    'Developer Guide No:39
    Public Const ACGetRsrvTypForPrlTypSQL As String = "spu_GetRsrvTypForPrlTyp"

	
	
	' Check if Reserves  are defined for a Peril Type & Reserve type
	Public Const ACChckDelForPrlRskTypStored As Boolean = True
    Public Const ACChckDelForPrlRskTypName As String = "ChckDelForPrlRskTyp"
    'Developer Guide No:39
    Public Const ACChckDelForPrlRskTypSQL As String = "spu_ChckDelForPrlRskTyp"

End Module