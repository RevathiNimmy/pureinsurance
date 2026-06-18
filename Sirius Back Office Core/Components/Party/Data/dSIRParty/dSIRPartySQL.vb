Option Strict Off
Option Explicit On
Imports System
Module SQL
	' ***************************************************************** '
	' Class Name: SQL
	'
	' Date: 07/09/1998
	'
	' Description: Contains the SQL Statements required by the
	'              SIRParty class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Select SIRParty SQL
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleSIRParty"
    'developer guide no. 39 (Guide)
    Public Const ACSelectSingleSQL As String = "spe_Party_sel"
	
	' Select SIRParty from event SQL
	Public Const ACSelectSingleEventStored As Boolean = True
    Public Const ACSelectSingleEventName As String = "SelectSingleSIREventParty"
    'developer guide no. 39 (Guide)
    Public Const ACSelectSingleEventSQL As String = "spe_Event_Party_sel"
	
	' Add SIRParty SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddSIRParty"
	'DC 28/06/00 Added extra parameter for Correspondence Type Id
	'Tomo060700 Added extra parameter for renewal stop code
	'sj 12/06/2002 - start
	'Add extra parameters for loyalty_number,alternative_identifier, marketing_segment_ind,
	'trading_name and sub_branch_id
	'Public Const ACAddSQL = "{call spe_Party_add (?,?,?,?,?,?,?,?,?,?," & _
	''                                             "?,?,?,?,?,?,?,?,?,?," & _
	''                                             "?,?,?,?,?,?,?,?,?,?," & _
	''                                             "?,?,?,?,?,?,?,?,?,?," & _
	''                                             "?,?,?,?,?,?,?)}"
    'developer guide no. 39 (Guide)
   
    Public Const ACAddSQL As String = "spe_Party_add"
	'sj 12/06/2002 - end
	' Delete SIRParty SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteSIRParty"
    'developer guide no. 39 (Guide)
    Public Const ACDeleteSQL As String = "spe_Party_del"
	
	' Update SIRParty SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateSIRParty"
	'DC 28/06/00 Added extra paraneter for Correspondence Type id
	'Tomo060700 Added extra parameter for renewal stop code
	'sj 12/06/2002 - start
	'Add extra parameters for loyalty_number,alternative_identifier, marketing_segment_ind,
	'trading_name and sub_branch_id
	'Public Const ACUpdateSQL = "{call spe_Party_upd (?,?,?,?,?,?,?,?,?,?," & _
	''                                                "?,?,?,?,?,?,?,?,?,?," & _
	''                                                "?,?,?,?,?,?,?,?,?,?," & _
	''                                                "?,?,?,?,?,?,?,?,?,?," & _
    ''                                                "?,?,?,?,?,?,?)}"
    'developer guide no. 39 (Guide)
    Public Const ACUpdateSQL As String = "spe_Party_upd"
	'sj 12/06/2002 - end
End Module