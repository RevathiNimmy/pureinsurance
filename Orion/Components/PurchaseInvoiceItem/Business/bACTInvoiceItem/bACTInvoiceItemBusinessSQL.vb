Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 02/11/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bACTInvoiceItem.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select All ACTInvoiceItem SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllACTInvoiceItem"
    'Developer Guide No. 39
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_Select_InvoiceItemList"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckACTInvoiceItemID"
    'Developer Guide No. 39
    Public Const ACCheckIDSQL As String = "spe_ACTInvoiceItem_check_id"
	
	' Check ID SQL
	Public Const ACDeleteInvoiceStored As Boolean = True
    Public Const ACDeleteInvoiceName As String = "DeleteInvoiceItems"
    'Developer Guide No. 39
    Public Const ACDeleteInvoiceSQL As String = "spu_ACT_Delete_Invoice_Item"
	
	' Select ACTInvoiceItem SQL
	Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleACTInvoiceItem"
    'Developer Guide No. 39
    Public Const ACSelectSingleSQL As String = "spe_Invoice_Item_sel"
	
	' Add ACTInvoiceItem SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddACTInvoiceItem"
    'Developer Guide No. 39
    Public Const ACAddSQL As String = "spu_ACT_Add_Invoice_Item"
	
	' Delete ACTInvoiceItem SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteACTInvoiceItem"
    'Developer Guide No. 39
    Public Const ACDeleteSQL As String = "spu_ACT_Delete_Invoice_item"
	
	' Update ACTInvoiceItem SQL
	Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateACTInvoiceItem"
    'Developer Guide No. 39
    Public Const ACUpdateSQL As String = "spe_Invoice_Item_upd"
End Module