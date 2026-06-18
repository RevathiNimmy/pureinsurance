Option Strict Off
Option Explicit On
Imports System
Module ACTUserAuthoritiesSQL
	
	Public Const ACSelectSingleStored As Boolean = True
	Public Const ACSelectSingleName As String = "SelectSingleACTUserAuthorities"
	Public Const ACSelectSingleSQL As String = "spe_User_Authorities_sel"
	
	' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields
	Public Const ACAddStored As Boolean = True
	Public Const ACAddName As String = "AddACTUserAuthorities"
	Public Const ACAddSQL As String = "spe_User_Authorities_add"
	
	Public Const ACDeleteStored As Boolean = True
	Public Const ACDeleteName As String = "DeleteACTUserAuthorities"
	Public Const ACDeleteSQL As String = "spe_User_Authorities_del"
	
	' 19/04/2005 - 2005 Staff Control Added New Client Manager Security fields
	Public Const ACUpdateStored As Boolean = True
	Public Const ACUpdateName As String = "UpdateACTUserAuthorities"
	Public Const ACUpdateSQL As String = "spe_User_Authorities_upd"
	
	'Party View
	Public Const ACGetViewOptionStored As Boolean = True
	Public Const ACGetViewOptionName As String = "GetViewOptionACTUserAuthorities"
	Public Const ACGetViewOptionSQL As String = "spu_ACT_GetPartyViewOptions"
	
	Public Const ACCheckIsRecommendClaimPaymentEnabledatProductStored As Boolean = True
	Public Const ACCheckIsRecommendClaimPaymentEnabledatProductName As String = "spu_Check_IsRecommendClaimPaymentEnabledatProduct"
	Public Const ACCheckIsRecommendClaimPaymentEnabledatProductSQL As String = "spu_Check_IsRecommendClaimPaymentEnabledatProduct"
End Module