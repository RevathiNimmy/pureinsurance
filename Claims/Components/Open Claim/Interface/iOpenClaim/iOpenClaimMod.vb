Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 16-JUN-00
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:Sravan
	'              Sammeer
	'              Pandu
	' JMK(28/02/2001): Add Claim Payment mode, allows limited editing. Assigned to g_nPMMode
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "Open Claim"
	
	Public Const g_nCLAIM_STATUS As Integer = 0
	Public Const g_nCLAIM_STATUS_DATE As Integer = 1
	Public Const g_nCLAIM_STATUS_TIME As Integer = 2
	Public Const g_nDESCRIPTION As Integer = 3
	Public Const g_nLOCATION As Integer = 4
	Public Const g_nLOSS_DATE As Integer = 5
	Public Const g_nLOSS_TIME As Integer = 6
	Public Const g_nREPORTED_DATE As Integer = 7
	Public Const g_nREPORTED_TIME As Integer = 8
	Public Const g_nLOSS_TO_DATE As Integer = 9
	Public Const g_nREPORTED_TO_DATE As Integer = 10
	Public Const g_nREPORTED_TO_TIME As Integer = 11
	Public Const g_nLAST_MODIFIED_DATE As Integer = 12
	Public Const g_nLAST_MODIFIED_TIME As Integer = 13
	Public Const g_nRISK_TYPE As Integer = 14
	Public Const g_nCLIENT_NAME As Integer = 15
	Public Const g_nCLIENT_ADDRESS As Integer = 16
	Public Const g_nCLIENT_TELNO As Integer = 17
	'Added By -Pandu
	Public Const g_nCLIENT_TELNOOFF As Integer = 18
	Public Const g_nCLIENT_FAXNO As Integer = 19
	Public Const g_nCLIENT_MOBILENO As Integer = 20
	Public Const g_nCLIENT_EMAIL As Integer = 21
	Public Const g_nCLIENT_VAT_REGNO As Integer = 22
	Public Const g_nCLIENT_CLAIMNO As Integer = 23
	Public Const g_nINSURER_NAME As Integer = 24
	Public Const g_nINSURER_ADDRESS As Integer = 25
	Public Const g_nINSURER_TELNO As Integer = 26
	Public Const g_nINSURER_FAXNO As Integer = 27
	Public Const g_nINSURER_CONTACT As Integer = 28
	Public Const g_nINSURER_EMAIL As Integer = 29
	Public Const g_nINSURER_CLAIMNO As Integer = 30
	'S4B Claim Enhancements R&D 2005
	Public Const g_nDRIVER_FORENAME As Integer = 31
	Public Const g_nDRIVER_SURNAME As Integer = 32
	Public Const g_nDRIVER_PASSEDTEST As Integer = 33
	Public Const g_nEMPLOYEE_FORENAME As Integer = 34
	Public Const g_nEMPLOYEE_SURNAME As Integer = 35
	Public Const g_nEMPLOYEE_LENGTHSERVICE As Integer = 36
	Public Const g_nEMPLOYEE_PREVIOUSCLAIMDETAILS As Integer = 37
	Public Const g_nRECOVERY_AGENT As Integer = 38
	Public Const g_nSOLICITOR_NAME As Integer = 39
	Public Const g_nULR_LOSS_DETAILS As Integer = 40
	Public Const g_nNONSTANDARD_EXCESS As Integer = 41
	Public Const g_nSUBSIDIARY_COMPANY As Integer = 42
	Public Const g_nPOLICY_NUMBER As Integer = 43
	Public Const g_nACCOUNT_HANDLER As Integer = 44
	Public Const g_nACCOUNT_EXEC As Integer = 45
	
	
	
	'Constant for CountryId for GBR - United Kingdom
	Public Const ACCountryGBR As Integer = 1
	
	'Constants For Check Boxes
	
	Public Const YesNoCheckNo As CheckState = CheckState.Unchecked
	Public Const YesNoCheckNone As CheckState = CheckState.Unchecked
	Public Const YesNoCheckyes As CheckState = CheckState.Checked
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	
	'S4B Claim Enhancements R&D 2005
	Public Const ACTabTitle1U As Integer = 101
	Public Const ACTabTitle2U As Integer = 0
	Public Const ACTabTitle3U As Integer = 102
	Public Const ACTabTitle4U As Integer = 103
	Public Const ACTabTitle5U As Integer = 104
	
	Public Const ACTabTitle1B As Integer = 101
	Public Const ACTabTitle2B As Integer = 115
	Public Const ACTabTitle3B As Integer = 116
	Public Const ACTabTitle4B As Integer = 117
	Public Const ACTabTitle5B As Integer = 118
	
	Public Const ACPolicyHolder As Integer = 105 '
	
	Public Const ACListTitle1 As Integer = 110 '
	Public Const ACListTitle2 As Integer = 111 '
	Public Const ACListTitle3 As Integer = 112 '
	Public Const ACListTitle4 As Integer = 113 '
	Public Const ACListTitle5 As Integer = 114 '
	
	'*************************************
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	Public Const ACFindNowButton As Integer = 204
	Public Const ACNewSearchButton As Integer = 205
	
	Public Const ACRiskTypeButton As Integer = 206
	Public Const ACAddressButton As Integer = 207
	'DC241001
	Public Const ACInsDetButton As Integer = 208
	
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
	'**********************************
	' Menus
	
	Public Const ACLossDateBefore As Integer = 330
	Public Const ACLossDateAfter As Integer = 331
	Public Const ACPolicyVoided As Integer = 332
	Public Const ACNoPolicyFound As Integer = 339 ' Readded in resource file
	Public Const ACDifferentPolicy As Integer = 334
	
	'Changes by Sameer on 15-07-00 . Resource File ( Label fields )
	'***********
	Public Const ACClmStatus As Integer = 105 'Label for the Claim Status Label
	Public Const ACClmStatusDate As Integer = 110 'Label for the Claim Status date Label
	Public Const ACLastModDate As Integer = 111 'Label for the Claim Status date Label
	Public Const ACYes As Integer = 112 'Label for the Yes chk box
	Public Const ACNo As Integer = 113 'Label for the No chk box
	
	
	Public Const ACHandler As Integer = 120 'Label for the Handler Label
	Public Const ACProgressStatus As Integer = 121 'Label for the Progress Status  Label
	Public Const ACDescription As Integer = 122 'Label for the Description  Label
	Public Const ACPrimaryCause As Integer = 123 'Label for the Primary Cause  Label
	Public Const ACSecondaryCause As Integer = 124 'Label for the Secondary Cause Label
	Public Const ACCatastrophecode As Integer = 125 'Label for the Catastrophe code Label
	Public Const ACLocation As Integer = 126 'Label for the Location Label
	Public Const ACTown As Integer = 127 'Label for the Town Label
	Public Const ACReportedDate As Integer = 128 'Label for the Reported Date Label
	Public Const ACLossDate As Integer = 129 'Label for the Loss Date Label
	Public Const ACLossToDate As Integer = 130 'Label for the Loss To Date Label
	Public Const ACReportedToDate As Integer = 131 'Label for the Reported To Date Label
	Public Const ACCurrency As Integer = 132 'Label for the Currency Label
	Public Const ACInformation As Integer = 133 'Label for the Information Label
	Public Const ACLikelyToClaim As Integer = 134 'Label for the Likely To Claim Label
	
	Public Const ACClientName As Integer = 114 'Label for the Client Name Label
	Public Const ACTelephoneNumberHome As Integer = 135 'Label for the Telephone Number Home Label
	Public Const ACTelephoneNumberOffice As Integer = 136 'Label for the Telephone Number Office Label
	Public Const ACClientFaxNumber As Integer = 137 'Label for the Client Fax Number Label
	Public Const ACMobileNumber As Integer = 138 'Label for the Client Mobile Number Label
	Public Const ACClientEmailNumber As Integer = 139 'Label for the Email Number Label
	Public Const ACVATRegistered As Integer = 140 'Label for the VAT Registered Label
	Public Const ACVATRegistrationNumber As Integer = 141 'Label for the VAT Registration Number Label
	Public Const ACClientClaimNumber As Integer = 142 'Label for the Client Claim Number Label
	
	Public Const ACInsurerName As Integer = 148 'Label for the Insurer Name
	'DC231100 - new caption for Underwriting
	Public Const ACAgentName As Integer = 154 ' Label for the Agent Name
	Public Const ACInsurerTelephoneNumber As Integer = 143 'Label for the Insurer Telephone Number Label
	Public Const ACInsurerFaxNumber As Integer = 144 'Label for the Fax Number Label
	Public Const ACContact As Integer = 145 'Label for the Contact Label
	Public Const ACInsurerEmailNumber As Integer = 146 'Label for the Insurer Email Number  Label
	Public Const ACInsurerClaimNumber As Integer = 147 'Label for the Insurer Claim Number Label
	'DC231100 - new caption for Underwriting
	Public Const ACAgentClaimNumber As Integer = 155 'Label for the Agent Claim Number
	
	Public Const ACUserDefnFldA As Integer = 149 'Label for the User defined field A
	Public Const ACUserDefnFldB As Integer = 150 'Label for the User defined field B
	Public Const ACUserDefnFldC As Integer = 151 'Label for the User defined field C
	Public Const ACUserDefnFldD As Integer = 152 'Label for the User defined field D
	Public Const ACUserDefnFldE As Integer = 153 'Label for the User defined field E
	Public Const ACClaimHandled As Integer = 332 'Label for the Claim Handled Checkbox
	
	'S4B Claim Enhancements R&D 2005
	Public Const AC_RES_TITLE As Integer = 156
	Public Const AC_RES_FORENAME As Integer = 157
	Public Const AC_RES_LASTNAME As Integer = 158
	Public Const AC_RES_DATE_PASSED_TEST As Integer = 159
	Public Const AC_RES_EMPLOYEE_DETAILS As Integer = 160
	Public Const AC_RES_LENGTH_OF_SERVICE As Integer = 161
	Public Const AC_RES_PREVIOUS_CLAIM As Integer = 162
	Public Const AC_RES_DETAILS As Integer = 163
	Public Const AC_RES_UNINSURED_LOSS_RECOVERY_DETAILS As Integer = 164
	Public Const AC_RES_UNINSURED_LOSS_RECOVERY As Integer = 165
	Public Const AC_RES_RECOVERY_AGENT As Integer = 166
	Public Const AC_RES_SOLICITOR_APPOINTED As Integer = 167
	Public Const AC_RES_SOLICITOR_NAME As Integer = 168
	Public Const AC_RES_NCD_DETAILS As Integer = 169
	Public Const AC_RES_AT_FAULT As Integer = 170
	Public Const AC_RES_BONUS_AFFECTED As Integer = 171
	Public Const AC_RES_DEDUCTIBLES As Integer = 172
	Public Const AC_RES_STANDARD_EXCESS As Integer = 173
	Public Const AC_RES_NON_STANDARD_EXCESS As Integer = 174
	Public Const AC_RES_INSURED_DRIVER_DETAILS As Integer = 175
	
	'Error Messages
	Public Const ACMandatoryFieldMsg As Integer = 310
	Public Const ACInvalidDateMsg As Integer = 311
	Public Const ACCannotBeGreaterThanLossDateMsg As Integer = 312
	Public Const ACInvalidTimeMsg As Integer = 313
	Public Const ACInvalidNumberMsg As Integer = 314
	Public Const ACEmptyVatMsg As Integer = 315
	Public Const ACCannotBeGreaterThanTodaysDateMsg As Integer = 316
	Public Const ACCannotBeGreaterThanPolicyDate As Integer = 317
	Public Const ACCannotBeLessThanPolicyDate As Integer = 318
	Public Const ACInvaildTimeMsg As Integer = 319
	Public Const ACOpenaClaim As Integer = 320
	Public Const ACCannotBeGreaterThanReportedDateMsg As Integer = 321
	
	'JMK 31/05/2001
	Public Const ACLossDateLaterThanReportedDate As Integer = 325
	Public Const ACLossDateLaterThanLossToDate As Integer = 326
	Public Const ACLossDateLaterThanCurrentDate As Integer = 327
	Public Const ACReportedDateLaterThanCurrentDate As Integer = 328
	Public Const ACReportedDateLaterThanReportedToDate As Integer = 329
	
	'Close Claim message constants
	Public Const ACCloseClaimTitle As Integer = 323
	Public Const ACCloseClaimMessage As Integer = 324
	
	'S4B Claim Enhancements R&D 2005
	Public Const AC_RES_OS_RESERVES1 As Integer = 330
	Public Const AC_RES_OS_RESERVES2 As Integer = 331
	
	Public Const AC_RES_TAX_REGISTERED As Integer = 333
	Public Const AC_RES_TAX_REGISTRATION_NUMBER As Integer = 334
	
	Public Const AC_Res_MsgRetroactiveDate As Integer = 335
	Public Const AC_Res_MsgLossDate1 As Integer = 336
	Public Const AC_Res_MsgReportedDate As Integer = 337
	Public Const AC_Res_MsgLossDate2 As Integer = 338
	
	'MSS250901 - Added For merge
	'AK 18042001
	Public Const ACGeminiIIMotor As String = "GEMINI IIM"
	Public Const ACGeminiIIHouseHold As String = "GEMINI IIH"
	Public Const ACCommercialVehicle As String = "CV"
	Public Const ACGIIMPrimaryCause As String = "022"
	Public Const ACGIIHPrimaryCause As String = "410"
	Public Const ACCVPrimaryCause As String = "022"
	'S4B Claim Enhancements R&D 2005
	Public Const AC_POLICYTYPE_GENERAL As String = "GENERAL"
	'MSS250901 - Merge end
	
	'Date Conversion Constants
	Public Const ACDateConversion As String = "short date"
    Public Const ACTimeConversion As String = "HH:mm"
	
	'Constants defined for the Claim Status field
	Public Const CLMProvisionalOpenClaim As Integer = 1
	Public Const CLMLiveOpenClaim As Integer = 2
	Public Const CLMClosed As Integer = 3
	Public Const CLMReOpened As Integer = 4
	Public Const CLMReClosed As Integer = 5
	
	'Constants defined for the Claim Status Description field
	Public Const g_sPROVISIONALOPENCLAIM As String = "Provisional Open Claim"
	Public Const g_sLIVEOPENCLAIM As String = "Live Open Claim"
	Public Const g_sCLOSED As String = "Closed"
	Public Const g_REOPENED As String = "ReOpened"
	Public Const g_RECLOSED As String = "ReClosed"
	
	
	
	'*****
	'End of the changes by Sameer on 15-07-00 . Resource File ( Label fields )
	'*************************************************************************
	
	' Constants for the search data array indexes.
	'CLient Details Constant -Pandu
	
	Public Const ACCClientName As Integer = 0
	Public Const ACCClientShortName As Integer = 1
	Public Const ACCAddress1 As Integer = 2
	Public Const ACCAddress2 As Integer = 3
	Public Const ACCAddress3 As Integer = 4
	Public Const ACCAddress4 As Integer = 5
	Public Const ACCPostCode As Integer = 6
	Public Const ACCTeleHome As Integer = 7
	Public Const ACCTeleOff As Integer = 8
	Public Const ACCFax As Integer = 9
	Public Const ACCMobile As Integer = 10
	Public Const ACCEmail As Integer = 11
	Public Const ACCPartyCnt As Integer = 12
	
	'JMK 01/06/2001
	Public Const ACCCountryId As Integer = 13
	Public Const ACICountryId As Integer = 13
	
	'AR20050303 - PN15644
	Public Const ACCAddressId As Integer = 14
	
	'Insurer Details Constants -Pandu
	Public Const ACIInsurerName As Integer = 0
	Public Const ACIInsurerShortName As Integer = 1
	Public Const ACIAddress1 As Integer = 2
	Public Const ACIAddress2 As Integer = 3
	Public Const ACIAddress3 As Integer = 4
	Public Const ACIAddress4 As Integer = 5
	Public Const ACIPostCode As Integer = 6
	Public Const ACITeleHome As Integer = 7
	Public Const ACIFax As Integer = 8
	Public Const ACIEmail As Integer = 9
	
	'AR20050303 - PN15644
	Public Const ACIAddressId As Integer = 11
    'PN48079
    Public Const ACIInsurerContact As Long = 12
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
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer

    'developer guide no. 107
    <ThreadStatic()> _
 Public g_nPMMode As Integer
	

	Public g_nREADMODE As Integer = gPMConstants.PMEComponentAction.PMView '0

	Public g_nADDMODE As Integer = gPMConstants.PMEComponentAction.PMAdd '1

	Public g_nEDITMODE As Integer = gPMConstants.PMEComponentAction.PMEdit '2
	' JMK(28/02/2001)
	Public Const g_nPAYMODE As Integer = 3
	'DC020403 ISS3153
	Public Const g_nVIEWMODE As Integer = 4
	'DC081203 PN8955
	Public Const g_nEDITADDMODE As Integer = 5
	
	'RWH(10/11/2000) claim numbering.
	Public Const ACInsFileSourceId As Integer = 5
	Public Const ACInsFileProductId As Integer = 8
	Public Const ACInsFileLeadAgentCnt As Integer = 10
	Public Const ACInsFileBusTypeId As Integer = 14
	Public Const ACInsFileBranchId As Integer = 17
	Public Const ACInsFileCoverStartDate As Integer = 21
	Public Const ACInsFileExpiryDate As Integer = 22

    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lClientAdressCnt As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lInsurerAdressCnt As Integer
    'AR20050303 - PN15664
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lClient_AddressId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lClient_AddressUsage As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lInsurer_AddressId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lInsurer_AddressUsage As Integer
    'AR20050404 - PN15664
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bClientAddressChanged As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bInsurerAddressChanged As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lClaimID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClaimNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPolicyNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lPolicyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sDescription As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lClaimStatusID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lProgressStatusID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lPrimaryCauseID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lSecondaryCauseID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lCatastropheCodeID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sLossFromDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sLossToDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sReportedDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sReportedToDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sLastModifiedDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lHandlerID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_nInfoOnly As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_nLikelyClaim As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sLocation As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lTown As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lRiskTypeID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientName As String = ""
    'Changed to long -Pandu
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientAddress As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientTelNo As String = ""
    'Added Client Office No.-Pandu
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientTelNoOff As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientFaxNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientMobileNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientEMail As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientClaimNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerName As String = ""
    'Changed to long -Pandu
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerAddress As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerTelNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerFaxNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerEmail As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerClaimNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerContact As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_nVATRegistered As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sVATRegisteredNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sComments As String = ""
    'DC240402
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vClaimComments As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lOrPolicyID As Integer
	
	'Added New Varaiables - Pandu
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClaimsStatusDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientShortName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerShortName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lUserDefFldA As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_lUserDefFldB As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lUserDefFldC As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lUserDefFldD As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lUserDefFldE As Integer
	
    ' Tomo190402 - start
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sProvisionalClaimNo As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sFullClaimNo As String = ""
	' Tomo190402 - end
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vClaimHandled As Object
	
	'Navigator Variables
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vClaimDate As String = ""
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtLossDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtLossTime As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtLossToDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtLossToTime As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtReportedDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtReportedTime As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtReportedToDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_dtReportedToTime As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtLastModifiedDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtLastModifiedTime As String = ""
	
    'Added Claim Status Date -Pandu
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtClaimStatusDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtPolicyFromDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dtPolicyToDate As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_nCurrencyId As Integer
	
	'Added Variables for Storing AddressDetails of Client -Pandu
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientAddress1 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientAddress2 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientAddress3 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientAddress4 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientPostCode As String = ""
	
    'JMK 01/06/2001
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lClientCountryId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lInsurerCountryId As Integer
    'TB 130103
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sClientCountryName As String = ""
	
	'Added Variables for Storing AddressDetails of Insurer -Pandu
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerAddress1 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerAddress2 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerAddress3 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerAddress4 As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sInsurerPostCode As String = ""
    'PN48079
    Public g_sInsurerContactName As String

    'Events
	Public Const PMBEventNewClaim As Integer = 3
	Public Const PMBEventClaChange As Integer = 6
	Public Const PMBEventDelClaim As Integer = 9
	
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.

    'developer guide no.(As per VB Code)
    'Public g_oBusiness As bOpenClaim.Business
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object

	
    'AK 180401
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPolicyType As String = ""
	
    'DC140904 PN14948 allow merge fields to work
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lPartyCnt As Integer
    'DD 29/03/2004/
    'developer guide no.101
    'Public g_vUnderwritingYearID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_vUnderwritingYearID As Object
	
    '2005 Client Manager Security
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oUserAuthorities As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bEditClaimAuthority As Boolean
	
	' Indicates if the rest of the roadmap should operate in
    ' 'quick close claim' mode, S4I ONLY
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bBalanceAndCloseClaim As Boolean
	
    'S4B Claims Enhancements R&D 2005
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sDriverTitle As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sDriverForename As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sDriverSurname As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_vDatePassedTest As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sEmployeeTitle As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sEmployeeForename As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sEmployeeSurname As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lEmployeeLengthOfService As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bEmployeePreviousClaim As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sEmployeePreviousClaimDetails As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bULR As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sRecoveryAgent As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bSolicitorAppointed As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sSolicitorName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sULRLossDetails As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lClaimAtFaultId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bBonusAffected As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lPolicyDeductibleId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_dNonStandardExcess As Double
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sSubsidiaryCompanyName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sAccountHandler As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sAccountExec As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lVersionId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCaseNumber As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lCaseID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bDisplayClaimReinsurance As Boolean
    <ThreadStatic()> _
  Public g_iUserOtherPartyID As Object
    Public g_sUserOtherPartyname As String
	'***************************************************************************
	' AK 030801
	' Renewal Status Type Codes
	'***************************************************************************
	Public Const PMRenewalStatusTypePreSelection As String = "PRERENSEL"
	Public Const PMRenewalStatusTypeRenewalSelected As String = "RENSEL"
	Public Const PMRenewalStatusTypeRenewalQuoted As String = "RENQUOTED"
	Public Const PMRenewalStatusTypeRenewalInvited As String = "INVITED"
	Public Const PMRenewalStatusTypeRenewalPending As String = "CONFPEND"
	Public Const PMRenewalStatusTypeLapsePending As String = "LAPSEPEND"
	Public Const PMRenewalStatusTypeRenewalConfirmed As String = "RENEWCONF"
	Public Const PMRenewalStatusTypePolicyLapseConfirmed As String = "LAPSECONF"
	Public Const PMRenewalStatusTypePolicyLapsed As String = "LAPSED"
	Public Const PMRenewalStatusTypePolicyRenewed As String = "RENEWED"
	Public Const PMRenewalStatusTypePolicyLapse As String = "LAPSE"
	
	'RWH(10/11/2000) claim numbering.
	Public Const ACProvisionalClaim As Integer = 3
	Public Const ACFullClaim As Integer = 4
	
	Public Const kDupClaimsColClaimNumber As Integer = 1
	Public Const kDupClaimsColDescription As Integer = 2
	Public Const kDupClaimsColProgressStatus As Integer = 3
	Public Const kDupClaimsColPrimaryCause As Integer = 4
	Public Const kDupClaimsColReportedDate As Integer = 5
	Public Const kDupClaimsColLastModified As Integer = 6
	Public Const kDupClaimsColStatus As Integer = 7
	
	Public Const kDupClaimId As Integer = 0
	Public Const kDupClaimNumber As Integer = 1
	Public Const kDupClaimDescription As Integer = 2
	Public Const kDupClaimProgressStatus As Integer = 3
	Public Const kDupClaimReportedDate As Integer = 4
	Public Const kDupClaimModifiedDate As Integer = 5
	Public Const kDupClaimPrimaryCause As Integer = 6
	Public Const kDupClaimStatusId As Integer = 7
	
	Public Const kSystemOptionDuplicateClaimCheck As Integer = 5002
	'Start (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)
    Public Const kSystemOptionRestrictNonZeroClosure As Integer = 5073
	Public Const kSystemOptionEnableLossDateonClaim As Integer = 5176
	'End (Girija chokkalingam) - (Tech Spec - LOA001 - Balance Close Claim.doc) - (5.2.1)

	'S4B Claim Enhancements R&D 2005
	Public Const kSYSOPT_IN_RENEWAL_CYCLE_THRESHOLD As Integer = 2021
	
	Public Const kEventTypeNewClaim As Integer = 3
	
	Public Const kDetailKey As Integer = 0
	Public Const kDetailCode As Integer = 1
	Public Const kDetailDesc As Integer = 2
	
	' claim transaction suppression indicators
	Public Const kClaimTransSuppProdReserve As Integer = 0
	Public Const kClaimTransSuppProdPayment As Integer = 1
	Public Const kClaimTransSuppProdRecovery As Integer = 2
	Public Const kClaimTransSuppClaimReserve As Integer = 3
	Public Const kClaimTransSuppClaimPayment As Integer = 4
	Public Const kClaimTransSuppClaimRecovery As Integer = 5
	
	
	Sub Main_Renamed()
		
		
        Dim o As New Interface_Renamed
		Dim lReturn As gPMConstants.PMEReturnCode = CType(CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
		lReturn = CType(o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)
		lReturn = o.Start()
		o.Dispose()
		
    End Sub
    
End Module
