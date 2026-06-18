Option Strict Off
Option Explicit On
Module GISSharedConstants
	' ***************************************************************** '
	' Module Name: GISSharedConstants
	'
	' Date:  15/05/1999
	'
	' Description: Main Module.
	'
	' Edit History:
	'
	' 290200 CJB Added new constants for Dictionary Type.
	' 010300 CJB Added constants for Motor & Household dictionary.
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "GISSharedConstants"
	
	' ***************************************************************** '
	' General Constants
	' ***************************************************************** '
	' Registry subkey TB 24/2/00
	Public Const ACOIMGISSubKey As String = "GIS"
	
	' Data Types
	Public Const GISDataTypeUnknown As Short = 0
	Public Const GISDataTypeDate As Short = 1
	Public Const GISDataTypeNumeric As Short = 2
	Public Const GISDataTypeShortList As Short = 3
	Public Const GISDataTypeLongList As Short = 4
	Public Const GISDataTypeText As Short = 5
	Public Const GISDataTypeNumericOutput As Short = 6
	Public Const GISDataTypeShortListCode As Short = 103
	Public Const GISDataTypeLongListCode As Short = 104
	' RFC070900 - Added some more GIS data types as required by Underwriting
	' RFC070900 - All of these are effectively numeric values
	' RFC070900 - Leave a Gap between 6 and 20 incase Polaris decide to create
	'             a new data type, so we can use the same number as them (hopefully).
	Public Const GISDataTypeOption As Short = 20
	Public Const GISDataTypeCurrency As Short = 21
	Public Const GISDataTypePercentage As Short = 22
	
	
	' Quote Types
	Public Const GISNBQteSurveyPremOnly As Short = 1
	Public Const GISNBQteSurveyPremTerms As Short = 2
	Public Const GISNBQteSurveyFull As Short = 3
	Public Const GISNBQteSingleScheme As Short = 4
	
	' CL100400
	Public Const GISMTAQuoteTypeMTAQuote1 As Integer = 1 ' MTA Proper (quote 1)
	Public Const GISMTAQuoteTypeMTAQuote2 As Integer = 2 ' MTA Proper (quote 2)
	Public Const GISMTAQuoteTypeAddCancellation As Integer = -1 ' MTA Add Cancellation
	Public Const GISMTAQuoteTypeCancelCancellation As Integer = -2 ' MTA Cancel Cancellation
	
	
	' CL100800 - for use when initializing a MTA dataset via MTAStart() etc...
	Public Const GISMTAStartTypePerm As Integer = 1 ' MTA Permanent
	Public Const GISMTAStartTypeTemp As Integer = 2 ' MTA Temporary
	Public Const GISMTAStartTypeCancel As Integer = 3 ' MTA Cancellation
	Public Const GISMTAStartTypeReinstate As Integer = 4 ' MTA Reinstatement
	
	' DD181001 - Renewal Quote Types
	Public Const GISRenQuote As Short = 99
	
	' Scheme Quote Status
	Public Const GISQteStatusUnknown As Short = 0
	Public Const GISQteStatusDecline As Short = 1
	Public Const GISQteStatusReferNoQuote As Short = 2
	Public Const GISQteStatusReferWithQuote As Short = 3
	Public Const GISQteStatusQuote As Short = 4
	
	' Dictionary Types
	Public Const GISDictionaryHome As String = "Home" 'CB 290200
	Public Const GISDictionaryMotor As String = "Motor" 'CB 290200
	
	' Registry Sub Key for GIS Settings
	Public Const GISRegSubKey As String = "GIS"
	Public Const GISRegDataSetPath As String = "DataSetsPath"
	Public Const GISRegEDISendPath As String = "EDISendPath"
	Public Const GISRegGISInDebug As String = "GISInDebug"
	Public Const GISRegHTTPPostToPage As String = "HTTPPostToPage"
	Public Const GISRegExternalDTDPath As String = "ExternalDTDPath" ' CL160200
	Public Const GISRegDSN As String = "DSN" ' RFC300300
	Public Const GISRegSaveOnQuote As String = "SaveOnQuote" ' RFC300300
	Public Const GISRegSaveOnMTAQuote As String = "SaveOnMTAQuote" ' SJ120401
	Public Const GISRegAuditOnQuote As String = "AuditOnQuote" ' RFC300300
	Public Const GISRegLookupPath As String = "LookupsPath"
	Public Const GISRegDCHostName As String = "DC_Hostname" ' CL190500 (home)
	Public Const GISRegDCHostPort As String = "DC_Hostport" ' CL190500 (home)
	Public Const GISRegDCEncKey As String = "DC_EncKey" ' CL190500 (home)
	Public Const GISRegDCMerchantID As String = "DC_MerchantID" ' CL190500 (home)
	Public Const GISRegDCMerchantPwd As String = "DC_MerchantPwd" ' CL190500 (home)
	Public Const GISRegDCLogfile As String = "DC_Logfile" ' CL190500 (home)
	Public Const GISRegDCTimeoutSecs As String = "DC_TimeoutSecs" ' CL190500 (home)
	Public Const GISRegTPAMTAEmailRequired As String = "TPAMTAEmailRequired" 'RAG210600
	Public Const GISRegTPAEmailAddress As String = "TPAEmailAddress" 'RAG210600
	Public Const GISRegPartySourceID As String = "PartySourceID" 'PWF010701
	Public Const GISRegPolicySourceID As String = "PolicySourceID" 'PH06082001
	Public Const GISRegPromptErrorsEmailAddress As String = "PromptErrorsEmailAddress" 'CJB171100
	Public Const GISRegPromptErrorsEmailFrom As String = "PromptErrorsEmailFrom" 'CJB171100
	Public Const GISRegErrorsEmailAddress As String = "ErrorsEmailAddress" 'CJB020801
	Public Const GISRegErrorsEmailFrom As String = "ErrorsEmailFrom" 'CJB020801
	Public Const GISRegInformationEmailAddress As String = "InformationEmailAddress" 'CJB040801
	Public Const GISRegInformationEmailFrom As String = "InformationEmailFrom" 'CJB040801
	Public Const GISRegGeminiNetQuote As String = "GeminiNetQuote" 'CJB260301
	Public Const GISQEMMethodsVersionNum As String = "QEMMethodsVersionNum" 'RFC100700
	Public Const GISRegRegistrationEmailRequired As String = "RegistrationEmailRequired" ' RG080600
	Public Const GISRegRegistrationEmailFrom As String = "RegistrationEmailFrom" 'RFC010800
	Public Const GISRegRegistrationEmailFromLycos As String = "RegistrationEmailFromLycos" 'GW270701
	Public Const GISRegRegistrationEmailSubject As String = "RegistrationEmailSubject" 'RFC010800
	Public Const GISRegNBSOFEmailRequired As String = "NBSOFEmailRequired" ' CL050600 'RFC010800
	Public Const GISRegNBSOFEmailFrom As String = "NBSOFEmailFrom" 'RFC010800
	Public Const GISRegNBSOFEmailFromLycos As String = "NBSOFEmailFromLycos" 'GW270701
	Public Const GISRegNBSOFEmailSubject As String = "NBSOFEmailSubject" 'RFC010800
	Public Const GISRegI4mEmail As String = "I4mEmail" 'NC080900
	Public Const GISRegI4mStaffLoadingPct As String = "I4mStaffLoadingPct" 'GW080100
	Public Const GISRegMTASOFEmailRequired As String = "MTASOFEmailRequired" ' RFC010800
	Public Const GISRegMTASOFEmailFrom As String = "MTASOFEmailFrom" 'RFC010800
	Public Const GISRegMTASOFEmailFromLycos As String = "MTASOFEmailFromLycos" 'GW270701
	Public Const GISRegMTASOFEmailSubject As String = "MTASOFEmailSubject" 'RFC010800
	Public Const GISRegSubKeyEmails As String = "Emails" 'RFC010800
	Public Const GISRegReports As String = "Reports" 'RJG 31/07/2000
	Public Const GISRegFailedTransReportPath As String = "FailedTransReportPath" 'RJG 31/07/2000
	Public Const GISRegBackOfficeReportPath As String = "BackOfficeReportPath" 'RJG 31/07/2000
	Public Const GISRegNBSOFEmailFooterPath As String = "NBSOFEmailFooterPath" ' CL090800
	Public Const GISRegPromptInterfaceURL As String = "PromptInterfaceURL" ' RFC080900
	Public Const GISRegStateFilesPath As String = "StateFilesPath" ' RFC220900
	Public Const GISRegStateTimeoutMins As String = "StateTimeoutMins" ' CL260900
	Public Const GISRegBOMRequired As String = "BOMRequired"
	Public Const GISRegConsumerMTAEmailFrom As String = "ConsumerMTAEmailFrom"
	Public Const GISRegConsumerMTAEmailFromLycos As String = "ConsumerMTAEmailFromLycos" 'GW270701
	Public Const GISRegConsumerMTAEmailTo As String = "ConsumerMTAEmailTo"
	Public Const GISRegVehicleLookupURL As String = "VehicleLookupURL"
	' RFC150501
	Public Const GISRegLoadSaveDBMode As String = "LoadSaveDBMode"
	Public Const GISRegLoadSaveDBModeSlow As Integer = 1
	Public Const GISRegLoadSaveDBModeFast As Integer = 2
	Public Const GISRegLoadSaveDBModeFastWithQuotes As Integer = 3
	' RFC310701
	Public Const GISRegLoadSaveSafeInserts As String = "LoadSaveSafeInserts"
	' DD301101
	Public Const GISRegRenewalCustomerMenu As String = "RenewalCustomerMenu"
	'RT201299
	Public Const GISRegSaveToDBOnExit As String = "SaveToDBOnExit"
	Public Const GISRegSaveToDBNoPromptNoSave As String = "1"
	Public Const GISRegSaveToDBNoPromptSave As String = "2"
	Public Const GISRegSaveToDBPromptForSave As String = "3"
	'RJG 10/04/2001 - Reg Setting for LogLevel
	Public Const GISRegGISLogLevel As String = "GISLogLevel"
	'PF 26/10/01 - Policy Type and Product Code settings
	Public Const GISRegPolicyTypeCode As String = "PolicyTypeCode"
	Public Const GISRegProductCode As String = "ProductCode"
	'DJM 03/04/2002
	Public Const GISRegUseRiskTypeID As String = "UseRiskTypeID"
	
	'RT100599
	Public Const GISSaveToDBNoPromptNoSave As String = "0"
	Public Const GISSaveToDBNoPromptSave As String = "1"
	Public Const GISSaveToDBPromptForSave As String = "2"
	
	' Scheme Quote Types
	Public Const GISQtePolarisTypeNewBusiness As Short = 1
	Public Const GISQtePolarisTypeFullCycle As Short = 2
	
	' Polaris Quote Return Values
	Public Const GISPolarisQuoteOK As Short = 1
	Public Const GISPolarisQuoteReferA As Short = 2
	Public Const GISPolarisQuoteReferB As Short = 5
	Public Const GISPolarisQuoteDeclineA As Short = 3
	Public Const GISPolarisQuoteDeclineB As Short = 6
	
	' Data Set Actions
	Public Const GISDSActionNone As Short = 0
	Public Const GISDSActionNewDataSet As Short = 1
	Public Const GISDSActionQuote As Short = 2
	Public Const GISDSActionSaveToDB As Short = 3
	Public Const GISDSActionLoadFromDB As Short = 4
	Public Const GISDSActionSearchForAddress As Short = 5
	Public Const GISDSActionUpdateQtePassword As Short = 6
	Public Const GISDSActionNBTransact As Short = 7
	Public Const GISDSActionRefer As Short = 8
	Public Const GISDSActionMTAQuote As Short = 9
	Public Const GISDSActionMTATransact As Short = 10
	Public Const GISDSActionFinancialDatacash As Short = 11 ' CL190500 (home)
	' Public Const GISDSActionSecurityDummy = 12 ' CL040600
	Public Const GISDSActionSecurityRegisterUser As Short = 12 ' RAG070600
	Public Const GISDSActionSecurityLoginUser As Short = 13 ' RAG070600
	Public Const GISDSActionUpdatePartyCnt As Short = 14 ' RAG150600
	Public Const GISDSActionGetQuotesPoliciesForParty As Short = 15 ' RFC210600
	Public Const GISDSActionGetPolicyVersions As Short = 16 ' RFC210600
	Public Const GISDSActionNBStart As Short = 17 ' RFC270600
	Public Const GISDSActionMTAStart As Short = 18 ' RFC270600
	Public Const GISDSActionFinancialBankAccountValidation As Short = 19 'CJB101000
	Public Const GISDSActionFinancialCalcPaymentMethodCharge As Short = 20 'CJB181000
	Public Const GISDSActionVehicleLookup As Short = 21 'RAG171100
	Public Const GISDSActionSendEmail As Short = 22 'RAG201100
	Public Const GISDSActionAddParty As Short = 23 'RJG 21/11/2000
	Public Const GISDSActionFindParty As Short = 24 'RJG 21/11/2000
	Public Const GISDSActionGetQuotesForParty As Short = 25 'RJG 21/11/2000
	Public Const GISDSActionGetParty As Short = 26 'RJG 28/11/2000
	Public Const GISDSActionFindQuote As Short = 27 'RJG 04/12/2000
	Public Const GISDSActionFinancialPremiumFinanceQuote As Short = 28 'CJB121200
	Public Const GISDSActionGetProductByAgent As Short = 29 'RJG 09/01/2001
	Public Const GISDSActionFindPolicy As Short = 30 'CL240101
	Public Const GISDSActionGetLookup As Short = 31 'RDC30052001
	Public Const GISDSActionPrintForm As Short = 32 'RJG13082001
	'Renewals Class
	Public Const GISDSActionRenSelection As Short = 33 'DD22102001
	Public Const GISDSActionRenQuotationBrokerLead As Short = 34 'DD22102001
	Public Const GISDSActionRenInvitationBrokerLead As Short = 35 'DD22102001
	Public Const GISDSActionRenReprintInvitationBrokerLead As Short = 36 'DD22102001
	Public Const GISDSActionRenInvitePreferredQuotes As Short = 37 'DD22102001
	Public Const GISDSActionRenConfDocsHoldingInsurer As Short = 38 'DD22102001
	Public Const GISDSActionRenConfirmRenewal As Short = 39 'DD22102001
	Public Const GISDSActionRenConfirmLapse As Short = 40 'DD22102001
	Public Const GISDSActionRenCompLapse As Short = 41 'DD22102001
	Public Const GISDSActionRenCompHoldingInsurer As Short = 42 'DD22102001
	Public Const GISDSActionRenCompAlternateInsurer As Short = 43 'DD22102001
	Public Const GISDSActionRenGetPolicyRenewalVersion As Short = 44 'DD22102001
	Public Const GISDSActionRenReminder As Short = 45 'DD22102001
	Public Const GISDSActionRenReprintConfirm As Short = 46 'DD22102001
	Public Const GISDSActionRenResendEDI As Short = 47 'DD22102001
	Public Const GISDSActionRenCompletion As Short = 48 'DD22102001
	Public Const GISDSActionRenListRenewals As Short = 49 'DD22102001
	Public Const GISDSActionRenUpdateRenewalControl As Short = 50 'DD01112001
	
	' Scheme Audit Constants
	Public Const GISSchemAuditNBRefer As Short = 0
	Public Const GISSchemeAuditNBQuote As Short = 1
	Public Const GISSchemeAuditNBGuaranteedQuote As Short = 2
	Public Const GISSchemeAuditNBTransact As Short = 3
	Public Const GISSchemeAuditNBDecline As Short = 4
	
	Public Const GISPolLinkIDName As String = "gis_policy_link_id"
	
	' Data Model Definition Attributes
	Public Const GISXMLAttribDataModelID As String = "DataModelID"
	Public Const GISXMLAttribDataModelCode As String = "DataModelCode"
	' RDC 27072001
	Public Const GISXMLAttribDataSetDefTimestamp As String = "DataSetDefTimestamp"
	
	' RFC13012000
	Public Const GISLowDate As Date = #1/1/1900#
	
	' RFC210600
	' Insurance File Search Types
	Public Const GISIFSTQuote As Integer = 1
	Public Const GISIFSTPolicy As Short = 2
	Public Const GISIFSTRenewal As Short = 3
	Public Const GISIFSTQuotePolicy As Short = 4
	Public Const GISIFSTQuotePolicyRenewal As Short = 5
	
	' ***************************************************************** '
	' Stored Procedure Column Positions
	' ***************************************************************** '
	' Object Array Column Positions
	Public Const GISObjColObjectId As Short = 0
	Public Const GISObjColModelId As Short = 1
	Public Const GISObjColObjectName As Short = 2
	Public Const GISObjColTableName As Short = 3
	Public Const GISObjColMaxInstances As Short = 4
	Public Const GISObjColIsQuoteObject As Short = 5
	Public Const GISObjColParentObjectName As Short = 6
	Public Const GISObjColPolarisObjId As Short = 7
	
	' Property Array Column Positions
	Public Const GISPropColObjectId As Short = 0
	Public Const GISPropColObjectName As Short = 1
	Public Const GISPropColPropertyId As Short = 2
	Public Const GISPropColPropertyName As Short = 3
	Public Const GISPropColColumnName As Short = 4
	Public Const GISPropColDataType As Short = 5
	Public Const GISPropColIsIdentifyingProperty As Short = 6
	Public Const GISPropColIsPrimaryKey As Short = 7
	Public Const GISPropColListId As Short = 8
	Public Const GISPropColPolarisPropId As Short = 9
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Array Column Positions Used by Methods
	' ***************************************************************** '
	
	' Used by the Method 'GetInstanceHierarchy'
	Public Const GISHierColObjectName As Integer = 0
	Public Const GISHierColOIKey As Integer = 1
	Public Const GISHierColChildNum As Integer = 2
	Public Const GISHierColIDName1 As Integer = 3
	Public Const GISHierColIDValue1 As Integer = 4
	Public Const GISHierColIDName2 As Integer = 5
	Public Const GISHierColIDValue2 As Integer = 6
	Public Const GISHierColIDName3 As Integer = 7
	Public Const GISHierColIDValue3 As Integer = 8
	Public Const GISHierColParentOIKey As Integer = 9
	
	' Used by the method 'GetObjectIdentity'
	Public Const GISIDColPropName As Integer = 0
	Public Const GISIDColPropValue As Integer = 1
	Public Const GISIDColIdentifyingProperty As Integer = 1
	
	' QEM Scheme Array Column Positions
	'Public Const GISQEMSchArraySize = 12 'sj 10/9/99
	'Public Const GISQEMSchArraySize = 15 'sj 04/1/00
	Public Const GISQEMSchArraySize As Short = 18 'CL120900
	
	Public Const GISQEMSchObjectName As Short = 0
	Public Const GISQEMSchClassName As Short = 1
	Public Const GISQEMSchSchemeNo As Short = 2
	Public Const GISQEMSchSchemeVer As Short = 3
	Public Const GISQEMSchFilename As Short = 4
	Public Const GISQEMSchQMInsurerRef As Short = 5
	Public Const GISQEMSchPolarisInsurerNo As Short = 6
	Public Const GISQEMSchType As Short = 7 ' CL160799
	Public Const GISQEMSchVariant As Short = 8 ' CL190799
	Public Const GISQEMSchCommPerc As Short = 9 ' CL030899
	Public Const GISQEMSchID As Short = 10 ' CL030899
	Public Const GISQEMSchDesc As Short = 11 ' CL030899
	Public Const GISQEMSchAbi81Insurer As Short = 12 'sj 09/09/99
	'sj 04/01/00 - start
	Public Const GISQEMSchAbi1EdiDirectory As Short = 13
	Public Const GISQEMSchAgencyCode As Short = 14
	Public Const GISQEMSchEdiMailBox As Short = 15
	'sj 04/01/00 - end
	' RFC260700 - Added Insurer Description to Select
	Public Const GISQEMSchInsurerDesc As Short = 16
	Public Const GISQEMSchDictVer As Short = 17 ' CL120900
	Public Const GISQEMSchTypeFlags As Short = 18 'CJB050601
	
	' RFC03091999 - Constants For Address Match Array Added
	
	'DB 15/11/99 - Extra constants added for Address Match Array
	Public Const GISAddressSubPremise As Integer = 0 ' Sub Premise (Could be just number)
	Public Const GISAddressPremiseNumber As Integer = 1 ' Premise Number (Could be name)
	Public Const GISAddressLine1 As Integer = 2 '1->2     Thoroughfare
	Public Const GISAddressLine2 As Integer = 3 '2->3     Town/City
	Public Const GISAddressLine3 As Integer = 4 '3->4     County
	Public Const GISAddressPostCode As Integer = 5 ' Postcode (Could have been recoded)
	
	' CL190500  - Datacash Result Array Columns
	Public Const GISDatacashResult As Integer = 0
	Public Const GISDatacashAuthCode As Integer = 1
	Public Const GISDatacashUniqueRef As Integer = 2
	Public Const GISDatacashTimeStamp As Integer = 3
	Public Const GISDatacashCardType As Integer = 4
	Public Const GISDatacashIssuer As Integer = 5
	Public Const GISDatacashCountry As Integer = 6
	
	
	
	' ***************************************************************** '
	' Used to store the empty XML Data Set & Def Files
	Public Const ACDataSetSuffix As String = "DS"
	Public Const ACDataSetDefSuffix As String = "DSD"
	Public Const ACXMLFileExtension As String = ".XML"
	Public Const ACXSLFileExtension As String = ".XSL"
	' ***************************************************************** '
	
	' For Defaults XML file   CB 310101
	Public Const ACGISDefaultsFileSuffix As String = "DEFAULTS"
	Public Const ACGISSaveXSLFileSuffix As String = "SAVEXSL"
	Public Const ACGISSetIDXSLFileSuffix As String = "SETID"
	
	'For GeminiNet Quote Types    CJB 260301
	Public Const GISGeminiNetHome As String = "Home"
	Public Const GISGeminiNetMotor As String = "Motor"
	
	'For Motor Dictionary
	Public Const ACPolExecAppRules As Integer = 57
	Public Const ACPolExecInitRules As Integer = 50
	Public Const ACPolExecMainRules As Integer = 51
	Public Const ACPolExecProRata As Integer = 41
	Public Const ACPolExecCancellation As Integer = 25
	Public Const ACPolExecNewBusOnly As Integer = 16 ' CL160799
	Public Const ACPolExecMTACancelPolicy As Integer = 25 ' CL100400
	
	'For Household Dictionary   CB 290200
	Public Const ACPolExecInitRulesHousehold As Integer = 27
	Public Const ACPolExecMainRulesHousehold As Integer = 28
	Public Const ACPolExecAppRulesHousehold As Integer = 33
	
	'GISPolarisEventCode - CL181200
	' These codes go into Polaris object EDIPartyControls, EventTypeSent at transaction
	Public Const GISPolEventCodeNB As String = "PRO"
	Public Const GISPolEventCodeMTA As String = "ADJ"
	Public Const GISPolEventCodeCancel As String = "CAN"
	Public Const GISPolEventCodeLapse As String = "PLL"
	Public Const GISPolEventCodeRenew As String = "RNC"
	
	'DB 10/12/99 Start
	'Registry setting keys for E-Mail Addresses for reports
	Public Const ACCGUReportEMail As String = "CGUReportEmail" 'CGU
	'DB 10/12/99 End
	
	'GIS lookup constants
	Public Const GISLookupTest As Integer = 0
	Public Const GISLookupLive As Integer = 1
	Public Const GISLookupTestOrLive As Integer = 99
	
	' Datacash Request Types - CL190500 (home)
	Public Const GISDatacashTypeAuthorise As String = "auth"
	Public Const GISDatacashTypePreAuthorise As String = "pre"
	Public Const GISDatacashTypeFulfill As String = "fulfill"
	Public Const GISDatacashTypeRefund As String = "refund" ' JP201000
	
	'GIS Add On Codes - RJG 06/06/2000
	Public Const GISBreakdownAddOn As String = "BREAKDOWN"
	Public Const GISULRAddOn As String = "ULR"
	Public Const Breakdown_Offer_Cover_Code As Short = 2
	Public Const ULR_Offer_Cover_Code As Short = 4
	Public Const Breakdown_Offer_Cover_Text As String = "Breakdown"
	Public Const ULR_Offer_Cover_Text As String = "Uninsured Loss Recovery"
	
	'GIS Xelector Brand codes for each channel - CL161100
	Public Const GISXelBrandCodeAutoBytel As String = "XEL"
	Public Const GISXelBrandCodeFirste As String = "FIRSTE"
	Public Const GISXelBrandCodeFTYM As String = "FTYM"
	Public Const GISXelBrandCodeMoneyNet As String = "MONEYNET"
	Public Const GISXelBrandCodeETrade As String = "ETRADE"
	Public Const GISXelBrandCodeGenXel As String = "GENXEL"
	Public Const GISXelBrandCodeFish4 As String = "FISH4"
	Public Const GISXelBrandCodeLycos As String = "LYCOS"
	Public Const GISXelBrandCodeLoot As String = "LOOT"
	Public Const GISXelBrandCodeTelegraph As String = "TELEGRAPH"
	Public Const GISXelBrandCodeLycos_Motor As String = "LYCOS_MOTOR"
	
	
	'GIS Xelector Brand names for each channel - CL161100
	Public Const GISXelBrandNameAutoBytel As String = "Autobytel"
	Public Const GISXelBrandNameFirste As String = "first-e"
	Public Const GISXelBrandNameFTYM As String = "FTyourmoney"
	Public Const GISXelBrandNameMoneyNet As String = "moneynet"
	Public Const GISXelBrandNameETrade As String = "E*TRADE"
	Public Const GISXelBrandNameGenXel As String = "Xelector"
	Public Const GISXelBrandNameFish4 As String = "Fish4"
	Public Const GISXelBrandNameLycos As String = "Lycos"
	Public Const GISXelBrandNameLoot As String = "Loot"
	Public Const GISXelBrandNameTelegraph As String = "Telegraph"
	Public Const GISXelBrandNameLycos_Motor As String = "Lycos_Motor"
	
	'GIS Xelector Broker Codes for each channel to send to Prompt - CJB 090301
	Public Const GISXelBrokerCodeAutoBytel As String = "AUTOBT"
	Public Const GISXelBrokerCodeFirstE As String = "First-e"
	Public Const GISXelBrokerCodeMoneyNet As String = "MoneyNet"
	Public Const GISXelBrokerCodeFTYM As String = "FTYourMoney"
	Public Const GISXelBrokerCodeETrade As String = "E-Trade"
	Public Const GISXelBrokerCodeGenXel As String = "XE"
	Public Const GISXelBrokerCodeFish4 As String = "Fish4"
	Public Const GISXelBrokerCodeLycos As String = "Lycos"
	Public Const GISXelBrokerCodeLoot As String = "Loot"
	Public Const GISXelBrokerCodeTelegraph As String = "Telegraph"
	Public Const GISXelBrokerCodeLycos_Motor As String = "Lycos_Motor"
	
	'Defaults XML Document Type Definition Constants - CJB 200301
	Public Const GISDefaultsMandatoryProperty As String = "MANDATORY_PROPERTY"
	
	'GIS QMM Insurer No. Offset - CJB 170401
	'(this is required as we are using QMM's Insurer Ref (unique per insurer) to put into
	' the Insurer No. field - since both are unique individually but perhaps not when
	' combined then we'll add an offset value to the QMM insurer ref before we store it in
	' the insurer no. field).
	Public Const GISQMMInsurerNoOffset As Short = 1000
	
	
	
	
	




    ' ***************************************************************** '
    ' Name: GetDataSetFileNames
    '
    ' Description: Calculates the Location and File Name of the XML
    '              files for a given Data Model.
    '
    ' ***************************************************************** '
    Public Function GetDataSetFileNames(ByVal v_sDataModelCode As String, ByRef r_sDataSetDefFile As String, ByRef r_sDataSetFile As String) As Integer

        Dim sPath As String
        Dim lReturn As Integer

        On Error GoTo Err_GetDataSetFileNames

        GetDataSetFileNames = gPMConstants.PMEReturnCode.PMTrue

        ' MUST have a Data Model Code
        If (Trim(v_sDataModelCode) = "") Then
            LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataSetFileNames", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataSetFileNames")
            GetDataSetFileNames = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Get the Path where they live
        lReturn = GetDataSetsPath(v_sDataModelCode:=v_sDataModelCode, r_sDataSetsPath:=sPath)
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetDataSetFileNames = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        r_sDataSetDefFile = sPath & Trim(UCase(v_sDataModelCode)) & ACDataSetDefSuffix & ACXMLFileExtension

        r_sDataSetFile = sPath & Trim(UCase(v_sDataModelCode)) & ACDataSetSuffix & ACXMLFileExtension

        Exit Function

