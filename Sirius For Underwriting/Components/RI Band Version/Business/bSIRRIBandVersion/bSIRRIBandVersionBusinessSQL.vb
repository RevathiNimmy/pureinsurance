Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 03/01/2001
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRRIBandVersion.Business class.
	'
	' Edit History:
	' ***************************************************************** '


	' Select RI Band SQL
	Public Const ACSelectRIBandStored As Boolean = True
	Public Const ACSelectRIBandName As String = "SelectRIBand"
	Public Const ACSelectRIBandSQL As String = "spu_RI_Band_Version_saa"

	' Delete TBR Arrangement SQL
	Public Const ACDeleteRIBandStored As Boolean = True
	Public Const ACDeleteRIBandName As String = "DeleteRIBand"
	Public Const ACDeleteRIBandSQL As String = "spu_RI_Band_Version_del"

	''Insert TBR Arrangement SQL
	Public Const ACInsertRIBandVersionStored As Boolean = True
	Public Const ACInsertRIBandVersionName As String = "InsertRIBandRate"
	Public Const ACInsertRIBandVersionSQL As String = "spu_RI_Band_Version_add"

End Module