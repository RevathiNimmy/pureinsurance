Option Strict Off
Option Explicit On
Imports System
Module SQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' ***************************************************************** '
	' Class Name: SQL
	'
	' Date: {TodaysDate}
	'
	' Description: Contains the SQL Statements required by the
	'              CLMCoinsuranceRecoveries class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select CLMCoinsuranceRecoveries SQL
	Public Const ACGetDetailsStored As Boolean = True
	Public Const ACGetDetailsName As String = "spu_get_Coinsurance_details"
	Public Const ACGetDetailsSQL As String = "spu_get_coinsurance_details"
	
	' Add CLMCoinsuranceRecoveries SQL
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "spu_insert_coinsurance"
	Public Const ACAddSQL As String = "spu_insert_coinsurance"
	
	' Delete CLMCoinsuranceRecoveries SQL
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "spu_delete_coinsurance"
	Public Const ACDeleteSQL As String = "spu_delete_coinsurance"
	
	' Update CLMCoinsuranceRecoveries SQL
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "spu_update_coinsurance"
	Public Const ACUpdateSQL As String = "spu_update_coinsurance"
	
	' Get Details for Details Screen
	Public Const ACGetDetailsShareStored As Boolean = True
	Public Const ACGetDetailsShareName As String = "spu_get_details_coinsurance"
	Public Const ACGetDetailsShareSQL As String = "spu_get_details_coinsurance"
	
	' Get Sum Insured for Reserve
	Public Const ACGetMainShareStored As Boolean = True
	Public Const ACGetMainShareName As String = "spu_get_main_share_coinsurance"
	Public Const ACGetMainShareSQl As String = "spu_get_main_share_coinsurance"
	
	' Get the Party names for Combo box in the details screen
	Public Const ACGetPartyStored As Boolean = True
	Public Const ACGetPartyName As String = "spu_get_party_coinsurer"
	Public Const ACGetPartySQl As String = "spu_get_party_coinsurer"
	
	' Get the Treatment Values
	Public Const ACTreatment_LookupStored As Boolean = False
	Public Const ACTreatment_LookupName As String = "Treatment_Lookup"
	Public Const ACTreatment_LookupSQL As String = "Select Distinct(description) from Coinsurance_Treatment"
	
	' Update the Treatment Values
	Public Const ACUpdateTreatmentStored As Boolean = True
	Public Const ACUpdateTreatmentName As String = "spu_update_treatment"
	Public Const ACUpdateTreatmentSQL As String = "spu_update_treatment"
	
	' Get the Treatment Value
	Public Const ACGetTreatmentStored As Boolean = True
	Public Const ACGetTreatmentName As String = "spu_get_treatment"
	Public Const ACGetTreatmentSQL As String = "spu_get_treatment"
	
	' Get the Claim_Number from the given Claim_id
	Public Const ACGetClaimNumberStored As Boolean = True
	Public Const ACGetClaimNumberName As String = "spu_get_claimnumber"
	Public Const ACGetClaimNumberSQl As String = "spu_get_claimnumber"
End Module