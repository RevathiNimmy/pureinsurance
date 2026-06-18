Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	
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
	
	''Start(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.4)
	Public Const ACSelectUserAuthority As String = "spu_Specific_User_Authority_Sel"
	Public Const ACSelectUserAuthorityScrName As String = "SelectUserAuthority"
	Public Const ACSelectUserAuthorityStored As Boolean = True
	''End(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.4)
	
	' DA QBE05
	
	Public Const ACRefreshRI2007Name As String = "RefreshRIArrangementRI2007"
	Public Const ACRefreshRI2007SQL As String = "spu_RI_Arrangement_refresh_RI2007"
	Public Const ACRefreshRI2007Stored As Boolean = True
	
	Public Const ACSelectRIArrangementLineRI2007Name As String = "SelectRIArrangementLinesRI2007"
	Public Const ACSelectRIArrangementLineRI2007SQL As String = "spu_RI_Arrangement_Line_saa_RI2007"
	Public Const ACSelectRIArrangementLineRI2007Stored As Boolean = True
	
	Public Const ACUpdateRIArrangementLineRI2007Name As String = "UpdateRIArrangementLinesRI2007"
	Public Const ACUpdateRIArrangementLineRI2007SQL As String = "spu_RI_Arrangement_Line_upd_RI2007"
	Public Const ACUpdateRIArrangementLineRI2007Stored As Boolean = True
	
	Public Const ACDeleteRIArrangementLineRI2007Name As String = "DeleteRIArrangementLinesRI2007"
	Public Const ACDeleteRIArrangementLineRI2007SQL As String = "spu_RI_Arrangement_Line_del_RI2007"
	Public Const ACDeleteRIArrangementLineRI2007Stored As Boolean = True
	
	Public Const ACAddBrokerParticipantsName As String = "AddBrokerParticipants"
	Public Const ACAddBrokerParticipantsSQL As String = "Spu_Sir_AddBrokerParticipants"
	Public Const ACAddBrokerParticipantsStored As Boolean = True
	
	Public Const ACUpdatePremiumPercentName As String = "Update Premium Percent Field"
	Public Const ACUpdatePremiumPercentSQL As String = "spu_upd_Premium_Percent_RI2007"
	
	Public Const ACGetGroupingIdName As String = "GetGroupingId"
    Public Const ACGetGroupingIdSQL As String = "Spu_RI_Arrangement_GetGroupingId"

    'Start E016
    Public Const ACSelPartyInsurerSQL As String = "spu_SIR_GetPartyInsurerDetails"
    Public Const ACSelPartyInsurerName As String = "SelectPartyInsurer"
    Public Const ACSelPartyInsuredStored As Boolean = True
    'End E016

    Public Const kSelectRIArrangementVersions As String = "SelectRIArrangementVersions"
    Public Const kSelectRIArrangementVersionsSQL As String = "spu_RI_Arrangement_sel_versions"
    Public Const kSelectRIArrangementVersionsStored As Boolean = True

    Public Const kSelectRatingSections As String = "SelectRatingSections"
    Public Const kSelectRatingSectionsSQL As String = "spu_Is_Rating_Section_Deleted"
    Public Const kSelectRatingSectionsStored As Boolean = True


End Module