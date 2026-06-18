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
	' Date: 20/07/2000
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRProduct.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Select All SIRProduct SQL
	Public Const ACAddRiskTypeStored As Boolean = True
	Public Const ACAddRiskTypeName As String = "AddRiskType"
	Public Const ACAddRiskTypeSQL As String = "spe_Risk_Type_Add"
	
	Public Const ACUpdRiskTypeStored As Boolean = True
	Public Const ACUpdRiskTypeName As String = "UpdRiskType"
	Public Const ACUpdRiskTypeSQL As String = "spe_Risk_Type_upd"
	
	Public Const ACDelRiskTypeStored As Boolean = True
	Public Const ACDelRiskTypeName As String = "DeleteRiskType"
	Public Const ACDelRiskTypeSQL As String = "spe_Risk_Type_Del"
	
	Public Const ACSelRiskTypeStored As Boolean = True
	Public Const ACSelRiskTypeName As String = "SelRiskType"
	'Public Const ACSelRiskTypeSQL = "spe_Risk_Type_Sel"
	Public Const ACSelRiskTypeSQL As String = "spu_Risk_Type_Sel"
	
	Public Const ACSaaRiskTypeStored As Boolean = True
	Public Const ACSaaRiskTypeName As String = "SelAllRisk_Type"
	Public Const ACSaaRiskTypeSQL As String = "spe_Risk_Type_Saa"
	
	Public Const ACAddCaptionIDStored As Boolean = True
	Public Const ACAddCaptionIDName As String = "AddPMCaption"
	Public Const ACAddCaptionIDSQL As String = "spu_pm_caption_id_return"
	
	Public Const ACSaaGISScreenStored As Boolean = True
	Public Const ACSaaGISScreenName As String = "SelAllGISScreen"
	Public Const ACSaaGISScreenSQL As String = "spu_GIS_Screen_saa2"
	
	Public Const ACSelRiskTypeGISScreenStored As Boolean = True
	Public Const ACSelRiskTypeGISScreenName As String = "SelRiskTypeGISScreen"
	Public Const ACSelRiskTypeGISScreenSQL As String = "spu_Risk_Type_GIS_Screen_Sel"
	
	Public Const ACDelRiskTypeGISScreenStored As Boolean = True
	Public Const ACDelRiskTypeGISScreenName As String = "DeleteRiskTypeGISScreen"
	Public Const ACDelRiskTypeGISScreenSQL As String = "spu_Risk_Type_GIS_Screen_del"
	
	Public Const ACAddRiskTypeGISScreenStored As Boolean = True
	Public Const ACAddRiskTypeGISScreenName As String = "AddRiskTypeGISScreen"
	Public Const ACAddRiskTypeGISScreenSQL As String = "spe_Risk_Type_GIS_Screen_add"
	
	Public Const ACSelRiskTypeGroupStored As Boolean = True
	Public Const ACSelRiskTypeGroupName As String = "SelRiskTypeGroup"
	Public Const ACSelRiskTypeGroupSQL As String = "spu_Risk_Type_Usage_sel"
	
	Public Const ACSaaRiskTypeGroupStored As Boolean = True
	Public Const ACSaaRiskTypeGroupName As String = "SelAllRiskTypeGroup"
	Public Const ACSaaRiskTypeGroupSQL As String = "spe_Risk_Type_Group_saa"
	
	Public Const ACDelRiskTypeUsageStored As Boolean = True
	Public Const ACDelRiskTypeUsageName As String = "DelRiskTypeUsage"
	Public Const ACDelRiskTypeUsageSQL As String = "spu_Risk_Type_Usage_Del"
	
	Public Const ACAddRiskTypeUsageStored As Boolean = True
	Public Const ACAddRiskTypeUsageName As String = "AddRiskTypeUsage"
	Public Const ACAddRiskTypeUsageSQL As String = "spu_Risk_Type_Usage_Add"
	
	Public Const ACAddRiskTypeRuleSetStored As Boolean = True
	Public Const ACAddRiskTypeRuleSetName As String = "AddRiskTypeRuleSet"
	Public Const ACAddRiskTypeRuleSetSQL As String = "spe_risk_type_rule_set_add"
	
	Public Const ACUpdRiskTypeRuleSetStored As Boolean = True
	Public Const ACUpdRiskTypeRuleSetName As String = "UpdRiskTypeRuleSet"
	Public Const ACUpdRiskTypeRuleSetSQL As String = "spe_risk_type_rule_set_upd"
	
	Public Const ACSelRiskTypeRuleSetStored As Boolean = True
	Public Const ACSelRiskTypeRuleSetName As String = "SelRiskTypeRuleSet"
	Public Const ACSelRiskTypeRuleSetSQL As String = "spe_risk_type_rule_set_sel"
	
	Public Const ACSaaRiskTypeRuleSetStored As Boolean = True
	Public Const ACSaaRiskTypeRuleSetName As String = "SaaRiskTypeRuleSet"
	Public Const ACSaaRiskTypeRuleSetSQL As String = "spe_risk_type_rule_set_saa"
	
	Public Const ACSelClausesStored As Boolean = True
	Public Const ACSelClausesName As String = "SelClauses"
	Public Const ACSelClausesSQL As String = "spu_Risk_Type_Clauses_Sel"
	
	Public Const ACDelClausesStored As Boolean = True
	Public Const ACDelClausesName As String = "DeleteClauses"
	Public Const ACDelClausesSQL As String = "spu_Risk_Type_Clauses_del"
	
	Public Const ACAddClausesStored As Boolean = True
	Public Const ACAddClausesName As String = "AddRiskTypeGISScreen"
	Public Const ACAddClausesSQL As String = "spu_Risk_Type_Clauses_add"
	
	Public Const ACSelRiskTypeRIModelUsageStored As Boolean = True
	Public Const ACSelRiskTypeRIModelUsageName As String = "SelRiskTypeRIModelUsage"
    Public Const ACSelRiskTypeRIModelUsageSQL As String = "spu_risk_type_ri_model_usage_saa"

    Public Const ACSaaRuleTypeStored As Boolean = True
    Public Const ACSaaRuleTypeName As String = "SelAllRule_Type"
    Public Const ACSaaRuleTypeSQL As String = "spe_Rule_Type_Saa"
End Module