Err_GetDataSetFileNames:

        GetDataSetFileNames = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataSetFileNamesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataSetFileNames", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function

    End Function


    ' ***************************************************************** '
    ' Name: GetDefaultsFileName
    '
    ' Description: Determines the Location and File Name of the GIS
    '              Defaults XML file for the current Data Model.
    '
    ' Date:        31/01/2001
    '
    ' History:     CJB - Created
    '
    ' ***************************************************************** '
    Public Function GetDefaultsFileName(ByVal v_sGisDataModelCode As String, ByRef GISDefaultsFile As String) As Integer

        Dim sPath As String
        Dim lReturn As Integer

        On Error GoTo Err_GetDefaultsFileName

        GetDefaultsFileName = gPMConstants.PMEReturnCode.PMTrue

        ' MUST have a Data Model Code
        If (Trim(v_sGisDataModelCode) = "") Then
            LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Data Model Code Supplied...", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultsFileName")
            GetDefaultsFileName = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Get the Path where it lives - note it lives with the actual dataset files
        lReturn = GetDataSetsPath(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetsPath:=sPath)
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            GetDefaultsFileName = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        GISDefaultsFile = sPath & Trim(UCase(v_sGisDataModelCode)) & ACGISDefaultsFileSuffix & ACXMLFileExtension

        Exit Function

