Option Strict Off
Option Explicit On
Imports System
Module FindPartySql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Class Name: FindPartySQL
	'
	' Date: 27th September 1996
	'
	' Description: Contains the SQL Statements to (Stored Procedures
	'              and Enbedded SQL) manipulate an FindParty
	'
	' Edit History:
	' ***************************************************************** '
	
	' Find Party SQL
	Public Const ACFindPartyStored As Boolean = False
	Public Const ACFindPartyName As String = "spu_find_party_claim"
	
	' Find Party Type
	Public Const ACFindTypeStored As Boolean = False
	Public Const ACFindTypeName As String = "spu_find_Party_type"
End Module