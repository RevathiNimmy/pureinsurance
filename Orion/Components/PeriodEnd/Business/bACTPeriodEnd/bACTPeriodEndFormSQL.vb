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
	' Date: 28/08/1998
	'
	' Description: Contains the SQL Statements required by the
	'              bACTPeriodEnd.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
    'developer guide no.39
    'start
	' spu_ACT_Get_Accounts_Journal
	Public Const ACGetAccountsStored As Boolean = True
	Public Const ACGetAccountsName As String = "GetAccountsJournal"
    Public Const ACGetAccountsSQL As String = "spu_ACT_Get_Accounts_Journal"
	
	' spu_ACT_Get_Period_Total
	Public Const ACGetPeriodTotalStored As Boolean = True
	Public Const ACGetPeriodTotalName As String = "GetPeriodTotal"
    Public Const ACGetPeriodTotalSQL As String = "spu_ACT_Select_trans_For_YearEnd"
	
	' spu_ACT_Get_Period_Dates
	Public Const ACGetPeriodDatesStored As Boolean = True
	Public Const ACGetPeriodDatesName As String = "GetPeriodDates"
    Public Const ACGetPeriodDatesSQL As String = "spu_ACT_Get_Period_Dates"
	
	'spu_ACT_SelAll_Sub_Branch
	Public Const ACGetAllSubBranchesStored As Boolean = True
	Public Const ACGetAllSubBranchesName As String = "GetAllSubBranches"
    Public Const ACGetAllSubBranchesSQL As String = "spu_ACT_SelAll_Sub_Branch"
    'end
    
End Module