Err_GetDefaultsFileName:

        GetDefaultsFileName = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultsFileNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultsFileName", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: GetDataSetsPath
    '
    ' Description: Return the Path for GIS Data Sets storage/retrieval.
    '
    ' ***************************************************************** '
    Public Function GetDataSetsPath(ByVal v_sDataModelCode As String, ByRef r_sDataSetsPath As String) As Integer

        Dim lReturn As Integer

        On Error GoTo Err_GetDataSetsPath

        GetDataSetsPath = gPMConstants.PMEReturnCode.PMTrue

        ' RFC110400 - Get the Data Model Specific Data Set Path
        lReturn = GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISRegDataSetPath, r_sSettingValue:=r_sDataSetsPath, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon)

        ' RFC110400 - If no Data Model Specific Data Set Path Found
        If (Trim(r_sDataSetsPath) = "") Then

            ' RFC110400 - Look for a setting in the Common\GIS
            ' i.e. Not Data Model Specifc
            lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=GISRegDataSetPath, r_sSettingValue:=r_sDataSetsPath, v_sSubKey:=GISRegSubKey)

        End If

        If (Trim(r_sDataSetsPath) = "") Then
            LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Data Sets Path (" & GISRegDataSetPath & ") Registry Setting.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataSetsPath")
            GetDataSetsPath = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If Right(r_sDataSetsPath, 1) <> "\" Then
            r_sDataSetsPath = r_sDataSetsPath & "\"
        End If

        Exit Function

