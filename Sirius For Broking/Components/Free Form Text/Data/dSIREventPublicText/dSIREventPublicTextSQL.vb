Option Strict Off
Option Explicit On
Imports System
Module SQL
	' ***************************************************************** '
	' Class Name: SQL
	'
	' Date: 04/06/2001
	'
	' Description: Contains the SQL Statements required by the
	'              EventPublicText class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select EventPublicText SQL
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleEventPublicText"
    'developer guide no. 39
    'Public Const ACSelectSingleSQL As String = "{call spe_Event_Public_Text_sel (?)}"
    Public Const ACSelectSingleSQL As String = "spe_Event_Public_Text_sel"
	
	' Add EventPublicText SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddEventPublicText"
    'developer guide no. 39
    'Public Const ACAddSQL As String = "{call spe_Event_Public_Text_add (?,?,?)}"
    Public Const ACAddSQL As String = "spe_Event_Public_Text_add"
	
	' Delete EventPublicText SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteEventPublicText"
    'developer guide no. 39
    'Public Const ACDeleteSQL As String = "{call spe_Event_Public_Text_del (?)}"
    Public Const ACDeleteSQL As String = "spe_Event_Public_Text_del"
	
	' Update EventPublicText SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateEventPublicText"
    'developer guide no. 39
    'Public Const ACUpdateSQL As String = "{call spe_Event_Public_Text_upd (?,?,?)}"
    Public Const ACUpdateSQL As String = "spe_Event_Public_Text_upd"
End Module