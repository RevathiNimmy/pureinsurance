Option Strict Off
Option Explicit On
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
	' Date: 16/09/2000
	'
	' Description: Contains the SQL Statements required by the
	'              bSirAgentCommission.Form class.
	'
	' Edit History: SR16092000- Created
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select Agent commissions for the given insurance file cnt
	Public Const ACSelectAgentCommisionStored As Boolean = True
	Public Const ACSelectAgentCommissionName As String = "SelectAgentcommision"
    'developer guide no 39. 
    Public Const ACSelectAgentCommissionSQL As String = "spu_sir_agent_commission_sel"
	
	'DN 21/02/03 - ISS 2274:Pass the transaction type as an extra parameter
	' Calculate Agent commissions for the given insurance file cnt
	Public Const ACCalculateAgentCommisionStored As Boolean = True
	Public Const ACCalculateAgentCommissionName As String = "Calculate Agent Commision"
    'developer guide no 39.  
    Public Const ACCalculateAgentCommissionSQL As String = "spu_sir_agent_commission_calc"
	
	'Update LeadCommission table for the given insurance file cnt
	Public Const ACUpdateLeadCommissionStored As Boolean = True
	Public Const ACUpdateLeadCommissionName As String = "UpdateLeadcommission"
    'developer guide no 39. 
    Public Const ACUpdateLeadCommissionSQL As String = "spu_sir_lead_commission_upd"
	
	
	'Get All the parties
	Public Const ACGetAllPartiesStored As Boolean = False
	Public Const ACGetAllPatiesName As String = "GetAllParties"
	Public Const ACGetAllPartiesSQL As String = "Select Party_cnt, Shortname from Party where party_type_id = 3 and is_deleted = 0"
	
	
	'Add Agent Commission Record
	Public Const ACAddAgentCommissionStored As Boolean = True
	Public Const ACAddAgentCommissionName As String = "AddAgentCommission"
	Public Const ACAddAgentCommisionSQL As String = "spu_sir_agent_commission_add"
	
	'Delete Agent commission Records
	Public Const ACDeleteAgentCommissionStored As Boolean = True
	Public Const ACDeleteAgentCommissionName As String = "DeleteAgentCommission"
    'developer guide no 39. 
    Public Const ACDeleteAgentCommissionSQL As String = "spu_sir_agent_commission_del"
	
	
	'Update Agent commission Record
	Public Const ACEditAgentCommissionStored As Boolean = True
	Public Const ACEditAgentCommissionName As String = "EditAgentCommission"
    'developer guide no 39. 
    Public Const ACEditAgentCommissionSQL As String = "spu_sir_agent_commission_upd"
	
	'Should we display this screen.
	Public Const ACCheckDisplayCommissionStored As Boolean = True
	Public Const ACCheckDisplayCommissionName As String = "CheckDisplayCommission"
    'developer guide no 39. 
    Public Const ACCheckDisplayCommissionSQL As String = "spu_check_display_commission"
    ' Copy Policy Commision SQL
    Public Const kCopyPolicyCommissionStored As Boolean = True
    Public Const kCopyPolicyCommissionName As String = "Copy Policy Commission"
    Public Const kCopyPolicyCommissionSQL As String = "spu_Policy_Commission_Copy"

End Module