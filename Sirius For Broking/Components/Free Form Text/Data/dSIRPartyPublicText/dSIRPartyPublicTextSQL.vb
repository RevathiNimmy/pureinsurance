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
	'              PartyPublicText class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select PartyPublicText SQL
	Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSinglePartyPublicText"
    'developer guide no. 39
    Public Const ACSelectSingleSQL As String = "spe_Party_Public_Text_sel"
	
	' Add PartyPublicText SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddPartyPublicText"
    Public Const ACAddSQL As String = "spe_Party_Public_Text_add"
	
	' Delete PartyPublicText SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeletePartyPublicText"
    Public Const ACDeleteSQL As String = "spe_Party_Public_Text_del"
	
	' Update PartyPublicText SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdatePartyPublicText"
    Public Const ACUpdateSQL As String = "spe_Party_Public_Text_upd"
End Module