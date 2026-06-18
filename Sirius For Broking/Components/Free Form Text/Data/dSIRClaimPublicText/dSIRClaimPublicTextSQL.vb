Option Strict Off
Option Explicit On
Imports System
Module SQL
	' ***************************************************************** '
	' Class Name: SQL
	'
	' Date: 14/01/01
	'
	' Description: Contains the SQL Statements required by the
	'              ClaimPublicText class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select ClaimPublicText SQL
    'developer guide no.39
    'start
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleClaimPublicText"
    Public Const ACSelectSingleSQL As String = "spe_Claim_Public_Text_sel"
	
	' Add ClaimPublicText SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddClaimPublicText"
    Public Const ACAddSQL As String = "spe_Claim_Public_Text_add"
	
	' DeleteClaimPublicText SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteClaimPublicText"
    Public Const ACDeleteSQL As String = "spe_Claim_Public_Text_del"
	
	' Update ClaimPublicText SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateClaimPublicText"
    Public Const ACUpdateSQL As String = "spe_Claim_Public_Text_upd"
    'end
End Module