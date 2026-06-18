Option Strict Off
Option Explicit On
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  27th September 1996
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bFindInsurance"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Constants for the search data array indexes.
	'EK 140199 Bug 206 List replaced to be compatible with interface
	Public Const ACIInsFileId As Integer = 0
	Public Const ACIInsFileSourceId As Integer = 1
	Public Const ACIInsFileCnt As Integer = 2
	Public Const ACIInsReference As Integer = 3
	Public Const ACIInsFolderName As Integer = 4
	Public Const ACIInsFileType As Integer = 5
	Public Const ACIInsuredLongName As Integer = 6
	Public Const ACIInsuredShortName As Integer = 7
	Public Const ACIInsuredId As Integer = 8
	Public Const ACIInsuredSourceId As Integer = 9
	Public Const ACILastModified As Integer = 10
	Public Const ACIInsHolderCnt As Integer = 11
	Public Const ACIInsFolderCnt As Integer = 12
	Public Const ACIProductID As Integer = 13
	Public Const ACIProductCode As Integer = 14
	Public Const ACIProductName As Integer = 15
	Public Const ACILeadAgentCnt As Integer = 16
	Public Const ACIDateCreated As Integer = 17
	Public Const ACIRegistration As Integer = 18 ' Tom 02/11/98
	Public Const ACIGISProperty As Integer = 18 ' Tom 29/06/00
	Public Const ACIIndexValue As Integer = 19 ' Tom 29/06/00
	
	' ED 05082002 : Maximum no of search results
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
    Public obj_m_vMTAInsuranceFileLinkArray As Object
	'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup START
	'KNCMGRISK Start
	'Array constants for the Risk Folder
	Public Const ACRFRiskFolderCnt As Integer = 0
	Public Const ACRFRiskFolderId As Integer = 1
	Public Const ACRFSourceId As Integer = 2
	Public Const ACRFRiskFolderTypeId As Integer = 3
	Public Const ACRFCode As Integer = 4
	Public Const ACRFDescription As Integer = 5
	Public Const ACRFInsuranceFolderCnt As Integer = 6
	
	'Array constants for the Insurance File Risk Link
	Public Const ACIFRLInsuranceFileCnt As Integer = 0
	Public Const ACIFRLRiskCnt As Integer = 1
	Public Const ACIFRLStatusFlag As Integer = 2
	Public Const ACIFRLOriginalRiskCnt As Integer = 3
	
	'Array constants for the Risk
	Public Const ACRRiskId As Integer = 0
	Public Const ACRRiskStatusId As Integer = 1
	Public Const ACRRiskFolderCnt As Integer = 2
	Public Const ACRAccumulationId As Integer = 3
	Public Const ACRRiskTypeId As Integer = 4
	Public Const ACRDescription As Integer = 5
	Public Const ACRSequenceNumber As Integer = 6
	Public Const ACRSumInsuredRequested As Integer = 7
	Public Const ACRInceptionDate As Integer = 8
	Public Const ACRExpiryDate As Integer = 9
	Public Const ACRIsNotIndexLinked As Integer = 10
	Public Const ACRIsAccumulated As Integer = 11
	Public Const ACRLapsedReasonId As Integer = 12
	Public Const ACRLapsedDate As Integer = 13
	Public Const ACRLapsedDescription As Integer = 14
	Public Const ACRVarDataRef As Integer = 15
	Public Const ACRTotalSumInsured As Integer = 16
	Public Const ACRTotalAnnualPremium As Integer = 17
	Public Const ACRTotalThisPremium As Integer = 18
	Public Const ACRIsRiAtRiskLevel As Integer = 19
	Public Const ACRIsAutoReinsured As Integer = 20
	Public Const ACRGISScreenId As Integer = 21
	Public Const ACREMLPercentage As Integer = 22
	
	Public Const ACRMax As Integer = 22
	
	'Array constants for the Risk Type
	Public Const ACRTRiskTypeId As Integer = 0
	Public Const ACRTRiskFolderTypeId As Integer = 1
	Public Const ACRTCaptionId As Integer = 2
	Public Const ACRTCode As Integer = 3
	Public Const ACRTDescription As Integer = 4
	Public Const ACRTEffectiveDate As Integer = 5
	Public Const ACRTIsDeleted As Integer = 6
	Public Const ACRTVarDataStructureId As Integer = 7
	Public Const ACRTInterfaceObjectName As Integer = 8
	Public Const ACRTInterfaceClassName As Integer = 9
	Public Const ACRTOverridePerilRiBand As Integer = 10
	Public Const ACRTOverridePerilXlBand As Integer = 11
	Public Const ACRTNBPremiumProRateTypeId As Integer = 12
	Public Const ACRTMTAPremiumProRateTypeId As Integer = 13
	Public Const ACRTRNPremiumProRateTypeId As Integer = 14
	Public Const ACRTIsShareWithCoinsurers As Integer = 15
	Public Const ACRTIsShareWithReinsurers As Integer = 16
	Public Const ACRTIsSuppressPublicText As Integer = 17
	Public Const ACRTIsSuppressPrivateText As Integer = 18
	Public Const ACRTIsSuppressTaxes As Integer = 19
	Public Const ACRTReportPointer As Integer = 20
	Public Const ACRTSectionMask As Integer = 21
	Public Const ACRTStampDutyRate1 As Integer = 22
	Public Const ACRTStampDutyRate2 As Integer = 23
	Public Const ACRTPrimarySort As Integer = 24
	Public Const ACRTSecondarySort As Integer = 25
	Public Const ACRTHeaderClause As Integer = 26
	Public Const ACRTTrailerClause As Integer = 27
	Public Const ACRTIsRiAtRiskLevel As Integer = 28
	Public Const ACRTIsAutoReinsured As Integer = 29
	Public Const ACRTHeaderClauseId As Integer = 30
	Public Const ACRTTrailerClauseId As Integer = 31
	Public Const ACRTAccumulationLevel As Integer = 32
	Public Const ACRTGISScreenId As Integer = 33
	'KNCMGRISK End
	'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup END
	
	
	Public Const AC4InsuranceFileCnt As Integer = 0
	Public Const AC4TypeInd As Integer = 1
	Public Const AC4InsuranceFileStatusId As Integer = 2
	Public Const AC4SequenceNo As Integer = 3
	'Public Const AC4ArraySize = 3
	Public Const AC4InsuranceFileType As Integer = 4
	Public Const AC4ArraySize As Integer = 4
	
	Public Const AC3StatusOnly As Integer = 0
	Public Const AC3Cancellation As Integer = 1
	Public Const AC3Reinstatement As Integer = 2
	Public Const AC3MTA As Integer = 3
End Module