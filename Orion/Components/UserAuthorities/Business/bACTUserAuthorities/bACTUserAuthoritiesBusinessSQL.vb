Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 14/02/2000
	'
	' Description: Contains the SQL Statements required by the
	'              bACTUserAuthorities.Business class.
	'
	' Edit History:
	'               SET 090402 - definition of 'ACGetAllDetailsSQL' amended to use new
	'               stored procedure cos old procedure returned rows
	'               for deleted users.
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All ACTUserAuthorities SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "SelectAllACTUserAuthorities"
	
	' SET090402 - changed to call new stored procedure
    'Public Const ACGetAllDetailsSQL = "{call spe_User_Authorities_saa}"
    'Developer Guide No 39
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_UserAuthorities"
	' SET090402 - end of change
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckACTUserAuthoritiesID"
    'Developer Guide No 39
    Public Const ACCheckIDSQL As String = "spe_ACTUserAuthorities_check_id"
	'Get User WriteOff Permission and Amount
	Public Const ACGetUserWriteOffDetailsName As String = "ACGetUserWriteOffDetailsName"
    Public Const ACGetUserWriteOffDetailsSQL As String = "Select User_id, has_write_off_authority,write_off_amount from user_authorities where user_id={uid}"
End Module