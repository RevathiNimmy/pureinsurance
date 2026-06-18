Option Strict Off
Option Explicit On
Imports System
Module MainModule
	'******************************************************************************
	' Module Name:      MainModule
	' History:          Created 06 July 2007
	' Description:      Main module containing public variable/constants.
	'******************************************************************************
	
	' Main public constant for all functions to identify which application this is
	Public Const ACApp As String = "uctCLMCaseClaimList"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	'**********************************************
	' list view for case claim
	'**********************************************
	Public Const KCaseColHIndexClaimid As Integer = 1
	Public Const kCaseColHIndexClaimNumber As Integer = 2
	Public Const kCaseColHIndexLossDate As Integer = 3
	Public Const kCaseColHIndexClaimHandler As Integer = 4
	Public Const kCaseColHIndexRiskType As Integer = 5
	Public Const kCaseColHIndexStatus As Integer = 6
	Public Const kCaseColHIndexTotalIndemnity As Integer = 7
	Public Const kCaseColHIndexTotalExpense As Integer = 8
	Public Const kCaseColHIndexTotalExcess As Integer = 9
	Public Const kCaseColHIndexInsuranceFileCnt As Integer = 10
	
	Public Const kTaxDetColHCodeClaimid As String = "ClaimId"
	Public Const kTaxDetColHCodeClaimNumber As String = "ClaimNumber"
	Public Const kTaxDetColHCodeLossDate As String = "LossDate"
	Public Const kTaxDetColHCodeClaimHandler As String = "ClaimHandler"
	Public Const kTaxDetColHCodeRiskType As String = "RiskType"
	Public Const kTaxDetColHCodeStatus As String = "Status"
	Public Const kTaxDetColHCodeTotalIndemnity As String = "TotalIndemnity"
	Public Const kTaxDetColHCodeTotalExpense As String = "TotalExpense"
	Public Const kTaxDetColHCodeTotalExcess As String = "TotalExcess"
	Public Const kTaxDetColHCodeInsuranceFileCnt As String = "PolicyID"
	
	'**********************************************
	' list view constants
	'**********************************************
	Public Const kPayDetailsSubItemsClaimNumber As Integer = 1
	Public Const kPayDetailsSubItemsLossdate As Integer = 2
	Public Const kPayDetailsSubItemsClaimHandler As Integer = 3
	Public Const kPayDetailsSubItemsRiskType As Integer = 4
	Public Const kPayDetailsSubItemsStatus As Integer = 5
	Public Const kPayDetailsSubItemsTotalIndemnity As Integer = 6
	Public Const kPayDetailsSubItemsTotalExpense As Integer = 7
	Public Const kPayDetailsSubItemsTotalExcess As Integer = 8
	Public Const kPayDetailsSubItemsInsuranceFileCnt As Integer = 9
	
	
	'**************************************************
	' case details array position constants
	'**************************************************
	Public Const kCaseClaimListClaimId As Integer = 0
	Public Const kCaseClaimListClaimNumber As Integer = 1
	Public Const kCaseClaimListLossDate As Integer = 2
	Public Const kCaseClaimListClaimHandler As Integer = 3
	Public Const kCaseClaimListRiskType As Integer = 4
	Public Const kCaseClaimListStatus As Integer = 5
	Public Const kCaseClaimListTotalIndemnity As Integer = 6
	Public Const kCaseClaimListTotalExpense As Integer = 7
	Public Const kCaseClaimListTotalExcess As Integer = 8
	Public Const kCaseClaimListInsuranceFileCnt As Integer = 9
	
	'**************************************************
	' Nav RoadMaps constants
	'**************************************************
	Public Const kRoadMapConstantOpenClaim As String = "OPENCLM"
	Public Const kRoadMapConstantMaintainClaim As String = "MAINCLM"
	Public Const kRoadMapConstantPayClaim As String = "PAYCLM"
	Public Const kRoadMapConstantSalvage As String = "SALVAGE"
	Public Const kRoadMapConstantTPRecovery As String = "TPRECOVER"
	
	
	'**************************************************
	' Registry Constants
	'**************************************************
	
	'List View Column Names
	Public Const kRegKeyConstLvwClaimId As Integer = 100
	Public Const kRegKeyConstLvwClaimNumber As Integer = 101
	Public Const kRegKeyConstLvwLossDate As Integer = 102
	Public Const kRegKeyConstLvwClaimHandler As Integer = 103
	Public Const kRegKeyConstLvwRiskType As Integer = 104
	Public Const kRegKeyConstLvwStatus As Integer = 105
	Public Const kRegKeyConstLvwTotalIndemnity As Integer = 106
	Public Const kRegKeyConstLvwTotalExpense As Integer = 107
	Public Const kRegKeyConstLvwTotalExcess As Integer = 108
	
	'Button
	Public Const kRegKeyConstOpenClaimButton As Integer = 200
	Public Const kRegKeyConstMaintainClaimButton As Integer = 201
	Public Const kRegKeyConstPayClaimButton As Integer = 202
	Public Const kRegKeyConstSalvageButton As Integer = 203
	Public Const kRegKeyConstTPRecoveryButton As Integer = 204
	Public Const kRegKeyConstLinkButton As Integer = 205
	Public Const kRegKeyConstUninkButton As Integer = 206
	'Message
	Public Const kRegKeyConstUnLinkMsg As Integer = 300
	Public Const kRegKeyConstEditCaseDocument As Integer = 301
	Public Const kRegKeyConstOpenCaseDocument As Integer = 302
	'************************************************
	' Constants for the search data array indexes.
	'************************************************
	Public Const kClaimDetailClaimId As Integer = 0
	Public Const kClaimDetailClaimNumber As Integer = 1
	Public Const kClaimDetailLossDate As Integer = 2
	Public Const kClaimDetailClaimHandler As Integer = 3
	Public Const kClaimDetailRiskType As Integer = 4
	Public Const kClaimDetailStatus As Integer = 5
	Public Const kClaimDetailTotalIndemnity As Integer = 6
	Public Const kClaimDetailTotalExpense As Integer = 7
	Public Const kClaimDetailTotalExcess As Integer = 8
	Public Const kClaimDetailInsuranceFileCnt As Integer = 9
	Public Const kClaimDetailBaseCaseID As Integer = 10
	Public Const kClaimDetailCaseNumber As Integer = 11
	
	'Case Details
	Public Const kCaseDetailCaseNumber As Integer = 0
	Public Const kCaseDetailCaseOpenedDate As Integer = 1
	Public Const kCaseDetailCaseVersion As Integer = 2
	Public Const kCaseDetailCaseProgress As Integer = 3
	Public Const kCaseDetailCaseAnalyst As Integer = 4
	Public Const kCaseDetailCaseAdmin As Integer = 5
	Public Const kCaseDetailTotalIndemnity As Integer = 6
	Public Const kCaseDetailTotalExpense As Integer = 7
	Public Const kCaseDetailTotalExcess As Integer = 8
	
	'Public variables
	'Public g_iSourceID As Integer
	'Public g_iLanguageID As Integer
	'Public g_iUserId As Integer
	'
	'Public g_lCurrencyID As Long
	'Public g_lInsurance_file_cnt As Long
End Module