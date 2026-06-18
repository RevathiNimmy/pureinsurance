Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 17/02/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	Public Const ScreenHelpID As Integer = 44000
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "uctListRiskControl"
	
	
	'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "long date"
	Public Const ACShortDate As String = "short date"
    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    <ThreadStatic()> _
 Public m_lCoverNoteUpTo As Integer
	
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	
	Public Const ACClientCode As Integer = 103
	Public Const ACStatus As Integer = 104
	
	' PW311002 - make constant names for column titles descriptive
	Public Const ACColRisk As Integer = 113
	Public Const ACColRiskDesc As Integer = 114
	Public Const ACColRiskTypeDesc As Integer = 115
	Public Const ACColStartDate As Integer = 116
	Public Const ACColEndDate As Integer = 117
	Public Const ACColRiskStatus As Integer = 118
	Public Const ACColSumInsured As Integer = 119
	Public Const ACColPremium As Integer = 120
	
	' PW311002 - additional list column titles
	Public Const ACColRiskNo As Integer = 121
	Public Const ACColRiskVar As Integer = 122
	Public Const ACColCoverage As Integer = 123
	Public Const ACColInsuredItem As Integer = 124
	Public Const ACColExtensions As Integer = 125
	
	Public Const ACPremiumTitle As Integer = 131
	Public Const ACColEditted As Integer = 132
    Public Const kColRiskLinkStatusTitle As Integer = 133
    Public Const kColRiskLinkDateTitle As Integer = 134
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACNewButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACFindNowButton As Integer = 206
	Public Const ACNewSearchButton As Integer = 207
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	'SB 31/03/98 defect 37
	Public Const ACLookupFailTitle As Integer = 308
	Public Const ACLookupFail As Integer = 309
	
	' Menus
    ' Constants for the search data array indexes.
	Public Const ACIInsFileCnt As Integer = 0
	Public Const ACIRiskId As Integer = 1
	Public Const ACIRiskDescription As Integer = 2
	Public Const ACIRiskTypeDescription As Integer = 3
	Public Const ACIRiskInceptionDate As Integer = 4
	Public Const ACIRiskExpiryDate As Integer = 5
	' AM 061200 Add new column for risk status
	Public Const ACIRiskStatus As Integer = 6
	Public Const ACIRiskTotalSumInsured As Integer = 7
	Public Const ACIRiskTotalAnnualPremium As Integer = 8
	Public Const ACIRiskGisScreen As Integer = 9
	Public Const ACIRiskTypeId As Integer = 10
	Public Const ACIInsuranceFolderCnt As Integer = 11
	Public Const ACIRiskStatusFlag As Integer = 12
	' PW311002 - add new columns for Risk Variations / Quote management
	Public Const ACIRiskNo As Integer = 13
	Public Const ACIVariationNo As Integer = 14
	Public Const ACIIsSelected As Integer = 15
	Public Const ACICoverage As Integer = 16
	Public Const ACIInsuredItem As Integer = 17
	Public Const ACIExtensions As Integer = 18
	' PW221102 - add risk tax
	' PS411
	Public Const ACIRiskTax As Integer = 19
	Public Const ACIFeeTax As Integer = 20
	Public Const ACIFeePremium As Integer = 21
	Public Const ACIRiskStatusCode As Integer = 22
	Public Const ACIRiskIsDiscounted As Integer = 23
	
	'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
	Public Const ACIRiskFolderCnt As Integer = 24
	
	Public Const ACICNNoteId As Integer = 25
	Public Const ACICNRef As Integer = 26
	Public Const ACICNFrom As Integer = 27
	Public Const ACICNTo As Integer = 28
	Public Const ACIPartyCnt As Integer = 29
	Public Const ACIAgentCnt As Integer = 30
	Public Const ACIRiskTypeCode As Integer = 31
	Public Const ACIDocTemplateId As Integer = 32
	Public Const ACIDocTemplateTypeId As Integer = 33
	
	Public Const ACIDocIsEditableAfterMerging As Integer = 34
    Public Const ACIStampDutyInsurer As Integer = 36
	Public Const ACIStampDutyInsured As Integer = 37
	
    Public Const ACICoverNoteUpTo As Integer = 39

    ' Wpr53
    Public Const ACIIsMandatoryRisk As Integer = 40
    Public Const kIRiskLinkStatus As Integer = 42
    Public Const kIRiskLingDate As Integer = 43
    Public Const kIOriginalRiskCnt As Integer = 38
    Public Const kIEditable As Integer = 41
	
	' PW311002 - add constants for listview positions
	Public Const ACColPosIsSelected As Integer = 0
	Public Const ACColPosEditted As Integer = 1
	Public Const ACColPosRiskNo As Integer = 2
	Public Const ACColPosCNAttached As Integer = 3
    Public Const ACColPosRiskDesc As Integer = 4
	Public Const ACColPosDiscounted As Integer = 5
	Public Const ACColPosVariationNo As Integer = 6
	Public Const ACColPosRiskStatus As Integer = 7
	Public Const ACColPosCoverage As Integer = 8
	Public Const ACColPosInsuredItem As Integer = 9
	Public Const ACColPosExtensions As Integer = 10
	Public Const ACColPosSumInsured As Integer = 11
	Public Const ACColPosFeeTax As Integer = 12
	Public Const ACColPosFeePremium As Integer = 13
	Public Const ACColPosTax As Integer = 14
	Public Const ACColPosPremium As Integer = 15
	Public Const ACColPosStartDate As Integer = 16
	Public Const ACColPosEndDate As Integer = 17
	Public Const ACColPosRiskTypeDesc As Integer = 18
	Public Const ACColPosGISScreen As Integer = 19
	Public Const ACColPosRiskTypeID As Integer = 20
    Public Const ACColPosStampInsurer As Integer = 21
    Public Const ACColPosStampInsured As Integer = 22
    Public Const kColPosRiskLinkStatus As Integer = 23
    Public Const kColPosRiskLingDate As Integer = 24
    Public Const kColPosRiskFolderKey As Integer = 25
	Public Const PMKeyNameVehicleRegNo As String = "vehicle_reg"
	
	' PW221102
	' PS411
	Public Const ACOptWhenTaxesRequired As Integer = 1007
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	'Constants for Date and Date Sort Column
	Public Const ACDateColumn As Integer = 4
	Public Const ACDateSortColumn As Integer = 6
	
	' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    Public g_iSourceID As Integer
    'Developer Guide No. 107
    Public g_iLanguageID As Integer
    'eck220500
    'Developer Guide No. 107

    Public g_iUserID As Integer
    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
    'Public g_oBusiness As bSIRFindInsurance.Form
    'eck190500
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
    'Public do we have a link to Gemini
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bPMGeminiLink As Boolean
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_bPMSwiftLink As Boolean
	
    Public Const ACDataPosCNRiskLinkId As Integer = 0
	Public Const ACDataPosCNRiskId As Integer = 1
	Public Const ACDataPosCNRef As Integer = 2
	Public Const ACDataPosCNFrom As Integer = 3
	Public Const ACDataPosCNTo As Integer = 4
	Public Const ACDataPosRowTag As Integer = 5
	Public Const ACDataPosCNAttach As Integer = 6
	Public Const ACDataCNCount As Integer = 6

End Module