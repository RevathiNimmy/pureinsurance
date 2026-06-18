Option Strict Off
Option Explicit On
Imports System
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	' ***************************************************************** '
    ' Table: RI Model
    ' Delete
	Public Const ACDeleteRIModelStored As Boolean = True
	Public Const ACDeleteRIModelName As String = "DeleteRIModel"
	Public Const ACDeleteRIModelSQL As String = "spu_RI_Model_del"
	'Start( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.1.1.1)
	Public Const ACGetRITypeForTreatyStored As Boolean = True
	Public Const ACGetRITypeForTreatySQL As String = "Spu_GetRITypeForTreaty"
	Public Const ACGetRITypeForTreatyName As String = "GetRITypeForTreaty"
	'End( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(6.1.1.1)
	
	' Insert
	Public Const ACInsertRIModelStored As Boolean = True
	Public Const ACInsertRIModelName As String = "InsertRIModel"
	Public Const ACInsertRIModelSQL As String = "spu_RI_Model_add"
	
	' Select
	Public Const ACSelectRIModelStored As Boolean = True
	Public Const ACSelectRIModelName As String = "SelectRIModel"
	Public Const ACSelectRIModelSQL As String = "spu_RI_Model_saa"
	
	' Update
	Public Const ACUpdateRIModelStored As Boolean = True
	Public Const ACUpdateRIModelName As String = "InsertRIModel"
	Public Const ACUpdateRIModelSQL As String = "spu_RI_Model_upd"

	' Table: RI Model Line
	' Delete
	Public Const ACDeleteRIModelLineStored As Boolean = True
	Public Const ACDeleteRIModelLineName As String = "DeleteRIModelLine"
	Public Const ACDeleteRIModelLineSQL As String = "spu_RI_Model_Line_del"
	
	' Insert
	Public Const ACInsertRIModelLineStored As Boolean = True
	Public Const ACInsertRIModelLineName As String = "InsertRIModelLine"
	Public Const ACInsertRIModelLineSQL As String = "spu_RI_Model_Line_add"
	
	' Select
	Public Const ACSelectRIModelLineStored As Boolean = True
	Public Const ACSelectRIModelLineName As String = "SelectRIModelLine"
	Public Const ACSelectRIModelLineSQL As String = "spu_RI_Model_Line_saa"
	
	'Check if there is any Retained REinsurer in XOL Treaty
	Public Const ACCheckRetainedReinsurerStored As Boolean = True
	Public Const ACCheckRetainedReinsurerSQL As String = "spu_Check_treaty_retained_party"
	Public Const ACCheckRetainedReinsurerName As String = "CheckRetainedReinsurer"
	
	Public Const ACSelectRiModelAuditInfoStored As Boolean = True
	Public Const ACSelectRiModelAuditInfoSQL As String = "Spu_GetRIModelAuditTrail"
    Public Const ACSelectRiModelAuditInfoName As String = "SelectRiModelAuditInfo"

    Public Const kGetRIExtendedLimitStored As Boolean = True
    Public Const kGetRIExtendedLimitSQL As String = "Spu_Get_Extended_Limit_Details"
	Public Const kGetRIExtendedLimitName As String = "GetRIExtendedLimit"

	' Select
	Public Const ACSelectModelCurrencyStored As Boolean = True
	Public Const ACSelectModelCurrencyName As String = "SelectRIModelCurrency"
	Public Const ACSelectModelCurrencySQL As String = "spu_RI_ModelCurrency_saa"
	'Save conversion currency rates
	Public Const ACUpdateModelCurrencyStored As Boolean = True
	Public Const ACUpdateModelCurrencyName As String = "UpdateRIModelCurrency"
	Public Const ACUpdateModelCurrencySQL As String = "spu_RI_ModelCurrencyRate_upd"
	'for Variable Quota share

	Public Const ACSelectRIModelVariableQuotaShareStored As Boolean = True
	Public Const ACSelectRIModelVariableQuotaShareName As String = "GetVariableQuotaShareConfig"
	Public Const ACSelectRIModelVariableQuotaShareSQL As String = "spu_GetVariableQuotaShareConfig"


	Public Const ACGetRIModelVariableQuotaShareStored As Boolean = True
	Public Const ACGetRIModelVariableQuotaShareName As String = "GetRIModelVariableQuotaShareConfig"
	Public Const ACGetRIModelVariableQuotaShareSQL As String = "spu_GetRIModelVariableQuotaShareConfig"


	Public Const ACSaveRIModelVariableQuotaShareStored As Boolean = True
	Public Const ACSaveRIModelVariableQuotaShareName As String = "SaveVariableQuotaShareConfig"
	Public Const ACSaveRIModelVariableQuotaShareSQL As String = "spu_SaveVariableQuotaShareConfig"

	Public Const ACDeleteRIModelVariableQuotaShareStored As Boolean = True
	Public Const ACDeleteRIModelVariableQuotaShareName As String = "DeleteVariableQuotaShareConfigByTreaty"
	Public Const ACDeleteRIModelVariableQuotaShareSQL As String = "spu_DeleteVariableQuotaShareConfig"

End Module