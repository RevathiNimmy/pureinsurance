Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 05/05/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMURiskType"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 123
	Public Const ACTabTitle4 As Integer = 124
	
	Public Const ACCode As Integer = 103
	Public Const ACEffectiveDate As Integer = 104
	Public Const ACDescription As Integer = 105
	Public Const ACReportPointer As Integer = 106
	Public Const ACSectionMask As Integer = 107
	Public Const ACStampDutyRate1 As Integer = 108
	Public Const ACStampDutyRate2 As Integer = 109
	Public Const ACPrimarySort As Integer = 110
	Public Const ACSecondarySort As Integer = 111
	Public Const ACHeaderClause As Integer = 112
	Public Const ACTrailerClause As Integer = 113
	Public Const ACShareWithCoInsurer As Integer = 114
	Public Const ACShareWithReInsurer As Integer = 115
	Public Const ACSuppressPublicText As Integer = 116
	Public Const ACSuppressPrivateText As Integer = 117
	Public Const ACSuppressTaxes As Integer = 118
	Public Const ACIsRiAtRiskLevel As Integer = 119
	Public Const ACIsAutoReinsured As Integer = 120
	Public Const ACAccumulationLevel As Integer = 121
	Public Const ACGISScreenId As Integer = 122
	Public Const ACAssociatedClientScreen As Integer = 128
	Public Const ACDisclosureScreen As Integer = 129
	
	'JMK 22/10/2001 display Insurer/Reinsurer
	Public Const ACApplyAtRiskLevel As Integer = 125
	Public Const ACAutoAllocation As Integer = 126
	Public Const ACShareWithInsurer As Integer = 127


	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	Public Const ACUndeleteButton As Integer = 213
	
	'TN20001010
	Public Const ACRIModelButton As Integer = 207
	Public Const ACApplyButton As Integer = 208
	
	Public Const ACRILimitButton As Integer = 209
	
	'JMK 22/10/2001 display Insurer/Reinsurer
	Public Const ACInsurerModelButton As Integer = 210
	Public Const ACInsurerLimitButton As Integer = 211
	' AMB 30/05/2003: 1.8.6 Deferred RI RFC
	Public Const ACDeferredRIButton As Integer = 212
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	
	Public Const ACIrisk_type_id As Integer = 0
	Public Const ACIrisk_folder_type_id As Integer = 1
	Public Const ACIcaption_id As Integer = 2
	Public Const ACICode As Integer = 3
	Public Const ACIDescription As Integer = 4
	Public Const ACIeffective_date As Integer = 5
	Public Const ACIis_deleted As Integer = 6
	Public Const ACIvar_data_structure_id As Integer = 7
	Public Const ACIinterface_object_name As Integer = 8
	Public Const ACIinterface_class_name As Integer = 9
	Public Const ACIoverride_peril_ri_band As Integer = 10
	Public Const ACIoverride_peril_xl_band As Integer = 11
	Public Const ACInb_premium_pro_rata_type_id As Integer = 12
	Public Const ACImta_premium_pro_rata_type_id As Integer = 13
	Public Const ACIrn_premium_pro_rata_type_id As Integer = 14
	Public Const ACIis_share_with_co_insurers As Integer = 15
	Public Const ACIis_share_with_re_insurers As Integer = 16
	Public Const ACIis_suppress_public_text As Integer = 17
	Public Const ACIis_suppress_private_text As Integer = 18
	Public Const ACIis_suppress_taxes As Integer = 19
	Public Const ACIreport_pointer As Integer = 20
	Public Const ACIvsection_mask As Integer = 21
	Public Const ACIstamp_duty_rate1 As Integer = 22
	Public Const ACIstamp_duty_rate2 As Integer = 23
	Public Const ACIprimary_sort As Integer = 24
	Public Const ACIsecondary_sort As Integer = 25
	Public Const ACIheader_clause As Integer = 26
	Public Const ACItrailer_clause As Integer = 27
	Public Const ACIis_ri_at_risk_level As Integer = 28
	Public Const ACIis_auto_reinsured As Integer = 29
	Public Const ACIheader_clause_id As Integer = 30
	Public Const ACItrailer_clause_id As Integer = 31
	Public Const ACIaccumulation_level As Integer = 32
	Public Const ACIgis_screen_id As Integer = 33
	Public Const ACIheader_clause_description As Integer = 34
	Public Const ACItrailer_clause_description As Integer = 35
	Public Const ACIis_deferred_ri_permitted As Integer = 36
	Public Const ACIclaims_is_post_taxes As Integer = 37
	Public Const ACIdisplay_Reinsurance As Integer = 38
	
	''QBENZ022
	Public Const ACIAllowRatingSectionAdd As Integer = 39
	Public Const ACIAllowRatingSectionEdit As Integer = 40
	Public Const ACIAllowRatingSectionDelete As Integer = 41
	Public Const ACIAllowEditRatingSectionRateType As Integer = 42
	Public Const ACIAllowEditRatingSectionRate As Integer = 43
	Public Const ACIAllowEditRatingSectionSumInsured As Integer = 44
	Public Const ACIAllowEditRatingSectionThisPremium As Integer = 45
	'PLICO Claim Enhancements
	Public Const ACIdisplay_ClaimReinsurance As Integer = 46
	
    Public Const ACIClaimsTypeBasis As Integer = 47
    Public Const ACIClaimsCoverBasis As Integer = 48
    Public Const kAttachClaimOutsideOfPolicyPeriod As Integer = 49
	' Public contants used for the start and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Required for help text link
	Public Const ScreenHelpID As Integer = 4097
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
	Public Enum ENSelectClause
		id = 0
		Code = 1
		Description = 2
		Selected = 3
		Default_Renamed = 4
		Branch = 5
	End Enum
	
	Public Enum ENClauseType
		ProductType = 1
		RiskType = 2
	End Enum
	Public Const ACSelectClauseDefaultProductID As Integer = 0
	'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
	Sub Main_Renamed()
		
    End Sub
    
End Module