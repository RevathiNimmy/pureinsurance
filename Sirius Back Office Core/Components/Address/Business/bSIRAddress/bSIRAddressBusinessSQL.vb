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
	' Date: 08/10/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRAddress.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All SIRAddress SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllSIRAddress"
    'developer guide no. 39
    Public Const ACGetAllDetailsSQL As String = "spe_Address_saa"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckSIRAddressID"
    'developer guide no. 39
    Public Const ACCheckIDSQL As String = "spe_SIRAddress_check_id"
	
	'CT 19/09/00
	' Insert accumulation SQL
	' RFC12/02/02 - Accumulation Add changed to a Stored Proc
	Public Const ACInsertAccumulationStored As Boolean = True
	Public Const ACInsertAccumulationName As String = "InsertAccumulation"
    'developer guide no. 39
    Public Const ACInsertAccumulationSQL As String = "spu_accumulation_add"
	
	'PSL 21/02/2003
	Public Const ACMultipleUseStored As Boolean = True
	Public Const ACMultipleUseName As String = "MultipleUse"
    'developer guide no. 39
    Public Const ACMultipleUseSQL As String = "spu_Address_MultipleUse"
	Public Const ACDuplicateAddressStored As Boolean = True
	Public Const ACDuplicateAddressName As String = "Duplicate"
	'developer guide no. 39
	Public Const ACDuplicateAddressSQL As String = "spu_Address_Duplicate"
End Module