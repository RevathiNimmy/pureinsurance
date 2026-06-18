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
	' DJM 07/05/2002 : To Allow changing of address using address usage type combobox
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'developer guide no.39
    'start
	' Select All SIRAddress SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllSIRAddress"
    Public Const ACGetAllDetailsSQL As String = "spe_Address_saa"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
	Public Const ACCheckIDName As String = "CheckSIRAddressID"
    Public Const ACCheckIDSQL As String = "spe_SIRAddress_check_id"
	
	'DJM 07/05/2002 : To Allow changing of address using address usage type combobox
	Public Const ACGetPartyAddressStored As Boolean = True
	Public Const ACGetPartyAddressName As String = "GetPartyAddress"
    Public Const ACGetPartyAddressSQL As String = "sp_get_party_address"
    'end
End Module