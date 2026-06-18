Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 29/10/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bACTInvoice.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All ACTInvoice SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllACTInvoice"
    'Developer Guide No. 39
    Public Const ACGetAllDetailsSQL As String = "spe_Invoice_saa"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckACTInvoiceID"
    'Developer Guide No. 39
    Public Const ACCheckIDSQL As String = "spe_ACTInvoice_check_id"
	
	' Get New Invoice_ID (no IDENTITY has been set on ID column)
	Public Const ACGetNewIDStored As Boolean = False
	Public Const ACGetNewIDName As String = "SelectNewInvoiceID"
	Public Const ACGetNewIDSQL As String = "SELECT MAX(invoice_id) MaxID FROM Invoice"
	
	' Get the account name
	Public Const ACGetAccountNameStored As Boolean = False
	Public Const ACGetAccountNameName As String = "GetAccountName"
	Public Const ACGetAccountNameSQL As String = "SELECT short_code FROM Account WHERE account_id = {account_id} "
	
	' Get the transdetail type id
	Public Const ACGetTransdetailTypeIdStored As Boolean = True
    Public Const ACGetTransdetailTypeIdName As String = "GetTransdetailTypeId"
    'Developer Guide No. 39
    Public Const ACGetTransdetailTypeIdSQL As String = "spu_ACT_Get_Transdetail_Type_Id"
	
	' Select ACTInvoice SQL
	Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleACTInvoice"
    'Developer Guide No. 39
    Public Const ACSelectSingleSQL As String = "spu_ACT_Select_Invoice"
	
	' Add ACTInvoice SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddACTInvoice"
    'Developer Guide No. 39
    Public Const ACAddSQL As String = "spu_ACT_Add_Invoice"
	
	' Delete ACTInvoice SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteACTInvoice"
    'Developer Guide No. 39
    Public Const ACDeleteSQL As String = "spu_ACT_Delete_Invoice"
	
	' Update ACTInvoice SQL
	Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateACTInvoice"
    'Developer Guide No. 39
    Public Const ACUpdateSQL As String = "spu_ACT_Update_Invoice"
End Module