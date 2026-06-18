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
	'              bSIRMaintainUserData.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
    'Developer Guide No. 39
    Public Const ACSQLCaptionReturn As String = "spu_pm_caption_id_return"
    Public Const ACSQLCaptionReturnName As String = "GetCaptionID"
    Public Const ACSQLCaptionReturnStored As Boolean = True


    ' Select PMLookup All SQL
    Public Const ACSelectAllStored As Boolean = False
    Public Const ACSelectAllName As String = "SelectPMLookupAll"
    Public Const ACSelectAllSQL As String = "SELECT tn.{Table_Name}_id, cap.caption, tn.code FROM {Table_Name} tn, pmcaption cap WHERE tn.is_deleted = 0 AND tn.effective_date <= {Effective_Date} AND tn.caption_id = cap.caption_id and cap.language_id = {Language_ID}"

    ' Select GIS lookup stuff
    Public Const ACSelectGISUserDefDetailStored As Boolean = True
    Public Const ACSelectGISUserDefDetailName As String = "SelectGISUserDefDetail"
    'Developer Guide No. 39
    Public Const ACSelectGISUserDefDetailSQL As String = "spu_GIS_user_def_detail_saa"

    ' Authority Rule stuff.
    Public Const ACSelectAuthorityRuleLinksStored As Boolean = True
    Public Const ACSelectAuthorityRuleLinksName As String = "SelectAuthorityRuleLinks"
    'Developer Guide No. 39
    Public Const ACSelectAuthorityRuleLinksSQL As String = "spu_PMUser_Auth_Rule_Link_saa"

    Public Const ACInsertAuthorityRuleLinksStored As Boolean = True
    Public Const ACInsertAuthorityRuleLinksName As String = "InsertAuthorityRuleLinks"
    'Developer Guide No. 39
    Public Const ACInsertAuthorityRuleLinksSQL As String = "spe_PMUser_Auth_Rule_Link_add"

    Public Const ACDeleteAuthorityRuleLinksStored As Boolean = True
    Public Const ACDeleteAuthorityRuleLinksName As String = "DeleteAuthorityRuleLinks"
    'Developer Guide No. 39
    Public Const ACDeleteAuthorityRuleLinksSQL As String = "spe_PMUser_Auth_Rule_Link_del"

    Public Const ACDeleteAuthorityRuleLinkStored As Boolean = True
    Public Const ACDeleteAuthorityRuleLinkName As String = "DeleteAuthorityRuleLink"
    'Developer Guide No. 39
    Public Const ACDeleteAuthorityRuleLinkSQL As String = "spe_PMUser_Auth_Rule_Link_del"

    'Public Const ACSelectAuthorityRuleSetStored = True
    'Public Const ACSelectAuthorityRuleSetName = "SelectAuthorityRuleSet"
    'Public Const ACSelectAuthorityRuleSetSQL = "{call spu_get_authority_rule_set(?)}"

    Public Const ACInsertRuleSetStored As Boolean = True
    Public Const ACInsertRuleSetName As String = "InsertRuleSet"
    'Developer Guide No. 39
    Public Const ACInsertRuleSetSQL As String = "spu_Rule_Set_add"

    Public Const ACUpdateRuleSetStored As Boolean = True
    Public Const ACUpdateRuleSetName As String = "UpdateRuleSet"
    'Developer Guide No. 39
    Public Const ACUpdateRuleSetSQL As String = "spu_Rule_Set_upd"

    Public Const ACDeleteAuthorityRuleStored As Boolean = True
    Public Const ACDeleteAuthorityRuleName As String = "DeleteAuthorityRule"
    'Developer Guide No. 39
    Public Const ACDeleteAuthorityRuleSQL As String = "spe_Authority_Rule_del"

    Public Const ACSelectRuleSetsStored As Boolean = True
    Public Const ACSelectRuleSetsName As String = "SelectRuleSets"
    'Developer Guide No. 39
    Public Const ACSelectRuleSetsSQL As String = "spu_Rule_Set_sel"

    'Authority Rule Detail stuff.
    Public Const ACInsertAuthorityRuleDetailStored As Boolean = True
    Public Const ACInsertAuthorityRuleDetailName As String = "InsertAuthorityRuleDetail"
    'Developer Guide No. 39
    Public Const ACInsertAuthorityRuleDetailSQL As String = "spe_Authority_Rule_Detail_add"

    Public Const ACDeleteAuthorityRuleDetailStored As Boolean = True
    Public Const ACDeleteAuthorityRuleDetailName As String = "DeleteAuthorityRuleDetail"
    'Developer Guide No. 39
    Public Const ACDeleteAuthorityRuleDetailSQL As String = "spe_Authority_Rule_Detail_del"

    Public Const ACSelectAuthorityRuleDetailStored As Boolean = True
    Public Const ACSelectAuthorityRuleDetailName As String = "SelectAuthorityRuleDetail"
    'Developer Guide No. 39
    Public Const ACSelectAuthorityRuleDetailSQL As String = "spu_Authority_Rule_Detail_sel"

    Public Const ACDeleteAuthorityRuleDetailForRuleStored As Boolean = True
    Public Const ACDeleteAuthorityRuleDetailForRuleName As String = "DeleteAuthorityRuleDetailForRule"
    'Developer Guide No. 39
    Public Const ACDeleteAuthorityRuleDetailForRuleSQL As String = "spu_Auth_Rule_Det_For_Rule_del"

    Public Const ACSelectUALTransactionTypeStored As Boolean = True
    Public Const ACSelectUALTransactionTypeName As String = "DeleteAuthorityRuleDetailForRule"
    'Developer Guide No. 39
    Public Const ACSelectUALTransactionTypeSQL As String = "spu_SIR_UAL_Transaction_Type_Sel"

    Public Const ACSaaRuleTypeStored As Boolean = True
    Public Const ACSaaRuleTypeName As String = "SelRule_Type"
    Public Const ACSaaRuleTypeSQL As String = "spu_Rule_type_select_filtered"
End Module