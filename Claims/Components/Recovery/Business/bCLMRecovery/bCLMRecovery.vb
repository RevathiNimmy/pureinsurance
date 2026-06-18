Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name:  MainModule
	' Date:         24/08/2000
	' Description:  Main Module.
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "bCLMRecovery"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	' Recovery array constants
	Public Const ACRRecoveryID As Integer = 0
	Public Const ACRPerilID As Integer = 1
	Public Const ACRRecoveryTypeID As Integer = 2
	Public Const ACRRecoveryType As Integer = 3
	Public Const ACRCurrencyID As Integer = 4
	Public Const ACRCurrency As Integer = 5
	Public Const ACRInitialReserve As Integer = 6
	Public Const ACRRevisedReserve As Integer = 7
	Public Const ACRReceivedToDate As Integer = 8
	Public Const ACRRevisionCount As Integer = 9
	Public Const ACRTaxToDate As Integer = 10
	Public Const ACRClaimID As Integer = 11
	Public Const ACRIsPostTaxes As Integer = 12
	'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
	Public Const ACRRecoveryPartyTypeID As Integer = 15
	Public Const ACRRecoveryPartyTypeCnt As Integer = 16
	Public Const ACRRecoveryParty As Integer = 17
	Public Const ACRRecoveryPartyDesc As Integer = 18
	Public Const ACRRecoveryClientAgentDefault As Integer = 0
	Public Const ACRRecoveryClientCode As Integer = 1
	Public Const ACRRecoveryClientName As Integer = 2
	Public Const ACRRecoveryAgentCode As Integer = 4
	Public Const ACRRecoveryAgentName As Integer = 5
	Public Const ACRRecoveryClientID As Integer = 0
	Public Const ACRRecoveryAgentID As Integer = 3
	'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
	
	' Coinsurance array constants
	Public Const ACCIRecoveryID As Integer = 0
	Public Const ACCIPartyCnt As Integer = 1
	Public Const ACCIDescription As Integer = 2
	Public Const ACCISharePercent As Integer = 3
	Public Const ACCIPaidToDate As Integer = 4
	Public Const ACCIIsTaxShared As Integer = 5
	Public Const ACCIThisPayment As Integer = 6
	Public Const ACCIThisPaymentLoss As Integer = 7
	Public Const ACCITaxAmount As Integer = 8
	Public Const ACCITaxAmountLoss As Integer = 9
	Public Const ACCIPaymentID As Integer = 10
	Public Const ACCIMAX As Integer = 10
	
	' Reinsurance array constants
	Public Const ACRIRecoveryID As Integer = 0
	Public Const ACRIArrangmentLineID As Integer = 1
	Public Const ACRITreatyID As Integer = 2
	Public Const ACRIFACPartyCnt As Integer = 3
	Public Const ACRIDescription As Integer = 4
	Public Const ACRISharePercent As Integer = 5
	Public Const ACRIPaidToDate As Integer = 6
	Public Const ACRIIsTaxShared As Integer = 7
	Public Const ACRIThisPayment As Integer = 8
	Public Const ACRIThisPaymentLoss As Integer = 9
	Public Const ACRITaxAmount As Integer = 10
	Public Const ACRITaxAmountLoss As Integer = 11
	Public Const ACRIPaymentID As Integer = 12
	Public Const ACRIMAX As Integer = 12
End Module