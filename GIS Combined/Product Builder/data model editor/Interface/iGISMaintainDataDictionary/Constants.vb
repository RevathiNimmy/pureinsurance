Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'developer guide no. 129
Imports SharedFiles
Module Constants
	' ***************************************************************** '
	' Module Name: Constants
	'
	' Date:  02082002
	'
	' Edit History:
	' RDC 02082002 created as copy of GISSharedConstants without
	'              CheckGISDSN, which tries to create DB connection
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "GISSharedConstants"
	
	' ***************************************************************** '
	' General Constants
	' ***************************************************************** '
	' Registry subkey TB 24/2/00
	Public Const ACOIMGISSubKey As String = "GIS"
	
	' Data Types
	Public Const GISDataTypeUnknown As Integer = 0
	Public Const GISDataTypeDate As Integer = 1
	Public Const GISDataTypeNumeric As Integer = 2
	Public Const GISDataTypeShortList As Integer = 3
	Public Const GISDataTypeLongList As Integer = 4
	Public Const GISDataTypeText As Integer = 5
	Public Const GISDataTypeNumericOutput As Integer = 6
	Public Const GISDataTypeComment As Integer = 7
	Public Const GISDataTypeShortListCode As Integer = 103
	Public Const GISDataTypeLongListCode As Integer = 104
	' RFC070900 - Added some more GIS data types as required by Underwriting
	' RFC070900 - All of these are effectively numeric values
	' RFC070900 - Leave a Gap between 6 and 20 incase Polaris decide to create
	'             a new data type, so we can use the same number as them (hopefully).
	Public Const GISDataTypeOption As Integer = 20
	Public Const GISDataTypeCurrency As Integer = 21
	Public Const GISDataTypePercentage As Integer = 22
	'SJ 15/07/2004 - start
	Public Const GISDataTypeInteger As Integer = 23
	'SJ 15/07/2004 - end
    Public Const GISDataTypecode As Integer = 24
    ' Quote Types
	Public Const GISNBQteSurveyPremOnly As Integer = 1
	Public Const GISNBQteSurveyPremTerms As Integer = 2
	Public Const GISNBQteSurveyFull As Integer = 3
	Public Const GISNBQteSingleScheme As Integer = 4
	
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
	Public Const GISRenQuote As Integer = 99
	
	' Scheme Quote Status
	Public Const GISQteStatusUnknown As Integer = 0
	Public Const GISQteStatusDecline As Integer = 1
	Public Const GISQteStatusReferNoQuote As Integer = 2
	Public Const GISQteStatusReferWithQuote As Integer = 3
	Public Const GISQteStatusQuote As Integer = 4
	
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
	Public Const GISQtePolarisTypeNewBusiness As Integer = 1
	Public Const GISQtePolarisTypeFullCycle As Integer = 2
	
	' Polaris Quote Return Values
	Public Const GISPolarisQuoteOK As Integer = 1
	Public Const GISPolarisQuoteReferA As Integer = 2
	Public Const GISPolarisQuoteReferB As Integer = 5
	Public Const GISPolarisQuoteDeclineA As Integer = 3
	Public Const GISPolarisQuoteDeclineB As Integer = 6
	
	' Data Set Actions
	Public Const GISDSActionNone As Integer = 0
	Public Const GISDSActionNewDataSet As Integer = 1
	Public Const GISDSActionQuote As Integer = 2
	Public Const GISDSActionSaveToDB As Integer = 3
	Public Const GISDSActionLoadFromDB As Integer = 4
	Public Const GISDSActionSearchForAddress As Integer = 5
	Public Const GISDSActionUpdateQtePassword As Integer = 6
	Public Const GISDSActionNBTransact As Integer = 7
	Public Const GISDSActionRefer As Integer = 8
	Public Const GISDSActionMTAQuote As Integer = 9
	Public Const GISDSActionMTATransact As Integer = 10
	Public Const GISDSActionFinancialDatacash As Integer = 11 ' CL190500 (home)
	' Public Const GISDSActionSecurityDummy = 12 ' CL040600
	Public Const GISDSActionSecurityRegisterUser As Integer = 12 ' RAG070600
	Public Const GISDSActionSecurityLoginUser As Integer = 13 ' RAG070600
	Public Const GISDSActionUpdatePartyCnt As Integer = 14 ' RAG150600
	Public Const GISDSActionGetQuotesPoliciesForParty As Integer = 15 ' RFC210600
	Public Const GISDSActionGetPolicyVersions As Integer = 16 ' RFC210600
	Public Const GISDSActionNBStart As Integer = 17 ' RFC270600
	Public Const GISDSActionMTAStart As Integer = 18 ' RFC270600
	Public Const GISDSActionFinancialBankAccountValidation As Integer = 19 'CJB101000
	Public Const GISDSActionFinancialCalcPaymentMethodCharge As Integer = 20 'CJB181000
	Public Const GISDSActionVehicleLookup As Integer = 21 'RAG171100
	Public Const GISDSActionSendEmail As Integer = 22 'RAG201100
	Public Const GISDSActionAddParty As Integer = 23 'RJG 21/11/2000
	Public Const GISDSActionFindParty As Integer = 24 'RJG 21/11/2000
	Public Const GISDSActionGetQuotesForParty As Integer = 25 'RJG 21/11/2000
	Public Const GISDSActionGetParty As Integer = 26 'RJG 28/11/2000
	Public Const GISDSActionFindQuote As Integer = 27 'RJG 04/12/2000
	Public Const GISDSActionFinancialPremiumFinanceQuote As Integer = 28 'CJB121200
	Public Const GISDSActionGetProductByAgent As Integer = 29 'RJG 09/01/2001
	Public Const GISDSActionFindPolicy As Integer = 30 'CL240101
	Public Const GISDSActionGetLookup As Integer = 31 'RDC30052001
	Public Const GISDSActionPrintForm As Integer = 32 'RJG13082001
	'Renewals Class
	Public Const GISDSActionRenSelection As Integer = 33 'DD22102001
	Public Const GISDSActionRenQuotationBrokerLead As Integer = 34 'DD22102001
	Public Const GISDSActionRenInvitationBrokerLead As Integer = 35 'DD22102001
	Public Const GISDSActionRenReprintInvitationBrokerLead As Integer = 36 'DD22102001
	Public Const GISDSActionRenInvitePreferredQuotes As Integer = 37 'DD22102001
	Public Const GISDSActionRenConfDocsHoldingInsurer As Integer = 38 'DD22102001
	Public Const GISDSActionRenConfirmRenewal As Integer = 39 'DD22102001
	Public Const GISDSActionRenConfirmLapse As Integer = 40 'DD22102001
	Public Const GISDSActionRenCompLapse As Integer = 41 'DD22102001
	Public Const GISDSActionRenCompHoldingInsurer As Integer = 42 'DD22102001
	Public Const GISDSActionRenCompAlternateInsurer As Integer = 43 'DD22102001
	Public Const GISDSActionRenGetPolicyRenewalVersion As Integer = 44 'DD22102001
	Public Const GISDSActionRenReminder As Integer = 45 'DD22102001
	Public Const GISDSActionRenReprintConfirm As Integer = 46 'DD22102001
	Public Const GISDSActionRenResendEDI As Integer = 47 'DD22102001
	Public Const GISDSActionRenCompletion As Integer = 48 'DD22102001
	Public Const GISDSActionRenListRenewals As Integer = 49 'DD22102001
	Public Const GISDSActionRenUpdateRenewalControl As Integer = 50 'DD01112001
	
	' Scheme Audit Constants
	Public Const GISSchemAuditNBRefer As Integer = 0
	Public Const GISSchemeAuditNBQuote As Integer = 1
	Public Const GISSchemeAuditNBGuaranteedQuote As Integer = 2
	Public Const GISSchemeAuditNBTransact As Integer = 3
	Public Const GISSchemeAuditNBDecline As Integer = 4
	
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
	Public Const GISIFSTPolicy As Integer = 2
	Public Const GISIFSTRenewal As Integer = 3
	Public Const GISIFSTQuotePolicy As Integer = 4
	Public Const GISIFSTQuotePolicyRenewal As Integer = 5
	
	' ***************************************************************** '
	' Stored Procedure Column Positions
	' ***************************************************************** '
	' Object Array Column Positions
	Public Const GISObjColObjectId As Integer = 0
	Public Const GISObjColModelId As Integer = 1
	Public Const GISObjColObjectName As Integer = 2
	Public Const GISObjColTableName As Integer = 3
	Public Const GISObjColMaxInstances As Integer = 4
	Public Const GISObjColIsQuoteObject As Integer = 5
	Public Const GISObjColParentObjectName As Integer = 6
	Public Const GISObjColPolarisObjId As Integer = 7
	
	' Property Array Column Positions
	Public Const GISPropColObjectId As Integer = 0
	Public Const GISPropColObjectName As Integer = 1
	Public Const GISPropColPropertyId As Integer = 2
	Public Const GISPropColPropertyName As Integer = 3
	Public Const GISPropColColumnName As Integer = 4
	Public Const GISPropColDataType As Integer = 5
	Public Const GISPropColIsIdentifyingProperty As Integer = 6
	Public Const GISPropColIsPrimaryKey As Integer = 7
	Public Const GISPropColListId As Integer = 8
	Public Const GISPropColPolarisPropId As Integer = 9
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
	Public Const GISQEMSchArraySize As Integer = 18 'CL120900
	
	Public Const GISQEMSchObjectName As Integer = 0
	Public Const GISQEMSchClassName As Integer = 1
	Public Const GISQEMSchSchemeNo As Integer = 2
	Public Const GISQEMSchSchemeVer As Integer = 3
	Public Const GISQEMSchFilename As Integer = 4
	Public Const GISQEMSchQMInsurerRef As Integer = 5
	Public Const GISQEMSchPolarisInsurerNo As Integer = 6
	Public Const GISQEMSchType As Integer = 7 ' CL160799
	Public Const GISQEMSchVariant As Integer = 8 ' CL190799
	Public Const GISQEMSchCommPerc As Integer = 9 ' CL030899
	Public Const GISQEMSchID As Integer = 10 ' CL030899
	Public Const GISQEMSchDesc As Integer = 11 ' CL030899
	Public Const GISQEMSchAbi81Insurer As Integer = 12 'sj 09/09/99
	'sj 04/01/00 - start
	Public Const GISQEMSchAbi1EdiDirectory As Integer = 13
	Public Const GISQEMSchAgencyCode As Integer = 14
	Public Const GISQEMSchEdiMailBox As Integer = 15
	'sj 04/01/00 - end
	' RFC260700 - Added Insurer Description to Select
	Public Const GISQEMSchInsurerDesc As Integer = 16
	Public Const GISQEMSchDictVer As Integer = 17 ' CL120900
	Public Const GISQEMSchTypeFlags As Integer = 18 'CJB050601
	
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
	Public Const Breakdown_Offer_Cover_Code As Integer = 2
	Public Const ULR_Offer_Cover_Code As Integer = 4
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
	Public Const GISQMMInsurerNoOffset As Integer = 1000
	
	
	
	
	
	' ***************************************************************** '
	' Name: GetBrandName
	'
	' Description: Get brand name given brand code
	'
	' Author: CL161100
	'
	' ***************************************************************** '
	Public Function GetBrandName(ByVal v_sBrandCode As String, ByRef r_sBrandName As String) As Integer
		
		Dim result As Integer = 0
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Determine the name of the channel
		Select Case v_sBrandCode
			Case GISXelBrandCodeAutoBytel : r_sBrandName = GISXelBrandNameAutoBytel
			Case GISXelBrandCodeFirste : r_sBrandName = GISXelBrandNameFirste
			Case GISXelBrandCodeFTYM : r_sBrandName = GISXelBrandNameFTYM
			Case GISXelBrandCodeMoneyNet : r_sBrandName = GISXelBrandNameMoneyNet
			Case GISXelBrandCodeETrade : r_sBrandName = GISXelBrandNameETrade
			Case GISXelBrandCodeGenXel : r_sBrandName = GISXelBrandNameGenXel
			Case GISXelBrandCodeFish4 : r_sBrandName = GISXelBrandNameFish4
			Case GISXelBrandCodeLycos : r_sBrandName = GISXelBrandNameLycos
			Case GISXelBrandCodeLoot : r_sBrandName = GISXelBrandNameLoot
			Case GISXelBrandCodeTelegraph : r_sBrandName = GISXelBrandNameTelegraph
			Case GISXelBrandCodeLycos_Motor : r_sBrandName = GISXelBrandNameLycos_Motor
			Case Else : r_sBrandName = "UNDEFINED-IN-BOM!"
		End Select
		
		Return result
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
		LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBrandName", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBrandName", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	
	
	' ***************************************************************** '
	' Name: GISToPMDataType
	'
	' Description: Converts a GIS Data Type to its equivalent
	'              PM Data Type
	'
	' ***************************************************************** '
	Public Function GISToPMDataType(ByVal v_iGISDataType As Integer, ByRef r_iPMDataType As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			Select Case v_iGISDataType
				Case GISDataTypeDate
					r_iPMDataType = gPMConstants.PMEDataType.PMDate
				Case GISDataTypeNumeric, GISDataTypeNumericOutput, GISDataTypeOption, GISDataTypeCurrency, GISDataTypePercentage
					r_iPMDataType = gPMConstants.PMEDataType.PMDecimal
				Case GISDataTypeShortList, GISDataTypeLongList, GISDataTypeText
					r_iPMDataType = gPMConstants.PMEDataType.PMString
				Case GISDataTypeShortListCode, GISDataTypeLongListCode, GISDataTypecode
					r_iPMDataType = gPMConstants.PMEDataType.PMString
					' RFC070900 - Add some more data type as required by Underwriting
					' RFC070900 - All are effectively NUMERIC, and therefore need to be treated as such by PMDAO.
				Case GISDataTypeInteger
                    r_iPMDataType = gPMConstants.PMEDataType.PMLong
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown GIS Data Type : " & v_iGISDataType, vApp:=ACApp, vClass:=ACClass, vMethod:=result)

            End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GISToPMDataTypeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GISToPMDataType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetDataSetFileNames
	'
	' Description: Calculates the Location and File Name of the XML
	'              files for a given Data Model.
	'
	' ***************************************************************** '
	Public Function GetDataSetFileNames(ByVal v_sDataModelCode As String, ByRef r_sDataSetDefFile As String, ByRef r_sDataSetFile As String) As Integer
		
		Dim result As Integer = 0
		Dim sPath As String = ""
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' MUST have a Data Model Code
			If v_sDataModelCode.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataSetFileNames", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataSetFileNames")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get the Path where they live
			lReturn = CType(GetDataSetsPath(v_sDataModelCode:=v_sDataModelCode, r_sDataSetsPath:=sPath), gPMConstants.PMEReturnCode)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			r_sDataSetDefFile = sPath & v_sDataModelCode.Trim().ToUpper() & ACDataSetDefSuffix & ACXMLFileExtension
			
			r_sDataSetFile = sPath & v_sDataModelCode.Trim().ToUpper() & ACDataSetSuffix & ACXMLFileExtension
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataSetFileNamesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataSetFileNames", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
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
		
		Dim result As Integer = 0
		Dim sPath As String = ""
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' MUST have a Data Model Code
			If v_sGisDataModelCode.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Data Model Code Supplied...", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultsFileName")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get the Path where it lives - note it lives with the actual dataset files
			lReturn = CType(GetDataSetsPath(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetsPath:=sPath), gPMConstants.PMEReturnCode)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			GISDefaultsFile = sPath & v_sGisDataModelCode.Trim().ToUpper() & ACGISDefaultsFileSuffix & ACXMLFileExtension
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultsFileNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultsFileName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetSaveXSLFileName
	'
	' Description: Determines the Location and File Name of the GIS
	'              Save to DB XSL file for the current Data Model.
	'
	' Date:        28/03/2001
	'
	' History:     RFC - Created
	'
	' ***************************************************************** '
	Public Function GetSaveXSLFileName(ByVal v_sGisDataModelCode As String, ByRef r_sSaveXSLFileName As String) As Integer
		
		Dim result As Integer = 0
		Dim sPath As String = ""
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' MUST have a Data Model Code
			If v_sGisDataModelCode.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Data Model Code Supplied...", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSaveXSLFileName")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Get the Path where it lives - note it lives with the actual dataset files
			lReturn = CType(GetDataSetsPath(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetsPath:=sPath), gPMConstants.PMEReturnCode)
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			r_sSaveXSLFileName = sPath & v_sGisDataModelCode.Trim().ToUpper() & ACGISSaveXSLFileSuffix & ACXSLFileExtension
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSaveXSLFileNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSaveXSLFileName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetDataSetsPath
	'
	' Description: Return the Path for GIS Data Sets storage/retrieval.
	'
	' ***************************************************************** '
	Public Function GetDataSetsPath(ByVal v_sDataModelCode As String, ByRef r_sDataSetsPath As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' RFC110400 - Get the Data Model Specific Data Set Path
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISRegDataSetPath, r_sSettingValue:=r_sDataSetsPath, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon), gPMConstants.PMEReturnCode)
			
			' RFC110400 - If no Data Model Specific Data Set Path Found
			If r_sDataSetsPath.Trim() = "" Then
				
				' RFC110400 - Look for a setting in the Common\GIS
				' i.e. Not Data Model Specifc
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=GISRegDataSetPath, r_sSettingValue:=r_sDataSetsPath, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
				
			End If
			
			If r_sDataSetsPath.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Data Sets Path (" & GISRegDataSetPath & ") Registry Setting.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataSetsPath")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Not r_sDataSetsPath.EndsWith("\") Then
				r_sDataSetsPath = r_sDataSetsPath & "\"
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataSetsPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataSetsPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetLoadSPPath
	'
	' Description: Return the Path for Dataset Load SP
	'
	' ***************************************************************** '
	Public Function GetLoadSPPath(ByVal v_sDataModelCode As String, ByRef r_sLoadSPPath As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lReturn = GetDataSetsPath(v_sDataModelCode, r_sLoadSPPath)
			
			If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Load SP Path", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoadSPPath")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Not r_sLoadSPPath.EndsWith("\") Then
				r_sLoadSPPath = r_sLoadSPPath & "\"
			End If
			
			r_sLoadSPPath = r_sLoadSPPath & "LoadSP"
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLoadSPPath failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoadSPPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetEDISendPath
	'
	' Description:
	'
	' ***************************************************************** '
	Public Function GetEDISendPath(ByRef r_sEDISendPath As String, ByVal v_sDataModelCode As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' RFC170400 - Get the Data Model Specific EDI Send Path
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISRegEDISendPath, r_sSettingValue:=r_sEDISendPath, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer), gPMConstants.PMEReturnCode)
			
			' RFC170400 - If we did not find a Data Model Specific Setting
			If r_sEDISendPath.Trim() = "" Then
				
				' Then Look in the default Place.
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=GISRegEDISendPath, r_sSettingValue:=r_sEDISendPath, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
				
			End If
			
			If r_sEDISendPath.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find EDI Send Path (" & GISRegEDISendPath & ") Registry Setting.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEDISendPath")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Not r_sEDISendPath.EndsWith("\") Then
				r_sEDISendPath = r_sEDISendPath & "\"
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEDISendPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEDISendPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetFailedTransReportPath
	'
	' Description: Get the path for the output file for FailedTransReport
	' Called from the XELFailedTransReport prog.
	'
	' ***************************************************************** '
	Public Function GetFailedTransReportPath(ByRef r_sFailedTransReportPath As String, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' RJG 31/07/2000 - Get the Data Model Specific Failed Trans Report Path
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISRegFailedTransReportPath, r_sSettingValue:=r_sFailedTransReportPath, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSubKey:=GISRegReports), gPMConstants.PMEReturnCode)
			
			' RJG 31/07/2000 - If we did not find a Data Model Specific Setting
			If r_sFailedTransReportPath.Trim() = "" Then
				
				' Then Look in the default Place.
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=GISRegFailedTransReportPath, r_sSettingValue:=r_sFailedTransReportPath, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
				
			End If
			
			If r_sFailedTransReportPath.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Failed Transaction Report Path (" & GISRegFailedTransReportPath & ") Registry Setting.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFailedTransReportPath")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Not r_sFailedTransReportPath.EndsWith("\") Then
				r_sFailedTransReportPath = r_sFailedTransReportPath & "\"
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFailedTransReportPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFailedTransReportPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetBackOfficeReportPath
	'
	' Description: Get the path for the output file for BackOfficeReport
	' Called from the XELBackOfficeReport prog.
	'
	' ***************************************************************** '
	Public Function GetBackOfficeReportPath(ByRef r_sBackOfficeReportPath As String, ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' RJG 31/07/2000 - Get the Data Model Specific Failed Trans Report Path
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISRegBackOfficeReportPath, r_sSettingValue:=r_sBackOfficeReportPath, v_sBusinessTypeCode:=v_sBusinessTypeCode, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSubKey:=GISRegReports), gPMConstants.PMEReturnCode)
			
			' RJG 31/07/2000 - If we did not find a Data Model Specific Setting
			If r_sBackOfficeReportPath.Trim() = "" Then
				
				' Then Look in the default Place.
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=GISRegBackOfficeReportPath, r_sSettingValue:=r_sBackOfficeReportPath, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
				
			End If
			
			If r_sBackOfficeReportPath.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Failed Back Office Report Path (" & GISRegBackOfficeReportPath & ") Registry Setting.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBackOfficeReportPath")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Not r_sBackOfficeReportPath.EndsWith("\") Then
				r_sBackOfficeReportPath = r_sBackOfficeReportPath & "\"
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBackOfficeReportPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBackOfficeReportPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: IsGISInDebug
	'
	' Description: Return the GIS Debug Status.
	'
	' ***************************************************************** '
	Public Function IsGISInDebug() As Boolean
		
		Dim result As Boolean = False
		Dim sDebug As String = ""
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = True
			
			lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=GISRegGISInDebug, r_sSettingValue:=sDebug, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
			
			
			Select Case sDebug.Trim().ToUpper()
				Case "1", "Y", "YES"
					Return True
				Case Else
					Return False
			End Select
		
		Catch 
		End Try
		
		
		
		result = True
		
		' Log Error Message
		LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS Debud setting from registry. Default to Debug = True.", vApp:=ACApp, vClass:=ACClass, vMethod:="IsGISInDebug", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: GetGISLogLevel
	'
	' Description: Return the GIS Log Level Status.
	'
	' ***************************************************************** '
	Public Function GetGISLogLevel() As Integer
		
		Dim result As Integer = 0
		Dim sLogLevel As String = ""
		Dim iLogLevel As gPMConstants.PMELogLevel
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMELogLevel.PMLogFatal
			
			lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=GISRegGISLogLevel, r_sSettingValue:=sLogLevel, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
			
			If sLogLevel.Trim() = "" Then
				iLogLevel = gPMConstants.PMELogLevel.PMLogFatal
			Else
				iLogLevel = CType(CInt(sLogLevel), gPMConstants.PMELogLevel)
			End If
			
			If iLogLevel = 0 Then
				iLogLevel = gPMConstants.PMELogLevel.PMLogFatal
			End If
			
			
			Return iLogLevel
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMELogLevel.PMLogFatal
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS Log Level setting from registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISLogLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetHTTPPostPage
	'
	' Description: Returns the name of the Page to POST  the HTTP
	'              requests to.
	'
	' ***************************************************************** '
	Public Function GetHTTPPostPage(ByRef r_sHTTPPostPage As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=GISRegHTTPPostToPage, r_sSettingValue:=r_sHTTPPostPage, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
			
			If r_sHTTPPostPage.Trim() = "" Then
				' RFC120101 - Remove the logging of this message as it makes the log file huge and
				'             could be causing thread locking issues.
				'        LogMessageFile _
				''            iType:=PMLogError, _
				''            sMsg:="Failed to find HTTP Post Page (" & GISRegHTTPPostToPage & ") Registry Setting.", _
				''            vApp:=ACApp, _
				''            vClass:=ACClass, _
				''            vMethod:="GetHTTPPostPage"
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHTTPPostPageFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHTTPPostPage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetExternalDTDPath
	'
	' Description: Returns the name External DTD Path
	'
	' Date: CL160200
	'
	' ***************************************************************** '
	Public Function GetExternalDTDPath(ByVal sDataModelCode As String, ByRef r_sExternalDTDPath As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=GISRegExternalDTDPath, r_sSettingValue:=r_sExternalDTDPath, v_sSubKey:=GISRegSubKey & "\" & sDataModelCode), gPMConstants.PMEReturnCode)
			
			If r_sExternalDTDPath.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find External DTD Path (" & GISRegExternalDTDPath & ") Registry Setting.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExternalDTDPath")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Not r_sExternalDTDPath.EndsWith("\") Then
				r_sExternalDTDPath = r_sExternalDTDPath & "\"
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExternalDTDPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExternalDTDPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
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
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sValue, sSubKeyDataModel, sSubKeyDataModelBusModel As String
		Dim PMERegSettingLevel As gPMConstants.PMERegSettingLevel
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			sSubKeyDataModelBusModel = ACOIMGISSubKey & "\" & v_sDataModelCode & "\" & v_sBusinessTypeCode
			sSubKeyDataModel = ACOIMGISSubKey & "\" & v_sDataModelCode
			
			' If we have a SubKey
			v_sSubKey = v_sSubKey.Trim()
			If v_sSubKey <> "" Then
				' Append the sub key to both versions of the path
				sSubKeyDataModelBusModel = sSubKeyDataModelBusModel & "\" & v_sSubKey
				sSubKeyDataModel = sSubKeyDataModel & "\" & v_sSubKey
			End If
			
			sValue = ""
			
			' Use the supplied (or default reg setting level)
			PMERegSettingLevel = v_lPMERegSettingLevel
			
			' If we have a Business Type Code the Look for the setting there
			If v_sBusinessTypeCode <> "" Then
				
				' First look for value in data model/bus model key
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=PMERegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=sValue, v_sSubKey:=sSubKeyDataModelBusModel), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			' If we haven't got the value, look at Data Model level
			If sValue.Trim() = "" Then
				
				' Value not found in data model/bus model key
				' Therefore look in data model key
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=PMERegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=sValue, v_sSubKey:=sSubKeyDataModel), gPMConstants.PMEReturnCode)
				
				If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			r_sSettingValue = sValue
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRegSettingFromDataBusModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettingFromDataBusModel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: CheckGISDSN
	'
	' Description:
	'
	' History: 29/03/2000 RFC - Created.
	'
	' ***************************************************************** '
	'Public Function CheckGISDSN( _
	''    ByVal v_sDataModelCode As String, _
	''    ByRef r_oDatabase As Object, _
	''    ByRef r_bNew As Boolean) As Long
	'
	'Dim sDSN As String
	'Dim lReturn As Long
	'
	'    On Error GoTo Err_CheckGISDSN
	'
	'    CheckGISDSN = PMTrue
	'
	'    lReturn = GetRegSettingFromDataBusModel( _
	''        v_sDataModelCode:=v_sDataModelCode, _
	''        v_sSettingName:=GISRegDSN, _
	''        r_sSettingValue:=sDSN)
	'
	'    If (lReturn <> PMTrue) Then
	'        CheckGISDSN = PMFalse
	'        Exit Function
	'    End If
	'
	'    If (sDSN = "") Then
	'        sDSN = "GIS"
	'    End If
	'
	'    If (r_oDatabase Is Nothing = True) Then
	'        r_bNew = True
	'    Else
	'        If (r_oDatabase.CurrentDSN = sDSN) Then
	'            r_bNew = False
	'        Else
	'            r_bNew = True
	'        End If
	'    End If
	'
	'    If (r_bNew = True) Then
	'        Set r_oDatabase = CreateObject("dPMDAO.Database")
	'        ' RDC 28062002 use Comp Serv to open database
	'        lReturn = NewDatabase(v_lPMProductFamily:=pmePFSiriusArchitecture, r_oDatabase:=r_oDatabase)
	'        If (lReturn <> PMTrue) Then
	'            CheckGISDSN = lReturn
	'            Exit Function
	'        End If
	'    End If
	'
	'    Exit Function
	'
	'Err_CheckGISDSN:
	'
	'    CheckGISDSN = PMError
	'
	'    ' Log Error Message
	'    LogMessageFile _
	''        iType:=PMLogOnError, _
	''        sMsg:="CheckGISDSN Failed", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:="CheckGISDSN", _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'
	'    Exit Function
	'
	'End Function
	
	' ***************************************************************** '
	' Name: SaveOnQuote
	'
	' Description: Gets the SaveOnQuote registry setting for this
	'              data model/business type combination.
	' ***************************************************************** '
	Public Function SaveOnQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Boolean
		
		Dim result As Boolean = False
		Dim sSaveOnQuote As String = ""
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = True
			
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSettingName:=GISRegSaveOnQuote, r_sSettingValue:=sSaveOnQuote), gPMConstants.PMEReturnCode)
			
			
			Select Case sSaveOnQuote.Trim().ToUpper()
				Case "", "1", "Y", "YES"
					Return True
				Case Else
					Return False
			End Select
		
		Catch 
		End Try
		
		
		
		result = False
		
		' Log Error Message
		LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS SaveOnQuote from registry. Default = True", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveOnQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	'sj 12/04/2001 - start
	' ***************************************************************** '
	' Name: SaveOnMTAQuote
	'
	' Description: Gets the SaveOnMTAQuote registry setting for this
	'              data model/business type combination.
	' ***************************************************************** '
	Public Function SaveOnMTAQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Boolean
		
		Dim result As Boolean = False
		Dim sSaveOnMTAQuote As String = ""
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = True
			
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSettingName:=GISRegSaveOnMTAQuote, r_sSettingValue:=sSaveOnMTAQuote), gPMConstants.PMEReturnCode)
			
			
			Select Case sSaveOnMTAQuote.Trim().ToUpper()
				Case "", "1", "Y", "YES"
					Return True
				Case Else
					Return False
			End Select
		
		Catch 
		End Try
		
		
		
		result = False
		
		' Log Error Message
		LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS SaveOnMTAQuote from registry. Default = True", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveOnMTAQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	'sj 12/04/2001 - end
	' ***************************************************************** '
	' Name: AuditOnQuote
	'
	' Description: Gets the AuditOnQuote registry setting for this
	'              data model/business type combination.
	' ***************************************************************** '
	Public Function AuditOnQuote(ByVal v_sGisDataModelCode As String, Optional ByVal v_sGisBusinessTypeCode As String = "") As Boolean
		
		Dim result As Boolean = False
		Dim sAuditOnQuote As String = ""
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = True
			
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sGisBusinessTypeCode, v_sSettingName:=GISRegAuditOnQuote, r_sSettingValue:=sAuditOnQuote), gPMConstants.PMEReturnCode)
			
			
			Select Case sAuditOnQuote.Trim().ToUpper()
				Case "", "1", "Y", "YES"
					Return True
				Case Else
					Return False
			End Select
		
		Catch 
		End Try
		
		
		
		result = False
		
		' Log Error Message
		LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS AuditOnQuote from registry. Default = True", vApp:=ACApp, vClass:=ACClass, vMethod:="AuditOnQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
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
	
	Public Function ReplaceCharacter(ByRef r_sString As String, ByVal v_vSearchChar As String, ByVal v_sReplaceChar As String, ByVal v_sSearchCharType As String) As Integer
		
		Dim result As Integer = 0
        Dim iCharPosition, iReplaceStringLength As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Search for Character value
			If v_sSearchCharType = "C" Then
				
				iReplaceStringLength = v_sReplaceChar.Length
				iCharPosition = (r_sString.IndexOf(v_vSearchChar) + 1)
				
				Do While iCharPosition > 0
					Mid(r_sString, iCharPosition, iReplaceStringLength) = v_vSearchChar
					iCharPosition = Strings.InStr(iCharPosition + iReplaceStringLength, r_sString, v_vSearchChar)
				Loop 
				
				'Search for ASCII value
			ElseIf v_sSearchCharType = "A" Then 
				
				If r_sString.Length > 0 Then
					
					For iCharPosition = 1 To r_sString.Length
						If Strings.Asc(Mid(r_sString, iCharPosition, 1)(0)) = StringsHelper.ToDoubleSafe(v_vSearchChar) Then
							Mid(r_sString, iCharPosition, 1) = v_sReplaceChar
						End If
					Next 
					
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReplaceCharacter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplaceCharacter", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetLookupsPath
	'
	' Description: Return the Path for GIS Lookups storage/retrieval.
	'
	' ***************************************************************** '
	Public Function GetLookupsPath(ByVal v_sDataModelCode As String, ByRef r_sLookupsPath As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' RFC110400 - Get the Data Model Specific Data Set Path
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISRegLookupPath, r_sSettingValue:=r_sLookupsPath, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon), gPMConstants.PMEReturnCode)
			
			' RFC110400 - If no Data Model Specific Data Set Path Found
			If r_sLookupsPath.Trim() = "" Then
				
				' RFC110400 - Look for a setting in the Common\GIS
				' i.e. Not Data Model Specifc
				lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=GISRegLookupPath, r_sSettingValue:=r_sLookupsPath, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
				
			End If
			
			If r_sLookupsPath.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Lookups Path (" & GISRegLookupPath & ") Registry Setting.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupsPath")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Not r_sLookupsPath.EndsWith("\") Then
				r_sLookupsPath = r_sLookupsPath & "\"
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupsPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupsPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetQEMMethodsVersionNum
	'
	' Description: Get the version of the Quote Engine Mapper methods
	'              to be called.
	' RFC100700
	' ***************************************************************** '
	Public Function GetQEMMethodsVersionNum(ByVal v_sDataModelCode As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sQEMMethodsVersionNum As String = ""
		
		Try 
			
			' Default the Setting to 1
			result = 1
			
			' RFC100700 - Get the Data Model Specific Setting
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISQEMMethodsVersionNum, r_sSettingValue:=sQEMMethodsVersionNum, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer), gPMConstants.PMEReturnCode)
			
			' RFC100700 - Did we find the Setting
			If sQEMMethodsVersionNum.Trim() <> "" Then
				' Yes, so return it.
				result = CInt(sQEMMethodsVersionNum)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Default to Version 1
			result = 1
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQEMMethodsVersionNumFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQEMMethodsVersionNum", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetLoadSaveDBMode
	'
	' Description: Get the Save to DB Version Mode.
	'              1 = Slow VB method
	'              2 = Fast XSL/SP Methods
	'              3 = Fast XSL Method with Saves Quotes to DB enabled.
	' RFC290301
	' ***************************************************************** '
	Public Function GetLoadSaveDBMode(ByVal v_sDataModelCode As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sLoadSaveDBMode As String = ""
		
		Try 
			
			' Default the Setting to 1
			result = 1
			
			' RFC290301 - Get the Data Model Specific Setting
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISRegLoadSaveDBMode, r_sSettingValue:=sLoadSaveDBMode, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer), gPMConstants.PMEReturnCode)
			
			' RFC290301 - Did we find the Setting
			If sLoadSaveDBMode.Trim() <> "" Then
				' Yes, so return it.
				result = CInt(sLoadSaveDBMode)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Default to Version 1
			result = 1
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLoadSaveDBModeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoadSaveDBMode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetStateFilesPath
	'
	' Description:
	'
	' RFC220900 - Created
	' ***************************************************************** '
	Public Function GetStateFilesPath(ByRef r_sStateFilesPath As String) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=GISRegStateFilesPath, r_sSettingValue:=r_sStateFilesPath, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
			
			
			If r_sStateFilesPath.Trim() = "" Then
				LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find State Files Path (" & GISRegStateFilesPath & ") Registry Setting.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStateFilesPath")
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If Not r_sStateFilesPath.EndsWith("\") Then
				r_sStateFilesPath = r_sStateFilesPath & "\"
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStateFilesPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStateFilesPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetStateTimeoutSecs
	'
	' Description:
	'
	' CL260900 - Created
	' ***************************************************************** '
	Public Function GetStateTimeoutMins(ByRef r_lStateTimeoutMins As Integer) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim s As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=GISRegStateTimeoutMins, r_sSettingValue:=s, v_sSubKey:=GISRegSubKey), gPMConstants.PMEReturnCode)
			
			Dim dbNumericTemp As Double
			If (s.Trim() = "") Or (Not Double.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
				s = "40" ' default - 40 mins
			End If
			
			r_lStateTimeoutMins = CInt(s)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStateTimeoutMins Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStateTimeoutMins", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: LogMessageFile
	'
	' Description: Wrapper function to the log message method of the
	'              message object.
	'
	' ***************************************************************** '
	Public Sub LogMessageFile(ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing)
		

		Try 
			
			' We cannot Initialise PMMessage, Log to Screen





'CONVERSION FOR LOGMESSAGEFILE OUTSIDE CATCH BLOCK
'			gPMFunctions.LogMessageToFile(sUsername:=g_sUserName, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), vErrNo:=CStr(vErrNo), vErrDesc:=CStr(vErrDesc))
            gPMFunctions.LogMessageToFile(sUsername:=g_sUserName, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))
		
		Catch 
			
			
			
			' Error Section.
			
			Exit Sub
		End Try
		
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: DebugLogMessageFile
	'
	'
	' ***************************************************************** '
	Public Sub DebugLogMessageFile(ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing)
		
        Static vInDebug As Boolean
		
		Try 
			
			' Check if we need to log this message.

			If vInDebug.Equals(False) Then
				vInDebug = IsGISInDebug()
			End If
			
			If vInDebug Then
				' We cannot Initialise PMMessage, Log to Screen





