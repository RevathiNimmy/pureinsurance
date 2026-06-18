Option Strict Off
Option Explicit On
Imports System
Public Module GIIConstants
	'******************************************************************************
	' GEMINI II SPECIFIC CONSTANT DEFINITIONS
	'******************************************************************************
	
	' IDP Jan 2003 GII Merged 1.6 / 1.8 Version
	
	' 24/3/00 - Tom added home linkage map constants
	
	'*****************************************************************************
	' New Polaris Data Types
	'*****************************************************************************
	Public Const GEMPolUnknown As Integer = 0
	Public Const GEMPolDate As Integer = 1
	Public Const GEMPolNumeric As Integer = 2
	Public Const GEMPolShortList As Integer = 3
	Public Const GEMPolLongList As Integer = 4
	Public Const GEMPolText As Integer = 5
	Public Const GEMPolNumeric2 As Integer = 6
	Public Const GEMPolRef As Integer = 9
	
	'****   Following Constants replaced with those in PMNavKeyConst Module   ****'
	''*****************************************************************************
	'' Navigator Key Name Constants
	''*****************************************************************************
	'Global Const PMKeyNameClientKey = "client_key"
	'Global Const PMKeyNameClientName = "client_name"
	'Global Const PMKeyNamePolicyKey = "policy_key"
	'Global Const PMKeyNamePolicyNo = "policy_no"
	'Global Const PMKeyNameListManager = "ListManager"
	'Global Const PMKeyNameScreenId = "SCREEN_ID"
	'Global Const PMKeyNameScreenDocument = "SCREEN_DOCUMENT"
	'Global Const PMKeyNameSchemeProperties = "PROFILE"
	'Global Const PMKeyNameScreenType = "SCREEN_TYPE"
	'Global Const PMKeyNameGis = "GIS"
	'Global Const PMKeyNameBusinessTypeId = "business_type_id"
	'Global Const PMKeyNameBusinessTypeCode = "business_type_code"
	'Global Const PMKeyNameDefaultRequired = "default_required"
	'Global Const PMKeyNameDefaultPolicyId = "default_policy_id"
	'Global Const PMKeyNamePartyCnt = "party_cnt"
	'Global Const PMKeyNameShortName = "shortname"
	'Global Const PMKeyNameInsuranceFolderCnt = "insurance_folder_cnt"
	'Global Const PMKeyNameInsuranceFileCnt = "insurance_file_cnt"
	'Global Const PMKeyNameDiaryActionCodeId = "diary_action_code_id"
	'Global Const PMKeyNameActionNo = "action_no"
	'Global Const PMKeyNameGISSchemeId = "scheme_id"
	'Global Const PMKeyNameTransactionType = "transaction_type"
	'Global Const PMKeyNameDescription = "description"
	'Global Const PMKeyNameTransactionTypeCode = "transaction_type_code"
	'Global Const PMKeyNameChooseStepPolicyStatus = "choose_step_policy_status"
	'Global Const PMKeyNameChooseStepKeyName = "choose_step_key_name"
	'Global Const PMKeyNameChooseStepKeyValue = "choose_step_key_value"
	
	'*****************************************************************************
	' Constants for FillCombo function in GeneralScreens
	'*****************************************************************************
	Public Const GEMRefill As Boolean = True
	Public Const GEMNoRefill As Boolean = False
	
	'*****************************************************************************
	' Gemini Date Formats
	'*****************************************************************************
	Public Const GEMShortDate As String = "DD/MM/YYYY"
	Public Const GEMLongDate As String = "D MMMM YYYY"
	Public Const GEMISODate As String = "YYYY-MM-DD HH:MM:SS" 'Added for 1.8 Release, GIS required format
	Public Const GEMMediumDate As String = "DD-MMM-YYYY" 'Needed for some grids due to space problems
	
	'*****************************************************************************
	' Gemini Time Format
	'*****************************************************************************
	Public Const GEMTime As String = "HH:MM"
	Public Const GEMLongTime As String = "HH:MM:SS"
	
	'*****************************************************************************
	' Constants for policy status
	'*****************************************************************************
	Public Const GEMPolicyIncomplete As Integer = 1
	Public Const GEMPolicyIncompleteSpecific As Integer = 4 ' start road map at select scheme
	Public Const GEMPolicyIncompleteQuote As Integer = 7 ' start road map at quote display
	Public Const GEMPolicyQuote As Integer = 10
	Public Const GEMPolicyRequoteRequired As Integer = 12
	Public Const GEMPolicyRequoted As Integer = 15
	Public Const GEMPolicyPending As Integer = 30
	Public Const GEMPolicyLive As Integer = 40
	Public Const GEMPolicyMTAPermanent As Integer = 1010
	Public Const GEMPolicyMTATemporary As Integer = 1020
	Public Const GEMPolicyMTAIncomplete As Integer = 1030
	Public Const GEMPolicyMTACancellation As Integer = 1040
	Public Const GEMPolicyMTAReinstatement As Integer = 1050
	Public Const GEMPolicyCancelPending As Integer = 48
	Public Const GEMPolicyCancelled As Integer = 50
	Public Const GEMPolicyExpired As Integer = 55
	Public Const GEMPolicyLapsed As Integer = 60
	
	'*****************************************************************************
	'Transaction Codes for Policy_life_cycle table
	'*****************************************************************************
	Public Const GEMPolLifeNB As Integer = 1
	Public Const GEMPolLifeMTA As Integer = 2
	Public Const GEMPolLifeInvite As Integer = 3
	Public Const GEMPolLifeConfirm As Integer = 4
	Public Const GEMPolLifeComplete As Integer = 5
	Public Const GEMPolLifeLapse As Integer = 6
	
	'*****************************************************************************
	' Screen Type Constants
	'*****************************************************************************
	Public Const GIIScreenTypeInsurerSpecific As Integer = 2
	Public Const GIIScreenTypeGeneric As Integer = 1
	
	'*****************************************************************************
	' Process Modes
	' To placed in GPMLibraries at a later date.
	'*****************************************************************************
	
	'PMProcessModeGeneric is a duplicate - but leave be as if you use it it will
	'   stop compilation. Switch GII code to use GIIPMProcessModeGeneric
	'Global Const PMProcessModeGeneric = 110
	Public Const GIIPMProcessModeGeneric As Integer = 110
	
	'*****************************************************************************
	'Transaction Types
	'*****************************************************************************
	
	Public Const PMTransactionTypeReview As String = "G_REVIEW"
	Public Const PMTransactionTypeDefaults As String = "G_DEFAULTS"
	Public Const PMTransactionTypeMTAFullQuote As String = "G_MTA_FQ"
	
	'*****************************************************************************
	'Run_mode types
	'
	' JSB 2/1/1 - New key for roadmaps, currently used to identify Maintain motor,
	'             could be made use of later for other things
	'*****************************************************************************
	Public Const PMRunModeTypeMMD As String = "MMD"
	
	'*****************************************************************************
	'Business Types
	'*****************************************************************************
	Public Const PMGISBusinessTypeMotor As String = "GIIM"
	Public Const PMGISBusinessTypeHousehold As String = "GIIH"
	Public Const PMGISBusinessTypeTruck As String = "GIIT"
	
	Public Const PMLegacyBusinessTypeMotor As String = "PC"
	Public Const PMLegacyBusinessTypeHousehold As String = "PH"
	Public Const PMLegacyBusinessTypeTruck As String = "CV"
	
	' Would have used PMLegacy above, but household is "PH" for VBS and "HH" as an abi code
	Public Const PMClassOfBusinessMotor As String = "PC"
	Public Const PMClassOfBusinessHome As String = "HH"
	Public Const PMClassOfBusinessTruck As String = "CV"
	
	' TB 18/6/01: NI Rates
	Public Const PMLegacyCountryCoyUK As Integer = 1
	Public Const PMLegacyCountryCoyNI As Integer = 2
	Public Const PMCountryCodeUK As String = "GBR"
	Public Const PMCountryCodeNI As String = "NI"
	
	'JSB - I know declaring these here is wrong, but doubt if the following will change plus only going
	'to be used in one place(iPMBClientManager) so not going to write a a new business component for this
	Public Const PMGISBusinessTypeMotorID As String = "1"
	Public Const PMGISBusinessTypeHouseholdID As String = "2"
	Public Const PMGISBusinessTypeTruckID As String = "3"
	
	'*****************************************************************************
	'Legacy Type
	'*****************************************************************************
	Public Const PMLegacyTypeVbs As String = "V"
	Public Const PMLegacyTypeForms As String = "F"
	
	'*****************************************************************************
	'Data Model Codes
	'*****************************************************************************
	Public Const PMGISDataModelCodeMotor As String = "GIIMotor"
	Public Const PMGISDataModelCodeHousehold As String = "GIIHouse"
	Public Const PMGISDataModelCodeTruck As String = "GIITruck"
	
	Public Const GIIRebrokeDataModelCodeMotor As String = "GIIMrebroke"
	Public Const GIIRebrokeDataModelCodeHousehold As String = "GIIHrebroke"
	Public Const GIIRebrokeDataModelCodeTruck As String = "GIITrebroke"
	
	'*****************************************************************************
	' Declaration of API Functions.
	'*****************************************************************************
	Public Declare Function SetWindowLong Lib "user32"  Alias "SetWindowLongA"(ByVal hwnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
	Public Declare Function GetWindowLong Lib "user32"  Alias "GetWindowLongA"(ByVal hwnd As Integer, ByVal nIndex As Integer) As Integer
	Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Integer) As Integer
	Declare Function SetFocusAPI Lib "user32"  Alias "SetFocus"(ByVal hwnd As Integer) As Integer
	Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
	Public Declare Function FindWindow Lib "user32"  Alias "FindWindowA"(ByVal lpClassName As Integer, ByVal lpWindowName As String) As Integer
	Public Declare Function GetSystemMenu Lib "user32" (ByVal hwnd As Integer, ByVal bRevert As Integer) As Integer
	Public Declare Function DeleteMenu Lib "user32" (ByVal hMenu As Integer, ByVal nPosition As Integer, ByVal wFlags As Integer) As Integer
	
	Public Const WM_ACTIVATE As Integer = &H6s
	Public Const WS_EX_APPWINDOW As Integer = &H40000
	Public Const WS_EX_TOOLWINDOW As Integer = &H80
	Public Const GWL_EXSTYLE As Integer = (-20)
	
	'JSB - used disable 'x' button
	Public Const SC_CLOSE As Integer = &HF060s
	Public Const MF_BYCOMMAND As Integer = &H0
	
	' Used to show or hide the drop-down list box part of a combo control.
	' Using the Send Message API . The following Parameters Values are used
	' wParam  TRUE (nonzero) to show the list box. Zero to hide it.
	' lParam  Not used—set to zero.
	
	Public Const CB_SHOWDROPDOWN As Integer = &H14Fs
	
	'*****************************************************************************
	'Life style constants
	'*****************************************************************************
	Public Const GIILifeStyleInsured As String = "INSURED"
	'*****************************************************************************
	'Contact Code Constants
	'*****************************************************************************
	Public Const GIIContactCodeEmail As String = "EMAIL"
	Public Const GIIContactCodeFax As String = "FAX"
	Public Const GIIContactCodeHomeTel As String = "HOME_TEL"
	Public Const GIIContactCodeWorkTel As String = "WORK_TEL"
	'*****************************************************************************
	'Diary Constants
	'*****************************************************************************
	Public Const GIIDiaryPMBClientPolicyLink As String = "CLIPOL"
	
	
	'*****************************************************************************
	'Constants for policy status
	'*****************************************************************************
	Public Const GIIPolicyIncomplete As Integer = 0
	Public Const GIIPolicyQuote As Integer = 1
	Public Const GIIPolicyNBComplete As Integer = 10
	Public Const GIIPolicyRequoteRequired As Integer = 12
	Public Const GIIPolicyRequoted As Integer = 15
	Public Const GIIPolicyPending As Integer = 20
	Public Const GIIPolicyPendingTransmitted As Integer = 30
	Public Const GIIPolicyLive As Integer = 40
	Public Const GIIPolicyMTAPermanent As Integer = 1010
	Public Const GIIPolicyMTATemporary As Integer = 1020
	Public Const GIIPolicyMTAIncomplete As Integer = 1030
	Public Const GIIPolicyMTACancellation As Integer = 1040
	Public Const GIIPolicyMTAReinstatement As Integer = 1050
	Public Const GIIPolicyCancelPending As Integer = 0
	Public Const GIIPolicyCancelled As Integer = 50
	Public Const GIIPolicyExpired As Integer = 0
	Public Const GIIPolicyLapsed As Integer = 0
	Public Const GIIPolicyCoverDefault As Integer = 0
	
	'*****************************************************************************
	'Constants for property manager
	'*****************************************************************************
	Public Const GIIPropGroupLinkage As String = "Linkage"
	Public Const GIIPropPropertyOutputLinkage As String = "Output_Linkage"
	
	'*****************************************************************************
	' Form Type Constants
	'*****************************************************************************
	Public Const ACFormNumberProposalForm As Integer = 0
	' TB 6/7/01 -  'Special code.  so cobol is reset for each print from quote display
	Public Const ACFormNumberPropFromQuoteDisplay As Integer = 3
	Public Const ACFormNumberProposalFormReprint As Integer = 5
	Public Const ACFormNumberCoverNotePrint As Integer = 10
	Public Const ACFormNumberDuplicateDocsPrint As Integer = 15
	Public Const ACFormNumberReTransmitDetails As Integer = 20
	Public Const ACFormNumberRenewalInvitation As Integer = 25
	Public Const ACFormNumberRenewalForms As Integer = 27
	'sj 12/07/2001 - start
	Public Const ACFormNumberCancellationLetter As Integer = 30
	Public Const ACFormNumberWarningOneLetter As Integer = 32
	Public Const ACFormNumberWarningTwoLetter As Integer = 34
	Public Const ACFormNumberTimeOnRiskLetter As Integer = 36
	'sj 12/07/2001 - end
	
	'*****************************************************************************
	' Choose Step Constants
	'*****************************************************************************
	Public Const GIIChooseStepPolicyStatus As String = "policy_status"
	Public Const GIIChooseStepSchemeType As String = "scheme_type"
	Public Const GIIChooseStepQuickQuoteInProgress As String = "quick_quote"
	
	'*****************************************************************************
	'Cover type Constants
	'*****************************************************************************
	Public Const DO_QUOTE_COVER_TYPE_COMP As String = "Comprehensive"
	Public Const DO_QUOTE_COVER_TYPE_TPFT As String = "Third Party, Fire And Theft"
	Public Const DO_QUOTE_COVER_TYPE_TP As String = "Third Party"
	
	' Legacy Business Types
	Public Const ACBusinessTypeCV As Integer = 1
	Public Const ACBusinessTypeMotor As Integer = 2
	Public Const ACBusinessTypeHome As Integer = 3
	
	' Cobol Linkage Table Item Type Codes
	' Pic X (length in column item_length)
	Public Const ACTypeCodeString As String = "X"
	
	' Pic 9 (length in column item_length)
	'       (item_decimal_places gives decimal places if any)
	Public Const ACTypeCodeNumeric As String = "9"
	
	' Pic S9 (length and decimals as above)
	Public Const ACTypeCodeSignedNumeric As String = "S"
	
	' Date (either dd, ddmm, ddmmyy or ddmmccyy depending on length)
	Public Const ACTypeCodeDate As String = "D"
	
	' Reverse Date (either dd, mmdd, yymmdd or ccyymmdd depending on length)
	Public Const ACTypeCodeReverseDate As String = "R"
	
	' Filler (contains spaces only)
	Public Const ACTypeCodeFiller As String = "F"
	
	' Comment row (Has no length used for debugging etc eg gives 01 level name)
	Public Const ACTypeCodeComment As String = "C"
	
	' Occurs Start (Has no length)
	Public Const ACTypeCodeOccursStart As String = "O"
	
	' Occurs End (Has no length)
	Public Const ACTypeCodeOccursEnd As String = "E"
	
	' Redefines Start (Has no length)
	Public Const ACTypeCodeRedefStart As String = "I"
	
	' Redefines End (Has no length)
	Public Const ACTypeCodeRedefEnd As String = "N"
	
	' Binary Numeric (Comp Field)
	Public Const ACTypeCodeBinaryNumeric As String = "B"
	
	' Signed Binary Numeric (Comp Field)
	Public Const ACTypeCodeSignedBinaryNumeric As String = "G"
	
	' Binary Reverse Date ( either yymmdd or ccyymmdd depending on length)
	Public Const ACTypeCodeBinaryReverseDate As String = "V"
	
	' Cobol Linkage Table Conversion Type Codes
	' Y/N field
	Public Const ACConversionYesNo As String = "Y"
	
	' Conversion Code List field uses bGIICodeConvert
	Public Const ACConversionCode As String = "C"
	
	' Conversion Lookup List field
	Public Const ACConversionLookup As String = "L"
	
	' Conversion Addon
	Public Const ACConversionAddOnValue As String = "V"
	Public Const ACConversionAddOnYesNo As String = "F"
	
	'' Insurer Description
	Public Const ACConversionInsurerDescription As String = "D"
	'' Insurer ABI Code List 1
	Public Const ACConversionABICode1 As String = "1"
	
	' Set to Not Mapped at Run-Time
	Public Const ACTypeNoMap As String = "Z"
	
	' Linkage Map ID's
	' Input maps start at 10
	
	' Commercial Vehicle
	Public Const ACInputLinkageMapCV1 As Integer = 30
	Public Const ACInputLinkageMapCV2 As Integer = 40
	'Output maps start at 10010
	Public Const ACOutputLinkageMapCV1 As Integer = 10010
	
	' Private Car
	Public Const ACInputLinkageMapMMIQuoteIn As Integer = 3010
	Public Const ACInputLinkageMapMMLinkRT As Integer = 3110
	Public Const ACInputLinkageMapMControlBlock As Integer = 4010
	Public Const ACInputLinkageMapMQuoteOutput As Integer = 4030
	Public Const ACInputLinkageMapProposalRecord As Integer = 1060
	' Household
	Public Const ACInputLinkageMapQHRisk As Integer = 6010
	Public Const ACInputLinkageMapQHPostQuote As Integer = 7010
	Public Const ACOutputLinkageMapQHQuoteOut As Integer = 8010
	Public Const ACOutputLinkageMapQHSurveyOut As Integer = 9010
	' Commercial Vehicle
	Public Const ACInputLinkageMapCVPreQuote As Integer = 10010
	Public Const ACOutputLinkageMapCVQuoteOut As Integer = 11010
	' CV Post Quote
	Public Const ACInputLinkageMapCVPostQuote As Integer = 12010
	
	' QuoteType Constants
	Public Const ACQuoteTypeQuotesOnly As Integer = 1
	Public Const ACQuoteTypeQuotesDetail As Integer = 2
	Public Const ACQuoteTypeQuotesFull As Integer = 3
	Public Const ACQuoteTypePostQuoteForm As Integer = 4
	Public Const ACQuoteTypePostQuoteEDI As Integer = 5
	Public Const ACQuoteTypePayScreenValidate As Integer = 6
	Public Const ACQuoteTypePostQuoteValidate As Integer = 7
	Public Const ACQuoteTypePostQuoteSave As Integer = 8
	Public Const ACQuoteTypePostQuoteLoad As Integer = 9
	Public Const ACQuoteTypeAuthorisationScreenValidate As Integer = 10
	Public Const ACQuoteTypeSaveUnixMTA As Integer = 11 ' CBL val 58
	Public Const ACQuoteTypePreQuoteSave As Integer = 12 ' CBL val 23
	Public Const ACQuoteTypeNBRequote As Integer = 13
	Public Const ACQuoteTypePageResults As Integer = 13
	Public Const ACQuoteTypeHouseholdBreakdown As Integer = 14
	Public Const ACQuoteTypeMTAFormsEDI As Integer = 15 '  CBL val 99
	Public Const ACQuoteTypeEndorsements As Integer = 16
	Public Const ACQuoteTypeReTransmitEDI As Integer = 17 ' CBL VAL 80
	Public Const ACQuoteTypeReprintDocs As Integer = 18 ' CBL Val 70
	Public Const ACQuoteTypeCoverNote As Integer = 19 ' CBL val 72
	Public Const ACQuoteTypeNoFormEdiTransact As Integer = 20 'sj 21/3/2001
	Public Const ACQuoteTypeRenewalInvitation As Integer = 21 'sj 20/4/2001
	Public Const ACQuoteTypeRenQuotationBrokerLead As Integer = 22 'sj 20/4/2001
	Public Const ACQuoteTypeRenewalTransact As Integer = 23
	Public Const ACQuoteTypeLapse As Integer = 24
	Public Const ACQuoteTypeBrokerLeadQuoteSurvey As Integer = 25
	Public Const ACQuoteTypeBrokerDirectValidate As Integer = 26 ' CBL VAL 53  TB 10/07/2001
	'sj 13/07/2001 - start
	Public Const ACQuoteTypeCancellationLetter As Integer = 27
	Public Const ACQuoteTypeWarningOneLetter As Integer = 28
	Public Const ACQuoteTypeWarningTwoLetter As Integer = 29
	Public Const ACQuoteTypeTimeOnRiskLetter As Integer = 30
	Public Const ACQuoteTypeEdiMTA As Integer = 31
	Public Const ACQuoteTypeEdiTor As Integer = 32
	'sj 13/07/2001 - end
	'sj 08/08/2001 - start
	Public Const ACQuoteTypeRenewalForms As Integer = 33
	'sj 08/08/2001 - end
	Public Const ACQuoteTypeMTAQuote As Integer = 34 '  CBL VAL 05  TB 26/11/01
	Public Const ACQuoteTypeBrokerDirectMTACommission As Integer = 35 ' CBL VAL 54 TB 23/1/02
	'sj 05/03/2002 - start
	Public Const ACQuoteTypeLoadVbsData As Integer = 36
	Public Const ACQuoteTypeSaveVbsData As Integer = 37
	'sj 05/03/2002 - end
	'sjd 10/10/2002 - start
	Public Const ACQuoteTypeMtaCancel As Integer = 38
	'sjd 10/10/2002
	Public Const ACQuoteTypeRenPayScreenValidate As Integer = 39 'JSB 22/05/03 - renewal pay screen validate constant
	Public Const ACQuoteTypeRenPremiumUpdate As Integer = 40 'JSB 30/07/03 - update with new premium
	
	' ExcessArray Constants
	Public Const ACExcessArrayBound As Integer = 2
	Public Const ACExcessArrayPos As Integer = 0
	Public Const ACExcessArrayLength As Integer = 1
	Public Const ACExcessArrayDescription As Integer = 2
	
	' Premium Analysis Array
	Public Const ACPremiumAnalysisArrayFields As Integer = 5
	Public Const ACPremiumAnalysisArrayMaxBound As Integer = 200
	Public Const ACPremiumAnalysisCodePos As Integer = 0
	Public Const ACPremiumAnalysisCodeLength As Integer = 1
	Public Const ACPremiumAnalysisAmtPos As Integer = 2
	Public Const ACPremiumAnalysisAmtLength As Integer = 3
	Public Const ACPremiumAnalysisTotalPos As Integer = 4
	Public Const ACPremiumAnalysisTotalLength As Integer = 5
	
	
	' sj - start
	'Property Id Constants (Temporary ) for demo
	Public Const ACPropNetPrem As Integer = 1
	Public Const ACPropLoadCode As Integer = 2
	Public Const ACPropLoadAmt As Integer = 3
	Public Const ACPropRunTot As Integer = 4
	Public Const ACPropEndCode As Integer = 5
	Public Const ACPropEndVar1 As Integer = 6
	Public Const ACPropEndVar2 As Integer = 7
	
	'Array constants
	Public Const ACBreakdownLoadCode As Integer = 0
	Public Const ACBreakdownLoadAmt As Integer = 1
	Public Const ACBreakdownRunTot As Integer = 2
	
	Public Const ACEndorsementsEndCode As Integer = 0
	Public Const ACEndorsementsEndVar1 As Integer = 1
	Public Const ACEndorsementsEndVar2 As Integer = 2
	
	'sj - end
	
	' GIS Object Constants
	
	Public Const ACGIIMQuickQuoteResult As String = "Quick_Quote_Result"
	Public Const ACGIIMQuickQuoteResult_SchemeID As String = "Scheme_ID"
	Public Const ACGIIMQuickQuoteResult_SchemeDesc As String = "Scheme_Desc"
	Public Const ACGIIMQuickQuoteResult_CommisionRate As String = "Commision_Rate"
	Public Const ACGIIMQuickQuoteResult_CommisionAmount As String = "Commision_Amount"
	Public Const ACGIIMQuickQuoteResult_SchemeType As String = "Scheme_Type"
	Public Const ACGIIMQuickQuoteResult_Premium As String = "Premium"
	Public Const ACGIIMQuickQuoteResult_QuoteType As String = "Quote_Type"
	Public Const ACGIIMQuickQuoteResult_NCBProtected As String = "NCB_Protected"
	Public Const ACGIIMQuickQuoteResult_CoverType As String = "Cover_Type"
	Public Const ACGIIMQuickQuoteResult_TotalExcess As String = "Total_Excess"
	Public Const ACGIIMQuickQuoteResult_IPT As String = "IPT"
	Public Const ACGIIMQuickQuoteResult_Compulsory_Excess As String = "Compulsory_Excess"
	Public Const ACGIIMQuickQuoteResult_Voluntary_Excess As String = "Voluntary_Excess"
	Public Const ACGIIMQuickQuoteResult_Rates_Date As String = "Rates_Date"
	Public Const ACGIIMQuickQuoteResult_Vehicle_Group As String = "Vehicle_Group"
	Public Const ACGIIMQuickQuoteResult_Vehicle_Area As String = "Vehicle_Area"
	Public Const ACGIIMQuickQuoteResult_NCD_Years As String = "NCD_Years"
	Public Const ACGIIMQuickQuoteResult_NCD_Discount As String = "NCD_Discount"
	Public Const ACGIIMQuickQuoteResult_Young_Driver_Excess As String = "Young_Driver_Excess"
	Public Const ACGIIMQuickQuoteResult_Comp_Non_AD_Excess As String = "Compulsory_Non_AD_Excess"
	Public Const ACGIIMQuickQuoteResult_Comp_Non_AD_Desc As String = "Compulsory_Non_AD_Desc"
	Public Const ACGIIMQuickQuoteResult_Windscreen_Excess As String = "Windscreen_Excess"
	Public Const ACGIIMQuickQuoteResult_Windscreen_Limit As String = "Windscreen_Limit"
	Public Const ACGIIMQuickQuoteResult_CommissionMinimum As String = "Commission_Minimum"
	
	Public Const ACGIIMPremiumAnalysis As String = "Premium_Analysis"
	Public Const ACGIIMPremiumAnalysis_Code As String = "Code"
	Public Const ACGIIMPremiumAnalysis_Description As String = "Description"
	Public Const ACGIIMPremiumAnalysis_Amount As String = "Amount"
	Public Const ACGIIMPremiumAnalysis_Running_Total As String = "Running_Total"
	
	Public Const ACGIIMNotesBreakdown As String = "Notes_Breakdown"
	Public Const ACGIIMNotesBreakdown_Code As String = "Code"
	Public Const ACGIIMNotesBreakdown_Description As String = "Description"
	
	Public Const ACGIIMExcess As String = "Excess_Breakdown"
	Public Const ACGIIMExcess_Code As String = "Code"
	Public Const ACGIIMExcess_Amt As String = "Amt"
	Public Const ACGIIMExcess_SectionCode As String = "Section_Code"
	Public Const ACGIIMExcess_Description As String = "Description"
	
	Public Const ACGIIMEndorsementsBreakdown As String = "Endorsements_Breakdown"
	Public Const ACGIIMEndorsementsBreakdown_Code As String = "Code"
	Public Const ACGIIMEndorsementsBreakdown_Title As String = "Title"
	
	Public Const ACGIIMReferrals As String = "Referrals"
	Public Const ACGIIMReferrals_SchemeID As String = "Scheme_ID"
	Public Const ACGIIMReferrals_SchemeDesc As String = "Scheme_Desc"
	Public Const ACGIIMReferrals_CommissionRate As String = "Commission_Rate"
	Public Const ACGIIMReferrals_CommissionAmount As String = "Commission_Amount"
	Public Const ACGIIMReferrals_SchemeType As String = "Scheme_Type"
	Public Const ACGIIMReferrals_Premium As String = "Premium"
	Public Const ACGIIMReferrals_QuoteType As String = "Quote_Type"
	Public Const ACGIIMReferrals_NCB_Protected As String = "NCB_Protected"
	Public Const ACGIIMReferrals_CoverType As String = "Cover_Type"
	Public Const ACGIIMReferrals_TotalExcesss As String = "Total_Excesss"
	
	Public Const ACGIIMReferReasons As String = "Refer_Reasons"
	Public Const ACGIIMReferReasons_Reason As String = "Reason"
	Public Const ACGIIMReferReasons_Code As String = "Code"
	Public Const ACGIIMReferReasons_Text As String = "Text"
	Public Const ACGIIMReferReasons_VehiclePRN As String = "Vehicle_PRN"
	Public Const ACGIIMReferReasons_DriverPRN As String = "Driver_PRN"
	
	Public Const ACGIIMDeclines As String = "Declines"
	Public Const ACGIIMDeclines_SchemeID As String = "Scheme_ID"
	Public Const ACGIIMDeclines_SchemeDesc As String = "Scheme_Desc"
	Public Const ACGIIMDeclines_CommissionRate As String = "Commission_Rate"
	Public Const ACGIIMDeclines_CommissionAmount As String = "Commission_Amount"
	Public Const ACGIIMDeclines_SchemeType As String = "Scheme_Type"
	Public Const ACGIIMDeclines_CoverType As String = "Cover_Type"
	
	Public Const ACGIIMDeclineReasons As String = "Decline_Reasons"
	Public Const ACGIIMDeclineReasons_Reason As String = "Reason"
	Public Const ACGIIMDeclineReasons_Code As String = "Code"
	Public Const ACGIIMDeclineReasons_Text As String = "Text"
	Public Const ACGIIMDeclineReasons_VehiclePRN As String = "Vehicle_PRN"
	Public Const ACGIIMDeclineReasons_DriverPRN As String = "Driver_PRN"
	
	'IJR 2003-03-05 Start
	Public Const ACGIIMQuoteBinder As String = "Quote_Binder"
	'IJR 2003-03-05 End
	
	Public Const ACGIIMQuoteError As String = "Quote_Error"
	Public Const ACGIIMQuoteErrorBreakdown As String = "Quote_Error_Breakdown"
	
	'IJR 2003-03-05 Start
	Public Const ACGIIMQuoteBinder_QuoteBinderId As String = "Quote_Binder_Id"
	Public Const ACGIIMQuoteError_QuoteBinderId As String = "Quote_Binder_Id"
	'IJR 2003-03-05 End
	'IDP March 2003
	Public Const ACGIIMQuoteBinder_GISPolicyLinkID As String = "GIS_POLICY_LINK_ID"
	Public Const ACGIIMQuoteError_GISPolicyLinkID As String = "GIS_POLICY_LINK_ID"
	'IDP END
	
	Public Const ACGIIMQuoteError_SchemeId As String = "Scheme_Id"
	
	Public Const ACGIIMQuoteErrorBreakdown_Description As String = "Description"
	Public Const ACGIIMQuoteErrorBreakdown_ScreenName As String = "Screen_Name"
	Public Const ACGIIMQuoteErrorBreakdown_Level As String = "Level"
	
	' GIS Object Constants - End
	
	'Public Const DO_QUOTE_COVER_TYPE_COMP = "Comprehensive"
	'Public Const DO_QUOTE_COVER_TYPE_TPFT = "Third Party, Fire And Theft"
	'Public Const DO_QUOTE_COVER_TYPE_TP = "Third Party"
	
	'Constants moved from QuoteConst
	Public Const ACListItemQuotes As String = "Quotes"
	Public Const ACListItemDeclines As String = "Declines"
	Public Const ACListItemReferrals As String = "Referrals"
	
	Public Const ACListItemPremiumOverride As String = "PremiumOverride"
	Public Const ACListItemPremiumFinance As String = "PremiumFinance"
	Public Const ACListItemBrokerAddOns As String = "BrokerAddOns"
	Public Const ACListItemInsurerAddOns As String = "InsurerAddOns"
	
	' Select GisCobolLinkage SQL
	Public Const ACGetSchemeDetailsStored As Boolean = True
	Public Const ACGetSchemeDetailsName As String = "SelectSchemeDetails"
    Public Const ACGetSchemeDetailsSQL As String = "spu_Scheme_Details_Select"
	
	' Cobol Linkage Control Block Constants
	Public Const ACGCob_control_block As String = "gcob-control-block"
	Public Const ACG2_control_input As String = "g2-control-input"
	Public Const ACG2_iccs As String = "g2-iccs"
	Public Const ACG2_coy As String = "g2-coy"
	Public Const ACG2_qmcoy As String = "g2-qmcoy"
	Public Const ACG2_op_id As String = "g2-op-id"
	Public Const ACG2_ctl_name As String = "g2-ctl-name"
	Public Const ACG2_rpr_doc As String = "g2-rpr-doc"
	Public Const ACG2_ipt_data As String = "g2-ipt-data"
	' Public Const ACG2_ipt_record_OCC_START = "g2-ipt-record-OCC-START"
	' Public Const ACG2_ipt_risk = "g2-ipt-risk"
	' Public Const ACG2_ipt_rate = "g2-ipt-rate"
	' Public Const ACG2_ipt_record_OCC_END = "g2-ipt-record-OCC-END"
	Public Const ACG2_Form As String = "g2-form"
	Public Const ACG2_run_type As String = "g2-run-type"
	Public Const ACG2_int_or_dll As String = "g2-int-or-dll"
	Public Const ACG2_call_type As String = "g2-call-type"
	Public Const ACG2_quote_list As String = "g2-quote-list"
	Public Const ACG2_no_quotes As String = "g2-no-quotes"
	Public Const ACG2_quote_line_OCC_START As String = "g2-quote-line-OCC-START"
	Public Const ACG2_quote_mnemo As String = "g2-quote-mnemo"
	Public Const ACG2_quote_coy As String = "g2-quote-coy"
	Public Const ACG2_quote_schm As String = "g2-quote-schm"
	Public Const ACG2_scheme_ID As String = "g2-scheme-id"
	Public Const ACG2_quote_line_OCC_END As String = "g2-quote-line-OCC-END"
	Public Const ACG2_excess_list As String = "g2-excess-list"
	Public Const ACG2_no_excesses As String = "g2-no-excesses"
	Public Const ACG2_excess_line_OCC_START As String = "g2-excess-line-OCC-START"
	Public Const ACG2_excess_amount As String = "g2-excess-amount"
	Public Const ACG2_excess_line_OCC_END As String = "g2-excess-line-OCC-END"
	Public Const ACG2_ncd_protect As String = "g2-ncd-protect"
	Public Const ACG2_cov_required As String = "g2-cov-required"
	Public Const ACG2_mta_type As String = "g2-mta-type"
	Public Const ACG2_gcob_ren_rec_no As String = "gcob-ren-rec-no"
	
	' TB 29/9/00 - MTA fields for cobol
	Public Const ACG2_g2_mta_effective_date As String = "g2-mta-effective-date"
	Public Const ACG2_g2_mta_effective_Time As String = "g2-mta-effective-time"
	Public Const ACG2_g2_mta_end_date As String = "g2-mta-end-date"
	Public Const ACG2_g2_mta_end_time As String = "g2-mta-end-time"
	Public Const ACG2_g2_mta_premium As String = "g2-mta-premium"
	Public Const ACG2_g2_mta_premium_inc_ipt As String = "g2-mta-premium-inc-ipt"
	Public Const ACG2_g2_mta_new_original_premium As String = "g2-mta-new-original-premium"
	Public Const ACG2_g2_mta_ipt_prem As String = "g2-mta-ipt-prem"
	Public Const ACG2_g2_mta_pay_method As String = "g2-mta-pay-method"
	Public Const ACG2_g2_mta_requote_prem_inc_ipt As String = "g2-mta-requote-prem-inc-ipt"
	'sj 19/07/2001 - start
	Public Const ACG2_g2_mta_requote_prem_excl_ipt As String = "g2-mta-requote-prem-excl-ipt"
	Public Const ACG2_g2_mta_link_read_recno As String = "g2-mta-link-read-recno"
	Public Const ACG2_g2_hist_entry_no As String = "g2-hist-entry-no"
	'sj 19/07/2001 - end
	
	Public Const ACG2_policy_prospect As String = "g2-policy-prospect"
	Public Const ACGCob_control_output As String = "gcob-control-output"
	Public Const ACGCob_return_code As String = "gcob-return-code"
	Public Const ACGCob_error_text As String = "gcob-error-text"
	Public Const ACGCob_error_level As String = "gcob-error-level"
	Public Const ACGCob_policy_status As String = "gcob-policy-status"
	Public Const ACGCob_vbs_ref As String = "gcob-vbs-ref"
	Public Const ACGCob_qmm_ref As String = "gcob-qmm-ref"
	Public Const ACGCob_scheme_data As String = "gcob-scheme-data"
	Public Const ACGS_edi_data As String = "gs-edi-data"
	Public Const ACGS_agency_code As String = "gs-agency-code"
	Public Const ACGS_send_mailbox_id As String = "gs-send-mailbox-id"
	Public Const ACGS_send_network_id As String = "gs-send-network-id"
	Public Const ACGS_recv_mailbox_id As String = "gs-recv-mailbox-id"
	Public Const ACGS_recv_network_id As String = "gs-recv-network-id"
	Public Const ACGS_test_live_flag As String = "gs-test-live-flag"
	Public Const ACGS_client_ref As String = "gs-client-ref"
	Public Const ACGS_covernote_number As String = "gs-covernote-number"
	Public Const ACGS_prev_insr_desc As String = "gs-prev-insr-desc"
	Public Const ACGS_prev_insr_codelist_1 As String = "gs-prev-insr-codelist-1"
	Public Const ACGS_broker_name As String = "gs-broker-name"
	Public Const ACG2_Mta_Details_Changed As String = "g2-mta-details-changed"
	Public Const ACGCobPolSystemNum As String = "gcob-pol-system-num"
	Public Const ACGCobPolVersionNum As String = "gcob-pol-version-num"
	Public Const ACGCobCliSystemNum As String = "gcob-cli-system-num"
	Public Const ACGCobCliVersionNum As String = "gcob-cli-version-num"
	
	Public Const ACG2GeminiNetFlag As String = "g2-gemini-net-flag"
	
	' TB 10/07/2001 - Broker Direct Return Fields
	Public Const ACGCOB_Broker_Direct As String = "gcob-broker-direct"
	' TB 29/1/02 - More Broker Direct fields
	
	Public Const ACGCOB_Comm_Rate As String = "gcob-comm-rate"
	Public Const ACGCOB_Comm_Amt As String = "gcob-comm-amt"
	Public Const ACGCOB_Comm_Min As String = "gcob-comm-min"
	' SJD 16/8/02 - MTA pass type for use in COBOL
	Public Const ACGCOB_Mta_Pass As String = "gcob-mta-pass"
	
	' PMB lookup properties from accounts screen
	Public Const ACGS_ux_insurer As String = "gs-ux-insurer"
	Public Const ACGS_ux_risk As String = "gs-ux-risk"
	Public Const ACGS_ux_brokerage As String = "gs-ux-brokerage"
	Public Const ACGS_ux_analysis As String = "gs-ux-analysis"
	
	Public Const ACG2_screen_to_validate As String = "g2-screen-to-validate"
	Public Const ACG2_req_xs As String = "g2-req-xs"
	Public Const ACG2_req_ncd_protect As String = "g2-req-ncd-protect"
	Public Const ACG2_req_cov As String = "g2-req-cov"
	Public Const ACG2_home_schm As String = "g2-home-schm"
	Public Const ACG2_home_coy As String = "g2-home-coy"
	
	Public Const ACG2_UpdatedPremium As String = "g2-update-premium" 'JSB 30/7/03
	
	' Optional fileds, EDI
	Public Const ACgcob_edi_msg_file As String = "gcob-edi-msg-file"
	Public Const ACgcob_edi_seq_no As String = "gcob-edi-seq-no"
	Public Const ACgcob_edi_msg_type As String = "gcob-edi-msg-type"
	Public Const ACgcob_memo_msg_file As String = "gcob-memo-msg-file"
	Public Const ACgcob_memo_seq_no As String = "gcob-memo-seq-no"
	Public Const ACgcob_memo_msg_type As String = "gcob-memo-msg-type"
	Public Const ACgcob_Unique_Client_Key As String = "gcob-unique-client-key"
	Public Const ACgcob_Unique_Policy_key As String = "gcob-unique-policy-key"
	
	' Cover / Quote Constants
	Public Const ACCoverComprehensive As String = "Comprehensive"
	Public Const ACCoverTPFT As String = "Third Party, Fire And Theft"
	Public Const ACCoverTP As String = "Third Party"
	'Public Const ACQuoteTypeQuotesOnly = 1
	'Public Const ACQuoteTypeQuotesFull = 3
	'Public Const ACQuoteTypePostQuoteForm = 4
	'Public Const ACQuoteTypePostQuoteEDI = 5
	
	' Cobol Linkage QuoteOutput Constants
	Public Const ACquote_output As String = "quote-output"
	Public Const ACquote_output_1 As String = "quote-output-1"
	Public Const ACqo_mmop_count As String = "qo-mmop-count"
	Public Const ACqo_op_entry_OCC_START As String = "qo-op-entry-OCC-START"
	Public Const ACqo_ocoyno As String = "qo-ocoyno"
	Public Const ACqo_omnemo As String = "qo-omnemo"
	Public Const ACqo_ovehgrp As String = "qo-ovehgrp"
	Public Const ACqo_oareagr As String = "qo-oareagr"
	Public Const ACqo_oscheme As String = "qo-oscheme"
	Public Const ACqo_orateset As String = "qo-orateset"
	Public Const ACqo_ocycle As String = "qo-ocycle"
	Public Const ACqo_ocover As String = "qo-ocover"
	Public Const ACqo_oerrcode As String = "qo-oerrcode"
	Public Const ACqo_onetprem As String = "qo-onetprem"
	Public Const ACqo_oncdp As String = "qo-oncdp"
	Public Const ACqo_oclasx As String = "qo-oclasx"
	Public Const ACqo_oncompxs As String = "qo-oncompxs"
	Public Const ACqo_oncomxsyng As String = "qo-oncomxsyng"
	Public Const ACqo_onvolxs As String = "qo-onvolxs"
	Public Const ACqo_oncompxst As String = "qo-oncompxst"
	Public Const ACqo_oncomxsyngt As String = "qo-oncomxsyngt"
	Public Const ACqo_onvolxst As String = "qo-onvolxst"
	Public Const ACqo_onncmpxs As String = "qo-onncmpxs" ' Total of all Non Compulsary XS
	Public Const ACqo_onncmpxst As String = "qo-onncmpxst"
	Public Const ACqo_onvehxs As String = "qo-onvehxs" ' Vehicle XS
	Public Const ACqo_onvehxst As String = "qo-onvehxst"
	Public Const ACqo_onareaxs As String = "qo-onareaxs" ' Area XS
	Public Const ACqo_onareaxst As String = "qo-onareaxst"
	Public Const ACqo_onpncdxs As String = "qo-onpncdxs" ' Protected NCD XS
	Public Const ACqo_onpncdxst As String = "qo-onpncdxst"
	Public Const ACqo_onwsxs As String = "qo-onwsxs" ' Windscreen XS
	Public Const ACqo_onwsxst As String = "qo-onwsxst"
	Public Const ACqo_onwslim As String = "qo-onwslim" ' Max amount of windscreen cover
	Public Const ACqo_ondagexst As String = "qo-ondagexst" ' Type for following occurs
	Public Const ACqo_ondoccxst As String = "qo-ondoccxst"
	Public Const ACqo_ondnstxst As String = "qo-ondnstxst"
	Public Const ACqo_onddisxst As String = "qo-onddisxst"
	Public Const ACqo_ondsexxst As String = "qo-ondsexxst"
	Public Const ACqo_ondrvxs_OCC_START As String = "qo-ondrvxs-OCC-START"
	Public Const ACqo_ondagexs As String = "qo-ondagexs" ' Driver Age
	Public Const ACqo_ondoccxs As String = "qo-ondoccxs" ' Driver Occ.
	Public Const ACqo_ondnstxs As String = "qo-ondnstxs" ' Driver Non Standard
	Public Const ACqo_onddisxs As String = "qo-onddisxs" ' Driver disability
	Public Const ACqo_ondsexxs As String = "qo-ondsexxs" ' Driver Sex
	Public Const ACqo_ondrvxs_OCC_END As String = "qo-ondrvxs-OCC-END"
	Public Const ACqo_ovterm As String = "qo-ovterm"
	Public Const ACqo_ovload As String = "qo-ovload"
	Public Const ACqo_oplrrecid As String = "qo-oplrrecid"
	Public Const ACqo_ogarterm As String = "qo-ogarterm"
	Public Const ACqo_oratdri As String = "qo-oratdri"
	Public Const ACqo_oloadpts As String = "qo-oloadpts"
	Public Const ACqo_oxdoc As String = "qo-oxdoc"
	Public Const ACqo_oplrterm As String = "qo-oplrterm"
	Public Const ACfiller_1 As String = "filler-1"
	Public Const ACqo_onnoseq As String = "qo-onnoseq"
	Public Const ACqo_onsequence_OCC_START As String = "qo-onsequence-OCC-START"
	Public Const ACqo_onseqcode As String = "qo-onseqcode"
	Public Const ACqo_onseqamt As String = "qo-onseqamt"
	Public Const ACqo_onseqstot As String = "qo-onseqstot"
	Public Const ACqo_onsequence_OCC_END As String = "qo-onsequence-OCC-END"
	Public Const ACqo_ononotes As String = "qo-ononotes" ' No. of Underwriting Notes
	Public Const ACqo_onotes_OCC_START As String = "qo-onotes-OCC-START"
	Public Const ACqo_onoteno As String = "qo-onoteno" ' Underwriting Note Code No.  By adding 8000 to code you obtain the code for the naration file (PMDIR/A/PONF(Co. No).ISE)
	Public Const ACqo_onval1 As String = "qo-onval1" ' Stores value which is substitued within the above string for displaying variable value in the text
	Public Const ACqo_onval2 As String = "qo-onval2"
	Public Const ACqo_onotes_OCC_END As String = "qo-onotes-OCC-END"
	Public Const ACqo_ovbs As String = "qo-ovbs" ' "O" = EDI Only; "V" = Can Be EDI;
	Public Const ACqo_ofrm As String = "qo-ofrm" ' "F" = Forms availible;
	Public Const ACqo_ogtd As String = "qo-ogtd" ' "G" = Guaranteed; "P" = Polaris;
	Public Const ACqo_o2carm As String = "qo-o2carm" ' "S" = 2nd Car Scheme; "N" = Net Rates;
	Public Const ACfiller_2 As String = "filler-2"
	Public Const ACqo_op_entry_OCC_END As String = "qo-op-entry-OCC-END"
	Public Const ACquote_output_2 As String = "quote-output-2"
	Public Const ACqo_mmres_count As String = "qo-mmres-count"
	Public Const ACqo_mmres_cover_ptr_OCC_START As String = "qo-mmres-cover-ptr-OCC-START"
	Public Const ACqo_mmres_cover_ptr As String = "qo-mmres-cover-ptr"
	Public Const ACqo_mmres_cover_ptr_OCC_END As String = "qo-mmres-cover-ptr-OCC-END"
	Public Const ACqo_mmres_cover_count_OCC_START As String = "qo-mmres-cover-count-OCC-START"
	Public Const ACqo_mmres_cover_count As String = "qo-mmres-cover-count"
	Public Const ACqo_mmres_cover_count_OCC_END As String = "qo-mmres-cover-count-OCC-END"
	Public Const ACqo_mmres_array As String = "qo-mmres-array"
	Public Const ACqo_mmres_item_OCC_START As String = "qo-mmres-item-OCC-START"
	Public Const ACqo_mmres_cover As String = "qo-mmres-cover"
	Public Const ACqo_mmres_net As String = "qo-mmres-net"
	Public Const ACqo_mmres_xs As String = "qo-mmres-xs"
	Public Const ACqo_mmres_coyno As String = "qo-mmres-coyno"
	Public Const ACqo_mmres_scheme As String = "qo-mmres-scheme"
	Public Const ACqo_mmres_coy As String = "qo-mmres-coy"
	Public Const ACqo_mmres_vbs As String = "qo-mmres-vbs"
	Public Const ACqo_mmres_frm As String = "qo-mmres-frm"
	Public Const ACqo_mmres_gtd As String = "qo-mmres-gtd"
	Public Const ACqo_mmres_2car As String = "qo-mmres-2car"
	Public Const ACqo_mmres_err_ptr As String = "qo-mmres-err-ptr"
	Public Const ACqo_mmres_item_OCC_END As String = "qo-mmres-item-OCC-END"
	Public Const ACquote_output_3 As String = "quote-output-3"
	Public Const ACfiller_3 As String = "filler-3"
	
	'Post Quote Constants
	Public Const ACPQpay_instal_usual As String = "pay-instal-usual"
	Public Const ACPQpay_instal_amt As String = "pay-instal-amt"
	Public Const ACPQipt_ipt_prem As String = "ipt-ipt-prem"
	Public Const ACPQpay_instal_dept As String = "pay-instal-dept"
	Public Const ACPQpay_instal_no As String = "pay-instal-no"
	
	'sj 04/12/2000 - start
	'Public Const PROP_QUOTE_COVER_TYPE = "O81P1331"
	'Public Const PROP_QUOTE_PREMIUM_INC_IPT = "O81P1328"
	'Public Const PROP_QUOTE_IPT = "O81P1333"
	'Public Const PROP_QUOTE_TOTAL_EXCESS = "O81P1332"
	'Public Const PROP_QUOTE_SCHEME_ID = "O81P1323"
	'Public Const PROP_QUOTE_SCHEME_DESC = "O81P1324"
	'Public Const PROP_QUOTE_SCHEME_TYPE = "O81P1327"
	'Public Const PROP_QUOTE_RATESET = "O81P1377"
	'Public Const PROP_QUOTE_VEHGRP = "O81P1378"
	'Public Const PROP_QUOTE_AREAGRP = "O81P1379"
	'Public Const PROP_QUOTE_NCDP = "O81P1381"
	'Public Const PROP_QUOTE_VOLXS = "O81P1335"
	'Public Const PROP_QUOTE_COMPXS = "O81P1334"
	'Public Const PROP_QUOTE_YOUNG_DRIVER = "O81P1382"
	'Public Const PROP_QUOTE_COMP_NON_AD_EXCESS = "O81P1383"
	'Public Const PROP_QUOTE_COMP_NON_AD_DESC = "O81P1384"
	'Public Const PROP_QUOTE_WINDSCREEN_XS = "O81P1385"
	'Public Const PROP_QUOTE_WINDSCREEN_LIMIT = "O81P1386"
	'
	'Public Const PROP_EXCESS_AMT = "O87P1369"
	'Public Const PROP_EXCESS_DESC = "O87P1371"
	'Public Const PROP_EXCESS_CODE = "O87P1372"
	'
	'Public Const PROP_ANALYSIS_CODE = "O90P1387"
	'Public Const PROP_ANALYSIS_DESC = "O90P1388"
	'Public Const PROP_ANALYSIS_AMT = "O90P1389"
	'Public Const PROP_ANALYSIS_TOTAL = "O90P1390"
	'
	'Public Const PROP_NOTES_VALUE1 = "O88P1997"
	'Public Const PROP_NOTES_VALUE2 = "O88P1998"
	
	Public Const PROP_QUOTE_COVER_TYPE As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_CoverType
	Public Const PROP_QUOTE_PREMIUM_INC_IPT As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Premium
	Public Const PROP_QUOTE_IPT As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_IPT
	Public Const PROP_QUOTE_TOTAL_EXCESS As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_TotalExcess
	Public Const PROP_QUOTE_SCHEME_ID As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_SchemeID
	Public Const PROP_QUOTE_SCHEME_DESC As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_SchemeDesc
	Public Const PROP_QUOTE_SCHEME_TYPE As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_SchemeType
	Public Const PROP_QUOTE_RATESET As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Rates_Date
	Public Const PROP_QUOTE_VEHGRP As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Vehicle_Group
	Public Const PROP_QUOTE_AREAGRP As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Vehicle_Area
	Public Const PROP_QUOTE_NCDP As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_NCBProtected
	Public Const PROP_QUOTE_VOLXS As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Voluntary_Excess
	Public Const PROP_QUOTE_COMPXS As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Compulsory_Excess
	Public Const PROP_QUOTE_YOUNG_DRIVER As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Young_Driver_Excess
	Public Const PROP_QUOTE_COMP_NON_AD_EXCESS As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Comp_Non_AD_Excess
	Public Const PROP_QUOTE_COMP_NON_AD_DESC As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Comp_Non_AD_Desc
	Public Const PROP_QUOTE_WINDSCREEN_XS As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Windscreen_Excess
	Public Const PROP_QUOTE_WINDSCREEN_LIMIT As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_Windscreen_Limit
	Public Const PROP_QUOTE_INSURER_MNEMONIC As String = ACGIIMQuickQuoteResult & ":" & "Insurer_Mnemonic"
	' TB 21/02/01
	Public Const PROP_QUOTE_COMMISSION_AMOUNT As String = ACGIIMQuickQuoteResult & ":" & "Commission_Amount"
	' SJD 25/02/02
	Public Const PROP_QUOTE_COMMISSION_MINIMUM As String = ACGIIMQuickQuoteResult & ":" & ACGIIMQuickQuoteResult_CommissionMinimum
	' TB 30/1/02
	Public Const PROP_QUOTE_COMMISSION_RATE As String = ACGIIMQuickQuoteResult & ":" & "Commission_Rate"
	Public Const PROP_QUOTE_QUOTE_TYPE As String = ACGIIMQuickQuoteResult & ":" & "Quote_Type"
	' 02/05/2001 PSA - Start
	Public Const PROP_QUOTE_TRUE_NET_PREMIUM As String = ACGIIMQuickQuoteResult & ":" & "True_Net_Premium"
	' 02/05/2001 PSA - End
	
	Public Const PROP_EXCESS_AMT As String = ACGIIMExcess & ":" & ACGIIMExcess_Amt
	Public Const PROP_EXCESS_DESC As String = ACGIIMExcess & ":" & ACGIIMExcess_Description
	Public Const PROP_EXCESS_CODE As String = ACGIIMExcess & ":" & ACGIIMExcess_SectionCode
	
	Public Const PROP_ANALYSIS_CODE As String = ACGIIMPremiumAnalysis & ":" & ACGIIMPremiumAnalysis_Code
	Public Const PROP_ANALYSIS_DESC As String = ACGIIMPremiumAnalysis & ":" & ACGIIMPremiumAnalysis_Description
	Public Const PROP_ANALYSIS_AMT As String = ACGIIMPremiumAnalysis & ":" & ACGIIMPremiumAnalysis_Amount
	Public Const PROP_ANALYSIS_TOTAL As String = ACGIIMPremiumAnalysis & ":" & ACGIIMPremiumAnalysis_Running_Total
	
	Public Const PROP_NOTES_CODE As String = ACGIIMNotesBreakdown & ":" & ACGIIMNotesBreakdown_Code
	Public Const PROP_NOTES_DESCRIPTION As String = ACGIIMNotesBreakdown & ":" & ACGIIMNotesBreakdown_Description
	Public Const PROP_NOTES_VALUE1 As String = ACGIIMNotesBreakdown & ":" & "Value1"
	Public Const PROP_NOTES_VALUE2 As String = ACGIIMNotesBreakdown & ":" & "Value2"
	
	'Declines
	Public Const PROP_DECLINES_SCHEME_ID As String = ACGIIMDeclines & ":" & "Scheme_Id"
	Public Const PROP_DECLINES_COVER_TYPE As String = ACGIIMDeclines & ":" & "Cover_Type"
	Public Const PROP_DECLINES_SCHEME_DESC As String = ACGIIMDeclines & ":" & "Scheme_Desc"
	
	'Decline Reasons
	Public Const PROP_DECLINE_REASONS_CODE As String = ACGIIMDeclineReasons & ":" & "Code"
	Public Const PROP_DECLINE_REASONS_REASON As String = ACGIIMDeclineReasons & ":" & "Reason"
	Public Const PROP_DECLINE_REASONS_TEXT As String = ACGIIMDeclineReasons & ":" & "Text"
	'sj 04/12/2000 - end
	
	'sj 04/05/2001 - start
	'Quick Quote Binder
	Public Const ACGIIQuickQuoteBinder As String = "Quote_Binder"
	Public Const ACGIIGisSchemeId As String = "gis_scheme_id"
	Public Const PROP_QUOTE_BINDER_GIS_SCHEME_ID As String = ACGIIQuickQuoteBinder & ":" & ACGIIGisSchemeId
	'sj 04/05/2001 - end
	
	'SSL 20/8/01 - start
	Public Const ACGIIPreferredInd As String = "Preferred_Ind"
	'ssl - end
	
	' TB 15/1/00 - Types passed to PackLinkageString
	Public Const ACUnPackString As Integer = 1
	Public Const ACPackNewString As Integer = 2
	Public Const ACAddToString As Integer = 3
	
	' Message Type
	Public Const ACErrMessageGISOnly As Integer = 0
	Public Const ACErrMessagePMOnly As Integer = 1
	Public Const ACErrMessageBoth As Integer = 2
	
	' Household List Constants
	Public Const ACDescriptionFileListId As Integer = 1000
	Public Const ACControlFileListId As Integer = 1500
	Public Const ACYesNoListId As Integer = 1600
	Public Const ACYearsAtCurrentAddressListId As Integer = 1800
	'Public Const ACUnderwritingTextListId = 1900
	Public Const ACUnderwritingHeaderListId As Integer = 2030
	
	'RT100599 - Gis SaveToDBOnExit Constants
	Public Const GISSaveToDBNoPromptNoSave As String = "0"
	Public Const GISSaveToDBNoPromptSave As String = "1"
	Public Const GISSaveToDBPromptForSave As String = "2"
	
	Public Const GIIRegUseQuoteCompany As String = "UseQuoteCompany"
	Public Const GIIUseSameCompanyToQuote As Integer = 0
	
	'RDT010301 - Risk Codes
	Public Const GIIRiskCodeHHContents As String = "107"
	Public Const GIIRiskCodeHHBuildings As String = "108"
	Public Const GIIRiskCodeHHCombined As String = "109"
	Public Const GIIRiskCodePMTPO As String = "291"
	Public Const GIIRiskCodePMTPFT As String = "292"
	Public Const GIIRiskCodePMComp As String = "293"
	Public Const GIIRiskCodeMCTPO As String = "294"
	Public Const GIIRiskCodeMCTPFT As String = "295"
	Public Const GIIRiskCodeMCComp As String = "296"
	Public Const GIIRiskCodeCVTPO As String = "297"
	Public Const GIIRiskCodeCVTPFT As String = "298"
	Public Const GIIRiskCodeCVComp As String = "299"
	Public Const GIIRiskCodeTravel As String = "150"
	
	'TB
	'Diary Constants
	Public Const GIIBatchComponent As String = "bGIIBatchTask.Business"
	Public Const GIITaskCodeDiaryCovernote As String = "DRY_CVN"
	Public Const GIITaskCodeDiaryCancellationLetter As String = "DRY_L"
	Public Const GIITaskCodeDiaryWarningOneLetter As String = "DRY_R"
	Public Const GIITaskCodeDiaryWarningTwoLetter As String = "DRY_S"
	Public Const GIITaskCodeDiaryTimeOnRiskLetter As String = "DRY_T"
	Public Const GIITaskCodeDiaryMTA As String = "DRY_MTA"
	Public Const GIITaskCodeDiaryLapse As String = "DRY_LAP"
	Public Const GIITaskCodeDiaryTor As String = "DRY_TOR"
	
	' IDP Jan 2003 GII Merged 1.6 / 1.8 Version
End Module