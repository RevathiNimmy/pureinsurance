Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule

    Public Const ACApp As String = "iPMURenewalCatchUp"

    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager
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
    Public Const PMFieldPosRenewalDate As Integer = 15
    Public Const PMFieldPosHolderName As Integer = 16
    Public Const PMFieldPosAgentStopReason As Integer = 17
    Public Const PMFieldPosClosedBranch As Integer = 18
    Public Const PMFieldPosAgentInTransfer As Integer = 19
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
    Public Const PMIsQuotedDesc As String = "Not Quoted"
    Public Const kDiscountRecurringTypeIdPolicy As Integer = 3

    ' Constants for the Renewals search data array indexes.
    Public Const ACIRenewalStatusCnt As Integer = 0
    Public Const ACIRenewalProduct As Integer = 1
    Public Const ACIRenewalInsuranceHolder As Integer = 2
    Public Const ACIRenewalShortname As Integer = 3
    Public Const ACIRenewalPartyType As Integer = 4
    Public Const ACIRenewalLiveInsuranceRef As Integer = 5
    Public Const ACIRenewalPolicyCnt As Integer = 6
    Public Const ACIRenewalInsuranceRef As Integer = 7
    Public Const ACIRenewalInsuranceFolder As Integer = 8
    Public Const ACIRenewalInsuranceStructID As Integer = 9
    Public Const ACIRenewalStatusTypeId As Integer = 10
    Public Const ACIRenewalStatusType As Integer = 11
    Public Const ACIRenewalCriticalDate As Integer = 12
    Public Const ACIRenewalLivePolicyCnt As Integer = 13
    Public Const ACIRenewalCoverStartDate As Integer = 14
    Public Const ACIRenewalExpiryDate As Integer = 15
    Public Const ACIRenewalAgentCnt As Integer = 16
    Public Const ACIRenewalProductId As Integer = 17
    Public Const ACIRenewalDate As Integer = 18
    Public Const ACIRenewalLeadAgentCode As Integer = 19
    Public Const ACIRenewalAccHandlerCode As Integer = 20
    Public Const ACIRenewalSourceCode As Integer = 21
    Public Const ACIRenewalClaimsIndicator As Integer = 22
    Public Const ACIRenewalSourceID As Integer = 23
    Public Const ACRenewalDeleteFromListView As Integer = 24 'set to 1 if this record is deleted from listview
    Public Const ACIRenewalIsBranchDeleted As Integer = 25
    Public Const ACIRenewalIsInTransferMode As Integer = 26
    Public Const ACIRenewalTransferToPartyCnt As Integer = 27
    Public Const ACIRenewalTransferToPartyShortName As Integer = 28
    Public Const ACIRenewalLivePolicyAgentCode As Integer = 29
    Public Const ACIRenewalIsTrueMonthlyPolicy As Integer = 30
    Public Const ACIRenewalAnniversaryCopy As Integer = 31
    Public Const ACIPaymentMethod As Integer = 32
    Public Const ACIRenewalResolvedName As Integer = 34
    Public Const ACIRenewalLeadAgentDescription As Integer = 35

    Public Const ACRiskPosCnt As Integer = 0

    'Doc Generation modes.
    Public Const ACPrintMode As Integer = 2
    Public Const ACPrintSilentMode As Integer = 3
    Public Const ACSpoolSilentMode As Integer = 4

    Public Const ACDOCTypeDebitNote As Integer = 3
    Public Const ACDocTypeSchedule As Integer = 4
    Public Const ACDocTypeCertificate As Integer = 5
    Public Const ACDocTypeLapse As Integer = 8
    Public Const ACDocTypeRenewalDebitNote As Integer = 14
    Public Const ACDocTypeNoticePrint As Integer = 6

    Public Const ACRenModeStandard As Integer = 0
    Public Const ACRenModeRI As Integer = 1 'renewal invites
    Public Const ACRenModeRAA As Integer = 2 'renewal acceptance with amendment
    Public Const ACRenModeRA As Integer = 3 'renewal acceptance without amendment
    ' Public instance of the object manager.
    'developer guide no. 107

End Module
