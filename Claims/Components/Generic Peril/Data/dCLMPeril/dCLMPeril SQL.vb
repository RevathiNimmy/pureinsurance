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
	'              CLMPeril class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Get Controls SQL
	Public Const ACGetControlsStored As Boolean = True
	Public Const ACGetControlsName As String = "spu_get_controls"
	Public Const ACGetControlsSQL As String = "spu_get_controls"
	
	' Get Reserve Type SQL
	Public Const ACGetReserveTypeStored As Boolean = True
	Public Const ACGetReserveTypeName As String = "spu_get_reserve_type"
	Public Const ACGetReserveTypeSQL As String = "spu_get_reserve_type"
	
	' Get Reserve Details SQL
	Public Const ACGetReserveDetailsStored As Boolean = True
	Public Const ACGetReserveDetailsName As String = "spu_get_reserve_details"
	Public Const ACGetReserveDetailsSQL As String = "spu_get_reserve_details"
	
	' Get Payment Details SQL
	'AK 210503 - added another parameter to this stored procedure
	Public Const ACGetPaymentDetailsStored As Boolean = True
	Public Const ACGetPaymentDetailsName As String = "spu_get_payment_details"
	Public Const ACGetPaymentDetailsSQL As String = "spu_get_payment_details"
	
	' Get Party Details SQL
	Public Const ACGetPartyDetailsStored As Boolean = True
	Public Const ACGetPartyDetailsName As String = "spu_get_party_details"
	'DC251103 was get_party_details
	Public Const ACGetPartyDetailsSQL As String = "spu_get_claim_party_details"
	
	' Add Party Details SQL
	Public Const ACAddPartyStored As Boolean = True
	Public Const ACAddPartyName As String = "spu_add_party"
	Public Const AcAddPartySQL As String = "spu_add_party"
	
	' Update Reserve Details SQL
	Public Const ACUpdateReserveDetailsStored As Boolean = True
	Public Const ACUpdateReserveDetailsName As String = "spu_update_reserve_details"
	Public Const ACUpdateReserveDetailsSQL As String = "spu_update_reserve_details"
	
	' Update Payment Details SQL
	'Public Const ACUpdatePaymentDetailsStored = True
	'Public Const ACUpdatePaymentDetailsName = "spu_update_payment_details"
	'Public Const ACUpdatePaymentDetailsSQL = "spu_update_payment_details"
	
	' Update General tab Details SQL
	Public Const ACUpdateGeneralDetailsStored As Boolean = True
	Public Const ACUpdateGeneralDetailsName As String = "spu_update_general_details"
	Public Const ACUpdateGeneralDetailsSQL As String = "spu_update_general_details"
	
	' delete Party Details SQL
	Public Const ACDeletePartyStored As Boolean = True
	Public Const ACDeletePartyName As String = "spu_delete_party"
	Public Const ACDeletePartySQL As String = "spu_delete_party"
	
	' Claim Lookup SQL
	Public Const ACClaimLookupStored As Boolean = True
	Public Const ACClaimLookupName As String = "spu_claimlookup"
	Public Const ACClaimLookupSQL As String = "spu_claimlookup"
	
	' Recovery Details
	Public Const ACGetRecoveryDetailsStored As Boolean = True
	Public Const ACGetRecoveryDetailsName As String = "spu_get_recovery_details"
	Public Const ACGetRecoveryDetailsSQL As String = "spu_get_recovery_details"
	
	'' Receipt Details
	'Public Const ACGetReceiptDetailsStored = True
	'Public Const ACGetReceiptDetailsName = "spu_get_receipt_details"
	'Public Const ACGetReceiptDetailsSQL = "spu_get_receipt_details"
	
	' Adding comments
	Public Const ACAddCommentsStoredUW As Boolean = True
	Public Const ACAddCommentsNameUW As String = "spu_add_comments"
	Public Const ACAddCommentsSQLUW As String = "spu_add_comments"
	
	' Get comments
	Public Const ACGetCommentsStoredUW As Boolean = True
	Public Const ACGetCommentsNameUW As String = "spu_get_comments"
	Public Const ACGetCommentsSQLUW As String = "spu_get_comments"
	
	' Get Controls View SQL
	Public Const ACGetControlsViewStored As Boolean = True
	Public Const ACGetControlsViewName As String = "spu_get_controls_view"
	Public Const ACGetControlsViewSQL As String = "spu_get_controls_view"
	
	Public Const ACGetSystemOptionStored As Boolean = False
	Public Const ACGetSystemOptionName As String = "Get System Option"
	Public Const ACGetSystemOptionSQL As String = "SELECT value FROM system_options WHERE option_number = {option_number}"
	
	'DC240402 -Start
	Public Const ACGetClaimCommentsStored As Boolean = True
	Public Const ACGetClaimComments As String = "GetClaimCommemts"
	Public Const ACGetClaimCommentsSQL As String = "spu_claim_comments_sel"
	
	Public Const ACDeleteClaimCommentsStored As Boolean = True
	Public Const ACDeleteClaimCommentsName As String = "DeleteClaimComments"
	Public Const ACDeleteClaimCommentsSQL As String = "spu_claim_comments_del"
	
	Public Const ACAddClaimCommentsStored As Boolean = True
	Public Const ACAddClaimCommentsName As String = "AddClaimComments"
	Public Const ACAddClaimCommentsSQL As String = "spu_claim_comments_add"
	'DC240402 -End
	
	'S4B Claim Enhancements R&D 2005
	Public Const AC_CLAIM_PERIL_SELECT_SQL As String = "spu_CLM_Claim_Peril_Sel"
	Public Const AC_CLAIM_PERIL_SELECT_NAME As String = "Get Claim Peril details"
	Public Const AC_CLAIM_PERIL_SELECT_SP As Boolean = True
End Module