Err_GetDataSetsPath:

        GetDataSetsPath = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataSetsPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataSetsPath", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function

    End Function


    ' ***************************************************************** '
    ' Name: IsGISInDebug
    '
    ' Description: Return the GIS Debug Status.
    '
    ' ***************************************************************** '
    Public Function IsGISInDebug() As Boolean

        Dim sDebug As String
        Dim lReturn As Integer

        On Error GoTo Err_IsGISInDebug

        IsGISInDebug = True

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=GISRegGISInDebug, r_sSettingValue:=sDebug, v_sSubKey:=GISRegSubKey)

        Select Case UCase(Trim(sDebug))
            Case "1"
                IsGISInDebug = True
            Case "Y", "YES"
                IsGISInDebug = True
            Case Else
                IsGISInDebug = False
        End Select

        Exit Function

Err_IsGISInDebug:

        IsGISInDebug = True

        ' Log Error Message
        LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS Debud setting from registry. Default to Debug = True.", vApp:=ACApp, vClass:=ACClass, vMethod:="IsGISInDebug", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: GetGISLogLevel
    '
    ' Description: Return the GIS Log Level Status.
    '
    ' ***************************************************************** '
    Public Function GetGISLogLevel() As Short

        Dim sLogLevel As String
        Dim iLogLevel As Short
        Dim lReturn As Integer

        On Error GoTo Err_GetGISLogLevel

        GetGISLogLevel = gPMConstants.PMELogLevel.PMLogFatal

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=GISRegGISLogLevel, r_sSettingValue:=sLogLevel, v_sSubKey:=GISRegSubKey)

        If (Trim(sLogLevel) = "") Then
            iLogLevel = gPMConstants.PMELogLevel.PMLogFatal
        Else
            iLogLevel = CShort(sLogLevel)
        End If

        If iLogLevel = 0 Then
            iLogLevel = gPMConstants.PMELogLevel.PMLogFatal
        End If

        GetGISLogLevel = iLogLevel

        Exit Function

