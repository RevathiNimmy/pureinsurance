Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 06/09/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMURenSelection"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
    'developer guide no.50
    'developer guide no. 107
    <ThreadStatic()> _
    Public objfrmInterface As frmInterface

	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACRenewalDate As Integer = 102
	Public Const ACProductCode As Integer = 103
	Public Const ACPreview As Integer = 104
	Public Const ACPrint As Integer = 105
	Public Const ACPolicyRef As Integer = 106
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACReprintButton As Integer = 204
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Constants to define print operations
	Public Const AC_VIEW_ONLY As Integer = 0
	Public Const AC_PRINT_ONLY As Integer = 1
	Public Const AC_PRINT_AND_VIEW As Integer = 2
	
	'RWH(16/11/2000) Renewal List Position constants.
	Public Const PMFieldPosInsuranceFileCnt As Integer = 0
	Public Const PMFieldPosInsuranceHolderCnt As Integer = 1
	Public Const PMFieldPosProductID As Integer = 2
	Public Const PMFieldPosLeadAgentCnt As Integer = 3
	Public Const PMFieldPosInsuranceRef As Integer = 4
	Public Const PMFieldPosCoverStartDate As Integer = 5
	Public Const PMFieldPosCoverEndDate As Integer = 6
	Public Const PMFieldPosClientName As Integer = 7
	Public Const PMFieldPosAgentName As Integer = 8
	Public Const PMFieldPosIsAutoRenewable As Integer = 9
	Public Const PMFieldPosProductCode As Integer = 10
	Public Const PMFieldPosPolicyStopReason As Integer = 11
	Public Const PMFieldPosClientStopReason As Integer = 12
	Public Const PMFieldPosReferredAtRenewal As Integer = 13
	Public Const PMFieldPosInsuranceFolderCnt As Integer = 14
	Public Const PMFieldPosRenewalDate As Integer = 15 'RWH(17/05/2001)
	Public Const PMFieldPosClientCode As Integer = 16
	Public Const PMFieldPosAgentStopReason As Integer = 17
	Public Const PMFieldPosIsDeleted As Integer = 18
	Public Const PMFieldPosIsInTransferMode As Integer = 19
	Public Const PMFieldPosIsTrueMonthlyPolicy As Integer = 20
	Public Const PMFieldPosAnniversaryCopy As Integer = 21
	Public Const PMFieldPosRenewalDayNumber As Integer = 22
	Public Const PMFieldPosAnniversaryDate As Integer = 23
	Public Const PMFieldPosAnniversaryRenewalWeeks As Integer = 24
	Public Const PMFieldPosPutOnNextInstalmentRenewal As Integer = 25
	Public Const PMFieldPosLatestInstalmentPlanInsuranceFileCnt As Integer = 26
	Public Const PMFieldPosLeadAllowConsolidatedCommission As Integer = 27
	Public Const PMFieldPosSubAllowConsolidatedCommission As Integer = 28
	Public Const PMFieldPosRenewalCount As Integer = 29
	'1.12 WR25
	Public Const PMFieldPosRenewalProductId As Integer = 30
	Public Const PMFieldPosOriginalProductId As Integer = 31
    Public Const PMFieldPosTMPAutoRenFAC As Integer = 32
    Public Const PMFieldAlternateReference As Integer = 34
	'RWH(23/11/2000) Risk Array position constants.
	Public Const ACRiskPosCnt As Integer = 0
	Public Const ACRiskPosStatusId As Integer = 1
	Public Const ACRiskPosFolder As Integer = 2
	Public Const ACRiskPosAccumId As Integer = 3
	Public Const ACRiskPosTypeId As Integer = 4
	Public Const ACRiskPosDescription As Integer = 5
	Public Const ACRiskPosSequenceNo As Integer = 6
	Public Const ACRiskPosSumInsReq As Integer = 7
	Public Const ACRiskPosInceptionDate As Integer = 8
	Public Const ACRiskPosExpiryDate As Integer = 9
	Public Const ACRiskPosIsNotIndexLinked As Integer = 10
	Public Const ACRiskPosIsAccum As Integer = 11
	Public Const ACRiskPosLapsedReasonId As Integer = 12
	Public Const ACRiskPosLapsedDate As Integer = 13
	Public Const ACRiskPosLapsedDesc As Integer = 14
	Public Const ACRiskPosVarDataRef As Integer = 15
	Public Const ACRiskPosTotalSumIns As Integer = 16
	Public Const ACRiskPosTotalAnnualPrem As Integer = 17
	Public Const ACRiskPosTotalThisPrem As Integer = 18
	Public Const ACRiskPosIsRiAtRiskLevel As Integer = 19
	Public Const ACRiskPosIsAutoReinsured As Integer = 20
	Public Const ACRiskPosGisScreenId As Integer = 21
	
	
	'TN20010719
	Public Const PMIsQuotedDesc As String = "Not Quoted"
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
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
	
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Public Const ScreenhelpID As Integer = 4099
	
	' policy discount constants
	Public Const kDiscountRecurringTypeIdTransaction As Integer = 1
	Public Const kDiscountRecurringTypeIdTerm As Integer = 2
	Public Const kDiscountRecurringTypeIdPolicy As Integer = 3
	
    
	Sub Main_Renamed()
		
	End Sub
End Module