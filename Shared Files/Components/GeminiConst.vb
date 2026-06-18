Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
'Module GeminiConstants
Public Module GeminiConstants
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	'******************************************************************************
	' GEMINI SPECIFIC CONSTANT DEFINITIONS
	'******************************************************************************
	
	' Max string length in database fields
	Public Const GEMMaxDBText As Integer = 70
	
	' Polaris Return Values
	Public Const GEMPolOK As Integer = 0
	
	'' Old Polaris Data Types
	'Global Const GEMPolList = 1
	'Global Const GEMPolDate = 2
	'Global Const GEMPolNumeric = 3
	'Global Const GEMPolText = 4
	
	' New Polaris Data Types
	Public Const GEMPolUnknown As Integer = 0
	Public Const GEMPolDate As Integer = 1
	Public Const GEMPolNumeric As Integer = 2
	Public Const GEMPolShortList As Integer = 3
	Public Const GEMPolLongList As Integer = 4
	Public Const GEMPolText As Integer = 5
	Public Const GEMPolNumeric2 As Integer = 6
	Public Const GEMPolRef As Integer = 9
	
	'Gemini Quotation Return Codes
	Public Const GemQuoteTrue As Integer = 1
	Public Const GemQuoteRefer As Integer = 2
	Public Const GemQuoteDecline As Integer = 3
	Public Const GemQuoteError As Integer = 4
	Public Const GemQuoteReferContinue As Integer = 5
	Public Const GemQuoteDeclineContinue As Integer = 6
	Public Const GemQuoteCalculationEnded As Integer = 7
	
	
	Public Const GEMPolMaxStringLen As Integer = 256
	
	' Column Definitions for User Field Array
	Public Const GEMUserFieldColumns As Integer = 1 ' Number of columns in array
	Public Const GEMUserFieldName As Integer = 0
	Public Const GEMUserFieldValue As Integer = 1
	
	
	' Column Definitions for Screen Array
	Public Const GEMScreenColumns As Integer = 7 ' Number of columns in array
	Public Const GEMScreenTag As Integer = 0
	Public Const GEMScreenValue As Integer = 1
	Public Const GEMScreenUpperInstance As Integer = 2
	Public Const GEMScreenMiddleInstance As Integer = 3
	Public Const GEMScreenLowerInstance As Integer = 4
	Public Const GEMScreenLevelFourInstance As Integer = 5
	Public Const GEMScreenLevelFiveInstance As Integer = 6
	Public Const GEMScreenLevelSixInstance As Integer = 7
	'Global Const GEMScreenName = 5
	'Global Const GEMScreenNameIdx = 6
	
	
	' Constants for columns in screen control array
	Public Const GEMControlColumns As Integer = 10
	Public Const GEMControlOrder As Integer = 0
	Public Const GEMControlPropID As Integer = 1
	Public Const GEMControlName As Integer = 2
	Public Const GEMControlNameIdx As Integer = 3
	Public Const GEMControlLabel As Integer = 4
	Public Const GEMControlLabelIdx As Integer = 5
	Public Const GEMControlCaption As Integer = 6
	Public Const GEMControlAssumed As Integer = 7
	Public Const GEMControlType As Integer = 8
	Public Const GEMControlTag As Integer = 9
	Public Const GEMControlValue As Integer = 10
	
	
	' Column Definitions for Database Array
	Public Const GEMDbColumns As Integer = 8 ' Number of columns in array
	Public Const GEMDbTableName As Integer = 0
	Public Const GEMDbFieldName As Integer = 1
	Public Const GEMDbFieldValue As Integer = 2
	Public Const GEMDbUpperInstance As Integer = 3
	Public Const GEMDbMiddleInstance As Integer = 4
	Public Const GEMDbLowerInstance As Integer = 5
	Public Const GEMDbLevelFourInstance As Integer = 6
	Public Const GEMDbLevelFiveInstance As Integer = 7
	Public Const GEMDbLevelSixInstance As Integer = 8
	
	
	' Key Name Constants
	Public Const PMKeyNameComponentManager As String = "ComponentManager"
	Public Const PMKeyNameClientKey As String = "client_key"
	Public Const PMKeyNameClientCode As String = "client_code"
	Public Const PMKeyNameClientName As String = "client_name"
	Public Const PMKeyNamePolicyKey As String = "policy_key"
	Public Const PMKeyNamePolicyNo As String = "policy_no"
	Public Const PMKeyNamePolicyStatus As String = "policy_status"
	Public Const PMKeyNamePolicyFunction As String = "policy_function"
	Public Const PMKeyNamePolicyRecordStatus As String = "record_status"
	Public Const PMKeyNamePolicySchemeDetails As String = "scheme_details"
	Public Const PMKeyNamePolicyNavChooseStepResult As String = "navchoosestep_result" 'CL030399
	Public Const PMKeyNamePolicyFunctionCode As String = "policy_function_code" 'sj 10/03/99
	Public Const PMKeyNameThisInstance As String = "ThisInstance"
	Public Const PMKeyNameMaxInstance As String = "MaxInstance"
	Public Const PMKeyNameListManager As String = "ListManager" 'sj 15/02/98
	Public Const PMKeyNameScreenId As String = "SCREEN_ID"
	Public Const PMKeyNameScreenDocument As String = "SCREEN_DOCUMENT"
	Public Const PMKeyNameSchemeProperties As String = "PROFILE"
	Public Const PMKeyNameScreenType As String = "SCREEN_TYPE"
	Public Const PMKeyNameGis As String = "GIS"
	'ND 250500 - Re-added for new iFindClient screen
	Public Const PMKeyNameAgentCode As String = "agent_code"
	Public Const PMKeyNameAgentKey As String = "agent_key"
	
	'sj 28/10/98 - start
	Public Const PMKeyNameRenewalStatus As String = "Renewal_Status"
	Public Const PMKeyNameRenewalAction As String = "Renewal_Action"
	Public Const PMKeyNameQuoteType As String = "Quote_Type"
	Public Const PMKeyNameExistingPolicyKey As String = "Existing_Policy_Key"
	Public Const PMKeyNameInsurerPolicyKey As String = "Insurer_Policy_Key"
	'sj 28/10/98 - end
	
	'sj 06/08/99 - start
	' Navigator constants for diary system
	Public Const PMKeyNameDiaryActionCodeId As String = "diary_action_code_id"
	Public Const PMKeyNameActionNo As String = "action_no"
	Public Const PMKeyNameGISSchemeId As String = "scheme_id"
	Public Const PMKeyNameTransactionType As String = "transaction_type"
	Public Const PMKeyNameDescription As String = "description"
	
	'sj 06/08/99 - end
	
	'sj 20/09/99 - start
	Public Const PMKeyNameBusinessTypeId As String = "business_type_id"
	Public Const PMKeyNameDefaultRequired As String = "default_required"
	Public Const PMKeyNameDefaultPolicyId As String = "default_policy_id"
	'sj 20/09/99 - end
	
	'sj 27/09/99 - start
	Public Const PMKeyNamePartyCnt As String = "party_cnt"
	Public Const PMKeyNameInsuranceFolderCnt As String = "insurance_folder_cnt"
	Public Const PMKeyNameInsuranceFileCnt As String = "insurance_file_cnt"
	'sj 27/09/99 - end
	
	' Constants for FillCombo function in GeneralScreens
	Public Const GEMRefill As Boolean = True
	Public Const GEMNoRefill As Boolean = False
	
	' Gemini Date Formats
	Public Const GEMShortDate As String = "DD/MM/YYYY"
	Public Const GEMLongDate As String = "D MMMM YYYY"
	
	' Standard FieldNames for User fields table
	Public Const GEMUserRefCounter As String = "RefCounter"
	Public Const GEMUserHairBeauty As String = "HairBeauty"
	Public Const GEMUserBuildingCivil As String = "BuildingCivil"
	Public Const GEMUserIncurredLoss As String = "IncurredLoss"
	Public Const GEMUserComputerCover As String = "ComputerCover"
	Public Const GEMUserLicence As String = "Licence"
	Public Const GEMUserOpMedical As String = "OpMedical"
	Public Const GEMUserEntertainment As String = "Entertainment"
	Public Const GEMUserFreezer As String = "Freezer"
	Public Const GEMUserPolicyType As String = "PolicyType"
	
	Public Const GEMUserRulesReturnVal As String = "RulesReturnVal"
	
	'************************************************************
	' Processes (Roadmaps) ids
	'************************************************************
	Public Const GEMProcessFull As Integer = 1
	Public Const GEMProcessFullWithAccounts As Integer = 2
	Public Const GEMProcessReviewPolicy As Integer = 3
	Public Const GEMProcessMidTermAdjustments As Integer = 4
	Public Const GEMProcessPostQuote As Integer = 5
	Public Const GEMProcessPostQuoteWithAccounts As Integer = 6
	Public Const GEMProcessDefaultSetup As Integer = 9
	Public Const GEMProcessReviewPolicyWithAccounts As Integer = 50
	Public Const GEMProcessClaims As Integer = 60 'sj 19/10/98
	Public Const GEMProcessCompareOldWithNew As Integer = 70 'bb 27/10/98
	Public Const GEMProcessCancel As Integer = 80 'sj 21/01/99
	Public Const GEMProcessReinstate As Integer = 85 'sj 21/01/99
	
	Public Const GEMProcessSpecific As Integer = 101 ' start roadmap from select scheme
	Public Const GEMProcessSpecificWithAccounts As Integer = 102
	Public Const GEMProcessStartAtQuote As Integer = 111 ' start roadmap from quote display
	Public Const GEMProcessStartAtQuoteWithAccounts As Integer = 121
	
	Public Const GEMProcessFullCV As Integer = 3001
	Public Const GEMProcessFullWithAccountsCV As Integer = 3002
	Public Const GEMProcessReviewPolicyCV As Integer = 3003
	Public Const GEMProcessMidTermAdjustmentsCV As Integer = 3004
	Public Const GEMProcessPostQuoteCV As Integer = 3005
	Public Const GEMProcessPostQuoteWithAccountsCV As Integer = 3006
	Public Const GEMProcessDefaultSetupCV As Integer = 3009
	Public Const GEMProcessReviewPolicyWithAccountsCV As Integer = 3050
	
	Public Const GEMProcessSpecificCV As Integer = 3101 ' start roadmap from select scheme
	Public Const GEMProcessSpecificWithAccountsCV As Integer = 3102
	Public Const GEMProcessStartAtQuoteCV As Integer = 3111 ' start roadmap from quote display
	Public Const GEMProcessStartAtQuoteWithAccountsCV As Integer = 3121
	
	Public Const GEMProcessRenewalRevisedFullCV As Integer = 5001 'sj 02/11/98
	'sj 08/12/98 - start
	Public Const GEMProcessRenewalReviewPolicyCV As Integer = 5002
	'Global Const GEMProcessRenewalRevisedFullCV = 6303 'sj 02/03/99
	'Global Const GEMProcessRenewalReviewPolicyCV = 6302
	Public Const GEMProcessRenewalReviewPolicy As Integer = 5003
	Public Const GEMProcessRenewalRevisedFull As Integer = 5004
	'sj 08/12/98 -end
	
	' RDC 24/06/99 Start
	' Marine Vessel
	Public Const GEMProcessFullMV As Integer = 4001
	Public Const GEMProcessFullWithAccountsMV As Integer = 4002
	Public Const GEMProcessReviewPolicymV As Integer = 4003
	Public Const GEMProcessMidTermAdjustmentsMV As Integer = 4004
	Public Const GEMProcessPostQuoteMV As Integer = 4005
	Public Const GEMProcessPostQuoteWithAccountsMV As Integer = 4006
	Public Const GEMProcessDefaultSetupMV As Integer = 4009
	Public Const GEMProcessReviewPolicyWithAccountsMV As Integer = 4050
	
	Public Const GEMProcessSpecificMV As Integer = 4101
	Public Const GEMProcessSpecificWithAccountsMV As Integer = 4102
	Public Const GEMProcessStartAtQuoteMV As Integer = 4111
	Public Const GEMProcessStartAtQuoteWithAccountsMV As Integer = 4121
	
	Public Const GEMProcessRenewalRevisedFullMV As Integer = 7001
	Public Const GEMProcessRenewalReviewPolicyMV As Integer = 7002
	' RDC 24/06/99 End
	
	Public Const GEMProcessRenewalOverride As Integer = 5040 'sj 29/01/99
	
	Public Const GEMProcessISTAM As Integer = 6011
	Public Const GEMProcessPostQuoteTAM As Integer = 6012
	Public Const GEMProcessISTAMAcc As Integer = 6021
	Public Const GEMProcessPostQuoteTAMAcc As Integer = 6022
	Public Const GEMProcessMTATAM As Integer = 6040
	Public Const GEMProcessSpecificTAM As Integer = 6101 ' start roadmap from select scheme
	Public Const GEMProcessSpecificWithAccountsTAM As Integer = 6102
	Public Const GEMProcessStartAtQuoteTAM As Integer = 6111 ' start roadmap from quote display
	Public Const GEMProcessStartAtQuoteWithAccountsTAM As Integer = 6121
	
	'sj 10/03/99 - start
	'************************************************************
	' Process codes for Navigator maps
	' There should be a one to one relationship with the process ids
	'************************************************************
	Public Const GEMProcessCodeFull As String = "G_CP_NB_Q"
	Public Const GEMProcessCodeFullWithAccounts As String = "G_CP_NB_QL"
	Public Const GEMProcessCodeReviewPolicy As String = "G_CP_REVP"
	Public Const GEMProcessCodeMidTermAdjustments As String = "G_CP_MTA"
	Public Const GEMProcessCodeMidTermAdjustmentsWithAccounts As String = "G_CP_MTAL" 'DN 14/06/99
	Public Const GEMProcessCodePostQuote As String = "G_CP_SPQ"
	Public Const GEMProcessCodePostQuoteWithAccounts As String = "G_CP_SPQL"
	Public Const GEMProcessCodeDefaultSetup As String = "G_CP_DEF"
	Public Const GEMProcessCodeReviewPolicyWithAccounts As String = "G_CP_REVL"
	Public Const GEMProcessCodeClaims As String = "G_CP_CLM"
	Public Const GEMProcessCodeCompareOldWithNew As String = "G_CP_OTN"
	Public Const GEMProcessCodeCancel As String = "G_CP_CAN"
	Public Const GEMProcessCodeReinstate As String = "G_CP_REN"
	Public Const GEMProcessCodeSpecific As String = "G_CP_IS_S" ' start roadmap from select scheme
	Public Const GEMProcessCodeSpecificWithAccounts As String = "G_CP_IS_SL"
	Public Const GEMProcessCodeStartAtQuote As String = "G_CP_SQD" ' start roadmap from quote display
	Public Const GEMProcessCodeStartAtQuoteWithAccounts As String = "G_CP_SQDL"
	
	Public Const GEMProcessCodeFullCV As String = "G_CV_Q"
	Public Const GEMProcessCodeFullWithAccountsCV As String = "G_CV_QL"
	Public Const GEMProcessCodeReviewPolicyCV As String = "G_CV_REVP"
	Public Const GEMProcessCodeMidTermAdjustmentsCV As String = "G_CV_MTA"
	Public Const GEMProcessCodeMidTermAdjustmentsCVWithAccounts As String = "G_CV_MTAL" 'DN 14/06/99
	Public Const GEMProcessCodePostQuoteCV As String = "G_CV_SPQ"
	Public Const GEMProcessCodePostQuoteWithAccountsCV As String = "G_CV_SPQL"
	Public Const GEMProcessCodeDefaultSetupCV As String = "G_CV_DEF"
	Public Const GEMProcessCodeReviewPolicyWithAccountsCV As String = "G_CV_REVL"
	
	Public Const GEMProcessCodeSpecificCV As String = "G_CV_SS" ' start roadmap from select scheme
	Public Const GEMProcessCodeSpecificWithAccountsCV As String = "G_CV_SSL"
	Public Const GEMProcessCodeStartAtQuoteCV As String = "G_CV_SQD" ' start roadmap from quote display
	Public Const GEMProcessCodeStartAtQuoteWithAccountsCV As String = "G_CV_SQDL"
	
	Public Const GEMProcessCodeRenewalRevisedFullCV As String = "G_CV_RRF"
	Public Const GEMProcessCodeRenewalReviewPolicyCV As String = "G_CV_RREVP"
	
	' RDC 24/06/99 Start
	' Constants for Marine Vessel
	Public Const GEMProcessCodeFullMV As String = "G_HKJ_NB_Q"
	Public Const GEMProcessCodeFullWithAccountsMV As String = "G_HKJ_QL"
	Public Const GEMProcessCodeReviewPolicyMV As String = "G_HKJ_REVP"
	Public Const GEMProcessCodeMidTermAdjustmentsMV As String = "G_HKJ_MTA"
	Public Const GEMProcessCodeMidTermAdjustmentsMVWithAccounts As String = "G_HKJ_MTAL"
	Public Const GEMProcessCodePostQuoteMV As String = "G_HKJ_SPQ"
	Public Const GEMProcessCodePostQuoteWithAccountsMV As String = "G_HKJ_SPQL"
	Public Const GEMProcessCodeDefaultSetupMV As String = "G_HKJ_DEF"
	Public Const GEMProcessCodeReviewPolicyWithAccountsMV As String = "G_HKJ_REVL"
	
	Public Const GEMProcessCodeSpecificMV As String = "G_HKJ_SS" ' start roadmap from select scheme
	Public Const GEMProcessCodeSpecificWithAccountsMV As String = "G_HKJ_SSL"
	Public Const GEMProcessCodeStartAtQuoteMV As String = "G_HKJ_SQD" ' start roadmap from quote display
	Public Const GEMProcessCodeStartAtQuoteWithAccountsMV As String = "G_HKJ_SQDL"
	
	Public Const GEMProcessCodeRenewalRevisedFullMV As String = "G_HKJ_RRF"
	Public Const GEMProcessCodeRenewalReviewPolicyMV As String = "G_HKJ_RREVP"
	' RDC 24/06/99 End
	
	Public Const GEMProcessCodeRenewalReviewPolicy As String = "G_CP_RREVP"
	Public Const GEMProcessCodeRenewalRevisedFull As String = "G_CP_RRF"
	Public Const GEMProcessCodeRenewalOverride As String = "G_CP_ROVR"
	
	Public Const GEMProcessCodeISTAM As String = "G_LB_IS"
	Public Const GEMProcessCodePostQuoteTAM As String = "G_LB_PQ"
	Public Const GEMProcessCodeISTAMAcc As String = "G_LB_ISL"
	Public Const GEMProcessCodePostQuoteTAMAcc As String = "G_LB_PQL"
	Public Const GEMProcessCodeMTATAM As String = "G_LB_MTA"
	Public Const GEMProcessCodeMTATAMWithAccounts As String = "G_LB_MTAL" 'DN 14/06/99
	Public Const GEMProcessCodeSpecificTAM As String = "G_LB_SS" ' start roadmap from select scheme
	Public Const GEMProcessCodeSpecificWithAccountsTAM As String = "G_LB_SSL"
	Public Const GEMProcessCodeStartAtQuoteTAM As String = "G_LB_SQD" ' start roadmap from quote display
	Public Const GEMProcessCodeStartAtQuoteWithAccountsTAM As String = "G_LB_SQDL"
	
	Public Const GEMProcessCodeFullCC As String = "G_CC_Q" 'ak 10/06/99
	Public Const GEMProcessCodeFullWithAccountsCC As String = "G_CC_QL" 'ak 10/06/99
	Public Const GEMProcessCodeReviewPolicyCC As String = "G_CC_REVP" 'ak 10/06/99
	Public Const GEMProcessCodeMidTermAdjustmentsCC As String = "G_CC_MTA" 'ak 10/06/99
	Public Const GEMProcessCodeMidTermAdjustmentsCCWithAccounts As String = "G_CC_MTAL" 'DN 14/06/99
	Public Const GEMProcessCodePostQuoteCC As String = "G_CC_SPQ" 'ak 10/06/99
	Public Const GEMProcessCodePostQuoteWithAccountsCC As String = "G_CC_SPQL" 'ak 10/06/99
	Public Const GEMProcessCodeDefaultSetupCC As String = "G_CC_DEF" 'ak 10/06/99
	Public Const GEMProcessCodeReviewPolicyWithAccountsCC As String = "G_CC_REVL" 'ak 10/06/99
	Public Const GEMProcessCodeSpecificCC As String = "G_CC_SS" 'ak 10/06/99
	Public Const GEMProcessCodeSpecificWithAccountsCC As String = "G_CC_SSL" 'ak 10/06/99
	Public Const GEMProcessCodeStartAtQuoteCC As String = "G_CC_SQD" 'ak 10/06/99
	Public Const GEMProcessCodeStartAtQuoteWithAccountsCC As String = "G_CC_SQDL" 'ak 10/06/99
	' ND 190600 New constant for cancellation road map
	Public Const GEMProcessCodeCancelWithAccounts As String = "G_CP_CANL"
	'sj 10/11/99 - end
	
	' Polaris Refences
	Public Const GEMPolActivityRef As Integer = 1
	Public Const GEMPolCoverRef As Integer = 2
	Public Const GEMPolSubCoverRef As Integer = 3
	Public Const GEMPolInsuredObjectRef As Integer = 4
	Public Const GEMPolInsuredSubObjectRef As Integer = 5
	Public Const GEMPolPartyRef As Integer = 6
	
	' Polaris document calls
	Public Const GEMELCertificate As Integer = 29
	Public Const GEMFreeFormat As Integer = 27
	Public Const GEMProposalForm As Integer = 22
	Public Const GEMScheduleForm As Integer = 23
	Public Const GEMMatFactDiscForm As Integer = 24
	Public Const GEMStatementRiskForm As Integer = 25
	Public Const GEMPaymentArrangeForm As Integer = 26
	Public Const GEMQuotationLetter As Integer = 30 ' DN 27/04/99
	Public Const GEMHoldingCoveredLetter As Integer = 31
	Public Const GEMCertificateOfInsurance As Integer = 32
	Public Const GEMCoverNote As Integer = 33
	Public Const GEMCancellationSchedule As Integer = 34 ' DN 27/04/99
	
	' Polaris documents
	Public Const GEMQuotationLetterCode As String = "O" ' DN 27/04/99
	Public Const GEMHoldingCoveredLetterCode As String = "P"
	Public Const GEMCertificateOfInsuranceCode As String = "Q"
	Public Const GEMCoverNoteCode As String = "R"
	Public Const GEMCancellationScheduleCode As String = "S" ' DN 27/04/99
	Public Const GEMELCertificateCode As String = "T"
	Public Const GEMFreeFormatCode As String = "U"
	Public Const GEMProposalFormCode As String = "V"
	Public Const GEMScheduleFormCode As String = "W"
	Public Const GEMMatFactDiscFormCode As String = "X"
	Public Const GEMStatementRiskFormCode As String = "Y"
	Public Const GEMPaymentArrangeFormCode As String = "Z"
	
	Public Const GEMDefProposalFormCode As String = "A" ' CL150199
	Public Const GEMEndorsementFormCode As String = "B" ' CL150199
	
	' Polaris Id's
	Public Const GEMCoverAllowedMax As Integer = 75
	Public Const GEMCoverAllowed As Integer = 82
	Public Const GEMCAInputCoverRef As Integer = 5373972
	Public Const GEMCAOutputCoverRef As Integer = 5373973
	Public Const GEMCoverAllowedBreakdownMax As Integer = 100
	Public Const GEMCoverAllowedBreakdown As Integer = 83
	Public Const GEMCABInputSubCoverRef As Integer = 5439526
	Public Const GEMCABOutputSubCoverRef As Integer = 5439527
	
	Public Const GEMPremisesRatingPostcode As Integer = 655375
	Public Const GEMTradeCode As Integer = 917505
	Public Const GEMRiskEventCommencementDate As Integer = 852000
	
	' Premium Quote Breakdown Object Constants
	'Public Const PQB = 84
	'Public Const PQB_AMT = 5505025
	'Public Const PQB_DESCRIPTION = 5505028
	'Public Const PQB_COVER_ALLOWED_RSN = 5505027
	'Public Const PQB_COVER_ALLOWED_BREAKDOWN_RSN = 5505032
	'Public Const PQB_RUNNING_TOTAL = 5505033
	'Public Const PQB_INSURED_OBJECT_REF = 5505040
	'Public Const PQB_CODE_VALUE = 5505036
	'Public Const PQB_DISPLAY_FLAG = 5505044
	'Public Const PQB_RSN = 5505029
	'Public Const PQB_INSURED_OBJECT_TYPE_VALUE = 0
	'Public Const PQB_PARTY_REF = 0
	'Public Const PQB_TYPE_CODE_VALUE = 0
	'Public Const PQB_CA_ACTIVITY_REF = 0
	'Public Const PQB_CA_COVER_CODE_VALUE = 0
	'Public Const PQB_CA_INSURED_OBJECT_CATEGORY_COVERED_VALUE = 0
	'Public Const PQB_CA_INSURED_OBJECT_TYPE_VALUE = 0
	'Public Const PQB_CA_PARTY_REF = 0
	'Public Const PQB_CA_PARTY_TYPE_COVERED_CODE_VALUE = 0
	'Public Const PQB_CAB_ACTIVITY_REF = 0
	'Public Const PQB_CAB_COVER_BREAKDOWN_CODE_VALUE = 0
	'Public Const PQB_CAB_INSURED_SUB_OBJECT_REF = 0
	'Public Const PQB_CAB_INSURED_SUB_OBJECT_TYPE_VALUE = 0
	'Public Const PQB_CAB_PARTY_REF = 0
	'Public Const PQB_CAB_PARTY_TYPE_COVERED_CODE_VALUE = 0
	
	
	' Misc Premium Quote Breakdown
	'Public Const MPQB = 101
	'Public Const MPQB_CODE = 6619138
	
	'Public Const PQB_DISPLAY_FLAG =
	
	' Cover Allowed Object Constants
	'Public Const CA = 82
	'Public Const CA_RSN = 5373958
	'Public Const CA_BASIS_CODE = 5373954
	'Public Const CA_COVER_CODE_VALUE = 5373955
	'Public Const CA_DESCRIPTION = 5373956
	'Public Const CA_EXCLUDED_FROM_COVER_VALUE_IND = 5373954
	'Public Const CA_INSURED_OBJECT_REF = 5373960
	'Public Const CA_ACTIVITY_REF = 5373953
	'Public Const CA_INSURED_OBJECT_CATEGORY_COVERED_VALUE = 53739
	'Public Const CA_INSURED_OBJECT_TYPE_VALUE = 53739
	'Public Const CA_PARTY_REF = 53739
	'Public Const CA_PARTY_TYPE_COVERED_CODE_VALUE = 53739
	
	' Cover Allowed Breakdown Object Constants
	'Public Const CAB = 83
	'Public Const CAB_RSN = 5439504
	'Public Const CAB_AMT = 5439490
	'Public Const CAB_DESCRIPTION = 5439525
	'Public Const CAB_APPLICATION_CODE_VALUE = 5439491
	'Public Const CAB_COVER_AMT_TYPE_CODE_VALUE = 5439492
	'Public Const CAB_EXCESS_TYPE_CODE_VALUE = 5439498
	'Public Const CAB_LOSS_CODE_VALUE = 5439503
	'Public Const CAB_INSURED_SUB_OBJECT_REF = 5439515
	'Public Const CAB_COVER_BREAKDOWN_CODE_VALUE = 5439523
	'Public Const CAB_COVER_ALLOWED_RSN = 5439511
	'Public Const CAB_ACTIVITY_REF = 0
	'Public Const CAB_CA_ACTIVITY_REF = 0
	'Public Const CAB_CA_COVER_CODE_VALUE = 0
	'Public Const CAB_CA_INSURED_OBJECT_CATEGORY_COVERED_VALUE = 0
	'Public Const CAB_CA_INSURED_OBJECT_TYPE_VALUE = 0
	'Public Const CAB_CA_PARTY_REF = 0
	'Public Const CAB_CA_PARTY_TYPE_COVERED_CODE_VALUE = 0
	'Public Const CAB_INSURED_SUB_OBJECT_TYPE_VALUE = 0
	'Public Const CAB_PARTY_REF = 0
	'Public Const CAB_PARTY_TYPE_COVERED_CODE_VALUE = 0
	
	
	
	' Misc Cover Allowed Breakdown Object Constants
	'Public Const MCAB = 100
	'Public Const MCAB_COVER_AMT_TYPE = 6553605
	'Public Const MCAB_EXCESS_TYPE_CODE = 6553615
	'Public Const MCAB_COVER_BREAKDOWN_CODE = 6553607
	
	' Calculated Result Object Constants
	'Public Const CR = 80
	'Public Const CR_AMT = 5242881
	'Public Const CR_BASED_ON_AMT = 5242882
	'Public Const CR_DESCRIPTION = 5242884
	'Public Const CR_PCT = 5242886
	'Public Const CR_RSN = 5242887
	'Public Const CR_RUNNING_TOTAL = 5242888
	'Public Const CR_CODE_VALUE = 5242889
	
	' Misc Calculated Result Object Constants
	'Public Const MCR = 97
	'Public Const MCR_CODE = 6356995
	
	
	' Quotation Condition Object Constants
	'Public Const QC = 12
	'Public Const QC_DESCRIPTION = 786435
	'Public Const QC_CODE_VALUE = 786440
	'Public Const QC_INSURED_OBJECT_TYPE_VALUE = 786441
	'Public Const QC_INSURED_OBJECT_REF = 786437
	'Public Const QC_RSN = 786436
	
	' Misc Quotation Condition
	'Public Const MQC = 102
	'Public Const MQC_CODE = 6684673
	
	' Premises Cover Breakdown Object Constants
	'Public Const PCB = 56
	'Public Const PCB_CODE = 3670017
	
	' Premises Object Constants
	'Public Const PREMISES = 10
	'Public Const PREMISES_INSURED_OBJECT_REF = 655376
	'Public Const PREMISES_DESCRIPTION = 655366
	
	
	' Constants for popup menu status on screens
	Public Const GEMMenuCancel As Integer = 0
	Public Const GEMMenuForm As Integer = 1
	Public Const GEMMenuPropForm As Integer = 2
	Public Const GEMMenuCoverNote As Integer = 3 'DN 24/05/99
	Public Const GEMMenuCopyInstance As Integer = 5
	Public Const GEMMenuPasteInstance As Integer = 6
	Public Const GEMMenuExit As Integer = 9
	
	
	' Constants for assumed info

	Public GEMColorAssumed As Color = Color.Blue

	Public GEMColorNotAssumed As Color = SystemColors.WindowText
	Public Const GEMAssumedFalse As Integer = 0
	Public Const GEMAssumedTrue As Integer = 1
	Public Const GEMAssumedToggle As Integer = 2
	
	' Constants for policy status
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
	'sj 15/01/99 - start
	Public Const GEMPolicyCancelPending As Integer = 48
	Public Const GEMPolicyCancelled As Integer = 50
	Public Const GEMPolicyExpired As Integer = 55
	Public Const GEMPolicyLapsed As Integer = 60
	Public Const GEMPolicyCoverDefault As Integer = 700 'IB
	'sj 15/01/99 - end
	' sj 02/09/98 - New fields for renewal
	
	' Renewal Audit Types
	Public Const GEMRenewalAuditAuto As Integer = 10
	Public Const GEMRenewalAuditManual As Integer = 20
	
	' Parent App Ids
	Public Const GEMRenewalSelection As Integer = 10
	Public Const GEMRenewalQuote As Integer = 20
	Public Const GEMRenewalInvite As Integer = 30
	Public Const GEMRenewalConfirm As Integer = 40
	Public Const GEMRenewalComplete As Integer = 50
	Public Const GEMRenewalLapse As Integer = 60
	Public Const GEMRenewalMTA As Integer = 70
	Public Const GEMRenewalHousekeeping As Integer = 80
	Public Const GEMRenewalClaim As Integer = 90
	Public Const GEMRenewalWorkManager As Integer = 100
	'sj 15/01/99 - start
	'' Policy statuses
	'Global Const GEMPolicyExpired = 50
	'Global Const GEMPolicyLapsed = 60
	'sj 15/01/99 - end
	
	' Renewal policy statuses
	Public Const GEMRenewalStatusNone As Integer = 0
	Public Const GEMRenewalStatusCreated As Integer = 2010
	Public Const GEMRenewalStatusQuoted As Integer = 2020
	Public Const GEMRenewalStatusReferred As Integer = 2025
	Public Const GEMRenewalStatusDecline As Integer = 2026
	Public Const GEMRenewalStatusInvited As Integer = 2030
	Public Const GEMRenewalStatusConfirmed As Integer = 2040
	Public Const GEMRenewalStatusRenewed As Integer = 2050
	' ND 060700 Status for move to accounts
	Public Const GEMRenewalStatusMovedToAccounts As Integer = 2055
	Public Const GEMRenewalStatusLapsed As Integer = 2060
	
	'
	'Renewal Status Text
	Public Const GEMRenewalStatusTxtCreatedDeferred As String = "Policy deferred at renewal"
	Public Const GEMRenewalStatusTxtCreated As String = "Policy Created"
	Public Const GEMRenewalStatusTxtQuoted As String = "Policy Quoted"
	Public Const GEMRenewalStatusTxtQuotedMaxParam As String = "Policy quoted - exceeds maximum rate increase"
	Public Const GEMRenewalStatusTxtQuotedMinParam As String = "Policy quoted - exceeds maximum rate decrease"
	Public Const GEMRenewalStatusTxtDeclined As String = "Renewal quotation declined"
	Public Const GEMRenewalStatusTxtReferred As String = "Renewal quotation referred"
	Public Const GEMRenewalStatusTxtInvited As String = "Renewal invited"
	Public Const GEMRenewalStatusTxtConfirmed As String = "Renewal confirmed"
	Public Const GEMRenewalStatusTxtRenewed As String = "Policy renewed"
	Public Const GEMRenewalStatusTxtMoveToAccounts As String = "Policy renewal moved to accounts"
	Public Const GEMRenewalStatusTxtLapsed As String = "Policy lapsed"
	Public Const GEMRenewalStatusTxtMTAReceived As String = "Policy details reset due to MTA"
	Public Const GEMRenewalStatusTxtClaimReceived As String = "Policy details reset due to Claim"
	Public Const GEMRenewalStatusTxtLapseAtRenewal As String = "Policy to be lapsed at renewal"
	Public Const GEMRenewalStatusTxtSuspend As String = "Policy suspended"
	Public Const GEMRenewalStatusTxtUnSuspend As String = "Policy suspension removed    "
	Public Const GEMRenewalStatusTxtUnLapseAtRenewal As String = "Policy not to be lapsed at renewal"
	Public Const GEMRenewalStatusTxtInvDocReprint As String = "Invitation docs reprinted"
	Public Const GEMRenewalStatusTxtAlternateQuote As String = "Rebroking quote generated"
	Public Const GEMRenewalStatusTxtReQuote As String = "Revised quote generated"
	Public Const GEMRenewalStatusTxtRevisedSelected As String = "Revised quote selected"
	Public Const GEMRenewalStatusTxtInsurerSelected As String = "Insurer quote selected"
	
	'Renewal Action Ids
	Public Const GEMRenewalActionNone As Integer = 1
	Public Const GEMRenewalActionSuspendDeferred As Integer = 10
	Public Const GEMRenewalActionSuspendRange As Integer = 15
	Public Const GEMRenewalActionSuspendManual As Integer = 20
	Public Const GEMRenewalActionLapseAuto As Integer = 30
	Public Const GEMRenewalActionLapseManual As Integer = 40
	Public Const GEMRenewalActionLapseAltQuote As Integer = 45
	Public Const GEMRenewalActionCancel As Integer = 50
	
	'Renewal Audit Ids
	Public Const GEMRenewalAuditTypeManual As Integer = 10
	Public Const GEMRenewalAuditTypeAutomatic As Integer = 20
	
	'Command Line Flags
	Public Const GEMRenewalFlagSelect As String = "/s"
	Public Const GEMRenewalFlagQuote As String = "/q"
	Public Const GEMRenewalFlagInvite As String = "/i"
	Public Const GEMRenewalFlagConfirm As String = "/c"
	Public Const GEMRenewalFlagComplete As String = "/r"
	Public Const GEMRenewalFlagLapse As String = "/l"
	Public Const GEMRenewalFlagHousekeeping As String = "/h"
	Public Const GEMRenewalFlagUserId As String = "/u"
	
	'Initial Status
	Public Const GEMRenewalInitialStatus As Integer = 500
	Public Const GEMRenewalAlternateQuote As Integer = 520
	Public Const GEMRenewalRequote As Integer = 550
	
	'User Field Constants
	Public Const GEMUserFieldRenewalQuote As String = "Alternate_Quote"
	Public Const GEMUserFieldDatabaseUpdated As String = "Database_Updated"
	Public Const GEMUserFieldPremiumOverride As String = "Premium_Override"
	
	'sj 23/09/98 - end
	
	'sj 15/01/99 - start
	'Cancellation statuses
	Public Const GEMCancelCodeCreated As Integer = 1
	Public Const GEMCancelCodeReferred As Integer = 2
	Public Const GEMCancelCodeQuoted As Integer = 3
	Public Const GEMCancelCodeDeclined As Integer = 4
	Public Const GEMCancelCodeCompleted As Integer = 5
	
	Public Const GEMCancelTextCreated As String = "Created"
	Public Const GEMCancelTextReferred As String = "Referred"
	Public Const GEMCancelTextDeclined As String = "Declined"
	Public Const GEMCancelTextQuoted As String = "Quoted"
	'sj 15/01/99 - end
	
	' Gemini Schemes Variant array
	Public Const PMInsurerNo As Integer = 0
	Public Const PMSchemeNo As Integer = 1
	Public Const PMSchemeVer As Integer = 2
	Public Const PMSchemeDesc As Integer = 3
	Public Const PMActivationLevel As Integer = 4
	Public Const PMPrintPrivilege As Integer = 5
	Public Const PMBrokerGroup As Integer = 6
	Public Const PMCommissionPerc As Integer = 7
	Public Const PMAppId As Integer = 8
	Public Const PMAppVer As Integer = 9
	Public Const PMModuleCollection As Integer = 10 '(IB)110699
	
	' modules ticked states (IB)150699
	Public Const PMModulesClear As Integer = 0
	Public Const PMModulesSomeTicked As Integer = 1
	Public Const PMModulesAllTicked As Integer = 2
	Public Const PMCoverClear As Integer = 0
	Public Const PMCoverTicked As Integer = 1
	
	
	' ??????Gemini Quote Breakdown Variant array??????
	' ****Must Investigate****
	Public Const PMPremium As Integer = 3
	
	' Gemini Quote Breakdown Variant array
	Public Const PMResult As Integer = 0
	Public Const PMPremiums As Integer = 1
	Public Const PMExcesses As Integer = 2
	Public Const PMSumIns As Integer = 3
	Public Const PMTerms As Integer = 4
	Public Const PMLimits As Integer = 5
	Public Const PMBenefits As Integer = 6
	Public Const PMFranchises As Integer = 7
	Public Const PMEndorsements As Integer = 8
	Public Const PMLongEndorsements As Integer = 9
	
	
	' Column definitions for Authorisation Data
	Public Const GEMAuthDesc As Integer = 0
	Public Const GEMAuthOldVal As Integer = 1
	Public Const GEMAuthNewVal As Integer = 2
	Public Const GEMAuthProperty As Integer = 3
	Public Const GEMAuthInstance As Integer = 4
	Public Const GEMAuthTableName As Integer = 5
	Public Const GEMAuthFieldName As Integer = 6
	Public Const GEMAuthFlag As Integer = 7
	Public Const GEMAuthObjRef As Integer = 8 'DN 08/02/00
	
	' Constants for business Type
	Public Const GemBusinessTypeCP As Integer = 0
	Public Const GemBusinessTypeCV As Integer = 1
	Public Const GemBusinessTypeCC As Integer = 2
	Public Const GemBusinessTypeMV As Integer = 3
	
	' Constants for Gemini Policy Locking
	
	Public Const GEMPolicyLock As String = "GeminiPolicyLock"
	
	
	'MN220298 - Get All scheme details array
	
	Public Const GEMAllInsurerNo As Integer = 0
	Public Const GEMAllInsurerDesc As Integer = 1
	Public Const GEMAllSchemeNo As Integer = 2
	Public Const GEMAllSchemeVer As Integer = 3
	Public Const GEMAllSchemeDesc As Integer = 4
	Public Const GEMAllAgencyCode As Integer = 5
	Public Const GEMAllProductCode As Integer = 6
	Public Const GEMAllActivationLevel As Integer = 7
	Public Const GEMAllPrintingPrivilege As Integer = 8
	Public Const GEMAllBrokerGroup As Integer = 9
	Public Const GEMAllCommissionPerc As Integer = 10
	Public Const GEMAllSelectionDayNum As Integer = 11
	Public Const GEMAllMinChangeNum As Integer = 12
	Public Const GEMAllMaxChangeNum As Integer = 13
	Public Const GEMAllQuoteDayNum As Integer = 14
	Public Const GEMAllInviteDayNum As Integer = 15
	Public Const GEMAllLapseDayNum As Integer = 16
	Public Const GEMAllConfirmDayNum As Integer = 17
	Public Const GEMAllSchemeStatus As Integer = 18
	
	'sj 15/04/99 - start
	'sj Transaction Codes for Policy_life_cycle table
	Public Const GEMPolLifeNB As Integer = 1
	Public Const GEMPolLifeMTA As Integer = 2
	Public Const GEMPolLifeInvite As Integer = 3
	Public Const GEMPolLifeConfirm As Integer = 4
	Public Const GEMPolLifeComplete As Integer = 5
	Public Const GEMPolLifeLapse As Integer = 6
	'sj 15/04/99 - end
	
	'(IB)301199 - Agent select/change policy client constants
	Public Const GEMFindClientModeNormal As Integer = 0
	Public Const GEMFindClientModeSelectAgent As Integer = 1
	Public Const GEMFindClientModeSelectCurrentClient As Integer = 2
	Public Const GEMFindClientModeSelectCorrectClient As Integer = 3
	Public Const GEMFindPolicyModeMustExist As Integer = 4
	Public Const GEMFindClientModeSelectCorrectAgent As Integer = 5
	' RDC 07022000 HKJ copy policy process mode
	Public Const GEMFindPolicyModeHKJCopyPolicy As Integer = 6
	Public Const GEMFindPolicyModeHKJChangeStatus As Integer = 7
End Module