Err_GetGISLogLevel:

        GetGISLogLevel = gPMConstants.PMELogLevel.PMLogFatal

        ' Log Error Message
        LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS Log Level setting from registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISLogLevel", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function

    End Function


    ' ***************************************************************** '
    ' Name: GetRegSettingFromDataBusModel
    '
    ' Description:
    '
    ' Date: CL220200
    ' RFC310700 - Optionally specify a SubKey
    ' RFC290300 - Make Business Type Code Optional
    ' RFC110400 - Optionally specify the RegSetting Level i.e. Client/Common/Server(default)
    ' ***************************************************************** '
    Public Function GetRegSettingFromDataBusModel(ByVal v_sDataModelCode As String, ByVal v_sSettingName As String, ByRef r_sSettingValue As String, Optional ByVal v_sBusinessTypeCode As String = "", Optional ByVal v_lPMERegSettingLevel As gPMConstants.PMERegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer, Optional ByVal v_sSubKey As String = "") As Integer

        Dim lReturn As Integer
        Dim sValue As String
        Dim sSubKeyDataModel, sSubKeyDataModelBusModel As String
        Dim PMERegSettingLevel As gPMConstants.PMERegSettingLevel

        On Error GoTo Err_GetRegSettingFromDataBusModel

        GetRegSettingFromDataBusModel = gPMConstants.PMEReturnCode.PMTrue

        sSubKeyDataModelBusModel = ACOIMGISSubKey & "\" & v_sDataModelCode & "\" & v_sBusinessTypeCode
        sSubKeyDataModel = ACOIMGISSubKey & "\" & v_sDataModelCode

        ' If we have a SubKey
        v_sSubKey = Trim(v_sSubKey)
        If (v_sSubKey <> "") Then
            ' Append the sub key to both versions of the path
            sSubKeyDataModelBusModel = sSubKeyDataModelBusModel & "\" & v_sSubKey
            sSubKeyDataModel = sSubKeyDataModel & "\" & v_sSubKey
        End If

        sValue = ""

        ' Use the supplied (or default reg setting level)
        PMERegSettingLevel = v_lPMERegSettingLevel

        ' If we have a Business Type Code the Look for the setting there
        If (v_sBusinessTypeCode <> "") Then

            ' First look for value in data model/bus model key
            lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=PMERegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=sValue, v_sSubKey:=sSubKeyDataModelBusModel)

            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetRegSettingFromDataBusModel = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        End If

        ' If we haven't got the value, look at Data Model level
        If Trim(sValue) = "" Then

            ' Value not found in data model/bus model key
            ' Therefore look in data model key
            lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=PMERegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=sValue, v_sSubKey:=sSubKeyDataModel)

            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetRegSettingFromDataBusModel = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        End If

        r_sSettingValue = sValue

        Exit Function

