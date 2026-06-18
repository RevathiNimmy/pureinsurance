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
	' Date: 30/04/2003
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRCoinsurance.Business class.
	'
	' Edit History: Created by Alix Bergeret
	' ***************************************************************** '
	
	'SQL Statements
	
	Public Const ACSelectTaxGroupTaxBandsStored As Boolean = True
	Public Const ACSelectTaxGroupTaxBandsName As String = "SelectTaxGroupTaxBands"
	Public Const ACSelectTaxGroupTaxBandsSQL As String = "spu_taxgroup_taxbands_sel"
	
	Public Const ACDeleteTaxGroupTaxBandsStored As Boolean = True
	Public Const ACDeleteTaxGroupTaxBandsName As String = "DeleteTaxGroupTaxBands"
	Public Const ACDeleteTaxGroupTaxBandsSQL As String = "spu_taxgroup_taxbands_del"
	
	Public Const ACInsertTaxGroupTaxBandsStored As Boolean = True
	Public Const ACInsertTaxGroupTaxBandsName As String = "InsertTaxGroupTaxBands"
	Public Const ACInsertTaxGroupTaxBandsSQL As String = "spu_taxgroup_taxbands_add"
End Module