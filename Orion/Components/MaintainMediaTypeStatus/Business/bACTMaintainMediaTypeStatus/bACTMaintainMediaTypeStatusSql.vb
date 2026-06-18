Option Strict Off
Option Explicit On
Imports System
Module MaintainMediaTypeStatusSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	
	Public Const ACPaymentMaintenanceGetUserReverseAllocationQueryStored As Boolean = True
	Public Const ACPaymentMaintenanceGetUserReverseAllocationQueryName As String = "SelectUserReverseAllocationQuery"
	Public Const ACPaymentMaintenanceGetUserReverseAllocationQuerySQL As String = "spe_User_Authorities_Sel"
	
	Public Const ACGetPolicyReceiptsForMediaTypeStatusMaintenanceStored As Boolean = True
	Public Const ACGetPolicyReceiptsForMediaTypeStatusMaintenanceName As String = "Get Policy Receipts for Media Type Status Maintenance "
	Public Const ACGetPolicyReceiptsForMediaTypeStatusMaintenanceSQL As String = "spu_ACT_Find_Receipt"
	
	Public Const ACGetPolicyStatusForMediaTypeStatusMaintenanceStored As Boolean = True
	Public Const ACGetPolicyStatusForMediaTypeStatusMaintenanceName As String = "Get Policy Status for Media Type Status Maintenance"
	Public Const ACGetPolicyStatusForMediaTypeStatusMaintenanceSQL As String = "spu_ACT_Get_Policy_Status_For_MediaType_Status_Maintenance"
	
	Public Const ACUpdateReceiptMediaTypeStatusStored As Boolean = True
	Public Const ACUpdateReceiptMediaTypeStatusName As String = "Update Receipt Media Type Status"
	Public Const ACUpdateReceiptMediaTypeStatusSQL As String = "spu_ACT_Update_Receipt_MediaType_Status"
    'developer guide no. 211

    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module