Err_GetRegSettingFromDataBusModel:

        GetRegSettingFromDataBusModel = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRegSettingFromDataBusModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettingFromDataBusModel", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function

    End Function


    ' ************************************************************************** '
    ' Name: ReplaceCharacter
    '
    ' Description: Function to examine a string for a specified character or
    '              number of contiguous characters and replace with a
    '              specified replacement character or number of characters.
    '
    '              If the character to search for is an actual character e.g. "x" or "*"
    '              then pass "C" in for the SearchCharType, else if you are supplying a
    '              numeric ASCII value pass "A" in. However, with the latter you may
    '              obviusly only search for one character and replace by one character.
    '
    '              Handles empty search strings but number of characters in
    '              search character string must match number of replacement
    '              characters.
    '
    ' Author:      C J Barnes
    '
    ' Date:        6/4/2000
    '
    ' ************************************************************************** '

    Public Function ReplaceCharacter(ByRef r_sString As String, ByVal v_vSearchChar As Object, ByVal v_sReplaceChar As String, ByVal v_sSearchCharType As String) As Integer

        Dim lReturn As Integer
        Dim iCharPosition As Short
        Dim iReplaceStringLength As Short

        On Error GoTo Err_ReplaceCharacter

        ReplaceCharacter = gPMConstants.PMEReturnCode.PMTrue

        'Search for Character value
        If v_sSearchCharType = "C" Then

            iReplaceStringLength = Len(v_sReplaceChar)
            'UPGRADE_WARNING: Couldn't resolve default property of object v_vSearchChar. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            iCharPosition = InStr(1, r_sString, v_vSearchChar)

            Do While iCharPosition > 0
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vSearchChar. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                Mid(r_sString, iCharPosition, iReplaceStringLength) = v_vSearchChar
                'UPGRADE_WARNING: Couldn't resolve default property of object v_vSearchChar. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                iCharPosition = InStr(iCharPosition + iReplaceStringLength, r_sString, v_vSearchChar)
            Loop

            'Search for ASCII value
        ElseIf v_sSearchCharType = "A" Then

            If Len(r_sString) > 0 Then

                For iCharPosition = 1 To Len(r_sString)
                    'UPGRADE_WARNING: Couldn't resolve default property of object v_vSearchChar. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    If Asc(Mid(r_sString, iCharPosition, 1)) = v_vSearchChar Then
                        Mid(r_sString, iCharPosition, 1) = v_sReplaceChar
                    End If
                Next

            End If
        End If

        Exit Function

