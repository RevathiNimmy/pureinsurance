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
	' Class Name: BusinessSQL
	'
	' Date: 07/05/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRPolicyNumMaint.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	' Get All Numbering Schemes SQL
	Public Const ACGetAllDetailsStored As Boolean = True
	Public Const ACGetAllDetailsName As String = "GetPolicyNumMaint"
	Public Const ACGetAllDetailsSQL As String = "spu_numbering_scheme_saa"
	
	' Delete a Numbering Scheme SQL
	Public Const ACDeleteAllDetailsStored As Boolean = True
	Public Const ACDeleteAllDetailsName As String = "DeletePolicyNumMaint"
	Public Const ACDeleteAllDetailsSQL As String = "spu_policy_num_maintenance_del"
	
	' Insert a Numbering Scheme SQL
	Public Const ACInsertNumberingSchemeStored As Boolean = True
	Public Const ACInsertNumberingSchemeName As String = "InsertNumberingScheme"
	Public Const ACInsertNumberingSchemeSQL As String = "spe_numbering_scheme_add"
	
	' Update a Numbering Scheme SQL
	Public Const ACUpdateNumberingSchemeStored As Boolean = True
	Public Const ACUpdateNumberingSchemeName As String = "UpdateNumberingScheme"
	Public Const ACUpdateNumberingSchemeSQL As String = "spe_numbering_scheme_upd"
	
	'increment numbering sheme
	Public Const ACIncrementNumberingSchemeStored As Boolean = True
	Public Const ACIncrementNumberingSchemeName As String = "increment numbering scheme"
	Public Const ACIncrementNumberingSchemeSQL As String = "spu_increment_numbering_scheme"
	
	
	'increment numbering sheme
	Public Const ACResetNumberingSchemeStored As Boolean = True
	Public Const ACResetNumberingSchemeName As String = "Reset numbering scheme"
	Public Const ACResetNumberingSchemeSQL As String = "spu_reset_numbering_scheme"
	
	' MIPS Client Numbering
	Public Const ACGetNumberingSchemeIdsFromPartyTypeSQL As String = "spu_get_client_auto_num_ids"
	Public Const ACGetNumberingSchemeIdsFromPartyTypeName As String = "GetNumberingSchemeIdsFromPartyType"
    Public Const ACGetNumberingSchemeIdsFromPartyTypeStored As Boolean = True

    'Enhancement PM041940 - WPR63_Add the State to the Numbering Scheme (Jai 21/04/2015)
    Public Const ACGetStateCodeForPartySQL As String = "spu_Get_State_Code_For_Party"
    Public Const ACGetStateCodeForPartyName As String = "GetStateCodeForParty"
    Public Const ACGetStateCodeForPartyStored As Boolean = True
	
	' Maintain Party Code
	Public Const ACGetIsReadOnlyTypeSQL As String = "spu_get_client_readonly_details"
	Public Const ACGetIsReadOnlyName As String = "GetIsReadOnly"
	Public Const ACGetIsReadOnlyTypeStored As Boolean = True
	
	
	' Get Source detail
	Public Const ACGetSourceDetailStored As Boolean = True
	Public Const ACGetSourceDetailName As String = "GetSourceDetail"
	Public Const ACGetSourceDetailSQL As String = "spu_PM_Select_Source"
	
	' Delete Abandoned Numbers
	Public Const ACDeleteAbandonedNumbersStored As Boolean = True
	Public Const ACDeleteAbandonedNumbersName As String = "DeleteAbandonedNumbers"
    Public Const ACDeleteAbandonedNumbersSQL As String = "spe_abandoned_numbers_del"

    Public Const ACPolicyNumberingSchemegetandIncrementSQL As String = "spu_Policy_Numbering_scheme_GetAndIncrement"
    Public Const ACPolicyNumberingSchemegetandIncrementName As String = "Policy_Numbering_scheme_GetAndIncrement"
    Public Const ACPolicyNumberingSchemegetandIncrementStored As Boolean = True


    Public Const ACPolicyNumberingSchemegetandIncrementPeriodSQL As String = "spu_Policy_numbering_scheme_GetAndIncrement_Period"
    Public Const ACPolicyNumberingSchemegetandIncrementPeriodName As String = "Policy_numbering_scheme_GetAndIncrement_Period"
    Public Const ACPolicyNumberingSchemegetandIncrementPeriodStored As Boolean = True
End Module