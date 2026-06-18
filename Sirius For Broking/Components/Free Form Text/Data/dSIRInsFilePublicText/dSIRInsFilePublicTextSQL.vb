Option Strict Off
Option Explicit On
Imports System
Module SQL
	' ***************************************************************** '
	' Class Name: SQL
	'
	' Date: 08/09/1998
	'
	' Description: Contains the SQL Statements required by the
	'              InsFilePublicText class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select InsFilePublicText SQL
	Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleInsFilePublicText"
    'Developer Guide No 39
    'Public Const ACSelectSingleSQL As String = "{call spe_Ins_File_Public_Text_sel (?)}"
    Public Const ACSelectSingleSQL As String = "spe_Ins_File_Public_Text_sel"
	
	' Add InsFilePublicText SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddInsFilePublicText"
    'Developer Guide No 39
    'Public Const ACAddSQL As String = "{call spe_Ins_File_Public_Text_add (?,?,?)}"
    Public Const ACAddSQL As String = "spe_Ins_File_Public_Text_add"
	
	' Delete InsFilePublicText SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteInsFilePublicText"
    'Developer Guide No 39
    'Public Const ACDeleteSQL As String = "{call spe_Ins_File_Public_Text_del (?)}"
    Public Const ACDeleteSQL As String = "spe_Ins_File_Public_Text_del"
	
	' Update InsFilePublicText SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateInsFilePublicText"
    'Developer Guide No 39
    'Public Const ACUpdateSQL As String = "{call spe_Ins_File_Public_Text_upd (?,?,?)}"
    Public Const ACUpdateSQL As String = "spe_Ins_File_Public_Text_upd"
End Module