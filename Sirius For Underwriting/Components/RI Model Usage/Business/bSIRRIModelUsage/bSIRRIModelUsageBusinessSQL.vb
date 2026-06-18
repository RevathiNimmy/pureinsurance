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
	' Date: 09/06/1999
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRRIModelUsage.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select Risk Type RI Model Usage SQL
	Public Const ACSelectRiskTypeRIModelUsageStored As Boolean = True
	Public Const ACSelectRiskTypeRIModelUsageName As String = "SelectRiskTypeRIModelUsage"
	Public Const ACSelectRiskTypeRIModelUsageSQL As String = "spu_Risk_Type_ri_model_usage_saa"
	
	' Delete Risk Type RI Model Usage SQL
	Public Const ACDeleteRiskTypeRIModelUsageStored As Boolean = True
	Public Const ACDeleteRiskTypeRIModelUsageName As String = "DeleteRiskTypeRIModelUsage"
	Public Const ACDeleteRiskTypeRIModelUsageSQL As String = "spu_Risk_Type_ri_model_usage_del"
	
	' Insert Risk Type RI Model Usage SQL
	Public Const ACInsertRiskTypeRIModelUsageStored As Boolean = True
	Public Const ACInsertRiskTypeRIModelUsageName As String = "InsertRiskTypeRIModelUsage"
	Public Const ACInsertRiskTypeRIModelUsageSQL As String = "spu_Risk_Type_ri_model_usage_add"
	
	' Update Risk Type RI Model Usage SQL
	Public Const ACUpdateRiskTypeRIModelUsageStored As Boolean = True
	Public Const ACUpdateRiskTypeRIModelUsageName As String = "UpdateRiskTypeRIModelUsage"
	Public Const ACUpdateRiskTypeRIModelUsageSQL As String = "spu_Risk_Type_ri_model_usage_upd"
	
	' Select RI Model Is Deferred SQL
	Public Const ACSelectRiskTypeRIModelIsDeferredStored As Boolean = True
	Public Const ACSelectRiskTypeRIModelIsDeferredName As String = "SelectRiskTypeRIModelIsDeferred"
	Public Const ACSelectRiskTypeRIModelIsDeferredSQL As String = "spu_Risk_Type_RI_Model_is_deferred_saa"
	
	'Check if duplicate record exists
	Public Const ACValidateRIModelUsageSQL As String = "spu_SIR_ValidateRIModelUsage"
	Public Const ACValidateRIModelUsageName As String = "ValidateRIModelUsage"
End Module