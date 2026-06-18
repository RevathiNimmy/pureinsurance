Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 06/09/2000
	'
	' Description: Contains the SQL Statements required by the
	'              bSirPerilAllocation.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
    'developer guide no.39
    'start
	'Select All the commission arrangements from the system.
	Public Const ACSelectAllCommissionStored As Boolean = True
	Public Const ACSelectAllCommissionName As String = "SelectAllCommission"
    Public Const ACSelectAllCommissionSQL As String = "spu_sir_Commission_sel_all"
	
	'Select the commission arrangements for the given parameters
	Public Const ACSelectCommissionStored As Boolean = True
	Public Const ACSelectCommssionName As String = "SelectCommission"
    Public Const ACSelectCommissionSQL As String = "spu_Sir_Commission_sel"
	
	'Get Commission rate for the given party_type, party etc
	Public Const ACCalcCommissionStored As Boolean = True
	Public Const ACCalcCommissionName As String = "CalculateCommission"
    Public Const ACCalcCommissionsQL As String = "spu_sir_Calc_Commission_rate"
	
	'Get All the parties
	Public Const ACGetAllPartiesStored As Boolean = False
	Public Const ACGetAllPatiesName As String = "GetAllParties"
	'TN20001119 (Start)
	'Public Const ACGetAllPartiesSQL = "Select Party_Agent_Type_id , PA.Party_cnt, P.shortname from Party_Agent PA,  Party P   Where PA.PArty_cnt = P.PArty_cnt And P.is_deleted = 0"
    Public Const ACGetAllPartiesSQL As String = "Select Party_Agent_Type_id , PA.Party_cnt, " & Strings.Chr(13) & Strings.Chr(10) & _
                                                "P.shortname,PA.commission_level_id from Party_Agent PA,Party P" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "Where PA.PArty_cnt = P.PArty_cnt And P.is_deleted = 0" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "Union" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "select 4, P.party_cnt, P.shortname,NULL" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "from party P, party_insurer PI" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "Where P.party_cnt = PI.party_cnt" & Strings.Chr(13) & Strings.Chr(10) & _
                                                "And P.is_deleted = 0"
	'TN20001119 (End)
	
	'Add the commission arrangement to the database
	Public Const ACAddCommissionStored As Boolean = True
	Public Const ACAddCommissionName As String = "AddCommission"
	'Start - Renuka - (WPR64 Paralleling)
	Public Const ACAddCommissionSQL As String = "spu_sir_Commission_add"
	'End - Renuka - (WPR64 Paralleling)
	
	'Delete the Commission arrangement from the database
	Public Const ACDeleteCommissionStored As Boolean = True
	Public Const ACDeleteCommissionName As String = "DeleteCommission"
    Public Const ACDeleteCommissionSQL As String = "spu_sir_commission_del"
	
	'UnDelete the Commission arrangement from the database
	Public Const ACUnDeleteCommissionStored As Boolean = True
	Public Const ACUnDeleteCommissionName As String = "UnDeleteCommission"
    Public Const ACUnDeleteCommissionSQL As String = "spu_sir_commission_undel"
    'end
	
	'Modify the commission rate
	Public Const ACEditCommissionStored As Boolean = True
	Public Const ACEditCommissionName As String = "EditCommission"
	'Start - Renuka - (WPR64 Paralleling)
	Public Const ACEditCommissionSQL As String = "spu_sir_commission_upd"
    'End - Renuka - (WPR64 Paralleling)
    Public Const ACSelectCommissionLevelStored As Boolean = True
    Public Const ACSelectCommissionLevelName As String = "SelectCommissionLevel"
    Public Const ACSelectCommissionLevelSQL As String = "spu_sir_select_commission_level"
End Module