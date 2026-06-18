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
	' Date: 31/07/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTCompany.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    ' Select Company SQL
    'developer guide no. 39
    'start
	Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectAllCompany"
    Public Const ACGetDetailsSQL As String = "spu_ACT_Select_Company"
	
	' Select All Company SQL
	Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllCompany"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_SelAll_Company"
	
	' Check ID SQL
	Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckCompanyID"
    Public Const ACCheckIDSQL As String = "spu_ACT_Check_Company"
	
	' Add Company SQL
	Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddCompany"
    Public Const ACAddSQL As String = "spu_ACT_Add_Company"
	
	' Delete Company SQL
	Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteCompany"
    Public Const ACDeleteSQL As String = "spu_ACT_Delete_Company"
	
	' Update Company SQL
	Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateCompany"
    Public Const ACUpdateSQL As String = "spu_ACT_Update_Company"
    'developer guide no. 29(No Solutions)
    'Shared Sub New()
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module