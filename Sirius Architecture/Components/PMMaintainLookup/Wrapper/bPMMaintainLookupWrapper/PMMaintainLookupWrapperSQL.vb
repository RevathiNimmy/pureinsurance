Option Strict Off
Option Explicit On
Imports System
Module PMMaintainLookupWrapperSQL
	
	' Get a Product's tables
	Public Const ACGetTablesStored As Boolean = True
    Public Const ACGetTablesName As String = "GetTables"
    ' developer guide no. 39
    Public Const ACGetTablesSQL As String = "spu_pm_lookup_table_sel"
	
	' Get selected tabel
	Public Const ACGetTableStored As Boolean = True
    Public Const ACGetTableName As String = "GetTableDets"
    ' developer guide no. 39
    Public Const ACGetTableSQL As String = "spu_pm_lookup_table_sel_all"
End Module