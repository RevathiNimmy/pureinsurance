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
	' Class Name    :BusinessSQL
	' Date          :16/08/2000
	' Description   :Contains the SQL Statements required by the
	'               bCLMDefnFlds.Business class.
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	' Example select using embedded SQL
	' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
	' Public Const ACSelectStored = False
	' Public Const ACSelectName = "SelectRisk"
	' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"
	
	
	' Select All Risk Data Defn SQL
	Public Const ACGetRiskDataDefnStored As Boolean = True
    Public Const ACGetRiskDataDefnName As String = "GetRiskDataDefn"

    'Developer Guide No.: 39
    Public Const ACGetRiskDataDefnSQL As String = "spu_get_risk_data_defn"
	
	' Select All Peril Data Defn SQL
	Public Const ACGetPerilDataDefnStored As Boolean = True
    Public Const ACGetPerilDataDefnName As String = "GetPerilDataDefn"

    'Developer Guide No.: 39
    Public Const ACGetPerilDataDefnSQL As String = "spu_get_peril_data_defn"
	
	' Select All Lookup Tables SQL
	Public Const ACSelLookupTablesStored As Boolean = True
	Public Const ACSelLookupTablesName As String = "SelLookupTables"

    'Developer Guide No.: 39
    Public Const ACSelLookupTablesSQL As String = "spu_sel_lookup_tables"

	' Select All Party Types SQL
	Public Const ACSelPartyTypesStored As Boolean = True
	Public Const ACSelPartyTypesName As String = "SelPartyTypes"

    'Developer Guide No.: 39
    Public Const ACSelPartyTypesSQL As String = "spu_sel_party_types"
	
	' Select Check Risk Caption Exists SQL
	Public Const ACChkRiskCaptionExistsStored As Boolean = True
	Public Const ACChkRiskCaptionExistsName As String = "ChkRiskCaptionExists"

    'Developer Guide No.: 39
    Public Const ACChkRiskCaptionExistsSQL As String = "spu_chk_risk_caption_exists"
	
	' Select Check Peril Caption Exists SQL
	Public Const ACChkPerilCaptionExistsStored As Boolean = True
	Public Const ACChkPerilCaptionExistsName As String = "ChkPerilCaptionExists"

    'Developer Guide No.: 39
    Public Const ACChkPerilCaptionExistsSQL As String = "spu_chk_peril_caption_exists"
	
	'*****************
	'' Select Check Risk Lookup Exists SQL
	'Public Const ACChkRiskLookupExistsStored = True
	'Public Const ACChkRiskLookupExistsName = "ChkRiskLookupExists"
	'Public Const ACChkRiskLookupExistsSQL = "{call spu_chk_risk_lookup_exists (?,?)}"
	'
	'' Select Check Peril Lookup Exists SQL
	'Public Const ACChkPerilLookupExistsStored = True
	'Public Const ACChkPerilLookupExistsName = "ChkPerilLookupExists"
	'Public Const ACChkPerilLookupExistsSQL = "{call spu_chk_peril_lookup_exists (?,?)}"
	
	'*****************
	
	' Select Check Data Definition Id Exists SQL
	Public Const ACChkDataDefnIDExistsStored As Boolean = True
    Public Const ACChkDataDefnIDExistsName As String = "ChkDataDefnIdExists"

    'Developer Guide No.: 39
    Public Const ACChkDataDefnIDExistsSQL As String = "spu_chk_data_defn_id_exists"
	
	' Select Check Data Definition Id For Party Exists SQL
	Public Const ACChkDataDefnIDForPartyExistsStored As Boolean = True
    Public Const ACChkDataDefnIDForPartyExistsName As String = "ChkDataDefnIdForPartyExists"

    'Developer Guide No.: 39
    Public Const ACChkDataDefnIDForPartyExistsSQL As String = "spu_chk_data_defn_prty_exists"
	
	
	' Select Check Risk Caption Exists SQL
	Public Const ACChkRiskDispOrdExistsStored As Boolean = True
    Public Const ACChkRiskDispOrdExistsName As String = "ChkRiskDisplayOrderExists"

    'Developer Guide No.: 39
    Public Const ACChkRiskDispOrdExistsSQL As String = "spu_chk_risk_disp_ord_exists"
	
	' Select Check Datan Definition Id Exists SQL
	Public Const ACChkPerilDispOrdExistsStored As Boolean = True
    Public Const ACChkPerilDispOrdExistsName As String = "ChkPerilDisplayOrderExists"

    'Developer Guide No.: 39
    Public Const ACChkPerilDispOrdExistsSQL As String = "spu_chk_peril_disp_ord_exists"
	'*******************************
	
	' Select All GetClmForResvType SQL
	Public Const ACGetClmForResvTypeStored As Boolean = True
    Public Const ACGetClmForResvTypeName As String = "GetClmForResvType"

    'Developer Guide No.: 39
    Public Const ACGetClmForResvTypeSQL As String = "spu_get_clm_for_resv_type"
	
	'' Check ID SQL
	'Public Const ACCheckIDStored = True
	'Public Const ACCheckIDName = "CheckCLMDefnFldsID"
	'Public Const ACCheckIDSQL = "{call spe_CLMDefnFlds_check_id (?)}"
	
	
	'CT 15/11/00 constants for adding user defined mege codes into wp_fields table
	' Insert WP Fields SQL
	Public Const ACInsertWPFieldsStored As Boolean = True
    Public Const ACInsertWPFieldsName As String = "InsertWPFields"

    'Developer Guide No.: 39
    Public Const ACInsertWPFieldsSQL As String = "spe_wp_fields_add"

	'CT 20/11/00 constants for selecting claim risk details
	Public Const ACSelectClaimRiskDetailsStored As Boolean = True
    Public Const ACSelectClaimRiskDetailsName As String = "SelectClaimRisk"

    'Developer Guide No.: 39
    Public Const ACSelectClaimRiskDetailsSQL As String = "spu_claimriskdetails"
	
	'DC240501 constants for selecting claim peril details
	Public Const ACSelectClaimPerilDetailsStored As Boolean = True
    Public Const ACSelectClaimPerilDetailsName As String = "SelectClaimPeril"

    'Developer Guide No.: 39
    Public Const ACSelectClaimPerilDetailsSQL As String = "spu_claimperildetails"
	
	Public Const ACTabsListStored As Boolean = True
    Public Const ACTabsListName As String = "ClaimTabList"

    'Developer Guide No.: 39
    Public Const ACTabsListSQL As String = "spu_Claim_Tab_List"
End Module