'CONVERSION FOR LOGMESSAGEFILE OUTSIDE CATCH BLOCK
'				gPMFunctions.LogMessageToFile(sUsername:=g_sUserName, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), vErrNo:=CStr(vErrNo), vErrDesc:=CStr(vErrDesc))
                gPMFunctions.LogMessageToFile(sUsername:=g_sUserName, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))
			End If
		
		Catch 
			
			
			
			' Error Section.
			
			Exit Sub
		End Try
		
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: GetLoadSaveSafeInserts
	'
	' Description: Setting to decide is to use safe inserts for the
	'              SaveTODbViaXSL method.
	'              Safe Inserts are those that check to see if the row
	'              already exists before doing the insert.
	' RFC310701
	' ***************************************************************** '
	Public Function GetLoadSaveSafeInserts(ByVal v_sDataModelCode As String) As Boolean
		
		Dim result As Boolean = False
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sLoadSaveSafeInserts As String = ""
		
		Try 
			
			' Default the Setting to False
			
			lReturn = CType(GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISRegLoadSaveSafeInserts, r_sSettingValue:=sLoadSaveSafeInserts, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer), gPMConstants.PMEReturnCode)
			
			
			Select Case sLoadSaveSafeInserts.Trim().ToUpper()
				Case "1", "Y", "YES"
					Return True
				Case Else
					Return False
			End Select
		
		Catch 
		End Try
		
		
		
		' Default to Version 1
		result = False
		
		' Log Error Message
		LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLoadSaveSafeInsertsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoadSaveSafeInserts", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
End Module
