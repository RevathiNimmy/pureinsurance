Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 08/09/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTBank.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select Bank SQL
	Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectBank"
    'Developer Guide no.39
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_Bank"
	
	' Select All Bank SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllBank"
    'Developer Guide no.39
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_Bank"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckBankID"
    'Developer Guide no.39
    Public Const ACCheckIDSQL As String = "spu_ACT_Check_Bank"
	
	' Add Bank SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddBank"

	Public Const ACAddSQL As String = "spu_ACT_Add_Bank"
	
	' Delete Bank SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteBank"
    'Developer Guide no.39
    Public Const ACDeleteSQL As String = "spu_ACT_Delete_Bank"
	
	' Update Bank SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateBank"
	Public Const ACUpdateSQL As String = "spu_ACT_Update_Bank"
	
	' Get Account Holder details SQL
	Public Const ACGetHeadOfficeDetailsStored As Boolean = False
	Public Const ACGetHeadOfficeDetailsName As String = "GETHEADOFFICEDETAILS"
	Public Const ACGetHeadOfficeDetailsSQL As String = "SELECT bank_name FROM bank WHERE bank_id = "
	
	' Get bank account details
	Public Const ACGetBankAccountStored As Boolean = True
    Public Const ACGetBankAccountName As String = "GetBankAccounts"
    'Developer Guide no.39
    Public Const ACGetBankAccountSQL As String = "spu_ACT_Get_BankAccount"
	
	' Get Bank ID SQL
	'MKR 27/10/2004 PN 13451 --start
	Public Const ACGetBankIdStored As Boolean = True
    Public Const ACGetBankIdName As String = "GETBANKID"
    'Developer Guide no.39
    Public Const ACGetBankIdSQL As String = "spu_ACT_GetBankByShortCode"
	'MKR 27/10/2004 PN 13451 --end
	
	' Delete BankAccount SQL
	Public Const ACDeleteBankAccountStored As Boolean = True
    Public Const ACDeleteBankAccountName As String = "DeleteBankAccount"
    'Developer Guide no.39
    Public Const ACDeleteBankAccountSQL As String = "spu_ACT_Delete_BankAccount"

End Module