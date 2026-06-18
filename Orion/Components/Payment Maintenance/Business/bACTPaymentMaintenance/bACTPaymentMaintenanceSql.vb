Option Strict Off
Option Explicit On
Imports System
Module FindPaymentMaintenanceSql
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	
	Public Const ACPaymentMaintenanceGetUserReverseAllocationQueryStored As Boolean = True
	Public Const ACPaymentMaintenanceGetUserReverseAllocationQueryName As String = "SelectUserReverseAllocationQuery"
	Public Const ACPaymentMaintenanceGetUserReverseAllocationQuerySQL As String = "spe_User_Authorities_Sel"
	
	Public Const ACPaymentMaintenanceUpdateCashListItemQueryStored As Boolean = True
	Public Const ACPaymentMaintenanceUpdateCashListItemQueryName As String = "UpdateCashListItemQuery"
	Public Const ACPaymentMaintenanceUpdateCashListItemQuerySQL As String = "spu_ACT_Set_CashListItem_ReverseAllocation"
	
	Public Const ACPaymentMaintenanceGetCancelPaymentListDataQueryStored As Boolean = True
	Public Const ACPaymentMaintenanceGetCancelPaymentListDataQueryName As String = "GetCancelPaymentListDataQuery"
	Public Const ACPaymentMaintenanceGetCancelPaymentListDataQuerySQL As String = "spu_ACT_Select_View_Allocation"
	
	Public Const kGetDocumentDetailsName As String = "Returns details for the specified document / account keys"
	Public Const kGetDocumentDetailsSQL As String = "spu_ACT_Get_Document_Details_For_Account"
	
	Public Const ACPaymentMaintenanceGetEventTypeIdQueryStored As Boolean = True
	Public Const ACPaymentMaintenanceGetEventTypeIdQueryName As String = "GetEventTypeIdQuery"
	Public Const ACPaymentMaintenanceGetEventTypeIdQuerySQL As String = "spu_ACT_Get_EventId_FromEventCode"
	
	Public Const ACMediTypeForValidationIDQueryStored As Boolean = True
	Public Const ACMediTypeForValidationIDQueryName As String = "GetMediTypeForValidationIDQuery"
	Public Const ACMediTypeForValidationIDQuerySQL As String = "spu_ACT_Select_MediaType_Against_ValidationId"
    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module