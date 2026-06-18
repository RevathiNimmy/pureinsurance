Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '

	' RI Arrangement
	Public Const ACRefreshRIName As String = "RefreshRIArrangement"
	Public Const ACRefreshRISQL As String = "spu_RI_Arrangement_refresh"
    Public Const ACRefreshRIStored As Boolean = True

    Public Const ACSelectRIArrangementName As String = "SelectRIArrangements"
	Public Const ACSelectRIArrangementSQL As String = "spu_RI_Arrangement_saa"
	Public Const ACSelectRIArrangementStored As Boolean = True
	
	Public Const ACSelectRIArrangementBandsName As String = "SelectRIArrangementBands"
	Public Const ACSelectRIArrangementBandsSQL As String = "spu_RI_Arrangement_sel_bands"
	Public Const ACSelectRIArrangementBandsStored As Boolean = True
	
	Public Const ACUpdateRIArrangementName As String = "UpdateRIArrangement"
	Public Const ACUpdateRIArrangementSQL As String = "spu_RI_Arrangement_upd"
	Public Const ACUpdateRIArrangementStored As Boolean = True

	' RI Arrangment Line
	Public Const ACInsertRIArrangementLineName As String = "InsertRIArrangementLines"
	Public Const ACInsertRIArrangementLineSQL As String = "spu_RI_Arrangement_Line_add"
	Public Const ACInsertRIArrangementLineStored As Boolean = True
	
	Public Const ACSelectRIArrangementLineName As String = "SelectRIArrangementLines"
	Public Const ACSelectRIArrangementLineSQL As String = "spu_RI_Arrangement_Line_saa"
	Public Const ACSelectRIArrangementLineStored As Boolean = True
	
	Public Const ACUpdateRIArrangementLineName As String = "UpdateRIArrangementLines"
	Public Const ACUpdateRIArrangementLineSQL As String = "spu_RI_Arrangement_Line_upd"
	Public Const ACUpdateRIArrangementLineStored As Boolean = True
	
    ' Treaty details
	Public Const ACSelectTreatyName As String = "SelectTreaty"
	Public Const ACSelectTreatySQL As String = "spu_Treaty_sel"
	Public Const ACSelectTreatyStored As Boolean = True
	
    ' Risk
	Public Const ACUpdateRiskStatusStored As Boolean = True
	Public Const ACUpdateRiskStatusName As String = "UpdateRiskStatus"
	Public Const ACUpdateRiskStatusSQL As String = "spu_UpdateRiskStatus"
	
	Public Const ACAutoReinsureRiskStored As Boolean = True
	Public Const ACAutoReinsureRiskName As String = "AutoReinsureRisk"
	Public Const ACAutoReinsureRiskSQL As String = "spu_auto_reinsure_risk"
	

	' Taxes
	Public Const ACCalculateTreatyTaxName As String = "Calculate Treaty Amount"
	Public Const ACCalculateTreatyTaxSQL As String = "spu_SIR_Calculate_Treaty_Tax_Amounts"
	Public Const ACCalculateTreatyTaxStored As Boolean = True
	
    Public Const ACCalculateFaculativeTaxName As String = "Calculate Treaty Party Amount"
	Public Const ACCalculateFaculativeTaxSQL As String = "spu_SIR_Calculate_Treaty_Party_Tax_Amounts"
	Public Const ACCalculateFaculativeTaxStored As Boolean = True
	
	Public Const ACDeleteTaxCalculationEntriesName As String = "DeleteTaxEntries"
	Public Const ACDeleteTaxCalculationEntriesSQL As String = "spu_SIR_Delete_Tax_Calculations"
	Public Const ACDeleteTaxCalculationEntriesStored As Boolean = True
	
	'Display Reinsurance Screen
	Public Const ACAutoDisReinsureScrStored As Boolean = True
	Public Const ACAutoDisReinsureScrName As String = "AutoDisReinsureScr"
	Public Const ACAutoDisReinsureScrSQL As String = "spu_Reinsurance_Screen_sel"
	
    Public Const ACSelectUserAuthority As String = "spu_Specific_User_Authority_Sel"
	Public Const ACSelectUserAuthorityScrName As String = "SelectUserAuthority"
	Public Const ACSelectUserAuthorityStored As Boolean = True
    ' PN71191
	Public Const ACSelectRIArrangementGetSumInsuredAndPremiumName As String = "SelectRIArrangementsGetSumInsuredAndPremium"
	Public Const ACSelectRIArrangementGetSumInsuredAndPremiumSQL As String = "spu_RI_Arrangement_Get_SumInsuredAndPremium_ByRisk"
	Public Const ACSelectRIArrangementGetSumInsuredAndPremiumStored As Boolean = True

    Public Const kRefreshRIName As String = "RefreshRIArrangementForRI2007Disabled"
    Public Const kRefreshRISQL As String = "spu_RI2007Disabled_Arrangement_refresh"
    Public Const kRefreshRIStored As Boolean = True
End Module