Err_ReplaceCharacter:

        ReplaceCharacter = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReplaceCharacter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplaceCharacter", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function

    End Function


    ' ***************************************************************** '
    ' Name: LogMessageFile
    '
    ' Description: Wrapper function to the log message method of the
    '              message object.
    '
    ' ***************************************************************** '
    Public Sub LogMessageFile(ByRef iType As gPMConstants.PMELogLevel, ByRef sMsg As String, Optional ByRef vApp As String = "", Optional ByRef vClass As String = "", Optional ByRef vMethod As String = "", Optional ByRef vErrNo As Integer = 0, Optional ByRef vErrDesc As String = "")

        Dim lErrorValue As Integer
        Dim vTimestamp As Object
        Dim lMessageID As Integer

        On Error GoTo Err_LogMessage

        ' We cannot Initialise PMMessage, Log to Screen
        LogMessageToFile(sUsername:=ACSTSClientUserName, iType:=iType, sMsg:=sMsg, vApp:=vApp, vClass:=vClass, vMethod:=vMethod, vErrNo:=vErrNo, vErrDesc:=vErrDesc)

        Exit Sub

Err_LogMessage:

        ' Error Section.

        Exit Sub

    End Sub


    ' ***************************************************************** '
    ' Name: DebugLogMessageFile
    '
    '
    ' ***************************************************************** '
    Public Sub DebugLogMessageFile(ByRef iType As gPMConstants.PMELogLevel, ByRef sMsg As String, Optional ByRef vApp As String = "", Optional ByRef vClass As String = "", Optional ByRef vMethod As String = "", Optional ByRef vErrNo As Integer = 0, Optional ByRef vErrDesc As String = "")

        Dim lErrorValue As Integer
        Dim vTimestamp As Object
        Dim lMessageID As Integer
        Static vInDebug As Object

        On Error GoTo Err_LogMessage

        ' Check if we need to log this message.
        'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1041"'
        If (IsNothing(vInDebug)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vInDebug. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vInDebug = IsGISInDebug()
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object vInDebug. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        If (vInDebug = True) Then
            ' We cannot Initialise PMMessage, Log to Screen
            LogMessageToFile(sUsername:=ACSTSClientUserName, iType:=iType, sMsg:=sMsg, vApp:=vApp, vClass:=vClass, vMethod:=vMethod, vErrNo:=vErrNo, vErrDesc:=vErrDesc)
        End If

        Exit Sub

Err_LogMessage:

        ' Error Section.

        Exit Sub

    End Sub


End Module